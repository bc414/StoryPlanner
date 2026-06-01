using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class Subject : INoteable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public int SubjectDefinitionId { get; set; }

        public OwnerType OwnerType => OwnerType.Subject;
    }
}
