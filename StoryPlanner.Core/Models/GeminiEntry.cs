using CommunityToolkit.Mvvm.ComponentModel;

namespace StoryPlanner.Core.Models;

public partial class GeminiEntry : ObservableObject
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
    [ObservableProperty] private bool _isAnalyzed;

    public static List<GeminiEntry> FromJson(List<GeminiJsonReader> inputs)
    {
        List<GeminiEntry> answers = new List<GeminiEntry>();
        foreach (GeminiJsonReader input in inputs)
        {
            if (!input.Title.Contains("Gemini Canvas"))
            {
                GeminiEntry answer = new GeminiEntry();
                answer.Time = input.Time.ToLocalTime();
                answer.Prompt = input.Title.Substring(9); //Strip out "Prompted " from the beginning
                answer.ResponseHtml = input.SafeHtmlItems != null && input.SafeHtmlItems.Count > 0 ? input.SafeHtmlItems[0].Html : "No Response";
                answers.Add(answer);
            }
        }

        return answers;
    }
}