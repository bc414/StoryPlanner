namespace StoryPlanner.Core
{
    /// <summary>
    /// An authored year at which the world's causal regime changed — eras are DERIVED as the
    /// intervals between consecutive pivots (N pivots → N+1 eras), so overlap and gaps are
    /// structurally impossible and eras have nothing of their own to name or store.
    ///
    /// Pivots are world-wide, never per-theater: the world is connected and causal once ocean
    /// travel exists. A pivot may be a single event with propagation lag or a loosely related
    /// cluster; it is NOT a pointer to a causal note — propagation is emergent on the timeline
    /// (spans terminating at different years in different theaters), never stored. A span
    /// crossing a pivot is not an error; it may record how long a cause took to arrive.
    /// </summary>
    public class Pivot
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
