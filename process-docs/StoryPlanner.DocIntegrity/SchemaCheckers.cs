using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// What a schema checker sees besides the file: the repository root, the governing skill
/// folder, and the corpus ids a registry row may name. Built once per check by
/// <see cref="ArtifactScope"/>; tests build it directly.
/// </summary>
public sealed record CheckContext(string RepoRoot, string SkillFolder, IReadOnlySet<string> CorporaIds)
{
    public static CheckContext From(string repoRoot, string skillFolder)
        => new(repoRoot, skillFolder, Corpora.Ids(skillFolder));
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
        [WellKnown.HypothesisStatus, WellKnown.HypothesisIndex, WellKnown.Studies, WellKnown.Leads, WellKnown.Findings, WellKnown.DeclinedCandidates, WellKnown.Corpora, WellKnown.Decisions, WellKnown.QuestionList,
         WellKnown.Directions, WellKnown.Index, WellKnown.Definition];

    internal static string[] Lines(string path) => File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');

    internal static readonly Regex IsoDate = new(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled);

    internal static bool ExactDate(string value)
        => IsoDate.IsMatch(value) && DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
}

/// <summary>
/// schemas/hypothesis-file-schema.md. Shape from the fifty real files (four frontmatter keys, two
/// sections, four entry kinds, minute-precision timestamps); checks from decisions: an evidence
/// entry is written only by a promotion from a referee-checked candidate and carries its
/// citation and falsifier (decisions d-2026-09-05-3, d-2026-09-06-2), an iteration entry is a
/// wording boundary and status is computed from the entries after the last one
/// (d-2026-09-05-4), baselining is Brian's dated entry and resets on a challenge
/// (d-2026-09-05-1). The citation cites the candidate's token and the directions version and
/// body hash it was judged under (d-2026-09-09-12). One file, three artifacts, one checker.
/// </summary>
public static class HypothesisFile
{
    static readonly string[] Keys = ["id", "status", "baselined", "created"];
    static readonly string[] Statuses = ["untested", "evidenced", "challenged"];

    static readonly Regex Entry = new(
        @"^- (?<kind>[a-z]+) \| (?<ts>\d{4}-\d{2}-\d{2}T\d{2}:\d{2})(?<rest>.*)$", RegexOptions.Compiled);

    static readonly Regex Citation = new(
        @"^ \| \((?<study>[a-z0-9-]+)/(?<candidate>[a-z0-9-]+); directions-(?<n>\d+)@(?<hash>[0-9a-f]{6,64})\) \[(?<tag>supporting|challenging)\]:",
        RegexOptions.Compiled);

    static readonly Regex FileName = new(@"^(?<id>\d{3})-[a-z0-9-]+\.md$", RegexOptions.Compiled);

    public sealed record RecordEntry(string Kind, string Timestamp, string Rest, int Line, IReadOnlyList<string> Continuation);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();
        var lines = SchemaCheckers.Lines(path);

        // ---- frontmatter ----
        var fm = new Dictionary<string, string>(StringComparer.Ordinal);
        var close = -1;
        if (lines.Length > 0 && lines[0].Trim() == "---")
            for (var i = 1; i < lines.Length; i++)
            {
                if (lines[i].Trim() == "---") { close = i; break; }
                var colon = lines[i].IndexOf(':');
                if (colon > 0) fm[lines[i][..colon].Trim()] = lines[i][(colon + 1)..].Trim();
            }
        if (close < 0)
        {
            findings.Add(Finding.Fail("hypothesis.frontmatter", file, "no frontmatter between --- lines at the top"));
            return findings;
        }
        var keys = fm.Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
        if (!keys.SequenceEqual(Keys.OrderBy(k => k, StringComparer.Ordinal)))
            findings.Add(Finding.Fail("hypothesis.frontmatter", file,
                $"frontmatter keys are exactly id, status, baselined, created; found [{string.Join(", ", keys)}]"));

