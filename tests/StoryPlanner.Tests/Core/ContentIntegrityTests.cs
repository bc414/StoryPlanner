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
}
