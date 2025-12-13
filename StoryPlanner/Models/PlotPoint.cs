using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
namespace StoryPlanner.Models;


public class PlotPoint
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; // Short slug
    public string Description { get; set; } = string.Empty; // Your planning notes
    
    // The text intended for final copy-paste
    public string? VerbatimText { get; set; } 
    
    // Workflow tracking
    public DraftStatus Status { get; set; } = DraftStatus.Idea;
    
    // Pacing Control (1-10)
    public int Intensity { get; set; } = 5; 
    
    // Optional World Date (e.g. "1007.12.05") independent of chapter
    public string? WorldDate { get; set; } 

    // --- Relationships ---

    // 1. Structure (Time)
    public int? ChapterId { get; set; }
    [JsonIgnore]
    public Chapter? Chapter { get; set; }
    public int OrderInChapter { get; set; }

    // 2. Geography (Space)
    public int? LocationId { get; set; }
    [JsonIgnore]
    public Location? Location { get; set; }

    // 3. Narrative (Threads)
    public ObservableCollection<PlotPointThread> ThreadAssignments { get; set; } = new();

    // 4. Causality (Self-Referencing)
    public ObservableCollection<PlotPointDependency> Prerequisites { get; set; } = new(); // Causes
    public ObservableCollection<PlotPointDependency> Dependents { get; set; } = new();    // Effects

    // 5. Meaning & Actors (Payloads)
    public ObservableCollection<PlotPointTheme> ThemeAssignments { get; set; } = new();
    public ObservableCollection<PlotPointCharacter> CharacterAppearances { get; set; } = new();

    public ObservableCollection<PlotPointNote> NoteReferences { get; set; } = new();
}