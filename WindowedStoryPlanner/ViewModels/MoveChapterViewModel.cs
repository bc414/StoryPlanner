using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Pure data holder for the "Move…" dialog — pick a target story, an anchor chapter within it,
/// and before/after. Follows the same convention as ConversationPickerViewModel/ScanPreviewViewModel:
/// the window's code-behind sets DialogResult on its Move/Cancel buttons; the caller
/// (ChapterLibraryViewModel.OpenMoveDialog) reads the selections after ShowDialog() returns true
/// and performs the actual reassignment/renumbering. This class does no mutation itself.
/// </summary>
public partial class MoveChapterViewModel : ObservableObject
{
    private readonly IViewModelRegistry _registry;

    public ChapterViewModel Chapter { get; }
    public ObservableCollection<StoryOption> StoryOptions { get; }
    public ObservableCollection<ChapterViewModel> AnchorCandidates { get; } = new();

    [ObservableProperty]
    private StoryOption? _selectedStory;

    [ObservableProperty]
    private ChapterViewModel? _selectedAnchor;

    [ObservableProperty]
    private bool _placeBefore = true;

    public MoveChapterViewModel(ChapterViewModel chapter, IViewModelRegistry registry)
    {
        Chapter = chapter;
        _registry = registry;
        StoryOptions = StoryOption.BuildTargetList(registry.AllStoryViewModels);

        _selectedStory = StoryOptions.FirstOrDefault(s => s.Id == chapter.StoryId) ?? StoryOptions.FirstOrDefault();
        RebuildAnchorCandidates();
    }

    partial void OnSelectedStoryChanged(StoryOption? value) => RebuildAnchorCandidates();

    private void RebuildAnchorCandidates()
    {
        AnchorCandidates.Clear();
        if (SelectedStory is null) return;

        foreach (var c in _registry.AllChapterViewModels
                     .Where(c => c.StoryId == SelectedStory.Id && c.Id != Chapter.Id)
                     .OrderBy(c => c.OrderIndex))
            AnchorCandidates.Add(c);

        // Default to appending at the end of the target story.
        SelectedAnchor = AnchorCandidates.LastOrDefault();
        PlaceBefore = false;
    }
}
