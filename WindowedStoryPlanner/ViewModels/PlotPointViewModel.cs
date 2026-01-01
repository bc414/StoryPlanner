using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner.ViewModels;

public partial class PlotPointViewModel : EntityViewModel
{
    public PlotPoint Model { get; }

    // --- Sub-ViewModels (Wrappers for Complex Lists) ---
    // We wrap the threads so we can display the Thread Name + Trajectory easily
    public ObservableCollection<StoryThreadViewModel> StoryThreads { get; } = new();
    public ObservableCollection<CharacterViewModel> Characters { get; } = new();
    public ObservableCollection<CodexEntryViewModel> CodexEntries { get; } = new();
    public ObservableCollection<ThemeViewModel> Themes { get; } = new();
    public ObservableCollection<ChapterViewModel> Chapters { get; } = new();
    public ObservableCollection<LocationViewModel> Locations { get; } = new();
    
    public ObservableCollection<ChapterViewModel> AllChapters => MainViewModel.Instance.ChapterViewModels;

    public PlotPointViewModel(PlotPoint model)
    {
        Model = model;

        // 1. Hydrate the Thread wrappers
        if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
        {

        }

        // 2. Listen for changes (Optional: If you add threads dynamically, sync the collections)
        // For a prototype, you can just manually add to both collections in your 'AddThread' command.
    }

    //Do this after the others are loaded
    public void PopulateAssociatedViewModelCollections()
    {
        StoryThreads.Clear();
        foreach (PlotPointThread threadAssignment in Model.ThreadAssignments)
        {
            StoryThreads.Add(MainViewModel.Instance.StoryThreadDictionary[threadAssignment.StoryThread]);
        }
        
        Characters.Clear();
        foreach (PlotPointCharacter characterJunction in Model.CharacterAppearances)
        {
            Characters.Add(MainViewModel.Instance.CharacterDictionary[characterJunction.Character]);
        }
        
        CodexEntries.Clear();
        foreach (PlotPointCodexEntry plotPointCodexEntry in Model.CodexReferences)
        {
            CodexEntries.Add(MainViewModel.Instance.CodexEntryDictionary[plotPointCodexEntry.CodexEntry]);
        }
        
        Themes.Clear();
        foreach (PlotPointTheme themeAssignment in Model.ThemeAssignments)
        {
            Themes.Add(MainViewModel.Instance.ThemeDictionary[themeAssignment.Theme]);
        }
        
        Chapters.Clear();
        if (Model.Chapter != null)
        {
            Chapters.Add(MainViewModel.Instance.ChapterDictionary[Model.Chapter]);
        }
        
        Locations.Clear();
        if (Model.Location != null)
        {
            Locations.Add(MainViewModel.Instance.LocationDictionary[Model.Location]);
        }
    }

    // --- Wrapper Properties ---

    public string Title
    {
        get => Model.Title;
        set => SetProperty(Model.Title, value, Model, (u, n) => u.Title = n);
    }
    
    public int OrderInChapter
    {
        get => Model.OrderInChapter;
        set
        {
            if (SetProperty(Model.OrderInChapter, value, Model, (u, n) => u.OrderInChapter = n))
            {
                RefreshFullOrder(); // <--- Update immediately
            }
        }
    }
    
    public string FullOrder => Model.Chapter == null 
        ? "? " 
        : $"{Model.Chapter.OrderIndex}.{Model.OrderInChapter + 1} ";
    
    public void RefreshFullOrder()
    {
        OnPropertyChanged(nameof(FullOrder));
    }

    public string Stakes
    {
        get => Model.Stakes;
        set => SetProperty(Model.Stakes, value, Model, (u, n) => u.Stakes = n);
    }

    public string Synopsis
    {
        get => Model.Synopsis;
        set => SetProperty(Model.Synopsis, value, Model, (u, n) => u.Synopsis = n);
    }

    public string Outcome
    {
        get => Model.Outcome;
        set => SetProperty(Model.Outcome, value, Model, (u, n) => u.Outcome = n);
    }

    public string? VerbatimText
    {
        get => Model.VerbatimText;
        set => SetProperty(Model.VerbatimText, value, Model, (u, n) => u.VerbatimText = n);
    }

    public DraftStatus Status
    {
        get => Model.Status;
        set => SetProperty(Model.Status, value, Model, (u, n) => u.Status = n);
    }

