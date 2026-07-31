namespace StoryPlanner.Core.Models
{
    /// <summary>
    /// A timeline column — the x-axis coordinate of the master timeline. A display coordinate,
    /// not a taxonomy: ordered by narrative density/importance (OrderIndex), not map position,
    /// and reorderable at runtime. Deliberately carries NO ColorHex — hue is reserved for
    /// subject type (+ plot points); theater identity comes from column headers and neutral
    /// banding, or two colour systems would collide.
    /// </summary>
    public class Theater
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }

    public static class UnplacedTheater
    {
        public static readonly Theater Definition = new()
        {
            Id = 0,   // 0 is never a valid EF-generated PK — same sentinel pattern as UnassignedStory
            Name = "(Unplaced)",
        };
    }
}
