using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.DataOps;

/// <summary>
/// Groups an existing flat <c>Chapter.OrderIndex</c> run into stories, per a config file Brian
/// has already fully specified — see <c>configs/stories.v2.json</c> and
/// <c>configs/stories.archive.json</c>. Ranges in the config are against the file's *current*
/// global OrderIndex at the time this op first runs; afterward, OrderIndex is per-story.
///
/// Idempotent: if every configured range already has a matching Story row with its chapters
/// correctly assigned and contiguously renumbered, <see cref="Apply"/> is a no-op. This matters
/// because after a successful run the config's global from/to values no longer describe the
/// file (OrderIndex is per-story by then) — re-interpreting them a second time would be wrong,
/// so a second run must detect "already done" up front rather than recompute ranges.
/// </summary>
public sealed class AssignStories : IDataOperation
{
    public string Name => "assign-stories";

    private readonly record struct StoryRange(
        int OrderIndex, string Title, string Abbreviation, string ColorHex, int From, int To);

    private Dictionary<string, long> _rowCountsBefore = new();

    public async Task Apply(AppDbContext ctx, JsonElement config)
    {
        _rowCountsBefore = PlanIntegrity.SnapshotRowCounts(ctx);

        var ranges = ParseRanges(config);
        var existingStories = await ctx.Stories.ToListAsync();

        if (AlreadyApplied(ranges, existingStories, await ctx.Chapters.ToListAsync()))
            return; // no-op: a prior successful run already produced this exact target state

        // Snapshot chapters' CURRENT OrderIndex before any mutation — this is what the
        // config's from/to values are interpreted against. On a fresh file that's the
        // original global 1..70 run; on a resumed partial run it's whatever is currently
        // on disk, which is the only state that can be meaningfully compared to from/to.
        var chapters = await ctx.Chapters.ToListAsync();
        var currentOrder = chapters.ToDictionary(c => c.Id, c => c.OrderIndex);

        foreach (var range in ranges.OrderBy(r => r.OrderIndex))
        {
            var story = existingStories.FirstOrDefault(s => s.OrderIndex == range.OrderIndex && s.Title == range.Title);
            if (story is null)
            {
                story = new Story
                {
                    Title = range.Title,
                    Abbreviation = range.Abbreviation,
                    ColorHex = range.ColorHex,
                    OrderIndex = range.OrderIndex
                };
                ctx.Stories.Add(story);
                existingStories.Add(story);
            }
            else
            {
                // Keep display metadata in sync if the config was edited between runs.
                story.Abbreviation = range.Abbreviation;
                story.ColorHex = range.ColorHex;
            }
        }

        await ctx.SaveChangesAsync(); // assigns Id to any newly-added Story

        foreach (var range in ranges.OrderBy(r => r.OrderIndex))
        {
            var story = existingStories.First(s => s.OrderIndex == range.OrderIndex && s.Title == range.Title);

            var chaptersInRange = chapters
                .Where(c => currentOrder[c.Id] >= range.From && currentOrder[c.Id] <= range.To)
                .OrderBy(c => currentOrder[c.Id])
                .ToList();

            var next = 1;
            foreach (var chapter in chaptersInRange)
            {
                chapter.StoryId = story.Id;
                chapter.OrderIndex = next++;
            }
        }
    }

    public IEnumerable<PlanIntegrity.Violation> ExtraChecks(AppDbContext ctx, JsonElement config)
    {
        var violations = new List<PlanIntegrity.Violation>();

        violations.AddRange(PlanIntegrity.CompareRowCounts(
            _rowCountsBefore,
            PlanIntegrity.SnapshotRowCounts(ctx),
            allowedToChange: new HashSet<string> { "Chapters", "Stories" }));

        var ranges = ParseRanges(config);
        var stories = ctx.Stories.ToList();
        var chapters = ctx.Chapters.ToList();

        var storyIdByOrderIndex = new Dictionary<int, int>();
        foreach (var range in ranges)
        {
            var story = stories.FirstOrDefault(s => s.OrderIndex == range.OrderIndex && s.Title == range.Title);
            if (story is null)
            {
                violations.Add(new PlanIntegrity.Violation(
                    "assignstories.story_missing", $"orderIndex {range.OrderIndex} ({range.Title})"));
                continue;
            }
            storyIdByOrderIndex[range.OrderIndex] = story.Id;
        }

        var configuredStoryIds = storyIdByOrderIndex.Values.ToHashSet();
        foreach (var chapter in chapters)
        {
            if (chapter.StoryId != 0 && !configuredStoryIds.Contains(chapter.StoryId))
                violations.Add(new PlanIntegrity.Violation(
                    "assignstories.unexpected_storyid", $"chapter:{chapter.Id} storyId={chapter.StoryId}"));
        }

        // (StoryId, OrderIndex) unique and contiguous 1..n within every configured story.
        // StoryId = 0 (unassigned) is exempt — the config may deliberately leave chapters there.
        foreach (var group in chapters.Where(c => c.StoryId != 0).GroupBy(c => c.StoryId))
        {
            var orders = group.Select(c => c.OrderIndex).OrderBy(o => o).ToList();
            if (!orders.SequenceEqual(Enumerable.Range(1, orders.Count)))
                violations.Add(new PlanIntegrity.Violation(
                    "assignstories.noncontiguous_order", $"story:{group.Key} orders=[{string.Join(",", orders)}]"));

            foreach (var dup in orders.GroupBy(o => o).Where(g => g.Count() > 1))
                violations.Add(new PlanIntegrity.Violation(
                    "assignstories.duplicate_order", $"story:{group.Key} orderIndex:{dup.Key}"));
        }

        return violations;
    }

    private static bool AlreadyApplied(
        IReadOnlyList<StoryRange> ranges, IReadOnlyList<Story> stories, IReadOnlyList<Chapter> chapters)
    {
        foreach (var range in ranges)
        {
            var story = stories.FirstOrDefault(s => s.OrderIndex == range.OrderIndex && s.Title == range.Title);
            if (story is null) return false;

            var expectedCount = range.To - range.From + 1;
            var assigned = chapters.Where(c => c.StoryId == story.Id).ToList();
            if (assigned.Count != expectedCount) return false;

            var orders = assigned.Select(c => c.OrderIndex).OrderBy(o => o).ToList();
            if (!orders.SequenceEqual(Enumerable.Range(1, expectedCount))) return false;
        }
        return true;
    }

    private static List<StoryRange> ParseRanges(JsonElement config) =>
        config.GetProperty("stories").EnumerateArray()
            .Select(e => new StoryRange(
                e.GetProperty("orderIndex").GetInt32(),
                e.GetProperty("title").GetString() ?? "",
                e.TryGetProperty("abbreviation", out var abbr) ? abbr.GetString() ?? "" : "",
                e.TryGetProperty("colorHex", out var color) ? color.GetString() ?? "" : "",
                e.GetProperty("from").GetInt32(),
                e.GetProperty("to").GetInt32()))
            .ToList();
}
