using System.Collections.Generic;
using System.Linq;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

/// <summary>
/// An era is the interval between two consecutive pivots — derived, never stored. StartYear is
/// null for the era before the first pivot, EndYear null for the era after the last (open
/// edges of the known timeline). The label is purely positional ("before 870", "870–930");
/// eras have no identity of their own to name.
/// </summary>
public readonly record struct Era(int? StartYear, int? EndYear)
{
    public string Label => (StartYear, EndYear) switch
    {
        (null, int e) => $"before {e}",
        (int s, null) => $"{s} onward",
        (int s, int e) => $"{s}–{e}",
        _ => "all of time" // no pivots at all — one era covering everything
    };

    public bool Contains(double fractionalYear) =>
        (StartYear is null || fractionalYear >= StartYear.Value) &&
        (EndYear is null || fractionalYear < EndYear.Value);
}

public static class Eras
{
    /// <summary>N distinct pivot years → N+1 eras, in chronological order. Duplicate pivot
    /// years collapse (a zero-width era is not a thing); zero pivots → the single all-of-time era.</summary>
    public static IReadOnlyList<Era> FromPivots(IEnumerable<Pivot> pivots)
    {
        var years = pivots.Select(p => p.Year).Distinct().OrderBy(y => y).ToList();
        var eras = new List<Era>(years.Count + 1);
        int? prev = null;
        foreach (var y in years)
        {
            eras.Add(new Era(prev, y));
            prev = y;
        }
        eras.Add(new Era(prev, null));
        return eras;
    }
}
