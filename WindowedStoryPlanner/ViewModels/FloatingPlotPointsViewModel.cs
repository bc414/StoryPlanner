using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

public class FloatingPlotPointsViewModel : ObservableObject
{
    private readonly StoryService _storyService;

    public ICollectionView FloatingPlotPointsView { get; private set; }
    
    // The Source Collection (All PlotPoints)
    // We bind this to the Service's Local View
    public ObservableCollection<PlotPoint> AllPlotPoints { get; private set; }

    public FloatingPlotPointsViewModel(StoryService storyService)
    {
        _storyService = storyService;
        
        // 1. Get the raw list of ALL points (from context.PlotPoints.Local)
        // You might need to expose this in your service as:
        // public ObservableCollection<PlotPoint> AllPlotPoints => _context.PlotPoints.Local.ToObservableCollection();
        AllPlotPoints = _storyService.PlotPoints; 

        // 2. Create a CollectionView wrapper
        FloatingPlotPointsView = CollectionViewSource.GetDefaultView(AllPlotPoints);

        // 3. Define the Filter
        FloatingPlotPointsView.Filter = (obj) =>
        {
            if (obj is PlotPoint p)
            {
                return p.ChapterId == null; // Show only if it has no chapter
            }
            return false;
        };
    }
}