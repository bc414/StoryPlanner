using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterViewModel : OwnerViewModel
{
    private readonly Chapter _chapter;

    public ICollectionView PlotPointsInChapter { get; }

    public int Id => _chapter.Id;

    public ChapterViewModel(
        Chapter chapter,
        IViewModelRegistry viewModelRegistry,
        IStoryService storyService,
        IEditorCoordinator editorCoordinator)
        : base(viewModelRegistry, storyService, editorCoordinator)
    {
        _chapter = chapter;

        var noteTracks = storyService.NoteTrackDefinitions
            .Where(ntd => ntd.OwnerType == OwnerType.Chapter)
            .ToList();

        var propertyDefs = storyService.NarrativePropertyDefinitions
            .Where(npd => npd.OwnerType == OwnerType.Chapter)
            .ToList();

        InitializeCollections(chapter.Id, OwnerType.Chapter, noteTracks, propertyDefs);

        PlotPointsInChapter = CollectionViewSource.GetDefaultView(
            viewModelRegistry.AllPlotPointViewModels);
        PlotPointsInChapter.Filter = FilterPlotPoints;
        PlotPointsInChapter.SortDescriptions.Add(
            new SortDescription(nameof(PlotPointViewModel.OrderInChapter), ListSortDirection.Ascending));
    }

    private bool FilterPlotPoints(object obj)
    {
        if (obj is not PlotPointViewModel plotPoint) return false;
        return plotPoint.ChapterId == _chapter.Id;
    }

    // ── Properties ───────────────────────────────────────────────────────

    public string FullTitle => $"{OrderIndex}. {Title}";

    public string Title
    {
        get => _chapter.Title;
        set
        {
            if (SetProperty(_chapter.Title, value, _chapter, (u, n) => u.Title = n))
                OnPropertyChanged(nameof(FullTitle));
        }
    }

    public int OrderIndex
    {
        get => _chapter.OrderIndex;
        set
        {
            if (SetProperty(_chapter.OrderIndex, value, _chapter, (u, n) => u.OrderIndex = n))
                OnPropertyChanged(nameof(FullTitle));
        }
    }

    public string Summary
    {
        get => _chapter.Summary;
        set => SetProperty(_chapter.Summary, value, _chapter, (u, n) => u.Summary = n);
    }

    public string Description
    {
        get => _chapter.Description;
        set => SetProperty(_chapter.Description, value, _chapter, (u, n) => u.Description = n);
    }

    // ── Commands ─────────────────────────────────────────────────────────

    [RelayCommand]
    public async Task AddPlotPoint()
    {
        // Determine the next sort order from the service layer so it is
        // correct even if the window is not yet open (Sections not initialized).
        int nextOrder = _storyService.PlotPoints
            .Where(p => p.ChapterId == _chapter.Id)
            .Select(p => p.OrderInChapter)
            .DefaultIfEmpty(0)
            .Max() + 1;

        await _editorCoordinator.CreatePlotPointAsync(_chapter.Id, nextOrder);
    }
}