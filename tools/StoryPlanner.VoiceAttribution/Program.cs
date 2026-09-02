using System.Diagnostics;
using System.Globalization;
using StoryPlanner.Core;
using StoryPlanner.VoiceAttribution;

// Read-only voice attribution: which notes of a .storyplan exist in the lineage corpus, where
// they appeared first, and in whose role. Writes a sidecar CSV — never touches the plan.
//
//   dotnet run --project tools/StoryPlanner.VoiceAttribution -c Release -- <plan.storyplan> <lineage.db>
//        [--snapshots <dir>] [--gdoc-snapshots] [--out <attribution.csv>] [--exclude-story Paratext]
//        [--k 6] [--R 8] [--G 6] [--verbatim-coverage 0.90] [--paste-scale 16] [--max-sources 0] [--min-words 4]
//        [--exclude-aistudio 22,23,24,25]
//        [--sample <n> [--sample-labels a,b,c] [--sample-flips <m>] [--seed 40] [--sample-out <file.md>]]
//            (--sample-flips m: append every echo candidate plus m random PlanFirst flips)
//        [--verdicts <sheet.md>[,<sheet2.md>]]
//        [--render <dir> --arcs "1-5,6-9,..." --subjects 1,2,3 --subject-types "Deferred for a plot point,..." --manifest <read-manifest.md>]
//
// Labels describe the note's structural relationship to its sources (verbatim / edited-paste /
// framed-paste / fragment / phrase / none / short); Role says whose source (brian / model).
// Thresholds are provisional until the calibration gate.

var args_ = Environment.GetCommandLineArgs().Skip(1).ToList();
var flags = new HashSet<string> { "--gdoc-snapshots" };
string? Opt(string name) { int i = args_.IndexOf(name); return i >= 0 && i + 1 < args_.Count ? args_[i + 1] : null; }
bool Flag(string name) => args_.Contains(name);
var positional = new List<string>();
for (int i = 0; i < args_.Count; i++)
{
    if (args_[i].StartsWith("--")) { if (!flags.Contains(args_[i])) i++; continue; }
    positional.Add(args_[i]);
}
if (positional.Count < 2)
{
    Console.Error.WriteLine("usage: <plan.storyplan> <lineage.db> [options — see Program.cs header]");
    return 2;
}

var inv = CultureInfo.InvariantCulture;
var planPath = positional[0];
var lineagePath = positional[1];
int k = int.Parse(Opt("--k") ?? "6");
var labels = new LabelThresholds(
    int.Parse(Opt("--R") ?? "8"),
    int.Parse(Opt("--G") ?? "6"),
    double.Parse(Opt("--verbatim-coverage") ?? "0.90", inv),
    Opt("--paste-scale") is string ps ? int.Parse(ps) : null);
var settings = new Settings(labels, int.Parse(Opt("--max-sources") ?? "0"), int.Parse(Opt("--min-words") ?? "4"));
var snapshotsDir = Opt("--snapshots");
var outCsv = Opt("--out") ?? "attribution.csv";
var excludeStory = Opt("--exclude-story");
var excludedChats = new HashSet<int>((Opt("--exclude-aistudio") ?? string.Join(",", LineageReader.DefaultExcludedAiStudioChats))
    .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())));

void Log(string s) => Console.Error.WriteLine(s);
var sw = Stopwatch.StartNew();

Log($"voice attribution — k={k}, R={labels.R}, G={labels.G}, verbatim≥{labels.VerbatimCoverage}, paste-scale={labels.PasteWords}, max-sources={settings.MaxSources}");
Log($"plan: {planPath}");
var plan = new PlanReader(planPath);
Log($"  notes {plan.Notes.Count}, subjects {plan.Subjects.Count}, plot points {plan.PlotPoints.Count}, links {plan.Links.Count}, chapters {plan.Chapters.Count}");

Log($"lineage: {lineagePath}");
var index = new VoiceIndex(k);
var snapshots = snapshotsDir is not null || Flag("--gdoc-snapshots") ? new PlanSnapshotIndex(k) : null;
LineageReader.Load(lineagePath, index, snapshots, Flag("--gdoc-snapshots"), excludedChats, Log);
Log($"  index: {index.SourceCount} sources, {index.ShingleCount:N0} distinct shingles  [{sw.Elapsed:m\\:ss}]");

