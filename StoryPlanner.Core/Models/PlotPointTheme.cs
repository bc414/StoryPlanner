using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

// Link: PlotPoint <-> Theme (Thematic Argument)
public class PlotPointTheme
{
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int ThemeId { get; set; }
    [JsonIgnore]
    public Theme Theme { get; set; } = null!;

    public ThemeProminence Prominence { get; set; }
    public string? Commentary { get; set; } // "Shows the failure of diplomacy"
}