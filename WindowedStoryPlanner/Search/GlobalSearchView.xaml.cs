using System.Windows.Controls;
using System.Windows.Input;

namespace WindowedStoryPlanner;

public partial class GlobalSearchView : UserControl
{
    public GlobalSearchView()
    {
        InitializeComponent();
    }

    private void Results_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is GlobalSearchViewModel vm)
            vm.OpenSelectedCommand.Execute(null);
    }
}
