using System.Collections.Generic;
using System.Linq;

namespace WindowedStoryPlanner;

/// <summary>
/// Subjects holding identical values on every property of a board.
///
/// <para>There is deliberately NO tuple row on the group itself. Every member card already renders
/// the same five chips — that is what makes them a group — so a header repeating them would say
/// the same thing a seventh time. The header instead carries what the cards cannot: how many
/// subjects share the position, and which, so a collapsed group is still identifiable.</para>
///
/// Counting members is not scoring them: a group of six is not thereby more important than a group
/// of two, and nothing here explains why a group exists or suggests what might join it.
/// </summary>
public sealed class MatchGroupViewModel
{
    public required IReadOnlyList<SubjectCardViewModel> Cards { get; init; }

    public int Count => Cards.Count;

    public string CountLabel => Count == 1 ? "1 subject" : $"{Count} subjects";

    /// <summary>Members in one line for the collapsed header; the full list is the tooltip, since
    /// six civilizational-system names do not fit on a row.</summary>
    public string MemberNames => string.Join(" · ", Cards.Select(c => c.Name));
}
