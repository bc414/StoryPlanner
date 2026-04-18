using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class StoryThread
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "The Changeling Front"
    public string Description { get; set; } = string.Empty;
    // NEW: Store the emoji directly in the database
    public string Icon { get; set; } = "🧵"; // Default to a thread spool?
    public ThreadScope ThreadScope { get; set; }

    public ObservableCollection<PlotPointStoryThread> PlotPointAssignments { get; set; } = new();

    public ObservableCollection<Note> Notes { get; set; } = new();
}