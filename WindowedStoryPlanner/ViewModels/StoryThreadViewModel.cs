using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class StoryThreadViewModel : EntityViewModel
{
    private readonly StoryThread _storyThread;
    public StoryThread StoryThread => _storyThread;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLinkingMode))]
    private bool _isPlotPointReorderMode;

    public override bool IsLinkingMode => !IsPlotPointReorderMode && !NoteCollectionViewModel.IsNoteReorderMode;

    public StoryThreadViewModel(StoryThread storyThread)
    {
        _storyThread = storyThread;

        // Initialize the notes collection generic to the entity
        NoteCollectionViewModel = new NoteCollectionViewModel(storyThread.Notes);
        NoteCollectionViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NoteCollectionViewModel.IsNoteReorderMode))
            {
                OnPropertyChanged(nameof(IsLinkingMode));
            }
        };
        
        // 1. Sort by the Thread-Specific Order
        var sortedPoints = storyThread.PlotPointAssignments
            .OrderBy(x => x.SortOrder)
            .Select(x => x.PlotPoint);

        // 2. Initialize with a REORDER STRATEGY
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel(
            sortedPoints, 
            ReorderThreadAssignments // <--- Pass the function below
        );
    }
    
    // This runs whenever the user clicks Up/Down in the UI
    private void ReorderThreadAssignments(int oldIndex, int newIndex)
    {
        // 1. Move the item in the Model Collection (ObservableCollection<PlotPointThread>)
        _storyThread.PlotPointAssignments.Move(oldIndex, newIndex);

        // 2. Recalculate the SortOrder integers to match the new list order
        for (int i = 0; i < _storyThread.PlotPointAssignments.Count; i++)
        {
            _storyThread.PlotPointAssignments[i].SortOrder = i;
        }
    }
    
    [RelayCommand]
    public void AddPlotPoint()
    {
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
        };
        PlotPointThread junction = new PlotPointThread
        {
            PlotPoint = plotPoint,
            StoryThread = _storyThread,
            SortOrder = _storyThread.PlotPointAssignments.Count + 1,
            IsPrimary = true,
            ThreadTrajectory = GoalTrajectory.Unset,
        };
        _storyThread.PlotPointAssignments.Add(junction);
        plotPoint.ThreadAssignments.Add(junction);
        var plotPointVM = MainViewModel.Instance.RegisterNewPlotPoint(plotPoint);
        PlotPointCollectionViewModel.ViewModelCollection.Add(plotPointVM);
    }

    // --- Properties Wrapper ---

    public string Name
    {
        get => _storyThread.Name;
        set => SetProperty(_storyThread.Name, value, _storyThread, (u, n) => u.Name = n);
    }

    public string Description
    {
        get => _storyThread.Description;
        set => SetProperty(_storyThread.Description, value, _storyThread, (u, n) => u.Description = n);
    }

    public string Icon
    {
        get => _storyThread.Icon;
        set => SetProperty(_storyThread.Icon, value, _storyThread, (u, n) => u.Icon = n);
    }

    public ThreadScope ThreadScope
    {
        get => _storyThread.ThreadScope;
        set => SetProperty(_storyThread.ThreadScope, value, _storyThread, (u, n) => u.ThreadScope = n);
    }
}