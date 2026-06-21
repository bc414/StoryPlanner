using System.Windows.Controls;
using System.Windows.Input;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

public partial class ExportView : UserControl
{
    public ExportView()
    {
        InitializeComponent();
    }

    private void SearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is ExportViewModel vm)
            vm.AddAnchorCommand.Execute(null);
    }
}
