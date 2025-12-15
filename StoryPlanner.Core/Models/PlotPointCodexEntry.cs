using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace StoryPlanner.Core.Models;

public partial class PlotPointCodexEntry : ObservableObject
{
    // --- KEYS (Standard) ---
    public int PlotPointId { get; set; }
    [JsonIgnore]
    public PlotPoint PlotPoint { get; set; } = null!;

    public int CodexEntryId { get; set; }
    [JsonIgnore]
    public CodexEntry CodexEntry { get; set; } = null!;

    // --- PAYLOAD (Observable for Live Edit) ---
    
    // How is this element used? (Mentioned, Active Usage, Subversion, etc.)
    [ObservableProperty]
    private CodexUsageType _usageType = CodexUsageType.Mentioned;

    // The specific context. 
    // E.g. "Referencing Note #3 regarding shield opacity."
    [ObservableProperty]
    private string _commentary = string.Empty;

    //TODO: not sure if this is mandatory. Should it be opt in?
    [ObservableProperty]
    private int _logicalOrder;
}