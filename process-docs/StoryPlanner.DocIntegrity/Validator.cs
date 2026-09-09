using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

public sealed record ValidationReport(IReadOnlyList<Finding> Findings)
{
    public bool Passed => Findings.All(f => f.Level != FindingLevel.Failure);
    public int Failures => Findings.Count(f => f.Level == FindingLevel.Failure);
}

/// <summary>
/// Every check the skill's schema lists (schemas/skill-schema.md § Checks), plus the rulings
/// of 2026-09-05 recorded in the closed decisions record. The validator reads the skill's
/// tables and nothing else: it says what the method as written has, never what any file on
/// disk did.
///
/// Findings carry a check id so a test asserts on the id, never on the prose.
/// </summary>
public static class Validator
{
    public const int SkillLineBudget = 500;          // Anthropic's published figure
    public const int SkillDescriptionBudget = 1024;  // same source

    public static ValidationReport Validate(string skillFolder)
    {
        SkillDocument doc;
        try
        {
            doc = SkillReader.Read(skillFolder);
        }
        catch (MapFormatException ex)
        {
            return new ValidationReport([Finding.Fail(ex.CheckId, "—", ex.Message)]);
        }

        var findings = new List<Finding>();
        CheckIds(doc, findings);
        CheckReferences(doc, findings);
        CheckClosedSets(doc, findings);
        CheckProcessRows(doc, findings);
        CheckArtifacts(doc, findings);
        CheckEnables(doc, findings);
        CheckGate(doc, findings);
        CheckQuestionListWriters(doc, findings);
        CheckMutation(doc, findings);
        CheckFileShape(doc, findings);
        CheckSkill(doc, findings);
        ReportInformational(doc, findings);
        return new ValidationReport(findings);
    }

    static string At(string file, int line) => $"{file}:{line}";

    // ---- ids: one namespace across the three tables ----

    static void CheckIds(SkillDocument doc, List<Finding> findings)
    {
        var seen = new Dictionary<string, string>(StringComparer.Ordinal);
        void Claim(string id, string table, string file, int line)
        {
            if (!ClosedSets.IdPattern.IsMatch(id))
                findings.Add(Finding.Fail("id.charset", id,
                    $"{At(file, line)}: an id is lowercase [a-z0-9-]+"));
            if (seen.TryGetValue(id, out var other))
                findings.Add(Finding.Fail("id.duplicate", id,
                    $"{At(file, line)}: id already used in {other}; ids are unique across all tables"));
            else seen[id] = table;
        }

        foreach (var a in doc.Activities) Claim(a.Id, "Activities", a.File, a.Line);
        foreach (var a in doc.Artifacts) Claim(a.Id, "Artifacts", a.File, a.Line);
        foreach (var p in doc.Processes) Claim(p.Id, $"Processes of {p.Activity}", p.File, p.Line);
    }

    // ---- references resolve ----

