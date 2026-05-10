using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Linq;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class PlotPointSubjectLinkViewModel : NarrativeElementViewModel
    {
        private readonly PlotPointSubjectLink _link;
        public PlotPointSubjectLink Link => _link;

        public int Id => _link.Id;

        public int SubjectId
        {
            get => _link.SubjectId;
            set => SetProperty(_link.SubjectId, value, _link, (l, n) => l.SubjectId = n);
        }

        public int PlotPointId
        {
            get => _link.PlotPointId;
            set => SetProperty(_link.PlotPointId, value, _link, (l, n) => l.PlotPointId = n);
        }

        public int ChapterOrderIndex =>
            _viewModelRegistry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == _link.PlotPointId)
                ?.ChapterId is int chapterId
                    ? _viewModelRegistry.AllChapterViewModels
                        .FirstOrDefault(c => c.Id == chapterId)
                        ?.OrderIndex ?? int.MaxValue
                    : int.MaxValue;

        public int PlotPointOrderInChapter =>
            _viewModelRegistry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == _link.PlotPointId)
                ?.OrderInChapter ?? int.MaxValue;

        public PlotPointSubjectLinkViewModel(
            PlotPointSubjectLink link,
            IViewModelRegistry viewModelRegistry,
            IStoryService storyService,
            IContentFactory editorCoordinator)
            : base(viewModelRegistry, storyService, editorCoordinator)
        {
            _link = link;

            var subjectDefId = storyService.Subjects
                .First(s => s.Id == link.SubjectId)
                .SubjectDefinitionId;

            var noteTracks = storyService.NoteTrackDefinitions
                .Where(ntd => ntd.OwnerType == OwnerType.PlotPointSubjectLink
                           && ntd.SubjectDefinitionId == subjectDefId)
                .ToList();

            var propertyDefs = storyService.NarrativePropertyDefinitions
                .Where(npd => npd.OwnerType == OwnerType.PlotPointSubjectLink
                           && npd.SubjectDefinitionId == subjectDefId)
                .ToList();

            InitializeCollections(link.Id, OwnerType.PlotPointSubjectLink, noteTracks, propertyDefs);
        }
    }
}
