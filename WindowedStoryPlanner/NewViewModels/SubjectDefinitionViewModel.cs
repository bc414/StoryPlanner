using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class SubjectDefinitionViewModel : ObservableObject
    {
        [ObservableProperty] SubjectDefinition _model;
        private readonly IStoryService _storyService;

        public string SubjectType
        {
            get => _model.SubjectType;
            set => SetProperty(_model.SubjectType, value, _model, (m, v) => m.SubjectType = v);
        }

        public int Id => Model.Id;

        public SubjectDefinitionViewModel(SubjectDefinition model, IStoryService storyService)
        {
            _model = model;
            _storyService = storyService;

            SubjectType = _model.SubjectType;
        }
    }
}
