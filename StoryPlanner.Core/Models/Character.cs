using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Inspiration { get; set; } = string.Empty;
    public string Archetype { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservableCollection<Note> Notes { get; set; } = new();
    public ObservableCollection<PlotPointCharacter> Appearances { get; set; } = new();
}