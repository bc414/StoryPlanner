using System.Collections.ObjectModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner; // Adjust namespace to match your project

public class DesignTimeStoryService : IStoryService
{
    // --- Data Collections ---
    public ObservableCollection<Chapter> Chapters { get; }
    public ObservableCollection<StoryThread> StoryThreads { get; }
    public ObservableCollection<Character> Characters { get; }
    public ObservableCollection<Theme> Themes { get; }
    public ObservableCollection<Location> Locations { get; }
    public ObservableCollection<CodexEntry> CodexEntries { get; }
    public ObservableCollection<SourceMaterial> SourceMaterials { get; }
    public ObservableCollection<PlotPoint> PlotPoints { get; }
    public ObservableCollection<GeminiEntry> GeminiEntries { get; }

    // --- State Properties ---
    // We hardcode these so the UI acts as if a file is already open
    public string CurrentFilePath => "Design-Time-Project.db";
    public bool IsProjectLoaded => true;

    // --- Constructor ---
    public DesignTimeStoryService()
    {
        // 1. Ask the Factory for the pure object graph
        // (Make sure DesignDataFactory namespace is imported, likely 'StoryPlanner')
        var data = DesignDataFactory.CreateWorld();

        // 2. Wrap the Lists in ObservableCollections
        // This makes them compatible with the Interface requirements
        Chapters = new ObservableCollection<Chapter>(data.Chapters);
        Characters = new ObservableCollection<Character>(data.Characters);
        StoryThreads = new ObservableCollection<StoryThread>(data.StoryThreads);
        Themes = new ObservableCollection<Theme>(data.Themes);
        Locations = new ObservableCollection<Location>(data.Locations);
        CodexEntries = new ObservableCollection<CodexEntry>(data.CodexEntries);
        SourceMaterials = new ObservableCollection<SourceMaterial>(data.Sources);
        
        // Assuming your Factory generates PlotPoints inside chapters, 
        // we might need to flatten them if the interface expects a master list.
        // If your Factory returns a flat list of points, use that. 
        // Otherwise, aggregate them here:
        var allPoints = data.Chapters.SelectMany(c => c.PlotPoints).ToList();
        PlotPoints = new ObservableCollection<PlotPoint>(allPoints);

        GeminiEntries = new ObservableCollection<GeminiEntry>(); // Empty for design time, or add sample data to factory
    }

    // --- Method Stubs ---
    // These do nothing because the Designer cannot "Run" actions.
    // We return CompletedTask to satisfy the 'async Task' signature.

    public Task CreateProjectAsync(string filePath) => Task.CompletedTask;

    public Task OpenProjectAsync(string filePath) => Task.CompletedTask;

    public Task LoadDataAsync() => Task.CompletedTask;

    public Task SaveAsync() => Task.CompletedTask;

    public Task StoreGeminiEntriesAsync(string file) => Task.CompletedTask;

    public void Dispose()
    {
        // Nothing to dispose in design mode
    }
}