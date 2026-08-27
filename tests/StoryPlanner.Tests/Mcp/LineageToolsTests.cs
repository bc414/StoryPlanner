using StoryPlanner.GDocHistory;
using StoryPlanner.GeminiCorpus;
using StoryPlanner.Lineage;
using StoryPlanner.Mcp;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Fixture-tier tests for the LINEAGE corpus — one real temp lineage.db written the way the
/// production ingests write it: the gemini layer by GeminiCorpusDb, the AI Studio and
/// NotebookLM layers by LineageDb, each recording its run in the shared IngestRuns ledger.
/// </summary>
public class LineageToolsTests : IDisposable
{
    private readonly string _dir = Directory.CreateTempSubdirectory("lineage-test").FullName;
    private readonly string _dbPath;

    /// <summary>Longer than the 500-char minimum window, so windowing is actually exercised.</summary>
    private static readonly string LongBody = string.Join("\n",
        Enumerable.Range(0, 100).Select(i => $"This is line {i:000} of a very long model answer about the organizer."));

    public LineageToolsTests()
    {
        _dbPath = Path.Combine(_dir, "lineage.db");

        // Gemini layer — written by ITS ingest's Db class, into the same file.
        using (var conn = GeminiCorpusDb.OpenWrite(_dbPath))
        {
            GeminiCorpusDb.ReplaceEntries(conn, [
                new GeminiEntry(
                    EntryId: "abc12345", ThreadId: "T001", ThreadPos: 1, ThreadSize: 1,
                    Date: "2025-11-02", LocalTime: "2025-11-02 20:15", Subject: "creative-writing",
                    Subtopic: "worldbuilding-lore", TopicLabel: "Coltbert economics",
                    ThreadSummary: "Deriving the crossbow decision", Intent: "explore", Gem: null,
                    Title: "Coltbert and the crossbow", Prompt: "Why did Coltbert reject the crossbow subsidy?",
                    Response: "The crossbow decision traces to the tally-stick reform.", Type: "conversational",
                    IsPlanPaste: false, PromptChars: 44, ResponseChars: 55)
            ]);
            GeminiCorpusDb.ReplaceReports(conn, [
                new GeminiReport(Slug: "2025-W49", Title: "Week 49 — the crossbow decision",
                    Kind: "weekly", Body: "This week the Coltbert crossbow decision was settled.")
            ]);
            GeminiCorpusDb.RecordIngestRun(conn, 2);
        }

        // AI Studio + NotebookLM layers — written by the lineage ingest's Db class.
        using (var conn = LineageDb.OpenWrite(_dbPath))
        {
            LineageDb.ReplaceAiStudio(conn, [
                new AiStudioChat(
                    ChatKey: "Note Organizer Part 0", Title: "Note Organizer Part 0",
                    Date: "2026-02-20T10:00:00Z", Model: "models/gemini-test",
                    SystemInstruction: "SYSINSTRUCT: you are a narrative co-architect.",
                    Turns: [
                        new AiStudioTurn(1, "user", "2026-02-20T10:00:00Z", false, "How should the note organizer work?"),
                        new AiStudioTurn(2, "model", "2026-02-20T10:01:00Z", false, LongBody),
                        new AiStudioTurn(3, "user", "2026-02-20T10:02:00Z", true, "[Attached document: doc123]"),
                        new AiStudioTurn(4, "model", "2026-02-20T10:03:00Z", false, "Final thoughts on the organizer.")
                    ],
                    TurnsMissingCreateTime: 0)
            ]);
            LineageDb.RecordIngestRun(conn, "aistudio", 4);

            LineageDb.ReplaceNotebookLm(conn, [
                new NlmNotebook(
                    Slug: "perspective-analysis", Title: "Perspective Analysis",
                    AuthoredDate: "2026-02", CaptureFile: "Perspective Analysis.htm",
                    CapturedUtc: "2026-08-13T00:00:00Z",
                    Turns: [
                        new NlmTurn(1, "user", "Break down the perspective used in Silver."),
                        new NlmTurn(2, "model", "Silver uses third-person limited with a shift to Eve.")
                    ],
                    Notes: [new NlmNote(1, "Literary Blueprints", "179d", "")])
            ]);
            LineageDb.RecordIngestRun(conn, "notebooklm", 3);
        }

        // GDoc revision history layer — written by its own Db class.
        using (var conn = GDocHistoryDb.OpenWrite(_dbPath))
        {
            GDocHistoryDb.ReplaceGDocHistory(conn,
                [
                    new GDocSnapshot("2025-04-18", "Cover art for TLTT\n\nOCs\nMali is a thestral volunteer", 22407, "v1-only"),
                    new GDocSnapshot("2025-04-28", "Cover art for TLTT\n\nOCs\nMali is a thestral volunteer\nComet Shine runs Star Energy", 32594, "v1-only"),
                ],
                [
                    new GDocDiffEntry("2025-04-28", "2025-04-18",
                        "# Changes: 2025-04-28 (from 2025-04-18)\n+1 lines / -0 lines\n\n--- under: OCs, near line 4 ---\n  Mali is a thestral volunteer\n+ Comet Shine runs Star Energy",
                        1, 0, 10187),
                ]);
            GDocHistoryDb.RecordIngestRun(conn, 3);
        }
    }

