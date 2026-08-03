using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowedStoryPlanner
{
    public partial class SubjectDefinitionViewModel : ObservableObject
    {
        SubjectDefinition _model;

        public string SubjectType
        {
            get => _model.SubjectType;
            set => SetProperty(_model.SubjectType, value, _model, (m, v) => m.SubjectType = v);
        }

        public int Id => _model.Id;
        public SubjectDefinition Model => _model;

        public int DisplayOrder
        {
            get => _model.DisplayOrder;
            set
            {
                if (_model.DisplayOrder != value)
                {
                    _model.DisplayOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public SubjectDefinitionViewModel(SubjectDefinition model)
        {
            _model = model;

            SubjectType = _model.SubjectType;
        }
    }
}
