using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views;

public partial class CommonWindow : Window
{
    // ── Injected dependencies ─────────────────────────────────────────────

    private readonly IViewModelRegistry _registry;
    private readonly IContentFactory _editorCoordinator;
    private readonly IStoryService _storyService;
    private readonly AppSettings _appSettings;

    // ── Mode state ────────────────────────────────────────────────────────

    private EditorMode _currentMode;

    /// <summary>The subject anchor. Set in Expansion/Linking; resolved when transitioning Gardener → Linking.</summary>
    private SubjectViewModel? _subjectElement;

    /// <summary>The plot point anchor. Set in Gardener; resolved when transitioning Linking → Gardener.</summary>
    private PlotPointViewModel? _plotPointElement;

    /// <summary>The currently selected link. Non-null when a link card is selected in Linking or Gardener.</summary>
    private PlotPointSubjectLinkViewModel? _selectedLink;

    // ── Properties ─────────────────────────────────────────────────────────

    public AppSettings AppSettings => _appSettings;

    // ── Constructor ───────────────────────────────────────────────────────

    /// <param name="initialMode">
    ///   Must be <see cref="EditorMode.Expansion"/> or <see cref="EditorMode.Linking"/> when
    ///   <paramref name="primaryElement"/> is a <see cref="SubjectViewModel"/>, 
    ///   or <see cref="EditorMode.Gardener"/> when it is a <see cref="PlotPointViewModel"/>.
    /// </param>
    /// <param name="primaryElement">The subject or plot point that anchors this window.</param>
    /// <param name="initialLink">Optional link to pre-select on open.</param>
    public CommonWindow(
        IViewModelRegistry registry,
        IContentFactory editorCoordinator,
        IStoryService storyService,
        AppSettings appSettings,
        EditorMode initialMode,
        NarrativeElementViewModel primaryElement,
        PlotPointSubjectLinkViewModel? initialLink = null)
    {
        _registry          = registry;
        _editorCoordinator = editorCoordinator;
        _storyService      = storyService;
        _appSettings        = appSettings;
        _currentMode       = initialMode;

        DataContext = this;
        InitializeComponent();

        switch (initialMode)
        {
            case EditorMode.Expansion:
            case EditorMode.Linking:
                _subjectElement = (SubjectViewModel)primaryElement;
                _subjectElement.OnWindowOpened();
                Title = $"Subject — {_subjectElement.Name}";
                break;

            case EditorMode.Gardener:
                _plotPointElement = (PlotPointViewModel)primaryElement;
                _plotPointElement.OnWindowOpened();
                Title = $"Plot Point — {_plotPointElement.Title}";
                break;
        }

        if (initialLink is not null)
        {
            _selectedLink = initialLink;
            _selectedLink.OnWindowOpened();
        }

        UpdateLayout();
        _registry.LinksInvalidated += UpdateLayout;
    }

    protected override void OnClosed(EventArgs e)
    {
        _registry.LinksInvalidated -= UpdateLayout;
        _subjectElement?.OnWindowClosed();
        _plotPointElement?.OnWindowClosed();
        _selectedLink?.OnWindowClosed();
        base.OnClosed(e);
    }

    // ── Mode button clicks ────────────────────────────────────────────────

    private void ExpansionMode_Click(object sender, RoutedEventArgs e) => SwitchToExpansion();
    private void LinkingMode_Click(object sender, RoutedEventArgs e)   => SwitchToLinking();
    private void GardenerMode_Click(object sender, RoutedEventArgs e)  => SwitchToGardener();

    private void SwitchToExpansion()
    {
        // Reachable only from Linking — close the selected link and hide the panels
        _selectedLink?.OnWindowClosed();
        _selectedLink = null;
        _currentMode  = EditorMode.Expansion;
        UpdateLayout();
    }

