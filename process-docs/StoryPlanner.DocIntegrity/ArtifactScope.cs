namespace StoryPlanner.DocIntegrity;

/// <summary>A file resolved to the artifact class that governs it and the checker for that class.</summary>
public sealed record GovernedFile(string RepoRoot, string SkillFolder, ArtifactRow Row, SchemaChecker Checker)
{
    public CheckContext Context => CheckContext.From(RepoRoot, SkillFolder);
}

/// <summary>
/// One artifact class that has a checker, as one skill folder's table declares it, with its
/// path pattern resolved against that folder's actual name.
/// </summary>
public sealed record CheckedClass(string RepoRoot, string SkillFolder, ArtifactRow Row, ArtifactPath Path, SchemaChecker Checker)
{
    /// <summary>Every file on disk the class governs, repo-wide.</summary>
    public IReadOnlyList<string> Files() => StateBuilder.Matches(RepoRoot, Path, null, null);
}

/// <summary>
/// The artifacts table is the scope. A path is matched against every row's pattern in every
/// governed skill folder of its repository, placeholders as wildcards; the first row with a
/// checker wins; no match, or a match with no checker, is silence. The tool carries no path
/// list of its own: adding a row to the Artifacts table and a schema file is what puts a file
/// under the hook and under <c>check</c>.
///
/// A row whose pattern lies under <c>.claude/skills/&lt;name&gt;/</c> names the skill folder by
/// its post-swap name; it is matched against the governing folder's actual name, so the
/// corpora file is governed in <c>v3-buildout-2</c> today and in <c>v3-buildout</c> after.
/// </summary>
public static class ArtifactScope
{
    const string SkillsPrefix = ".claude/skills/";

    /// <summary>The class governing a file, with the repository root found above it; null outside a repository.</summary>
    public static GovernedFile? Locate(string filePath)
    {
        var full = Path.GetFullPath(filePath);
        var root = RepoLocator.FindRoot(full);
        return root is null ? null : Locate(root, full);
    }

    public static GovernedFile? Locate(string repoRoot, string filePath)
    {
        var rel = Path.GetRelativePath(repoRoot, Path.GetFullPath(filePath)).Replace('\\', '/');
        foreach (var c in CheckedClasses(repoRoot))
            if (c.Path.ToRegex().IsMatch(rel))
                return new GovernedFile(c.RepoRoot, c.SkillFolder, c.Row, c.Checker);
        return null;
    }

    /// <summary>
    /// Every class with a checker that every governed skill folder under the root declares, in
    /// folder then table order. A row whose path is no single pattern, a directory, or
    /// unparseable declares nothing here.
    /// </summary>
    public static IEnumerable<CheckedClass> CheckedClasses(string repoRoot)
    {
        foreach (var skillFolder in GovernedSkill.All(repoRoot))
        {
            IReadOnlyList<ArtifactRow> rows;
            try { rows = SkillReader.ReadArtifacts(skillFolder); }
            catch (MapFormatException) { continue; }

            var skillRel = Path.GetRelativePath(repoRoot, skillFolder).Replace('\\', '/').TrimEnd('/');
            foreach (var row in rows)
            {
                var checker = SchemaCheckers.For(row.Id);
                if (checker is null) continue;
                if (!ArtifactPath.TryParse(row.Path, out var ap, out _) || ap!.NoSinglePattern || ap.IsDirectory) continue;

                var pattern = ap.Pattern;
                if (pattern.StartsWith(SkillsPrefix, StringComparison.Ordinal))
                {
                    var slash = pattern.IndexOf('/', SkillsPrefix.Length);
                    if (slash < 0) continue;
                    pattern = skillRel + pattern[slash..];
                }
                yield return new CheckedClass(repoRoot, skillFolder, row, ap with { Pattern = pattern }, checker);
            }
        }
    }
}
