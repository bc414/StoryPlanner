using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

public partial class ConversationLibraryViewModel : ObservableObject
{
    private readonly IStoryService      _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager     _windowManager;

    public ObservableCollection<ConversationViewModel> Conversations => _registry.AllConversationViewModels;

    public ICollectionView FilteredConversations { get; }

    [ObservableProperty] private string _searchText = string.Empty;

    // When on, cards start with their summary expander open. Cards bind this OneWay,
    // so individual expanders can still be toggled without fighting the shared setting.
    [ObservableProperty] private bool _showExpandedCards;

    // Date (chronological, ascending) is the default; Unread surfaces conversations
    // with the most blocks still to read first.
    [ObservableProperty] private ConversationSortMode _sortMode = ConversationSortMode.Date;

    // IContentFactory used to be injected here so the reader's coverage checklist could create
    // notes. That path is gone (2026-07-31); the library never creates content of its own.
    public ConversationLibraryViewModel(
        IStoryService storyService,
        IViewModelRegistry registry,
        IWindowManager windowManager)
    {
        _storyService   = storyService;
        _registry       = registry;
        _windowManager  = windowManager;

        FilteredConversations = new ListCollectionView(Conversations) { Filter = FilterConversation };
        ApplySort();
    }

    partial void OnSearchTextChanged(string value)    => FilteredConversations.Refresh();
    partial void OnSortModeChanged(ConversationSortMode value) => ApplySort();

    // Rebuild the view's sort. UnreadCount is a computed property, so this re-sorts on
    // demand (when the mode changes) rather than tracking live block-state edits.
    private void ApplySort()
    {
        var sorts = FilteredConversations.SortDescriptions;
        sorts.Clear();
        if (SortMode == ConversationSortMode.Unread)
        {
            // Most still-to-read first, breaking ties chronologically.
            sorts.Add(new SortDescription(nameof(ConversationViewModel.UnreadCount), ListSortDirection.Descending));
            sorts.Add(new SortDescription(nameof(ConversationViewModel.ConversationDate), ListSortDirection.Ascending));
        }
        else
        {
            sorts.Add(new SortDescription(nameof(ConversationViewModel.ConversationDate), ListSortDirection.Ascending));
        }
    }

