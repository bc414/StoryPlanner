using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
