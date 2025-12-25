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
        
        // --- NEW: LIVE UPDATE LISTENER ---
        _storyThread.PlotPointAssignments.CollectionChanged += (s, e) =>
        {
            // 1. Re-fetch the points in the correct SortOrder
            var updatedPoints = _storyThread.PlotPointAssignments
                .OrderBy(x => x.SortOrder)
                .Select(x => x.PlotPoint);

            // 2. Clear and Rebuild the View List
            PlotPointCollectionViewModel.ViewModelCollection.Clear();
            foreach (var p in updatedPoints)
            {
                // Look up the Singleton ViewModel for this Plot Point
                var ppVM = MainViewModel.Instance.PlotPointDictionary[p];
                PlotPointCollectionViewModel.ViewModelCollection.Add(ppVM);
            }
        };
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
    public async Task AddPlotPoint()
    {
        // 1. Create the new Plot Point
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
        };

        // 2. SAVE the Plot Point FIRST to generate its ID
        // The junction needs a valid PlotPointId to exist in the database.
        var plotPointVM = await MainViewModel.Instance.RegisterNewPlotPoint(plotPoint);

        // 3. Create the Junction Object
        PlotPointThread junction = new PlotPointThread
        {
            PlotPoint = plotPoint,
            PlotPointId = plotPoint.Id, // Ensure ID is linked
            StoryThread = _storyThread,
            ThreadId = _storyThread.Id, // Ensure ID is linked
            SortOrder = _storyThread.PlotPointAssignments.Count,
            IsPrimary = true,
            ThreadTrajectory = GoalTrajectory.Unset,
        };

        // ---------------------------------------------------------
        // CRITICAL SECTION: ORDER MATTERS
        // ---------------------------------------------------------

        // 4. Add to PLOT POINT first [Fixes Blank Payload]
        // The data must be here BEFORE the UI listener fires.
        plotPoint.ThreadAssignments.Add(junction);

        // 5. Add to THREAD second [Fixes UI Update]
        // This fires the .CollectionChanged listener in your constructor.
        // The UI rebuilds, finds the data in Step 4, and renders correctly.
        _storyThread.PlotPointAssignments.Add(junction);

        // 6. Update the ViewModel Wrapper (Badge display)
        plotPointVM.StoryThreads.Add(this);

        // ---------------------------------------------------------

        // 7. REMOVE DUPLICATE [Fixes Double Entry]
        // DELETE THIS LINE: 
        // PlotPointCollectionViewModel.ViewModelCollection.Add(plotPointVM);

        // 8. SAVE EVERYTHING [Fixes Data Loss]
        // We modified the relationships AFTER the first save, so we must save again.
        await MainViewModel.Instance.SaveChanges(false);
    }

    public void UnlinkPlotPoint(PlotPointViewModel plotPointVM)
    {
        plotPointVM.UnlinkThread(this);
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