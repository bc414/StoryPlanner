using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public class PlotPoint
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // --- 5 Ws Context ---
    public string Stakes { get; set; } = string.Empty;   // Input (Why)
    public string Synopsis { get; set; } = string.Empty; // Process (What/How)
    public string Outcome { get; set; } = string.Empty;  // Output (Result)
    public string? VerbatimText { get; set; }

    public DraftStatus Status { get; set; } = DraftStatus.Idea;
    public string? WorldDate { get; set; }

    // --- The 5 Axes ---
    public CoreDriver CoreDriver { get; set; } = Models.CoreDriver.Unset;
    public TensionPhase TensionPhase { get; set; } = Models.TensionPhase.Unset;
    public ConflictType ConflictType { get; set; } = Models.ConflictType.Unset;
    public Presentation Presentation { get; set; } = Models.Presentation.Unset;
    // Note: GoalTrajectory is intentionally absent (it lives in ThreadAssignments)

    // --- Relationships ---
    public int? ChapterId { get; set; }
    [JsonIgnore]
    public Chapter? Chapter { get; set; }
    public int OrderInChapter { get; set; }

    public int? LocationId { get; set; }
    [JsonIgnore]
    public Location? Location { get; set; }

    // --- Collections ---
    // We keep ObservableCollection here so the VM can listen to adds/removes
    public ObservableCollection<PlotPointThread> ThreadAssignments { get; set; } = new();
    public ObservableCollection<PlotPointCharacter> CharacterAppearances { get; set; } = new();
    public ObservableCollection<PlotPointTheme> ThemeAssignments { get; set; } = new();
    public ObservableCollection<PlotPointCodexEntry> CodexReferences { get; set; } = new();
    
    public int GetTotalTextLength()
    {
        int length = 0;
        length += Synopsis.Length;
        length += Stakes.Length;
        length += Outcome.Length;
        foreach (var c in CharacterAppearances)
        {
            if (c.DevelopmentNote != null)
            {
                length += c.DevelopmentNote.Length; 
            }
        }
        foreach (var c in CodexReferences)
        {
            if (c.Commentary != null)
            {
                length += c.Commentary.Length; 
            }
        }
        foreach (var c in ThemeAssignments)
        {
            if (c.Commentary != null)
            {
                length += c.Commentary.Length; 
            }
        }
        foreach (var c in ThreadAssignments)
        {
            if (c.ImpactDescription != null)
            {
                length += c.ImpactDescription.Length; 
            }
        }
        
        return length;
    }
}