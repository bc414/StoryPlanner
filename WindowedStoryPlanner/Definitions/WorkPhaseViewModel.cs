using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Editable row for one <see cref="WorkPhase"/> in the Definitions tab.
    /// </summary>
    public partial class WorkPhaseViewModel : ObservableObject
    {
        private readonly WorkPhase _model;
        private readonly IStoryService _storyService;

        public WorkPhaseViewModel(WorkPhase model, IStoryService storyService)
        {
            _model = model;
            _storyService = storyService;
        }

        public int Id => _model.Id;
        public WorkPhase Model => _model;

        public string Name
        {
            get => _model.Name;
            set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
        }

        public string Description
        {
            get => _model.Description;
            set => SetProperty(_model.Description, value, _model, (m, v) => m.Description = v);
        }

        public int DisplayOrder
        {
            get => _model.DisplayOrder;
            set => SetProperty(_model.DisplayOrder, value, _model, (m, v) => m.DisplayOrder = v);
        }

        public bool RequiresZeroFlaggedNotes
        {
            get => _model.RequiresZeroFlaggedNotes;
            set => SetProperty(_model.RequiresZeroFlaggedNotes, value, _model, (m, v) => m.RequiresZeroFlaggedNotes = v);
        }

        public bool RequiresZeroUnsetNotes
        {
            get => _model.RequiresZeroUnsetNotes;
            set => SetProperty(_model.RequiresZeroUnsetNotes, value, _model, (m, v) => m.RequiresZeroUnsetNotes = v);
        }

        /// <summary>Label for the gating-phase combo on the property-definition grid.</summary>
        public override string ToString() => Name;
    }
}
