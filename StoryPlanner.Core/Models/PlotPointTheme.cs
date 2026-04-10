using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

// Link: PlotPoint <-> Theme (Thematic Argument)
public partial class PlotPointTheme : ObservableObject, IAuditableText
{
    // --- KEYS (Standard) ---
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int ThemeId { get; set; }
    [JsonIgnore]
    public Theme Theme { get; set; } = null!;

    // --- PAYLOAD (Observable for Live Edit) ---

    [ObservableProperty]
    private DateTime _lastModified = DateTime.UtcNow;

    [ObservableProperty]
    private ThemeProminence _prominence;

    [ObservableProperty]
    private string? _commentary; // "Shows the failure of diplomacy"
}