    static void CheckReferences(SkillDocument doc, List<Finding> findings)
    {
        var activityIds = doc.Activities.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        var artifactIds = doc.Artifacts.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        var termini = GraphRules.Termini(doc).ToHashSet(StringComparer.Ordinal);

        foreach (var a in doc.Activities)
        {
            foreach (var e in a.Enables.Where(e => !activityIds.Contains(e)))
                findings.Add(Finding.Fail("ref.enables", a.Id,
                    $"{At(a.File, a.Line)}: enables '{e}', which is not an activity id"));
            if (!termini.Contains(a.Id) && !File.Exists(doc.ActivityPath(a.Id)))
                findings.Add(Finding.Fail("ref.companion", a.Id,
                    $"{At(a.File, a.Line)}: no {a.Id}.md in the skill folder; every activity but the " +
                    "terminus owns processes, and its file is where they are"));
        }

        foreach (var p in doc.Processes)
        {
            foreach (var r in p.Reads.Where(r => !artifactIds.Contains(r)))
                findings.Add(Finding.Fail("ref.reads", p.Id,
                    $"{At(p.File, p.Line)}: reads '{r}', which is not an artifact id"));
            foreach (var w in p.Writes.Where(w => !artifactIds.Contains(w)))
                findings.Add(Finding.Fail("ref.writes", p.Id,
                    $"{At(p.File, p.Line)}: writes '{w}', which is not an artifact id"));
        }

        // A schema is the file schemas/<name>-schema.md; the cell is the link
        // [<name>-schema](schemas/<name>-schema.md), so the text is the schema id, the target is
        // the file the text names, and the file's title is its id. The suffix keeps a schema's
        // file name apart from a singleton class's own, so a schema id is never a class id.
        var folder = SkillReader.SchemasFolder;
        var suffix = SkillReader.SchemaSuffix;
        foreach (var a in doc.Artifacts.Where(a => a.Schema.Length > 0))
        {
            var link = SkillReader.SchemaLink.Match(a.SchemaCell.Trim());
            if (!link.Success)
                findings.Add(Finding.Fail("ref.schema", a.Id,
                    $"{At(a.File, a.Line)}: schema '{a.SchemaCell}' is not a link; the cell is " +
                    $"[<name>{suffix}]({folder}/<name>{suffix}.md)"));
            else if (!ClosedSets.IdPattern.IsMatch(a.Schema))
                findings.Add(Finding.Fail("ref.schema", a.Id,
                    $"{At(a.File, a.Line)}: schema '{a.Schema}' is not an id; the link's text is the id of " +
                    $"{folder}/<name>{suffix}.md, a lowercase slug"));
            else if (!a.Schema.EndsWith(suffix, StringComparison.Ordinal) || a.Schema.Length == suffix.Length)
                findings.Add(Finding.Fail("ref.schema", a.Id,
                    $"{At(a.File, a.Line)}: schema '{a.Schema}' is not <name>{suffix}; the suffix keeps a schema's " +
                    "file name apart from the class's own"));
            else if (artifactIds.Contains(a.Schema))
                findings.Add(Finding.Fail("ref.schema", a.Id,
                    $"{At(a.File, a.Line)}: schema '{a.Schema}' is an artifact id; a schema is never a class"));
            else if (link.Groups["target"].Value != $"{folder}/{a.Schema}.md")
                findings.Add(Finding.Fail("ref.schema", a.Id,
                    $"{At(a.File, a.Line)}: schema link '{a.SchemaCell}' points at '{link.Groups["target"].Value}'; " +
                    $"its text names {folder}/{a.Schema}.md"));
            else if (!File.Exists(doc.SchemaPath(a.Schema)))
                findings.Add(Finding.Fail("ref.schema", a.Id,
                    $"{At(a.File, a.Line)}: schema '{a.Schema}' names no file " +
                    $"{folder}/{a.Schema}.md; a schema is a file"));
        }

        foreach (var schema in doc.Artifacts.Select(a => a.Schema).Where(s => s.Length > 0).Distinct(StringComparer.Ordinal))
        {
            var path = doc.SchemaPath(schema);
            if (!File.Exists(path)) continue;
            var text = File.ReadAllText(path);
            var title = new MarkdownOutline(text).Headings.FirstOrDefault();
            if (title is null || title.Level != 1 || title.Text != schema)
                findings.Add(Finding.Fail("schema.shape", schema,
                    $"{folder}/{schema}.md: the title is '# {schema}'; found " +
                    (title is null ? "no heading" : $"'{new string('#', title.Level)} {title.Text}' at line {title.Line}")));

            // A schema in the four-section shape whose Shape declares a sections table is held
            // to the one grammar (schema.fields); a Shape that is prose, or a schema not yet
            // converted, is held at its review. A reference type names a class by its artifact
            // id or by the schema its rows link to.
            var shapeText = StateBuilder.Section(text, "Shape");
            if (!ShapeReader.HasSectionsTable(shapeText)) continue;
            var shape = ShapeReader.Parse(shapeText);
            foreach (var problem in shape.Problems)
                findings.Add(Finding.Fail("schema.fields", schema, $"{folder}/{schema}.md: {problem}"));
            var schemaIds = doc.Artifacts.Select(a => a.Schema).Where(s => s.Length > 0).ToHashSet(StringComparer.Ordinal);
            foreach (var (_, field, type) in shape.References())
                if (type.TargetClass is { } target && !artifactIds.Contains(target) && !schemaIds.Contains(target + SkillReader.SchemaSuffix))
                    findings.Add(Finding.Fail("schema.fields", schema,
                        $"{folder}/{schema}.md: line {field.Line}: '{field.Key}' references the class '{target}', which no Artifacts row declares by id or by schema"));
        }
    }

