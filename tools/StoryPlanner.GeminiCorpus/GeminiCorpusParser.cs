using System.Text.Json;
using System.Text.RegularExpressions;

namespace StoryPlanner.GeminiCorpus;

public sealed record GeminiEntry(
    string EntryId,
    string ThreadId,
    int ThreadPos,
    int ThreadSize,
    string Date,
    string LocalTime,
    string Subject,
    string? Subtopic,
    string TopicLabel,
    string ThreadSummary,
    string Intent,
    string? Gem,
    string Title,
    string Prompt,
    string Response,
    string Type,
    bool IsPlanPaste,
    int PromptChars,
    int ResponseChars);

public sealed record GeminiReport(
    string Slug,
    string Title,
    string Kind,
    string Body);

public static partial class GeminiCorpusParser
{
    private const int PlanPasteWordThreshold = 20_000;

    [GeneratedRegex(@"^\d{4}-W\d{2}$")]
    private static partial Regex WeeklySlugPattern();

    public static (List<GeminiEntry> Entries, int Stubbed) ParseEntries(string corpusDir)
    {
        var indexPath = Path.Combine(corpusDir, "corpus_index.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(indexPath));
        var rows = doc.RootElement.GetProperty("entries");

        var entries = new List<GeminiEntry>();
        int stubbed = 0;

        foreach (var row in rows.EnumerateArray())
        {
            if (!IsStoryTagged(row)) continue;

            var entryId = row.GetProperty("id").GetString()!;
            var filePath = Path.Combine(corpusDir, row.GetProperty("file").GetString()!);
            var type = row.GetProperty("type").GetString()!;
            var promptWords = row.TryGetProperty("prompt_words", out var pw) ? pw.GetInt32() : 0;
            var promptChars = row.TryGetProperty("prompt_chars", out var pc) ? pc.GetInt32() : 0;
            var responseChars = row.TryGetProperty("response_chars", out var rc) ? rc.GetInt32() : 0;

            var (prompt, response) = ExtractBodies(filePath, type);
            var isPlanPaste = promptWords > PlanPasteWordThreshold;
            if (isPlanPaste)
            {
                prompt = $"[Plan export attached — {promptWords:N0} words, {promptChars:N0} chars]";
                stubbed++;
            }

            entries.Add(new GeminiEntry(
                EntryId: entryId,
                ThreadId: row.GetProperty("thread_id").GetString()!,
                ThreadPos: row.GetProperty("thread_pos").GetInt32(),
                ThreadSize: row.GetProperty("thread_size").GetInt32(),
                Date: row.GetProperty("date").GetString()!,
                LocalTime: row.TryGetProperty("local_time", out var lt) ? lt.GetString()! : row.GetProperty("date").GetString()!,
                Subject: row.GetProperty("subject").GetString()!,
                Subtopic: row.TryGetProperty("subtopic", out var st) && st.ValueKind != JsonValueKind.Null ? st.GetString() : null,
                TopicLabel: row.TryGetProperty("topic_label", out var tl) ? tl.GetString() ?? "" : "",
                ThreadSummary: row.TryGetProperty("thread_summary", out var ts) ? ts.GetString() ?? "" : "",
                Intent: row.TryGetProperty("intent", out var it) ? it.GetString() ?? "" : "",
                Gem: row.TryGetProperty("gem", out var gm) && gm.ValueKind != JsonValueKind.Null ? gm.GetString() : null,
                Title: row.TryGetProperty("title", out var ti) ? ti.GetString() ?? "" : "",
                Prompt: prompt,
                Response: response,
                Type: type,
                IsPlanPaste: isPlanPaste,
                PromptChars: promptChars,
                ResponseChars: responseChars));
        }

        return (entries, stubbed);
    }

    public static List<GeminiReport> ParseReports(string reportDir)
    {
        var reports = new List<GeminiReport>();
        foreach (var file in Directory.GetFiles(reportDir, "*.md").OrderBy(f => Path.GetFileNameWithoutExtension(f)))
        {
            var slug = Path.GetFileNameWithoutExtension(file);
            var body = File.ReadAllText(file);
            var title = ExtractFirstHeading(body) ?? slug;
            var kind = WeeklySlugPattern().IsMatch(slug) ? "weekly" : "appendix";
            reports.Add(new GeminiReport(slug, title, kind, body));
        }
        return reports;
    }

    private static bool IsStoryTagged(JsonElement row)
    {
        var subject = row.GetProperty("subject").GetString();
        if (subject == "creative-writing") return true;
        if (row.TryGetProperty("secondary_subjects", out var sec) && sec.ValueKind == JsonValueKind.Array)
        {
            foreach (var s in sec.EnumerateArray())
                if (s.GetString() == "creative-writing") return true;
        }
        return false;
    }

    private static (string Prompt, string Response) ExtractBodies(string filePath, string type)
    {
        if (!File.Exists(filePath))
            return ("", "");

        var text = File.ReadAllText(filePath);

        // Strip YAML front matter (--- ... ---)
        var bodyStart = 0;
        if (text.StartsWith("---"))
        {
            var endFm = text.IndexOf("\n---", 3, StringComparison.Ordinal);
            if (endFm >= 0) bodyStart = endFm + 4;
        }
        var body = text[bodyStart..];

        if (type == "activity")
            return ("", body.Trim());

        var promptIdx = body.IndexOf("\n## Prompt\n", StringComparison.Ordinal);
        var responseIdx = body.IndexOf("\n## Response\n", StringComparison.Ordinal);

        if (promptIdx < 0 && responseIdx < 0)
        {
            // Fallback: try without trailing newline (end of file)
            promptIdx = body.IndexOf("\n## Prompt", StringComparison.Ordinal);
            responseIdx = body.IndexOf("\n## Response", StringComparison.Ordinal);
        }

        string prompt, response;

        if (promptIdx >= 0 && responseIdx >= 0 && responseIdx > promptIdx)
        {
            var promptStart = body.IndexOf('\n', promptIdx + 1) + 1;
            var promptSectionEnd = body.IndexOf('\n', promptStart);
            while (promptSectionEnd >= 0 && promptSectionEnd < responseIdx)
                promptSectionEnd = body.IndexOf('\n', promptSectionEnd + 1);
            prompt = body[(body.IndexOf('\n', promptIdx + 1) + 1)..responseIdx].Trim();
            response = body[(body.IndexOf('\n', responseIdx + 1) + 1)..].Trim();
        }
        else if (promptIdx >= 0)
        {
            prompt = body[(body.IndexOf('\n', promptIdx + 1) + 1)..].Trim();
            response = "";
        }
        else
        {
            prompt = "";
            response = body.Trim();
        }

        return (prompt, response);
    }

    private static string? ExtractFirstHeading(string markdown)
    {
        foreach (var line in markdown.Split('\n'))
        {
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith("# "))
                return trimmed[2..].Trim();
        }
        return null;
    }
}
