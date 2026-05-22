namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Controls how a <see cref="NoteTrackViewModel"/> presents itself.
/// Each value maps to one of the three text fields on <see cref="StoryPlanner.Core.Models.NoteTrackDefinition"/>
/// and determines whether the track is editable.
/// </summary>
public enum TrackDisplayMode
{
    /// <summary>Editable. Header shows <c>DisplayQuestion</c>.</summary>
    Active,

    /// <summary>Read-only. Header shows <c>UsageDirective</c>.</summary>
    Reference,

    /// <summary>Editable. Header shows <c>AuditDirective</c>. Promotion to Confirmed is allowed.</summary>
    Audit,
}