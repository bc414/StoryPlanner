using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowedStoryPlanner.Views;

public partial class PlotPointCodexEntryView : UserControl, IPayloadWindow
{
    // Property to receive the Command from the ViewModel
    public ICommand UnlinkCommand { get; set; }

    public PlotPointCodexEntryView()
    {
        InitializeComponent();
    }
}