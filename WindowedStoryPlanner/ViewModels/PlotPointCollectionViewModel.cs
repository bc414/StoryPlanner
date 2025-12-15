using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public class PlotPointCollectionViewModel : ObservableObject, IDropTarget
{
    public ObservableCollection<PlotPointViewModel> ViewModelCollection { get; }
    private ObservableCollection<PlotPoint> _modelCollection { get; }

    public PlotPointCollectionViewModel(ObservableCollection<PlotPoint> sourceCollection)
    {
        _modelCollection = sourceCollection;
        ViewModelCollection = new ObservableCollection<PlotPointViewModel>();

        foreach (PlotPoint p in sourceCollection)
        {
            ViewModelCollection.Add(MainViewModel.Instance.PlotPointDictionary[p]);
        }
    }
    
    public void DragOver(IDropInfo dropInfo)
    {
        
    }

    public void Drop(IDropInfo dropInfo)
    {
        //For reordering
    }
}