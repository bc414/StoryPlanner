using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using ModelContextProtocol.Server;

namespace StoryPlanner.Mcp;

/// <summary>
/// Retrieval over the Gemini-era corpus — the founding-era web-app conversations and the curated
/// weekly story-development reports built from them. A fifth corpus, joined to nothing.
///
/// The workflow is REPORTS FIRST: the curated digests answer "when was X decided?" directly.
/// The raw conversation entries are the detail pass, drilled into from a report hit when the
/// verbatim exchange matters.
///
/// Same standing rules as every other tool family: these report what was said. They never rank
/// threads by importance, propose what to take from a conversation, or suggest which era's
/// version of a decision is correct.
/// </summary>
[McpServerToolType]
public sealed class GeminiCorpusTools(GeminiCorpusStore store)
{
    private const int DefaultWindow = 40_000;
    private const int MaxWindow = 50_000;

    [McpServerTool(Name = "search_gemini")]
    [Description(
        "Regex search across the Gemini founding-era corpus (Sep 2025 – Jun 2026). Reports are " +
        "searched and listed FIRST — they are curated digests that cite the week and thread. " +
        "Drill into raw entries via get_gemini_entry only when you need the verbatim exchange. " +
        "Start with report hits for provenance questions (\"when was X decided?\"). " +
        "Most Gemini-era decisions were superseded by later work, so this corpus is provenance, " +
        "not ground truth — a quotation carries lower confidence than one from the Claude or " +
        "AI Studio conversations.")]
    public string SearchGemini(
        [Description("Regex pattern (.NET syntax). Case-insensitive unless caseSensitive=true.")]
        string pattern,
        [Description("Scope: \"all\" (default) searches reports then entries; \"reports\" searches only reports; \"entries\" searches only entries.")]
        string scope = "all",
        [Description("Restrict entries to a subtopic: worldbuilding-lore, characters-arcs, politics-war-military, story-craft-structure, themes-philosophy, general, romance-relationships.")]
        string? subtopic = null,
        [Description("Match case-sensitively (default false).")]
        bool caseSensitive = false,
        [Description("Match whole words only (default false).")]
        bool wholeWord = false,
        [Description("Characters of context around each match snippet (20-2000, default 200).")]
        int contextChars = 200,
        [Description("Maximum hits returned (1-250, default 40). The total is always reported even when hits are dropped.")]
        int limit = 40)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        Regex rx;
        try { rx = Query.BuildRegex(pattern, caseSensitive, wholeWord); }
        catch (ArgumentException ex) { return $"Invalid regex: {ex.Message}"; }

        contextChars = Math.Clamp(contextChars, 20, 2000);
        limit = Math.Clamp(limit, 1, 250);

        var searchReports = scope is "all" or "reports";
        var searchEntries = scope is "all" or "entries";
        if (!searchReports && !searchEntries)
            return $"Unknown scope \"{scope}\" — pass \"all\" (default), \"reports\", or \"entries\".";

        var sb = new StringBuilder();
        int totalReportHits = 0, totalEntryHits = 0;
        var reportLines = new List<string>();
        var entryLines = new List<string>();

