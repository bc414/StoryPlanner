namespace StoryPlanner.Models;

public class Theme
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // "The Cost of War"
    
    public string Description { get; set; } = string.Empty;
    
    public string ColorHex { get; set; } = string.Empty;
    public List<Note> Notes { get; set; } = new();
    public List<PlotPointTheme> PlotPointAssignments { get; set; } = new();
}