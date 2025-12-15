using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class StoryThread
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "The Changeling Front"
    public string Description { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#FFFFFF"; // For UI pill badges
    public ThreadScope ThreadScope { get; set; }

    public ObservableCollection<PlotPointThread> PlotPointAssignments { get; set; } = new();

    public ObservableCollection<Note> Notes { get; set; } = new();
}