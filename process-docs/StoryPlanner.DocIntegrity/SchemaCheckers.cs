using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// What a schema checker sees besides the file: the repository root and the governing skill
/// folder. Built once per check by <see cref="ArtifactScope"/>; tests build it directly.
/// </summary>
public sealed record CheckContext(string RepoRoot, string SkillFolder)
{
    public static CheckContext From(string repoRoot, string skillFolder) => new(repoRoot, skillFolder);
}

/// <summary>One artifact class's schema, applied to one governed file. Findings name the file as their row.</summary>
public delegate IReadOnlyList<Finding> SchemaChecker(CheckContext ctx, string path);

/// <summary>
/// The checkers that exist, by artifact id. A class gets its checker when its schema is
/// written (decisions.md, 2026-09-06 and 2026-09-07); the batch classes got theirs with the
/// runner's rebuild of 2026-09-09. An id with no checker is silence, never a failure.
/// </summary>
public static class SchemaCheckers
{
    public static SchemaChecker? For(string artifactId) => artifactId switch
    {
        _ when WellKnown.HypothesisArtifacts.Contains(artifactId) => HypothesisFile.Check,
        WellKnown.HypothesisIndex => HypothesisIndex.Check,
        WellKnown.Studies => Registry.Check,
        WellKnown.Leads => Leads.Check,
        WellKnown.Findings => FindingsChecker.Check,
        WellKnown.DeclinedCandidates => DeclinedCandidates.Check,
        WellKnown.Corpora => Corpora.Check,
        WellKnown.Decisions => Decisions.Check,
        WellKnown.QuestionList => Questions.Check,
        WellKnown.Directions => Directions.Check,
        WellKnown.Index => BatchIndex.Check,
        WellKnown.Definition => Definition.Check,
        _ => null,
    };

    /// <summary>The artifact ids that dispatch to a checker, one per class (the three hypothesis rows count once).</summary>
    public static readonly string[] CheckedIds =
        [WellKnown.HypothesisRecord, WellKnown.HypothesisIndex, WellKnown.Studies, WellKnown.Leads, WellKnown.Findings, WellKnown.DeclinedCandidates, WellKnown.Corpora, WellKnown.Decisions, WellKnown.QuestionList,
         WellKnown.Directions, WellKnown.Index, WellKnown.Definition];

    internal static string[] Lines(string path) => File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');

    internal static readonly Regex IsoDate = new(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled);

    internal static bool ExactDate(string value)
        => IsoDate.IsMatch(value) && DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
}

/// <summary>
/// schemas/hypothesis-file-schema.md. One file, three artifacts under three mutations
/// (d-2026-09-11-9): § Hypothesis edited in place, § Origin frozen, § Record appended. The
/// engine holds the sections, the fields and their types, the kind enum and the candidate
/// reference, reported under `hypothesis.shape` and `hypothesis.entry`; five class rules hold
/// what no schema language expresses (d-2026-09-11-23). The frontmatter and its status mirror
/// left with d-2026-09-11-12, the created entry became § Origin with d-2026-09-11-8, and the
/// citation decomposed into fields with d-2026-09-11-15 and -18.
/// </summary>
public static class HypothesisFile
{
    public const string SchemaId = "hypothesis-file-schema";

    /// <summary>Per kind, the fields it must carry; a field of another kind on it is also a failure.</summary>
    static readonly Dictionary<string, string[]> Required = new(StringComparer.Ordinal)
    {
        ["evidence"] = ["candidate", "tag", "finding", "falsifier"],
        ["iteration"] = ["from", "reason"],
        ["baselined"] = ["rationale"],
    };

    static readonly string[] KindFields =
        ["candidate", "tag", "finding", "falsifier", "from", "reason", "rationale"];

    static readonly Regex FileName = new(@"^(?<id>\d{3})-[a-z0-9-]+\.md$", RegexOptions.Compiled);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();

