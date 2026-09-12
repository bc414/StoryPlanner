using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// compose-candidates (decisions d-2026-09-10-5, -7): the generated candidates view. It is
/// study-scoped — it spans the study's referee batch and its study-level files — so it runs in
/// DocIntegrity, not the runner (d-2026-09-08-4). It joins the referee results (each a
/// (finding, target) claim the referee judged), the finding text from findings.md, the decline
/// reasons from declined-candidates.md and the promotions read from the hypothesis records into
/// one read-only, regenerable file: the diagnostic candidates in a section that opens the
/// document, each materialising its finding, verdict, falsifier and status; the non-diagnostic
/// claims in a labelled section at the foot (d-2026-09-10-7). It is not governed: a deterministic
/// program emits it from validated inputs (d-2026-09-10-2), so it carries no checker.
///
/// First-pass conventions this reads, decided here and refined against the first real candidates
/// (d-2026-09-10-7), matched by the referee directions and itemizer when they are authored:
///   - the referee batch is the study batch whose definition's directions resolve under
///     <c>docs/v3-framework/referee/</c> (d-2026-09-10-5: the referee batch names the referee's
///     directions by path into the referee folder);
///   - the referee batch's index gives each item a locator <c>&lt;finding-slug&gt; → &lt;target&gt;</c>,
///     the candidate's identity, which this parses;
///   - the referee directions declare one block field (the falsifier) and one enum field (the
///     verdict), whose class labels carry <c>non-diagnostic</c>, <c>supporting</c> or
///     <c>challenging</c>.
/// </summary>
public static class Compose
{
    public const char Arrow = '→';
    public const string FileName = "candidates.md";
    public const string RefereeFolder = "referee";

    public enum Verdict { DiagnosticSupporting, DiagnosticChallenging, NonDiagnostic }
    public enum Status { Promoted, Declined, Pending }

    public sealed record Candidate(string FindingSlug, string Target, Verdict Verdict, string Falsifier, string Finding, Status Status, string? Reason);

    /// <summary>The candidates.md text for one study, joined from the referee results, findings.md, declined-candidates.md and the hypothesis records.</summary>
    public static string Build(CheckContext ctx, string studyDir)
    {
        var study = Path.GetFileName(Path.GetFullPath(studyDir))!;
        var findings = FindingTexts(Path.Combine(studyDir, "findings.md"));
        var declined = DeclinedReasons(Path.Combine(studyDir, "declined-candidates.md"));
        var promoted = PromotedCandidates(study, ctx);

        var candidates = new List<Candidate>();
        var refereeBatch = RefereeBatch(studyDir);
        if (refereeBatch is not null)
        {
            var (definition, directions, index) = refereeBatch.Value;
            var falsifierKey = directions.Output.FirstOrDefault(o => o.Kind == OutputKind.Block)?.Key;
            var verdictKey = directions.Output.FirstOrDefault(o => o.Kind == OutputKind.Enum)?.Key;
            foreach (var row in index.Rows)
            {
                var identity = ParseIdentity(row.Locator);
                if (identity is null) continue;
                var (findingSlug, target) = identity.Value;
                var resultPath = Path.Combine(definition.ResultsDir, row.Item + ".md");
                if (!File.Exists(resultPath)) continue;
                var (answer, _) = ResultFile.Parse(directions, File.ReadAllText(resultPath));
                var verdict = ClassifyVerdict(verdictKey is null ? null : answer[verdictKey]?.GetValue<string>());
                var falsifier = falsifierKey is not null && answer[falsifierKey] is { } fv ? fv.GetValue<string>() : "";
                var status = promoted.Contains((findingSlug, target)) ? Status.Promoted
                    : declined.ContainsKey((findingSlug, target)) ? Status.Declined
                    : Status.Pending;
                candidates.Add(new Candidate(findingSlug, target, verdict, falsifier,
                    findings.GetValueOrDefault(findingSlug, ""), status,
                    declined.GetValueOrDefault((findingSlug, target))));
            }
        }

        return Render(study, candidates);
    }

    /// <summary>Writes candidates.md for the study and returns its path.</summary>
    public static string Write(CheckContext ctx, string studyDir)
    {
        var path = Path.Combine(studyDir, FileName);
        File.WriteAllText(path, Build(ctx, studyDir));
        return path;
    }

