using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }

    public static class UnassignedStory
    {
        public static readonly Story Definition = new()
        {
            Id = 0,   // 0 is never a valid EF-generated PK
            Title = "(Unassigned)",
        };
    }
}
