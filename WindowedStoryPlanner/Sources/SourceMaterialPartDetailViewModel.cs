namespace WindowedStoryPlanner;

/// <summary>
/// Cross-cut view of every note citing one specific Part — the drill-down from the coverage
/// grid's "N notes" cell. Complements SourceMaterialDetailViewModel, which shows every note
/// citing the Work at any depth.
/// </summary>
public class SourceMaterialPartDetailViewModel : TaggedNotesViewModelBase
{
    public SourceMaterialPartViewModel Part { get; }

    public SourceMaterialPartDetailViewModel(SourceMaterialPartViewModel part, IViewModelRegistry registry) : base(registry)
    {
        Part = part;
    }

    protected override bool Matches(NoteViewModel note) =>
        note.SourceReferences.Any(r => r.Part?.Id == Part.Id);
    protected override string TagPropertyName => nameof(NoteViewModel.SourceReferences);
}
