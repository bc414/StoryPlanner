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
/// launcher: execute, read, pause/resume, the global ceiling as the effective one, the idle
/// limit as a host setting, and the refusal of a second execution on a live batch. The routes
/// are the same state the page binds to. Tier: pure (temp folders, loopback only).
/// </summary>
public class RunnerHostApiTests : IAsyncLifetime
{
    private TempBatch _t = null!;
    private FakeLauncher _launcher = null!;
    private RunnerHost _host = null!;
    private WebApplication _app = null!;
    private HttpClient _http = null!;

    public async Task InitializeAsync()
    {
        _t = new TempBatch();
        _launcher = new FakeLauncher { Hold = new SemaphoreSlim(0) };
        // The gate reads a supplied figure, never the developer's real ~/.claude.json cache.
        _host = new RunnerHost(new HostConfig(LaunchDir: _t.LaunchDir, MaxParallel: 1, IdleMinutes: 7), _launcher, "test", workingDir: _t.WorkingDir,
            utilization: () => null, logPath: Path.Combine(_t.Root, "host-log.txt"));
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
    public async Task Ping_answers_and_the_host_reports_its_ceilings_and_idle_limit()
    {
        var ping = await _http.GetAsync("/api/ping");
        Assert.Equal(HttpStatusCode.OK, ping.StatusCode);

        var host = await _http.GetFromJsonAsync<JsonElement>("/api/host");
        Assert.Equal(1, host.GetProperty("maxParallel").GetInt32());
        Assert.Equal(80, host.GetProperty("utilizationCap").GetInt32());
        Assert.Equal(7, host.GetProperty("idleMinutes").GetInt32());
        Assert.Equal(Path.GetFullPath(_t.LaunchDir), host.GetProperty("launchDir").GetString());
    }

    [Fact]
    public async Task Execute_runs_the_batch_under_the_host_ceiling_and_the_routes_show_and_steer_it()
    {
        _t.WriteItems(3);
        var exec = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(_t.DefinitionPath));
        Assert.Equal(HttpStatusCode.OK, exec.StatusCode);
        var result = await exec.Content.ReadFromJsonAsync<ExecuteResult>();
        Assert.Equal(_t.Id, result!.BatchId);
        Assert.Contains("execution 1, 3 item(s) — 3 to call", result.Message);

        await Wait.Until(() => _host.InFlight == 1, what: "first child");
        var again = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(_t.DefinitionPath));
        Assert.Equal(HttpStatusCode.BadRequest, again.StatusCode);   // already executing

        var batch = await _http.GetFromJsonAsync<JsonElement>("/api/batches/" + _t.Id);
        Assert.True(batch.GetProperty("live").GetBoolean());
        Assert.Equal(1, batch.GetProperty("inFlight").GetInt32());
        Assert.Equal("Running", batch.GetProperty("items")[0].GetProperty("state").GetString());
        Assert.Equal(TimeSpan.FromMinutes(7), _launcher.Requests.First().IdleLimit);

        var stream = await _http.GetFromJsonAsync<JsonElement>($"/api/stream?batch={_t.Id}&item=item-01&tail=5");
        Assert.Equal("init", stream.GetProperty("events")[0].GetProperty("kind").GetString());

        var pause = await _http.PostAsJsonAsync("/api/batch-control", new ControlRequest(_t.Id, "pause"));
        Assert.Equal(HttpStatusCode.OK, pause.StatusCode);
        _launcher.Hold!.Release();
        await Wait.Until(() => _host.InFlight == 0, what: "first child done");
        await Task.Delay(300);
        Assert.Equal(1, _launcher.Launched);                    // paused: no second launch
        Assert.Equal(1, _launcher.MaxConcurrent);               // the host ceiling

        await _http.PostAsJsonAsync("/api/batch-control", new ControlRequest(_t.Id, "resume"));
        _launcher.Hold.Release(); _launcher.Hold.Release();
        await Wait.Until(() => _host.Batch(_t.Id)!.Completed, what: "batch complete");

        var batches = await _http.GetFromJsonAsync<JsonElement>("/api/batches");
        Assert.Equal(3, batches[0].GetProperty("succeeded").GetInt32());
        var log = File.ReadAllText(Path.Combine(_t.Root, "host-log.txt"));
        Assert.Contains("paused", log);

