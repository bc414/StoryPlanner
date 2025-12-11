namespace StoryPlanner.Models;

// Link: PlotPoint <-> PlotPoint (Causality/Paradox Check)
public class PlotPointDependency
{
    public int PrerequisiteId { get; set; }
    public PlotPoint Prerequisite { get; set; } = null!;

    public int DependentId { get; set; }
    public PlotPoint Dependent { get; set; } = null!;
}