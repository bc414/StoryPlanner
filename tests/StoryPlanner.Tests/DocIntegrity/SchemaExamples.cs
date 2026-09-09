using System;
using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;

namespace StoryPlanner.Tests;

/// <summary>
/// The example block of each schema file is the fixture its checker is tested against
/// (decisions.md, 2026-09-06: the example blocks are fixtures; 2026-09-07: each schema is its
/// own file, <c>schemas/&lt;name&gt;-schema.md</c>). This reads the first fenced block of that
/// file from the real skill folder, so a schema edited without its checker fails a test here;
/// and since 2026-09-09 the engine reads a class's Shape from the same file, so a fixture skill
/// folder gets copies of the real schema files it names.
/// It is the one place the tests read the real folder for content rather than for a verdict.
/// </summary>
public static class SchemaExamples
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

    public static string SchemaPath(string schemaId) => Path.Combine(SkillFolder(), SkillReader.SchemasFolder, schemaId + ".md");

    /// <summary>The first fenced block of <c>schemas/&lt;schemaId&gt;.md</c>, with LF line endings.</summary>
    public static string Block(string schemaId)
    {
        var path = SchemaPath(schemaId);
        if (!File.Exists(path)) throw new InvalidOperationException($"no schema file {path}");
        var lines = File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');
        var open = Array.FindIndex(lines, l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        if (open < 0) throw new InvalidOperationException($"{schemaId}.md has no fenced example");
        var close = Array.FindIndex(lines, open + 1, l => l.TrimStart().StartsWith("```", StringComparison.Ordinal));
        if (close < 0) throw new InvalidOperationException($"{schemaId}.md: the fence is never closed");
        return string.Join('\n', lines.Skip(open + 1).Take(close - open - 1)) + "\n";
    }

    /// <summary>Copies the real schema files named into a fixture skill folder's schemas/, so the engine finds their Shapes.</summary>
    public static void CopyInto(string skillFolder, params string[] schemaIds)
    {
        var schemas = Path.Combine(skillFolder, SkillReader.SchemasFolder);
        Directory.CreateDirectory(schemas);
        foreach (var id in schemaIds)
            File.Copy(SchemaPath(id), Path.Combine(schemas, id + ".md"), overwrite: true);
    }
}
