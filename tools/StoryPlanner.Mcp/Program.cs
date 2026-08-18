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

// Optional, unlike the two .storyplan files: the plan's citations resolve without it, and only
// the cited text is unavailable when it is absent. So no fail-fast here — the source-text tools
// report that they are unconfigured and every other tool works unchanged.
var sourceTextPath = Environment.GetEnvironmentVariable("STORYPLAN_SOURCE_TEXTS");

// Also optional: the LINEAGE corpus — the founding-era chats (Gemini web app, AI Studio,
// NotebookLM) in one database (provenance, not ground truth). Replaced STORYPLAN_GEMINI_CORPUS
// on 2026-08-18, when gemini.db was absorbed as lineage.db's first source layer.
var lineagePath = Environment.GetEnvironmentVariable("STORYPLAN_LINEAGE");

builder.Services.AddSingleton(new StoryPlanSources(workingPath, archivePath));
builder.Services.AddSingleton(new SourceTextStore(sourceTextPath));
builder.Services.AddSingleton(new LineageStore(lineagePath));

builder.Services
    .AddMcpServer(o => o.ServerInstructions = ServerInfo.Instructions)
    .WithStdioServerTransport()
    .WithTools<PlanTools>()
    .WithTools<ArchiveTools>()
    .WithTools<ConversationTools>()
    .WithTools<FlaggedTools>()
    .WithTools<ReferenceTools>()
    .WithTools<SourceTextTools>()
    .WithTools<LineageTools>();

var host = builder.Build();

// Eager load both files at startup — fail fast with a clear stderr message if either
// is missing or unreadable, rather than surfacing the failure mid-conversation.
try
{
    var sources = host.Services.GetRequiredService<StoryPlanSources>();
    sources.LoadAll();
    Console.Error.WriteLine(
        $"storyplanner-mcp: loaded working plan '{workingPath}' and archive '{archivePath}'.");

    // Manifest only — source-text bodies are streamed per query and never held resident.
    var sourceTexts = host.Services.GetRequiredService<SourceTextStore>();
    Console.Error.WriteLine(sourceTexts.IsConfigured
        ? $"storyplanner-mcp: source texts '{sourceTextPath}' ({sourceTexts.Manifest().Count} units)."
        : "storyplanner-mcp: no source-text corpus (STORYPLAN_SOURCE_TEXTS unset or missing).");

    var lineage = host.Services.GetRequiredService<LineageStore>();
    Console.Error.WriteLine(lineage.IsConfigured
        ? $"storyplanner-mcp: lineage corpus '{lineagePath}' ({lineage.GeminiEntries().Count} gemini entries, " +
          $"{lineage.Reports().Count} reports, {lineage.AiChats().Count} aistudio chats, " +
          $"{lineage.NlmNotebooks().Count} nlm notebooks)."
        : "storyplanner-mcp: no lineage corpus (STORYPLAN_LINEAGE unset or missing).");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"FATAL: failed to load .storyplan files at startup: {ex.Message}");
    return 1;
}

await host.RunAsync();
return 0;
