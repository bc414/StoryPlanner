namespace StoryPlanner.ProcessMap;

/// <summary>
/// Turns the tables of <c>process-map.md</c> into typed rows. Tables are identified by their
/// column signature, never by position or heading text, so a section may be renamed without
/// breaking the read. A table whose signature is not one of the five is a refusal, not a skip:
/// an unrecognised table in this file means the schema drifted.
/// </summary>
public static class MapReader
{
    static readonly string[] RootsCols = ["id", "kind", "root", "source"];
    static readonly string[] FilesCols = ["id", "path", "keep", "governed-by"];
    static readonly string[] ProcessCols =
        ["id", "level", "kind", "process", "actor", "inputs", "outputs", "roots", "governed-by", "state"];
    static readonly string[] EdgeCols = ["from", "to", "kind", "label"];
    static readonly string[] BootstrapCols = ["row", "retired by"];

    public static ProcessMapDocument Read(string markdown)
    {
        var tables = MapTables.ReadAll(markdown);

        MarkdownTable? roots = null, files = null, processes = null, edges = null, bootstrap = null;

        foreach (var t in tables)
        {
            var sig = t.Headers.Select(h => h.ToLowerInvariant()).ToArray();
            if (Match(sig, RootsCols)) Assign(ref roots, t, "Roots");
            else if (Match(sig, FilesCols)) Assign(ref files, t, "Files");
            else if (Match(sig, ProcessCols)) Assign(ref processes, t, "Processes");
            else if (Match(sig, EdgeCols)) Assign(ref edges, t, "Edges");
            else if (Match(sig, BootstrapCols)) Assign(ref bootstrap, t, "Bootstrap");
            else
                throw new MapFormatException(
                    $"line {t.HeaderLine}: a table with columns [{string.Join(" | ", t.Headers)}] " +
                    "matches no known schema. Refusing to guess what it is.");
        }

        Require(roots, "Roots");
        Require(files, "Files");
        Require(processes, "Processes");
        Require(edges, "Edges");
        Require(bootstrap, "Bootstrap rows and what retires them");

        return new ProcessMapDocument(
            roots!.Rows.Select(r => new RootRow(r.Cells[0], r.Cells[1], r.Cells[2], r.Cells[3], r.Line)).ToList(),
            files!.Rows.Select(r => new FileRow(r.Cells[0], r.Cells[1], r.Cells[2], r.Cells[3], r.Line)).ToList(),
            processes!.Rows.Select(r => new ProcessRow(
                r.Cells[0], r.Cells[1], r.Cells[2], r.Cells[3], r.Cells[4],
                Ids(r.Cells[5]), Ids(r.Cells[6]), Ids(r.Cells[7]),
                r.Cells[8], r.Cells[9], r.Line)).ToList(),
            edges!.Rows.Select(r => new EdgeRow(r.Cells[0], r.Cells[1], r.Cells[2], r.Cells[3], r.Line)).ToList(),
            bootstrap!.Rows.Select(r => new BootstrapRow(r.Cells[0], r.Cells[1], r.Line)).ToList());
    }

    static bool Match(string[] sig, string[] cols) => sig.SequenceEqual(cols);

    static void Assign(ref MarkdownTable? slot, MarkdownTable t, string name)
    {
        if (slot is not null)
            throw new MapFormatException(
                $"line {t.HeaderLine}: a second {name} table. There is one copy of the map.");
        slot = t;
    }

    static void Require(MarkdownTable? t, string name)
    {
        if (t is null) throw new MapFormatException($"no {name} table found.");
    }

    /// <summary>Space-separated id lists; an empty cell is an empty list, not a one-element one.</summary>
    static IReadOnlyList<string> Ids(string cell)
        => cell.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
