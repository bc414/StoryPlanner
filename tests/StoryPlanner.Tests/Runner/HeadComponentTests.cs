using Bunit;
using Microsoft.AspNetCore.Components.Web;
using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The head's leaf components rendered against a <see cref="BatchSnapshot"/>: parameter-driven,
/// no injected services, so they render with nothing registered (the Canalave convention).
/// Pins what the page shows and which controls exist for a live versus a finished batch.
/// Tier: RazorComponents (bUnit).
/// </summary>
public class HeadComponentTests : BunitContext
{
    private static ItemSnapshot Item(string id, string state, double cost = 0.2, bool callable = false) =>
        new(id, "note-" + id, state, state == "Pending" ? 0 : 1, state == "Failed" ? 1 : 0, state == "Failed" ? "no structured output" : "ok", cost, null, null, 42, null, callable);

    private static BatchSnapshot Batch(bool live, bool paused = false, params ItemSnapshot[] items) =>
        new("docs/x/batches/01-full", "C:/x", "verification-of-x-y", "01-full", "full", "sonnet", null, "directions-1", live, !live, paused, false,
            items.Count(i => i.State == "Running"), null, items,
            items.Count(i => i.State == "Pending"), items.Count(i => i.State == "Succeeded"), items.Count(i => i.State == "Failed"),
            items.Sum(i => i.CostUsd), null, new BatchStages(true, true, !live, false), null);

    [Fact]
    public async Task CallTable_shows_state_check_and_cost_and_reports_the_selected_row()
    {
        string? selected = null;
        var cut = Render<CallTable>(p => p
            .Add(c => c.Items, [Item("item-01", "Succeeded"), Item("item-02", "Failed"), Item("item-03", "Pending", 0)])
            .Add(c => c.SelectedItem, "item-02")
            .Add(c => c.OnSelect, id => selected = id));

        var rows = cut.FindAll("tbody tr");
        Assert.Equal(3, rows.Count);
        Assert.Contains("selected", rows[1].ClassList);
        Assert.Equal("Failed", rows[1].QuerySelector(".state")!.TextContent);
        Assert.Contains("no structured output", rows[1].TextContent);
        Assert.Contains("$0.200", rows[0].TextContent);
        Assert.DoesNotContain("$", rows[2].TextContent);        // a pending item has no cost yet
        Assert.Contains("note-item-01", rows[0].TextContent);

        await rows[2].ClickAsync(new MouseEventArgs());
        Assert.Equal("item-03", selected);
    }

    [Fact]
    public async Task BatchList_offers_execute_only_where_something_is_pending_and_nothing_is_running_or_scheduled()
    {
        BatchSnapshot? executed = null;
        var pending = Batch(live: false, items: [Item("item-01", "Succeeded"), Item("item-02", "Pending", 0)]);
        var failed = Batch(live: false, items: [Item("item-01", "Succeeded"), Item("item-02", "Failed")]);   // reattempted, so executable
        var done = Batch(live: false, items: Item("item-01", "Succeeded"));
        var running = Batch(live: true, items: [Item("item-01", "Running"), Item("item-02", "Pending", 0)]);
        var scheduled = Batch(live: true, items: Item("item-01", "Pending", 0)) with { Scheduled = true, NotBeforeUtc = "2026-09-16T04:00:00Z" };
        var broken = pending with { Error = "definition: no model" };
        var cut = Render<BatchList>(p => p
            .Add(c => c.Batches, [pending, failed, done, running, scheduled, broken])
            .Add(c => c.OnExecute, b => executed = b));

        var rows = cut.FindAll("tbody tr");
        Assert.Equal(6, rows.Count);
        var button = Assert.Single(rows[0].QuerySelectorAll("button"));
        Assert.Equal("execute", button.TextContent);
        Assert.Single(rows[1].QuerySelectorAll("button"));
        foreach (var r in rows.Skip(2)) Assert.Empty(r.QuerySelectorAll("button"));

        await button.ClickAsync(new MouseEventArgs());
        Assert.Same(pending, executed);
    }

