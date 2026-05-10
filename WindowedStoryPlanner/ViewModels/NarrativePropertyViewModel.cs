using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class NarrativePropertyViewModel : ObservableObject
    {
        private NarrativePropertyValueViewModel _selectedValue;
        public NarrativePropertyValueViewModel SelectedValue
        {
            get => _selectedValue;
            set
            {
                SetProperty(ref _selectedValue, value);
                if (actualValue != null)
                {
                    actualValue.ValueDefinitionId = value.Id;
                }
            }
        }

        private NarrativePropertyValue? actualValue;

        public ObservableCollection<NarrativePropertyValueViewModel> ValidValues { get; set; }
        public NarrativePropertyViewModel(int ownerId, OwnerType ownerType, NarrativePropertyDefinition narrativePropertyDefinition, IViewModelRegistry viewModelRegistry, IStoryService storyService)
        {
            //get valid values
            ValidValues = new ObservableCollection<NarrativePropertyValueViewModel>();
            var defs = viewModelRegistry.AllNarrativePropertyValueDefinitions.Where(n => n.Id == narrativePropertyDefinition.Id);
            foreach (var def in defs)
            {
                ValidValues.Add(def);
            }

            actualValue = storyService.NarrativePropertyValues.FirstOrDefault(n => n.OwnerId == ownerId && n.ValueDefinitionId == narrativePropertyDefinition.Id);
            _selectedValue = ValidValues.FirstOrDefault(v => v.Id == actualValue?.ValueDefinitionId) ?? ValidValues.First();
        }

        
    }
}
