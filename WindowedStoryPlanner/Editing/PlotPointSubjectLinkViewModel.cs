using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace WindowedStoryPlanner
{
    public partial class PlotPointSubjectLinkViewModel : NarrativeElementViewModel
    {
        private readonly PlotPointSubjectLink _link;
        public PlotPointSubjectLink Link => _link;

        public int Id => _link.Id;

        public int SubjectId
        {
            get => _link.SubjectId;
            set
            {
                if (_link.SubjectId == value) return;
                UnsubscribeSubject();
                SetProperty(_link.SubjectId, value, _link, (l, n) => l.SubjectId = n);
                SubscribeSubject();
                OnPropertyChanged(nameof(SubjectName));
                OnPropertyChanged(nameof(SubjectTypeName));
            }
        }

        public int PlotPointId
        {
            get => _link.PlotPointId;
            set
            {
                if (_link.PlotPointId == value) return;
                UnsubscribePlotPoint();
                SetProperty(_link.PlotPointId, value, _link, (l, n) => l.PlotPointId = n);
                SubscribePlotPoint();
                OnPropertyChanged(nameof(PlotPointDisplayText));
                OnPropertyChanged(nameof(ChapterOrderIndex));
                OnPropertyChanged(nameof(PlotPointOrderInChapter));
            }
        }

        public int ChapterOrderIndex =>
            _viewModelRegistry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == _link.PlotPointId)
                ?.ChapterId is int chapterId
                    ? _viewModelRegistry.AllChapterViewModels
                        .FirstOrDefault(c => c.Id == chapterId)
                        ?.OrderIndex ?? int.MaxValue
                    : int.MaxValue;

        public int PlotPointOrderInChapter =>
            _viewModelRegistry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == _link.PlotPointId)
                ?.OrderInChapter ?? int.MaxValue;

        public string SubjectName =>
            _viewModelRegistry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == _link.SubjectId)
                ?.Name ?? string.Empty;

        public string SubjectTypeName =>
            _viewModelRegistry.AllSubjectViewModels
                .FirstOrDefault(s => s.Id == _link.SubjectId) is SubjectViewModel subject
                    ? _viewModelRegistry.AllSubjectDefinitionViewModels
                        .FirstOrDefault(d => d.Id == subject.SubjectDefinitionId)
                        ?.SubjectType ?? string.Empty
                    : string.Empty;

        public string PlotPointDisplayText =>
            _viewModelRegistry.AllPlotPointViewModels
                .FirstOrDefault(pp => pp.Id == _link.PlotPointId) is PlotPointViewModel pp
                    ? $"{pp.FullOrder}{pp.Title}"
                    : string.Empty;

        public bool HasNotes =>
            _storyService.Notes.Any(n =>
                n.OwnerId == _link.Id &&
                n.OwnerType == OwnerType.PlotPointSubjectLink);

        /// <summary>True when this link's subject is the owning plot point's designated POV
        /// character — the gate for IsFocalCharacterOnly tracks (see InitializeCollections).</summary>
        private bool IsFocalLink =>
            _storyService.PlotPoints
                .FirstOrDefault(p => p.Id == _link.PlotPointId)
                ?.FocalCharacterId == _link.SubjectId;

        private bool HasNotesOnTrack(int trackDefinitionId) =>
            _storyService.Notes.Any(n =>
                n.OwnerId == _link.Id &&
                n.OwnerType == OwnerType.PlotPointSubjectLink &&
                n.NoteTrackDefinitionId == trackDefinitionId);

        private bool CanDelete() => !HasNotes;

        [RelayCommand(CanExecute = nameof(CanDelete))]
        private void DeleteSelf()
        {
            var modelToRemove = _storyService.PlotPointsSubjectLinks
                .FirstOrDefault(l => l.Id == _link.Id);
            if (modelToRemove is null) return;

            _storyService.PlotPointsSubjectLinks.Remove(modelToRemove);

            var vmToRemove = _viewModelRegistry.AllPlotPointSubjectLinkViewModels
                .FirstOrDefault(vm => vm.Id == _link.Id);
            if (vmToRemove is not null)
                _viewModelRegistry.AllPlotPointSubjectLinkViewModels.Remove(vmToRemove);

            _storyService.SaveAsync().FireAndForget();
            _viewModelRegistry.RaiseLinksInvalidated();
        }

        public PlotPointSubjectLinkViewModel(
            PlotPointSubjectLink link,
            IViewModelRegistry viewModelRegistry,
            IStoryService storyService,
            IContentFactory editorCoordinator,
            AppSettings appSettings,
            ExportService exportService)
            : base(viewModelRegistry, storyService, editorCoordinator, appSettings, exportService)
        {
            _link = link;

            var subjectDefId = storyService.Subjects
                .First(s => s.Id == link.SubjectId)
                .SubjectDefinitionId;

            InitializeCollections(
                link.Id,
                OwnerType.PlotPointSubjectLink,
                // A track flagged IsFocalCharacterOnly (see docs/design-conversations/053 blocks
                // 262-263) only belongs on the POV character's own link — showing the gap on an
                // observed character's link would be a category error. The safety valve:
                // existing notes are never hidden, even on a non-focal link.
                () => storyService.NoteTrackDefinitions
                    .Where(ntd => ntd.OwnerType == OwnerType.PlotPointSubjectLink
                               && ntd.SubjectDefinitionId == subjectDefId
                               && (!ntd.IsFocalCharacterOnly || IsFocalLink || HasNotesOnTrack(ntd.Id)))
                    .ToList(),
                () => storyService.NarrativePropertyDefinitions
                    .Where(npd => npd.OwnerType == OwnerType.PlotPointSubjectLink
                               && npd.SubjectDefinitionId == subjectDefId)
                    .OrderBy(npd => npd.DisplayOrder)
                    .ToList());

            SubscribePlotPoint();
            SubscribeSubject();

            // Re-evaluate CanDelete when notes are added/removed or mutated.
            // Guard against bulk load: AllNoteViewModels grows during load but HasNotes
            // reads from _storyService.Notes (pre-populated), so no-op until story is settled.
            _viewModelRegistry.AllNoteViewModels.CollectionChanged += (_, _) =>
            {
                if (!_viewModelRegistry.IsStoryLoaded) return;
                OnPropertyChanged(nameof(HasNotes));
                DeleteSelfCommand.NotifyCanExecuteChanged();
            };
            _viewModelRegistry.NoteViewModelMutated += args =>
            {
                if (args.OwnerId != _link.Id || args.OwnerType != OwnerType.PlotPointSubjectLink)
                    return;
                OnPropertyChanged(nameof(HasNotes));
                DeleteSelfCommand.NotifyCanExecuteChanged();
            };
        }

        private void SubscribePlotPoint()
        {
            var vm = _viewModelRegistry.AllPlotPointViewModels.FirstOrDefault(pp => pp.Id == _link.PlotPointId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .AddHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnPlotPointPropertyChanged);
        }

        private void UnsubscribePlotPoint()
        {
            var vm = _viewModelRegistry.AllPlotPointViewModels.FirstOrDefault(pp => pp.Id == _link.PlotPointId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .RemoveHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnPlotPointPropertyChanged);
        }

        private void SubscribeSubject()
        {
            var vm = _viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == _link.SubjectId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .AddHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnSubjectPropertyChanged);
        }

        private void UnsubscribeSubject()
        {
            var vm = _viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == _link.SubjectId);
            if (vm is null) return;
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>
                .RemoveHandler(vm, nameof(INotifyPropertyChanged.PropertyChanged), OnSubjectPropertyChanged);
        }

        private void OnPlotPointPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PlotPointDisplayText));
            OnPropertyChanged(nameof(ChapterOrderIndex));
            OnPropertyChanged(nameof(PlotPointOrderInChapter));
        }

        private void OnSubjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SubjectName));
            OnPropertyChanged(nameof(SubjectTypeName));
        }
    }
}
