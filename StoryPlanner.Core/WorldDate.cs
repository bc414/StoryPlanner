using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StoryPlanner.Core;

/// <summary>
/// One endpoint of a world date: a year with optional month and day. Year is the precision
/// floor — these are real fabula dates, not myths; characters may perceive them as myths, the
/// record does not. Month/day are progressive pinning: null means "to be determined", never
/// "approximately". Negative years are BLB (before Luna's banishment); year 0 is the
/// banishment itself.
/// </summary>
public readonly record struct WorldDatePoint : IComparable<WorldDatePoint>
{
    public int Year { get; }
    public int? Month { get; }
    public int? Day { get; }

    public WorldDatePoint(int year, int? month = null, int? day = null)
    {
        if (month is < 1 or > 12)
            throw new ArgumentOutOfRangeException(nameof(month), month, "Month must be 1-12.");
        if (day is not null && month is null)
            throw new ArgumentException("Day requires a month.", nameof(day));
        if (day is < 1 or > 31)
            throw new ArgumentOutOfRangeException(nameof(day), day, "Day must be 1-31.");
        Year = year;
        Month = month;
        Day = day;
    }

    /// <summary>Earliest moment this point could denote, as a fractional year (for layout/sort).</summary>
    public double EarliestFraction => Fraction(Month ?? 1, Day ?? 1);

    /// <summary>Latest moment this point could denote, as a fractional year (exclusive upper edge).</summary>
    public double LatestFraction => Month is null
        ? Year + 1.0
        : Day is null ? Fraction(Month.Value + 1, 1) : Fraction(Month.Value, Day.Value + 1);

    // Uniform 12×31 grid: display/sort approximation only, never stored. A fictional calendar's
    // real month lengths are unknown; what matters is monotonic ordering, which this preserves.
    private double Fraction(int month, int day) => Year + (month - 1) / 12.0 + (day - 1) / 372.0;

    public int CompareTo(WorldDatePoint other)
    {
        var c = Year.CompareTo(other.Year);
        if (c != 0) return c;
        c = (Month ?? 1).CompareTo(other.Month ?? 1);
        if (c != 0) return c;
        return (Day ?? 1).CompareTo(other.Day ?? 1);
    }

    public override string ToString() => Month is null
        ? Year.ToString(CultureInfo.InvariantCulture)
        : Day is null
            ? $"{Year}-{Month:00}"
            : $"{Year}-{Month:00}-{Day:00}";
}

