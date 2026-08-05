using System.Text.Json;
using StoryPlanner.SourceTexts;

// Ingests published source material — episode transcripts, fic chapters, game flavour text —
// into sources.db for the MCP server to retrieve. Reads the .storyplan read-only for its
// Work/Part spine and never writes to it.
//
//   dotnet run --project tools/StoryPlanner.SourceTexts -- <config.json> [--apply] [--work NAME]

var positional = args.Where(a => !a.StartsWith("--")).ToList();
var apply = args.Contains("--apply");
var onlyWorkIndex = Array.IndexOf(args, "--work");
var onlyWork = onlyWorkIndex >= 0 && onlyWorkIndex + 1 < args.Length ? args[onlyWorkIndex + 1] : null;

if (positional.Count != 1)
{
    Console.Error.WriteLine("Usage: dotnet run -- <config.json> [--apply] [--work NAME]");
    return 2;
}

var configPath = positional[0];
if (!File.Exists(configPath))
{
    Console.Error.WriteLine($"Config file not found: {configPath}");
    return 2;
}

using var doc = JsonDocument.Parse(await File.ReadAllTextAsync(configPath));
var root = doc.RootElement;

var storyplanPath = root.GetProperty("storyplan").GetString()!;
var outputPath = root.GetProperty("output").GetString()!;

Console.WriteLine($"source-texts ingest ({(apply ? "APPLY" : "DRY RUN")})");
Console.WriteLine($"  plan   : {storyplanPath} (read-only)");
Console.WriteLine($"  output : {outputPath}");

var spine = PlanSpine.Load(storyplanPath);
Console.WriteLine($"  spine  : {spine.Works.Count} work(s), {spine.Works.Sum(w => w.Parts.Count)} part(s)");

var report = new IngestReport();
var allUnits = new List<SourceTextUnit>();
var touchedWorks = new List<string>();

