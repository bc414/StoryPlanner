using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    /// <summary>
    /// Represents a field on an entity that has to be assigned a value
    /// </summary>
    public class NarrativePropertyDefinition
    {
        public int Id { get; set; }
        public int SubjectDefinitionId { get; set; }
        public OwnerType OwnerType { get; set; }
        public string Name { get; set; }
        public string Question { get; set; } //UI facing text describing what question the value of this property answers
        public string Explanation { get; set; } //verbose explanation for why the property exists and how to use it
    }
}
