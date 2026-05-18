using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
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

    // Guard against re-entrant OnNoteMutated calls triggered by ReindexSection
    // writing SortOrder. If SortOrder ever raises NoteViewModelMutated in future,
    // this prevents an infinite loop.
    private bool _isReindexing;

    public string SectionHeader => _targetState.ToString();
    public NoteState TargetState => _targetState;
    public ICollectionView SectionNotes { get; }
    private readonly CollectionViewSource _cvs;

    [ObservableProperty]
    private NoteViewModel? _selectedNote;

    partial void OnSelectedNoteChanged(NoteViewModel? value)
    {
        // When this section gains a selection, broadcast it so all other sections
        // across the entire NarrativeElementFullView clear their own selection.
        if (value is not null)
            _viewModelRegistry.RaiseNoteSelected(value.Id);
    }

    public NoteTrackSectionViewModel(
        int ownerId,
        OwnerType ownerType,
        NoteTrackDefinition definition,
        NoteState targetState,
        IViewModelRegistry viewModelRegistry,
        IStoryService storyService)
    {
        _ownerId           = ownerId;
        _ownerType         = ownerType;
        _definition        = definition;
        _targetState       = targetState;
        _storyService      = storyService;
        _viewModelRegistry = viewModelRegistry;

        _cvs = new CollectionViewSource { Source = viewModelRegistry.AllNoteViewModels };
        SectionNotes = _cvs.View;
        SectionNotes.Filter = FilterNote;
        SectionNotes.SortDescriptions.Add(
            new SortDescription(nameof(NoteViewModel.SortOrder), ListSortDirection.Ascending));

        _viewModelRegistry.NoteViewModelMutated += OnNoteMutated;
        _viewModelRegistry.NoteSelected         += OnNoteSelected;
    }

    public void Dispose()
    {
        _viewModelRegistry.NoteViewModelMutated -= OnNoteMutated;
        _viewModelRegistry.NoteSelected         -= OnNoteSelected;
    }

    // Clear this section's selection when another section anywhere in the
    // application has selected a different note.
    private void OnNoteSelected(int noteId)
    {
        if (SelectedNote is not null && SelectedNote.Id != noteId)
            SelectedNote = null;
    }

    private void OnNoteMutated(int noteId)
    {
        // Guard: ReindexSection writes SortOrder on NoteViewModels. If SortOrder
        // is ever wired to raise NoteViewModelMutated, this prevents re-entrant
        // calls cascading back into here infinitely.
        if (_isReindexing) return;

        SectionNotes.Refresh();

        var current = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();

        _isReindexing = true;
        try
        {
            ReindexSection(current);
        }
        finally
        {
            _isReindexing = false;
        }
    }

    private bool FilterNote(object obj)
    {
        if (obj is not NoteViewModel note) return false;

        bool isUnassignedTrack = _definition.Id == UnassignedTrack.Definition.Id;

        return note.OwnerId    == _ownerId
            && note.OwnerType  == _ownerType
            && note.NoteState  == _targetState
            && (isUnassignedTrack
                ? note.NoteTrackDefinitionId == null
                : note.NoteTrackDefinitionId == _definition.Id);
    }

    [RelayCommand]
    private void MoveNoteUp()
    {
        if (SelectedNote is null) return;

        var note  = SelectedNote; // capture before Refresh() can clear the binding
        var notes = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        int index = notes.IndexOf(note);
        if (index <= 0) return;

        notes.RemoveAt(index);
        notes.Insert(index - 1, note);

        ReindexSection(notes);
        _viewModelRegistry.RaiseNoteMutated(note.Id); // triggers Refresh() synchronously
        SelectedNote = note;                           // re-assert after refresh clears it
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void MoveNoteDown()
    {
        if (SelectedNote is null) return;

        var note  = SelectedNote;
        var notes = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        int index = notes.IndexOf(note);
        if (index < 0 || index >= notes.Count - 1) return;

        notes.RemoveAt(index);
        notes.Insert(index + 1, note);

        ReindexSection(notes);
        _viewModelRegistry.RaiseNoteMutated(note.Id);
        SelectedNote = note;
        _ = _storyService.SaveAsync();
    }

    public void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is NoteViewModel)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
        else
        {
            dropInfo.Effects = System.Windows.DragDropEffects.None;
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not NoteViewModel note) return;

        var sectionNotes = SectionNotes
            .Cast<NoteViewModel>()
            .OrderBy(n => n.SortOrder)
            .ToList();

        int originalIndex = sectionNotes.FindIndex(n => n.Id == note.Id);
        int insertAt      = dropInfo.InsertIndex;

        if (originalIndex >= 0 && originalIndex < insertAt)
            insertAt--;

        sectionNotes.RemoveAll(n => n.Id == note.Id);
        insertAt = Math.Clamp(insertAt, 0, sectionNotes.Count);
        sectionNotes.Insert(insertAt, note);

        ReindexSection(sectionNotes);

        note.OwnerId               = _ownerId;
        note.OwnerType             = _ownerType;
        note.NoteTrackDefinitionId = _definition.Id == UnassignedTrack.Definition.Id
                                        ? null
                                        : _definition.Id;
        note.NoteState             = _targetState;

        _viewModelRegistry.RaiseNoteMutated(note.Id);
        _ = _storyService.SaveAsync();
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    /// <summary>
    /// Assigns contiguous 1-based SortOrder values to the supplied ordered list.
    /// Call this after any operation that changes note ordering within a section.
    /// </summary>
    private static void ReindexSection(List<NoteViewModel> orderedNotes)
    {
        for (int i = 0; i < orderedNotes.Count; i++)
            orderedNotes[i].SortOrder = (i + 1);
    }

    [RelayCommand]
    private void PromoteNote()
    {
        if (SelectedNote is null) return;

        SelectedNote.NoteState = SelectedNote.NoteState switch
        {
            NoteState.Flagged => NoteState.Unset,
            NoteState.Unset   => NoteState.Confirmed,
            _                 => SelectedNote.NoteState
        };

        _viewModelRegistry.RaiseNoteMutated(SelectedNote.Id);
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void DemoteNote()
    {
        if (SelectedNote is null) return;

        SelectedNote.NoteState = SelectedNote.NoteState switch
        {
            NoteState.Confirmed => NoteState.Unset,
            NoteState.Unset     => NoteState.Flagged,
            _                   => SelectedNote.NoteState
        };

        _viewModelRegistry.RaiseNoteMutated(SelectedNote.Id);
        _ = _storyService.SaveAsync();
    }

    // Raised when a hotkey moves a note out of this section so the track VM
    // can transfer selection to whichever section now owns it.
    public event Action<NoteViewModel>? SelectionTransferRequested;

    [RelayCommand]
    private void ToggleConfirmed()
    {
        if (SelectedNote is null) return;
        var note = SelectedNote;

        var newState = note.NoteState switch
        {
            NoteState.Confirmed => NoteState.Unset,
            NoteState.Unset     => NoteState.Confirmed,
            _                   => note.NoteState
        };

        if (newState != note.NoteState)
            PlaceAtEndOfSection(note, newState);

        note.NoteState = newState;
        _viewModelRegistry.RaiseNoteMutated(note.Id);
        _ = _storyService.SaveAsync();

        if (note.NoteState != _targetState)
            SelectionTransferRequested?.Invoke(note);
    }

    [RelayCommand]
    private void ToggleFlagged()
    {
        if (SelectedNote is null) return;
        var note = SelectedNote;

        var newState = note.NoteState switch
        {
            NoteState.Flagged => NoteState.Unset,
            NoteState.Unset   => NoteState.Flagged,
            _                 => note.NoteState
        };

        if (newState != note.NoteState)
            PlaceAtEndOfSection(note, newState);

        note.NoteState = newState;
        _viewModelRegistry.RaiseNoteMutated(note.Id);
        _ = _storyService.SaveAsync();

        if (note.NoteState != _targetState)
            SelectionTransferRequested?.Invoke(note);
    }

    // Sets the note's SortOrder to one past the current maximum in the destination
    // section so it arrives at the end rather than being sorted by its stale order.
    // Must be called before NoteState is changed so the destination filter still
    // excludes the note when computing the max.
    private void PlaceAtEndOfSection(NoteViewModel note, NoteState destinationState)
    {
        // Find the sibling section VM for the destination state via the registry —
        // filter the same AllNoteViewModels source directly to avoid depending on
        // the parent track VM from here.
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

    [RelayCommand]
    private void DeleteNote(NoteViewModel note)
    {
        if (note is null) return;

        // Clear selection if this note was selected so _selectedNote
        // in CommonWindow doesn't hold a dangling reference.
        if (SelectedNote?.Id == note.Id)
            SelectedNote = null;

        // Remove from the registry's AllNoteViewModels so it's filtered out
        // when SectionNotes.Refresh() is called below.
        _viewModelRegistry.AllNoteViewModels.Remove(note);

        _storyService.DeleteNote(note.Id);
        _viewModelRegistry.RaiseNoteMutated(note.Id);
        _ = _storyService.SaveAsync();
    }
}
