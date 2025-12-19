using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterViewModel : EntityViewModel
{
    private readonly Chapter _chapter;
    public Chapter Chapter => _chapter;

    public ChapterViewModel(Chapter chapter)
    {
        _chapter = chapter;

        // Initialize the notes collection generic to the entity
        NoteCollectionViewModel = new NoteCollectionViewModel(chapter.Notes);
        // TODO: need to sort somehow
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel(chapter.PlotPoints);
        PlotPointCollectionViewModel.ViewModelCollection.CollectionChanged += PlotPointCollection_CollectionChanged;
    }

    private void PlotPointCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateSortOrders();
    }

    private void UpdateSortOrders()
    {
        for (int i = 0; i < PlotPointCollectionViewModel.ViewModelCollection.Count; i++)
        {
            PlotPointCollectionViewModel.ViewModelCollection[i].OrderInChapter = i;
        }
    }

    // --- Properties Wrapper ---

    public string Title
    {
        get => _chapter.Title;
        set => SetProperty(_chapter.Title, value, _chapter, (u, n) => u.Title = n);
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

    public int OrderIndex
    {
        get => _chapter.OrderIndex;
        set => SetProperty(_chapter.OrderIndex, value, _chapter, (u, n) => u.OrderIndex = n);
    }

    [RelayCommand]
    public void AddPlotPoint()
    {
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
            ChapterId = _chapter.Id,
            OrderInChapter = _chapter.PlotPoints.Count + 1
        };
        _chapter.PlotPoints.Add(plotPoint);
        var plotPointVM = MainViewModel.Instance.RegisterNewPlotPoint(plotPoint);
        PlotPointCollectionViewModel.ViewModelCollection.Add(plotPointVM);
    }
}