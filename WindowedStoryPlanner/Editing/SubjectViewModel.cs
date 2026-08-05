using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WindowedStoryPlanner
{
    public partial class SubjectViewModel : NarrativeElementViewModel
    {
        private Subject _subject;
        public Subject Subject => _subject;

        public int Id => _subject.Id;
        public int SubjectDefinitionId => _subject.SubjectDefinitionId;

        public string Name
        {
            get => _subject.Name;
            set => SetProperty(_subject.Name, value, _subject, (s, n) => s.Name = n);
        }

        public string Description
        {
            get => _subject.Description;
            set => SetProperty(_subject.Description, value, _subject, (s, n) => s.Description = n);
        }

        public string Abbreviation
        {
            get => _subject.Abbreviation;
            set => SetProperty(_subject.Abbreviation, value, _subject, (s, n) => s.Abbreviation = n);
        }

        public string ColorHex
        {
            get => _subject.ColorHex;
            set => SetProperty(_subject.ColorHex, value, _subject, (s, n) => s.ColorHex = n);
        }

        /// <summary>Authorial designation that this subject may narrate somewhere in
        /// third-person-limited — the only thing that populates a PlotPoint's focal-character
        /// picker (see PlotPointViewModel.FocalCharacterChoices).</summary>
        public bool IsPovCharacter
        {
            get => _subject.IsPovCharacter;
            set => SetProperty(_subject.IsPovCharacter, value, _subject, (s, n) => s.IsPovCharacter = n);
        }

        public ICollectionView? PlotPointSubjectLinks { get; private set; }
        private readonly IWindowManager _windowManager;

        [RelayCommand]
        private void Open() => _windowManager.OpenSubjectWindow(this);

        public SubjectViewModel(Subject subject, IViewModelRegistry viewModelRegistry, IStoryService storyService, IContentFactory editorCoordinator, IWindowManager windowManager, AppSettings appSettings, ExportService exportService)
            : base(viewModelRegistry, storyService, editorCoordinator, appSettings, exportService)
        {
            _subject = subject;

            InitializeTracksAndProperties();

            if (viewModelRegistry.IsStoryLoaded)
                BuildLinkView();

            _windowManager = windowManager;
        }

        protected override void OnStoryFullyLoaded()
        {
            BuildLinkView();
        }

        private void BuildLinkView()
        {
            //TODO: need own view, follow note track section pattern
            var view = new ListCollectionView(_viewModelRegistry.AllPlotPointSubjectLinkViewModels)
            {
                Filter = obj => obj is PlotPointSubjectLinkViewModel link && link.SubjectId == _subject.Id,
                IsLiveSorting = true,
            };
            view.LiveSortingProperties.Add(nameof(PlotPointSubjectLinkViewModel.ChapterOrderIndex));
            view.LiveSortingProperties.Add(nameof(PlotPointSubjectLinkViewModel.PlotPointOrderInChapter));
            view.SortDescriptions.Add(new SortDescription(nameof(PlotPointSubjectLinkViewModel.ChapterOrderIndex), ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription(nameof(PlotPointSubjectLinkViewModel.PlotPointOrderInChapter), ListSortDirection.Ascending));
            PlotPointSubjectLinks = view;
            OnPropertyChanged(nameof(PlotPointSubjectLinks));
        }

        /// <summary>
        /// Changes the subject's type and rebuilds its note tracks and narrative properties
        /// to match the new SubjectDefinition.
        /// </summary>
        public void ChangeSubjectDefinition(int newSubjectDefinitionId)
        {
            if (_subject.SubjectDefinitionId == newSubjectDefinitionId) return;

            _subject.SubjectDefinitionId = newSubjectDefinitionId;

            // Re-register the factories for the new definition, then rebuild+initialize
            // tracks exactly as OnWindowOpened does — no manual Clear() needed here.
            InitializeTracksAndProperties();
            RebuildAndInitializeTracks();

            OnPropertyChanged(nameof(SubjectDefinitionId));
        }

        private void InitializeTracksAndProperties()
        {
            var noteTracks = _storyService.NoteTrackDefinitions
                .Where(ntd => ntd.OwnerType == OwnerType.Subject
                           && ntd.SubjectDefinitionId == _subject.SubjectDefinitionId)
                .ToList();

            var propertyDefs = _storyService.NarrativePropertyDefinitions
                .Where(npd => npd.OwnerType == OwnerType.Subject
                           && npd.SubjectDefinitionId == _subject.SubjectDefinitionId)
                .ToList();

            InitializeCollections(
                _subject.Id,
                OwnerType.Subject,
                () => _storyService.NoteTrackDefinitions
                          .Where(ntd => ntd.OwnerType == OwnerType.Subject
                                     && ntd.SubjectDefinitionId == _subject.SubjectDefinitionId)
                          .ToList(),
                () => _storyService.NarrativePropertyDefinitions
                          .Where(npd => npd.OwnerType == OwnerType.Subject
                                     && npd.SubjectDefinitionId == _subject.SubjectDefinitionId)
                          .OrderBy(npd => npd.DisplayOrder)
                          .ToList());
        }

        /// <summary>
        /// Rebuilt alongside the note tracks and narrative properties, so a relation authored in
        /// the Definitions tab while the app is running appears on the next window open rather
        /// than only after reopening the project.
        /// </summary>
        protected override void OnCollectionsRebuilt() => RebuildSubjectRelations();

        /// <summary>
        /// Edges this subject may draw, one picker per relation definition scoped to its type.
        /// Empty — and so invisible — until a relation is authored in the Definitions tab, which is
        /// why the migration changes nothing about an existing file's editor.
        ///
        /// Lives on SubjectViewModel rather than NarrativeElementViewModel because relations are
        /// Subject→Subject only; there is no polymorphic form of this and none should be added.
        /// </summary>
        public ObservableCollection<SubjectRelationViewModel> SubjectRelations { get; } = new();

        private void RebuildSubjectRelations()
        {
            SubjectRelations.Clear();

            foreach (var definition in _storyService.SubjectRelationDefinitions
                         .Where(d => d.SubjectDefinitionId == _subject.SubjectDefinitionId)
                         .OrderBy(d => d.DisplayOrder)
                         .ThenBy(d => d.Id))
                SubjectRelations.Add(new SubjectRelationViewModel(
                    _subject.Id, definition, _viewModelRegistry, _storyService));
        }

        public Brush BadgeBackground
        {
            get
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(
                        !string.IsNullOrEmpty(ColorHex) ? ColorHex : "#CCCCCC");
                    return new SolidColorBrush(color);
                }
                catch
                {
                    return Brushes.LightGray;
                }
            }
        }

        public Brush BadgeForeground
        {
            get
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(
                        !string.IsNullOrEmpty(ColorHex) ? ColorHex : "#CCCCCC");
                    // Luminance formula to determine if dark or light
                    return (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) < 186 ? Brushes.White : Brushes.Black;
                }
                catch
                {
                    return Brushes.Black;
                }
            }
        }

        /// <summary>All subject types the user can pick from.</summary>
        public IReadOnlyList<SubjectDefinitionViewModel> AvailableSubjectDefinitions =>
            _viewModelRegistry.AllSubjectDefinitionViewModels;

        /// <summary>
        /// The currently selected SubjectDefinition. Setting this calls ChangeSubjectDefinition.
        /// </summary>
        public SubjectDefinitionViewModel? SelectedSubjectDefinition
        {
            get => AvailableSubjectDefinitions.FirstOrDefault(s => s.Id == _subject.SubjectDefinitionId);
            set
            {
                if (value is null) return;
                ChangeSubjectDefinition(value.Id);
                OnPropertyChanged();
            }
        }
    }
}
