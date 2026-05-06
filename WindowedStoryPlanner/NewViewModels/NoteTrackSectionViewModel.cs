using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels
{
    /// <summary>
    /// Represents one of the three state-filtered sections (Confirmed, Unset, Flagged)
    /// within a single NoteTrackViewModel. Owns its own ICollectionView over
    /// AllNoteViewModels, filtered by OwnerId + NoteTrackDefinitionId + NoteState.
    /// Implements IDropTarget to accept NoteViewModel drops and reject everything else
    /// so that PlotPoint/Subject drops bubble up to the owning window.
    /// </summary>
    public partial class NoteTrackSectionViewModel : ObservableObject, IDropTarget
    {
        private readonly int _ownerId;
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
            NoteTrackDefinition definition,
            NoteState targetState,
            IViewModelRegistry viewModelRegistry,
            IStoryService storyService)
        {
            _ownerId = ownerId;
            _definition = definition;
            _targetState = targetState;
            _storyService = storyService;
            _viewModelRegistry = viewModelRegistry;

            // Each section owns an independent view — NOT GetDefaultView —
            // so each can have its own filter without overwriting siblings.
            var cvs = new CollectionViewSource
            {
                Source = viewModelRegistry.AllNoteViewModels
            };
            SectionNotes = cvs.View;
            SectionNotes.Filter = FilterNote;
            SectionNotes.SortDescriptions.Add(
                new SortDescription(nameof(NoteViewModel.SortOrder), ListSortDirection.Ascending));

            // Subscribe to the registry broadcast so this section refreshes
            // whenever any drop handler mutates a note's filter-relevant properties.
            _viewModelRegistry.NoteViewModelMutated += OnNoteMutated;
        }

        /// <summary>
        /// Call this when the owning window closes to prevent memory leaks
        /// from the event subscription keeping this object alive.
        /// </summary>
        public void Dispose()
        {
            _viewModelRegistry.NoteViewModelMutated -= OnNoteMutated;
        }

        private void OnNoteMutated(int noteId)
        {
            // Re-evaluate the filter and re-sort. This is cheap:
            // O(n notes) predicate passes, only on active (open-window) sections.
            SectionNotes.Refresh();
        }

        private bool FilterNote(object obj)
        {
            if (obj is not NoteViewModel note) return false;
            return note.OwnerId == _ownerId
                && note.NoteTrackDefinitionId == _definition.Id
                && note.NoteState == _targetState;
        }

        // ── Keyboard Reorder Commands ────────────────────────────────────────

        [RelayCommand]
        private void MoveNoteUp()
        {
            if (SelectedNote is null) return;

            var notes = SectionNotes
                .Cast<NoteViewModel>()
                .OrderBy(n => n.SortOrder)
                .ToList();

            int index = notes.IndexOf(SelectedNote);
            if (index <= 0) return; // already at top

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

            var notes = SectionNotes
                .Cast<NoteViewModel>()
                .OrderBy(n => n.SortOrder)
                .ToList();

            int index = notes.IndexOf(SelectedNote);
            if (index < 0 || index >= notes.Count - 1) return; // already at bottom

            notes.RemoveAt(index);
            notes.Insert(index + 1, SelectedNote);

            ReindexSection(notes);
            _viewModelRegistry.RaiseNoteMutated(SelectedNote.Id);
            _ = _storyService.SaveAsync();
        }

        // ── GongSolutions IDropTarget ────────────────────────────────────────

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is NoteViewModel)
            {
                // Accept: show the insert-line adorner between items.
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = System.Windows.DragDropEffects.Move;
            }
            else
            {
                // Reject: setting None without marking Handled lets the event
                // bubble up to the owning Window where OwnerViewModel.DragOver
                // can accept PlotPoint/Subject drops.
                dropInfo.Effects = System.Windows.DragDropEffects.None;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is not NoteViewModel note) return;

            // 1. Collect the current section contents excluding the dragged note,
            //    ordered by SortOrder, so InsertIndex maps to the correct slot.
            var sectionNotes = SectionNotes
                .Cast<NoteViewModel>()
                .Where(n => n.Id != note.Id)
                .OrderBy(n => n.SortOrder)
                .ToList();

            // 2. Clamp and splice.
            int insertAt = Math.Clamp(dropInfo.InsertIndex, 0, sectionNotes.Count);
            sectionNotes.Insert(insertAt, note);

            // 3. Reindex the full section densely with spacing of 10.
            ReindexSection(sectionNotes);

            // 4. Mutate the state fields that determine which section/track/owner
            //    this note belongs to. Property setters fire OnPropertyChanged.
            note.OwnerId = _ownerId;
            note.NoteTrackDefinitionId = _definition.Id;
            note.NoteState = _targetState;

            // 5. Broadcast so every other active section refreshes its view.
            //    This section will also receive OnNoteMutated and self-refresh.
            _viewModelRegistry.RaiseNoteMutated(note.Id);

            // 6. Persist asynchronously. Fire-and-forget is acceptable here:
            //    the UI state is already correct; the save is a background concern.
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
    }
}
