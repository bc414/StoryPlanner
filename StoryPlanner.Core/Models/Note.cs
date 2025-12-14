using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public class Note
{
    public int Id { get; set; }
    
    // The core info: "Mag-Rifles jam in sand" or "Celestia loves cake"
    public string Content { get; set; } = string.Empty; 
    
    // Is this an immutable fact or a soft guideline?
    public bool IsStrictRule { get; set; } = true; 

    // --- OWNERSHIP (Polymorphic-ish) ---
    // A Note usually belongs to ONE context.
    
    public int? CodexEntryId { get; set; }
    [JsonIgnore]
    public CodexEntry? CodexEntry { get; set; }

    public int? CharacterId { get; set; }
    [JsonIgnore]
    public Character? Character { get; set; }

    public int? ThemeId { get; set; }
    [JsonIgnore]
    public Theme? Theme { get; set; }
    
    public int? ChapterId { get; set; }
    [JsonIgnore]
    public Chapter? Chapter { get; set; }
    
    public int? StoryThreadId { get; set; }
    [JsonIgnore]
    public StoryThread StoryThread { get; set; }
    
    public int? SourceMaterialId { get; set; }
    [JsonIgnore]
    public SourceMaterial? SourceMaterial { get; set; }

    // --- THE WEB OF LOGIC (Self-Referencing) ---
    // Note B is true because Note A is true.
    public List<NoteDependency> Prerequisites { get; set; } = new();
    public List<NoteDependency> Dependents { get; set; } = new();

    // --- NARRATIVE USAGE ---
    // Where is this note referenced in the actual story?
    public List<PlotPointNote> PlotPointReferences { get; set; } = new();
}