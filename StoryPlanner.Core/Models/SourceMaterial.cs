namespace StoryPlanner.Core.Models;

public class SourceMaterial
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Abbreviation { get; set; } = string.Empty;

    public string ColorHex { get; set; } = "#cccccc";

    public List<Note> Notes { get; set; } = new();
}