using System.Text.RegularExpressions;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// schemas/directions-schema.md on the engine: the frontmatter and its questions tokens, the
/// body's sections per study type, then the class's own rules through the shared reader — the
/// classes, the criteria's numbering, the What to produce lines, and the version number. The
/// study type is read from the folder the file sits in, or the referee folder.
/// </summary>
public static class Directions
{
    public const string SchemaId = "directions-schema";

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();
        var folder = Path.GetFileName(Path.GetDirectoryName(Path.GetFullPath(path))!);
        var kind = StudyKinds.OfFolder(folder);

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) findings.Add(EngineCheck.Unavailable(SchemaId, file));
        foreach (var p in engine.Problems)
        {
            var id = p.Section switch
            {
                "frontmatter" => "directions.frontmatter",
                DirectionsFile.ClassesSection => "directions.classes",
                DirectionsFile.CriteriaSection => "directions.criteria",
                DirectionsFile.WhatToProduce => "directions.output",
                _ => "directions.sections",
            };
            // The content of these three sections is held by the shared reader below, once.
            if (p.Section is DirectionsFile.ClassesSection or DirectionsFile.CriteriaSection or DirectionsFile.WhatToProduce && p.Kind != ProblemKind.Missing) continue;
            findings.Add(Finding.Fail(id, file, p.Message));
        }

        var d = DirectionsFile.Read(path);
        foreach (var p in d.Problems)
            findings.Add(Finding.Fail(p.Section switch
            {
                "frontmatter" => "directions.frontmatter",
                "classes" => "directions.classes",
                "criteria" => "directions.criteria",
                "output" => "directions.output",
                _ => "directions.sections",
            }, file, p.Message));

        if (d.HasTitle) findings.Add(Finding.Fail("directions.sections", file, "the file has no title line; the body begins at its first section"));
        if (!d.HasFrontmatter) findings.Add(Finding.Fail("directions.frontmatter", file, "no frontmatter between --- lines at the top"));

        if (kind is null)
            findings.Add(Finding.Info("directions.folder", file, $"'{folder}' is neither a study folder nor the referee folder; the study type is unknown and the sections are not held"));
        else
        {
            var wantsQuestions = StudyKinds.HasQuestions(kind.Value);
            var has = d.Frontmatter.ContainsKey("questions");
            if (wantsQuestions && !has) findings.Add(Finding.Fail("directions.frontmatter", file, $"a {kind.Value.ToString().ToLowerInvariant()}'s directions name the questions they freeze or read with"));
            if (!wantsQuestions && has) findings.Add(Finding.Fail("directions.frontmatter", file, $"the {kind.Value.ToString().ToLowerInvariant()}'s directions carry no questions line"));

            var expected = DirectionsFile.SectionsFor(kind.Value);
            var actual = d.SectionHeadings;
            var required = expected.Where(e => !e.Optional).Select(e => e.Heading).ToList();
            var allowed = expected.Select(e => e.Heading).ToList();
            var ordered = actual.Where(allowed.Contains).Select(h => allowed.IndexOf(h)).ToList();
            var ok = required.All(actual.Contains) && actual.All(allowed.Contains) && actual.Distinct().Count() == actual.Count
                     && ordered.Zip(ordered.Skip(1)).All(p => p.Second > p.First);
            if (!ok)
                findings.Add(Finding.Fail("directions.sections", file,
                    $"a {kind.Value.ToString().ToLowerInvariant()}'s sections are [{string.Join(", ", expected.Select(e => e.Optional ? e.Heading + "?" : e.Heading))}] in order; found [{string.Join(", ", actual)}]"));
        }

        if (d.HasClasses && !d.Classes.Any(c => c.Text.Contains("cannot", StringComparison.OrdinalIgnoreCase) || c.Label.Contains("cannot", StringComparison.Ordinal) || c.Label.Contains("unplaced", StringComparison.Ordinal)))
            findings.Add(Finding.Fail("directions.classes", file, "no class is reserved for an item the criteria cannot place"));
        if (d.Output.Count == 0 && !d.Problems.Any(p => p.Section == "output"))
            findings.Add(Finding.Fail("directions.output", file, "What to produce declares at least one field"));

        var n = DirectionsFile.VersionOf(file);
        if (n is null) findings.Add(Finding.Fail("directions.version", file, "the file is directions-N.md"));
        else
        {
            var siblings = Directory.GetFiles(Path.GetDirectoryName(Path.GetFullPath(path))!, "directions-*.md")
                .Select(f => DirectionsFile.VersionOf(Path.GetFileName(f))).Where(v => v is not null).Select(v => v!.Value).OrderBy(v => v).ToList();
            var expectedN = siblings.Where(v => v < n).DefaultIfEmpty(0).Max() + 1;
            if (n != expectedN) findings.Add(Finding.Fail("directions.version", file, $"the next version in this folder is {expectedN}; found {n}"));
        }
        return findings;
    }
}

