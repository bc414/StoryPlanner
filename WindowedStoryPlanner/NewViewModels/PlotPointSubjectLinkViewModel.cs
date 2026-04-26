using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class PlotPointSubjectLinkViewModel : OwnerViewModel
    {
        private readonly IViewModelRegistry _viewModelRegistry;

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

        PlotPointSubjectLink _link;

        public PlotPointSubjectLinkViewModel(PlotPointSubjectLink link, IViewModelRegistry viewModelRegistry, IStoryService storyService) : base(viewModelRegistry, storyService)
        {
            _link = link;
            _viewModelRegistry = viewModelRegistry;

            // Resolve the subject's definition id from the in-memory collection
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

            InitializeCollections(link.Id, OwnerType.PlotPointSubjectLink,
                                  noteTracks, propertyDefs);
        }
    }
}
