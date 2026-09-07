namespace StoryPlanner.DocIntegrity;

/// <summary>
/// Where the repository is, from any path inside it. Artifact paths in the tables are
/// repo-relative, so every verb that resolves one needs the root; the CLI finds it by walking
/// up to <c>.git</c>, and <c>--repo</c> overrides that for a copy outside the repository.
/// </summary>
public static class RepoLocator
{
    /// <summary>The nearest ancestor of <paramref name="startPath"/> holding a <c>.git</c> entry, or null.</summary>
    public static string? FindRoot(string startPath)
    {
        var full = Path.GetFullPath(startPath);
        var dir = Directory.Exists(full) ? new DirectoryInfo(full) : new FileInfo(full).Directory;
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, ".git"))
                              && !File.Exists(Path.Combine(dir.FullName, ".git")))
            dir = dir.Parent;
        return dir?.FullName;
    }
}
