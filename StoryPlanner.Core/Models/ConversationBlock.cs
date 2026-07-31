namespace StoryPlanner.Core;

public class ConversationBlock
{
    public int        Id             { get; set; }
    public int        ConversationId { get; set; }
    public int        BlockNumber    { get; set; }
    public string     Speaker        { get; set; } = string.Empty; // "user" | "assistant"
    public string     RawContent     { get; set; } = string.Empty;
    // Optional navigation aid, authored outside the app and imported from a *_meta.json.
    // Empty is an ordinary state: a conversation imported straight from a raw export has no
    // summaries at all, and the reader shows such blocks by number and speaker alone.
    public string     Summary        { get; set; } = string.Empty;
    public bool       IsCompaction   { get; set; }
    public BlockState BlockState     { get; set; }
}

public enum BlockState
{
    Unread,
    Skipped,
    Flagged,
    Done
}
