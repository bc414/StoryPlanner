using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// TimelineViewState is the payload of the timeline's UiSettings row. The contract under
/// test: round-trips are lossless, and every bad input (missing row → null, corrupt JSON)
/// yields null so the timeline falls back to defaults instead of failing the open.
/// </summary>
public class TimelineViewStateTests
{
    [Fact]
    public void Round_trips_all_fields()
    {
        var state = new TimelineViewState
        {
            PixelsPerYear = 18.5,
            CenterYear = 1007.25,
            CollapsedTheaters = [3, 7],
            CollapsedEras = ["-400..0", "854..914"],
        };

        var restored = TimelineViewState.TryDeserialize(state.Serialize());

        Assert.NotNull(restored);
        Assert.Equal(18.5, restored.PixelsPerYear);
        Assert.Equal(1007.25, restored.CenterYear);
        Assert.Equal([3, 7], restored.CollapsedTheaters);
        Assert.Equal(["-400..0", "854..914"], restored.CollapsedEras);
    }

    [Fact]
    public void Null_fields_round_trip_as_null()
    {
        var restored = TimelineViewState.TryDeserialize(new TimelineViewState().Serialize());

        Assert.NotNull(restored);
        Assert.Null(restored.PixelsPerYear);
        Assert.Null(restored.CenterYear);
        Assert.Empty(restored.CollapsedTheaters);
        Assert.Empty(restored.CollapsedEras);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not json at all")]
    [InlineData("{ \"PixelsPerYear\": ")]        // truncated
    [InlineData("{ \"PixelsPerYear\": \"12\" }")] // wrong type for the field
    public void Bad_input_yields_null_never_throws(string? json)
    {
        Assert.Null(TimelineViewState.TryDeserialize(json));
    }

    [Fact]
    public void Unknown_properties_are_ignored()
    {
        // A payload written by a future version with extra fields must still restore.
        var restored = TimelineViewState.TryDeserialize(
            """{ "PixelsPerYear": 12, "SomeFutureField": true }""");

        Assert.NotNull(restored);
        Assert.Equal(12, restored.PixelsPerYear);
    }
}
