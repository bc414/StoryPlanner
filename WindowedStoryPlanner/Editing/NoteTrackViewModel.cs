using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner;

public partial class NoteTrackViewModel : ObservableObject
{
    private readonly int _ownerId;
    private readonly OwnerType _ownerType;
    private readonly NoteTrackDefinition _definition;
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _viewModelRegistry;
    private readonly IContentFactory _editorCoordinator;
    private readonly AppSettings _appSettings;

    public ObservableCollection<NoteTrackSectionViewModel> Sections { get; } = new();

    private EditorMode _editorMode = EditorMode.Expansion;
    public EditorMode EditorMode
    {
        get => _editorMode;
        set
        {
            if (_editorMode != value)
            {
                _editorMode = value;
                OnPropertyChanged(nameof(DisplayOrder));
                OnPropertyChanged(nameof(IsVisibleInCurrentMode));
            }
        }
    }

    public int DisplayOrder => _editorMode switch
    {
        EditorMode.Expansion => _definition.ExpansionModeDisplayOrder,
        EditorMode.Linking => _definition.LinkingModeDisplayOrder,
        EditorMode.Gardener => _definition.GardenerModeDisplayOrder,
        EditorMode.Audit => _definition.AuditModeDisplayOrder,
        EditorMode.SceneDesign => _definition.SceneDesignModeDisplayOrder,
        _ => _definition.ExpansionModeDisplayOrder
    };

    /// <summary>
    /// False when the definition hides this track in the current editor mode — the
    /// parent NarrativeElementViewModel then routes it to the collapsed hidden group
    /// instead of the populated/empty split. The Unassigned track's flags are all
    /// false, so it is always visible.
    /// </summary>
    public bool IsVisibleInCurrentMode => !(_editorMode switch
    {
        EditorMode.Expansion => _definition.HiddenInExpansionMode,
        EditorMode.Linking => _definition.HiddenInLinkingMode,
        EditorMode.Gardener => _definition.HiddenInGardenerMode,
        EditorMode.Audit => _definition.HiddenInAuditMode,
        EditorMode.SceneDesign => _definition.HiddenInSceneDesignMode,
        _ => _definition.HiddenInExpansionMode
    });
    public string TrackName     => _definition.TrackName;
    public string Explanation   => _definition.UsageDirective;
    public TrackType TrackType  => _definition.TrackType;
    public string CognitiveMode => _definition.TrackType.GetCognitiveMode();
    public NoteTrackDefinition Definition => _definition;

    public int OwnerId         => _ownerId;
    public OwnerType OwnerType => _ownerType;

    public AppSettings AppSettings => _appSettings;

    // ── HasNotes ──────────────────────────────────────────────────────────

    /// <summary>
    /// True after the registry has raised <see cref="IViewModelRegistry.StoryLoaded"/>.
    /// Guards <see cref="RefreshHasNotes"/> against O(notes × tracks) calls during bulk load.
    /// </summary>
    private bool _storyLoaded;

    /// <summary>
    /// True when this track owns at least one note in any state.
    /// Drives the populated/empty layout split in NarrativeElementFullView.
    /// </summary>
    [ObservableProperty]
    private bool _hasNotes;

    private void RefreshHasNotes()
    {
        bool isUnassigned = _definition.Id == UnassignedTrack.Definition.Id;
        HasNotes = _viewModelRegistry.AllNoteViewModels.Any(n =>
            n.OwnerId   == _ownerId &&
            n.OwnerType == _ownerType &&
            (isUnassigned
                ? n.NoteTrackDefinitionId == null
                : n.NoteTrackDefinitionId == _definition.Id));
    }

    // ── IsFirstTrack ──────────────────────────────────────────────────────

    /// <summary>
    /// Set by the parent NarrativeElementViewModel on the first populated track
    /// (by DisplayOrder). Drives the archive-mode full-width trigger in XAML.
    /// Observable so the XAML trigger reacts when tracks move between groups.
    /// </summary>
    [ObservableProperty]
    private bool _isFirstTrack;