/// <summary>
/// A structured world date: a start point and an optional end point. Either endpoint may be
/// absent (still to be determined) but not both. Whether a start-only value means an *event*
/// ("1007") or a *condition whose end is TBD* ("1007..") is determined by the owning note's
/// TRACK (event track vs condition track; see NoteTrackDefinition.SupportsWorldDateEnd), never
/// stored in the value — plot points are always events. This is why parse and render both take
/// the track's shape as context rather than persisting a discriminator.
///
/// Notation:
///   1007            event in 1007, month/day TBD
///   1007-03-15      ISO, progressively pinned
///   854..914        interval, year precision both ends
///   1007..          interval, end TBD        ..1007   interval, start TBD
///   -100..0         negative years are BLB; 0 is the banishment
/// "300 BLB" is accepted on legacy input and normalises to -300 (see <see cref="WorldDateLegacy"/>).
/// </summary>
public readonly record struct WorldDate
{
    public WorldDatePoint? Start { get; }
    public WorldDatePoint? End { get; }

    public WorldDate(WorldDatePoint? start, WorldDatePoint? end = null)
    {
        if (start is null && end is null)
            throw new ArgumentException("A world date needs at least one endpoint.");
        if (start is { } s && end is { } e && s.CompareTo(e) > 0)
            throw new ArgumentException($"Inverted interval: {s}..{e}. Start must not be after end.");
        Start = start;
        End = end;
    }

    public static WorldDate Event(WorldDatePoint at) => new(at);

    /// <summary>True when either endpoint is still to be determined.</summary>
    public bool IsPartial => Start is null || End is null;

    /// <summary>Earliest fractional year this date can touch; null when the start is TBD.</summary>
    public double? EarliestFraction => Start?.EarliestFraction;

    /// <summary>
    /// Latest fractional year this date can touch. For a start-only value this is the start
    /// point's own upper edge — correct for events; for an end-TBD condition the caller decides
    /// how far the faded tail runs (that is rendering, not data).
    /// </summary>
    public double? LatestFraction => End?.LatestFraction ?? Start?.LatestFraction;

    // ── Notation ────────────────────────────────────────────────────────────────

    private static readonly Regex PointRx = new(
        @"^(?<y>-?\d{1,6})(?:-(?<m>\d{1,2})(?:-(?<d>\d{1,2}))?)?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool TryParsePoint(string text, out WorldDatePoint point, out string error)
    {
        point = default;
        error = "";
        var m = PointRx.Match(text.Trim());
        if (!m.Success)
        {
            error = $"'{text.Trim()}' is not a date — expected YYYY, YYYY-MM, or YYYY-MM-DD " +
                    "(negative year = BLB; use '..' for intervals, e.g. 854..914).";
            return false;
        }
        var year = int.Parse(m.Groups["y"].Value, CultureInfo.InvariantCulture);
        int? month = m.Groups["m"].Success ? int.Parse(m.Groups["m"].Value, CultureInfo.InvariantCulture) : null;
        int? day = m.Groups["d"].Success ? int.Parse(m.Groups["d"].Value, CultureInfo.InvariantCulture) : null;
        if (month is < 1 or > 12)
        {
            error = $"Month {month} is out of range (1-12) in '{text.Trim()}'. " +
                    "If you meant a year range, the notation is 'A..B', not 'A-B'.";
            return false;
        }
        if (day is < 1 or > 31)
        {
            error = $"Day {day} is out of range (1-31) in '{text.Trim()}'.";
            return false;
        }
        point = new WorldDatePoint(year, month, day);
        return true;
    }

    /// <summary>
    /// Parses the notation. Empty/whitespace input yields success with a null date ("no date").
    /// Inverted intervals are rejected here — unrepresentable rather than detectable downstream.
    /// Interval notation on a value destined for an event slot is the *editor's* job to refuse
    /// (it knows the track); the parser accepts any well-formed value.
    /// </summary>
    public static bool TryParse(string? text, out WorldDate? date, out string error)
    {
        date = null;
        error = "";
        if (string.IsNullOrWhiteSpace(text)) return true;

        var t = text.Trim();
        var sep = t.IndexOf("..", StringComparison.Ordinal);
        if (sep < 0)
        {
            if (!TryParsePoint(t, out var at, out error)) return false;
            date = new WorldDate(at);
            return true;
        }

        var startText = t[..sep].Trim();
        var endText = t[(sep + 2)..].Trim();
        if (startText.Length == 0 && endText.Length == 0)
        {
            error = "'..' alone is not a date — give at least one endpoint (e.g. '1007..' or '..1007').";
            return false;
        }

        WorldDatePoint? start = null, end = null;
        if (startText.Length > 0)
        {
            if (!TryParsePoint(startText, out var s, out error)) return false;
            start = s;
        }
        if (endText.Length > 0)
        {
            if (!TryParsePoint(endText, out var e, out error)) return false;
            end = e;
        }
        if (start is { } sp && end is { } ep && sp.CompareTo(ep) > 0)
        {
            error = $"Inverted interval: {sp}..{ep}. Start must not be after end.";
            return false;
        }
        date = new WorldDate(start, end);
        return true;
    }

    /// <summary>
    /// Renders the notation. <paramref name="asInterval"/> comes from the track shape
    /// (SupportsWorldDateEnd) or is true for any value that has an end: a start-only value on a
    /// condition track renders "1007.." (in force, end TBD); the same stored value on an event
    /// track renders "1007".
    /// </summary>
    public string ToNotation(bool asInterval = false)
    {
        if (End is null && !asInterval) return Start?.ToString() ?? "";
        return $"{Start?.ToString() ?? ""}..{End?.ToString() ?? ""}";
    }

    public override string ToString() => ToNotation(End is not null);
}

/// <summary>
/// One-way conversion of the legacy free-text WorldDate strings ("993", "870-928", "-100-0",
/// "300 BLB", "998?") into structured <see cref="WorldDate"/>s. Used by the DataOps migration
/// op and by transition-period read fallbacks; never by new input paths — new input goes
/// through <see cref="WorldDate.TryParse"/> and its stricter notation.
/// </summary>
public static class WorldDateLegacy
{
    // Legacy semantics: a bare hyphen between two ints is a RANGE separator ("870-928",
    // "-100-0"), never ISO year-month. The new notation reverses this ("1007-03" is March);
    // the two parsers must stay separate for exactly this reason.
    private static readonly Regex LegacyRangeRx = new(
        @"^\s*(-?\d+)\s*-\s*(-?\d+)\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private static readonly Regex LegacySingleRx = new(
        @"^\s*(-?\d+)\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private static readonly Regex BlbRx = new(
        @"^\s*(\d+)\s*BLB\s*$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public enum Outcome
    {
        Empty,        // no input — nothing to convert
        Point,        // single year
        Range,        // year range (start <= end)
        Unconvertible // "?", inverted ranges, free prose — needs the triage surface
    }

    /// <summary>
    /// Mechanical, deterministic conversion. A trailing '?' is stripped (string-era approximation
    /// workaround — "I only know the year" is precision, not approximation). "N BLB" → -N.
    /// Inverted ranges are unconvertible by design: flag, never guess.
    /// </summary>
    public static Outcome TryConvert(string? raw, out WorldDate? date)
    {
        date = null;
        if (string.IsNullOrWhiteSpace(raw)) return Outcome.Empty;

        var t = raw.Trim();
        if (t == "?") return Outcome.Unconvertible;
        if (t.EndsWith('?')) t = t[..^1].Trim();

        var blb = BlbRx.Match(t);
        if (blb.Success)
        {
            date = WorldDate.Event(new WorldDatePoint(-int.Parse(blb.Groups[1].Value, CultureInfo.InvariantCulture)));
            return Outcome.Point;
        }

        var single = LegacySingleRx.Match(t);
        if (single.Success)
        {
            date = WorldDate.Event(new WorldDatePoint(int.Parse(single.Groups[1].Value, CultureInfo.InvariantCulture)));
            return Outcome.Point;
        }

        var range = LegacyRangeRx.Match(t);
        if (range.Success)
        {
            var start = int.Parse(range.Groups[1].Value, CultureInfo.InvariantCulture);
            var end = int.Parse(range.Groups[2].Value, CultureInfo.InvariantCulture);
            if (start > end) return Outcome.Unconvertible; // e.g. "954-914" — flag, never guess
            date = new WorldDate(new WorldDatePoint(start), new WorldDatePoint(end));
            return Outcome.Range;
        }

        return Outcome.Unconvertible;
    }
}
