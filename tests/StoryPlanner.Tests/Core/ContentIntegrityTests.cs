using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// ContentIntegrity is the id-based extraction of ContentDeleter's SourceMaterial/Part delete
/// guards — the testing skill's "Known gap" note asks for exactly this shape (predicates over
/// IStoryService and ids, not view models) so the guard logic is reachable without standing up
/// the WPF view-model graph. Opens the fixture through the real StoryService, per the testing
/// skill's "don't mock IStoryService" rule.
/// </summary>
public class ContentIntegrityTests
{
    [Fact]
    public async Task SourceMaterialHasDependents_is_false_for_an_uncited_work_with_no_parts()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" }));
        var svc = await plan.OpenStoryServiceAsync();

        Assert.False(ContentIntegrity.SourceMaterialHasDependents(svc, 1));
    }

    [Fact]
    public async Task SourceMaterialHasDependents_is_true_when_a_part_exists_under_it()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" });
            ctx.SourceMaterialParts.Add(new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.SourceMaterialHasDependents(svc, 1));
    }

    [Fact]
    public async Task SourceMaterialHasDependents_is_true_when_a_note_cites_the_work_directly()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "Equestria at War" });
            // No Part — a note citing the Work as a whole (SourceMaterialPartId null).
            ctx.NoteSourceReferences.Add(new NoteSourceReference
            {
                Id = 1, NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.SourceMaterialHasDependents(svc, 1));
    }

    [Fact]
    public async Task SourceMaterialPartHasReferences_is_false_for_an_uncited_part()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" });
            ctx.SourceMaterialParts.Add(new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.False(ContentIntegrity.SourceMaterialPartHasReferences(svc, 1));
    }

    [Fact]
    public async Task SourceMaterialPartHasReferences_is_true_when_a_note_cites_it()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" });
            ctx.SourceMaterialParts.Add(new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" });
            ctx.NoteSourceReferences.Add(new NoteSourceReference
            {
                Id = 1, NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1, SourceMaterialPartId = 1
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.SourceMaterialPartHasReferences(svc, 1));
    }

    [Fact]
    public async Task SourceMaterialPartHasReferences_does_not_confuse_a_citation_of_a_sibling_part()
    {
        // Note 696's shape: several notes citing DIFFERENT Parts of the same Work. Citing one
        // Part must not make an uncited sibling Part look cited.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.Add(new SourceMaterial { Id = 1, Name = "MLP:FiM" });
            ctx.SourceMaterialParts.AddRange(
                new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S1E01" },
                new SourceMaterialPart { Id = 2, SourceMaterialId = 1, Code = "S1E02" });
            ctx.NoteSourceReferences.Add(new NoteSourceReference
            {
                Id = 1, NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 1, SourceMaterialPartId = 1
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.SourceMaterialPartHasReferences(svc, 1));
        Assert.False(ContentIntegrity.SourceMaterialPartHasReferences(svc, 2));
    }

    // ── Narrative property / work phase guards ────────────────────────────────

    [Fact]
    public async Task WorkPhaseHasDependents_is_true_only_while_a_property_gates_on_it()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.WorkPhases.AddRange(
                new WorkPhase { Id = 1, Name = "Expansion", DisplayOrder = 1 },
                new WorkPhase { Id = 2, Name = "Audit", DisplayOrder = 2 });
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Gated", GatingWorkPhaseId = 2
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.False(ContentIntegrity.WorkPhaseHasDependents(svc, 1));
        Assert.True(ContentIntegrity.WorkPhaseHasDependents(svc, 2));
    }

    [Fact]
    public async Task NarrativePropertyDefinitionHasDependents_is_true_once_any_owner_assigns_a_value()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.AddRange(
                new NarrativePropertyDefinition
                {
                    Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.Subject, Name = "Assigned"
                },
                new NarrativePropertyDefinition
                {
                    Id = 2, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.Subject, Name = "Untouched"
                });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "Pole A" },
                new NarrativePropertyValueDefinition { Id = 2, NarrativePropertyDefinitionId = 2, ValueName = "Pole A" });
            ctx.NarrativePropertyValues.Add(new NarrativePropertyValue
            {
                Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 1
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.NarrativePropertyDefinitionHasDependents(svc, 1));
        // A property whose values nobody has picked is free to delete — and its sibling's
        // assignment must not make it look used.
        Assert.False(ContentIntegrity.NarrativePropertyDefinitionHasDependents(svc, 2));
    }

    [Fact]
    public async Task NarrativePropertyValueDefinitionHasAssignments_distinguishes_siblings()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Axis"
            });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "Pole A" },
                new NarrativePropertyValueDefinition { Id = 2, NarrativePropertyDefinitionId = 1, ValueName = "Pole B" });
            ctx.NarrativePropertyValues.Add(new NarrativePropertyValue
            {
                Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 1
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.NarrativePropertyValueDefinitionHasAssignments(svc, 1));
        Assert.False(ContentIntegrity.NarrativePropertyValueDefinitionHasAssignments(svc, 2));
    }

    // ── Guards added 2026-08-02 for the delete paths that used to bypass ContentDeleter ──

    [Fact]
    public async Task HasNotes_resolves_the_polymorphic_pair_not_just_the_id()
    {
        using var plan = SyntheticPlan.Create();
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.HasNotes(svc, SyntheticPlan.SubjectId, OwnerType.Subject));
        Assert.False(ContentIntegrity.HasNotes(svc, SyntheticPlan.EmptySubjectId, OwnerType.Subject));
        // Same numeric id, different owner type — the pair is the join, not the id.
        Assert.True(ContentIntegrity.HasNotes(svc, SyntheticPlan.PlotPointId, OwnerType.PlotPoint));
    }

    [Fact]
    public async Task ThemeHasNotes_is_true_only_for_the_theme_a_note_is_tagged_with()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.Themes.AddRange(
                new Theme { Id = 1, Name = "Tagged" },
                new Theme { Id = 2, Name = "Untagged" });
            ctx.Notes.Find(SyntheticPlan.VisibleNoteId)!.ThemeId = 1;
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.ThemeHasNotes(svc, 1));
        Assert.False(ContentIntegrity.ThemeHasNotes(svc, 2));
    }

    [Fact]
    public async Task SubjectDefinitionHasDependents_sees_subjects_tracks_and_properties()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            // 90: nothing hangs off it. 91: only a track definition. 92: only a property definition.
            ctx.SubjectDefinitions.AddRange(
                new SubjectDefinition { Id = 90, SubjectType = "Free", DisplayOrder = 90 },
                new SubjectDefinition { Id = 91, SubjectType = "TrackScoped", DisplayOrder = 91 },
                new SubjectDefinition { Id = 92, SubjectType = "PropertyScoped", DisplayOrder = 92 });
            ctx.NoteTrackDefinitions.Add(new NoteTrackDefinition
            {
                Id = 91, SubjectDefinitionId = 91, OwnerType = OwnerType.Subject, TrackName = "Scoped"
            });
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 92, SubjectDefinitionId = 92, OwnerType = OwnerType.Subject, Name = "Scoped"
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        // The baseline definition types real subjects — the common refusal case.
        Assert.True(ContentIntegrity.SubjectDefinitionHasDependents(svc, SyntheticPlan.CharacterDefId));
        Assert.False(ContentIntegrity.SubjectDefinitionHasDependents(svc, 90));
        Assert.True(ContentIntegrity.SubjectDefinitionHasDependents(svc, 91));
        Assert.True(ContentIntegrity.SubjectDefinitionHasDependents(svc, 92));
    }

    [Fact]
    public async Task NoteTrackDefinitionHasNotes_distinguishes_a_used_track_from_an_empty_one()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.NoteTrackDefinitions.Add(new NoteTrackDefinition
        {
            Id = 90, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
            OwnerType = OwnerType.Subject, TrackName = "Never used"
        }));
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.NoteTrackDefinitionHasNotes(svc, SyntheticPlan.BackstoryTrackId));
        Assert.False(ContentIntegrity.NoteTrackDefinitionHasNotes(svc, 90));
    }

    [Fact]
    public async Task SubjectRelationDefinitionHasAssignments_distinguishes_a_used_relation_from_an_empty_one()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectRelationDefinitions.AddRange(
                new SubjectRelationDefinition
                {
                    Id = 1, Name = "Ancestor",
                    SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    TargetSubjectDefinitionId = SyntheticPlan.CharacterDefId
                },
                new SubjectRelationDefinition
                {
                    Id = 2, Name = "Never drawn",
                    SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    TargetSubjectDefinitionId = SyntheticPlan.CharacterDefId
                });
            ctx.SubjectRelations.Add(new SubjectRelation
            {
                Id = 1, RelationDefinitionId = 1,
                SubjectId = SyntheticPlan.SubjectId, TargetSubjectId = SyntheticPlan.EmptySubjectId
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.SubjectRelationDefinitionHasAssignments(svc, 1));
        Assert.False(ContentIntegrity.SubjectRelationDefinitionHasAssignments(svc, 2));
    }

    [Fact]
    public async Task PropertyBoardHasMembers_distinguishes_a_populated_board_from_an_empty_one()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.PropertyBoards.AddRange(
                new PropertyBoard { Id = 1, Name = "Axes", SubjectDefinitionId = SyntheticPlan.CharacterDefId },
                new PropertyBoard { Id = 2, Name = "Empty", SubjectDefinitionId = SyntheticPlan.CharacterDefId });
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Boundary", PropertyBoardId = 1
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.PropertyBoardHasMembers(svc, 1));
        Assert.False(ContentIntegrity.PropertyBoardHasMembers(svc, 2));
    }

    [Fact]
    public async Task SubjectDefinitionHasDependents_also_counts_boards_and_relations_at_either_end()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectDefinitions.AddRange(
                new SubjectDefinition { Id = 90, SubjectType = "Board host" },
                new SubjectDefinition { Id = 91, SubjectType = "Relation source" },
                new SubjectDefinition { Id = 92, SubjectType = "Relation target" },
                new SubjectDefinition { Id = 93, SubjectType = "Truly unused" });
            ctx.PropertyBoards.Add(new PropertyBoard { Id = 1, Name = "Axes", SubjectDefinitionId = 90 });
            ctx.SubjectRelationDefinitions.Add(new SubjectRelationDefinition
            {
                Id = 1, Name = "Descends from", SubjectDefinitionId = 91, TargetSubjectDefinitionId = 92
            });
        });
        var svc = await plan.OpenStoryServiceAsync();

        Assert.True(ContentIntegrity.SubjectDefinitionHasDependents(svc, 90));
        Assert.True(ContentIntegrity.SubjectDefinitionHasDependents(svc, 91));
        // The TARGET end too — deleting it strands the relation just as thoroughly.
        Assert.True(ContentIntegrity.SubjectDefinitionHasDependents(svc, 92));
        Assert.False(ContentIntegrity.SubjectDefinitionHasDependents(svc, 93));
    }
}
