using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core
{
    /// <summary>
    /// Represents a field on an entity that has to be assigned a value.
    ///
    /// Scoped by the compound key (SubjectDefinitionId, OwnerType), exactly like
    /// NoteTrackDefinition — so a property can exist only on, say, Civilizational System subjects.
    /// Note the same asymmetry the track definitions have: OwnerType.PlotPoint and
    /// OwnerType.Chapter rows ignore SubjectDefinitionId (their call sites filter on OwnerType
    /// alone); Subject and PlotPointSubjectLink rows use both.
    ///
    /// SINGLE-SELECT: at most one NarrativePropertyValue per (owner, definition). Absence of a row
    /// is "unset" — a legal, long-lived authorial state, not missing data. There is deliberately no
    /// "(none)" value definition; its Id would be written into ValueDefinitionId and read back as a
    /// real answer. The schema cannot enforce either invariant (no FKs, no unique constraints), so
    /// PlanIntegrity does.
    /// </summary>
    public class NarrativePropertyDefinition
    {
        public int Id { get; set; }
        public int SubjectDefinitionId { get; set; }
        public OwnerType OwnerType { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty; //UI facing text describing what question the value of this property answers
        public string Explanation { get; set; } = string.Empty; //verbose explanation for why the property exists and how to use it

        /// <summary>
        /// The WorkPhase at which leaving this property unset counts as an open gap. Null = never
        /// gates. Reporting only — nothing is ever blocked, and CanPromoteToConfirmed does not
        /// consult this.
        /// </summary>
        public int? GatingWorkPhaseId { get; set; }
    }
}
