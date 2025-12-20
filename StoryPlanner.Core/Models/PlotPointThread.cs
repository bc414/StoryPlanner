using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

// Link: PlotPoint <-> Thread
public partial class PlotPointThread : ObservableObject
{
    // --- KEYS (Standard) ---
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int ThreadId { get; set; }
    [JsonIgnore]
    public StoryThread StoryThread { get; set; } = null!;

    // --- PAYLOAD (Observable for Live Edit) ---

    [ObservableProperty]
    private GoalTrajectory _threadTrajectory;

    // Helps the UI decide which badge to show on the main timeline
    // (This might trigger a visual update on the card, so it must be observable)
    [ObservableProperty]
    private bool _isPrimary;

    // Describe the impact that this plot point has for the trajectory of the thread
    [ObservableProperty]
    private string _impactDescription = string.Empty;

    [ObservableProperty]
    private int _sortOrder;
}