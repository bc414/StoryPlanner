using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowedStoryPlanner;

public partial class NarrativeElementViewModel : ObservableObject, IDropTarget, IEditorModeAware
{
    protected readonly IStoryService _storyService;
    protected readonly IViewModelRegistry _viewModelRegistry;
    protected readonly IContentFactory _editorCoordinator;
    private readonly ExportService _exportService;

    // Internal master list used for lifecycle management (Initialize / Uninitialize).
    public ObservableCollection<NoteTrackViewModel> NoteTracks { get; set; } = new();
    public ObservableCollection<NarrativePropertyViewModel> NarrativeProperties { get; set; } = new();

    /// <summary>Tracks that own at least one note — displayed left-to-right in the main scroll area.</summary>
    public ObservableCollection<NoteTrackViewModel> PopulatedNoteTracks { get; } = new();

    /// <summary>Tracks with no notes — stacked vertically in a WrapPanel to the right of populated tracks.</summary>
    public ObservableCollection<NoteTrackViewModel> EmptyNoteTracks { get; } = new();

    /// <summary>
    /// Tracks the definition hides in the current editor mode (regardless of notes) —
    /// shown in a collapsed "Hidden in this mode" expander after the empty tracks.
    /// </summary>
    public ObservableCollection<NoteTrackViewModel> HiddenNoteTracks { get; } = new();

    // Keyed delegates so we can cleanly unsubscribe when tracks are torn down.
    private readonly Dictionary<NoteTrackViewModel, PropertyChangedEventHandler> _trackHandlers = new();

    private int _openWindowCount = 0;

    private Func<List<NoteTrackDefinition>>? _noteTrackFactory;
    private Func<List<NarrativePropertyDefinition>>? _propertyFactory;
    private int _ownerId;
    private OwnerType _ownerType;
    private EditorMode _editorMode = EditorMode.Expansion;

