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

    public string SectionHeader => _targetState.ToString();
    public NoteState TargetState => _targetState;

    // Authoritative sorted list for this section.
    // Owned entirely by this class — no CVS, no live-shaping, no WPF filter subscriptions.
    // ObservableCollection.Move() produces targeted Move notifications identical to what
    // ICollectionViewLiveShaping emitted, without the per-note PropertyChanged overhead.
    private readonly ObservableCollection<NoteViewModel> _sectionSource = new();
    public ReadOnlyObservableCollection<NoteViewModel> SectionNotes { get; }

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

    public bool IsVisible    => _targetState == NoteState.Unset || _parentTrack.HasNotes;
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

        SectionNotes = new ReadOnlyObservableCollection<NoteViewModel>(_sectionSource);

        foreach (var n in viewModelRegistry.AllNoteViewModels
                     .Where(BelongsToThisSection)
                     .OrderBy(n => n.SortOrder))
            _sectionSource.Add(n);

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
                    InsertSorted(note);

        if (e.OldItems is not null)
            foreach (NoteViewModel note in e.OldItems)
                _sectionSource.Remove(note);
    }

    private void OnNoteMutated(NoteMutatedArgs args)
    {
        if (args.OwnerId != _ownerId || args.OwnerType != _ownerType) return;

        var note = _viewModelRegistry.AllNoteViewModels.FirstOrDefault(n => n.Id == args.NoteId);
        if (note is null) return;

        bool shouldBeHere = BelongsToThisSection(note);
        bool isHere       = _sectionSource.Contains(note);

        if (shouldBeHere && !isHere)
        {
            // Note transferred into this section — place it and close any gaps.
            InsertSorted(note);
            ReindexSection();
        }
        else if (!shouldBeHere && isHere)
        {
            // Note transferred out — close the gap it left.
            _sectionSource.Remove(note);
            ReindexSection();
        }
        // Membership unchanged: the command that raised the mutation already
        // called ReindexSection(), so no redundant pass is needed here.
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
        int index = _sectionSource.IndexOf(note);
        if (index <= 0) return;
        _sectionSource.Move(index, index - 1);
        ReindexSection();
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
        int index = _sectionSource.IndexOf(note);
        if (index < 0 || index >= _sectionSource.Count - 1) return;
        _sectionSource.Move(index, index + 1);
        ReindexSection();
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

        int originalIndex = _sectionSource.IndexOf(note);
        int insertAt      = dropInfo.InsertIndex;

        // Adjust for the gap left by removing the note from its current position.
        if (originalIndex >= 0 && originalIndex < insertAt) insertAt--;
        if (originalIndex >= 0) _sectionSource.RemoveAt(originalIndex);

        insertAt = Math.Clamp(insertAt, 0, _sectionSource.Count);
        _sectionSource.Insert(insertAt, note);
        ReindexSection();

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
        // Membership removal is handled uniformly by OnNoteMutated — no manual
        // _sectionSource.Remove needed here (unlike the old asymmetric approach).
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

    /// <summary>Inserts <paramref name="note"/> at the position its
    /// <see cref="NoteViewModel.SortOrder"/> dictates within the already-sorted
    /// <see cref="_sectionSource"/>.</summary>
    private void InsertSorted(NoteViewModel note)
    {
        int i = 0;
        while (i < _sectionSource.Count && _sectionSource[i].SortOrder <= note.SortOrder)
            i++;
        _sectionSource.Insert(i, note);
    }

    /// <summary>Rewrites 1-based <see cref="NoteViewModel.SortOrder"/> values using
    /// the current index in <see cref="_sectionSource"/> as the canonical order.</summary>
    private void ReindexSection()
    {
        for (int i = 0; i < _sectionSource.Count; i++)
            _sectionSource[i].SortOrder = i + 1;
    }

    private void PlaceAtEndOfSection(NoteViewModel note, NoteState destinationState)
    {
        int max = _viewModelRegistry.AllNoteViewModels
            .Where(n => n.OwnerId              == _ownerId
                     && n.OwnerType            == _ownerType
                     && n.NoteTrackDefinitionId == note.NoteTrackDefinitionId
                     && n.NoteState            == destinationState
                     && n.Id                   != note.Id)
            .Select(n => n.SortOrder)
            .DefaultIfEmpty(0)
            .Max();
        note.SortOrder = max + 1;
    }
}
