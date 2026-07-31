namespace WindowedStoryPlanner;

/// <summary>
/// Cross-cut view of every note tagged with one theme — the evidence the proposition is argued
/// from. The theme entity holds the architecture; this view is a read-only aggregate of evidence
/// that lives on subject and link tracks, never a container of it.
/// </summary>
/// <remarks>
/// Primary constructor on purpose: a derived property initializer runs BEFORE the base
/// constructor, which seeds the list by calling <see cref="Matches"/>. Assigning Theme in a
/// constructor body instead would run after that call and dereference null.
/// </remarks>
public class ThemeDetailViewModel(ThemeViewModel theme, IViewModelRegistry registry, IWindowManager windowManager)
    : TaggedNotesViewModelBase(registry, windowManager)
{
    public ThemeViewModel Theme { get; } = theme;

    protected override bool Matches(NoteViewModel note) => note.SelectedTheme?.Id == Theme.Id;

    protected override bool AffectsMembership(string? propertyName) =>
        propertyName is nameof(NoteViewModel.SelectedTheme) or null or "";
}
