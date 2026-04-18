using System.Linq; // Ensure Linq is available
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class StoryThreadViewModel : EntityViewModel
{
    private readonly StoryThread _storyThread;
    public StoryThread StoryThread => _storyThread;

    // --- FIX 1: The Safety Flag ---
    // Prevents the "Revert Loop" by ignoring events caused by our own reordering
    private bool _isInternalReorder = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLinkingMode))]
    private bool _isPlotPointReorderMode;

    public override bool IsLinkingMode => !NoteCollectionViewModel.IsNoteReorderMode;

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

        // --- FIX 2: Sort the Model Collection First ---
        // Just like ChapterViewModel, we must align the Model's index 
        // with the Logical SortOrder before binding the UI.
        var sortedList = _storyThread.PlotPointAssignments.OrderBy(x => x.SortOrder).ToList();

        if (!_storyThread.PlotPointAssignments.SequenceEqual(sortedList))
        {
            _isInternalReorder = true; // Lock events while we fix the startup order
            _storyThread.PlotPointAssignments.Clear();
            foreach (var item in sortedList)
            {
                _storyThread.PlotPointAssignments.Add(item);
            }
            _isInternalReorder = false;
        }

        // 1. Prepare the View Source (Now guaranteed to match Model indices)
        var sortedPoints = _storyThread.PlotPointAssignments.Select(x => x.PlotPoint);

        // 2. Initialize with Reorder Strategy
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel(
            sortedPoints,
            ReorderThreadAssignments
        );

        // --- FIX 3: Protected Listener ---
        // This updates the UI when data changes externally, but ignores our internal reorders.
        _storyThread.PlotPointAssignments.CollectionChanged += (s, e) =>
        {
            // STOP if we are currently moving items ourselves
            if (_isInternalReorder) return;

            // Otherwise, refresh the list (External updates)
            var updatedPoints = _storyThread.PlotPointAssignments
                .OrderBy(x => x.SortOrder)
                .Select(x => x.PlotPoint);

            PlotPointCollectionViewModel.ViewModelCollection.Clear();
            foreach (var p in updatedPoints)
            {
                var ppVM = MainViewModel.Instance.PlotPointDictionary[p];
                PlotPointCollectionViewModel.ViewModelCollection.Add(ppVM);
            }
        };
    }

    // --- FIX 4: Protected Reorder Logic ---
    private void ReorderThreadAssignments(int oldIndex, int newIndex)
    {
        _isInternalReorder = true; // LOCK: Don't let the listener fire
        try
        {
            // 1. Move the item in the Model
            // Since we sorted in the constructor, these indices now match perfectly.
            _storyThread.PlotPointAssignments.Move(oldIndex, newIndex);

            // 2. Update the integer SortOrder for persistence
            for (int i = 0; i < _storyThread.PlotPointAssignments.Count; i++)
            {
                _storyThread.PlotPointAssignments[i].SortOrder = i;
            }
        }
        finally
        {
            _isInternalReorder = false; // UNLOCK
        }
    }

    [RelayCommand]
    public async Task SortByChapterOrder()
    {
        _isInternalReorder = true; // Lock listener
        try
        {
            // 1. Sort the assignment wrappers based on the Chapter Order, then the Point's order within that chapter
            var sortedAssignments = _storyThread.PlotPointAssignments
                .OrderBy(ppt => ppt.PlotPoint?.Chapter?.OrderIndex ?? int.MaxValue) // Put unassigned chapters last
                .ThenBy(ppt => ppt.PlotPoint?.OrderInChapter ?? int.MaxValue)
                .ToList();

            // 2. Re-populate the Model collection to reflect this order
            _storyThread.PlotPointAssignments.Clear();
            foreach (var assignment in sortedAssignments)
            {
                _storyThread.PlotPointAssignments.Add(assignment);
            }

            // 3. Update the persistent SortOrder index
            for (int i = 0; i < _storyThread.PlotPointAssignments.Count; i++)
            {
                _storyThread.PlotPointAssignments[i].SortOrder = i;
            }

            // 4. Force UI Refresh (Since we blocked the automatic listener)
            PlotPointCollectionViewModel.ViewModelCollection.Clear();
            foreach (var assignment in sortedAssignments)
            {
                if (assignment.PlotPoint != null && MainViewModel.Instance.PlotPointDictionary.TryGetValue(assignment.PlotPoint, out var vm))
                {
                    PlotPointCollectionViewModel.ViewModelCollection.Add(vm);
                }
            }

            await MainViewModel.Instance.SaveChangesSilently();
        }
        finally
        {
            _isInternalReorder = false; // Unlock listener
        }
    }

    [RelayCommand]
    public async Task AddPlotPoint()
    {
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
        };

        var plotPointVM = await MainViewModel.Instance.RegisterNewPlotPoint(plotPoint);

        PlotPointStoryThread junction = new PlotPointStoryThread
        {
            PlotPoint = plotPoint,
            PlotPointId = plotPoint.Id,
            StoryThread = _storyThread,
            StoryThreadId = _storyThread.Id,
            SortOrder = _storyThread.PlotPointAssignments.Count,
            IsPrimary = true,
            ThreadTrajectory = GoalTrajectory.Unset,
        };

        // Add to PlotPoint first (Database requirement)
        plotPoint.ThreadAssignments.Add(junction);

        // Add to Thread (Triggers UI update via Listener)
        // Since this is an "Add", we WANT the listener to fire, so we don't set _isInternalReorder.
        _storyThread.PlotPointAssignments.Add(junction);

        plotPointVM.StoryThreads.Add(this);

        await MainViewModel.Instance.SaveChangesSilently();
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