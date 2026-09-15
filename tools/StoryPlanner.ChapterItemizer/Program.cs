using System.Diagnostics;
using System.Text;
using System.Text.Json;
using StoryPlanner.BatchFiles;
using StoryPlanner.ChapterItemizer;

// The chapter itemizer (d-2026-09-14-4): cuts a folder of converter-format story files into one
// item per `## Chapter` heading and writes a batch's index.md, with an itemizer line, and the item
// bodies under items/, refusing a batch that already has an index — an itemizer runs once per batch.
//
//   dotnet run --project tools/StoryPlanner.ChapterItemizer -- <config.json> <batch-folder>
//
// The config is authored and lives with the tool: {"source": "<folder of <slug>.md files>",
// "exclude": ["<slug>", ...]}, the source relative to the current directory. The excluded stories
// are the batch's narrowing, recorded in the index head by name. The itemizer line carries the
// date and the commit the tool ran at, marked dirty when the tool's folder has uncommitted changes.

if (args.Length < 2) return Usage();
var configPath = Path.GetFullPath(args[0]);
var batchDir = Path.GetFullPath(args[1]);
if (!File.Exists(configPath)) { Console.Error.WriteLine($"config not found: {configPath}"); return 2; }
if (!Directory.Exists(batchDir)) { Console.Error.WriteLine($"batch folder not found: {batchDir}"); return 2; }
var indexPath = Path.Combine(batchDir, "index.md");
if (File.Exists(indexPath)) { Console.Error.WriteLine($"{indexPath} exists — an itemizer runs once per batch; a new cut is a new batch."); return 2; }

var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
if (config?.Source is null) { Console.Error.WriteLine("config names no source folder"); return 2; }
var source = Path.GetFullPath(config.Source);
if (!Directory.Exists(source)) { Console.Error.WriteLine($"source folder not found: {source}"); return 2; }

var exclude = new HashSet<string>(config.Exclude ?? [], StringComparer.Ordinal);
var stories = Directory.GetFiles(source, "*.md")
    .Select(f => new Chapters.Story(Path.GetFileNameWithoutExtension(f), File.ReadAllText(f)))
    .ToList();
var present = stories.Select(s => s.Slug).ToHashSet(StringComparer.Ordinal);
foreach (var e in exclude.Where(e => !present.Contains(e)))
    Console.Error.WriteLine($"warning: excluded story not in the source folder: {e}");

IReadOnlyList<Chapters.Item> items;
try { items = Chapters.CutAll(stories, exclude); }
catch (InvalidOperationException ex) { Console.Error.WriteLine(ex.Message); return 2; }

var itemsDir = Path.Combine(batchDir, "items");
Directory.CreateDirectory(itemsDir);
foreach (var it in items)
    File.WriteAllText(Path.Combine(itemsDir, it.Id + ".md"), it.Body, new UTF8Encoding(false));

var excluded = exclude.Where(present.Contains).OrderBy(e => e, StringComparer.Ordinal).ToList();
var index = IndexFile.Render(
    Path.GetFileName(batchDir),
    $"tools/StoryPlanner.ChapterItemizer, {DateTime.Now:yyyy-MM-dd} {Commit(Path.GetDirectoryName(configPath)!)}",
    Chapters.LocatorNotation,
    sourceHash: null,
    items.Select(it => (it.Id, it.Locator, it.Description)),
    Chapters.Narrowing(excluded));
File.WriteAllText(indexPath, index, new UTF8Encoding(false));
Console.WriteLine($"{items.Count} items from {stories.Count - excluded.Count} stories → {batchDir} (index.md and items/)");
return 0;

static int Usage()
{
    Console.Error.WriteLine("Usage: ChapterItemizer <config.json> <batch-folder>");
    return 2;
}

/// <summary>The short commit the tool ran at, `-dirty` when its folder has uncommitted changes; the assembly version when git is unavailable.</summary>
static string Commit(string toolDir)
{
    try
    {
        var sha = Git(toolDir, "rev-parse --short HEAD");
        if (sha.Length == 0) return Fallback();
        var status = Git(toolDir, "status --porcelain -- .");
        return status.Length == 0 ? sha : sha + "-dirty";
    }
    catch { return Fallback(); }

    static string Fallback() => typeof(Chapters).Assembly.GetName().Version?.ToString() ?? "0";
}

static string Git(string cwd, string arguments)
{
    var psi = new ProcessStartInfo("git", arguments) { WorkingDirectory = cwd, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
    using var p = Process.Start(psi)!;
    var output = p.StandardOutput.ReadToEnd();
    p.WaitForExit();
    return p.ExitCode == 0 ? output.Trim() : "";
}

sealed record Config(string? Source, string[]? Exclude);
