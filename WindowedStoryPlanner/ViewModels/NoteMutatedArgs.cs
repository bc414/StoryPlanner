using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Event payload for <see cref="IViewModelRegistry.NoteViewModelMutated"/>.
/// Carries ownership context so subscribers can filter with zero collection lookups.
/// </summary>
public readonly record struct NoteMutatedArgs(
    int       NoteId,
    int       OwnerId,
    OwnerType OwnerType,
    int?      NoteTrackDefinitionId);
