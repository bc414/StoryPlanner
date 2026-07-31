using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using ModelContextProtocol.Server;
using StoryPlanner.Core;

namespace StoryPlanner.Mcp;

/// <summary>
/// Tools over the imported AI conversation transcripts (Conversation Reader data, stored in
/// the working .storyplan file). An independent corpus — these tools never join the plan.
/// </summary>
[McpServerToolType]
public sealed class ConversationTools(StoryPlanSources sources)
{
    private PlanCache C => sources.Get(Corpus.Working);

    private const int BlockContentCap = 20_000;

    private static string BlockStateLabel(BlockState s) => s switch
    {
        BlockState.Unread => "unread",
        BlockState.Skipped => "skipped",
        BlockState.Flagged => "flagged",
        BlockState.Done => "done",
        _ => $"state{(int)s}"
    };

    [McpServerTool(Name = "search_conversations")]
    [Description("Regex search over imported AI conversation transcripts: block RawContent, block Summary, conversation titles, and conversation ArcSummary. Each hit labels which field matched and carries the author's block triage state (unread/skipped/flagged/done). Fetch full block text with get_blocks.")]
    public string SearchConversations(
        [Description("Regular expression (.NET syntax). Case-insensitive unless caseSensitive=true.")] string pattern,
        [Description("Match case-sensitively (default false).")] bool caseSensitive = false,
        [Description("Match whole words only (default false).")] bool wholeWord = false,
        [Description("Characters of context around each match snippet (default 150, max 2000).")] int contextChars = 150,
        [Description("Maximum hits returned (default 40, max 250).")] int limit = 40)
    {
        Regex rx;
        try { rx = Query.BuildRegex(pattern, caseSensitive, wholeWord); }
        catch (ArgumentException ex) { return $"Invalid regex: {ex.Message}"; }

        contextChars = Math.Clamp(contextChars, 20, 2000);
        limit = Math.Clamp(limit, 1, 250);

        var c = C;
        var lines = new List<string>();
        int blockContentHits = 0, blockSummaryHits = 0, convHits = 0;

        try
        {
            foreach (var conv in c.Conversations)
            {
                var mTitle = rx.Match(conv.Title);
                var mArc = rx.Match(conv.ArcSummary);
                if (!mTitle.Success && !mArc.Success) continue;
                convHits++;
                if (lines.Count < limit)
                {
                    var where = mTitle.Success
                        ? $"title: \"{conv.Title}\""
                        : $"arc: \"{Query.Snippet(conv.ArcSummary, mArc, contextChars)}\"";
                    lines.Add($"conv:{conv.Id} [{conv.Platform} {conv.ConversationDate:yyyy-MM-dd}] \"{Query.Truncate(conv.Title, 60)}\" — {where}");
                }
            }

            foreach (var b in c.Blocks)
            {
                var mContent = rx.Match(b.RawContent);
                var mSummary = rx.Match(b.Summary);
                if (!mContent.Success && !mSummary.Success) continue;
                if (mContent.Success) blockContentHits++; else blockSummaryHits++;
                if (lines.Count < limit)
                {
                    var conv = c.ConversationById.TryGetValue(b.ConversationId, out var cv) ? cv : null;
                    var convLabel = conv is null ? $"conv:{b.ConversationId}?" : $"conv:{conv.Id} \"{Query.Truncate(conv.Title, 45)}\"";
                    var where = mContent.Success
                        ? $"content: \"{Query.Snippet(b.RawContent, mContent, contextChars)}\""
                        : $"summary: \"{Query.Snippet(b.Summary, mSummary, contextChars)}\"";
                    lines.Add($"block:{b.Id} [{convLabel} #{b.BlockNumber} {b.Speaker}] ({BlockStateLabel(b.BlockState)}) {where}");
                }
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return "Regex timed out (2s) — simplify the pattern.";
        }

        var total = convHits + blockContentHits + blockSummaryHits;
        var sb = new StringBuilder();
        sb.AppendLine($"# search conversations /{pattern}/ — {total} matches " +
                      $"(conversations {convHits}, block content {blockContentHits}, block summaries {blockSummaryHits})" +
                      (total > lines.Count ? $". Showing first {lines.Count}." : "."));
        if (total == 0) sb.AppendLine("(no matches)");
        foreach (var l in lines) sb.AppendLine(l);
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "get_blocks")]
    [Description("Fetch full transcript blocks by block id: verbatim RawContent (capped at 20k chars per block), speaker, block number, triage state, compaction marker, stored summary, and the owning conversation. Ids come from search_conversations hits.")]
    public string GetBlocks(
        [Description("Block ids.")] int[] ids)
    {
        var c = C;
        var sb = new StringBuilder();
        int found = 0, missing = 0;
        var body = new StringBuilder();

        foreach (var id in ids.Distinct())
        {
            if (!c.BlockById.TryGetValue(id, out var b))
            {
                missing++;
                body.AppendLine($"## block:{id} — not found");
                continue;
            }
            found++;
            var conv = c.ConversationById.TryGetValue(b.ConversationId, out var cv) ? cv : null;
            var convLabel = conv is null
                ? $"conv:{b.ConversationId}?"
                : $"conv:{conv.Id} \"{conv.Title}\" [{conv.Platform} {conv.ConversationDate:yyyy-MM-dd}]";
            body.AppendLine($"## block:{b.Id} — {convLabel} #{b.BlockNumber} — {b.Speaker} — {BlockStateLabel(b.BlockState)}" +
                            (b.IsCompaction ? " — COMPACTION BLOCK" : ""));
            if (b.Summary.Length > 0) body.AppendLine($"summary: {Query.OneLine(b.Summary)}");
            var content = b.RawContent;
            if (content.Length > BlockContentCap)
            {
                body.AppendLine(content[..BlockContentCap]);
                body.AppendLine($"[BLOCK TRUNCATED — {content.Length - BlockContentCap} more chars in block:{b.Id}]");
            }
            else body.AppendLine(content.Length == 0 ? "(empty)" : content.TrimEnd());
            body.AppendLine();
        }

        sb.AppendLine($"# get_blocks — {found} returned, {missing} not found");
        sb.Append(body);
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "list_conversations")]
    [Description("Inventory of all imported conversations: id, platform, date, title, block count, and per-state triage tallies (unread/skipped/flagged/done). Ordered by conversation date.")]
    public string ListConversations()
    {
        var c = C;
        var sb = new StringBuilder();
        sb.AppendLine($"# conversations — {c.Conversations.Count} total, {c.Blocks.Count} blocks");
        foreach (var conv in c.Conversations.OrderBy(x => x.ConversationDate).ThenBy(x => x.Id))
        {
            var blocks = c.Blocks.Where(b => b.ConversationId == conv.Id).ToList();
            var tallies = blocks.GroupBy(b => b.BlockState)
                .OrderBy(g => (int)g.Key)
                .Select(g => $"{g.Count()} {BlockStateLabel(g.Key)}");
            sb.AppendLine($"conv:{conv.Id} [{conv.Platform} {conv.ConversationDate:yyyy-MM-dd}] \"{conv.Title}\" — {blocks.Count} blocks: {string.Join(", ", tallies)}");
        }
        return Query.Cap(sb);
    }
}
