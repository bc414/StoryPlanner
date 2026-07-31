using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using StoryPlanner.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

/// <summary>
/// Drives the Scan Preview dialog: lists every conversation from a Claude export scan with its
/// advisory classification, lets the user hand-pick exactly which ones to export for Cowork
/// (most conversations are off-topic and must NOT be exported), resolve NeedsConfirmation matches,
/// and mark off-topic conversations as ignored so they stop resurfacing as New on future scans.
/// </summary>
public partial class ScanPreviewViewModel : ObservableObject
{
    private readonly IStoryService _storyService;

    public ObservableCollection<ScanPreviewRowViewModel> Rows { get; }
    public ICollectionView FilteredRows { get; }

    [ObservableProperty] private string _searchText = string.Empty;

    // Unchanged/Ignored rows are the majority of a long-running project's history repeating on
    // every scan — hidden by default so the New/Reopened/NeedsConfirmation rows the user actually
    // needs to act on aren't buried.
    [ObservableProperty] private bool _hideUnchangedAndIgnored = true;

    [ObservableProperty] private string _statusMessage = string.Empty;

    public ScanPreviewViewModel(IStoryService storyService, System.Collections.Generic.IEnumerable<ConversationSyncItem> items)
    {
        _storyService = storyService;
        Rows = new ObservableCollection<ScanPreviewRowViewModel>(items.Select(i => new ScanPreviewRowViewModel(i)));
        FilteredRows = new ListCollectionView(Rows) { Filter = FilterRow };
    }

    partial void OnSearchTextChanged(string value) => FilteredRows.Refresh();
    partial void OnHideUnchangedAndIgnoredChanged(bool value) => FilteredRows.Refresh();

    private bool FilterRow(object obj)
    {
        if (obj is not ScanPreviewRowViewModel row) return false;

        if (HideUnchangedAndIgnored &&
            row.Classification is ConversationSyncClassification.Unchanged or ConversationSyncClassification.Ignored)
            return false;

        if (!string.IsNullOrWhiteSpace(SearchText) &&
            !row.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }

    public int TotalCount    => Rows.Count;
    public int SelectedCount => Rows.Count(r => r.IsSelected);

    // ── Per-row actions ───────────────────────────────────────────────────────

    /// <summary>Confirms a guessed match: backfills the Claude uuid onto the matched DB
    /// conversation so every later scan recognizes it with certainty, then reclassifies this row.</summary>
    [RelayCommand]
    private async Task ConfirmMatch(ScanPreviewRowViewModel row)
    {
        if (row.Item.ProposedMatchConversationId is not { } matchId) return;
        await BackfillAndReclassify(row, matchId);
    }

    /// <summary>Rejects a guessed match — the row falls back to New, letting the user Ignore it
    /// (if off-topic), export it as a fresh conversation, or manually map it instead.</summary>
    [RelayCommand]
    private void RejectMatch(ScanPreviewRowViewModel row)
    {
        row.Item.ProposedMatchConversationId = null;
        row.Item.ProposedMatchTitle          = string.Empty;
        row.Item.DbBlockCount                = 0;
        row.Item.Classification              = ConversationSyncClassification.New;
        row.RefreshClassification();
        FilteredRows.Refresh();
    }

    [RelayCommand]
    private async Task IgnoreRow(ScanPreviewRowViewModel row)
    {
        if (!row.CanIgnore) return;

        await _storyService.IgnoreConversationAsync(row.Item.Export.Uuid, row.Item.Export.Title);
        row.Item.Classification = ConversationSyncClassification.Ignored;
        row.IsSelected = false;
        row.RefreshClassification();
        FilteredRows.Refresh();
    }

    /// <summary>Reverses a mistaken Ignore. Re-runs the real scan logic for just this row (rather
    /// than guessing New) so a conversation that's actually already in the DB — just missed by the
    /// heuristic, e.g. because its title was edited after import — comes back as NeedsConfirmation
    /// instead of silently offering to import it as a duplicate.</summary>
    [RelayCommand]
    private async Task UnignoreRow(ScanPreviewRowViewModel row)
    {
        if (!row.CanUnignore) return;

        await _storyService.UnignoreConversationAsync(row.Item.Export.Uuid);
        ApplyRescanResult(row, await _storyService.RescanOneAsync(row.Item.Export));
    }

    /// <summary>Manual override for when the automatic date/block-count heuristic either missed
    /// the real match (falls through to New) or proposed the wrong one (NeedsConfirmation) — e.g.
    /// a same-day conversation on a different platform, or a title edited so far it's ambiguous.
    /// Lets the user point this row directly at a specific existing Claude conversation.</summary>
    [RelayCommand]
    private async Task MapToExisting(ScanPreviewRowViewModel row)
    {
        if (!row.CanMapToExisting) return;

        var candidates = _storyService.Conversations
            .Where(c => c.Platform == "Claude" && string.IsNullOrEmpty(c.SourceUuid));

        var pickerVm = new ConversationPickerViewModel(candidates);
        var window = new ConversationPickerWindow
        {
            DataContext = pickerVm,
            Owner = Application.Current.Windows.OfType<ScanPreviewWindow>().FirstOrDefault()
        };

        if (window.ShowDialog() != true || pickerVm.SelectedConversationId is not { } targetId) return;
        await BackfillAndReclassify(row, targetId);
    }

    private async Task BackfillAndReclassify(ScanPreviewRowViewModel row, int matchedConversationId)
    {
        await _storyService.BackfillConversationUuidAsync(matchedConversationId, row.Item.Export.Uuid);
        ApplyRescanResult(row, await _storyService.RescanOneAsync(row.Item.Export));
    }

    private void ApplyRescanResult(ScanPreviewRowViewModel row, ConversationSyncItem rescanned)
    {
        row.Item.Classification             = rescanned.Classification;
        row.Item.MatchedConversationId       = rescanned.MatchedConversationId;
        row.Item.ProposedMatchConversationId = rescanned.ProposedMatchConversationId;
        row.Item.ProposedMatchTitle          = rescanned.ProposedMatchTitle;
        row.Item.DbBlockCount                = rescanned.DbBlockCount;
        row.Item.ExistingSourceFilePrefix    = rescanned.ExistingSourceFilePrefix;
        row.IsSelected = rescanned.Classification is ConversationSyncClassification.New or ConversationSyncClassification.Reopened;
        row.RefreshClassification();
        FilteredRows.Refresh();
    }

    // ── Bulk actions (operate on whatever is checked) ───────────────────────────

    [RelayCommand]
    private async Task IgnoreSelected()
    {
        foreach (var row in Rows.Where(r => r.IsSelected && r.CanIgnore).ToList())
        {
            await _storyService.IgnoreConversationAsync(row.Item.Export.Uuid, row.Item.Export.Title);
            row.Item.Classification = ConversationSyncClassification.Ignored;
            row.IsSelected = false;
            row.RefreshClassification();
        }
        FilteredRows.Refresh();
    }

    [RelayCommand]
    private async Task ExportSelected()
    {
        var selected = Rows.Where(r => r.IsSelected).Select(r => r.Item).ToList();
        if (selected.Count == 0)
        {
            StatusMessage = "No conversations checked — check the ones you want to hand to Cowork first.";
            return;
        }

        var dlg = new OpenFolderDialog { Title = "Choose an output folder for the Cowork content files" };
        if (dlg.ShowDialog() != true) return;

        var written = await _storyService.ExportConversationContentAsync(selected, dlg.FolderName);
        StatusMessage = $"Wrote {written.Count} content file(s) to {dlg.FolderName}.";
    }
}
