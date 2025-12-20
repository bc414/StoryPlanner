using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class StoryThreadViewModel : EntityViewModel
{
    private readonly StoryThread _storyThread;
    public StoryThread StoryThread => _storyThread;

    public StoryThreadViewModel(StoryThread storyThread)
    {
        _storyThread = storyThread;

        // Initialize the notes collection generic to the entity
        NoteCollectionViewModel = new NoteCollectionViewModel(storyThread.Notes);
        NoteCollectionViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(NoteCollectionViewModel.IsNoteReorderMode))
            {
                OnPropertyChanged(nameof(IsLinkingMode));
            }
        };
    }

    // --- Properties Wrapper ---

    public string Name
    {
        get => _storyThread.Name;
        set => SetProperty(_storyThread.Name, value, _storyThread, (u, n) => u.Name = n);
    }

    public string Description
    {
        get => _storyThread.Description;
        set => SetProperty(_storyThread.Description, value, _storyThread, (u, n) => u.Description = n);
    }

    public string Icon
    {
        get => _storyThread.Icon;
        set => SetProperty(_storyThread.Icon, value, _storyThread, (u, n) => u.Icon = n);
    }

    public ThreadScope ThreadScope
    {
        get => _storyThread.ThreadScope;
        set => SetProperty(_storyThread.ThreadScope, value, _storyThread, (u, n) => u.ThreadScope = n);
    }
}