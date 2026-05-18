using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class NoteTrackDefinitionViewModel : ObservableObject
    {
        private readonly NoteTrackDefinition _model;
        public NoteTrackDefinition Model => _model;
        private readonly IStoryService _storyService;
        private readonly IReadOnlyList<SubjectDefinitionViewModel> _subjectDefinitions;

        public int Id           => _model.Id;
        public OwnerType OwnerType
        {
            get => _model.OwnerType;
            set => SetProperty(_model.OwnerType, value, _model, (m, v) => m.OwnerType = v);
        }
        public string TrackName
        {
            get => _model.TrackName;
            set => SetProperty(_model.TrackName, value, _model, (m, v) => m.TrackName = v);
        }
        public string DisplayQuestion
        {
            get => _model.DisplayQuestion;
            set => SetProperty(_model.DisplayQuestion, value, _model, (m, v) => m.DisplayQuestion = v);
        }
        public string UsageDirective
        {
            get => _model.UsageDirective;
            set => SetProperty(_model.UsageDirective, value, _model, (m, v) => m.UsageDirective = v);
        }
        public string AuditDirective
        {
            get => _model.AuditDirective;
            set => SetProperty(_model.AuditDirective, value, _model, (m, v) => m.AuditDirective = v);
        }
        public int DisplayOrder
        {
            get => _model.DisplayOrder;
            set => SetProperty(_model.DisplayOrder, value, _model, (m, v) => m.DisplayOrder = v);
        }
        public CognitiveMode CognitiveMode
        {
            get => _model.CognitiveMode;
            set => SetProperty(_model.CognitiveMode, value, _model, (m, v) => m.CognitiveMode = v);
        }
        public bool IsSingleton
        {
            get => _model.IsSingleton;
            set => SetProperty(_model.IsSingleton, value, _model, (m, v) => m.IsSingleton = v);
        }
        public bool SupportsWorldDate
        {
            get => _model.SupportsWorldDate;
            set => SetProperty(_model.SupportsWorldDate, value, _model, (m, v) => m.SupportsWorldDate = v);
        }
        public bool SupportsTheme
        {
            get => _model.SupportsTheme;
            set => SetProperty(_model.SupportsTheme, value, _model, (m, v) => m.SupportsTheme = v);
        }
        public bool CanEditInAuditMode
        {
            get => _model.CanEditInAuditMode;
            set => SetProperty(_model.CanEditInAuditMode, value, _model, (m, v) => m.CanEditInAuditMode = v);
        }

        // Resolves SubjectDefinitionId → display string; sets SubjectDefinitionId on write
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

        public NoteTrackDefinitionViewModel(
            NoteTrackDefinition model,
            IStoryService storyService,
            IReadOnlyList<SubjectDefinitionViewModel> subjectDefinitions)
        {
            _model = model;
            _storyService = storyService;
            _subjectDefinitions = subjectDefinitions;
        }
    }
}
