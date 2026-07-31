using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowedStoryPlanner;

public partial class StoryLibraryViewModel : ObservableObject
{
    private readonly IContentFactory    _factory;
    private readonly IContentDeleter    _deleter;
    private readonly IViewModelRegistry _registry;
    private readonly IStoryService      _storyService;

    public ObservableCollection<StoryViewModel> Stories => _registry.AllStoryViewModels;

    public StoryLibraryViewModel(
        IContentFactory     factory,
        IContentDeleter     deleter,
        IViewModelRegistry  registry,
        IStoryService       storyService)
    {
        _factory      = factory;
        _deleter      = deleter;
        _registry     = registry;
        _storyService = storyService;
    }

    [RelayCommand]
    private async Task AddStory()
    {
        var vm = await _factory.CreateStoryAsync();

        // Set OrderIndex to max + 1 — same convention as ChapterLibraryViewModel.AddChapter.
        vm.OrderIndex = (Stories.Count > 0 ? Stories.Max(s => s.OrderIndex) : 0) + 1;
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void MoveStoryUp(StoryViewModel vm)
    {
        int index = Stories.IndexOf(vm);
        if (index <= 0) return;

        var other = Stories[index - 1];
        (vm.OrderIndex, other.OrderIndex) = (other.OrderIndex, vm.OrderIndex);
        Stories.Move(index, index - 1);
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void MoveStoryDown(StoryViewModel vm)
    {
        int index = Stories.IndexOf(vm);
        if (index < 0 || index >= Stories.Count - 1) return;

        var other = Stories[index + 1];
        (vm.OrderIndex, other.OrderIndex) = (other.OrderIndex, vm.OrderIndex);
        Stories.Move(index, index + 1);
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private async Task DeleteStory(StoryViewModel vm)
    {
        // Always succeeds — Story is container-only (no notes to guard on); its chapters are
        // orphaned to the "(Unassigned)" sentinel rather than refused or cascaded. The bool
        // return is kept for the same shape as the other TryDelete*Async guards.
        if (!await _deleter.TryDeleteStoryAsync(vm))
            MessageBox.Show(
                "Could not delete story.",
                "Delete Failed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }
}
