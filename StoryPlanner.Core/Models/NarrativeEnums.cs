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
    Unset = 0,
    Motif = 1,         // Background flavor
    Discussion = 2,    // Talked about
    Demonstration = 3, // actively shown in events
    CentralConflict = 4 // The core point of the scene
}

public enum CharacterRole
{
    Unset = 0,
    Supporting = 1,
    Protagonist = 2,
    PointOfView = 3,    // We are in their head
    Antagonist = 4
}

public enum CharacterDevImpact
{
    Static = 0,        // No change
    CoreValueLearned = 1,    // New belief acquired
    CoreValueDemonstration = 2,    //They teach others
    CoreValueChange = 3,  // Major decision/change
}

public enum CodexCategory
{
    Unset = 0,
    Nation = 1,
    MagicSystem = 2,
    Technology = 3,
    History = 4,
    Organization = 5,
    WorldRules = 6,
    SocietalDifferences = 7,
    Concept = 8,
    Backstory = 9,
}

public enum CodexUsageType
{
    Mentioned = 0,
    ActiveUsage = 1,   // Used in the scene
    Definition = 2,    // Explained to reader
    Subversion = 3,    // Rule broken/twist
    Failure = 4        // Technology/Magic fails
}

// 5 Axes of Plot Point analysis
// 1. Core Driver - what is the driver of this scene?
// 2. Scene Phase - Is it a local climax, reflection, or setup?
// 3. Conflict type - who is the opponent?
// 4. Goal Trajectory - are the goals getting closer or further? (additional fields accompanying it to describe what goal and how the plot point moves the goal)
// 5. Presentation - what lens is the story being told through here? (Real time vs introspection vs summary, flashback, etc)

// 1. THE ENGINE (Topic)
// The driver is whatever determines the outcome of the scene.
public enum CoreDriver
{
    Unset = 0,
    World,      // Entering a new place (physically or culturally) and exits
    Mystery,    // There is a lack of information. Must find the answer.
    Character,  // Internal disatisfaction. Must result in character development or change
    Action      // The status quo is being broken. There must be a resolution and a new status quo.
}

// 2. THE PACE (Tension)
public enum TensionPhase
{
    Unset = 0,       // [Red Border]

    // --- Baseline (Static) ---
    Situation,       // Establishing the 'Normal' / Where we are. (Replaces Grounding)

    // --- The Climb (Rising Energy) ---
    Spark,           // The disruption / Ignition.
    Anticipation,    // The dread / planning / waiting.
    Escalation,      // The friction / conflict / heat.

    // --- The Peak (Max Energy) ---
    LocalClimax,     // The Turning Point.

    // --- The Comedown (Falling Energy) ---
    Impact,          // Phase 1: The immediate force (Good or Bad). Shock/Celebration.
    Resolution,      // Phase 2: The New Normal. Processing. Safety.

    // --- Utility ---
    Transition       // Bridge
}

// 3. THE OPPOSITION - Who is the *enemy* in this scene?
// This uses FLAGS because scenes often have layered conflicts.
[Flags]
public enum ConflictType
{
    Unset = 0,

    Peaceful = 1 << 0, // No conflict at all (for scenes that are purely reflective or expositional)

    // 1. You vs. Someone Else (The Rival)
    Interpersonal = 1 << 1,

    // 2. You vs. The World (The Setting)
    Environmental = 1 << 2,

    // 3. You vs. Yourself (The Flaw)
    Internal = 1 << 3,

    // 4. You vs. The Culture (The System)
    Societal = 1 << 4,

    // 5. You vs. The Machine (The Logic/Magic/Industry)
    Technological = 1 << 5,

    // 6. You vs. The Universe (The Destiny/Gods)
    Existential = 1 << 6
}

// 4. THE RESULT (Change)
// "How did the character's Condition change regarding their goals?" What is the trajectory of their goals?
public enum GoalTrajectory
{
    Unset = 0,
    Stagnant,    // No progress (Filler/Neutral)
    Positive,          // Success / Progress (+)
    Negative,         // Setback / Failure (-)
    Triumph,    // Major leap forward (++)
    Disaster, // Major disaster (--)
}

// 5. NARRATIVE MODE - The Camera & Time
// How is the reader experiencing this event?
public enum Presentation
{
    Unset = 0,
    RealTime,       // Default: Dialogue and Action as it happens
    Summary,            // Compression: "They walked for three days."
    Flashback,          // Time Jump: Events from the past
    Montage,            // Collection: Multiple short snippets over time
    Artifact,         // Artifact: A letter, news report, or log entry
    InternalThought // Internal: Pure thought process
}

public enum ThreadScope
{
    /// <summary>
    /// The specific focus of the current chapter/sequence.
    /// It must be dealt with 'Immediately' to move the serial story forward.
    /// (e.g., Survive the Blizzard, Win the Council Vote).
    /// </summary>
    Immediate = 0,
    /// <summary>
    /// Spans the entire story or large sections. Distributed and persistent.
    /// (e.g., The War, The Redemption Arc, The Mystery).
    /// </summary>
    Overarching = 1
}

public enum IdeaState
{
    Written = 0,
    PartiallyAnalyzed = 1,
    FullyAnalyzed = 2,
}