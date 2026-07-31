using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Note-state rollups per subject and per track, plus a derived read of which work phases are
/// satisfied. Pure counting over authored data — no scoring, no readiness verdict.
///
/// Recomputed when the tab is shown (see ProgressView's IsVisibleChanged) and on project load.
/// Deliberately NOT subscribed to note mutations: that would recount every subject on every edit,
/// across thousands of notes — the O(n^2) shape the deferred-load machinery exists to avoid —
/// whereas recomputing on show costs nothing while the tab is hidden. The Refresh button remains
/// for recomputing without leaving the tab.
/// </summary>
public partial class ProgressViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager _windowManager;
    private readonly AppSettings _appSettings;

    public ObservableCollection<ProgressSubjectRow> Subjects { get; } = new();
    public ObservableCollection<string> PhaseStatuses { get; } = new();

    [ObservableProperty]
    private string _summary = "No project loaded.";

    /// <summary>
    /// In the v1 archive, Confirmed means "review closed — migrated or deliberately superseded,
    /// disposition not recorded", NOT "stable". Rendering an archive file's counts under the v2
    /// labels would assert something the data does not support, so the header says so instead.
    /// </summary>
    public string ConfirmedColumnHeader => _appSettings.IsArchiveMode ? "Review closed" : "Confirmed";

    public bool IsArchiveMode => _appSettings.IsArchiveMode;

    public ProgressViewModel(
        IStoryService storyService,
        IViewModelRegistry registry,
        IWindowManager windowManager,
        AppSettings appSettings)
    {
        _storyService  = storyService;
        _registry      = registry;
        _windowManager = windowManager;
        _appSettings   = appSettings;

        _registry.StoryLoaded += Rebuild;
        _appSettings.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppSettings.IsArchiveMode))
            {
                OnPropertyChanged(nameof(ConfirmedColumnHeader));
                OnPropertyChanged(nameof(IsArchiveMode));
            }
        };
    }

    [RelayCommand]
    public void Rebuild()
    {
        Subjects.Clear();
        PhaseStatuses.Clear();

        var subjectNotes = _storyService.Notes
            .Where(n => n.OwnerType == OwnerType.Subject)
            .GroupBy(n => n.OwnerId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var trackNameById = _storyService.NoteTrackDefinitions.ToDictionary(t => t.Id, t => t.TrackName);
        var subjectTypeById = _storyService.SubjectDefinitions.ToDictionary(s => s.Id, s => s.SubjectType);

        foreach (var subject in _storyService.Subjects.OrderBy(s => s.Name))
        {
            var notes = subjectNotes.TryGetValue(subject.Id, out var list) ? list : new List<Note>();

            var tracks = notes
                .GroupBy(n => n.NoteTrackDefinitionId)
                .Select(g => new ProgressTrackRow
                {
                    TrackName = g.Key is int id && trackNameById.TryGetValue(id, out var name)
                        ? name
                        : "(unassigned)",
                    Confirmed = g.Count(n => n.NoteState == NoteState.Confirmed),
                    Unset     = g.Count(n => n.NoteState == NoteState.Unset),
                    Flagged   = g.Count(n => n.NoteState == NoteState.Flagged)
                })
                .OrderBy(t => t.TrackName)
                .ToList();

            Subjects.Add(new ProgressSubjectRow
            {
                SubjectId = subject.Id,
                Name = subject.Name,
                SubjectType = subjectTypeById.TryGetValue(subject.SubjectDefinitionId, out var st) ? st : "",
                Confirmed = notes.Count(n => n.NoteState == NoteState.Confirmed),
                Unset     = notes.Count(n => n.NoteState == NoteState.Unset),
                Flagged   = notes.Count(n => n.NoteState == NoteState.Flagged),
                Tracks = tracks
            });
        }

        BuildPhaseStatuses();

        var totalFlagged = _storyService.Notes.Count(n => n.NoteState == NoteState.Flagged);
        var totalUnset   = _storyService.Notes.Count(n => n.NoteState == NoteState.Unset);
        Summary = $"{_storyService.Notes.Count} notes across all owners — "
                + $"{totalUnset} unset, {totalFlagged} flagged.";
    }

    /// <summary>
    /// Phase completion is DERIVED from the criteria on each WorkPhase row and never stored —
    /// the same principle as timeline eras being the gaps between pivots. Counts every note,
    /// not just subject-owned ones, because a phase is a statement about the whole file.
    /// </summary>
    private void BuildPhaseStatuses()
    {
        var flagged = _storyService.Notes.Count(n => n.NoteState == NoteState.Flagged);
        var unset   = _storyService.Notes.Count(n => n.NoteState == NoteState.Unset);

        foreach (var phase in _storyService.WorkPhases.OrderBy(p => p.DisplayOrder))
        {
            var unmet = new List<string>();
            if (phase.RequiresZeroFlaggedNotes && flagged > 0) unmet.Add($"{flagged} flagged");
            if (phase.RequiresZeroUnsetNotes   && unset   > 0) unmet.Add($"{unset} unset");

            var gated = _storyService.NarrativePropertyDefinitions
                .Count(p => p.GatingWorkPhaseId == phase.Id);
            if (gated > 0) unmet.Add($"{gated} gating propert{(gated == 1 ? "y" : "ies")} — see Property Gaps");

            PhaseStatuses.Add(unmet.Count == 0
                ? $"{phase.DisplayOrder}. {phase.Name} — criteria met"
                : $"{phase.DisplayOrder}. {phase.Name} — {string.Join("; ", unmet)}");
        }

        if (_storyService.WorkPhases.Count == 0)
            PhaseStatuses.Add("No work phases defined — add them in the Definitions tab.");
    }

    [RelayCommand]
    private void OpenSubject(ProgressSubjectRow? row)
    {
        if (row is null) return;
        var vm = _registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == row.SubjectId);
        if (vm is not null) _windowManager.OpenSubjectWindow(vm);
    }
}
