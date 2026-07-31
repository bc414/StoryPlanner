using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowedStoryPlanner.ViewModels;
using CoreWorldDate = StoryPlanner.Core.WorldDate;

namespace WindowedStoryPlanner.Views;

/// <summary>
/// The full-fidelity second path to a note's world date: discrete Start/End year-month-day
/// fields in a popup, alongside the compact notation TextBox. Both paths converge on ONE write:
/// this control composes the notation string and assigns <see cref="NoteViewModel.WorldDate"/>,
/// so every rule the compact field enforces (parse validation, inverted-interval rejection,
/// interval-on-event-track rejection, legacy-string blanking) applies identically here — there
/// is no second validator to drift.
/// </summary>
public partial class WorldDatePickerControl : UserControl
{
    /// <summary>
    /// The note to edit, passed explicitly rather than inherited from DataContext so the control
    /// works inside a timeline card, whose DataContext is a display object rather than a note.
    /// </summary>
    public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
        nameof(Note), typeof(NoteViewModel), typeof(WorldDatePickerControl), new PropertyMetadata(null));

    public NoteViewModel? Note
    {
        get => (NoteViewModel?)GetValue(NoteProperty);
        set => SetValue(NoteProperty, value);
    }

    /// <summary>
    /// Optional: invoked after a successful Apply or Clear. NoteView leaves this unset — there,
    /// saving is the track section's job. The timeline sets it, because an edit made from a card
    /// has to persist and re-place the mark itself.
    /// </summary>
    public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
        nameof(SaveCommand), typeof(ICommand), typeof(WorldDatePickerControl), new PropertyMetadata(null));

    public ICommand? SaveCommand
    {
        get => (ICommand?)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public WorldDatePickerControl()
    {
        InitializeComponent();
    }

    private NoteViewModel? Vm => Note ?? DataContext as NoteViewModel;

    private void CommitSave()
    {
        if (SaveCommand?.CanExecute(null) == true) SaveCommand.Execute(null);
    }

    private void Popup_Opened(object sender, System.EventArgs e)
    {
        // Prefill from the current value by parsing the same notation the compact field shows.
        StartYear.Text = StartMonth.Text = StartDay.Text = EndYear.Text = EndMonth.Text = EndDay.Text = "";
        if (Vm is { } vm && CoreWorldDate.TryParse(vm.WorldDate, out var date, out _) && date is { } d)
        {
            if (d.Start is { } s)
            {
                StartYear.Text = s.Year.ToString();
                StartMonth.Text = s.Month?.ToString() ?? "";
                StartDay.Text = s.Day?.ToString() ?? "";
            }
            if (d.End is { } end)
            {
                EndYear.Text = end.Year.ToString();
                EndMonth.Text = end.Month?.ToString() ?? "";
                EndDay.Text = end.Day?.ToString() ?? "";
            }
        }
        UpdatePreview();
    }

    private void AnyPart_TextChanged(object sender, TextChangedEventArgs e) => UpdatePreview();

    private void UpdatePreview()
    {
        if (Preview is null) return; // during InitializeComponent
        var (notation, error) = Compose();
        if (error is not null)
        {
            Preview.Text = error;
            ApplyBtn.IsEnabled = false;
            return;
        }
        if (notation.Length == 0)
        {
            Preview.Text = "→ no date (undated is a valid state)";
            ApplyBtn.IsEnabled = true;
            return;
        }
        if (!CoreWorldDate.TryParse(notation, out _, out var parseError))
        {
            Preview.Text = parseError;
            ApplyBtn.IsEnabled = false;
            return;
        }
        Preview.Text = $"→ {notation}";
        ApplyBtn.IsEnabled = true;
    }

    /// <summary>Composes the notation from the six boxes; error text instead when the shape is
    /// impossible (month without year, etc.) so the parser never sees garbage it would blame
    /// on syntax.</summary>
    private (string Notation, string? Error) Compose()
    {
        var (start, startErr) = ComposePoint(StartYear.Text, StartMonth.Text, StartDay.Text, "start");
        if (startErr is not null) return ("", startErr);
        var (end, endErr) = ComposePoint(EndYear.Text, EndMonth.Text, EndDay.Text, "end");
        if (endErr is not null) return ("", endErr);

        var isCondition = Vm?.SupportsWorldDateEnd ?? false;
        if (start.Length == 0 && end.Length == 0) return ("", null);
        if (!isCondition) return (start, null); // event track: end boxes aren't even visible

        var sb = new StringBuilder(start).Append("..").Append(end);
        return (sb.ToString(), null);
    }

    private static (string Point, string? Error) ComposePoint(string year, string month, string day, string which)
    {
        year = year.Trim(); month = month.Trim(); day = day.Trim();
        if (year.Length == 0)
        {
            if (month.Length > 0 || day.Length > 0)
                return ("", $"The {which} needs a year first — year is the precision floor.");
            return ("", null);
        }
        if (!int.TryParse(year, out var y))
            return ("", $"'{year}' is not a year (negative = BLB).");
        if (month.Length == 0)
        {
            if (day.Length > 0)
                return ("", $"The {which} day needs a month first.");
            return (y.ToString(), null);
        }
        if (!int.TryParse(month, out var m))
            return ("", $"'{month}' is not a month.");
        if (day.Length == 0)
            return ($"{y}-{m:00}", null);
        if (!int.TryParse(day, out var d))
            return ("", $"'{day}' is not a day.");
        return ($"{y}-{m:00}-{d:00}", null);
    }

    private void Apply_Click(object sender, RoutedEventArgs e)
    {
        if (Vm is not { } vm) return;
        var (notation, error) = Compose();
        if (error is not null) return; // button disabled anyway; belt and braces
        vm.WorldDate = notation;       // the ONE write path — same as typing in the compact field
        if (!vm.HasWorldDateError)
        {
            OpenToggle.IsChecked = false;
            CommitSave();
        }
        else
            Preview.Text = vm.WorldDateError; // e.g. interval on an event track
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        if (Vm is { } vm) vm.WorldDate = "";
        OpenToggle.IsChecked = false;
        CommitSave();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => OpenToggle.IsChecked = false;
}
