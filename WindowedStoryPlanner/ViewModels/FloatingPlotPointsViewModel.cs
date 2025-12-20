using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace WindowedStoryPlanner.ViewModels;

public partial class FloatingPlotPointsViewModel : ObservableObject
{
    // The wrapper that the View binds to
    public PlotPointCollectionViewModel PlotPointCollectionViewModel { get; }

    public FloatingPlotPointsViewModel()
    {
        // 1. Initial Populate: Find all VMs where the Chapters list is empty
        var unassignedPoints = MainViewModel.Instance.PlotPointViewModels
            .Where(vm => vm.Chapters.Count == 0) // Check the ViewModel's collection, not the Model
            .Select(vm => vm.Model);

        // Initialize the wrapper
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel(unassignedPoints, null);

        // 2. Subscribe to Global List Changes (New/Deleted items)
        MainViewModel.Instance.PlotPointViewModels.CollectionChanged += OnGlobalListChanged;

        // 3. Subscribe to Individual Item Link Changes (Existing items)
        foreach (var vm in MainViewModel.Instance.PlotPointViewModels)
        {
            vm.Chapters.CollectionChanged += (s, e) => OnChapterLinkChanged(vm);
        }
    }

    // Handles when a Plot Point is created or deleted entirely
    private void OnGlobalListChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (PlotPointViewModel newItem in e.NewItems)
            {
                // Start listening to this new item's links
                newItem.Chapters.CollectionChanged += (s, args) => OnChapterLinkChanged(newItem);

                // If it starts unassigned, show it
                if (newItem.Chapters.Count == 0)
                {
                    PlotPointCollectionViewModel.ViewModelCollection.Add(newItem);
                }
            }
        }

        if (e.OldItems != null)
        {
            foreach (PlotPointViewModel oldItem in e.OldItems)
            {
                // Stop listening
                // Note: We can't easily unsubscribe the anonymous lambda, but since the object is dying, it's usually acceptable.
                // For stricter memory management, you'd use a named method for the event handler.
                
                PlotPointCollectionViewModel.ViewModelCollection.Remove(oldItem);
            }
        }
    }

    // Handles when a Plot Point is Linked or Unlinked
    private void OnChapterLinkChanged(PlotPointViewModel vm)
    {
        bool isFloating = vm.Chapters.Count == 0;
        bool isInList = PlotPointCollectionViewModel.ViewModelCollection.Contains(vm);

        if (isFloating && !isInList)
        {
            PlotPointCollectionViewModel.ViewModelCollection.Add(vm);
        }
        else if (!isFloating && isInList)
        {
            PlotPointCollectionViewModel.ViewModelCollection.Remove(vm);
        }
    }

    [RelayCommand]
    public void AddFloatingPlotPoint()
    {
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "",
            ChapterId = null,
            OrderInChapter = 0
        };

        // Registering adds it to MainViewModel -> OnGlobalListChanged fires -> Adds to our list
        PlotPointViewModel plotPointVM = MainViewModel.Instance.RegisterNewPlotPoint(plotPoint);
        
        MainViewModel.Instance.OpenEditorWindow(plotPointVM);
    }
    
    [RelayCommand]
    public void DeleteFloatingPlotPoint(PlotPointViewModel vm)
    {
        if (vm == null) return;

        if (MessageBox.Show($"Are you sure you want to delete '{vm.Title}' entirely?", 
                            "Confirm Delete", 
                            MessageBoxButton.YesNo, 
                            MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            // Remove from global source
            MainViewModel.Instance.PlotPoints.Remove(vm.Model);
            MainViewModel.Instance.PlotPointViewModels.Remove(vm);
        }
    }
}