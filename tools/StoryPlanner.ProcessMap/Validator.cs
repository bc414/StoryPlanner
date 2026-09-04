using System.Text.RegularExpressions;

namespace StoryPlanner.ProcessMap;

public sealed record ValidationReport(IReadOnlyList<Finding> Findings)
{
    public bool Passed => Findings.All(f => f.Level != FindingLevel.Failure);
    public int Failures => Findings.Count(f => f.Level == FindingLevel.Failure);
}

/// <summary>
/// Every rule of <c>process-map.md</c> § Format, plus the rulings of 2026-09-04 recorded in
/// <c>docs/v3-framework/methodology-revision-2-handoff.md</c> § Rulings.
///
/// Findings carry a rule id so a test asserts on the id, never on the prose.
/// </summary>
public static class Validator
{
    public const int SkillLineBudget = 500;          // Anthropic's published figure
    public const int SkillDescriptionBudget = 1024;  // same source

    public static ValidationReport Validate(string repoRoot, string skillFolder)
    {
        var findings = new List<Finding>();
        var mapPath = Path.Combine(skillFolder, "process-map.md");
        if (!File.Exists(mapPath))
            return new ValidationReport([Finding.Fail("map.missing", "—",
                $"no process-map.md in {skillFolder}")]);

        ProcessMapDocument doc;
        try
        {
            doc = MapReader.Read(File.ReadAllText(mapPath));
        }
        catch (MapFormatException ex)
        {
            return new ValidationReport([Finding.Fail("map.unparseable", "—", ex.Message)]);
        }

        CheckIds(doc, findings);
        CheckEnums(doc, findings);
        CheckProcessRows(doc, findings);
        CheckEdges(doc, findings);
        CheckGovernedBy(repoRoot, doc, findings);
        CheckRootSources(repoRoot, doc, findings);
        CheckRootCitation(doc, findings);
        CheckFileTraffic(doc, findings);
        CheckPromotionGate(doc, findings);
        CheckBootstrap(doc, findings);
        CheckCodebook(repoRoot, findings);
        CheckSkill(skillFolder, findings);
        ReportInformational(doc, findings);

        return new ValidationReport(findings);
    }

    // ---- ids and references ----

    static void CheckIds(ProcessMapDocument doc, List<Finding> findings)
    {
        var seen = new Dictionary<string, string>(StringComparer.Ordinal);
        void Claim(string id, string table, int line)
        {
            if (!Regex.IsMatch(id, @"^[A-Za-z0-9.-]+$"))
                findings.Add(Finding.Fail("id.charset", id,
                    $"line {line}: an id may only use [A-Za-z0-9.-]"));
            if (seen.TryGetValue(id, out var other))
                findings.Add(Finding.Fail("id.duplicate", id,
                    $"line {line}: id already used in {other}; ids are unique across tables"));
            else seen[id] = table;
        }

        foreach (var r in doc.Roots) Claim(r.Id, "Roots", r.Line);
        foreach (var f in doc.Files) Claim(f.Id, "Files", f.Line);
        foreach (var p in doc.Processes) Claim(p.Id, "Processes", p.Line);

        var fileIds = doc.Files.Select(f => f.Id).ToHashSet(StringComparer.Ordinal);
        var rootIds = doc.Roots.Select(r => r.Id).ToHashSet(StringComparer.Ordinal);
        var procIds = doc.Processes.Select(p => p.Id).ToHashSet(StringComparer.Ordinal);

        foreach (var p in doc.Processes)
        {
            foreach (var f in p.Inputs.Where(f => !fileIds.Contains(f)))
                findings.Add(Finding.Fail("ref.file", p.Id, $"line {p.Line}: input '{f}' is not a file id"));
            foreach (var f in p.Outputs.Where(f => !fileIds.Contains(f)))
                findings.Add(Finding.Fail("ref.file", p.Id, $"line {p.Line}: output '{f}' is not a file id"));
            foreach (var r in p.Roots.Where(r => !rootIds.Contains(r)))
                findings.Add(Finding.Fail("ref.root", p.Id, $"line {p.Line}: root '{r}' is not a root id"));
        }

        foreach (var e in doc.Edges)
        {
            if (!procIds.Contains(e.From))
                findings.Add(Finding.Fail("ref.edge", $"{e.From}→{e.To}",
                    $"line {e.Line}: '{e.From}' is not a process id"));
            if (!procIds.Contains(e.To))
                findings.Add(Finding.Fail("ref.edge", $"{e.From}→{e.To}",
                    $"line {e.Line}: '{e.To}' is not a process id"));
        }
    }