        if (!FileName.IsMatch(file))
            findings.Add(Finding.Fail("hypothesis.shape", file, "the file is NNN-slug.md"));

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return findings; }
        foreach (var p in engine.Problems)
            findings.Add(Finding.Fail(p.Section == "Record" ? "hypothesis.entry" : "hypothesis.shape", file, p.Message));

        var doc = engine.Document;
        var positions = doc.Entries.Where(e => e.Section == "Record").ToList();
        var arr = doc.Root["Record"] as JsonArray ?? [];

        string? lastDate = null;
        var challengeStands = false;

        for (var i = 0; i < arr.Count; i++)
        {
            var obj = (JsonObject)arr[i]!;
            var pos = i < positions.Count ? positions[i] : null;
            var line = pos?.Line ?? 0;
            var kind = obj[DocumentReader.HeadingProperty]?.GetValue<string>() ?? "";

            // An iteration entry is a wording boundary: nothing above it binds to the wording below.
            if (kind == "iteration") challengeStands = false;

            if (!Required.ContainsKey(kind))
                findings.Add(Finding.Fail("hypothesis.entry", file,
                    $"line {line}: '{kind}' is not an entry kind; the heading is one of {string.Join(", ", Required.Keys)}"));

            if (Required.TryGetValue(kind, out var required))
            {
                var id = $"hypothesis.{kind}.fields";
                foreach (var key in required.Where(k => obj[k] is null))
                    findings.Add(Finding.Fail(id, file, $"line {line}: a {kind} entry carries '{key}'"));
                foreach (var key in KindFields.Where(k => !required.Contains(k) && obj[k] is not null))
                    findings.Add(Finding.Fail(id, file, $"line {line}: '{key}' is not a field of a {kind} entry"));
            }

            if (kind == "baselined" && challengeStands)
                findings.Add(Finding.Fail("hypothesis.baselined.challenged", file,
                    $"line {line}: a challenging entry stands unresolved under the current wording; only a reword clears it"));
            if (kind == "evidence" && obj["tag"]?.GetValue<string>() == "challenging") challengeStands = true;

            var date = obj["date"]?.GetValue<string>();
            if (date is not null && SchemaCheckers.ExactDate(date))
            {
                var dLine = pos?.FieldLines.GetValueOrDefault("date", line) ?? line;
                if (lastDate is not null && string.CompareOrdinal(date, lastDate) < 0)
                    findings.Add(Finding.Fail("hypothesis.entry.date", file,
                        $"line {dLine}: {date} is earlier than the entry before it, {lastDate}"));
                lastDate = date;
            }
        }

        return findings.DistinctBy(f => (f.CheckId, f.Message)).ToList();
    }

}

/// <summary>schemas/hypothesis-index-schema.md: two columns, id order, every file listed, every link resolving.</summary>
public static class HypothesisIndex
{
    static readonly Regex Link = new(@"^\[(?<slug>[a-z0-9-]+)\]\((?<file>\d{3}-[a-z0-9-]+\.md)\)$", RegexOptions.Compiled);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var dir = Path.GetDirectoryName(path)!;
        var findings = new List<Finding>();

        IReadOnlyList<MarkdownTable> tables;
        try { tables = MapTables.ReadAll(File.ReadAllText(path)); }
        catch (MapFormatException ex) { return [Finding.Fail("hypothesis-index.table", file, ex.Message)]; }

        var table = tables.FirstOrDefault(t => t.Headers.Select(h => h.ToLowerInvariant()).SequenceEqual(["id", "slug"]));
        if (table is null) return [Finding.Fail("hypothesis-index.table", file, "no table with columns ID | Slug")];

        var listed = new HashSet<string>(StringComparer.Ordinal);
        var lastId = -1;
        foreach (var row in table.Rows)
        {
            if (!int.TryParse(row.Cells[0], out var id) || row.Cells[0].Length != 3)
            {
                findings.Add(Finding.Fail("hypothesis-index.link", file, $"line {row.Line}: the id is NNN; found '{row.Cells[0]}'"));
                continue;
            }
            if (id <= lastId)
                findings.Add(Finding.Fail("hypothesis-index.order", file, $"line {row.Line}: ids ascend; {row.Cells[0]} follows {lastId:000}"));
            lastId = id;

            var m = Link.Match(row.Cells[1]);
            if (!m.Success || m.Groups["file"].Value != $"{row.Cells[0]}-{m.Groups["slug"].Value}.md")
            {
                findings.Add(Finding.Fail("hypothesis-index.link", file, $"line {row.Line}: the slug cell is [slug](NNN-slug.md) with the same NNN"));
                continue;
            }
            listed.Add(m.Groups["file"].Value);
            if (!File.Exists(Path.Combine(dir, m.Groups["file"].Value)))
                findings.Add(Finding.Fail("hypothesis-index.link", file, $"line {row.Line}: {m.Groups["file"].Value} does not exist"));
        }

        foreach (var f in Directory.GetFiles(dir, "*.md").Select(Path.GetFileName).OrderBy(n => n, StringComparer.Ordinal))
            if (f is not null && Regex.IsMatch(f, @"^\d{3}-") && !listed.Contains(f))
                findings.Add(Finding.Fail("hypothesis-index.missing", file, $"{f} has no row"));

