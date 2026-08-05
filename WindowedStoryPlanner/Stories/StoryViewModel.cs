using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Container-only — Story owns no notes and has no OwnerType (a deliberate scope limit; see
/// the plan this shipped with). Unlike NarrativeElementViewModel's subclasses, it derives
/// straight from ObservableObject.
/// </summary>
public partial class StoryViewModel : ObservableObject, IColorHexOwner
{
    private readonly Story _story;
    private readonly IViewModelRegistry _viewModelRegistry;

    public Story Story => _story;
    public int Id => _story.Id;

    public StoryViewModel(Story story, IViewModelRegistry viewModelRegistry)
    {
        _story = story;
        _viewModelRegistry = viewModelRegistry;
    }

    public string Title
    {
        get => _story.Title;
        set => SetProperty(_story.Title, value, _story, (s, n) => s.Title = n);
    }

    public string Abbreviation
    {
        get => _story.Abbreviation;
        set => SetProperty(_story.Abbreviation, value, _story, (s, n) => s.Abbreviation = n);
    }

    /// <summary>
    /// Story colour, "#RRGGBB", edited through <see cref="ColorPickerControl"/> in the library grid
    /// and read by the timeline for plot-point edges. Empty is legal and renders as ChipInk's neutral.
    /// </summary>
    public string ColorHex
    {
        get => _story.ColorHex;
        set => SetProperty(_story.ColorHex, value, _story, (s, n) => s.ColorHex = n);
    }

    // Publication/reading order — also the leading digit of every chapter's and plot point's
    // FullNumber/FullOrder, so a reorder here invalidates those the same way a chapter reorder
    // already does.
    public int OrderIndex
    {
        get => _story.OrderIndex;
        set
        {
            if (SetProperty(_story.OrderIndex, value, _story, (s, n) => s.OrderIndex = n))
                _viewModelRegistry.RaiseLinksInvalidated();
        }
    }
}
