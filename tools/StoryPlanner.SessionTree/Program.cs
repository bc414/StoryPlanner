using System.Text;
using StoryPlanner.SessionTree;

// Rebuilds a fork family from codesessions.db as one Markdown document — the stretches forked
// sessions share once, each branch once under the stretch it continues, dialogue only. Any
// member's id (or an unambiguous prefix) names the family.
//
//   dotnet run --project tools/StoryPlanner.SessionTree -- <session-id-or-prefix> [--db PATH] [--out FILE]
//
// Without --out the document goes to stdout; with it, the header (members, tree, warnings) is
// printed instead. Read-only against the archive; the disk is read only to disclose what the
// last ingest has not yet seen.

Console.OutputEncoding = new UTF8Encoding(false);

string? Option(string name)
{
    var i = Array.IndexOf(args, name);
    return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
}

var dbPath = Option("--db") ?? FamilyReader.DefaultDb;
var outPath = Option("--out");
var positional = args.Where((a, i) => !a.StartsWith("--") && (i == 0 || args[i - 1] is not ("--db" or "--out"))).ToList();

if (positional.Count != 1)
{
    Console.Error.WriteLine("Usage: dotnet run --project tools/StoryPlanner.SessionTree -- <session-id-or-prefix> [--db PATH] [--out FILE]");
    return 2;
}
if (!File.Exists(dbPath))
{
    Console.Error.WriteLine($"Archive not found: {dbPath}");
    return 2;
}

using var conn = FamilyReader.Open(dbPath);

var matches = FamilyReader.Resolve(conn, positional[0]);
if (matches.Count != 1)
{
    Console.Error.WriteLine(matches.Count == 0
        ? $"No main session in the archive starts with \"{positional[0]}\". A session newer than the last ingest needs the ingest first."
        : $"\"{positional[0]}\" matches {matches.Count} sessions — give more of the id:\n  " + string.Join("\n  ", matches));
    return 1;
}

var family = FamilyReader.Load(conn, matches[0]);
var disk = FamilyReader.CheckDisk(family, FamilyReader.DefaultProjectsRoot);
var tree = ForkTree.Build(family.Members.Select(m => (m.SessionId, m.Records)).ToList());
var rendered = TreeRenderer.Render(family, tree, disk, dbPath);

if (outPath is null)
{
    Console.Write(rendered.Document);
    return 0;
}

File.WriteAllText(outPath, rendered.Document, new UTF8Encoding(false));
Console.Write(rendered.Outline);
Console.WriteLine($"Wrote {outPath} — {rendered.Document.Length:N0} chars.");
return 0;
