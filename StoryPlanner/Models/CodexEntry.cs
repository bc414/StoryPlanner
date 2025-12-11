namespace StoryPlanner.Models;

public class CodexEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public CodexCategory Category { get; set; }

    // "Lore Facts"
    public List<Note> Notes { get; set; } = new();
}