        var status = fm.GetValueOrDefault("status", "");
        if (!Statuses.Contains(status))
            findings.Add(Finding.Fail("hypothesis.frontmatter", file, $"status '{status}' is not untested, evidenced or challenged"));

        var baselined = fm.GetValueOrDefault("baselined", "");
        if (baselined != "false" && !SchemaCheckers.IsoDate.IsMatch(baselined))
            findings.Add(Finding.Fail("hypothesis.frontmatter", file, $"baselined is false or an ISO date; found '{baselined}'"));

        var created = fm.GetValueOrDefault("created", "");
        if (!SchemaCheckers.IsoDate.IsMatch(created))
            findings.Add(Finding.Fail("hypothesis.frontmatter", file, $"created is an ISO date; found '{created}'"));

        var name = FileName.Match(file);
        if (!name.Success)
            findings.Add(Finding.Fail("hypothesis.frontmatter", file, "the file is NNN-slug.md"));
        else if (!int.TryParse(fm.GetValueOrDefault("id", ""), out var id) || id != int.Parse(name.Groups["id"].Value))
            findings.Add(Finding.Fail("hypothesis.frontmatter", file,
                $"id '{fm.GetValueOrDefault("id", "")}' is not the NNN of the file name"));

        // ---- sections ----
        var text = string.Join('\n', lines);
        var outline = new MarkdownOutline(text);
        var h2 = outline.Headings.Where(h => h.Level == 2).Select(h => h.Text).ToList();
        if (!h2.SequenceEqual(["Hypothesis", "Record"], StringComparer.Ordinal))
            findings.Add(Finding.Fail("hypothesis.sections", file,
                $"the sections are ## Hypothesis then ## Record; found [{string.Join(", ", h2)}]"));
        else if (StateBuilder.Section(text, "Hypothesis").Trim().Length == 0)
            findings.Add(Finding.Fail("hypothesis.sections", file, "## Hypothesis is empty"));

        // ---- entries ----
        var entries = ParseEntries(text, lines, findings, file);
        if (entries.Count == 0)
            findings.Add(Finding.Fail("hypothesis.created-first", file, "the record holds no entries; the first is created"));
        else
        {
            if (entries[0].Kind != "created")
                findings.Add(Finding.Fail("hypothesis.created-first", file, $"the first entry is created; found {entries[0].Kind} at line {entries[0].Line}"));
            if (entries.Count(e => e.Kind == "created") > 1)
                findings.Add(Finding.Fail("hypothesis.created-first", file, "more than one created entry"));
        }

        foreach (var e in entries)
        {
            if (e.Kind == "evidence")
            {
                if (!Citation.IsMatch(e.Rest))
                    findings.Add(Finding.Fail("hypothesis.evidence.citation", file,
                        $"line {e.Line}: an evidence entry cites (<study>/<slug>; directions-N@<hash>) [supporting|challenging]; " +
                        "an entry without that citation was not produced by the pipeline"));
                if (!e.Continuation.Any(c => c.TrimStart().StartsWith("Falsifier:", StringComparison.Ordinal)))
                    findings.Add(Finding.Fail("hypothesis.evidence.no-falsifier", file,
                        $"line {e.Line}: an evidence entry carries a 'Falsifier:' line; an entry without one is malformed"));
            }
            else if (!e.Rest.StartsWith(':'))
                findings.Add(Finding.Fail("hypothesis.entry", file, $"line {e.Line}: '- {e.Kind} | <timestamp>:' then the text"));
        }

        // ---- status and baselined, from the entries bound to the current wording ----
        var lastIteration = -1;
        for (var i = 0; i < entries.Count; i++) if (entries[i].Kind == "iteration") lastIteration = i;
        var current = entries.Skip(lastIteration + 1).ToList();
        var evidence = current.Where(e => e.Kind == "evidence").ToList();
        var implied = evidence.Any(e => e.Rest.Contains("[challenging]", StringComparison.Ordinal)) ? "challenged"
            : evidence.Count > 0 ? "evidenced"
            : "untested";
        if (Statuses.Contains(status) && status != implied)
            findings.Add(Finding.Fail("hypothesis.status.mismatch", file,
                $"status is '{status}' but the entries after the last iteration imply '{implied}'"));