    // --- 5 Axes Wrappers ---

    public CoreDriver CoreDriver
    {
        get => Model.CoreDriver;
        set => SetProperty(Model.CoreDriver, value, Model, (u, n) => u.CoreDriver = n);
    }

    public TensionPhase TensionPhase
    {
        get => Model.TensionPhase;
        set => SetProperty(Model.TensionPhase, value, Model, (u, n) => u.TensionPhase = n);
    }

    public ConflictType ConflictType
    {
        get => Model.ConflictType;
        set => SetProperty(Model.ConflictType, value, Model, (u, n) => u.ConflictType = n);
    }

    public Presentation Presentation
    {
        get => Model.Presentation;
        set => SetProperty(Model.Presentation, value, Model, (u, n) => u.Presentation = n);
    }

    
    
    [RelayCommand]
    private void ViewLocationPayload(LocationViewModel vm)
    {
        // Location is a single assignment, so payload is just the Location itself or null
        // But for consistency with others, we might not have a dedicated payload window for simple assignment.
        // However, we can still provide Unlink.
        
        // Since Location is 1:1, we don't have a "Payload Object" like "PlotPointTheme".
        // We just open the Location details? Or maybe nothing?
        // User asked for "View/Edit Connection Details". 
        // For simple properties, maybe just select it?
        
        // Let's just allow Unlink for now or open details.
        
        if (MessageBox.Show($"Unlink Location '{vm.Location.Name}'?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            UnlinkLocation(vm);
        }
    }

    [RelayCommand]
    private void ViewChapterPayload(ChapterViewModel vm)
    {
        if (MessageBox.Show($"Unlink Chapter '{vm.Chapter.Title}'?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            UnlinkChapter(vm);
        }
    }
    /*
    [RelayCommand]
    private void ViewThemePayload(ThemeViewModel vm)
    {
        OpenPayloadWindow<ThemeViewModel, PlotPointThemeView>(
            vm,
            // Payload Selector
            (v) => v.Theme.PlotPointAssignments.FirstOrDefault(x => x.PlotPoint == Model),
            // Unlink Action
            () => UnlinkTheme(vm) 
        );
    }

    [RelayCommand]
    private void ViewCharacterPayload(CharacterViewModel vm)
    {
        OpenPayloadWindow<CharacterViewModel, PlotPointCharacterView>(
            vm,
            (v) => v.Character.Appearances.FirstOrDefault(x => x.PlotPoint == Model), 
            () => UnlinkCharacter(vm)
        );
    }

    [RelayCommand]
    private void ViewCodexPayload(CodexEntryViewModel vm)
    {
        OpenPayloadWindow<CodexEntryViewModel, PlotPointCodexEntryView>(
            vm,
            (v) => v.CodexEntry.PlotPointReferences.FirstOrDefault(x => x.PlotPoint == Model),
            () => UnlinkCodexEntry(vm)
        );
    }

    [RelayCommand]
    public void ViewThreadPayload(StoryThreadViewModel vm)
    {
        OpenPayloadWindow<StoryThreadViewModel, PlotPointThreadView>(
            vm,
            (v) => v.StoryThread.PlotPointAssignments.FirstOrDefault(x => x.PlotPoint == Model),
            () => UnlinkThread(vm)
        );
    }
    
    private void OpenPayloadWindow<TViewModel, TWindow>(
        TViewModel itemViewModel,
        Func<TViewModel, object> getPayload, // Returns object (DataContext)
        Action unlinkAction)                 // Just a simple action callback
        where TWindow : Window, IPayloadWindow, new() 
    {
        var payload = getPayload(itemViewModel);

        if (payload != null)
        {
            TWindow window = new TWindow();
            window.DataContext = payload;

            window.UnlinkCommand = new RelayCommand(() =>
            {
                // 1. Run the specific Unlink logic (passed in from caller)
                unlinkAction();

                // 2. Close the window
                window.Close(); 
            });

            window.Show();
        }
    }
    */
    

    
    [RelayCommand]
    public void UnlinkTheme(ThemeViewModel themeViewModel)
    {
        // Find the junction object (PlotPointTheme)
        PlotPointTheme payload = themeViewModel.Theme.PlotPointAssignments
            .FirstOrDefault(pp => pp.PlotPoint == Model);

        if (payload != null)
        {
            // 1. Remove from Plot Point Model
            Model.ThemeAssignments.Remove(payload);

            // 2. Remove from Theme Model
            themeViewModel.Theme.PlotPointAssignments.Remove(payload);

            // 3. Remove from PlotPointViewModel's ThemeViewModel Collection
            Themes.Remove(themeViewModel);

            // 4. Remove this PlotPoint from the Theme Window's list
            themeViewModel.PlotPointCollectionViewModel?.ViewModelCollection.Remove(this);
        }
    }
    
    [RelayCommand]
    public void UnlinkCharacter(CharacterViewModel characterViewModel)
    {
        // Find the junction object (PlotPointCharacter)
        PlotPointCharacter payload = characterViewModel.Character.Appearances
            .FirstOrDefault(pp => pp.PlotPoint == Model);

        if (payload != null)
        {
            // 1. Remove from Plot Point Model
            Model.CharacterAppearances.Remove(payload);

            // 2. Remove from Character Model
            characterViewModel.Character.Appearances.Remove(payload);

            // 3. Remove from PlotPointViewModel's CharacterViewModel Collection
            Characters.Remove(characterViewModel);

            // 4. Remove this PlotPoint from the Character Window's list
            characterViewModel.PlotPointCollectionViewModel?.ViewModelCollection.Remove(this);
        }
    }
    
    [RelayCommand]
    public void UnlinkCodexEntry(CodexEntryViewModel codexEntryViewModel)
    {
        // Find the junction object (PlotPointCodexEntry)
        PlotPointCodexEntry payload = codexEntryViewModel.CodexEntry.PlotPointReferences
            .FirstOrDefault(pp => pp.PlotPoint == Model);

        if (payload != null)
        {
            // 1. Remove from Plot Point Model
            Model.CodexReferences.Remove(payload);

            // 2. Remove from Codex Entry Model
            codexEntryViewModel.CodexEntry.PlotPointReferences.Remove(payload);

            // 3. Remove from PlotPointViewModel's CodexEntryViewModel Collection
            CodexEntries.Remove(codexEntryViewModel);

            // 4. Remove this PlotPoint from the Codex Window's list
            codexEntryViewModel.PlotPointCollectionViewModel?.ViewModelCollection.Remove(this);
        }
    }
    
    [RelayCommand]
    public void UnlinkThread(StoryThreadViewModel StoryThreadViewModel)
    {
        PlotPointThread payload = StoryThreadViewModel.StoryThread.PlotPointAssignments.Where(pp => pp.PlotPoint == Model).FirstOrDefault();
        if(payload != null)
        {
            //1. Remove from Plot Point Model
            Model.ThreadAssignments.Remove(payload);

            //2. Remove from Story Thread Model
            StoryThreadViewModel.StoryThread.PlotPointAssignments.Remove(payload);

            //3. Remove from PlotPointViewModel's StoryThreadViewModel Collection
            StoryThreads.Remove(StoryThreadViewModel);

            //4. Remove from StoryThreadViewModel's PlotPointViewModel Collection
            StoryThreadViewModel.PlotPointCollectionViewModel.ViewModelCollection.Remove(this);
        }
    }
    
    public void UnlinkLocation(LocationViewModel locationViewModel)
    {
        if (Model.Location == locationViewModel.Location)
        {
            Model.Location = null;
            Locations.Clear();
            locationViewModel.Location.PlotPoints.Remove(Model);
            locationViewModel.PlotPointCollectionViewModel.ViewModelCollection.Remove(this);
        }
    }

    public void UnlinkChapter(ChapterViewModel chapterViewModel)
    {
        if (Model.Chapter == chapterViewModel.Chapter)
        {
            Model.Chapter = null;
            Chapters.Clear();
            chapterViewModel.Chapter.PlotPoints.Remove(Model);
            chapterViewModel.PlotPointCollectionViewModel.ViewModelCollection.Remove(this);
            RefreshFullOrder();
        }
    }

    [RelayCommand]
    public void UnlinkEntity(object o)
    {
        if (o == null) return;
        
        var entity = o as EntityViewModel;
        if (entity == null) return;

        switch (entity)
        {
            case StoryThreadViewModel threadVM:
                UnlinkThread(threadVM);
                break;

            case CharacterViewModel charVM:
                UnlinkCharacter(charVM);
                break;

            case ThemeViewModel themeVM:
                UnlinkTheme(themeVM);
                break;

            case CodexEntryViewModel codexVM:
                UnlinkCodexEntry(codexVM);
                break;

            case ChapterViewModel chapterVM:
                UnlinkChapter(chapterVM);
                break;

            case LocationViewModel locVM:
                UnlinkLocation(locVM);
                break;

            default:
                // Optional: Log warning or ignore
                MessageBox.Show("UnlinkEntity: Unsupported entity type.");
                break;
        }
    }

    public bool IsLinkedTo(EntityViewModel other)
    {
        return other switch
        {
            CharacterViewModel c => Model.CharacterAppearances.Any(x => x.Character == c.Character),
            ThemeViewModel t => Model.ThemeAssignments.Any(x => x.Theme == t.Theme),
            StoryThreadViewModel s => Model.ThreadAssignments.Any(x => x.StoryThread == s.StoryThread),
            CodexEntryViewModel e => Model.CodexReferences.Any(x => x.CodexEntry == e.CodexEntry),
            
            // 1:1 Relationships (Check if IDs match or objects match)
            ChapterViewModel ch => Model.ChapterId == ch.Chapter.Id, 
            LocationViewModel l => Model.LocationId == l.Location.Id,
            
            _ => false
        };
    }
    
    public void LinkCharacter(CharacterViewModel characterViewModel)
    {
        if (IsLinkedTo(characterViewModel)) return; // <-- Refactored check

        PlotPointCharacter newPayload = new PlotPointCharacter()
        {
            PlotPoint = Model,
            PlotPointId = Model.Id,
            Character = characterViewModel.Character,
            CharacterId = characterViewModel.Character.Id,
        };
        Model.CharacterAppearances.Add(newPayload);
        characterViewModel.Character.Appearances.Add(newPayload);
        Characters.Add(characterViewModel);
        
        //ViewCharacterPayload(characterViewModel);
    }
    
    public void LinkThread(StoryThreadViewModel threadViewModel)
    {
        if (IsLinkedTo(threadViewModel)) return;

        PlotPointThread newPayload = new PlotPointThread()
        {
            PlotPoint = Model,
            PlotPointId = Model.Id,
            StoryThread = threadViewModel.StoryThread,
            ThreadId = threadViewModel.StoryThread.Id,
            SortOrder = threadViewModel.StoryThread.PlotPointAssignments.Count
        };

        Model.ThreadAssignments.Add(newPayload);
        threadViewModel.StoryThread.PlotPointAssignments.Add(newPayload);
        StoryThreads.Add(threadViewModel);

        //ViewThreadPayload(threadViewModel);
    }

    public void LinkTheme(ThemeViewModel themeViewModel)
    {
        if (IsLinkedTo(themeViewModel)) return;

        PlotPointTheme newPayload = new PlotPointTheme()
        {
            PlotPoint = Model,
            PlotPointId = Model.Id,
            Theme = themeViewModel.Theme,
            ThemeId = themeViewModel.Theme.Id,
        };

        Model.ThemeAssignments.Add(newPayload);
        themeViewModel.Theme.PlotPointAssignments.Add(newPayload);
        Themes.Add(themeViewModel);

        //ViewThemePayload(themeViewModel);
    }

    public void LinkCodexEntry(CodexEntryViewModel codexViewModel)
    {
        if (IsLinkedTo(codexViewModel)) return;

        PlotPointCodexEntry newPayload = new PlotPointCodexEntry()
        {
            PlotPoint = Model,
            PlotPointId = Model.Id,
            CodexEntry = codexViewModel.CodexEntry,
            CodexEntryId = codexViewModel.CodexEntry.Id,
        };

        Model.CodexReferences.Add(newPayload);
        codexViewModel.CodexEntry.PlotPointReferences.Add(newPayload);
        CodexEntries.Add(codexViewModel);

        //ViewCodexPayload(codexViewModel);
    }

    public void LinkLocation(LocationViewModel locationViewModel)
    {
        if (IsLinkedTo(locationViewModel)) return; // 1:1 Logic is slightly different, but checking equality works

        Model.Location = locationViewModel.Location;
        Model.LocationId = locationViewModel.Location.Id;

        locationViewModel.Location.PlotPoints.Add(Model);
        
        Locations.Clear();
        Locations.Add(locationViewModel);
        
        locationViewModel.PlotPointCollectionViewModel.ViewModelCollection.Add(this);
    }

    public void LinkChapter(ChapterViewModel chapterViewModel)
    {
        // 1. Safety Checks
        if (chapterViewModel == null) return;
        if (IsLinkedTo(chapterViewModel)) return;

        // 2. Unlink from previous (The "Move" Logic)
        if (Model.Chapter != null)
        {
            // Use FirstOrDefault() to prevent crashes if the list is empty for some reason
            var oldChapterVM = Chapters.FirstOrDefault();

            // If we have the VM loaded, clean it up properly via the method
            if (oldChapterVM != null)
            {
                UnlinkChapter(oldChapterVM);
            }
            else
            {
                // Fallback: If we don't have the VM loaded but the Model has a chapter, 
                // we must manually clear the model reference to avoid database constraint errors
                Model.Chapter = null;
            }
        }

        // 3. Link New
        Model.Chapter = chapterViewModel.Chapter;
        Model.ChapterId = chapterViewModel.Chapter.Id;

        // Update the Target Window (Chapter Window)
        chapterViewModel.Chapter.PlotPoints.Add(Model);
        chapterViewModel.PlotPointCollectionViewModel.ViewModelCollection.Add(this);

        // Update This Card's internal list
        Chapters.Clear();
        Chapters.Add(chapterViewModel);

        // Ensure persistence of order
        Model.OrderInChapter = chapterViewModel.Chapter.PlotPoints.Count - 1;
        RefreshFullOrder();
    }

    [RelayCommand]
    public void MoveToChapter(ChapterViewModel targetChapter)
    {
        // All the complexity is now handled centrally in LinkChapter
        LinkChapter(targetChapter);
    }

    // --- UI State (Not in Model) ---

    // Example: Used for Drag & Drop visuals
    [ObservableProperty]
    private bool _isBeingDragged;
    
    public ObservableCollection<ThemeBadge> ThemeBadges 
    {
        get
        {
            var badges = new ObservableCollection<ThemeBadge>();
            foreach (var assign in Model.ThemeAssignments)
            {
                // 1. Get Color
                var color = (Color)ColorConverter.ConvertFromString(
                    !string.IsNullOrEmpty(assign.Theme.ColorHex) ? assign.Theme.ColorHex : "#CCCCCC");
            
                var brush = new SolidColorBrush(color);

                // 2. Get Text (Fallback to first 2 letters if Abbreviation is empty)
                string text = !string.IsNullOrEmpty(assign.Theme.Abbreviation) 
                    ? assign.Theme.Abbreviation 
                    : assign.Theme.Name.Substring(0, Math.Min(2, assign.Theme.Name.Length)).ToUpper();

                badges.Add(new ThemeBadge 
                { 
                    Text = text, 
                    Background = brush, 
                    // Simple logic: If background is dark, use white text. If light, use black.
                    Foreground = IsDark(color) ? Brushes.White : Brushes.Black 
                });
            }
            return badges;
        }
    }

    // Helper to determine contrast
    private bool IsDark(Color c) => (c.R * 0.299 + c.G * 0.587 + c.B * 0.114) < 186;
    
    public string ThreadIcons
    {
        get
        {
            // Returns a string of icons like "⚔️💍"
            var icons = Model.ThreadAssignments
                .Select(t => t.StoryThread.Icon)
                .Where(icon => !string.IsNullOrEmpty(icon));
            
            return string.Join(" ", icons);
        }
    }

    // Example: Used to validate "Mandatory Fields" based on status
    public bool IsOutcomeMissing => Status >= DraftStatus.Planned && string.IsNullOrWhiteSpace(Outcome);

    // --- Commands ---

    [RelayCommand]
    private void AddThread(StoryThread thread)
    {
        // 1. Create Data Link
        var newLink = new PlotPointThread 
        { 
            PlotPoint = Model, 
            StoryThread = thread,
            ThreadId = thread.Id,
            ThreadTrajectory = GoalTrajectory.Unset 
        };

        // 2. Update Model
        Model.ThreadAssignments.Add(newLink);

        // 3. Update View
        //Threads.Add(new PlotPointThreadViewModel(newLink));
    }
}

public struct ThemeBadge
{
    public string Text { get; set; }
    public Brush Background { get; set; }
    public Brush Foreground { get; set; } // For contrast
}