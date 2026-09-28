using Microsoft.Data.Sqlite;
using StoryPlanner.Core;

namespace StoryPlanner.VoiceAttribution;

/// <summary>
/// The Conversations corpus as a voice source (2026-09-28): the imported Claude and Gemini
/// transcripts inside a v2 <c>.storyplan</c>, which v2 notes may have been pasted from. Roles
/// map like the lineage layers — <c>user</c> blocks are <c>brian</c>, <c>assistant</c> blocks
/// <c>model</c>. Ids are <c>block:{Id}</c>, the block id <c>get_blocks</c> accepts.
///
/// Blocks carry no timestamps; the import kept only the conversation's <c>created_at</c>
/// (<c>ConversationDate</c>) and, for conversations synced from a Claude export, its
/// <c>updated_at</c> (<c>SourceUpdatedAt</c>). Every block is dated by <c>created_at</c> —
/// the earliest it can be — and the span is kept so the LastModified check can say when an edit
/// falls inside it. Within a conversation blocks are added in order, so on the shared date a
/// prompt beats the reply that repeats it. The block <c>Summary</c> is Brian's navigation note
/// and is not indexed (it never reads as note text).
/// </summary>
public static class ConversationReader
{
    /// <summary>A conversation's date span. <c>To</c> is null when the import recorded no end.</summary>
    public sealed record Span(DateOnly From, DateOnly? To);

    /// <returns>block source id → its conversation's span, for every block indexed.</returns>
    public static Dictionary<string, Span> Load(string planPath, VoiceIndex index, Action<string> log)
    {
        var spans = new Dictionary<string, Span>();
        var cs = new SqliteConnectionStringBuilder { DataSource = planPath, Mode = SqliteOpenMode.ReadOnly }.ToString();
        using var conn = new SqliteConnection(cs);
        conn.Open();

        var conversations = new Dictionary<int, (string Platform, Span Span)>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select Id, Platform, ConversationDate, SourceUpdatedAt from Conversations";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (LineageReader.ParseDate(r.IsDBNull(2) ? null : r.GetString(2)) is not DateOnly from) continue;
                var to = LineageReader.ParseDate(r.IsDBNull(3) ? null : r.GetString(3));
                conversations[r.GetInt32(0)] = (r.IsDBNull(1) ? "" : r.GetString(1), new Span(from, to is DateOnly t && t >= from ? t : null));
            }
        }

        int user = 0, assistant = 0, undated = 0;
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select b.Id, b.ConversationId, b.Speaker, b.RawContent from ConversationBlocks b " +
                              "join Conversations c on c.Id = b.ConversationId order by c.ConversationDate, c.Id, b.BlockNumber";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (!conversations.TryGetValue(r.GetInt32(1), out var conv)) { undated++; continue; }
                bool isUser = r.GetString(2).Equals("user", StringComparison.OrdinalIgnoreCase);
                var id = $"block:{r.GetInt32(0)}";
                var layer = "conv-" + (conv.Platform.Length > 0 ? conv.Platform.ToLowerInvariant() : "unknown");
                index.Add(new VoiceSource(id, layer, isUser ? "brian" : "model", conv.Span.From), r.IsDBNull(3) ? "" : r.GetString(3));
                spans[id] = conv.Span;
                if (isUser) user++; else assistant++;
            }
        }
        log($"  conversations: {conversations.Count}; blocks user {user}, assistant {assistant}{(undated > 0 ? $", {undated} skipped (conversation undated)" : "")}; " +
            $"{conversations.Values.Count(c => c.Span.To is null)} conversations with no recorded end");
        return spans;
    }
}
