using System.Globalization;
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
/// The checkers that exist, by artifact id. A class gets its checker when its first file is
/// written (decisions.md, 2026-09-06): five are what the re-founding of the hypothesis
/// files writes, and the decisions file's came with its schema (2026-09-07). An id with no
/// checker is silence, never a failure.
/// </summary>
public static class SchemaCheckers
{
    public static SchemaChecker? For(string artifactId) => artifactId switch
    {
        _ when WellKnown.HypothesisArtifacts.Contains(artifactId) => HypothesisFile.Check,
        WellKnown.HypothesisIndex => HypothesisIndex.Check,
        WellKnown.Studies => Registry.Check,
        WellKnown.LeadsArtifact => Leads.Check,
        WellKnown.Corpora => Corpora.Check,
        WellKnown.Decisions => Decisions.Check,
        _ => null,
    };

    /// <summary>The artifact ids that dispatch to a checker, one per class (the three hypothesis rows count once).</summary>
    public static readonly string[] CheckedIds =
        [WellKnown.HypothesisStatus, WellKnown.HypothesisIndex, WellKnown.Studies, WellKnown.LeadsArtifact, WellKnown.Corpora, WellKnown.Decisions];

    internal static string[] Lines(string path) => File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');

    internal static readonly Regex IsoDate = new(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled);
}

/// <summary>
/// schemas/hypothesis-file-schema.md. Shape from the fifty real files (four frontmatter keys, two
/// sections, four entry kinds, minute-precision timestamps); checks from decisions: an evidence
/// entry is written only by a promotion from a referee-checked candidate and carries its
/// citation and falsifier (decisions d-2026-09-05-3, d-2026-09-06-2), an iteration entry is a
/// wording boundary and status is computed from the entries after the last one
/// (d-2026-09-05-4), baselining is Brian's dated entry and resets on a challenge
/// (d-2026-09-05-1). One file, three artifacts, one checker.
/// </summary>
public static class HypothesisFile
{
    static readonly string[] Keys = ["id", "status", "baselined", "created"];
    static readonly string[] Statuses = ["untested", "evidenced", "challenged"];

    static readonly Regex Entry = new(
        @"^- (?<kind>[a-z]+) \| (?<ts>\d{4}-\d{2}-\d{2}T\d{2}:\d{2})(?<rest>.*)$", RegexOptions.Compiled);

    static readonly Regex Citation = new(
        @"^ \| \((?<study>[a-z0-9-]+) (?<candidate>C-\d+); (?<codebook>[^@\s]+)@(?<hash>[0-9a-f]{6,64})\) \[(?<tag>supporting|challenging)\]:",
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
                        $"line {e.Line}: an evidence entry cites (<study> <C-id>; <codebook>@<hash>) [supporting|challenging]; " +
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
        catch (MapFormatException ex) { return [Finding.Fail("index.table", file, ex.Message)]; }

        var table = tables.FirstOrDefault(t => t.Headers.Select(h => h.ToLowerInvariant()).SequenceEqual(["id", "slug"]));
        if (table is null) return [Finding.Fail("index.table", file, "no table with columns ID | Slug")];

        var listed = new HashSet<string>(StringComparer.Ordinal);
        var lastId = -1;
        foreach (var row in table.Rows)
        {
            if (!int.TryParse(row.Cells[0], out var id) || row.Cells[0].Length != 3)
            {
                findings.Add(Finding.Fail("index.link", file, $"line {row.Line}: the id is NNN; found '{row.Cells[0]}'"));
                continue;
            }
            if (id <= lastId)
                findings.Add(Finding.Fail("index.order", file, $"line {row.Line}: ids ascend; {row.Cells[0]} follows {lastId:000}"));
            lastId = id;

            var m = Link.Match(row.Cells[1]);
            if (!m.Success || m.Groups["file"].Value != $"{row.Cells[0]}-{m.Groups["slug"].Value}.md")
            {
                findings.Add(Finding.Fail("index.link", file, $"line {row.Line}: the slug cell is [slug](NNN-slug.md) with the same NNN"));
                continue;
            }
            listed.Add(m.Groups["file"].Value);
            if (!File.Exists(Path.Combine(dir, m.Groups["file"].Value)))
                findings.Add(Finding.Fail("index.link", file, $"line {row.Line}: {m.Groups["file"].Value} does not exist"));
        }

        foreach (var f in Directory.GetFiles(dir, "*.md").Select(Path.GetFileName).OrderBy(n => n, StringComparer.Ordinal))
            if (f is not null && Regex.IsMatch(f, @"^\d{3}-") && !listed.Contains(f))
                findings.Add(Finding.Fail("index.missing", file, $"{f} has no row"));

        return findings;
    }
}

