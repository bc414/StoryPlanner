namespace StoryPlanner.Core.Models;

public enum DraftStatus
{
    Idea = 0,
    Planned = 1,
    Outlined = 2,
    Drafted = 3,       // Prose exists
    Finalized = 4      // Ready to publish
}

public enum ThemeProminence
{
    Motif = 1,         // Background flavor
    Discussion = 2,    // Talked about
    Manifestation = 3, // actively shown in events
    CentralConflict = 4 // The core point of the scene
}

public enum CharacterRole
{
    Background = 0,
    Supporting = 1,
    Protagonist = 2,
    PointOfView = 3    // We are in their head
}

public enum CharacterDevImpact
{
    Static = 0,        // No change
    Reinforced = 1,    // Beliefs strengthened
    Challenged = 2,    // Beliefs shaken
    PivotalShift = 3,  // Major decision/change
    Transformation = 4 // Fundamentally new person
}

public enum CodexCategory
{
    World = 0,
    Nation = 1,
    MagicSystem = 2,
    Technology = 3,
    History = 4,
    Organization = 5
}

public enum CodexUsageType
{
    Mentioned = 0,
    ActiveUsage = 1,   // Used in the scene
    Definition = 2,    // Explained to reader
    Subversion = 3,    // Rule broken/twist
    Failure = 4        // Technology/Magic fails
}