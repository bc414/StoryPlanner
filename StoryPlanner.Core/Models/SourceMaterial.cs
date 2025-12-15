using CommunityToolkit.Mvvm.ComponentModel;

namespace StoryPlanner.Core.Models;

public partial class SourceMaterial : ObservableObject
{
    public int Id { get; set; }
    
    [ObservableProperty]
    private string _name = string.Empty;
    
    [ObservableProperty]
    private string _abbreviation = string.Empty;
    
    [ObservableProperty]
    private string _colorHex = "#FFFFFF"; // UI binds to this
    
    public List<Note> Notes { get; set; } = new();
}