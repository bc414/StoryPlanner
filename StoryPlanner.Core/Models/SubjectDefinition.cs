using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class SubjectDefinition
    {
        public int Id { get; set; }
        public string SubjectType { get; set; }
        public int DisplayOrder { get; set; }

        //Can add properties for tooltips and displays later, but not configurable stuff that goes into note tracks
    }
}