/// <summary>
/// schemas/study-registry-schema.md: id · type · corpus · go, appended at Brian's go; ids of
/// three forms; the corpus a name from the corpora file, verified-artifacts, or candidates.
/// </summary>
public static class Registry
{
    public const string VerifiedArtifacts = "verified-artifacts";
    public const string Candidates = "candidates";
    static readonly Regex Referee = new(@"^referee-(\d+)$", RegexOptions.Compiled);

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

            string? expectedType = null, idCorpus = null;
            if (Referee.IsMatch(id)) { expectedType = "verification"; idCorpus = Candidates; }
            else if (id.StartsWith("exploration-of-", StringComparison.Ordinal))
            {
                expectedType = "exploratory";
                idCorpus = CorpusOf(id["exploration-of-".Length..], known, ordinalRequired: false);
            }
            else if (id.StartsWith("round-of-", StringComparison.Ordinal))
            {
                expectedType = "verification";
                idCorpus = CorpusOf(id["round-of-".Length..], known, ordinalRequired: true);
            }
            else
            {
                findings.Add(Finding.Fail("registry.id", file,
                    $"line {row.Line}: '{id}' is exploration-of-<corpus>[-n], round-of-<corpus>-n or referee-n"));
                continue;
            }

            if (type != expectedType)
                findings.Add(Finding.Fail("registry.type", file, $"line {row.Line}: {id} is {expectedType}; found '{type}'"));

            if (idCorpus is null)
            {
                if (ctx.CorporaIds.Count > 0)
                    findings.Add(Finding.Fail("registry.corpus", file,
                        $"line {row.Line}: '{id}' names no known corpus; the ids are [{string.Join(" ", known.OrderBy(k => k))}]"));
            }
            else if (corpus != idCorpus)
                findings.Add(Finding.Fail("registry.corpus", file, $"line {row.Line}: the corpus cell is '{idCorpus}' for {id}; found '{corpus}'"));
        }
        return findings;
    }

    /// <summary>The corpus an id's remainder names: the longest known id, with an ordinal after it where required.</summary>
    static string? CorpusOf(string remainder, HashSet<string> known, bool ordinalRequired)
    {
        var m = Regex.Match(remainder, @"^(?<c>.+?)(?:-(?<n>\d+))?$");
        var c = m.Groups["c"].Value;
        var hasOrdinal = m.Groups["n"].Success;
        if (known.Count == 0) return remainder; // unchecked: reported as information elsewhere
        if (hasOrdinal && known.Contains(c)) return c;
        if (!ordinalRequired && known.Contains(remainder)) return remainder;
        return null;
    }
}

/// <summary>schemas/leads-artifact-schema.md: titled by its study, six sections in order.</summary>
public static class Leads
{
    public static readonly string[] Sections = ["Method", "Questions in view", "Leads", "Bins", "Proposed questions", "Corrections"];

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
/// schemas/decisions-schema.md: a "# Decisions" title, a free head, sections "## Revision N" that
/// ascend and open with at most one paragraph, then entries: a "### " title stating the ruling
/// and keyed "- key: value" lines in the order id, date, supersedes, prompted by, decision,
/// not taken, a value continued by two-space indent. The three machine-read values are exact:
/// the id is d-&lt;date&gt;-&lt;n&gt;, written with the entry, its date the entry's own and its
/// n one more than the previous entry of that date or 1, and a wrong id fails naming the
/// expected one; the date is YYYY-MM-DD and never earlier than the entry before; supersedes
/// is ids separated by single spaces, each an earlier entry of the same file that no other
/// entry supersedes. Nothing derives an id and the tool never writes one.
/// </summary>
public static class Decisions
{
    public const string Title = "Decisions";
    public static readonly string[] Keys = ["id", "date", "supersedes", "prompted by", "decision", "not taken"];
    static readonly string[] Required = ["id", "date", "prompted by", "decision", "not taken"];
    static readonly Regex SectionHeading = new(@"^Revision (?<n>[1-9]\d*)$", RegexOptions.Compiled);
    static readonly Regex Keyed = new(@"^- (?<key>[a-z][a-z ]*): (?<value>.*)$", RegexOptions.Compiled);
    public static readonly Regex Id = new(@"^d-(?<date>\d{4}-\d{2}-\d{2})-(?<n>[1-9]\d*)$", RegexOptions.Compiled);

