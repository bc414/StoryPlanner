using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class NarrativePropertyValue
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int ValueDefinitionId { get; set; } //e.g. Positive, a possible value of GoalTrajectory
    }
}