        // A second execution after completion is accepted and says it has nothing to call.
        var second = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(_t.DefinitionPath));
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
        Assert.Contains("0 to call, 3 skipped as answered", (await second.Content.ReadFromJsonAsync<ExecuteResult>())!.Message);
    }

    [Fact]
    public async Task A_pilot_calls_one_item_and_the_batch_route_reports_the_rest_pending()
    {
        _t.WriteItems(3);
        _launcher.Hold = null;
        var exec = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(_t.DefinitionPath, Item: "item-02"));
        Assert.Equal(HttpStatusCode.OK, exec.StatusCode);
        Assert.Contains("1 to call", (await exec.Content.ReadFromJsonAsync<ExecuteResult>())!.Message);

        await Wait.Until(() => _host.Batch(_t.Id)!.Succeeded == 1 && !_host.Batch(_t.Id)!.Live, what: "pilot done");
        var batch = await _http.GetFromJsonAsync<JsonElement>("/api/batches/" + _t.Id);
        Assert.Equal(3, batch.GetProperty("items").GetArrayLength());
        Assert.Equal(2, batch.GetProperty("pending").GetInt32());
        Assert.False(batch.GetProperty("completed").GetBoolean());
        Assert.True(batch.GetProperty("stages").GetProperty("piloted").GetBoolean());
        Assert.False(batch.GetProperty("stages").GetProperty("executed").GetBoolean());
        Assert.Equal(1, _launcher.Launched);
    }

    [Fact]
    public async Task A_scheduled_execution_waits_for_its_time_shows_as_scheduled_and_can_be_unscheduled()
    {
        _t.WriteItems(1);
        _launcher.Hold = null;
        var soon = DateTimeOffset.UtcNow.AddSeconds(3);
        var exec = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(_t.DefinitionPath, NotBefore: soon));
        Assert.Equal(HttpStatusCode.OK, exec.StatusCode);
        Assert.Contains("scheduled", (await exec.Content.ReadFromJsonAsync<ExecuteResult>())!.Message);

        var batch = await _http.GetFromJsonAsync<JsonElement>("/api/batches/" + _t.Id);
        Assert.True(batch.GetProperty("scheduled").GetBoolean());
        Assert.Equal(0, _launcher.Launched);

        var again = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(_t.DefinitionPath));
        Assert.Equal(HttpStatusCode.BadRequest, again.StatusCode);                       // already scheduled
        var pause = await _http.PostAsJsonAsync("/api/batch-control", new ControlRequest(_t.Id, "pause"));
        Assert.Equal(HttpStatusCode.BadRequest, pause.StatusCode);                       // not started: only unschedule applies

        await Wait.Until(() => _launcher.Launched == 1, timeoutMs: 8000, what: "scheduled start");
        await Wait.Until(() => _host.Batch(_t.Id)!.Completed, what: "batch complete");

        var later = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(_t.DefinitionPath, NotBefore: DateTimeOffset.UtcNow.AddHours(1)));
        Assert.Equal(HttpStatusCode.OK, later.StatusCode);
        var un = await _http.PostAsJsonAsync("/api/batch-control", new ControlRequest(_t.Id, "unschedule"));
        Assert.Equal(HttpStatusCode.OK, un.StatusCode);
        Assert.False((await _http.GetFromJsonAsync<JsonElement>("/api/batches/" + _t.Id)).GetProperty("scheduled").GetBoolean());
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

        var (junk, e7) = Schedule.ParseAt("soon", now, null);
        Assert.Null(junk); Assert.Contains("HH:mm", e7);
    }

    [Fact]
    public async Task The_cap_holds_every_launch_while_the_supplied_utilization_is_at_or_over_it()
    {
        var figure = new Utilization(85, DateTimeOffset.UtcNow.AddHours(2), DateTimeOffset.UtcNow);
        using var capped = new RunnerHost(new HostConfig(LaunchDir: _t.LaunchDir, MaxParallel: 1, UtilizationCap: 80), _launcher, "test", workingDir: _t.WorkingDir,
            utilization: () => figure, logPath: Path.Combine(_t.Root, "host-log-2.txt"));
        _t.WriteItems(1);
        _launcher.Hold = null;
        var exec = capped.Execute(_t.DefinitionPath, null);
        Assert.True(exec.Ok, exec.Message);
        await Task.Delay(700);
        Assert.Equal(0, _launcher.Launched);
        Assert.Contains("85% ≥ cap 80%", capped.Batch(_t.Id)!.HoldReason);

        figure = figure with { Percent = 40 };
        await Wait.Until(() => capped.Batch(_t.Id)!.Completed, what: "launch once under the cap");
        Assert.Equal(1, _launcher.Launched);
    }

    [Fact]
    public async Task Host_settings_change_live_and_an_unknown_control_or_batch_is_refused()
    {
        var put = await _http.PutAsJsonAsync("/api/host/settings", new HostSettingsRequest(MaxParallel: 3, UtilizationCap: 55, IdleMinutes: 20));
        Assert.Equal(HttpStatusCode.OK, put.StatusCode);
        Assert.Equal(3, _host.MaxParallel);
        Assert.Equal(55, _host.UtilizationCap);
        Assert.Equal(20, _host.IdleMinutes);

        var bad = await _http.PostAsJsonAsync("/api/batch-control", new ControlRequest("nope", "reroll"));
        Assert.Equal(HttpStatusCode.BadRequest, bad.StatusCode);

        var nowhere = await _http.PostAsJsonAsync("/api/batches", new ExecuteRequest(Path.Combine(_t.Root, "elsewhere", "definition.md")));
        Assert.Equal(HttpStatusCode.BadRequest, nowhere.StatusCode);
        var missing = await _http.GetAsync("/api/batches/no/such/batch");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }
}
