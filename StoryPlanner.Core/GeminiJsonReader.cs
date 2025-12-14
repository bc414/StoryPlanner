using System.Text.Json.Serialization;

namespace StoryPlanner.Core;

/// <summary>
/// Represents one prompt and response to Gemini
/// </summary>
public class GeminiJsonReader
{
    [JsonPropertyName("header")]
    public string Header { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("products")]
    public List<string> Products { get; set; }

    [JsonPropertyName("activityControls")]
    public List<string> ActivityControls { get; set; }

    [JsonPropertyName("safeHtmlItem")]
    public List<SafeHtmlItem> SafeHtmlItems { get; set; }
}

public class SafeHtmlItem
{
    [JsonPropertyName("html")]
    public string Html { get; set; }
}