namespace StoryPlanner.Models;
using System.ComponentModel.DataAnnotations;
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
    public CodexEntry? CodexEntry { get; set; }

    public int? CharacterId { get; set; }
    public Character? Character { get; set; }

    public int? ThemeId { get; set; }
    public Theme? Theme { get; set; }

    // --- THE WEB OF LOGIC (Self-Referencing) ---
    // Note B is true because Note A is true.
    public List<NoteDependency> Prerequisites { get; set; } = new();
    public List<NoteDependency> Dependents { get; set; } = new();

    // --- NARRATIVE USAGE ---
    // Where is this note referenced in the actual story?
    public List<PlotPointNote> PlotPointReferences { get; set; } = new();
}