        return findings;
    }
}

/// <summary>
/// schemas/study-registry-schema.md on the engine (d-2026-09-14-7 and -9 to -12): the title,
/// then one-line entries, each a study id <c>&lt;type&gt;-of-&lt;question&gt;[-&lt;slug&gt;]</c>,
/// its type verification or exploration, its question the longest slug heading an entry in the
/// question list; nothing else is authored. The engine holds the shape, reported under
/// <c>registry.shape</c>; the class holds the title, the id form with its question resolved, and
/// uniqueness.
/// </summary>
public static class Registry
{
    public const string SchemaId = "study-registry-schema";
    public const string Title = "Studies";
    public static readonly string[] Types = ["verification", "exploration"];
    const string Of = "-of-";

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return findings; }
        foreach (var p in engine.Problems)
            findings.Add(Finding.Fail("registry.shape", file, p.Message));
        var doc = engine.Document;
        if (doc.Title != Title)
            findings.Add(Finding.Fail("registry.title", file, $"the title is '# {Title}'"));

        var questions = Questions.SlugsOf(ctx);
        if (questions is null)
            findings.Add(Finding.Info("registry.questions-unavailable", file,
                "the question list could not be read; the question segment of each id is not checked"));

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in Ids(doc))
        {
            if (!seen.Add(id)) findings.Add(Finding.Fail("registry.duplicate", file, $"{id} appears twice"));
            var problem = IdProblem(id, questions);
            if (problem is not null) findings.Add(Finding.Fail("registry.id", file, $"'{id}': {problem}"));
        }
        return findings;
    }

    /// <summary>The ids a parsed registry lists, in order.</summary>
    public static IReadOnlyList<string> Ids(ParsedDocument doc)
        => (doc.Root["body"] as JsonArray ?? []).Select(n => n?.ToString() ?? "").ToList();

    /// <summary>The type an id's prefix names, or null when the id carries no type prefix.</summary>
    public static string? TypeOf(string id)
        => Types.FirstOrDefault(t => id.StartsWith(t + Of, StringComparison.Ordinal));

    /// <summary>
    /// Why an id is outside its form, or null: a lowercase slug; a type prefix; then the longest
    /// slug heading an entry in the question list; then nothing or a slug. With no list the
    /// question goes unchecked, which Check reports as information.
    /// </summary>
    static string? IdProblem(string id, IReadOnlySet<string>? questions)
    {
        if (!ClosedSets.IdPattern.IsMatch(id)) return "not a lowercase slug";
        var type = TypeOf(id);
        if (type is null) return "an id is verification-of-<question>[-<slug>] or exploration-of-<question>[-<slug>]";
        var rest = id[(type.Length + Of.Length)..];
        if (rest.Length == 0) return "no question after the type";
        if (questions is null) return null;
        var question = questions
            .Where(q => rest == q || rest.StartsWith(q + "-", StringComparison.Ordinal))
            .OrderByDescending(q => q.Length).FirstOrDefault();
        return question is null ? $"'{rest}' begins with no slug heading an entry in the question list" : null;
    }
}

