using System;
using System.IO;
using System.Linq;

namespace StoryPlanner.Tests;

/// <summary>
/// The example blocks of <c>artifacts.md</c> are the fixtures the record checkers are tested
/// against (decisions.md, 2026-09-06: "artifacts.md stays one file, and its example blocks are
/// fixtures"). This reads the first fenced block under a format heading from the real skill
/// folder, so a format edited without its checker fails a test here. It is the one place the
/// tests read the real folder for content rather than for a verdict.
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

    /// <summary>The governed skill folder: v3-buildout once it carries an artifacts table, v3-buildout-2 until then.</summary>
    public static string SkillFolder()
    {
        var root = RepoRoot();
        foreach (var name in new[] { "v3-buildout", "v3-buildout-2" })
        {
            var folder = Path.Combine(root, ".claude", "skills", name);
            if (File.Exists(Path.Combine(folder, "artifacts.md"))) return folder;
        }
        throw new InvalidOperationException("no skill folder with an artifacts.md");
    }

    /// <summary>The first fenced block under <c>## &lt;heading&gt;</c> in artifacts.md, with LF line endings.</summary>
    public static string Block(string heading)
    {
        var lines = File.ReadAllText(Path.Combine(SkillFolder(), "artifacts.md")).Replace("\r\n", "\n").Split('\n');
        var start = Array.FindIndex(lines, l => l.Trim() == "## " + heading);
        if (start < 0) throw new InvalidOperationException($"artifacts.md has no section '## {heading}'");
        var open = Array.FindIndex(lines, start + 1, l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        if (open < 0) throw new InvalidOperationException($"'## {heading}' has no fenced example");
        var close = Array.FindIndex(lines, open + 1, l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        if (close < 0) throw new InvalidOperationException($"'## {heading}': the fence is never closed");
        return string.Join('\n', lines.Skip(open + 1).Take(close - open - 1)) + "\n";
    }
}
