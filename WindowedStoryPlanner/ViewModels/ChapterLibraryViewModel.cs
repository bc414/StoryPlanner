using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterLibraryViewModel : ObservableObject
{
    private readonly IContentFactory    _factory;
    private readonly IContentDeleter    _deleter;
    private readonly IWindowManager     _windowManager;
    private readonly IViewModelRegistry _registry;
    private readonly IStoryService      _storyService;

    public ObservableCollection<ChapterViewModel> Chapters => _registry.AllChapterViewModels;

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
    }

    [RelayCommand]
    private async Task AddChapter()
    {
        var vm = await _factory.CreateChapterAsync();
        
        // Set OrderIndex to max + 1
        vm.OrderIndex = (Chapters.Count > 0 ? Chapters.Max(c => c.OrderIndex) : 0) + 1;
        
        //_windowManager.OpenChapterWindow(vm);
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void OpenChapter(ChapterViewModel chapter) =>
        _windowManager.OpenChapterWindow(chapter);

    [RelayCommand]
    private void MoveChapterUp(ChapterViewModel vm)
    {
        int index = Chapters.IndexOf(vm);
        if (index <= 0) return;

        var other = Chapters[index - 1];
        (vm.OrderIndex, other.OrderIndex) = (other.OrderIndex, vm.OrderIndex);
        Chapters.Move(index, index - 1);
        _ = _storyService.SaveAsync();
    }

    [RelayCommand]
    private void MoveChapterDown(ChapterViewModel vm)
    {
        int index = Chapters.IndexOf(vm);
        if (index < 0 || index >= Chapters.Count - 1) return;

        var other = Chapters[index + 1];
        (vm.OrderIndex, other.OrderIndex) = (other.OrderIndex, vm.OrderIndex);
        Chapters.Move(index, index + 1);
        _ = _storyService.SaveAsync();
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