/// <summary>
/// schemas/leads-schema.md on the engine (d-2026-09-13-19 to -35): an exploration's consolidated
/// leads. The engine holds the sections and each lead's typed fields, reported under
/// `leads.shape` and `leads.entry`; the class's own rules are the title with the folder's
/// exploration, the heading as the citation token with a unique slug, every cited item token in
/// the index of a batch under the study, the appended reread lines, and the shortcoming parts.
/// </summary>
public static class Leads
{
    public const string SchemaId = "leads-schema";
    public const string ExplorationPrefix = "exploration-of-";
    public static readonly string[] Parts = ["slice", "itemizer", "directions", "consolidation", "execution", "corpus"];
    static readonly Regex RereadLine = new(@"^- reread: (?<date>\d{4}-\d{2}-\d{2}) (?<what>\S.*)$", RegexOptions.Compiled);
    static readonly Regex ItemCite = new(@"^(?<study>[a-z0-9-]+)/(?<batch>[0-9]{2}-[a-z0-9-]+)/(?<item>[a-z0-9-]+)$", RegexOptions.Compiled);
    static readonly Regex ShortcomingLine = new(@"^(?<part>[a-z]+):\s*\S", RegexOptions.Compiled);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var studyDir = Path.GetDirectoryName(Path.GetFullPath(path))!;
        var study = Path.GetFileName(studyDir)!;
        var findings = new List<Finding>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return findings; }
        foreach (var p in engine.Problems)
        {
            if (p.Key == "reread") continue; // the appended line, held below
            var id = p.Section == "Leads" && p.Kind is ProblemKind.Missing or ProblemKind.Unknown or ProblemKind.Order or ProblemKind.Type or ProblemKind.Form or ProblemKind.Duplicate
                ? "leads.entry" : "leads.shape";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var doc = engine.Document;

        // ---- title and study ----
        var title = $"{study} — leads";
        if (doc.Title != title)
            findings.Add(Finding.Fail("leads.title", file, $"the title is '# {title}', the id of the study whose folder holds the file"));
        if (!study.StartsWith(ExplorationPrefix, StringComparison.Ordinal))
            findings.Add(Finding.Fail("leads.title", file, $"'{study}' is not an exploration's id; leads belong to an exploration"));

        // ---- the items of the batches under the study, for citations ----
        var batches = new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal);
        var batchesDir = Path.Combine(studyDir, "batches");
        if (Directory.Exists(batchesDir))
            foreach (var dir in Directory.GetDirectories(batchesDir))
            {
                var indexPath = Path.Combine(dir, "index.md");
                batches[Path.GetFileName(dir)] = File.Exists(indexPath)
                    ? StoryPlanner.BatchFiles.IndexFile.Parse(File.ReadAllText(indexPath)).Rows.Select(r => r.Item).ToList()
                    : [];
            }

        // ---- the reread lines, by position ----
        var lines = SchemaCheckers.Lines(path);
        var positions = doc.Entries.Where(e => e.Section == "Leads").ToList();
        var lastDate = new Dictionary<int, string>();
        for (var i = 0; i < lines.Length; i++)
        {
            if (!lines[i].StartsWith("- reread:", StringComparison.Ordinal)) continue;
            var m = RereadLine.Match(lines[i]);
            if (!m.Success)
                findings.Add(Finding.Fail("leads.reread", file, $"line {i + 1}: a reread line is '- reread: YYYY-MM-DD <what the source showed>'"));
            var owner = positions.LastOrDefault(e => e.Line < i + 1);
            if (owner is null || !owner.FieldLines.Values.Any(l => l < i + 1))
            { findings.Add(Finding.Fail("leads.reread", file, $"line {i + 1}: a reread line sits beneath a lead's fields")); continue; }
            var fieldAfter = lines.Skip(i + 1).TakeWhile(l => !l.StartsWith("### ", StringComparison.Ordinal) && !l.StartsWith("## ", StringComparison.Ordinal))
                .Any(l => StoryPlanner.BatchFiles.KeyedLines.IsKeyedLine(l) && !l.StartsWith("- reread:", StringComparison.Ordinal));
            if (fieldAfter)
                findings.Add(Finding.Fail("leads.reread", file, $"line {i + 1}: a keyed field line follows a reread line; reread lines close a lead"));
            if (m.Success && SchemaCheckers.ExactDate(m.Groups["date"].Value))
            {
                var date = m.Groups["date"].Value;
                if (lastDate.TryGetValue(owner.Line, out var before) && string.CompareOrdinal(date, before) < 0)
                    findings.Add(Finding.Fail("leads.reread", file, $"line {i + 1}: {date} is earlier than the reread line before it, {before}"));
                lastDate[owner.Line] = date;
            }
        }

        // ---- the entries ----
        var slugs = new HashSet<string>(StringComparer.Ordinal);
        var arr = doc.Root["Leads"] as JsonArray ?? [];
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
                findings.Add(Finding.Fail("leads.entry", file, $"line {line}: the heading is '{study}/<slug>', the id of the study whose folder holds the file then the slug; found '{heading}'"));
            else if (!ClosedSets.IdPattern.IsMatch(slug))
                findings.Add(Finding.Fail("leads.entry", file, $"line {line}: '{slug}' is not a lowercase slug"));
            else if (!slugs.Add(slug))
                findings.Add(Finding.Fail("leads.entry", file, $"line {line}: '{slug}' repeats a slug in this file"));

            var cites = obj["cites"] is JsonArray c ? c.Select(x => x?.ToString() ?? "").ToList() : [];
            var cLine = pos?.FieldLines.GetValueOrDefault("cites", line) ?? line;
            if (obj["cites"] is not null && cites.Count == 0)
                findings.Add(Finding.Fail("leads.cites", file, $"line {cLine}: cites holds at least one item token"));
            foreach (var cite in cites)
            {
                var it = ItemCite.Match(cite);
                if (!it.Success)
                { findings.Add(Finding.Fail("leads.cites", file, $"line {cLine}: '{cite}' is not '<study>/<batch>/<item>'")); continue; }
                if (it.Groups["study"].Value != study)
                { findings.Add(Finding.Fail("leads.cites", file, $"line {cLine}: '{cite}' cites a batch outside this study")); continue; }
                if (!batches.TryGetValue(it.Groups["batch"].Value, out var items))
                { findings.Add(Finding.Fail("leads.cites", file, $"line {cLine}: '{cite}' cites no batch under this study")); continue; }
                if (!items.Contains(it.Groups["item"].Value, StringComparer.Ordinal))
                    findings.Add(Finding.Fail("leads.cites", file, $"line {cLine}: '{it.Groups["item"].Value}' is not an item of that batch's index"));
            }
        }

        // ---- shortcomings ----
        if (doc.Root["Shortcomings"] is JsonArray shortcomings)
            foreach (var s in shortcomings)
            {
                var text = s?.ToString() ?? "";
                var m = ShortcomingLine.Match(text);
                if (!m.Success || !Parts.Contains(m.Groups["part"].Value, StringComparer.Ordinal))
                    findings.Add(Finding.Fail("leads.shortcoming", file, $"a Shortcomings line is '<part>: <what the review found>', the part one of {string.Join(", ", Parts)}; found '{(text.Length <= 60 ? text : text[..60] + "…")}'"));
            }

        return findings.DistinctBy(f => (f.CheckId, f.Message)).ToList();
    }
}

