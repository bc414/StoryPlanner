using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteViewModel : ObservableObject
{
    private readonly Note _note;

    public NoteViewModel(Note note)
    {
        _note = note;
    }

    // --- Read-Only Identity ---

    public int Id => _note.Id;
    public int OwnerId => _note.OwnerId;
    public int NoteTrackDefinitionId => _note.NoteTrackDefinitionId;
    public DateTime LastModified => _note.LastModified;

    // --- Read-Write Fields ---

    public string Content
    {
        get => _note.Content;
        set => SetProperty(_note.Content, value, _note, (n, v) => n.Content = v);
    }

    public NoteState NoteState
    {
        get => _note.NoteState;
        set
        {
            if (SetProperty(_note.NoteState, value, _note, (n, v) => n.NoteState = v))
            {
                OnPropertyChanged(nameof(IsFlagged));
                OnPropertyChanged(nameof(StateLabel));
            }
        }
    }

    public string FlagReason
    {
        get => _note.FlagReason;
        set => SetProperty(_note.FlagReason, value, _note, (n, v) => n.FlagReason = v);
    }

    public int SortOrder
    {
        get => _note.SortOrder;
        set => SetProperty(_note.SortOrder, value, _note, (n, v) => n.SortOrder = v);
    }

    public string WorldDate
    {
        get => _note.WorldDate;
        set => SetProperty(_note.WorldDate, value, _note, (n, v) => n.WorldDate = v);
    }

    // --- UI Helpers ---

    public bool IsFlagged => _note.NoteState == NoteState.Flagged;

    public string StateLabel => _note.NoteState switch
    {
        NoteState.Confirmed => "✓ Confirmed",
        NoteState.Flagged   => "⚑ Flagged",
        NoteState.Unset     => "– Unreviewed",
        _                   => string.Empty
    };

    /// <summary>
    /// Exposes the underlying model for persistence purposes (e.g. EF Core change tracking).
    /// </summary>
    public Note Model => _note;
}
