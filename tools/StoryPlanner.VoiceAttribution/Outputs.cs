using System.Globalization;
using System.Text;
using StoryPlanner.Core;

namespace StoryPlanner.VoiceAttribution;

public static class Outputs
{
    // ---------- attribution.csv ----------

    public static readonly string[] Columns =
    {
        "NoteId", "Content", "OwnerType", "OwnerName", "Story", "Chapter", "ChapterOrder", "PlotPointId", "OrderInChapter",
        "LinkId", "SubjectId", "NoteState", "Words", "Shingles", "Coverage", "TokenCoverage", "Label", "Role",
        "OriginLayer", "OriginId", "OriginDate", "OriginShare", "Sources", "LongestRun", "Tie",
        "PlanFirst", "EchoedBy", "EchoCandidate", "FirstSnapshot", "OriginPassage", "UncoveredText", "SourceWindow", "SourcePrompt",
        "MatchedSpans",
    };

    /// <summary>Every run credited to a MODEL-role source, as "start-end:sourceId" character offsets into Content. Runs matching Brian's own prompts are his and are not listed.</summary>
    private static string Spans(Row r) =>
        string.Join(';', r.Match.Spans.Where(s => s.Source.Role == "model").Select(s => $"{s.Start}-{s.End}:{s.Source.Id}"));

    public static void WriteCsv(string path, List<Row> rows)
    {
        using var w = new StreamWriter(path, false, new UTF8Encoding(false));
        w.WriteLine(string.Join(',', Columns));
        var inv = CultureInfo.InvariantCulture;
        foreach (var r in rows)
        {
            var m = r.Match;
            w.WriteLine(string.Join(',', new[]
            {
                r.Note.Id.ToString(), Q(r.Note.Content), r.OwnerTypeName, Q(r.OwnerName), Q(r.Story), Q(r.Chapter),
                r.ChapterOrder.ToString(), r.PlotPointId.ToString(), r.OrderInChapter.ToString(), r.LinkId.ToString(), r.SubjectId.ToString(),
                PlanReader.StateName(r.Note.State), m.Words.ToString(), m.Shingles.ToString(),
                m.Coverage?.ToString("0.000", inv) ?? "", m.TokenCoverage.ToString("0.000", inv), r.Label, r.Role,
                r.OriginLayer, r.OriginId, r.OriginDate,
                m.Origin is null ? "" : m.OriginShare.ToString("0.00", inv), Q(r.Sources), m.LongestRunWords.ToString(),
                m.Tie ? "true" : "false", r.PlanFirst ? "true" : "false", r.EchoedBy, r.EchoCandidate ? "true" : "false",
                Q(r.FirstSnapshot), Q(Trunc(m.OriginPassage, 300)), Q(Trunc(m.UncoveredText, 300)), Q(r.SourceWindow), Q(Trunc(r.SourcePrompt.Replace('\n', ' '), 200)),
                Q(Spans(r)),
            }));
        }
    }

    private static string Q(string s)
    {
        if (s.IndexOfAny(new[] { ',', '"', '\n', '\r' }) < 0) return s;
        return "\"" + s.Replace("\"", "\"\"") + "\"";
    }

    private static string Trunc(string s, int n) => s.Length <= n ? s : s[..n] + "…";

    // ---------- calibration sample ----------

