using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using ModelContextProtocol.Server;

namespace StoryPlanner.Mcp;

/// <summary>
/// Retrieval over the LINEAGE corpus — the founding-era material behind the story's decisions,
/// four source layers in one database: the pre-AI Google Doc revision history, the Gemini
/// web-app conversations with their curated weekly reports, the never-imported AI Studio chats,
/// and the NotebookLM captures. One tool
/// family because the caller's question is lineage-shaped ("where did this come from / when
/// was X decided"), not platform-shaped.
///
/// The workflow is REPORTS FIRST: the curated digests answer "when was X decided?" directly;
/// everything else is the detail pass. Lineage is opt-in archeology — the default flow for any
/// question is the working plan; this corpus is reached for deliberately, and nothing in it is
/// ground truth.
///
/// Same standing rules as every other tool family: these report what was said. They never rank
/// sources by importance, propose what to take from a conversation, or suggest which era's
/// version of a decision is correct.
/// </summary>
[McpServerToolType]
public sealed class LineageTools(LineageStore store)
{
    private const int DefaultWindow = 40_000;
    private const int MaxWindow = 50_000;

    private static readonly string[] Sources = ["all", "gemini", "aistudio", "notebooklm", "gdoc"];
    private static readonly string[] Scopes = ["all", "reports", "entries", "chats", "notes", "system", "snapshots"];

    // ── search_lineage ────────────────────────────────────────────────────────

    [McpServerTool(Name = "search_lineage")]
    [Description(
        "Regex search across the LINEAGE corpus — the pre-AI Google Doc revision history " +
        "(Apr 2025 – Jan 2026), the Gemini web-app chats (Sep 2025 – Jun 2026), the AI Studio " +
        "chats (early 2026), and the NotebookLM captures. Reports are searched and listed FIRST: " +
        "curated digests that answer \"when was X decided?\" directly; drill into raw material " +
        "via get_lineage only when the verbatim exchange matters. Everything here is provenance, " +
        "not ground truth — most was superseded by later work. Hits carry source-prefixed ids " +
        "(gdoc:/gdoc-snapshot:/gemini:/report:/aistudio:/nlm:/nlm-note:) that get_lineage accepts.")]
    public string SearchLineage(
        [Description("Regex pattern (.NET syntax). Case-insensitive unless caseSensitive=true.")]
        string pattern,
        [Description("Source layer: \"all\" (default), \"gemini\", \"aistudio\", \"notebooklm\", or \"gdoc\".")]
        string source = "all",
        [Description("Scope: \"all\" (default) = gdoc diffs + reports + entries + chats + notes; " +
                     "\"reports\" / \"entries\" (gemini layer); \"chats\" (AI Studio + NotebookLM turns); " +
                     "\"notes\" (NotebookLM studio notes); \"system\" (AI Studio system instructions — " +
                     "searched ONLY under this scope); \"snapshots\" (Google Doc full-text snapshots — " +
                     "searched ONLY under this scope; use gdoc: diffs in the default search instead).")]
        string scope = "all",
        [Description("Restrict gemini entries to a subtopic: worldbuilding-lore, characters-arcs, politics-war-military, story-craft-structure, themes-philosophy, general, romance-relationships.")]
        string? subtopic = null,
        [Description("Match case-sensitively (default false).")]
        bool caseSensitive = false,
        [Description("Match whole words only (default false).")]
        bool wholeWord = false,
        [Description("Characters of context around each match snippet (20-2000, default 200).")]
        int contextChars = 200,
        [Description("Maximum hits listed per section (1-250, default 40). Totals are always reported even when hits are dropped.")]
        int limit = 40)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        Regex rx;
        try { rx = Query.BuildRegex(pattern, caseSensitive, wholeWord); }
        catch (ArgumentException ex) { return $"Invalid regex: {ex.Message}"; }

