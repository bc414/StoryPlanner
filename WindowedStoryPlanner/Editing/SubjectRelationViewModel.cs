using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// One relation on one subject — a picker over the subjects it may point at.
    ///
    /// Deliberately shaped like <see cref="NarrativePropertyViewModel"/> rather than like a note
    /// track: absence of a <see cref="SubjectRelation"/> row is "unset", a legal and long-lived
    /// state, and clearing DELETES the row rather than blanking it. There is no "(none)" entry in
    /// <see cref="Candidates"/> for the same reason the property picker has none — a sentinel's id
    /// would be stored and read back as a real answer.
    ///
    /// The edge is never derived. Nothing here inspects names, note text, dates, or shared
    /// vocabulary to propose a target; assignment is authorial (CLAUDE.md: retrieval, not
    /// suggestion). The one succession recorded in the real file skips three intervening regimes
    /// and shares no name token with its target, which is exactly why.
    /// </summary>
    public partial class SubjectRelationViewModel : ObservableObject
    {
        private readonly SubjectRelationDefinition _definition;
        private readonly IStoryService _storyService;
        private readonly IViewModelRegistry _registry;
        private readonly int _subjectId;

        /// <summary>The persisted edge on a single-valued relation, or null when unset.</summary>
        private SubjectRelation? _edge;

        public SubjectRelationViewModel(
            int subjectId,
            SubjectRelationDefinition definition,
            IViewModelRegistry registry,
            IStoryService storyService)
        {
            _subjectId    = subjectId;
            _definition   = definition;
            _registry     = registry;
            _storyService = storyService;

            if (_definition.IsSingle)
            {
                _edge = _storyService.SubjectRelations
                    .FirstOrDefault(r => r.RelationDefinitionId == _definition.Id && r.SubjectId == _subjectId);

                _selectedTarget = _registry.AllSubjectViewModels
                    .FirstOrDefault(s => s.Id == _edge?.TargetSubjectId);
            }
            else
            {
                foreach (var vm in CurrentTargetViewModels())
                    Targets.Add(vm);
            }
        }

        public int DefinitionId => _definition.Id;
        public string Name => _definition.Name;
        public string Question => _definition.Question;
        public string Explanation => _definition.Explanation;
        public bool IsSingle => _definition.IsSingle;
        public bool IsMulti => !_definition.IsSingle;

        /// <summary>
        /// Subjects this edge may point at: the target type, minus this subject, and on a hierarchy
        /// relation minus everything already beneath it. Excluding descendants is what makes a cycle
        /// unauthorable rather than merely reported — PlanIntegrity stays the auditor for rows that
        /// arrive by another route.
        ///
        /// Recomputed on read rather than cached: the candidate set depends on edges the author may
        /// have changed elsewhere in this same session.
        /// </summary>
        public IEnumerable<SubjectViewModel> Candidates
        {
            get
            {
                var excluded = _definition.FormsHierarchy
                    ? SubjectRelationGraph.Descendants(_storyService.SubjectRelations, _definition.Id, _subjectId)
                    : (IReadOnlySet<int>)new HashSet<int>();

                // Already-chosen targets drop out on a multi relation, so the same edge cannot be
                // added twice.
                var alreadyChosen = IsMulti
                    ? Targets.Select(t => t.Id).ToHashSet()
                    : new HashSet<int>();

                return _registry.AllSubjectViewModels
                    .Where(s => s.SubjectDefinitionId == _definition.TargetSubjectDefinitionId
                             && s.Id != _subjectId
                             && !excluded.Contains(s.Id)
                             && !alreadyChosen.Contains(s.Id))
                    .OrderBy(s => s.Name, System.StringComparer.CurrentCultureIgnoreCase);
            }
        }

        // ── Multi-valued relations ────────────────────────────────────────────

        /// <summary>Chosen targets, in SortOrder. Empty on a single-valued relation.</summary>
        public ObservableCollection<SubjectViewModel> Targets { get; } = new();

        /// <summary>The combo's pending pick, cleared as soon as Add consumes it.</summary>
        [ObservableProperty]
        private SubjectViewModel? _pendingTarget;

        private IEnumerable<SubjectViewModel> CurrentTargetViewModels() =>
            _storyService.SubjectRelations
                .Where(r => r.RelationDefinitionId == _definition.Id && r.SubjectId == _subjectId)
                .OrderBy(r => r.SortOrder).ThenBy(r => r.Id)
                .Select(r => _registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == r.TargetSubjectId))
                .OfType<SubjectViewModel>();

        [RelayCommand]
        private void AddTarget()
        {
            if (PendingTarget is null || IsSingle) return;

            var nextOrder = _storyService.SubjectRelations
                .Where(r => r.RelationDefinitionId == _definition.Id && r.SubjectId == _subjectId)
                .Select(r => r.SortOrder)
                .DefaultIfEmpty(-1)
                .Max() + 1;

            _storyService.SubjectRelations.Add(new SubjectRelation
            {
                RelationDefinitionId = _definition.Id,
                SubjectId = _subjectId,
                TargetSubjectId = PendingTarget.Id,
                SortOrder = nextOrder
            });

            Targets.Add(PendingTarget);
            PendingTarget = null;
            OnPropertyChanged(nameof(Candidates));
            _storyService.SaveAsync().FireAndForget();
        }

        [RelayCommand]
        private void RemoveTarget(SubjectViewModel? target)
        {
            if (target is null) return;

            var row = _storyService.SubjectRelations.FirstOrDefault(
                r => r.RelationDefinitionId == _definition.Id
                  && r.SubjectId == _subjectId
                  && r.TargetSubjectId == target.Id);
            if (row is not null) _storyService.SubjectRelations.Remove(row);

            Targets.Remove(target);
            OnPropertyChanged(nameof(Candidates));
            _storyService.SaveAsync().FireAndForget();
        }

        private SubjectViewModel? _selectedTarget;
        public SubjectViewModel? SelectedTarget
        {
            get => _selectedTarget;
            set
            {
                if (!SetProperty(ref _selectedTarget, value)) return;

                if (value is null)
                {
                    if (_edge is not null)
                    {
                        _storyService.SubjectRelations.Remove(_edge);
                        _edge = null;
                    }
                }
                else if (_edge is null)
                {
                    _edge = new SubjectRelation
                    {
                        RelationDefinitionId = _definition.Id,
                        SubjectId = _subjectId,
                        TargetSubjectId = value.Id
                    };
                    _storyService.SubjectRelations.Add(_edge);
                }
                else
                {
                    _edge.TargetSubjectId = value.Id;
                }

                // Not awaited — nothing here reads the new row's assigned Id. But .FireAndForget()
                // rather than a bare discard: a discarded Task swallows its exception, so a failed
                // save would be silent, which is worse than a crash for an app whose product is
                // the data file.
                _storyService.SaveAsync().FireAndForget();

                ClearCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanClear() => SelectedTarget is not null;

        [RelayCommand(CanExecute = nameof(CanClear))]
        private void Clear() => SelectedTarget = null;
    }
}
