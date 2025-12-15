using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class CodexEntryViewModel : EntityViewModel
{
    private readonly CodexEntry _codexEntry;

    public CodexEntryViewModel(CodexEntry codexEntry)
    {
        _codexEntry = codexEntry;

        // Initialize the collection VM using the list from the model
        NoteCollectionViewModel = new NoteCollectionViewModel(codexEntry.Notes);
    }

    // --- Properties Wrapper ---
    // Wraps the Model properties to trigger PropertyChanged notifications for the UI

    public string Title
    {
        get => _codexEntry.Title;
        set => SetProperty(_codexEntry.Title, value, _codexEntry, (u, n) => u.Title = n);
    }

    public string Type
    {
        get => _codexEntry.Type;
        set => SetProperty(_codexEntry.Type, value, _codexEntry, (u, n) => u.Type = n);
    }

    public string Description
    {
        get => _codexEntry.Description;
        set => SetProperty(_codexEntry.Description, value, _codexEntry, (u, n) => u.Description = n);
    }

    public CodexCategory Category
    {
        get => _codexEntry.Category;
        set => SetProperty(_codexEntry.Category, value, _codexEntry, (u, n) => u.Category = n);
    }
}