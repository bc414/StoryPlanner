using System;
using System.Windows;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    /// <summary>
    /// Interaction logic for ChapterWindow.xaml
    /// </summary>
    public partial class ChapterWindow : Window
    {
        public ChapterWindow()
        {
            InitializeComponent();
            /*
            // 1. Apply the limit BEFORE the window loads.
            // Using WorkArea is better than PrimaryScreenHeight as it accounts for the Taskbar.
            this.MaxHeight = SystemParameters.WorkArea.Height * 0.9;

            // 2. Wait until the window is effectively on screen and sized.
            this.ContentRendered += (s, e) =>
            {
                // Switch to Manual mode. This "freezes" the window at its current 
                // calculated size (which was clamped by MaxHeight).
                this.SizeToContent = SizeToContent.Manual;

                // Now remove the limit so the user can resize it larger if they want.
                this.MaxHeight = double.PositiveInfinity;
            };*/

            this.Loaded += OnLoaded;
            this.Closed += OnClosed;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NarrativeElementViewModel vm)
                vm.OnWindowOpened();
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            if (DataContext is NarrativeElementViewModel vm)
                vm.OnWindowClosed();
        }
    }
}
