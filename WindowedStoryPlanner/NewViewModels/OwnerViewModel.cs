using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class OwnerViewModel : ObservableObject, IDropTarget
    {
        protected readonly IStoryService _storyService;
        protected readonly IViewModelRegistry _viewModelRegistry;
        protected readonly IEditorCoordinator _editorCoordinator;

        [ObservableProperty]
        private NoteViewModel? _selectedNote;

        public ObservableCollection<NoteTrackViewModel> NoteTracks { get; set; } = new();
        public ObservableCollection<NarrativePropertyViewModel> NarrativeProperties { get; set; } = new();

        public OwnerViewModel(
            IViewModelRegistry viewModelRegistry,
            IStoryService storyService,
            IEditorCoordinator editorCoordinator)
        {
            _storyService = storyService;
            _viewModelRegistry = viewModelRegistry;
            _editorCoordinator = editorCoordinator;
        }

        protected void InitializeCollections(
            int ownerId,
            OwnerType ownerType,
            List<NoteTrackDefinition> noteTrackDefinitions,
            List<NarrativePropertyDefinition> narrativePropertyDefinitions)
        {
            foreach (var ntd in noteTrackDefinitions)
            {
                var trackVm = new NoteTrackViewModel(
                    ntd,
                    ownerId,
                    ownerType,
                    _viewModelRegistry,
                    _storyService,
                    _editorCoordinator,       // ← threaded through
                    OnSectionSelectionChanged);
                NoteTracks.Add(trackVm);
            }

            foreach (var npd in narrativePropertyDefinitions)
            {
                var propVm = new NarrativePropertyViewModel(
                    ownerId,
                    ownerType,
                    npd,
                    _viewModelRegistry,
                    _storyService);
                NarrativeProperties.Add(propVm);
            }
        }

        /// <summary>
        /// Called by any NoteTrackSectionViewModel when its ListBox selection changes.
        /// Keeps OwnerViewModel.SelectedNote in sync for F-key hotkey commands.
        /// </summary>
        private void OnSectionSelectionChanged(NoteViewModel? note)
        {
            SelectedNote = note;
        }

        public void OnWindowOpened()
        {
            if (NoteTracks.Count == 0) return;
            foreach (var track in NoteTracks)
                track.Initialize();
        }

        public void OnWindowClosed()
        {
            foreach (var track in NoteTracks)
                track.Uninitialize();
        }

        public void MoveSelectedNoteToTrack(int noteTrackDefinitionId)
        {
            if (SelectedNote is null) return;

            var targetTrack = NoteTracks
                .FirstOrDefault(t => t.Definition.Id == noteTrackDefinitionId);
            if (targetTrack is null) return;

            var unsetSection = targetTrack.Sections
                .FirstOrDefault(s => s.TargetState == NoteState.Unset);

            int maxOrder = unsetSection?.SectionNotes
                .Cast<NoteViewModel>()
                .Select(n => n.SortOrder)
                .DefaultIfEmpty(0)
                .Max() ?? 0;

            SelectedNote.NoteTrackDefinitionId = noteTrackDefinitionId;
            SelectedNote.NoteState = NoteState.Unset;
            SelectedNote.SortOrder = maxOrder + 10;

            _viewModelRegistry.RaiseNoteMutated(SelectedNote.Id);
            _ = _storyService.SaveAsync();
        }

        // ── GongSolutions IDropTarget (PlotPoint ↔ Subject linking) ──────────

        public virtual void DragOver(IDropInfo dropInfo) { }
        public virtual async void Drop(IDropInfo dropInfo)
        {
            var source = dropInfo.Data;
            var target = this;

            switch (source, target)
            {
                case (SubjectViewModel s, PlotPointViewModel p):
                    await _editorCoordinator.CreatePlotPointSubjectLinkAsync(p, s); break;
                case (PlotPointViewModel p, SubjectViewModel s):
                    await _editorCoordinator.CreatePlotPointSubjectLinkAsync(p, s); break;
                case (PlotPointSubjectLinkViewModel l, PlotPointViewModel p):
                    l.PlotPointId = p.Id; break;
                case (PlotPointSubjectLinkViewModel l, SubjectViewModel s):
                    l.SubjectId = s.Id; break;
            }
        }

        public void RegisterTrackHotkeys(Window window)
        {
            foreach (var track in NoteTracks.Where(t => t.FunctionKeyNumber.HasValue))
            {
                var key = KeyFromFunctionNumber(track.FunctionKeyNumber!.Value);
                var definitionId = track.Definition.Id;

                window.InputBindings.Add(new KeyBinding(
                    new RelayCommand(() => MoveSelectedNoteToTrack(definitionId)),
                    key,
                    ModifierKeys.None));
            }
        }

        private static Key KeyFromFunctionNumber(int n) => n switch
        {
            1  => Key.F1,  2  => Key.F2,  3  => Key.F3,  4  => Key.F4,
            5  => Key.F5,  6  => Key.F6,  7  => Key.F7,  8  => Key.F8,
            9  => Key.F9,  10 => Key.F10, 11 => Key.F11, 12 => Key.F12,
            _ => throw new ArgumentOutOfRangeException(nameof(n))
        };
    }
}
