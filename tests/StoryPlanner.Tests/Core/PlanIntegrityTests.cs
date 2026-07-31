using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// PlanIntegrity is the id-based extraction of the referential-integrity assumptions
/// ContentDeleter's guards encode ad hoc against view models — see the testing skill's
/// "Known gap" note. These tests are its first direct coverage: seed a fixture with rows
/// that deliberately violate the invariants no foreign key enforces, and confirm Check
/// reports them rather than silently returning plausible-looking wrong data.
/// </summary>
public class PlanIntegrityTests
{
    [Fact]
    public void Check_passes_clean_on_an_unmodified_synthetic_plan()
    {
        using var plan = SyntheticPlan.Create();
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Empty(violations);
    }

    [Fact]
    public void Check_reports_a_note_whose_owner_does_not_exist()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.Notes.Add(new Note
        {
            Id = 999, OwnerId = 424242, OwnerType = OwnerType.Subject,
            NoteState = NoteState.Unset, Content = "orphaned", SortOrder = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "note.owner_missing" && v.Detail.Contains("note:999"));
    }

    [Fact]
    public void Check_reports_a_link_whose_plot_point_or_subject_is_dangling()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPointSubjectLinks.Add(new PlotPointSubjectLink
        {
            Id = 999, PlotPointId = 424242, SubjectId = 424243
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "link.plotpoint_missing" && v.Detail.Contains("link:999"));
        Assert.Contains(violations, v => v.Rule == "link.subject_missing" && v.Detail.Contains("link:999"));
    }

    [Fact]
    public void Check_reports_a_plot_point_pointing_at_a_missing_chapter()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPoints.Add(new PlotPoint
        {
            Id = 999, Title = "Dangling", ChapterId = 424242, OrderInChapter = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "plotpoint.chapter_missing" && v.Detail.Contains("plotpoint:999"));
    }

    [Fact]
    public void Check_reports_a_plot_point_pointing_at_a_missing_focal_character()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPoints.Add(new PlotPoint
        {
            Id = 999, Title = "Dangling focal", FocalCharacterId = 424242, OrderInChapter = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "plotpoint.focal_character_missing" && v.Detail.Contains("plotpoint:999"));
    }

