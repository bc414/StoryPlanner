using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

// Link: PlotPoint <-> Character (Development Arc)
public partial class PlotPointCharacter : ObservableObject, IAuditableText
{
    // --- KEYS (Standard) ---
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int CharacterId { get; set; }
    [JsonIgnore]
    public Character Character { get; set; } = null!;

    // --- PAYLOAD (Observable for Live Edit) ---

    [ObservableProperty]
    private DateTime _lastModified = DateTime.UtcNow;

    [ObservableProperty]
    private CharacterRole _role;

    [ObservableProperty]
    private CharacterDevImpact _developmentImpact;

    [ObservableProperty]
    private string? _developmentNote; // "Loses trust in Command"

    //TODO: not sure if this must be filled. Should it be opt in?
    [ObservableProperty] private int _logicalOrder;
}