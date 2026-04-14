using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.Views;
using System.ComponentModel;
using System.IO;
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
    public ObservableCollection<Idea> Ideas => _storyService.Ideas;

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

    public PlotPointCollectionViewModel PlotPointsByTextViewModel { get; set; } = new();

    [ObservableProperty]
    private NoteCollectionViewModel _unassignedNotesVM;

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

    public ICollectionView CodexEntriesGroupedView { get; private set; }
    
    // 1. The Source Collection
    public ObservableCollection<EntityViewModel> DashboardEntities { get; } = new();

    // 2. The Sorted View for the UI
    [ObservableProperty]
    private ICollectionView _dashboardEntitiesView;

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
    public void JsonToMarkdown()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open Story File",
            Filter = "Google Takeout (*.json)|*.json"
        };

        if (dialog.ShowDialog() == true)
        {
            string fileName = dialog.FileName;
            var exporter = new GeminiExporter();
            exporter.ConvertJsonToMarkdownFiles(
                fileName, fileName + ".md"
            );
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

    public async Task SaveChangesSilently()
    {
        await _storyService.SaveAsync();
    }

    public void UpdateState()
    {
        IsProjectLoaded = _storyService.IsProjectLoaded;
        if (IsProjectLoaded)
        {
            WindowTitle = $"Story Planner - {_storyService.CurrentFilePath}";

            UnassignedNotesVM = new NoteCollectionViewModel(_storyService.UnassignedNotes);

            // 1. PlotPoints
            PlotPointViewModels = CreateViewModelCollection<PlotPoint, PlotPointViewModel>(
                PlotPoints, // No need to OrderBy here anymore, the View handles it!
                PlotPointDictionary);
            EnableLiveSorting(PlotPointViewModels, nameof(PlotPointViewModel.Title));
            
            PlotPointsByTextViewModel.SetAndSortItems(PlotPoints, (p1, p2) => 
            {
                int len1 = p1.GetTotalTextLength();
                int len2 = p2.GetTotalTextLength();
                // Sort descending (longest text first)
                return len2.CompareTo(len1); 
            });
        
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
            
            // --- NEW GROUPING LOGIC ---
            // 1. Get the default view for the collection
            CodexEntriesGroupedView = CollectionViewSource.GetDefaultView(CodexEntryViewModels);
        
            // 2. Clear old groups (important for reloading)
            CodexEntriesGroupedView.GroupDescriptions.Clear();
        
            // 3. Add the GroupDescription based on the 'Category' property
            CodexEntriesGroupedView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CodexEntryViewModel.Category)));
            
            // --- THE FIX: ENABLE LIVE GROUPING ---
            // This tells WPF to move the item to the correct group instantly when "Category" changes.
            if (CodexEntriesGroupedView is ICollectionViewLiveShaping liveView && liveView.CanChangeLiveGrouping)
            {
                liveView.IsLiveGrouping = true;
                liveView.LiveGroupingProperties.Add(nameof(CodexEntryViewModel.Category));
            }

            // Must come last because it needs the above dictionaries populated
            foreach(var ppvm in PlotPointViewModels)
            {
                ppvm.PopulateAssociatedViewModelCollections();
            }
            
            RefreshDashboardList();

            // Notify UI to refresh bindings since the collections were replaced
            OnPropertyChanged(nameof(Characters));
            OnPropertyChanged(nameof(Chapters));
            OnPropertyChanged(nameof(Threads));
            OnPropertyChanged(nameof(GeminiEntries));
            OnPropertyChanged(nameof(Ideas));
            OnPropertyChanged(nameof(PlotPoints));
            OnPropertyChanged(nameof(CodexEntries));
            OnPropertyChanged(nameof(Themes));
            OnPropertyChanged(nameof(Locations));
            OnPropertyChanged(nameof(SourceMaterials));

            OnPropertyChanged(nameof(CharacterViewModels));
            OnPropertyChanged(nameof(PlotPointViewModels));
            OnPropertyChanged(nameof(StoryThreadViewModels));
            OnPropertyChanged(nameof(ThemeViewModels));
            OnPropertyChanged(nameof(ChapterViewModels));
            OnPropertyChanged(nameof(LocationViewModels));
            OnPropertyChanged(nameof(CodexEntryViewModels));
            OnPropertyChanged(nameof(CodexEntriesGroupedView));

            OnPropertyChanged(nameof(UnassignedNotesVM));

            RefreshStatistics();
        }
        else
        {
            WindowTitle = "Story Planner - No Project Loaded";
        }
    }
    
    [RelayCommand]
    public void OpenNoteCategorizer()
    {
        // Use a specific key for the singleton window
        string key = "NoteCategorizer"; 
    
        // Checks if the window is already open and brings it to the front if it is
        if (ActivateIfOpen(key)) return;

        // Instantiate the new temporary whiteboard components
        var vm = new CategorizerViewModel(); 
        var win = new CategorizerView { DataContext = vm };
    
        // Registers the window so MainViewModel knows to track it
        RegisterWindow(key, win);
        win.Show();
    }
    
    private void RefreshDashboardList()
    {
        DashboardEntities.Clear();

        // 1. Add only the specific types you requested
        foreach (var vm in CharacterViewModels) DashboardEntities.Add(vm);
        foreach (var vm in ThemeViewModels) DashboardEntities.Add(vm);
        foreach (var vm in StoryThreadViewModels) DashboardEntities.Add(vm);
        foreach (var vm in ChapterViewModels) DashboardEntities.Add(vm);
        foreach (var vm in CodexEntryViewModels) DashboardEntities.Add(vm);

        // 2. Create View
        DashboardEntitiesView = CollectionViewSource.GetDefaultView(DashboardEntities);

        // 3. Apply Sort (CharacterCount Descending)
        DashboardEntitiesView.SortDescriptions.Clear();
        DashboardEntitiesView.SortDescriptions.Add(new SortDescription(nameof(EntityViewModel.CharacterCount), ListSortDirection.Descending));

        SearchResultsView = new ListCollectionView(DashboardEntities);
        SearchResultsView.Filter = FilterSearchResults;
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

    public async Task<PlotPointViewModel> RegisterNewPlotPoint(PlotPoint plotPoint)
    {
        _storyService.PlotPoints.Add(plotPoint);
        await _storyService.SaveAsync();
        PlotPointViewModel viewModel = new PlotPointViewModel(plotPoint);
        PlotPointViewModels.Add(viewModel);
        PlotPointDictionary[plotPoint] = viewModel;
        OpenEditorWindow(viewModel);
        return viewModel;
        //caller must add relevant connections. If chapter window, then add the plotPoint to the chapter's list
    }

    [RelayCommand]
    public async Task AddCharacter()
    {
        Character newCharacter = new Character();
        _storyService.Characters.Add(newCharacter);
        await _storyService.SaveAsync();
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
    public async Task AddChapter()
    {
        Chapter newChapter = new Chapter();
        _storyService.Chapters.Add(newChapter);
        await _storyService.SaveAsync();
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
    public async Task AddStoryThread()
    {
        StoryThread newStoryThread = new StoryThread();
        _storyService.StoryThreads.Add(newStoryThread);
        await _storyService.SaveAsync();
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
    public async Task AddTheme()
    {
        Theme newTheme = new Theme();
        _storyService.Themes.Add(newTheme);
        await _storyService.SaveAsync();
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
    public async Task AddLocation()
    {
        Location newLocation = new Location();
        _storyService.Locations.Add(newLocation);
        await _storyService.SaveAsync();
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
    public async Task AddCodexEntry()
    {
        CodexEntry newCodexEntry = new CodexEntry();
        _storyService.CodexEntries.Add(newCodexEntry);
        await _storyService.SaveAsync();
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

    [RelayCommand]
    public async Task AddIdea()
    {
        Idea newIdea = new Idea();
        _storyService.Ideas.Add(newIdea);
        await _storyService.SaveAsync();
    }

    // --- LAUNCHER COMMANDS ---
    
    [RelayCommand]
    public void OpenIdea(Idea idea)
    {
        Window window = new IdeaWindow();
        window.DataContext = idea;
        window.Show();
    }

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

    [RelayCommand]
    public void OpenRandomAnalysisEntity()
    {
        var pendingEntities = new List<EntityViewModel>();

        // Gather all entities that have at least one note requiring analysis
        if (CharacterViewModels != null) pendingEntities.AddRange(CharacterViewModels.Where(x => x.HasAnalysisPending));
        if (PlotPointViewModels != null) pendingEntities.AddRange(PlotPointViewModels.Where(x => x.HasAnalysisPending));
        if (ChapterViewModels != null) pendingEntities.AddRange(ChapterViewModels.Where(x => x.HasAnalysisPending));
        if (StoryThreadViewModels != null) pendingEntities.AddRange(StoryThreadViewModels.Where(x => x.HasAnalysisPending));
        if (ThemeViewModels != null) pendingEntities.AddRange(ThemeViewModels.Where(x => x.HasAnalysisPending));
        if (LocationViewModels != null) pendingEntities.AddRange(LocationViewModels.Where(x => x.HasAnalysisPending));
        if (CodexEntryViewModels != null) pendingEntities.AddRange(CodexEntryViewModels.Where(x => x.HasAnalysisPending));

        if (pendingEntities.Count > 0)
        {
            var random = new Random();
            var selected = pendingEntities[random.Next(pendingEntities.Count)];
            OpenEditorWindow(selected);
        }
        else
        {
            MessageBox.Show("No notes currently require further analysis!", "All Caught Up", MessageBoxButton.OK, MessageBoxImage.Information);
        }
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

    [RelayCommand]
    public void BackupToJSON()
    {
        if (!IsProjectLoaded) return;

        try
        {
            var dialog = new SaveFileDialog
            {
                Title = "Backup Project to JSON",
                Filter = "JSON Files (*.json)|*.json",
                FileName = $"Backup_{DateTime.Now:yyyyMMdd_HHmm}.json"
            };

            if (dialog.ShowDialog() == true)
            {
                string json = _storyService.GetFullProjectJson();
                File.WriteAllText(dialog.FileName, json);
                MessageBox.Show("Backup saved successfully!", "Success");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error backing up: {ex.Message}", "Error");
        }
    }

    [RelayCommand]
    public async Task RestoreFromJSON()
    {
        if (!IsProjectLoaded) return;

        var dialog = new OpenFileDialog
        {
            Title = "Restore Project from JSON",
            Filter = "JSON Files (*.json)|*.json"
        };

        if (dialog.ShowDialog() == true)
        {
            if (MessageBox.Show("This will overwrite your current database. Continue?", "Warning", 
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    string json = await File.ReadAllTextAsync(dialog.FileName);
                    await _storyService.RestoreProjectFromJsonAsync(json);
                    
                    // Force the UI update logic just in case
                    UpdateState(); 
                    
                    MessageBox.Show("Project restored successfully!", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error restoring: {ex.Message}", "Error");
                }
            }
        }
    }

    [RelayCommand]
    public void CopyAiContext()
    {
        if (!IsProjectLoaded) return;
        
        try
        {
            // Set true/false based on if you want the heavy text
            string json = _storyService.GetAiContextJson(includeVerbatim: false);
            Clipboard.SetText(json);
            MessageBox.Show("Story Context copied to Clipboard!", "AI Ready");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
    
    // Add inside MainViewModel class

    [RelayCommand]
    public void AddSourceMaterial()
    {
        // Create new with default values
        var newSource = new SourceMaterial 
        { 
            Name = "New Source", 
            Abbreviation = "NS", 
            ColorHex = "#808080" 
        };
    
        // Add directly to the Service collection (which the UI is bound to)
        _storyService.SourceMaterials.Add(newSource);
    }

    [RelayCommand]
    public void DeleteSourceMaterial(SourceMaterial source)
    {
        if (source == null) return;

        if (MessageBox.Show($"Are you sure you want to delete '{source.Name}'?", "Confirm Delete", 
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            _storyService.SourceMaterials.Remove(source);
        }
    }


    // --- Global Search Properties ---
    [ObservableProperty]
    private ICollectionView _searchResultsView;

    [ObservableProperty]
    private string _searchText = "";

    // --- Audit Properties ---
    [ObservableProperty]
    private ObservableCollection<AuditableItemViewModel> _auditableItems = new();

    

    [RelayCommand]
    public void RefreshAuditableItems()


    {
        // 1. Create a temporary standard List (does not notify UI)
        var tempItems = new List<AuditableItemViewModel>();
        foreach (var item in _storyService.GetAllAuditableTexts())
        {
            AuditableItemViewModel vm = null;
            if (item is Note note)
            {
                string ownerName = "Unassigned";
                if (note.Chapter != null)
                {
                    ownerName = "Chapter: " + note.Chapter.Title;
                }
                else if (note.Character != null)
                {
                    ownerName = "Character: " + note.Character.Name;
                }
                else if (note.Theme != null)
                {
                    ownerName = "Theme: " + note.Theme.Name;
                }
                else if (note.StoryThread != null)
                {
                    ownerName = "Story Thread: " + note.StoryThread.Name;
                }
                else if (note.CodexEntry != null)
                {
                    ownerName = "Codex Entry: " + note.CodexEntry.Title;
                }
                vm = new AuditableItemViewModel(
                    note,
                    ownerName,
                    note.Content,
                    "Note",
                    note.LastModified,
                    () => { note.LastModified = DateTime.UtcNow; },
                    () => {
                        if (note.Chapter != null)
                        {
                            OpenEditorWindow(ChapterViewModels.FirstOrDefault(c => c.Chapter.Id == note.ChapterId));

                        }
                        else if (note.Character != null)
                        {
                            OpenEditorWindow(CharacterViewModels.FirstOrDefault(c => c.Character.Id == note.CharacterId));
                        }
                        else if(note.Theme != null)
                        {
                            OpenEditorWindow(ThemeViewModels.FirstOrDefault(t => t.Theme.Id == note.ThemeId));
                        }
                        else if(note.StoryThread != null)
                        {
                            OpenEditorWindow(StoryThreadViewModels.FirstOrDefault(s => s.StoryThread.Id == note.StoryThreadId));
                        }
                        else if(note.CodexEntry != null)
                        {
                            OpenEditorWindow(CodexEntryViewModels.FirstOrDefault(c => c.CodexEntry.Id == note.CodexEntryId));
                        }
                    }
                );
            }
            else if (item is PlotPoint pp)
            {
                vm = new AuditableItemViewModel(
                    pp,
                    "Plot Point: " + pp.Title,
                    pp.Synopsis,
                    "PlotPoint Synopsis",
                    pp.LastModified,
                    () => { pp.LastModified = DateTime.UtcNow; },
                    () => { OpenEditorWindow(PlotPointViewModels.FirstOrDefault(p => p.Model.Id == pp.Id)); }
                );
            }
            else if (item is PlotPointThread ppt)
            {
                vm = new AuditableItemViewModel(
                    ppt,
                    "Thread: " + (ppt.StoryThread?.Name ?? "Unknown Thread") + " | Plot Point: " + (ppt.PlotPoint?.Title ?? "Unknown Plot Point"),
                    ppt.ImpactDescription,
                    "Thread Impact",
                    ppt.LastModified,
                    () => { ppt.LastModified = DateTime.UtcNow; },
                    () => { OpenEditorWindow(PlotPointViewModels.FirstOrDefault(p => p.Model.Id == ppt.PlotPointId)); }
                );
            }
            else if (item is PlotPointTheme pptheme)
            {
                vm = new AuditableItemViewModel(
                    pptheme,
                    "Theme: " + (pptheme.Theme?.Name ?? "Unknown Theme") + " | Plot Point: " + (pptheme.PlotPoint?.Title ?? "Unknown Plot Point"),
                    pptheme.Commentary,
                    "Theme Commentary",
                    pptheme.LastModified,
                    () => { pptheme.LastModified = DateTime.UtcNow; },
                    () => { OpenEditorWindow(PlotPointViewModels.FirstOrDefault(p => p.Model.Id == pptheme.PlotPointId)); }
                );
            }
            else if (item is PlotPointCodexEntry ppce)
            {
                vm = new AuditableItemViewModel(
                    ppce,
                    "Codex Entry: " + (ppce.CodexEntry?.Title ?? "Unknown Codex Entry") + " | Plot Point: " + (ppce.PlotPoint?.Title ?? "Unknown Plot Point"),
                    ppce.Commentary,
                    "Codex Commentary",
                    ppce.LastModified,
                    () => { ppce.LastModified = DateTime.UtcNow; },
                    () => { OpenEditorWindow(PlotPointViewModels.FirstOrDefault(p => p.Model.Id == ppce.PlotPointId)); }
                );
            }
            else if (item is PlotPointCharacter ppc)
            {
                vm = new AuditableItemViewModel(
                    ppc,
                    "Character: " + (ppc.Character?.Name ?? "Unknown Character") + " | Plot Point: " + (ppc.PlotPoint?.Title ?? "Unknown Plot Point"),
                    ppc.DevelopmentNote,
                    "Character Dev Note",
                    ppc.LastModified,
                    () => { ppc.LastModified = DateTime.UtcNow; },
                    () => { OpenEditorWindow(PlotPointViewModels.FirstOrDefault(p => p.Model.Id == ppc.PlotPointId)); }
                );
            }
            
            if (vm != null && !string.IsNullOrWhiteSpace(vm.Text))
            {
                tempItems.Add(vm);
            }
        }

        // 3. Sort by date, take only the oldest 200, and execute the query
        var cappedAndSorted = tempItems
            .OrderBy(x => x.LastModified)
            .Take(200)
            .ToList();

        // 4. Finally, update the actual UI collection
        AuditableItems.Clear();
        foreach (var sortedVm in cappedAndSorted)
        {
            AuditableItems.Add(sortedVm);
        }
    }


    // Automatically called by the CommunityToolkit when SearchText changes
    partial void OnSearchTextChanged(string value)
    {
        SearchResultsView?.Refresh();
    }

    private bool FilterSearchResults(object obj)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
            return true; // Show all if search is empty

        if (obj is EntityViewModel entity)
        {
            string textToSearch = string.Empty;

            // Extract the searchable name/title
            if (entity is CharacterViewModel c) textToSearch = c.Name;
            else if (entity is ThemeViewModel t) textToSearch = t.Name;
            else if (entity is StoryThreadViewModel s) textToSearch = s.Name;
            else if (entity is ChapterViewModel ch) textToSearch = ch.Title;
            else if (entity is CodexEntryViewModel ce) textToSearch = ce.Title;
            else if (entity is LocationViewModel l) textToSearch = l.Name;

            if (string.IsNullOrEmpty(textToSearch)) return false;

            // Case-insensitive search
            return textToSearch.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    [RelayCommand]
    public void OpenAuditWindow()
    {
        // Use a specific key for the singleton window
        string key = "AuditWindow";

        // Checks if the window is already open and brings it to the front if it is
        if (ActivateIfOpen(key)) return;

        var win = new AuditWindow { DataContext = this }; // 'this' is the MainViewModel

        // Registers the window so MainViewModel knows to track it
        RegisterWindow(key, win);
        win.Show();

        // Optional: Automatically fetch the items when the window opens
        RefreshAuditableItems();
    }

    // 1. The Collection the DataGrid binds to
    public ObservableCollection<NotePropertyStats> DataGridStats { get; } = new();

// 2. The Command triggered by the Refresh button
    [RelayCommand]
    public void RefreshStatistics()
    {
        DataGridStats.Clear();

        // Row 3: Total Notes in DB
        DataGridStats.Add(_storyService.GetNoteStatsByCondition(
            "Total Notes", 
            n => true));
        
        // Row 1: Needs Analysis
        DataGridStats.Add(_storyService.GetNoteStatsByCondition(
            "Needs Analysis", 
            n => n.NeedsFurtherAnalysis));

        // Row 2: Incorporated into Draft
        DataGridStats.Add(_storyService.GetNoteStatsByCondition(
            "Incorporated", 
            n => n.IsIncorporated));
    }

    // Inside MainViewModel.cs
    public void DeleteNoteFromDatabase(Note note)
    {
        _storyService.DeleteNote(note);
    }

    [RelayCommand]
    public async Task PurgeUnassignedNotes()
    {
        var result = MessageBox.Show(
            "Are you sure you want to permanently delete all unassigned/orphaned notes?",
            "Confirm Purge",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            await _storyService.PurgeUnassignedNotesAsync();
            MessageBox.Show("Unassigned notes purged successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}