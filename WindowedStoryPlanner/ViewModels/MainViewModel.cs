using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.Views;
using System.ComponentModel;
using System.Windows.Data;

namespace WindowedStoryPlanner.ViewModels;

public partial class MainViewModel : ObservableObject
{
    // ... (rest of class)
    public static MainViewModel Instance
    {
        get
        {
            return _instance;
        }
    }

    private static MainViewModel _instance;
    
    private readonly IStoryService _storyService;
    
    [ObservableProperty] private bool _isProjectLoaded;
    [ObservableProperty] private string _windowTitle = "Story Planner";
    
    // --- Lists for the Launcher Widgets ---
    public ObservableCollection<Character> Characters => _storyService.Characters;
    public ObservableCollection<Chapter> Chapters => _storyService.Chapters;
    public ObservableCollection<StoryThread> Threads => _storyService.StoryThreads;
    public ObservableCollection<Theme> Themes => _storyService.Themes;
    public ObservableCollection<Location> Locations => _storyService.Locations;
    public ObservableCollection<CodexEntry> CodexEntries => _storyService.CodexEntries;
    public ObservableCollection<SourceMaterial> SourceMaterials => _storyService.SourceMaterials;

    public ObservableCollection<GeminiEntry> GeminiEntries => _storyService.GeminiEntries;

    // Track open windows to prevent duplicates
    private Dictionary<object, Window> _openWindows = new();
    
    // Track all plot points
    public ObservableCollection<PlotPoint> PlotPoints => _storyService.PlotPoints;

    //View model collections are for library view item templates
    public ObservableCollection<PlotPointViewModel> PlotPointViewModels { get; set; }
    public ObservableCollection<CharacterViewModel> CharacterViewModels { get; set; }
    public ObservableCollection<ChapterViewModel> ChapterViewModels { get; set; }
    public ObservableCollection<StoryThreadViewModel> StoryThreadViewModels { get; set; }
    public ObservableCollection<ThemeViewModel> ThemeViewModels { get; set; }
    public ObservableCollection<LocationViewModel> LocationViewModels { get; set; }
    public ObservableCollection<CodexEntryViewModel> CodexEntryViewModels { get; set; }

    //Need a dictionary for reasonable performance converting navigation property model lists to view model list
    //which is needed for item templates
    public Dictionary<PlotPoint, PlotPointViewModel> PlotPointDictionary = new();
    public Dictionary<Character, CharacterViewModel> CharacterDictionary = new();
    public Dictionary<Chapter, ChapterViewModel> ChapterDictionary = new();
    public Dictionary<StoryThread, StoryThreadViewModel> StoryThreadDictionary = new();
    public Dictionary<Theme, ThemeViewModel> ThemeDictionary = new();
    public Dictionary<Location, LocationViewModel> LocationDictionary = new();
    public Dictionary<CodexEntry, CodexEntryViewModel> CodexEntryDictionary = new();

    //Collection views for sorting/filtering
    private ICollectionView _chaptersView;

    // The PUBLIC PROPERTY the UI binds to, which includes the logic 
    // to raise the INotifyPropertyChanged event.
    public ICollectionView ChaptersView
    {
        get => _chaptersView;
        set
        {
            // 1. Check if the value is changing
            if (_chaptersView != value)
            {
                // 2. Set the private backing field to the new value
                _chaptersView = value;

                // 3. Notify the UI (the View) that the property value has changed.
                OnPropertyChanged(nameof(ChaptersView));
            }
        }
    }


