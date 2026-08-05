using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Editable row for one <see cref="PropertyBoard"/> in the Definitions tab, and the item the
    /// Boards tab's selector binds to. Same shape as the other definition rows: ids resolve to and
    /// from display strings so the grid can use plain combo boxes.
    /// </summary>
    public partial class PropertyBoardViewModel : ObservableObject
    {
        private readonly PropertyBoard _model;
        private readonly IReadOnlyList<SubjectDefinitionViewModel> _subjectDefinitions;

        public PropertyBoardViewModel(
            PropertyBoard model,
            IReadOnlyList<SubjectDefinitionViewModel> subjectDefinitions)
        {
            _model = model;
            _subjectDefinitions = subjectDefinitions;
        }

        public int Id => _model.Id;
        public PropertyBoard Model => _model;
        public int SubjectDefinitionId => _model.SubjectDefinitionId;

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

        /// <summary>
        /// Whether "(unset)" is a band on each grid axis. Off means a subject unset on either axis
        /// is absent from that grid entirely — see <see cref="PropertyBoard.IncludeUnsetBand"/>.
        /// </summary>
        public bool IncludeUnsetBand
        {
            get => _model.IncludeUnsetBand;
            set => SetProperty(_model.IncludeUnsetBand, value, _model, (m, v) => m.IncludeUnsetBand = v);
        }

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

        /// <summary>Label for the Boards tab selector, which shows several boards at once.</summary>
        public string DisplayLabel => string.IsNullOrWhiteSpace(SelectedSubjectType)
            ? Name
            : $"{Name} — {SelectedSubjectType}";
    }
}
