using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using StoryPlanner.AgentRunner;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The host and its JSON routes over a real localhost listener on a free port, with the fake
/// launcher: enqueue, read, pause/resume, the global ceiling as the effective one, and the
/// refusal of a second enqueue on a live run. The routes are the same state the page binds
/// to, so what they return is what the page shows. Tier: pure (temp folders, loopback only).
/// </summary>
public class RunnerHostApiTests : IAsyncLifetime
{
    private TempRun _t = null!;
    private FakeLauncher _launcher = null!;
    private RunnerHost _host = null!;
    private WebApplication _app = null!;
    private HttpClient _http = null!;

    public async Task InitializeAsync()
    {
        _t = new TempRun(work: "smoke", run: "r1");
        _launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        // The gate reads a supplied figure, never the developer's real ~/.claude.json cache: a live
        // subscription window at or over the cap would hold every launch and time these tests out.
        _host = new RunnerHost(new HostConfig(FanoutRoot: _t.FanoutRoot, MaxParallel: 1), _launcher, "test", utilization: () => null);
        _app = RunnerApi.BuildApp(_host, "http://127.0.0.1:0");
        await _app.StartAsync();
        var address = _app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses.First();
        _http = new HttpClient { BaseAddress = new Uri(address) };
    }

    public async Task DisposeAsync()
    {
        _http.Dispose();
        await _app.StopAsync();
        await _app.DisposeAsync();
        _host.Dispose();
        _t.Dispose();
    }

    [Fact]
    public async Task Ping_answers_and_the_host_reports_its_ceilings()
    {
        var ping = await _http.GetAsync("/api/ping");
        Assert.Equal(HttpStatusCode.OK, ping.StatusCode);

        var host = await _http.GetFromJsonAsync<JsonElement>("/api/host");
        Assert.Equal(1, host.GetProperty("maxParallel").GetInt32());
        Assert.Equal(80, host.GetProperty("utilizationCap").GetInt32());
    }

    [Fact]
    public async Task Enqueue_runs_the_batch_under_the_host_ceiling_and_the_routes_show_and_steer_it()
    {
        _t.WriteJobs(3, maxParallel: 3);   // the run asks for 3; the host allows 1 → effective 1
        var enq = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(_t.JobFilePath));
        Assert.Equal(HttpStatusCode.OK, enq.StatusCode);
        var result = await enq.Content.ReadFromJsonAsync<EnqueueResult>();
        Assert.Equal("smoke/r1", result!.RunId);

