using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryPlanner.Core;

/// <summary>
/// Greedy interval packing for one lane pool: sort by start (longest-first on ties), assign
/// each item to the lowest-numbered lane whose previous occupant has ended. On intervals this
/// greedy is provably optimal — lane count equals maximum concurrency — with zero distortion
/// of the time axis.
///
/// Operates on PIXEL intervals, not years, and must be re-run per zoom: event markers have
/// fixed pixel height, so "doesn't overlap in time" no longer implies "doesn't overlap on
/// screen". This is also why conditions and events pack in two separate pools per theater —
/// at low zoom a marker's fixed height would otherwise block a lane across years of bar space.
/// </summary>
public static class LanePacker
{
    /// <summary>
    /// Returns the lane index for each input item, parallel to the input order, plus the total
    /// lane count. <paramref name="tops"/>/<paramref name="bottoms"/> are pixel edges
    /// (bottom > top). Touching edges (next.top == prev.bottom) do NOT conflict.
    /// </summary>
    public static (int[] Lanes, int LaneCount) Pack(IReadOnlyList<double> tops, IReadOnlyList<double> bottoms)
    {
        if (tops.Count != bottoms.Count)
            throw new ArgumentException("tops and bottoms must be parallel.");

        var order = Enumerable.Range(0, tops.Count)
            .OrderBy(i => tops[i])
            .ThenByDescending(i => bottoms[i] - tops[i]) // longest-first on tie
            .ThenBy(i => i)                              // deterministic
            .ToList();

        var lanes = new int[tops.Count];
        var laneBottoms = new List<double>(); // per lane: bottom edge of its last occupant

        foreach (var i in order)
        {
            var placed = false;
            for (var lane = 0; lane < laneBottoms.Count; lane++)
            {
                if (laneBottoms[lane] <= tops[i])
                {
                    lanes[i] = lane;
                    laneBottoms[lane] = bottoms[i];
                    placed = true;
                    break;
                }
            }
            if (!placed)
            {
                lanes[i] = laneBottoms.Count;
                laneBottoms.Add(bottoms[i]);
            }
        }

        return (lanes, laneBottoms.Count);
    }
}
