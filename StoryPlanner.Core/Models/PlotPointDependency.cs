using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

// Link: PlotPoint <-> PlotPoint (Causality/Paradox Check)
public class PlotPointDependency
{
    public int PrerequisiteId { get; set; }
    [JsonIgnore]
    public PlotPoint Prerequisite { get; set; } = null!;

    public int DependentId { get; set; }
    [JsonIgnore]
    public PlotPoint Dependent { get; set; } = null!;
}