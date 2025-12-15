using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class PlotPointViewModel : ObservableObject
{
    private readonly PlotPoint _model;

    // --- Sub-ViewModels (Wrappers for Complex Lists) ---
    // We wrap the threads so we can display the Thread Name + Trajectory easily
    public ObservableCollection<PlotPointThreadViewModel> Threads { get; } = new();

    public PlotPointViewModel(PlotPoint model)
    {
        _model = model;

        // 1. Hydrate the Thread wrappers
        foreach (var threadAssignment in model.ThreadAssignments)
        {
            Threads.Add(new PlotPointThreadViewModel(threadAssignment));
        }

        // 2. Listen for changes (Optional: If you add threads dynamically, sync the collections)
        // For a prototype, you can just manually add to both collections in your 'AddThread' command.
    }

    // --- Wrapper Properties ---

    public string Title
    {
        get => _model.Title;
        set => SetProperty(_model.Title, value, _model, (u, n) => u.Title = n);
    }

    public string Stakes
    {
        get => _model.Stakes;
        set => SetProperty(_model.Stakes, value, _model, (u, n) => u.Stakes = n);
    }

    public string Synopsis
    {
        get => _model.Synopsis;
        set => SetProperty(_model.Synopsis, value, _model, (u, n) => u.Synopsis = n);
    }

    public string Outcome
    {
        get => _model.Outcome;
        set => SetProperty(_model.Outcome, value, _model, (u, n) => u.Outcome = n);
    }

    public DraftStatus Status
    {
        get => _model.Status;
        set => SetProperty(_model.Status, value, _model, (u, n) => u.Status = n);
    }

    // --- 5 Axes Wrappers ---

    public CoreDriver CoreDriver
    {
        get => _model.CoreDriver;
        set => SetProperty(_model.CoreDriver, value, _model, (u, n) => u.CoreDriver = n);
    }

    public TensionPhase TensionPhase
    {
        get => _model.TensionPhase;
        set => SetProperty(_model.TensionPhase, value, _model, (u, n) => u.TensionPhase = n);
    }

    public ConflictType ConflictType
    {
        get => _model.ConflictType;
        set => SetProperty(_model.ConflictType, value, _model, (u, n) => u.ConflictType = n);
    }

    public Presentation Presentation
    {
        get => _model.Presentation;
        set => SetProperty(_model.Presentation, value, _model, (u, n) => u.Presentation = n);
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
            foreach (var assign in _model.ThemeAssignments)
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
            var icons = _model.ThreadAssignments
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
            PlotPoint = _model, 
            StoryThread = thread,
            ThreadId = thread.Id,
            ThreadTrajectory = GoalTrajectory.Unset 
        };

        // 2. Update Model
        _model.ThreadAssignments.Add(newLink);

        // 3. Update View
        Threads.Add(new PlotPointThreadViewModel(newLink));
    }
}

public struct ThemeBadge
{
    public string Text { get; set; }
    public Brush Background { get; set; }
    public Brush Foreground { get; set; } // For contrast
}