using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindowedStoryPlanner;

/// <summary>
/// The point-and-click path to an authored "#RRGGBB". The problem it solves is narrow and worth
/// stating: typing a hex is a guess that only resolves after the field loses focus, so the colour
/// could not be seen before it was committed. Here every choice is visible as a swatch, and the
/// preview bar shows the pending colour carrying its own hex in ChipInk's derived ink.
///
/// The swatches are SAMPLED, not chosen — regular steps through HSV (see <see cref="BuildHues"/>).
/// No colour is recommended over another, and the hex box reaches anything between lattice points,
/// so the palette is a convenience and never a proposal.
///
/// One write path: clicking a swatch fills the hex box, the hex box drives the preview, and Apply
/// hands the hex box back to <see cref="ColorPickerControl"/>, which performs the single write to
/// <see cref="IColorHexOwner.ColorHex"/>. Parsing is <see cref="ChipInk.TryParseColor"/> throughout —
/// no second validator.
///
/// A modal Window rather than a Popup, and the reason is load-bearing: two of the three hosts are
/// DataGrid cells. Routed events raised inside a Popup travel back up the LOGICAL tree — through
/// the Popup, into the DataGridCell and DataGrid — so the grid's own mouse and key handling sits
/// in the path, and in practice every control inside the popup went inert: swatch clicks, the hex
/// TextBox, and all three buttons. A Window is its own top-level element with its own routing and
/// cannot be reached by that. (WorldDatePickerControl keeps its Popup because no DataGrid hosts it.)
/// </summary>
public partial class ColorPickerWindow : Window
{
    /// <summary>The hex to write when <see cref="ShowDialog"/> returned true. "" means cleared.</summary>
    public string ResultHex { get; private set; } = "";

    public ColorPickerWindow(string currentHex)
    {
        InitializeComponent();

        HueSwatches.ItemsSource = Hues;
        NeutralSwatches.ItemsSource = Neutrals;

        // Prefill verbatim: an unparseable authored string is shown rather than discarded, because
        // silently losing what Brian typed is worse than showing something the picker can't render.
        HexBox.Text = currentHex;
        UpdatePreview();

        Loaded += (_, _) => { HexBox.Focus(); HexBox.SelectAll(); };
    }

    // ---- The palette ------------------------------------------------------------------------

    private static readonly IReadOnlyList<string> Hues = BuildHues();

    /// <summary>Neutrals get their own row: they are the S=0 column of the same lattice, where hue
    /// is meaningless, so spreading them across twelve identical hue slots would be noise.</summary>
    private static readonly IReadOnlyList<string> Neutrals = new[]
    {
        "#FFFFFF", "#E0E0E0", "#C0C0C0", "#999999", "#666666", "#333333", "#000000",
    };

    /// <summary>
    /// Twelve hues at 30° steps × five saturation/value rows: pale, soft, pure, deep, dark.
    /// A regular sampling of HSV — the grid is generated so that no swatch reflects a judgment
    /// about which colours are good ones.
    /// </summary>
    private static string[] BuildHues()
    {
        var rows = new (double S, double V)[]
        {
            (0.25, 1.00), // pale
            (0.50, 1.00), // soft
            (1.00, 1.00), // pure
            (1.00, 0.75), // deep
            (1.00, 0.50), // dark
        };

        var result = new string[rows.Length * 12];
        var i = 0;
        foreach (var (s, v) in rows)
            for (var h = 0; h < 12; h++)
                result[i++] = FromHsv(h * 30.0, s, v);

        return result;
    }

    /// <summary>HSV→RGB. Only this direction is needed: the reverse exists solely to place a
    /// draggable thumb, and this picker has none.</summary>
    private static string FromHsv(double hueDegrees, double saturation, double value)
    {
        var c = value * saturation;
        var sector = hueDegrees / 60.0;
        var x = c * (1 - Math.Abs(sector % 2 - 1));
        var m = value - c;

        var (r, g, b) = (int)sector switch
        {
            0 => (c, x, 0.0),
            1 => (x, c, 0.0),
            2 => (0.0, c, x),
            3 => (0.0, x, c),
            4 => (x, 0.0, c),
            _ => (c, 0.0, x),
        };

        static byte Channel(double v, double m) => (byte)Math.Round(Math.Clamp(v + m, 0, 1) * 255);
        return $"#{Channel(r, m):X2}{Channel(g, m):X2}{Channel(b, m):X2}";
    }

    internal static string Normalize(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

    // ---- Editing ----------------------------------------------------------------------------

    private void Swatch_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string hex }) return;

        HexBox.Text = hex; // the ONE path — the swatch fills the box, the box drives everything else
        HexBox.SelectAll();
    }

    private void Hex_TextChanged(object sender, TextChangedEventArgs e) => UpdatePreview();

    private void UpdatePreview()
    {
        if (PreviewSwatch is null) return; // during InitializeComponent

        var text = HexBox.Text.Trim();

        if (text.Length == 0)
        {
            PreviewSwatch.Background = ChipInk.FillBrush(null);
            PreviewText.Text = "";
            Hint.Text = "No colour — pick a swatch, or leave empty. Empty is a legal, unfinished state.";
            ApplyBtn.IsEnabled = true;
            return;
        }

        if (!ChipInk.TryParseColor(text, out var color))
        {
            PreviewSwatch.Background = ChipInk.FillBrush(null);
            PreviewText.Text = "";
            Hint.Text = $"'{text}' is not a colour yet.";
            ApplyBtn.IsEnabled = false;
            return;
        }

        var normalized = Normalize(color);
        PreviewSwatch.Background = ChipInk.FillBrush(normalized);
        PreviewText.Text = normalized;
        PreviewText.Foreground = ChipInk.InkBrush(normalized);
        Hint.Text = "";
        ApplyBtn.IsEnabled = true;
    }

    private void Apply_Click(object sender, RoutedEventArgs e)
    {
        var text = HexBox.Text.Trim();
        if (text.Length == 0)
        {
            ResultHex = "";
        }
        else
        {
            if (!ChipInk.TryParseColor(text, out var color)) return; // button is disabled anyway
            ResultHex = Normalize(color); // alpha is dropped on write; the field is #RRGGBB
        }

        DialogResult = true;
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        ResultHex = "";
        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
