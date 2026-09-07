using System.Text.RegularExpressions;

namespace StoryPlanner.DocIntegrity;

/// <summary>
/// What a record checker sees besides the file: the repository root, the governing skill
/// folder, and the corpus ids a registry row may name. Built once per check by
/// <see cref="ArtifactScope"/>; tests build it directly.
/// </summary>
public sealed record RecordContext(string RepoRoot, string SkillFolder, IReadOnlySet<string> CorporaIds)
{
    public static RecordContext From(string repoRoot, string skillFolder)
        => new(repoRoot, skillFolder, Corpora.Ids(skillFolder));
}

/// <summary>One artifact class's format, applied to one file. Findings name the file as their row.</summary>
public delegate IReadOnlyList<Finding> RecordChecker(RecordContext ctx, string path);

/// <summary>
/// The checkers that exist, by artifact id. A class gets its checker when its first instance
/// is written (decisions.md, 2026-09-06): the five here are what the re-founding of the
/// hypothesis records writes. An id with no checker is silence, never a failure.
/// </summary>
public static class RecordCheckers
{
    public static RecordChecker? For(string artifactId) => artifactId switch
    {
        _ when WellKnown.HypothesisArtifacts.Contains(artifactId) => HypothesisFile.Check,
        WellKnown.HypothesisIndex => HypothesisIndex.Check,
        WellKnown.Instances => Registry.Check,
        WellKnown.LeadsArtifact => Leads.Check,
        WellKnown.Corpora => Corpora.Check,
        _ => null,
    };

    /// <summary>The artifact ids that dispatch to a checker, one per class (the three hypothesis rows count once).</summary>
    public static readonly string[] CheckedIds =
        [WellKnown.HypothesisStatus, WellKnown.HypothesisIndex, WellKnown.Instances, WellKnown.LeadsArtifact, WellKnown.Corpora];

    internal static string[] Lines(string path) => File.ReadAllText(path).Replace("\r\n", "\n").Split('\n');

    internal static readonly Regex IsoDate = new(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled);
}

/// <summary>
/// artifacts.md § Hypothesis file. Shape from the fifty real files (four frontmatter keys, two
/// sections, four entry kinds, minute-precision timestamps); rules from decisions: an evidence
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
        @"^ \| \((?<instance>[a-z0-9-]+) (?<candidate>C-\d+); (?<codebook>[^@\s]+)@(?<hash>[0-9a-f]{6,64})\) \[(?<tag>supporting|challenging)\]:",
        RegexOptions.Compiled);

    static readonly Regex FileName = new(@"^(?<id>\d{3})-[a-z0-9-]+\.md$", RegexOptions.Compiled);

    public sealed record RecordEntry(string Kind, string Timestamp, string Rest, int Line, IReadOnlyList<string> Continuation);

    public static IReadOnlyList<Finding> Check(RecordContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var findings = new List<Finding>();
        var lines = RecordCheckers.Lines(path);

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
        if (baselined != "false" && !RecordCheckers.IsoDate.IsMatch(baselined))
            findings.Add(Finding.Fail("hypothesis.frontmatter", file, $"baselined is false or an ISO date; found '{baselined}'"));

        var created = fm.GetValueOrDefault("created", "");
        if (!RecordCheckers.IsoDate.IsMatch(created))
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
                        $"line {e.Line}: an evidence entry cites (<instance> <C-id>; <codebook>@<hash>) [supporting|challenging]; " +
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

        if (baselined != "false" && RecordCheckers.IsoDate.IsMatch(baselined))
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

/// <summary>artifacts.md § Hypothesis index: two columns, id order, every file listed, every link resolving.</summary>
public static class HypothesisIndex
{
    static readonly Regex Link = new(@"^\[(?<slug>[a-z0-9-]+)\]\((?<file>\d{3}-[a-z0-9-]+\.md)\)$", RegexOptions.Compiled);

    public static IReadOnlyList<Finding> Check(RecordContext ctx, string path)
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
/// artifacts.md § Instance registry: id · type · corpus · go, appended at Brian's go; ids of
/// three forms; the corpus a name from the corpora file, verified-artifacts, or candidates.
/// </summary>
public static class Registry
{
    public const string VerifiedArtifacts = "verified-artifacts";
    public const string Candidates = "candidates";
    static readonly Regex Referee = new(@"^referee-(\d+)$", RegexOptions.Compiled);

    public static IReadOnlyList<Finding> Check(RecordContext ctx, string path)
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
            if (!RecordCheckers.IsoDate.IsMatch(go)) findings.Add(Finding.Fail("registry.go", file, $"line {row.Line}: go is an ISO date; found '{go}'"));

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

/// <summary>artifacts.md § Leads artifact: titled by its instance, six sections in order.</summary>
public static class Leads
{
    public static readonly string[] Sections = ["Method", "Questions in view", "Leads", "Bins", "Proposed questions", "Corrections"];

    public static IReadOnlyList<Finding> Check(RecordContext ctx, string path)
    {
        var file = Path.GetFileName(path);
        var instance = Path.GetFileName(Path.GetDirectoryName(path)!)!;
        var findings = new List<Finding>();
        var outline = new MarkdownOutline(File.ReadAllText(path));

        var title = outline.Headings.FirstOrDefault();
        var expected = $"{instance} — leads";
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
/// artifacts.md § Corpora: one section per corpus, its id as the heading, then what, where and
/// read-by lines, then caveats. Also the source of corpus ids for the registry: the sections
/// of CORPORA.md, or until it exists, the id table at the top of CORPUS-STATUS.md.
/// </summary>
public static class Corpora
{
    public const string FileName = "CORPORA.md";
    public const string LegacyFileName = "CORPUS-STATUS.md";
    static readonly string[] Fields = ["- what:", "- where:", "- read by:"];

    public static IReadOnlyList<Finding> Check(RecordContext ctx, string path)
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

    /// <summary>The corpus ids the skill folder declares, or an empty set when neither file declares any.</summary>
    public static IReadOnlySet<string> Ids(string skillFolder)
    {
        var corpora = Path.Combine(skillFolder, FileName);
        if (File.Exists(corpora))
            return new MarkdownOutline(File.ReadAllText(corpora)).Headings
                .Where(h => h.Level == 2 && ClosedSets.IdPattern.IsMatch(h.Text))
                .Select(h => h.Text).ToHashSet(StringComparer.Ordinal);

        var legacy = Path.Combine(skillFolder, LegacyFileName);
        if (!File.Exists(legacy)) return new HashSet<string>(StringComparer.Ordinal);
        try
        {
            var table = MapTables.ReadAll(File.ReadAllText(legacy))
                .FirstOrDefault(t => t.Headers.Count >= 1 && t.Headers[0].Equals("id", StringComparison.OrdinalIgnoreCase));
            return table is null
                ? new HashSet<string>(StringComparer.Ordinal)
                : table.Rows.Select(r => r.Cells[0]).ToHashSet(StringComparer.Ordinal);
        }
        catch (MapFormatException)
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }
    }
}
