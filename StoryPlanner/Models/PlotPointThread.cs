namespace StoryPlanner.Models;

// Link: PlotPoint <-> Thread
public class PlotPointThread
{
    public int PlotPointId { get; set; }
    public PlotPoint PlotPoint { get; set; } = null!;

    public int ThreadId { get; set; }
    public StoryThread StoryThread { get; set; } = null!;
}