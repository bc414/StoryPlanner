using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Editable row for one <see cref="SubjectRelationDefinition"/> — a kind of edge subjects may
    /// draw to one another. Same shape as the other definition rows.
    /// </summary>
    public partial class SubjectRelationDefinitionViewModel : ObservableObject
    {
        private readonly SubjectRelationDefinition _model;
        private readonly IReadOnlyList<SubjectDefinitionViewModel> _subjectDefinitions;

        public SubjectRelationDefinitionViewModel(
            SubjectRelationDefinition model,
            IReadOnlyList<SubjectDefinitionViewModel> subjectDefinitions)
        {
            _model = model;
            _subjectDefinitions = subjectDefinitions;
        }

        public int Id => _model.Id;
        public SubjectRelationDefinition Model => _model;
        public int SubjectDefinitionId => _model.SubjectDefinitionId;
        public int TargetSubjectDefinitionId => _model.TargetSubjectDefinitionId;

        public string Name
        {
            get => _model.Name;
            set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
        }

        public string InverseName
        {
            get => _model.InverseName;
            set => SetProperty(_model.InverseName, value, _model, (m, v) => m.InverseName = v);
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

        public bool IsSingle
        {
            get => _model.IsSingle;
            set => SetProperty(_model.IsSingle, value, _model, (m, v) => m.IsSingle = v);
        }

        /// <summary>
        /// Acyclic parent pointer, walkable as a tree. Requires the source and target subject types
        /// to match; PlanIntegrity reports subjectrelationdefinition.hierarchy_cross_type otherwise.
        /// </summary>
        public bool FormsHierarchy
        {
            get => _model.FormsHierarchy;
            set => SetProperty(_model.FormsHierarchy, value, _model, (m, v) => m.FormsHierarchy = v);
        }

        public string SelectedSubjectType
        {
            get => _subjectDefinitions.FirstOrDefault(s => s.Id == _model.SubjectDefinitionId)?.SubjectType ?? string.Empty;
            set
            {
                var match = _subjectDefinitions.FirstOrDefault(s => s.SubjectType == value);
                if (match is not null)
                {
                    SetProperty(_model.SubjectDefinitionId, match.Id, _model, (m, v) => m.SubjectDefinitionId = v);
                    OnPropertyChanged(nameof(DisplayLabel));
                }
            }
        }

        public string SelectedTargetSubjectType
        {
            get => _subjectDefinitions.FirstOrDefault(s => s.Id == _model.TargetSubjectDefinitionId)?.SubjectType ?? string.Empty;
            set
            {
                var match = _subjectDefinitions.FirstOrDefault(s => s.SubjectType == value);
                if (match is not null)
                {
                    SetProperty(_model.TargetSubjectDefinitionId, match.Id, _model, (m, v) => m.TargetSubjectDefinitionId = v);
                    OnPropertyChanged(nameof(DisplayLabel));
                }
            }
        }

        /// <summary>Label for the tree view's edge selector.</summary>
        public string DisplayLabel => string.IsNullOrWhiteSpace(Name) ? $"(unnamed relation {Id})" : Name;
    }
}
