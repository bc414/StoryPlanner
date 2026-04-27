using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteViewModel : ObservableObject
{
    private readonly Note _note;
    private readonly IStoryService _storyService;

    public NoteViewModel(Note note, IStoryService storyService)
    {
        _note = note;
        _storyService = storyService;
        _noteTrackDefinition = note.NoteTrackDefinitionId.HasValue 
            ? storyService.GetNoteTrackDefinition(note.NoteTrackDefinitionId.Value) 
            : null;
    }

    public int Id => _note.Id;

    public int OwnerId
    {
        get => _note.OwnerId;
        set => SetProperty(_note.OwnerId, value, _note, (n, v) => n.OwnerId = v);
    }

    public int? NoteTrackDefinitionId
    {
        get => _note.NoteTrackDefinitionId;
        set
        {
            if (SetProperty(_note.NoteTrackDefinitionId, value, _note, (n, v) => n.NoteTrackDefinitionId = v))
            {
                _noteTrackDefinition = value.HasValue
                    ? _storyService.GetNoteTrackDefinition(value.Value)
                    : null;
    
                OnPropertyChanged(nameof(NoteTrackDefinition));
                OnPropertyChanged(nameof(SupportsWorldDate));
                // Add more derived properties here as needed
            }
        }
    }

    private NoteTrackDefinition? _noteTrackDefinition;
    public NoteTrackDefinition? NoteTrackDefinition => _noteTrackDefinition;
    
    // --- Computed from NoteTrackDefinition ---
    
    public bool SupportsWorldDate => _noteTrackDefinition?.SupportsWorldDate ?? false;
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
        NoteState.Confirmed => "✓",
        NoteState.Flagged   => "⚑",
        NoteState.Unset     => "–",
        _                   => string.Empty
    };

    /// <summary>
    /// Exposes the underlying model for persistence purposes (e.g. EF Core change tracking).
    /// </summary>
    public Note Model => _note;
}
