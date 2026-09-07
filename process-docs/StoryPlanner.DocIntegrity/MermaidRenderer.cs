using System.Text;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// Generates <c>map.md</c>: the activities and their <c>enables</c> edges, one section per
/// activity (its processes, the artifacts they touch, and what the tables derive for it),
/// the whole graph, the consumers table and the validation report. One whole file, never
/// hand-edited, and deterministic: the same rows produce byte-identical text, so a diff is a
/// row change and never a re-render. Until 2026-09-06 the activity pieces were rendered into
/// marked sections inside the authored files; they are files-only now (rulings log).
///
/// Shapes carry the mode: hexagon for hitl, box for session, stadium for agent, slanted box
/// for an artifact. Reads and writes are solid arrows into and out of a process; an
/// instrument read is dashed.
/// </summary>
public static class MermaidRenderer
{
    const string Stamp =
        "> **UNVALIDATED** — rendered with `--force` while `validate` still fails. Not the record.\n\n";

    const string ClassDefs = """
          classDef hitl fill:#e9d8e4,stroke:#7a3e6d,color:#2b1a27
          classDef session fill:#dce6f0,stroke:#3b5b7c,color:#14202c
          classDef agent fill:#f5e6c8,stroke:#b7791f,color:#3a2a08
          classDef artifact fill:#f6f6f4,stroke:#8a94a0,color:#2a2f36
          classDef activity fill:#dcebdd,stroke:#4b7f52,color:#122816
          classDef terminus fill:#e4e4ea,stroke:#5b5b7a,color:#1c1c2c
        """;

    // ---- level-1: the activities ----

    public static string Level1(SkillDocument doc)
    {
        var sb = new StringBuilder();
        var termini = GraphRules.Termini(doc).ToHashSet(StringComparer.Ordinal);
        var ids = doc.Activities.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);

