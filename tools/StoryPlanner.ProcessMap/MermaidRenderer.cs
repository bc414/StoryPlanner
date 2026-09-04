using System.Text;

namespace StoryPlanner.ProcessMap;

/// <summary>
/// Generates the five marked sections of <c>process-map.md</c>. Never hand-edited, and
/// deterministic: the same rows produce byte-identical text, so a diff is a row change and
/// never a re-render.
///
/// Conventions carried from <c>process-map-1-draft.md</c>: a classDef per actor kind, slanted
/// boxes for files, diamonds for choices, a circle marked ∥ for a fork, dashed for optional,
/// edge labels from the Edges table.
/// </summary>
public static class MermaidRenderer
{
    public static readonly string[] SectionNames = ["level-1", "level-2", "level-3", "consumers", "validation"];

    const string ClassDefs = """
          classDef brian fill:#e9d8e4,stroke:#7a3e6d,color:#2b1a27
          classDef hitl fill:#dce6f0,stroke:#3b5b7c,color:#14202c
          classDef agent fill:#f5e6c8,stroke:#b7791f,color:#3a2a08
          classDef script fill:#dcebdd,stroke:#4b7f52,color:#122816
          classDef tool fill:#e4e4ea,stroke:#5b5b7a,color:#1c1c2c
          classDef file fill:#f6f6f4,stroke:#8a94a0,color:#2a2f36
        """;

    static readonly Dictionary<string, string> LevelNames = new(StringComparer.Ordinal)
    {
        ["P"] = "the buildout cycle",
        ["E"] = "exploratory WU",
        ["V"] = "verification WU",
        ["S"] = "synthesis WU",
        ["I"] = "infrastructure WU",
        ["R"] = "a runner run",
        ["F"] = "the referee run",
        ["M"] = "promotion",
    };

