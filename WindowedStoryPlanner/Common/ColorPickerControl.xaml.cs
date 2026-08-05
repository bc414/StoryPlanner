using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowedStoryPlanner;

/// <summary>
/// The colour field's face wherever one is edited: a swatch and its hex, clickable, opening
/// <see cref="ColorPickerWindow"/>. Used by the Story library grid, the allowed-value grid, and
/// the subject widget, so all three colour fields are authored the same way.
///
/// Everything interesting lives in the window; this is deliberately a button and a write. The
/// target is read at click time and passed nowhere, which is also why DataGrid row recycling
/// cannot misdirect the write: the dialog is modal, so the row cannot be reused mid-edit.
/// </summary>
public partial class ColorPickerControl : UserControl
{
    /// <summary>
    /// The entity whose colour this edits, passed explicitly rather than inherited from DataContext.
    /// There is deliberately NO `Target ?? DataContext as IColorHexOwner` fallback: inside a
    /// virtualized DataGrid a fallback would silently work, hiding an unset binding.
    /// </summary>
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
        nameof(Target), typeof(IColorHexOwner), typeof(ColorPickerControl), new PropertyMetadata(null));

    public IColorHexOwner? Target
    {
        get => (IColorHexOwner?)GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    /// <summary>
    /// Optional: invoked after a successful Apply or Clear. All three current hosts set it — the
    /// Stories grid in particular had no other save, so a colour lived only in memory until exit.
    /// </summary>
    public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
        nameof(SaveCommand), typeof(ICommand), typeof(ColorPickerControl), new PropertyMetadata(null));

    public ICommand? SaveCommand
    {
        get => (ICommand?)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public ColorPickerControl()
    {
        InitializeComponent();
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        if (Target is not { } target) return;

        var dialog = new ColorPickerWindow(target.ColorHex) { Owner = Window.GetWindow(this) };
        if (dialog.ShowDialog() != true) return;

        target.ColorHex = dialog.ResultHex; // the ONE write — same setter the field always had
        if (SaveCommand?.CanExecute(null) == true) SaveCommand.Execute(null);
    }
}
