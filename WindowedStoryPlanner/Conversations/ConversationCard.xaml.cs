using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

public partial class ConversationCard : UserControl
{
    public ConversationCard()
    {
        InitializeComponent();
    }

    // Double-clicking the card opens the reader. The title TextBox is editable now
    // (for renaming), so a double-click used to select a word while renaming must NOT
    // also open the reader — checked explicitly via OriginalSource rather than relying
    // on InputBinding gesture-recognition timing relative to a descendant control.
    private void RootBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2) return;
        if (VisualTreeSearch.FindAncestor<TextBox>(e.OriginalSource as DependencyObject) is not null) return;
        if (DataContext is not ConversationViewModel vm) return;

        if (VisualTreeSearch.FindAncestor<ItemsControl>(this)?.DataContext is ConversationLibraryViewModel libraryVm
            && libraryVm.OpenConversationReaderCommand.CanExecute(vm))
        {
            libraryVm.OpenConversationReaderCommand.Execute(vm);
        }
    }
}
