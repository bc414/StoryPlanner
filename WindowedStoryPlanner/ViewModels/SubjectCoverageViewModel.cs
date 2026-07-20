using StoryPlanner.Core.Models;
using System.Collections.Generic;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Represents one subject + its associated tracks for a conversation's routing header.
/// </summary>
public class SubjectCoverageViewModel
{
    public SubjectViewModel                      Subject { get; }
    public IReadOnlyList<CoverageTrackViewModel> Tracks  { get; }
    public ConversationSubjectCoverage           Model   { get; }

    public string SubjectName => Subject.Name;

    public SubjectCoverageViewModel(
        SubjectViewModel subject,
        IReadOnlyList<CoverageTrackViewModel> tracks,
        ConversationSubjectCoverage model)
    {
        Subject = subject;
        Tracks  = tracks;
        Model   = model;
    }
}
