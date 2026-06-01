using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class NarrativePropertyValueViewModel : ObservableObject
    {
        private readonly NarrativePropertyValueDefinition _narrativePropertyValueDefinition;

        public NarrativePropertyValueViewModel(NarrativePropertyValueDefinition narrativePropertyValueDefinition)
        {
            _narrativePropertyValueDefinition = narrativePropertyValueDefinition;
        }

        public string ValueName => _narrativePropertyValueDefinition.ValueName;
        public string Description => _narrativePropertyValueDefinition.Description;
        public int Id => _narrativePropertyValueDefinition.Id;
    }
}
