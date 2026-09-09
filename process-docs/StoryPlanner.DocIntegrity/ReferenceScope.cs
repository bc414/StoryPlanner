using StoryPlanner.BatchFiles;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// Governance by reference (decisions.md, "A file is governed by its path or by reference"):
/// a file no Artifacts row matches is governed when a governed file's declared reference
/// resolves to it. The references declared today are a definition's <c>directions</c> and
/// <c>calibration</c> lines; the hook searches every definition for one that resolves to the
/// written file and checks it as that target, and <c>check .</c> walks every reference.
/// </summary>
public static class ReferenceScope
{
    /// <summary>The class and checker a file is governed under by reference, or null when no definition names it.</summary>
    public static GovernedFile? Locate(string repoRoot, string filePath)
    {
        var full = Path.GetFullPath(filePath);
        foreach (var skillFolder in GovernedSkill.All(repoRoot))
        {
            var ctx = CheckContext.From(repoRoot, skillFolder);
            IReadOnlyList<ArtifactRow> rows;
            try { rows = SkillReader.ReadArtifacts(skillFolder); }
            catch (MapFormatException) { continue; }
            foreach (var definitionPath in Definition.All(ctx))
            {
                DefinitionFile d;
                try { d = DefinitionFile.Read(definitionPath); }
                catch (IOException) { continue; }
                if (Same(d.DirectionsPath, full))
                {
                    var row = rows.FirstOrDefault(r => r.Id == WellKnown.Directions);
                    if (row is not null) return new GovernedFile(repoRoot, skillFolder, row, Directions.Check);
                }
                if (Same(d.CalibrationPath, full))
                {
                    // A calibration has no checker of its own yet; the definition that names it holds the three together.
                    var row = rows.FirstOrDefault(r => r.Id == WellKnown.Calibration);
                    if (row is not null) return new GovernedFile(repoRoot, skillFolder, row, (c, _) => Definition.Check(c, definitionPath));
                }
            }
        }
        return null;
    }

    /// <summary>A write into a batch's results folder: held to the batch's directions through its definition.</summary>
    public static GovernedFile? LocateResult(string repoRoot, string filePath)
    {
        var full = Path.GetFullPath(filePath);
        var dir = Path.GetDirectoryName(full);
        if (dir is null || Path.GetFileName(dir) != "results") return null;
        var definition = Path.Combine(Path.GetDirectoryName(dir)!, "definition.md");
        if (!File.Exists(definition)) return null;
        foreach (var skillFolder in GovernedSkill.All(repoRoot))
        {
            IReadOnlyList<ArtifactRow> rows;
            try { rows = SkillReader.ReadArtifacts(skillFolder); }
            catch (MapFormatException) { continue; }
            var row = rows.FirstOrDefault(r => r.Id == WellKnown.Results);
            if (row is null) continue;
            return new GovernedFile(repoRoot, skillFolder, row, (c, _) => Results.CheckBatch(c, definition));
        }
        return null;
    }

    /// <summary>Every file reached by reference from the definitions the table locates, with the checker for each, for <c>check .</c>.</summary>
    public static IEnumerable<(string Path, SchemaChecker Checker)> Targets(CheckContext ctx)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var definitionPath in Definition.All(ctx))
        {
            DefinitionFile d;
            try { d = DefinitionFile.Read(definitionPath); }
            catch (IOException) { continue; }
            if (d.DirectionsPath is not null && File.Exists(d.DirectionsPath) && seen.Add(d.DirectionsPath))
                yield return (d.DirectionsPath, Directions.Check);
            yield return (definitionPath, (c, _) => Results.CheckBatch(c, definitionPath));
        }
    }

    static bool Same(string? a, string b) => a is not null && string.Equals(Path.GetFullPath(a), b, StringComparison.OrdinalIgnoreCase);
}
