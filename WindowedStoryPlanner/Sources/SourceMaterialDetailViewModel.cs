namespace WindowedStoryPlanner;

public class SourceMaterialDetailViewModel : TaggedNotesViewModelBase
{
    public SourceMaterialViewModel SourceMaterial { get; }

    public SourceMaterialDetailViewModel(SourceMaterialViewModel sourceMaterial, IViewModelRegistry registry) : base(registry)
    {
        SourceMaterial = sourceMaterial;
    }

    // SourceMaterialId is carried on every reference regardless of which Part (if any) it
    // points at, so this catches citations at any depth under this Work.
    protected override bool Matches(NoteViewModel note) =>
        note.SourceReferences.Any(r => r.Work.Id == SourceMaterial.Id);
    protected override string TagPropertyName => nameof(NoteViewModel.SourceReferences);
}
