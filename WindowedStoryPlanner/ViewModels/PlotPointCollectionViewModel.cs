using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class PlotPointCollectionViewModel : ObservableObject
{
    public ObservableCollection<PlotPointViewModelOld> ViewModelCollection { get; set; } = new();

    // Strategy: If null, reordering is disabled (Buttons Hidden)
    private readonly Action<int, int>? _reorderStrategy;

    // Default Constructor (Empty)
    public PlotPointCollectionViewModel() { }

    // Constructor with generic source and optional reorder logic
    public PlotPointCollectionViewModel(
        IEnumerable<PlotPoint> sourceItems, 
        Action<int, int>? reorderStrategy = null)
    {
        _reorderStrategy = reorderStrategy;
        
        // Populate
        foreach (var p in sourceItems)
        {
            ViewModelCollection.Add(MainViewModel.Instance.PlotPointDictionary[p]);
        }
    }

    // Property to control visibility of buttons in the View
    public bool CanReorder => _reorderStrategy != null;

    [RelayCommand]
    public void MoveItemUp(PlotPointViewModelOld item)
    {
        int index = ViewModelCollection.IndexOf(item);
        if (index > 0) MoveItem(index, index - 1);
    }

    [RelayCommand]
    public void MoveItemDown(PlotPointViewModelOld item)
    {
        int index = ViewModelCollection.IndexOf(item);
        if (index < ViewModelCollection.Count - 1) MoveItem(index, index + 1);
    }

    private void MoveItem(int oldIndex, int newIndex)
    {
        // 1. Move visually
        ViewModelCollection.Move(oldIndex, newIndex);

        // 2. Delegate the data persistence to the parent
        _reorderStrategy?.Invoke(oldIndex, newIndex);
    }
    
    // Helper to refresh the list with specific sorting
    public void SetAndSortItems(IEnumerable<PlotPoint> items, Comparison<PlotPoint> sortLogic)
    {
        ViewModelCollection.Clear();
        var sortedList = items.ToList();
        sortedList.Sort(sortLogic);

        foreach (var p in sortedList)
        {
            ViewModelCollection.Add(MainViewModel.Instance.PlotPointDictionary[p]);
        }
    }
}