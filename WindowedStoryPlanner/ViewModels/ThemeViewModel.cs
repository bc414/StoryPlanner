using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace WindowedStoryPlanner.ViewModels;

public partial class ThemeViewModel : EntityViewModel
{
    private readonly Theme _theme;
    public Theme Theme => _theme;

    public ThemeViewModel(Theme theme, IEditorCoordinator editorCoordinator) : base(editorCoordinator)
    {
        _theme = theme;

        // Initialize the notes collection generic to the entity
        NoteCollectionViewModel = new NoteCollectionViewModel(theme.Notes);
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
        var relevantPoints = _theme.PlotPointAssignments.Select(x => x.PlotPoint);
        PlotPointCollectionViewModel = new PlotPointCollectionViewModel();
        PlotPointCollectionViewModel.SetAndSortItems(relevantPoints, standardSorter);

        // 4. Live Updates
        _theme.PlotPointAssignments.CollectionChanged += (s, e) =>
        {
            var updatedPoints = _theme.PlotPointAssignments.Select(x => x.PlotPoint);
            PlotPointCollectionViewModel.SetAndSortItems(updatedPoints, standardSorter);
        };
    }

    // --- Properties Wrapper ---

    public string Name
    {
        get => _theme.Name;
        set
        {
            if (SetProperty(_theme.Name, value, _theme, (u, n) => u.Name = n))
            {
                // Fix: Name is the fallback for BadgeText, so we must notify it
                OnPropertyChanged(nameof(BadgeText)); 
            }
        }
    }

    public string Description
    {
        get => _theme.Description;
        set => SetProperty(_theme.Description, value, _theme, (u, n) => u.Description = n);
    }

    public string Abbreviation
    {
        get => _theme.Abbreviation;
        set
        {
            if (SetProperty(_theme.Abbreviation, value, _theme, (u, n) => u.Abbreviation = n))
            {
                // Fix: Abbreviation is the primary source for BadgeText
                OnPropertyChanged(nameof(BadgeText)); 
            }
        }
    }

    public string ColorHex
    {
        get => _theme.ColorHex;
        set
        {
            if (SetProperty(_theme.ColorHex, value, _theme, (u, n) => u.ColorHex = n))
            {
                // Fix: ColorHex drives both Background and Foreground
                OnPropertyChanged(nameof(BadgeBackground));
                OnPropertyChanged(nameof(BadgeForeground));
            }
        }
    }

    // --- NEW COMPUTED BADGE PROPERTIES ---

    public string BadgeText => !string.IsNullOrEmpty(Abbreviation)
        ? Abbreviation
        : (Name.Length >= 2 ? Name.Substring(0, 2) : Name).ToUpper();

    public Brush BadgeBackground
    {
        get
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(
                    !string.IsNullOrEmpty(ColorHex) ? ColorHex : "#CCCCCC");
                return new SolidColorBrush(color);
            }
            catch
            {
                return Brushes.LightGray;
            }
        }
    }

    public Brush BadgeForeground
    {
        get
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(
                    !string.IsNullOrEmpty(ColorHex) ? ColorHex : "#CCCCCC");
                // Luminance formula to determine if dark or light
                return (color.R * 0.299 + color.G * 0.587 + color.B * 0.114) < 186 ? Brushes.White : Brushes.Black;
            }
            catch
            {
                return Brushes.Black;
            }
        }
    }
}