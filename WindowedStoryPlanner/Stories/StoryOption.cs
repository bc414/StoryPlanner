using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Display option for story pickers (the Chapters tab's filter combo, the Move… dialog's target
/// combo). Wraps either a real <see cref="StoryViewModel"/> or the "(Unassigned)" sentinel
/// (Id 0, see <see cref="UnassignedStory"/>) — both are legal <c>Chapter.StoryId</c> values, so
/// both must be pickable.
/// </summary>
public sealed record StoryOption(int Id, string Label)
{
    /// <summary>Sentinel meaning "no filter" — never a legal Chapter.StoryId. Only used in filter lists.</summary>
    public const int AllStoriesId = -1;

    public override string ToString() => Label;

    /// <summary>Real stories (reading order) plus "(Unassigned)" last — for a Move… target picker.</summary>
    public static ObservableCollection<StoryOption> BuildTargetList(IEnumerable<StoryViewModel> stories)
    {
        var list = new ObservableCollection<StoryOption>();
        foreach (var s in stories.OrderBy(s => s.OrderIndex))
            list.Add(new StoryOption(s.Id, s.Title));
        list.Add(new StoryOption(UnassignedStory.Definition.Id, UnassignedStory.Definition.Title));
        return list;
    }

    /// <summary>"(All Stories)" first, then real stories, then "(Unassigned)" — for the Chapters tab filter.</summary>
    public static ObservableCollection<StoryOption> BuildFilterList(IEnumerable<StoryViewModel> stories)
    {
        var list = new ObservableCollection<StoryOption> { new(AllStoriesId, "(All Stories)") };
        foreach (var s in stories.OrderBy(s => s.OrderIndex))
            list.Add(new StoryOption(s.Id, s.Title));
        list.Add(new StoryOption(UnassignedStory.Definition.Id, UnassignedStory.Definition.Title));
        return list;
    }
}
