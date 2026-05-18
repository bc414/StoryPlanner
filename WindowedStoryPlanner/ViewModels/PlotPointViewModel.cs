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
                _viewModelRegistry.RaiseLinksInvalidated();
            }
        }

        public int OrderInChapter
        {
            get => _plotPoint.OrderInChapter;
            set
            {
                SetProperty(_plotPoint.OrderInChapter, value, _plotPoint, (p, n) => p.OrderInChapter = n);
                OnPropertyChanged(nameof(FullOrder));
                _viewModelRegistry.RaiseLinksInvalidated();
            }
        }

        public string Title
        {
            get => _plotPoint.Title;
            set => SetProperty(_plotPoint.Title, value, _plotPoint, (p, n) => p.Title = n);
        }

        public string FullOrder => ChapterId == null
    ? "? "
    : $"{_viewModelRegistry.AllChapterViewModels.FirstOrDefault(c => c.Id == ChapterId)?.OrderIndex.ToString() ?? "?"}.{OrderInChapter} ";

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

            InitializeCollections(
                plotPoint.Id,
                OwnerType.PlotPoint,
                () => storyService.NoteTrackDefinitions
                          .Where(ntd => ntd.OwnerType == OwnerType.PlotPoint)
                          .ToList(),
                () => storyService.NarrativePropertyDefinitions
                          .Where(npd => npd.OwnerType == OwnerType.PlotPoint)
                          .ToList());

            var view = new ListCollectionView(viewModelRegistry.AllPlotPointSubjectLinkViewModels)
            {
                Filter = obj => obj is PlotPointSubjectLinkViewModel link && link.PlotPointId == _plotPoint.Id,
                CustomSort = Comparer<object>.Create((a, b) =>
                {
                    if (a is not PlotPointSubjectLinkViewModel la || b is not PlotPointSubjectLinkViewModel lb)
                        return 0;

                    var subjectA = viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == la.SubjectId);
                    var subjectB = viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == lb.SubjectId);

                    var defOrderA = viewModelRegistry.AllSubjectDefinitionViewModels
                        .FirstOrDefault(d => d.Id == subjectA?.SubjectDefinitionId)?.DisplayOrder ?? int.MaxValue;
                    var defOrderB = viewModelRegistry.AllSubjectDefinitionViewModels
                        .FirstOrDefault(d => d.Id == subjectB?.SubjectDefinitionId)?.DisplayOrder ?? int.MaxValue;

                    int defCompare = defOrderA.CompareTo(defOrderB);
                    if (defCompare != 0) return defCompare;

                    return string.Compare(subjectA?.Name, subjectB?.Name, StringComparison.CurrentCultureIgnoreCase);
                })
            };
            PlotPointSubjectLinks = view;

            _windowManager = windowManager;
        }
    }
}
