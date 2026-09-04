using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StoryPlanner.AgentRunner;

public sealed record ControlRequest(string Run, string Action, string? Job = null, int? Value = null);
public sealed record EnqueueRequest(string Path, string? Job = null, DateTimeOffset? NotBefore = null);
public sealed record HostSettingsRequest(int? MaxParallel = null, int? UtilizationCap = null);

/// <summary>
/// The state the page binds to, exposed a second time as JSON so a terminal or a Claude
/// Code session can read a batch and steer the harness the same way the buttons do. Run
/// ids contain slashes, so the run-addressed routes take the id as a catch-all or a query
/// value rather than a path segment. Harness only: no route can change a job's model,
/// inputs, protocol or instructions, and none re-launches a job under a new id.
/// </summary>
public static class RunnerApi
{
    public static void MapRunnerApi(this IEndpointRouteBuilder app, RunnerHost host, IHostApplicationLifetime lifetime)
    {
        app.MapGet("/api/ping", () => Results.Ok(new { ok = true, startedUtc = host.StartedUtc, fanoutRoot = host.FanoutRoot }));

        app.MapGet("/api/host", () =>
        {
            var u = host.ReadUtilization();
            return Results.Ok(new
            {
                host.MaxParallel, host.UtilizationCap, host.InFlight, host.ShuttingDown, host.FanoutRoot,
                utilization = u is null ? null : new { u.Percent, u.ResetsAt, u.ReadAtUtc, u.Stale },
                runs = host.Runs().Select(Summary),
            });
        });

        app.MapGet("/api/runs", () => Results.Ok(host.Runs().Select(Summary)));

        app.MapGet("/api/runs/{**id}", (string id) =>
            host.Run(id) is { } r ? Results.Ok(r) : Results.NotFound(new { error = $"no run {id}" }));

        app.MapGet("/api/stream", (string run, string job, int? tail) =>
        {
            var snapshot = host.Run(run);
            var j = snapshot?.Jobs.FirstOrDefault(x => x.Id == job);
            if (j?.StreamPath is null) return Results.NotFound(new { error = $"no attempt stream for {run} / {job}" });
            return Results.Ok(new { run, job, path = j.StreamPath, events = StreamEvents.ReadTail(j.StreamPath, tail ?? 200) });
        });

        app.MapPost("/api/runs", (EnqueueRequest req) =>
        {
            var r = host.Enqueue(req.Path, req.Job, req.NotBefore);
            return r.Ok ? Results.Ok(r) : Results.BadRequest(r);
        });

        app.MapPost("/api/run-control", (ControlRequest req) =>
        {
            var ok = req.Action switch
            {
                "unschedule" => host.Unschedule(req.Run),
                "pause" => host.Pause(req.Run),
                "resume" => host.Resume(req.Run),
                "stop" => host.Stop(req.Run),
                "cancel" => req.Job is not null && host.Cancel(req.Run, req.Job),
                "maxParallel" => req.Value is { } n && host.SetRunMaxParallel(req.Run, n),
                _ => false,
            };
            return ok ? Results.Ok(new { ok = true, req.Run, req.Action })
                      : Results.BadRequest(new { ok = false, error = $"{req.Action} not applied to {req.Run} — not live, unknown action, or missing job/value" });
        });

        app.MapPut("/api/host/settings", (HostSettingsRequest req) =>
        {
            if (req.MaxParallel is { } p) host.SetMaxParallel(p);
            if (req.UtilizationCap is { } c) host.SetUtilizationCap(c);
            return Results.Ok(new { host.MaxParallel, host.UtilizationCap });
        });

        app.MapPost("/api/host/shutdown", (bool? now) =>
        {
            _ = Task.Run(async () => { await host.ShutdownAsync(now ?? false); lifetime.StopApplication(); });
            return Results.Ok(new { ok = true, now = now ?? false });
        });
    }

    private static object Summary(RunSnapshot r) => new
    {
        r.RunId, r.Work, r.Live, r.Completed, r.Paused, r.StopRequested, r.InFlight, r.Pending, r.Succeeded, r.Failed,
        r.CostUsd, jobs = r.Jobs.Count, r.InputsResolvable, r.LastActivityUtc, r.Stages, r.Scheduled, r.NotBeforeUtc,
    };

    /// <summary>The web application: Razor components on the page, the JSON routes beside them, the host as a singleton.</summary>
    public static WebApplication BuildApp(RunnerHost host, string url)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ContentRootPath = AppContext.BaseDirectory, Args = [] });
        builder.WebHost.UseUrls(url);
        builder.Logging.ClearProviders();
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddSingleton(host);

        var app = builder.Build();
        app.UseStaticFiles();
        app.UseAntiforgery();
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        app.MapRunnerApi(host, app.Lifetime);
        return app;
    }
}
