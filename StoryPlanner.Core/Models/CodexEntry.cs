using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class CodexEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CodexCategory Category { get; set; }
    public string Abbreviation { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#";

    // "Lore Facts"
    public ObservableCollection<Note> Notes { get; set; } = new();
    public ObservableCollection<PlotPointCodexEntry> PlotPointReferences { get; set; } = new();
}