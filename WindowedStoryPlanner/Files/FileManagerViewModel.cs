using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

public partial class FileManagerViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly ProjectLoader _projectLoader;

    [ObservableProperty] private bool _isProjectLoaded;

    public FileManagerViewModel(IStoryService storyService, ProjectLoader projectLoader)
    {
        _storyService  = storyService;
        _projectLoader = projectLoader;
    }

    /// <summary>
    /// Raised after a successful CreateNewProject or OpenProject so the shell
    /// can react (e.g. refresh bindings, update title).
    /// </summary>
    public event Action? ProjectLoaded;

    [RelayCommand]
    public async Task CreateNewProject()
    {
        var dialog = new SaveFileDialog
        {
            Title = "Create New Story File",
            Filter = "Story Planner File (*.storyplan)|*.storyplan",
            FileName = "MyNewStory.storyplan"
        };

        if (dialog.ShowDialog() == true)
        {
            await _storyService.CreateProjectAsync(dialog.FileName);
            OnProjectLoaded();
        }
    }

    [RelayCommand]
    public async Task OpenProject()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open Story File",
            Filter = "Story Database (*.storyplan)|*.storyplan"
        };

        if (dialog.ShowDialog() == true)
        {
            await _storyService.OpenProjectAsync(dialog.FileName);
            OnProjectLoaded();
        }
    }

    public async Task OpenProjectFromPath(string path)
    {
        await _storyService.OpenProjectAsync(path);
        OnProjectLoaded();
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        await _storyService.SaveAsync();
        MessageBox.Show("Saved!", "Story Planner", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void OnProjectLoaded()
    {
        _projectLoader.Load();
        RefreshState();
        ProjectLoaded?.Invoke();
    }

    private void RefreshState() => IsProjectLoaded = _storyService.IsProjectLoaded;
}
