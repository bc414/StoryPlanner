using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public partial class Note : ObservableObject
{
    public int Id { get; set; }
    
    // --- 1. OBSERVABLE PROPERTIES (The UI binds directly to these) ---

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _isStrictRule = true;

    // --- 2. PERSISTENCE HELPERS ---
    
    // Stores the position in the list (updated by Drag & Drop)
    public int SortOrder { get; set; }

    // --- 3. OWNERSHIP (Foreign Keys) ---
    // These generally don't need to be Observable because a Note rarely 
    // "moves" to a different container while you are looking at it.
    
    public int? CodexEntryId { get; set; }
    [JsonIgnore]
    public CodexEntry? CodexEntry { get; set; }
    
    public int? LocationId { get; set; }
    [JsonIgnore]
    public Location? Location { get; set; }

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
    
    // --- 3. Source Material (The Color Driver) ---
    
    [ObservableProperty]
    private int? _sourceMaterialId;

    // We make the navigation property Observable too.
    // When you change the ID, you usually manually update this object 
    // (or EF Core does it), so notifying here ensures the UI color updates.
    [ObservableProperty]
    [property: JsonIgnore] // Keep attributes on the property
    private SourceMaterial? _sourceMaterial;
}