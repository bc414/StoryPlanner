using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Linq;
using System.Windows;

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
            set
            {
                if (_link.SubjectId == value) return;
                UnsubscribeSubject();
                SetProperty(_link.SubjectId, value, _link, (l, n) => l.SubjectId = n);
                SubscribeSubject();
                OnPropertyChanged(nameof(SubjectName));
                OnPropertyChanged(nameof(SubjectTypeName));
            }
        }

        public int PlotPointId
        {
            get => _link.PlotPointId;
            set
            {
                if (_link.PlotPointId == value) return;
                UnsubscribePlotPoint();
                SetProperty(_link.PlotPointId, value, _link, (l, n) => l.PlotPointId = n);
                SubscribePlotPoint();
                OnPropertyChanged(nameof(PlotPointDisplayText));
                OnPropertyChanged(nameof(ChapterOrderIndex));
                OnPropertyChanged(nameof(PlotPointOrderInChapter));
            }
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

        public string SubjectName =>
            _viewModelRegistry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == _link.SubjectId)
                ?.Name ?? string.Empty;

        public string SubjectTypeName =>
            _viewModelRegistry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == _link.SubjectId) is SubjectViewModel subject
                    ? _viewModelRegistry.AllSubjectDefinitionViewModels
                        .FirstOrDefault(d => d.Id == subject.SubjectDefinitionId)
                        ?.SubjectType ?? string.Empty
                    : string.Empty;

        public string PlotPointDisplayText =>
            _viewModelRegistry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == _link.PlotPointId) is PlotPointViewModel pp
                    ? $"{pp.FullOrder}{pp.Title}"
                    : string.Empty;

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

            SubscribePlotPoint();
            SubscribeSubject();
        }

        private void SubscribePlotPoint()
        {
            var vm = _viewModelRegistry.AllPlotPointViewModels.FirstOrDefault(pp => pp.Id == _link.PlotPointId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .AddHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnPlotPointPropertyChanged);
        }

        private void UnsubscribePlotPoint()
        {
            var vm = _viewModelRegistry.AllPlotPointViewModels.FirstOrDefault(pp => pp.Id == _link.PlotPointId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .RemoveHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnPlotPointPropertyChanged);
        }

        private void SubscribeSubject()
        {
            var vm = _viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == _link.SubjectId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .AddHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnSubjectPropertyChanged);
        }

        private void UnsubscribeSubject()
        {
            var vm = _viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == _link.SubjectId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .RemoveHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnSubjectPropertyChanged);
        }

        private void OnPlotPointPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PlotPointDisplayText));
            OnPropertyChanged(nameof(ChapterOrderIndex));
            OnPropertyChanged(nameof(PlotPointOrderInChapter));
        }

        private void OnSubjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SubjectName));
            OnPropertyChanged(nameof(SubjectTypeName));
        }
    }
}
