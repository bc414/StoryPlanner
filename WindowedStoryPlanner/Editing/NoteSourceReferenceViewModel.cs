using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// One citation on a note — a resolved (Work, optional Part) pair backing a
/// NoteSourceReference row. Display-only wrapper; mutation goes through
/// NoteViewModel.AddSourceReference/RemoveSourceReference, which own the underlying model
/// (mutate-then-save, per wpf-conventions).
/// </summary>
public class NoteSourceReferenceViewModel
{
    private readonly NoteSourceReference _model;
    public NoteSourceReference Model => _model;
    public int Id => _model.Id;

    public SourceMaterialViewModel Work { get; }

    /// <summary>Null means this reference cites the Work as a whole, not a particular Part.</summary>
    public SourceMaterialPartViewModel? Part { get; }

    public string DisplayLabel => Part is not null ? $"{Work.Name} · {Part.Code}" : Work.Name;

    public NoteSourceReferenceViewModel(NoteSourceReference model, SourceMaterialViewModel work, SourceMaterialPartViewModel? part)
    {
        _model = model;
        Work = work;
        Part = part;
    }
}
