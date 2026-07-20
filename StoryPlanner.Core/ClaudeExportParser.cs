using System.Text;
using System.Text.Json;

namespace StoryPlanner.Core;

/// <summary>
/// Parses a Claude conversations.json export (a top-level JSON array of conversations) into
/// lightweight in-memory DTOs, for the in-app "scan a fresh export" workflow.
/// Ported from tools/ConversationSplitter/ClaudeParser.cs, extended to also capture each
/// conversation's uuid/created_at/updated_at — the identity needed to recognize a reopened
/// conversation across export cycles. The original CLI tool is left untouched; this is a
/// deliberate, self-contained copy so Core has no dependency on the tool project.
/// </summary>
public static class ClaudeExportParser
{
    // Best-effort markers that suggest a context-compaction summary block
    private static readonly string[] CompactionMarkers =
    [
        "<context_window_compaction>",
        "This is a summary of our conversation",
        "The following is a condensed summary",
        "[Conversation summary]",
        "[Context summary]"
    ];

    public static List<ParsedClaudeConversation> Parse(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var doc    = JsonDocument.Parse(stream, new JsonDocumentOptions { AllowTrailingCommas = true });

        if (doc.RootElement.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException("conversations.json root must be a JSON array.");

        var results = new List<ParsedClaudeConversation>();

        foreach (var convElem in doc.RootElement.EnumerateArray())
        {
            var conv = ParseConversation(convElem);
            if (conv is not null)
                results.Add(conv);
        }

        return results;
    }

    private static ParsedClaudeConversation? ParseConversation(JsonElement elem)
    {
        string uuid  = elem.GetStringOrEmpty("uuid");
        string title = elem.GetStringOrEmpty("name");
        string createdAt = elem.GetStringOrEmpty("created_at");
        string updatedAt = elem.GetStringOrEmpty("updated_at");

        if (!elem.TryGetProperty("chat_messages", out var messages) ||
            messages.ValueKind != JsonValueKind.Array)
            return null;

        var blocks = new List<ParsedClaudeBlock>();
        int blockNum = 0;

        foreach (var msg in messages.EnumerateArray())
        {
            string sender = msg.GetStringOrEmpty("sender") switch
            {
                "human"     => "user",
                "assistant" => "assistant",
                var s       => s
            };

            var sb = new StringBuilder();

            AppendAttachmentPlaceholders(msg, sb);

            if (msg.TryGetProperty("content", out var contentArr) &&
                contentArr.ValueKind == JsonValueKind.Array)
            {
                foreach (var part in contentArr.EnumerateArray())
                {
                    string type = part.GetStringOrEmpty("type");
                    if (type == "text" && part.TryGetProperty("text", out var textProp))
                    {
                        string text = textProp.GetString() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            if (sb.Length > 0) sb.AppendLine();
                            sb.Append(text);
                        }
                    }
                    // thinking / tool_use / tool_result → skip
                }
            }

            string rawContent = sb.ToString().Trim();
            if (string.IsNullOrEmpty(rawContent))
                continue;

            blockNum++;
            blocks.Add(new ParsedClaudeBlock
            {
                BlockNumber  = blockNum,
                Speaker      = sender,
                RawContent   = rawContent,
                IsCompaction = IsCompactionBlock(rawContent, sender)
            });
        }

        // Drop low-signal conversations
        if (blocks.Count <= 2)
            return null;

        return new ParsedClaudeConversation
        {
            Uuid             = uuid,
            Title            = string.IsNullOrWhiteSpace(title) ? "(untitled)" : title,
            ConversationDate = createdAt,
            UpdatedAt        = updatedAt,
            Blocks           = blocks
        };
    }

    private static void AppendAttachmentPlaceholders(JsonElement msg, StringBuilder sb)
    {
        if (msg.TryGetProperty("attachments", out var attachments) &&
            attachments.ValueKind == JsonValueKind.Array)
        {
            foreach (var att in attachments.EnumerateArray())
            {
                string name = att.GetStringOrEmpty("file_name");
                if (string.IsNullOrWhiteSpace(name)) name = att.GetStringOrEmpty("id");
                if (!string.IsNullOrWhiteSpace(name))
                    sb.AppendLine($"[Attached file: {name}]");
            }
        }

        if (msg.TryGetProperty("files", out var files) &&
            files.ValueKind == JsonValueKind.Array)
        {
            foreach (var f in files.EnumerateArray())
            {
                string name = f.GetStringOrEmpty("file_name");
                if (string.IsNullOrWhiteSpace(name)) name = f.GetStringOrEmpty("id");
                if (!string.IsNullOrWhiteSpace(name))
                    sb.AppendLine($"[Attached file: {name}]");
            }
        }
    }

    private static bool IsCompactionBlock(string rawContent, string speaker)
    {
        if (speaker != "assistant") return false;
        foreach (var marker in CompactionMarkers)
            if (rawContent.Contains(marker, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }
}

internal static class ClaudeJsonElementExtensions
{
    public static string GetStringOrEmpty(this JsonElement elem, string propertyName)
    {
        if (elem.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String)
            return prop.GetString() ?? string.Empty;
        return string.Empty;
    }
}

/// <summary>A conversation parsed from a Claude conversations.json export, prior to any DB comparison.</summary>
public class ParsedClaudeConversation
{
    public string Uuid             { get; set; } = string.Empty;
    public string Title            { get; set; } = string.Empty;
    public string ConversationDate { get; set; } = string.Empty; // ISO 8601 created_at
    public string UpdatedAt        { get; set; } = string.Empty; // ISO 8601 updated_at
    public List<ParsedClaudeBlock> Blocks { get; set; } = new();
}

public class ParsedClaudeBlock
{
    public int    BlockNumber  { get; set; }
    public string Speaker      { get; set; } = string.Empty; // "user" | "assistant"
    public string RawContent   { get; set; } = string.Empty;
    public bool   IsCompaction { get; set; }
}
