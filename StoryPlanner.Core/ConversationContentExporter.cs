using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

/// <summary>
/// Writes the user-selected rows from a conversation scan out as NNN_{slug}_content.json files —
/// the input Cowork analyzes to produce a matching _meta.json. Ported from
/// tools/ConversationSplitter/ContentFileWriter.cs + IndexWriter.cs, extended to embed sourceUuid/
/// sourceUpdatedAt (so import can match by identity) and to reuse a reopened conversation's
/// existing prefix instead of renumbering it.
/// </summary>
public static class ConversationContentExporter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented          = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    public class ExportedFile
    {
        public string ContentFileName { get; set; } = string.Empty;
        public string Prefix          { get; set; } = string.Empty;
        public string Title           { get; set; } = string.Empty;
        public string Date            { get; set; } = string.Empty;
        public int    BlockCount      { get; set; }
        public string FirstPrompt     { get; set; } = string.Empty;
    }

    /// <summary>
    /// Writes one content.json per selected item. Reopened conversations (ExistingSourceFilePrefix
    /// set) reuse that prefix; New conversations get the next NNN above the highest prefix already
    /// used by any DB conversation, so numbering never collides with earlier export cycles.
    /// </summary>
    public static List<ExportedFile> Export(
        IReadOnlyList<ConversationSyncItem> selectedItems,
        string outputFolder,
        IEnumerable<Conversation> allDbConversations)
    {
        Directory.CreateDirectory(outputFolder);

        int nextIndex = allDbConversations
            .Select(c => LeadingIndex(c.SourceFilePrefix))
            .DefaultIfEmpty(0)
            .Max() + 1;

        var written = new List<ExportedFile>();

        foreach (var item in selectedItems)
        {
            string prefix = string.IsNullOrEmpty(item.ExistingSourceFilePrefix)
                ? $"{nextIndex++:D3}_{Slugify(item.Export.Title)}"
                : item.ExistingSourceFilePrefix;

            string fileName = $"{prefix}_content.json";
            string path     = Path.Combine(outputFolder, fileName);

            var payload = new
            {
                platform          = "Claude",
                title             = item.Export.Title,
                conversationDate  = item.Export.ConversationDate,
                sourceUuid        = item.Export.Uuid,
                sourceUpdatedAt   = item.Export.UpdatedAt,
                systemInstruction = string.Empty,
                blocks            = item.Export.Blocks.Select(b => new
                {
                    blockNumber  = b.BlockNumber,
                    speaker      = b.Speaker,
                    rawContent   = b.RawContent,
                    isCompaction = b.IsCompaction
                })
            };

            File.WriteAllText(path, JsonSerializer.Serialize(payload, JsonOptions));

            string firstPrompt = item.Export.Blocks
                .FirstOrDefault(b => b.Speaker == "user" && !string.IsNullOrWhiteSpace(b.RawContent))
                ?.RawContent ?? string.Empty;
            if (firstPrompt.Length > 200) firstPrompt = firstPrompt[..200];

            written.Add(new ExportedFile
            {
                ContentFileName = fileName,
                Prefix          = prefix,
                Title           = item.Export.Title,
                Date            = item.Export.ConversationDate,
                BlockCount      = item.Export.Blocks.Count,
                FirstPrompt     = firstPrompt
            });
        }

        WriteIndex(written, outputFolder);
        return written;
    }

    private static int LeadingIndex(string prefix)
    {
        if (string.IsNullOrEmpty(prefix)) return 0;
        var digits = new string(prefix.TakeWhile(char.IsDigit).ToArray());
        return int.TryParse(digits, out var n) ? n : 0;
    }

    private static void WriteIndex(List<ExportedFile> files, string outputFolder)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Conversation Export Batch");
        sb.AppendLine();
        sb.AppendLine($"Generated {DateTime.Now:yyyy-MM-dd HH:mm}  |  {files.Count} conversations");
        sb.AppendLine();
        sb.AppendLine("| Prefix | Date | Blocks | Title | First Prompt (preview) |");
        sb.AppendLine("|--------|------|--------|-------|------------------------|");

        foreach (var f in files.OrderBy(f => f.Prefix, StringComparer.OrdinalIgnoreCase))
        {
            string preview = f.FirstPrompt.Length > 80 ? f.FirstPrompt[..80].TrimEnd() + "…" : f.FirstPrompt;
            preview = preview.Replace("|", "\\|").Replace("\n", " ").Replace("\r", "");
            string title = f.Title.Replace("|", "\\|");
            sb.AppendLine($"| {f.Prefix} | {FormatDate(f.Date)} | {f.BlockCount} | {title} | {preview} |");
        }

        File.WriteAllText(Path.Combine(outputFolder, "index.md"), sb.ToString(), Encoding.UTF8);

        var jsonPayload = files.Select(f => new
        {
            contentFile = f.ContentFileName,
            prefix      = f.Prefix,
            title       = f.Title,
            date        = f.Date,
            blockCount  = f.BlockCount,
            firstPrompt = f.FirstPrompt
        });
        File.WriteAllText(Path.Combine(outputFolder, "index.json"), JsonSerializer.Serialize(jsonPayload, JsonOptions), Encoding.UTF8);
    }

    private static string FormatDate(string iso) =>
        DateTime.TryParse(iso, out var dt) ? dt.ToString("yyyy-MM-dd") : iso;

    private static string Slugify(string title)
    {
        string lower   = title.ToLowerInvariant();
        string ascii   = Regex.Replace(lower, @"[^a-z0-9\s-]", " ");
        string hyphens = Regex.Replace(ascii.Trim(), @"\s+", "-");
        string clean   = Regex.Replace(hyphens, @"-+", "-").Trim('-');
        return clean.Length > 60 ? clean[..60].TrimEnd('-') : clean;
    }
}
