using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryPlanner.Core.Timeline;

/// <summary>
/// Maps a fractional world year to a pixel offset. Normally linear (space = time, the
/// invariant), with explicitly-chosen ranges compressed to a fixed height — the one sanctioned
/// departure, because a collapse is authored and labelled rather than silently applied. Inside a
/// collapsed range the mapping stays monotonic, so anything still drawn there keeps its relative
/// order; it is compression, never deletion.
/// </summary>
public sealed class YearAxis
{
    private readonly record struct Segment(
        double FromYear, double ToYear, double PixelTop, double PixelHeight, bool IsCollapsed);

    private readonly List<Segment> _segments;

    public double MinYear { get; }
    public double MaxYear { get; }
    public double Top { get; }
    public double Height { get; }

    private YearAxis(List<Segment> segments, double minYear, double maxYear, double top)
    {
        _segments = segments;
        MinYear = minYear;
        MaxYear = maxYear;
        Top = top;
        Height = segments.Count == 0 ? 0 : segments[^1].PixelTop + segments[^1].PixelHeight - top;
    }

    /// <summary>
    /// Builds the axis. <paramref name="collapsed"/> ranges are clipped to [minYear, maxYear],
    /// merged where they overlap, and each rendered at <paramref name="collapsedHeight"/> pixels
    /// regardless of how many years it spans.
    /// </summary>
    public static YearAxis Build(
        double minYear,
        double maxYear,
        double pixelsPerYear,
        IEnumerable<(double From, double To)>? collapsed = null,
        double collapsedHeight = 26,
        double top = 0)
    {
        if (maxYear < minYear) (minYear, maxYear) = (maxYear, minYear);

        var ranges = (collapsed ?? Enumerable.Empty<(double, double)>())
            .Select(r => (From: Math.Max(r.From, minYear), To: Math.Min(r.To, maxYear)))
            .Where(r => r.To > r.From)
            .OrderBy(r => r.From)
            .ToList();

        // Merge overlaps so no year is compressed twice.
        var merged = new List<(double From, double To)>();
        foreach (var r in ranges)
        {
            if (merged.Count > 0 && r.From <= merged[^1].To)
                merged[^1] = (merged[^1].From, Math.Max(merged[^1].To, r.To));
            else
                merged.Add(r);
        }

        var segments = new List<Segment>();
        var cursorYear = minYear;
        var cursorPixel = top;

        void AddNormal(double toYear)
        {
            if (toYear <= cursorYear) return;
            var h = (toYear - cursorYear) * pixelsPerYear;
            segments.Add(new Segment(cursorYear, toYear, cursorPixel, h, false));
            cursorYear = toYear;
            cursorPixel += h;
        }

        foreach (var (from, to) in merged)
        {
            AddNormal(from);
            segments.Add(new Segment(from, to, cursorPixel, collapsedHeight, true));
            cursorYear = to;
            cursorPixel += collapsedHeight;
        }
        AddNormal(maxYear);

        if (segments.Count == 0) // degenerate: zero-width span
            segments.Add(new Segment(minYear, minYear, top, 0, false));

        return new YearAxis(segments, minYear, maxYear, top);
    }

    /// <summary>Pixel offset for a fractional year. Clamped to the axis range.</summary>
    public double YOf(double year)
    {
        if (year <= MinYear) return Top;
        if (year >= MaxYear) return Top + Height;

        foreach (var s in _segments)
        {
            if (year > s.ToYear) continue;
            var span = s.ToYear - s.FromYear;
            var frac = span <= 0 ? 0 : (year - s.FromYear) / span;
            return s.PixelTop + frac * s.PixelHeight;
        }
        return Top + Height;
    }

    /// <summary>True when this year falls inside a compressed range — callers fold point items
    /// into the range's summary band rather than drawing them at an unreadable height.</summary>
    public bool IsCollapsedAt(double year) =>
        _segments.Any(s => s.IsCollapsed && year >= s.FromYear && year < s.ToYear);

    /// <summary>The compressed ranges as (fromYear, toYear, pixelTop, pixelHeight), for drawing
    /// their summary bands.</summary>
    public IEnumerable<(double FromYear, double ToYear, double PixelTop, double PixelHeight)> CollapsedBands() =>
        _segments.Where(s => s.IsCollapsed)
                 .Select(s => (s.FromYear, s.ToYear, s.PixelTop, s.PixelHeight));
}
