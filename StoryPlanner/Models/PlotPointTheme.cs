namespace StoryPlanner.Models;

// Link: PlotPoint <-> Theme (Thematic Argument)
public class PlotPointTheme
{
    public int PlotPointId { get; set; }
    public PlotPoint PlotPoint { get; set; } = null!;

    public int ThemeId { get; set; }
    public Theme Theme { get; set; } = null!;

    public ThemeProminence Prominence { get; set; }
    public string? Commentary { get; set; } // "Shows the failure of diplomacy"
}