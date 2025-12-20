using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterViewModel : EntityViewModel
{
    private readonly Chapter _chapter;
    public Chapter Chapter => _chapter;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLinkingMode))]
    private bool _isPlotPointReorderMode;

    public override bool IsLinkingMode => !IsPlotPointReorderMode && !NoteCollectionViewModel.IsNoteReorderMode;

    public ChapterViewModel(Chapter chapter)
    {
        _chapter = chapter;

        // Initialize the notes collection generic to the entity
        NoteCollectionViewModel = new NoteCollectionViewModel(chapter.Notes);
        NoteCollectionViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NoteCollectionViewModel.IsNoteReorderMode))
            {
                OnPropertyChanged(nameof(IsLinkingMode));
            }
        };
        
        // --- THE FIX: SORT THE MODEL FIRST ---
        // We must ensure the underlying ObservableCollection is sorted by OrderInChapter.
        // Otherwise, the UI (which we want sorted) and the Model (which might be unsorted)
        // will have mismatched indices, causing Drag & Drop to move the wrong items.
        
        var sortedList = _chapter.PlotPoints.OrderBy(p => p.OrderInChapter).ToList();
        
        // Only modify if actually out of order to avoid unnecessary events
        if (!_chapter.PlotPoints.SequenceEqual(sortedList))
        {
            _chapter.PlotPoints.Clear();
            foreach (var p in sortedList)
            {
                _chapter.PlotPoints.Add(p);
            }
        }
        
        // Initialize with Chapter-Specific Reorder Logic
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel(
            chapter.PlotPoints,
            (oldIndex, newIndex) => 
            {
                // 1. Move in Model
                _chapter.PlotPoints.Move(oldIndex, newIndex);
            
                // 2. Update Sort Indexes
                UpdateSortOrders();
            }
        );
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
                foreach (var pp in PlotPointCollectionViewModel.ViewModelCollection)
                {
                    pp.RefreshFullOrder();
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