/// <summary>
/// schemas/corpora-schema.md on the engine (d-2026-09-14-13 and -17 to -19): the title, a head
/// paragraph, then one <c>###</c> entry per corpus with what, where, read through and optional
/// caveats. The engine holds the fields, reported under <c>corpora.entry</c>, and stray lines,
/// under <c>corpora.shape</c>; the class holds the title, the head paragraph, and the heading as
/// a slug unique in the file. No checker reads the entry ids: the index head names no corpus
/// (d-2026-09-14-21).
/// </summary>
public static class Corpora
{
    public const string SchemaId = "corpora-schema";
    public const string FileName = "CORPORA.md";
    public const string Title = "Corpora";

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return findings; }
        foreach (var p in engine.Problems)
        {
            var id = p.Kind is ProblemKind.Missing or ProblemKind.Unknown or ProblemKind.Order or ProblemKind.Type or ProblemKind.Form or ProblemKind.Duplicate
                ? "corpora.entry" : "corpora.shape";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var doc = engine.Document;
        if (doc.Title != Title)
            findings.Add(Finding.Fail("corpora.title", file, $"the title is '# {Title}'"));
        if (!doc.LeadProse.TryGetValue("body", out var lead) || lead.All(l => l.Trim().Length == 0))
            findings.Add(Finding.Fail("corpora.shape", file, "the head paragraph between the title and the first entry is missing"));

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var e in doc.Entries.Where(e => e.Section == "body"))
        {
            if (!ClosedSets.IdPattern.IsMatch(e.Heading))
                findings.Add(Finding.Fail("corpora.entry", file, $"line {e.Line}: an entry heading is a corpus id, a lowercase slug; found '{e.Heading}'"));
            else if (!seen.Add(e.Heading))
                findings.Add(Finding.Fail("corpora.entry", file, $"line {e.Line}: {e.Heading} appears twice"));
        }
        return findings;
    }
}

/// <summary>
/// schemas/decisions-schema.md on the engine: the Shape's head, sections "## Revision N" and the
/// entries' typed fields are the engine's; the class's own rules are the title, sections that
/// ascend and open with at most one paragraph, the id d-&lt;date&gt;-&lt;n&gt; written with the
/// entry and held to its date's sequence, dates that never go backwards, and supersession that
/// is whole and never chains. Nothing derives an id and the tool never writes one.
/// </summary>
public static class Decisions
{
    public const string SchemaId = "decisions-schema";
    public const string Title = "Decisions";
    public const string SectionName = "Revision N";
    public static readonly string[] Keys = ["id", "date", "supersedes", "raised by", "decision", "not taken"];
    static readonly Regex SectionHeading = new(@"^Revision (?<n>[1-9]\d*)$", RegexOptions.Compiled);
    public static readonly Regex Id = new(@"^d-(?<date>\d{4}-\d{2}-\d{2})-(?<n>[1-9]\d*)$", RegexOptions.Compiled);

