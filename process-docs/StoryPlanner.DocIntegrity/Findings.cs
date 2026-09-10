using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// schemas/findings-schema.md (d-2026-09-09-13 to -16): a verification's analysis. The engine
/// holds the shape; this class holds the rules the schema's Checks name: the title and the
/// study, the entry headings, the question token against the study's corpus and its
/// directions' frontmatter, the citations against the batch's directions and index, the
/// supersedes chain, the withdrawn lines, and the shortcoming parts. It also reads a file
/// for the state render: which questions its standing findings answer.
/// </summary>
public static class FindingsChecker
{
    public const string SchemaId = "findings-schema";
    public const string VerificationPrefix = "verification-of-";
    public static readonly string[] Parts = ["item", "itemizer", "directions", "calibration", "execution", "corpus"];
    static readonly Regex WithdrawnLine = new(@"^- withdrawn: (?<date>\d{4}-\d{2}-\d{2}) (?<what>\S.*)$", RegexOptions.Compiled);
    static readonly Regex TallyCite = new(@"^(?<study>[a-z0-9-]+)/(?<batch>[0-9]{2}-[a-z0-9-]+) § (?<field>[a-z][a-z0-9 ]*)$", RegexOptions.Compiled);
    static readonly Regex ItemCite = new(@"^(?<study>[a-z0-9-]+)/(?<batch>[0-9]{2}-[a-z0-9-]+)/(?<item>[a-z0-9-]+)$", RegexOptions.Compiled);
    static readonly Regex ShortcomingLine = new(@"^(?<part>[a-z]+):\s*\S", RegexOptions.Compiled);