    // ---- closed sets ----

    static void CheckEnums(ProcessMapDocument doc, List<Finding> findings)
    {
        foreach (var r in doc.Roots)
            if (!ClosedSets.RootKinds.Contains(r.Kind))
                findings.Add(Finding.Fail("enum.root-kind", r.Id,
                    $"line {r.Line}: kind '{r.Kind}' is not one of {Join(ClosedSets.RootKinds)}"));

        foreach (var f in doc.Files)
            if (!ClosedSets.Keeps.Contains(f.Keep))
                findings.Add(Finding.Fail("enum.keep", f.Id,
                    $"line {f.Line}: keep '{f.Keep}' is not one of {Join(ClosedSets.Keeps)}"));

        foreach (var p in doc.Processes)
        {
            if (!ClosedSets.Levels.Contains(p.Level))
                findings.Add(Finding.Fail("enum.level", p.Id,
                    $"line {p.Line}: level '{p.Level}' is not one of {Join(ClosedSets.Levels)}"));
            if (!ClosedSets.ProcessKinds.Contains(p.Kind))
                findings.Add(Finding.Fail("enum.process-kind", p.Id,
                    $"line {p.Line}: kind '{p.Kind}' is not one of {Join(ClosedSets.ProcessKinds)}"));
            if (!ClosedSets.States.Contains(p.State))
                findings.Add(Finding.Fail("enum.state", p.Id,
                    $"line {p.Line}: state '{p.State}' is not one of {Join(ClosedSets.States)}"));
        }

        foreach (var e in doc.Edges)
            if (!ClosedSets.EdgeKinds.Contains(e.Kind))
                findings.Add(Finding.Fail("enum.edge-kind", $"{e.From}→{e.To}",
                    $"line {e.Line}: kind '{e.Kind}' is not one of {Join(ClosedSets.EdgeKinds)}"));
    }

    static string Join(string[] values) => string.Join(" | ", values);

    // ---- process row minima ----

    static void CheckProcessRows(ProcessMapDocument doc, List<Finding> findings)
    {
        foreach (var p in doc.Processes)
        {
            if (!ClosedSets.IsActor(p.Actor))
                findings.Add(Finding.Fail("row.actor", p.Id,
                    $"line {p.Line}: actor '{p.Actor}' is not brian | script | tool | hitl:<x> | agent:<x>, " +
                    "exactly one per row"));
            if (p.Roots.Count == 0)
                findings.Add(Finding.Fail("row.roots-empty", p.Id,
                    $"line {p.Line}: a process cites at least one root, or it has no reason to exist"));
            if (p.Inputs.Count == 0)
                findings.Add(Finding.Fail("row.inputs-empty", p.Id,
                    $"line {p.Line}: a process reads at least one file; one that reads nothing is " +
                    "deriving from recall, which C8 forbids"));
            if (p.Outputs.Count == 0)
                findings.Add(Finding.Fail("row.outputs-empty", p.Id,
                    $"line {p.Line}: a process writes at least one file; one that writes nothing is " +
                    "indistinguishable from not having run"));
            if (p.Text.Length == 0)
                findings.Add(Finding.Fail("row.text-empty", p.Id, $"line {p.Line}: no process description"));
        }
    }