    [Fact]
    public async Task CallTable_gives_each_callable_row_its_own_queue_jump_without_selecting_it()
    {
        string? selected = null, called = null;
        var cut = Render<CallTable>(p => p
            .Add(c => c.Items, [Item("item-01", "Succeeded"), Item("item-02", "Pending", 0, callable: true), Item("item-03", "Pending", 0)])
            .Add(c => c.OnSelect, id => selected = id)
            .Add(c => c.OnCall, id => called = id));

        var rows = cut.FindAll("tbody tr");
        Assert.Empty(rows[0].QuerySelectorAll("button"));     // answered: nothing to call
        Assert.Empty(rows[2].QuerySelectorAll("button"));     // pending but not callable (e.g. not live)
        var button = Assert.Single(rows[1].QuerySelectorAll("button"));
        Assert.Equal("call now", button.TextContent);

        await button.ClickAsync(new MouseEventArgs());
        Assert.Equal("item-02", called);
        Assert.Null(selected);                                // the button's click stops at the button
    }

    [Fact]
    public async Task HarnessControls_offer_pause_stop_cancel_and_the_queue_jump_only_for_a_live_batch_each_for_the_selection_it_applies_to()
    {
        var finished = Render<HarnessControls>(p => p.Add(c => c.Batch, Batch(live: false, items: Item("item-01", "Succeeded"))));
        Assert.Empty(finished.FindAll("button"));
        Assert.Contains("executed", finished.Markup);

        var actions = new List<string>();
        var live = Render<HarnessControls>(p => p
            .Add(c => c.Batch, Batch(live: true, items: [Item("item-01", "Running"), Item("item-02", "Pending", 0, callable: true)]))
            .Add(c => c.SelectedItem, "item-02")
            .Add(c => c.OnAction, a => actions.Add(a)));
        var buttons = live.FindAll("button");
        Assert.Equal(["pause", "stop after in-flight", "cancel selected", "call selected now"], buttons.Select(b => b.TextContent));
        Assert.True(buttons[2].HasAttribute("disabled"));      // the selected item is pending, not running …
        Assert.False(buttons[3].HasAttribute("disabled"));     // … and callable now
        await buttons[0].ClickAsync(new MouseEventArgs());
        await live.FindAll("button")[3].ClickAsync(new MouseEventArgs());
        Assert.Equal(["pause", "call"], actions);

        var paused = Render<HarnessControls>(p => p.Add(c => c.Batch, Batch(live: true, paused: true, items: Item("item-01", "Running"))).Add(c => c.SelectedItem, "item-01"));
        Assert.Equal("resume", paused.FindAll("button")[0].TextContent);
        Assert.False(paused.FindAll("button")[2].HasAttribute("disabled"));
        Assert.True(paused.FindAll("button")[3].HasAttribute("disabled"));      // running: nothing to jump
    }

    [Fact]
    public async Task HarnessControls_offer_only_unschedule_for_a_scheduled_execution()
    {
        var actions = new List<string>();
        var scheduled = Batch(live: true, items: Item("item-01", "Pending", 0)) with { Scheduled = true, NotBeforeUtc = "2026-09-04T09:00:00+00:00" };
        var cut = Render<HarnessControls>(p => p.Add(c => c.Batch, scheduled).Add(c => c.OnAction, a => actions.Add(a)));

        var button = Assert.Single(cut.FindAll("button"));
        Assert.Equal("unschedule", button.TextContent);
        Assert.Contains("scheduled for", cut.Markup);
        await button.ClickAsync(new MouseEventArgs());
        Assert.Equal(["unschedule"], actions);
    }

