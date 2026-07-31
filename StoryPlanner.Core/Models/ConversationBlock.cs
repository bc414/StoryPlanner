namespace StoryPlanner.Core;

public class ConversationBlock
{
    public int        Id             { get; set; }
    public int        ConversationId { get; set; }
    public int        BlockNumber    { get; set; }
    public string     Speaker        { get; set; } = string.Empty; // "user" | "assistant"
    public string     RawContent     { get; set; } = string.Empty;
    public string     Summary        { get; set; } = string.Empty;
    public bool       HasDecisions   { get; set; }
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
