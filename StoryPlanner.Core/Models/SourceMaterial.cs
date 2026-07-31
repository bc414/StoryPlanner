namespace StoryPlanner.Core;

/// <summary>
/// A citable external work (an MLP:FiM season, Equestria at War, another fanfic, ...). This is
/// the top tier of the two-tier Work/Part model — see <see cref="SourceMaterialPart"/>. The
/// coverage view (Sources tab) treats the full Work+Part set as a closed, pre-enumerated corpus,
/// seeded by the DataOps seed-source-material op, not accreted by first citation: negative
/// space (Parts nobody has cited) is only visible if the set is complete up front.
/// </summary>
public class SourceMaterial
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The noun for this Work's Parts, e.g. "Episode" (MLP:FiM), "Country" (Equestria at War),
    /// "Chapter" (another fanfic). Empty means this Work has no Parts — cite the Work itself.
    /// Metadata-driven label, same principle as NoteTrackDefinition's prose fields: the UI reads
    /// this from data rather than switching on the Work's identity.
    /// </summary>
    public string PartNoun { get; set; } = string.Empty;

    public int OrderIndex { get; set; }
}