        try
        {
            if (searchReports)
            {
                foreach (var (report, body) in store.StreamReports())
                {
                    var m = rx.Match(body);
                    if (!m.Success) continue;
                    totalReportHits++;
                    if (reportLines.Count < limit)
                        reportLines.Add(
                            $"report:{report.Id} [{report.Slug}] \"{Query.Truncate(report.Title, 60)}\" — " +
                            $"{Query.Snippet(body, m, contextChars)}");
                }
            }

            if (searchEntries)
            {
                foreach (var (entry, prompt, response) in store.StreamEntries(subtopic))
                {
                    var mPrompt = entry.IsPlanPaste ? Match.Empty : rx.Match(prompt);
                    var mResponse = rx.Match(response);
                    var mTopic = rx.Match(entry.TopicLabel);
                    var mSummary = rx.Match(entry.ThreadSummary);

                    if (!mPrompt.Success && !mResponse.Success && !mTopic.Success && !mSummary.Success)
                        continue;
                    totalEntryHits++;
                    if (entryLines.Count >= limit) continue;

                    var label = EntryLabel(entry);
                    string where;
                    if (mPrompt.Success)
                        where = $"prompt: \"{Query.Snippet(prompt, mPrompt, contextChars)}\"";
                    else if (mResponse.Success)
                        where = $"response: \"{Query.Snippet(response, mResponse, contextChars)}\"";
                    else if (mTopic.Success)
                        where = $"topic: \"{entry.TopicLabel}\"";
                    else
                        where = $"summary: \"{entry.ThreadSummary}\"";

                    if (entry.IsPlanPaste) where += " [plan paste]";
                    entryLines.Add($"{label} — {where}");
                }
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return "Regex timed out (2s) — simplify the pattern.";
        }

        var total = totalReportHits + totalEntryHits;
        sb.AppendLine($"# search_gemini /{pattern}/ — {total} match(es)");

        if (searchReports)
        {
            sb.AppendLine();
            sb.AppendLine($"## Reports ({totalReportHits} hit(s){(totalReportHits > reportLines.Count ? $", showing first {reportLines.Count}" : "")})");
            if (reportLines.Count == 0) sb.AppendLine("(no matches in reports)");
            foreach (var l in reportLines) sb.AppendLine(l);
        }

        if (searchEntries)
        {
            sb.AppendLine();
            sb.AppendLine($"## Entries ({totalEntryHits} hit(s){(totalEntryHits > entryLines.Count ? $", showing first {entryLines.Count}" : "")})");
            if (entryLines.Count == 0)
                sb.AppendLine("(no matches in entries" +
                              (subtopic is not null ? $" [subtopic={subtopic}]" : "") + ")");
            foreach (var l in entryLines) sb.AppendLine(l);
        }

        return Query.Cap(sb);
    }

    [McpServerTool(Name = "get_gemini_report")]
    [Description(
        "Fetch a Gemini-era report by id or slug — the PRIMARY read tool. Reports are curated " +
        "weekly digests (150KB–840KB) that walk every thread turn by turn, recording the author's " +
        "prompts, model contributions, and every overrule. Plus 5 appendices (canon, timeline, " +
        "characters, method, and the planner engineering track). Windowed: pass offset to continue " +
        "reading a long report.")]
    public string GetGeminiReport(
        [Description("Report id (from search_gemini).")]
        int? id = null,
        [Description("Report slug (\"2025-W49\", \"APPENDIX-A-canon\", \"INDEX\"). Case-insensitive.")]
        string? slug = null,
        [Description("Start character offset into the report body. Default 0.")]
        int offset = 0,
        [Description("Characters to return (500-50000, default 40000).")]
        int length = DefaultWindow)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        offset = Math.Max(0, offset);
        length = Math.Clamp(length, 500, MaxWindow);

        (GeminiCorpusStore.ReportManifest Report, string Body)? hit;
        if (id is not null)
            hit = store.FetchReport(id.Value);
        else if (slug is not null)
            hit = store.FetchReportBySlug(slug);
        else
            return "Pass either id or slug. Available slugs: " +
                   string.Join(", ", store.Reports().Select(r => r.Slug));

        if (hit is null)
            return id is not null
                ? $"No report with id {id}."
                : $"No report with slug \"{slug}\". Available: " +
                  string.Join(", ", store.Reports().Select(r => r.Slug));

        var (report, body) = hit.Value;
        var sb = new StringBuilder();
        sb.AppendLine($"## report:{report.Id} — [{report.Slug}] \"{report.Title}\"");
        sb.AppendLine($"{report.BodyChars:N0} chars total | kind:{report.Kind}");
        sb.AppendLine();

        var window = body.Substring(
            Math.Min(offset, body.Length),
            Math.Min(length, Math.Max(0, body.Length - offset)));
        sb.Append(window);

        var end = offset + window.Length;
        if (end < report.BodyChars)
            sb.AppendLine($"\n\n[WINDOWED — {report.BodyChars - end:N0} more chars. " +
                          $"Continue: get_gemini_report(id: {report.Id}, offset: {end})]");

