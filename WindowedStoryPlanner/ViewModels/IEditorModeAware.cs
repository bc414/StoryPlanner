namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Implemented by any view-model that owns <see cref="NoteTrackViewModel"/> instances
/// so that windows can push a <see cref="TrackDisplayMode"/> to all tracks uniformly.
/// </summary>
public interface IEditorModeAware
{
    void SetTrackDisplayMode(TrackDisplayMode mode);
}