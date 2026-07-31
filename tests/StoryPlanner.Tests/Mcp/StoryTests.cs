using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.Mcp;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The story layer's MCP surface: list_stories / get_stories, story-grouped chapter
/// inventories, and the "story" count_notes_plan dimension. Also covers the archive's
/// permanent steady state — zero Story rows, every chapter at StoryId 0 — which must render
/// as an honest "(Unassigned)" grouping, not an empty or missing section.
/// </summary>
public class StoryTests
{
    [Fact]
    public void List_stories_reports_title_abbreviation_and_chapter_counts()
    {
        using var plan = SyntheticPlan.Create();
        var reference = new ReferenceTools(plan.Sources);

        var result = reference.ListStories("working");

        Assert.Contains("Test Story", result);
        Assert.Contains("[TS]", result);
        Assert.Contains($"story:{SyntheticPlan.StoryId}", result);
        Assert.Contains("(Unassigned)", result); // the fixture's original chapter has no story
    }

    [Fact]
    public void Get_stories_with_empty_ids_matches_the_list_stories_inventory()
    {
        using var plan = SyntheticPlan.Create();
        var reference = new ReferenceTools(plan.Sources);

        var result = reference.GetStories([], "working");

        Assert.Contains("Test Story", result);
    }

    [Fact]
    public void Get_stories_by_id_lists_its_ordered_chapters()
    {
        using var plan = SyntheticPlan.Create();
        var reference = new ReferenceTools(plan.Sources);

        var result = reference.GetStories([SyntheticPlan.StoryId], "working");

        Assert.Contains("Story chapter", result);
        Assert.Contains($"chapter:{SyntheticPlan.SecondChapterId}", result);
        Assert.Contains("CH#1", result);
    }

    [Fact]
    public void Chapter_inventory_groups_under_story_headings()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetChaptersPlan([]);

        // Query.StoryLabel prefers the abbreviation when one is set (matches the compact
        // "TS CH#12"-style chapter labels used everywhere else) — the fixture's Test Story
        // has abbreviation "TS", so that's what the heading shows, not the full title.
        Assert.Contains("## TS", result);
        Assert.Contains("## (Unassigned)", result);
        Assert.Contains("Testchapter", result);
        Assert.Contains("Story chapter", result);
    }

    [Fact]
    public void Count_notes_by_story_resolves_owned_notes_and_reports_no_story_for_subjects()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.CountNotesPlan(["story"]);

        // Chapter/PlotPoint/Link-owned notes in the fixture all resolve through chapter:1,
        // which has StoryId 0 — "(Unassigned)". Subject-owned notes carry no chapter at all.
        Assert.Contains("(Unassigned)", result);
        Assert.Contains("(no story)", result);
    }

    [Fact]
    public async Task A_corpus_with_zero_stories_renders_an_unassigned_grouping_not_an_empty_section()
    {
        var dir = Directory.CreateTempSubdirectory("zero-story-tests-");
        var file = Path.Combine(dir.FullName, "zero.storyplan");
        try
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={file}").Options;
            using (var ctx = new AppDbContext(options))
            {
                await ctx.Database.MigrateAsync();
                ctx.Chapters.Add(new Chapter { Id = 1, Title = "Lonely Chapter", OrderIndex = 1 }); // StoryId defaults to 0
                await ctx.SaveChangesAsync();
            }

            using var sources = new StoryPlanSources(file, file);
            sources.LoadAll();

            var reference = new ReferenceTools(sources);
            var plan = new PlanTools(sources);

            var stories = reference.ListStories("working");
            Assert.Contains("— 0", stories); // zero real stories
            Assert.Contains("(Unassigned) — 1 chapters", stories);

            var chapters = plan.GetChaptersPlan([]);
            Assert.Contains("## (Unassigned)", chapters);
            Assert.Contains("Lonely Chapter", chapters);
        }
        finally
        {
            try { Directory.Delete(dir.FullName, recursive: true); } catch (IOException) { }
        }
    }
}
