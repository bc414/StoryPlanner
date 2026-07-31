using System.Windows;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Interaction logic for MoveChapterWindow.xaml. Follows the same convention as
    /// ConversationPickerWindow: this class only sets DialogResult; the caller
    /// (ChapterLibraryViewModel.OpenMoveDialog) reads the bound ViewModel's selections
    /// after ShowDialog() returns true and performs the actual move.
    /// </summary>
    public partial class MoveChapterWindow : Window
    {
        public MoveChapterWindow()
        {
            InitializeComponent();
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
