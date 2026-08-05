using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core
{
    /// <summary>
    /// Declares a kind of edge one Subject may draw to another — "Ancestor", "Rival of",
    /// "Splintered from". A Type Object row exactly like NoteTrackDefinition and
    /// NarrativePropertyDefinition: adding a relation is data entry, not a code change.
    ///
    /// Scoped by SubjectDefinitionId (the SOURCE type) plus TargetSubjectDefinitionId (what may
    /// be pointed AT). Unlike the property/track definitions there is no OwnerType here, and
    /// deliberately so: both endpoints are Subjects, always. Making this polymorphic would
    /// reproduce NarrativePropertyValue's documented trap, where the missing OwnerType forces the
    /// same three-hop trace at five separate call sites.
    ///
    /// Edges carry no notes. There is no OwnerType.SubjectRelation and no NarrativeElementViewModel
    /// subclass for one — why a succession happened belongs on the successor's Causality of
    /// Creation / History track, which is where that content already lives in the real files.
    /// Should that ever change it is one enum value and one view model, with no data migration.
    /// </summary>
    public class SubjectRelationDefinition
    {
        public int Id { get; set; }

        /// <summary>The subject type that may hold this edge.</summary>
        public int SubjectDefinitionId { get; set; }

        /// <summary>The subject type the edge may point at. Equal to SubjectDefinitionId for the
        /// ordinary same-type case, and REQUIRED to be equal when FormsHierarchy is set.</summary>
        public int TargetSubjectDefinitionId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>Reading in the stored direction, source → target ("Ancestor").</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Reading against the stored direction, target → source ("Succeeded by"). Used
        /// as the label when the tree view walks inverted; never a second stored edge.</summary>
        public string InverseName { get; set; } = string.Empty;

        /// <summary>UI-facing text describing what question this edge answers.</summary>
        public string Question { get; set; } = string.Empty;

        /// <summary>Verbose explanation of why the relation exists and how to use it.</summary>
        public string Explanation { get; set; } = string.Empty;

        /// <summary>At most one target per (subject, relation). The same invariant single-select
        /// narrative properties have, and just as unenforceable in a schema with no unique
        /// constraints — PlanIntegrity reports a second as subjectrelation.duplicate_for_single.
        /// </summary>
        public bool IsSingle { get; set; }

        /// <summary>
        /// This edge is a parent pointer: acyclic, and walkable as a tree. The subject picker
        /// excludes the subject's own descendants so a cycle cannot be authored, and PlanIntegrity
        /// audits for cycles arriving by other routes.
        ///
        /// An explicit flag rather than something derived from IsSingle plus same-type, because a
        /// single same-type relation can legitimately be cyclic — a symmetric "Rival of" is the
        /// obvious case. Requires SubjectDefinitionId == TargetSubjectDefinitionId: a chain that
        /// changes subject type partway down is not a chain.
        /// </summary>
        public bool FormsHierarchy { get; set; }
    }
}