        if (baselined != "false" && SchemaCheckers.IsoDate.IsMatch(baselined))
        {
            if (!current.Any(e => e.Kind == "baselined"))
                findings.Add(Finding.Fail("hypothesis.baselined", file,
                    "baselined carries a date but no baselined entry is bound to the current wording"));
            if (implied != "evidenced")
                findings.Add(Finding.Fail("hypothesis.baselined", file,
                    $"baselined carries a date but the current-wording entries imply '{implied}'; a challenge or a rewording resets it"));
        }

        return findings;
    }

    /// <summary>Top-level entries with their continuation lines; anything else in the record is a finding.</summary>
    static List<RecordEntry> ParseEntries(string text, string[] lines, List<Finding> findings, string file)
    {
        var entries = new List<RecordEntry>();
        var start = Array.FindIndex(lines, l => l.Trim() == "## Record");
        if (start < 0) return entries;

        string? kind = null, ts = null, rest = null;
        var line = 0;
        var cont = new List<string>();
        void Flush()
        {
            if (kind is not null) entries.Add(new RecordEntry(kind, ts!, rest!, line, cont));
            kind = null; cont = [];
        }

        for (var i = start + 1; i < lines.Length; i++)
        {
            var raw = lines[i];
            if (raw.TrimStart().StartsWith('#')) break;
            if (raw.Trim().Length == 0) continue;
            if (raw.StartsWith("- ", StringComparison.Ordinal))
            {
                Flush();
                var m = Entry.Match(raw);
                if (!m.Success)
                {
                    findings.Add(Finding.Fail("hypothesis.entry", file,
                        $"line {i + 1}: an entry is '- <kind> | YYYY-MM-DDTHH:MM' with kind created, evidence, iteration or baselined"));
                    continue;
                }
                kind = m.Groups["kind"].Value; ts = m.Groups["ts"].Value; rest = m.Groups["rest"].Value; line = i + 1;
                if (kind is not ("created" or "evidence" or "iteration" or "baselined"))
                {
                    findings.Add(Finding.Fail("hypothesis.entry", file, $"line {i + 1}: '{kind}' is not an entry kind"));
                    kind = null;
                }
                continue;
            }
            if (raw.StartsWith("  ", StringComparison.Ordinal) && kind is not null) { cont.Add(raw); continue; }
            findings.Add(Finding.Fail("hypothesis.entry", file,
                $"line {i + 1}: neither an entry nor a two-space continuation of one"));
        }
        Flush();
        return entries;
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
/// schemas/study-registry-schema.md: id · type · corpus · go, appended at Brian's go; the id
/// <c>&lt;type&gt;-of-&lt;corpus&gt;-&lt;slug&gt;</c>, its type the one the prefix names
/// (verification, exploration, audit), its corpus a name from the corpora file,
/// verified-artifacts, or skill for an audit (d-2026-09-08-2, d-2026-09-09-12).
/// </summary>
public static class Registry
{
    public const string VerifiedArtifacts = "verified-artifacts";
    public const string Skill = "skill";
    public static readonly string[] Types = ["verification", "exploration", "audit"];

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();

        IReadOnlyList<MarkdownTable> tables;
        try { tables = MapTables.ReadAll(File.ReadAllText(path)); }
        catch (MapFormatException ex) { return [Finding.Fail("registry.table", file, ex.Message)]; }

        var table = tables.FirstOrDefault(t => t.Headers.Select(h => h.ToLowerInvariant()).SequenceEqual(["id", "type", "corpus", "go"]));
        if (table is null) return [Finding.Fail("registry.table", file, "no table with columns id | type | corpus | go")];

        var known = new HashSet<string>(ctx.CorporaIds, StringComparer.Ordinal) { VerifiedArtifacts };
        if (ctx.CorporaIds.Count == 0)
            findings.Add(Finding.Info("registry.corpora-unavailable", file,
                "no corpus ids could be read from the skill folder; corpus names are not checked"));

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var row in table.Rows)
        {
            var (id, type, corpus, go) = (row.Cells[0], row.Cells[1], row.Cells[2], row.Cells[3]);
            if (!seen.Add(id)) findings.Add(Finding.Fail("registry.duplicate", file, $"line {row.Line}: {id} appears twice"));
            if (!SchemaCheckers.IsoDate.IsMatch(go)) findings.Add(Finding.Fail("registry.go", file, $"line {row.Line}: go is an ISO date; found '{go}'"));
            if (!ClosedSets.IdPattern.IsMatch(id))
            {
                findings.Add(Finding.Fail("registry.id", file, $"line {row.Line}: '{id}' is not a lowercase slug"));
                continue;
            }

            string? expectedType = null, idCorpus = null;
            if (id.StartsWith("audit-of-", StringComparison.Ordinal))
            {
                expectedType = "audit";
                idCorpus = Skill;
                if (id.Length == "audit-of-".Length) findings.Add(Finding.Fail("registry.id", file, $"line {row.Line}: '{id}' is audit-of-<slug>"));
            }
            else if (id.StartsWith("exploration-of-", StringComparison.Ordinal))
            {
                expectedType = "exploration";
                idCorpus = CorpusOf(id["exploration-of-".Length..], known, slugRequired: false);
            }
            else if (id.StartsWith("verification-of-", StringComparison.Ordinal))
            {
                expectedType = "verification";
                idCorpus = CorpusOf(id["verification-of-".Length..], known, slugRequired: true);
            }
            else
            {
                findings.Add(Finding.Fail("registry.id", file,
                    $"line {row.Line}: '{id}' is verification-of-<corpus>-<slug>, exploration-of-<corpus>[-<slug>] or audit-of-<slug>"));
                continue;
            }

            if (type != expectedType)
                findings.Add(Finding.Fail("registry.type", file, $"line {row.Line}: {id} is {expectedType}; found '{type}'"));

            if (idCorpus is null)
            {
                if (ctx.CorporaIds.Count > 0)
                    findings.Add(Finding.Fail("registry.corpus", file,
                        $"line {row.Line}: '{id}' names no known corpus followed by a slug; the ids are [{string.Join(" ", known.OrderBy(k => k))}]"));
            }
            else if (corpus != idCorpus)
                findings.Add(Finding.Fail("registry.corpus", file, $"line {row.Line}: the corpus cell is '{idCorpus}' for {id}; found '{corpus}'"));
        }
        return findings;
    }

    /// <summary>The corpus an id's remainder names: the longest known id, with a slug after it where required.</summary>
    static string? CorpusOf(string remainder, HashSet<string> known, bool slugRequired)
    {
        if (known.Count == 0) return remainder; // unchecked: reported as information elsewhere
        var candidates = known.Where(c => remainder == c || remainder.StartsWith(c + "-", StringComparison.Ordinal)).OrderByDescending(c => c.Length).ToList();
        if (candidates.Count == 0) return null;
        var corpus = candidates[0];
        var hasSlug = remainder.Length > corpus.Length + 1;
        if (slugRequired && !hasSlug) return null;
        return corpus;
    }
}

