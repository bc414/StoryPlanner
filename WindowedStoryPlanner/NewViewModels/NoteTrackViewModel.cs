using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels
{
    /// <summary>
    /// Represents a single note track for a single owner
    /// </summary>
    public partial class NoteTrackViewModel : ObservableObject, IDropTarget
    {
        private readonly int _ownerId;
        private readonly OwnerType _ownerType;
        private readonly NoteTrackDefinition _definition;
        private readonly IStoryService _storyService;
        private readonly IViewModelRegistry _viewModelRegistry;

        public ICollectionView Notes { get; }

        public NoteTrackViewModel(
            NoteTrackDefinition definition,
            int ownerId,
            OwnerType ownerType,
            IViewModelRegistry registry,
            IStoryService storyService)
        {
            _definition = definition;
            _ownerId = ownerId;
            _ownerType = ownerType;
            _storyService = storyService;
            _viewModelRegistry = registry;

            //TODO: separate collection views for flagged, unset and confirmed
            Notes = CollectionViewSource.GetDefaultView(
                registry.AllNoteViewModels);
            Notes.Filter = FilterNote;
            Notes.SortDescriptions.Add(new SortDescription(nameof(Note.SortOrder), ListSortDirection.Ascending));
        }

        private bool FilterNote(object obj)
        {
            if (obj is not NoteViewModel note) return false;
            return note.OwnerId == _ownerId
                && note.NoteTrackDefinitionId == _definition.Id;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            
        }

        public void Drop(IDropInfo dropInfo)
        {
            
        }

        [RelayCommand]
        public async Task CreateNewNote()
        {
            Note? last = _storyService.Notes.Where(n => n.OwnerId == _ownerId && n.NoteTrackDefinitionId == _definition.Id).OrderBy(n => n.SortOrder).LastOrDefault();
            int orderToUse;
            if (last != null)
            {
                orderToUse = last.SortOrder + 1;
            }
            else
            {
                orderToUse = 1;
            }
            Note newNote = new Note()
            {
                OwnerId = _ownerId,
                NoteTrackDefinitionId = _definition.Id,
                SortOrder = orderToUse
            };
            _storyService.Notes.Add(newNote);
            await _storyService.SaveAsync();
            _viewModelRegistry.AllNoteViewModels.Add(new NoteViewModel(newNote));
        }

        public string DisplayQuestion => _definition.DisplayQuestion;
        public string TrackName => _definition.TrackName;
        public int? FunctionKeyNumber => _definition.FunctionKeyNumber;
    }
}
