namespace StoryPlanner.Core;

public class ConversationSubjectCoverageTrack
{
    public int Id                        { get; set; }
    public int ConversationSubjectCoverageId { get; set; } // FK to ConversationSubjectCoverage
    public int NoteTrackDefinitionId     { get; set; }     // FK to NoteTrackDefinition
    public bool IsAdded                  { get; set; }     // has a Note been created for this suggestion?
}
