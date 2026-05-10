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

    public string SectionHeader => _targetState.ToString();
    public NoteState TargetState => _targetState;
    public ICollectionView SectionNotes { get; }

    [ObservableProperty]
    private NoteViewModel? _selectedNote;

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

        var cvs = new CollectionViewSource { Source = viewModelRegistry.AllNoteViewModels };
        SectionNotes = cvs.View;
        SectionNotes.Filter = FilterNote;
        SectionNotes.SortDescriptions.Add(
            new SortDescription(nameof(NoteViewModel.SortOrder), ListSortDirection.Ascending));

        _viewModelRegistry.NoteViewModelMutated += OnNoteMutated;
    }

    public void Dispose()
    {
        _viewModelRegistry.NoteViewModelMutated -= OnNoteMutated;
    }

    private void OnNoteMutated(int noteId) => SectionNotes.Refresh();

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

        var notes = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        int index = notes.IndexOf(SelectedNote);
        if (index <= 0) return;

        notes.RemoveAt(index);
        notes.Insert(index - 1, SelectedNote);

        ReindexSection(notes);
        _viewModelRegistry.RaiseNoteMutated(SelectedNote.Id);
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void MoveNoteDown()
    {
        if (SelectedNote is null) return;

        var notes = SectionNotes.Cast<NoteViewModel>().OrderBy(n => n.SortOrder).ToList();
        int index = notes.IndexOf(SelectedNote);
        if (index < 0 || index >= notes.Count - 1) return;

        notes.RemoveAt(index);
        notes.Insert(index + 1, SelectedNote);

        ReindexSection(notes);
        _viewModelRegistry.RaiseNoteMutated(SelectedNote.Id);
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
            .Where(n => n.Id != note.Id)
            .OrderBy(n => n.SortOrder)
            .ToList();

        int insertAt = Math.Clamp(dropInfo.InsertIndex, 0, sectionNotes.Count);
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
    /// Assigns SortOrder values of 10, 20, 30... to the provided ordered list.
    /// Call this after any operation that changes note ordering within a section.
    /// </summary>
    private static void ReindexSection(List<NoteViewModel> orderedNotes)
    {
        for (int i = 0; i < orderedNotes.Count; i++)
            orderedNotes[i].SortOrder = (i + 1) * 10;
    }

    [RelayCommand]
    private void PromoteNote()
    {
        if (SelectedNote is null) return;

        SelectedNote.NoteState = SelectedNote.NoteState switch
        {
            NoteState.Flagged   => NoteState.Unset,
            NoteState.Unset     => NoteState.Confirmed,
            _                   => SelectedNote.NoteState
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
}
