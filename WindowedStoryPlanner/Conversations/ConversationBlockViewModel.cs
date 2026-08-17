using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Threading.Tasks;

namespace WindowedStoryPlanner;

public partial class ConversationBlockViewModel : ObservableObject
{
    public ConversationBlock Model { get; }

    private readonly IStoryService _storyService;

    // Back-reference to the parent conversation VM — set by ConversationReaderViewModel
    public ConversationViewModel? ParentConversation { get; set; }

    public ConversationBlockViewModel(ConversationBlock model, IStoryService storyService)
    {
        Model         = model;
        _storyService = storyService;
    }

    // ── Passthrough display properties ─────────────────────────────────────────

    public int    BlockNumber  => Model.BlockNumber;
    public string Speaker      => Model.Speaker;
    public string RawContent   => Model.RawContent;
    public bool   IsCompaction => Model.IsCompaction;

    // ── The author's own navigation note ───────────────────────────────────────

    /// <summary>
    /// Brian's hand-written note on this block — not a machine summary; nothing writes it but
    /// this setter. The reader's middle-column card IS the editor, bound with
    /// UpdateSourceTrigger=LostFocus, so this runs at most once per focus session: that is what
    /// makes persisting from inside it one save per edit rather than one per keystroke.
    /// SetProperty's equality check means focusing in and out without typing saves nothing.
    /// </summary>
    public string Summary
    {
        get => Model.Summary;
        set
        {
            if (!SetProperty(Model.Summary, value, Model, (m, v) => m.Summary = v)) return;
            OnPropertyChanged(nameof(HasSummary));
            // Called directly rather than through a helper so [CallerMemberName] labels a failed
            // save "Summary" — a helper's name would tell Brian nothing about what didn't persist.
            _storyService.SaveAsync().FireAndForget();
        }
    }

    /// <summary>
    /// Drives the card's "Add a note…" affordance. Empty is an ordinary, permanent state — most
    /// blocks will never carry a note, and nothing is ever substituted in for an absent one.
    /// </summary>
    public bool HasSummary => !string.IsNullOrWhiteSpace(Model.Summary);

    // ── Mutable state ──────────────────────────────────────────────────────────

    [ObservableProperty]
    private BlockState _blockState = BlockState.Unread;

    // Multi-select membership (two-way bound to ListBoxItem.IsSelected in the reader window).
    [ObservableProperty]
    private bool _isSelected;

    partial void OnBlockStateChanged(BlockState value)
    {
        Model.BlockState = value;
        ParentConversation?.RefreshStats();
        PersistStateAsync().FireAndForget();
    }

    private async Task PersistStateAsync()
    {
        await _storyService.SaveAsync();
    }

    // Sets the state without triggering the per-block save/stat-refresh in
    // OnBlockStateChanged — the bulk path saves and refreshes once for the whole
    // selection instead (N concurrent SaveAsync calls on one DbContext would throw).
    public void SetStateBulk(BlockState value)
    {
        if (_blockState == value) return;
        _blockState      = value;
        Model.BlockState = value;
        OnPropertyChanged(nameof(BlockState));
    }

    // ── State commands (used by right-click menu and keyboard shortcuts) ────────
    // When this block is part of a multi-selection, the command applies to the whole
    // selection; a block outside the selection (e.g. right-clicked directly) stays
    // single-target — standard Windows selection semantics.

    [RelayCommand] public void MarkUnread()  => Mark(BlockState.Unread);
    [RelayCommand] public void MarkSkipped() => Mark(BlockState.Skipped);
    [RelayCommand] public void MarkFlagged() => Mark(BlockState.Flagged);
    [RelayCommand] public void MarkDone()    => Mark(BlockState.Done);

    private void Mark(BlockState state)
    {
        if (IsSelected && ParentConversation?.SelectedCount > 1)
            ParentConversation.ApplyStateToSelectionAsync(state).FireAndForget();
        else
            BlockState = state;
    }

    // ── Initialization ─────────────────────────────────────────────────────────

    // Called after construction to sync the observable property with the persisted model value
    // without triggering a save.
    public void Initialize()
    {
        _blockState = Model.BlockState;
    }
}
