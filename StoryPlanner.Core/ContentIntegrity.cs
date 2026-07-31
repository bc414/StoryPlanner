namespace StoryPlanner.Core;

/// <summary>
/// Id-based delete-guard predicates, expressed against <see cref="IStoryService"/>'s in-memory
/// collections — the same shape <c>ContentDeleter</c> itself reads — rather than view models.
/// This is the extraction the testing skill's "Known gap" note asks for: <c>ContentDeleter</c>'s
/// <c>TryDelete*</c> methods become thin wrappers around these, so the guard logic is testable
/// against a Fixture-tier <c>SyntheticPlan</c> without standing up the view-model graph. Nothing
/// but <c>ContentDeleter</c> should call these.
/// </summary>
public static class ContentIntegrity
{
    /// <summary>True if any SourceMaterialPart or NoteSourceReference still points at this
    /// Work — deleting it would orphan them.</summary>
    public static bool SourceMaterialHasDependents(IStoryService storyService, int sourceMaterialId) =>
        storyService.SourceMaterialParts.Any(p => p.SourceMaterialId == sourceMaterialId) ||
        storyService.NoteSourceReferences.Any(r => r.SourceMaterialId == sourceMaterialId);

    /// <summary>True if any NoteSourceReference still cites this Part.</summary>
    public static bool SourceMaterialPartHasReferences(IStoryService storyService, int sourceMaterialPartId) =>
        storyService.NoteSourceReferences.Any(r => r.SourceMaterialPartId == sourceMaterialPartId);
}
