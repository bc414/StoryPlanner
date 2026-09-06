namespace StoryPlanner.ProcessMap;

/// <summary>
/// Turns a skill folder's tables into typed rows: the Activities table in <c>SKILL.md</c>, the
/// Artifacts table in <c>artifacts.md</c>, and the Processes table at the head of each
/// activity file named by a router row. Tables are identified by their column signature,
/// never by position or heading text. A table whose signature is not the one its file is
/// allowed to hold is a refusal, not a skip: an unrecognised table in these files means the
/// schema drifted.
///
/// A router row whose activity file is missing yields no processes; that is the validator's
/// <c>ref.companion</c> finding, not a refusal here, because the terminus legitimately has
/// no file.
/// </summary>
public static class SkillReader
{
    static readonly string[] ActivityCols = ["id", "enables", "description"];
    static readonly string[] ProcessCols = ["id", "mode", "instruments", "reads", "writes", "state", "description"];
    static readonly string[] ArtifactCols = ["id", "path", "mutation", "format", "description"];

    public const string UnknownSignature = "table.unknown-signature";

    public static SkillDocument Read(string skillFolder)
    {
        var skillPath = Path.Combine(skillFolder, "SKILL.md");
        var artifactsPath = Path.Combine(skillFolder, "artifacts.md");
        if (!File.Exists(skillPath))
            throw new MapFormatException($"no SKILL.md in {skillFolder}", "skill.missing");
        if (!File.Exists(artifactsPath))
            throw new MapFormatException($"no artifacts.md in {skillFolder}", "artifacts.missing");

        var activities = Only(skillPath, ActivityCols, "Activities").Rows
            .Select(r => new ActivityRow(r.Cells[0], Ids(r.Cells[1]), r.Cells[2], "SKILL.md", r.Line))
            .ToList();

        var artifacts = Only(artifactsPath, ArtifactCols, "Artifacts").Rows
            .Select(r => new ArtifactRow(r.Cells[0], r.Cells[1], r.Cells[2], r.Cells[3], r.Cells[4], "artifacts.md", r.Line))
            .ToList();

        var processes = new List<ProcessRow>();
        foreach (var a in activities)
        {
            var file = Path.Combine(skillFolder, a.Id + ".md");
            if (!File.Exists(file)) continue;
            var name = a.Id + ".md";
            processes.AddRange(Only(file, ProcessCols, "Processes").Rows.Select(r => new ProcessRow(
                r.Cells[0], a.Id, r.Cells[1], Ids(r.Cells[2]), Ids(r.Cells[3]), Ids(r.Cells[4]),
                r.Cells[5], r.Cells[6], name, r.Line)));
        }

        var known = activities.Select(a => a.Id + ".md")
            .Concat(WellKnown.NonActivityFiles)
            .ToHashSet(StringComparer.Ordinal);
        var orphans = Directory.GetFiles(skillFolder, "*.md")
            .Select(Path.GetFileName)
            .Where(n => n is not null && !known.Contains(n))
            .Select(n => n!)
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToList();

        return new SkillDocument(skillFolder, activities, processes, artifacts, orphans);
    }

    /// <summary>The one table a file may hold. Any other signature, or a second copy, is a refusal.</summary>
    static MarkdownTable Only(string path, string[] cols, string name)
    {
        var file = Path.GetFileName(path);
        IReadOnlyList<MarkdownTable> tables;
        try
        {
            tables = MapTables.ReadAll(File.ReadAllText(path));
        }
        catch (MapFormatException ex)
        {
            throw new MapFormatException($"{file}: {ex.Message}", ex.RuleId);
        }

        MarkdownTable? found = null;
        foreach (var t in tables)
        {
            var sig = t.Headers.Select(h => h.ToLowerInvariant()).ToArray();
            if (!sig.SequenceEqual(cols))
                throw new MapFormatException(
                    $"{file}:{t.HeaderLine}: a table with columns [{string.Join(" | ", t.Headers)}] is not " +
                    $"the {name} table this file holds. Refusing to guess what it is.",
                    UnknownSignature);
            if (found is not null)
                throw new MapFormatException(
                    $"{file}:{t.HeaderLine}: a second {name} table. There is one copy of every table.",
                    UnknownSignature);
            found = t;
        }

        return found ?? throw new MapFormatException($"{file}: no {name} table found.");
    }

    /// <summary>Space-separated id lists; an empty cell is an empty list, not a one-element one.</summary>
    static IReadOnlyList<string> Ids(string cell)
        => cell.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
