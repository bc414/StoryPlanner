using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner;

/// <summary>
/// Wraps one Conversation entity. Serves double duty:
///   - Library card: stats, derived state, progress bar.
///   - Reader window DataContext: block list and selection.
/// The window manager enforces at most one open reader window per conversation,
/// so merging these concerns into one class is safe.
/// </summary>
public partial class ConversationViewModel : ObservableObject
{
    private readonly IStoryService _storyService;

    public Conversation Model { get; }

    // Ordered block VMs populated by ProjectLoader
    public ObservableCollection<ConversationBlockViewModel> Blocks { get; } = new();

    public ConversationViewModel(Conversation model, IStoryService storyService)
    {
        Model         = model;
        _storyService = storyService;
    }

    /// <summary>
    /// Builds one fully-wired VM per conversation — blocks grouped, ordered, initialized, and
    /// parented; OnStatsRefreshed attached. The ONE construction recipe, shared by ProjectLoader
    /// (initial load) and ConversationLibraryViewModel (post-import rebuild): when the 2026-07-31
    /// pipeline redesign changed block-VM construction, both copies had to change in lockstep,
    /// which is exactly the drift this factory removes.
    /// </summary>
    public static List<ConversationViewModel> BuildAll(IStoryService storyService, Action onStatsRefreshed)
    {
        var blocksByConv = storyService.ConversationBlocks
            .GroupBy(b => b.ConversationId)
            .ToDictionary(g => g.Key, g => g.OrderBy(b => b.BlockNumber).ToList());

        var result = new List<ConversationViewModel>();
        foreach (var conv in storyService.Conversations)
        {
            var convVm = new ConversationViewModel(conv, storyService);
            if (blocksByConv.TryGetValue(conv.Id, out var blocks))
                foreach (var block in blocks)
                {
                    var blockVm = new ConversationBlockViewModel(block, storyService) { ParentConversation = convVm };
                    blockVm.Initialize();
                    convVm.Blocks.Add(blockVm);
                }
            convVm.OnStatsRefreshed = onStatsRefreshed;
            result.Add(convVm);
        }
        return result;
    }

    // ── Passthrough display properties ─────────────────────────────────────────

    public int Id => Model.Id;

    public string Title
    {
        get => Model.Title;
        set => SetProperty(Model.Title, value, Model, (m, v) => m.Title = v);
    }

    public string   Platform         => Model.Platform;
    public DateTime ConversationDate => Model.ConversationDate;
    public string   ArcSummary       => Model.ArcSummary;
    public int      BlockCount       => Model.BlockCount;

    // A conversation imported straight from a raw export has no arc summary. Rather than showing
    // an empty header band / an expander that opens onto nothing, the surfaces that display it
    // collapse. Empty is ordinary here, not a defect.
    public bool HasArcSummary => !string.IsNullOrWhiteSpace(Model.ArcSummary);

    // ── Library stats (computed from block VMs) ────────────────────────────────

    public int UnreadCount  => Blocks.Count(b => b.BlockState == BlockState.Unread);
    public int SkippedCount => Blocks.Count(b => b.BlockState == BlockState.Skipped);
    public int FlaggedCount => Blocks.Count(b => b.BlockState == BlockState.Flagged);
    public int DoneCount    => Blocks.Count(b => b.BlockState == BlockState.Done);

    public ConversationDerivedState DerivedState
    {
        get
        {
            if (!Blocks.Any()) return ConversationDerivedState.Unstarted;
            if (UnreadCount == Blocks.Count) return ConversationDerivedState.Unstarted;
            if (UnreadCount == 0 && FlaggedCount == 0) return ConversationDerivedState.Complete;
            return ConversationDerivedState.InProgress;
        }
    }

    public double ProgressFraction =>
        Blocks.Count == 0 ? 0.0 : (double)(DoneCount + SkippedCount) / Blocks.Count;

    // Set by ProjectLoader so dashboard totals refresh when any block state changes.
    internal Action? OnStatsRefreshed { get; set; }

    // Call whenever any block state changes so library card bindings update
    public void RefreshStats()
    {
        OnPropertyChanged(nameof(UnreadCount));
        OnPropertyChanged(nameof(SkippedCount));
        OnPropertyChanged(nameof(FlaggedCount));
        OnPropertyChanged(nameof(DoneCount));
        OnPropertyChanged(nameof(DerivedState));
        OnPropertyChanged(nameof(ProgressFraction));
        OnStatsRefreshed?.Invoke();
    }

    // ── Reader window state ────────────────────────────────────────────────────

    [ObservableProperty]
    private ConversationBlockViewModel? _selectedBlock;

    public int SelectedCount => Blocks.Count(b => b.IsSelected);

    // Bulk state application for the multi-selection: set every selected block's
    // state without per-block persistence, then refresh stats and save exactly once.
    public async Task ApplyStateToSelectionAsync(BlockState state)
    {
        foreach (var block in Blocks.Where(b => b.IsSelected))
            block.SetStateBulk(state);

        RefreshStats();
        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private void SelectBlock(ConversationBlockViewModel block)
    {
        SelectedBlock = block;
    }

    // ── Selected-block state shortcuts (F1–F4 in the reader window) ──────────────
    // Delegate to the block's own state commands so persistence/stat-refresh stays
    // in one place. No-op when nothing is selected.

    [RelayCommand] private void SetSelectedUnread()  => SelectedBlock?.MarkUnread();
    [RelayCommand] private void SetSelectedSkipped() => SelectedBlock?.MarkSkipped();
    [RelayCommand] private void SetSelectedFlagged() => SelectedBlock?.MarkFlagged();
    [RelayCommand] private void SetSelectedDone()    => SelectedBlock?.MarkDone();
}

public enum ConversationDerivedState
{
    Unstarted,
    InProgress,
    Complete
}
