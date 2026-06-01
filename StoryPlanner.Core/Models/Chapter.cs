using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class Chapter
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    // The explicit order in the book (1, 2, 3...)
    public int OrderIndex { get; set; }

    public OwnerType OwnerType => OwnerType.Chapter;
}