    public static void WriteSample(string path, List<Row> population, string[] labels, int perLabel, int seed, int flipsToSample,
        Settings settings, int k, SourceContext ctx)
    {
        var rng = new Random(seed);
        var t = settings.Labels;
        var sb = new StringBuilder();
        sb.AppendLine("# Calibration sample — voice attribution");
        sb.AppendLine();
        sb.AppendLine($"{perLabel} notes per label ({string.Join(", ", labels)}), seeded random (seed {seed}){(flipsToSample > 0 ? $", plus every echo candidate and {flipsToSample} PlanFirst flips" : "")}. Type after `verdict:` on any entry — first word from: ok · brian · model · mixed · phrase · wrong-source · boilerplate · ? — then anything. Blank = no objection.");
        sb.AppendLine();
        sb.AppendLine("## Rulings");
        sb.AppendLine();
        sb.AppendLine("```");
        sb.AppendLine($"R (lift run, words)       provisional: {t.R}          ruling:");
        sb.AppendLine($"G (authored gap, words)   provisional: {t.G}          ruling:");
        sb.AppendLine($"verbatim token coverage   provisional: {t.VerbatimCoverage:0.00}       ruling:");
        sb.AppendLine($"paste scale (words)       provisional: {t.PasteWords}         ruling:");
        sb.AppendLine($"k (window size)           provisional: {k}          ruling:");
        sb.AppendLine("```");
        sb.AppendLine();

        var picked = new HashSet<int>();
        foreach (var label in labels)
        {
            var pool = population.Where(r => r.Label == label).ToList();
            var pick = pool.OrderBy(_ => rng.Next()).Take(perLabel).OrderBy(r => r.ChapterOrder).ThenBy(r => r.Note.Id).ToList();
            sb.AppendLine($"# {label}  ({pick.Count} of {pool.Count})");
            sb.AppendLine();
            int i = 0;
            foreach (var r in pick) { picked.Add(r.Note.Id); Entry(sb, r, label, ++i, ctx); }
        }
        if (flipsToSample > 0)
        {
            var echoes = population.Where(r => r.EchoCandidate && !picked.Contains(r.Note.Id)).OrderBy(r => r.ChapterOrder).ThenBy(r => r.Note.Id).ToList();
            sb.AppendLine($"# echo candidates — every one not already sampled  ({echoes.Count})");
            sb.AppendLine();
            int i = 0;
            foreach (var r in echoes) { picked.Add(r.Note.Id); Entry(sb, r, "echo", ++i, ctx); }

            var flipPool = population.Where(r => r.PlanFirst && !picked.Contains(r.Note.Id)).ToList();
            var flips = flipPool.OrderBy(_ => rng.Next()).Take(flipsToSample).OrderBy(r => r.ChapterOrder).ThenBy(r => r.Note.Id).ToList();
            sb.AppendLine($"# PlanFirst flips  ({flips.Count} of {flipPool.Count})");
            sb.AppendLine();
            i = 0;
            foreach (var r in flips) Entry(sb, r, "planfirst", ++i, ctx);
        }
        File.WriteAllText(path, sb.ToString(), new UTF8Encoding(false));
    }

