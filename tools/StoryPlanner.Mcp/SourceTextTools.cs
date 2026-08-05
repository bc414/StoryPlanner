using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using ModelContextProtocol.Server;
using StoryPlanner.Core;

namespace StoryPlanner.Mcp;

/// <summary>
/// Retrieval over the published source material the plan cites — MLP:FiM episode transcripts,
/// Equestria at War flavour text, and the fanfics tracked as Works. A fourth corpus, joined to
/// the plan only through (Work Name, Part Code).
///
/// These tools answer "what does the cited text say", never "what should you take from it".
/// There is no ranking, no relevance ordering, no suggestion of which Part to mine next: results
/// come back in document order, and an uncited or untexted Part is reported as coverage rather
/// than as a queue position. Having the full canon in reach makes the opposite tempting, which is
/// exactly why it is stated here.
/// </summary>
[McpServerToolType]
public sealed class SourceTextTools(StoryPlanSources sources, SourceTextStore store)
{
    private const int DefaultWindow = 40_000;

    [McpServerTool(Name = "list_source_texts")]
    [Description(
        "Coverage of the source-text corpus: which Works and Parts have their text available, how " +
        "much of it, and which do not. Pass work to get the flat per-Part list for one Work. " +
        "A Part with no text is ordinary — the fic is ongoing, the episode was never transcribed — " +
        "and is reported as coverage, never as a defect or a suggestion about what to acquire next.")]
    public string ListSourceTexts(
        [Description("Work name exactly as it appears in the plan (\"FiM\", \"EaW\", \"P&K\", \"Pax Chrysalia\"). Omit for a summary of every Work.")]
        string? work = null)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        var c = sources.Get(Corpus.Working);
        var manifest = store.Manifest();
        var sb = new StringBuilder();

