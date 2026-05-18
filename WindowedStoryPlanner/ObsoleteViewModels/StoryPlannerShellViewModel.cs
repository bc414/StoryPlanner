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

public partial class StoryPlannerShellViewModel : ObservableObject
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

    public FileManagerViewModel FileManager { get; }

    public StoryPlannerShellViewModel(IStoryService storyService)
    {
        _storyService = storyService;

        FileManager.ProjectLoaded += UpdateState; // shell reacts to project load

        UpdateState();
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

            

            // Notes
            foreach(Note note in _storyService.Notes)
            {
                //AllNoteViewModels.Add(new NoteViewModel(note, _storyService, t));
            }

            // NarrativePropertyValues (raw collection)
            AllNarrativePropertyValues = _storyService.NarrativePropertyValues;

            // NarrativePropertyValueDefinitions (ViewModel collection)
            foreach(NarrativePropertyValueDefinition value in _storyService.NarrativePropertyValueDefinitions)
            {
                AllNarrativePropertyValueDefinitions.Add(new NarrativePropertyValueViewModel(value));
            }

            
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

    // Implement the interface member, backed by DefinitionsEditor:
    public IReadOnlyList<SubjectDefinitionViewModel> AllSubjectDefinitionViewModels =>
        DefinitionsEditor.SubjectDefinitions ?? [];
}