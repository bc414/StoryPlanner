using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using WindowedStoryPlanner.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WindowedStoryPlanner.Views;

public partial class CommonWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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

    private PlotPointSubjectLinkViewModel? _selectedLink;
    public PlotPointSubjectLinkViewModel? SelectedLink
    {
        get => _selectedLink;
        set
        {
            if (value == _selectedLink) return;

            _selectedLink?.OnWindowClosed();
            _selectedLink = value;
            _selectedLink?.OnWindowOpened();

            Notify(nameof(SelectedLink));
            UpdateLayout();
        }
    }

    // ── Link list (set once as ItemsSource; updated in-place) ─────────────

    public ObservableCollection<PlotPointSubjectLinkViewModel> LinkItems { get; } = [];

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

        // Wire up pickers
        SubjectPicker.Registry       = _registry;
        SubjectPicker.SubjectSelected   += OnSubjectPicked;
        PlotPointPicker.Registry     = _registry;
        PlotPointPicker.PlotPointSelected += OnPlotPointPicked;

        // Wire up create-link pickers
        CreateLinkPlotPointPicker.Registry        = _registry;
        CreateLinkPlotPointPicker.PlotPointSelected += OnCreateLinkPlotPointPicked;
        CreateLinkSubjectPicker.Registry          = _registry;
        CreateLinkSubjectPicker.SubjectSelected   += OnCreateLinkSubjectPicked;

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
            SelectedLink = initialLink;
            SelectedLink.OnWindowOpened();
        }

        UpdateLayout();
        _registry.LinksInvalidated += UpdateLayout;
    }

    protected override void OnClosed(EventArgs e)
    {
        _registry.LinksInvalidated -= UpdateLayout;
        _subjectElement?.OnWindowClosed();
        _plotPointElement?.OnWindowClosed();
        SelectedLink?.OnWindowClosed();
        base.OnClosed(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            e.Handled = true;
            Close();
        }
        else
        {
            base.OnKeyDown(e);
        }
    }

    // ── Mode button clicks ────────────────────────────────────────────────

    private void ExpansionMode_Click(object sender, RoutedEventArgs e) => SwitchToExpansion();
    private void LinkingMode_Click(object sender, RoutedEventArgs e)   => SwitchToLinking();
    private void GardenerMode_Click(object sender, RoutedEventArgs e)  => SwitchToGardener();

    private void SwitchToExpansion()
    {
        // Reachable only from Linking — close the selected link and hide the panels
        ClearSelectedLinkSilent();
        _currentMode = EditorMode.Expansion;
        UpdateLayout();
    }

    private void SwitchToLinking()
    {
        if (_currentMode == EditorMode.Expansion)
        {
            _currentMode = EditorMode.Linking;
            UpdateLayout();
            return;
        }

        if (_currentMode == EditorMode.Gardener && SelectedLink is not null)
        {
            var subject = _registry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == SelectedLink.SubjectId);
            if (subject is null) return;

            SetPlotPointElement(null);
            SetSubjectElement(subject);
            _currentMode = EditorMode.Linking;
            UpdateLayout();
        }
    }

    private void SwitchToGardener()
    {
        if (_currentMode != EditorMode.Linking || SelectedLink is null) return;

        var plotPoint = _registry.AllPlotPointViewModels
            .FirstOrDefault(pp => pp.Id == SelectedLink.PlotPointId);
        if (plotPoint is null) return;

        SetSubjectElement(null);
        SetPlotPointElement(plotPoint);
        _currentMode = EditorMode.Gardener;
        UpdateLayout();
    }

    // ── Primary element helpers ───────────────────────────────────────────

    /// <summary>
    /// Closes the current subject element, assigns the new one (or null), opens it,
    /// and updates the window title. Does NOT change mode or call UpdateLayout.
    /// </summary>
    private void SetSubjectElement(SubjectViewModel? subject)
    {
        _subjectElement?.OnWindowClosed();
        _subjectElement = subject;
        _subjectElement?.OnWindowOpened();
        Title = subject is not null ? $"Subject — {subject.Name}" : Title;
    }

    /// <summary>
    /// Closes the current plot point element, assigns the new one (or null), opens it,
    /// and updates the window title. Does NOT change mode or call UpdateLayout.
    /// </summary>
    private void SetPlotPointElement(PlotPointViewModel? plotPoint)
    {
        _plotPointElement?.OnWindowClosed();
        _plotPointElement = plotPoint;
        _plotPointElement?.OnWindowOpened();
        Title = plotPoint is not null ? $"Plot Point — {plotPoint.Title}" : Title;
    }

    /// <summary>
    /// Clears SelectedLink without triggering the property setter's UpdateLayout call.
    /// Use this when a full UpdateLayout will be called shortly afterward.
    /// </summary>
    private void ClearSelectedLinkSilent()
    {
        _selectedLink?.OnWindowClosed();
        _selectedLink = null;
        Notify(nameof(SelectedLink));
    }

    // ── Picker button clicks ──────────────────────────────────────────────

    private void SubjectPickerButton_Click(object sender, RoutedEventArgs e)
    {
        SubjectPickerPopup.IsOpen = !SubjectPickerPopup.IsOpen;
    }

    private void PlotPointPickerButton_Click(object sender, RoutedEventArgs e)
    {
        PlotPointPickerPopup.IsOpen = !PlotPointPickerPopup.IsOpen;
    }

    // ── Create-link picker button ─────────────────────────────────────────

    private void CreateLinkButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentMode == EditorMode.Gardener)
        {
            // Close the other one in case it was left open, then toggle this one
            CreateLinkPlotPointPickerPopup.IsOpen = false;
            CreateLinkSubjectPickerPopup.IsOpen   = !CreateLinkSubjectPickerPopup.IsOpen;
        }
        else
        {
            CreateLinkSubjectPickerPopup.IsOpen    = false;
            CreateLinkPlotPointPickerPopup.IsOpen  = !CreateLinkPlotPointPickerPopup.IsOpen;
        }
    }

    // ── Picker selection handlers ─────────────────────────────────────────

    private void OnSubjectPicked(SubjectViewModel subject)
    {
        SubjectPickerPopup.IsOpen = false;

        // Always land in Expansion; clear any selected link and plot point
        ClearSelectedLinkSilent();
        SetPlotPointElement(null);
        SetSubjectElement(subject);
        _currentMode = EditorMode.Expansion;
        UpdateLayout();
    }

    private void OnPlotPointPicked(PlotPointViewModel plotPoint)
    {
        PlotPointPickerPopup.IsOpen = false;

        // Always land in Gardener; clear any selected link and subject
        ClearSelectedLinkSilent();
        SetSubjectElement(null);
        SetPlotPointElement(plotPoint);
        _currentMode = EditorMode.Gardener;
        UpdateLayout();
    }

    // ── Create-link pick handlers ─────────────────────────────────────────

    /// <summary>
    /// Expansion and Linking modes: user picked a plot point to link to the current subject.
    /// Creates (or finds) the link, then selects it. Switches to Linking if in Expansion.
    /// </summary>
    private async void OnCreateLinkPlotPointPicked(PlotPointViewModel plotPoint)
    {
        CreateLinkPlotPointPickerPopup.IsOpen = false;

        if (_subjectElement is null) return;

        await _editorCoordinator.CreatePlotPointSubjectLinkAsync(plotPoint, _subjectElement);

        var link = _registry.AllPlotPointSubjectLinkViewModels
            .FirstOrDefault(l => l.PlotPointId == plotPoint.Id && l.SubjectId == _subjectElement.Id);
        if (link is null) return;

        // Switch to Linking if still in Expansion (subject is already open, no re-init needed)
        if (_currentMode == EditorMode.Expansion)
            _currentMode = EditorMode.Linking;

        // Selecting the link calls OnWindowOpened + UpdateLayout via the property setter
        SelectedLink = link;
    }

    /// <summary>
    /// Gardener mode: user picked a subject to link to the current plot point.
    /// Creates (or finds) the link, then selects it. Stays in Gardener mode.
    /// </summary>
    private async void OnCreateLinkSubjectPicked(SubjectViewModel subject)
    {
        CreateLinkSubjectPickerPopup.IsOpen = false;

        if (_plotPointElement is null) return;

        await _editorCoordinator.CreatePlotPointSubjectLinkAsync(_plotPointElement, subject);

        var link = _registry.AllPlotPointSubjectLinkViewModels
            .FirstOrDefault(l => l.PlotPointId == _plotPointElement.Id && l.SubjectId == subject.Id);
        if (link is null) return;

        // Stay in Gardener; selecting the link calls OnWindowOpened + UpdateLayout
        SelectedLink = link;
    }

    // ── Link card selection ───────────────────────────────────────────────

    private void LinkCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var newLink = LinkCardsListBox.SelectedItem as PlotPointSubjectLinkViewModel;
        if (newLink == SelectedLink) return;

        SelectedLink?.OnWindowClosed();
        SelectedLink = newLink;
        SelectedLink?.OnWindowOpened();

        // Defer UpdateLayout so it runs after WPF finishes processing
        // the current SelectionChanged event cycle. Without this deferral,
        // replacing ItemsSource inside a SelectionChanged handler causes WPF
        // to schedule its own deferred collection-reconciliation work that fires
        // *after* our SelectedItem assignment, resetting selection to the first item.
        Dispatcher.InvokeAsync(UpdateLayout, DispatcherPriority.Loaded);
    }

    // ── Clear selected link (X button) ───────────────────────────────────

    private void ClearSelectedLink_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedLink is null) return;

        SelectedLink.OnWindowClosed();
        SelectedLink = null;

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
        if (plotPoint is null && SelectedLink is not null)
            plotPoint = _registry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == SelectedLink.PlotPointId);

        if (plotPoint is null) return;

        new CommonWindow(_registry, _editorCoordinator, _storyService, AppSettings,
            EditorMode.Gardener, plotPoint, SelectedLink)
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
                _subjectElement?.SetEditorMode(EditorMode.Expansion);
                _subjectElement?.SetTrackDisplayMode(TrackDisplayMode.Active);
                break;

            case EditorMode.Linking:
                leftElement  = _subjectElement;
                rightElement = SelectedLink;
                showMiddle   = true;
                _subjectElement?.SetEditorMode(EditorMode.Linking);
                _subjectElement?.SetTrackDisplayMode(TrackDisplayMode.Reference);
                SelectedLink?.SetEditorMode(EditorMode.Linking);
                SelectedLink?.SetTrackDisplayMode(TrackDisplayMode.Active);
                break;

            case EditorMode.Gardener:
                leftElement  = SelectedLink;
                rightElement = _plotPointElement;
                showMiddle   = true;
                SelectedLink?.SetEditorMode(EditorMode.Gardener);
                SelectedLink?.SetTrackDisplayMode(TrackDisplayMode.Active);
                _plotPointElement?.SetEditorMode(EditorMode.Gardener);
                _plotPointElement?.SetTrackDisplayMode(TrackDisplayMode.Active);
                break;
        }

        // Panels
        LeftPanel.Content    = leftElement;
        LeftPanel.Visibility = leftElement  is not null ? Visibility.Visible : Visibility.Collapsed;

        RightPanel.Content    = rightElement;
        RightPanel.Visibility = rightElement is not null ? Visibility.Visible : Visibility.Collapsed;

        LinkPanelBorder.Visibility = showMiddle ? Visibility.Visible : Visibility.Collapsed;

        // Column widths
        var leftCol  = ContentGrid.ColumnDefinitions[0];
        var rightCol = ContentGrid.ColumnDefinitions[2];

        bool leftVisible  = leftElement  is not null;
        bool rightVisible = rightElement is not null;

        if (leftVisible && rightVisible)
        {
            leftCol.Width  = new GridLength(1, GridUnitType.Star);
            rightCol.Width = new GridLength(1, GridUnitType.Star);
        }
        else
        {
            leftCol.Width  = leftVisible  ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            rightCol.Width = rightVisible ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
        }

        // Rebuild link list in-place — ItemsSource reference never changes,
        // so WPF does NOT reset selection or run container reconciliation.
        // The two-way SelectedItem binding keeps SelectedLink in sync automatically.
        var fresh = GetSortedLinks().ToList();
        for (int i = LinkItems.Count - 1; i >= 0; i--)
            if (!fresh.Contains(LinkItems[i])) LinkItems.RemoveAt(i);
        for (int i = 0; i < fresh.Count; i++)
            if (i >= LinkItems.Count || !ReferenceEquals(LinkItems[i], fresh[i]))
                LinkItems.Insert(i, fresh[i]);

        // Mode buttons
        ExpansionModeButton.Tag = _currentMode == EditorMode.Expansion ? "Active" : null;
        LinkingModeButton.Tag   = _currentMode == EditorMode.Linking   ? "Active" : null;
        GardenerModeButton.Tag  = _currentMode == EditorMode.Gardener  ? "Active" : null;

        ExpansionModeButton.IsEnabled = _currentMode == EditorMode.Linking;
        LinkingModeButton.IsEnabled   = _currentMode == EditorMode.Expansion ||
                                        (_currentMode == EditorMode.Gardener && SelectedLink is not null);
        GardenerModeButton.IsEnabled  = _currentMode == EditorMode.Linking && SelectedLink is not null;
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
