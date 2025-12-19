using System.Windows;
using System.Windows.Input;

namespace WindowedStoryPlanner.Views;

public partial class PlotPointThemeWindow : Window, IPayloadWindow
{
    public ICommand UnlinkCommand { get; set; }
    public PlotPointThemeWindow()
    {
        InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}