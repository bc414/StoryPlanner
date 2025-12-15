using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public static MainViewModel Instance { get; private set; }
    
    private readonly IStoryService _storyService;
    
    [ObservableProperty] private bool _isProjectLoaded;
    [ObservableProperty] private string _windowTitle = "Story Planner";
    
    // --- Lists for the Launcher Widgets ---
    public ObservableCollection<Character> Characters => _storyService.Characters;
    public ObservableCollection<Chapter> Chapters => _storyService.Chapters;
    public ObservableCollection<StoryThread> Threads => _storyService.Threads;
    
    public ObservableCollection<GeminiEntry> GeminiEntries => _storyService.GeminiEntries;

    // Track open windows to prevent duplicates
    private Dictionary<object, Window> _openWindows = new();
    
    // Track all plot points
    public ObservableCollection<PlotPoint> PlotPoints => _storyService.PlotPoints;

    //View model collections are for library view item templates
    public ObservableCollection<PlotPointViewModel> PlotPointViewModels { get; set; }
    public ObservableCollection<CharacterViewModel> CharacterViewModels { get; set; }
    
    //Need a dictionary for reasonable performance converting navigation property model lists to view model list
    //which is needed for item templates
    public Dictionary<PlotPoint, PlotPointViewModel> PlotPointDictionary = new();
    public Dictionary<Character, CharacterViewModel> CharacterDictionary = new();
    

    public MainViewModel(IStoryService storyService)
    {
        Instance = this;
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
            Title = "Open Gemini Prompts"
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
            OnPropertyChanged(nameof(GeminiEntries));
            OnPropertyChanged(nameof(PlotPoints));
            
            
            PlotPointViewModels = CreateViewModelCollection<PlotPoint, PlotPointViewModel>(PlotPoints, PlotPointDictionary);
            
            CharacterViewModels = CreateViewModelCollection<Character, CharacterViewModel>(Characters, CharacterDictionary);
        }
        else
        {
            WindowTitle = "Story Planner - No Project Loaded";
        }
    }
    
    public ObservableCollection<TViewModel> CreateViewModelCollection<TModel, TViewModel>(
        ObservableCollection<TModel> models, Dictionary<TModel, TViewModel> map)
    {
        ObservableCollection<TViewModel> collection = new ObservableCollection<TViewModel>();
        foreach (TModel model in models)
        {
            var vm = (TViewModel)Activator.CreateInstance(typeof(TViewModel), model);
            collection.Add(vm);
            map[model] = vm;
        }

        return collection;
    }

    public PlotPointViewModel RegisterNewPlotPoint(PlotPoint plotPoint)
    {
        _storyService.PlotPoints.Add(plotPoint);
        PlotPointViewModel viewModel = new PlotPointViewModel(plotPoint);
        PlotPointViewModels.Add(viewModel);
        PlotPointDictionary[plotPoint] = viewModel;
        OpenEditorWindow(viewModel);
        return viewModel;
        //caller must add relevant connections. If chapter window, then add the plotPoint to the chapter's list
    }

    [RelayCommand]
    public void AddCharacter()
    {
        Character newCharacter = new Character();
        _storyService.Characters.Add(newCharacter);
        CharacterViewModel viewModel = new CharacterViewModel(newCharacter);
        CharacterViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteCharacter(CharacterViewModel viewModel)
    {
        Character character = viewModel.Character;
        if (MessageBox.Show($"Are you sure you want to delete character '{character.Name}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            CharacterViewModels.Remove(viewModel);
            CharacterDictionary.Remove(character);
            _storyService.Characters.Remove(character);
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
    
    public void OpenEditorWindow(EntityViewModel viewModel)
    {
        if (viewModel == null) return;

        // 1. CHECK: Is it already open?
        if (_openWindows.TryGetValue(viewModel, out var existingWin))
        {
            // Safety: Check if it's actually alive (user might have closed it)
            if (existingWin.IsLoaded)
            {
                existingWin.Activate(); // Bring to front
                if (existingWin.WindowState == WindowState.Minimized)
                    existingWin.WindowState = WindowState.Normal;
                return;
            }
            // If it's not loaded, remove the dead reference
            _openWindows.Remove(viewModel);
        }

        // 2. CREATE: Make the Window
        var window = GetWindowBasedOnType(viewModel);
    
        window.DataContext = viewModel;

        // 3. TRACK: Add to dictionary and listen for close
        _openWindows[viewModel] = window;
        window.Closed += (s, e) => _openWindows.Remove(viewModel);

        window.Show();
    }

    private Window GetWindowBasedOnType(object viewModel)
    {
        if (viewModel is CharacterViewModel)
        {
            return new CharacterWindow();
        }
        else if (viewModel is ChapterViewModel)
        {
            
        }
        else if (viewModel is GeminiEntry)
        {
            return new GeminiEntryWindow();
        }

        return null;
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