    // ── Note counts ───────────────────────────────────────────────────────

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UnconfirmedNoteCount))]
    private int _totalNoteCount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UnconfirmedNoteCount))]
    private int _confirmedNoteCount;

    /// <summary>
    /// Notes that exist but have not yet been confirmed.
    /// Used by archive mode to surface subjects with the most outstanding work.
    /// </summary>
    public int UnconfirmedNoteCount => TotalNoteCount - ConfirmedNoteCount;

    private bool _storyLoaded;

    private void RefreshNoteCounts()
    {
        var owned = _viewModelRegistry.AllNoteViewModels
            .Where(n => n.OwnerId == _ownerId && n.OwnerType == _ownerType)
            .ToList();

        TotalNoteCount     = owned.Count;
        ConfirmedNoteCount = owned.Count(n => n.NoteState == NoteState.Confirmed);
    }

    private void OnAllNotesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (!_storyLoaded) return;
        RefreshNoteCounts();
    }

    private void OnNoteViewModelMutated(NoteMutatedArgs args)
    {
        if (!_storyLoaded) return;
        if (args.OwnerId != _ownerId || args.OwnerType != _ownerType) return;
        RefreshNoteCounts();
    }

    private void OnStoryLoaded()
    {
        _storyLoaded = true;
        RefreshNoteCounts();
        OnStoryFullyLoaded();
    }

    /// <summary>
    /// Called once after the registry signals that bulk loading is complete and
    /// <see cref="RefreshNoteCounts"/> has run. Override in subclasses to release
    /// any <see cref="System.ComponentModel.ICollectionView.DeferRefresh"/> tokens
    /// or perform other one-time post-load initialization.
    /// </summary>
    protected virtual void OnStoryFullyLoaded() { }

    // ── Quick Export ──────────────────────────────────────────────────────

    [ObservableProperty] private int    _quickExportScope;
    [ObservableProperty] private string _quickExportStatus = string.Empty;

    [RelayCommand(CanExecute = nameof(CanQuickExport))]
    private void QuickExport()
    {
        var markdown   = _exportService.BuildQuickMarkdown(_ownerId, _ownerType, QuickExportScope);
        _exportService.CopyToClipboard(markdown);
        var entityName = GetEntityDisplayName();
        var path       = _exportService.WriteToFile(markdown, "", $"{entityName}-scope{QuickExportScope}");
        QuickExportStatus = $"Saved + copied  ({Path.GetFileName(path)})";
        _ = Task.Delay(5000).ContinueWith(_ =>
            Application.Current.Dispatcher.Invoke(() => QuickExportStatus = string.Empty));
    }

    private bool CanQuickExport() => _ownerType != OwnerType.PlotPointSubjectLink;

    private string GetEntityDisplayName() => _ownerType switch
    {
        OwnerType.Subject   => _storyService.Subjects.FirstOrDefault(s => s.Id == _ownerId)?.Name    ?? "Entity",
        OwnerType.PlotPoint => _storyService.PlotPoints.FirstOrDefault(p => p.Id == _ownerId)?.Title ?? "Entity",
        OwnerType.Chapter   => _storyService.Chapters.FirstOrDefault(c => c.Id == _ownerId)?.Title   ?? "Entity",
        _                   => "Entity"
    };

    public NarrativeElementViewModel(
        IViewModelRegistry viewModelRegistry,
        IStoryService storyService,
        IContentFactory editorCoordinator,
        AppSettings appSettings,
        ExportService exportService)
    {
        _storyService      = storyService;
        _viewModelRegistry = viewModelRegistry;
        _editorCoordinator = editorCoordinator;
        AppSettings        = appSettings;
        _exportService     = exportService;

        _viewModelRegistry.AllNoteViewModels.CollectionChanged += OnAllNotesCollectionChanged;
        _viewModelRegistry.NoteViewModelMutated += OnNoteViewModelMutated;
        _viewModelRegistry.StoryLoaded          += OnStoryLoaded;
    }

    protected void InitializeCollections(
        int ownerId,
        OwnerType ownerType,
        Func<List<NoteTrackDefinition>> noteTrackFactory,
        Func<List<NarrativePropertyDefinition>> propertyFactory)
    {
        _ownerId          = ownerId;
        _ownerType        = ownerType;
        _noteTrackFactory = noteTrackFactory;
        _propertyFactory  = propertyFactory;

        RefreshNoteCounts();
        RebuildNoteTracks();
    }

    private void RebuildNoteTracks()
    {
        if (_noteTrackFactory is null || _propertyFactory is null) return;

        // Unsubscribe from any existing tracks before clearing.
        foreach (var track in NoteTracks)
            UnsubscribeFromTrack(track);

        NoteTracks.Clear();
        NarrativeProperties.Clear();
        PopulatedNoteTracks.Clear();
        EmptyNoteTracks.Clear();
        HiddenNoteTracks.Clear();

        foreach (var ntd in _noteTrackFactory())
        {
            var track = new NoteTrackViewModel(
                ntd, _ownerId, _ownerType,
                _viewModelRegistry, _storyService, _editorCoordinator, AppSettings);
            track.EditorMode = _editorMode;
            NoteTracks.Add(track);
            SubscribeToTrack(track);
            DistributeTrack(track);
        }

        // Always add an unassigned track at the end for notes with no track set.
        var unassigned = new NoteTrackViewModel(
            UnassignedTrack.Definition, _ownerId, _ownerType,
            _viewModelRegistry, _storyService, _editorCoordinator, AppSettings);
        unassigned.EditorMode = _editorMode;
        NoteTracks.Add(unassigned);
        SubscribeToTrack(unassigned);
        DistributeTrack(unassigned);

        foreach (var npd in _propertyFactory())
            NarrativeProperties.Add(new NarrativePropertyViewModel(
                _ownerId, _ownerType, npd, _viewModelRegistry, _storyService));

        RefreshIsFirstTrack();
    }

    // ── Track distribution helpers ────────────────────────────────────────

    private void SubscribeToTrack(NoteTrackViewModel track)
    {
        PropertyChangedEventHandler handler = (_, e) =>
        {
            if (e.PropertyName == nameof(NoteTrackViewModel.HasNotes))
                OnTrackHasNotesChanged(track);
        };
        _trackHandlers[track] = handler;
        track.PropertyChanged += handler;
    }

    private void UnsubscribeFromTrack(NoteTrackViewModel track)
    {
        if (_trackHandlers.TryGetValue(track, out var handler))
        {
            track.PropertyChanged -= handler;
            _trackHandlers.Remove(track);
        }
    }

    /// <summary>
    /// Inserts <paramref name="track"/> into the correct sorted collection: the hidden
    /// group when the definition hides it in the current mode (even with notes — mode
    /// visibility outranks the populated/empty split), else by <see cref="NoteTrackViewModel.HasNotes"/>.
    /// </summary>
    private void DistributeTrack(NoteTrackViewModel track)
    {
        if (!track.IsVisibleInCurrentMode)
            InsertSorted(HiddenNoteTracks, track);
        else if (track.HasNotes)
            InsertSorted(PopulatedNoteTracks, track);
        else
            InsertSorted(EmptyNoteTracks, track);
    }

    private static void InsertSorted(ObservableCollection<NoteTrackViewModel> collection, NoteTrackViewModel track)
    {
        int index = 0;
        while (index < collection.Count && collection[index].DisplayOrder <= track.DisplayOrder)
            index++;
        collection.Insert(index, track);
    }

    private void OnTrackHasNotesChanged(NoteTrackViewModel track)
    {
        RemoveFromDistributedCollections(track);
        DistributeTrack(track);
        RefreshIsFirstTrack();
    }

    private void RemoveFromDistributedCollections(NoteTrackViewModel track)
    {
        PopulatedNoteTracks.Remove(track);
        EmptyNoteTracks.Remove(track);
        HiddenNoteTracks.Remove(track);
    }

    /// <summary>
    /// Marks the first track by DisplayOrder in <see cref="PopulatedNoteTracks"/> as
    /// <see cref="NoteTrackViewModel.IsFirstTrack"/> so the archive-mode width trigger fires correctly.
    /// </summary>
    private void RefreshIsFirstTrack()
    {
        foreach (var t in NoteTracks)
            t.IsFirstTrack = false;

        var first = PopulatedNoteTracks.FirstOrDefault(); // already sorted by DisplayOrder
        if (first is not null)
            first.IsFirstTrack = true;
    }

    // Single private teardown — used by both close and rebuild paths.
    private void TeardownTracks()
    {
        foreach (var track in NoteTracks)
        {
            track.Uninitialize();        // disposes sections, removes SelectionTransfer handlers
            UnsubscribeFromTrack(track); // removes HasNotes PropertyChanged handler + clears _trackHandlers entry
        }
    }

    public void RebuildAndInitializeTracks()
    {
        TeardownTracks();
        RebuildNoteTracks();           // clears + repopulates NoteTracks / PopulatedNoteTracks / EmptyNoteTracks
        foreach (var track in NoteTracks)
            track.Initialize();
    }

    public void OnWindowOpened()
    {
        _openWindowCount++;
        if (_openWindowCount > 1) return;
        RebuildAndInitializeTracks();
    }

    public void OnWindowClosed()
    {
        _openWindowCount = Math.Max(0, _openWindowCount - 1);
        if (_openWindowCount > 0) return;
        TeardownTracks();
    }

    // ── IEditorModeAware ──────────────────────────────────────────────────

    /// <summary>
    /// Stores the current editor mode and pushes it to all tracks, which updates their
    /// <see cref="NoteTrackViewModel.DisplayOrder"/> and per-mode visibility. A full
    /// redistribution then handles reordering and hide/reappear in one pass, regardless
    /// of whether the tracks were built before or after the mode was known.
    /// </summary>
    public void SetEditorMode(EditorMode mode)
    {
        _editorMode = mode;
        foreach (var track in NoteTracks)
            track.EditorMode = mode;

        PopulatedNoteTracks.Clear();
        EmptyNoteTracks.Clear();
        HiddenNoteTracks.Clear();
        foreach (var track in NoteTracks)
            DistributeTrack(track);
        RefreshIsFirstTrack();
    }

    /// <summary>
    /// Pushes <paramref name="mode"/> to every track so they update their
    /// <see cref="NoteTrackViewModel.HeaderText"/>, <see cref="NoteTrackViewModel.IsReadOnly"/>,
    /// and <see cref="NoteTrackViewModel.CanPromoteToConfirmed"/> in one call.
    /// </summary>
    public void SetTrackDisplayMode(TrackDisplayMode mode)
    {
        foreach (var track in NoteTracks)
            track.TrackDisplayMode = mode;
    }

    // ── GongSolutions IDropTarget (PlotPoint ↔ Subject linking) ──────────

    public virtual void DragOver(IDropInfo dropInfo)
    {
        // Let NoteViewModel drags pass through to NoteTrackSectionView's drop handler
        if (dropInfo.Data is NoteViewModel)
        {
            dropInfo.Effects = DragDropEffects.None;
            return;
        }

        bool canDrop = (dropInfo.Data, this) switch
        {
            (SubjectViewModel,              PlotPointViewModel) => true,
            (PlotPointViewModel,            SubjectViewModel)   => true,
            (PlotPointSubjectLinkViewModel, PlotPointViewModel) => true,
            (PlotPointSubjectLinkViewModel, SubjectViewModel)   => true,
            _ => false
        };

        if (canDrop)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }
    }

    public virtual async void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is NoteViewModel) return;

        var source = dropInfo.Data;
        var target = this;

        switch (source, target)
        {
            case (SubjectViewModel s, PlotPointViewModel p):
                await _editorCoordinator.CreatePlotPointSubjectLinkAsync(p, s); break;
            case (PlotPointViewModel p, SubjectViewModel s):
                await _editorCoordinator.CreatePlotPointSubjectLinkAsync(p, s); break;
            case (PlotPointSubjectLinkViewModel l, PlotPointViewModel p):
                l.PlotPointId = p.Id;
                _viewModelRegistry.RaiseLinksInvalidated();
                break;
            case (PlotPointSubjectLinkViewModel l, SubjectViewModel s):
                l.SubjectId = s.Id;
                _viewModelRegistry.RaiseLinksInvalidated();
                break;
        }
    }

    public AppSettings AppSettings { get; }
}
