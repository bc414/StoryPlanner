using System.Collections.ObjectModel;

namespace StoryPlanner.Core.Models;

public class Chapter
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // 0 = "(Unassigned)" sentinel (see UnassignedStory), never a real EF-generated Story PK.
    public int StoryId { get; set; }

    // The explicit order within its story (1, 2, 3...)
    public int OrderIndex { get; set; }

    public OwnerType OwnerType => OwnerType.Chapter;
}