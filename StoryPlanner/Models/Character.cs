namespace StoryPlanner.Models;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Inspiration { get; set; } = string.Empty;
    public string Archetype { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Note> Notes { get; set; } = new();
    public List<PlotPointCharacter> Appearances { get; set; } = new();
}