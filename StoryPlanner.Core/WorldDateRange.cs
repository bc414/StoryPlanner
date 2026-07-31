namespace StoryPlanner.Core;

/// <summary>
/// A half-open window on the world timeline, in fractional years, used to ask whether a
/// <see cref="WorldDate"/> falls inside it. Retrieval only: it answers "does this overlap",
/// never "how well" — there is no score, no distance, no ranking.
///
/// Two rules live here and nowhere else, because both are subtle enough that a second copy
/// would drift silently:
///
/// 1. <b>Inclusive year in, exclusive edge out.</b> A range named by years includes all of the
///    end year, so its upper edge is <c>toYear + 1.0</c>. This matches a bare year's own
///    <see cref="WorldDatePoint.LatestFraction"/>, which is why "1007" is inside "1007..1007".
/// 2. <b>A start-only date means different things on different tracks.</b> On an EVENT track it
///    is a point whose extent is its own precision; on a CONDITION track
///    (<c>NoteTrackDefinition.SupportsWorldDateEnd</c>) it means "in force, end TBD" and so
///    extends to +infinity. The track supplies that, never the stored value — see
///    <see cref="WorldDate"/>'s remarks.
/// </summary>
public readonly record struct WorldDateRange(double Lo, double Hi)
{
    /// <summary>The whole timeline — what an empty range input means.</summary>
    public static readonly WorldDateRange All = new(double.NegativeInfinity, double.PositiveInfinity);

    /// <summary>Year-named range, both ends inclusive and optional. Null = unbounded that way.</summary>
    public static WorldDateRange FromYears(int? fromYear, int? toYear) => new(
        fromYear ?? double.NegativeInfinity,
        toYear is int ty ? ty + 1.0 : double.PositiveInfinity);

    /// <summary>
    /// Range from a parsed <see cref="WorldDate"/> bound. <paramref name="asInterval"/> comes from
    /// the input text having used ".." — the parser cannot tell "1007" (the year 1007) from
    /// "1007.." (1007 onwards), since both store a start with no end. Same context rule as
    /// <see cref="WorldDate.ToNotation"/>. A null bound is <see cref="All"/>.
    /// </summary>
    public static WorldDateRange FromBound(WorldDate? bound, bool asInterval)
    {
        if (bound is not { } b) return All;
        var lo = b.Start?.EarliestFraction ?? double.NegativeInfinity;
        var hi = b.End is not null
            ? b.End.Value.LatestFraction
            : asInterval ? double.PositiveInfinity
                         : b.Start?.LatestFraction ?? double.PositiveInfinity;
        return new WorldDateRange(lo, hi);
    }

    /// <summary>
    /// The extent a date can touch, as fractional years. A TBD start reaches back to -infinity
    /// (it is unknown, not absent); a start-only value on a condition track reaches +infinity.
    /// Also the sort key for chronological ordering: earliest, then latest.
    /// </summary>
    public static (double Earliest, double Latest) Span(WorldDate date, bool isConditionTrack) => (
        date.EarliestFraction ?? double.NegativeInfinity,
        date.End is not null
            ? date.LatestFraction!.Value
            : isConditionTrack ? double.PositiveInfinity
                               : date.LatestFraction ?? double.PositiveInfinity);

    /// <summary>
    /// Whether the date touches this range at any point. Both intervals are half-open
    /// [earliest, latest) and [Lo, Hi), so BOTH comparisons are strict — an exclusive upper edge
    /// that lands exactly on an inclusive lower bound does not overlap it. (Writing this as
    /// <c>latest >= Lo</c> admits a note dated 914 into the range "915..", which is what the
    /// pre-promotion copy of this predicate did.)
    /// </summary>
    public bool Overlaps(WorldDate date, bool isConditionTrack)
    {
        var (earliest, latest) = Span(date, isConditionTrack);
        return latest > Lo && earliest < Hi;
    }

    public bool IsUnbounded => double.IsNegativeInfinity(Lo) && double.IsPositiveInfinity(Hi);
}
