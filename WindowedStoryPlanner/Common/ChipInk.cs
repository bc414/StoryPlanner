using System.Windows.Media;

namespace WindowedStoryPlanner;

/// <summary>
/// Turns an authored "#RRGGBB" into the brushes a filled value chip needs.
///
/// The ink colour is DERIVED from the fill rather than authored alongside it: the colour field
/// exists so Brian can pick hues that mean something to him, and making him also pick a readable
/// text colour for each one would be busywork with an obvious right answer. Relative luminance
/// per WCAG, thresholded once — no gradient, no ranking, nothing configurable.
///
/// An empty or unparseable hex is a legal, visibly-unfinished state, not an error: it falls back
/// to the same neutral the Story and Subject colour columns already use.
/// </summary>
public static class ChipInk
{
    /// <summary>The neutral used by StoryViewModel and SubjectViewModel for an unset colour.</summary>
    public const string NeutralHex = "#CCCCCC";

    private static readonly Brush Neutral = Freeze(new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)));
    private static readonly Brush DarkInk = Freeze(new SolidColorBrush(Color.FromRgb(0x22, 0x22, 0x22)));
    private static readonly Brush LightInk = Freeze(new SolidColorBrush(Colors.White));

    /// <summary>Fill for a chip or swatch. Empty/unparseable → the neutral.</summary>
    public static Brush FillBrush(string? colorHex) =>
        TryParse(colorHex, out var color) ? Freeze(new SolidColorBrush(color)) : Neutral;

    /// <summary>Text colour that stays legible on <paramref name="colorHex"/>.</summary>
    public static Brush InkBrush(string? colorHex) =>
        TryParse(colorHex, out var color) && RelativeLuminance(color) < 0.5 ? LightInk : DarkInk;

    private static bool TryParse(string? colorHex, out Color color)
    {
        color = default;
        if (string.IsNullOrWhiteSpace(colorHex)) return false;

        try
        {
            // ColorConverter throws rather than returning null on malformed input, and a colour
            // typed one character at a time is malformed most of the way through.
            if (ColorConverter.ConvertFromString(colorHex.Trim()) is Color parsed)
            {
                color = parsed;
                return true;
            }
        }
        catch (System.FormatException) { }
        catch (System.ArgumentException) { }

        return false;
    }

    private static double RelativeLuminance(Color c)
    {
        static double Channel(byte v)
        {
            var s = v / 255.0;
            return s <= 0.03928 ? s / 12.92 : System.Math.Pow((s + 0.055) / 1.055, 2.4);
        }

        return 0.2126 * Channel(c.R) + 0.7152 * Channel(c.G) + 0.0722 * Channel(c.B);
    }

    private static Brush Freeze(SolidColorBrush brush)
    {
        // Frozen brushes are shareable across threads and cheaper to render; these are created
        // per cell across ten grids, so it is worth the one call.
        brush.Freeze();
        return brush;
    }
}
