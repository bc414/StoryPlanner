using StoryPlanner.CodeSessions;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure-tier tests for the Claude Code transcript reduction — JSONL lines in, dialogue
/// records out. The extraction line under test is communication vs computation: text kept
/// verbatim, tool calls stubbed, thinking and tool payloads dropped with disclosure.
/// </summary>
public class CodeSessionExtractorTests
{
    private static string User(string uuid, string content, string? parent = null, string ts = "2026-08-01T10:00:00Z") =>
        $$"""{"type":"user","uuid":"{{uuid}}","parentUuid":{{(parent is null ? "null" : $"\"{parent}\"")}},"timestamp":"{{ts}}","sessionId":"s1","message":{"role":"user","content":{{content}} } }""";

    private static string Assistant(string uuid, string contentArray, string? parent = null, string ts = "2026-08-01T10:01:00Z") =>
        $$"""{"type":"assistant","uuid":"{{uuid}}","parentUuid":{{(parent is null ? "null" : $"\"{parent}\"")}},"timestamp":"{{ts}}","sessionId":"s1","message":{"role":"assistant","content":{{contentArray}} } }""";

    [Fact]
    public void User_string_content_is_kept_verbatim()
    {
        var session = CodeSessionExtractor.Extract([User("u1", "\"Fix the color picker please\"")]);

        var rec = Assert.Single(session.Records);
        Assert.Equal("Fix the color picker please", rec.Body);
        Assert.Equal("user", rec.Role);
    }

