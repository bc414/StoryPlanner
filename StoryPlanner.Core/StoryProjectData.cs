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
    public List<Character> Characters { get; set; } = new();
    public List<StoryThread> StoryThreads { get; set; } = new();
    public List<Theme> Themes { get; set; } = new();
    public List<Location> Locations { get; set; } = new();
    public List<CodexEntry> CodexEntries { get; set; } = new();
    
    // --- Meta & Aux ---
    public List<SourceMaterial> SourceMaterials { get; set; } = new();
    public List<GeminiEntry> GeminiEntries { get; set; } = new();
    public List<Idea> Ideas { get; set; } = new();

    // Note: We don't need a separate "Notes" list here because Notes are children 
    // of the entities above (Character.Notes, Theme.Notes, etc.). 
    // However, if you have "Orphan Notes", you might want a list for them.
    // For safety, let's assume notes travel with their parents.
}