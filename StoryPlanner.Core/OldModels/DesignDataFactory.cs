using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace StoryPlanner.Core.Models;

public record StoryWorldData(
    List<SourceMaterial> Sources,
    List<Note> Notes,
    List<Subject> Subjects,
    List<Chapter> Chapters,
    List<PlotPoint> PlotPoints
);

public static class DesignDataFactory
{
    public static StoryWorldData CreateWorld()
    {
        
        var sources = new List<SourceMaterial>
        {
            new() { Id = 1, Name = "Canon FIM", Abbreviation = "FIM", ColorHex = "#D63384" },
            new() { Id = 2, Name = "Equestria at War", Abbreviation = "EaW", ColorHex = "#FD7E14" },
            new() { Id = 3, Name = "Original Fiction", Abbreviation = "OC", ColorHex = "#20C997" }
        };

        return null;
    }
}