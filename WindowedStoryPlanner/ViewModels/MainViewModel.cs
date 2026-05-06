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

public partial class MainViewModel : ObservableObject, IEditorCoordinator, IViewModelRegistry
{
    private readonly IStoryService _storyService;

    [ObservableProperty]
    private DefinitionsEditorViewModel _definitionsEditor;

    public ObservableCollection<SubjectViewModel> AllSubjectViewModels { get; set; } = new ObservableCollection<SubjectViewModel>();

    public ObservableCollection<PlotPointViewModel> AllPlotPointViewModels { get; set; } = new ObservableCollection<PlotPointViewModel>();

    public ObservableCollection<PlotPointSubjectLinkViewModel> AllPlotPointSubjectLinkViewModels { get; set; } = new ObservableCollection<PlotPointSubjectLinkViewModel>();

    public ObservableCollection<ChapterViewModel> AllChapterViewModels { get; set; } = new ObservableCollection<ChapterViewModel>();
    public ObservableCollection<NoteTrackDefinitionViewModel> AllNoteTrackDefinitionViewModels { get; set; } = new ObservableCollection<NoteTrackDefinitionViewModel>();
    public ObservableCollection<NoteViewModel> AllNoteViewModels { get; set; } = new ObservableCollection<NoteViewModel>();

    public ObservableCollection<NarrativePropertyValue> AllNarrativePropertyValues
    {
        get; private set;
    }

    public ObservableCollection<NarrativePropertyValueViewModel> AllNarrativePropertyValueDefinitions { get; set; } = new ObservableCollection<NarrativePropertyValueViewModel>();


    [ObservableProperty] private bool _isProjectLoaded;
    [ObservableProperty] private string _windowTitle = "Story Planner";
    
    
    public ObservableCollection<SourceMaterial> SourceMaterials => _storyService.SourceMaterials;

    public ObservableCollection<GeminiEntry> GeminiEntries => _storyService.GeminiEntries;
    public ObservableCollection<Idea> Ideas => _storyService.Ideas;

    // Track open windows to prevent duplicates
    private Dictionary<object, Window> _openWindows = new();
    

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
    //public ObservableCollection<EntityViewModel> DashboardEntities { get; } = new();

    // 2. The Sorted View for the UI
    [ObservableProperty]
    private ICollectionView _dashboardEntitiesView;

    public MainViewModel(IStoryService storyService)
    {
        _storyService = storyService;
        UpdateState();
    }

    // --- IEditorCoordinator factory methods ---

    public async Task<NoteViewModel> CreateNoteAsync(int ownerId, int noteTrackDefinitionId, int sortOrder)
    {
        Note newNote = new Note
        {
            OwnerId = ownerId,
            NoteTrackDefinitionId = noteTrackDefinitionId,
            NoteState = NoteState.Unset,
            SortOrder = sortOrder,
            LastModified = DateTime.UtcNow
        };

        _storyService.Notes.Add(newNote);
        await _storyService.SaveAsync(); // populates newNote.Id

        var vm = new NoteViewModel(newNote, _storyService);
        AllNoteViewModels.Add(vm);
        return vm;
    }

