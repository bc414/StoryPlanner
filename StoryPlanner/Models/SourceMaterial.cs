namespace StoryPlanner.Models;

public class SourceMaterial
{
    public int SourceMaterialId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Abbreviation { get; set; } = string.Empty;

    public string ColorHex { get; set; } = "#cccccc";

    public List<Note> Notes { get; set; } = new();
}