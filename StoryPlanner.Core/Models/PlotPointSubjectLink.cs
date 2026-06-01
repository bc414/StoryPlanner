using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public class PlotPointSubjectLink : INoteable
    {
        public int Id { get; set; }
        public int PlotPointId { get; set; }
        public int SubjectId { get; set; }

        public OwnerType OwnerType => OwnerType.PlotPointSubjectLink;
    }
}
