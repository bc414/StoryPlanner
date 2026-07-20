using ConversationSplitter;

// ─── Usage ────────────────────────────────────────────────────────────────────
// ConversationSplitter <claude-json> <gemini-folder> <output-folder>
//
// <claude-json>    path to conversations.json
// <gemini-folder>  folder containing Gemini AI Studio *.json files (Selected_Chats/)
// <output-folder>  where NNN_*_content.json, index.md, index.json are written
// ─────────────────────────────────────────────────────────────────────────────

if (args.Length < 3)
{
    Console.Error.WriteLine("Usage: ConversationSplitter <claude-json> <gemini-folder> <output-folder>");
    return 1;
}

string claudeFile    = args[0];
string geminiFolder  = args[1];
string outputFolder  = args[2];

if (!File.Exists(claudeFile))    { Console.Error.WriteLine($"Not found: {claudeFile}");   return 1; }
if (!Directory.Exists(geminiFolder)) { Console.Error.WriteLine($"Not found: {geminiFolder}"); return 1; }

Directory.CreateDirectory(outputFolder);

// ─── Parse ────────────────────────────────────────────────────────────────────

Console.WriteLine("=== Parsing Claude conversations ===");
var claudeConvs = ClaudeParser.Parse(claudeFile);

Console.WriteLine();
Console.WriteLine("=== Parsing Gemini AI Studio conversations ===");
var geminiConvs = GeminiParser.ParseFolder(geminiFolder);

// ─── Sort and assign indices ───────────────────────────────────────────────────

var all = claudeConvs.Concat(geminiConvs)
    .OrderBy(c => ParseDate(c.ConversationDate))
    .ThenBy(c => c.Title, StringComparer.OrdinalIgnoreCase)
    .ToList();

int idx = 1;
foreach (var conv in all)
    conv.SourceIndex = idx++;

// ─── Write content files ───────────────────────────────────────────────────────

Console.WriteLine();
Console.WriteLine($"=== Writing {all.Count} content files to {outputFolder} ===");

var indexEntries = new List<IndexEntry>();

foreach (var conv in all)
{
    string fileName = ContentFileWriter.Write(conv, outputFolder);

    // First non-empty user block for the preview
    string firstPrompt = conv.Blocks
        .FirstOrDefault(b => b.Speaker == "user" && !string.IsNullOrWhiteSpace(b.RawContent))
        ?.RawContent ?? string.Empty;
    // Trim to 200 chars for the index
    if (firstPrompt.Length > 200)
        firstPrompt = firstPrompt[..200];

    indexEntries.Add(new IndexEntry
    {
        Index       = conv.SourceIndex,
        ContentFile = fileName,
        Platform    = conv.Platform,
        Title       = conv.Title,
        Date        = conv.ConversationDate,
        BlockCount  = conv.Blocks.Count,
        FirstPrompt = firstPrompt
    });

    Console.WriteLine($"  [{conv.SourceIndex:D3}] {conv.Platform,-7} {FormatDate(conv.ConversationDate)}  {conv.Blocks.Count,4} blocks  {conv.Title}");
}

// ─── Write index ───────────────────────────────────────────────────────────────

Console.WriteLine();
Console.WriteLine("=== Writing index ===");
IndexWriter.Write(indexEntries, outputFolder);

Console.WriteLine();
Console.WriteLine($"Done. {all.Count} conversations written to {Path.GetFullPath(outputFolder)}.");
return 0;

// ─── Helpers ──────────────────────────────────────────────────────────────────

static DateTime ParseDate(string iso)
{
    if (DateTime.TryParse(iso, out var dt)) return dt;
    return DateTime.MinValue;
}

static string FormatDate(string iso)
{
    if (DateTime.TryParse(iso, out var dt)) return dt.ToString("yyyy-MM-dd");
    return iso;
}
