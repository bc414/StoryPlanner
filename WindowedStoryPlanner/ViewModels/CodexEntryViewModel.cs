using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class CodexEntryViewModel : EntityViewModel
{
    private readonly CodexEntry _codexEntry;
    public CodexEntry CodexEntry => _codexEntry;

    public CodexEntryViewModel(CodexEntry codexEntry)
    {
        _codexEntry = codexEntry;

        // Initialize the collection VM using the list from the model
        NoteCollectionViewModel = new NoteCollectionViewModel(codexEntry.Notes);
        NoteCollectionViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NoteCollectionViewModel.IsNoteReorderMode))
            {
                OnPropertyChanged(nameof(IsLinkingMode));
            }
        };
        
        // 2. Define Sorter
        Comparison<PlotPoint> standardSorter = (a, b) =>
        {
            int aChapter = a.Chapter?.OrderIndex ?? int.MaxValue;
            int bChapter = b.Chapter?.OrderIndex ?? int.MaxValue;

            if (aChapter != bChapter) return aChapter.CompareTo(bChapter);
            return a.OrderInChapter.CompareTo(b.OrderInChapter);
        };

        // 3. Extract & Initialize
        var relevantPoints = _codexEntry.PlotPointReferences.Select(x => x.PlotPoint);
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel();
        PlotPointCollectionViewModel.SetAndSortItems(relevantPoints, standardSorter);

        // 4. Live Updates
        _codexEntry.PlotPointReferences.CollectionChanged += (s, e) =>
        {
            var updatedPoints = _codexEntry.PlotPointReferences.Select(x => x.PlotPoint);
            PlotPointCollectionViewModel.SetAndSortItems(updatedPoints, standardSorter);
        };
    }

    // --- Properties Wrapper ---
    // Wraps the Model properties to trigger PropertyChanged notifications for the UI

    public string Title
    {
        get => _codexEntry.Title;
        set
        {
            if (SetProperty(_codexEntry.Title, value, _codexEntry, (u, n) => u.Title = n))
            {
                // Fix: Title is the fallback for BadgeText
                OnPropertyChanged(nameof(BadgeText));
            }
        }
    }

    public string Type
    {
        get => _codexEntry.Type;
        set => SetProperty(_codexEntry.Type, value, _codexEntry, (u, n) => u.Type = n);
    }

    public string Description
    {
        get => _codexEntry.Description;
        set => SetProperty(_codexEntry.Description, value, _codexEntry, (u, n) => u.Description = n);
    }

    public CodexCategory Category
    {
        get => _codexEntry.Category;
        set => SetProperty(_codexEntry.Category, value, _codexEntry, (u, n) => u.Category = n);
    }
    
    public string Abbreviation
    {
        get => _codexEntry.Abbreviation;
        set { if (SetProperty(_codexEntry.Abbreviation, value, _codexEntry, (u, n) => u.Abbreviation = n)) OnPropertyChanged(nameof(BadgeText)); }
    }

    public string ColorHex
    {
        get => _codexEntry.ColorHex;
        set { if (SetProperty(_codexEntry.ColorHex, value, _codexEntry, (u, n) => u.ColorHex = n)) OnPropertyChanged(nameof(BadgeBrush)); }
    }

    // --- COMPUTED BADGE PROPERTIES ---
    public string BadgeText => !string.IsNullOrWhiteSpace(Abbreviation)
        ? Abbreviation.ToUpper()
        : (Title.Length > 3 ? Title.Substring(0, 3) : Title).ToUpper();

    public Brush BadgeBrush
    {
        get
        {
            try { return new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorHex ?? "#999999")); }
            catch { return Brushes.Gray; }
        }
    }
}