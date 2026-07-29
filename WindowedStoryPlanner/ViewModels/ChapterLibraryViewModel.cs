using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterLibraryViewModel : ObservableObject
{
    private readonly IContentFactory    _factory;
    private readonly IContentDeleter    _deleter;
    private readonly IWindowManager     _windowManager;
    private readonly IViewModelRegistry _registry;
    private readonly IStoryService      _storyService;

    /// <summary>Sorted by (story reading order, chapter order) and filtered by SelectedStoryFilter.</summary>
    public ICollectionView Chapters { get; }

    public ObservableCollection<StoryOption> StoryFilterOptions { get; private set; }

    [ObservableProperty]
    private StoryOption? _selectedStoryFilter;

    public ChapterLibraryViewModel(
        IContentFactory     factory,
        IContentDeleter     deleter,
        IWindowManager      windowManager,
        IViewModelRegistry  registry,
        IStoryService       storyService)
    {
        _factory       = factory;
        _deleter       = deleter;
        _windowManager = windowManager;
        _registry      = registry;
        _storyService  = storyService;

        Chapters = new ListCollectionView(_registry.AllChapterViewModels)
        {
            Filter = FilterByStory,
            CustomSort = Comparer<object>.Create((a, b) =>
            {
                if (a is not ChapterViewModel ca || b is not ChapterViewModel cb) return 0;
                var storyOrderA = _registry.AllStoryViewModels.FirstOrDefault(s => s.Id == ca.StoryId)?.OrderIndex ?? 0;
                var storyOrderB = _registry.AllStoryViewModels.FirstOrDefault(s => s.Id == cb.StoryId)?.OrderIndex ?? 0;
                int cmp = storyOrderA.CompareTo(storyOrderB);
                return cmp != 0 ? cmp : ca.OrderIndex.CompareTo(cb.OrderIndex);
            })
        };

        StoryFilterOptions = StoryOption.BuildFilterList(_registry.AllStoryViewModels);
        SelectedStoryFilter = StoryFilterOptions.FirstOrDefault();

        _registry.AllStoryViewModels.CollectionChanged += (_, _) => RebuildStoryFilterOptions();
    }

    private void RebuildStoryFilterOptions()
    {
        var previouslySelectedId = SelectedStoryFilter?.Id;
        StoryFilterOptions = StoryOption.BuildFilterList(_registry.AllStoryViewModels);
        OnPropertyChanged(nameof(StoryFilterOptions));
        SelectedStoryFilter = StoryFilterOptions.FirstOrDefault(o => o.Id == previouslySelectedId)
            ?? StoryFilterOptions.FirstOrDefault();
    }

    partial void OnSelectedStoryFilterChanged(StoryOption? value) => Chapters.Refresh();

    private bool FilterByStory(object obj)
    {
        if (obj is not ChapterViewModel ch) return true;
        if (SelectedStoryFilter is null || SelectedStoryFilter.Id == StoryOption.AllStoriesId) return true;
        return ch.StoryId == SelectedStoryFilter.Id;
    }

    [RelayCommand]
    private async Task AddChapter()
    {
        var vm = await _factory.CreateChapterAsync();

        int targetStoryId = 0;
        if (SelectedStoryFilter is { } filter && filter.Id != StoryOption.AllStoriesId)
            targetStoryId = filter.Id;

        vm.StoryId = targetStoryId;
        vm.OrderIndex = _registry.AllChapterViewModels
            .Where(c => c.StoryId == targetStoryId && c.Id != vm.Id)
            .Select(c => c.OrderIndex)
            .DefaultIfEmpty(0)
            .Max() + 1;

        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void OpenChapter(ChapterViewModel chapter) =>
        _windowManager.OpenChapterWindow(chapter);

    [RelayCommand]
    private void MoveChapterUp(ChapterViewModel vm)
    {
        var siblings = _registry.AllChapterViewModels
            .Where(c => c.StoryId == vm.StoryId)
            .OrderBy(c => c.OrderIndex)
            .ToList();
        int index = siblings.IndexOf(vm);
        if (index <= 0) return;

        var other = siblings[index - 1];
        (vm.OrderIndex, other.OrderIndex) = (other.OrderIndex, vm.OrderIndex);
        Chapters.Refresh();
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void MoveChapterDown(ChapterViewModel vm)
    {
        var siblings = _registry.AllChapterViewModels
            .Where(c => c.StoryId == vm.StoryId)
            .OrderBy(c => c.OrderIndex)
            .ToList();
        int index = siblings.IndexOf(vm);
        if (index < 0 || index >= siblings.Count - 1) return;

        var other = siblings[index + 1];
        (vm.OrderIndex, other.OrderIndex) = (other.OrderIndex, vm.OrderIndex);
        Chapters.Refresh();
        _ = _storyService.SaveAsync();
    }

    /// <summary>
    /// Opens the Move… dialog: pick a target story, an anchor chapter within it, before/after.
    /// Replaces repeated ▲/▼ clicks for cross-story moves and arbitrary repositioning; ▲/▼ stay
    /// for same-story nudges.
    /// </summary>
    [RelayCommand]
    private void OpenMoveDialog(ChapterViewModel chapter)
    {
        var dialogVm = new MoveChapterViewModel(chapter, _registry);
        var window = new MoveChapterWindow { DataContext = dialogVm, Owner = Application.Current.MainWindow };
        if (window.ShowDialog() != true || dialogVm.SelectedStory is null) return;

        PerformMove(chapter, dialogVm.SelectedStory, dialogVm.SelectedAnchor, dialogVm.PlaceBefore);
        Chapters.Refresh();
        _ = _storyService.SaveAsync();
    }

    private void PerformMove(ChapterViewModel chapter, StoryOption targetStory, ChapterViewModel? anchor, bool placeBefore)
    {
        int oldStoryId = chapter.StoryId;
        int newStoryId = targetStory.Id;

        var siblings = _registry.AllChapterViewModels
            .Where(c => c.StoryId == newStoryId && c.Id != chapter.Id)
            .OrderBy(c => c.OrderIndex)
            .ToList();

        int insertAt = anchor is null
            ? siblings.Count
            : siblings.IndexOf(anchor) + (placeBefore ? 0 : 1);
        insertAt = Math.Clamp(insertAt, 0, siblings.Count);

        siblings.Insert(insertAt, chapter);
        for (int i = 0; i < siblings.Count; i++)
            siblings[i].OrderIndex = i + 1;

        chapter.StoryId = newStoryId;

        // The source story's remaining chapters must stay contiguous too.
        if (oldStoryId != newStoryId)
        {
            var oldSiblings = _registry.AllChapterViewModels
                .Where(c => c.StoryId == oldStoryId && c.Id != chapter.Id)
                .OrderBy(c => c.OrderIndex)
                .ToList();
            for (int i = 0; i < oldSiblings.Count; i++)
                oldSiblings[i].OrderIndex = i + 1;
        }
    }

    [RelayCommand]
    private async Task DeleteChapter(ChapterViewModel vm)
    {
        if (!await _deleter.TryDeleteChapterAsync(vm))
            MessageBox.Show(
                "Cannot delete a chapter that still has notes.",
                "Delete Failed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }
}