    static void CheckEdges(ProcessMapDocument doc, List<Finding> findings)
    {
        foreach (var e in doc.Edges.Where(e => e.Kind == "choice" && e.Label.Length == 0))
            findings.Add(Finding.Fail("edge.choice-label", $"{e.From}→{e.To}",
                $"line {e.Line}: a choice edge's label is the branch condition and cannot be empty"));
    }

    // ---- governed-by: a reading assignment, so a bare repo-relative path ----

    static void CheckGovernedBy(string repoRoot, ProcessMapDocument doc, List<Finding> findings)
    {
        void Check(string id, string cell, int line)
        {
            if (cell.Length == 0)
            {
                findings.Add(Finding.Fail("governed-by.empty", id,
                    $"line {line}: no governing document. Precedence needs a named file: when the row " +
                    "and the prose disagree, the prose wins on procedure — but only if there is one"));
                return;
            }
            if (cell.Contains(Locus.SectionSign) || cell.Contains(Locus.ItemSign))
            {
                findings.Add(Finding.Fail("governed-by.syntax", id,
                    $"line {line}: '{cell}' addresses a section. governed-by is a reading assignment " +
                    "and a precedence declaration, both document-granular; section precision belongs " +
                    "in Roots.source"));
                return;
            }
            if (!Locus.TryValidatePath(cell, out var error))
            {
                findings.Add(Finding.Fail("governed-by.syntax", id, $"line {line}: {error}"));
                return;
            }
            if (!File.Exists(Path.Combine(repoRoot, cell)))
                findings.Add(Finding.Fail("governed-by.missing-file", id,
                    $"line {line}: '{cell}' does not exist under the repo root"));
        }

        foreach (var p in doc.Processes) Check(p.Id, p.GovernedBy, p.Line);
        foreach (var f in doc.Files) Check(f.Id, f.GovernedBy, f.Line);
    }

    // ---- Roots.source: a citation, so the full locus grammar ----

