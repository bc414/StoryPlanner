using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public partial class Note : ObservableObject, IAuditableText
{
    // ... (Existing properties for Id, Content, Foreign Keys, etc. remain unchanged) ...
    public int Id { get; set; }

    [ObservableProperty]
    private DateTime _lastModified = DateTime.UtcNow;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private bool _needsFurtherAnalysis = false;

    [ObservableProperty]
    private int _sortOrder;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderColor))]
    private bool _isIncorporated;

    public int? CodexEntryId { get; set; }
    [JsonIgnore] public CodexEntry? CodexEntry { get; set; }
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
    [NotifyPropertyChangedFor(nameof(BulletColor))]
    private int? _sourceMaterialId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BulletColor))]
    [property: JsonIgnore]
    private SourceMaterial? _sourceMaterial;

    // CHANGED: Default is now Black (#000000) instead of Gray
    public string BulletColor => _sourceMaterial?.ColorHex ?? "#000000";
    
    // Green (#32CD32) if Included, LightGray (#d3d3d3) if not.
    public string BorderColor => IsIncorporated ? "#32CD32" : "#d3d3d3";

    [RelayCommand]
    public void SetSource(SourceMaterial source)
    {
        SourceMaterial = source;
        SourceMaterialId = source.Id;
    }

    [RelayCommand]
    public void ClearSource()
    {
        SourceMaterial = null;
        SourceMaterialId = null;
    }
    
    [RelayCommand]
    public void ToggleIncorporated()
    {
        IsIncorporated = !IsIncorporated;
    }

    [RelayCommand]
    public void ToggleAnalysis()
    {
        NeedsFurtherAnalysis = !NeedsFurtherAnalysis;
    }
}