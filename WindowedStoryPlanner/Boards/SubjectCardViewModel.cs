using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// One subject as it appears on a board: its name, plus one chip per board property in
/// DisplayOrder. The item behind <c>SubjectCardView</c>, which the grids and the tree both use.
///
/// <para><b>The card does not know where it is.</b> It renders the same five chips in a grid cell
/// as in a tree node, even though two of a cell's chips restate its coordinates and a tree node's
/// differ from its parent's in a readable way. There is no comparison mode and no
/// position-awareness: one rendering, so a pattern learned in one view reads identically in the
/// other. Drift along a chain is read from the chip COLOURS changing, which is what the colour
/// field exists for.</para>
///
/// Chip order is the property order, so a slot always means the same property — which is why
/// colour lives on the value and not on the property.
/// </summary>
public partial class SubjectCardViewModel : ObservableObject
{
    private readonly System.Action<SubjectViewModel>? _open;

    public SubjectCardViewModel(
        SubjectViewModel subject,
        IReadOnlyList<ValueChip> chips,
        System.Action<SubjectViewModel>? open = null)
    {
        Subject = subject;
        Chips = new ObservableCollection<ValueChip>(chips);
        _open = open;
    }

    public SubjectViewModel Subject { get; }
    public int SubjectId => Subject.Id;
    public string Name => Subject.Name;
    public ObservableCollection<ValueChip> Chips { get; }

    /// <summary>Opens the subject's editor. Supplied by whichever view built the card, rather than
    /// reached for through a RelativeSource walk — the card is hosted inside three different
    /// ItemsControl nestings and the walk would have to differ per host.</summary>
    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private void Open() => _open?.Invoke(Subject);

    /// <summary>
    /// Builds a card for one subject against one board's ordered properties.
    /// <paramref name="valueDefsByProperty"/> and <paramref name="heldValueIds"/> are prepared once
    /// per rebuild by the caller — one card is shared across every grid and every match group, so
    /// this runs once per subject rather than once per placement.
    /// </summary>
    public static SubjectCardViewModel Build(
        SubjectViewModel subject,
        IReadOnlyList<NarrativePropertyDefinition> boardProperties,
        IReadOnlyDictionary<int, List<NarrativePropertyValueDefinition>> valueDefsByProperty,
        IReadOnlySet<int> heldValueIds,
        System.Action<SubjectViewModel>? open = null)
    {
        var chips = new List<ValueChip>(boardProperties.Count);

        foreach (var property in boardProperties)
        {
            var values = valueDefsByProperty.TryGetValue(property.Id, out var v)
                ? v
                : new List<NarrativePropertyValueDefinition>();

            var held = values.FirstOrDefault(vd => heldValueIds.Contains(vd.Id));

            chips.Add(held is null
                ? new ValueChip(property.Name, string.Empty, string.Empty, IsUnset: true)
                : new ValueChip(property.Name, held.ValueName, held.ColorHex, IsUnset: false));
        }

        return new SubjectCardViewModel(subject, chips, open);
    }
}
