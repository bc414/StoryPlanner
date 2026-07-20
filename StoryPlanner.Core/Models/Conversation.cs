namespace StoryPlanner.Core.Models;

public class Conversation
{
    public int      Id               { get; set; }
    public string   Title            { get; set; } = string.Empty;
    public DateTime ConversationDate { get; set; }
    public string   Platform         { get; set; } = string.Empty; // "Claude" | "Gemini"
    public int      BlockCount       { get; set; }
    public string   ArcSummary       { get; set; } = string.Empty;

    // The NNN_{slug} prefix shared by the source _content.json/_meta.json pair.
    // Stable even if Title is edited later; used to skip re-importing the same conversation.
    public string   SourceFilePrefix { get; set; } = string.Empty;

    // The Claude conversation's own uuid (from conversations.json). This is the durable identity
    // used to recognize a reopened/extended conversation across export cycles. Empty for records
    // imported before this field existed, until backfilled via a confirmed scan match.
    public string    SourceUuid      { get; set; } = string.Empty;

    // The Claude export's updated_at for this conversation, used to detect whether a later export
    // reflects new activity even when the block count hasn't changed yet.
    public DateTime? SourceUpdatedAt { get; set; }
}
