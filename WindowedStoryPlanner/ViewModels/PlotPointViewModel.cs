using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class PlotPointViewModel : NarrativeElementViewModel
    {
        public ICollectionView PlotPointSubjectLinks { get; set; }
        private PlotPoint _plotPoint;
        public PlotPoint PlotPoint => _plotPoint;
        private readonly IWindowManager _windowManager;

        public int Id => _plotPoint.Id;

        public int? ChapterId
        {
            get => _plotPoint.ChapterId;
            set
            { 
                SetProperty(_plotPoint.ChapterId, value, _plotPoint, (p, n) => p.ChapterId = n);
                OnPropertyChanged(nameof(FullOrder));
            }
        }

        public int OrderInChapter
        {
            get => _plotPoint.OrderInChapter;
            set
            {
                SetProperty(_plotPoint.OrderInChapter, value, _plotPoint, (p, n) => p.OrderInChapter = n);
                OnPropertyChanged(nameof(FullOrder));
            }
        }

        public string Title
        {
            get => _plotPoint.Title;
            set => SetProperty(_plotPoint.Title, value, _plotPoint, (p, n) => p.Title = n);
        }

        public string FullOrder => ChapterId == null
        ? "? "
        : $"{_viewModelRegistry.AllChapterViewModels.FirstOrDefault(c => c.Id == ChapterId)?.OrderIndex : '? '}.{OrderInChapter} ";

        [RelayCommand]
        private void Open() => _windowManager.OpenCommonWindow(this);

        public PlotPointViewModel(
            PlotPoint plotPoint,
            IViewModelRegistry viewModelRegistry,
            IStoryService storyService,
            IContentFactory editorCoordinator,
            IWindowManager windowManager)
            : base(viewModelRegistry, storyService, editorCoordinator)
        {
            _plotPoint = plotPoint;

            var noteTracks = storyService.NoteTrackDefinitions
                .Where(ntd => ntd.OwnerType == OwnerType.PlotPoint)
                .ToList();

            var propertyDefs = storyService.NarrativePropertyDefinitions
                .Where(npd => npd.OwnerType == OwnerType.PlotPoint)
                .ToList();

            InitializeCollections(plotPoint.Id, OwnerType.PlotPoint, noteTracks, propertyDefs);

            PlotPointSubjectLinks = CollectionViewSource.GetDefaultView(
                viewModelRegistry.AllPlotPointSubjectLinkViewModels);
            PlotPointSubjectLinks.Filter = FilterLinks;
            _windowManager = windowManager;
        }

        private bool FilterLinks(object obj)
        {
            if (obj is not PlotPointSubjectLinkViewModel link) return false;
            return link.PlotPointId == _plotPoint.Id;
        }
    }
}
