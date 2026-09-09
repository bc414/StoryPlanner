using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StoryPlanner.AgentRunner;

public sealed record ControlRequest(string Batch, string Action, string? Item = null);
public sealed record ExecuteRequest(string Path, string? Item = null, DateTimeOffset? NotBefore = null);
public sealed record HostSettingsRequest(int? MaxParallel = null, int? UtilizationCap = null, int? IdleMinutes = null);

/// <summary>
/// The state the page binds to, exposed a second time as JSON so a terminal or a Claude
/// Code session can read a batch and steer the harness the same way the buttons do. Batch
/// ids contain slashes, so the batch-addressed routes take the id as a catch-all or a query
/// value rather than a path segment. Harness only: no route can change a batch's model,
/// directions or items, and none calls an item that has answered.
/// </summary>
public static class RunnerApi
{
    public static void MapRunnerApi(this IEndpointRouteBuilder app, RunnerHost host, IHostApplicationLifetime lifetime)
    {
        app.MapGet("/api/ping", () => Results.Ok(new { ok = true, startedUtc = host.StartedUtc, workingDir = host.WorkingDir, launchDir = host.LaunchDir }));

        app.MapGet("/api/host", () =>
        {
            var u = host.ReadUtilization();
            return Results.Ok(new
            {
                host.MaxParallel, host.UtilizationCap, host.IdleMinutes, host.InFlight, host.ShuttingDown, host.WorkingDir, host.LaunchDir,
                utilization = u is null ? null : new { u.Percent, u.ResetsAt, u.ReadAtUtc, u.Stale },
                batches = host.Batches().Select(Summary),
            });
        });

        app.MapGet("/api/batches", () => Results.Ok(host.Batches().Select(Summary)));

        app.MapGet("/api/batches/{**id}", (string id) =>
            host.Batch(id) is { } b ? Results.Ok(b) : Results.NotFound(new { error = $"no batch {id}" }));

        app.MapGet("/api/stream", (string batch, string item, int? tail) =>
        {
            var snapshot = host.Batch(batch);
            var i = snapshot?.Items.FirstOrDefault(x => x.Item == item);
            if (i?.StreamPath is null) return Results.NotFound(new { error = $"no call stream for {batch} / {item}" });
            return Results.Ok(new { batch, item, path = i.StreamPath, events = StreamEvents.ReadTail(i.StreamPath, tail ?? 200) });
        });

        app.MapPost("/api/batches", (ExecuteRequest req) =>
        {
            var r = host.Execute(req.Path, req.Item, req.NotBefore);
            return r.Ok ? Results.Ok(r) : Results.BadRequest(r);
        });

        app.MapPost("/api/batch-control", (ControlRequest req) =>
        {
            var ok = req.Action switch
            {
                "unschedule" => host.Unschedule(req.Batch),
                "pause" => host.Pause(req.Batch),
                "resume" => host.Resume(req.Batch),
                "stop" => host.Stop(req.Batch),
                "cancel" => req.Item is not null && host.Cancel(req.Batch, req.Item),
                _ => false,
            };
            return ok ? Results.Ok(new { ok = true, req.Batch, req.Action })
                      : Results.BadRequest(new { ok = false, error = $"{req.Action} not applied to {req.Batch} — not live, unknown action, or missing item" });
        });

        app.MapPut("/api/host/settings", (HostSettingsRequest req) =>
        {
            if (req.MaxParallel is { } p) host.SetMaxParallel(p);
            if (req.UtilizationCap is { } c) host.SetUtilizationCap(c);
            if (req.IdleMinutes is { } m) host.SetIdleMinutes(m);
            return Results.Ok(new { host.MaxParallel, host.UtilizationCap, host.IdleMinutes });
        });

        app.MapPost("/api/host/shutdown", (bool? now) =>
        {
            _ = Task.Run(async () => { await host.ShutdownAsync(now ?? false); lifetime.StopApplication(); });
            return Results.Ok(new { ok = true, now = now ?? false });
        });
    }

    private static object Summary(BatchSnapshot b) => new
    {
        b.Id, b.Study, b.Name, b.Kind, b.Model, b.Directions, b.Live, b.Completed, b.Paused, b.StopRequested, b.InFlight, b.Pending, b.Succeeded, b.Failed,
        b.CostUsd, items = b.Items.Count, b.LastActivityUtc, b.Stages, b.Scheduled, b.NotBeforeUtc, b.Error,
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
