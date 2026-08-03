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
    /// <summary>True if the owner identified by the polymorphic pair still owns any note.
    /// The generic guard every note-owning entity's delete checks first.</summary>
    public static bool HasNotes(IStoryService storyService, int ownerId, OwnerType ownerType) =>
        storyService.Notes.Any(n => n.OwnerId == ownerId && n.OwnerType == ownerType);

    /// <summary>True if any note is tagged with this theme — deleting the theme would silently
    /// erase that tagging work (Note.ThemeId is a raw id with nothing else guarding it).</summary>
    public static bool ThemeHasNotes(IStoryService storyService, int themeId) =>
        storyService.Notes.Any(n => n.ThemeId == themeId);

    /// <summary>
    /// True if anything still hangs off this SubjectDefinition: subjects typed by it, or note-track /
    /// narrative-property definitions scoped to it. These are Type Object rows — the schema's
    /// load-bearing metadata — so deleting one with dependents strands every one of them.
    /// </summary>
    public static bool SubjectDefinitionHasDependents(IStoryService storyService, int subjectDefinitionId) =>
        storyService.Subjects.Any(s => s.SubjectDefinitionId == subjectDefinitionId) ||
        storyService.NoteTrackDefinitions.Any(t => t.SubjectDefinitionId == subjectDefinitionId) ||
        storyService.NarrativePropertyDefinitions.Any(p => p.SubjectDefinitionId == subjectDefinitionId);

    /// <summary>
    /// True if any note carries this track id. Deleting a track with notes silently demotes them to
    /// "Unassigned" (categorization lost by reference), and for a condition track it also flips
    /// their date semantics — event-vs-condition lives on the track, not the note.
    /// </summary>
    public static bool NoteTrackDefinitionHasNotes(IStoryService storyService, int noteTrackDefinitionId) =>
        storyService.Notes.Any(n => n.NoteTrackDefinitionId == noteTrackDefinitionId);

    /// <summary>True if any SourceMaterialPart or NoteSourceReference still points at this
    /// Work — deleting it would orphan them.</summary>
    public static bool SourceMaterialHasDependents(IStoryService storyService, int sourceMaterialId) =>
        storyService.SourceMaterialParts.Any(p => p.SourceMaterialId == sourceMaterialId) ||
        storyService.NoteSourceReferences.Any(r => r.SourceMaterialId == sourceMaterialId);

    /// <summary>True if any NoteSourceReference still cites this Part.</summary>
    public static bool SourceMaterialPartHasReferences(IStoryService storyService, int sourceMaterialPartId) =>
        storyService.NoteSourceReferences.Any(r => r.SourceMaterialPartId == sourceMaterialPartId);

    /// <summary>True if any NarrativePropertyDefinition gates on this WorkPhase.</summary>
    public static bool WorkPhaseHasDependents(IStoryService storyService, int workPhaseId) =>
        storyService.NarrativePropertyDefinitions.Any(p => p.GatingWorkPhaseId == workPhaseId);

    /// <summary>
    /// True if any owner has selected one of this property's allowed values. Deleting the property
    /// would orphan the assignment rows, and because NarrativePropertyValue has no OwnerType there
    /// would be no way afterwards to work out what they had meant.
    /// </summary>
    public static bool NarrativePropertyDefinitionHasDependents(IStoryService storyService, int propertyDefinitionId)
    {
        var valueDefIds = storyService.NarrativePropertyValueDefinitions
            .Where(v => v.NarrativePropertyDefinitionId == propertyDefinitionId)
            .Select(v => v.Id)
            .ToHashSet();

        return storyService.NarrativePropertyValues.Any(v => valueDefIds.Contains(v.ValueDefinitionId));
    }

    /// <summary>True if any owner has this specific value assigned.</summary>
    public static bool NarrativePropertyValueDefinitionHasAssignments(IStoryService storyService, int valueDefinitionId) =>
        storyService.NarrativePropertyValues.Any(v => v.ValueDefinitionId == valueDefinitionId);
}