        sb.Append("```mermaid\nflowchart TD\n").Append(ClassDefs).Append('\n');
        foreach (var a in doc.Activities)
            sb.Append("  ").Append(NodeId(a.Id))
              .Append(termini.Contains(a.Id) ? $"[[\"{a.Id}\"]]:::terminus" : $"[\"{a.Id}\"]:::activity")
              .Append('\n');
        sb.Append('\n');
        foreach (var a in doc.Activities)
        foreach (var e in a.Enables.Where(ids.Contains))
            sb.Append("  ").Append(NodeId(a.Id)).Append(" --> ").Append(NodeId(e)).Append('\n');
        sb.Append("```\n");
        return sb.ToString();
    }

    // ---- activity: one file's processes and what is derived for it ----

    public static string Activity(SkillDocument doc, string activityId)
    {
        var members = doc.ProcessesOf(activityId).ToList();
        var artifactIds = doc.Artifacts.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        var sb = new StringBuilder();

        sb.Append("```mermaid\nflowchart LR\n").Append(ClassDefs).Append('\n');
        foreach (var p in members) sb.Append("  ").Append(ProcessNode(p)).Append('\n');

        var touched = members
            .SelectMany(p => p.Reads.Concat(p.Instruments).Concat(p.Writes))
            .Where(artifactIds.Contains)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(a => a, StringComparer.Ordinal)
            .ToList();
        foreach (var a in touched) sb.Append("  ").Append(ArtifactNode(a)).Append('\n');
        sb.Append('\n');
        foreach (var p in members) AppendEdges(sb, p, artifactIds);
        sb.Append("```\n\n");

        var writtenHere = members.SelectMany(p => p.Writes).ToHashSet(StringComparer.Ordinal);
        var inputs = members
            .SelectMany(p => p.Reads.Concat(p.Instruments.Where(artifactIds.Contains)))
            .Where(a => !writtenHere.Contains(a))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(a => a, StringComparer.Ordinal);
        var outputs = members.SelectMany(p => p.Writes).Distinct(StringComparer.Ordinal).OrderBy(a => a, StringComparer.Ordinal);
        var instruments = members.SelectMany(p => p.Instruments).Distinct(StringComparer.Ordinal).OrderBy(a => a, StringComparer.Ordinal);
        var enabledBy = GraphRules.EnabledBy(doc, activityId);
        var enables = doc.Activities.FirstOrDefault(a => a.Id == activityId)?.Enables ?? [];

        sb.Append("Derived from the tables, never authored:\n\n");
        sb.Append("- **inputs**: ").Append(List(inputs)).Append('\n');
        sb.Append("- **outputs**: ").Append(List(outputs)).Append('\n');
        sb.Append("- **instruments**: ").Append(List(instruments)).Append('\n');
        sb.Append("- **enabled by**: ").Append(List(enabledBy)).Append('\n');
        sb.Append("- **enables**: ").Append(List(enables)).Append('\n');
        return sb.ToString();
    }

    // ---- map.md: the activities, one section each, the whole graph, consumers, validation ----

    public static string Map(SkillDocument doc, ValidationReport report, bool forced)
    {
        var artifactIds = doc.Artifacts.Select(a => a.Id).ToHashSet(StringComparer.Ordinal);
        var sb = new StringBuilder();
        sb.Append("# map.md — generated by process-docs/StoryPlanner.DocIntegrity\n\n");
        sb.Append("Never hand-edited: `render` rewrites this file whole from the tables in SKILL.md, the activity " +
                  "files and artifacts.md, and the write hook rewrites it after every passing validate. " +
                  "Nothing here is authored.\n\n");
        if (forced) sb.Append(Stamp);

        sb.Append("## The activities\n\n").Append(Level1(doc)).Append('\n');

        sb.Append("## Each activity\n\n");
        foreach (var a in doc.Activities)
        {
            if (!doc.ProcessesOf(a.Id).Any()) continue;
            sb.Append("### ").Append(a.Id).Append("\n\n").Append(Activity(doc, a.Id)).Append('\n');
        }

        sb.Append("## The whole graph\n\n```mermaid\nflowchart TD\n").Append(ClassDefs).Append('\n');
        foreach (var a in doc.Activities)
        {
            var members = doc.ProcessesOf(a.Id).ToList();
            if (members.Count == 0) continue;
            sb.Append("  subgraph ").Append(NodeId(a.Id)).Append("[\"").Append(a.Id).Append("\"]\n");
            foreach (var p in members) sb.Append("    ").Append(ProcessNode(p)).Append('\n');
            sb.Append("  end\n");
        }
        foreach (var a in doc.Artifacts) sb.Append("  ").Append(ArtifactNode(a.Id)).Append('\n');
        sb.Append('\n');
        foreach (var p in doc.Processes) AppendEdges(sb, p, artifactIds);
        sb.Append("```\n\n");

        sb.Append("## Consumers\n\n");
        sb.Append("| artifact | written by | read by | instrument of |\n|---|---|---|---|\n");
        foreach (var t in GraphRules.Traffic(doc))
            sb.Append("| ").Append(t.ArtifactId)
              .Append(" | ").Append(List(t.Writers))
              .Append(" | ").Append(List(t.Readers))
              .Append(" | ").Append(List(t.InstrumentOf))
              .Append(" |\n");
        sb.Append('\n');

        sb.Append("## Validation\n\n");
        sb.Append(report.Passed
            ? $"Last run: **passed** ({report.Findings.Count} note(s)).\n\n"
            : $"Last run: **{report.Failures} failure(s)**.\n\n");
        sb.Append("| level | rule | row | message |\n|---|---|---|---|\n");
        foreach (var f in report.Findings)
            sb.Append("| ").Append(f.Level.ToString().ToLowerInvariant())
              .Append(" | ").Append(f.RuleId)
              .Append(" | ").Append(f.RowId)
              .Append(" | ").Append(f.Message.Replace("|", "\\|"))
              .Append(" |\n");
        return sb.ToString();
    }

    // ---- pieces ----

    static string ProcessNode(ProcessRow p)
    {
        var label = $"{p.Id}<br/>{Escape(p.Mode)}";
        var shape = p.Mode switch
        {
            "hitl" => $"{{{{\"{label}\"}}}}",
            "agent" => $"([\"{label}\"])",
            _ => $"[\"{label}\"]",
        };
        var cls = ClosedSets.Modes.Contains(p.Mode) ? p.Mode : "session";
        return $"{NodeId(p.Id)}{shape}:::{cls}";
    }

    static string ArtifactNode(string id) => $"{NodeId(id)}[/\"{id}\"/]:::artifact";

    static void AppendEdges(StringBuilder sb, ProcessRow p, HashSet<string> artifactIds)
    {
        foreach (var a in p.Reads.Where(artifactIds.Contains))
            sb.Append("  ").Append(NodeId(a)).Append(" --> ").Append(NodeId(p.Id)).Append('\n');
        foreach (var a in p.Instruments.Where(artifactIds.Contains))
            sb.Append("  ").Append(NodeId(a)).Append(" -.-> ").Append(NodeId(p.Id)).Append('\n');
        foreach (var a in p.Writes.Where(artifactIds.Contains))
            sb.Append("  ").Append(NodeId(p.Id)).Append(" --> ").Append(NodeId(a)).Append('\n');
    }

    static string List(IEnumerable<string> items)
    {
        var list = items.ToList();
        return list.Count == 0 ? "—" : string.Join(" ", list);
    }

    /// <summary>
    /// Mermaid node ids take no hyphens, so <c>referee-run</c> is drawn as <c>refereerun</c>.
    /// Two ids that differ only by hyphens would collide; <see cref="CheckNodeIds"/> refuses.
    /// </summary>
    public static string NodeId(string id) => id.Replace("-", "").Replace(".", "");

    /// <summary>Refuses to draw a graph whose ids merge once hyphens are dropped.</summary>
    public static void CheckNodeIds(SkillDocument doc)
    {
        var all = doc.Activities.Select(a => a.Id)
            .Concat(doc.Processes.Select(p => p.Id))
            .Concat(doc.Artifacts.Select(a => a.Id));
        var collisions = all.GroupBy(NodeId, StringComparer.Ordinal).Where(g => g.Count() > 1).ToList();
        if (collisions.Count > 0)
            throw new MapFormatException(
                "ids that merge once hyphens are dropped, so a diagram would draw them as one node: " +
                string.Join("; ", collisions.Select(g => string.Join(" and ", g))),
                "render.node-id-collision");
    }

    static string Escape(string text) => text
        .Replace("\"", "&quot;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");
}
