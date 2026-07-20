using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// One suggested (subject, track) coverage entry in a conversation's routing header.
/// IsAdded reflects whether a Note has been created for this suggestion — a plain boolean,
/// not a triage tri-state, since un-added suggestions need no explicit dismissal.
/// </summary>
public partial class CoverageTrackViewModel : ObservableObject
{
    private readonly IStoryService _storyService;

    public ConversationSubjectCoverageTrack Model { get; }
    public NoteTrackDefinitionViewModel      Track { get; }
    public SubjectViewModel                  Subject { get; }

    public string TrackName => Track.TrackName;

    [ObservableProperty]
    private bool _isAdded;

    public CoverageTrackViewModel(
        ConversationSubjectCoverageTrack model,
        NoteTrackDefinitionViewModel track,
        SubjectViewModel subject,
        IStoryService storyService)
    {
        Model         = model;
        Track         = track;
        Subject       = subject;
        _storyService = storyService;
        _isAdded      = model.IsAdded; // direct field assign — no save triggered on init
    }

    partial void OnIsAddedChanged(bool value)
    {
        Model.IsAdded = value;
        _ = _storyService.SaveAsync();
    }
}
