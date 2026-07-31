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
    public string Summary      => Model.Summary;
    public bool   HasDecisions => Model.HasDecisions;
    public bool   IsCompaction => Model.IsCompaction;

    // ── Mutable state ──────────────────────────────────────────────────────────

    [ObservableProperty]
    private BlockState _blockState = BlockState.Unread;

    partial void OnBlockStateChanged(BlockState value)
    {
        Model.BlockState = value;
        ParentConversation?.RefreshStats();
        _ = PersistStateAsync();
    }

    private async Task PersistStateAsync()
    {
        await _storyService.SaveAsync();
    }

    // ── State commands (used by right-click menu and keyboard shortcuts) ────────

    [RelayCommand] public void MarkUnread()  => BlockState = BlockState.Unread;
    [RelayCommand] public void MarkSkipped() => BlockState = BlockState.Skipped;
    [RelayCommand] public void MarkFlagged() => BlockState = BlockState.Flagged;
    [RelayCommand] public void MarkDone()    => BlockState = BlockState.Done;

    // ── Initialization ─────────────────────────────────────────────────────────

    // Called after construction to sync the observable property with the persisted model value
    // without triggering a save.
    public void Initialize()
    {
        _blockState = Model.BlockState;
    }
}
