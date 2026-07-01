namespace WindowedStoryPlanner.ViewModels;

public class SourceMaterialDetailViewModel : TaggedNotesViewModelBase
{
    public SourceMaterialViewModel SourceMaterial { get; }

    public SourceMaterialDetailViewModel(SourceMaterialViewModel sourceMaterial, IViewModelRegistry registry) : base(registry)
    {
        SourceMaterial = sourceMaterial;
    }

    protected override bool Matches(NoteViewModel note) => note.SelectedSourceMaterial?.Id == SourceMaterial.Id;
    protected override string TagPropertyName => nameof(NoteViewModel.SelectedSourceMaterial);
}