foreach (var src in root.GetProperty("sources").EnumerateArray())
{
    var workName = src.GetProperty("work").GetString()!;
    if (onlyWork is not null && !workName.Equals(onlyWork, StringComparison.OrdinalIgnoreCase)) continue;

    var reader = src.GetProperty("reader").GetString()!;
    var path = src.GetProperty("path").GetString()!;

    var work = spine.FindWork(workName);
    if (work is null)
    {
        report.Error($"{workName}: no Work of that name in the plan. Names must match exactly — " +
                     "refusing to guess which Work was meant.");
        continue;
    }

    Console.WriteLine();
    Console.WriteLine($"— {workName} ({reader})");

    IReadOnlyList<SourceTextUnit> units;
    switch (reader)
    {
        case "fimfiction-epub":
        {
            var chapters = FimfictionEpubReader.ReadChapters(path);
            var splitCodes = src.TryGetProperty("splitChapters", out var sc)
                ? sc.EnumerateArray().Select(e => e.GetString()!).ToHashSet(StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            // Section Parts are Parts of the Work but not chapters of the EPUB — they were
            // promoted out of a chapter, and counting them here would make the order mapping
            // refuse on a count it should never have included. Identified by their parent's
            // code from splitChapters, never by guessing at the shape of a code.
            var parts = work.Parts
                .Where(p => !splitCodes.Any(sc => p.Code.StartsWith($"{sc}-", StringComparison.OrdinalIgnoreCase)))
                .Select(p => (p.Code, p.Name))
                .ToList();
            units = FimfictionEpubReader.ToUnits(workName, chapters, parts, splitCodes, path, report);
            break;
        }
        case "fim-transcript-json":
        {
            var mergeInto = src.TryGetProperty("mergeInto", out var mi)
                ? mi.EnumerateObject().ToDictionary(p => p.Name, p => p.Value.GetString()!)
                : new Dictionary<string, string>();
            units = FimTranscriptJsonReader.Read(
                workName, await File.ReadAllTextAsync(path), mergeInto, path, report);
            break;
        }
        case "hoi4-localisation":
        {
            var includeTooltips = src.TryGetProperty("includeTooltips", out var it) && it.GetBoolean();
            var codes = work.Parts.Select(p => p.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var collected = new List<SourceTextUnit>();
            foreach (var file in Directory.EnumerateFiles(path, "country_*_l_english.yml"))
            {
                var tag = Hoi4LocalisationReader.TagFromFileName(Path.GetFileName(file));
                if (tag is null || !codes.Contains(tag)) continue; // a mod country Brian never seeded
                collected.AddRange(Hoi4LocalisationReader.Read(
                    workName, tag, await File.ReadAllTextAsync(file), includeTooltips, file));
            }
            units = collected;
            break;
        }
        default:
            report.Error($"{workName}: unknown reader \"{reader}\".");
            continue;
    }

    Console.WriteLine($"  {units.Count} unit(s), {units.Sum(u => u.Body.Length):N0} chars");
    allUnits.AddRange(units);
    touchedWorks.Add(workName);
}

// ── Coverage: both directions of mismatch, always ───────────────────────────────
// A Part with no text is ordinary (the fic is ongoing, the episode was never transcribed) and is
// reported as coverage, not as a defect. A unit with no Part is the dangerous direction — it
// means the text is addressed to something the plan does not have — so it is listed in full.
Console.WriteLine();
Console.WriteLine("— coverage");
foreach (var workName in touchedWorks)
{
    var work = spine.FindWork(workName)!;
    var codes = work.Parts.Select(p => p.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
    var unitCodes = allUnits.Where(u => u.WorkName == workName)
        .Select(u => u.PartCode).ToHashSet(StringComparer.OrdinalIgnoreCase);

    var covered = codes.Count(c => unitCodes.Contains(c));
    var orphans = unitCodes.Where(c => !codes.Contains(c)).OrderBy(c => c).ToList();

    Console.WriteLine($"  {workName}: {covered}/{work.Parts.Count} part(s) have text" +
                      (orphans.Count > 0 ? $", {orphans.Count} unit code(s) with NO matching Part" : ""));
    foreach (var o in orphans) Console.WriteLine($"      no Part: {o}");

    // A Part whose sections were promoted to Parts of their own is covered, not empty — its text
    // lives under the sub-codes. Reporting it as "no text" would read as a gap in acquisition
    // rather than the split it actually is.
    var split = work.Parts
        .Where(p => !unitCodes.Contains(p.Code) && unitCodes.Any(c => c.StartsWith($"{p.Code}-", StringComparison.OrdinalIgnoreCase)))
        .ToList();
    foreach (var p in split)
        Console.WriteLine($"      split:   {p.Code} — {p.Name} -> " +
                          $"{unitCodes.Count(c => c.StartsWith($"{p.Code}-", StringComparison.OrdinalIgnoreCase))} section Part(s)");

    var missing = work.Parts.Where(p => !unitCodes.Contains(p.Code)).Except(split).ToList();
    if (missing.Count is > 0 and <= 40)
        foreach (var p in missing)
            Console.WriteLine($"      no text: {p.Code}{(p.Name.Length > 0 ? $" — {p.Name}" : "")}");
    else if (missing.Count > 40)
        Console.WriteLine($"      no text: {missing.Count} part(s) — {string.Join(", ", missing.Take(12).Select(p => p.Code))}, …");
}

report.PrintMappings();
report.PrintNotes();
report.PrintProblems();

if (report.HasErrors)
{
    Console.WriteLine();
    Console.WriteLine("Refusing to write: fix the errors above first.");
    return 1;
}

if (!apply)
{
    Console.WriteLine();
    Console.WriteLine($"DRY RUN — nothing written. {allUnits.Count} unit(s) would be written to {outputPath}.");
    Console.WriteLine("Re-run with --apply once the mapping above looks right.");
    return 0;
}

using var conn = SourceTextDb.OpenWrite(outputPath);
var written = SourceTextDb.Replace(conn, touchedWorks, allUnits);
Console.WriteLine();
Console.WriteLine($"WROTE {written} unit(s) to {outputPath}");
return 0;
