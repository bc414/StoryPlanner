using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public class PlotPointNote
{
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int NoteId { get; set; }
    [JsonIgnore]
    public Note Note { get; set; } = null!;

    // Did we follow the note?
    // "Applied" (Physics worked), "Subverted" (Character acted out of character)
    public CodexUsageType UsageType { get; set; } 
    public string? Comment { get; set; }
}