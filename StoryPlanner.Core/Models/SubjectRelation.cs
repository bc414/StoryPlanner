using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core
{
    /// <summary>
    /// One authored edge: this Subject points at that Subject, under this relation definition.
    ///
    /// Absence of a row is "unset" — a legal, long-lived authorial state, not missing data, and
    /// there is deliberately no sentinel target row (its id would be stored and read back as a
    /// real answer, the same reasoning as NarrativePropertyValueDefinition's missing "(none)").
    ///
    /// No OwnerType column is needed and none should be added: both ends are Subjects, and
    /// RelationDefinitionId resolves both types. Assignment is authorial — an edge is NEVER
    /// derived from names, note text, dates, or shared vocabulary. The real file's one recorded
    /// succession (Griffonian Republic ← Grover III's Enlightenment) shares no name token with its
    /// target and skips three intervening regimes, which is exactly why nothing may infer these.
    /// </summary>
    public class SubjectRelation
    {
        public int Id { get; set; }
        public int RelationDefinitionId { get; set; }

        /// <summary>The subject holding the edge.</summary>
        public int SubjectId { get; set; }

        /// <summary>The subject pointed at.</summary>
        public int TargetSubjectId { get; set; }

        /// <summary>Ordering among several targets on a non-single relation. Ignored when the
        /// definition is IsSingle.</summary>
        public int SortOrder { get; set; }
    }
}
