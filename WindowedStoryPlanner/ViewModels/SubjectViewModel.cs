using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WindowedStoryPlanner.ViewModels
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

        public ICollectionView PlotPointSubjectLinks { get; set; }
        private readonly IWindowManager _windowManager;

        [RelayCommand]
        private void Open() => _windowManager.OpenCommonWindow(this);

        public SubjectViewModel(Subject subject, IViewModelRegistry viewModelRegistry, IStoryService storyService, IContentFactory editorCoordinator, IWindowManager windowManager)
            : base(viewModelRegistry, storyService, editorCoordinator)
        {
            _subject = subject;

            InitializeTracksAndProperties();

            //TODO: need own view, follow note track section pattern
            var view = new ListCollectionView(viewModelRegistry.AllPlotPointSubjectLinkViewModels)
            {
                Filter = obj => obj is PlotPointSubjectLinkViewModel link && link.SubjectId == _subject.Id,
                IsLiveSorting = true,
            };
            view.LiveSortingProperties.Add(nameof(PlotPointSubjectLinkViewModel.ChapterOrderIndex));
            view.LiveSortingProperties.Add(nameof(PlotPointSubjectLinkViewModel.PlotPointOrderInChapter));
            view.SortDescriptions.Add(new SortDescription(nameof(PlotPointSubjectLinkViewModel.ChapterOrderIndex), ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription(nameof(PlotPointSubjectLinkViewModel.PlotPointOrderInChapter), ListSortDirection.Ascending));
            PlotPointSubjectLinks = view;

            _windowManager = windowManager;
        }

        /// <summary>
        /// Changes the subject's type and rebuilds its note tracks and narrative properties
        /// to match the new SubjectDefinition.
        /// </summary>
        public void ChangeSubjectDefinition(int newSubjectDefinitionId)
        {
            if (_subject.SubjectDefinitionId == newSubjectDefinitionId) return;

            _subject.SubjectDefinitionId = newSubjectDefinitionId;

            NoteTracks.Clear();
            NarrativeProperties.Clear();
            InitializeTracksAndProperties();

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

            InitializeCollections(_subject.Id, OwnerType.Subject, noteTracks, propertyDefs);
        }

        public string BadgeText => !string.IsNullOrWhiteSpace(Abbreviation)
        ? Abbreviation
        : (Name.Length > 3 ? Name.Substring(0, 3) : Name).ToUpper();

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