    public static IReadOnlyDictionary<string, string> RenderAll(
        ProcessMapDocument doc, ValidationReport report, bool forced)
    {
        var stamp = forced
            ? "> **UNVALIDATED** — rendered with `--force` while `validate` still fails. Not the record.\n\n"
            : "";
        return new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["level-1"] = stamp + Level1(doc),
            ["level-2"] = stamp + Levels(doc, ["E", "V", "S", "I"]),
            ["level-3"] = stamp + Levels(doc, ["R", "F", "M"]),
            ["consumers"] = stamp + Consumers(doc),
            ["validation"] = stamp + ValidationTable(report),
        };
    }

    // ---- diagrams ----

    static string Level1(ProcessMapDocument doc) => Diagram(doc, ["P"], "Level 1 — the buildout cycle");

    static string Levels(ProcessMapDocument doc, string[] levels)
    {
        var sb = new StringBuilder();
        foreach (var level in levels)
        {
            sb.Append(Diagram(doc, [level], $"{level} — {LevelNames[level]}"));
            sb.Append('\n');
        }
        return sb.ToString().TrimEnd('\n') + "\n";
    }

    /// <summary>
    /// One diagram over the rows of <paramref name="levels"/>. A neighbour outside those levels
    /// is drawn as one collapsed node per level, so a diagram never silently drops an edge.
    /// </summary>
    static string Diagram(ProcessMapDocument doc, string[] levels, string title)
    {
        var members = doc.Processes.Where(p => levels.Contains(p.Level)).ToList();
        var memberIds = members.Select(p => p.Id).ToHashSet(StringComparer.Ordinal);
        var byId = doc.Processes.ToDictionary(p => p.Id, StringComparer.Ordinal);

        var sb = new StringBuilder();
        sb.Append("### ").Append(title).Append("\n\n```mermaid\nflowchart TD\n");
        sb.Append(ClassDefs).Append('\n');

        var choiceSources = doc.Edges.Where(e => e.Kind == "choice").Select(e => e.From).ToHashSet(StringComparer.Ordinal);
        var forkSources = doc.Edges.Where(e => e.Kind == "fork").Select(e => e.From).ToHashSet(StringComparer.Ordinal);

        foreach (var p in members)
        {
            var label = $"{p.Id} {Escape(p.Text)}<br/>{Escape(p.Actor)}";
            var shape = forkSources.Contains(p.Id) ? $"((\"∥ {label}\"))"
                : choiceSources.Contains(p.Id) ? $"{{\"{label}\"}}"
                : $"[\"{label}\"]";
            sb.Append("  ").Append(NodeId(p.Id)).Append(shape)
              .Append(":::").Append(ActorClass(p.Actor)).Append('\n');
        }

        // Collapsed neighbours, one node per foreign level.
        var foreignLevels = doc.Edges
            .Where(e => memberIds.Contains(e.From) ^ memberIds.Contains(e.To))
            .Select(e => memberIds.Contains(e.From) ? e.To : e.From)
            .Where(byId.ContainsKey)
            .Select(id => byId[id].Level)
            .Where(l => !levels.Contains(l))
            .Distinct()
            .OrderBy(l => l, StringComparer.Ordinal)
            .ToList();
        foreach (var l in foreignLevels)
            sb.Append("  ").Append(CollapsedId(l))
              .Append($"[\"{l} — {LevelNames.GetValueOrDefault(l, "other")}\"]\n");

        // Files touched by the member rows.
        var files = members.SelectMany(p => p.Inputs.Concat(p.Outputs)).Distinct()
            .OrderBy(f => f, StringComparer.Ordinal).ToList();
        foreach (var f in files)
        {
            var row = doc.Files.FirstOrDefault(x => x.Id == f);
            sb.Append("  ").Append(NodeId(f)).Append($"[/\"{Escape(row?.Path ?? f)}\"/]:::file\n");
        }

        sb.Append('\n');

        foreach (var e in doc.Edges)
        {
            var fromIn = memberIds.Contains(e.From);
            var toIn = memberIds.Contains(e.To);
            if (!fromIn && !toIn) continue;
            var from = fromIn ? NodeId(e.From) : CollapsedFor(byId, e.From, levels);
            var to = toIn ? NodeId(e.To) : CollapsedFor(byId, e.To, levels);
            if (from is null || to is null) continue;
            sb.Append("  ").Append(EdgeLine(from, to, e.Kind, e.Label)).Append('\n');
        }

        foreach (var p in members)
        {
            foreach (var f in p.Inputs) sb.Append("  ").Append(NodeId(f)).Append(" --> ").Append(NodeId(p.Id)).Append('\n');
            foreach (var f in p.Outputs) sb.Append("  ").Append(NodeId(p.Id)).Append(" --> ").Append(NodeId(f)).Append('\n');
        }

        sb.Append("```\n");
        return sb.ToString();
    }

    static string? CollapsedFor(IReadOnlyDictionary<string, ProcessRow> byId, string id, string[] levels)
    {
        if (!byId.TryGetValue(id, out var row)) return null;
        return levels.Contains(row.Level) ? NodeId(id) : CollapsedId(row.Level);
    }

    static string EdgeLine(string from, string to, string kind, string label)
    {
        var arrow = kind == "optional" ? "-.->" : "-->";
        if (label.Length == 0) return $"{from} {arrow} {to}";
        return kind == "optional"
            ? $"{from} -. \"{Escape(label)}\" .-> {to}"
            : $"{from} -- \"{Escape(label)}\" --> {to}";
    }

    // ---- tables ----

    static string Consumers(ProcessMapDocument doc)
    {
        var sb = new StringBuilder();
        sb.Append("| file | written by | read by |\n|---|---|---|\n");
        foreach (var t in GraphRules.Traffic(doc))
            sb.Append("| ").Append(t.FileId)
              .Append(" | ").Append(t.Producers.Count == 0 ? "—" : string.Join(" ", t.Producers))
              .Append(" | ").Append(t.Consumers.Count == 0 ? "—" : string.Join(" ", t.Consumers))
              .Append(" |\n");
        return sb.ToString();
    }

    static string ValidationTable(ValidationReport report)
    {
        var sb = new StringBuilder();
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

    // ---- ids ----

    /// <summary>
    /// Mermaid node ids take no dots, so <c>P.1</c> is drawn as <c>P1</c> — the same display form
    /// draft 1 used by hand, which is what makes the <c>nodes</c> set comparison possible.
    /// </summary>
    public static string NodeId(string id) => id.Replace(".", "").Replace("-", "");

    static string CollapsedId(string level) => "LVL" + level;

    static string ActorClass(string actor)
    {
        if (actor == "brian") return "brian";
        if (actor == "script") return "script";
        if (actor == "tool") return "tool";
        if (actor.StartsWith("hitl:", StringComparison.Ordinal)) return "hitl";
        if (actor.StartsWith("agent:", StringComparison.Ordinal)) return "agent";
        return "tool";
    }

    static string Escape(string text) => text
        .Replace("\"", "&quot;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");
}
