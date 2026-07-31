using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The two subtle rules in world-date range intersection: inclusive-year-in / exclusive-edge-out,
/// and the event-vs-condition meaning of a start-only date. Both are shared by the MCP server's
/// get_notes_in_date_range and the app's date-range window, so a drift between them shows up
/// here first. Pure — no .storyplan, no DbContext.
/// </summary>
public class WorldDateRangeTests
{
    private static WorldDate Parse(string notation)
    {
        Assert.True(WorldDate.TryParse(notation, out var d, out var error), error);
        return d!.Value;
    }

    private static WorldDateRange Range(string notation)
    {
        Assert.True(WorldDate.TryParse(notation, out var d, out var error), error);
        return WorldDateRange.FromBound(d, asInterval: notation.Contains(".."));
    }

    // ── Inclusive year in, exclusive edge out ───────────────────────────────────

    [Theory]
    [InlineData("1007", "1007..1007", true)]   // the named year is inside its own range
    [InlineData("1007", "1007", true)]         // a bare-year range is that year
    [InlineData("1007", "1008..", false)]      // ...and stops before the next one
    [InlineData("1007", "..1006", false)]
    [InlineData("1007", "..1007", true)]       // upper bound is inclusive of the whole year
    [InlineData("1007", "854..914", false)]
    [InlineData("914", "854..914", true)]
    [InlineData("854", "854..914", true)]
    [InlineData("853", "854..914", false)]
    [InlineData("915", "854..914", false)]
    public void Event_dates_intersect_year_ranges_at_the_right_edges(string date, string range, bool expected) =>
        Assert.Equal(expected, Range(range).Overlaps(Parse(date), isConditionTrack: false));

    [Fact]
    public void FromYears_matches_the_notation_form()
    {
        // The MCP tool takes ints; the window takes notation. They must agree.
        Assert.Equal(Range("854..914"), WorldDateRange.FromYears(854, 914));
        Assert.Equal(Range("854.."), WorldDateRange.FromYears(854, null));
        Assert.Equal(Range("..914"), WorldDateRange.FromYears(null, 914));
        Assert.Equal(WorldDateRange.All, WorldDateRange.FromYears(null, null));
    }

    [Fact]
    public void Empty_input_is_the_whole_timeline()
    {
        Assert.True(WorldDate.TryParse("", out var none, out _));
        Assert.Null(none);
        var all = WorldDateRange.FromBound(none, asInterval: false);
        Assert.True(all.IsUnbounded);
        Assert.True(all.Overlaps(Parse("-300"), isConditionTrack: false));
        Assert.True(all.Overlaps(Parse("9999"), isConditionTrack: false));
    }

    // ── The event/condition split ───────────────────────────────────────────────

    [Fact]
    public void Start_only_on_a_condition_track_stays_in_force_indefinitely()
    {
        var inForceSince1007 = Parse("1007");

        Assert.True(Range("1500..1600").Overlaps(inForceSince1007, isConditionTrack: true));
        Assert.True(Range("1007..1007").Overlaps(inForceSince1007, isConditionTrack: true));
        Assert.False(Range("..1006").Overlaps(inForceSince1007, isConditionTrack: true));
    }

    [Fact]
    public void The_same_stored_value_on_an_event_track_does_not()
    {
        var happenedIn1007 = Parse("1007");

        Assert.False(Range("1500..1600").Overlaps(happenedIn1007, isConditionTrack: false));
        Assert.True(Range("1007..1007").Overlaps(happenedIn1007, isConditionTrack: false));
    }

    [Fact]
    public void A_closed_interval_ignores_the_track_shape()
    {
        // Once an end is stored there is nothing left for the track to disambiguate.
        var span = Parse("854..914");
        foreach (var isCondition in new[] { true, false })
        {
            Assert.True(Range("900..1000").Overlaps(span, isCondition));
            Assert.False(Range("915..").Overlaps(span, isCondition));
            Assert.False(Range("..853").Overlaps(span, isCondition));
        }
    }

    [Fact]
    public void A_TBD_start_reaches_back_indefinitely()
    {
        // "..914" is unknown-start, not absent-start: it can touch any earlier range.
        var endsIn914 = Parse("..914");
        Assert.True(Range("100..200").Overlaps(endsIn914, isConditionTrack: true));
        Assert.False(Range("915..").Overlaps(endsIn914, isConditionTrack: true));
    }

    // ── Sub-year precision at the boundaries ────────────────────────────────────

    [Fact]
    public void Month_precision_narrows_the_span_within_the_year()
    {
        var march1007 = Parse("1007-03");
        Assert.True(Range("1007").Overlaps(march1007, isConditionTrack: false));

        var (earliest, latest) = WorldDateRange.Span(march1007, isConditionTrack: false);
        Assert.True(earliest > 1007.0);   // not the start of the year
        Assert.True(latest < 1008.0);     // nor the end of it
    }

    [Fact]
    public void A_dated_month_range_excludes_a_month_outside_it()
    {
        var june = Parse("1007-06");
        Assert.True(Range("1007-06..1007-08").Overlaps(june, isConditionTrack: false));
        Assert.False(Range("1007-07..1007-08").Overlaps(june, isConditionTrack: false));
    }

    // ── Legacy free text reaches the same span ──────────────────────────────────

    [Theory]
    [InlineData("870-928", 870, 928)]   // legacy hyphen is a RANGE separator, never ISO month
    [InlineData("993", 993, 993)]
    [InlineData("300 BLB", -300, -300)]
    [InlineData("998?", 998, 998)]      // trailing '?' was the string era's precision workaround
    public void Legacy_values_convert_to_the_same_range_arithmetic(string raw, int lo, int hi)
    {
        var note = new Note { WorldDate = raw };
        var date = note.EffectiveWorldDate();
        Assert.NotNull(date);

        Assert.True(WorldDateRange.FromYears(lo, hi).Overlaps(date!.Value, isConditionTrack: false));
        Assert.False(WorldDateRange.FromYears(hi + 1, null).Overlaps(date.Value, isConditionTrack: false));
    }

    [Theory]
    [InlineData("?")]
    [InlineData("954-914")]        // inverted — flag, never guess
    [InlineData("sometime before the war")]
    public void Unconvertible_legacy_text_has_no_date_rather_than_a_guessed_one(string raw)
    {
        var note = new Note { WorldDate = raw };
        Assert.Null(note.EffectiveWorldDate());
        Assert.True(note.HasAnyWorldDate());   // still a date-shaped claim, just not a usable one
    }

    [Fact]
    public void Structured_columns_win_over_leftover_legacy_text()
    {
        var note = new Note { WorldDate = "993" };
        note.SetWorldDate(new WorldDate(new WorldDatePoint(1007)));

        Assert.Equal(1007, note.EffectiveWorldDate()!.Value.Start!.Value.Year);
    }

    [Fact]
    public void An_undated_note_has_no_date_signal_at_all()
    {
        var note = new Note();
        Assert.False(note.HasAnyWorldDate());
        Assert.Null(note.EffectiveWorldDate());
    }
}
