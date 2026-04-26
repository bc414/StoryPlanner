namespace StoryPlanner.Core.Models;

public class NotePropertyStats
{
    // The name of the row (e.g., "Needs Analysis", "Incorporated")
    public string StatName { get; set; } = string.Empty;
    
    // The metric columns
    public int Total { get; set; }
    public int Characters { get; set; }
    public int Themes { get; set; }
    public int Codex { get; set; }
    public int Chapters { get; set; }
    public int Threads { get; set; }
    public int Unassigned { get; set; }
}