using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public partial class Note : ObservableObject
{
    // ... (Existing properties for Id, Content, Foreign Keys, etc. remain unchanged) ...
    public int Id { get; set; }

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _isStrictRule = true;

    [ObservableProperty]
    private int _sortOrder;

    public int? CodexEntryId { get; set; }
    [JsonIgnore] public CodexEntry? CodexEntry { get; set; }
    public int? LocationId { get; set; }
    [JsonIgnore] public Location? Location { get; set; }
    public int? CharacterId { get; set; }
    [JsonIgnore] public Character? Character { get; set; }
    public int? ThemeId { get; set; }
    [JsonIgnore] public Theme? Theme { get; set; }
    public int? ChapterId { get; set; }
    [JsonIgnore] public Chapter? Chapter { get; set; }
    public int? StoryThreadId { get; set; }
    [JsonIgnore] public StoryThread StoryThread { get; set; }

    // --- Source Material Logic ---

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderColor))]
    private int? _sourceMaterialId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderColor))]
    [property: JsonIgnore]
    private SourceMaterial? _sourceMaterial;

    // CHANGED: Default is now Black (#000000) instead of Gray
    public string BorderColor => _sourceMaterial?.ColorHex ?? "#d3d3d3";

    [RelayCommand]
    public void SetSource(SourceMaterial source)
    {
        SourceMaterial = source;
        SourceMaterialId = source.Id;
    }
}