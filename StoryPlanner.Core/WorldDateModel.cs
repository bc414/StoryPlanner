using StoryPlanner.Core;

namespace StoryPlanner.Core;

/// <summary>
/// Bridges the flat nullable-int date columns on the row-vessel models (Note, PlotPoint) to
/// the <see cref="WorldDate"/> value type. Models stay behavior-free by convention; this is
/// the one place the column↔struct mapping lives.
/// </summary>
public static class WorldDateModel
{
    public static WorldDate? GetWorldDate(this Note n)
        => Compose(n.WorldDateStartYear, n.WorldDateStartMonth, n.WorldDateStartDay,
                   n.WorldDateEndYear, n.WorldDateEndMonth, n.WorldDateEndDay);

    /// <summary>
    /// The note's date regardless of conversion state — structured columns first, the legacy
    /// free-text string converted mechanically as a fallback. Null = undated or unconvertible.
    /// This is the one read path every consumer shares (MCP tools, the timeline, the cross-cut
    /// views); a second copy of it would drift silently, which is exactly what the legacy
    /// fallback cannot afford.
    /// </summary>
    public static WorldDate? EffectiveWorldDate(this Note n)
    {
        try
        {
            var structured = n.GetWorldDate();
            if (structured is not null) return structured;
        }
        catch (ArgumentException)
        {
            return null; // malformed columns — report as unparsed, never guess
        }
        var outcome = WorldDateLegacy.TryConvert(n.WorldDate, out var legacy);
        return outcome is WorldDateLegacy.Outcome.Point or WorldDateLegacy.Outcome.Range ? legacy : null;
    }

    /// <summary>True when the note carries ANY date signal — structured or legacy text,
    /// including unconvertible legacy text ("?" is a date-shaped claim, just not a usable one).</summary>
    public static bool HasAnyWorldDate(this Note n) =>
        n.WorldDateStartYear is not null || n.WorldDateEndYear is not null ||
        !string.IsNullOrWhiteSpace(n.WorldDate);

    public static void SetWorldDate(this Note n, WorldDate? date)
    {
        n.WorldDateStartYear = date?.Start?.Year;
        n.WorldDateStartMonth = date?.Start?.Month;
        n.WorldDateStartDay = date?.Start?.Day;
        n.WorldDateEndYear = date?.End?.Year;
        n.WorldDateEndMonth = date?.End?.Month;
        n.WorldDateEndDay = date?.End?.Day;
    }

    /// <summary>Plot points are events: start point only, by construction.</summary>
    public static WorldDatePoint? GetFabulaDate(this PlotPoint pp)
        => pp.FabulaYear is int y ? new WorldDatePoint(y, pp.FabulaMonth, pp.FabulaDay) : null;

    public static void SetFabulaDate(this PlotPoint pp, WorldDatePoint? at)
    {
        pp.FabulaYear = at?.Year;
        pp.FabulaMonth = at?.Month;
        pp.FabulaDay = at?.Day;
    }

    private static WorldDate? Compose(
        int? sy, int? sm, int? sd, int? ey, int? em, int? ed)
    {
        WorldDatePoint? start = sy is int y1 ? new WorldDatePoint(y1, sm, sd) : null;
        WorldDatePoint? end = ey is int y2 ? new WorldDatePoint(y2, em, ed) : null;
        if (start is null && end is null) return null;
        return new WorldDate(start, end);
    }
}
