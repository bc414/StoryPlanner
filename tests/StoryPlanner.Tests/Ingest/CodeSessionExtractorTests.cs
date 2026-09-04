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

    // ---- Human-authored tool results (2026-09-04) -------------------------------------
    // The one clause qualifying "tool results are computation": a result carrying the author's
    // words or decision is kept. What these tests protect is the AUTHORSHIP distinction —
    // "Typed:" is the author's own prose and must never be recorded as a machine-authored
    // "Chose:", nor the reverse.

    /// <summary>A user record whose answer/verdict rides in the top-level toolUseResult sidecar.</summary>
    private static string UserWithResult(string uuid, string contentArray, string sidecar, string ts = "2026-08-01T10:02:00Z") =>
        $$"""{"type":"user","uuid":"{{uuid}}","parentUuid":null,"timestamp":"{{ts}}","sessionId":"s1","toolUseResult":{{sidecar}},"message":{"role":"user","content":{{contentArray}} } }""";

    private const string OneResultPart = """[{"type":"tool_result","tool_use_id":"t1","content":"The user answered: ..."}]""";

    [Fact]
    public void An_answer_matching_an_offered_label_is_recorded_as_a_choice()
    {
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", OneResultPart, """
            {"questions":[{"question":"Which approach?","header":"Approach","multiSelect":false,
                           "options":[{"label":"Keep it prose only","description":"d1"},
                                      {"label":"Add a relation field","description":"d2"}]}],
             "answers":{"Which approach?":"Keep it prose only"}}
            """)]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("[AskUserQuestion — 1 answered]", rec.Body);
        Assert.Contains("Q: Which approach?", rec.Body);
        Assert.Contains("Chose: Keep it prose only", rec.Body);
        Assert.DoesNotContain("Typed:", rec.Body);
        Assert.Equal(1, session.HumanResults);
    }

    [Fact]
    public void Free_text_is_kept_verbatim_and_marked_as_typed()
    {
        const string typed = "I was imagining a locator on note, like history.\nIt would be optional.";
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", OneResultPart, $$"""
            {"questions":[{"question":"How should it be shaped?","header":"Shape","multiSelect":false,
                           "options":[{"label":"Flat item rows","description":"d1"}]}],
             "answers":{"How should it be shaped?":{{System.Text.Json.JsonSerializer.Serialize(typed)}} } }
            """)]);

        var rec = Assert.Single(session.Records);
        Assert.Contains($"Typed: {typed}", rec.Body);
        Assert.DoesNotContain("Chose:", rec.Body);
    }

    [Fact]
    public void Free_text_that_merely_begins_with_a_label_is_still_the_authors_own_words()
    {
        // The prefix-match trap: a real answer opens with a label and keeps going. Attributing
        // this to the machine would silently strip the author's reasoning from the archive.
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", OneResultPart, """
            {"questions":[{"question":"Prose or relation?","header":"Shape","multiSelect":false,
                           "options":[{"label":"Keep it prose only","description":"d1"}]}],
             "answers":{"Prose or relation?":"Keep it prose only. The whole point is a cross-cutting view."}}
            """)]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("Typed: Keep it prose only. The whole point is a cross-cutting view.", rec.Body);
        Assert.DoesNotContain("Chose:", rec.Body);
    }

    [Fact]
    public void A_multi_select_answer_joining_several_labels_is_a_choice()
    {
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", OneResultPart, """
            {"questions":[{"question":"Which fields?","header":"Fields","multiSelect":true,
                           "options":[{"label":"Abbreviation","description":"d1"},
                                      {"label":"ColorHex","description":"d2"},
                                      {"label":"Neither — keep it minimal","description":"d3"}]}],
             "answers":{"Which fields?":"Abbreviation, ColorHex"}}
            """)]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("Chose: Abbreviation, ColorHex", rec.Body);
        Assert.DoesNotContain("Typed:", rec.Body);
    }

    [Fact]
    public void Every_question_in_one_call_is_rendered_in_the_order_asked()
    {
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", OneResultPart, """
            {"questions":[{"question":"First?","header":"A","multiSelect":false,"options":[{"label":"Yes","description":"d"}]},
                          {"question":"Second?","header":"B","multiSelect":false,"options":[{"label":"No","description":"d"}]}],
             "answers":{"Second?":"No","First?":"Yes"}}
            """)]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("[AskUserQuestion — 2 answered]", rec.Body);
        Assert.True(rec.Body.IndexOf("Q: First?", StringComparison.Ordinal) <
                    rec.Body.IndexOf("Q: Second?", StringComparison.Ordinal),
            "questions must follow the order they were asked, not the answer dict's order");
    }

    [Fact]
    public void A_rejection_reason_is_kept_as_the_authors_own_words()
    {
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", """
            [{"type":"tool_result","tool_use_id":"t1","content":"The user doesn't want to proceed with this tool use. The tool use was rejected (eg. if it was a file edit, the new_string was NOT written to the file). The user provided the following reason for the rejection:  Actually don't do F4 (I need to redesign that pipeline anyway)"}]
            """, "\"User rejected tool use\"")]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("[Rejected by user]", rec.Body);
        Assert.Contains("Typed: Actually don't do F4 (I need to redesign that pipeline anyway)", rec.Body);
        Assert.DoesNotContain("STOP what you are doing", rec.Body);
    }

    [Fact]
    public void A_rejection_without_a_typed_reason_records_only_the_verdict()
    {
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", """
            [{"type":"tool_result","tool_use_id":"t1","content":"The user doesn't want to proceed with this tool use. The tool use was rejected (eg. if it was a file edit, the new_string was NOT written to the file). STOP what you are doing and wait for the user to tell you how to proceed."}]
            """, "\"User rejected tool use\"")]);

        var rec = Assert.Single(session.Records);
        Assert.Equal("[Rejected by user]", rec.Body);
    }

    [Fact]
    public void A_plan_is_stored_on_the_assistant_record_and_the_user_record_keeps_only_the_verdict()
    {
        const string plan = "# Plan: cut the coverage suggestion\n\nStep one, step two.";
        var session = CodeSessionExtractor.Extract([
            Assistant("a1", $$"""
                [{"type":"tool_use","id":"t1","name":"ExitPlanMode","input":{"plan":{{System.Text.Json.JsonSerializer.Serialize(plan)}} } }]
                """),
            UserWithResult("u1", OneResultPart, $$"""{"plan":{{System.Text.Json.JsonSerializer.Serialize(plan)}} }""")
        ]);

        var assistant = session.Records.Single(r => r.Uuid == "a1");
        var user = session.Records.Single(r => r.Uuid == "u1");

        Assert.Contains("[tool_use: ExitPlanMode]", assistant.Body);
        Assert.Contains("cut the coverage suggestion", assistant.Body);
        Assert.Equal("[Plan approved by user]", user.Body);
        // Claude-authored prose must not land on a user-role record — that would pollute the
        // "the author's own framing" query this feature exists to serve.
        Assert.DoesNotContain("Step one", user.Body);
        Assert.Equal(1, session.PlanSnapshots);
        Assert.Equal(0, session.PlanDrift);
    }

    [Fact]
    public void A_rejected_plan_is_still_stored_because_it_survives_nowhere_else()
    {
        var session = CodeSessionExtractor.Extract([
            Assistant("a1", """
                [{"type":"tool_use","id":"t1","name":"ExitPlanMode","input":{"plan":"# Abandoned proposal\n\nThe approach that was never taken."}}]
                """),
            UserWithResult("u1", """
                [{"type":"tool_result","tool_use_id":"t1","content":"The user doesn't want to proceed with this tool use. The tool use was rejected (eg. if it was a file edit, the new_string was NOT written to the file)."}]
                """, "\"User rejected tool use\"")
        ]);

        Assert.Contains("The approach that was never taken.", session.Records.Single(r => r.Uuid == "a1").Body);
        Assert.Equal("[Rejected by user]", session.Records.Single(r => r.Uuid == "u1").Body);
        Assert.Equal(1, session.PlanSnapshots);
    }

    [Fact]
    public void A_plan_that_changed_between_proposal_and_approval_is_counted_as_drift()
    {
        var session = CodeSessionExtractor.Extract([
            Assistant("a1", """[{"type":"tool_use","id":"t1","name":"ExitPlanMode","input":{"plan":"proposed text"}}]"""),
            UserWithResult("u1", OneResultPart, """{"plan":"approved text, differing"}""")
        ]);

        Assert.Equal(1, session.PlanDrift);

        // BOTH texts are kept, on the proposing record. A plan is revised between the call and
        // the approval often enough to matter, and the approved text is the one actually agreed
        // to — storing only the proposal would quietly archive a plan nobody accepted.
        var assistant = session.Records.Single(r => r.Uuid == "a1").Body;
        Assert.Contains("proposed text", assistant);
        Assert.Contains("[Plan as approved — differs from the proposal above]", assistant);
        Assert.Contains("approved text, differing", assistant);

        // ...and still nothing Claude-authored on the user-role record.
        Assert.Equal("[Plan approved by user]", session.Records.Single(r => r.Uuid == "u1").Body);
    }

    [Theory]
    // A sidecar that is a bare string, not the {questions, answers} object.
    [InlineData("\"InputValidationError: JSON parse failed (6950 bytes)\"")]
    // The malformed-call shape: an input that never parsed into questions at all.
    [InlineData("""{"questions":[],"answers":{}}""")]
    // An answer dict keyed by text that matches no question asked.
    [InlineData("""{"questions":[{"question":"Asked?","header":"H","multiSelect":false,"options":[]}],"answers":{"A different question":"x"}}""")]
    // Not a question result at all — an ordinary tool's structured payload.
    [InlineData("""{"filenames":["a.cs"],"numFiles":1}""")]
    public void A_result_that_carries_no_human_words_falls_back_to_the_ordinary_elision(string sidecar)
    {
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", OneResultPart, sidecar)]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("[tool result elided —", rec.Body);
        Assert.Equal(0, session.HumanResults);
    }

    [Fact]
    public void A_second_tool_result_in_one_record_does_not_reuse_the_first_ones_sidecar()
    {
        // The sidecar describes ONE result. Applying it twice would attribute an answer to a
        // tool call that never asked anything.
        var session = CodeSessionExtractor.Extract([UserWithResult("u1", """
            [{"type":"tool_result","tool_use_id":"t1","content":"first"},
             {"type":"tool_result","tool_use_id":"t2","content":"second"}]
            """, """
            {"questions":[{"question":"Which?","header":"H","multiSelect":false,"options":[{"label":"A","description":"d"}]}],
             "answers":{"Which?":"A"}}
            """)]);

        var rec = Assert.Single(session.Records);
        Assert.Contains("Chose: A", rec.Body);
        Assert.Contains("[tool result elided —", rec.Body);
        Assert.Equal(1, session.HumanResults);
    }

    [Fact]
    public void An_ordinary_tool_call_is_unaffected_by_the_ExitPlanMode_special_case()
    {
        var session = CodeSessionExtractor.Extract([Assistant("a1",
            """[{"type":"tool_use","id":"t1","name":"Read","input":{"file_path":"WorldDateModel.cs"}}]""")]);

        Assert.Equal("[tool_use: Read — WorldDateModel.cs]", Assert.Single(session.Records).Body);
        Assert.Equal(0, session.PlanSnapshots);
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
