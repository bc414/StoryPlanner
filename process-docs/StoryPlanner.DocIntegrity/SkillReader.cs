namespace StoryPlanner.DocIntegrity;

/// <summary>
/// Turns a skill folder's tables into typed rows: the Activities and Artifacts tables in
/// <c>SKILL.md</c>, and the Processes table at the head of each activity file named by a
/// router row. Tables are identified by their column signature, never by position or heading
/// text. A table whose signature is not one its file is allowed to hold is a refusal, not a
/// skip: an unrecognised table in these files means the schema drifted.
///
/// A router row whose activity file is missing yields no processes; that is the validator's
/// <c>ref.companion</c> finding, not a refusal here, because the terminus legitimately has
/// no file. A format is the file <c>formats/&lt;id&gt;.md</c> the Artifacts table's
/// <c>format</c> column names; a file there that no row names is listed as an orphan.
/// </summary>
public static class SkillReader
{
    static readonly string[] ActivityCols = ["id", "enables", "description"];
    static readonly string[] ProcessCols = ["id", "mode", "instruments", "reads", "writes", "state", "description"];
    static readonly string[] ArtifactCols = ["id", "path", "mutation", "format", "description"];

    public const string UnknownSignature = "table.unknown-signature";
    public const string FormatsFolder = "formats";

    public static SkillDocument Read(string skillFolder)
    {
        var skillPath = Path.Combine(skillFolder, "SKILL.md");
        if (!File.Exists(skillPath))
            throw new MapFormatException($"no SKILL.md in {skillFolder}", "skill.missing");

        var (activitiesTable, artifactsTable) = Router(skillPath);

        var activities = activitiesTable.Rows
            .Select(r => new ActivityRow(r.Cells[0], Ids(r.Cells[1]), r.Cells[2], "SKILL.md", r.Line))
            .ToList();

        var artifacts = artifactsTable.Rows
            .Select(r => new ArtifactRow(r.Cells[0], r.Cells[1], r.Cells[2], r.Cells[3], r.Cells[4], "SKILL.md", r.Line))
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

        var formatIds = artifacts.Select(a => a.Format).Where(f => f.Length > 0).ToHashSet(StringComparer.Ordinal);
        var formatsDir = Path.Combine(skillFolder, FormatsFolder);
        var orphanFormats = Directory.Exists(formatsDir)
            ? Directory.GetFiles(formatsDir, "*.md")
                .Select(Path.GetFileName)
                .Where(n => n is not null && !formatIds.Contains(Path.GetFileNameWithoutExtension(n)))
                .Select(n => n!)
                .OrderBy(n => n, StringComparer.Ordinal)
                .ToList()
            : [];

        return new SkillDocument(skillFolder, activities, processes, artifacts, orphans, orphanFormats);
    }

    /// <summary>The Artifacts table alone, for scope resolution that needs no activity file.</summary>
    public static IReadOnlyList<ArtifactRow> ReadArtifacts(string skillFolder)
    {
        var skillPath = Path.Combine(skillFolder, "SKILL.md");
        if (!File.Exists(skillPath))
            throw new MapFormatException($"no SKILL.md in {skillFolder}", "skill.missing");
        var (_, artifactsTable) = Router(skillPath);
        return artifactsTable.Rows
            .Select(r => new ArtifactRow(r.Cells[0], r.Cells[1], r.Cells[2], r.Cells[3], r.Cells[4], "SKILL.md", r.Line))
            .ToList();
    }

    /// <summary>
    /// Whether a SKILL.md holds an Artifacts table: the shape that makes a skill folder
    /// governed. A file that does not parse holds none.
    /// </summary>
    public static bool HasArtifactsTable(string skillPath)
    {
        if (!File.Exists(skillPath)) return false;
        try
        {
            return MapTables.ReadAll(File.ReadAllText(skillPath)).Any(t => Signature(t).SequenceEqual(ArtifactCols));
        }
        catch (MapFormatException)
        {
            return false;
        }
    }

    /// <summary>SKILL.md's two tables, each exactly once; any other signature, or a second copy, is a refusal.</summary>
    static (MarkdownTable Activities, MarkdownTable Artifacts) Router(string skillPath)
    {
        MarkdownTable? activities = null, artifacts = null;
        foreach (var t in ReadTables(skillPath))
        {
            var sig = Signature(t);
            if (sig.SequenceEqual(ActivityCols)) activities = Once(activities, t, "Activities");
            else if (sig.SequenceEqual(ArtifactCols)) artifacts = Once(artifacts, t, "Artifacts");
            else
                throw new MapFormatException(
                    $"SKILL.md:{t.HeaderLine}: a table with columns [{string.Join(" | ", t.Headers)}] is neither the " +
                    "Activities nor the Artifacts table. Refusing to guess what it is.",
                    UnknownSignature);
        }
        if (activities is null) throw new MapFormatException("SKILL.md: no Activities table found.");
        if (artifacts is null) throw new MapFormatException("SKILL.md: no Artifacts table found.", "artifacts.missing");
        return (activities, artifacts);
    }

    static MarkdownTable Once(MarkdownTable? found, MarkdownTable t, string name)
        => found is null
            ? t
            : throw new MapFormatException(
                $"SKILL.md:{t.HeaderLine}: a second {name} table. There is one copy of every table.", UnknownSignature);

    /// <summary>The one table an activity file may hold. Any other signature, or a second copy, is a refusal.</summary>
    static MarkdownTable Only(string path, string[] cols, string name)
    {
        var file = Path.GetFileName(path);
        MarkdownTable? found = null;
        foreach (var t in ReadTables(path))
        {
            if (!Signature(t).SequenceEqual(cols))
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

    static IReadOnlyList<MarkdownTable> ReadTables(string path)
    {
        try
        {
            return MapTables.ReadAll(File.ReadAllText(path));
        }
        catch (MapFormatException ex)
        {
            throw new MapFormatException($"{Path.GetFileName(path)}: {ex.Message}", ex.RuleId);
        }
    }

    static string[] Signature(MarkdownTable t) => t.Headers.Select(h => h.ToLowerInvariant()).ToArray();

    /// <summary>Space-separated id lists; an empty cell is an empty list, not a one-element one.</summary>
    static IReadOnlyList<string> Ids(string cell)
        => cell.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
