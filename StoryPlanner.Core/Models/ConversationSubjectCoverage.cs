namespace StoryPlanner.Core;

public class ConversationSubjectCoverage
{
    public int Id             { get; set; }
    public int ConversationId { get; set; }
    public int SubjectId      { get; set; } // FK to Subject
}
