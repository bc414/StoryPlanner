using System.Text.RegularExpressions;
using StoryPlanner.Mcp;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Guards the id-demotion pass: ids were moved from leading every line to a trailing
/// parenthetical (name-led output instead), specifically so the two-pass search-then-fetch
/// loop keeps working — an id must still be a single, unambiguous, greppable token on its
/// line. These tests fail loudly if a future formatting change quietly drops the id instead
/// of just repositioning it.
/// </summary>
public class IdFormatTests
{
    private static readonly Regex IdToken = new(@"\(\w+:\d+[,)]");

    [Fact]
    public void Every_top_level_search_hit_line_carries_a_parseable_id()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        // Matches subject/plotpoint/chapter names via "Test" and a note's content via
        // VisibleSecret — broad enough to exercise several hit categories at once.
        var result = tools.SearchPlan($"Test|{SyntheticPlan.VisibleSecret}");

        var topLevelLines = result
            .Split('\n')
            .Select(l => l.TrimEnd('\r'))
            .Where(l => l.Length > 0 && !l.StartsWith('#') && !l.StartsWith(' '));

        var checkedAny = false;
        foreach (var line in topLevelLines)
        {
            checkedAny = true;
            Assert.Matches(IdToken, line);
        }
        Assert.True(checkedAny, "Expected at least one hit line to check — the search pattern matched nothing.");
    }

    [Fact]
    public void Every_fetched_note_header_carries_a_parseable_id()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetNotesPlan([
            SyntheticPlan.VisibleNoteId, SyntheticPlan.PlotPointNoteId,
            SyntheticPlan.ChapterNoteId, SyntheticPlan.LinkNoteId
        ]);

        var headerLines = result
            .Split('\n')
            .Select(l => l.TrimEnd('\r'))
            .Where(l => l.StartsWith("## "));

        var checkedAny = false;
        foreach (var line in headerLines)
        {
            checkedAny = true;
            Assert.Matches(IdToken, line);
        }
        Assert.True(checkedAny, "Expected at least one note header to check.");
    }
}
