using System.Collections.Generic;

namespace StoryPlanner.Core;

public class ExportResult
{
    public HashSet<int> FullSubjectIds { get; } = new();
    public HashSet<int> FullPlotPointIds { get; } = new();
    public HashSet<int> ThinPlotPointIds { get; } = new();
    public HashSet<(int PlotPointId, int SubjectId)> ActiveLinks { get; } = new();
}
