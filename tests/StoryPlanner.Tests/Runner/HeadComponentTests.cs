using Bunit;
using Microsoft.AspNetCore.Components.Web;
using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The head's leaf components rendered against a <see cref="RunSnapshot"/>: parameter-driven,
/// no injected services, so they render with nothing registered (the Canalave convention).
/// Pins what the page shows and which controls exist for a live versus a finished run.
/// Tier: RazorComponents (bUnit).
/// </summary>
public class HeadComponentTests : BunitContext
{
    private static JobSnapshot Job(string id, string state, double cost = 0.2) =>
        new(id, "item " + id, "sonnet", state, state == "Pending" ? 0 : 1, state == "Failed" ? 1 : 0, state == "Failed" ? "missing: x" : "ok", cost, null, null, 42, null);

    private static RunSnapshot Run(bool live, bool paused = false, params JobSnapshot[] jobs) =>
        new("work/run-1", "C:/x", "work", live, !live, paused, false, 2, jobs.Count(j => j.State == "Running"), null, jobs,
            jobs.Count(j => j.State == "Pending"), jobs.Count(j => j.State == "Succeeded"), jobs.Count(j => j.State == "Failed"),
            jobs.Sum(j => j.CostUsd), true, null, null, new RunStages(true, null, true, true, false, !live, false, false), null);

    [Fact]
    public async Task JobTable_shows_state_check_and_cost_and_reports_the_selected_row()
    {
        string? selected = null;
        var cut = Render<JobTable>(p => p
            .Add(c => c.Jobs, [Job("job-01", "Succeeded"), Job("job-02", "Failed"), Job("job-03", "Pending", 0)])
            .Add(c => c.SelectedId, "job-02")
            .Add(c => c.OnSelect, id => selected = id));

        var rows = cut.FindAll("tbody tr");
        Assert.Equal(3, rows.Count);
        Assert.Contains("selected", rows[1].ClassList);
        Assert.Equal("Failed", rows[1].QuerySelector(".state")!.TextContent);
        Assert.Contains("missing: x", rows[1].TextContent);
        Assert.Contains("$0.200", rows[0].TextContent);
        Assert.DoesNotContain("$", rows[2].TextContent);        // a pending job has no cost yet

        await rows[2].ClickAsync(new MouseEventArgs());
        Assert.Equal("job-03", selected);
    }

    [Fact]
    public async Task HarnessControls_offer_pause_stop_and_cancel_only_for_a_live_run_and_cancel_only_for_a_running_selection()
    {
        var finished = Render<HarnessControls>(p => p.Add(c => c.Run, Run(live: false, jobs: Job("job-01", "Succeeded"))));
        Assert.Empty(finished.FindAll("button"));
        Assert.Contains("finished", finished.Markup);

        var actions = new List<string>();
        var live = Render<HarnessControls>(p => p
            .Add(c => c.Run, Run(live: true, jobs: [Job("job-01", "Running"), Job("job-02", "Pending", 0)]))
            .Add(c => c.SelectedJobId, "job-02")
            .Add(c => c.OnAction, a => actions.Add(a)));
        var buttons = live.FindAll("button");
        Assert.Equal(["pause", "stop after in-flight", "cancel selected"], buttons.Select(b => b.TextContent));
        Assert.True(buttons[2].HasAttribute("disabled"));      // the selected job is pending, not running
        await buttons[0].ClickAsync(new MouseEventArgs());
        Assert.Equal(["pause"], actions);

        var paused = Render<HarnessControls>(p => p.Add(c => c.Run, Run(live: true, paused: true, jobs: Job("job-01", "Running"))).Add(c => c.SelectedJobId, "job-01"));
        Assert.Equal("resume", paused.FindAll("button")[0].TextContent);
        Assert.False(paused.FindAll("button")[2].HasAttribute("disabled"));
    }

