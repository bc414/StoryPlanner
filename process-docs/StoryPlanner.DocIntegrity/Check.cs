namespace StoryPlanner.DocIntegrity;

/// <summary>
/// The <c>check</c> verb: everything governed at or under one path, following the artifacts
/// tables. A governed skill folder gets the validator's checks of the method's shape; a
/// governed file gets its class's schema; a folder gets every skill folder and every governed
/// file under it, so the repository root is the whole set and a narrower folder bounds the
/// check. A single file inside a skill folder is checked the way the hook checks a write to
/// it: the folder's shape, then the file's own schema if its class has one. Findings on
/// governed files name the file by its repo-relative path.
/// </summary>
public static class Check
{
    public sealed record Result(ValidationReport Report, IReadOnlyList<string> SkillFolders, IReadOnlyList<string> GovernedFiles);

    public static Result Run(string repoRoot, string path)
    {
        var full = Path.GetFullPath(path);
        if (File.Exists(full)) return CheckFile(repoRoot, full);
        if (Directory.Exists(full)) return CheckFolder(repoRoot, full);
        throw new MapFormatException($"no such file or folder: {full}", "folder.missing");
    }

    static Result CheckFile(string repoRoot, string full)
    {
        var findings = new List<Finding>();
        var folders = new List<string>();
        var files = new List<string>();

        var folder = GovernedSkill.Locate(full);
        if (folder is not null)
        {
            findings.AddRange(Validator.Validate(folder).Findings);
            folders.Add(folder);
        }

        var governed = ArtifactScope.Locate(repoRoot, full)
                       ?? ReferenceScope.Locate(repoRoot, full)
                       ?? ReferenceScope.LocateResult(repoRoot, full);
        if (governed is not null)
        {
            findings.AddRange(CheckOne(governed.Checker, governed.Context, repoRoot, full));
            files.Add(full);
        }
        return new Result(new ValidationReport(findings), folders, files);
    }

    static Result CheckFolder(string repoRoot, string full)
    {
        var findings = new List<Finding>();
        var folders = GovernedSkill.All(repoRoot).Where(f => IsUnder(f, full)).ToList();
        foreach (var folder in folders)
            findings.AddRange(Validator.Validate(folder).Findings);

        var classes = ArtifactScope.CheckedClasses(repoRoot).ToList();
        var contexts = new Dictionary<string, CheckContext>(StringComparer.Ordinal);
        var files = new List<string>();
        foreach (var c in classes)
        {
            if (!contexts.TryGetValue(c.SkillFolder, out var ctx))
                contexts[c.SkillFolder] = ctx = CheckContext.From(repoRoot, c.SkillFolder);
            foreach (var file in c.Files())
            {
                if (!IsUnder(file, full) || files.Contains(file, StringComparer.Ordinal)) continue;
                files.Add(file);
                findings.AddRange(CheckOne(c.Checker, ctx, repoRoot, file));
            }
        }

        // Governance by reference: every file a definition reaches (its directions, wherever
        // they sit) and every batch's results, held to the declaration its directions make.
        foreach (var (skillFolder, ctx) in contexts)
            foreach (var (target, checker) in ReferenceScope.Targets(ctx))
            {
                if (!IsUnder(target, full)) continue;
                if (checker == (SchemaChecker)Directions.Check && files.Contains(target, StringComparer.OrdinalIgnoreCase)) continue;
                if (!files.Contains(target, StringComparer.OrdinalIgnoreCase)) files.Add(target);
                findings.AddRange(CheckOne(checker, ctx, repoRoot, target));
            }

        // A class with a checker that a checked skill folder's table gives no in-repo file
        // pattern for is reported, never failed: nothing can be checked for it.
        foreach (var folder in folders)
            foreach (var id in SchemaCheckers.CheckedIds)
                if (!classes.Any(c => c.SkillFolder == folder && c.Row.Id == id))
                    findings.Add(Finding.Info("check.no-row", id,
                        $"{Path.GetFileName(folder)}: no artifact row with a parseable in-repo path; nothing checked for this class"));

        return new Result(new ValidationReport(findings.DistinctBy(f => (f.CheckId, f.RowId, f.Message)).ToList()), folders, files);
    }

    static IEnumerable<Finding> CheckOne(SchemaChecker checker, CheckContext ctx, string repoRoot, string file)
    {
        var rel = Path.GetRelativePath(repoRoot, file).Replace('\\', '/');
        IReadOnlyList<Finding> findings;
        try { findings = checker(ctx, file); }
        catch (MapFormatException ex) { findings = [Finding.Fail(ex.CheckId, rel, ex.Message)]; }
        return findings.Select(f => f with { RowId = rel });
    }

    static bool IsUnder(string path, string folder)
    {
        var rel = Path.GetRelativePath(folder, path);
        if (rel == ".") return true;
        return !Path.IsPathRooted(rel) && rel != ".."
               && !rel.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal)
               && !rel.StartsWith("../", StringComparison.Ordinal);
    }
}