/// <summary>schemas/leads-schema.md: titled by its study, five sections in order (d-2026-09-08-19); the class is `leads` since d-2026-09-09-22.</summary>
public static class Leads
{
    public static readonly string[] Sections = ["Method", "Questions in view", "Leads", "Proposed questions", "Corrections"];

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var study = Path.GetFileName(Path.GetDirectoryName(path)!)!;
        var findings = new List<Finding>();
        var outline = new MarkdownOutline(File.ReadAllText(path));

        var title = outline.Headings.FirstOrDefault();
        var expected = $"{study} — leads";
        if (title is null || title.Level != 1 || title.Text != expected)
            findings.Add(Finding.Fail("leads.title", file,
                $"the title is '# {expected}'; found " + (title is null ? "no heading" : $"'{title.Text}'")));

        var h2 = outline.Headings.Where(h => h.Level == 2).Select(h => h.Text).ToList();
        if (!h2.SequenceEqual(Sections, StringComparer.Ordinal))
            findings.Add(Finding.Fail("leads.sections", file,
                $"the sections are [{string.Join(", ", Sections)}]; found [{string.Join(", ", h2)}]"));
        return findings;
    }
}

/// <summary>
/// schemas/corpora-schema.md: one section per corpus, its id as the heading, then what, where and
/// read-by lines, then caveats. Also the source of corpus ids for the registry: the section
/// headings of CORPORA.md.
/// </summary>
public static class Corpora
{
    public const string FileName = "CORPORA.md";
    static readonly string[] Fields = ["- what:", "- where:", "- read by:"];

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();
        var text = File.ReadAllText(path);
        var outline = new MarkdownOutline(text);
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var h in outline.Headings.Where(h => h.Level == 2))
        {
            if (!ClosedSets.IdPattern.IsMatch(h.Text))
            {
                findings.Add(Finding.Fail("corpora.section", file, $"line {h.Line}: a section heading is a corpus id, a lowercase slug; found '{h.Text}'"));
                continue;
            }
            if (!seen.Add(h.Text))
                findings.Add(Finding.Fail("corpora.duplicate", file, $"line {h.Line}: {h.Text} appears twice"));

            var body = StateBuilder.Section(text, h.Text).Split('\n').Where(l => l.Trim().Length > 0).Take(3).ToList();
            for (var i = 0; i < Fields.Length; i++)
            {
                var ok = i < body.Count && body[i].StartsWith(Fields[i], StringComparison.Ordinal)
                         && body[i][Fields[i].Length..].Trim().Length > 0;
                if (!ok)
                {
                    findings.Add(Finding.Fail("corpora.fields", file,
                        $"{h.Text}: the first three lines are '- what:', '- where:', '- read by:', each with a value"));
                    break;
                }
            }
        }
        if (seen.Count == 0) findings.Add(Finding.Fail("corpora.section", file, "no corpus sections"));
        return findings;
    }

    /// <summary>The corpus ids the skill folder declares, or an empty set when the file is absent.</summary>
    public static IReadOnlySet<string> Ids(string skillFolder)
    {
        var corpora = Path.Combine(skillFolder, FileName);
        if (!File.Exists(corpora)) return new HashSet<string>(StringComparer.Ordinal);
        return new MarkdownOutline(File.ReadAllText(corpora)).Headings
            .Where(h => h.Level == 2 && ClosedSets.IdPattern.IsMatch(h.Text))
            .Select(h => h.Text).ToHashSet(StringComparer.Ordinal);
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
/// schemas/question-entry-schema.md on the engine: the entries and their typed fields, the
/// hypotheses ids resolved by the engine; the class's own rules are the title with the file's
/// own corpus, the heading as the citation token with a slug unique in the list, dates that
/// never go backwards, and the appended withdrawn line, at most once, beneath the fields.
/// </summary>
public static class Questions
{
    public const string SchemaId = "question-entry-schema";
    public static readonly string[] Keys = ["date", "hypotheses", "raised by", "question", "suggested test"];
    static readonly Regex WithdrawnLine = new(@"^- withdrawn: (?<date>\d{4}-\d{2}-\d{2}) (?<reason>\S.*)$", RegexOptions.Compiled);

    /// <summary>One entry as read; Date is empty when the date line was not exact.</summary>
    public sealed record Entry(string Slug, int Line, string Date, IReadOnlyList<string> Hypotheses, bool Withdrawn);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path) => Read(ctx, path).Findings;

    /// <summary>The entries of one list in order, and every finding against the schema.</summary>
    public static (IReadOnlyList<Entry> Entries, IReadOnlyList<Finding> Findings) Read(CheckContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var corpus = Path.GetFileNameWithoutExtension(path);
        var findings = new List<Finding>();
        var entries = new List<Entry>();

        var engine = EngineCheck.Run(SchemaId, ctx, path);
        if (engine.ShapeUnavailable) { findings.Add(EngineCheck.Unavailable(SchemaId, file)); return (entries, findings); }
        foreach (var p in engine.Problems)
        {
            if (p.Key == "withdrawn") continue; // the appended line, held below
            var id = p.Key == "date" && p.Kind is ProblemKind.Form or ProblemKind.Type ? "question.entry.date"
                : p.Key == "hypotheses" && p.Kind is ProblemKind.Form or ProblemKind.Type or ProblemKind.Reference ? "question.hypotheses"
                : "question.entry.fields";
            findings.Add(Finding.Fail(id, file, p.Message));
        }
        var doc = engine.Document;
        var title = $"{corpus} — questions";
        if (doc.Title != title)
            findings.Add(Finding.Fail("question.title", file, $"the title is '# {title}', the file's own name"));
        if (ctx.CorporaIds.Count == 0)
            findings.Add(Finding.Info("question.corpora-unavailable", file, "no corpus ids could be read from the skill folder; the corpus is not checked"));
        else if (!ctx.CorporaIds.Contains(corpus))
            findings.Add(Finding.Fail("question.title", file, $"'{corpus}' is not a corpus id in CORPORA.md"));
        if (References.FilesOf(WellKnown.HypothesisStatus, ctx) is null)
            findings.Add(Finding.Info("question.hypotheses-unavailable", file, "no hypothesis class could be located from the artifacts table; hypothesis ids are not checked"));

        // ---- the withdrawn lines, by position in the file ----
        var lines = SchemaCheckers.Lines(path);
        var withdrawnOf = new Dictionary<int, int>(); // entry heading line → count
        var positions = doc.Entries.Where(e => e.Section == "body").ToList();
        for (var i = 0; i < lines.Length; i++)
        {
            if (!lines[i].StartsWith("- withdrawn:", StringComparison.Ordinal)) continue;
            var owner = positions.LastOrDefault(e => e.Line < i + 1);
            if (!WithdrawnLine.IsMatch(lines[i]))
                findings.Add(Finding.Fail("question.withdrawn", file, $"line {i + 1}: a withdrawn line is '- withdrawn: YYYY-MM-DD <reason>'"));
            if (owner is null) continue;
            var fieldsAfter = lines.Skip(i + 1).TakeWhile(l => !l.StartsWith("### ", StringComparison.Ordinal))
                .Any(l => StoryPlanner.BatchFiles.KeyedLines.IsKeyedLine(l) && !l.StartsWith("- withdrawn:", StringComparison.Ordinal));
            var fieldsBefore = owner.FieldLines.Values.Any(l => l < i + 1);
            if (!fieldsBefore)
                findings.Add(Finding.Fail("question.withdrawn", file, $"line {i + 1}: withdrawn sits beneath the fields, never before them"));
            else if (fieldsAfter)
                findings.Add(Finding.Fail("question.withdrawn", file, $"line {i + 1}: a keyed line after withdrawn; withdrawn is the last line of an entry"));
            withdrawnOf[owner.Line] = withdrawnOf.GetValueOrDefault(owner.Line) + 1;
        }
        foreach (var (line, count) in withdrawnOf.Where(kv => kv.Value > 1))
            findings.Add(Finding.Fail("question.withdrawn", file, $"line {line}: withdrawn appears twice; a question is withdrawn once"));

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
            if (prefix != corpus)
                findings.Add(Finding.Fail("question.slug", file, $"line {line}: the heading is '{corpus}/<slug>', the file's own corpus then the slug; found '{heading}'"));
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
            var ids = obj["hypotheses"] is JsonArray h ? h.Select(x => x!.GetValue<string>()).ToList() : [];
            entries.Add(new Entry(slug, line, dateValue, ids, withdrawnOf.ContainsKey(line)));
        }
        return (entries, findings.DistinctBy(f => (f.CheckId, f.Message)).ToList());
    }
}
