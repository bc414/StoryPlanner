namespace StoryPlanner.Core;

public class ConversationBlock
{
    public int        Id             { get; set; }
    public int        ConversationId { get; set; }
    public int        BlockNumber    { get; set; }
    public string     Speaker        { get; set; } = string.Empty; // "user" | "assistant"
    public string     RawContent     { get; set; } = string.Empty;
    // The author's own navigation note, typed into the Conversation Reader's middle column
    // (2026-08-11). It used to be an AI summary imported from a *_meta.json; that pipeline is
    // retired, the imported text was wiped by the wipe-block-summaries DataOps op, and no import
    // path writes this field any more — it is Brian's, in his words.
    // Empty is an ordinary, permanent state: most blocks never get a note, and the reader shows
    // those by number and speaker alone. Never substitute an excerpt of RawContent for one.
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