    static string Render(string study, IReadOnlyList<Candidate> candidates)
    {
        var diagnostic = candidates.Where(c => c.Verdict != Verdict.NonDiagnostic).ToList();
        var nonDiagnostic = candidates.Where(c => c.Verdict == Verdict.NonDiagnostic).ToList();

        var sb = new StringBuilder();
        sb.Append($"# {study} — candidates\n\n");
        sb.Append("Generated by compose-candidates through process-docs/StoryPlanner.DocIntegrity; " +
                  "read-only and regenerable, never hand-edited. The status is derived — promoted where a " +
                  "hypothesis record cites the candidate, declined with its reason from declined-candidates.md, " +
                  "else pending.\n\n");

        sb.Append("## Diagnostic candidates\n\n");
        if (diagnostic.Count == 0) sb.Append("None.\n\n");
        foreach (var c in diagnostic)
        {
            sb.Append($"### {c.FindingSlug} {Arrow} {c.Target}\n\n");
            sb.Append($"- verdict: {VerdictText(c.Verdict)}\n");
            sb.Append($"- status: {c.Status.ToString().ToLowerInvariant()}\n");
            if (c.Status == Status.Declined && c.Reason is { Length: > 0 })
                sb.Append(KeyedLines.RenderBlock("reason", c.Reason)).Append('\n');
            sb.Append(KeyedLines.RenderBlock("falsifier", c.Falsifier)).Append('\n');
            sb.Append(KeyedLines.RenderBlock("finding", c.Finding)).Append("\n\n");
        }

        sb.Append("## Non-diagnostic claims\n\n");
        sb.Append("Context the referee set aside — not promotion options, not counted.\n\n");
        if (nonDiagnostic.Count == 0) sb.Append("None.\n\n");
        foreach (var c in nonDiagnostic)
        {
            sb.Append($"### {c.FindingSlug} {Arrow} {c.Target}\n\n");
            sb.Append(KeyedLines.RenderBlock("finding", c.Finding)).Append("\n\n");
        }
        return sb.ToString();
    }

    static string VerdictText(Verdict v) => v switch
    {
        Verdict.DiagnosticSupporting => "diagnostic supporting",
        Verdict.DiagnosticChallenging => "diagnostic challenging",
        _ => "non-diagnostic",
    };

    static Verdict ClassifyVerdict(string? label)
    {
        if (label is null) return Verdict.NonDiagnostic;
        var l = label.ToLowerInvariant();
        if (l.Contains("non-diagnostic") || l.Contains("nondiagnostic")) return Verdict.NonDiagnostic;
        if (l.Contains("challeng")) return Verdict.DiagnosticChallenging;
        if (l.Contains("support")) return Verdict.DiagnosticSupporting;
        return Verdict.NonDiagnostic;
    }

    /// <summary>The study's referee batch: the batch whose definition's directions resolve under the referee folder.</summary>
    static (DefinitionFile Definition, DirectionsFile Directions, IndexFile Index)? RefereeBatch(string studyDir)
    {
        var batchesDir = Path.Combine(studyDir, "batches");
        if (!Directory.Exists(batchesDir)) return null;
        foreach (var dir in Directory.GetDirectories(batchesDir).OrderBy(d => d, StringComparer.Ordinal))
        {
            var definitionPath = Path.Combine(dir, "definition.md");
            if (!File.Exists(definitionPath)) continue;
            DefinitionFile definition;
            try { definition = DefinitionFile.Read(definitionPath); } catch (IOException) { continue; }
            if (definition.DirectionsPath is not { } dp || !File.Exists(dp)) continue;
            var underReferee = Path.GetFileName(Path.GetDirectoryName(Path.GetFullPath(dp))!) == RefereeFolder;
            if (!underReferee) continue;
            if (!File.Exists(definition.IndexPath)) continue;
            return (definition, DirectionsFile.Read(dp), IndexFile.Read(definition.IndexPath));
        }
        return null;
    }

    /// <summary>A referee item's identity, <c>&lt;finding-slug&gt; → &lt;target&gt;</c>, from its index locator; null when the locator is not that form.</summary>
    static (string FindingSlug, string Target)? ParseIdentity(string locator)
    {
        var arrow = locator.IndexOf(Arrow);
        if (arrow < 0) return null;
        var slug = locator[..arrow].Trim();
        var target = locator[(arrow + 1)..].Trim();
        return slug.Length == 0 || target.Length == 0 ? null : (slug, target);
    }

