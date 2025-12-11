namespace StoryPlanner.Models;

// Link: PlotPoint <-> Character (Development Arc)
public class PlotPointCharacter
{
    public int PlotPointId { get; set; }
    public PlotPoint PlotPoint { get; set; } = null!;

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    public CharacterRole Role { get; set; }
    public CharacterDevImpact DevelopmentImpact { get; set; }
    public string? DevelopmentNote { get; set; } // "Loses trust in Command"
}