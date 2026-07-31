namespace StoryPlanner.Core;

/// <summary>
/// One citation: a Note pointing at a SourceMaterial (and optionally a SourceMaterialPart within
/// it). A note-owned junction, not polymorphic — only Note cites source material, so no
/// OwnerType column is needed (contrast PlotPointSubjectLink or the NarrativeProperty* family).
/// Many rows per note are expected and intentional: a note may cite several episodes (e.g. "the
/// Wonderbolts were useless in a crisis, as shown in Sonic Rainboom, Secret of my Excess,
/// Equestria Games and Twilight's Kingdom" cites four Parts for one claim). Splitting such a
/// note into one-per-episode was considered and rejected — see the plan's "why not just split
/// the notes" analysis; the claim is often the relation between the cited Parts, or one
/// proposition backed by several independent citations, not one claim per citation.
///
/// SourceMaterialId is carried directly (denormalized from the Part's parent when a Part is
/// set) so "every note citing this Work at any depth" is a single-column query without a join
/// through SourceMaterialParts. Invariant SourceMaterialPart.SourceMaterialId == SourceMaterialId
/// (when a Part is set) is enforced in code via PlanIntegrity, not the schema — no FKs, by
/// design (see CLAUDE.md).
/// </summary>
public class NoteSourceReference
{
    public int Id { get; set; }
    public int NoteId { get; set; }
    public int SourceMaterialId { get; set; }

    /// <summary>Null means the note cites the Work as a whole, not any particular Part.</summary>
    public int? SourceMaterialPartId { get; set; }

    public int SortOrder { get; set; }
}
