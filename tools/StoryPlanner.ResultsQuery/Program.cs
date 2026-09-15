using StoryPlanner.ResultsQuery;

// A query engine over an exploration batch's results (2026-09-15, built inside
// exploration-of-technique-mechanism-goal-co-occurrence under building-a-tool). Every verb
// prints to stdout and writes nothing; the first line printed is the query's canonical string,
// which a lead carries and `run` or the page re-runs.
//
//   ResultsQuery columns <definition.md>
//   ResultsQuery run <definition.md> "<query string>"
//   ResultsQuery <view> <definition.md> [--field f] [--where col=regex ...] [--story s | --item i]
//                [--sample n [--seed s]] [--col c] [--n 1|2|3] [--position k] [--exclude regex] [--top n] [--group sep]
//   ResultsQuery serve <definition.md> [--url http://127.0.0.1:5191]
//
// views: list, cites, terms, pairs, by-story, sort, health. `terms` over one column with a
// filter on another is the cross-column table.

if (args.Length < 2) return Usage();
var verb = args[0];
var definitionPath = Path.GetFullPath(args[1]);
if (!File.Exists(definitionPath)) { Console.Error.WriteLine($"No definition at {definitionPath}"); return 2; }

try
{
    switch (verb)
    {
        case "columns": return Columns();
        case "run": return Run();
        case "serve": return await Serve();
        default:
            if (!Query.Views.Contains(verb)) return Usage();
            return View(verb);
    }
}
catch (InvalidOperationException e) { Console.Error.WriteLine(e.Message); return 2; }

int Usage()
{
    Console.Error.WriteLine("Usage:\n" +
        "  ResultsQuery columns <definition.md>\n" +
        "  ResultsQuery run <definition.md> \"<query string>\"\n" +
        "  ResultsQuery <list|cites|terms|pairs|by-story|sort|health> <definition.md> [--field f] [--where col=regex ...] [--story s | --item i]\n" +
        "               [--sample n [--seed s]] [--col c] [--n 1|2|3] [--position k] [--exclude regex] [--top n] [--group sep]\n" +
        "  ResultsQuery serve <definition.md> [--url http://127.0.0.1:5191]");
    return 2;
}

string? Opt(string name)
{
    for (var i = 2; i + 1 < args.Length; i++) if (args[i] == name) return args[i + 1];
    return null;
}

int Columns()
{
    var batch = LoadedBatch.Load(definitionPath, Opt("--group") ?? LoadedBatch.DefaultGroupSeparator);
    Console.WriteLine($"batch {batch.BatchId}: {batch.Items} item(s), {batch.Answered} answered, {batch.Malformed.Count} malformed, {batch.Missing.Count} missing; {batch.Stories.Count} stories");
    if (batch.Fields.Count == 0) { Console.WriteLine("no bar-part field: no list of line field's text declares parts separated by ' | '"); return 0; }
    foreach (var f in batch.Fields)
    {
        var lines = batch.Lines.Count(l => l.Field == f.Key);
        Console.WriteLine($"\nfield {f.Key}: {f.Columns.Count} columns, {lines} line(s), {batch.Arity.Count(a => a.Field == f.Key)} arity problem(s)");
        foreach (var c in f.Columns) Console.WriteLine($"  {c.Position}  {c.Id,-12} {c.Label}");
    }
    return 0;
}

int Run()
{
    if (args.Length < 3) return Usage();
    var (q, error) = Query.Parse(args[2]);
    if (q is null) { Console.Error.WriteLine(error); return 2; }
    var batch = LoadedBatch.Load(definitionPath, q.Group ?? LoadedBatch.DefaultGroupSeparator);
    return Print(batch, q);
}

int View(string view)
{
    var group = Opt("--group");
    var batch = LoadedBatch.Load(definitionPath, group ?? LoadedBatch.DefaultGroupSeparator);
    var field = Opt("--field") ?? batch.Fields.FirstOrDefault()?.Key;
    if (field is null) { Console.Error.WriteLine("the batch has no bar-part field"); return 2; }
    var filters = new List<Filter>();
    for (var i = 2; i + 1 < args.Length; i++)
        if (args[i] == "--where")
        {
            var eq = args[i + 1].IndexOf('=');
            if (eq <= 0) { Console.Error.WriteLine($"--where takes col=regex, not '{args[i + 1]}'"); return 2; }
            filters.Add(new Filter(args[i + 1][..eq], args[i + 1][(eq + 1)..]));
        }
    int? Int(string name) => Opt(name) is { } s ? (int.TryParse(s, out var x) ? x : throw new InvalidOperationException($"{name} takes a number")) : null;
    var q = new Query(batch.BatchId, batch.Answered, field, filters, Opt("--story"), Opt("--item"), Int("--sample"), Int("--seed"), view,
        Opt("--col"), Int("--n"), Int("--position"), Opt("--exclude"), Int("--top"), group);
    return Print(batch, q);
}

int Print(LoadedBatch batch, Query q)
{
    var (result, error) = Views.Run(batch, q);
    if (result is null) { Console.Error.WriteLine(error); return 2; }
    Console.Out.Write(result.Render());
    return 0;
}

async Task<int> Serve()
{
    var url = Opt("--url") ?? "http://127.0.0.1:5191";
    var source = new BatchSource(definitionPath);
    var app = Head.BuildApp(source, url);
    Console.WriteLine($"serving {source.Load().BatchId} on {url}; the batch is re-read on every query");
    await app.RunAsync();
    return 0;
}