    private LineageTools Tools() => new(new LineageStore(_dbPath));

    [Fact]
    public void An_unconfigured_corpus_reports_itself_instead_of_failing()
    {
        var tools = new LineageTools(new LineageStore(null));

        Assert.Contains("STORYPLAN_LINEAGE", tools.ListLineage());
        Assert.Contains("STORYPLAN_LINEAGE", tools.SearchLineage("anything"));
        Assert.Contains("STORYPLAN_LINEAGE", tools.GetLineage(["gemini:1"]));
    }

    [Fact]
    public void Search_hits_carry_source_prefixed_ids_and_field_prefixes()
    {
        var result = Tools().SearchLineage("crossbow");
        Assert.Contains("report:1", result);
        Assert.Contains("gemini:1", result);
        Assert.Contains("prompt:", result); // field priority: prompt before response, as in the retired search_gemini

        var responseOnly = Tools().SearchLineage("tally-stick");
        Assert.Contains("response:", responseOnly);

        var aiHit = Tools().SearchLineage("note organizer");
        Assert.Contains("aistudio:1 t#1", aiHit);
        Assert.Contains("body:", aiHit);

        var nlmHit = Tools().SearchLineage("third-person limited");
        Assert.Contains("nlm:1 t#2", nlmHit);
    }

    [Fact]
    public void System_instructions_match_only_under_scope_system()
    {
        var defaultScope = Tools().SearchLineage("SYSINSTRUCT");
        Assert.Contains("0 match(es)", defaultScope);

        var systemScope = Tools().SearchLineage("SYSINSTRUCT", scope: "system");
        Assert.Contains("aistudio-system:1", systemScope);
    }

    [Fact]
    public void Unknown_source_or_scope_refuses_rather_than_falling_back()
    {
        Assert.Contains("Unknown source", Tools().SearchLineage("x", source: "claude"));
        Assert.Contains("Unknown scope", Tools().SearchLineage("x", scope: "bodies"));
        Assert.Contains("Unknown detail", Tools().ListLineage(detail: "everything"));
    }

    [Fact]
    public void Get_lineage_windows_and_the_named_offset_actually_continues()
    {
        var first = Tools().GetLineage(["aistudio:1"], length: 500);

        Assert.Contains("line 000", first);
        Assert.DoesNotContain("line 099", first);
        Assert.Contains("WINDOWED", first);

        var offset = int.Parse(System.Text.RegularExpressions.Regex.Match(first, @"offset: (\d+)\)").Groups[1].Value);
        var second = Tools().GetLineage(["aistudio:1"], offset: offset, length: 2000);
        Assert.DoesNotContain("line 000", second);
        Assert.Contains("line 0", second); // continues inside the long body, not from the top
    }

    [Fact]
    public void From_turn_starts_the_window_at_that_turns_boundary()
    {
        var result = Tools().GetLineage(["aistudio:1"], fromTurn: 4, length: 500);

        Assert.Contains("### t#4 model", result);
        Assert.Contains("Final thoughts on the organizer.", result);
        Assert.DoesNotContain("line 000", result);
    }

    [Fact]
    public void A_placeholder_turn_is_disclosed_in_search_hits()
    {
        var result = Tools().SearchLineage("Attached document");

        Assert.Contains("aistudio:1 t#3", result);
        Assert.Contains("[attached-document placeholder]", result);
    }

    [Fact]
    public void Reports_fetch_by_slug_through_the_prefixed_id()
    {
        var result = Tools().GetLineage(["report:2025-W49"]);

        Assert.Contains("crossbow decision was settled", result);
    }

    [Fact]
    public void List_shows_per_source_status_reports_first_and_authored_or_pending_dates()
    {
        var result = Tools().ListLineage();

        Assert.Contains("gemini: 1 entries", result);
        Assert.Contains("aistudio: 1 chats", result);
        Assert.Contains("notebooklm: 1 notebook(s)", result);
        Assert.Contains("last ingested", result);
        Assert.Contains("report:1 [2025-W49]", result);
        Assert.Contains("nlm:1 [2026-02]", result);
        Assert.Contains("nlm-note:1 \"Literary Blueprints\" (179d before capture)", result);
        // Reports are the entry point: their section precedes the AI Studio and NLM sections.
        Assert.True(result.IndexOf("## Reports", StringComparison.Ordinal) <
                    result.IndexOf("## AI Studio", StringComparison.Ordinal));
    }

