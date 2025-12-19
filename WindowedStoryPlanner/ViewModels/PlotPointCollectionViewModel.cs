using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class PlotPointCollectionViewModel : ObservableObject
{
    public ObservableCollection<PlotPointViewModel> ViewModelCollection { get; set; }
    public ObservableCollection<PlotPoint> ModelCollection { get; }

    public PlotPointCollectionViewModel(ObservableCollection<PlotPoint> sourceCollection)
    {
        ModelCollection = sourceCollection;
        ViewModelCollection = new ObservableCollection<PlotPointViewModel>();

        foreach (PlotPoint p in sourceCollection)
        {
            ViewModelCollection.Add(MainViewModel.Instance.PlotPointDictionary[p]);
        }
    }

    public PlotPointCollectionViewModel()
    {
        ModelCollection = new ObservableCollection<PlotPoint>();
        ViewModelCollection = new ObservableCollection<PlotPointViewModel>();
    }

    [RelayCommand]
    public void MoveItemUp(PlotPointViewModel item)
    {
        if (item == null) return;
        int index = ViewModelCollection.IndexOf(item);
        if (index > 0)
        {
            MoveItem(index, index - 1);
        }
    }

    [RelayCommand]
    public void MoveItemDown(PlotPointViewModel item)
    {
        if (item == null) return;
        int index = ViewModelCollection.IndexOf(item);
        if (index < ViewModelCollection.Count - 1)
        {
            MoveItem(index, index + 1);
        }
    }

    private void MoveItem(int oldIndex, int newIndex)
    {
        // 1. Move in ViewModel Collection (UI)
        ViewModelCollection.Move(oldIndex, newIndex);
        
        // 2. Move in Model Collection (Data)
        // We need to find the model object corresponding to the moved VM
        var itemVM = ViewModelCollection[newIndex];
        var itemModel = itemVM.Model;
        
        int modelIndex = ModelCollection.IndexOf(itemModel);
        // Note: ModelCollection index might differ if ViewModelCollection is filtered, 
        // but for PlotPoints in a Chapter, they should be 1:1. 
        // If they are not 1:1, we might need a more robust swap, but assuming 1:1 for now.
        if (modelIndex >= 0)
        {
             // We can't easily guess the new model index if the list is filtered. 
             // Ideally we just update SortOrders and let the parent resort if needed, 
             // but 'Move' implies immediate visual feedback.
             
             // Simplest approach for 1:1 lists (Chapter Plot Points):
             ModelCollection.Move(oldIndex, newIndex);
        }

        // 3. Update Sort Orders persistence
        UpdateSortOrders();
    }

    private void UpdateSortOrders()
    {
        for (int i = 0; i < ViewModelCollection.Count; i++)
        {
            ViewModelCollection[i].OrderInChapter = i;
        }
    }
}