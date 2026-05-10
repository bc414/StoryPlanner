using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using StoryPlanner.Core;

namespace WindowedStoryPlanner.ViewModels;

public partial class FileManagerViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly ProjectLoader _projectLoader;

    [ObservableProperty] private bool _isProjectLoaded;
    [ObservableProperty] private string _windowTitle = "Story Planner";

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
            Filter = "Story Database (*.db)|*.db",
            FileName = "MyNewStory.db"
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
            Filter = "Story Database (*.db)|*.db"
        };

        if (dialog.ShowDialog() == true)
        {
            await _storyService.OpenProjectAsync(dialog.FileName);
            OnProjectLoaded();
        }
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        await _storyService.SaveAsync();
        MessageBox.Show("Saved!", "Story Planner", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    public void JsonToMarkdown()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open Story File",
            Filter = "Google Takeout (*.json)|*.json"
        };

        if (dialog.ShowDialog() == true)
        {
            var exporter = new GeminiExporter();
            exporter.ConvertJsonToMarkdownFiles(dialog.FileName, dialog.FileName + ".md");
        }
    }

    [RelayCommand]
    public async Task StoreGeminiPrompts()
    {
        var dialog = new OpenFileDialog { Title = "Open Gemini Prompts" };

        if (dialog.ShowDialog() == true)
        {
            await _storyService.StoreGeminiEntriesAsync(dialog.FileName);
            RefreshState();
        }
    }

    [RelayCommand]
    public void CopyAiContext()
    {
        if (!IsProjectLoaded) return;

        try
        {
            string json = _storyService.GetAiContextJson(includeVerbatim: false);
            Clipboard.SetText(json);
            MessageBox.Show("Story Context copied to Clipboard!", "AI Ready");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void OnProjectLoaded()
    {
        _projectLoader.Load();
        RefreshState();
        ProjectLoaded?.Invoke();
    }

    private void RefreshState()
    {
        IsProjectLoaded = _storyService.IsProjectLoaded;
        WindowTitle = _storyService.IsProjectLoaded
            ? $"Story Planner - {_storyService.CurrentFilePath}"
            : "Story Planner - No Project Loaded";
    }
}
