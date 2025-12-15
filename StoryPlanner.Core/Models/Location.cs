using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // "Canterlot Castle"
    public string Region { get; set; } = string.Empty; // "Central Equestria"
    public string Description { get; set; } = string.Empty;
    
    public ObservableCollection<PlotPoint> PlotPoints { get; set; } = new();
    public ObservableCollection<Note> Notes { get; set; } = new();
}