using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteTrackSectionViewModel : ObservableObject, IDropTarget
{
    private readonly int _ownerId;
    private readonly OwnerType _ownerType;
    private readonly NoteTrackDefinition _definition;
    private readonly NoteState _targetState;
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _viewModelRegistry;
    private readonly NoteTrackViewModel _parentTrack;

    private bool _isReindexing;

    public string SectionHeader => _targetState.ToString();
    public NoteState TargetState => _targetState;
    public ICollectionView SectionNotes { get; }

    // Pre-filtered backing store: contains only notes that belong to this section.
    // Replacing a global-source CVS + live-filtering avoids WPF subscribing to
    // PropertyChanged on every note in AllNoteViewModels for every section instance,
    // which was O(notes × sections) per PropertyChanged raise and dominated open time.
    private readonly ObservableCollection<NoteViewModel> _sectionSource = new();

    [ObservableProperty]
    private NoteViewModel? _selectedNote;

    partial void OnSelectedNoteChanged(NoteViewModel? value)
    {
        if (value is not null)
            _viewModelRegistry.RaiseNoteSelected(value.Id);
    }

    // ── Read-only state (mirrored from parent track) ──────────────────────

    private bool _isReadOnly;
    private bool _canPromoteToConfirmed;

    public bool IsReadOnly
    {
        get => _isReadOnly;
        private set { if (_isReadOnly != value) { _isReadOnly = value; OnPropertyChanged(); } }
    }

    public bool CanPromoteToConfirmed
    {
        get => _canPromoteToConfirmed;
        private set { if (_canPromoteToConfirmed != value) { _canPromoteToConfirmed = value; OnPropertyChanged(); } }
    }

    public void RefreshReadonlyState()
    {
        IsReadOnly            = _parentTrack.IsReadOnly;
        CanPromoteToConfirmed = _parentTrack.CanPromoteToConfirmed;
    }

    // ── Visibility / drop-zone sizing ─────────────────────────────────────

    public bool IsVisible => _targetState == NoteState.Unset || _parentTrack.HasNotes;

    public double MinDropHeight =>
        _targetState == NoteState.Unset && !_parentTrack.HasNotes ? 40.0 : 0.0;

    // ── Constructor ───────────────────────────────────────────────────────

    public NoteTrackSectionViewModel(
        int ownerId,
        OwnerType ownerType,
        NoteTrackDefinition definition,
        NoteState targetState,
        IViewModelRegistry viewModelRegistry,
        IStoryService storyService,
        NoteTrackViewModel parentTrack)
    {
        _ownerId           = ownerId;
        _ownerType         = ownerType;
        _definition        = definition;
        _targetState       = targetState;
        _storyService      = storyService;
        _viewModelRegistry = viewModelRegistry;
        _parentTrack       = parentTrack;

        _isReadOnly            = parentTrack.IsReadOnly;
        _canPromoteToConfirmed = parentTrack.CanPromoteToConfirmed;

        // Populate the pre-filtered source with notes that already belong here.
        foreach (var n in viewModelRegistry.AllNoteViewModels.Where(BelongsToThisSection))
            _sectionSource.Add(n);

        // CVS sorts only the small pre-filtered collection.
        // No filter predicate, no live-filtering → zero per-note PropertyChanged
        // subscriptions at open time.
        var cvs = new CollectionViewSource { Source = _sectionSource };
        SectionNotes = cvs.View;
        SectionNotes.SortDescriptions.Add(
            new SortDescription(nameof(NoteViewModel.SortOrder), ListSortDirection.Ascending));

        // Live sorting so SortOrder writes produce targeted Move notifications
        // instead of full Resets.
        if (SectionNotes is ICollectionViewLiveShaping liveShaping)
        {
            liveShaping.IsLiveSorting = true;
            liveShaping.LiveSortingProperties.Add(nameof(NoteViewModel.SortOrder));
        }

        // Keep _sectionSource in sync with global add/delete events.
        viewModelRegistry.AllNoteViewModels.CollectionChanged += OnAllNotesCollectionChanged;

        _viewModelRegistry.NoteViewModelMutated += OnNoteMutated;
        _viewModelRegistry.NoteSelected         += OnNoteSelected;
        _parentTrack.PropertyChanged            += OnParentTrackPropertyChanged;
    }

    public void Dispose()
    {
        _viewModelRegistry.AllNoteViewModels.CollectionChanged -= OnAllNotesCollectionChanged;
        _viewModelRegistry.NoteViewModelMutated -= OnNoteMutated;
        _viewModelRegistry.NoteSelected         -= OnNoteSelected;
        _parentTrack.PropertyChanged            -= OnParentTrackPropertyChanged;
    }

    // ── Internal registry events ──────────────────────────────────────────

    private void OnNoteSelected(int noteId)
    {
        if (SelectedNote is not null && SelectedNote.Id != noteId)
            SelectedNote = null;
    }

    private void OnAllNotesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is not null)
            foreach (NoteViewModel note in e.NewItems)
                if (BelongsToThisSection(note))
                    _sectionSource.Add(note);

        if (e.OldItems is not null)
            foreach (NoteViewModel note in e.OldItems)
                _sectionSource.Remove(note);
    }

    private void OnNoteMutated(NoteMutatedArgs args)
    {
        if (_isReindexing) return;
        if (args.OwnerId != _ownerId || args.OwnerType != _ownerType) return;

        // A mutation may have changed NoteState or NoteTrackDefinitionId, so
        // re-evaluate membership for just the mutated note.
        var note = _viewModelRegistry.AllNoteViewModels.FirstOrDefault(n => n.Id == args.NoteId);
        if (note is not null)
        {
            bool shouldBeHere = BelongsToThisSection(note);
            bool isHere       = _sectionSource.Contains(note);

            if (shouldBeHere && !isHere)
                _sectionSource.Add(note);
            else if (!shouldBeHere && isHere)
                _sectionSource.Remove(note);
        }

        // Normalise the 1-based SortOrder sequence after a note may have left.
        var current = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        _isReindexing = true;
        try   { ReindexSection(current); }
        finally { _isReindexing = false; }
    }

    private void OnParentTrackPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NoteTrackViewModel.HasNotes))
        {
            OnPropertyChanged(nameof(IsVisible));
            OnPropertyChanged(nameof(MinDropHeight));
        }
    }

    private bool BelongsToThisSection(NoteViewModel note)
    {
        if (note.OwnerId != _ownerId || note.OwnerType != _ownerType) return false;
        if (note.NoteState != _targetState) return false;

        bool isUnassignedTrack = _definition.Id == UnassignedTrack.Definition.Id;
        return isUnassignedTrack
            ? note.NoteTrackDefinitionId == null
            : note.NoteTrackDefinitionId == _definition.Id;
    }

    // ── Reorder commands ──────────────────────────────────────────────────

    [RelayCommand]
    private void MoveNoteUp()
    {
        if (IsReadOnly || SelectedNote is null) return;
        var note  = SelectedNote;
        var notes = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        int index = notes.IndexOf(note);
        if (index <= 0) return;
        notes.RemoveAt(index);
        notes.Insert(index - 1, note);
        ReindexSection(notes);
        _viewModelRegistry.RaiseNoteMutated(new NoteMutatedArgs(
            note.Id, note.OwnerId, note.OwnerType, note.NoteTrackDefinitionId));
        SelectedNote = note;
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void MoveNoteDown()
    {
        if (IsReadOnly || SelectedNote is null) return;
        var note  = SelectedNote;
        var notes = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        int index = notes.IndexOf(note);
        if (index < 0 || index >= notes.Count - 1) return;
        notes.RemoveAt(index);
        notes.Insert(index + 1, note);
        ReindexSection(notes);
        _viewModelRegistry.RaiseNoteMutated(new NoteMutatedArgs(
            note.Id, note.OwnerId, note.OwnerType, note.NoteTrackDefinitionId));
        SelectedNote = note;
        _ = _storyService.SaveAsync();
    }

    // ── Drag-drop ─────────────────────────────────────────────────────────

    public void DragOver(IDropInfo dropInfo)
    {
        if (IsReadOnly) { dropInfo.Effects = System.Windows.DragDropEffects.None; return; }
        if (dropInfo.Data is NoteViewModel)
        {
            if (_targetState == NoteState.Confirmed && !CanPromoteToConfirmed)
            { dropInfo.Effects = System.Windows.DragDropEffects.None; return; }
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
        else { dropInfo.Effects = System.Windows.DragDropEffects.None; }
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (IsReadOnly) return;
        if (dropInfo.Data is not NoteViewModel note) return;
        if (_targetState == NoteState.Confirmed && !CanPromoteToConfirmed) return;

        var sectionNotes = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        int originalIndex = sectionNotes.FindIndex(n => n.Id == note.Id);
        int insertAt      = dropInfo.InsertIndex;
        if (originalIndex >= 0 && originalIndex < insertAt) insertAt--;
        sectionNotes.RemoveAll(n => n.Id == note.Id);
        insertAt = Math.Clamp(insertAt, 0, sectionNotes.Count);
        sectionNotes.Insert(insertAt, note);
        ReindexSection(sectionNotes);

        note.OwnerId               = _ownerId;
        note.OwnerType             = _ownerType;
        note.NoteTrackDefinitionId = _definition.Id == UnassignedTrack.Definition.Id ? null : _definition.Id;
        note.NoteState             = _targetState;

        _viewModelRegistry.RaiseNoteMutated(new NoteMutatedArgs(
            note.Id, note.OwnerId, note.OwnerType, note.NoteTrackDefinitionId));
        _ = _storyService.SaveAsync();
    }

    // ── State-change commands ─────────────────────────────────────────────

    [RelayCommand]
    private void PromoteNote()
    {
        if (IsReadOnly || SelectedNote is null) return;
        SelectedNote.NoteState = SelectedNote.NoteState switch
        {
            NoteState.Flagged                          => NoteState.Unset,
            NoteState.Unset when CanPromoteToConfirmed => NoteState.Confirmed,
            _                                          => SelectedNote.NoteState,
        };
        _viewModelRegistry.RaiseNoteMutated(new NoteMutatedArgs(
            SelectedNote.Id, SelectedNote.OwnerId, SelectedNote.OwnerType, SelectedNote.NoteTrackDefinitionId));
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void DemoteNote()
    {
        if (IsReadOnly || SelectedNote is null) return;
        SelectedNote.NoteState = SelectedNote.NoteState switch
        {
            NoteState.Confirmed => NoteState.Unset,
            NoteState.Unset     => NoteState.Flagged,
            _                   => SelectedNote.NoteState,
        };
        _viewModelRegistry.RaiseNoteMutated(new NoteMutatedArgs(
            SelectedNote.Id, SelectedNote.OwnerId, SelectedNote.OwnerType, SelectedNote.NoteTrackDefinitionId));
        _ = _storyService.SaveAsync();
    }

    public event Action<NoteViewModel>? SelectionTransferRequested;

    [RelayCommand]
    private void ToggleConfirmed()
    {
        if (IsReadOnly || SelectedNote is null) return;
        var note = SelectedNote;
        var newState = note.NoteState switch
        {
            NoteState.Confirmed                        => NoteState.Unset,
            NoteState.Unset when CanPromoteToConfirmed => NoteState.Confirmed,
            _                                          => note.NoteState,
        };
        if (newState != note.NoteState) PlaceAtEndOfSection(note, newState);
        note.NoteState = newState;
        _viewModelRegistry.RaiseNoteMutated(new NoteMutatedArgs(
            note.Id, note.OwnerId, note.OwnerType, note.NoteTrackDefinitionId));
        _ = _storyService.SaveAsync();
        if (note.NoteState != _targetState) SelectionTransferRequested?.Invoke(note);
    }

    [RelayCommand]
    private void ToggleFlagged()
    {
        if (IsReadOnly || SelectedNote is null) return;
        var note = SelectedNote;
        var newState = note.NoteState switch
        {
            NoteState.Flagged => NoteState.Unset,
            NoteState.Unset   => NoteState.Flagged,
            _                 => note.NoteState,
        };
        if (newState != note.NoteState) PlaceAtEndOfSection(note, newState);
        note.NoteState = newState;
        _viewModelRegistry.RaiseNoteMutated(new NoteMutatedArgs(
            note.Id, note.OwnerId, note.OwnerType, note.NoteTrackDefinitionId));
        _ = _storyService.SaveAsync();
        if (note.NoteState != _targetState) SelectionTransferRequested?.Invoke(note);
    }

    [RelayCommand]
    private void DeleteNote(NoteViewModel note)
    {
        if (IsReadOnly || note is null) return;
        if (SelectedNote?.Id == note.Id) SelectedNote = null;
        var mutationArgs = new NoteMutatedArgs(
            note.Id, note.OwnerId, note.OwnerType, note.NoteTrackDefinitionId);
        _viewModelRegistry.AllNoteViewModels.Remove(note);
        _storyService.DeleteNote(note.Id);
        _viewModelRegistry.RaiseNoteMutated(mutationArgs);
        _ = _storyService.SaveAsync();
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private static void ReindexSection(List<NoteViewModel> orderedNotes)
    {
        for (int i = 0; i < orderedNotes.Count; i++)
            orderedNotes[i].SortOrder = i + 1;
    }

    private void PlaceAtEndOfSection(NoteViewModel note, NoteState destinationState)
    {
        int max = _viewModelRegistry.AllNoteViewModels
            .Where(n => n.OwnerId   == _ownerId
                     && n.OwnerType == _ownerType
                     && n.NoteTrackDefinitionId == note.NoteTrackDefinitionId
                     && n.NoteState == destinationState
                     && n.Id        != note.Id)
            .Select(n => n.SortOrder)
            .DefaultIfEmpty(0)
            .Max();
        note.SortOrder = max + 1;
    }
}