    [Fact]
    public async Task HarnessControls_offer_only_unschedule_for_a_scheduled_run()
    {
        var actions = new List<string>();
        var scheduled = Run(live: true, jobs: Job("job-01", "Pending", 0)) with { Scheduled = true, NotBeforeUtc = "2026-09-04T09:00:00+00:00" };
        var cut = Render<HarnessControls>(p => p.Add(c => c.Run, scheduled).Add(c => c.OnAction, a => actions.Add(a)));

        var button = Assert.Single(cut.FindAll("button"));
        Assert.Equal("unschedule", button.TextContent);
        Assert.Contains("scheduled for", cut.Markup);
        await button.ClickAsync(new MouseEventArgs());
        Assert.Equal(["unschedule"], actions);
    }

    [Fact]
    public void HostBar_shows_both_usage_windows_their_resets_the_cache_age_and_marks_a_stale_or_locked_cache()
    {
        var now = DateTimeOffset.UtcNow;
        var fresh = new Utilization(20, now.AddHours(3), now.AddMinutes(-5), SevenDayPercent: 42, SevenDayResetsAt: now.AddDays(3));
        var cut = Render<HostBar>(p => p.Add(c => c.InFlight, 2).Add(c => c.MaxParallel, 4).Add(c => c.UtilizationCap, 80).Add(c => c.Utilization, fresh));

        var meters = cut.FindAll(".meter");
        Assert.Equal(2, meters.Count);
        Assert.Contains("20%", meters[0].TextContent);
        Assert.Contains("in 2 h 5", meters[0].TextContent);          // 2 h 59 min, rounded down by the minute
        Assert.Contains("42%", meters[1].TextContent);
        Assert.Contains("in 2 d 23 h", meters[1].TextContent);
        Assert.Contains("5 min ago", cut.Find(".cachemeta").TextContent);
        Assert.DoesNotContain("stale", cut.Markup);
        Assert.Equal("width:20%", cut.Find(".meter .fill").GetAttribute("style"));
        Assert.Equal("left:80%", cut.Find(".capmark").GetAttribute("style"));

        var stale = new Utilization(85, now.AddMinutes(-10), now.AddHours(-9), LockedReason: "over_limit");
        var cut2 = Render<HostBar>(p => p.Add(c => c.UtilizationCap, 80).Add(c => c.Utilization, stale));
        Assert.Contains("stale", cut2.Find(".usage").ClassList);
        Assert.Contains("reset passed", cut2.Markup);
        Assert.Contains("locked: over_limit", cut2.Markup);
        Assert.Contains("over", cut2.Find(".meter .fill").ClassList);   // 85 ≥ cap 80

        var none = Render<HostBar>(p => p.Add(c => c.Utilization, (Utilization?)null));
        Assert.Contains("utilization unknown", none.Markup);
    }

    [Fact]
    public void StageStrip_lights_detected_stages_and_greys_calibration_for_a_protocol()
    {
        var cut = Render<StageStrip>(p => p.Add(c => c.S, new RunStages(true, null, true, false, true, false, false, true)));
        var on = cut.FindAll(".stage.on").Select(e => e.TextContent).ToList();
        Assert.Equal(["instrument", "enumerated", "piloted", "run.md"], on);
        Assert.Contains("na", cut.Find(".stage:nth-child(2)").ClassList);
    }

    [Fact]
    public void StreamPane_renders_parsed_text_and_says_when_there_is_nothing()
    {
        var empty = Render<StreamPane>(p => p.Add(c => c.Events, []));
        Assert.Contains("no stream yet", empty.Markup);

        var cut = Render<StreamPane>(p => p
            .Add(c => c.Title, "job-01 — attempt 1 (live)")
            .Add(c => c.Events, [new StreamEvent("tool", "Write arm-A.md (12 chars)", "{raw}"), new StreamEvent("done", "done — $0.210", "{raw2}")]));
        var events = cut.FindAll(".ev");
        Assert.Equal(2, events.Count);
        Assert.Contains("Write arm-A.md", events[0].TextContent);
        Assert.DoesNotContain("{raw}", cut.Markup);
        Assert.Contains("(live)", cut.Markup);
    }
}
