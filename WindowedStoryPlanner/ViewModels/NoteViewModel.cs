using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteViewModel : ObservableObject
{
    private readonly Note _note;
    private readonly IStoryService _storyService;
    private readonly ObservableCollection<ThemeViewModel> _themes;

    public NoteViewModel(Note note, IStoryService storyService, ObservableCollection<ThemeViewModel> themes)
    {
        _note = note;
        _storyService = storyService;
        _themes = themes;
        _noteTrackDefinition = note.NoteTrackDefinitionId.HasValue
            ? storyService.GetNoteTrackDefinition(note.NoteTrackDefinitionId.Value)
            : null;
    }

    public int Id => _note.Id;
    public Note Note => _note;

    public int OwnerId
    {
        get => _note.OwnerId;
        set => SetProperty(_note.OwnerId, value, _note, (n, v) => n.OwnerId = v);
    }

    public OwnerType OwnerType
    {
        get => _note.OwnerType;
        set => SetProperty(_note.OwnerType, value, _note, (n, v) => n.OwnerType = v);
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
                OnPropertyChanged(nameof(SupportsTheme));
            }
        }
    }

    private NoteTrackDefinition? _noteTrackDefinition;
    public NoteTrackDefinition? NoteTrackDefinition => _noteTrackDefinition;

    public bool SupportsWorldDate => _noteTrackDefinition?.SupportsWorldDate ?? false;
    public bool SupportsTheme     => _noteTrackDefinition?.SupportsTheme     ?? false;

    public DateTime LastModified => _note.LastModified;

    public string Content
    {
        get => _note.Content;
        set
        {
            if (SetProperty(_note.Content, value, _note, (n, v) => n.Content = v))
                OnPropertyChanged(nameof(IsEmpty));
        }
    }

    public bool IsEmpty => string.IsNullOrWhiteSpace(_note.Content);

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

    public bool IsFlagged => _note.NoteState == NoteState.Flagged;

    public string StateLabel => _note.NoteState switch
    {
        NoteState.Confirmed => "✓",
        NoteState.Flagged   => "⚑",
        NoteState.Unset     => "–",
        _                   => string.Empty
    };

    // ── Theme ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Shared collection reference — same instance across all NoteViewModels.
    /// Bound as ComboBox ItemsSource in NoteView.xaml.
    /// </summary>
    public ObservableCollection<ThemeViewModel> AvailableThemes => _themes;

    /// <summary>
    /// Resolves ThemeId → ThemeViewModel for display; sets ThemeId on write.
    /// Null means "no theme assigned".
    /// </summary>
    public ThemeViewModel? SelectedTheme
    {
        get => _themes.FirstOrDefault(t => t.Id == _note.ThemeId);
        set
        {
            var newId = value?.Id;
            if (SetProperty(_note.ThemeId, newId, _note, (n, v) => n.ThemeId = v))
                OnPropertyChanged();
        }
    }

    public Note Model => _note;

    [RelayCommand]
    private void ClearTheme()
    {
        SelectedTheme = null;
    }
}