    [Fact]
    public void Check_passes_when_focal_character_is_null_or_resolves()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
            ctx.PlotPoints.Single(p => p.Id == SyntheticPlan.PlotPointId).FocalCharacterId = SyntheticPlan.SubjectId);
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.DoesNotContain(violations, v => v.Rule == "plotpoint.focal_character_missing");
    }

    [Fact]
    public void Check_reports_a_source_material_part_pointing_at_a_missing_work()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.SourceMaterialParts.Add(new SourceMaterialPart
        {
            Id = 999, SourceMaterialId = 424242, Code = "S1E01"
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "sourcepart.material_missing" && v.Detail.Contains("part:999"));
    }

    [Fact]
    public void Check_reports_a_note_source_reference_whose_note_or_material_is_dangling()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.NoteSourceReferences.Add(new NoteSourceReference
        {
            Id = 999, NoteId = 424242, SourceMaterialId = 424243
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "sourcereference.note_missing" && v.Detail.Contains("reference:999"));
        Assert.Contains(violations, v => v.Rule == "sourcereference.material_missing" && v.Detail.Contains("reference:999"));
    }

    [Fact]
    public void Check_reports_a_reference_whose_part_belongs_to_a_different_work()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.AddRange(
                new SourceMaterial { Id = 1, Name = "MLP:FiM" },
                new SourceMaterial { Id = 2, Name = "Equestria at War" });
            ctx.SourceMaterialParts.Add(new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" });
            // References material 2 but cites a Part that actually belongs to material 1 — the
            // invariant PlanIntegrity is meant to catch since nothing in the schema can enforce it.
            ctx.NoteSourceReferences.Add(new NoteSourceReference
            {
                Id = 999, NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 2, SourceMaterialPartId = 1
            });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "sourcereference.part_parent_mismatch" && v.Detail.Contains("reference:999"));
    }

    [Fact]
    public void Check_reports_a_second_narrative_property_value_for_the_same_owner_and_property()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Test Axis"
            });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "Pole A" },
                new NarrativePropertyValueDefinition { Id = 2, NarrativePropertyDefinitionId = 1, ValueName = "Pole B" });
            // Both poles assigned to the same subject — narrative properties are single-select and
            // nothing in the schema can say so, which is exactly why PlanIntegrity has to.
            ctx.NarrativePropertyValues.AddRange(
                new NarrativePropertyValue { Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 1 },
                new NarrativePropertyValue { Id = 2, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 2 });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "narrativevalue.duplicate_for_property"
                                      && v.Detail.Contains("property:1"));
    }

    [Fact]
    public void Check_does_not_confuse_a_subject_and_a_chapter_that_share_an_id()
    {
        // NarrativePropertyValue has no OwnerType column, so an OwnerId-only predicate would read
        // these two assignments as a duplicate on one owner. The fixture's SubjectId and ChapterId
        // are both 1, which is what makes this reproducible.
        Assert.Equal(SyntheticPlan.SubjectId, SyntheticPlan.ChapterId);

        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.AddRange(
                new NarrativePropertyDefinition
                {
                    Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.Subject, Name = "Subject Axis"
                },
                new NarrativePropertyDefinition
                {
                    Id = 2, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.Chapter, Name = "Chapter Axis"
                });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "Subject value" },
                new NarrativePropertyValueDefinition { Id = 2, NarrativePropertyDefinitionId = 2, ValueName = "Chapter value" });
            ctx.NarrativePropertyValues.AddRange(
                new NarrativePropertyValue { Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 1 },
                new NarrativePropertyValue { Id = 2, OwnerId = SyntheticPlan.ChapterId, ValueDefinitionId = 2 });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.DoesNotContain(violations, v => v.Rule == "narrativevalue.duplicate_for_property");
        Assert.DoesNotContain(violations, v => v.Rule == "narrativevalue.owner_missing");
    }

    [Fact]
    public void Check_reports_a_property_definition_gating_on_a_missing_work_phase()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
        {
            Id = 999, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
            OwnerType = OwnerType.Subject, Name = "Dangling gate", GatingWorkPhaseId = 424242
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "narrativepropertydefinition.workphase_missing"
                                      && v.Detail.Contains("property:999"));
    }

    [Fact]
    public void Check_passes_when_a_property_gates_on_a_work_phase_that_exists()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.WorkPhases.Add(new WorkPhase
            {
                Id = 1, Name = "Expansion", DisplayOrder = 1, RequiresZeroFlaggedNotes = true
            });
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Gated", GatingWorkPhaseId = 1
            });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.DoesNotContain(violations, v => v.Rule == "narrativepropertydefinition.workphase_missing");
    }

    [Fact]
    public void ComputeNoteChecksum_is_stable_across_reads_and_changes_when_content_changes()
    {
        using var plan = SyntheticPlan.Create();
        using var ctx1 = OpenContext(plan.Path);
        using var ctx2 = OpenContext(plan.Path);

        Assert.Equal(PlanIntegrity.ComputeNoteChecksum(ctx1), PlanIntegrity.ComputeNoteChecksum(ctx2));

        var before = PlanIntegrity.ComputeNoteChecksum(ctx1);
        plan.ExternalWrite(ctx =>
        {
            var note = ctx.Notes.First(n => n.Id == SyntheticPlan.VisibleNoteId);
            note.Content += " changed";
        });
        using var ctx3 = OpenContext(plan.Path);
        var after = PlanIntegrity.ComputeNoteChecksum(ctx3);

        Assert.NotEqual(before, after);
    }

    [Fact]
    public void CompareRowCounts_flags_only_tables_outside_the_allowed_set()
    {
        var before = new Dictionary<string, long> { ["Chapters"] = 1, ["Notes"] = 5 };
        var afterGrew = new Dictionary<string, long> { ["Chapters"] = 2, ["Notes"] = 5 };

        var violations = PlanIntegrity.CompareRowCounts(before, afterGrew, allowedToChange: new HashSet<string> { "Chapters" });
        Assert.Empty(violations);

        var violationsUnexpected = PlanIntegrity.CompareRowCounts(before, afterGrew, allowedToChange: new HashSet<string>());
        Assert.Single(violationsUnexpected);
        Assert.Equal("rowcount.changed", violationsUnexpected[0].Rule);
    }

    private static AppDbContext OpenContext(string path) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={path}").Options);
}
