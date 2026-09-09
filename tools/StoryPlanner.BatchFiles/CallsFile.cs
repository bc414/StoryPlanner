using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace StoryPlanner.BatchFiles;

/// <summary>One call as the calls file holds it: the item, the call's number, and what the runner recorded about it.</summary>
public sealed record CallEntry(
    string Item,
    int Call,
    string Model,
    string? Effort,
    string Harness,
    string DirectionsHash,
    string ItemHash,
    string PromptHash,
    string Started,
    string Ended,
    int Exit,
    string Check,
    double? Cost,
    int? Turns,
    string? SessionId,
    bool Pilot)
{
    public const string Ok = "ok";
    public bool Succeeded => Exit == 0 && Check == Ok;
}

/// <summary>
/// The batch's <c>calls.md</c>: a title, a head line recording the definition's hash at the
/// first execution, then one entry per call in the keyed-line grammar, appended by the runner
/// and never edited. The batch's state is read from it: an item with a successful call has
/// its result; every other item is called by the next execution.
/// </summary>
public sealed class CallsFile
{
    static readonly Regex Heading = new(@"^### (?<item>[a-z0-9-]+) — call (?<n>\d+)$", RegexOptions.Compiled);
    public static readonly string[] Keys = ["model", "effort", "harness", "directions hash", "item hash", "prompt hash", "started", "ended", "exit", "check", "cost", "turns", "session", "pilot"];

    public string? Title { get; }
    public string? DefinitionHash { get; }
    public IReadOnlyList<CallEntry> Entries { get; }
    public IReadOnlyList<string> Problems { get; }

    CallsFile(string? title, string? definitionHash, IReadOnlyList<CallEntry> entries, IReadOnlyList<string> problems)
    { Title = title; DefinitionHash = definitionHash; Entries = entries; Problems = problems; }

    public static CallsFile Empty => new(null, null, [], []);

    public static CallsFile Read(string path) => File.Exists(path) ? Parse(File.ReadAllText(path)) : Empty;

    public bool HasSucceeded(string item) => Entries.Any(e => e.Item == item && e.Succeeded);
    public int CallsOf(string item) => Entries.Count(e => e.Item == item);
    public int Executions => Entries.Count == 0 ? 0 : Entries.Max(e => e.Call);

    public static CallsFile Parse(string text)
    {
        var lines = Hashing.NormalizeNewlines(text).Split('\n');
        var problems = new List<string>();
        string? title = null, definitionHash = null;
        var entries = new List<CallEntry>();
        var i = 0;
        while (i < lines.Length && lines[i].Trim().Length == 0) i++;
        if (i < lines.Length && lines[i].StartsWith("# ", StringComparison.Ordinal)) { title = lines[i][2..].Trim(); i++; }
        var headLines = new List<string>();
        var headStart = i;
        while (i < lines.Length && !lines[i].StartsWith("### ", StringComparison.Ordinal)) { headLines.Add(lines[i]); i++; }
        var head = KeyedLines.Read(headLines, headStart + 1);
        definitionHash = head.Value("definition");
        foreach (var s in head.Stray) problems.Add($"line {s.Line}: a line outside the head and the entries");

        string? item = null;
        var call = 0;
        var entryStart = 0;
        var body = new List<string>();
        void Flush()
        {
            if (item is null) return;
            var block = KeyedLines.Read(body, entryStart + 1);
            foreach (var s in block.Stray) problems.Add($"line {s.Line}: neither a keyed line nor a continuation");
            string V(string k) => block.Value(k) ?? "";
            entries.Add(new CallEntry(item, call, V("model"), block.Value("effort"), V("harness"), V("directions hash"), V("item hash"), V("prompt hash"),
                V("started"), V("ended"), int.TryParse(V("exit"), out var exit) ? exit : -1, V("check"),
                double.TryParse(V("cost"), NumberStyles.Float, CultureInfo.InvariantCulture, out var cost) ? cost : null,
                int.TryParse(V("turns"), out var turns) ? turns : null, block.Value("session"), V("pilot") == "yes"));
            item = null;
            body = [];
        }
        for (; i < lines.Length; i++)
        {
            var m = Heading.Match(lines[i]);
            if (m.Success)
            {
                Flush();
                item = m.Groups["item"].Value;
                call = int.Parse(m.Groups["n"].Value);
                entryStart = i + 1;
                continue;
            }
            if (lines[i].StartsWith("###", StringComparison.Ordinal)) { problems.Add($"line {i + 1}: an entry heading is '### <item> — call <n>'"); Flush(); continue; }
            body.Add(lines[i]);
        }
        Flush();
        return new CallsFile(title, definitionHash, entries, problems);
    }

    /// <summary>The head a first execution writes: title and the definition's hash.</summary>
    public static string RenderHead(string batch, string definitionHash)
        => $"# {batch} — calls\n\n{KeyedLines.RenderLine("definition", definitionHash)}\n";

    public static string RenderEntry(CallEntry e)
    {
        var sb = new StringBuilder();
        sb.Append("\n### ").Append(e.Item).Append(" — call ").Append(e.Call).Append("\n\n");
        sb.Append(KeyedLines.RenderLine("model", e.Model)).Append('\n');
        if (e.Effort is not null) sb.Append(KeyedLines.RenderLine("effort", e.Effort)).Append('\n');
        sb.Append(KeyedLines.RenderLine("harness", e.Harness)).Append('\n');
        sb.Append(KeyedLines.RenderLine("directions hash", e.DirectionsHash)).Append('\n');
        sb.Append(KeyedLines.RenderLine("item hash", e.ItemHash)).Append('\n');
        sb.Append(KeyedLines.RenderLine("prompt hash", e.PromptHash)).Append('\n');
        sb.Append(KeyedLines.RenderLine("started", e.Started)).Append('\n');
        sb.Append(KeyedLines.RenderLine("ended", e.Ended)).Append('\n');
        sb.Append(KeyedLines.RenderLine("exit", e.Exit.ToString(CultureInfo.InvariantCulture))).Append('\n');
        sb.Append(KeyedLines.RenderLine("check", e.Check)).Append('\n');
        if (e.Cost is { } c) sb.Append(KeyedLines.RenderLine("cost", c.ToString("F4", CultureInfo.InvariantCulture))).Append('\n');
        if (e.Turns is { } t) sb.Append(KeyedLines.RenderLine("turns", t.ToString(CultureInfo.InvariantCulture))).Append('\n');
        if (e.SessionId is not null) sb.Append(KeyedLines.RenderLine("session", e.SessionId)).Append('\n');
        sb.Append(KeyedLines.RenderLine("pilot", e.Pilot ? "yes" : "no")).Append('\n');
        return sb.ToString();
    }
}
