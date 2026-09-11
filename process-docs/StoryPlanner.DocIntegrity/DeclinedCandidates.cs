using System.Text.Json.Nodes;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// schemas/declined-candidates-schema.md on the engine: one entry per candidate a promotion
/// declined, appended by promoting-refereed-candidates. The engine holds the Body-section
/// entries and their typed fields (date, reason); this class holds the rules the schema's Checks
/// name: the title with the study, the entry heading <c>&lt;finding-slug&gt; → &lt;target&gt;</c>
/// unique in the file, dates that never go backwards, and the two references — the finding-slug
/// against this study's findings.md, the target against a hypothesis file. A candidate is
/// identified by (finding, target); a declined one is terminal, never withdrawn or superseded,
/// and a promotion is found through the hypothesis records, never recorded here.
/// </summary>
public static class DeclinedCandidates
{
    public const string SchemaId = "declined-candidates-schema";
    public const string VerificationPrefix = "verification-of-";
    const char Arrow = '→'; // → , the separator in `<finding-slug> → <target>`

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var studyDir = Path.GetDirectoryName(Path.GetFullPath(path))!;
        var study = Path.GetFileName(studyDir)!;
        var findings = new List<Finding>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return findings; }
        foreach (var p in engine.Problems)
        {
            var id = p.Section == "body" && p.Kind is ProblemKind.Missing or ProblemKind.Unknown or ProblemKind.Order or ProblemKind.Type or ProblemKind.Form or ProblemKind.Duplicate
                ? "declined-candidates.entry"
                : "declined-candidates.shape";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var doc = engine.Document;

        // ---- title and study ----
        var title = $"{study} — declined candidates";
        if (doc.Title != title)
            findings.Add(Finding.Fail("declined-candidates.title", file, $"the title is '# {title}', the id of the study whose folder holds the file"));
        if (!study.StartsWith(VerificationPrefix, StringComparison.Ordinal))
            findings.Add(Finding.Fail("declined-candidates.title", file, $"'{study}' is not a verification's id; declined candidates belong to a verification"));

        // ---- the references: standing findings of this study, and the hypothesis files ----
        var standing = StandingFindings(studyDir);
        var hypothesisFiles = References.FilesOf(WellKnown.HypothesisStatus, ctx);

        // ---- entries: heading `<finding-slug> → <target>`, unique, dates ascending ----
        var seen = new HashSet<string>(StringComparer.Ordinal);
        string? lastDate = null;
        var positions = doc.Entries.Where(e => e.Section == "body").ToList();
        var arr = doc.Root["body"] as JsonArray ?? [];
        for (var i = 0; i < arr.Count; i++)
        {
            var obj = (JsonObject)arr[i]!;
            var pos = i < positions.Count ? positions[i] : null;
            var line = pos?.Line ?? doc.TitleLine;
            var heading = obj[DocumentReader.HeadingProperty]?.GetValue<string>() ?? "";

            var arrow = heading.IndexOf(Arrow);
            if (arrow < 0)
                findings.Add(Finding.Fail("declined-candidates.entry", file,
                    $"line {line}: the heading is '<finding-slug> → <target>'; found '{heading}'"));
            else
            {
                var slug = heading[..arrow].Trim();
                var target = heading[(arrow + 1)..].Trim();
                if (!ClosedSets.IdPattern.IsMatch(slug))
                    findings.Add(Finding.Fail("declined-candidates.entry", file, $"line {line}: '{slug}' is not a lowercase slug"));
                if (!seen.Add(heading))
                    findings.Add(Finding.Fail("declined-candidates.entry", file, $"line {line}: the (finding, target) pair '{heading}' repeats"));

                if (standing is not null && ClosedSets.IdPattern.IsMatch(slug) && !standing.Contains(slug))
                    findings.Add(Finding.Fail("declined-candidates.references", file,
                        $"line {line}: '{slug}' names no standing finding in {study}/findings.md"));
                if (hypothesisFiles is not null && !hypothesisFiles.Any(f => Path.GetFileNameWithoutExtension(f) == target))
                    findings.Add(Finding.Fail("declined-candidates.references", file,
                        $"line {line}: '{target}' names no hypothesis file NNN-slug.md"));
            }

            var date = obj["date"]?.GetValue<string>();
            if (date is not null && SchemaCheckers.IsoDate.IsMatch(date))
            {
                var dLine = pos?.FieldLines.GetValueOrDefault("date", line) ?? line;
                if (SchemaCheckers.ExactDate(date))
                {
                    if (lastDate is not null && string.CompareOrdinal(date, lastDate) < 0)
                        findings.Add(Finding.Fail("declined-candidates.entry", file,
                            $"line {dLine}: {date} is earlier than the entry before it, {lastDate}"));
                    lastDate = date;
                }
            }
        }

        return findings.DistinctBy(f => (f.CheckId, f.Message)).ToList();
    }

    /// <summary>The slugs of this study's standing findings — neither withdrawn nor superseded — or null when findings.md is absent.</summary>
    static IReadOnlySet<string>? StandingFindings(string studyDir)
    {
        var path = Path.Combine(studyDir, "findings.md");
        if (!File.Exists(path)) return null;
        var entries = FindingsChecker.ReadEntries(File.ReadAllText(path));
        var superseded = entries.Where(e => e.Supersedes is not null)
            .Select(e => SlugOf(e.Supersedes!))
            .ToHashSet(StringComparer.Ordinal);
        return entries.Where(e => !e.Withdrawn && !superseded.Contains(e.Slug))
            .Select(e => e.Slug).ToHashSet(StringComparer.Ordinal);
    }

    static string SlugOf(string token)
    {
        var slash = token.IndexOf('/');
        return slash < 0 ? token : token[(slash + 1)..];
    }
}
