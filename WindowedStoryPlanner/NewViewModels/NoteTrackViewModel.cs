using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels
{
    /// <summary>
    /// Represents a single note track for a single owner.
    /// Owns three NoteTrackSectionViewModels (Confirmed, Unset, Flagged)
    /// which are created lazily via Initialize() when the owning window opens.
    /// </summary>
    public partial class NoteTrackViewModel : ObservableObject
    {
        private readonly int _ownerId;
        private readonly OwnerType _ownerType;
        private readonly NoteTrackDefinition _definition;
        private readonly IStoryService _storyService;
        private readonly IViewModelRegistry _viewModelRegistry;
        private readonly IEditorCoordinator _editorCoordinator;

        public ObservableCollection<NoteTrackSectionViewModel> Sections { get; } = new();

        public int DisplayOrder => _definition.DisplayOrder;
        public string DisplayQuestion => _definition.DisplayQuestion;
        public string TrackName => _definition.TrackName;
        public string Explanation => _definition.Explanation;
        public CognitiveMode CognitiveMode => _definition.CognitiveMode;
        public NoteTrackDefinition Definition => _definition;

        /// <summary>
        /// Set by CommonWindow when this track is placed in a panel.
        /// F1–F6 for the left panel, F7–F12 for the right panel, based on DisplayOrder.
        /// Null when the track has no assigned key (more than 6 tracks in a panel).
        /// Observed by NoteTrackView to show/hide the F-key badge.
        /// </summary>
        [ObservableProperty]
        private int? _assignedFunctionKey;

        public NoteTrackViewModel(
            NoteTrackDefinition definition,
            int ownerId,
            OwnerType ownerType,
            IViewModelRegistry registry,
            IStoryService storyService,
            IEditorCoordinator editorCoordinator)
        {
            _definition         = definition;
            _ownerId            = ownerId;
            _ownerType          = ownerType;
            _storyService       = storyService;
            _viewModelRegistry  = registry;
            _editorCoordinator  = editorCoordinator;
        }

        /// <summary>
        /// Creates the three NoteTrackSectionViewModels and their ICollectionViews.
        /// Call from OwnerViewModel.OnWindowOpened().
        /// </summary>
        public void Initialize()
        {
            if (Sections.Count > 0) return;

            // Order is intentional: Confirmed (stable) → Unset (provisional) → Flagged (needs work)
            Sections.Add(new NoteTrackSectionViewModel(
                _ownerId, _definition, NoteState.Confirmed,
                _viewModelRegistry, _storyService));

            Sections.Add(new NoteTrackSectionViewModel(
                _ownerId, _definition, NoteState.Unset,
                _viewModelRegistry, _storyService));

            Sections.Add(new NoteTrackSectionViewModel(
                _ownerId, _definition, NoteState.Flagged,
                _viewModelRegistry, _storyService));
        }

        /// <summary>
        /// Disposes all sections, unsubscribing from registry events.
        /// Call from OwnerViewModel.OnWindowClosed().
        /// </summary>
        public void Uninitialize()
        {
            foreach (var section in Sections)
                section.Dispose();
            Sections.Clear();
        }

        [RelayCommand]
        public async Task CreateNewNote()
        {
            // New notes land in Unset — the provisional/uncategorized state.
            var unsetSection = Sections.FirstOrDefault(s => s.TargetState == NoteState.Unset);

            int maxOrder = unsetSection?.SectionNotes
                .Cast<NoteViewModel>()
                .Select(n => n.SortOrder)
                .DefaultIfEmpty(0)
                .Max() ?? 0;

            // Delegate both model and ViewModel creation to IEditorCoordinator,
            // keeping IStoryService and AllNoteViewModels in sync atomically.
            await _editorCoordinator.CreateNoteAsync(_ownerId, _definition.Id, maxOrder + 10);
        }
    }
}
