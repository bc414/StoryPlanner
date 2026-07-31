using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

public partial class SourceMaterialPartViewModel : ObservableObject
{
    private readonly SourceMaterialPart _model;
    private readonly IStoryService _storyService;

    public SourceMaterialPart Model => _model;
    public int Id => _model.Id;
    public int SourceMaterialId => _model.SourceMaterialId;

    public string Code
    {
        get => _model.Code;
        set => SetProperty(_model.Code, value, _model, (m, v) => m.Code = v);
    }

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
    }

    public string Description
    {
        get => _model.Description;
        set => SetProperty(_model.Description, value, _model, (m, v) => m.Description = v);
    }

    public int OrderIndex
    {
        get => _model.OrderIndex;
        set => SetProperty(_model.OrderIndex, value, _model, (m, v) => m.OrderIndex = v);
    }

    /// <summary>
    /// Whether this Part has been deliberately reviewed for TLTT material. Orthogonal to
    /// whether any note cites it — see CLAUDE.md's negative-space 2x2 (untouched / cited from
    /// memory / confirmed empty / mined).
    /// </summary>
    public SourcePartReviewState ReviewState
    {
        get => _model.ReviewState;
        set
        {
            if (SetProperty(_model.ReviewState, value, _model, (m, v) => m.ReviewState = v))
            {
                OnPropertyChanged(nameof(IsReviewed));
                OnPropertyChanged(nameof(IsUntouched));
            }
        }
    }

    public bool IsReviewed
    {
        get => ReviewState == SourcePartReviewState.Reviewed;
        set => ReviewState = value ? SourcePartReviewState.Reviewed : SourcePartReviewState.NotReviewed;
    }

    /// <summary>"S3E01 — The Crystal Empire Part 1" style label for search results and chips.</summary>
    public string DisplayLabel => string.IsNullOrWhiteSpace(Name) ? Code : $"{Code} — {Name}";

    // ── Coverage ─────────────────────────────────────────────────────────────
    // Deliberately NOT a live-reactive property (would need a permanent subscription to the
    // shared NoteSourceReferences collection, outliving delete for every Part ever created —
    // see the file header's leak note). Correct on load and on RefreshNoteCount(); the Sources
    // tab calls that explicitly on Work-selection and via a Refresh Coverage command, matching
    // the app's "explicit action, nothing silently updates" ethos (CLAUDE.md: retrieval, not
    // suggestion).

    /// <summary>How many notes cite this Part, counted fresh from IStoryService each read.</summary>
    public int NoteCount => _storyService.NoteSourceReferences.Count(r => r.SourceMaterialPartId == Id);

    /// <summary>
    /// The rewatch-queue signal — never reviewed AND never cited. Deliberately NOT the same as
    /// NoteCount == 0: a Reviewed Part with zero notes is "confirmed empty" (deliberately
    /// checked, nothing there), not untouched. See CLAUDE.md's negative-space 2x2.
    /// </summary>
    public bool IsUntouched => ReviewState == SourcePartReviewState.NotReviewed && NoteCount == 0;

    /// <summary>Call after any citation add/remove elsewhere to bring NoteCount/IsUntouched
    /// back in sync for display.</summary>
    public void RefreshNoteCount()
    {
        OnPropertyChanged(nameof(NoteCount));
        OnPropertyChanged(nameof(IsUntouched));
    }

    public SourceMaterialPartViewModel(SourceMaterialPart model, IStoryService storyService)
    {
        _model = model;
        _storyService = storyService;
    }
}