        await Wait.Until(() => _host.InFlight == 1, what: "first child");
        var again = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(_t.JobFilePath));
        Assert.Equal(HttpStatusCode.BadRequest, again.StatusCode);   // already running

        var run = await _http.GetFromJsonAsync<JsonElement>("/api/runs/smoke/r1");
        Assert.True(run.GetProperty("live").GetBoolean());
        Assert.Equal(1, run.GetProperty("inFlight").GetInt32());
        Assert.Equal("Running", run.GetProperty("jobs")[0].GetProperty("state").GetString());

        var stream = await _http.GetFromJsonAsync<JsonElement>("/api/stream?run=smoke/r1&job=job-01&tail=5");
        Assert.Equal("init", stream.GetProperty("events")[0].GetProperty("kind").GetString());

        var pause = await _http.PostAsJsonAsync("/api/run-control", new ControlRequest("smoke/r1", "pause"));
        Assert.Equal(HttpStatusCode.OK, pause.StatusCode);
        _launcher.Hold!.Release();
        await Wait.Until(() => _host.InFlight == 0, what: "first child done");
        await Task.Delay(300);
        Assert.Equal(1, _launcher.Launched);                    // paused: no second launch
        Assert.Equal(1, _launcher.MaxConcurrent);               // the host ceiling, not the run's 3

        await _http.PostAsJsonAsync("/api/run-control", new ControlRequest("smoke/r1", "resume"));
        _launcher.Hold.Release(); _launcher.Hold.Release();
        await Wait.Until(() => _host.Run("smoke/r1")!.Completed, what: "batch complete");

        var runs = await _http.GetFromJsonAsync<JsonElement>("/api/runs");
        Assert.Equal(3, runs[0].GetProperty("succeeded").GetInt32());
        Assert.True(File.Exists(Path.Combine(_t.FanoutRoot, "host-log.txt")));
        Assert.Contains("paused", File.ReadAllText(Path.Combine(_t.FanoutRoot, "host-log.txt")));

        // A second enqueue after completion is accepted and says it has nothing to launch.
        var second = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(_t.JobFilePath));
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
        Assert.Contains("0 to launch, 3 skipped as succeeded", (await second.Content.ReadFromJsonAsync<EnqueueResult>())!.Message);
    }

    /// <summary>
    /// The run route describes the job file, not the enqueued subset: a pilot leaves the other
    /// jobs pending and the batch incomplete (the 2026-09-05 audit run read "pending 0, batch
    /// complete" after its pilot).
    /// </summary>
    [Fact]
    public async Task A_pilot_enqueue_launches_one_job_and_the_run_route_reports_the_rest_pending()
    {
        _t.WriteJobs(3);
        _launcher.Hold = null;
        var enq = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(_t.JobFilePath, Job: "job-02"));
        Assert.Equal(HttpStatusCode.OK, enq.StatusCode);
        Assert.Contains("1 job(s) enqueued — 1 to launch", (await enq.Content.ReadFromJsonAsync<EnqueueResult>())!.Message);

        await Wait.Until(() => _host.Run("smoke/r1")!.Succeeded == 1 && !_host.Run("smoke/r1")!.Live, what: "pilot done");
        var run = await _http.GetFromJsonAsync<JsonElement>("/api/runs/smoke/r1");
        Assert.Equal(3, run.GetProperty("jobs").GetArrayLength());
        Assert.Equal(2, run.GetProperty("pending").GetInt32());
        Assert.False(run.GetProperty("completed").GetBoolean());
        Assert.True(run.GetProperty("stages").GetProperty("piloted").GetBoolean());
        Assert.False(run.GetProperty("stages").GetProperty("batchComplete").GetBoolean());
        Assert.Equal(1, _launcher.Launched);
    }

    [Fact]
    public async Task A_scheduled_enqueue_waits_for_its_time_shows_as_scheduled_and_can_be_unscheduled()
    {
        _t.WriteJobs(1);
        _launcher.Hold = null;
        var soon = DateTimeOffset.UtcNow.AddSeconds(3);
        var enq = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(_t.JobFilePath, NotBefore: soon));
        Assert.Equal(HttpStatusCode.OK, enq.StatusCode);
        Assert.Contains("scheduled", (await enq.Content.ReadFromJsonAsync<EnqueueResult>())!.Message);

        var run = await _http.GetFromJsonAsync<JsonElement>("/api/runs/smoke/r1");
        Assert.True(run.GetProperty("scheduled").GetBoolean());
        Assert.NotNull(run.GetProperty("notBeforeUtc").GetString());
        Assert.Equal(0, _launcher.Launched);

        var again = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(_t.JobFilePath));
        Assert.Equal(HttpStatusCode.BadRequest, again.StatusCode);                       // already scheduled
        var pause = await _http.PostAsJsonAsync("/api/run-control", new ControlRequest("smoke/r1", "pause"));
        Assert.Equal(HttpStatusCode.BadRequest, pause.StatusCode);                       // not started: only unschedule applies

        await Wait.Until(() => _launcher.Launched == 1, timeoutMs: 8000, what: "scheduled start");
        await Wait.Until(() => _host.Run("smoke/r1")!.Completed, what: "batch complete");
        var log = File.ReadAllText(Path.Combine(_t.FanoutRoot, "host-log.txt"));
        Assert.Contains("scheduled for", log);
        Assert.Contains("scheduled time reached", log);

        // A second scheduled enqueue on the (now finished) run, then unscheduled: no attempt, no row.
        var later = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(_t.JobFilePath, NotBefore: DateTimeOffset.UtcNow.AddHours(1)));
        Assert.Equal(HttpStatusCode.OK, later.StatusCode);
        var un = await _http.PostAsJsonAsync("/api/run-control", new ControlRequest("smoke/r1", "unschedule"));
        Assert.Equal(HttpStatusCode.OK, un.StatusCode);
        Assert.False((await _http.GetFromJsonAsync<JsonElement>("/api/runs/smoke/r1")).GetProperty("scheduled").GetBoolean());
        Assert.Equal(1, _launcher.Launched);
    }

    [Fact]
    public void At_forms_parse_to_the_next_clock_time_an_instant_or_the_cached_reset()
    {
        var now = new DateTimeOffset(2026, 9, 3, 23, 30, 0, TimeSpan.Zero).ToLocalTime();
        var (clock, e1) = Schedule.ParseAt("04:00", now, null);
        Assert.Null(e1);
        Assert.True(clock > now && clock!.Value - now < TimeSpan.FromHours(24));
        Assert.Equal(4, clock.Value.ToLocalTime().Hour);

        var (iso, e2) = Schedule.ParseAt("2026-09-04T04:00", now, null);
        Assert.Null(e2);
        Assert.Equal(4, iso!.Value.ToLocalTime().Hour);

        var (past, e3) = Schedule.ParseAt("2026-09-01T04:00", now, null);
        Assert.Null(past); Assert.Contains("past", e3);

        var (noCache, e4) = Schedule.ParseAt("reset", now, null);
        Assert.Null(noCache); Assert.Contains("no cached", e4);

        var cached = new Utilization(85, now.AddHours(2), now);
        var (reset, e5) = Schedule.ParseAt("reset", now, cached);
        Assert.Null(e5);
        Assert.Equal(now.AddHours(2).AddMinutes(1), reset);

        var (passed, e6) = Schedule.ParseAt("reset", now, new Utilization(85, now.AddMinutes(-5), now));
        Assert.Null(passed); Assert.Contains("already past", e6);

        var (junk, e7) = Schedule.ParseAt("soon", now, null);
        Assert.Null(junk); Assert.Contains("HH:mm", e7);
    }

    [Fact]
    public async Task The_cap_holds_every_launch_while_the_supplied_utilization_is_at_or_over_it()
    {
        var figure = new Utilization(85, DateTimeOffset.UtcNow.AddHours(2), DateTimeOffset.UtcNow);
        using var capped = new RunnerHost(new HostConfig(FanoutRoot: _t.FanoutRoot, MaxParallel: 1, UtilizationCap: 80), _launcher, "test", utilization: () => figure);
        _t.WriteJobs(1);
        _launcher.Hold = null;
        var enq = capped.Enqueue(_t.JobFilePath, null);
        Assert.True(enq.Ok, enq.Message);
        await Task.Delay(700);
        Assert.Equal(0, _launcher.Launched);
        Assert.Contains("85% ≥ cap 80%", capped.Run("smoke/r1")!.HoldReason);

        figure = figure with { Percent = 40 };
        await Wait.Until(() => capped.Run("smoke/r1")!.Completed, what: "launch once under the cap");
        Assert.Equal(1, _launcher.Launched);
    }

    [Fact]
    public async Task Host_settings_change_live_and_an_unknown_control_is_refused()
    {
        var put = await _http.PutAsJsonAsync("/api/host/settings", new HostSettingsRequest(MaxParallel: 3, UtilizationCap: 55));
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(3, _host.MaxParallel);
        Assert.Equal(55, _host.UtilizationCap);

        var bad = await _http.PostAsJsonAsync("/api/run-control", new ControlRequest("nope", "reroll"));
        Assert.Equal(HttpStatusCode.BadRequest, bad.StatusCode);

        var outside = await _http.PostAsJsonAsync("/api/runs", new EnqueueRequest(Path.Combine(_t.Root, "elsewhere", "jobs.json")));
        Assert.Equal(HttpStatusCode.BadRequest, outside.StatusCode);   // run folders live under the fanout root
    }
}
