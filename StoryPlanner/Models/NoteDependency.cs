using System.Text.Json.Serialization;
namespace StoryPlanner.Models;

public class NoteDependency
{
    public int PrerequisiteId { get; set; }
    [JsonIgnore]
    public Note Prerequisite { get; set; } = null!;

    public int DependentId { get; set; }
    [JsonIgnore]
    public Note Dependent { get; set; } = null!;

    // "Logical Consequence", "Character Motivation", "Thematic Echo"
    public string Type { get; set; } = string.Empty; 
}