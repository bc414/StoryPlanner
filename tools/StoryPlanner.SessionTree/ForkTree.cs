namespace StoryPlanner.SessionTree;

/// <summary>One dialogue item as the family holds it: shared by every session that passes through it.</summary>
public sealed class Node
{
    internal Node(SessionRecord? record, Item? item)
    {
        Record = record;
        Item = item;
    }

    /// <summary>
    /// The earliest-stamped copy. A fork re-stamps the record it was made at, so the earliest
    /// copy is the original, when the words were actually written.
    /// </summary>
    public SessionRecord? Record { get; internal set; }
    public Item? Item { get; }
    public List<string> Sessions { get; } = [];
    public List<string> EndingHere { get; } = [];
    public Segment Segment { get; internal set; } = null!;
    internal List<Node> Children { get; } = [];
    internal Dictionary<(ItemKind, string, string), Node> ChildByKey { get; } = [];
}

/// <summary>A maximal run of dialogue items shared by exactly the same sessions.</summary>
public sealed class Segment
{
    public int Number { get; internal init; }
    public Segment? Parent { get; internal init; }
    public List<Node> Nodes { get; } = [];
    public IReadOnlyList<string> Sessions { get; internal init; } = [];
    public List<string> EndingHere { get; } = [];
    public List<Segment> Children { get; } = [];
    public string Label => $"S{Number}";
}

public sealed record Rewind(string SessionId, string ParentUuid, IReadOnlyList<string> PromptTimestamps);

/// <summary>
/// The family as a tree, built over the DIALOGUE, not the archive's records, because a fork's
/// copy of history is not byte-faithful (observed 2026-09-29): it re-stamps the record it was
/// made at — the same 8,501-char reply sits at three different timestamps in three forks — and
/// it can drop records, such as one of a set of parallel tool results. Neither the timestamp nor
/// the raw record sequence is a fork identity, so:
///
/// - each session's records are first reduced to the items the document shows (TurnFilter),
///   which leaves out every tool record, where the dropped copies were;
/// - an assistant reply, a hand-back or a compaction marker is keyed by its text alone, which
///   survives the re-stamp;
/// - Brian's prompts and actions keep their timestamp in the key, so "yes" typed at two
///   different moments stays two events. A prompt at a fork point, if one were ever re-stamped,
///   would split early and print twice — a visible duplicate, never a false merge.
///
/// The items go into a trie; single-session chains collapse into segments, and a segment ends
/// exactly where the set of sessions passing through changes — a fork, or a session ending.
/// </summary>
public sealed class ForkTree
{
    private ForkTree(IReadOnlyList<Segment> segments, IReadOnlyDictionary<string, IReadOnlyList<Node>> paths)
    {
        Segments = segments;
        Paths = paths;
    }

    /// <summary>Depth first, children in the order they diverged; S1 is the first.</summary>
    public IReadOnlyList<Segment> Segments { get; }

    /// <summary>Each session's dialogue as nodes, in its own order.</summary>
    public IReadOnlyDictionary<string, IReadOnlyList<Node>> Paths { get; }

    public static (ItemKind, string, string) Key(SessionRecord record, Item item) =>
        item.Kind is ItemKind.Prompt or ItemKind.Action
            ? (item.Kind, record.Timestamp, item.Text)
            : (item.Kind, "", item.Text);

    public static ForkTree Build(IReadOnlyList<(string SessionId, IReadOnlyList<SessionRecord> Records)> sessions)
    {
        var root = new Node(null, null);
        var paths = new Dictionary<string, IReadOnlyList<Node>>(StringComparer.Ordinal);
        foreach (var (sessionId, records) in sessions)
        {
            var node = root;
            var path = new List<Node>();
            foreach (var record in records)
            {
                if (TurnFilter.Map(record) is not { } item) continue;
                var key = Key(record, item);
                if (!node.ChildByKey.TryGetValue(key, out var child))
                {
                    child = new Node(record, item);
                    node.ChildByKey[key] = child;
                    node.Children.Add(child);
                }
                else if (string.CompareOrdinal(record.Timestamp, child.Record!.Timestamp) < 0)
                {
                    child.Record = record;
                }
                child.Sessions.Add(sessionId);
                path.Add(child);
                node = child;
            }
            if (node != root) node.EndingHere.Add(sessionId);
            paths[sessionId] = path;
        }

        var segments = new List<Segment>();
        foreach (var top in Ordered(root.Children)) Collapse(top, null, segments);
        return new ForkTree(segments, paths);
    }

    private static void Collapse(Node start, Segment? parent, List<Segment> segments)
    {
        var segment = new Segment { Number = segments.Count + 1, Parent = parent, Sessions = [.. start.Sessions] };
        segments.Add(segment);
        parent?.Children.Add(segment);

        var node = start;
        while (true)
        {
            segment.Nodes.Add(node);
            node.Segment = segment;
            // A session ending here shrinks the set that continues, so it always closes a segment.
            if (node.Children.Count != 1 || node.EndingHere.Count > 0) break;
            node = node.Children[0];
        }
        segment.EndingHere.AddRange(node.EndingHere);
        foreach (var child in Ordered(node.Children)) Collapse(child, segment, segments);
    }

    /// <summary>Branches in the order they diverged; the first session to reach one breaks a tie.</summary>
    private static IEnumerable<Node> Ordered(List<Node> children) =>
        children.Select((n, i) => (n, i))
            .OrderBy(x => x.n.Record!.Timestamp, StringComparer.Ordinal)
            .ThenBy(x => x.i)
            .Select(x => x.n);

    /// <summary>
    /// A rewind re-sends a prompt from an earlier point, so two of Brian's prompts in one session
    /// answer to the same parent. Comparing ParentUuid strings works even where the parent row
    /// itself was dropped (thinking is never stored). Parallel tool calls also share parents, but
    /// those children are tool calls and tool results, never prompts.
    /// </summary>
    public static IReadOnlyList<Rewind> Rewinds(string sessionId, IReadOnlyList<SessionRecord> records) =>
        records.Where(r => r.ParentUuid is not null && TurnFilter.IsPrompt(r))
            .GroupBy(r => r.ParentUuid!, StringComparer.Ordinal)
            .Where(g => g.Count() > 1)
            .Select(g => new Rewind(sessionId, g.Key, g.Select(r => r.Timestamp).OrderBy(t => t, StringComparer.Ordinal).ToList()))
            .ToList();
}
