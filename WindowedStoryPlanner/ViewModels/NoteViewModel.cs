using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteViewModel : ObservableObject
{
    public Note Model { get; }

    public NoteViewModel(Note model)
    {
        Model = model;
    }

    // --- Data Wrappers ---

    public string Content
    {
        get => Model.Content;
        set => SetProperty(Model.Content, value, Model, (u, n) => u.Content = n);
    }

    public bool IsStrictRule
    {
        get => Model.IsStrictRule;
        set => SetProperty(Model.IsStrictRule, value, Model, (u, n) => u.IsStrictRule = n);
    }

    //TODO: Expose the Note model's plot point references
}