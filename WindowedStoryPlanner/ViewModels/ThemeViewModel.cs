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

    public ThemeViewModel(Theme theme)
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
    }

    // --- Properties Wrapper ---

    public string Name
    {
        get => _theme.Name;
        set => SetProperty(_theme.Name, value, _theme, (u, n) => u.Name = n);
    }

    public string Description
    {
        get => _theme.Description;
        set => SetProperty(_theme.Description, value, _theme, (u, n) => u.Description = n);
    }

    public string Abbreviation
    {
        get => _theme.Abbreviation;
        set => SetProperty(_theme.Abbreviation, value, _theme, (u, n) => u.Abbreviation = n);
    }

    public string ColorHex
    {
        get => _theme.ColorHex;
        set => SetProperty(_theme.ColorHex, value, _theme, (u, n) => u.ColorHex = n);
    }

    public ObservableCollection<ThemeBadge> ThemeBadges
    {
        get
        {
            var badges = new ObservableCollection<ThemeBadge>();
            var color = (Color)ColorConverter.ConvertFromString(
                    !string.IsNullOrEmpty(ColorHex) ? ColorHex : "#CCCCCC");

            var brush = new SolidColorBrush(color);
            badges.Add(new ThemeBadge
            {
                Text = Abbreviation,
                Background = brush,
                // Simple logic: If background is dark, use white text. If light, use black.
                Foreground = IsDark(color) ? Brushes.White : Brushes.Black
            });
            return badges;
        }
    }

    private bool IsDark(Color c) => (c.R * 0.299 + c.G * 0.587 + c.B * 0.114) < 186;
}