using System.Linq;

namespace WindowedStoryPlanner;

/// <summary>
/// Cross-cut view of every note citing a Work at any depth. Complements
/// SourceMaterialPartDetailViewModel, which narrows to one specific Part.
/// </summary>
/// <remarks>
/// Primary constructor on purpose — see ThemeDetailViewModel's remarks: a derived property
/// initializer runs before the base constructor, which seeds the list via <see cref="Matches"/>.
/// </remarks>
public class SourceMaterialDetailViewModel(SourceMaterialViewModel sourceMaterial, IViewModelRegistry registry, IWindowManager windowManager)
    : TaggedNotesViewModelBase(registry, windowManager)
{
    public SourceMaterialViewModel SourceMaterial { get; } = sourceMaterial;

    // SourceMaterialId is carried on every reference regardless of which Part (if any) it
    // points at, so this catches citations at any depth under this Work.
    protected override bool Matches(NoteViewModel note) =>
        note.SourceReferences.Any(r => r.Work.Id == SourceMaterial.Id);

    protected override bool AffectsMembership(string? propertyName) =>
        propertyName is nameof(NoteViewModel.SourceReferences) or null or "";
}
