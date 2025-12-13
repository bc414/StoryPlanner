using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Models;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly StoryService _storyService;
    
    [ObservableProperty] private bool _isProjectLoaded;
    [ObservableProperty] private string _windowTitle = "Story Planner";
    
    // --- Lists for the Launcher Widgets ---
    public ObservableCollection<Character> Characters => _storyService.Characters;
    public ObservableCollection<Chapter> Chapters => _storyService.Chapters;
    public ObservableCollection<StoryThread> Threads => _storyService.Threads;

    // Track open windows to prevent duplicates
    private Dictionary<object, Window> _openWindows = new();

    public MainViewModel(StoryService storyService)
    {
        _storyService = storyService;
        UpdateState();
    }

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
            UpdateState();
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
            UpdateState();
        }
    }

    [RelayCommand]
    public async Task StoreGeminiPrompts()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open Gemini Prompts",
            Filter = "*.json"
        };

        if (dialog.ShowDialog() == true)
        {
            await _storyService.StoreGeminiEntriesAsync(dialog.FileName);
            UpdateState();
        }
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        await _storyService.SaveAsync();
        MessageBox.Show("Saved!", "Story Planner", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void UpdateState()
    {
        IsProjectLoaded = _storyService.IsProjectLoaded;
        if (IsProjectLoaded)
        {
            WindowTitle = $"Story Planner - {_storyService.CurrentFilePath}";
            // Notify UI to refresh bindings since the collections were replaced
            OnPropertyChanged(nameof(Characters));
            OnPropertyChanged(nameof(Chapters));
            OnPropertyChanged(nameof(Threads));
        }
        else
        {
            WindowTitle = "Story Planner - No Project Loaded";
        }
    }

    // --- LAUNCHER COMMANDS ---

    [RelayCommand]
    public void OpenFloatingPoints()
    {
        // Use a specific key for the singleton window
        string key = "FloatingPoints"; 
        if (ActivateIfOpen(key)) return;

        var vm = new FloatingPlotPointsViewModel(_storyService);
        var win = new FloatingPlotPointsWindow { DataContext = vm };
        
        RegisterWindow(key, win);
        win.Show();
    }

    [RelayCommand]
    public void OpenCharacter(Character character)
    {
        if (character == null) return;
        if (ActivateIfOpen(character)) return;

        var vm = new CharacterViewModel(character);
        var win = new CharacterWindow { DataContext = vm };
        
        RegisterWindow(character, win);
        win.Show();
    }

    [RelayCommand]
    public void OpenChapter(Chapter chapter)
    {
        if (chapter == null) return;
        if (ActivateIfOpen(chapter)) return;

        // Assumes you have/will create ChapterViewModel & ChapterWindow
        // var vm = new ChapterViewModel(chapter, _storyService);
        // var win = new ChapterWindow { DataContext = vm };
        
        // RegisterWindow(chapter, win);
        // win.Show();
        
        MessageBox.Show($"Opening Chapter {chapter.OrderIndex}: {chapter.Title}");
    }

    [RelayCommand]
    public void OpenThread(StoryThread thread)
    {
        if (thread == null) return;
        MessageBox.Show($"Opening Thread: {thread.Name}");
    }

    // --- WINDOW MANAGEMENT HELPERS ---

    private bool ActivateIfOpen(object key)
    {
        if (_openWindows.TryGetValue(key, out var win))
        {
            if (win.WindowState == WindowState.Minimized)
                win.WindowState = WindowState.Normal;
            win.Activate();
            return true;
        }
        return false;
    }

    private void RegisterWindow(object key, Window win)
    {
        win.Closed += (s, e) => _openWindows.Remove(key);
        _openWindows.Add(key, win);
    }
}