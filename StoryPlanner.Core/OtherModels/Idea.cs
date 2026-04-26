using CommunityToolkit.Mvvm.ComponentModel;

namespace StoryPlanner.Core.Models;

public partial class Idea : ObservableObject
{
    public int Id { get; set; }

    [ObservableProperty] private string? _text;
    
    [ObservableProperty] private IdeaState _state;
}