    // ---- closed sets ----

    static void CheckClosedSets(SkillDocument doc, List<Finding> findings)
    {
        foreach (var p in doc.Processes)
        {
            var modes = p.Mode.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (modes.Length != 1)
                findings.Add(Finding.Fail("row.mode-count", p.Id,
                    $"{At(p.File, p.Line)}: mode '{p.Mode}' is {modes.Length} value(s); a process is one run of one mode"));
            else if (!ClosedSets.Modes.Contains(modes[0]))
                findings.Add(Finding.Fail("enum.mode", p.Id,
                    $"{At(p.File, p.Line)}: mode '{p.Mode}' is not one of {Join(ClosedSets.Modes)}"));

            if (!ClosedSets.States.Contains(p.State))
                findings.Add(Finding.Fail("enum.state", p.Id,
                    $"{At(p.File, p.Line)}: state '{p.State}' is not one of {Join(ClosedSets.States)}"));
        }

        foreach (var a in doc.Artifacts)
            if (!ClosedSets.Mutations.Contains(a.Mutation))
                findings.Add(Finding.Fail("enum.mutation", a.Id,
                    $"{At(a.File, a.Line)}: mutation '{a.Mutation}' is not one of {Join(ClosedSets.Mutations)}"));
    }

    static string Join(string[] values) => string.Join(" | ", values);

    // ---- process row minima ----

    static void CheckProcessRows(SkillDocument doc, List<Finding> findings)
    {
        var artifactIds = doc.Artifacts.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        foreach (var p in doc.Processes)
        {
            var reads = p.Reads.Count + p.Instruments.Count(artifactIds.Contains);
            if (reads == 0)
                findings.Add(Finding.Fail("row.reads-empty", p.Id,
                    $"{At(p.File, p.Line)}: a process reads at least one artifact (an artifact named as an " +
                    "instrument counts); one that reads nothing is deriving from recall"));
            if (p.Writes.Count == 0)
                findings.Add(Finding.Fail("row.writes-empty", p.Id,
                    $"{At(p.File, p.Line)}: a process writes at least one artifact; one that writes nothing " +
                    "is indistinguishable from not having run"));
            if (p.Mode == ClosedSets.Hitl && p.Writes.Count == 0)
                findings.Add(Finding.Fail("row.hitl-writes-nothing", p.Id,
                    $"{At(p.File, p.Line)}: an hitl process writes the artifact that records the decision made in it"));
            if (p.Description.Length == 0)
                findings.Add(Finding.Fail("row.description-empty", p.Id, $"{At(p.File, p.Line)}: no description"));
        }
    }

    // ---- artifacts: path syntax and traffic ----

    static void CheckArtifacts(SkillDocument doc, List<Finding> findings)
    {
        foreach (var a in doc.Artifacts)
            if (!ArtifactPath.TryParse(a.Path, out _, out var error))
                findings.Add(Finding.Fail("artifact.path-syntax", a.Id, $"{At(a.File, a.Line)}: {error}"));

        foreach (var t in GraphRules.Traffic(doc))
        {
            if (t.IsRead) continue;
            var a = doc.Artifact(t.ArtifactId)!;
            findings.Add(Finding.Fail("artifact.never-read", t.ArtifactId,
                t.Writers.Count == 0
                    ? $"{At(a.File, a.Line)}: no process reads or writes it"
                    : $"{At(a.File, a.Line)}: written by {string.Join(", ", t.Writers)} and read by nothing. " +
                      "An artifact read only by a person is read by a process the tables are missing a row for"));
        }
    }

    // ---- the enables graph ----