    private static void Entry(StringBuilder sb, Row r, string group, int i, SourceContext ctx)
    {
        var m = r.Match;
        var inv = CultureInfo.InvariantCulture;
        sb.AppendLine($"### {group} {i:00} · note {r.Note.Id} · {r.OwnerTypeName}{(r.Chapter.Length > 0 ? " · " + r.Chapter : "")} · \"{r.OwnerName}\"");
        sb.AppendLine($"label **{r.Label}** · role **{(r.Role.Length > 0 ? r.Role : "—")}** · token coverage {m.TokenCoverage.ToString("0.00", inv)} · longest run {m.LongestRunWords}w · longest gap {m.LongestUncoveredWords}w · origin {(m.Origin is null ? "—" : $"{r.OriginId} ({r.OriginLayer}/{m.Origin.Role}, {r.OriginDate}, share {m.OriginShare:0.00})")} · first snapshot {(string.IsNullOrEmpty(r.FirstSnapshot) ? "—" : r.FirstSnapshot)}{(r.FirstSnapshotDate is DateOnly fd ? $" ({fd:yyyy-MM-dd})" : "")}{(r.PlanFirst ? $" · **PlanFirst → brian**, echoed by {r.EchoedBy}" : "")}{(r.EchoCandidate ? " · **ECHO CANDIDATE**" : "")}{(m.Tie ? " · **TIE**" : "")}");
        if (!string.IsNullOrEmpty(r.Sources)) sb.AppendLine($"sources: {r.Sources}");
        sb.AppendLine();
        string originText = "", prompt = "";
        if (m.Origin is not null) (originText, prompt) = ctx.Fetch(r.OriginId, m.Origin.Role);
        sb.AppendLine("NOTE (every run a model-role source contains in **bold** — same marking as scan.html):");
        foreach (var line in Highlight(r.Note.Content, m).Split('\n')) sb.AppendLine("> " + line);
        sb.AppendLine();
        if (m.Origin is not null)
        {
            sb.AppendLine($"SOURCE {r.OriginId} — {r.OriginLayer}/**{m.Origin.Role}** {r.OriginDate}{(string.IsNullOrEmpty(prompt) ? "" : $" · the prompt it answered: \"{Trunc(prompt.Replace('\n', ' '), 160)}\"")}");
            foreach (var line in SourceContext.Window(originText, m.OriginPassage.Length > 0 ? m.OriginPassage : m.MatchedPassage).Split('\n')) sb.AppendLine("> " + line);
            sb.AppendLine();
        }
        if (!string.IsNullOrEmpty(m.UncoveredText) && m.LongestUncoveredWords >= 3)
            sb.AppendLine($"UNCOVERED (longest run no source explains, {m.LongestUncoveredWords}w): \"{Trunc(m.UncoveredText, 300)}\"\n");
        sb.AppendLine("verdict:");
        sb.AppendLine();
    }

    /// <summary>Bolds every span credited to a model-role source (any source, not just the origin) — the same marking scan.html draws from MatchedSpans.</summary>
    private static string Highlight(string note, VoiceMatchResult m)
    {
        var spans = m.Spans.Where(s => s.Source.Role == "model").OrderBy(s => s.Start).ToList();
        if (spans.Count == 0) return note;
        var sb = new StringBuilder();
        int cursor = 0;
        foreach (var s in spans)
        {
            if (s.Start < cursor) continue;
            sb.Append(note, cursor, s.Start - cursor).Append("**").Append(note, s.Start, s.End - s.Start).Append("**");
            cursor = s.End;
        }
        sb.Append(note, cursor, note.Length - cursor);
        return sb.ToString();
    }

    // ---------- render (reading order) ----------

    public static void Render(string dir, PlanReader plan, List<Row> rows, List<(string Name, int From, int To)> arcs, IReadOnlySet<int> subjectIds, string? excludeStory, TextWriter manifest)
    {
        Directory.CreateDirectory(dir);
        var byNote = rows.ToDictionary(r => r.Note.Id);
        var notesByOwner = plan.Notes.GroupBy(n => (n.OwnerType, n.OwnerId)).ToDictionary(g => g.Key, g => g.OrderBy(n => n.Id).ToList());
        var linksByPp = plan.Links.Values.GroupBy(l => l.PlotPointId).ToDictionary(g => g.Key, g => g.OrderBy(l => l.Id).ToList());
        var ppsByChapter = plan.PlotPoints.Values.Where(p => p.ChapterId is not null).GroupBy(p => p.ChapterId!.Value).ToDictionary(g => g.Key, g => g.OrderBy(p => p.OrderInChapter).ToList());

        string Prefix(PlanReader.Note n)
        {
            var r = byNote[n.Id];
            var origin = r.Match.Origin is null ? "—" : $"{r.OriginLayer}/{r.Role} {r.OriginDate} {r.OriginId}{(r.PlanFirst ? " PlanFirst" : "")}{(r.EchoCandidate ? " Echo" : "")}{(r.Match.Tie ? " TIE" : "")}";
            return $"[{n.Id} | {r.Label} | {origin} | {(string.IsNullOrEmpty(r.FirstSnapshot) ? "—" : r.FirstSnapshot)}]";
        }
        // Bold = AI text, and nothing else: model-role spans on rows the tool still attributes to
        // a model. Rows ruled Brian's (PlanFirst / echo) and phrase rows render plain; the prefix
        // carries the provenance.
        string Body(PlanReader.Note n)
        {
            var r = byNote[n.Id];
            bool claim = r.Role == "model" && r.Label is not (VoiceLabel.Phrase or VoiceLabel.None or VoiceLabel.Short);
            return claim ? Highlight(n.Content, r.Match) : n.Content;
        }
        void Notes(StringBuilder sb, int ownerType, int ownerId, string indent)
        {
            if (!notesByOwner.TryGetValue((ownerType, ownerId), out var list)) return;
            foreach (var n in list)
            {
                if (n.State == 1) continue; // flagged: ignored in this WU by decision
                var text = Body(n).Replace("\r", "").Replace("\n", "\n" + indent + "  ");
                sb.AppendLine($"{indent}- {Prefix(n)} {text}");
            }
        }

        manifest.WriteLine("# Read manifest — what the reading view contains");
        manifest.WriteLine();
        int Count(int ownerType, int ownerId) => notesByOwner.TryGetValue((ownerType, ownerId), out var l) ? l.Count(n => n.State != 1) : 0;

        var chaptersByStory = plan.Chapters.Values.GroupBy(c => c.StoryId).ToDictionary(g => g.Key, g => g.OrderBy(c => c.OrderIndex).ToList());
        foreach (var (storyId, chapters) in chaptersByStory)
        {
            var storyName = plan.StoryName(storyId);
            if (excludeStory is not null && storyName.Contains(excludeStory, StringComparison.OrdinalIgnoreCase))
            {
                manifest.WriteLine($"## {storyName} — EXCLUDED (not rendered)");
                manifest.WriteLine();
                continue;
            }
            var storyArcs = arcs.Count > 0 && chapters.Count > 1 ? arcs : new List<(string, int, int)> { (Slug(storyName), int.MinValue, int.MaxValue) };
            foreach (var (arcName, from, to) in storyArcs)
            {
                var arcChapters = chapters.Where(c => c.OrderIndex >= from && c.OrderIndex <= to).ToList();
                if (arcChapters.Count == 0) continue;
                var sb = new StringBuilder();
                sb.AppendLine($"# {storyName} — {arcName}");
                sb.AppendLine();
                sb.AppendLine("Note prefix: `[id | label | origin layer/role date id | first snapshot]`. **Bold** = text the tool attributes to an AI source (model-role spans). Rows marked PlanFirst / Echo are Brian's (the model echoed the plan) and render plain. Labels and origins are the tool's mechanical output (see attribution.csv).");
                sb.AppendLine();
                manifest.WriteLine($"## {storyName} — {arcName} (`{arcName}.md`)");
                foreach (var ch in arcChapters)
                {
                    sb.AppendLine($"## CH#{ch.OrderIndex} \"{ch.Title}\" (chapter:{ch.Id})");
                    sb.AppendLine();
                    var chNotes = Count(PlanReader.OwnerChapter, ch.Id);
                    if (chNotes > 0) { sb.AppendLine("### chapter notes"); Notes(sb, PlanReader.OwnerChapter, ch.Id, ""); sb.AppendLine(); }
                    int ppTotal = 0, linkTotal = 0, linkCount = 0;
                    foreach (var pp in ppsByChapter.GetValueOrDefault(ch.Id, new()))
                    {
                        var ppNotes = Count(PlanReader.OwnerPlotPoint, pp.Id);
                        ppTotal += ppNotes;
                        sb.AppendLine($"### PP \"{pp.Title}\" (pp:{pp.Id}, pos {pp.OrderInChapter})");
                        Notes(sb, PlanReader.OwnerPlotPoint, pp.Id, "");
                        foreach (var l in linksByPp.GetValueOrDefault(pp.Id, new()))
                        {
                            linkCount++;
                            var ln = Count(PlanReader.OwnerLink, l.Id);
                            linkTotal += ln;
                            var subj = plan.Subjects.TryGetValue(l.SubjectId, out var s) ? s.Name : $"subject:{l.SubjectId}";
                            sb.AppendLine($"#### × {subj} (link:{l.Id}, subject:{l.SubjectId}) — {ln} notes");
                            Notes(sb, PlanReader.OwnerLink, l.Id, "  ");
                        }
                        sb.AppendLine();
                    }
                    manifest.WriteLine($"- CH#{ch.OrderIndex} \"{ch.Title}\": {chNotes} chapter notes, {ppsByChapter.GetValueOrDefault(ch.Id, new()).Count} plot points ({ppTotal} notes), {linkCount} links ({linkTotal} notes)");
                }
                manifest.WriteLine();
                File.WriteAllText(Path.Combine(dir, arcName + ".md"), sb.ToString(), new UTF8Encoding(false));
            }
        }

        manifest.WriteLine("## Subjects rendered");
        foreach (var sid in subjectIds.OrderBy(x => x))
        {
            if (!plan.Subjects.TryGetValue(sid, out var s)) { manifest.WriteLine($"- subject:{sid}: NOT FOUND"); continue; }
            var sb = new StringBuilder();
            sb.AppendLine($"# Subject \"{s.Name}\" (subject:{s.Id}) — [{s.SubjectType}]");
            sb.AppendLine();
            Notes(sb, PlanReader.OwnerSubject, s.Id, "");
            var edges = plan.Links.Values.Where(l => l.SubjectId == s.Id).Select(l => plan.PlotPoints.GetValueOrDefault(l.PlotPointId)).Where(p => p is not null)
                .Select(p => (p!, plan.Chapters.GetValueOrDefault(p!.ChapterId ?? -1))).OrderBy(t => t.Item2?.OrderIndex ?? 999).ThenBy(t => t.Item1.OrderInChapter).ToList();
            if (edges.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("## scene links (read their notes in the arc files)");
                foreach (var (pp, ch) in edges) sb.AppendLine($"- {(ch is null ? "(unplaced)" : $"CH#{ch.OrderIndex}")} \"{pp.Title}\" (pp:{pp.Id})");
            }
            File.WriteAllText(Path.Combine(dir, $"subject-{s.Id}.md"), sb.ToString(), new UTF8Encoding(false));
            manifest.WriteLine($"- subject:{s.Id} \"{s.Name}\" [{s.SubjectType}]: {Count(PlanReader.OwnerSubject, s.Id)} notes, {edges.Count} scene links (`subject-{s.Id}.md`)");
        }
    }

    private static string Slug(string s) => new string(s.ToLowerInvariant().Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray()).Trim('-');
}
