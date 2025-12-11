namespace StoryPlanner.Models;

public class PlotPointNote
{
    public int PlotPointId { get; set; }
    public PlotPoint PlotPoint { get; set; } = null!;

    public int NoteId { get; set; }
    public Note Note { get; set; } = null!;

    // Did we follow the note?
    // "Applied" (Physics worked), "Subverted" (Character acted out of character)
    public CodexUsageType UsageType { get; set; } 
    public string? Comment { get; set; }
}