    [Fact]
    public void HostBar_shows_both_usage_windows_the_idle_limit_and_marks_a_stale_or_locked_cache()
    {
        var now = DateTimeOffset.UtcNow;
        var fresh = new Utilization(20, now.AddHours(3), now.AddMinutes(-5), SevenDayPercent: 42, SevenDayResetsAt: now.AddDays(3));
        var cut = Render<HostBar>(p => p.Add(c => c.InFlight, 2).Add(c => c.MaxParallel, 4).Add(c => c.UtilizationCap, 80).Add(c => c.IdleMinutes, 10).Add(c => c.Utilization, fresh));

        var meters = cut.FindAll(".meter");
        Assert.Equal(2, meters.Count);
        Assert.Contains("20%", meters[0].TextContent);
        Assert.Contains("in 2 h 5", meters[0].TextContent);
        Assert.Contains("42%", meters[1].TextContent);
        Assert.Contains("5 min ago", cut.Find(".cachemeta").TextContent);
        Assert.DoesNotContain("stale", cut.Markup);
        Assert.Equal("width:20%", cut.Find(".meter .fill").GetAttribute("style"));
        Assert.Equal("left:80%", cut.Find(".capmark").GetAttribute("style"));
        Assert.Contains("idle min", cut.Markup);
        Assert.Equal("10", cut.FindAll("input")[2].GetAttribute("value"));

        var stale = new Utilization(85, now.AddMinutes(-10), now.AddHours(-9), LockedReason: "over_limit");
        var cut2 = Render<HostBar>(p => p.Add(c => c.UtilizationCap, 80).Add(c => c.Utilization, stale));
        Assert.Contains("stale", cut2.Find(".usage").ClassList);
        Assert.Contains("reset passed", cut2.Markup);
        Assert.Contains("locked: over_limit", cut2.Markup);
        Assert.Contains("over", cut2.Find(".meter .fill").ClassList);

        var none = Render<HostBar>(p => p.Add(c => c.Utilization, (Utilization?)null));
        Assert.Contains("utilization unknown", none.Markup);
    }

    [Fact]
    public void StageStrip_lights_detected_stages()
    {
        var cut = Render<StageStrip>(p => p.Add(c => c.S, new BatchStages(true, true, false, false)));
        var on = cut.FindAll(".stage.on").Select(e => e.TextContent).ToList();
        Assert.Equal(["defined", "itemized"], on);
        Assert.Equal(4, cut.FindAll(".stage").Count);
    }

    [Fact]
    public void StreamPane_labels_thinking_and_usage_events_by_kind_without_repeating_the_kind_in_the_text()
    {
        var cut = Render<StreamPane>(p => p.Add(c => c.Events,
            [new StreamEvent("init", "model claude-sonnet-5; tools []; mcp servers 0", "{}"),
             new StreamEvent("thinking", "~6,606 tokens so far (estimated), 55 steps", "{}"),
             new StreamEvent("usage", "five-hour 49%, seven-day 18%", "{}")]));
        var events = cut.FindAll(".ev");
        Assert.Equal("initmodel claude-sonnet-5; tools []; mcp servers 0", events[0].TextContent);
        Assert.Equal("thinking", events[1].QuerySelector(".k")!.TextContent);
        Assert.Contains("usage", events[2].QuerySelector(".k")!.ClassList);
    }

    [Fact]
    public void StreamPane_renders_parsed_text_and_says_when_there_is_nothing()
    {
        var empty = Render<StreamPane>(p => p.Add(c => c.Events, []));
        Assert.Contains("no stream yet", empty.Markup);

        var cut = Render<StreamPane>(p => p
            .Add(c => c.Title, "item-01 — call 1 (live)")
            .Add(c => c.Events, [new StreamEvent("text", "Reading the note.", "{raw}"), new StreamEvent("done", "done — $0.210", "{raw2}")]));
        var events = cut.FindAll(".ev");
        Assert.Equal(2, events.Count);
        Assert.Contains("Reading the note.", events[0].TextContent);
        Assert.DoesNotContain("{raw}", cut.Markup);
        Assert.Contains("(live)", cut.Markup);
    }
}
