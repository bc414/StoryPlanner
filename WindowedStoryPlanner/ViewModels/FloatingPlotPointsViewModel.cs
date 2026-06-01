using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

public partial class FloatingPlotPointsViewModel : ObservableObject, IDropTarget
{
    private readonly IViewModelRegistry _viewModelRegistry;
    private readonly IStoryService _storyService;
    private readonly IContentFactory _editorCoordinator;
    private readonly IWindowManager _windowManager;

    public ICollectionView PlotPoints { get; }

    public FloatingPlotPointsViewModel(
        IViewModelRegistry viewModelRegistry,
        IStoryService storyService,
        IContentFactory editorCoordinator,
        IWindowManager windowManager)
    {
        _viewModelRegistry = viewModelRegistry;
        _storyService = storyService;
        _editorCoordinator = editorCoordinator;
        _windowManager = windowManager;

        PlotPoints = new ListCollectionView(viewModelRegistry.AllPlotPointViewModels)
        {
            Filter = o => o is PlotPointViewModel p && p.ChapterId == null
        };
        PlotPoints.SortDescriptions.Add(
            new SortDescription(nameof(PlotPointViewModel.OrderInChapter), ListSortDirection.Ascending));
    }

    [ObservableProperty]
    private PlotPointViewModel? _selectedPlotPoint;

    [RelayCommand]
    private void OpenWindow() => _windowManager.OpenFloatingPlotPointsWindow(this);

    [RelayCommand]
    public async Task AddPlotPoint()
    {
        int nextOrder = _storyService.PlotPoints
            .Where(p => p.ChapterId == null)
            .Select(p => p.OrderInChapter)
            .DefaultIfEmpty(0)
            .Max() + 1;

        await _editorCoordinator.CreatePlotPointAsync(chapterId: null, nextOrder);
    }

    [RelayCommand]
    private void MovePlotPointUp()
    {
        if (SelectedPlotPoint is null) return;
        var neighbor = PlotPoints.Cast<PlotPointViewModel>()
            .Where(p => p.OrderInChapter < SelectedPlotPoint.OrderInChapter)
            .OrderByDescending(p => p.OrderInChapter)
            .FirstOrDefault();
        if (neighbor is null) return;
        (SelectedPlotPoint.OrderInChapter, neighbor.OrderInChapter) =
            (neighbor.OrderInChapter, SelectedPlotPoint.OrderInChapter);
        PlotPoints.Refresh();
    }

    [RelayCommand]
    private void MovePlotPointDown()
    {
        if (SelectedPlotPoint is null) return;
        var neighbor = PlotPoints.Cast<PlotPointViewModel>()
            .Where(p => p.OrderInChapter > SelectedPlotPoint.OrderInChapter)
            .OrderBy(p => p.OrderInChapter)
            .FirstOrDefault();
        if (neighbor is null) return;
        (SelectedPlotPoint.OrderInChapter, neighbor.OrderInChapter) =
            (neighbor.OrderInChapter, SelectedPlotPoint.OrderInChapter);
        PlotPoints.Refresh();
    }

    // Accepts drops FROM chapter windows — clears ChapterId
    public void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is PlotPointViewModel)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not PlotPointViewModel plotPoint) return;
        plotPoint.ChapterId = null;
        plotPoint.OrderInChapter = PlotPoints.Cast<PlotPointViewModel>().Count() + 1;
        PlotPoints.Refresh();
    }
}