using System.Text.Json;

namespace StoryPlanner.Core;

/// <summary>
/// The timeline viewport state persisted in the UiSettings table (one row, keyed by
/// <see cref="UiSettingKey"/>). Every field is optional: a missing row, an unparseable
/// payload, or a null field all mean "use the built-in default" — restore must never
/// fail an open. Stale theater ids / era keys in the collapse sets are inert (the
/// timeline just never matches them), so no validation happens here.
/// </summary>
public class TimelineViewState
{
    public const string UiSettingKey = "Timeline.ViewState";

    public double? PixelsPerYear { get; set; }
    public double? CenterYear { get; set; }
    public List<int> CollapsedTheaters { get; set; } = new();
    public List<string> CollapsedEras { get; set; } = new();

    public string Serialize() => JsonSerializer.Serialize(this);

    /// <summary>Null on null/empty/corrupt input — callers fall back to defaults.</summary>
    public static TimelineViewState? TryDeserialize(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            return JsonSerializer.Deserialize<TimelineViewState>(json);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
