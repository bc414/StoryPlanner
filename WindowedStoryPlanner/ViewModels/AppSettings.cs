using CommunityToolkit.Mvvm.ComponentModel;

namespace WindowedStoryPlanner.ViewModels;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private bool _isArchiveMode;
}