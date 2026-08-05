using System.ComponentModel;

namespace WindowedStoryPlanner;

/// <summary>
/// A view model holding an authored "#RRGGBB". Implemented by StoryViewModel,
/// NarrativePropertyValueDefinitionViewModel and SubjectViewModel so one
/// <see cref="ColorPickerControl"/> serves all three colour fields.
///
/// <see cref="INotifyPropertyChanged"/> is part of the contract, not incidental: the picker's
/// collapsed face binds Target.ColorHex, and that binding only refreshes because the implementer
/// raises the change. All three already derive from ObservableObject, so this costs them nothing.
/// </summary>
public interface IColorHexOwner : INotifyPropertyChanged
{
    /// <summary>"#RRGGBB", or empty — which is a legal, long-lived "no colour", never missing data.</summary>
    string ColorHex { get; set; }
}
