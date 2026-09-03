using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StoryPlanner.PocketReader;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// One instance each for the life of the page. Blazor WebAssembly "scoped" is effectively
// singleton — there is one circuit, the browser tab.
builder.Services.AddScoped<PlanStore>();
builder.Services.AddScoped<ReaderState>();

var host = builder.Build();

// Restore whatever files were picked on an earlier visit before the first render, so the
// random view never flashes the picker on a phone that already holds both plans.
await host.Services.GetRequiredService<PlanStore>().InitializeAsync();
await host.Services.GetRequiredService<ReaderState>().InitializeAsync();

await host.RunAsync();
