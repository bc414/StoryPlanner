using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterViewModel : OwnerViewModel
{
    private readonly Chapter _chapter;
    public ICollectionView PlotPointsInChapter;
    private IViewModelRegistry _viewModelRegistry;
    private IStoryService _storyService;

    public int Id => _chapter.Id;
    public ChapterViewModel(Chapter chapter, IViewModelRegistry viewModelRegistry, IStoryService storyService) : base(viewModelRegistry, storyService)
    {
        _chapter = chapter;
        _viewModelRegistry = viewModelRegistry;
        _storyService = storyService;

        var noteTracks = storyService.NoteTrackDefinitions
                .Where(ntd => ntd.OwnerType == OwnerType.Chapter)
                .ToList();

        var propertyDefs = storyService.NarrativePropertyDefinitions
            .Where(npd => npd.OwnerType == OwnerType.Chapter)
            .ToList();

        InitializeCollections(chapter.Id, OwnerType.Chapter,
                              noteTracks, propertyDefs);

        PlotPointsInChapter = CollectionViewSource.GetDefaultView(viewModelRegistry.AllPlotPointViewModels);
        PlotPointsInChapter.Filter = FilterPlotPoints;
        PlotPointsInChapter.SortDescriptions.Add(new SortDescription(nameof(PlotPointViewModel.OrderInChapter), ListSortDirection.Ascending));

    }

    private bool FilterPlotPoints(object obj)
    {
        if (obj is not PlotPointViewModel plotPoint) return false;
        return plotPoint.ChapterId.HasValue ? plotPoint.ChapterId == _chapter.Id : false;
    }

    public void UpdateSortOrders()
    {
        for (int i = 0; i < PlotPointCollectionViewModel.ViewModelCollection.Count; i++)
        {
            PlotPointCollectionViewModel.ViewModelCollection[i].OrderInChapter = i;
        }
    }

    // --- Properties Wrapper ---

    // 1. New Computed Property
    public string FullTitle => $"{OrderIndex}. {Title}";

    // 2. Update existing properties to notify FullTitle when they change
    public string Title
    {
        get => _chapter.Title;
        set
        {
            if (SetProperty(_chapter.Title, value, _chapter, (u, n) => u.Title = n))
            {
                OnPropertyChanged(nameof(FullTitle));
            }
        }
    }

    public int OrderIndex
    {
        get => _chapter.OrderIndex;
        set
        {
            // Standard Property Update
            if (SetProperty(_chapter.OrderIndex, value, _chapter, (u, n) => u.OrderIndex = n))
            {
                // 1. Update own title (e.g. "Chapter 1: The Beginning")
                OnPropertyChanged(nameof(Title)); 

                // 2. THE SIMPLER FIX: Push update to all children
                // Since this ViewModel already holds the collection of PlotPoints, 
                // we can just loop through them.
                foreach (var o in PlotPointsInChapter)
                {
                    if(o is PlotPointViewModel plotPoint)
                    {
                        
                    }
                }
            }
        }
    }

    public string Summary
    {
        get => _chapter.Summary;
        set => SetProperty(_chapter.Summary, value, _chapter, (u, n) => u.Summary = n);
    }

    public string Description
    {
        get => _chapter.Description;
        set => SetProperty(_chapter.Description, value, _chapter, (u, n) => u.Description = n);
    }

    

    [RelayCommand]
    public async Task AddPlotPoint()
    {
        PlotPoint? last = _storyService.PlotPoints.Where(p => p.ChapterId == _chapter.Id).OrderBy(p => p.OrderInChapter).LastOrDefault();
        int orderToUse;
        if(last != null)
        {
            orderToUse = last.OrderInChapter + 1;
        }
        else
        {
            orderToUse = 1;
        }
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
            ChapterId = _chapter.Id,
            OrderInChapter = orderToUse,
        };
        _storyService.PlotPoints.Add(plotPoint);
        await _storyService.SaveAsync();

        _viewModelRegistry.AllPlotPointViewModels.Add(new PlotPointViewModel(plotPoint, _viewModelRegistry, _storyService));
    }
}