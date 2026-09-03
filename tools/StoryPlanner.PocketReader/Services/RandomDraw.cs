using StoryPlanner.Core;

namespace StoryPlanner.PocketReader;

public enum RandomUnit { Note, Subject, PlotPoint, Link }

public enum CorpusScope { Plan, Archive, Both }

/// <summary>One drawable thing: which file, what kind, which row.</summary>
public readonly record struct ItemRef(Corpus Corpus, RandomUnit Kind, int Id);

/// <summary>
/// Uniform random selection over the authored rows of the selected corpora. Every row of the
/// chosen unit is equally likely; the corpus toggle is the only control. The one departure
/// from pure uniformity is the recent ring: an item drawn in the last few taps is skipped so
/// "next" does not repeat, which is not-repeat, never ranking. An empty pool is reported as
/// empty (null), never quietly widened to another corpus or unit.
/// </summary>
public static class RandomDraw
{
    public const int RecentRingSize = 20;

    public static IReadOnlyList<ItemRef> Pool(PlanCache? working, PlanCache? archive, CorpusScope scope, RandomUnit unit)
    {
        var pool = new List<ItemRef>();
        if (scope is CorpusScope.Plan or CorpusScope.Both && working is not null) Add(pool, working, unit);
        if (scope is CorpusScope.Archive or CorpusScope.Both && archive is not null) Add(pool, archive, unit);
        return pool;
    }

    public static ItemRef? Draw(IReadOnlyList<ItemRef> pool, IReadOnlyCollection<ItemRef> recent, Random rng)
    {
        if (pool.Count == 0) return null;

        // Skip the recent ring only while doing so leaves something to draw from; a pool no
        // larger than the ring would otherwise be un-drawable.
        var candidates = pool.Count > recent.Count
            ? pool.Where(p => !recent.Contains(p)).ToList()
            : pool.ToList();
        if (candidates.Count == 0) candidates = pool.ToList();

        return candidates[rng.Next(candidates.Count)];
    }

    private static void Add(List<ItemRef> pool, PlanCache c, RandomUnit unit)
    {
        switch (unit)
        {
            case RandomUnit.Note:
                pool.AddRange(c.Notes.Select(n => new ItemRef(c.Corpus, unit, n.Id)));
                break;
            case RandomUnit.Subject:
                pool.AddRange(c.Subjects.Select(s => new ItemRef(c.Corpus, unit, s.Id)));
                break;
            case RandomUnit.PlotPoint:
                pool.AddRange(c.PlotPoints.Select(p => new ItemRef(c.Corpus, unit, p.Id)));
                break;
            case RandomUnit.Link:
                pool.AddRange(c.Links.Select(l => new ItemRef(c.Corpus, unit, l.Id)));
                break;
        }
    }
}
