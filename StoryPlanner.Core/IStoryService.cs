using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public interface IStoryService : IDisposable
{
    // --- Data Collections ---
    ObservableCollection<Chapter> Chapters { get; }
    ObservableCollection<StoryThread> Threads { get; }
    ObservableCollection<Character> Characters { get; }
    ObservableCollection<Theme> Themes { get; }
    ObservableCollection<Location> Locations { get; }
    ObservableCollection<CodexEntry> CodexEntries { get; }
    ObservableCollection<SourceMaterial> SourceMaterials { get; }
    ObservableCollection<PlotPoint> PlotPoints { get; }
    ObservableCollection<GeminiEntry> GeminiEntries { get; }

    // --- State Properties ---
    string CurrentFilePath { get; }
    bool IsProjectLoaded { get; }

    // --- Methods ---
    Task CreateProjectAsync(string filePath);
    Task OpenProjectAsync(string filePath);
    Task LoadDataAsync();
    Task SaveAsync();
    Task StoreGeminiEntriesAsync(string file);
}