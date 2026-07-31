using StoryPlanner.Core;
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
    // Legacy free-text values are converted on read via Core's WorldDateLegacy (shared with
    // the DataOps conversion op) until a file has been converted; these prove the fallback.

    [Theory]
    [InlineData("993", 993, 993)]
    [InlineData("1007", 1007, 1007)]
    [InlineData("-131", -131, -131)]
    [InlineData("870-928", 870, 928)]
    [InlineData("-100-0", -100, 0)]
    [InlineData("0-100", 0, 100)]
    [InlineData("  993  ", 993, 993)]
    public void Legacy_WorldDate_reads_years_and_ranges_including_negatives(string raw, int start, int end)
    {
        var date = Query.EffectiveWorldDate(new Note { WorldDate = raw });

        Assert.NotNull(date);
        Assert.Equal(start, date!.Value.Start!.Value.Year);
        Assert.Equal(end, (date.Value.End ?? date.Value.Start)!.Value.Year);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("sometime after the war")]
    [InlineData("early spring")]
    [InlineData("993ish")]
    public void Legacy_WorldDate_flags_unparseable_values_rather_than_guessing(string raw)
    {
        Assert.Null(Query.EffectiveWorldDate(new Note { WorldDate = raw }));
    }

    [Fact]
    public void Structured_date_wins_over_legacy_text_when_both_exist()
    {
        var note = new Note { WorldDate = "993", WorldDateStartYear = 1007 };
        Assert.Equal(1007, Query.EffectiveWorldDate(note)!.Value.Start!.Value.Year);
    }

    [Fact]
    public void WorldDate_label_marks_unparsed_values_visibly()
    {
        Assert.Contains("(unparsed)", Query.WorldDateLabel(new Note { WorldDate = "sometime after the war" }));
        Assert.DoesNotContain("(unparsed)", Query.WorldDateLabel(new Note { WorldDate = "993" }));
        Assert.Equal("", Query.WorldDateLabel(new Note()));
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

    // ── Source material: two-tier Work/Part, a note may cite several Parts ─────

    [Fact]
    public void SourceLabel_renders_every_citation_not_just_the_first()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" });
            ctx.SourceMaterialParts.AddRange(
                new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" },
                new SourceMaterialPart { Id = 2, SourceMaterialId = 1, Code = "S3E02" });
            ctx.NoteSourceReferences.AddRange(
                new NoteSourceReference { NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1, SourceMaterialPartId = 1, SortOrder = 0 },
                new NoteSourceReference { NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1, SourceMaterialPartId = 2, SortOrder = 1 });
        });
        var cache = plan.Sources.Get(Corpus.Working);
        var note = cache.Notes.Single(n => n.Id == SyntheticPlan.VisibleNoteId);

        Assert.Equal("source:MLP:FiM·S3E01,MLP:FiM·S3E02", Query.SourceLabel(cache, note));
    }

    [Fact]
    public void SourceLabel_is_empty_when_a_note_has_no_citations()
    {
        using var plan = SyntheticPlan.Create();
        var cache = plan.Sources.Get(Corpus.Working);
        var note = cache.Notes.Single(n => n.Id == SyntheticPlan.VisibleNoteId);

        Assert.Equal("", Query.SourceLabel(cache, note));
    }

    [Fact]
    public void ListSourceMaterials_reports_untouched_parts_as_negative_space()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM", PartNoun = "Episode" });
            ctx.SourceMaterialParts.AddRange(
                new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" },     // cited
                new SourceMaterialPart { Id = 2, SourceMaterialId = 1, Code = "S3E02", ReviewState = SourcePartReviewState.Reviewed }, // reviewed, 0 cites -> NOT untouched
                new SourceMaterialPart { Id = 3, SourceMaterialId = 1, Code = "S3E03" });     // untouched
            ctx.NoteSourceReferences.Add(new NoteSourceReference
            {
                NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1, SourceMaterialPartId = 1
            });
        });
        var reference = new ReferenceTools(plan.Sources);

        var result = reference.ListSourceMaterials();

        Assert.Contains("1 untouched", result); // only S3E03
        Assert.Contains("S3E01: 1 note(s)", result);
        Assert.Contains("S3E02: 0 note(s), reviewed", result);
        Assert.Contains("S3E03: 0 note(s)", result);
        Assert.Contains("S3E03", result.Split('\n').Single(l => l.Contains("<- untouched")));
    }

    // ── Focal character (POV) ────────────────────────────────────────────────

    [Fact]
    public void GetPlotPoints_emits_the_focal_character_when_designated()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.Subjects.Single(s => s.Id == SyntheticPlan.SubjectId).IsPovCharacter = true;
            ctx.PlotPoints.Single(p => p.Id == SyntheticPlan.PlotPointId).FocalCharacterId = SyntheticPlan.SubjectId;
        });
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetPlotPointsPlan([SyntheticPlan.PlotPointId]);

        Assert.Contains($"focal: Testcharacter (subject:{SyntheticPlan.SubjectId})", result);
    }

    [Fact]
    public void GetPlotPoints_omits_the_focal_line_when_undesignated()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetPlotPointsPlan([SyntheticPlan.PlotPointId]);

        Assert.DoesNotContain("focal:", result);
    }

    [Fact]
    public void GetTrackDefinitions_reports_the_focalCharacterOnly_flag()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
            ctx.NoteTrackDefinitions.Single(t => t.Id == SyntheticPlan.LinkTrackId).IsFocalCharacterOnly = true);
        var reference = new ReferenceTools(plan.Sources);

        var result = reference.GetTrackDefinitions(["Revelation"]);

        Assert.Contains("focalCharacterOnly", result);
    }

    [Fact]
    public void Count_notes_by_source_groups_multi_cite_notes_by_their_full_citation_set()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" });
            ctx.SourceMaterialParts.Add(new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" });
            ctx.NoteSourceReferences.Add(new NoteSourceReference
            {
                NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1, SourceMaterialPartId = 1
            });
        });
        var tools = new PlanTools(plan.Sources);

        var result = tools.CountNotesPlan(["source"]);

        Assert.Contains("MLP:FiM·S3E01 | 1", result);
        Assert.Contains("(no source)", result); // every other note in the fixture
    }
}