/// <summary>schemas/index-schema.md on the engine: the title, the head, the table; the class's rules through the shared reader.</summary>
public static class BatchIndex
{
    public const string SchemaId = "index-schema";
    public static readonly string[] ExtraCorpora = ["candidates", "skill"];

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var batch = Path.GetFileName(Path.GetDirectoryName(Path.GetFullPath(path))!);
        var findings = new List<Finding>();
        var corpora = ctx.CorporaIds.Concat(ExtraCorpora).ToList();
        var engine = EngineCheck.Run(SchemaId, ctx, path, new Dictionary<string, IReadOnlyCollection<string>> { ["corpus"] = corpora });
        if (engine.ShapeUnavailable) findings.Add(EngineCheck.Unavailable(SchemaId, file));
        foreach (var p in engine.Problems)
        {
            if (ctx.CorporaIds.Count == 0 && p.Key == "corpus" && p.Kind == ProblemKind.Type) continue; // no corpus ids to hold it to
            var id = p.Section == "head" ? "index.head"
                : p.Key == "item" ? "index.item"
                : p.Key == "locator" ? "index.locator"
                : "index.table";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var index = IndexFile.Read(path);
        var expected = $"{batch} — index";
        if (index.Title != expected)
            findings.Add(Finding.Fail("index.title", file, $"the title is '# {expected}'; found " + (index.Title is null ? "no title" : $"'{index.Title}'")));

        foreach (var p in index.Problems.Where(p => p.Part is "item" or "locator"))
            findings.Add(Finding.Fail("index." + p.Part, file, p.Message));
        if (ctx.CorporaIds.Count == 0)
            findings.Add(Finding.Info("index.corpora-unavailable", file, "no corpus ids could be read from the skill folder; the corpus is held to candidates and skill only"));
        return findings.DistinctBy(f => (f.CheckId, f.Message)).ToList();
    }
}

/// <summary>
/// schemas/definition-schema.md on the engine, then the class's rules: the batch folder's
/// form and number, the title, the directions resolving and passing their schema, kind and
/// calibration agreeing with the directions, the study's one model, and the file unchanged
/// since the calls file recorded its hash. Checking a definition checks its targets: this is
/// where a referee's directions and calibrations are governed, by reference.
/// </summary>
public static class Definition
{
    public const string SchemaId = "definition-schema";
    static readonly Regex BatchName = new(@"^(?<n>[0-9]{2})-(?<slug>[a-z0-9-]+)$", RegexOptions.Compiled);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();
        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) findings.Add(EngineCheck.Unavailable(SchemaId, file));
        foreach (var p in engine.Problems)
        {
            var id = p.Key switch
            {
                "directions" when p.Kind == ProblemKind.Reference => "definition.directions",
                "calibration" when p.Kind == ProblemKind.Reference => "definition.calibration",
                _ => "definition.fields",
            };
            findings.Add(Finding.Fail(id, file, p.Message));
        }

        var d = DefinitionFile.Read(path);
        foreach (var p in d.Problems) findings.Add(Finding.Fail("definition.fields", file, p.Message));

        // ---- the folder ----
        var m = BatchName.Match(d.Batch);
        var batchesDir = Path.GetDirectoryName(d.BatchDir)!;
        if (!m.Success || Path.GetFileName(batchesDir) != "batches")
            findings.Add(Finding.Fail("definition.batch", file, $"the folder is batches/<nn>-<slug>; found '{Path.GetFileName(batchesDir)}/{d.Batch}'"));
        else
        {
            var n = int.Parse(m.Groups["n"].Value);
            var siblings = Directory.GetDirectories(batchesDir).Select(Path.GetFileName).Select(x => BatchName.Match(x!)).Where(x => x.Success).ToList();
            var numbers = siblings.Select(x => int.Parse(x.Groups["n"].Value)).OrderBy(x => x).ToList();
            var expected = numbers.Where(x => x < n).DefaultIfEmpty(0).Max() + 1;
            if (n != expected) findings.Add(Finding.Fail("definition.batch", file, $"the next batch number in this study is {expected:00}; found {n:00}"));
            if (siblings.Count(x => x.Groups["slug"].Value == m.Groups["slug"].Value) > 1)
                findings.Add(Finding.Fail("definition.batch", file, $"the slug '{m.Groups["slug"].Value}' repeats in this study"));
        }
        var expectedTitle = $"{d.Batch} — definition";
        if (d.Title != expectedTitle)
            findings.Add(Finding.Fail("definition.title", file, $"the title is '# {expectedTitle}'; found " + (d.Title is null ? "no title" : $"'{d.Title}'")));

        // ---- the directions, by reference ----
        DirectionsFile? directions = null;
        if (d.DirectionsPath is not null && File.Exists(d.DirectionsPath))
        {
            var targetFindings = Directions.Check(ctx, d.DirectionsPath);
            var rel = Path.GetRelativePath(ctx.RepoRoot, d.DirectionsPath).Replace('\\', '/');
            foreach (var f in targetFindings.Where(f => f.Level == FindingLevel.Failure))
                findings.Add(Finding.Fail("definition.directions", file, $"{rel} fails {f.CheckId}: {f.Message}"));
            directions = DirectionsFile.Read(d.DirectionsPath);
        }

        // ---- kind and calibration ----
        if (directions is not null)
        {
            if (directions.HasClasses && d.Kind is null)
                findings.Add(Finding.Fail("definition.fields", file, "kind is present when the directions have Classes: sample or full"));
            if (!directions.HasClasses && d.Kind is not null)
                findings.Add(Finding.Fail("definition.fields", file, "kind is absent when the directions have no Classes"));
        }
        var calibrationLine = d.Fields.Field("calibration");
        if (d.Kind == "full" && calibrationLine is null)
            findings.Add(Finding.Fail("definition.calibration", file, "a batch of kind full names the accepting calibration of its directions"));
        if (d.Kind != "full" && calibrationLine is not null)
            findings.Add(Finding.Fail("definition.calibration", file, "only a batch of kind full names a calibration"));
        if (d.CalibrationPath is not null && File.Exists(d.CalibrationPath) && directions is not null)
        {
            var cal = CalibrationFile.Read(d.CalibrationPath);
            var rel = Path.GetRelativePath(ctx.RepoRoot, d.CalibrationPath).Replace('\\', '/');
            if (!cal.TitleParsed)
                findings.Add(Finding.Fail("definition.calibration", file, $"{rel}: the title is '# Calibration — directions-N@<hash> — <date>'"));
            else if (!Hashing.Cites(cal.Hash!, directions.BodyHash))
                findings.Add(Finding.Fail("definition.calibration", file, $"{rel} judges directions at {cal.Hash}; the named directions' body hash is {directions.BodyHash[..12]}…"));
            if (!cal.Accepted)
                findings.Add(Finding.Fail("definition.calibration", file, $"{rel}: the verdict does not accept the version"));
        }

        // ---- the study's one model ----
        if (d.Model is not null && Directory.Exists(batchesDir))
            foreach (var other in Directory.GetDirectories(batchesDir).Select(dir => Path.Combine(dir, "definition.md")).Where(p => File.Exists(p) && Path.GetFullPath(p) != d.Path))
            {
                var o = DefinitionFile.Read(other);
                if (o.Model is not null && o.Model != d.Model)
                    findings.Add(Finding.Fail("definition.model", file, $"{Path.GetFileName(Path.GetDirectoryName(other))} names model '{o.Model}'; a study has one model"));
            }

        // ---- frozen ----
        if (File.Exists(d.CallsPath))
        {
            var calls = CallsFile.Read(d.CallsPath);
            if (calls.DefinitionHash is not null && calls.DefinitionHash != d.Hash)
                findings.Add(Finding.Fail("definition.frozen", file, $"the file's hash is {d.Hash[..12]}…; calls.md recorded {calls.DefinitionHash[..Math.Min(12, calls.DefinitionHash.Length)]}… at the first execution"));
        }
        return findings.DistinctBy(f => (f.CheckId, f.Message)).ToList();
    }

    /// <summary>Every definition file the Artifacts table locates, for the reference scope.</summary>
    public static IReadOnlyList<string> All(CheckContext ctx) => References.FilesOf(WellKnown.Definition, ctx) ?? [];
}

