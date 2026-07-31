using StoryPlanner.Core;
using Xunit;

using StoryPlanner.Mcp;

namespace StoryPlanner.Tests;

/// <summary>
/// Cache invalidation and graph traversal. The freshness test is the one most likely to
/// regress silently: in WAL mode the .storyplan file's mtime does NOT advance on write,
/// so any mtime-based check would appear to work and never fire.
/// </summary>
public class FreshnessAndGraphTests
{
    [Fact]
    public void An_external_commit_is_visible_to_the_next_call()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        const string marker = "ZZEXTERNALEDIT";

        // Search output echoes the caller's pattern in its header, so assert on the
        // match count rather than the raw string.
        Assert.Contains("(no matches)", tools.SearchPlan(marker));

        // A separate connection commits, exactly as the WPF app does on SaveAsync().
        plan.ExternalWrite(ctx =>
        {
            var note = ctx.Notes.First(n => n.Id == SyntheticPlan.VisibleNoteId);
            note.Content += " " + marker;
        });

        var after = tools.SearchPlan(marker);
        Assert.Contains(marker, after);
        Assert.Contains($"note:{SyntheticPlan.VisibleNoteId}", after);
    }

    [Fact]
    public void A_new_row_committed_externally_appears_without_restart()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);
        var reference = new ReferenceTools(plan.Sources);

        Assert.Contains("subjects: 2", reference.GetStats("working"));

        plan.ExternalWrite(ctx => ctx.Subjects.Add(new Subject
        {
            Id = 99, Name = "Latecomer", SubjectDefinitionId = SyntheticPlan.CharacterDefId
        }));

        Assert.Contains("subjects: 3", reference.GetStats("working"));
    }

    [Fact]
    public void The_main_file_mtime_does_not_track_writes_so_it_cannot_be_the_signal()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);
        tools.SearchPlan("anything"); // force a load

        var before = File.GetLastWriteTimeUtc(plan.Path);
        plan.ExternalWrite(ctx =>
        {
            var note = ctx.Notes.First(n => n.Id == SyntheticPlan.VisibleNoteId);
            note.Content += " ZZMTIMEPROBE";
        });
        var after = File.GetLastWriteTimeUtc(plan.Path);

        // Documents WHY invalidation uses PRAGMA data_version. If this assertion ever starts
        // failing it means journal mode changed — revisit StoryPlanSources.EnsureFresh.
        Assert.Equal(before, after);

        // ...and the change is still picked up, because data_version did move.
        Assert.Contains("ZZMTIMEPROBE", tools.SearchPlan("ZZMTIMEPROBE"));
    }

    // ── Graph traversal: edges are embedded in fetches ───────────────────────

    [Fact]
    public void Subject_fetch_lists_its_scene_links_as_followable_ids()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetSubjectsPlan([SyntheticPlan.SubjectId], includeNotes: false);

        Assert.Contains($"link:{SyntheticPlan.LinkId}", result);
        Assert.Contains("Testscene", result);
    }

    [Fact]
    public void Plot_point_fetch_lists_chapter_and_linked_subjects()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetPlotPointsPlan([SyntheticPlan.PlotPointId], includeNotes: false);

        Assert.Contains($"chapter:{SyntheticPlan.ChapterId}", result);
        Assert.Contains($"subject:{SyntheticPlan.SubjectId}", result);
        Assert.Contains($"link:{SyntheticPlan.LinkId}", result);
    }

    [Fact]
    public void Chapter_fetch_with_empty_ids_returns_the_inventory()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetChaptersPlan([]);

        Assert.Contains("inventory", result);
        Assert.Contains("Testchapter", result);
        Assert.Contains("1 plot points", result);
    }

    [Fact]
    public void A_subject_with_no_links_says_so_rather_than_omitting_the_section()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.GetSubjectsPlan([SyntheticPlan.EmptySubjectId], includeNotes: false);

        // Absence is information — 221 of 263 real subjects have no links.
        Assert.Contains("scenes: none", result);
    }

    [Fact]
    public void IncludeNotes_false_omits_content_but_keeps_edges_and_tallies()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var lean = tools.GetSubjectsPlan([SyntheticPlan.SubjectId], includeNotes: false);
        var full = tools.GetSubjectsPlan([SyntheticPlan.SubjectId], includeNotes: true);

        Assert.DoesNotContain(SyntheticPlan.VisibleSecret, lean);
        Assert.Contains(SyntheticPlan.VisibleSecret, full);
        Assert.Contains("notes:", lean);   // tallies survive
        Assert.Contains($"link:{SyntheticPlan.LinkId}", lean); // edges survive
    }

    // ── Search mechanics ────────────────────────────────────────────────────

    [Fact]
    public void Regex_alternation_works_because_the_vocabulary_is_the_callers_job()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.SearchPlan($"{SyntheticPlan.VisibleSecret}|Testscene|nonexistentterm");

        Assert.Contains($"note:{SyntheticPlan.VisibleNoteId}", result);
        Assert.Contains($"plotpoint:{SyntheticPlan.PlotPointId}", result);
    }

    [Fact]
    public void An_invalid_regex_reports_itself_instead_of_throwing()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        var result = tools.SearchPlan("([unclosed");

        Assert.Contains("Invalid regex", result);
    }

    [Fact]
    public void Search_is_case_insensitive_by_default_and_case_sensitive_on_request()
    {
        using var plan = SyntheticPlan.Create();
        var tools = new PlanTools(plan.Sources);

        Assert.Contains("note:", tools.SearchPlan("zzvisible"));
        Assert.Contains("(no matches)", tools.SearchPlan("zzvisible", caseSensitive: true));
    }
}
