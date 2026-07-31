using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace WindowedStoryPlanner
{
    public partial class PlotPointViewModel : NarrativeElementViewModel
    {
        public ICollectionView? PlotPointSubjectLinks { get; private set; }
        private PlotPoint _plotPoint;
        public PlotPoint PlotPoint => _plotPoint;
        private readonly IWindowManager _windowManager;

        public int Id => _plotPoint.Id;

        public int? ChapterId
        {
            get => _plotPoint.ChapterId;
            set
            {
                SetProperty(_plotPoint.ChapterId, value, _plotPoint, (p, n) => p.ChapterId = n);
                OnPropertyChanged(nameof(FullOrder));
                _viewModelRegistry.RaiseLinksInvalidated();
            }
        }

        public int OrderInChapter
        {
            get => _plotPoint.OrderInChapter;
            set
            {
                SetProperty(_plotPoint.OrderInChapter, value, _plotPoint, (p, n) => p.OrderInChapter = n);
                OnPropertyChanged(nameof(FullOrder));
                _viewModelRegistry.RaiseLinksInvalidated();
            }
        }

        public string Title
        {
            get => _plotPoint.Title;
            set => SetProperty(_plotPoint.Title, value, _plotPoint, (p, n) => p.Title = n);
        }

        // "{story reading order}.{chapter order}.{position in chapter}" — e.g. "3.12.4".
        public string FullOrder
        {
            get
            {
                if (ChapterId is null) return "? ";
                var chapter = _viewModelRegistry.AllChapterViewModels.FirstOrDefault(c => c.Id == ChapterId);
                return chapter is null ? "?.? " : $"{chapter.FullNumber}.{OrderInChapter} ";
            }
        }

        /// <summary>Candidates for the focal-character picker: subjects the author has
        /// designated as POV-capable (Subject.IsPovCharacter), with subjects already linked to
        /// this scene sorted first, then alphabetically. Not restricted to linked subjects —
        /// POV may legitimately be set before links exist.</summary>
        public IEnumerable<SubjectViewModel> FocalCharacterChoices =>
            _viewModelRegistry.AllSubjectViewModels
                .Where(s => s.IsPovCharacter)
                .OrderByDescending(s => _viewModelRegistry.AllPlotPointSubjectLinkViewModels
                    .Any(l => l.PlotPointId == _plotPoint.Id && l.SubjectId == s.Id))
                .ThenBy(s => s.Name, StringComparer.CurrentCultureIgnoreCase);

        /// <summary>Resolves FocalCharacterId → SubjectViewModel for display; sets
        /// FocalCharacterId on write. Null means "no POV designated for this scene". Setting
        /// this rebuilds every link's note tracks so IsFocalCharacterOnly tracks move to (or
        /// off) the newly (un)designated character's link.</summary>
        public SubjectViewModel? SelectedFocalCharacter
        {
            get => _viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == _plotPoint.FocalCharacterId);
            set
            {
                var newId = value?.Id;
                if (_plotPoint.FocalCharacterId == newId) return;
                SetProperty(_plotPoint.FocalCharacterId, newId, _plotPoint, (p, n) => p.FocalCharacterId = n);
                OnPropertyChanged();
                RebuildLinkedTracks();
            }
        }

        [RelayCommand]
        private void ClearFocalCharacter() => SelectedFocalCharacter = null;

        private void RebuildLinkedTracks()
        {
            foreach (var link in _viewModelRegistry.AllPlotPointSubjectLinkViewModels
                         .Where(l => l.PlotPointId == _plotPoint.Id))
                link.RebuildAndInitializeTracks();
        }

        [RelayCommand]
        private void Open() => _windowManager.OpenPlotPointWindow(this);

        public PlotPointViewModel(
            PlotPoint plotPoint,
            IViewModelRegistry viewModelRegistry,
            IStoryService storyService,
            IContentFactory editorCoordinator,
            IWindowManager windowManager,
            AppSettings appSettings,
            ExportService exportService)
            : base(viewModelRegistry, storyService, editorCoordinator, appSettings, exportService)
        {
            _plotPoint = plotPoint;

            InitializeCollections(
                plotPoint.Id,
                OwnerType.PlotPoint,
                () => storyService.NoteTrackDefinitions
                          .Where(ntd => ntd.OwnerType == OwnerType.PlotPoint)
                          .ToList(),
                () => storyService.NarrativePropertyDefinitions
                          .Where(npd => npd.OwnerType == OwnerType.PlotPoint)
                          .OrderBy(npd => npd.DisplayOrder)
                          .ToList());

            if (viewModelRegistry.IsStoryLoaded)
                BuildLinkView();

            _windowManager = windowManager;
        }

        protected override void OnStoryFullyLoaded()
        {
            BuildLinkView();
        }

        private void BuildLinkView()
        {
            var view = new ListCollectionView(_viewModelRegistry.AllPlotPointSubjectLinkViewModels)
            {
                Filter = obj => obj is PlotPointSubjectLinkViewModel link && link.PlotPointId == _plotPoint.Id,
                CustomSort = Comparer<object>.Create((a, b) =>
                {
                    if (a is not PlotPointSubjectLinkViewModel la || b is not PlotPointSubjectLinkViewModel lb)
                        return 0;

                    var subjectA = _viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == la.SubjectId);
                    var subjectB = _viewModelRegistry.AllSubjectViewModels.FirstOrDefault(s => s.Id == lb.SubjectId);

                    var defOrderA = _viewModelRegistry.AllSubjectDefinitionViewModels
                        .FirstOrDefault(d => d.Id == subjectA?.SubjectDefinitionId)?.DisplayOrder ?? int.MaxValue;
                    var defOrderB = _viewModelRegistry.AllSubjectDefinitionViewModels
                        .FirstOrDefault(d => d.Id == subjectB?.SubjectDefinitionId)?.DisplayOrder ?? int.MaxValue;

                    int defCompare = defOrderA.CompareTo(defOrderB);
                    if (defCompare != 0) return defCompare;

                    return string.Compare(subjectA?.Name, subjectB?.Name, StringComparison.CurrentCultureIgnoreCase);
                })
            };
            PlotPointSubjectLinks = view;
            OnPropertyChanged(nameof(PlotPointSubjectLinks));
        }
    }
}