/// <summary>
/// <c>results.declared</c> (decisions.md, "Results are the runner's"): every result file of a
/// batch, held to the declaration its directions make, reached through the definition; a
/// result for an item the index does not list fails too. Run by <c>check</c> over a batch
/// folder and by the hook on a write into <c>results/</c>.
/// </summary>
public static class Results
{
    public const string CheckId = "results.declared";

    public static IReadOnlyList<Finding> CheckBatch(CheckContext ctx, string definitionPath)
    {
        var findings = new List<Finding>();
        var d = DefinitionFile.Read(definitionPath);
        if (!Directory.Exists(d.ResultsDir)) return findings;
        if (d.DirectionsPath is null || !File.Exists(d.DirectionsPath))
        {
            findings.Add(Finding.Fail(CheckId, Rel(ctx, d.ResultsDir), "the batch's definition names no directions that resolve, so its results cannot be held to a declaration"));
            return findings;
        }
        var directions = DirectionsFile.Read(d.DirectionsPath);
        var index = File.Exists(d.IndexPath) ? IndexFile.Read(d.IndexPath) : null;
        foreach (var result in Directory.GetFiles(d.ResultsDir, "*.md").OrderBy(f => f, StringComparer.Ordinal))
        {
            var item = Path.GetFileNameWithoutExtension(result);
            var rel = Rel(ctx, result);
            if (index is not null && !index.Rows.Any(r => r.Item == item))
                findings.Add(Finding.Fail(CheckId, rel, $"no item '{item}' in the batch's index"));
            var (_, problems) = ResultFile.Parse(directions, File.ReadAllText(result));
            foreach (var p in problems) findings.Add(Finding.Fail(CheckId, rel, p));
        }
        return findings;
    }

    static string Rel(CheckContext ctx, string path) => Path.GetRelativePath(ctx.RepoRoot, path).Replace('\\', '/');
}
