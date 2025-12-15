using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class ChapterViewModel : EntityViewModel
{
    private readonly Chapter _chapter;

    public ChapterViewModel(Chapter chapter)
    {
        _chapter = chapter;

        // Initialize the notes collection generic to the entity
        NoteCollectionViewModel = new NoteCollectionViewModel(chapter.Notes);
    }

    // --- Properties Wrapper ---

    public string Title
    {
        get => _chapter.Title;
        set => SetProperty(_chapter.Title, value, _chapter, (u, n) => u.Title = n);
    }

    public string Summary
    {
        get => _chapter.Summary;
        set => SetProperty(_chapter.Summary, value, _chapter, (u, n) => u.Summary = n);
    }

    public string Description
    {
        get => _chapter.Description;
        set => SetProperty(_chapter.Description, value, _chapter, (u, n) => u.Description = n);
    }

    public int OrderIndex
    {
        get => _chapter.OrderIndex;
        set => SetProperty(_chapter.OrderIndex, value, _chapter, (u, n) => u.OrderIndex = n);
    }
}