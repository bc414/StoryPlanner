using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    public partial class CommonWindow : Window
    {
        // ── Injected dependencies ─────────────────────────────────────────────

        private readonly IViewModelRegistry _registry;
        private readonly IContentFactory _editorCoordinator;
        private readonly IStoryService _storyService;

        // ── State ─────────────────────────────────────────────────────────────

        private NarrativeElementViewModel? _mainElement;
        private NarrativeElementViewModel? _secondaryElement;
        private bool _isSubjectMode;

        public PlotPointSubjectLinkViewModel? SelectedLink { get; private set; }

        // ── Derived layout helpers ────────────────────────────────────────────

        private NarrativeElementViewModel? LeftElement =>
            _isSubjectMode ? _mainElement : _secondaryElement;

        private NarrativeElementViewModel? RightElement =>
            _isSubjectMode ? _secondaryElement : _mainElement;

        // ── Constructor ───────────────────────────────────────────────────────

        /// <param name="initialElement">
        ///   Must be a <see cref="SubjectViewModel"/> or <see cref="PlotPointViewModel"/>.
        ///   Determines layout mode and is shown immediately on open.
        /// </param>
        /// <param name="initialLink">
        ///   Optional link to select on open, restoring a previous context
        ///   (e.g. when spawning a new window from a middle-click).
        /// </param>
        public CommonWindow(
            IViewModelRegistry registry,
            IContentFactory editorCoordinator,
            IStoryService storyService,
            NarrativeElementViewModel initialElement,
            PlotPointSubjectLinkViewModel? initialLink = null)
        {
            _registry          = registry;
            _editorCoordinator = editorCoordinator;
            _storyService      = storyService;
            _isSubjectMode     = initialElement is SubjectViewModel;

            DataContext = this;
            InitializeComponent();

            // Capture note selections from any ListBox in the window by type —
            // NoteViewModel items only appear in note section ListBoxes.
            AddHandler(Selector.SelectionChangedEvent,
                new SelectionChangedEventHandler(OnAnySelectionChanged));

            _mainElement = initialElement;
            _mainElement.OnWindowOpened();
            Title = TitleFor(_mainElement);

            if (initialLink is not null)
            {
                SelectedLink = initialLink;
                RefreshSecondaryElement();
            }

            UpdateLayout();
            _registry.LinksInvalidated += UpdateLayout;
        }

        protected override void OnClosed(EventArgs e)
        {
            _registry.LinksInvalidated -= UpdateLayout;
            _mainElement?.OnWindowClosed();
            _secondaryElement?.OnWindowClosed();
            base.OnClosed(e);
        }

        // ── Note selection (routed from any section ListBox in the tree) ──────

        private NoteViewModel? _selectedNote;

        private void OnAnySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is NoteViewModel note)
                _selectedNote = note;
            else if (e.RemovedItems.Count > 0 && e.RemovedItems[0] is NoteViewModel)
                _selectedNote = null;
        }

        // ── Nav button event handlers ─────────────────────────────────────────

        private void NavigateLeft_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedLink is null || _isSubjectMode) return;

            var subject = _registry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == SelectedLink.SubjectId);
            if (subject is null) return;

            PivotToElement(subject, newIsSubjectMode: true);
        }

        private void NavigateRight_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedLink is null || !_isSubjectMode) return;

            var plotPoint = _registry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == SelectedLink.PlotPointId);
            if (plotPoint is null) return;

            PivotToElement(plotPoint, newIsSubjectMode: false);
        }

        private void NavigateLeft_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                OpenLeftInNewWindow();
                e.Handled = true;
            }
        }

        private void NavigateRight_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                OpenRightInNewWindow();
                e.Handled = true;
            }
        }

        // ── Card selection ────────────────────────────────────────────────────

        private void LinkCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedLink = LinkCardsListBox.SelectedItem as PlotPointSubjectLinkViewModel;
            RefreshSecondaryElement();
            UpdateLayout();
        }

        // ── Open in new window (middle-click, accumulative) ───────────────────

        private void OpenLeftInNewWindow()
        {
            if (LeftElement is null || SelectedLink is null || _isSubjectMode) return;

            var subject = _registry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == SelectedLink.SubjectId);
            if (subject is null) return;

            new CommonWindow(_registry, _editorCoordinator, _storyService, subject, SelectedLink)
                .Show();
        }

        private void OpenRightInNewWindow()
        {
            if (RightElement is null || SelectedLink is null || !_isSubjectMode) return;

            var plotPoint = _registry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == SelectedLink.PlotPointId);
            if (plotPoint is null) return;

            new CommonWindow(_registry, _editorCoordinator, _storyService, plotPoint, SelectedLink)
                .Show();
        }

        // ── F-key assignment ──────────────────────────────────────────────────

        private static void AssignFunctionKeys(
            NarrativeElementViewModel? leftElement,
            NarrativeElementViewModel? rightElement)
        {
            AssignRange(leftElement,  startKey: 1);
            AssignRange(rightElement, startKey: 7);
        }

        private static void AssignRange(NarrativeElementViewModel? element, int startKey)
        {
            if (element is null) return;

            var ordered = element.NoteTracks
                .OrderBy(t => t.DisplayOrder)
                .ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                int keyNumber = startKey + i;
                ordered[i].AssignedFunctionKey = keyNumber <= startKey + 5 ? keyNumber : null;
            }
        }

        // ── Window-level F-key handling ───────────────────────────────────────

        private void CommonWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int? fNumber = e.Key switch
            {
                Key.F1  => 1,  Key.F2  => 2,  Key.F3  => 3,  Key.F4  => 4,
                Key.F5  => 5,  Key.F6  => 6,  Key.F7  => 7,  Key.F8  => 8,
                Key.F9  => 9,  Key.F10 => 10, Key.F11 => 11, Key.F12 => 12,
                _ => null
            };

            if (fNumber is null || _selectedNote is null) return;

            var allTracks = (LeftElement?.NoteTracks  ?? Enumerable.Empty<NoteTrackViewModel>())
                .Concat(RightElement?.NoteTracks ?? Enumerable.Empty<NoteTrackViewModel>());

            var target = allTracks.FirstOrDefault(t => t.AssignedFunctionKey == fNumber);
            if (target is null) return;

            MoveNoteToTrack(_selectedNote, target);
            e.Handled = true;
        }

        private void MoveNoteToTrack(NoteViewModel note, NoteTrackViewModel targetTrack)
        {
            var unsetSection = targetTrack.Sections
                .FirstOrDefault(s => s.TargetState == NoteState.Unset);

            int maxOrder = unsetSection?.SectionNotes
                .Cast<NoteViewModel>()
                .Select(n => n.SortOrder)
                .DefaultIfEmpty(0)
                .Max() ?? 0;

            note.OwnerId               = targetTrack.OwnerId;
            note.OwnerType             = targetTrack.OwnerType;
            note.NoteTrackDefinitionId = targetTrack.Definition.Id == UnassignedTrack.Definition.Id
                                            ? null
                                            : targetTrack.Definition.Id;
            note.NoteState             = NoteState.Unset;
            note.SortOrder             = maxOrder + 10;

            _registry.RaiseNoteMutated(note.Id);
            _ = _storyService.SaveAsync();
        }

        // ── Private helpers ───────────────────────────────────────────────────

        /// <summary>
        /// Pivots the window so <paramref name="newMain"/> becomes the main element
        /// while the current main slides to secondary. The selected link is preserved.
        /// Used by nav buttons — the constructor handles initial setup.
        /// </summary>
        private void PivotToElement(NarrativeElementViewModel newMain, bool newIsSubjectMode)
        {
            _mainElement?.OnWindowClosed();

            _mainElement   = newMain;
            _isSubjectMode = newIsSubjectMode;

            Title = TitleFor(_mainElement);

            _mainElement.OnWindowOpened();

            RefreshSecondaryElement();
            UpdateLayout();
        }

        private void RefreshSecondaryElement()
        {
            _secondaryElement?.OnWindowClosed();
            _secondaryElement = null;

            if (SelectedLink is null) return;

            _secondaryElement = SelectedLink;
            _secondaryElement.OnWindowOpened();
        }

        private new void UpdateLayout()
        {
            LeftPanel.Content    = LeftElement;
            LeftPanel.Visibility = LeftElement  is not null ? Visibility.Visible : Visibility.Collapsed;

            RightPanel.Content    = RightElement;
            RightPanel.Visibility = RightElement is not null ? Visibility.Visible : Visibility.Collapsed;

            // Resize columns: collapse inactive side to 0; give primary 2× width when both visible
            var leftCol  = ContentGrid.ColumnDefinitions[0];
            var rightCol = ContentGrid.ColumnDefinitions[2];

            bool leftVisible  = LeftElement  is not null;
            bool rightVisible = RightElement is not null;
            // In subject mode the subject (main) is on the left; in plot-point mode it is on the right
            bool primaryIsLeft = _isSubjectMode;

            if (leftVisible && rightVisible)
            {
                leftCol.Width  = primaryIsLeft
                    ? new GridLength(2, GridUnitType.Star)
                    : new GridLength(1, GridUnitType.Star);
                rightCol.Width = primaryIsLeft
                    ? new GridLength(1, GridUnitType.Star)
                    : new GridLength(2, GridUnitType.Star);
            }
            else
            {
                leftCol.Width  = leftVisible  ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
                rightCol.Width = rightVisible ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            }

            AssignFunctionKeys(LeftElement, RightElement);

            LinkCardsListBox.ItemsSource = GetSortedLinks().ToList();

            LinkCardsListBox.SelectionChanged -= LinkCards_SelectionChanged;
            LinkCardsListBox.SelectedItem      = SelectedLink;
            LinkCardsListBox.SelectionChanged += LinkCards_SelectionChanged;

            NavigateLeftButton.IsEnabled  = !_isSubjectMode && SelectedLink is not null;
            NavigateRightButton.IsEnabled =  _isSubjectMode && SelectedLink is not null;
            DeselectLinkButton.IsEnabled  = SelectedLink is not null;
        }

        private IEnumerable<PlotPointSubjectLinkViewModel> GetSortedLinks()
        {
            if (_isSubjectMode && _mainElement is SubjectViewModel subject)
                return _registry.AllPlotPointSubjectLinkViewModels
                    .Where(l => l.SubjectId == subject.Id)
                    .OrderBy(l => l.ChapterOrderIndex)
                    .ThenBy(l => l.PlotPointOrderInChapter);

            if (!_isSubjectMode && _mainElement is PlotPointViewModel plotPoint)
                return _registry.AllPlotPointSubjectLinkViewModels
                    .Where(l => l.PlotPointId == plotPoint.Id)
                    .OrderBy(l => l.SubjectTypeName)
                    .ThenBy(l => l.SubjectName, StringComparer.CurrentCultureIgnoreCase);

            return Enumerable.Empty<PlotPointSubjectLinkViewModel>();
        }

        private static string TitleFor(NarrativeElementViewModel? vm) => vm switch
        {
            SubjectViewModel s    => $"Subject — {s.Name}",
            PlotPointViewModel pp => $"Plot Point — {pp.Title}",
            _                     => "Story Planner"
        };

        private void DeselectLink_Click(object sender, RoutedEventArgs e)
        {
            SelectedLink = null;
            _secondaryElement?.OnWindowClosed();
            _secondaryElement = null;
            UpdateLayout();
        }
    }
}
