using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// Editable row for one <see cref="NarrativePropertyValueDefinition"/> — an allowed answer for
    /// one property. Distinct from <see cref="NarrativePropertyValueViewModel"/>, which is the
    /// read-only projection the entity editor's picker binds to.
    /// </summary>
    public partial class NarrativePropertyValueDefinitionViewModel : ObservableObject
    {
        private readonly NarrativePropertyValueDefinition _model;
        private readonly IStoryService _storyService;
        private readonly IReadOnlyList<NarrativePropertyDefinitionViewModel> _properties;

        public NarrativePropertyValueDefinitionViewModel(
            NarrativePropertyValueDefinition model,
            IStoryService storyService,
            IReadOnlyList<NarrativePropertyDefinitionViewModel> properties)
        {
            _model = model;
            _storyService = storyService;
            _properties = properties;
        }

        public int Id => _model.Id;
        public NarrativePropertyValueDefinition Model => _model;

        public int NarrativePropertyDefinitionId => _model.NarrativePropertyDefinitionId;

        public string ValueName
        {
            get => _model.ValueName;
            set => SetProperty(_model.ValueName, value, _model, (m, v) => m.ValueName = v);
        }

        public string Description
        {
            get => _model.Description;
            set => SetProperty(_model.Description, value, _model, (m, v) => m.Description = v);
        }

        /// <summary>Owning property, resolved to its name for the grid's read-only context column.</summary>
        public string PropertyName =>
            _properties.FirstOrDefault(p => p.Id == _model.NarrativePropertyDefinitionId)?.Name ?? "(unknown)";
    }
}
