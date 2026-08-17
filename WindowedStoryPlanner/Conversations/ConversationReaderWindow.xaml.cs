using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using StoryPlanner.Core;
using WindowedStoryPlanner;

namespace WindowedStoryPlanner;

public partial class ConversationReaderWindow : Window
{
    private ConversationViewModel? _vm;
    private bool _webViewReady;

    public ConversationReaderWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        KeyDown += OnKeyDown;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        _vm = DataContext as ConversationViewModel;
        if (_vm is null) return;

        _vm.PropertyChanged += OnVmPropertyChanged;

        await ContentWebView.EnsureCoreWebView2Async();
        _webViewReady = true;

        RenderSelectedBlock();
    }

    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ConversationViewModel.SelectedBlock))
            RenderSelectedBlock();
    }

    private void RenderSelectedBlock()
    {
        if (!_webViewReady || _vm is null) return;

        var block = _vm.SelectedBlock;
        if (block is null)
        {
            ContentWebView.NavigateToString(
                "<html><body style='font-family:sans-serif;color:#999;padding:20px'>" +
                "Select a block to read it.</body></html>");
            return;
        }

        string html = ConversationMarkdownRenderer.Render(block.RawContent, _vm.Platform, block.Speaker);
        ContentWebView.NavigateToString(html);
    }

    // Reading pane follows the last block added to the selection. SelectedItem is no
    // longer bound (a two-way single-item binding would collapse Ctrl/Shift
    // multi-selections), so this is the ListBoxes' writer of SelectedBlock — joined by
    // OnSummaryGotKeyboardFocus below, which covers the clicks a note editor swallows.
    private void OnBlockSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (_vm is null) return;
        if (e.AddedItems.OfType<ConversationBlockViewModel>().LastOrDefault() is { } last)
            _vm.SelectedBlock = last;
    }

    // A TextBox handles the mouse-down that would otherwise select its ListBoxItem, so clicking
    // into a note neither selects the row nor moves the reading pane. Reproduce the click that
    // was eaten: a block already in the multi-selection keeps it (so a bulk mark still applies to
    // everything the user picked), anything else collapses the selection onto itself the way a
    // plain click would.
    private void OnSummaryGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (_vm is null) return;
        if (sender is not FrameworkElement { DataContext: ConversationBlockViewModel block }) return;

        if (block.IsSelected) _vm.SelectedBlock = block;
        else                  SummaryList.SelectedItem = block; // -> OnBlockSelectionChanged
    }

    // Enter and Escape both hand the keyboard back to the row, which is what restores arrow
    // navigation and the U/S/F/D shortcuts after a note is written.
    private void OnSummaryPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox box) return;
        var binding = box.GetBindingExpression(TextBox.TextProperty);

        switch (e.Key)
        {
            // Escape reverts: the commit is on LostFocus, so the model still holds the pre-edit
            // text and pushing it back into the box IS the revert. It also clears the binding's
            // dirty flag, so the LostFocus that follows writes nothing.
            case Key.Escape:
                binding?.UpdateTarget();
                FocusOwningRow(box);
                e.Handled = true;
                break;
            // AcceptsReturn is false, so Enter is otherwise inert here. Commit and leave.
            case Key.Enter:
                binding?.UpdateSource();
                FocusOwningRow(box);
                e.Handled = true;
                break;
        }
    }

    private static void FocusOwningRow(DependencyObject from)
    {
        if (VisualTreeSearch.FindAncestor<ListBoxItem>(from) is { } row) Keyboard.Focus(row);
    }

    // Keyboard shortcuts: U=Unread, S=Skip, F=Flag, D=Done
    // Only fires when a block is selected and focus is not in the WebView (which handles its own keys)
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_vm?.SelectedBlock is not { } block) return;
        if (e.OriginalSource is Microsoft.Web.WebView2.Wpf.WebView2) return;
        // The summary cards are editable now: inside a text field these are letters, not triage.
        // Without this, typing "used" into a note retriages the block three times.
        if (Keyboard.FocusedElement is TextBoxBase) return;

        switch (e.Key)
        {
            case Key.U: block.MarkUnreadCommand.Execute(null);  e.Handled = true; break;
            case Key.S: block.MarkSkippedCommand.Execute(null); e.Handled = true; break;
            case Key.F: block.MarkFlaggedCommand.Execute(null); e.Handled = true; break;
            case Key.D: block.MarkDoneCommand.Execute(null);    e.Handled = true; break;
        }
    }

    // LostFocus is not guaranteed to fire during window teardown, and a note lives outside the
    // model until it does. Commit explicitly so closing the reader mid-sentence keeps the text —
    // the Summary setter's own save then persists it, and App.OnExit catches it again on shutdown.
    protected override void OnClosing(CancelEventArgs e)
    {
        if (Keyboard.FocusedElement is TextBox box)
            box.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

        base.OnClosing(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        if (_vm is not null)
            _vm.PropertyChanged -= OnVmPropertyChanged;

        base.OnClosed(e);
    }
}