        if (!Sources.Contains(source))
            return $"Unknown source \"{source}\" — pass one of: {string.Join(", ", Sources)}.";
        if (!Scopes.Contains(scope))
            return $"Unknown scope \"{scope}\" — pass one of: {string.Join(", ", Scopes)}.";

        contextChars = Math.Clamp(contextChars, 20, 2000);
        limit = Math.Clamp(limit, 1, 250);

        var doGemini = source is "all" or "gemini";
        var doAiStudio = source is "all" or "aistudio";
        var doNlm = source is "all" or "notebooklm";
        var doGDoc = source is "all" or "gdoc";
        var doGDocDiffs = doGDoc && scope is "all";
        var doGDocSnapshots = doGDoc && scope is "snapshots";
        var doReports = doGemini && scope is "all" or "reports";
        var doEntries = doGemini && scope is "all" or "entries";
        var doAiChats = doAiStudio && scope is "all" or "chats";
        var doNlmChats = doNlm && scope is "all" or "chats";
        var doNotes = doNlm && scope is "all" or "notes";
        var doSystem = doAiStudio && scope is "system";

        var sections = new List<(string Header, int Total, List<string> Lines)>();

        try
        {
            if (doGDocDiffs)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (diff, body) in store.StreamGDocDiffs())
                {
                    var m = rx.Match(body);
                    if (!m.Success) continue;
                    total++;
                    if (lines.Count >= limit) continue;
                    lines.Add($"gdoc:{diff.Id} [{diff.Date} from {diff.FromDate}] " +
                              $"+{diff.LinesAdded}/-{diff.LinesRemoved} — " +
                              $"body: \"{Query.Snippet(body, m, contextChars)}\"");
                }
                sections.Add(("Google Doc diffs (pre-AI story plan changes)", total, lines));
            }

            if (doGDocSnapshots)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (snapshot, body) in store.StreamGDocSnapshots())
                {
                    var m = rx.Match(body);
                    if (!m.Success) continue;
                    total++;
                    if (lines.Count >= limit) continue;
                    lines.Add($"gdoc-snapshot:{snapshot.Id} [{snapshot.Date}] " +
                              $"{snapshot.BodyChars:N0} chars — " +
                              $"body: \"{Query.Snippet(body, m, contextChars)}\"");
                }
                sections.Add(("Google Doc snapshots (point-in-time full text)", total, lines));
            }

            if (doReports)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (report, body) in store.StreamReports())
                {
                    var m = rx.Match(body);
                    if (!m.Success) continue;
                    total++;
                    if (lines.Count < limit)
                        lines.Add($"report:{report.Id} [{report.Slug}] \"{Query.Truncate(report.Title, 60)}\" — " +
                                  $"{Query.Snippet(body, m, contextChars)}");
                }
                sections.Add(("Reports (gemini layer)", total, lines));
            }

            if (doEntries)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (entry, prompt, response) in store.StreamGeminiEntries(subtopic))
                {
                    var mPrompt = entry.IsPlanPaste ? Match.Empty : rx.Match(prompt);
                    var mResponse = rx.Match(response);
                    var mTopic = rx.Match(entry.TopicLabel);
                    var mSummary = rx.Match(entry.ThreadSummary);
                    if (!mPrompt.Success && !mResponse.Success && !mTopic.Success && !mSummary.Success)
                        continue;
                    total++;
                    if (lines.Count >= limit) continue;

                    string where;
                    if (mPrompt.Success) where = $"prompt: \"{Query.Snippet(prompt, mPrompt, contextChars)}\"";
                    else if (mResponse.Success) where = $"response: \"{Query.Snippet(response, mResponse, contextChars)}\"";
                    else if (mTopic.Success) where = $"topic: \"{entry.TopicLabel}\"";
                    else where = $"summary: \"{entry.ThreadSummary}\"";
                    if (entry.IsPlanPaste) where += " [plan paste]";
                    lines.Add($"{GeminiLabel(entry)} — {where}");
                }
                sections.Add(("Gemini entries", total, lines));
            }

            if (doAiChats)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (chat, turn) in store.StreamAiTurns())
                {
                    var mBody = rx.Match(turn.Body);
                    var mTitle = rx.Match(chat.Title);
                    if (!mBody.Success && !mTitle.Success) continue;
                    // A title hit would repeat once per turn — report it once, on turn 1.
                    if (!mBody.Success && turn.TurnIndex != 1) continue;
                    total++;
                    if (lines.Count >= limit) continue;

                    var where = mBody.Success
                        ? $"body: \"{Query.Snippet(turn.Body, mBody, contextChars)}\""
                        : $"title: \"{chat.Title}\"";
                    if (turn.IsPlaceholder) where += " [attached-document placeholder]";
                    lines.Add($"aistudio:{chat.Id} t#{turn.TurnIndex} [{DateOnlyPart(chat.Date)}] " +
                              $"\"{Query.Truncate(chat.Title, 50)}\" {turn.Role} — {where}");
                }
                sections.Add(("AI Studio turns", total, lines));
            }

            if (doNlmChats)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (nb, turnIndex, role, body) in store.StreamNlmTurns())
                {
                    var m = rx.Match(body);
                    if (!m.Success) continue;
                    total++;
                    if (lines.Count >= limit) continue;
                    lines.Add($"nlm:{nb.Id} t#{turnIndex} \"{Query.Truncate(nb.Title, 50)}\" {role} — " +
                              $"body: \"{Query.Snippet(body, m, contextChars)}\"");
                }
                sections.Add(("NotebookLM turns", total, lines));
            }

            if (doNotes)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (note, body) in store.StreamNlmNotes())
                {
                    var mTitle = rx.Match(note.Title);
                    var mBody = rx.Match(body);
                    if (!mTitle.Success && !mBody.Success) continue;
                    total++;
                    if (lines.Count >= limit) continue;
                    var where = mBody.Success
                        ? $"body: \"{Query.Snippet(body, mBody, contextChars)}\""
                        : $"title: \"{note.Title}\"";
                    lines.Add($"nlm-note:{note.Id} \"{Query.Truncate(note.Title, 60)}\" — {where}");
                }
                sections.Add(("NotebookLM notes", total, lines));
            }

            if (doSystem)
            {
                var total = 0;
                var lines = new List<string>();
                foreach (var (chat, system) in store.StreamAiSystemInstructions())
                {
                    var m = rx.Match(system);
                    if (!m.Success) continue;
                    total++;
                    if (lines.Count >= limit) continue;
                    lines.Add($"aistudio-system:{chat.Id} \"{Query.Truncate(chat.Title, 50)}\" — " +
                              $"system: \"{Query.Snippet(system, m, contextChars)}\"");
                }
                sections.Add(("AI Studio system instructions", total, lines));
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return "Regex timed out (2s) — simplify the pattern.";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# search_lineage /{pattern}/ — {sections.Sum(s => s.Total)} match(es)");
        foreach (var (header, total, lines) in sections)
        {
            sb.AppendLine();
            sb.AppendLine($"## {header} ({total} hit(s){(total > lines.Count ? $", showing first {lines.Count}" : "")})");
            if (lines.Count == 0) sb.AppendLine("(no matches)");
            foreach (var l in lines) sb.AppendLine(l);
        }

        return Query.Cap(sb);
    }

    // ── get_lineage ───────────────────────────────────────────────────────────

    [McpServerTool(Name = "get_lineage")]
    [Description(
        "Fetch full text from the LINEAGE corpus by source-prefixed id: \"gdoc:5\" (one day's " +
        "diff from the pre-AI story plan), \"gdoc-snapshot:3\" (full text at a date), \"report:3\" " +
        "or \"report:2025-W49\" (curated digest — the PRIMARY read), \"gemini:12\" (one web-app " +
        "exchange), \"aistudio:4\" (a whole AI Studio chat, turn by turn), \"aistudio-system:4\" " +
        "(that chat's system instruction), \"nlm:2\" or \"nlm:perspective-analysis\" (a whole " +
        "NotebookLM notebook), \"nlm-note:5\" (one studio note). Long texts are windowed via " +
        "offset/length; for chats and notebooks, fromTurn starts the window at a turn boundary " +
        "instead (from a search hit's t#).")]
    public string GetLineage(
        [Description("Source-prefixed ids (from search_lineage / list_lineage).")]
        string[] ids,
        [Description("Start character offset into each item's text. Default 0.")]
        int offset = 0,
        [Description("Characters to return per item (500-50000, default 40000).")]
        int length = DefaultWindow,
        [Description("For aistudio:/nlm: ids — start the window at this turn's boundary (used when offset is 0).")]
        int fromTurn = 0)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        offset = Math.Max(0, offset);
        length = Math.Clamp(length, 500, MaxWindow);

        var sb = new StringBuilder();
        int found = 0, missing = 0;
        var body = new StringBuilder();

        foreach (var rawId in ids.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var sep = rawId.IndexOf(':');
            var prefix = sep > 0 ? rawId[..sep].Trim().ToLowerInvariant() : "";
            var rest = sep > 0 ? rawId[(sep + 1)..].Trim() : "";

            var rendered = prefix switch
            {
                "gdoc" => RenderGDocDiff(body, rest, offset, length),
                "gdoc-snapshot" => RenderGDocSnapshot(body, rest, offset, length),
                "report" => RenderReport(body, rest, offset, length),
                "gemini" => RenderGeminiEntry(body, rest, offset, length),
                "aistudio" => RenderAiChat(body, rest, offset, length, fromTurn),
                "aistudio-system" => RenderAiSystem(body, rest, offset, length),
                "nlm" => RenderNlmNotebook(body, rest, offset, length, fromTurn),
                "nlm-note" => RenderNlmNote(body, rest),
                _ => Unknown(body, rawId)
            };
            if (rendered) found++; else missing++;
        }

        sb.AppendLine($"# get_lineage — {found} returned" +
                      (missing > 0 ? $", {missing} not found" : "") +
                      (offset > 0 ? $" (from offset {offset})" : ""));
        sb.Append(body);
        return Query.Cap(sb);

        static bool Unknown(StringBuilder body, string rawId)
        {
            body.AppendLine($"## {rawId} — unknown id form. Valid prefixes: gdoc:, gdoc-snapshot:, " +
                            "report:, gemini:, aistudio:, aistudio-system:, nlm:, nlm-note:");
            return false;
        }
    }

    private bool RenderGDocDiff(StringBuilder body, string rest, int offset, int length)
    {
        if (!int.TryParse(rest, out var id) || store.FetchGDocDiff(id) is not { } hit)
        {
            body.AppendLine($"## gdoc:{rest} — not found");
            return false;
        }
        var (diff, text) = hit;
        body.AppendLine($"## gdoc:{diff.Id} — changes on {diff.Date} (from {diff.FromDate})");
        body.AppendLine($"+{diff.LinesAdded} lines / -{diff.LinesRemoved} lines | {diff.DeltaBytes:+#;-#;0} bytes");
        body.AppendLine();
        AppendWindow(body, text, offset, length, $"get_lineage(ids: [\"gdoc:{diff.Id}\"]");
        return true;
    }

    private bool RenderGDocSnapshot(StringBuilder body, string rest, int offset, int length)
    {
        if (!int.TryParse(rest, out var id) || store.FetchGDocSnapshot(id) is not { } hit)
        {
            body.AppendLine($"## gdoc-snapshot:{rest} — not found");
            return false;
        }
        var (snapshot, text) = hit;
        body.AppendLine($"## gdoc-snapshot:{snapshot.Id} — snapshot on {snapshot.Date}");
        body.AppendLine($"{snapshot.BodyChars:N0} chars | {snapshot.FileBytes:N0} bytes | extraction: {snapshot.Source}");
        body.AppendLine();
        AppendWindow(body, text, offset, length, $"get_lineage(ids: [\"gdoc-snapshot:{snapshot.Id}\"]");
        return true;
    }

    private bool RenderReport(StringBuilder body, string rest, int offset, int length)
    {
        var hit = int.TryParse(rest, out var id) ? store.FetchReport(id) : store.FetchReportBySlug(rest);
        if (hit is null)
        {
            body.AppendLine($"## report:{rest} — not found. Available slugs: " +
                            string.Join(", ", store.Reports().Select(r => r.Slug)));
            return false;
        }
        var (report, text) = hit.Value;
        body.AppendLine($"## report:{report.Id} — [{report.Slug}] \"{report.Title}\"");
        body.AppendLine($"{report.BodyChars:N0} chars total | kind:{report.Kind}");
        body.AppendLine();
        AppendWindow(body, text, offset, length, $"get_lineage(ids: [\"report:{report.Id}\"]");
        return true;
    }

    private bool RenderGeminiEntry(StringBuilder body, string rest, int offset, int length)
    {
        if (!int.TryParse(rest, out var id) || store.FetchGeminiEntry(id) is not { } hit)
        {
            body.AppendLine($"## gemini:{rest} — not found");
            return false;
        }
        var (entry, prompt, response) = hit;
        body.AppendLine($"## {GeminiLabel(entry)}");
        body.AppendLine($"{entry.Date} | {entry.Intent} | topic: {entry.TopicLabel}");
        if (entry.Gem is not null) body.AppendLine($"gem: {entry.Gem}");
        body.AppendLine($"thread summary: {entry.ThreadSummary}");
        body.AppendLine();
        AppendWindow(body, $"### Prompt\n\n{prompt}\n\n### Response\n\n{response}",
            offset, length, $"get_lineage(ids: [\"gemini:{entry.Id}\"]");
        return true;
    }

    private bool RenderAiChat(StringBuilder body, string rest, int offset, int length, int fromTurn)
    {
        if (!int.TryParse(rest, out var id) || store.FetchAiChat(id) is not { } hit)
        {
            body.AppendLine($"## aistudio:{rest} — not found");
            return false;
        }
        var (chat, turns) = hit;
        body.AppendLine($"## aistudio:{chat.Id} — \"{chat.Title}\" (key: \"{chat.ChatKey}\")");
        body.AppendLine($"{DateOnlyPart(chat.Date)} | {chat.TurnCount} turns | {chat.TotalChars:N0} chars" +
                        (chat.Model is not null ? $" | model: {chat.Model}" : ""));
        if (chat.SystemChars > 0)
            body.AppendLine($"system instruction: {chat.SystemChars:N0} chars — fetch with aistudio-system:{chat.Id}");
        body.AppendLine();

        var composed = ComposeTurns(turns.Select(t =>
            (t.TurnIndex, t.Role, t.CreateTime is not null ? DateOnlyPart(t.CreateTime) : null, t.Body)));
        var start = EffectiveOffset(composed, offset, fromTurn);
        AppendWindow(body, composed, start, length, $"get_lineage(ids: [\"aistudio:{chat.Id}\"]");
        return true;
    }

    private bool RenderAiSystem(StringBuilder body, string rest, int offset, int length)
    {
        if (!int.TryParse(rest, out var id) || store.FetchAiSystem(id) is not { } hit)
        {
            body.AppendLine($"## aistudio-system:{rest} — not found");
            return false;
        }
        var (chat, system) = hit;
        body.AppendLine($"## aistudio-system:{chat.Id} — system instruction of \"{chat.Title}\"");
        body.AppendLine();
        AppendWindow(body, system, offset, length, $"get_lineage(ids: [\"aistudio-system:{chat.Id}\"]");
        return true;
    }

    private bool RenderNlmNotebook(StringBuilder body, string rest, int offset, int length, int fromTurn)
    {
        var hit = int.TryParse(rest, out var id)
            ? store.FetchNlmNotebook(id, null)
            : store.FetchNlmNotebook(null, rest);
        if (hit is null)
        {
            var available = store.NlmNotebooks().Select(n => n.Slug).ToList();
            body.AppendLine($"## nlm:{rest} — not found" +
                            (available.Count > 0 ? $". Available slugs: {string.Join(", ", available)}" : ""));
            return false;
        }
        var (nb, turns) = hit.Value;
        body.AppendLine($"## nlm:{nb.Id} — \"{nb.Title}\" ({nb.Slug})");
        body.AppendLine($"{nb.AuthoredDate ?? "undated — authored date pending"} | {nb.TurnCount} turns | " +
                        $"{nb.NoteCount} note(s) | captured {DateOnlyPart(nb.CapturedUtc)} from \"{nb.CaptureFile}\"");
        var notes = store.NlmNotes().Where(o => o.Slug.Equals(nb.Slug, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var note in notes)
            body.AppendLine($"  nlm-note:{note.Id} \"{note.Title}\"" +
                            (note.RelativeAge.Length > 0 ? $" ({note.RelativeAge} before capture)" : ""));
        body.AppendLine();

        var composed = ComposeTurns(turns.Select(t => (t.TurnIndex, t.Role, (string?)null, t.Body)));
        var start = EffectiveOffset(composed, offset, fromTurn);
        AppendWindow(body, composed, start, length, $"get_lineage(ids: [\"nlm:{nb.Id}\"]");
        return true;
    }

    private bool RenderNlmNote(StringBuilder body, string rest)
    {
        if (!int.TryParse(rest, out var id) || store.FetchNlmNote(id) is not { } hit)
        {
            body.AppendLine($"## nlm-note:{rest} — not found");
            return false;
        }
        var (note, text) = hit;
        body.AppendLine($"## nlm-note:{note.Id} — \"{note.Title}\"" +
                        (note.RelativeAge.Length > 0 ? $" ({note.RelativeAge} before capture)" : ""));
        body.AppendLine(text.Length > 0
            ? text
            : "(title-only — the saved studio panel renders note previews; the body was never captured, not withheld)");
        body.AppendLine();
        return true;
    }

    // ── list_lineage ──────────────────────────────────────────────────────────

    [McpServerTool(Name = "list_lineage")]
    [Description(
        "Inventory of the LINEAGE corpus: per-source ingest status (\"never ingested\" is " +
        "disclosed distinctly from \"zero rows\"), the pre-AI Google Doc revision history " +
        "(diffs + snapshots), all curated reports (the primary entry point), the AI Studio " +
        "chats, and the NotebookLM notebooks with their notes. Pass detail=\"gemini-threads\" " +
        "for the full per-thread inventory of the gemini layer instead of the summary.")]
    public string ListLineage(
        [Description("\"summary\" (default) or \"gemini-threads\" (the gemini layer's full thread inventory).")]
        string detail = "summary")
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;
        if (detail is not ("summary" or "gemini-threads"))
            return $"Unknown detail \"{detail}\" — pass \"summary\" (default) or \"gemini-threads\".";

        var entries = store.GeminiEntries();
        var reports = store.Reports();
        var aiChats = store.AiChats();
        var notebooks = store.NlmNotebooks();
        var notes = store.NlmNotes();
        var gdocDiffs = store.GDocDiffs();
        var gdocSnapshots = store.GDocSnapshots();
        var runs = store.LatestIngestRuns();

        var sb = new StringBuilder();
        sb.AppendLine("# lineage corpus — founding-era provenance, four source layers, never ground truth");
        sb.AppendLine("(reached for deliberately — the default for any question is the working plan)");
        sb.AppendLine();
        sb.AppendLine("## Sources");
        sb.AppendLine(SourceStatus("gdoc", runs,
            $"{gdocDiffs.Count} diffs, {gdocSnapshots.Count} snapshots",
            "tools/StoryPlanner.GDocHistory"));
        sb.AppendLine(SourceStatus("gemini", runs,
            $"{entries.Count:N0} entries across {entries.Select(e => e.ThreadId).Distinct().Count()} threads, {reports.Count} reports",
            "tools/StoryPlanner.GeminiCorpus"));
        sb.AppendLine(SourceStatus("aistudio", runs,
            $"{aiChats.Count} chats, {aiChats.Sum(c => c.TurnCount)} turns",
            "tools/StoryPlanner.Lineage"));
        sb.AppendLine(SourceStatus("notebooklm", runs,
            $"{notebooks.Count} notebook(s), {notebooks.Sum(n => n.TurnCount)} turns, {notes.Count} note(s)",
            "tools/StoryPlanner.Lineage"));

        if (detail == "gemini-threads")
        {
            var threads = entries.GroupBy(e => e.ThreadId).Select(g =>
            {
                var first = g.OrderBy(e => e.Date).ThenBy(e => e.ThreadPos).First();
                var last = g.OrderByDescending(e => e.Date).ThenByDescending(e => e.ThreadPos).First();
                return (first.ThreadId, DateStart: first.Date, DateEnd: last.Date, Turns: g.Count(),
                    first.TopicLabel, first.ThreadSummary, first.Subtopic);
            }).OrderBy(t => t.DateStart).ThenBy(t => t.ThreadId).ToList();

            sb.AppendLine();
            sb.AppendLine($"## Gemini threads ({threads.Count})");
            foreach (var t in threads)
            {
                var dateRange = t.DateStart == t.DateEnd ? t.DateStart : $"{t.DateStart}..{t.DateEnd}";
                var sub = t.Subtopic is not null ? $" [{t.Subtopic}]" : "";
                sb.AppendLine($"{t.ThreadId} [{dateRange}] {t.Turns} turn(s){sub} — " +
                              $"{Query.Truncate(t.TopicLabel, 50)}: {Query.Truncate(t.ThreadSummary, 100)}");
            }
            return Query.Cap(sb);
        }

        // GDoc diffs — the pre-AI story plan
        if (gdocDiffs.Count > 0 || gdocSnapshots.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine($"## Google Doc revision history ({gdocDiffs.Count} diffs, {gdocSnapshots.Count} snapshots) — " +
                          "pre-AI story plan (Apr 2025 – Jan 2026)");
            sb.AppendLine("Diffs show what changed each day (searched by default); snapshots are full text " +
                          "(scope \"snapshots\" only, retrievable by id).");
            foreach (var d in gdocDiffs)
                sb.AppendLine($"gdoc:{d.Id} [{d.Date} from {d.FromDate}] +{d.LinesAdded}/-{d.LinesRemoved}, " +
                              $"{d.DeltaBytes:+#;-#;0} bytes, {d.BodyChars:N0} diff chars");
            if (gdocSnapshots.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Snapshots (point-in-time full text, not in default search):");
                foreach (var s in gdocSnapshots)
                    sb.AppendLine($"  gdoc-snapshot:{s.Id} [{s.Date}] {s.BodyChars:N0} chars ({s.Source})");
            }
        }

        // Reports — they are the primary entry point.
        sb.AppendLine();
        sb.AppendLine($"## Reports ({reports.Count}) — curated digests, the primary entry point");
        foreach (var r in reports)
        {
            var sizeLabel = r.BodyChars > 100_000 ? $"{r.BodyChars / 1000}K chars" : $"{r.BodyChars:N0} chars";
            sb.AppendLine($"report:{r.Id} [{r.Slug}] \"{r.Title}\" ({r.Kind}, {sizeLabel})");
        }

        sb.AppendLine();
        sb.AppendLine($"## AI Studio chats ({aiChats.Count}) — never imported into the Conversations corpus, by construction");
        foreach (var c in aiChats)
            sb.AppendLine($"aistudio:{c.Id} [{DateOnlyPart(c.Date)}] \"{c.Title}\" — {c.TurnCount} turns, " +
                          $"{c.TotalChars / 1000}K chars (key: \"{c.ChatKey}\")");

        sb.AppendLine();
        sb.AppendLine($"## NotebookLM notebooks ({notebooks.Count})");
        foreach (var nb in notebooks)
        {
            sb.AppendLine($"nlm:{nb.Id} [{nb.AuthoredDate ?? "undated — authored date pending"}] " +
                          $"\"{nb.Title}\" ({nb.Slug}) — {nb.TurnCount} turns, {nb.NoteCount} note(s), " +
                          $"captured {DateOnlyPart(nb.CapturedUtc)}");
            foreach (var note in notes.Where(o => o.Slug.Equals(nb.Slug, StringComparison.OrdinalIgnoreCase)))
                sb.AppendLine($"  nlm-note:{note.Id} \"{note.Title}\"" +
                              (note.RelativeAge.Length > 0 ? $" ({note.RelativeAge} before capture)" : ""));
        }

        return Query.Cap(sb);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string SourceStatus(
        string source, IReadOnlyDictionary<string, LineageStore.IngestRun> runs, string counts, string ingestTool)
    {
        return runs.TryGetValue(source, out var run)
            ? $"{source}: {counts} (last ingested {DateOnlyPart(run.RunUtc)})"
            : $"{source}: never ingested — run {ingestTool} against this file";
    }

    private static string ComposeTurns(IEnumerable<(int TurnIndex, string Role, string? Time, string Body)> turns)
    {
        var sb = new StringBuilder();
        foreach (var t in turns)
        {
            sb.Append(TurnHeading(t.TurnIndex, t.Role, t.Time));
            sb.Append('\n');
            sb.Append(t.Body);
            sb.Append("\n\n");
        }
        return sb.ToString().TrimEnd();
    }

    private static string TurnHeading(int turnIndex, string role, string? time) =>
        time is not null ? $"### t#{turnIndex} {role} [{time}]" : $"### t#{turnIndex} {role}";

    /// <summary>fromTurn resolves to that turn's heading boundary; an explicit offset wins.</summary>
    private static int EffectiveOffset(string composed, int offset, int fromTurn)
    {
        if (offset > 0 || fromTurn <= 1) return offset;
        var idx = composed.IndexOf($"### t#{fromTurn} ", StringComparison.Ordinal);
        return idx >= 0 ? idx : offset;
    }

    private static void AppendWindow(StringBuilder body, string text, int offset, int length, string continueCallPrefix)
    {
        var window = text.Substring(
            Math.Min(offset, text.Length),
            Math.Min(length, Math.Max(0, text.Length - offset)));
        body.AppendLine(window);

        var end = offset + window.Length;
        if (end < text.Length)
            body.AppendLine($"\n[WINDOWED — {text.Length - end:N0} more chars. " +
                            $"Continue: {continueCallPrefix}, offset: {end})]");
        body.AppendLine();
    }

    private static string DateOnlyPart(string isoOrDate) =>
        isoOrDate.Length >= 10 && isoOrDate[4] == '-' ? isoOrDate[..10] : isoOrDate;

    private static string GeminiLabel(LineageStore.GeminiEntryManifest e) =>
        $"gemini:{e.Id} [{e.ThreadId} #{e.ThreadPos}/{e.ThreadSize} {e.Date}] " +
        $"\"{Query.Truncate(e.Title, 55)}\"";
}
