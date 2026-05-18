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
        public int DisplayOrder { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public string DisplayQuestion { get; set; } = string.Empty;
        public string UsageDirective { get; set;  } = string.Empty;
        public string AuditDirective {  get; set; } = string.Empty;
        public bool IsSingleton { get; set; }
        public CognitiveMode CognitiveMode { get; set; }
        public bool SupportsWorldDate { get; set; }
        public bool SupportsTheme { get; set; }
        public bool CanEditInAuditMode { get; set; }
        
    }

    public enum CognitiveMode
    {
        InUniverseContent,      // User as in-universe historian/physicist
        ReaderExperienceDesign,     // User as director
        NotesToSelf,           // User to future self, treating the project as a project rather than in-universe
        Analogical,            // User connecting the project world to the real world
        LinguisticExecution,   // User as writer
    }

    public static class UnassignedTrack
    {
        public static readonly NoteTrackDefinition Definition = new()
        {
            Id = 0,   // 0 is never a valid EF-generated PK
            TrackName = "Unassigned",
            DisplayQuestion = "Notes not yet assigned to a track",
            DisplayOrder = int.MaxValue,
        };
    }
}
