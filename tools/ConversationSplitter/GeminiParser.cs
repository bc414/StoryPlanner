using System.Text.Json;

namespace ConversationSplitter;

/// <summary>
/// Parses Gemini AI Studio export files (one conversation per file).
/// Format: a single JSON object with runSettings, systemInstruction, and chunkedPrompt.chunks[].
/// Chunks with isThought:true are stripped; driveDocument chunks become placeholder blocks.
/// </summary>
public static class GeminiParser
{
    public static List<ConversationData> ParseFolder(string folderPath)
    {
        var files = Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly);
        Console.WriteLine($"Found {files.Length} Gemini AI Studio files in {Path.GetFileName(folderPath)}.");

        var results = new List<ConversationData>();
        foreach (var file in files)
        {
            var conv = ParseFile(file);
            if (conv is not null)
                results.Add(conv);
        }

        Console.WriteLine($"  Gemini: {results.Count} conversations survive the ≤2-block filter.");
        return results;
    }

    private static ConversationData? ParseFile(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var doc    = JsonDocument.Parse(stream, new JsonDocumentOptions { AllowTrailingCommas = true });

        var root = doc.RootElement;
        if (root.ValueKind != JsonValueKind.Object)
            return null;

        // Title from filename (strip extension, replace _ with space, trim trailing _/space)
        string rawName = Path.GetFileNameWithoutExtension(filePath)
                             .Replace('_', ' ')
                             .TrimEnd(' ', '_', '.');
        string title = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rawName.ToLower());

        string systemInstruction = string.Empty;
        if (root.TryGetProperty("systemInstruction", out var sysInstr) &&
            sysInstr.TryGetProperty("text", out var sysText))
            systemInstruction = sysText.GetString() ?? string.Empty;

        if (!root.TryGetProperty("chunkedPrompt", out var chunkedPrompt) ||
            !chunkedPrompt.TryGetProperty("chunks", out var chunks) ||
            chunks.ValueKind != JsonValueKind.Array)
            return null;

        string conversationDate = string.Empty;
        var blocks = new List<BlockData>();
        int blockNum = 0;

        foreach (var chunk in chunks.EnumerateArray())
        {
            // Strip thinking chunks
            if (chunk.TryGetProperty("isThought", out var isThought) &&
                isThought.ValueKind == JsonValueKind.True)
                continue;

            // driveDocument chunks → placeholder block (count as a block for context)
            if (chunk.TryGetProperty("driveDocument", out var driveDoc))
            {
                string docId = string.Empty;
                if (driveDoc.TryGetProperty("id", out var idProp))
                    docId = idProp.GetString() ?? string.Empty;

                string chunkDate = chunk.GetStringOrEmpty("createTime");
                if (conversationDate == string.Empty && !string.IsNullOrEmpty(chunkDate))
                    conversationDate = chunkDate;

                string role = MapRole(chunk.GetStringOrEmpty("role"));
                blockNum++;
                blocks.Add(new BlockData
                {
                    BlockNumber = blockNum,
                    Speaker     = role,
                    RawContent  = $"[Attached document: {docId}]",
                    IsCompaction = false
                });
                continue;
            }

            string text = chunk.GetStringOrEmpty("text");
            if (string.IsNullOrWhiteSpace(text))
                continue;

            string createTime = chunk.GetStringOrEmpty("createTime");
            if (conversationDate == string.Empty && !string.IsNullOrEmpty(createTime))
                conversationDate = createTime;

            string speaker = MapRole(chunk.GetStringOrEmpty("role"));
            blockNum++;
            blocks.Add(new BlockData
            {
                BlockNumber  = blockNum,
                Speaker      = speaker,
                RawContent   = text.Trim(),
                IsCompaction = false
            });
        }

        if (blocks.Count <= 2)
            return null;

        return new ConversationData
        {
            Platform          = "Gemini",
            Title             = title,
            ConversationDate  = conversationDate,
            SystemInstruction = systemInstruction,
            Blocks            = blocks
        };
    }

    private static string MapRole(string role) => role switch
    {
        "user"  => "user",
        "model" => "assistant",
        _       => role
    };
}
