using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class StoryThreadViewModel : EntityViewModel
{
    private readonly StoryThread _storyThread;
    public StoryThread StoryThread => _storyThread;

    public override bool IsLinkingMode => !NoteCollectionViewModel.IsNoteReorderMode;

    // Shared sorter: chapter order, then position within chapter
    private static readonly Comparison<PlotPoint> ChapterOrderSorter = (a, b) =>
    {
        int aChapter = a.Chapter?.OrderIndex ?? int.MaxValue;
        int bChapter = b.Chapter?.OrderIndex ?? int.MaxValue;
        if (aChapter != bChapter) return aChapter.CompareTo(bChapter);
        return a.OrderInChapter.CompareTo(b.OrderInChapter);
    };

    public StoryThreadViewModel(StoryThread storyThread, IEditorCoordinator editorCoordinator) : base(editorCoordinator)
    {
        _storyThread = storyThread;

        NoteCollectionViewModel = new NoteCollectionViewModel(storyThread.Notes);
        NoteCollectionViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NoteCollectionViewModel.IsNoteReorderMode))
                OnPropertyChanged(nameof(IsLinkingMode));
        };

        var points = _storyThread.PlotPointAssignments.Select(x => x.PlotPoint);
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel();
        PlotPointCollectionViewModel.SetAndSortItems(points, ChapterOrderSorter);

        _storyThread.PlotPointAssignments.CollectionChanged += (s, e) =>
        {
            var updatedPoints = _storyThread.PlotPointAssignments.Select(x => x.PlotPoint);
            PlotPointCollectionViewModel.SetAndSortItems(updatedPoints, ChapterOrderSorter);
        };
    }

    [RelayCommand]
    public async Task AddPlotPoint()
    {
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
        };

        var plotPointVM = await _editorCoordinator.RegisterNewPlotPoint(plotPoint);

        PlotPointStoryThread junction = new PlotPointStoryThread
        {
            PlotPoint = plotPoint,
            PlotPointId = plotPoint.Id,
            StoryThread = _storyThread,
            StoryThreadId = _storyThread.Id,
            IsPrimary = true,
            ThreadTrajectory = GoalTrajectory.Unset,
        };

        plotPoint.ThreadAssignments.Add(junction);
        _storyThread.PlotPointAssignments.Add(junction);

        plotPointVM.StoryThreads.Add(this);
    }

    public void UnlinkPlotPoint(PlotPointViewModelOld plotPointVM)
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