using System.Text;

namespace StoryPlanner.SessionTree;

/// <summary>
/// Renders a family as one Markdown document: a header (members, the segment outline,
/// warnings), then each segment once, depth first. Output depends only on the archive's
/// contents, never on which member named the family or when it ran, so two runs over the same
/// ingest are byte-identical.
/// </summary>
public static class TreeRenderer
{
    public sealed record Rendered(string Outline, string Document);

    private sealed record SubagentBlock(string AgentId, SubagentInfo? Subagent, string Report, string Source, string ReportTimestamp, bool First);

    public static Rendered Render(Family family, ForkTree tree, DiskStatus? disk, string dbPath)
    {
        var (atNode, afterNode) = PlaceSubagents(family, tree);

        var outline = new StringBuilder();
        outline.Ln($"# Fork family · {Stamp(family.Root.Timestamp)}");
        outline.Ln();
        outline.Ln($"Rebuilt by `tools/StoryPlanner.SessionTree` from the code-sessions archive (`{dbPath}`, " +
                           $"last ingest run {Stamp(family.LastIngestUtc)}). Every stretch the forked sessions share " +
                           "appears once; each branch appears once, under the stretch it continues. Dialogue only: " +
                           "tool calls, tool results, thinking and harness injections are omitted, except subagent " +
                           "prompts and reports and compaction markers.");
        outline.Ln();
        outline.Ln("**Reading it.** **Brian** turns are his own words. In an `[AskUserQuestion]` block, `Chose:` " +
                           "is a Claude-written option label he selected (his decision, not his wording), `Typed:` is " +
                           "his own text, and a `Q:` line is Claude's wording. **Claude** turns and subagent reports are " +
                           "model output. Session titles are the platform's machine labels.");
        outline.Ln();

        outline.Ln("## Sessions");
        outline.Ln();
        outline.Ln("| session | title (platform label) | first | last | archive records | extract v | disk |");
        outline.Ln("|---|---|---|---|---|---|---|");
        foreach (var m in family.Members)
        {
            var status = disk is not null && disk.MemberStatus.TryGetValue(m.SessionId, out var s) ? s : "not checked";
            outline.Ln($"| `{Short(m.SessionId)}` | {(m.Title ?? "").Replace("|", "\\|")} | {Stamp(m.FirstTimestamp)} | " +
                               $"{Stamp(m.LastTimestamp)} | {m.Records.Count} | {m.ExtractVersion} | {status} |");
        }
        outline.Ln();

        outline.Ln("## Tree");
        outline.Ln();
        outline.Ln("```");
        foreach (var top in tree.Segments.Where(s => s.Parent is null)) OutlineLine(outline, top, "", "", "");
        outline.Ln("```");
        outline.Ln();

        outline.Ln("## Warnings");
        outline.Ln();
        var warnings = Warnings(family, tree, disk);
        if (warnings.Count == 0) outline.Ln("None.");
        foreach (var w in warnings) outline.Ln($"- {w}");
        outline.Ln();

        var doc = new StringBuilder(outline.ToString());
        foreach (var segment in tree.Segments) RenderSegment(doc, segment, atNode, afterNode);

        return new Rendered(outline.ToString(), doc.ToString());
    }

    private static void OutlineLine(StringBuilder sb, Segment s, string lead, string branch, string childLead)
    {
        var ends = s.EndingHere.Count > 0 ? $" · ends: {string.Join(", ", s.EndingHere.Select(Short))}" : "";
        sb.Ln($"{lead}{branch}{s.Label}  {Range(s)} · {s.Nodes.Count} items · " +
                      $"{string.Join(", ", s.Sessions.Select(Short))}{ends}");
        for (var i = 0; i < s.Children.Count; i++)
        {
            var last = i == s.Children.Count - 1;
            OutlineLine(sb, s.Children[i], lead + childLead, last ? "└── " : "├── ", last ? "    " : "│   ");
        }
    }

