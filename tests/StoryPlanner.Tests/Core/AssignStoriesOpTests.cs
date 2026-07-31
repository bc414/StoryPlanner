using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.DataOps;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Runs DataOpEnvelope + AssignStories against a dedicated flat five-chapter file — NOT the
/// shared SyntheticPlan baseline, which already assigns its second chapter to a story and would
/// make chapter ranges here degenerate. Chapters 1-3 -> Story A, chapters 4-5 -> Story B,
/// mirroring the live TLTT v2 shape (a flat OrderIndex run split into story ranges) at a
/// tractable scale.
/// </summary>
public class AssignStoriesOpTests
{
    private const string TwoStoryConfig = """
        { "stories": [
            { "orderIndex": 1, "title": "Story A", "abbreviation": "A", "colorHex": "#111111", "from": 1, "to": 3 },
            { "orderIndex": 2, "title": "Story B", "abbreviation": "B", "colorHex": "#222222", "from": 4, "to": 5 }
        ] }
        """;

    [Fact]
    public async Task Apply_creates_stories_and_renumbers_chapters_preserving_relative_order()
    {
        var (path, dir) = await BuildFlatFiveChapterFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(new AssignStories(), path, ParseConfig(TwoStoryConfig), apply: true);
            Assert.Equal(0, exitCode);

            using var verify = OpenContext(path);
            var stories = await verify.Stories.OrderBy(s => s.OrderIndex).ToListAsync();
            Assert.Equal(2, stories.Count);
            Assert.Equal("Story A", stories[0].Title);
            Assert.Equal("Story B", stories[1].Title);

            var chapters = await verify.Chapters.ToListAsync();

            var inA = chapters.Where(c => c.StoryId == stories[0].Id).OrderBy(c => c.OrderIndex).ToList();
            Assert.Equal(3, inA.Count);
            Assert.Equal([1, 2, 3], inA.Select(c => c.OrderIndex));
            Assert.Equal(["Ch1", "Ch2", "Ch3"], inA.Select(c => c.Title)); // relative order preserved

            var inB = chapters.Where(c => c.StoryId == stories[1].Id).OrderBy(c => c.OrderIndex).ToList();
            Assert.Equal(2, inB.Count);
            Assert.Equal([1, 2], inB.Select(c => c.OrderIndex));
            Assert.Equal(["Ch4", "Ch5"], inB.Select(c => c.Title));

            // Titles are byte-identical to the pre-op values — only StoryId/OrderIndex moved.
            Assert.All(chapters, c => Assert.StartsWith("Ch", c.Title));
        }
        finally
        {
            TryDelete(dir);
        }
    }

    [Fact]
    public async Task A_second_apply_is_a_no_op()
    {
        var (path, dir) = await BuildFlatFiveChapterFile();
        try
        {
            var first = await DataOpEnvelope.RunAsync(new AssignStories(), path, ParseConfig(TwoStoryConfig), apply: true);
            Assert.Equal(0, first);

            var second = await DataOpEnvelope.RunAsync(new AssignStories(), path, ParseConfig(TwoStoryConfig), apply: true);
            Assert.Equal(0, second);

            using var verify = OpenContext(path);
            Assert.Equal(2, await verify.Stories.CountAsync()); // not 4 — no duplicate stories
            Assert.Equal(5, await verify.Chapters.CountAsync());
        }
        finally
        {
            TryDelete(dir);
        }
    }

    [Fact]
    public async Task Dry_run_reports_zero_violations_but_persists_nothing()
    {
        var (path, dir) = await BuildFlatFiveChapterFile();
        try
        {
            var exitCode = await DataOpEnvelope.RunAsync(new AssignStories(), path, ParseConfig(TwoStoryConfig), apply: false);
            Assert.Equal(0, exitCode);

            using var verify = OpenContext(path);
            Assert.Empty(await verify.Stories.ToListAsync());
            Assert.All(await verify.Chapters.ToListAsync(), c => Assert.Equal(0, c.StoryId));
        }
        finally
        {
            TryDelete(dir);
        }
    }

    private static JsonElement ParseConfig(string json) => JsonDocument.Parse(json).RootElement;

    private static AppDbContext OpenContext(string path) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={path}").Options);

    private static void TryDelete(string dir)
    {
        try { Directory.Delete(dir, recursive: true); } catch (IOException) { }
    }

    private static async Task<(string Path, string Dir)> BuildFlatFiveChapterFile()
    {
        var dir = Directory.CreateTempSubdirectory("assign-stories-tests-");
        var file = Path.Combine(dir.FullName, "flat.storyplan");

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={file}").Options;
        using var ctx = new AppDbContext(options);
        await ctx.Database.MigrateAsync();
        ctx.Chapters.AddRange(
            new Chapter { Id = 1, Title = "Ch1", OrderIndex = 1 },
            new Chapter { Id = 2, Title = "Ch2", OrderIndex = 2 },
            new Chapter { Id = 3, Title = "Ch3", OrderIndex = 3 },
            new Chapter { Id = 4, Title = "Ch4", OrderIndex = 4 },
            new Chapter { Id = 5, Title = "Ch5", OrderIndex = 5 });
        await ctx.SaveChangesAsync();

        return (file, dir.FullName);
    }
}
