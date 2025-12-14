namespace StoryPlanner.Core.Models;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // "Canterlot Castle"
    public string Region { get; set; } = string.Empty; // "Central Equestria"
    public string Description { get; set; } = string.Empty;
    
    public List<PlotPoint> PlotPoints { get; set; } = new();
}