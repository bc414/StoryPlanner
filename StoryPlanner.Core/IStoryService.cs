using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public interface IStoryService : IDisposable
{
    // --- Data Collections ---
    ObservableCollection<Subject> Subjects { get; }
    ObservableCollection<PlotPoint> PlotPoints { get; }
    ObservableCollection<PlotPointSubjectLink> PlotPointsSubjectLinks { get; }
    ObservableCollection<Chapter> Chapters { get; }
    ObservableCollection<Note> Notes { get; }
    ObservableCollection<SubjectDefinition> SubjectDefinitions { get; }
    ObservableCollection<NoteTrackDefinition> NoteTrackDefinitions { get; }
    ObservableCollection<NarrativePropertyDefinition> NarrativePropertyDefinitions { get; }
    ObservableCollection<NarrativePropertyValueDefinition> NarrativePropertyValueDefinitions { get; }
    ObservableCollection<NarrativePropertyValue> NarrativePropertyValues { get; }
    ObservableCollection<SourceMaterial> SourceMaterials { get; }
    ObservableCollection<GeminiEntry> GeminiEntries { get; }
    ObservableCollection<Idea> Ideas { get; }
    // Inside IStoryService.cs

    // --- State Properties ---
    string CurrentFilePath { get; }
    bool IsProjectLoaded { get; }

    // --- Methods ---
    Task CreateProjectAsync(string filePath);
    Task OpenProjectAsync(string filePath);
    Task SaveAsync();
    Task StoreGeminiEntriesAsync(string file);
    string GetFullProjectJson();
    string GetAiContextJson(bool includeVerbatim);
    // Inside IStoryService.cs
    NotePropertyStats GetNoteStatsByCondition(string statName, Func<Note, bool> condition);
    void DeleteNote(Note note);
    Task PurgeUnassignedNotesAsync();
    IEnumerable<IAuditableText> GetAllAuditableTexts();
}