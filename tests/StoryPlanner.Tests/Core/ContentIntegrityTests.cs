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
}