    /// <summary>One entry with the id the shape expects for it; only entries whose date line is exact get one.</summary>
    public sealed record Entry(string Id, string Title, int Line, string Date, IReadOnlyList<string> Supersedes);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path) => Read(ctx, path).Findings;

    /// <summary>The entries of a decisions file in order, and every finding against the schema.</summary>
    public static (IReadOnlyList<Entry> Entries, IReadOnlyList<Finding> Findings) Read(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();
        var entries = new List<Entry>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return (entries, findings); }
        foreach (var p in engine.Problems)
        {
            var id = p.Section != SectionName ? "decisions.shape"
                : p.Key == "id" && p.Kind is ProblemKind.Form or ProblemKind.Type ? "decisions.entry.id"
                : p.Key == "date" && p.Kind is ProblemKind.Form or ProblemKind.Type ? "decisions.entry.date"
                : p.Key == "supersedes" && p.Kind is ProblemKind.Form or ProblemKind.Type ? "decisions.supersedes"
                : "decisions.entry.fields";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var doc = engine.Document;
        if (doc.Title != Title)
            findings.Add(Finding.Fail("decisions.shape", file, $"the first line is '# {Title}'"));
        if (doc.Root["head"] is JsonValue headNode && headNode.GetValue<string>().Split('\n').Any(l => l.StartsWith("### ", StringComparison.Ordinal)))
            findings.Add(Finding.Fail("decisions.shape", file, "an entry outside a section; the head holds none"));

        var sections = doc.Root[SectionName] as JsonArray ?? [];
        var sectionN = 0;
        var perDate = new Dictionary<string, int>(StringComparer.Ordinal);
        var ids = new HashSet<string>(StringComparer.Ordinal);
        var superseded = new Dictionary<string, int>(StringComparer.Ordinal);
        string? lastDate = null;
        var positions = doc.Entries.Where(e => e.Section == SectionName).ToList();
        var entryIndex = 0;

        foreach (var sectionNode in sections)
        {
            var heading = sectionNode![DocumentReader.HeadingProperty]!.GetValue<string>();
            var m = SectionHeading.Match(heading);
            if (!m.Success) findings.Add(Finding.Fail("decisions.shape", file, $"a section is '## Revision N'; found '{heading}'"));
            else
            {
                var n = int.Parse(m.Groups["n"].Value);
                if (n <= sectionN) findings.Add(Finding.Fail("decisions.shape", file, $"sections ascend; Revision {n} follows Revision {sectionN}"));
                sectionN = n;
            }
            var arr = sectionNode[SectionName] as JsonArray ?? [];
            foreach (var entryNode in arr)
            {
                var obj = (JsonObject)entryNode!;
                var pos = entryIndex < positions.Count ? positions[entryIndex] : null;
                entryIndex++;
                var titleText = obj[DocumentReader.HeadingProperty]?.GetValue<string>() ?? "";
                var titleLine = pos?.Line ?? 0;
                if (titleText.Length == 0)
                    findings.Add(Finding.Fail("decisions.shape", file, $"line {titleLine}: an entry's title states the ruling in one line"));

                string? id = null;
                var date = obj["date"]?.GetValue<string>();
                if (date is not null && SchemaCheckers.IsoDate.IsMatch(date))
                {
                    var dateLine = pos?.FieldLines.GetValueOrDefault("date", titleLine) ?? titleLine;
                    if (!SchemaCheckers.ExactDate(date))
                        findings.Add(Finding.Fail("decisions.entry.date", file, $"line {dateLine}: the date is exactly YYYY-MM-DD; found '{date}'"));
                    else
                    {
                        if (lastDate is not null && string.CompareOrdinal(date, lastDate) < 0)
                            findings.Add(Finding.Fail("decisions.entry.date", file, $"line {dateLine}: {date} is earlier than the entry before it, {lastDate}"));
                        lastDate = date;
                        perDate[date] = perDate.GetValueOrDefault(date) + 1;
                        id = $"d-{date}-{perDate[date]}";
                    }
                }

                var written = obj["id"]?.GetValue<string>();
                if (written is not null && id is not null && written != id)
                    findings.Add(Finding.Fail("decisions.entry.id", file,
                        $"line {pos?.FieldLines.GetValueOrDefault("id", titleLine) ?? titleLine}: the id is {id}, the entry's date and the next number of that date; found '{written}'"));

                var targets = new List<string>();
                if (obj["supersedes"] is JsonArray sup)
                {
                    var supLine = pos?.FieldLines.GetValueOrDefault("supersedes", titleLine) ?? titleLine;
                    foreach (var tNode in sup)
                    {
                        var t = tNode!.GetValue<string>();
                        if (!Id.IsMatch(t)) continue; // the engine reported the form
                        if (targets.Contains(t))
                            findings.Add(Finding.Fail("decisions.supersedes", file, $"line {supLine}: {t} is named twice"));
                        else if (!ids.Contains(t))
                            findings.Add(Finding.Fail("decisions.supersedes", file, $"line {supLine}: {t} is not an entry earlier in this file"));
                        else if (superseded.TryGetValue(t, out var by))
                            findings.Add(Finding.Fail("decisions.supersedes", file, $"line {supLine}: {t} is already superseded by the entry at line {by}; supersession is whole and never chains"));
                        else { targets.Add(t); superseded[t] = titleLine; }
                    }
                }

                if (id is not null)
                {
                    ids.Add(id);
                    entries.Add(new Entry(id, titleText, titleLine, date!, targets));
                }
            }
        }

        // A section opens with at most one paragraph before its entries.
        foreach (var (section, lead) in doc.LeadProse)
        {
            var paragraphs = 0;
            var inParagraph = false;
            foreach (var l in lead)
            {
                if (l.Trim().Length == 0) { inParagraph = false; continue; }
                if (!inParagraph) { paragraphs++; inParagraph = true; }
            }
            if (paragraphs > 1)
                findings.Add(Finding.Fail("decisions.shape", file, $"a section opens with at most one paragraph, then entries; a {section} section opens with {paragraphs}"));
        }
        return (entries, findings.DistinctBy(f => (f.CheckId, f.Message)).ToList());
    }
}

