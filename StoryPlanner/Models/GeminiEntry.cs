using StoryPlanner.Components;

namespace StoryPlanner.Models;

public class GeminiEntry
{
    public int Id { get; set; }
    
    /// <summary>
    /// The date and time of the prompt and response
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// My prompt to Gemini
    /// </summary>
    public string Prompt { get; set; } = null!;

    /// <summary>
    /// Gemini's response formated as Html
    /// </summary>
    public string ResponseHtml { get; set; } = null!;
    
    /// <summary>
    /// Whether I have read through the contents and added the details to the StoryPlanner
    /// </summary>
    public bool IsAnalyzed { get; set; }

    public static GeminiEntry FromJson(GeminiJsonReader input)
    {
        GeminiEntry answer = new GeminiEntry();
        answer.Time = input.Time;
        answer.Prompt = input.Title.Substring(9); //Strip out "Prompted " from the beginning
        answer.ResponseHtml = input.SafeHtmlItems[0].Html;
        return answer;
    }
}