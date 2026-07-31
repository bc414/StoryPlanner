using System;
using System.Collections.Generic;
using System.Text;

namespace StoryPlanner.Core
{
    /// <summary>
    /// Specifies a valid value for a particular property. Ordering within a property comes from
    /// row order (Id); for an axis-shaped property, seed the poles in order so the picker reads
    /// as a spectrum.
    ///
    /// There is deliberately no "unset"/"none" row: absence of a NarrativePropertyValue is how
    /// unset is represented. A sentinel row's Id would be written into
    /// NarrativePropertyValue.ValueDefinitionId and read back as a real answer.
    /// </summary>
    public class NarrativePropertyValueDefinition
    {
        public int Id { get; set; }
        public int NarrativePropertyDefinitionId { get; set; }
        public string ValueName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