    public async Task CreatePlotPointSubjectLinkAsync(PlotPointViewModel plotPoint, SubjectViewModel subject)
    {
        // Guard: link must not already exist
        if (AllPlotPointSubjectLinkViewModels.Any(l => l.PlotPointId == plotPoint.Id && l.SubjectId == subject.Id))
            return;

        var link = new PlotPointSubjectLink
        {
            PlotPointId = plotPoint.Id,
            SubjectId = subject.Id
        };

        _storyService.PlotPointsSubjectLinks.Add(link);
        await _storyService.SaveAsync(); // populates link.Id

        var vm = new PlotPointSubjectLinkViewModel(link, this, _storyService, this);
        AllPlotPointSubjectLinkViewModels.Add(vm);
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

            // Subjects
            foreach (Subject subject in _storyService.Subjects)
                AllSubjectViewModels.Add(new SubjectViewModel(subject, this, _storyService, this));

            foreach (PlotPoint plotPoint in _storyService.PlotPoints)
                AllPlotPointViewModels.Add(new PlotPointViewModel(plotPoint, this, _storyService, this));

            foreach (PlotPointSubjectLink link in _storyService.PlotPointsSubjectLinks)
                AllPlotPointSubjectLinkViewModels.Add(new PlotPointSubjectLinkViewModel(link, this, _storyService, this));

            foreach (Chapter chapter in _storyService.Chapters)
            {
                AllChapterViewModels.Add(new ChapterViewModel(chapter, this, _storyService, this));
            }

            

            // Notes
            foreach(Note note in _storyService.Notes)
            {
                AllNoteViewModels.Add(new NoteViewModel(note, _storyService));
            }

            // NarrativePropertyValues (raw collection)
            AllNarrativePropertyValues = _storyService.NarrativePropertyValues;

            // NarrativePropertyValueDefinitions (ViewModel collection)
            foreach(NarrativePropertyValueDefinition value in _storyService.NarrativePropertyValueDefinitions)
            {
                AllNarrativePropertyValueDefinitions.Add(new NarrativePropertyValueViewModel(value));
            }

            DefinitionsEditor = new DefinitionsEditorViewModel(_storyService);
            DefinitionsEditor.Initialize();

            RefreshDashboardList();

            // Notify UI to refresh bindings since the collections were replaced
            OnPropertyChanged(nameof(AllChapterViewModels));
            OnPropertyChanged(nameof(AllNarrativePropertyValueDefinitions));
            OnPropertyChanged(nameof(AllNarrativePropertyValues));
            OnPropertyChanged(nameof(GeminiEntries));
            OnPropertyChanged(nameof(Ideas));
            OnPropertyChanged(nameof(AllPlotPointViewModels));
            OnPropertyChanged(nameof(AllSubjectViewModels));
            OnPropertyChanged(nameof(AllPlotPointSubjectLinkViewModels));
            OnPropertyChanged(nameof(AllNoteViewModels));
            OnPropertyChanged(nameof(SourceMaterials));


            RefreshStatistics();
        }
        else
        {
            WindowTitle = "Story Planner - No Project Loaded";
        }
    }
    
    private void RefreshDashboardList()
    {
        /*DashboardEntities.Clear();

        

        // 2. Create View
        DashboardEntitiesView = CollectionViewSource.GetDefaultView(DashboardEntities);

        // 3. Apply Sort (CharacterCount Descending)
        DashboardEntitiesView.SortDescriptions.Clear();
        //DashboardEntitiesView.SortDescriptions.Add(new SortDescription(nameof(EntityViewModel.CharacterCount), ListSortDirection.Descending));

        SearchResultsView = new ListCollectionView(DashboardEntities);
        SearchResultsView.Filter = FilterSearchResults;*/
    }

    public async Task RegisterNewFloatingPlotPointAsync()
    {
        await CreatePlotPointAsync(null, 1);
    }

    public async Task<PlotPointViewModel> CreatePlotPointAsync(int? chapterId, int orderInChapter)
    {
        PlotPoint plotPoint = new PlotPoint
        {
            Title = "New Plot Point",
            ChapterId = chapterId,
            OrderInChapter = orderInChapter
        };

        _storyService.PlotPoints.Add(plotPoint);
        await _storyService.SaveAsync(); // populates plotPoint.Id

        var vm = new PlotPointViewModel(plotPoint, this, _storyService, this);
        AllPlotPointViewModels.Add(vm);
        return vm;
    }

    [RelayCommand]
    public async Task AddSubject()
    {
        Subject subject = new Subject();
        _storyService.Subjects.Add(subject);
        await _storyService.SaveAsync();
        SubjectViewModel viewModel = new SubjectViewModel(subject, this, _storyService, this);
        AllSubjectViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteSubject(SubjectViewModel viewModel)
    {
        //Subject character = viewModel.
        //if (MessageBox.Show($"Are you sure you want to delete character '{character.Name}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            //CharacterViewModels.Remove(viewModel);
            //CharacterDictionary.Remove(character);
            //_storyService.Characters.Remove(character);
        }
    }

    [RelayCommand]
    public async Task AddChapter()
    {
        Chapter newChapter = new Chapter();
        _storyService.Chapters.Add(newChapter);
        await _storyService.SaveAsync();
        ChapterViewModel viewModel = new ChapterViewModel(newChapter, this, _storyService, this);
        AllChapterViewModels.Add(viewModel);
        OpenEditorWindow(viewModel);
    }

    [RelayCommand]
    public void DeleteChapter(ChapterViewModel viewModel)
    {
        /*Chapter chapter = viewModel.Chapter;
        if (MessageBox.Show($"Are you sure you want to delete chapter '{chapter.Title}')?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            ChapterViewModels.Remove(viewModel);
            ChapterDictionary.Remove(chapter);
            _storyService.Chapters.Remove(chapter);
        }*/
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
        /*string key = "FloatingPoints"; 
        if (ActivateIfOpen(key)) return;

        var vm = new FloatingPlotPointsViewModel();
        var win = new FloatingPlotPointsWindow { DataContext = vm };
        
        RegisterWindow(key, win);
        win.Show();*/
    }
    
    public void OpenEditorWindow(NarrativeElementViewModel viewModel)
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
        //TODO: new windows


        return null;
    }

    [RelayCommand]
    public void OpenRandomAnalysisEntity()
    {
        //TODO: get flagged

        /*var pendingEntities = new List<EntityViewModel>();

        // Gather all entities that have at least one note requiring analysis
        if (CharacterViewModels != null) pendingEntities.AddRange(CharacterViewModels.Where(x => x.HasAnalysisPending));
        if (PlotPointViewModels != null) pendingEntities.AddRange(PlotPointViewModels.Where(x => x.HasAnalysisPending));
        if (ChapterViewModels != null) pendingEntities.AddRange(ChapterViewModels.Where(x => x.HasAnalysisPending));
        if (StoryThreadViewModels != null) pendingEntities.AddRange(StoryThreadViewModels.Where(x => x.HasAnalysisPending));
        if (ThemeViewModels != null) pendingEntities.AddRange(ThemeViewModels.Where(x => x.HasAnalysisPending));
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
        }*/
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

    public event Action<int> NoteViewModelMutated;

    [RelayCommand]
    public void RefreshAuditableItems()
    {
        //TODO: use flagged, rework the whole thing
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

        if (obj is NarrativeElementViewModel entity)
        {
            string textToSearch = string.Empty;

            // Extract the searchable name/title
            if (entity is SubjectViewModel s) textToSearch = s.Name;
            else if (entity is ChapterViewModel ch) textToSearch = ch.Title;
            else if (entity is PlotPointViewModel p) textToSearch = p.Title;

            if (string.IsNullOrEmpty(textToSearch)) return false;

            // Case-insensitive search
            return textToSearch.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    [RelayCommand]
    public void OpenAuditWindow()
    {
        //TODO rework
    }

    

    // 2. The Command triggered by the Refresh button
    [RelayCommand]
    public void RefreshStatistics()
    {
        //TODO
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

    public void RaiseNoteMutated(int noteId)
    {
        NoteViewModelMutated?.Invoke(noteId);
    }
}