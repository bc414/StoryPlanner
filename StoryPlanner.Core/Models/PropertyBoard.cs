using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core
{
    /// <summary>
    /// A named set of narrative properties held under comparison — the scope for the pairwise
    /// grids and the subject tree.
    ///
    /// The board exists because membership must be AUTHORED rather than automatic. Narrative
    /// properties serve two purposes now: project-management bookkeeping (a gating WorkPhase, the
    /// Property Gaps report) and story data (the political axes of the fabula). A view that
    /// cross-tabulated every property in a subject type's scope would mix the two the moment a
    /// bookkeeping property is added. Membership is opt-in, one board at a time.
    ///
    /// Members are the NarrativePropertyDefinition rows carrying this board's id, filtered to
    /// OwnerType.Subject; ordering is their own DisplayOrder. A property belongs to at most one
    /// board — a many-to-many join was considered and is not needed.
    /// </summary>
    public class PropertyBoard
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>The subject type this board compares. Every member property must be scoped to
        /// the same one; PlanIntegrity reports a mismatch.</summary>
        public int SubjectDefinitionId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether "(unset)" is a band on each axis of this board's grids.
        ///
        /// Per board, and that is the whole point. Off: a subject with no value on either axis of
        /// a grid is simply absent from that grid — correct for the political axes, where every
        /// system is meant to sit somewhere and a blank margin would be noise. On: "(unset)" is an
        /// ordinary band, correct for a bookkeeping board where "not yet decided" is one real
        /// answer among several.
        ///
        /// Not a runtime checkbox: it records what the board is for. Note the consequence — with
        /// this off, grid populations legitimately differ from each other and from the subject
        /// count, and that is not a defect.
        /// </summary>
        public bool IncludeUnsetBand { get; set; }
    }
}
