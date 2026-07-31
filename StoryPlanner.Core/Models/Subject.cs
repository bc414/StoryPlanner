using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core
{
    public class Subject : INoteable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public int SubjectDefinitionId { get; set; }

        /// <summary>Timeline column this subject's dated notes appear in. 0 = the permanent
        /// "(Unplaced)" sentinel, same pattern as Chapter.StoryId. Single-select by design:
        /// a cross-theater phenomenon is its own subject placed where the thing IS (e.g. "The
        /// Aquileian Cartel in Skyfall" sits in Skyfall) — horizontal co-occurrence at the same
        /// date is the causality signal.</summary>
        public int TheaterId { get; set; }

        public OwnerType OwnerType => OwnerType.Subject;
    }
}