    /// <summary>One entry with the id the shape expects for it; only entries whose date line is exact get one.</summary>
    public sealed record Entry(string Id, string Title, int Line, string Date, IReadOnlyList<string> Supersedes);

    sealed record Field(string Key, string Value, int Line, List<string> Continuation);

    public static IReadOnlyList<Finding> Check(CheckContext ctx, string path) => Read(path).Findings;

    /// <summary>The entries of a decisions file in order, and every finding against the schema.</summary>
    public static (IReadOnlyList<Entry> Entries, IReadOnlyList<Finding> Findings) Read(string path)
    {
        var file = Path.GetFileName(path);
        var lines = SchemaCheckers.Lines(path);
        var findings = new List<Finding>();
        var entries = new List<Entry>();

        var first = Array.FindIndex(lines, l => l.Trim().Length > 0);
        if (first < 0 || lines[first] != "# " + Title)
            findings.Add(Finding.Fail("decisions.shape", file, $"the first line is '# {Title}'"));

        var sectionN = 0;
        var inSection = false;
        var leadParagraphs = 0;
        var inParagraph = false;

        string? title = null;
        var titleLine = 0;
        var fields = new List<Field>();
        var stray = new List<int>();

        var ids = new HashSet<string>(StringComparer.Ordinal);
        var perDate = new Dictionary<string, int>(StringComparer.Ordinal);
        var superseded = new Dictionary<string, int>(StringComparer.Ordinal);
        string? lastDate = null;

        void Flush()
        {
            if (title is null) return;
            var keys = fields.Select(f => f.Key).ToList();

            foreach (var f in fields.Where(f => !Keys.Contains(f.Key)))
                findings.Add(Finding.Fail("decisions.entry.fields", file,
                    $"line {f.Line}: '{f.Key}' is not a key; the keys are {string.Join(", ", Keys)}"));
            foreach (var dup in keys.GroupBy(k => k).Where(g => g.Count() > 1))
                findings.Add(Finding.Fail("decisions.entry.fields", file, $"line {titleLine}: '{dup.Key}' appears twice"));
            var missing = Required.Where(k => !keys.Contains(k)).ToList();
            if (missing.Count > 0)
                findings.Add(Finding.Fail("decisions.entry.fields", file,
                    $"line {titleLine}: missing {string.Join(", ", missing)}"));
            var order = keys.Where(Keys.Contains).Select(k => Array.IndexOf(Keys, k)).ToList();
            if (order.Zip(order.Skip(1)).Any(p => p.Second <= p.First))
                findings.Add(Finding.Fail("decisions.entry.fields", file,
                    $"line {titleLine}: the keys are in the order {string.Join(", ", Keys)}"));
            foreach (var f in fields.Where(f => f.Value.Trim().Length == 0))
                findings.Add(Finding.Fail("decisions.entry.fields", file, $"line {f.Line}: '{f.Key}' has no value on its line"));
            if (stray.Count > 0)
                findings.Add(Finding.Fail("decisions.entry.fields", file,
                    $"line {titleLine}: {stray.Count} line(s) neither keyed nor a two-space continuation, first at line {stray[0]}"));

            string? id = null;
            var date = fields.FirstOrDefault(f => f.Key == "date");
            if (date is not null)
            {
                var exact = SchemaCheckers.IsoDate.IsMatch(date.Value)
                            && DateOnly.TryParseExact(date.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                if (date.Continuation.Count > 0)
                    findings.Add(Finding.Fail("decisions.entry.date", file, $"line {date.Line}: the date is one line"));
                if (!exact)
                    findings.Add(Finding.Fail("decisions.entry.date", file, $"line {date.Line}: the date is exactly YYYY-MM-DD; found '{date.Value}'"));
                else
                {
                    if (lastDate is not null && string.CompareOrdinal(date.Value, lastDate) < 0)
                        findings.Add(Finding.Fail("decisions.entry.date", file,
                            $"line {date.Line}: {date.Value} is earlier than the entry before it, {lastDate}"));
                    lastDate = date.Value;
                    perDate[date.Value] = perDate.GetValueOrDefault(date.Value) + 1;
                    id = $"d-{date.Value}-{perDate[date.Value]}";
                }
            }

            var written = fields.FirstOrDefault(f => f.Key == "id");
            if (written is not null && id is not null)
            {
                if (written.Continuation.Count > 0)
                    findings.Add(Finding.Fail("decisions.entry.id", file, $"line {written.Line}: the id is one line"));
                if (written.Value != id)
                    findings.Add(Finding.Fail("decisions.entry.id", file,
                        $"line {written.Line}: the id is {id}, the entry's date and the next number of that date; found '{written.Value}'"));
            }

            var targets = new List<string>();
            var sup = fields.FirstOrDefault(f => f.Key == "supersedes");
            if (sup is not null)
            {
                if (sup.Continuation.Count > 0)
                    findings.Add(Finding.Fail("decisions.supersedes", file, $"line {sup.Line}: supersedes is one line"));
                var tokens = sup.Value.Split(' ');
                if (tokens.Any(t => !Id.IsMatch(t)))
                    findings.Add(Finding.Fail("decisions.supersedes", file,
                        $"line {sup.Line}: ids d-YYYY-MM-DD-n separated by single spaces and nothing else; found '{sup.Value}'"));
                else
                    foreach (var t in tokens)
                    {
                        if (targets.Contains(t))
                            findings.Add(Finding.Fail("decisions.supersedes", file, $"line {sup.Line}: {t} is named twice"));
                        else if (!ids.Contains(t))
                            findings.Add(Finding.Fail("decisions.supersedes", file,
                                $"line {sup.Line}: {t} is not an entry earlier in this file"));
                        else if (superseded.TryGetValue(t, out var by))
                            findings.Add(Finding.Fail("decisions.supersedes", file,
                                $"line {sup.Line}: {t} is already superseded by the entry at line {by}; supersession is whole and never chains"));
                        else
                        {
                            targets.Add(t);
                            superseded[t] = titleLine;
                        }
                    }
            }

            if (id is not null)
            {
                ids.Add(id);
                entries.Add(new Entry(id, title, titleLine, date!.Value, targets));
            }
            title = null;
        }

        for (var i = first + 1; i < lines.Length; i++)
        {
            var raw = lines[i];
            var blank = raw.Trim().Length == 0;

            if (raw.StartsWith("# ", StringComparison.Ordinal))
            {
                Flush();
                findings.Add(Finding.Fail("decisions.shape", file, $"line {i + 1}: one title, '# {Title}', at the top"));
                continue;
            }
            if (raw.StartsWith("## ", StringComparison.Ordinal))
            {
                Flush();
                var m = SectionHeading.Match(raw[3..]);
                if (!m.Success)
                    findings.Add(Finding.Fail("decisions.shape", file, $"line {i + 1}: a section is '## Revision N'; found '{raw[3..]}'"));
                else
                {
                    var n = int.Parse(m.Groups["n"].Value);
                    if (n <= sectionN)
                        findings.Add(Finding.Fail("decisions.shape", file, $"line {i + 1}: sections ascend; Revision {n} follows Revision {sectionN}"));
                    sectionN = n;
                }
                inSection = true;
                leadParagraphs = 0;
                inParagraph = false;
                continue;
            }
            if (raw.StartsWith("### ", StringComparison.Ordinal))
            {
                Flush();
                if (!inSection)
                    findings.Add(Finding.Fail("decisions.shape", file, $"line {i + 1}: an entry outside a section; the head holds none"));
                title = raw[4..].Trim();
                titleLine = i + 1;
                fields = [];
                stray = [];
                if (title.Length == 0)
                    findings.Add(Finding.Fail("decisions.shape", file, $"line {i + 1}: an entry's title states the ruling in one line"));
                continue;
            }

            if (title is not null)
            {
                if (blank) continue;
                if (raw.StartsWith("  ", StringComparison.Ordinal))
                {
                    if (fields.Count == 0) stray.Add(i + 1);
                    else fields[^1].Continuation.Add(raw);
                    continue;
                }
                var km = Keyed.Match(raw);
                if (km.Success) fields.Add(new Field(km.Groups["key"].Value, km.Groups["value"].Value, i + 1, []));
                else stray.Add(i + 1);
                continue;
            }

            if (inSection)
            {
                if (blank) { inParagraph = false; continue; }
                if (!inParagraph)
                {
                    leadParagraphs++;
                    inParagraph = true;
                    if (leadParagraphs == 2)
                        findings.Add(Finding.Fail("decisions.shape", file,
                            $"line {i + 1}: a section opens with at most one paragraph, then entries"));
                }
            }
            // Before the first section: the head, free prose.
        }
        Flush();
        return (entries, findings);
    }
}
