using System.Text;
using StoryPlanner.BatchFiles;
using StoryPlanner.MarkdownItemizer;

// The Markdown itemizer: cuts one document into units by the unit rule and writes a batch's
// index and item bodies into its folder. The audit's itemizer (the skill's own text is the
// corpus); any Markdown document a study itemizes by paragraph, list item and table row.
//
//   dotnet run --project tools/StoryPlanner.MarkdownItemizer -- <document.md> <batch-folder> [--corpus skill]
//
// The batch folder holds the definition a session wrote; this writes index.md beside it and the
// bodies under items/. An index already present is refused: an itemizer runs once per batch.

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: MarkdownItemizer <document.md> <batch-folder> [--corpus <id>]");
    return 2;
}
var doc = Path.GetFullPath(args[0]);
var batchDir = Path.GetFullPath(args[1]);
var corpusIdx = Array.IndexOf(args, "--corpus");
var corpus = corpusIdx >= 0 && corpusIdx + 1 < args.Length ? args[corpusIdx + 1] : "skill";
if (!File.Exists(doc)) { Console.Error.WriteLine($"Document not found: {doc}"); return 2; }
if (!Directory.Exists(batchDir)) { Console.Error.WriteLine($"Batch folder not found: {batchDir}"); return 2; }
var indexPath = Path.Combine(batchDir, "index.md");
if (File.Exists(indexPath)) { Console.Error.WriteLine($"{indexPath} exists — an itemizer runs once per batch; a new cut is a new batch."); return 2; }

var text = File.ReadAllText(doc);
var units = UnitSplitter.Split(text);
var itemsDir = Path.Combine(batchDir, "items");
Directory.CreateDirectory(itemsDir);
foreach (var u in units)
    File.WriteAllText(Path.Combine(itemsDir, u.Id + ".md"), UnitSplitter.RenderItem(u), new UTF8Encoding(false));

var version = typeof(UnitSplitter).Assembly.GetName().Version?.ToString() ?? "0";
var index = IndexFile.Render(
    Path.GetFileName(batchDir),
    $"tools/StoryPlanner.MarkdownItemizer, {version}, {Path.GetFileName(doc)}",
    corpus,
    "the unit's ordinal in the document and the heading of the section it sits in, `<n> in <heading>`; the body carries the section then the unit's text verbatim",
    Hashing.Sha256Hex(text),
    units.Select(u => (u.Id, UnitSplitter.Locator(u), UnitSplitter.Description(u))));
File.WriteAllText(indexPath, index, new UTF8Encoding(false));
Console.WriteLine($"{units.Count} units from {doc} → {batchDir} (index.md and items/)");
foreach (var g in units.GroupBy(u => u.Section))
    Console.WriteLine($"  {g.Count(),3}  {g.Key}");
return 0;