/// <summary>
/// schemas/question-entry-schema.md on the engine: the entries and their typed fields; the
/// class's own rules are the title, the heading as the citation token <c>questions/&lt;slug&gt;</c>
/// with a slug unique in the list, dates that never go backwards, and the appended withdrawn and
/// reinstated lines beneath the fields, alternating and starting with withdrawn. The buildout
/// keeps one list, <c>questions.md</c> (d-2026-09-13-38).
/// </summary>
public static class Questions
{
    public const string SchemaId = "question-entry-schema";
    public const string Title = "Questions";
    /// <summary>The token prefix every entry heading carries: the list's own file name.</summary>
    public const string Prefix = "questions";
    public static readonly string[] Keys = ["date", "raised by", "question", "suggested test"];
    static readonly string[] AppendedKinds = ["withdrawn", "reinstated"];
    static readonly Regex AppendedLine = new(@"^- (?<kind>withdrawn|reinstated): (?<date>\d{4}-\d{2}-\d{2}) (?<reason>\S.*)$", RegexOptions.Compiled);

    /// <summary>One entry as read; Date is empty when the date line was not exact; Withdrawn when its last appended line is a withdrawal.</summary>
    public sealed record Entry(string Slug, int Line, string Date, bool Withdrawn);

    static string? AppendedKindOf(string line)
        => AppendedKinds.FirstOrDefault(k => line.StartsWith($"- {k}:", StringComparison.Ordinal));

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path) => Read(ctx, path).Findings;

    /// <summary>
    /// The slugs heading the list's entries, for a checker that resolves a question outside a
    /// token type (the registry's id segment, d-2026-09-14-12): the list through the Artifacts
    /// table of the governing skill folder, or, where that folder has no table, at the
    /// singleton's declared path under the repo root. Null when no list can be read.
    /// </summary>
    public static IReadOnlySet<string>? SlugsOf(CheckContext ctx)
    {
        var path = References.FilesOf(WellKnown.QuestionList, ctx)?.FirstOrDefault()
                   ?? Path.Combine(ctx.RepoRoot, "docs", "v3-framework", "questions.md");
        if (!File.Exists(path)) return null;
        var marker = $"### {Prefix}/";
        var slugs = new HashSet<string>(StringComparer.Ordinal);
        foreach (var line in File.ReadAllLines(path))
        {
            if (!line.StartsWith(marker, StringComparison.Ordinal)) continue;
            var slug = line[marker.Length..].Trim();
            if (ClosedSets.IdPattern.IsMatch(slug)) slugs.Add(slug);
        }
        return slugs;
    }

    /// <summary>The entries of one list in order, and every finding against the schema.</summary>
    public static (IReadOnlyList<Entry> Entries, IReadOnlyList<Finding> Findings) Read(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();
        var entries = new List<Entry>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return (entries, findings); }
        foreach (var p in engine.Problems)
        {
            if (p.Key is "withdrawn" or "reinstated") continue; // the appended lines, held below
            var id = p.Key == "date" && p.Kind is ProblemKind.Form or ProblemKind.Type ? "question.entry.date"
                : "question.entry.fields";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var doc = engine.Document;
        if (doc.Title != Title)
            findings.Add(Finding.Fail("question.title", file, $"the title is '# {Title}'"));

        // ---- the withdrawn and reinstated lines, by position in the file ----
        var lines = SchemaCheckers.Lines(path);
        var lastKindOf = new Dictionary<int, string>(); // entry heading line → its last appended kind
        var positions = doc.Entries.Where(e => e.Section == "body").ToList();
        for (var i = 0; i < lines.Length; i++)
        {
            var kind = AppendedKindOf(lines[i]);
            if (kind is null) continue;
            var owner = positions.LastOrDefault(e => e.Line < i + 1);
            if (!AppendedLine.IsMatch(lines[i]))
                findings.Add(Finding.Fail("question.withdrawn", file, $"line {i + 1}: a {kind} line is '- {kind}: YYYY-MM-DD <reason>'"));
            if (owner is null) continue;
            var fieldsAfter = lines.Skip(i + 1).TakeWhile(l => !l.StartsWith("### ", StringComparison.Ordinal))
                .Any(l => StoryPlanner.BatchFiles.KeyedLines.IsKeyedLine(l) && AppendedKindOf(l) is null);
            var fieldsBefore = owner.FieldLines.Values.Any(l => l < i + 1);
            if (!fieldsBefore)
                findings.Add(Finding.Fail("question.withdrawn", file, $"line {i + 1}: {kind} sits beneath the fields, never before them"));
            else if (fieldsAfter)
                findings.Add(Finding.Fail("question.withdrawn", file, $"line {i + 1}: a keyed line after {kind}; the appended lines close an entry"));
            var previous = lastKindOf.GetValueOrDefault(owner.Line);
            var expected = previous == "withdrawn" ? "reinstated" : "withdrawn";
            if (kind != expected)
                findings.Add(Finding.Fail("question.withdrawn", file, $"line {i + 1}: {kind} follows {previous ?? "the fields"}; withdrawn and reinstated alternate, starting with withdrawn"));
            lastKindOf[owner.Line] = kind;
        }

        // ---- headings and dates ----
        var slugs = new HashSet<string>(StringComparer.Ordinal);
        string? lastDate = null;
        var arr = doc.Root["body"] as JsonArray ?? [];
        for (var i = 0; i < arr.Count; i++)
        {
            var obj = (JsonObject)arr[i]!;
            var pos = i < positions.Count ? positions[i] : null;
            var heading = obj[DocumentReader.HeadingProperty]?.GetValue<string>() ?? "";
            var line = pos?.Line ?? 0;
            var slash = heading.IndexOf('/');
            var prefix = slash < 0 ? "" : heading[..slash];
            var slug = slash < 0 ? heading : heading[(slash + 1)..];
            if (prefix != Prefix)
                findings.Add(Finding.Fail("question.slug", file, $"line {line}: the heading is '{Prefix}/<slug>'; found '{heading}'"));
            else if (!ClosedSets.IdPattern.IsMatch(slug))
                findings.Add(Finding.Fail("question.slug", file, $"line {line}: '{slug}' is not a lowercase slug"));
            else if (!slugs.Add(slug))
                findings.Add(Finding.Fail("question.slug", file, $"line {line}: '{slug}' repeats a slug in this list"));

            var dateValue = "";
            var date = obj["date"]?.GetValue<string>();
            if (date is not null && SchemaCheckers.IsoDate.IsMatch(date))
            {
                var dateLine = pos?.FieldLines.GetValueOrDefault("date", line) ?? line;
                if (!SchemaCheckers.ExactDate(date))
                    findings.Add(Finding.Fail("question.entry.date", file, $"line {dateLine}: the date is exactly YYYY-MM-DD; found '{date}'"));
                else
                {
                    if (lastDate is not null && string.CompareOrdinal(date, lastDate) < 0)
                        findings.Add(Finding.Fail("question.entry.date", file, $"line {dateLine}: {date} is earlier than the entry before it, {lastDate}"));
                    lastDate = date;
                    dateValue = date;
                }
            }
            entries.Add(new Entry(slug, line, dateValue, lastKindOf.GetValueOrDefault(line) == "withdrawn"));
        }
        return (entries, findings.DistinctBy(f => (f.CheckId, f.Message)).ToList());
    }
}