    static readonly Regex FindingHeading = new(@"^### [a-z0-9-]+/(?<slug>[a-z0-9-]+)$", RegexOptions.Compiled);

    /// <summary>The finding block text per finding slug, read from findings.md; empty map when the file is absent.</summary>
    static IReadOnlyDictionary<string, string> FindingTexts(string path)
    {
        var texts = new Dictionary<string, string>(StringComparer.Ordinal);
        if (!File.Exists(path)) return texts;
        var lines = File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');
        var inFindings = false;
        string? slug = null;
        var block = new List<string>();
        var capturing = false;
        void Flush() { if (slug is not null) texts[slug] = string.Join('\n', block).Trim(); slug = null; block = []; capturing = false; }
        foreach (var l in lines)
        {
            if (l.StartsWith("## ", StringComparison.Ordinal)) { Flush(); inFindings = l[3..].Trim() == "Findings"; continue; }
            if (!inFindings) continue;
            if (l.StartsWith("### ", StringComparison.Ordinal))
            {
                Flush();
                var m = FindingHeading.Match(l);
                slug = m.Success ? m.Groups["slug"].Value : null;
                continue;
            }
            if (slug is null) continue;
            if (l.StartsWith("- finding:", StringComparison.Ordinal)) { block.Add(l["- finding:".Length..].Trim()); capturing = true; }
            else if (l.StartsWith("- ", StringComparison.Ordinal)) capturing = false;          // the finding block ends at the next keyed line
            else if (capturing && l.StartsWith("  ", StringComparison.Ordinal)) block.Add(l.Trim());
        }
        Flush();
        return texts;
    }

    static readonly Regex DeclinedHeading = new(@"^### (?<slug>[a-z0-9-]+) → (?<target>[A-Za-z0-9-]+)$", RegexOptions.Compiled);

    /// <summary>The decline reason per (finding-slug, target), read from declined-candidates.md; empty when absent.</summary>
    static IReadOnlyDictionary<(string, string), string> DeclinedReasons(string path)
    {
        var reasons = new Dictionary<(string, string), string>();
        if (!File.Exists(path)) return reasons;
        var lines = File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');
        (string, string)? key = null;
        var block = new List<string>();
        void Flush() { if (key is not null) reasons[key.Value] = string.Join('\n', block).Trim(); key = null; block = []; }
        foreach (var l in lines)
        {
            var m = DeclinedHeading.Match(l);
            if (m.Success) { Flush(); key = (m.Groups["slug"].Value, m.Groups["target"].Value); continue; }
            if (key is null) continue;
            if (l.StartsWith("- reason:", StringComparison.Ordinal)) block.Add(l["- reason:".Length..].Trim());
            else if (block.Count > 0 && l.StartsWith("  ", StringComparison.Ordinal)) block.Add(l.Trim());
        }
        Flush();
        return reasons;
    }

    // The evidence entry's citation is a field since d-2026-09-11-15; the owner is the study
    // whose findings.md holds the finding, for an iteration-sourced entry too (d-2026-09-11-18).
    static readonly Regex Citation = new(
        @"(?m)^- candidate: (?<study>[a-z0-9-]+)/(?<slug>[a-z0-9-]+)\s*$", RegexOptions.Compiled);
    static readonly Regex HypothesisName = new(@"^(?<name>\d{3}-[a-z0-9-]+)\.md$", RegexOptions.Compiled);

    /// <summary>The (finding-slug, target) candidates promoted for this study: read from the evidence citations across the hypothesis records.</summary>
    static IReadOnlySet<(string, string)> PromotedCandidates(string study, CheckContext ctx)
    {
        var promoted = new HashSet<(string, string)>();
        var files = References.FilesOf(WellKnown.HypothesisRecord, ctx);
        if (files is null) return promoted;
        foreach (var file in files)
        {
            var name = HypothesisName.Match(Path.GetFileName(file));
            if (!name.Success) continue;
            var target = name.Groups["name"].Value;
            foreach (Match m in Citation.Matches(File.ReadAllText(file)))
                if (m.Groups["study"].Value == study)
                    promoted.Add((m.Groups["slug"].Value, target));
        }
        return promoted;
    }
}
