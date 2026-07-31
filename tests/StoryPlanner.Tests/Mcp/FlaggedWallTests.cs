using StoryPlanner.Core;
using Xunit;

using StoryPlanner.Mcp;

namespace StoryPlanner.Tests;

/// <summary>
/// The flagged wall is the server's central epistemic guarantee: flagged notes are open
/// questions, not settled lore, so their content and FlagReason must never reach an LLM
/// through an ordinary tool. Existence is disclosed as counts; content requires a
/// deliberate call to the flagged family. Same rule as NoteExportRenderer.cs:28.
/// </summary>
public class FlaggedWallTests
{
    [Fact]
    public void Search_never_returns_a_flagged_notes_body_or_reason()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var byContent = tools.SearchPlan(SyntheticPlan.FlaggedContentSecret);
        var byReason = tools.SearchPlan(SyntheticPlan.FlaggedReasonSecret);

        // No snippet escapes: the text surrounding the match is never emitted...
        Assert.DoesNotContain(SyntheticPlan.FlaggedContentEnvelope, byContent);
        Assert.DoesNotContain(SyntheticPlan.FlaggedReasonEnvelope, byReason);

        // ...and no hit line is produced for the flagged note (id is a trailing parenthetical
        // in the current output format — see Query.OwnerLabel/OwnerRef).
        Assert.DoesNotContain($"(note:{SyntheticPlan.FlaggedNoteId})", byContent);
        Assert.DoesNotContain($"(note:{SyntheticPlan.FlaggedNoteId})", byReason);
        Assert.Contains("notes 0", byContent); // zero ordinary note hits
    }

    [Fact]
    public void Search_discloses_walled_matches_as_a_count()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        // Matches only a flagged note's content — the hit must be counted, not shown.
        var result = tools.SearchPlan(SyntheticPlan.FlaggedContentSecret);

        Assert.Contains("walled", result);
        Assert.Contains("list_open_questions", result);
    }

    [Fact]
    public void Search_still_returns_ordinary_notes()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.SearchPlan(SyntheticPlan.VisibleSecret);

        Assert.Contains($"note:{SyntheticPlan.VisibleNoteId}", result);
        Assert.Contains(SyntheticPlan.VisibleSecret, result);
    }

    [Fact]
    public void Fetch_by_id_returns_a_stub_for_a_flagged_note_never_content()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetNotesPlan([SyntheticPlan.FlaggedNoteId]);

        Assert.DoesNotContain(SyntheticPlan.FlaggedContentSecret, result);
        Assert.DoesNotContain(SyntheticPlan.FlaggedReasonSecret, result);
        Assert.Contains("FLAGGED", result);
        Assert.Contains("get_open_questions", result);
    }

    [Fact]
    public void Fetch_by_id_distinguishes_flagged_from_missing()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetNotesPlan([SyntheticPlan.VisibleNoteId, SyntheticPlan.FlaggedNoteId, 999999]);

        Assert.Contains("1 returned", result);
        Assert.Contains("1 flagged (walled)", result);
        Assert.Contains("1 not found", result);
    }

    [Fact]
    public void Subject_fetch_discloses_flagged_tally_without_leaking_content()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetSubjectsPlan([SyntheticPlan.SubjectId]);

        Assert.DoesNotContain(SyntheticPlan.FlaggedContentSecret, result);
        Assert.DoesNotContain(SyntheticPlan.FlaggedReasonSecret, result);
        Assert.DoesNotContain(SyntheticPlan.FlaggedContentEnvelope, result);
        Assert.Contains("2 flagged (walled", result);   // the tally IS disclosed
        Assert.Contains(SyntheticPlan.VisibleSecret, result); // ordinary notes still returned
    }

    [Fact]
    public void Flagged_family_is_the_one_door_that_opens()
    {
        using var plan = SyntheticPlan.Create();
        var flagged = new FlaggedTools(plan.Sources);

        var index = flagged.ListOpenQuestions();
        var detail = flagged.GetOpenQuestions([SyntheticPlan.FlaggedNoteId]);

        Assert.Contains(SyntheticPlan.FlaggedReasonSecret, index);   // reason in the compact index
        Assert.Contains(SyntheticPlan.FlaggedContentSecret, detail); // full content on demand
        Assert.Contains(SyntheticPlan.FlaggedReasonSecret, detail);
    }

    [Fact]
    public void Flagged_family_searches_flag_reason_text()
    {
        using var plan = SyntheticPlan.Create();
        var flagged = new FlaggedTools(plan.Sources);

        // FlagReason is a lore corpus in its own right; it is regex-searchable HERE and nowhere else.
        var result = flagged.ListOpenQuestions(pattern: SyntheticPlan.FlaggedReasonSecret);

        Assert.Contains($"q:{SyntheticPlan.FlaggedNoteId}", result);
        Assert.Contains("1 after filters", result);
    }

    [Fact]
    public void Flagged_family_refuses_a_non_flagged_id_rather_than_serving_it()
    {
        using var plan = SyntheticPlan.Create();
        var flagged = new FlaggedTools(plan.Sources);

        var result = flagged.GetOpenQuestions([SyntheticPlan.VisibleNoteId]);

        Assert.Contains("not flagged", result);
        Assert.DoesNotContain(SyntheticPlan.VisibleSecret, result);
    }

    [Fact]
    public void Pure_question_flagged_notes_are_labeled_not_silently_empty()
    {
        using var plan = SyntheticPlan.Create();
        var flagged = new FlaggedTools(plan.Sources);

        var result = flagged.GetOpenQuestions([SyntheticPlan.FlaggedNoteId2]);

        Assert.Contains("empty — pure question", result);
    }

    [Fact]
    public void Counts_include_flagged_because_a_count_is_not_content()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.CountNotesPlan(["state"]);

        Assert.Contains("flagged | 2", result);
        Assert.DoesNotContain(SyntheticPlan.FlaggedContentSecret, result);
    }
}
