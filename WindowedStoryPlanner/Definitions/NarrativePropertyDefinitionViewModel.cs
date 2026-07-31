using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Editable row for one <see cref="NarrativePropertyDefinition"/> in the Definitions tab.
    /// Mirrors <see cref="NoteTrackDefinitionViewModel"/>: ids are resolved to and from display
    /// strings so the grid can use plain combo boxes over the definition lists.
    /// </summary>
    public partial class NarrativePropertyDefinitionViewModel : ObservableObject
    {
        private readonly NarrativePropertyDefinition _model;
        private readonly IStoryService _storyService;
        private readonly IReadOnlyList<SubjectDefinitionViewModel> _subjectDefinitions;
        private readonly IReadOnlyList<WorkPhaseViewModel> _workPhases;

        public NarrativePropertyDefinitionViewModel(
            NarrativePropertyDefinition model,
            IStoryService storyService,
            IReadOnlyList<SubjectDefinitionViewModel> subjectDefinitions,
            IReadOnlyList<WorkPhaseViewModel> workPhases)
        {
            _model = model;
            _storyService = storyService;
            _subjectDefinitions = subjectDefinitions;
            _workPhases = workPhases;
        }

        public int Id => _model.Id;
        public NarrativePropertyDefinition Model => _model;

        public string Name
        {
            get => _model.Name;
            set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
        }

        public string Question
        {
            get => _model.Question;
            set => SetProperty(_model.Question, value, _model, (m, v) => m.Question = v);
        }

        public string Explanation
        {
            get => _model.Explanation;
            set => SetProperty(_model.Explanation, value, _model, (m, v) => m.Explanation = v);
        }

        public int DisplayOrder
        {
            get => _model.DisplayOrder;
            set => SetProperty(_model.DisplayOrder, value, _model, (m, v) => m.DisplayOrder = v);
        }

        public OwnerType OwnerType
        {
            get => _model.OwnerType;
            set => SetProperty(_model.OwnerType, value, _model, (m, v) => m.OwnerType = v);
        }

        // Resolves SubjectDefinitionId → display string; sets SubjectDefinitionId on write.
        // Meaningful for OwnerType.Subject and .PlotPointSubjectLink only — the PlotPoint and
        // Chapter call sites filter on OwnerType alone and ignore this, exactly as they do for
        // note track definitions.
        public string SelectedSubjectType
        {
            get => _subjectDefinitions.FirstOrDefault(s => s.Id == _model.SubjectDefinitionId)?.SubjectType ?? string.Empty;
            set
            {
                var match = _subjectDefinitions.FirstOrDefault(s => s.SubjectType == value);
                if (match is not null)
                    SetProperty(_model.SubjectDefinitionId, match.Id, _model, (m, v) => m.SubjectDefinitionId = v);
            }
        }

        /// <summary>
        /// The phase at which an unset value counts as an open gap. Null is the normal state and
        /// means the property never gates — leaving it null keeps a property you are still unsure
        /// about fully usable without it reporting anything.
        /// </summary>
        public WorkPhaseViewModel? SelectedWorkPhase
        {
            get => _workPhases.FirstOrDefault(p => p.Id == _model.GatingWorkPhaseId);
            set => SetProperty(_model.GatingWorkPhaseId, value?.Id, _model, (m, v) => m.GatingWorkPhaseId = v);
        }
    }
}
