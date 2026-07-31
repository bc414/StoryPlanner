using System.Linq;

namespace WindowedStoryPlanner;

/// <summary>
/// Cross-cut view of every note citing one specific Part — the drill-down from the coverage
/// grid's "N notes" cell. Complements SourceMaterialDetailViewModel, which shows every note
/// citing the Work at any depth.
/// </summary>
/// <remarks>
/// Primary constructor on purpose — see ThemeDetailViewModel's remarks: a derived property
/// initializer runs before the base constructor, which seeds the list via <see cref="Matches"/>.
/// </remarks>
public class SourceMaterialPartDetailViewModel(SourceMaterialPartViewModel part, IViewModelRegistry registry, IWindowManager windowManager)
    : TaggedNotesViewModelBase(registry, windowManager)
{
    public SourceMaterialPartViewModel Part { get; } = part;

    protected override bool Matches(NoteViewModel note) =>
        note.SourceReferences.Any(r => r.Part?.Id == Part.Id);

    protected override bool AffectsMembership(string? propertyName) =>
        propertyName is nameof(NoteViewModel.SourceReferences) or null or "";
}
