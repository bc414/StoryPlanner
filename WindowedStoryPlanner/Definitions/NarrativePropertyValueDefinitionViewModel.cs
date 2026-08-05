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
    public partial class NarrativePropertyValueDefinitionViewModel : ObservableObject, IColorHexOwner
    {
        private readonly NarrativePropertyValueDefinition _model;
        private readonly IReadOnlyList<NarrativePropertyDefinitionViewModel> _properties;

        public NarrativePropertyValueDefinitionViewModel(
            NarrativePropertyValueDefinition model,
            IReadOnlyList<NarrativePropertyDefinitionViewModel> properties)
        {
            _model = model;
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

        /// <summary>
        /// Display colour, "#RRGGBB", edited through <see cref="ColorPickerControl"/> in the grid.
        /// Empty is a legal, unfinished state and renders as ChipInk's neutral; nothing assigns one.
        /// </summary>
        public string ColorHex
        {
            get => _model.ColorHex;
            set => SetProperty(_model.ColorHex, value, _model, (m, v) => m.ColorHex = v);
        }
    }
}
