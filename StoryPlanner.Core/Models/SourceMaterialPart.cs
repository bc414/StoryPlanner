namespace StoryPlanner.Core;

/// <summary>
/// One unit of a mining pass over a <see cref="SourceMaterial"/> — one episode watched, one
/// EaW country played, one chapter reread. Parts are pre-enumerated (seeded from a reviewable
/// DataOps config), not created on first citation, so a Part with zero references is a genuine
/// negative-space signal rather than an artifact of accretion.
/// </summary>
public class SourceMaterialPart
{
    public int Id { get; set; }
    public int SourceMaterialId { get; set; }

    /// <summary>Short locator, e.g. "S3E01", "Griffonian Empire", "ch1".</summary>
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OrderIndex { get; set; }

    /// <summary>
    /// Whether this Part has been deliberately passed over for TLTT material — orthogonal to
    /// whether any note cites it. A Part can be Reviewed with zero citations ("watched it again,
    /// nothing there") or NotReviewed with citations ("cited from memory, never revisited").
    /// Both axes are needed for a truthful negative-space queue; see CLAUDE.md.
    /// </summary>
    public SourcePartReviewState ReviewState { get; set; }
}

public enum SourcePartReviewState
{
    NotReviewed,
    Reviewed
}
