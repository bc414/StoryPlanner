using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoryPlanner.Mcp;

var builder = Host.CreateApplicationBuilder(args);

// stdout is the JSON-RPC channel — every log line must go to stderr.
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

var workingPath = Environment.GetEnvironmentVariable("STORYPLAN_WORKING");
var archivePath = Environment.GetEnvironmentVariable("STORYPLAN_ARCHIVE");

if (string.IsNullOrWhiteSpace(workingPath) || string.IsNullOrWhiteSpace(archivePath))
{
    Console.Error.WriteLine(
        "FATAL: STORYPLAN_WORKING and STORYPLAN_ARCHIVE environment variables must both be set " +
        "to absolute paths of .storyplan files.");
    return 1;
}

builder.Services.AddSingleton(new StoryPlanSources(workingPath, archivePath));

builder.Services
    .AddMcpServer(o => o.ServerInstructions = ServerInfo.Instructions)
    .WithStdioServerTransport()
    .WithTools<PlanTools>()
    .WithTools<ArchiveTools>()
    .WithTools<ConversationTools>()
    .WithTools<FlaggedTools>()
    .WithTools<ReferenceTools>();

var host = builder.Build();

// Eager load both files at startup — fail fast with a clear stderr message if either
// is missing or unreadable, rather than surfacing the failure mid-conversation.
try
{
    var sources = host.Services.GetRequiredService<StoryPlanSources>();
    sources.LoadAll();
    Console.Error.WriteLine(
        $"storyplanner-mcp: loaded working plan '{workingPath}' and archive '{archivePath}'.");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"FATAL: failed to load .storyplan files at startup: {ex.Message}");
    return 1;
}

await host.RunAsync();
return 0;
