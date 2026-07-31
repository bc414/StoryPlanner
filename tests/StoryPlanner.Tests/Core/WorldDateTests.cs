using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using StoryPlanner.Core.Timeline;
using Xunit;

namespace StoryPlanner.Tests.Core;

/// <summary>
/// Pure tests for the structured world-date type: notation round-trip, the legacy converter's
/// flag-never-guess rule, era derivation, and lane packing. No .storyplan, no DbContext.
/// </summary>
public class WorldDateTests
{
    // ── Notation parse/render round-trip ────────────────────────────────────────

    [Theory]
    [InlineData("1007")]
    [InlineData("1007-03")]
    [InlineData("1007-03-15")]
    [InlineData("-300")]
    [InlineData("0")]
    [InlineData("854..914")]
    [InlineData("-100..0")]
    [InlineData("1007..")]
    [InlineData("..1007")]
    [InlineData("854-06..914-11-02")]
    public void Notation_round_trips_exactly(string notation)
    {
        Assert.True(WorldDate.TryParse(notation, out var date, out var error), error);
        Assert.NotNull(date);
        // A start-only value's interval-ness comes from context; supply it when the input had "..".
        var asInterval = notation.Contains("..");
        Assert.Equal(notation, date!.Value.ToNotation(asInterval));
    }

    [Fact]
    public void Empty_input_is_no_date_not_an_error()
    {
        Assert.True(WorldDate.TryParse("", out var date, out _));
        Assert.Null(date);
        Assert.True(WorldDate.TryParse("   ", out date, out _));
        Assert.Null(date);
        Assert.True(WorldDate.TryParse(null, out date, out _));
        Assert.Null(date);
    }

    [Fact]
    public void Bare_year_and_full_year_interval_are_different_claims()
    {
        // "1007" = event in 1007, month/day TBD. "1007..1007" = extent covering the year.
        Assert.True(WorldDate.TryParse("1007", out var eventDate, out _));
        Assert.True(WorldDate.TryParse("1007..1007", out var conditionDate, out _));
        Assert.Null(eventDate!.Value.End);
        Assert.NotNull(conditionDate!.Value.End);
        Assert.NotEqual(eventDate, conditionDate);
    }

    [Theory]
    [InlineData("914..854")]         // inverted interval — unrepresentable, not detectable
    [InlineData("1007-13")]          // month out of range
    [InlineData("1007-00")]
    [InlineData("1007-03-32")]       // day out of range
    [InlineData("..")]               // no endpoint at all
    [InlineData("sometime after the war")]
    [InlineData("300 BLB")]          // legacy-only form: NOT valid in the new notation
    public void Malformed_notation_is_rejected_with_an_error(string notation)
    {
        Assert.False(WorldDate.TryParse(notation, out var date, out var error));
        Assert.Null(date);
        Assert.NotEqual("", error);
    }

    [Fact]
    public void Legacy_hyphen_range_reads_as_iso_month_in_new_notation_and_errors_clearly()
    {
        // "870-928" was a RANGE in the legacy free text; in the new notation a hyphen after the
        // year means ISO month, and 928 is not a month. The error must steer to "..".
        Assert.False(WorldDate.TryParse("870-928", out _, out var error));
        Assert.Contains("..", error);
    }

