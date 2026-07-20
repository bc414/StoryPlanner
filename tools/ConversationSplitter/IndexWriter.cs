using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConversationSplitter;

public static class IndexWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented          = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    public static void Write(List<IndexEntry> entries, string outputDir)
    {
        WriteMarkdown(entries, outputDir);
        WriteJson(entries, outputDir);
    }

    private static void WriteMarkdown(List<IndexEntry> entries, string outputDir)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Conversation Index");
        sb.AppendLine();
        sb.AppendLine($"Generated {DateTime.Now:yyyy-MM-dd HH:mm}  |  {entries.Count} conversations");
        sb.AppendLine();
        sb.AppendLine("| # | Platform | Date | Blocks | Title | First Prompt (preview) |");
        sb.AppendLine("|---|----------|------|--------|-------|------------------------|");

        foreach (var e in entries)
        {
            string preview = e.FirstPrompt.Length > 80
                ? e.FirstPrompt[..80].TrimEnd() + "…"
                : e.FirstPrompt;
            // Escape pipes
            preview = preview.Replace("|", "\\|").Replace("\n", " ").Replace("\r", "");
            string title = e.Title.Replace("|", "\\|");

            sb.AppendLine($"| {e.Index:D3} | {e.Platform} | {FormatDate(e.Date)} | {e.BlockCount} | {title} | {preview} |");
        }

        File.WriteAllText(Path.Combine(outputDir, "index.md"), sb.ToString(), Encoding.UTF8);
        Console.WriteLine("  Wrote index.md");
    }

    private static void WriteJson(List<IndexEntry> entries, string outputDir)
    {
        var payload = entries.Select(e => new
        {
            index       = e.Index,
            contentFile = e.ContentFile,
            platform    = e.Platform,
            title       = e.Title,
            date        = e.Date,
            blockCount  = e.BlockCount,
            firstPrompt = e.FirstPrompt
        });

        File.WriteAllText(
            Path.Combine(outputDir, "index.json"),
            JsonSerializer.Serialize(payload, JsonOptions),
            Encoding.UTF8);
        Console.WriteLine("  Wrote index.json");
    }

    private static string FormatDate(string iso)
    {
        if (DateTime.TryParse(iso, out var dt))
            return dt.ToString("yyyy-MM-dd");
        return iso;
    }
}