    public MainViewModel(IStoryService storyService)
    {
        _instance = this;
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

    public void UpdateState()
    {
        IsProjectLoaded = _storyService.IsProjectLoaded;
        if (IsProjectLoaded)
        {
            WindowTitle = $"Story Planner - {_storyService.CurrentFilePath}";
            
            // 1. PlotPoints
            PlotPointViewModels = CreateViewModelCollection<PlotPoint, PlotPointViewModel>(
                PlotPoints, // No need to OrderBy here anymore, the View handles it!
                PlotPointDictionary);
            EnableLiveSorting(PlotPointViewModels, nameof(PlotPointViewModel.Title));
        
            // 2. Characters
            CharacterViewModels = CreateViewModelCollection<Character, CharacterViewModel>(
                Characters, 
                CharacterDictionary);
            EnableLiveSorting(CharacterViewModels, nameof(CharacterViewModel.Name));

            // 3. Themes
            ThemeViewModels = CreateViewModelCollection<Theme, ThemeViewModel>(
                Themes, 
                ThemeDictionary);
            EnableLiveSorting(ThemeViewModels, nameof(ThemeViewModel.Name));

            // 4. Chapters (Sort by OrderIndex)
            ChapterViewModels = CreateViewModelCollection<Chapter, ChapterViewModel>(
                Chapters, 
                ChapterDictionary);
            EnableLiveSorting(ChapterViewModels, nameof(ChapterViewModel.OrderIndex));

            // 5. StoryThreads
            StoryThreadViewModels = CreateViewModelCollection<StoryThread, StoryThreadViewModel>(
                Threads, 
                StoryThreadDictionary);
            EnableLiveSorting(StoryThreadViewModels, nameof(StoryThreadViewModel.Name));

            // 6. Locations
            LocationViewModels = CreateViewModelCollection<Location, LocationViewModel>(
                Locations, 
                LocationDictionary);
            EnableLiveSorting(LocationViewModels, nameof(LocationViewModel.Name));

            // 7. CodexEntries
            CodexEntryViewModels = CreateViewModelCollection<CodexEntry, CodexEntryViewModel>(
                CodexEntries, 
                CodexEntryDictionary);
            EnableLiveSorting(CodexEntryViewModels, nameof(CodexEntryViewModel.Title));

            // Must come last because it needs the above dictionaries populated
            foreach(var ppvm in PlotPointViewModels)
            {
                ppvm.PopulateAssociatedViewModelCollections();
            }

            // Notify UI to refresh bindings since the collections were replaced
            OnPropertyChanged(nameof(Characters));
            OnPropertyChanged(nameof(Chapters));
            OnPropertyChanged(nameof(Threads));
            OnPropertyChanged(nameof(GeminiEntries));
            OnPropertyChanged(nameof(PlotPoints));
            OnPropertyChanged(nameof(CodexEntries));
            OnPropertyChanged(nameof(Themes));
            OnPropertyChanged(nameof(Locations));

            OnPropertyChanged(nameof(CharacterViewModels));
            OnPropertyChanged(nameof(PlotPointViewModels));
            OnPropertyChanged(nameof(StoryThreadViewModels));
            OnPropertyChanged(nameof(ThemeViewModels));
            OnPropertyChanged(nameof(ChapterViewModels));
            OnPropertyChanged(nameof(LocationViewModels));
            OnPropertyChanged(nameof(CodexEntryViewModels));
        }
        else
        {
            WindowTitle = "Story Planner - No Project Loaded";
        }
    }
    
    private void EnableLiveSorting<T>(ObservableCollection<T> collection, string sortProperty)
    {
        // Get the default view that the UI binds to
        var view = CollectionViewSource.GetDefaultView(collection);
    
        // 1. clear existing sorts
        view.SortDescriptions.Clear();
    
        // 2. Add the new sort description
        view.SortDescriptions.Add(new SortDescription(sortProperty, ListSortDirection.Ascending));
    
        // 3. Enable Live Sorting (so it updates on edits)
        // 3. CAST to ICollectionViewLiveShaping to access Live Sorting features
        if (view is ICollectionViewLiveShaping liveView)
        {
            if (liveView.CanChangeLiveSorting)
            {
                liveView.IsLiveSorting = true;
                liveView.LiveSortingProperties.Add(sortProperty);
            }
        }
    }
    
    public ObservableCollection<TViewModel> CreateViewModelCollection<TModel, TViewModel>(
        IEnumerable<TModel> models, Dictionary<TModel, TViewModel> map)
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

    [RelayCommand]
    public void AddChapter()
    {
        Chapter newChapter = new Chapter();
        _storyService.Chapters.Add(newChapter);
        ChapterViewModel viewModel = new ChapterViewModel(newChapter);
        ChapterViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteChapter(ChapterViewModel viewModel)
    {
        Chapter chapter = viewModel.Chapter;
        if (MessageBox.Show($"Are you sure you want to delete chapter '{chapter.Title}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            ChapterViewModels.Remove(viewModel);
            ChapterDictionary.Remove(chapter);
            _storyService.Chapters.Remove(chapter);
        }
    }

    [RelayCommand]
    public void AddStoryThread()
    {
        StoryThread newStoryThread = new StoryThread();
        _storyService.StoryThreads.Add(newStoryThread);
        StoryThreadViewModel viewModel = new StoryThreadViewModel(newStoryThread);
        StoryThreadViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteStoryThread(StoryThreadViewModel viewModel)
    {
        StoryThread storyThread = viewModel.StoryThread;
        if (MessageBox.Show($"Are you sure you want to delete story thread '{storyThread.Name}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            StoryThreadViewModels.Remove(viewModel);
            StoryThreadDictionary.Remove(storyThread);
            _storyService.StoryThreads.Remove(storyThread);
        }
    }

    [RelayCommand]
    public void AddTheme()
    {
        Theme newTheme = new Theme();
        _storyService.Themes.Add(newTheme);
        ThemeViewModel viewModel = new ThemeViewModel(newTheme);
        ThemeViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteTheme(ThemeViewModel viewModel)
    {
        Theme theme = viewModel.Theme;
        if (MessageBox.Show($"Are you sure you want to delete theme '{theme.Name}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            ThemeViewModels.Remove(viewModel);
            ThemeDictionary.Remove(theme);
            _storyService.Themes.Remove(theme);
        }
    }

    [RelayCommand]
    public void AddLocation()
    {
        Location newLocation = new Location();
        _storyService.Locations.Add(newLocation);
        LocationViewModel viewModel = new LocationViewModel(newLocation);
        LocationViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteLocation(LocationViewModel viewModel)
    {
        Location location = viewModel.Location;
        if (MessageBox.Show($"Are you sure you want to delete location '{location.Name}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            LocationViewModels.Remove(viewModel);
            LocationDictionary.Remove(location);
            _storyService.Locations.Remove(location);
        }
    }

    [RelayCommand]
    public void AddCodexEntry()
    {
        CodexEntry newCodexEntry = new CodexEntry();
        _storyService.CodexEntries.Add(newCodexEntry);
        CodexEntryViewModel viewModel = new CodexEntryViewModel(newCodexEntry);
        CodexEntryViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteCodexEntry(CodexEntryViewModel viewModel)
    {
        CodexEntry codexEntry = viewModel.CodexEntry;
        if (MessageBox.Show($"Are you sure you want to delete codex entry '{codexEntry.Title}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            CodexEntryViewModels.Remove(viewModel);
            CodexEntryDictionary.Remove(codexEntry);
            _storyService.CodexEntries.Remove(codexEntry);
        }
    }

    // --- LAUNCHER COMMANDS ---

    [RelayCommand]
    public void OpenGeminiEntry(GeminiEntry geminiEntry)
    {
        Window window = new GeminiEntryWindow();
        window.DataContext = geminiEntry;
        window.Show();
    }

    [RelayCommand]
    public void OpenFloatingPoints()
    {
        // Use a specific key for the singleton window
        string key = "FloatingPoints"; 
        if (ActivateIfOpen(key)) return;

        var vm = new FloatingPlotPointsViewModel();
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

        if (window == null)
        {
            MessageBox.Show("Could not determine the correct window type for this ViewModel.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    
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
        else if(viewModel is PlotPointViewModel)
        {
            return new PlotPointWindow();
        }
        else if (viewModel is ChapterViewModel)
        {
            return new ChapterWindow();
        }
        
        else if (viewModel is StoryThreadViewModel)
        {
            return new StoryThreadWindow();
        }
        else if (viewModel is ThemeViewModel)
        {
            return new ThemeWindow();
        }
        else if (viewModel is LocationViewModel)
        {
            return new LocationWindow();
        }
        else if (viewModel is CodexEntryViewModel)
        {
            return new CodexEntryWindow();
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