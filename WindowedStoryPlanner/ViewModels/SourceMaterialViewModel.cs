using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels;

public partial class SourceMaterialViewModel : ObservableObject
{
    private readonly SourceMaterial _model;
    private readonly IStoryService _storyService;

    public SourceMaterial Model => _model;

    public int Id => _model.Id;

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
    }

    public string Description
    {
        get => _model.Description;
        set => SetProperty(_model.Description, value, _model, (m, v) => m.Description = v);
    }

    public SourceMaterialViewModel(SourceMaterial model, IStoryService storyService)
    {
        _model = model;
        _storyService = storyService;
    }
}