        if (work is not null)
        {
            var planWork = c.SourceMaterials.FirstOrDefault(w => w.Name.Equals(work, StringComparison.OrdinalIgnoreCase));
            if (planWork is null)
                return $"No Work named \"{work}\" in the working plan. " +
                       "Names must match exactly — see list_source_materials. Refusing to guess which Work was meant.";

            var parts = c.SourceMaterialPartsByWork.TryGetValue(planWork.Id, out var ps) ? ps : [];
            var units = manifest.Where(u => u.WorkName.Equals(planWork.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            var byCode = units.GroupBy(u => u.PartCode, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

            sb.AppendLine($"# source texts — {planWork.Name}: {byCode.Count} of {parts.Count} Part(s) have text " +
                          $"({units.Count} unit(s), {units.Sum(u => (long)u.CharCount):N0} chars)");
            var partNoun = planWork.PartNoun.Length > 0 ? planWork.PartNoun : "Part";
            sb.AppendLine($"({partNoun} order is the plan's, not a reading recommendation.)");
            sb.AppendLine();

            foreach (var p in parts.OrderBy(p => p.OrderIndex))
            {
                var label = p.Name.Length > 0 ? $"{p.Code} — {p.Name}" : p.Code;
                if (byCode.TryGetValue(p.Code, out var us))
                    sb.AppendLine($"  {label}: {us.Sum(u => (long)u.CharCount):N0} chars, " +
                                  $"{us.Count} unit(s) (sourcepart:{p.Id}, sourcetext:{string.Join(",", us.Select(u => u.Id))})");
                else if (SectionCount(byCode.Keys, p.Code) is var n and > 0)
                    sb.AppendLine($"  {label}: (split into {n} section Part(s) — its text lives under those) (sourcepart:{p.Id})");
                else
                    sb.AppendLine($"  {label}: (no text) (sourcepart:{p.Id})");
            }
            return Query.Cap(sb);
        }

        sb.AppendLine($"# source texts — {manifest.Select(u => u.WorkName).Distinct().Count()} work(s), " +
                      $"{manifest.Count:N0} unit(s), {manifest.Sum(u => (long)u.CharCount):N0} chars");
        sb.AppendLine("(the text a citation points at; a separate corpus, joined to the plan only by Work name + Part code)");
        sb.AppendLine();

        foreach (var w in c.SourceMaterials.OrderBy(w => w.OrderIndex))
        {
            var parts = c.SourceMaterialPartsByWork.TryGetValue(w.Id, out var ps) ? ps : [];
            var units = manifest.Where(u => u.WorkName.Equals(w.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            var covered = units.Select(u => u.PartCode).Distinct(StringComparer.OrdinalIgnoreCase).Count();

            sb.AppendLine($"## {w.Name} — {covered} of {parts.Count} Part(s) have text");
            if (units.Count == 0)
            {
                sb.AppendLine("  (no text ingested for this Work)");
                continue;
            }
            sb.AppendLine($"  {units.Count:N0} unit(s), {units.Sum(u => (long)u.CharCount):N0} chars, " +
                          $"kind: {string.Join("/", units.Select(u => u.Kind).Distinct().OrderBy(k => k))}");

            // A Part whose sections were promoted to Parts of their own is covered, not empty.
            // Listing it as "no text" would read as a failed acquisition rather than the split
            // it is — the same distinction the ingest report draws.
            var codes = units.Select(u => u.PartCode).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var withoutOwnText = parts.Where(p => !codes.Contains(p.Code)).ToList();
            var split = withoutOwnText.Where(p => SectionCount(codes, p.Code) > 0).ToList();
            var textless = withoutOwnText.Except(split).ToList();

            if (split.Count > 0)
                sb.AppendLine($"  split into section Parts ({split.Count}): {string.Join(", ", split.Select(p => p.Code))}");
            if (textless.Count > 0)
                sb.AppendLine($"  no text ({textless.Count}): " +
                              Query.Truncate(string.Join(", ", textless.Select(p => p.Code)), 400));
            sb.AppendLine($"  -> list_source_texts(work: \"{w.Name}\") for the per-Part list");
        }
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "search_source_texts")]
    [Description(
        "Regex search across the source-text corpus — the index pass. Returns one line per hit with " +
        "its ids and a snippet, in DOCUMENT ORDER (never ranked by relevance or frequency). Narrow " +
        "with work/part/kind rather than expecting the best matches first, then read the full text " +
        "with get_source_text. The caller supplies the vocabulary; this tool does no fuzzy matching " +
        "and no stemming.")]
    public string SearchSourceTexts(
        [Description("Regex pattern. Alternation is the way to cover spelling variants — e.g. \"Wonderbolts?|Spitfire\".")]
        string pattern,
        [Description("Restrict to one Work (\"FiM\", \"EaW\", \"P&K\", \"Pax Chrysalia\").")]
        string? work = null,
        [Description("Restrict to one Part code (\"S3E01\", \"GRI\", \"ch121-queens-scientist\").")]
        string? part = null,
        [Description("Restrict to one kind: transcript | prose | flavor.")]
        string? kind = null,
        [Description("Case-sensitive matching. Default false.")]
        bool caseSensitive = false,
        [Description("Wrap the pattern in word boundaries. Default false.")]
        bool wholeWord = false,
        [Description("Max hits to list (1-250, default 40). The total is always reported even when hits are dropped.")]
        int limit = 40,
        [Description("Characters of context around each hit (20-2000, default 200).")]
        int contextChars = 200)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        limit = Math.Clamp(limit, 1, 250);
        contextChars = Math.Clamp(contextChars, 20, 2000);

        Regex rx;
        try { rx = Query.BuildRegex(pattern, caseSensitive, wholeWord); }
        catch (ArgumentException ex) { return $"Invalid regex: {ex.Message}"; }

        var c = sources.Get(Corpus.Working);
        var lines = new List<string>();
        var totalHits = 0;
        var unitsWithHits = 0;
        var perWork = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        try
        {
            foreach (var (unit, body) in store.Stream(work, part, kind))
            {
                var matches = rx.Matches(body);
                if (matches.Count == 0) continue;

                unitsWithHits++;
                totalHits += matches.Count;
                perWork[unit.WorkName] = perWork.GetValueOrDefault(unit.WorkName) + matches.Count;

                // Counters above increment before the cap below, so a truncated listing still
                // reports the true totals rather than the number that happened to fit.
                if (lines.Count >= limit) continue;

                lines.Add($"{Label(unit)} ({Refs(c, unit)})");
                lines.Add($"   {Query.Snippet(body, matches[0], contextChars)}");
                if (matches.Count > 1) lines.Add($"   (+{matches.Count - 1} more hit(s) in this unit)");
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return "Regex timed out (2s) — simplify the pattern.";
        }

        var sb = new StringBuilder();
        var scope = string.Join(", ", new[]
        {
            work is not null ? $"work={work}" : null,
            part is not null ? $"part={part}" : null,
            kind is not null ? $"kind={kind}" : null
        }.Where(s => s is not null));

        sb.AppendLine($"# search_source_texts \"{pattern}\"{(scope.Length > 0 ? $" [{scope}]" : "")} — " +
                      $"{totalHits} hit(s) in {unitsWithHits} unit(s). Showing first {Math.Min(unitsWithHits, limit)}.");
        if (perWork.Count > 1)
            sb.AppendLine($"by work: {string.Join(", ", perWork.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}={kv.Value}"))}");
        sb.AppendLine("(document order — not ranked)");
        sb.AppendLine();
        foreach (var l in lines) sb.AppendLine(l);
        if (unitsWithHits == 0)
            sb.AppendLine("(no matches — the corpus holds only what has been ingested; see list_source_texts for coverage)");
        return Query.Cap(sb);
    }

    [McpServerTool(Name = "get_source_text")]
    [Description(
        "Fetches full source text by id — the fetch pass after search_source_texts. Supports a " +
        "windowed read via offset/length because a single fic chapter can exceed the whole output " +
        "budget (the largest here is over 120,000 characters); the truncation notice names the " +
        "offset to continue from. Pass work+part instead of ids to fetch every unit of one Part.")]
    public string GetSourceText(
        [Description("sourcetext ids, as emitted by search_source_texts / list_source_texts.")]
        int[]? ids = null,
        [Description("Work name — with part, fetches that Part's units instead of using ids.")]
        string? work = null,
        [Description("Part code — with work, fetches that Part's units instead of using ids.")]
        string? part = null,
        [Description("Start character offset into each unit's body. Default 0.")]
        int offset = 0,
        [Description("Characters to return per unit (500-50000, default 40000).")]
        int length = DefaultWindow)
    {
        if (!store.IsConfigured) return store.NotConfiguredMessage;

        offset = Math.Max(0, offset);
        length = Math.Clamp(length, 500, 50_000);

        var resolved = ids?.Distinct().ToList() ?? [];
        if (resolved.Count == 0)
        {
            if (work is null || part is null)
                return "Pass either ids, or both work and part. " +
                       "Refusing to guess: fetching an unintended Part wastes the output budget the window exists to protect.";
            resolved = store.Manifest()
                .Where(u => u.WorkName.Equals(work, StringComparison.OrdinalIgnoreCase)
                            && u.PartCode.Equals(part, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.OrderIndex).Select(u => u.Id).ToList();
            if (resolved.Count == 0)
                return $"No source text for {work}·{part}. See list_source_texts(work: \"{work}\") for what is covered.";
        }

        var c = sources.Get(Corpus.Working);
        var sb = new StringBuilder();
        var missing = new List<int>();
        var found = 0;

        foreach (var id in resolved)
        {
            var hit = store.Fetch(id, offset, length);
            if (hit is null) { missing.Add(id); continue; }
            var (unit, body, sourceRef) = hit.Value;
            found++;

            sb.AppendLine($"## {Label(unit)} ({Refs(c, unit)})");
            sb.AppendLine($"{unit.CharCount:N0} chars total | kind:{unit.Kind} | {sourceRef}");
            sb.AppendLine();
            sb.AppendLine(body);
            var end = offset + body.Length;
            if (end < unit.CharCount)
                sb.AppendLine($"\n[WINDOWED — {unit.CharCount - end:N0} more chars. " +
                              $"Continue: get_source_text(ids: [{unit.Id}], offset: {end})]");
            sb.AppendLine();
        }

        var header = $"# get_source_text — {found} unit(s) returned" +
                     (missing.Count > 0 ? $", {missing.Count} id(s) not found" : "") +
                     (offset > 0 ? $" (from offset {offset})" : "");
        sb.Insert(0, header + "\n\n");
        foreach (var m in missing) sb.AppendLine($"— not found (sourcetext:{m})");
        return Query.Cap(sb);
    }

    /// <summary>
    /// How many unit codes are sections promoted out of this Part ("ch121" -> "ch121-father").
    /// Prefix-derived rather than configured, because the MCP server reads sources.db without the
    /// ingest config that named the split; the ingest itself uses the config and never guesses.
    /// </summary>
    private static int SectionCount(IEnumerable<string> unitCodes, string partCode) =>
        unitCodes.Count(c => c.StartsWith($"{partCode}-", StringComparison.OrdinalIgnoreCase));

    /// <summary>"FiM·S2E01 — The Return of Harmony Part 1" — the citation form, plus the label.</summary>
    private static string Label(SourceTextStore.Unit u)
    {
        var head = $"{u.WorkName}·{u.PartCode}";
        if (u.UnitKey.Length > 0) head += $"#{u.UnitKey}";
        return u.UnitLabel.Length > 0 ? $"{head} — {u.UnitLabel}" : head;
    }

    /// <summary>
    /// The callable ids, trailing. sourcepart is emitted only when the Part still resolves in the
    /// plan — sources.db is a separate file keyed by code, so a Part renamed or removed there
    /// leaves text that is readable but no longer citable, and saying so beats inventing an id.
    /// </summary>
    private static string Refs(PlanCache c, SourceTextStore.Unit u)
    {
        var refs = $"sourcetext:{u.Id}";
        var work = c.SourceMaterials.FirstOrDefault(w => w.Name.Equals(u.WorkName, StringComparison.OrdinalIgnoreCase));
        if (work is null) return refs + ", no matching Work in plan";
        var part = (c.SourceMaterialPartsByWork.TryGetValue(work.Id, out var ps) ? ps : [])
            .FirstOrDefault(p => p.Code.Equals(u.PartCode, StringComparison.OrdinalIgnoreCase));
        return part is null ? refs + ", no matching Part in plan" : $"{refs}, sourcepart:{part.Id}";
    }
}
