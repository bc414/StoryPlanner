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
        public string Explanation { get; set;  } = string.Empty;
        public bool IsSingleton { get; set; }
        public CognitiveMode CognitiveMode { get; set; }
        public bool SupportsWorldDate { get; set; }
        
    }

    public enum CognitiveMode
    {
        ZeroFocalization,      // User as in-universe historian/physicist
        SceneArchitecture,     // User as director
        Metatextual,           // User to future self, treating the project as a project rather than in-universe
        Analogical,            // User connecting the project world to the real world
        LinguisticExecution,   // User as writer
    }
}
