using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner;

public partial class NoteViewModel : ObservableObject
{
    private readonly Note _note;
    private readonly IStoryService _storyService;
    private readonly ObservableCollection<ThemeViewModel> _themes;
    private readonly ObservableCollection<SourceMaterialViewModel> _sourceMaterials;
    private readonly ObservableCollection<SourceMaterialPartViewModel> _sourceMaterialParts;

    public NoteViewModel(
        Note note,
        IStoryService storyService,
        ObservableCollection<ThemeViewModel> themes,
        ObservableCollection<SourceMaterialViewModel> sourceMaterials,
        ObservableCollection<SourceMaterialPartViewModel> sourceMaterialParts)
    {
        _note = note;
        _storyService = storyService;
        _themes = themes;
        _sourceMaterials = sourceMaterials;
        _sourceMaterialParts = sourceMaterialParts;
        _noteTrackDefinition = note.NoteTrackDefinitionId.HasValue
            ? storyService.GetNoteTrackDefinition(note.NoteTrackDefinitionId.Value)
            : null;

        // Resolve this note's existing citations. Orphaned rows (Work no longer resolvable)
        // are skipped rather than crashing — ContentDeleter's SourceMaterial guard is what's
        // supposed to prevent that state from ever existing, but a resolver should not assume.
        foreach (var r in storyService.NoteSourceReferences
                     .Where(r => r.NoteId == note.Id)
                     .OrderBy(r => r.SortOrder))
        {
            var work = _sourceMaterials.FirstOrDefault(s => s.Id == r.SourceMaterialId);
            if (work is null) continue;
            var part = r.SourceMaterialPartId.HasValue
                ? _sourceMaterialParts.FirstOrDefault(p => p.Id == r.SourceMaterialPartId.Value)
                : null;
            _sourceReferences.Add(new NoteSourceReferenceViewModel(r, work, part));
        }
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
    // Many-to-many: a note may cite several Parts for one claim (e.g. "the Wonderbolts were
    // useless in a crisis, as shown in Sonic Rainboom, Secret of my Excess, Equestria Games
    // and Twilight's Kingdom" cites four episodes for one proposition) — see the plan's "why
    // not just split the notes" analysis. Backed by NoteSourceReference rows, not a single FK.

    /// <summary>
    /// Shared collection reference — same instance across all NoteViewModels.
    /// Bound as the picker's Work search source in NoteView.xaml.
    /// </summary>
    public ObservableCollection<SourceMaterialViewModel> AvailableSourceMaterials => _sourceMaterials;

    /// <summary>
    /// Shared collection reference — same instance across all NoteViewModels.
    /// Bound as the picker's Part search source in NoteView.xaml.
    /// </summary>
    public ObservableCollection<SourceMaterialPartViewModel> AvailableSourceMaterialParts => _sourceMaterialParts;

    private readonly ObservableCollection<NoteSourceReferenceViewModel> _sourceReferences = new();

    /// <summary>This note's citations. Mutate only via AddSourceReference/RemoveSourceReference —
    /// both keep the underlying NoteSourceReference rows and OnPropertyChanged in sync.</summary>
    public ObservableCollection<NoteSourceReferenceViewModel> SourceReferences => _sourceReferences;

    /// <summary>Adds a citation. part = null cites the Work as a whole. No-ops on a duplicate
    /// (Work, Part) pair — re-citing the same thing twice is never useful.</summary>
    public void AddSourceReference(SourceMaterialViewModel work, SourceMaterialPartViewModel? part)
    {
        if (_sourceReferences.Any(r => r.Work.Id == work.Id && r.Part?.Id == part?.Id)) return;

        var model = new NoteSourceReference
        {
            NoteId = _note.Id,
            SourceMaterialId = work.Id,
            SourceMaterialPartId = part?.Id,
            SortOrder = _sourceReferences.Count
        };
        _storyService.NoteSourceReferences.Add(model);
        _sourceReferences.Add(new NoteSourceReferenceViewModel(model, work, part));
        OnPropertyChanged(nameof(SourceReferences));
        _ = _storyService.SaveAsync();
    }

    public void RemoveSourceReference(NoteSourceReferenceViewModel reference)
    {
        _storyService.NoteSourceReferences.Remove(reference.Model);
        _sourceReferences.Remove(reference);
        OnPropertyChanged(nameof(SourceReferences));
        _ = _storyService.SaveAsync();
    }

    /// <summary>Quick-add: creates a new Work (e.g. a fanfic not yet in the library). Does not
    /// cite it — call AddSourceReference afterward. Adds to the shared registry-backed
    /// collection so every open picker sees it, matching ContentFactory's
    /// mutate-then-save-then-sync-registry pattern.
    ///
    /// AWAITS the save deliberately: EF assigns model.Id there, and the caller immediately uses
    /// that id (as a Part's SourceMaterialId, and as a citation's). Fire-and-forget would hand
    /// back a Work whose Id is still 0 and silently orphan both rows.</summary>
    public async Task<SourceMaterialViewModel> CreateSourceMaterialAsync(string name)
    {
        var model = new SourceMaterial { Name = name, Description = string.Empty, OrderIndex = _sourceMaterials.Count };
        _storyService.SourceMaterials.Add(model);
        await _storyService.SaveAsync();          // Id assigned here — must precede any use of it
        var vm = new SourceMaterialViewModel(model, _storyService);
        _sourceMaterials.Add(vm);
        return vm;
    }

    /// <summary>Quick-add: creates a new Part under an existing Work (e.g. an episode missing
    /// from the seeded list). Does not cite it — call AddSourceReference afterward. Awaits the
    /// save for the same reason as CreateSourceMaterialAsync: the caller cites the new Part by
    /// id straight afterward.</summary>
    public async Task<SourceMaterialPartViewModel> CreateSourceMaterialPartAsync(
        SourceMaterialViewModel work, string code, string name)
    {
        var model = new SourceMaterialPart
        {
            SourceMaterialId = work.Id,
            Code = code,
            Name = name,
            Description = string.Empty,
            OrderIndex = _sourceMaterialParts.Count(p => p.SourceMaterialId == work.Id),
            ReviewState = SourcePartReviewState.NotReviewed
        };
        _storyService.SourceMaterialParts.Add(model);
        await _storyService.SaveAsync();          // Id assigned here
        var vm = new SourceMaterialPartViewModel(model, _storyService);
        _sourceMaterialParts.Add(vm);
        return vm;
    }
}
