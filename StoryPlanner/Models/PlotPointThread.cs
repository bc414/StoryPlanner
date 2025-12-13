using System.Text.Json.Serialization;
namespace StoryPlanner.Models;

// Link: PlotPoint <-> Thread
public class PlotPointThread
{
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int ThreadId { get; set; }
    [JsonIgnore]
    public StoryThread StoryThread { get; set; } = null!;
}