    static void CheckEnables(SkillDocument doc, List<Finding> findings)
    {
        foreach (var cycle in GraphRules.EnablesCycles(doc))
            findings.Add(Finding.Fail("enables.cycle", cycle[0],
                $"enables is a DAG, and these rows form a cycle: {string.Join(" → ", cycle)} → {cycle[0]}"));

        var termini = GraphRules.Termini(doc);
        if (termini.Count != 1)
            findings.Add(Finding.Fail("enables.terminus-count", "SKILL.md",
                $"{termini.Count} activities enable nothing ({string.Join(", ", termini)}); exactly one is the terminus"));

        foreach (var t in termini)
        {
            var owned = doc.ProcessesOf(t).Select(p => p.Id).ToList();
            if (owned.Count > 0)
                findings.Add(Finding.Fail("enables.terminus-owns-processes", t,
                    $"enables nothing, so it is the terminus, yet owns {owned.Count} process(es): {string.Join(" ", owned)}"));
        }

        var activityIds = doc.Activities.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        var terminiSet = termini.ToHashSet(StringComparer.Ordinal);
        var exempt = new List<string>();
        foreach (var a in doc.Activities)
        foreach (var e in a.Enables.Where(activityIds.Contains))
        {
            if (terminiSet.Contains(e)) { exempt.Add($"{a.Id} → {e}"); continue; }
            if (GraphRules.Backing(doc, a.Id, e).Count == 0)
                findings.Add(Finding.Fail("enables.unbacked", a.Id,
                    $"{At(a.File, a.Line)}: enables {e}, but nothing a process of {a.Id} writes is read by a " +
                    $"process of {e}. Every enables edge is backed by data flow"));
        }
        if (exempt.Count > 0)
            findings.Add(Finding.Vacuous("enables.vacuous",
                $"edges into the terminus are not checked for data flow, since it owns no processes: {string.Join("; ", exempt)}"));
    }

    // ---- the hitl gate: candidates → a hypothesis write ----

    static void CheckGate(SkillDocument doc, List<Finding> findings)
    {
        var source = WellKnown.Candidates;
        var targets = WellKnown.HypothesisArtifacts.Where(doc.HasArtifact).ToList();
        var label = $"{source} → {string.Join("|", WellKnown.HypothesisArtifacts)}";

        if (!doc.HasArtifact(source))
        {
            findings.Add(Finding.Vacuous("gate.vacuous", $"{label}: no artifact row '{source}'"));
            return;
        }
        if (targets.Count == 0)
        {
            findings.Add(Finding.Vacuous("gate.vacuous", $"{label}: no hypothesis artifact row"));
            return;
        }
        if (!doc.Processes.Any(p => p.ReadsOrInstruments(source)))
        {
            findings.Add(Finding.Vacuous("gate.vacuous",
                $"{label}: no process reads '{source}', so the check has no subject today — reported as vacuous, not as passing"));
            return;
        }
        if (!doc.Processes.Any(p => p.Writes.Any(targets.Contains)))
        {
            findings.Add(Finding.Vacuous("gate.vacuous",
                $"{label}: no process writes a hypothesis artifact, so the check has no subject today"));
            return;
        }

        // One finding per writing row, carrying its shortest ungated path.
        var shortest = GraphRules.UngatedPaths(doc, source, targets)
            .GroupBy(p => p.Nodes[^1], StringComparer.Ordinal)
            .Select(g => g.OrderBy(p => p.Nodes.Count).ThenBy(p => p.ToString(), StringComparer.Ordinal).First())
            .OrderBy(p => p.Nodes[^1], StringComparer.Ordinal);

        foreach (var path in shortest)
            findings.Add(Finding.Fail("gate.ungated", path.Nodes[^1],
                $"{path} writes a hypothesis artifact with no hitl process on the path from {source}. " +
                "The path ends at the write: a review after it is detection, and the check is preventive"));
    }

    // ---- questions are Brian's ----

    static void CheckQuestionListWriters(SkillDocument doc, List<Finding> findings)
    {
        if (!doc.HasArtifact(WellKnown.QuestionList))
        {
            findings.Add(Finding.Vacuous("question-list.vacuous", $"no artifact row '{WellKnown.QuestionList}'"));
            return;
        }
        foreach (var p in doc.Processes.Where(p => p.Writes.Contains(WellKnown.QuestionList) && p.Mode != ClosedSets.Hitl))
            findings.Add(Finding.Fail("question-list.writer-not-hitl", p.Id,
                $"{At(p.File, p.Line)}: writes {WellKnown.QuestionList} in mode '{p.Mode}'; a question is Brian's, " +
                "so every writer of the list is hitl"));
    }

    // ---- the mutation rule, frozen only, series exempt (rulings of 2026-09-05) ----