    [Fact]
    public void Inverted_interval_is_unconstructible()
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            new WorldDate(new WorldDatePoint(914), new WorldDatePoint(854)));
    }

    [Fact]
    public void Day_requires_month()
    {
        Assert.ThrowsAny<ArgumentException>(() => new WorldDatePoint(1007, month: null, day: 5));
    }

    // ── Legacy converter: mechanical, flag-never-guess ──────────────────────────

    [Theory]
    [InlineData("993", 993)]
    [InlineData("-100", -100)]
    [InlineData("  978  ", 978)]
    [InlineData("998?", 998)]     // trailing '?' was a string-era approximation workaround
    [InlineData("-300?", -300)]
    [InlineData("300 BLB", -300)] // in-world calendar abbreviation normalises to negative
    public void Legacy_points_convert_at_year_precision(string raw, int year)
    {
        Assert.Equal(WorldDateLegacy.Outcome.Point, WorldDateLegacy.TryConvert(raw, out var date));
        Assert.Equal(year, date!.Value.Start!.Value.Year);
        Assert.Null(date.Value.Start.Value.Month); // year precision, never fabricated months
        Assert.Null(date.Value.End);
    }

    [Theory]
    [InlineData("870-928", 870, 928)]
    [InlineData("-100-0", -100, 0)]
    [InlineData("1007-1011", 1007, 1011)]
    public void Legacy_ranges_convert_to_intervals(string raw, int start, int end)
    {
        Assert.Equal(WorldDateLegacy.Outcome.Range, WorldDateLegacy.TryConvert(raw, out var date));
        Assert.Equal(start, date!.Value.Start!.Value.Year);
        Assert.Equal(end, date.Value.End!.Value.Year);
    }

    [Theory]
    [InlineData("?")]                       // a date-shaped claim with no usable content
    [InlineData("954-914")]                 // inverted — flag, never guess (real data artifact)
    [InlineData("sometime after the war")]
    public void Legacy_unconvertibles_are_flagged_never_guessed(string raw)
    {
        Assert.Equal(WorldDateLegacy.Outcome.Unconvertible, WorldDateLegacy.TryConvert(raw, out var date));
        Assert.Null(date);
    }

    [Fact]
    public void Legacy_empty_is_empty()
    {
        Assert.Equal(WorldDateLegacy.Outcome.Empty, WorldDateLegacy.TryConvert("", out _));
        Assert.Equal(WorldDateLegacy.Outcome.Empty, WorldDateLegacy.TryConvert(null, out _));
    }

    // ── Ordering fractions ──────────────────────────────────────────────────────

    [Fact]
    public void Fractions_order_progressive_precision_monotonically()
    {
        var year = new WorldDatePoint(1007);
        var march = new WorldDatePoint(1007, 3);
        var midMarch = new WorldDatePoint(1007, 3, 15);

        Assert.True(year.EarliestFraction <= march.EarliestFraction);
        Assert.True(march.EarliestFraction <= midMarch.EarliestFraction);
        Assert.True(midMarch.LatestFraction <= march.LatestFraction);
        Assert.True(march.LatestFraction <= year.LatestFraction);
        Assert.True(year.LatestFraction - year.EarliestFraction > 0.99); // a whole year wide
    }

    // ── Era derivation ──────────────────────────────────────────────────────────

    [Fact]
    public void N_pivots_derive_n_plus_one_eras_with_no_overlap_or_gap()
    {
        var pivots = new[]
        {
            new Pivot { Year = 930 }, new Pivot { Year = 870 }, // deliberately unsorted
            new Pivot { Year = 978 }, new Pivot { Year = 1006 }, new Pivot { Year = 1011 },
        };
        var eras = Eras.FromPivots(pivots);

        Assert.Equal(6, eras.Count);
        Assert.Null(eras[0].StartYear);
        Assert.Equal(870, eras[0].EndYear);
        Assert.Equal((870, 930), (eras[1].StartYear, eras[1].EndYear));
        Assert.Equal((1011, (int?)null), (eras[5].StartYear, eras[5].EndYear));

        // Every year lands in exactly one era — overlap and gaps are structurally impossible.
        foreach (var y in new[] { -400.0, 869.9, 870.0, 1010.99, 1011.0, 2000.0 })
            Assert.Equal(1, eras.Count(e => e.Contains(y)));
    }

    [Fact]
    public void Zero_pivots_is_one_all_of_time_era_and_duplicates_collapse()
    {
        Assert.Single(Eras.FromPivots(Array.Empty<Pivot>()));
        Assert.Equal(2, Eras.FromPivots(new[] { new Pivot { Year = 870 }, new Pivot { Year = 870 } }).Count);
    }

    // ── Lane packing ────────────────────────────────────────────────────────────

    [Fact]
    public void Lane_count_equals_maximum_concurrency()
    {
        // Three overlapping at once (max concurrency 3), then a disjoint pair.
        double[] tops = { 0, 10, 20, 100, 110 };
        double[] bottoms = { 50, 60, 30, 120, 130 };
        var (lanes, count) = LanePacker.Pack(tops, bottoms);

        Assert.Equal(3, count);
        // Overlapping items never share a lane.
        for (var i = 0; i < tops.Length; i++)
            for (var j = i + 1; j < tops.Length; j++)
                if (tops[i] < bottoms[j] && tops[j] < bottoms[i])
                    Assert.NotEqual(lanes[i], lanes[j]);
    }

    [Fact]
    public void Touching_edges_do_not_conflict_and_reuse_the_first_lane()
    {
        double[] tops = { 0, 50 };
        double[] bottoms = { 50, 90 };
        var (lanes, count) = LanePacker.Pack(tops, bottoms);
        Assert.Equal(1, count);
        Assert.Equal(lanes[0], lanes[1]);
    }

    // ── YearAxis: linear by default, explicitly compressed where asked ──────────

    [Fact]
    public void Axis_with_no_collapse_is_plain_linear()
    {
        var axis = YearAxis.Build(900, 1000, pixelsPerYear: 10);
        Assert.Equal(0, axis.YOf(900));
        Assert.Equal(500, axis.YOf(950));
        Assert.Equal(1000, axis.YOf(1000));
        Assert.Equal(1000, axis.Height);
    }

    [Fact]
    public void Collapsed_range_compresses_to_a_fixed_height_and_shifts_everything_after_it()
    {
        // 400..900 collapsed to 26px instead of 5000px; 900..1000 stays linear.
        var axis = YearAxis.Build(400, 1000, 10, [(400, 900)], collapsedHeight: 26);

        Assert.Equal(0, axis.YOf(400));
        Assert.Equal(26, axis.YOf(900), 3);
        Assert.Equal(1026, axis.YOf(1000), 3);
        Assert.Equal(1026, axis.Height, 3);
        Assert.True(axis.IsCollapsedAt(650));
        Assert.False(axis.IsCollapsedAt(950));
    }

    [Fact]
    public void Mapping_stays_monotonic_inside_a_collapsed_range()
    {
        // Compression, never deletion: relative order survives even when squeezed.
        var axis = YearAxis.Build(0, 1000, 10, [(0, 900)], collapsedHeight: 26);
        var prev = double.NegativeInfinity;
        foreach (var year in new[] { 0, 100, 300, 600, 899, 900, 950, 1000 })
        {
            var y = axis.YOf(year);
            Assert.True(y >= prev, $"year {year} mapped to {y}, behind previous {prev}");
            prev = y;
        }
    }

    [Fact]
    public void Overlapping_collapsed_ranges_merge_so_no_year_compresses_twice()
    {
        var merged = YearAxis.Build(0, 1000, 10, [(100, 400), (300, 600)], collapsedHeight: 20);
        var single = YearAxis.Build(0, 1000, 10, [(100, 600)], collapsedHeight: 20);
        Assert.Equal(single.Height, merged.Height, 3);
        Assert.Single(merged.CollapsedBands());
    }

    [Fact]
    public void Collapsed_ranges_are_clipped_to_the_axis_and_out_of_range_ones_ignored()
    {
        var axis = YearAxis.Build(900, 1000, 10, [(500, 950), (2000, 3000)], collapsedHeight: 20);
        Assert.Single(axis.CollapsedBands());
        var band = axis.CollapsedBands().Single();
        Assert.Equal(900, band.FromYear);   // clipped up from 500
        Assert.Equal(950, band.ToYear);
    }

    [Fact]
    public void Years_outside_the_axis_clamp_rather_than_extrapolate()
    {
        var axis = YearAxis.Build(900, 1000, 10, top: 34);
        Assert.Equal(34, axis.YOf(500));
        Assert.Equal(34 + 1000, axis.YOf(5000));
    }

    // ── Fractional year → point, the mechanism behind drag-to-date ──────────────
    // Mirrors TimelineViewModel.PointAtFractionalYear. Kept here because the round trip through
    // WorldDatePoint's own 12x31 fraction grid is the part that must not drift: a drop must land
    // where the ghost promised.

    private static WorldDatePoint PointAt(double fractionalYear, bool month, bool day)
    {
        const double eps = 1e-6;
        var year = (int)Math.Floor(fractionalYear);
        var frac = fractionalYear - year;
        if (!month) return new WorldDatePoint(year);
        var m = Math.Clamp((int)Math.Floor(frac * 12 + eps) + 1, 1, 12);
        if (!day) return new WorldDatePoint(year, m);
        var withinMonth = frac - (m - 1) / 12.0;
        var d = Math.Clamp((int)Math.Floor(withinMonth * 372 + eps) + 1, 1, 31);
        return new WorldDatePoint(year, m, d);
    }

    [Theory]
    [InlineData(1007.0, 1)]
    [InlineData(1007.5, 7)]
    [InlineData(1007.99, 12)]
    public void Dropping_inside_a_year_picks_the_month_that_position_falls_in(double fy, int expectedMonth)
    {
        var p = PointAt(fy, month: true, day: false);
        Assert.Equal(1007, p.Year);
        Assert.Equal(expectedMonth, p.Month);
        Assert.Null(p.Day);
    }

    [Fact]
    public void A_dropped_point_maps_back_to_the_pixel_it_was_dropped_near()
    {
        // The ghost snaps the rule to PixelForPoint; that must land inside the span the user
        // aimed at, or the preview would promise a position the write doesn't honour.
        var axis = YearAxis.Build(1000, 1010, pixelsPerYear: 280);
        const double dropped = 1007.45;
        var p = PointAt(dropped, month: true, day: false);
        var snapped = axis.YOf(p.EarliestFraction);
        var raw = axis.YOf(dropped);
        Assert.True(Math.Abs(snapped - raw) <= 280.0 / 12, "snap moved further than one month");
        Assert.True(snapped <= raw, "a month's start is never below the point inside it");
    }

    [Fact]
    public void Day_precision_round_trips_through_the_same_grid_the_point_type_uses()
    {
        var original = new WorldDatePoint(1007, 6, 15);
        var recovered = PointAt(original.EarliestFraction, month: true, day: true);
        Assert.Equal(original, recovered);
    }

    [Fact]
    public void Negative_years_drop_into_the_year_below_the_boundary()
    {
        // -100.6 sits inside year -101, not -100: floor, never round, or a BLB drop would jump
        // a year forward.
        var p = PointAt(-100.6, month: true, day: false);
        Assert.Equal(-101, p.Year);
        Assert.Equal(5, p.Month);
    }
}
