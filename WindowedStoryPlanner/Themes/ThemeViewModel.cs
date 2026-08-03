using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

public partial class ThemeViewModel : ObservableObject
{
    private readonly Theme _model;

    public Theme Model => _model;

    public int Id => _model.Id;

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
    }

    public string Proposition
    {
        get => _model.Proposition;
        set => SetProperty(_model.Proposition, value, _model, (m, v) => m.Proposition = v);
    }

    public ThemeViewModel(Theme model)
    {
        _model       = model;
    }
}