    /// <summary>
    /// Rule 9's row-level shadow. The validator never sees a file edited; the one edit-shaped
    /// thing a row can show is the same artifact under reads and writes. For in-place that is
    /// the declared discipline and for append it is how appending works, so only frozen is
    /// reported. A frozen series (<see cref="ArtifactPath.IsSeries"/>) is exempt: reading the
    /// prior member to write the next is succession, not an edit, and flagging it would push
    /// rows to drop a real read.
    /// </summary>
    static void CheckMutation(SkillDocument doc, List<Finding> findings)
    {
        var frozen = doc.Artifacts
            .Where(a => a.Mutation == ClosedSets.Frozen)
            .Where(a => !(ArtifactPath.TryParse(a.Path, out var path, out _) && path!.IsSeries))
            .Select(a => a.Id)
            .ToHashSet(StringComparer.Ordinal);
        foreach (var p in doc.Processes)
        foreach (var a in p.Writes.Where(a => frozen.Contains(a) && p.ReadsOrInstruments(a)))
            findings.Add(Finding.Fail("mutation.read-and-write", p.Id,
                $"{At(p.File, p.Line)}: reads and writes '{a}', whose mutation is frozen. A frozen artifact " +
                "is written once and never edited; a process that reads and writes it is edit-shaped " +
                "(a series numbered by N or dated is exempt: reading one member to write the next is not an edit)"));
    }

    // ---- the activity file's one shape ----

    static void CheckFileShape(SkillDocument doc, List<Finding> findings)
    {
        foreach (var name in doc.OrphanActivityFiles)
            findings.Add(Finding.Fail("file.orphan-activity", name,
                "not SKILL.md, map.md, state.md or CORPORA.md, and no router row names an activity of " +
                "this name; an activity not in the table is ungoverned"));

        foreach (var name in doc.OrphanSchemaFiles)
            findings.Add(Finding.Fail("file.orphan-schema", $"{SkillReader.SchemasFolder}/{name}",
                "no artifact row's schema names it; a schema file nothing routes to is unreachable"));

        foreach (var a in doc.Activities)
        {
            var path = doc.ActivityPath(a.Id);
            if (!File.Exists(path)) continue;
            var file = a.Id + ".md";
            var outline = new MarkdownOutline(File.ReadAllText(path));

            var title = outline.Headings.FirstOrDefault();
            if (title is null || title.Level != 1 || title.Text != a.Id)
                findings.Add(Finding.Fail("file.shape", a.Id,
                    $"{file}: the title is '# {a.Id}'; found " +
                    (title is null ? "no heading" : $"'{new string('#', title.Level)} {title.Text}' at line {title.Line}")));

            var expected = new List<string> { "Preconditions" };
            expected.AddRange(doc.ProcessesOf(a.Id).Select(p => p.Id));
            expected.Add("Never");
            var found = outline.Headings.Where(h => h.Level == 2).Select(h => h.Text).ToList();
            if (!found.SequenceEqual(expected, StringComparer.Ordinal))
                findings.Add(Finding.Fail("file.shape", a.Id,
                    $"{file}: the sections are Preconditions, one per process id in table order, Never — " +
                    $"expected [{string.Join(", ", expected)}], found [{string.Join(", ", found)}]"));
        }

        CheckNoInlineGenerated(doc, findings);
        CheckNoDecisionIdsOutsideRevising(doc, findings);
    }

    public const string RevisingFile = "revising-the-method.md";

    /// <summary>
    /// Decision ids (<c>d-YYYY-MM-DD-n</c>, schemas/decisions-schema.md) are provenance, read and
    /// written in revising-the-method only. A standard-operating activity file is the
    /// decisions already applied and never cites one, so the id pattern anywhere else in
    /// the folder, the schema files included, is a failure.
    /// </summary>
    static readonly Regex DecisionId = new(@"\bd-\d{4}-\d{2}-\d{2}-\d+\b", RegexOptions.Compiled);

