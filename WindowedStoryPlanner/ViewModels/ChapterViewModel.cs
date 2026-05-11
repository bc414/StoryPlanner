using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterViewModel : NarrativeElementViewModel
{
    private readonly Chapter _chapter;
    public Chapter Chapter => _chapter;

    public ICollectionView PlotPointsInChapter { get; }

    public int Id => _chapter.Id;

    public ChapterViewModel(
        Chapter chapter,
        IViewModelRegistry viewModelRegistry,
        IStoryService storyService,
        IContentFactory editorCoordinator)
        : base(viewModelRegistry, storyService, editorCoordinator)
    {
        _chapter = chapter;

        var noteTracks = storyService.NoteTrackDefinitions
            .Where(ntd => ntd.OwnerType == OwnerType.Chapter)
            .ToList();

        var propertyDefs = storyService.NarrativePropertyDefinitions
            .Where(npd => npd.OwnerType == OwnerType.Chapter)
            .ToList();

        InitializeCollections(chapter.Id, OwnerType.Chapter, noteTracks, propertyDefs);

        PlotPointsInChapter = new ListCollectionView(viewModelRegistry.AllPlotPointViewModels)
        {
            Filter = FilterPlotPoints
        };
        PlotPointsInChapter.SortDescriptions.Add(
            new SortDescription(nameof(PlotPointViewModel.OrderInChapter), ListSortDirection.Ascending));
    }

    private bool FilterPlotPoints(object obj)
    {
        if (obj is not PlotPointViewModel plotPoint) return false;
        return plotPoint.ChapterId == _chapter.Id;
    }

    // ── Properties ───────────────────────────────────────────────────────

    public string FullTitle => $"{OrderIndex}. {Title}";

    public string Title
    {
        get => _chapter.Title;
        set
        {
            if (SetProperty(_chapter.Title, value, _chapter, (u, n) => u.Title = n))
                OnPropertyChanged(nameof(FullTitle));
        }
    }

    public int OrderIndex
    {
        get => _chapter.OrderIndex;
        set
        {
            if (SetProperty(_chapter.OrderIndex, value, _chapter, (u, n) => u.OrderIndex = n))
            {
                OnPropertyChanged(nameof(FullTitle));
                _viewModelRegistry.RaiseLinksInvalidated();
            }
        }
    }

    // ── Selection ────────────────────────────────────────────────────────

    [ObservableProperty]
    private PlotPointViewModel? _selectedPlotPoint;

    // ── Commands ─────────────────────────────────────────────────────────

    [RelayCommand]
    public async Task AddPlotPoint()
    {
        // Determine the next sort order from the service layer so it is
        // correct even if the window is not yet open (Sections not initialized).
        int nextOrder = _storyService.PlotPoints
            .Where(p => p.ChapterId == _chapter.Id)
            .Select(p => p.OrderInChapter)
            .DefaultIfEmpty(0)
            .Max() + 1;

        await _editorCoordinator.CreatePlotPointAsync(_chapter.Id, nextOrder);
    }

    [RelayCommand]
    private void MovePlotPointUp()
    {
        if (SelectedPlotPoint is null) return;

        var neighbor = PlotPointsInChapter.Cast<PlotPointViewModel>()
            .Where(p => p.OrderInChapter < SelectedPlotPoint.OrderInChapter)
            .OrderByDescending(p => p.OrderInChapter)
            .FirstOrDefault();

        if (neighbor is null) return;

        (SelectedPlotPoint.OrderInChapter, neighbor.OrderInChapter) =
            (neighbor.OrderInChapter, SelectedPlotPoint.OrderInChapter);

        PlotPointsInChapter.Refresh();
    }

    [RelayCommand]
    private void MovePlotPointDown()
    {
        if (SelectedPlotPoint is null) return;

        var neighbor = PlotPointsInChapter.Cast<PlotPointViewModel>()
            .Where(p => p.OrderInChapter > SelectedPlotPoint.OrderInChapter)
            .OrderBy(p => p.OrderInChapter)
            .FirstOrDefault();

        if (neighbor is null) return;

        (SelectedPlotPoint.OrderInChapter, neighbor.OrderInChapter) =
            (neighbor.OrderInChapter, SelectedPlotPoint.OrderInChapter);

        PlotPointsInChapter.Refresh();
    }

    // ── Drag & Drop ──────────────────────────────────────────────────────

    public override void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.Data is PlotPointViewModel)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }
        else
        {
            base.DragOver(dropInfo);
        }
    }

    public override async void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is not PlotPointViewModel plotPoint)
        {
            base.Drop(dropInfo);
            return;
        }

        int? oldChapterId = plotPoint.ChapterId;
        bool isNewChapter = oldChapterId != _chapter.Id;

        // Move to the end of this chapter
        int nextOrder = _storyService.PlotPoints
            .Where(p => p.ChapterId == _chapter.Id && p.Id != plotPoint.Id)
            .Select(p => p.OrderInChapter)
            .DefaultIfEmpty(0)
            .Max() + 1;

        plotPoint.ChapterId = _chapter.Id;
        plotPoint.OrderInChapter = nextOrder;

        // Compact the old chapter's ordering to remove the gap
        if (isNewChapter && oldChapterId.HasValue)
        {
            var oldChapterPlotPoints = _viewModelRegistry.AllPlotPointViewModels
                .Where(p => p.ChapterId == oldChapterId.Value)
                .OrderBy(p => p.OrderInChapter)
                .ToList();

            for (int i = 0; i < oldChapterPlotPoints.Count; i++)
                oldChapterPlotPoints[i].OrderInChapter = i + 1;

            // Refresh the old chapter's list view if it is open
            var oldChapterVm = _viewModelRegistry.AllChapterViewModels
                .FirstOrDefault(c => c.Id == oldChapterId.Value);
            oldChapterVm?.PlotPointsInChapter.Refresh();
        }

        PlotPointsInChapter.Refresh();
    }
}