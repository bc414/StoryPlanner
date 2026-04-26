using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    /// <summary>
    /// Specifies a valid value for a particular property
    /// </summary>
    public class NarrativePropertyValueDefinition
    {
        public int Id { get; set; }
        public int NarrativePropertyDefinitionId { get; set; }
        public string ValueName { get; set; }
        public string Description { get; set; }
    }
}
