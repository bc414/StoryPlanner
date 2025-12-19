using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace WindowedStoryPlanner.ViewModels;

public partial class FloatingPlotPointsViewModel : ObservableObject
{

    public ObservableCollection<PlotPointViewModel> FloatingPlotPoints { get; set; }

    public FloatingPlotPointsViewModel()
    {
        FloatingPlotPoints = new ObservableCollection<PlotPointViewModel>();
        
        // Filter: only show PlotPoints that do not have an assigned Chapter
        foreach (var pp in MainViewModel.Instance.PlotPointViewModels)
        {
            if (pp.Model.Chapter == null)
            {
                FloatingPlotPoints.Add(pp);
            }
        }

        // Subscribe to changes in the main list to keep this in sync
        MainViewModel.Instance.PlotPointViewModels.CollectionChanged += PlotPointViewModels_CollectionChanged;
    }

    private void PlotPointViewModels_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
         // Refresh logic (simplified)
         FloatingPlotPoints.Clear();
         foreach (var pp in MainViewModel.Instance.PlotPointViewModels)
         {
             if (pp.Model.Chapter == null)
             {
                 FloatingPlotPoints.Add(pp);
             }
         }
    }
}