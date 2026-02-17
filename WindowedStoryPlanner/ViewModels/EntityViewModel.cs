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

    public EntityViewModel()
    {

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
        MainViewModel.Instance.OpenEditorWindow(this);
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

        PlotPointViewModel? plotPoint = source as PlotPointViewModel ?? target as PlotPointViewModel;
        EntityViewModel? otherEntity = (source == plotPoint) ? target as EntityViewModel : source as EntityViewModel;

        bool isTypeCompatible = (source, target) switch
        {
            (CharacterViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, CharacterViewModel) => true,
            (ThemeViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, ThemeViewModel) => true,
            (StoryThreadViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, StoryThreadViewModel) => true,
            (CodexEntryViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, CodexEntryViewModel) => true,
            (ChapterViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, ChapterViewModel) => true,
            (LocationViewModel, PlotPointViewModel) => true,
            (PlotPointViewModel, LocationViewModel) => true,
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
            case (CharacterViewModel c, PlotPointViewModel p): p.LinkCharacter(c); break;
            case (PlotPointViewModel p, CharacterViewModel c): p.LinkCharacter(c); break;
            case (ThemeViewModel t, PlotPointViewModel p): p.LinkTheme(t); break;
            case (PlotPointViewModel p, ThemeViewModel t): p.LinkTheme(t); break;
            case (StoryThreadViewModel s, PlotPointViewModel p): p.LinkThread(s); break;
            case (PlotPointViewModel p, StoryThreadViewModel s): p.LinkThread(s); break;
            case (CodexEntryViewModel e, PlotPointViewModel p): p.LinkCodexEntry(e); break;
            case (PlotPointViewModel p, CodexEntryViewModel e): p.LinkCodexEntry(e); break;
            case (ChapterViewModel ch, PlotPointViewModel p): p.LinkChapter(ch); break;
            case (PlotPointViewModel p, ChapterViewModel ch): p.LinkChapter(ch); break;
            case (LocationViewModel l, PlotPointViewModel p): p.LinkLocation(l); break;
            case (PlotPointViewModel p, LocationViewModel l): p.LinkLocation(l); break;
        }
    }
}