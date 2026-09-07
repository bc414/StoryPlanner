using System;
using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;

namespace StoryPlanner.Tests;

/// <summary>
/// The example block of each format file is the fixture its checker is tested against
/// (decisions.md, 2026-09-06: the example blocks are fixtures; 2026-09-07: each format is its
/// own file). This reads the first fenced block of <c>formats/&lt;id&gt;.md</c> from the real
/// skill folder, so a format edited without its checker fails a test here. It is the one place
/// the tests read the real folder for content rather than for a verdict.
/// </summary>
public static class FormatExamples
{
    public static string RepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
            dir = dir.Parent;
        return dir?.FullName ?? throw new InvalidOperationException("no repository root above the test assembly");
    }

    /// <summary>The governed skill folder: v3-buildout once its SKILL.md holds an Artifacts table, v3-buildout-2 until then.</summary>
    public static string SkillFolder()
    {
        var root = RepoRoot();
        foreach (var name in new[] { "v3-buildout", "v3-buildout-2" })
        {
            var folder = Path.Combine(root, ".claude", "skills", name);
            if (SkillReader.HasArtifactsTable(Path.Combine(folder, "SKILL.md"))) return folder;
        }
        throw new InvalidOperationException("no skill folder whose SKILL.md holds an Artifacts table");
    }

    /// <summary>The first fenced block of <c>formats/&lt;id&gt;.md</c>, with LF line endings.</summary>
    public static string Block(string formatId)
    {
        var path = Path.Combine(SkillFolder(), SkillReader.FormatsFolder, formatId + ".md");
        if (!File.Exists(path)) throw new InvalidOperationException($"no format file {path}");
        var lines = File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');
        var open = Array.FindIndex(lines, l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        if (open < 0) throw new InvalidOperationException($"{formatId}.md has no fenced example");
        var close = Array.FindIndex(lines, open + 1, l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        if (close < 0) throw new InvalidOperationException($"{formatId}.md: the fence is never closed");
        return string.Join('\n', lines.Skip(open + 1).Take(close - open - 1)) + "\n";
    }
}