        return Query.Cap(sb);
    }

    [McpServerTool(Name = "get_gemini_entry")]
    [Description(
        "Fetch full text of Gemini-era conversation entries by id — the DETAIL PASS after finding " +
        "the relevant week/thread in a report. Shows metadata header then prompt and response. " +
        "Plan-paste entries (the full story plan pasted as a prompt) show a stub for the prompt " +
        "and the full response.")]
    public string GetGeminiEntry(
        [Description("Entry ids (from search_gemini).")]
        int[] ids,
        [Description("Start character offset into each entry's text. Default 0.")]
        int offset = 0,
        [Description("Characters to return per entry (500-50000, default 40000).")]
        int length = DefaultWindow)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        offset = Math.Max(0, offset);
        length = Math.Clamp(length, 500, MaxWindow);

        var sb = new StringBuilder();
        int found = 0, missing = 0;
        var body = new StringBuilder();

        foreach (var id in ids.Distinct())
        {
            var hit = store.FetchEntry(id);
            if (hit is null)
            {
                missing++;
                body.AppendLine($"## gemini:{id} — not found");
                continue;
            }
            found++;
            var (entry, prompt, response) = hit.Value;

            body.AppendLine($"## {EntryLabel(entry)}");
            body.AppendLine($"{entry.Date} | {entry.Intent} | topic: {entry.TopicLabel}");
            if (entry.Gem is not null) body.AppendLine($"gem: {entry.Gem}");
            body.AppendLine($"thread summary: {entry.ThreadSummary}");
            body.AppendLine();

            var combined = $"### Prompt\n\n{prompt}\n\n### Response\n\n{response}";
            var totalChars = combined.Length;

            var window = combined.Substring(
                Math.Min(offset, combined.Length),
                Math.Min(length, Math.Max(0, combined.Length - offset)));
            body.AppendLine(window);

            var end = offset + window.Length;
            if (end < totalChars)
                body.AppendLine($"\n[WINDOWED — {totalChars - end:N0} more chars. " +
                                $"Continue: get_gemini_entry(ids: [{entry.Id}], offset: {end})]");
            body.AppendLine();
        }

        sb.AppendLine($"# get_gemini_entry — {found} returned" +
                      (missing > 0 ? $", {missing} not found" : "") +
                      (offset > 0 ? $" (from offset {offset})" : ""));
        sb.Append(body);
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "list_gemini_threads")]
    [Description(
        "Inventory of the Gemini founding-era corpus: all story-tagged threads (date range, turn " +
        "count, topic, summary) and all available reports with their slugs and sizes. The reports " +
        "are the primary entry point — start there for provenance questions.")]
    public string ListGeminiThreads()
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        var entries = store.Entries();
        var reports = store.Reports();
        var sb = new StringBuilder();

        var threads = entries.GroupBy(e => e.ThreadId).Select(g =>
        {
            var first = g.OrderBy(e => e.Date).ThenBy(e => e.ThreadPos).First();
            var last = g.OrderByDescending(e => e.Date).ThenByDescending(e => e.ThreadPos).First();
            return new
            {
                first.ThreadId,
                DateStart = first.Date,
                DateEnd = last.Date,
                Turns = g.Count(),
                first.TopicLabel,
                first.ThreadSummary,
                first.Subtopic
            };
        }).OrderBy(t => t.DateStart).ThenBy(t => t.ThreadId).ToList();

        sb.AppendLine($"# gemini corpus — {entries.Count:N0} entries across {threads.Count} threads, " +
                      $"{reports.Count} reports");
        sb.AppendLine("(founding-era Gemini web-app conversations, Sep 2025 – Jun 2026; " +
                      "provenance, not ground truth)");

        // Reports first — they are the primary entry point
        sb.AppendLine();
        sb.AppendLine($"## Reports ({reports.Count})");
        foreach (var r in reports)
        {
            var sizeLabel = r.BodyChars > 100_000 ? $"{r.BodyChars / 1000}K chars" : $"{r.BodyChars:N0} chars";
            sb.AppendLine($"report:{r.Id} [{r.Slug}] \"{r.Title}\" ({r.Kind}, {sizeLabel})");
        }

        // Threads
        sb.AppendLine();
        sb.AppendLine($"## Threads ({threads.Count})");
        foreach (var t in threads)
        {
            var dateRange = t.DateStart == t.DateEnd ? t.DateStart : $"{t.DateStart}..{t.DateEnd}";
            var sub = t.Subtopic is not null ? $" [{t.Subtopic}]" : "";
            sb.AppendLine($"{t.ThreadId} [{dateRange}] {t.Turns} turn(s){sub} — " +
                          $"{Query.Truncate(t.TopicLabel, 50)}: {Query.Truncate(t.ThreadSummary, 100)}");
        }

        return Query.Cap(sb);
    }

    private static string EntryLabel(GeminiCorpusStore.EntryManifest e) =>
        $"gemini:{e.Id} [{e.ThreadId} #{e.ThreadPos}/{e.ThreadSize} {e.Date}] " +
        $"\"{Query.Truncate(e.Title, 55)}\"";
}