    private bool FilterConversation(object obj)
    {
        if (obj is not ConversationViewModel vm) return false;
        if (!string.IsNullOrWhiteSpace(SearchText) &&
            !vm.Title.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase))
            return false;
        return true;
    }

    public void Reload() { /* registry repopulated by ProjectLoader */ }

    // ── Dashboard stats ────────────────────────────────────────────────────────

    public int TotalConversations => Conversations.Count;
    public int UnstartedCount     => Conversations.Count(c => c.DerivedState == ConversationDerivedState.Unstarted);
    public int InProgressCount    => Conversations.Count(c => c.DerivedState == ConversationDerivedState.InProgress);
    public int CompleteCount      => Conversations.Count(c => c.DerivedState == ConversationDerivedState.Complete);

    public int TotalBlocks  => Conversations.Sum(c => c.Blocks.Count);
    public int TotalUnread  => Conversations.Sum(c => c.UnreadCount);
    public int TotalSkipped => Conversations.Sum(c => c.SkippedCount);
    public int TotalFlagged => Conversations.Sum(c => c.FlaggedCount);
    public int TotalDone    => Conversations.Sum(c => c.DoneCount);

    // Overall progress mirrors the per-card ProgressFraction: Done + Skipped counts as
    // "handled" (Flagged still needs a return visit, so it's not progress yet).
    public double OverallProgressFraction =>
        TotalBlocks == 0 ? 0.0 : (double)(TotalDone + TotalSkipped) / TotalBlocks;

    public int OverallProgressPercent => (int)System.Math.Round(OverallProgressFraction * 100);

    public void RefreshDashboard()
    {
        OnPropertyChanged(nameof(TotalConversations));
        OnPropertyChanged(nameof(UnstartedCount));
        OnPropertyChanged(nameof(InProgressCount));
        OnPropertyChanged(nameof(CompleteCount));
        OnPropertyChanged(nameof(TotalBlocks));
        OnPropertyChanged(nameof(TotalUnread));
        OnPropertyChanged(nameof(TotalSkipped));
        OnPropertyChanged(nameof(TotalFlagged));
        OnPropertyChanged(nameof(TotalDone));
        OnPropertyChanged(nameof(OverallProgressFraction));
        OnPropertyChanged(nameof(OverallProgressPercent));
    }

    // ── Commands ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Bulk import: point at a folder of NNN_{slug}_content.json files. A NNN_{slug}_meta.json with
    /// the same prefix supplies summaries when present; a content file without one imports anyway,
    /// with no summaries. Summaries are a navigation aid, not a precondition for reading.
    /// </summary>
    [RelayCommand]
    private async Task ImportFromFolder()
    {
        if (!_storyService.IsProjectLoaded) return;

        var dlg = new OpenFolderDialog
        {
            Title = "Select folder containing NNN_*_content.json files (NNN_*_meta.json optional)"
        };
        if (dlg.ShowDialog() != true) return;

        var result = await _storyService.ImportConversationsFolderAsync(dlg.FolderName);
        RebuildConversationVMs();
        RefreshDashboard();

        MessageBox.Show(DescribeImport(result), "Import Complete",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>Plain tally of what an import did — including how many landed without summaries,
    /// which is otherwise invisible until the conversation is opened.</summary>
    private static string DescribeImport(ConversationImportResult result)
    {
        if (result.Total == 0) return "Nothing to import.";

        var text = $"{result.Created} new, {result.Updated} updated.";
        if (result.WithoutSummaries > 0)
            text += $"\n{result.WithoutSummaries} imported without summaries (no meta file).";
        return text;
    }

    /// <summary>
    /// Stage 1 of the pipeline: pick a Claude conversations.json export, scan it against the DB's
    /// existing conversations, and open the Scan Preview so the user can hand-pick exactly which
    /// conversations to export for Cowork (most exported conversations are off-topic and must be
    /// kept out of that folder).
    /// </summary>
    [RelayCommand]
    private async Task ScanClaudeExport()
    {
        if (!_storyService.IsProjectLoaded) return;

        var dlg = new OpenFileDialog
        {
            Title = "Select a Claude conversations.json export",
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
        };
        if (dlg.ShowDialog() != true) return;

        System.Collections.Generic.List<ConversationSyncItem> items;
        try
        {
            items = await _storyService.ScanClaudeExportAsync(dlg.FileName);
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Failed to parse export:\n{ex.Message}", "Scan Failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // The preview can import directly (no Cowork round trip), which adds rows the library's
        // VMs know nothing about — so it gets a callback to rebuild them, the same refresh the
        // folder import does for itself.
        var previewVm = new ScanPreviewViewModel(_storyService, items, onImported: () =>
        {
            RebuildConversationVMs();
            RefreshDashboard();
        });
        var window = new ScanPreviewWindow
        {
            DataContext = previewVm,
            Owner = Application.Current.MainWindow
        };
        window.ShowDialog();
    }

    [RelayCommand]
    private void OpenConversationReader(ConversationViewModel vm)
    {
        _windowManager.OpenConversationReaderWindow(vm);
    }

    [RelayCommand]
    private async Task DeleteConversation(ConversationViewModel vm)
    {
        var result = MessageBox.Show(
            $"Delete \"{vm.Title}\" and its {vm.BlockCount} blocks? This cannot be undone.",
            "Delete Conversation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        await _storyService.DeleteConversationAsync(vm.Model);
        _registry.AllConversationViewModels.Remove(vm);
        RefreshDashboard();
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private void RebuildConversationVMs()
    {
        _registry.AllConversationViewModels.Clear();

        foreach (var convVm in ConversationViewModel.BuildAll(_storyService, RefreshDashboard))
            _registry.AllConversationViewModels.Add(convVm);
    }
}

public enum ConversationSortMode
{
    Date,
    Unread
}
