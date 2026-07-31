namespace WindowedStoryPlanner;

public class ThemeDetailViewModel : TaggedNotesViewModelBase
{
    public ThemeViewModel Theme { get; }

    public ThemeDetailViewModel(ThemeViewModel theme, IViewModelRegistry registry) : base(registry)
    {
        Theme = theme;
    }

    protected override bool Matches(NoteViewModel note) => note.SelectedTheme?.Id == Theme.Id;
    protected override string TagPropertyName => nameof(NoteViewModel.SelectedTheme);
}
