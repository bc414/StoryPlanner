using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class NoteTrackDefinition
    {
        public int Id { get; set; }
        public int SubjectDefinitionId { get; set; }
        public OwnerType OwnerType { get; set; }
        public int ExpansionModeDisplayOrder { get; set; }
        public int LinkingModeDisplayOrder { get; set; }
        public int GardenerModeDisplayOrder { get; set; }
        public int AuditModeDisplayOrder { get; set; }
        public int SceneDesignModeDisplayOrder { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public string DisplayQuestion { get; set; } = string.Empty;
        public string UsageDirective { get; set;  } = string.Empty;
        public string AuditDirective {  get; set; } = string.Empty;
        public bool IsSingleton { get; set; }
        public TrackType TrackType { get; set; }
        public bool SupportsWorldDate { get; set; }
        public bool SupportsTheme { get; set; }
        public bool CanEditInAuditMode { get; set; }
        
    }

    public enum TrackType
    {
        Unset,
        Ontology, //Ontologist
        Civilization, //Analytical worldbuilder - things made by agents in-universe in response to their Ontology
        History, //Historian
        Characterization, //Psychologist
        PageDesign, //Director
        WorldInference, //Reader psychologist
        ThematicEvidence, //Philosopher
        NotesToSelf, //Project manager
        Analogies, //real-world inspiration
        NarrativeArchitecture,
        Canon,
        Allegories //Social commentary
    }

    public static class UnassignedTrack
    {
        public static readonly NoteTrackDefinition Definition = new()
        {
            Id = 0,   // 0 is never a valid EF-generated PK
            TrackName = "Unassigned",
            DisplayQuestion = "Notes not yet assigned to a track",
            ExpansionModeDisplayOrder = int.MaxValue,
            LinkingModeDisplayOrder = int.MaxValue,
            GardenerModeDisplayOrder = int.MaxValue,
            AuditModeDisplayOrder = int.MaxValue,
            SceneDesignModeDisplayOrder = int.MaxValue,
        };
    }

    public static class TrackTypeExtensions
    {
        public static string GetCognitiveMode(this TrackType trackType)
        {
            switch (trackType)
            {
                case TrackType.Ontology:
                    return "Ontology Notes (Layer 1) - written by a world builder in god-mode defining what the rules of the fictional universe are";
                case TrackType.Civilization:
                    return "Civilization Notes (Layer 3) - written by a world builder building things made by in-universe agents in response to their ontology";
                case TrackType.History:
                    return "History Notes (Layer 2) - written by an in-universe historian reporting facts";
                case TrackType.Characterization:
                    return "Characterization Notes (Layer 3) - written by a psychologist asserting the truth of what makes a character who they are";
                case TrackType.PageDesign:
                    return "Page Notes - written by a director staging what the reader observes on the page";
                case TrackType.WorldInference:
                    return "World Inference Notes - written by a reader psychologist designing what a reader should infer from what is on the page";
                case TrackType.ThematicEvidence:
                    return "Thematic Evidence Notes (Layer 5) - written by a philosopher deploying evidence for a reader to arrive at a thematic proposition";
                case TrackType.NotesToSelf:
                    return "Notes to self - written by the author, talking about the story planning project and process";
                case TrackType.Analogies:
                    return "Analogical Notes - written by the author to document real-world inspiration (can be historical or present-day inspiration)";
                case TrackType.Allegories:
                    return "Allegorical Notes - written by the author as a reader inference target for social commentary";
                case TrackType.NarrativeArchitecture:
                    return "Narrative Architecture Notes (Layer 4) - written by the author planning out how the reader should experience the story world";
                case TrackType.Canon:
                    return "Canon Notes - written by the author to constrain the story within established canon, build upon and recontextualize canon";
                default:
                    return "Unset Notes";
            }
        }
    }
}
