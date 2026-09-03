using StoryPlanner.Core;
using StoryPlanner.Mcp;
using StoryPlanner.PocketReader;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Pure tests for the random view's pool and draw. The pool is the union of the selected
/// corpora's rows for the chosen unit — never widened when a corpus is missing — and the draw
/// is uniform apart from the no-repeat ring, which must never make a small pool un-drawable.
/// </summary>
public class RandomDrawTests
{
    [Fact]
    public void Pool_is_the_union_of_selected_corpora()
    {
        using var plan = SyntheticPlan.Create();
        var working = plan.Sources.Get(Corpus.Working);
        var archive = plan.Sources.Get(Corpus.Archive);

        var both = RandomDraw.Pool(working, archive, CorpusScope.Both, RandomUnit.Note);
        var planOnly = RandomDraw.Pool(working, archive, CorpusScope.Plan, RandomUnit.Note);
        var archiveOnly = RandomDraw.Pool(working, archive, CorpusScope.Archive, RandomUnit.Note);

        Assert.Equal(working.Notes.Count, planOnly.Count);
        Assert.Equal(archive.Notes.Count, archiveOnly.Count);
        Assert.Equal(planOnly.Count + archiveOnly.Count, both.Count);
        Assert.All(planOnly, r => Assert.Equal(Corpus.Working, r.Corpus));
        Assert.All(archiveOnly, r => Assert.Equal(Corpus.Archive, r.Corpus));
    }

    [Fact]
    public void Missing_corpus_yields_empty_never_the_other_file()
    {
        using var plan = SyntheticPlan.Create();
        var working = plan.Sources.Get(Corpus.Working);

        Assert.Empty(RandomDraw.Pool(working, archive: null, CorpusScope.Archive, RandomUnit.Note));
        Assert.Null(RandomDraw.Draw(Array.Empty<ItemRef>(), Array.Empty<ItemRef>(), new Random(1)));
    }

    [Fact]
    public void Unit_selects_the_row_type()
    {
        using var plan = SyntheticPlan.Create();
        var working = plan.Sources.Get(Corpus.Working);

        Assert.Equal(working.Subjects.Count, RandomDraw.Pool(working, null, CorpusScope.Plan, RandomUnit.Subject).Count);
        Assert.Equal(working.PlotPoints.Count, RandomDraw.Pool(working, null, CorpusScope.Plan, RandomUnit.PlotPoint).Count);
        Assert.Equal(working.Links.Count, RandomDraw.Pool(working, null, CorpusScope.Plan, RandomUnit.Link).Count);
        Assert.All(RandomDraw.Pool(working, null, CorpusScope.Plan, RandomUnit.Link), r => Assert.Equal(RandomUnit.Link, r.Kind));
    }

    [Fact]
    public void Recent_ring_skips_recent_items_but_never_empties_the_pool()
    {
        var pool = Enumerable.Range(1, 3).Select(i => new ItemRef(Corpus.Working, RandomUnit.Note, i)).ToList();
        var rng = new Random(7);

        // Two of three recent: the third is the only candidate.
        var recent = new[] { pool[0], pool[1] };
        for (var i = 0; i < 20; i++)
            Assert.Equal(pool[2], RandomDraw.Draw(pool, recent, rng));

        // Every item recent (pool <= ring): still draws something rather than nothing.
        Assert.NotNull(RandomDraw.Draw(pool, pool, rng));
    }

    [Fact]
    public void Draw_reaches_every_item()
    {
        var pool = Enumerable.Range(1, 5).Select(i => new ItemRef(Corpus.Archive, RandomUnit.Subject, i)).ToList();
        var seen = new HashSet<ItemRef>();
        var rng = new Random(11);
        for (var i = 0; i < 500; i++)
            seen.Add(RandomDraw.Draw(pool, Array.Empty<ItemRef>(), rng)!.Value);
        Assert.Equal(5, seen.Count);
    }
}
