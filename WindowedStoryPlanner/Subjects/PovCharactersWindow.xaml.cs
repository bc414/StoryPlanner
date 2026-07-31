using System.Windows;

namespace WindowedStoryPlanner;

public partial class PovCharactersWindow : Window
{
    public PovCharactersWindow()
    {
        InitializeComponent();
        SubjectPicker.SubjectSelected += OnSubjectPicked;
    }

    private void OnSubjectPicked(SubjectViewModel subject)
    {
        if (DataContext is PovCharactersViewModel vm)
            vm.AddCommand.Execute(subject);
    }

    // Disposal of the DataContext is WindowManager.ShowSingleton's job.
}
