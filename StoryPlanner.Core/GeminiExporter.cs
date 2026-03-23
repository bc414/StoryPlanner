using System.Text;
using System.Text.Json;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

public class GeminiExporter
{
    public void ConvertJsonToMarkdownFiles(string jsonFilePath, string outputDirectory)
    {
        // 1. Read the raw JSON content
        string jsonContent = File.ReadAllText(jsonFilePath);

        // 2. Deserialize using your existing Reader class
        // Note: Google Takeout often exports valid JSON, but ensure your file is a list [...]
        var rawEntries = JsonSerializer.Deserialize<List<GeminiJsonReader>>(jsonContent);

        // 3. Convert to your Clean Domain Models using your existing method
        var cleanEntries = GeminiEntry.FromJson(rawEntries);

        // Ensure output directory exists
        Directory.CreateDirectory(outputDirectory);

        int count = 0;
        int iteration = 0;
        StringBuilder mdBuilder = new StringBuilder();
        foreach (var entry in cleanEntries)
        {
            // 4. Generate the Markdown Content
            count++;
            

            mdBuilder.AppendLine($"# {SanitizeTitle(entry.Prompt)}"); // Title
            mdBuilder.AppendLine();
            mdBuilder.AppendLine($"**Date:** {entry.Time:yyyy-MM-dd HH:mm}");
            mdBuilder.AppendLine();
            mdBuilder.AppendLine($"## Prompt {count}");
            mdBuilder.AppendLine($"> {entry.Prompt}"); // Blockquote for prompt
            mdBuilder.AppendLine();
            mdBuilder.AppendLine($"## Gemini Response to prompt {count}");
            
            // Convert the HTML response to Markdown
            string markdownResponse = HtmlToMarkdown.Convert(entry.ResponseHtml);
            mdBuilder.AppendLine(markdownResponse);

            // 5. Generate a safe filename
            // Using a sanitized version of the prompt + date to avoid collisions
            string safeTitle = string.Join("_", entry.Prompt.Split(Path.GetInvalidFileNameChars()))
                                     .Trim().Replace(" ", "_");
            
            // Truncate filename if too long
            if (safeTitle.Length > 50) safeTitle = safeTitle.Substring(0, 50);
            
            mdBuilder.AppendLine();
            
            if (count >= 20 || mdBuilder.Length >= 2500000)
            {
                // 6. Write to file
                iteration++;
                string fileName = $"{iteration}.md";
                string fullPath = Path.Combine(outputDirectory, fileName);
                File.WriteAllText(fullPath, mdBuilder.ToString());
                mdBuilder = new StringBuilder();
                count = 0;
            }

            
        }
    }

    private string SanitizeTitle(string title)
    {
        // Basic cleanup if the prompt is very long (often the prompt IS the title in Takeout)
        if (title.Length > 100) return title.Substring(0, 97) + "...";
        return title;
    }
}