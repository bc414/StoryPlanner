using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class PlotPointThreadViewModel : ObservableObject
{
    private readonly PlotPointStoryThread _link;

    public PlotPointThreadViewModel(PlotPointStoryThread link)
    {
        _link = link;
    }

    // Read-Only: Comes from the StoryThread definition
    public string ThreadName => _link.StoryThread?.Name ?? "Unknown Thread";

    // Read-Write: The Trajectory specific to this scene
    public GoalTrajectory Trajectory
    {
        get => _link.ThreadTrajectory;
        set
        {
            if (SetProperty(_link.ThreadTrajectory, value, _link, (u, n) => u.ThreadTrajectory = n))
            {
                // Notify that the color might need to change
                OnPropertyChanged(nameof(BadgeColor)); 
            }
        }
    }

    public string ImpactDescription
    {
        get => _link.ImpactDescription;
        set => SetProperty(_link.ImpactDescription, value, _link, (u, n) => u.ImpactDescription = n);
    }

    public bool IsPrimary
    {
        get => _link.IsPrimary;
        set => SetProperty(_link.IsPrimary, value, _link, (u, n) => u.IsPrimary = n);
    }

    // UI Logic: Helper for your XAML badges
    public string BadgeColor => Trajectory switch
    {
        GoalTrajectory.Triumph => "Green",
        GoalTrajectory.Positive => "LightGreen",
        GoalTrajectory.Negative => "Orange",
        GoalTrajectory.Disaster => "Red",
        _ => "Gray"
    };
}