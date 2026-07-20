namespace ConversationSplitter;

// Internal DTOs — used only while the tool runs to build *_content.json output.
// These are NOT EF entities and have no relation to StoryPlanner.Core models.

public class ConversationData
{
    public string Platform         { get; set; } = string.Empty;  // "Claude" | "Gemini"
    public int    SourceIndex      { get; set; }                   // assigned after sorting
    public string Title            { get; set; } = string.Empty;
    public string ConversationDate { get; set; } = string.Empty;  // ISO 8601
    public string SystemInstruction { get; set; } = string.Empty;

    public List<BlockData> Blocks  { get; set; } = new();
}

public class BlockData
{
    public int    BlockNumber  { get; set; }
    public string Speaker      { get; set; } = string.Empty; // "user" | "assistant"
    public string RawContent   { get; set; } = string.Empty;
    public bool   IsCompaction { get; set; }
}

public class IndexEntry
{
    public int    Index       { get; set; }
    public string ContentFile { get; set; } = string.Empty;
    public string Platform    { get; set; } = string.Empty;
    public string Title       { get; set; } = string.Empty;
    public string Date        { get; set; } = string.Empty;
    public int    BlockCount  { get; set; }
    public string FirstPrompt { get; set; } = string.Empty;
}
