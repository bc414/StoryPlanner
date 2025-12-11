namespace StoryPlanner.Models;

public class StoryThread
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "The Changeling Front"
    public string Description { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#FFFFFF"; // For UI pill badges

    public List<PlotPointThread> PlotPointAssignments { get; set; } = new();
}