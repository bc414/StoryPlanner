using System;
using System.Windows;
using System.Windows.Input;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Interaction logic for ChapterWindow.xaml. Unlike CommonWindow this is a singleton per
    /// chapter, so its Story → Chapter picker cannot just swap DataContext on its own — the
    /// re-keying belongs to the WindowManager (<see cref="IWindowManager.RetargetChapterWindow"/>).
    /// </summary>
    public partial class ChapterWindow : Window
    {
        private readonly IWindowManager _windowManager;

        public ChapterWindow(IWindowManager windowManager, IViewModelRegistry registry)
        {
            _windowManager = windowManager;

            InitializeComponent();

            ChapterPicker.Registry = registry;
            ChapterPicker.ChapterSelected += OnChapterPicked;

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

        // ── Chapter picker ────────────────────────────────────────────────

        private void ChapterPickerButton_Click(object sender, RoutedEventArgs e)
        {
            ChapterPickerPopup.IsOpen = !ChapterPickerPopup.IsOpen;
        }

        private void OnChapterPicked(ChapterViewModel chapter)
        {
            ChapterPickerPopup.IsOpen = false;
            _windowManager.RetargetChapterWindow(this, chapter);
        }

        // ── Escape closes the window (the picker flyout first, if it is open) ──

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                if (ChapterPickerPopup.IsOpen)
                    ChapterPickerPopup.IsOpen = false;
                else
                    Close();
            }
            else
            {
                base.OnKeyDown(e);
            }
        }
    }
}
