using System.Windows;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Interaction logic for ConversationPickerWindow.xaml
    /// </summary>
    public partial class ConversationPickerWindow : Window
    {
        public ConversationPickerWindow()
        {
            InitializeComponent();
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConversationPickerViewModel { SelectedConversation: null })
            {
                MessageBox.Show("Pick a conversation from the list first.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
