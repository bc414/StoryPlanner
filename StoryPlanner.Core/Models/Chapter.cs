using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class Chapter
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // The explicit order in the book (1, 2, 3...)
    public int OrderIndex { get; set; } 

    public ObservableCollection<PlotPoint> PlotPoints { get; set; } = new();

    public ObservableCollection<Note> Notes { get; set; } = new();
}