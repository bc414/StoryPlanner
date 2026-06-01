using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core.Models
{
    public enum OwnerType
    {
        Subject, //a subject bucket
        PlotPoint, //a scene for the story
        Chapter, //a container for scenes of the story
        PlotPointSubjectLink //connects a plot point with a subject and holds a payload
    }
}
