using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Proposition {  get; set; } = string.Empty;
    }
}