    // ── Display mode ──────────────────────────────────────────────────────

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HeaderText))]
    [NotifyPropertyChangedFor(nameof(IsReadOnly))]
    [NotifyPropertyChangedFor(nameof(CanPromoteToConfirmed))]
    private TrackDisplayMode _trackDisplayMode = TrackDisplayMode.Active;

    /// <summary>
    /// Text shown in the track header, driven by <see cref="TrackDisplayMode"/>.
    /// </summary>
    public string HeaderText => TrackDisplayMode switch
    {
        TrackDisplayMode.Reference => _definition.UsageDirective,
        TrackDisplayMode.Audit     => _definition.AuditDirective,
        _                          => _definition.DisplayQuestion,
    };

    /// <summary>True when the track is in <see cref="TrackDisplayMode.Reference"/> — all edits blocked.</summary>
    public bool IsReadOnly => TrackDisplayMode == TrackDisplayMode.Reference;

    /// <summary>True only in <see cref="TrackDisplayMode.Audit"/> — the sole mode that allows promoting notes to Confirmed.</summary>
    public bool CanPromoteToConfirmed =>
        TrackDisplayMode == TrackDisplayMode.Audit || _appSettings.IsArchiveMode;

    partial void OnTrackDisplayModeChanged(TrackDisplayMode value)
    {
        foreach (var section in Sections)
            section.RefreshReadonlyState();
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public NoteTrackViewModel(
        NoteTrackDefinition definition,
        int ownerId,
        OwnerType ownerType,
        IViewModelRegistry registry,
        IStoryService storyService,
        IContentFactory editorCoordinator,
        AppSettings appSettings)
    {
        _definition        = definition;
        _ownerId           = ownerId;
        _ownerType         = ownerType;
        _storyService      = storyService;
        _viewModelRegistry = registry;
        _editorCoordinator = editorCoordinator;
        _appSettings       = appSettings;

        // During bulk load, AllNoteViewModels.CollectionChanged fires once per note
        // for every NoteTrackViewModel, which is O(notes × tracks) and wasteful.
        // Suppress it while loading and let the StoryLoaded event do a single refresh.
        _storyLoaded = _viewModelRegistry.IsStoryLoaded;

        // Registry/app-settings subscriptions live in Initialize/Uninitialize, NOT here: the
        // targets are app-lifetime singletons, so a constructor subscription with no teardown
        // pins every discarded track VM forever — and each leaked handler re-scans all notes on
        // every mutation. This was the layer's biggest gets-slower-the-longer-you-work leak.

        // Seed the initial value — story may already be loaded when this track is created.
        RefreshHasNotes();
    }

    // ── Lifecycle ─────────────────────────────────────────────────────────
    // Subscription teardown pairs with Initialize, and both are idempotent. Tracks built at
    // element-VM construction but never shown in a window simply never subscribe; every
    // window open rebuilds fresh tracks whose constructors re-seed HasNotes.

    private bool _subscribed;

    private void OnAppSettingsPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppSettings.IsArchiveMode))
            OnPropertyChanged(nameof(CanPromoteToConfirmed));
        foreach (var section in Sections)
            section.RefreshReadonlyState();
    }

    private void OnAllNotesCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (_storyLoaded) RefreshHasNotes();
    }

    private void OnNoteMutated(NoteMutatedArgs args)
    {
        if (args.OwnerId == _ownerId && args.OwnerType == _ownerType)
            RefreshHasNotes();
    }

    private void OnStoryLoaded()
    {
        _storyLoaded = true;
        RefreshHasNotes();
    }

    public void Initialize()
    {
        if (!_subscribed)
        {
            _appSettings.PropertyChanged += OnAppSettingsPropertyChanged;
            _viewModelRegistry.AllNoteViewModels.CollectionChanged += OnAllNotesCollectionChanged;
            _viewModelRegistry.NoteViewModelMutated += OnNoteMutated;
            _viewModelRegistry.StoryLoaded += OnStoryLoaded;
            _subscribed = true;
            RefreshHasNotes();
        }

        if (Sections.Count > 0) return;

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Confirmed,
            _viewModelRegistry, _storyService, _editorCoordinator, this));

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Unset,
            _viewModelRegistry, _storyService, _editorCoordinator, this));

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Flagged,
            _viewModelRegistry, _storyService, _editorCoordinator, this));

        foreach (var section in Sections)
            section.SelectionTransferRequested += OnSelectionTransferRequested;
    }

    public void Uninitialize()
    {
        if (_subscribed)
        {
            _appSettings.PropertyChanged -= OnAppSettingsPropertyChanged;
            _viewModelRegistry.AllNoteViewModels.CollectionChanged -= OnAllNotesCollectionChanged;
            _viewModelRegistry.NoteViewModelMutated -= OnNoteMutated;
            _viewModelRegistry.StoryLoaded -= OnStoryLoaded;
            _subscribed = false;
        }

        foreach (var section in Sections)
        {
            section.SelectionTransferRequested -= OnSelectionTransferRequested;
            section.Dispose();
        }
        Sections.Clear();
    }

    // ── Commands ──────────────────────────────────────────────────────────

    [RelayCommand]
    public async Task CreateNewNote()
    {
        if (IsReadOnly) return;

        // One creation path: a null anchor is the append-to-end case. The Unset
        // section is always present once Initialize() has run, which happens
        // before any NoteTrackView (and thus this button) can exist.
        var unsetSection = Sections.FirstOrDefault(s => s.TargetState == NoteState.Unset);
        if (unsetSection is null) return;

        await unsetSection.InsertNoteBeforeCommand.ExecuteAsync(null);
    }

    // ── Private helpers ───────────────────────────────────────────────────

    private void OnSelectionTransferRequested(NoteViewModel note)
    {
        var destination = Sections.FirstOrDefault(s => s.TargetState == note.NoteState);
        if (destination is not null)
            destination.SelectedNote = note;
    }
}