    private void SwitchToLinking()
    {
        if (_currentMode == EditorMode.Expansion)
        {
            // Subject already open — just surface the link panel
            _currentMode = EditorMode.Linking;
            UpdateLayout();
            return;
        }

        if (_currentMode == EditorMode.Gardener && _selectedLink is not null)
        {
            var subject = _registry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == _selectedLink.SubjectId);
            if (subject is null) return;

            _subjectElement = subject;
            _subjectElement.OnWindowOpened();

            _plotPointElement?.OnWindowClosed();
            _plotPointElement = null;

            Title        = $"Subject — {_subjectElement.Name}";
            _currentMode = EditorMode.Linking;
            UpdateLayout();
        }
    }

    private void SwitchToGardener()
    {
        if (_currentMode != EditorMode.Linking || _selectedLink is null) return;

        var plotPoint = _registry.AllPlotPointViewModels
            .FirstOrDefault(pp => pp.Id == _selectedLink.PlotPointId);
        if (plotPoint is null) return;

        _plotPointElement = plotPoint;
        _plotPointElement.OnWindowOpened();

        _subjectElement?.OnWindowClosed();
        _subjectElement = null;

        Title        = $"Plot Point — {_plotPointElement.Title}";
        _currentMode = EditorMode.Gardener;
        UpdateLayout();
    }

    // ── Link card selection ───────────────────────────────────────────────

    private void LinkCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var newLink = LinkCardsListBox.SelectedItem as PlotPointSubjectLinkViewModel;
        if (newLink == _selectedLink) return;

        _selectedLink?.OnWindowClosed();
        _selectedLink = newLink;
        _selectedLink?.OnWindowOpened();

        UpdateLayout();
    }

    // ── Clear selected link (X button) ───────────────────────────────────

    private void ClearSelectedLink_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedLink is null) return;

        // Close the selected link and clear the selection
        _selectedLink.OnWindowClosed();
        _selectedLink = null;

        UpdateLayout();

        e.Handled = true;
    }

    // ── Middle-click: open in new window ──────────────────────────────────

    private void ExpansionModeButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Middle || _subjectElement is null) return;

        new CommonWindow(_registry, _editorCoordinator, _storyService, AppSettings,
            EditorMode.Expansion, _subjectElement)
            .Show();
        e.Handled = true;
    }

    private void GardenerModeButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Middle) return;

        // In Linking with a link selected: resolve the plot point from the link
        PlotPointViewModel? plotPoint = _plotPointElement;
        if (plotPoint is null && _selectedLink is not null)
            plotPoint = _registry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == _selectedLink.PlotPointId);

        if (plotPoint is null) return;

        new CommonWindow(_registry, _editorCoordinator, _storyService, AppSettings,
            EditorMode.Gardener, plotPoint, _selectedLink)
            .Show();
        e.Handled = true;
    }

    // ── Layout ────────────────────────────────────────────────────────────

    private new void UpdateLayout()
    {
        NarrativeElementViewModel? leftElement  = null;
        NarrativeElementViewModel? rightElement = null;
        bool showMiddle = false;

        switch (_currentMode)
        {
            case EditorMode.Expansion:
                leftElement  = _subjectElement;
                rightElement = null;
                showMiddle   = false;
                _subjectElement?.SetTrackDisplayMode(TrackDisplayMode.Active);
                break;

            case EditorMode.Linking:
                leftElement  = _subjectElement;
                rightElement = _selectedLink;
                showMiddle   = true;
                _subjectElement?.SetTrackDisplayMode(TrackDisplayMode.Reference);
                _selectedLink?.SetTrackDisplayMode(TrackDisplayMode.Active);
                break;

            case EditorMode.Gardener:
                leftElement  = _selectedLink;
                rightElement = _plotPointElement;
                showMiddle   = true;
                _selectedLink?.SetTrackDisplayMode(TrackDisplayMode.Active);
                _plotPointElement?.SetTrackDisplayMode(TrackDisplayMode.Active);
                break;
        }

        // Panels
        LeftPanel.Content    = leftElement;
        LeftPanel.Visibility = leftElement  is not null ? Visibility.Visible : Visibility.Collapsed;

        RightPanel.Content    = rightElement;
        RightPanel.Visibility = rightElement is not null ? Visibility.Visible : Visibility.Collapsed;

        LinkPanelBorder.Visibility = showMiddle ? Visibility.Visible : Visibility.Collapsed;

        // Column widths — secondary (Reference/link) panel gets 1*, primary gets 2*
        var leftCol  = ContentGrid.ColumnDefinitions[0];
        var rightCol = ContentGrid.ColumnDefinitions[2];

        bool leftVisible  = leftElement  is not null;
        bool rightVisible = rightElement is not null;

        if (leftVisible && rightVisible)
        {
            // In both Linking and Gardener the right panel is primary
            leftCol.Width  = new GridLength(1, GridUnitType.Star);
            rightCol.Width = new GridLength(2, GridUnitType.Star);
        }
        else
        {
            leftCol.Width  = leftVisible  ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            rightCol.Width = rightVisible ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
        }

        // Link list
        LinkCardsListBox.ItemsSource = GetSortedLinks().ToList();

        LinkCardsListBox.SelectionChanged -= LinkCards_SelectionChanged;
        LinkCardsListBox.SelectedItem      = _selectedLink;
        LinkCardsListBox.SelectionChanged += LinkCards_SelectionChanged;

        // Mode buttons — Tag="Active" marks the selected mode; IsEnabled gates valid transitions
        ExpansionModeButton.Tag = _currentMode == EditorMode.Expansion ? "Active" : null;
        LinkingModeButton.Tag   = _currentMode == EditorMode.Linking   ? "Active" : null;
        GardenerModeButton.Tag  = _currentMode == EditorMode.Gardener  ? "Active" : null;

        ExpansionModeButton.IsEnabled = _currentMode == EditorMode.Linking;
        LinkingModeButton.IsEnabled   = _currentMode != EditorMode.Linking;
        GardenerModeButton.IsEnabled  = _currentMode == EditorMode.Linking && _selectedLink is not null;
    }

    private IEnumerable<PlotPointSubjectLinkViewModel> GetSortedLinks()
    {
        if ((_currentMode == EditorMode.Expansion || _currentMode == EditorMode.Linking)
            && _subjectElement is not null)
            return _registry.AllPlotPointSubjectLinkViewModels
                .Where(l => l.SubjectId == _subjectElement.Id)
                .OrderBy(l => l.ChapterOrderIndex)
                .ThenBy(l => l.PlotPointOrderInChapter);

        if (_currentMode == EditorMode.Gardener && _plotPointElement is not null)
            return _registry.AllPlotPointSubjectLinkViewModels
                .Where(l => l.PlotPointId == _plotPointElement.Id)
                .OrderBy(l => l.SubjectTypeName)
                .ThenBy(l => l.SubjectName, StringComparer.CurrentCultureIgnoreCase);

        return Enumerable.Empty<PlotPointSubjectLinkViewModel>();
    }
}
