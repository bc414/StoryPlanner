namespace StoryPlanner.ResultsQuery;

/// <summary>The one batch the page serves, re-read on every query so a running batch shows its current results.</summary>
public sealed class BatchSource(string definitionPath)
{
    public string DefinitionPath { get; } = definitionPath;
    public LoadedBatch Load(string? group = null) => LoadedBatch.Load(DefinitionPath, group ?? LoadedBatch.DefaultGroupSeparator);
}

/// <summary>The web application: the page's Razor components and the batch source as a singleton; no JSON routes.</summary>
public static class Head
{
    public static WebApplication BuildApp(BatchSource source, string url)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ContentRootPath = AppContext.BaseDirectory, Args = [] });
        builder.WebHost.UseUrls(url);
        // Outside publish/ the framework script lives only in the static web assets manifest,
        // which Production never reads: without this a bin/ run serves a page that never connects.
        builder.WebHost.UseStaticWebAssets();
        builder.Logging.ClearProviders();
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddSingleton(source);
        var app = builder.Build();
        app.UseStaticFiles();
        app.UseAntiforgery();
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        return app;
    }
}
