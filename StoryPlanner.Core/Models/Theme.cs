using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class Theme
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // "The Cost of War"
    
    public string Description { get; set; } = string.Empty;
    
    public string ColorHex { get; set; } = string.Empty;
    public ObservableCollection<Note> Notes { get; set; } = new();
    public ObservableCollection<PlotPointTheme> PlotPointAssignments { get; set; } = new();
}