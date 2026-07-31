using CommunityToolkit.Mvvm.ComponentModel;

namespace WindowedStoryPlanner;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private bool _isArchiveMode;
}