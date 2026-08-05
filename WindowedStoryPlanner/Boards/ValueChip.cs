using System.Windows.Media;

namespace WindowedStoryPlanner;

/// <summary>
/// One property's answer, rendered as a filled pill. Shared by <see cref="SubjectCardViewModel"/>
/// (a subject's own values) and <see cref="MatchGroupViewModel"/> (the tuple a group is defined
/// by), so a group's header chips are literally the same control as its members' — which is what
/// lets them line up in the same columns.
///
/// <see cref="IsUnset"/> is a legal, long-lived state rendered as a neutral outlined chip, never
/// omitted: a missing chip would shift every later slot, and slot position is how a chip says
/// which property it answers.
/// </summary>
public sealed record ValueChip(string PropertyName, string ValueName, string ColorHex, bool IsUnset)
{
    public Brush Fill => IsUnset ? Brushes.Transparent : ChipInk.FillBrush(ColorHex);
    public Brush Ink => IsUnset ? Brushes.Gray : ChipInk.InkBrush(ColorHex);
    public string Label => IsUnset ? "—" : ValueName;
    public string Tooltip => IsUnset ? $"{PropertyName}: unset" : $"{PropertyName}: {ValueName}";
}
