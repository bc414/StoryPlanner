using StoryPlanner.Core.Models;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core;

public class StoryProjectData
{
    // Metadata
    public string Title { get; set; } = "Story Project";
    public DateTime ExportDate { get; set; } = DateTime.Now;
    public int Version { get; set; } = 1;

    // --- The Core Content ---
    public List<Chapter> Chapters { get; set; } = new();
    public List<PlotPoint> PlotPoints { get; set; } = new(); // Optional: Usually nested in Chapters, but keeping a flat list can sometimes help validation
    
    // --- The World Bible ---
    public List<Subject> Subjects { get; set; } = new();
    public List<Note> Notes { get; set; } = new();
    
    // --- Meta & Aux ---
    public List<SourceMaterial> SourceMaterials { get; set; } = new();
    public List<GeminiEntry> GeminiEntries { get; set; } = new();
    public List<Idea> Ideas { get; set; } = new();
}