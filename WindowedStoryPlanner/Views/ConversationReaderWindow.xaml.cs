using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using StoryPlanner.Core;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

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

        string html = ConversationMarkdownRenderer.Render(block.RawContent, _vm.Platform);
        ContentWebView.NavigateToString(html);
    }

    // Keyboard shortcuts: U=Unread, S=Skip, F=Flag, D=Done
    // Only fires when a block is selected and focus is not in the WebView (which handles its own keys)
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_vm?.SelectedBlock is not { } block) return;
        if (e.OriginalSource is Microsoft.Web.WebView2.Wpf.WebView2) return;

        switch (e.Key)
        {
            case Key.U: block.MarkUnreadCommand.Execute(null);  e.Handled = true; break;
            case Key.S: block.MarkSkippedCommand.Execute(null); e.Handled = true; break;
            case Key.F: block.MarkFlaggedCommand.Execute(null); e.Handled = true; break;
            case Key.D: block.MarkDoneCommand.Execute(null);    e.Handled = true; break;
        }
    }

    private void SnapTopHalf_Click(object sender, RoutedEventArgs e) => WindowSnap.TopHalf(this);

    private void SnapBottomHalf_Click(object sender, RoutedEventArgs e) => WindowSnap.BottomHalf(this);

    protected override void OnClosed(EventArgs e)
    {
        if (_vm is not null)
            _vm.PropertyChanged -= OnVmPropertyChanged;

        base.OnClosed(e);
    }
}
