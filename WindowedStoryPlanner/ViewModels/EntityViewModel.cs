using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core.Models; // Added for Note
using System.Collections;
using System.Collections.Specialized; // Added for INotifyCollectionChanged
using System.ComponentModel; // Added for PropertyChanged
using System.Linq; // Added for LINQ
using System.Windows;
using WindowedStoryPlanner.Views;

namespace WindowedStoryPlanner.ViewModels;

public partial class EntityViewModel : ObservableObject, IDropTarget
{
    protected readonly IEditorCoordinator _editorCoordinator;

    // Changed to backing field to hook events when set
    private NoteCollectionViewModel _noteCollectionViewModel;

    public NoteCollectionViewModel NoteCollectionViewModel
    {
        get => _noteCollectionViewModel;
        set
        {
            if (SetProperty(ref _noteCollectionViewModel, value))
            {
                SubscribeToNotes();
                UpdateStats();
            }
        }
    }

    public int CharacterCount {
        get
        {
            return NoteCollectionViewModel.CharacterCount;
        }
    }


    public PlotPointCollectionViewModel PlotPointCollectionViewModel { get; set; }

    public virtual bool IsLinkingMode => !NoteCollectionViewModel.IsNoteReorderMode;

    // --- Computed Properties for UI ---

    [ObservableProperty]
    private bool _hasAnalysisPending;

    [ObservableProperty]
    private string _progressStats = ""; // The "X/Y" text

    public EntityViewModel(IEditorCoordinator editorCoordinator)
    {
        _editorCoordinator = editorCoordinator;
    }

    // --- Event Subscription Logic ---

    private void SubscribeToNotes()
    {
        if (_noteCollectionViewModel?.NoteCollection != null)
        {
            _noteCollectionViewModel.NoteCollection.CollectionChanged += OnNoteCollectionChanged;

            // Subscribe to existing notes
            foreach (var note in _noteCollectionViewModel.NoteCollection)
            {
                note.PropertyChanged += OnNotePropertyChanged;
            }
        }
    }

    private void OnNoteCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (Note note in e.NewItems)
                note.PropertyChanged += OnNotePropertyChanged;
        }

        if (e.OldItems != null)
        {
            foreach (Note note in e.OldItems)
                note.PropertyChanged -= OnNotePropertyChanged;
        }

        UpdateStats();
    }

    private void OnNotePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Update stats if relevant properties change
        if (e.PropertyName == nameof(Note.NeedsFurtherAnalysis) ||
            e.PropertyName == "IsIncorporated") // Matching your "IsIncorporated" naming
        {
            UpdateStats();
        }
    }

    private void UpdateStats()
    {
        if (_noteCollectionViewModel?.NoteCollection == null) return;

        var notes = _noteCollectionViewModel.NoteCollection;

        // 1. Rainbow Gradient Logic
        HasAnalysisPending = notes.Any(n => n.NeedsFurtherAnalysis);

        // 2. X/Y Counter Logic
        int total = notes.Count;
        if (total > 0)
        {
            // Assuming property is 'IsIncorporated' (or IsIncluded based on your model)
            int incorporated = notes.Count(n => n.IsIncorporated);
            ProgressStats = $"{incorporated}/{total}";
        }
        else
        {
            ProgressStats = "";
        }
    }

    // --- Navigation & Drag/Drop (Unchanged) ---

    [RelayCommand]
    public void OpenWindow()
    {
        _editorCoordinator.OpenEditorWindow(this);
    }

    [RelayCommand]
    public void Navigate(object parameter)
    {
        OpenWindow();
        if (parameter is Window sourceWindow)
        {
            if (sourceWindow is MainWindow) return;
            if (sourceWindow is FloatingPlotPointsWindow) return;
            sourceWindow.Close();
        }
    }

    public virtual void DragOver(IDropInfo dropInfo)
    {
        object source = dropInfo.Data;
        var target = this;

        PlotPointViewModelOld? plotPoint = source as PlotPointViewModelOld ?? target as PlotPointViewModelOld;
        EntityViewModel? otherEntity = (source == plotPoint) ? target as EntityViewModel : source as EntityViewModel;

        bool isTypeCompatible = (source, target) switch
        {
            (CharacterViewModel, PlotPointViewModelOld) => true,
            (PlotPointViewModelOld, CharacterViewModel) => true,
            (ThemeViewModel, PlotPointViewModelOld) => true,
            (PlotPointViewModelOld, ThemeViewModel) => true,
            (StoryThreadViewModel, PlotPointViewModelOld) => true,
            (PlotPointViewModelOld, StoryThreadViewModel) => true,
            (CodexEntryViewModel, PlotPointViewModelOld) => true,
            (PlotPointViewModelOld, CodexEntryViewModel) => true,
            (ChapterViewModel, PlotPointViewModelOld) => true,
            (PlotPointViewModelOld, ChapterViewModel) => true,
            _ => false
        };

        if (isTypeCompatible && plotPoint != null && otherEntity != null)
        {
            if (!plotPoint.IsLinkedTo(otherEntity))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
            else
            {
                dropInfo.Effects = DragDropEffects.None;
            }
        }
    }

    public virtual void Drop(IDropInfo dropInfo)
    {
        var source = dropInfo.Data;
        var target = this;

        switch (source, target)
        {
            case (CharacterViewModel c, PlotPointViewModelOld p): p.LinkCharacter(c); break;
            case (PlotPointViewModelOld p, CharacterViewModel c): p.LinkCharacter(c); break;
            case (ThemeViewModel t, PlotPointViewModelOld p): p.LinkTheme(t); break;
            case (PlotPointViewModelOld p, ThemeViewModel t): p.LinkTheme(t); break;
            case (StoryThreadViewModel s, PlotPointViewModelOld p): p.LinkThread(s); break;
            case (PlotPointViewModelOld p, StoryThreadViewModel s): p.LinkThread(s); break;
            case (CodexEntryViewModel e, PlotPointViewModelOld p): p.LinkCodexEntry(e); break;
            case (PlotPointViewModelOld p, CodexEntryViewModel e): p.LinkCodexEntry(e); break;
            case (ChapterViewModel ch, PlotPointViewModelOld p): p.LinkChapter(ch); break;
            case (PlotPointViewModelOld p, ChapterViewModel ch): p.LinkChapter(ch); break;
        }
    }
}