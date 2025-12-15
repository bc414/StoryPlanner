using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

// Link: PlotPoint <-> Thread
public class PlotPointThread
{
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int ThreadId { get; set; }
    [JsonIgnore]
    public StoryThread StoryThread { get; set; } = null!;

    public GoalTrajectory ThreadTrajectory { get; set; }

    // Helps the UI decide which badge to show on the main timeline
    public bool IsPrimary { get; set; }

    //Describe the impact that this plot point has for the trajectory of the thread
    public string ImpactDescription { get; set; } = null!;
}