    [Fact]
    public void A_never_ingested_source_is_disclosed_distinctly_from_zero_rows()
    {
        // A second db that only the gemini ingest has touched — the aistudio/notebooklm tables
        // don't exist at all, which must read as "never ingested", not as empty results.
        var geminiOnlyPath = Path.Combine(_dir, "gemini-only.db");
        using (var conn = GeminiCorpusDb.OpenWrite(geminiOnlyPath))
        {
            GeminiCorpusDb.ReplaceEntries(conn, []);
            GeminiCorpusDb.ReplaceReports(conn, []);
            GeminiCorpusDb.RecordIngestRun(conn, 0);
        }

        var result = new LineageTools(new LineageStore(geminiOnlyPath)).ListLineage();

        Assert.Contains("gemini: 0 entries", result);
        Assert.Contains("last ingested", result);
        Assert.Contains("aistudio: never ingested", result);
        Assert.Contains("notebooklm: never ingested", result);

        // And search over the missing layers finds nothing rather than throwing.
        Assert.Contains("0 match(es)", new LineageTools(new LineageStore(geminiOnlyPath)).SearchLineage("anything"));
    }

    [Fact]
    public void A_title_only_note_reports_never_captured_not_withheld()
    {
        var result = Tools().GetLineage(["nlm-note:1"]);

        Assert.Contains("Literary Blueprints", result);
        Assert.Contains("title-only", result);
        Assert.Contains("never captured", result);
    }

    [Fact]
    public void An_invalid_regex_is_an_error_message_not_an_exception()
    {
        Assert.Contains("Invalid regex", Tools().SearchLineage("(unclosed"));
    }

    [Fact]
    public void An_unknown_id_prefix_names_the_valid_forms()
    {
        var result = Tools().GetLineage(["conversation:5"]);

        Assert.Contains("unknown id form", result);
        Assert.Contains("aistudio-system:", result);
    }

    [Fact]
    public void GDoc_diffs_are_searched_under_default_scope()
    {
        var result = Tools().SearchLineage("Comet Shine");
        Assert.Contains("gdoc:1", result);
        Assert.Contains("body:", result);
    }

    [Fact]
    public void GDoc_snapshots_are_NOT_searched_under_default_scope()
    {
        var result = Tools().SearchLineage("thestral volunteer");
        Assert.DoesNotContain("gdoc-snapshot:", result);
    }

    [Fact]
    public void GDoc_snapshots_are_searched_under_scope_snapshots()
    {
        var result = Tools().SearchLineage("thestral volunteer", scope: "snapshots");
        Assert.Contains("gdoc-snapshot:", result);
    }

    [Fact]
    public void GDoc_diff_is_retrievable_by_prefixed_id()
    {
        var result = Tools().GetLineage(["gdoc:1"]);
        Assert.Contains("changes on 2025-04-28", result);
        Assert.Contains("from 2025-04-18", result);
    }

    [Fact]
    public void GDoc_snapshot_is_retrievable_by_prefixed_id()
    {
        var result = Tools().GetLineage(["gdoc-snapshot:1"]);
        Assert.Contains("snapshot on 2025-04-18", result);
        Assert.Contains("Cover art for TLTT", result);
    }

    [Fact]
    public void List_shows_gdoc_source_status()
    {
        var result = Tools().ListLineage();
        Assert.Contains("gdoc:", result);
        Assert.Contains("diffs", result);
        Assert.Contains("snapshots", result);
        Assert.Contains("last ingested", result);
    }

    [Fact]
    public void Missing_gdoc_tables_are_absent_not_an_error()
    {
        var geminiOnlyPath = Path.Combine(_dir, "gdoc-absent.db");
        using (var conn = GeminiCorpusDb.OpenWrite(geminiOnlyPath))
        {
            GeminiCorpusDb.ReplaceEntries(conn, []);
            GeminiCorpusDb.ReplaceReports(conn, []);
            GeminiCorpusDb.RecordIngestRun(conn, 0);
        }
        var result = new LineageTools(new LineageStore(geminiOnlyPath)).ListLineage();
        Assert.Contains("gdoc: never ingested", result);
    }

    public void Dispose()
    {
        try { Directory.Delete(_dir, recursive: true); } catch (IOException) { }
    }
}
