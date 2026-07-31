namespace StoryPlanner.Core
{
    /// <summary>
    /// A stage of the story-planning work, in order — pure expansion at the start, audit near the
    /// end. Configurable rows rather than an enum, the same Type Object move as SubjectDefinition
    /// and NoteTrackDefinition: adding or reordering a phase is data entry.
    ///
    /// DELIBERATELY NOT EditorMode. EditorMode (Expansion/Linking/Gardener/Audit/SceneDesign) is a
    /// window-level UI stance you switch into and out of freely; a WorkPhase is where the project
    /// as a whole has got to. The names overlap and the concepts do not — do not merge them, and do
    /// not derive one from the other.
    ///
    /// A phase is never STORED as reached. Completion is derived from the data the criteria below
    /// name (same principle as timeline eras, which are derived as the gaps between pivots and
    /// never stored). The criteria are deliberately a small explicit set rather than a rule engine:
    /// extend by adding columns, the way NoteTrackDefinition grew its Supports* flags.
    /// </summary>
    public class WorkPhase
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string Description { get; set; } = string.Empty;

        /// <summary>Phase is not complete while any note is Flagged — e.g. the end of Expansion.</summary>
        public bool RequiresZeroFlaggedNotes { get; set; }

        /// <summary>Phase is not complete while any note is Unset.</summary>
        public bool RequiresZeroUnsetNotes { get; set; }
    }
}
