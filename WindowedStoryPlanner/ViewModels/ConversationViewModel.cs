using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Wraps one Conversation entity. Serves double duty:
///   - Library card: stats, derived state, progress bar.
///   - Reader window DataContext: selected block, routing header subjects.
/// The window manager enforces at most one open reader window per conversation,
/// so merging these concerns into one class is safe.
/// </summary>
public partial class ConversationViewModel : ObservableObject
{
    private readonly IWindowManager     _windowManager;
    private readonly IContentFactory    _contentFactory;
    private readonly IViewModelRegistry _registry;

    public Conversation Model { get; }

    // Ordered block VMs populated by ProjectLoader
    public ObservableCollection<ConversationBlockViewModel> Blocks { get; } = new();

    // Subject coverage for the routing header — populated by BuildSubjectCoverages()
    public ObservableCollection<SubjectCoverageViewModel> SubjectCoverages { get; } = new();

    public ConversationViewModel(
        Conversation model,
        IWindowManager windowManager,
        IContentFactory contentFactory,
        IViewModelRegistry registry)
    {
        Model           = model;
        _windowManager  = windowManager;
        _contentFactory = contentFactory;
        _registry       = registry;
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

    // ── Routing header subject coverages ──────────────────────────────────────

    [RelayCommand]
    private void OpenSubject(SubjectCoverageViewModel coverage)
    {
        _windowManager.OpenCommonWindow(EditorMode.Expansion, coverage.Subject);
    }

    [RelayCommand]
    private async Task AddCoverageTrack(CoverageTrackViewModel track)
    {
        if (track.IsAdded)
        {
            // Misclick recovery: unmark only. No note deletion, no window pop-up —
            // toggling off should be a free, side-effect-free correction.
            track.IsAdded = false;
            return;
        }

        // Guard note creation by whether a note already exists for this (subject, track)
        // pairing — not by the checkbox history — so re-checking after an unmark never
        // creates a duplicate note.
        bool noteExists = _registry.AllNoteViewModels.Any(n =>
            n.OwnerId == track.Subject.Id
            && n.OwnerType == OwnerType.Subject
            && n.NoteTrackDefinitionId == track.Track.Id);

        if (!noteExists)
            await _contentFactory.CreateNoteAsync(track.Subject.Id, OwnerType.Subject, track.Track.Id, 1);

        track.IsAdded = true; // triggers its own SaveAsync via OnIsAddedChanged
        _windowManager.OpenCommonWindow(EditorMode.Expansion, track.Subject);
    }

    // Called by ProjectLoader (Phase 5) once coverage data is available
    public void BuildSubjectCoverages(
        IEnumerable<ConversationSubjectCoverage> coverages,
        IEnumerable<ConversationSubjectCoverageTrack> tracks,
        IEnumerable<SubjectViewModel> allSubjects,
        IEnumerable<NoteTrackDefinitionViewModel> allTracks,
        IStoryService storyService)
    {
        SubjectCoverages.Clear();
        foreach (var c in coverages.Where(c => c.ConversationId == Model.Id))
        {
            var subject = allSubjects.FirstOrDefault(s => s.Id == c.SubjectId);
            if (subject is null) continue;

            var trackVms = tracks
                .Where(t => t.ConversationSubjectCoverageId == c.Id)
                .Select(t =>
                {
                    var trackDef = allTracks.FirstOrDefault(td => td.Id == t.NoteTrackDefinitionId);
                    return trackDef is null ? null : new CoverageTrackViewModel(t, trackDef, subject, storyService);
                })
                .Where(t => t is not null)
                .Cast<CoverageTrackViewModel>()
                .ToList();

            SubjectCoverages.Add(new SubjectCoverageViewModel(subject, trackVms, c));
        }
    }
}

public enum ConversationDerivedState
{
    Unstarted,
    InProgress,
    Complete
}
