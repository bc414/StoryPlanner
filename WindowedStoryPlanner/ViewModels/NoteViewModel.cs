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
    private readonly ObservableCollection<SourceMaterialViewModel> _sourceMaterials;

    public NoteViewModel(
        Note note,
        IStoryService storyService,
        ObservableCollection<ThemeViewModel> themes,
        ObservableCollection<SourceMaterialViewModel> sourceMaterials)
    {
        _note = note;
        _storyService = storyService;
        _themes = themes;
        _sourceMaterials = sourceMaterials;
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
                OnPropertyChanged(nameof(SupportsWorldDateEnd));
                OnPropertyChanged(nameof(WorldDateHint));
                OnPropertyChanged(nameof(SupportsTheme));
                OnPropertyChanged(nameof(SupportsSourceMaterial));
            }
        }
    }

    private NoteTrackDefinition? _noteTrackDefinition;
    public NoteTrackDefinition? NoteTrackDefinition => _noteTrackDefinition;

    public bool SupportsWorldDate     => _noteTrackDefinition?.SupportsWorldDate     ?? false;
    public bool SupportsWorldDateEnd  => _noteTrackDefinition?.SupportsWorldDateEnd  ?? false;
    public bool SupportsTheme         => _noteTrackDefinition?.SupportsTheme         ?? false;
    public bool SupportsSourceMaterial => _noteTrackDefinition?.SupportsSourceMaterial ?? false;

    /// <summary>Watermark/tooltip for the date field, shaped by the track: event tracks take a
    /// single date, condition tracks an interval.</summary>
    public string WorldDateHint => SupportsWorldDateEnd
        ? "Interval: 854..914, 1007.. (end TBD), ..1007 (start TBD). Precision: YYYY, YYYY-MM, YYYY-MM-DD. Negative = BLB."
        : "Single date: 1007, 1007-03, 1007-03-15. Negative = BLB, 0 = the banishment.";

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

    /// <summary>
    /// The structured world date rendered/edited in notation form ("1007", "1007-03-15",
    /// "854..914", "1007.."). Reads prefer the structured columns and fall back to the legacy
    /// free-text string on unconverted files; a successful edit always writes structured and
    /// blanks the legacy string. Invalid input is kept on screen with <see cref="WorldDateError"/>
    /// set and nothing written — flag, never guess.
    /// </summary>
    public string WorldDate
    {
        get
        {
            if (_invalidWorldDateText is not null) return _invalidWorldDateText;
            try
            {
                if (_note.GetWorldDate() is { } d)
                    return d.ToNotation(asInterval: d.End is not null || SupportsWorldDateEnd);
            }
            catch (ArgumentException) { /* malformed columns — fall through to legacy text */ }
            return _note.WorldDate;
        }
        set
        {
            if (value == WorldDate) return;

            if (!StoryPlanner.Core.WorldDate.TryParse(value, out var date, out var error))
            {
                _invalidWorldDateText = value;
                WorldDateError = error;
                OnPropertyChanged();
                return;
            }
            if (date is { End: not null } && !SupportsWorldDateEnd)
            {
                _invalidWorldDateText = value;
                WorldDateError = "This is an event track — a note here asserts when something happened, " +
                                 "not over what period. Give a single date, or move the note to the condition track.";
                OnPropertyChanged();
                return;
            }

            _invalidWorldDateText = null;
            WorldDateError = "";
            _note.SetWorldDate(date);
            _note.WorldDate = string.Empty; // structured is now the one representation
            OnPropertyChanged();
        }
    }

    private string? _invalidWorldDateText;

    [ObservableProperty]
    private string _worldDateError = "";

    public bool HasWorldDateError => WorldDateError.Length > 0;

    partial void OnWorldDateErrorChanged(string value) => OnPropertyChanged(nameof(HasWorldDateError));

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

    // ── Source Material ─────────────────────────────────────────────────────

    /// <summary>
    /// Shared collection reference — same instance across all NoteViewModels.
    /// Bound as the search picker's ItemsSource in NoteView.xaml.
    /// </summary>
    public ObservableCollection<SourceMaterialViewModel> AvailableSourceMaterials => _sourceMaterials;

    /// <summary>
    /// Resolves SourceMaterialId → SourceMaterialViewModel for display; sets SourceMaterialId on write.
    /// Null means "no source material assigned".
    /// </summary>
    public SourceMaterialViewModel? SelectedSourceMaterial
    {
        get => _sourceMaterials.FirstOrDefault(s => s.Id == _note.SourceMaterialId);
        set
        {
            var newId = value?.Id;
            if (SetProperty(_note.SourceMaterialId, newId, _note, (n, v) => n.SourceMaterialId = v))
                OnPropertyChanged();
        }
    }

    [RelayCommand]
    private void ClearSourceMaterial()
    {
        SelectedSourceMaterial = null;
    }
}
