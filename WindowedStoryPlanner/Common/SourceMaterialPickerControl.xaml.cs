using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WindowedStoryPlanner;

/// <summary>
/// Multi-chip source-material citation picker. Chips are the note's existing
/// NoteSourceReferences (rendered from Note.SourceReferences); the "+" opens a popup offering a
/// combined Work/Part search plus an inline quick-add form. Follows WorldDatePickerControl's
/// shape — a Note DP pointing at the whole NoteViewModel, so the control can host outside
/// NoteView too — rather than a plain value DP, because add/remove/create are all mutations
/// that belong on the view model, not on this control.
/// </summary>
public partial class SourceMaterialPickerControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // ── Note DP ──────────────────────────────────────────────────────────────

    public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
        nameof(Note), typeof(NoteViewModel), typeof(SourceMaterialPickerControl), new PropertyMetadata(null));

    public NoteViewModel? Note
    {
        get => (NoteViewModel?)GetValue(NoteProperty);
        set => SetValue(NoteProperty, value);
    }

    private NoteViewModel? Vm => Note ?? DataContext as NoteViewModel;

    // ── Search results ───────────────────────────────────────────────────────

    private sealed class SearchResult
    {
        public required SourceMaterialViewModel Work { get; init; }
        public SourceMaterialPartViewModel? Part { get; init; }
        public required string Label { get; init; }
    }

    private List<SearchResult> _results = [];
    public bool HasResults => _results.Count > 0;

    // ── Popup lifecycle ──────────────────────────────────────────────────────

    private void Popup_Opened(object sender, System.EventArgs e)
    {
        SearchBox.Text = string.Empty;
        AddNewForm.Visibility = Visibility.Collapsed;
        WorkCombo.SelectedItem = null;
        WorkCombo.Text = string.Empty;
        PartCodeBox.Text = string.Empty;
        PartNameBox.Text = string.Empty;
        RebuildResults();
        SearchBox.Focus();
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => RebuildResults();

    private void RebuildResults()
    {
        var query = SearchBox.Text.Trim();
        if (Vm is null || query.Length == 0)
        {
            _results = [];
        }
        else
        {
            var lower = query.ToLowerInvariant();

            var workHits = Vm.AvailableSourceMaterials
                .Where(w => w.Name.ToLowerInvariant().Contains(lower))
                .OrderBy(w => w.Name)
                .Select(w => new SearchResult { Work = w, Part = null, Label = w.Name });

            var partHits = Vm.AvailableSourceMaterialParts
                .Where(p => p.Code.ToLowerInvariant().Contains(lower) || p.Name.ToLowerInvariant().Contains(lower))
                .OrderBy(p => p.OrderIndex)
                .Select(p =>
                {
                    var work = Vm.AvailableSourceMaterials.FirstOrDefault(w => w.Id == p.SourceMaterialId);
                    return work is null
                        ? null
                        : new SearchResult { Work = work, Part = p, Label = $"{work.Name} · {p.DisplayLabel}" };
                })
                .OfType<SearchResult>();

            _results = workHits.Concat(partHits).ToList();
        }

        ResultsList.ItemsSource = _results;
        Notify(nameof(HasResults));

        // Pre-fill the quick-add Part code with whatever was typed — the common quick-add case
        // is "this episode isn't in the seeded list yet", where the search text already IS the
        // code the author wants.
        PartCodeBox.Text = query;
    }

    private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ResultsList.SelectedItem is not SearchResult result || Vm is null) return;
        ResultsList.SelectedItem = null;
        Vm.AddSourceReference(result.Work, result.Part);
        OpenToggle.IsChecked = false;
    }

    private void RemoveChip_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: NoteSourceReferenceViewModel reference } || Vm is null) return;
        Vm.RemoveSourceReference(reference);
    }

    // ── Quick add ────────────────────────────────────────────────────────────

    private void ShowAddNewButton_Click(object sender, RoutedEventArgs e)
    {
        AddNewForm.Visibility = AddNewForm.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        if (AddNewForm.Visibility == Visibility.Visible && WorkCombo.Text.Length == 0 && SearchBox.Text.Length > 0)
        {
            // If the search text matches an existing Work by name, default to it rather than
            // proposing a duplicate Work.
            var existing = Vm?.AvailableSourceMaterials
                .FirstOrDefault(w => string.Equals(w.Name, SearchBox.Text.Trim(), System.StringComparison.OrdinalIgnoreCase));
            if (existing is not null) WorkCombo.SelectedItem = existing;
        }
    }

    private void CancelAddNew_Click(object sender, RoutedEventArgs e)
    {
        AddNewForm.Visibility = Visibility.Collapsed;
    }

    private void ConfirmAddNew_Click(object sender, RoutedEventArgs e)
    {
        if (Vm is null) return;

        SourceMaterialViewModel? work = WorkCombo.SelectedItem as SourceMaterialViewModel;
        if (work is null)
        {
            var typed = WorkCombo.Text.Trim();
            if (typed.Length == 0) return; // a citation needs at least a Work

            work = Vm.AvailableSourceMaterials
                .FirstOrDefault(w => string.Equals(w.Name, typed, System.StringComparison.OrdinalIgnoreCase));
            work ??= Vm.CreateSourceMaterial(typed);
        }

        var code = PartCodeBox.Text.Trim();
        SourceMaterialPartViewModel? part = null;
        if (code.Length > 0)
        {
            // Reuse an existing Part under this Work with the same code rather than duplicating it.
            part = Vm.AvailableSourceMaterialParts
                .FirstOrDefault(p => p.SourceMaterialId == work.Id &&
                                      string.Equals(p.Code, code, System.StringComparison.OrdinalIgnoreCase));
            part ??= Vm.CreateSourceMaterialPart(work, code, PartNameBox.Text.Trim());
        }

        Vm.AddSourceReference(work, part);
        OpenToggle.IsChecked = false;
    }

    // ── Constructor ───────────────────────────────────────────────────────

    public SourceMaterialPickerControl()
    {
        InitializeComponent();
    }
}
