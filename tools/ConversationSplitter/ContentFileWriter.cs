using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ConversationSplitter;

public static class ContentFileWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented       = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    public static string Write(ConversationData conv, string outputDir)
    {
        string slug     = Slugify(conv.Title);
        string fileName = $"{conv.SourceIndex:D3}_{slug}_content.json";
        string path     = Path.Combine(outputDir, fileName);

        // Write as a plain anonymous shape matching the spec schema
        var payload = new
        {
            platform         = conv.Platform,
            sourceIndex      = conv.SourceIndex,
            title            = conv.Title,
            conversationDate = conv.ConversationDate,
            systemInstruction = conv.SystemInstruction,
            blocks           = conv.Blocks.Select(b => new
            {
                blockNumber  = b.BlockNumber,
                speaker      = b.Speaker,
                rawContent   = b.RawContent,
                isCompaction = b.IsCompaction
            })
        };

        File.WriteAllText(path, JsonSerializer.Serialize(payload, JsonOptions));
        return fileName;
    }

    public static string Slugify(string title)
    {
        string lower = title.ToLowerInvariant();
        string ascii = Regex.Replace(lower, @"[^a-z0-9\s-]", " ");
        string hyphens = Regex.Replace(ascii.Trim(), @"\s+", "-");
        string clean = Regex.Replace(hyphens, @"-+", "-").Trim('-');
        return clean.Length > 60 ? clean[..60].TrimEnd('-') : clean;
    }
}
