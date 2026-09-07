namespace StoryPlanner.DocIntegrity;

/// <summary>A file resolved to the artifact class that governs it and the checker for that class.</summary>
public sealed record GovernedFile(string RepoRoot, string SkillFolder, ArtifactRow Row, FormatChecker Checker)
{
    public CheckContext Context => CheckContext.From(RepoRoot, SkillFolder);
}

/// <summary>
/// The artifacts table is the scope. A written path is matched against every row's pattern in
/// every governed skill folder of its repository, placeholders as wildcards; the first row
/// with a checker wins; no match, or a match with no checker, is silence. The tool carries no
/// path list of its own: adding a row and a format to artifacts.md is what puts a file under
/// the hook.
///
/// A row whose pattern lies under <c>.claude/skills/&lt;name&gt;/</c> names the skill folder by
/// its post-swap name; it is matched against the governing folder's actual name, so the
/// corpora file is governed in <c>v3-buildout-2</c> today and in <c>v3-buildout</c> after.
/// </summary>
public static class ArtifactScope
{
    const string SkillsPrefix = ".claude/skills/";

    public static GovernedFile? Locate(string filePath)
    {
        var full = Path.GetFullPath(filePath);
        var root = RepoLocator.FindRoot(full);
        if (root is null) return null;
        var rel = Path.GetRelativePath(root, full).Replace('\\', '/');

        foreach (var skillFolder in GovernedSkill.All(root))
        {
            IReadOnlyList<ArtifactRow> rows;
            try { rows = SkillReader.ReadArtifacts(skillFolder); }
            catch (MapFormatException) { continue; }

            var skillRel = Path.GetRelativePath(root, skillFolder).Replace('\\', '/').TrimEnd('/');
            foreach (var row in rows)
            {
                if (!ArtifactPath.TryParse(row.Path, out var ap, out _) || ap!.OutsideRepo || ap.IsDirectory) continue;
                var pattern = ap.Pattern;
                if (pattern.StartsWith(SkillsPrefix, StringComparison.Ordinal))
                {
                    var slash = pattern.IndexOf('/', SkillsPrefix.Length);
                    if (slash < 0) continue;
                    pattern = skillRel + pattern[slash..];
                }
                var probe = ap with { Pattern = pattern };
                if (!probe.ToRegex().IsMatch(rel)) continue;
                var checker = FormatCheckers.For(row.Id);
                if (checker is null) continue;
                return new GovernedFile(root, skillFolder, row, checker);
            }
        }
        return null;
    }
}