if (snapshotsDir is not null)
{
    Log($"snapshots: {snapshotsDir}");
    var loaded = SnapshotReader.Load(snapshotsDir, snapshots!, Log);
    Log($"  {loaded.Count} snapshots, {(loaded.Count > 0 ? $"{loaded.Min(l => l.Date):yyyy-MM-dd} → {loaded.Max(l => l.Date):yyyy-MM-dd}" : "")}  [{sw.Elapsed:m\\:ss}]");
}

using var ctx = new SourceContext(lineagePath);
Log("matching…");
var rows = Attribution.Run(plan, index, snapshots, settings, ctx.Fetch, Log);
Log($"  done  [{sw.Elapsed:m\\:ss}]");
Log("");
Log(Attribution.Summary(rows, excludeStory));

// The CSV is the evidence set for the analysis, so an excluded story is excluded from it too —
// not merely hidden in the views. (Brian, 2026-09-02: Paratext is not scene content.)
var population = excludeStory is null ? rows : rows.Where(r => !r.Story.Contains(excludeStory, StringComparison.OrdinalIgnoreCase)).ToList();
Outputs.WriteCsv(outCsv, population);
Log($"wrote {outCsv} ({population.Count} rows{(excludeStory is null ? "" : $"; {rows.Count - population.Count} {excludeStory} rows excluded")})");

if (Opt("--sample") is string sampleN)
{
    int n = int.Parse(sampleN), seed = int.Parse(Opt("--seed") ?? "40");
    var sampleLabels = (Opt("--sample-labels") ?? string.Join(",", VoiceLabel.All.Where(l => l != VoiceLabel.Short)))
        .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
    var samplePath = Opt("--sample-out") ?? Path.Combine(Path.GetDirectoryName(Path.GetFullPath(outCsv))!, "calibration-sample.md");
    int flips = int.Parse(Opt("--sample-flips") ?? "0");
    Outputs.WriteSample(samplePath, population, sampleLabels, n, seed, flips, settings, k, ctx);
    Log($"wrote {samplePath} ({n} per label: {string.Join(", ", sampleLabels)}; seed {seed}{(flips > 0 ? $"; + all echoes + {flips} flips" : "")})");
}

if (Opt("--verdicts") is string verdictFiles)
{
    var byId = rows.ToDictionary(r => r.Note.Id);
    foreach (var f in verdictFiles.Split(',', StringSplitOptions.RemoveEmptyEntries))
    {
        var verdicts = Verdicts.Parse(File.ReadAllText(f.Trim()));
        Log(Verdicts.Matrix(verdicts, byId, Path.GetFileName(f.Trim())));
    }
}

if (Opt("--render") is string renderDir)
{
    var arcs = new List<(string, int, int)>();
    foreach (var spec in (Opt("--arcs") ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries))
    {
        var parts = spec.Split('-');
        arcs.Add(($"arc-{spec.Trim()}", int.Parse(parts[0]), int.Parse(parts[1])));
    }
    var subjectIds = new HashSet<int>((Opt("--subjects") ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim())));
    foreach (var type in (Opt("--subject-types") ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()))
        foreach (var s in plan.Subjects.Values.Where(s => s.SubjectType.Equals(type, StringComparison.OrdinalIgnoreCase)))
            subjectIds.Add(s.Id);
    var manifestPath = Opt("--manifest") ?? Path.Combine(renderDir, "read-manifest.md");
    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(manifestPath))!);
    using (var manifest = new StreamWriter(manifestPath, false, new System.Text.UTF8Encoding(false)))
        Outputs.Render(renderDir, plan, rows, arcs, subjectIds, excludeStory, manifest);
    Log($"rendered {renderDir} ({arcs.Count} arcs, {subjectIds.Count} subjects); manifest {manifestPath}");
}

Log($"total {sw.Elapsed:m\\:ss}");
return 0;