    [Fact]
    public void Tool_use_collapses_to_a_stub_naming_the_tool_and_its_main_argument()
    {
        var session = CodeSessionExtractor.Extract([Assistant("a1",
            """[{"type":"text","text":"Editing now."},{"type":"tool_use","id":"t1","name":"Edit","input":{"file_path":"WorldDateModel.cs","old_string":"x","new_string":"y"}}]""")]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("Editing now.", rec.Body);
        Assert.Contains("[tool_use: Edit — WorldDateModel.cs]", rec.Body);
        Assert.DoesNotContain("old_string", rec.Body);
    }

    [Fact]
    public void Tool_results_are_elided_with_a_char_count_and_the_payload_is_never_stored()
    {
        const string payloadEnvelope = "SECRET-PAYLOAD-CONTENT-THE-DB-MUST-NEVER-HOLD";
        var session = CodeSessionExtractor.Extract([User("u1",
            $$"""[{"type":"tool_result","tool_use_id":"t1","content":"{{payloadEnvelope}} plus more output"}]""")]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("[tool result elided —", rec.Body);
        Assert.Contains("chars]", rec.Body);
        Assert.DoesNotContain(payloadEnvelope, rec.Body);
    }

    [Fact]
    public void Thinking_parts_are_dropped_without_a_marker()
    {
        var session = CodeSessionExtractor.Extract([Assistant("a1",
            """[{"type":"thinking","thinking":"internal reasoning about the fix"},{"type":"text","text":"Here is the fix."}]""")]);

        var rec = Assert.Single(session.Records);
        Assert.Equal("Here is the fix.", rec.Body);
        Assert.DoesNotContain("internal reasoning", rec.Body);
        Assert.DoesNotContain("thinking", rec.Body);
    }

    [Fact]
    public void Only_dialogue_record_types_survive_and_the_last_ai_title_wins()
    {
        var session = CodeSessionExtractor.Extract([
            """{"type":"queue-operation","operation":"enqueue","timestamp":"2026-08-01T09:59:00Z","sessionId":"s1"}""",
            """{"type":"ai-title","aiTitle":"First title","sessionId":"s1"}""",
            User("u1", "\"hello\""),
            """{"type":"file-history-snapshot","sessionId":"s1"}""",
            """{"type":"ai-title","aiTitle":"Final title","sessionId":"s1"}""",
            """{"type":"last-prompt","lastPrompt":"hello","sessionId":"s1"}"""
        ]);

        Assert.Single(session.Records);
        Assert.Equal("Final title", session.Title);
    }

    [Fact]
    public void Duplicate_uuids_are_deduped_and_counted_and_parentUuid_is_preserved()
    {
        var session = CodeSessionExtractor.Extract([
            User("u1", "\"first\""),
            Assistant("a1", """[{"type":"text","text":"reply"}]""", parent: "u1"),
            User("u1", "\"replayed duplicate\"")
        ]);

        Assert.Equal(2, session.Records.Count);
        Assert.Equal(1, session.DuplicateUuids);
        Assert.Equal("u1", session.Records.Single(r => r.Uuid == "a1").ParentUuid);
        // First occurrence wins.
        Assert.Equal("first", session.Records.Single(r => r.Uuid == "u1").Body);
    }

    [Fact]
    public void A_torn_trailing_line_is_counted_not_fatal()
    {
        var session = CodeSessionExtractor.Extract([
            User("u1", "\"complete line\""),
            """{"type":"user","uuid":"u2","timestamp":"2026-08-01T10:0"""
        ]);

        Assert.Single(session.Records);
        Assert.Equal(1, session.MalformedLines);
    }

    [Fact]
    public void A_giant_user_paste_is_stubbed_with_word_and_char_disclosure()
    {
        var words = string.Join(" ", Enumerable.Repeat("word", 20_001));
        var session = CodeSessionExtractor.Extract([User("u1", $"\"{words}\"")]);

        var rec = Assert.Single(session.Records);
        Assert.StartsWith("[Large paste —", rec.Body);
        Assert.Contains("20,001 words", rec.Body);
        Assert.Equal(1, session.LargePasteStubs);
    }

    [Fact]
    public void Records_come_back_in_timestamp_order_with_input_order_as_tiebreak()
    {
        var session = CodeSessionExtractor.Extract([
            User("u2", "\"later\"", ts: "2026-08-01T11:00:00Z"),
            User("u1", "\"earlier\"", ts: "2026-08-01T10:00:00Z"),
            User("u3", "\"also later\"", ts: "2026-08-01T11:00:00Z")
        ]);

        Assert.Equal(["u1", "u2", "u3"], session.Records.Select(r => r.Uuid));
    }

    [Fact]
    public void Empty_bodies_are_skipped_and_counted()
    {
        var session = CodeSessionExtractor.Extract([
            Assistant("a1", """[{"type":"thinking","thinking":"only thinking, nothing said"}]"""),
            User("u1", "\"real turn\"")
        ]);

        Assert.Single(session.Records);
        Assert.Equal(1, session.EmptyRecords);
    }
}

/// <summary>Pure tests for the progressive-ingest classification.</summary>
public class IngestPlanTests
{
    [Fact]
    public void A_session_with_no_stored_stamp_is_new()
    {
        Assert.Equal(IngestPlan.Change.New, IngestPlan.Classify(null, 100, "2026-08-01T00:00:00Z"));
    }

    [Fact]
    public void A_matching_stamp_is_unchanged_and_any_difference_is_changed()
    {
        var stored = (SourceBytes: 100L, SourceMtimeUtc: "2026-08-01T00:00:00Z");

        Assert.Equal(IngestPlan.Change.Unchanged, IngestPlan.Classify(stored, 100, "2026-08-01T00:00:00Z"));
        Assert.Equal(IngestPlan.Change.Changed, IngestPlan.Classify(stored, 250, "2026-08-01T00:00:00Z"));
        Assert.Equal(IngestPlan.Change.Changed, IngestPlan.Classify(stored, 100, "2026-08-02T00:00:00Z"));
    }

    [Fact]
    public void Stored_sessions_whose_files_vanished_are_reported_as_absent_retained()
    {
        // The retention conveyor belt: files age off disk; their rows must persist. This list
        // is disclosure only — nothing acts on it.
        var absent = IngestPlan.AbsentRetained(
            storedIds: ["kept-session", "aged-out-session"],
            foundIds: ["kept-session", "brand-new-session"]);

        Assert.Equal(["aged-out-session"], absent);
    }
}