    static void CheckNoDecisionIdsOutsideRevising(SkillDocument doc, List<Finding> findings)
    {
        var files = new List<string> { doc.SkillPath };
        files.AddRange(doc.Activities.Select(a => doc.ActivityPath(a.Id)).Where(File.Exists));
        files.AddRange(doc.SchemaPaths());
        foreach (var path in files)
        {
            if (string.Equals(Path.GetFileName(path), RevisingFile, StringComparison.Ordinal)) continue;
            var label = Path.GetRelativePath(doc.SkillFolder, path).Replace('\\', '/');
            var lines = File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');
            var inFence = false;
            for (var i = 0; i < lines.Length; i++)
            {
                // A schema's example block shows the id's shape; that is the shape, not a citation.
                if (lines[i].TrimStart().StartsWith("```", StringComparison.Ordinal)) { inFence = !inFence; continue; }
                if (inFence) continue;
                var m = DecisionId.Match(lines[i]);
                if (!m.Success) continue;
                findings.Add(Finding.Fail("decision.id-outside-revising", label,
                    $"line {i + 1} cites decision {m.Value}; decisions are cited only in {RevisingFile}"));
                break;
            }
        }
    }

    /// <summary>
    /// Generated text is files only since 2026-09-06. A marker-delimited block still sitting in
    /// an authored file is the earlier convention, not an error in the rows: informational,
    /// because <c>render</c> removes it and is gated on validate passing.
    /// </summary>
    static void CheckNoInlineGenerated(SkillDocument doc, List<Finding> findings)
    {
        foreach (var path in Render.AuthoredFiles(doc))
        {
            if (!Render.HasGeneratedBlock(File.ReadAllText(path))) continue;
            findings.Add(Finding.Info("info.generated.inline-block", Path.GetFileName(path),
                "holds a generated block; generated text is files only (map.md, state.md) and render removes the block"));
        }
    }

    // ---- SKILL.md's published limits and the one-level-deep rule ----

    static void CheckSkill(SkillDocument doc, List<Finding> findings)
    {
        var text = File.ReadAllText(doc.SkillPath).Replace("\r\n", "\n");
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

        // An activity file is linked by its router row (the schema names the file by the id);
        // any other companion must be named in SKILL.md itself.
        var activityFiles = doc.Activities.Select(a => a.Id + ".md").ToHashSet(StringComparer.Ordinal);
        var companions = Directory.GetFiles(doc.SkillFolder, "*.md")
            .Select(Path.GetFileName)
            .Where(n => n is not null && !string.Equals(n, "SKILL.md", StringComparison.Ordinal))
            .Select(n => n!)
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToList();

        foreach (var companion in companions)
        {
            if (activityFiles.Contains(companion)) continue;
            if (text.Contains(companion, StringComparison.Ordinal)) continue;

            var via = companions
                .Where(other => !string.Equals(other, companion, StringComparison.Ordinal))
                .FirstOrDefault(other =>
                    File.ReadAllText(Path.Combine(doc.SkillFolder, other)).Contains(companion, StringComparison.Ordinal));

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

    static void ReportInformational(SkillDocument doc, List<Finding> findings)
    {
        foreach (var t in GraphRules.Traffic(doc).Where(t => t.Writers.Count == 0 && t.IsRead))
            findings.Add(Finding.Info("info.artifact.never-written", t.ArtifactId,
                $"read by {string.Join(", ", t.Readers.Concat(t.InstrumentOf))} and written by no process; " +
                "informational (generated by a tool, or authored outside the method)"));

        var artifactIds = doc.Artifacts.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        var free = doc.Processes.SelectMany(p => p.Instruments)
            .Where(i => !artifactIds.Contains(i))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(i => i, StringComparer.Ordinal)
            .ToList();
        if (free.Count > 0)
            findings.Add(Finding.Info("info.instrument.free-name", "instruments",
                $"instrument names that are not artifact ids: {string.Join(", ", free)}. A program whose code is " +
                "an artifact is named by its id; check none of these should have been"));

        void Unused(string set, string[] declared, IEnumerable<string> used)
        {
            var live = used.ToHashSet(StringComparer.Ordinal);
            foreach (var v in declared.Where(v => !live.Contains(v)))
                findings.Add(Finding.Info("info.unused-enum-value", set,
                    $"'{v}' is declared by the schema and used by no row"));
        }

        Unused("Processes.mode", ClosedSets.Modes, doc.Processes.Select(p => p.Mode));
        Unused("Processes.state", ClosedSets.States, doc.Processes.Select(p => p.State));
        Unused("Artifacts.mutation", ClosedSets.Mutations, doc.Artifacts.Select(a => a.Mutation));
    }
}
