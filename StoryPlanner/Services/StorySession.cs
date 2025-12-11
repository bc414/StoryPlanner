namespace StoryPlanner.Services;

public class StorySession
{
    // Default to a specific file for now, or start blank
    public string CurrentStoryFilePath { get; set; } = "TheLionessOfTallTale.db"; 

    public string GetConnectionString()
    {
        return $"Data Source={CurrentStoryFilePath}";
    }
}