    static void CheckRootSources(string repoRoot, ProcessMapDocument doc, List<Finding> findings)
    {
        var outlines = new Dictionary<string, MarkdownOutline>(StringComparer.Ordinal);

        foreach (var r in doc.Roots)
        {
            if (r.Source.Length == 0)
            {
                findings.Add(Finding.Fail("source.empty", r.Id,
                    $"line {r.Line}: a root states where it is stated"));
                continue;
            }

            foreach (var part in r.Source.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!Locus.TryParse(part, out var locus, out var error))
                {
                    findings.Add(Finding.Fail("source.syntax", r.Id, $"line {r.Line}: '{part}' — {error}"));
                    continue;
                }

                var full = Path.Combine(repoRoot, locus!.Path);
                if (!File.Exists(full))
                {
                    findings.Add(Finding.Fail("source.missing-file", r.Id,
                        $"line {r.Line}: '{locus.Path}' does not exist under the repo root"));
                    continue;
                }
                if (locus.Heading is null) continue;

                if (!outlines.TryGetValue(full, out var outline))
                    outlines[full] = outline = new MarkdownOutline(File.ReadAllText(full));

                var matches = outline.Find(locus.Heading);
                if (matches.Count == 0)
                {
                    findings.Add(Finding.Fail("source.heading", r.Id,
                        $"line {r.Line}: '{locus.Path}' has no heading '{locus.Heading}'"));
                    continue;
                }
                if (matches.Count > 1)
                {
                    findings.Add(Finding.Fail("source.heading-ambiguous", r.Id,
                        $"line {r.Line}: '{locus.Path}' has {matches.Count} headings '{locus.Heading}'; " +
                        "a citation names one place"));
                    continue;
                }
                if (locus.Item is null) continue;

                var items = outline.CountOrderedItems(matches[0]);
                if (locus.Item > items)
                    findings.Add(Finding.Fail("source.item", r.Id,
                        $"line {r.Line}: '{locus.Heading}' has {items} ordered items; " +
                        $"{Locus.ItemSign} {locus.Item} does not exist"));
            }
        }
    }

    static void CheckRootCitation(ProcessMapDocument doc, List<Finding> findings)
    {
        var cited = doc.Processes.SelectMany(p => p.Roots).ToHashSet(StringComparer.Ordinal);
        foreach (var r in doc.Roots.Where(r => !cited.Contains(r.Id)))
            findings.Add(Finding.Fail("root.uncited", r.Id,
                $"line {r.Line}: no process cites this root. A root nothing acts on is not a root"));
    }

    // ---- file traffic: three distinct orphan kinds, no terminal-record exemption ----

    static void CheckFileTraffic(ProcessMapDocument doc, List<Finding> findings)
    {
        foreach (var t in GraphRules.Traffic(doc))
        {
            var f = doc.Files.First(x => x.Id == t.FileId);
            if (t.Producers.Count == 0 && t.Consumers.Count == 0)
                findings.Add(Finding.Fail("file.uncited", t.FileId,
                    $"line {f.Line}: no process reads or writes it"));
            else if (t.Consumers.Count == 0)
                findings.Add(Finding.Fail("file.written-never-read", t.FileId,
                    $"line {f.Line}: written by {string.Join(", ", t.Producers)} and read by nothing. " +
                    "A file read only by a person is read by a process this map is missing a row for"));
            else if (t.Producers.Count == 0)
                findings.Add(Finding.Fail("file.read-never-written", t.FileId,
                    $"line {f.Line}: read by {string.Join(", ", t.Consumers)} and written by nothing"));
        }
    }

    // ---- the promotion gate ----

    static void CheckPromotionGate(ProcessMapDocument doc, List<Finding> findings)
    {
        GateCheck(doc, findings, "f.cand", "f.hyp");
        GateCheck(doc, findings, null, "f.storyplan");
    }

    static void GateCheck(ProcessMapDocument doc, List<Finding> findings, string? source, string target)
    {
        var label = source is null ? $"anything → {target}" : $"{source} → {target}";

        if (doc.Files.All(f => f.Id != target))
        {
            findings.Add(Finding.Vacuous("gate.vacuous", $"{label}: no file row '{target}'"));
            return;
        }
        var writers = doc.Processes.Where(p => p.Outputs.Contains(target)).ToList();
        if (writers.Count == 0)
        {
            findings.Add(Finding.Vacuous("gate.vacuous",
                $"{label}: no process writes '{target}', so the rule has no subject today — " +
                "reported as vacuous, not as passing"));
            return;
        }

        if (source is not null && doc.Processes.All(p => !p.Inputs.Contains(source)))
        {
            findings.Add(Finding.Vacuous("gate.vacuous",
                $"{label}: no process reads '{source}', so the rule has no subject today"));
            return;
        }

        var readers = source is null ? "anything" : source;

        // One finding per writing row, carrying its shortest ungated path. The same write is
        // reachable many ways; listing every route reports one gap a dozen times.
        var shortest = GraphRules.UngatedPaths(doc, source, target)
            .GroupBy(p => p.Nodes[^1], StringComparer.Ordinal)
            .Select(g => g.OrderBy(p => p.Nodes.Count).ThenBy(p => p.ToString(), StringComparer.Ordinal).First())
            .OrderBy(p => p.Nodes[^1], StringComparer.Ordinal);

        foreach (var path in shortest)
            findings.Add(Finding.Fail("gate.ungated", path.Nodes[^1],
                $"{path} writes {target} with no brian actor on the path from {readers}. " +
                "The path ends at the write: a review after it is detection, and C1/C2 are preventive"));
    }

    // ---- bootstrap ----

    static void CheckBootstrap(ProcessMapDocument doc, List<Finding> findings)
    {
        var listed = doc.Bootstrap.Select(b => b.RowId).ToList();
        foreach (var b in doc.Bootstrap)
        {
            var row = doc.Processes.FirstOrDefault(p => p.Id == b.RowId);
            if (row is null)
                findings.Add(Finding.Fail("bootstrap.unknown-row", b.RowId,
                    $"line {b.Line}: no process with this id"));
            else if (row.Kind != "bootstrap")
                findings.Add(Finding.Fail("bootstrap.not-bootstrap", b.RowId,
                    $"line {b.Line}: listed as bootstrap but its kind is '{row.Kind}'"));
            if (b.RetiredBy.Length == 0)
                findings.Add(Finding.Fail("bootstrap.no-retirement", b.RowId,
                    $"line {b.Line}: a bootstrap row names what retires it"));
            if (listed.Count(x => x == b.RowId) > 1)
                findings.Add(Finding.Fail("bootstrap.duplicate", b.RowId, $"line {b.Line}: listed twice"));
        }

        foreach (var p in doc.Processes.Where(p => p.Kind == "bootstrap" && !listed.Contains(p.Id)))
            findings.Add(Finding.Fail("bootstrap.unlisted", p.Id,
                $"line {p.Line}: kind is bootstrap but no row says what retires it"));
    }

    // ---- the referee codebook's worked examples ----

    internal const string CodebookPath = "fanout/referee/codebook.md";

    static void CheckCodebook(string repoRoot, List<Finding> findings)
    {
        var path = Path.Combine(repoRoot, CodebookPath);
        if (!File.Exists(path))
        {
            findings.Add(Finding.Fail("codebook.missing", CodebookPath, "not found under the repo root"));
            return;
        }

        var text = File.ReadAllText(path).Replace("\r\n", "\n");
        var rules = Regex.Matches(text, @"\*\*(R\d+)\b").Select(m => m.Groups[1].Value)
            .ToHashSet(StringComparer.Ordinal);
        if (rules.Count == 0)
        {
            findings.Add(Finding.Fail("codebook.no-rules", CodebookPath,
                "no decision rules of the form **R<n> found"));
            return;
        }

        foreach (var (id, body) in CodebookExamples(text))
        {
            var line = body.Split('\n').FirstOrDefault(l => l.Contains("exercises R", StringComparison.OrdinalIgnoreCase));
            if (line is null)
            {
                findings.Add(Finding.Fail("codebook.example-exercises", id,
                    $"{CodebookPath}: worked example {id} declares no 'exercises R…' line. " +
                    "An example that names no rule is how a duty enters a codebook unruled"));
                continue;
            }
            var named = Regex.Matches(line, @"\bR\d+\b").Select(m => m.Value).ToList();
            if (named.Count == 0)
                findings.Add(Finding.Fail("codebook.example-exercises", id,
                    $"{CodebookPath}: {id}'s exercises line names no rule"));
            foreach (var r in named.Where(r => !rules.Contains(r)))
                findings.Add(Finding.Fail("codebook.unknown-rule", id,
                    $"{CodebookPath}: {id} exercises '{r}', which this codebook does not define"));
        }
    }

    /// <summary>Worked examples are bold-led blocks: <c>**E1 — …**</c> to the next one or heading.</summary>
    internal static IReadOnlyList<(string Id, string Body)> CodebookExamples(string text)
    {
        var result = new List<(string, string)>();
        var lines = text.Split('\n');
        string? current = null;
        var body = new List<string>();
        foreach (var line in lines)
        {
            var m = Regex.Match(line.Trim(), @"^\*\*(E\d+)\b");
            if (m.Success)
            {
                if (current is not null) result.Add((current, string.Join("\n", body)));
                current = m.Groups[1].Value;
                body = [line];
                continue;
            }
            if (current is null) continue;
            if (line.TrimStart().StartsWith('#'))
            {
                result.Add((current, string.Join("\n", body)));
                current = null;
                continue;
            }
            body.Add(line);
        }
        if (current is not null) result.Add((current, string.Join("\n", body)));
        return result;
    }

    // ---- SKILL.md's published limits and the one-level-deep rule ----

    static void CheckSkill(string skillFolder, List<Finding> findings)
    {
        var skillPath = Path.Combine(skillFolder, "SKILL.md");
        if (!File.Exists(skillPath))
        {
            findings.Add(Finding.Fail("skill.missing", "SKILL.md", $"no SKILL.md in {skillFolder}"));
            return;
        }

        var text = File.ReadAllText(skillPath).Replace("\r\n", "\n");
        var lines = text.Split('\n').Length;
        if (lines > SkillLineBudget)
            findings.Add(Finding.Fail("skill.line-budget", "SKILL.md",
                $"{lines} lines exceeds the published budget of {SkillLineBudget}"));

        var description = FrontmatterDescription(text);
        if (description is null)
            findings.Add(Finding.Fail("skill.description-missing", "SKILL.md",
                "no description in the frontmatter"));
        else if (description.Length > SkillDescriptionBudget)
            findings.Add(Finding.Fail("skill.description-length", "SKILL.md",
                $"description is {description.Length} characters, over the published {SkillDescriptionBudget}"));

        var companions = Directory.GetFiles(skillFolder, "*.md")
            .Select(Path.GetFileName)
            .Where(n => n is not null && !string.Equals(n, "SKILL.md", StringComparison.Ordinal))
            .Select(n => n!)
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToList();

        foreach (var companion in companions)
        {
            if (text.Contains(companion, StringComparison.Ordinal)) continue;

            var via = companions
                .Where(other => !string.Equals(other, companion, StringComparison.Ordinal))
                .FirstOrDefault(other =>
                    File.ReadAllText(Path.Combine(skillFolder, other)).Contains(companion, StringComparison.Ordinal));

            findings.Add(Finding.Fail("skill.companion-unlinked", companion,
                via is null
                    ? "not named by SKILL.md; a companion nothing routes to is unreachable"
                    : $"named only by {via}, not by SKILL.md. References are one level deep from SKILL.md"));
        }
    }

    internal static string? FrontmatterDescription(string text)
    {
        var lines = text.Split('\n');
        if (lines.Length == 0 || lines[0].Trim() != "---") return null;
        for (var i = 1; i < lines.Length; i++)
        {
            if (lines[i].Trim() == "---") return null;
            if (!lines[i].StartsWith("description:", StringComparison.Ordinal)) continue;

            var value = lines[i]["description:".Length..].Trim();
            var builder = new List<string> { value };
            for (var j = i + 1; j < lines.Length; j++)
            {
                var l = lines[j];
                if (l.Trim() == "---") break;
                if (Regex.IsMatch(l, @"^[A-Za-z_-]+:")) break;
                builder.Add(l.Trim());
            }
            return string.Join(" ", builder.Where(s => s.Length > 0)).Trim();
        }
        return null;
    }

    // ---- reports that are not verdicts ----

    static void ReportInformational(ProcessMapDocument doc, List<Finding> findings)
    {
        // Only fan-in worth looking at. One row per governor is the ordinary case; the report
        // exists so a document asked to govern many rows is visible, and can be split if it is
        // not in fact one reading assignment.
        foreach (var (file, rows) in GraphRules.GovernorFanIn(doc).Where(g => g.Rows.Count > 1))
            findings.Add(Finding.Info("info.governor-fan-in", file,
                $"governs {rows.Count} rows: {string.Join(" ", rows)}"));

        void Unused(string set, string[] declared, IEnumerable<string> used)
        {
            var live = used.ToHashSet(StringComparer.Ordinal);
            foreach (var v in declared.Where(v => !live.Contains(v)))
                findings.Add(Finding.Info("info.unused-enum-value", set,
                    $"'{v}' is declared by the schema and used by no row"));
        }

        Unused("Roots.kind", ClosedSets.RootKinds, doc.Roots.Select(r => r.Kind));
        Unused("Files.keep", ClosedSets.Keeps, doc.Files.Select(f => f.Keep));
        Unused("Processes.level", ClosedSets.Levels, doc.Processes.Select(p => p.Level));
        Unused("Processes.kind", ClosedSets.ProcessKinds, doc.Processes.Select(p => p.Kind));
        Unused("Processes.state", ClosedSets.States, doc.Processes.Select(p => p.State));
        Unused("Edges.kind", ClosedSets.EdgeKinds, doc.Edges.Select(e => e.Kind));
    }
}