    public static IReadOnlyList<string> Warnings(Family family, ForkTree tree, DiskStatus? disk)
    {
        var warnings = new List<string>();

        // The tree's own integrity check. Sibling branches never share words by construction;
        // one of Brian's prompts, or a substantial reply, in two siblings means the forks' copies
        // differed in a way the keys did not reconcile, and that text prints twice.
        foreach (var parent in tree.Segments.Where(s => s.Children.Count > 1))
        {
            var repeated = parent.Children
                .SelectMany(c => c.Nodes
                    .Where(n => n.Item!.Kind is ItemKind.Prompt || n.Item.Text.Length >= 200)
                    .Select(n => n.Item!.Text).Distinct().Select(text => (c, text)))
                .GroupBy(x => x.text, StringComparer.Ordinal)
                .Where(g => g.Count() > 1)
                .ToList();
            if (repeated.Count > 0)
                warnings.Add($"{string.Join(", ", parent.Children.Select(c => c.Label))} (branches of {parent.Label}) " +
                             $"repeat {repeated.Count} item(s) after they split — the forks' copies differ in a way " +
                             "this tool did not reconcile, so that text prints more than once.");
        }


        foreach (var m in family.Members.Where(m => m.ExtractVersion < 3))
            warnings.Add($"`{Short(m.SessionId)}` is at extract version {m.ExtractVersion}: its roles are not by authorship, " +
                         "so what the harness injected (skill loads, notifications, compaction summaries) may render as Brian's turns.");
        if (disk is not null)
        {
            foreach (var (id, status) in disk.MemberStatus.Where(kv => kv.Value.StartsWith(FamilyReader.NewerDialogue, StringComparison.Ordinal)))
                warnings.Add($"`{Short(id)}` has dialogue on disk newer than the archive's ({status}) — " +
                             "run the ingest; those turns are missing here.");
            foreach (var stem in disk.UningestedForks)
                warnings.Add($"`{stem}` on disk shares this family's first record but is not in the archive — " +
                             "run the ingest to include it.");
        }
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var m in family.Members)
        foreach (var r in ForkTree.Rewinds(m.SessionId, m.Records))
        {
            // A rewind in a shared stretch is copied into every fork that carries it; report it once.
            if (!seen.Add(string.Join("|", r.PromptTimestamps))) continue;
            warnings.Add($"`{Short(m.SessionId)}` looks rewound in place: its prompts at " +
                         $"{string.Join(", ", r.PromptTimestamps.Select(Stamp))} answer the same point. Both branches " +
                         "are printed in timestamp order; this tool does not split in-session rewinds.");
        }
        return warnings;
    }

    private static void RenderSegment(StringBuilder sb, Segment segment,
        IReadOnlyDictionary<Node, SubagentBlock> atNode, IReadOnlyDictionary<Node, List<SubagentBlock>> afterNode)
    {
        sb.Ln("---");
        sb.Ln();
        var continues = segment.Parent is null ? "root" : $"continues {segment.Parent.Label}";
        sb.Ln($"# {segment.Label} · {continues} · {string.Join(", ", segment.Sessions.Select(Short))}");
        sb.Ln();
        sb.Ln($"*{Range(segment)} · {segment.Nodes.Count} items*");
        sb.Ln();

        string? speaker = null;
        foreach (var node in segment.Nodes)
        {
            var item = node.Item;
            if (item is { Kind: ItemKind.Handback })
            {
                if (atNode.TryGetValue(node, out var block)) { Block(sb, block); speaker = null; }
            }
            else if (item is { Kind: ItemKind.Compaction })
            {
                sb.Ln($"*{item.Text}*");
                sb.Ln();
                speaker = null;
            }
            else if (item is not null)
            {
                var who = item.Kind == ItemKind.Reply ? "Claude" : "Brian";
                if (who != speaker)
                {
                    sb.Ln($"**{who}** · {Stamp(node.Record!.Timestamp)}");
                    sb.Ln();
                    speaker = who;
                }
                sb.Ln(item.Text);
                sb.Ln();
            }

            if (afterNode.TryGetValue(node, out var blocks))
            {
                foreach (var b in blocks) Block(sb, b);
                speaker = null;
            }
        }

        foreach (var id in segment.EndingHere)
        {
            sb.Ln($"*Session {Short(id)} ends here.*");
            sb.Ln();
        }
        if (segment.Children.Count > 0)
        {
            sb.Ln("*Branches here: " + string.Join(" · ", segment.Children.Select(c =>
                $"{c.Label} ({string.Join(", ", c.Sessions.Select(Short))})")) + "*");
            sb.Ln();
        }
    }

    private static void Block(StringBuilder sb, SubagentBlock b)
    {
        var asked = b.Subagent is null ? "" : $" · asked {Stamp(b.Subagent.FirstTimestamp)}";
        sb.Ln($"**Subagent** `agent-{b.AgentId}`{asked} · reported {Stamp(b.ReportTimestamp)} ({b.Source})" +
                      (b.First ? "" : " · a further report"));
        sb.Ln();
        if (b.First)
        {
            sb.Ln("*Prompt:*");
            sb.Ln();
            sb.Ln(b.Subagent is { Records.Count: > 0 }
                ? b.Subagent.Records[0].Body.Replace("\r\n", "\n").Trim()
                : "(the subagent's transcript is not in the archive)");
            sb.Ln();
        }
        sb.Ln("*Report:*");
        sb.Ln();
        sb.Ln(b.Report);
        sb.Ln();
        sb.Ln($"*— end of subagent agent-{b.AgentId} —*");
        sb.Ln();
    }

    /// <summary>
    /// A subagent's report is shown where it reached the parent. The agent-message hand-back is
    /// the report when one exists (a task notification then only points at it); a subagent with
    /// no hand-back in the family is shown at the point its own last record falls in the
    /// parent's timeline, with its final message as the report — often only "Report delivered."
    /// when the real one went by a message the archive stubbed, which the source label discloses.
    /// </summary>
    private static (Dictionary<Node, SubagentBlock> AtNode, Dictionary<Node, List<SubagentBlock>> AfterNode)
        PlaceSubagents(Family family, ForkTree tree)
    {
        // A fork that copied a subagent's transcript would carry it twice; its first record says so.
        var subagents = family.Subagents
            .GroupBy(s => s.Records.Count > 0 ? s.Records[0].Key : (s.SessionId, "", ""))
            .Select(g => g.First())
            .GroupBy(s => s.AgentId, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        var handbacks = tree.Segments.SelectMany(s => s.Nodes)
            .Select(n => (Node: n, n.Item))
            .Where(x => x.Item is { Kind: ItemKind.Handback })
            .GroupBy(x => x.Item!.AgentId!, StringComparer.Ordinal);

        var atNode = new Dictionary<Node, SubagentBlock>();
        var reported = new HashSet<string>(StringComparer.Ordinal);
        foreach (var group in handbacks)
        {
            var kept = group.Any(x => x.Item!.Shape == "agent-message")
                ? group.Where(x => x.Item!.Shape == "agent-message")
                : group;
            var first = true;
            foreach (var (node, item) in kept.OrderBy(x => x.Node.Record!.Timestamp, StringComparer.Ordinal))
            {
                subagents.TryGetValue(group.Key, out var sub);
                atNode[node] = new SubagentBlock(group.Key, sub, item!.Text, $"report from the {item.Shape} hand-back",
                    node.Record!.Timestamp, first);
                first = false;
            }
            reported.Add(group.Key);
        }

        var afterNode = new Dictionary<Node, List<SubagentBlock>>();
        foreach (var sub in subagents.Values.Where(s => !reported.Contains(s.AgentId)).OrderBy(s => s.FirstTimestamp, StringComparer.Ordinal))
        {
            if (!tree.Paths.TryGetValue(sub.ParentSessionId, out var path) || path.Count == 0) continue;
            var anchor = path.LastOrDefault(n => string.CompareOrdinal(n.Record!.Timestamp, sub.LastTimestamp) <= 0) ?? path[0];
            var finalMessage = sub.Records.Skip(1).Select(TurnFilter.Map).LastOrDefault(i => i is { Kind: ItemKind.Reply });
            var block = new SubagentBlock(sub.AgentId, sub,
                finalMessage?.Text ?? "(no report text in the archive)",
                "no hand-back in the parent; report is the subagent's final message",
                sub.LastTimestamp, true);
            if (!afterNode.TryGetValue(anchor, out var list)) afterNode[anchor] = list = [];
            list.Add(block);
        }

        return (atNode, afterNode);
    }

    private static string Range(Segment s)
    {
        var first = s.Nodes[0].Record!.Timestamp;
        var last = s.Nodes[^1].Record!.Timestamp;
        var end = first.Length >= 10 && last.Length >= 16 && first[..10] == last[..10] ? $"{last[11..16]}Z" : Stamp(last);
        return $"{Stamp(first)} – {end}";
    }

    /// <summary>LF, never AppendLine's CRLF: the item texts are LF-normalised, and one document gets one line ending.</summary>
    private static StringBuilder Ln(this StringBuilder sb, string s = "") => sb.Append(s).Append('\n');

    /// <summary>"2026-09-29T01:02:29.465Z" → "2026-09-29 01:02Z".</summary>
    public static string Stamp(string ts) => ts.Length >= 16 ? $"{ts[..10]} {ts[11..16]}Z" : ts;

    public static string Short(string sessionId) => sessionId.Length > 8 ? sessionId[..8] : sessionId;
}
