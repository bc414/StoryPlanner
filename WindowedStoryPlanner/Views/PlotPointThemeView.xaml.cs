using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowedStoryPlanner.Views;

public partial class PlotPointThemeView : UserControl, IPayloadWindow
{
    public ICommand UnlinkCommand { get; set; }
    public PlotPointThemeView()
    {
        InitializeComponent();
    }
}