    /// <summary>One finding as read: its slug, the question it names if any, what it supersedes if anything, and whether it is withdrawn.</summary>
    public sealed record Entry(string Slug, int Line, string? Question, string? Supersedes, bool Withdrawn);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path) => Read(ctx, path).Findings;

    /// <summary>The entries of one findings file in order, and every finding against the schema.</summary>
    public static (IReadOnlyList<Entry> Entries, IReadOnlyList<Finding> Findings) Read(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var studyDir = Path.GetDirectoryName(Path.GetFullPath(path))!;
        var study = Path.GetFileName(studyDir)!;
        var findings = new List<Finding>();
        var entries = new List<Entry>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return (entries, findings); }
        foreach (var p in engine.Problems)
        {
            if (p.Key == "withdrawn") continue; // the appended line, held below
            var id = p.Key == "question" ? "findings.question"
                : p.Key == "supersedes" ? "findings.supersedes"
                : p.Section == "Findings" && p.Kind is ProblemKind.Missing or ProblemKind.Unknown or ProblemKind.Order or ProblemKind.Type or ProblemKind.Form or ProblemKind.Duplicate ? "findings.entry"
                : "findings.shape";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var doc = engine.Document;

        // ---- title and study ----
        var title = $"{study} — findings";
        if (doc.Title != title)
            findings.Add(Finding.Fail("findings.title", file, $"the title is '# {title}', the id of the study whose folder holds the file"));
        if (!study.StartsWith(VerificationPrefix, StringComparison.Ordinal))
            findings.Add(Finding.Fail("findings.title", file, $"'{study}' is not a verification's id; findings belong to a verification"));
        var corpus = CorpusOf(study, ctx.CorporaIds);

        // ---- the batches under the study, for citations and the frozen questions ----
        var batches = new Dictionary<string, (DirectionsFile? Directions, IReadOnlyList<string> Items)>(StringComparer.Ordinal);
        var frozen = new HashSet<string>(StringComparer.Ordinal);
        var batchesDir = Path.Combine(studyDir, "batches");
        if (Directory.Exists(batchesDir))
        {
            foreach (var dir in Directory.GetDirectories(batchesDir))
            {
                var definitionPath = Path.Combine(dir, "definition.md");
                DirectionsFile? directions = null;
                IReadOnlyList<string> items = [];
                if (File.Exists(definitionPath))
                {
                    var definition = DefinitionFile.Read(definitionPath);
                    if (definition.DirectionsPath is { } dp && File.Exists(dp))
                    {
                        directions = DirectionsFile.Read(dp);
                        foreach (var q in directions.Questions) frozen.Add(q);
                    }
                    if (File.Exists(definition.IndexPath))
                        items = IndexFile.Parse(File.ReadAllText(definition.IndexPath)).Rows.Select(r => r.Item).ToList();
                }
                batches[Path.GetFileName(dir)] = (directions, items);
            }
        }

        // ---- the withdrawn lines, by position ----
        var lines = SchemaCheckers.Lines(path);
        var positions = doc.Entries.Where(e => e.Section == "Findings").ToList();
        var withdrawnOf = new Dictionary<int, int>();
        for (var i = 0; i < lines.Length; i++)
        {
            if (!lines[i].StartsWith("- withdrawn:", StringComparison.Ordinal)) continue;
            var owner = positions.LastOrDefault(e => e.Line < i + 1);
            if (!WithdrawnLine.IsMatch(lines[i]))
                findings.Add(Finding.Fail("findings.withdrawn", file, $"line {i + 1}: a withdrawn line is '- withdrawn: YYYY-MM-DD <what the results or the items showed>'"));
            if (owner is null) { findings.Add(Finding.Fail("findings.withdrawn", file, $"line {i + 1}: a withdrawn line sits beneath a finding's fields")); continue; }
            withdrawnOf[owner.Line] = withdrawnOf.GetValueOrDefault(owner.Line) + 1;
        }
        foreach (var (line, count) in withdrawnOf.Where(kv => kv.Value > 1))
            findings.Add(Finding.Fail("findings.withdrawn", file, $"line {line}: withdrawn appears twice; a finding is withdrawn once"));

        // ---- the entries ----
        var slugs = new Dictionary<string, int>(StringComparer.Ordinal);
        var superseded = new Dictionary<string, int>(StringComparer.Ordinal);
        var arr = doc.Root["Findings"] as JsonArray ?? [];
        for (var i = 0; i < arr.Count; i++)
        {
            var obj = (JsonObject)arr[i]!;
            var pos = i < positions.Count ? positions[i] : null;
            var line = pos?.Line ?? 0;
            var heading = obj[DocumentReader.HeadingProperty]?.GetValue<string>() ?? "";
            var slash = heading.IndexOf('/');
            var prefix = slash < 0 ? "" : heading[..slash];
            var slug = slash < 0 ? heading : heading[(slash + 1)..];
            if (prefix != study)
                findings.Add(Finding.Fail("findings.entry", file, $"line {line}: the heading is '{study}/<slug>', the id of the study whose folder holds the file then the slug; found '{heading}'"));
            else if (!ClosedSets.IdPattern.IsMatch(slug))
                findings.Add(Finding.Fail("findings.entry", file, $"line {line}: '{slug}' is not a lowercase slug"));
            else if (!slugs.TryAdd(slug, line))
                findings.Add(Finding.Fail("findings.entry", file, $"line {line}: '{slug}' repeats a slug in this file"));

            var question = obj["question"]?.GetValue<string>();
            if (question is not null)
            {
                var qLine = pos?.FieldLines.GetValueOrDefault("question", line) ?? line;
                var qSlash = question.IndexOf('/');
                var qCorpus = qSlash < 0 ? question : question[..qSlash];
                if (corpus is not null && qCorpus != corpus)
                    findings.Add(Finding.Fail("findings.question", file, $"line {qLine}: '{question}' is not a question of the study's corpus '{corpus}'"));
                if (batches.Count > 0 && !frozen.Contains(question))
                    findings.Add(Finding.Fail("findings.question", file, $"line {qLine}: '{question}' is not in the frontmatter of the directions the study's batches name; a finding answers a frozen question or names none"));
            }

            var supersedes = obj["supersedes"]?.GetValue<string>();
            if (supersedes is not null)
            {
                var sLine = pos?.FieldLines.GetValueOrDefault("supersedes", line) ?? line;
                var sSlug = supersedes.StartsWith(study + "/", StringComparison.Ordinal) ? supersedes[(study.Length + 1)..] : null;
                if (sSlug is null || !slugs.TryGetValue(sSlug, out var target) || target >= line)
                    findings.Add(Finding.Fail("findings.supersedes", file, $"line {sLine}: '{supersedes}' names no earlier finding in this file"));
                else if (withdrawnOf.ContainsKey(target))
                    findings.Add(Finding.Fail("findings.supersedes", file, $"line {sLine}: '{supersedes}' is withdrawn; a withdrawn finding is not amended"));
                else if (!superseded.TryAdd(sSlug, line))
                    findings.Add(Finding.Fail("findings.supersedes", file, $"line {sLine}: '{supersedes}' is superseded twice"));
                else if (withdrawnOf.ContainsKey(line))
                    findings.Add(Finding.Fail("findings.withdrawn", file, $"line {line}: a finding that supersedes another is not itself withdrawn in the same review; withdraw the superseding entry's successor instead"));
            }

            var cites = obj["cites"] is JsonArray c ? c.Select(x => x?.ToString() ?? "").ToList() : [];
            var cLine = pos?.FieldLines.GetValueOrDefault("cites", line) ?? line;
            if (obj["cites"] is not null && cites.Count == 0)
                findings.Add(Finding.Fail("findings.cites", file, $"line {cLine}: cites holds at least one line, a tally section or an item"));
            foreach (var cite in cites)
            {
                var t = TallyCite.Match(cite);
                var it = ItemCite.Match(cite);
                if (!t.Success && !it.Success)
                { findings.Add(Finding.Fail("findings.cites", file, $"line {cLine}: '{cite}' is neither '<study>/<batch> § <field>' nor '<study>/<batch>/<item>'")); continue; }
                var m = t.Success ? t : it;
                if (m.Groups["study"].Value != study)
                { findings.Add(Finding.Fail("findings.cites", file, $"line {cLine}: '{cite}' cites a batch outside this study")); continue; }
                if (!batches.TryGetValue(m.Groups["batch"].Value, out var batch))
                { findings.Add(Finding.Fail("findings.cites", file, $"line {cLine}: '{cite}' cites no batch under this study")); continue; }
                if (t.Success)
                {
                    var field = t.Groups["field"].Value;
                    if (batch.Directions is { } d && !d.Output.Any(o => o.Key == field && o.Kind == OutputKind.Enum))
                        findings.Add(Finding.Fail("findings.cites", file, $"line {cLine}: '{field}' is not an enum field of that batch's directions"));
                }
                else if (batch.Items.Count > 0 && !batch.Items.Contains(it.Groups["item"].Value, StringComparer.Ordinal))
                    findings.Add(Finding.Fail("findings.cites", file, $"line {cLine}: '{it.Groups["item"].Value}' is not an item of that batch's index"));
            }

            entries.Add(new Entry(slug, line, question, supersedes, withdrawnOf.ContainsKey(line)));
        }
        foreach (var (line, _) in withdrawnOf.Where(kv => superseded.Values.Any(l => l == kv.Key) == false && positions.Any(p => p.Line == kv.Key)))
        {
            // a withdrawn entry that a later entry supersedes: the successor amended it, so it is not also withdrawn
            var slug = entries.FirstOrDefault(e => e.Line == line)?.Slug;
            if (slug is not null && superseded.ContainsKey(slug))
                findings.Add(Finding.Fail("findings.withdrawn", file, $"line {line}: '{slug}' is withdrawn and superseded; one or the other"));
        }

        // ---- shortcomings ----
        if (doc.Root["Shortcomings"] is JsonArray shortcomings)
        {
            foreach (var s in shortcomings)
            {
                var text = s?.ToString() ?? "";
                var m = ShortcomingLine.Match(text);
                if (!m.Success || !Parts.Contains(m.Groups["part"].Value, StringComparer.Ordinal))
                    findings.Add(Finding.Fail("findings.shortcoming", file, $"a Shortcomings line is '<part>: <what the results showed>', the part one of {string.Join(", ", Parts)}; found '{Truncate(text)}'"));
            }
        }

        return (entries, findings.DistinctBy(f => (f.CheckId, f.Message)).ToList());
    }

    /// <summary>The questions a file's standing findings name, for the state render; a finding withdrawn or superseded answers nothing.</summary>
    public static IReadOnlyList<string> AnsweredQuestions(string text)
    {
        var entries = ReadEntries(text);
        var superseded = entries.Where(e => e.Supersedes is not null).Select(e => e.Supersedes!).ToHashSet(StringComparer.Ordinal);
        return entries.Where(e => !e.Withdrawn && e.Question is not null && !superseded.Contains(e.Slug)).Select(e => e.Question!).Distinct(StringComparer.Ordinal).ToList();
    }

    /// <summary>A light read of the Findings entries by their lines, for derivation; the checker reads through the engine.</summary>
    public static IReadOnlyList<Entry> ReadEntries(string text)
    {
        var lines = text.Replace("\r\n", "\n").Split('\n');
        var entries = new List<Entry>();
        var inFindings = false;
        string? slug = null; string? question = null; string? supersedes = null; var withdrawn = false; var line = 0;
        void Flush() { if (slug is not null) entries.Add(new Entry(slug, line, question, supersedes, withdrawn)); slug = null; question = null; supersedes = null; withdrawn = false; }
        for (var i = 0; i < lines.Length; i++)
        {
            var l = lines[i];
            if (l.StartsWith("## ", StringComparison.Ordinal)) { Flush(); inFindings = l[3..].Trim() == "Findings"; continue; }
            if (!inFindings) continue;
            if (l.StartsWith("### ", StringComparison.Ordinal))
            {
                Flush();
                var heading = l[4..].Trim();
                var slash = heading.IndexOf('/');
                slug = slash < 0 ? heading : heading[(slash + 1)..];
                line = i + 1;
                continue;
            }
            if (slug is null) continue;
            if (l.StartsWith("- question:", StringComparison.Ordinal)) question = l["- question:".Length..].Trim();
            else if (l.StartsWith("- supersedes:", StringComparison.Ordinal)) supersedes = l["- supersedes:".Length..].Trim();
            else if (l.StartsWith("- withdrawn:", StringComparison.Ordinal)) withdrawn = true;
        }
        Flush();
        return entries;
    }

    /// <summary>The corpus a verification's id names, the longest corpus id its remainder starts with; null when none is known.</summary>
    static string? CorpusOf(string study, IReadOnlySet<string> corpora)
    {
        if (!study.StartsWith(VerificationPrefix, StringComparison.Ordinal)) return null;
        var rest = study[VerificationPrefix.Length..];
        return corpora.Where(c => rest == c || rest.StartsWith(c + "-", StringComparison.Ordinal)).OrderByDescending(c => c.Length).FirstOrDefault();
    }

    static string Truncate(string s) => s.Length <= 60 ? s : s[..60] + "…";
}
