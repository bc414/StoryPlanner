using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels;

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

    public int DisplayOrder    => _definition.DisplayOrder;
    public string TrackName    => _definition.TrackName;
    public string Explanation  => _definition.UsageDirective;
    public CognitiveMode CognitiveMode => _definition.CognitiveMode;
    public NoteTrackDefinition Definition => _definition;

    public int OwnerId       => _ownerId;
    public OwnerType OwnerType => _ownerType;

    public AppSettings AppSettings => _appSettings;

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
        _appSettings = appSettings;

        _appSettings.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppSettings.IsArchiveMode))
                OnPropertyChanged(nameof(CanPromoteToConfirmed));
        };
    }

    // ── Lifecycle ─────────────────────────────────────────────────────────

    public void Initialize()
    {
        if (Sections.Count > 0) return;

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Confirmed,
            _viewModelRegistry, _storyService, this));

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Unset,
            _viewModelRegistry, _storyService, this));

        Sections.Add(new NoteTrackSectionViewModel(
            _ownerId, _ownerType, _definition, NoteState.Flagged,
            _viewModelRegistry, _storyService, this));

        foreach (var section in Sections)
            section.SelectionTransferRequested += OnSelectionTransferRequested;
    }

    public void Uninitialize()
    {
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

        var unsetSection = Sections.FirstOrDefault(s => s.TargetState == NoteState.Unset);

        int maxOrder = unsetSection?.SectionNotes
            .Cast<NoteViewModel>()
            .Select(n => n.SortOrder)
            .DefaultIfEmpty(0)
            .Max() ?? 0;

        int? trackId = _definition.Id == UnassignedTrack.Definition.Id ? null : _definition.Id;

        await _editorCoordinator.CreateNoteAsync(_ownerId, _ownerType, trackId, maxOrder + 1);
    }

    // ── Private helpers ───────────────────────────────────────────────────

    private void OnSelectionTransferRequested(NoteViewModel note)
    {
        var destination = Sections.FirstOrDefault(s => s.TargetState == note.NoteState);
        if (destination is not null)
            destination.SelectedNote = note;
    }

    public bool IsFirstTrack { get; set; }
}
