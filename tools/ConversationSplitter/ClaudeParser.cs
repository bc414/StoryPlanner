using System.Text;
using System.Text.Json;

namespace ConversationSplitter;

/// <summary>
/// Parses the Claude conversations.json export (a top-level JSON array of conversations).
/// Each conversation has chat_messages[], each message has content[] items typed by "type".
/// Thinking/tool_use/tool_result items are stripped; file/attachment items become placeholder lines.
/// </summary>
public static class ClaudeParser
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

    public static List<ConversationData> Parse(string filePath)
    {
        Console.WriteLine($"Reading {Path.GetFileName(filePath)} ({new FileInfo(filePath).Length / 1_048_576} MB)…");

        using var stream = File.OpenRead(filePath);
        using var doc    = JsonDocument.Parse(stream, new JsonDocumentOptions { AllowTrailingCommas = true });

        if (doc.RootElement.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException("conversations.json root must be a JSON array.");

        var results = new List<ConversationData>();

        foreach (var convElem in doc.RootElement.EnumerateArray())
        {
            var conv = ParseConversation(convElem);
            if (conv is not null)
                results.Add(conv);
        }

        Console.WriteLine($"  Claude: {results.Count} conversations survive the ≤2-block filter.");
        return results;
    }

    private static ConversationData? ParseConversation(JsonElement elem)
    {
        string title = elem.GetStringOrEmpty("name");
        string date  = elem.GetStringOrEmpty("created_at");

        if (!elem.TryGetProperty("chat_messages", out var messages) ||
            messages.ValueKind != JsonValueKind.Array)
            return null;

        var blocks = new List<BlockData>();
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

            // Attachments/files → placeholder lines before the message text
            AppendAttachmentPlaceholders(msg, sb);

            // content[] — only type=="text" parts contribute to RawContent
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
            blocks.Add(new BlockData
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

        return new ConversationData
        {
            Platform         = "Claude",
            Title            = string.IsNullOrWhiteSpace(title) ? "(untitled)" : title,
            ConversationDate = date,
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

internal static class JsonElementExtensions
{
    public static string GetStringOrEmpty(this JsonElement elem, string propertyName)
    {
        if (elem.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String)
            return prop.GetString() ?? string.Empty;
        return string.Empty;
    }
}
