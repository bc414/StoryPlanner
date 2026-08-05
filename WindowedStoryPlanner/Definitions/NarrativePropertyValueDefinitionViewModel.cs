using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
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
        /// Display colour, "#RRGGBB". Edited as hex text, following the Story colour column — WPF
        /// has no built-in picker and this layer has never added one.
        /// </summary>
        public string ColorHex
        {
            get => _model.ColorHex;
            set
            {
                if (SetProperty(_model.ColorHex, value, _model, (m, v) => m.ColorHex = v))
                    OnPropertyChanged(nameof(ColorBrush));
            }
        }

        /// <summary>Swatch fill for the grid. Falls back to the same neutral the Story and Subject
        /// rows use, so an unset colour looks unfinished rather than broken.</summary>
        public Brush ColorBrush => ChipInk.FillBrush(_model.ColorHex);
    }
}
