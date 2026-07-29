using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using Xunit;

using StoryPlanner.Mcp;

namespace StoryPlanner.Tests;

/// <summary>
/// Schema truth: facts about the data that Claude cannot know and would otherwise
/// re-derive incorrectly. These are the things the server owns precisely because
/// getting them wrong is silent and consequential.
/// </summary>
public class SchemaTruthTests
{
    // ── Per-file NoteState semantics ────────────────────────────────────────
    // The same enum value means different things in the two files. v2 Confirmed =
    // stable. v1 Confirmed = review closed, and whether the content was migrated to
    // v2 or deliberately superseded was NOT recorded. Emitting "Confirmed" for an
    // archive note asserts something the data does not support.

    [Fact]
    public void Working_confirmed_reads_as_confirmed()
    {
        Assert.Equal("confirmed", Query.StateLabel(Corpus.Working, NoteState.Confirmed));
        Assert.Equal("unset", Query.StateLabel(Corpus.Working, NoteState.Unset));
        Assert.Equal("flagged", Query.StateLabel(Corpus.Working, NoteState.Flagged));
    }

    [Fact]
    public void Archive_confirmed_never_reads_as_confirmed_and_records_the_ambiguity()
    {
        var label = Query.StateLabel(Corpus.Archive, NoteState.Confirmed);

        Assert.DoesNotContain("confirmed", label, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("closed", label);
        Assert.Contains("disposition-not-recorded", label);
    }

    [Fact]
    public void Archive_unset_reads_as_open_not_unset()
    {
        Assert.Equal("open", Query.StateLabel(Corpus.Archive, NoteState.Unset));
    }

    [Fact]
    public void Archive_tools_never_emit_the_word_confirmed()
    {
        using var plan = SyntheticPlan.Create();
        var archive = new ArchiveTools(plan.Sources);

        // PlotPointNoteId is stored with NoteState.Confirmed.
        var result = archive.GetNotesArchive([SyntheticPlan.PlotPointNoteId]);

        Assert.DoesNotContain("confirmed", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("closed(disposition-not-recorded)", result);
    }

    // ── WorldDate: mechanical parse, never a guess ──────────────────────────

    [Theory]
    [InlineData("993", 993, 993)]
    [InlineData("1007", 1007, 1007)]
    [InlineData("-131", -131, -131)]
    [InlineData("870-928", 870, 928)]
    [InlineData("-100-0", -100, 0)]
    [InlineData("0-100", 0, 100)]
    [InlineData("  993  ", 993, 993)]
    public void WorldDate_parses_years_and_ranges_including_negatives(string raw, int start, int end)
    {
        var (s, e, parsed) = Query.ParseWorldDate(raw);

        Assert.True(parsed);
        Assert.Equal(start, s);
        Assert.Equal(end, e);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("sometime after the war")]
    [InlineData("early spring")]
    [InlineData("993ish")]
    public void WorldDate_flags_unparseable_values_rather_than_guessing(string raw)
    {
        var (s, e, parsed) = Query.ParseWorldDate(raw);

        Assert.False(parsed);
        Assert.Equal(0, s);
        Assert.Equal(0, e);
    }

    [Fact]
    public void WorldDate_label_marks_unparsed_values_visibly()
    {
        Assert.Contains("(unparsed)", Query.WorldDateLabel("sometime after the war"));
        Assert.DoesNotContain("(unparsed)", Query.WorldDateLabel("993"));
    }

    [Fact]
    public void Chronology_counts_unparseable_dates_instead_of_dropping_them_silently()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetNotesInDateRangePlan();

        Assert.Contains("unparseable WorldDate values: 1", result);
    }

    [Fact]
    public void Chronology_range_filter_respects_parsed_bounds()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var inRange = tools.GetNotesInDateRangePlan(fromYear: 990, toYear: 995);
        var outOfRange = tools.GetNotesInDateRangePlan(fromYear: 1, toYear: 100);

        Assert.Contains($"note:{SyntheticPlan.VisibleNoteId}", inRange);   // WorldDate 993
        Assert.DoesNotContain($"note:{SyntheticPlan.VisibleNoteId}", outOfRange);
    }

    // ── Polymorphic owner resolution (no FKs exist to do this for us) ────────

    [Fact]
    public void All_four_owner_types_resolve_to_labels()
    {
        using var plan = SyntheticPlan.Create();
        var cache = plan.Sources.Get(Corpus.Working);

        Assert.Contains("Testcharacter", Query.OwnerLabel(cache, OwnerType.Subject, SyntheticPlan.SubjectId));
        Assert.Contains("Testscene", Query.OwnerLabel(cache, OwnerType.PlotPoint, SyntheticPlan.PlotPointId));
        Assert.Contains("Testchapter", Query.OwnerLabel(cache, OwnerType.Chapter, SyntheticPlan.ChapterId));

        var linkLabel = Query.OwnerLabel(cache, OwnerType.PlotPointSubjectLink, SyntheticPlan.LinkId);
        Assert.Contains("Testscene", linkLabel);
        Assert.Contains("Testcharacter", linkLabel);
    }

    [Fact]
    public void A_dangling_owner_id_is_reported_not_silently_blank()
    {
        using var plan = SyntheticPlan.Create();
        var cache = plan.Sources.Get(Corpus.Working);

        // No FK constraint exists to prevent this in the real schema, so the tool must cope.
        var label = Query.OwnerLabel(cache, OwnerType.Subject, 999999);

        Assert.Contains("missing", label);
    }

    [Fact]
    public void Plot_point_label_carries_its_chapter_position()
    {
        using var plan = SyntheticPlan.Create();
        var cache = plan.Sources.Get(Corpus.Working);

        var label = Query.OwnerLabel(cache, OwnerType.PlotPoint, SyntheticPlan.PlotPointId);

        Assert.Contains("CH#1", label);
    }

    // ── Track metadata (pass 3) ─────────────────────────────────────────────

    [Fact]
    public void Track_definitions_carry_the_question_and_cognitive_mode()
    {
        using var plan = SyntheticPlan.Create();
        var reference = new ReferenceTools(plan.Sources);

        var result = reference.GetTrackDefinitions(["Backstory"]);

        Assert.Contains("What is this character's history?", result);
        Assert.Contains("Revelations should draw from here.", result);
        Assert.Contains("in-universe historian", result); // TrackType.History cognitive mode
    }

    [Fact]
    public void Untracked_notes_are_labeled_not_dropped()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        // The chapter note has no NoteTrackDefinitionId.
        var result = tools.GetNotesPlan([SyntheticPlan.ChapterNoteId]);

        Assert.Contains("(untracked)", result);
    }
}
