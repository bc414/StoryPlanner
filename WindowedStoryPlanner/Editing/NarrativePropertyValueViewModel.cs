using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// One allowed answer for a <see cref="NarrativePropertyDefinition"/> — a row in the picker.
    /// Read-only projection: the definition rows are edited in the Definitions tab, not here.
    /// </summary>
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

        /// <summary>
        /// The owning property. Needed because the value list is filtered by it — the previous
        /// version compared <see cref="Id"/> (this row's own PK) against the property definition's
        /// PK, which is a different table's id space and matched only by coincidence.
        /// </summary>
        public int NarrativePropertyDefinitionId => _narrativePropertyValueDefinition.NarrativePropertyDefinitionId;
    }
}
