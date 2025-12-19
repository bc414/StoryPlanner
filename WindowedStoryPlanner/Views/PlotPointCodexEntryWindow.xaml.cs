using System.Windows;
using System.Windows.Input;

namespace WindowedStoryPlanner.Views;

public partial class PlotPointCodexEntryWindow : Window, IPayloadWindow
{
    // Property to receive the Command from the ViewModel
    public ICommand UnlinkCommand { get; set; }

    public PlotPointCodexEntryWindow()
    {
        InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}