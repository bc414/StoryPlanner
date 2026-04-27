using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class SubjectViewModel : OwnerViewModel
    {
        private Subject _subject;

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

        public SubjectViewModel(Subject subject, IViewModelRegistry viewModelRegistry, IStoryService storyService, IEditorCoordinator editorCoordinator)
            : base(viewModelRegistry, storyService, editorCoordinator)
        {
            _subject = subject;

            var noteTracks = storyService.NoteTrackDefinitions
                .Where(ntd => ntd.OwnerType == OwnerType.Subject
                           && ntd.SubjectDefinitionId == subject.SubjectDefinitionId)
                .ToList();

            var propertyDefs = storyService.NarrativePropertyDefinitions
                .Where(npd => npd.OwnerType == OwnerType.Subject
                           && npd.SubjectDefinitionId == subject.SubjectDefinitionId)
                .ToList();

            InitializeCollections(subject.Id, OwnerType.Subject,
                                  noteTracks, propertyDefs);
            //TODO: need own view, follow note track section pattern
            PlotPointSubjectLinks = CollectionViewSource.GetDefaultView(
                    viewModelRegistry.AllPlotPointSubjectLinkViewModels);
            PlotPointSubjectLinks.Filter = FilterLinks;
            PlotPointSubjectLinks.SortDescriptions.Add(
                new SortDescription(nameof(PlotPointSubjectLinkViewModel.ChapterOrderIndex), ListSortDirection.Ascending));
            PlotPointSubjectLinks.SortDescriptions.Add(
                new SortDescription(nameof(PlotPointSubjectLinkViewModel.PlotPointOrderInChapter), ListSortDirection.Ascending));
            PlotPointSubjectLinks.Refresh();
        }

        private bool FilterLinks(object obj)
        {
            if (obj is not PlotPointSubjectLinkViewModel link) return false;
            return link.SubjectId == _subject.Id;
        }
    }
}
