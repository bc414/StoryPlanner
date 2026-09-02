using System.Text.RegularExpressions;
using StoryPlanner.Core;

namespace StoryPlanner.VoiceAttribution;

/// <summary>Everything provisional until the calibration gate; every value is a CLI argument.</summary>
public sealed record Settings(
    LabelThresholds Labels,
    int MaxSources = 0,
    int MinWordsForSnapshot = 4,
    /// <summary>A source counts in the Sources column when it holds at least this many matched shingles or this share of them.</summary>
    int SourceMinShingles = 3,
    double SourceMinShare = 0.15);

/// <summary>One row of attribution.csv — a note with its mechanical provenance labels.</summary>
public sealed record Row(
    PlanReader.Note Note,
    string OwnerTypeName,
    string OwnerName,
    string Story,
    string Chapter,
    int ChapterOrder,
    int PlotPointId,
    int OrderInChapter,
    int LinkId,
    int SubjectId,
    VoiceMatchResult Match,
    string Label,
    string Role,
    bool PlanFirst,
    string EchoedBy,
    bool EchoCandidate,
    string FirstSnapshot,
    DateOnly? FirstSnapshotDate,
    string Sources,
    string SourceWindow,
    string SourcePrompt)
{
    public string OriginLayer => Match.Origin?.Layer ?? "";
    public string OriginId => Match.Origin?.Id ?? "";
    public string OriginDate => Match.Origin?.Date?.ToString("yyyy-MM-dd") ?? "";
    public bool IsPaste => Label is VoiceLabel.Verbatim or VoiceLabel.EditedPaste or VoiceLabel.FramedPaste;
    public bool IsLift => Label == VoiceLabel.Fragment;
    public int SourceCount => string.IsNullOrEmpty(Sources) ? 0 : Sources.Split(';').Length;
}

public static class Attribution
{
    /// <summary>
    /// The model citing the author's own plan back at him, in its own words. Matched against
    /// the source text in a window before the matched passage. This is not style detection —
    /// it is the model's citation phrasing, documented from the calibration sample
    /// ("In your notes under Coltbert's profile, you explicitly left a placeholder…").
    /// </summary>
    public static readonly Regex EchoLeadIn = new(
        @"\b(in|from|per|based on|according to)\s+your\s+(own\s+)?(notes?|plan|codex|lore bible|profile|outline|draft|text|document|story plan)\b|\byou(r notes?)?\s+(wrote|noted|mentioned|stated|specified|established|left a placeholder|ask yourself|explicitly)\b|\bas you (wrote|noted|mentioned|stated|put it)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static List<Row> Run(PlanReader plan, VoiceIndex index, PlanSnapshotIndex? snapshots, Settings s,
        Func<string, string, (string Text, string Prompt)> fetchSource, Action<string> log)
    {
        var rows = new List<Row>(plan.Notes.Count);
        int done = 0, flipped = 0, echoes = 0;
        foreach (var n in plan.Notes)
        {
            var m = index.Match(n.Content, s.MaxSources);
            var label = VoiceLabel.Of(m, s.Labels);
            string firstSnap = "", role = m.Origin?.Role ?? "", echoed = "";
            DateOnly? firstDate = null;
            bool planFirst = false, echoCandidate = false;

            if (snapshots is not null)
            {
                var (verdict, first) = snapshots.FirstAppearance(n.Content, s.MinWordsForSnapshot);
                firstSnap = verdict switch
                {
                    PlanSnapshotIndex.Verdict.Found => first!.Name,
                    PlanSnapshotIndex.Verdict.Absent => "absent",
                    _ => "ambiguous",
                };
                firstDate = first?.Date;
                if (m.Origin is { Role: "model", Date: DateOnly od } && firstDate is DateOnly fd && fd < od)
                {
                    planFirst = true; echoed = m.Origin.Id; role = "brian"; flipped++;
                }
            }

            string window = "", prompt = "";
            if (m.Origin is not null && (m.OriginPassage.Length > 0 || m.MatchedPassage.Length > 0))
            {
                var (text, p) = fetchSource(m.Origin.Id, m.Origin.Role);
                var passage = m.OriginPassage.Length > 0 ? m.OriginPassage : m.MatchedPassage;
                window = SourceContext.Window(text, passage, 40);
                prompt = p;
                // Echo: the model citing the plan back ("in your notes…") just before reproducing
                // it, inside the snapshot week where PlanFirst cannot date it. Treated like
                // PlanFirst (Brian, 2026-09-02): role becomes brian, the response is recorded.
                if (m.Origin.Role == "model" && !planFirst && SourceContext.LeadInBefore(text, passage, 400) is string before && EchoLeadIn.IsMatch(before))
                {
                    echoCandidate = true; echoes++; echoed = m.Origin.Id; role = "brian";
                }
            }

            var sources = string.Join(';', m.Votes
                .Where(v => v.Shingles >= s.SourceMinShingles || (double)v.Shingles / Math.Max(1, m.Matched) >= s.SourceMinShare)
                .Select(v => $"{v.Source.Id}/{v.Source.Role}×{v.Shingles}"));

            var ch = plan.ChapterOf(n);
            int ppId = 0, order = 0, linkId = 0, subjectId = 0;
            switch (n.OwnerType)
            {
                case PlanReader.OwnerPlotPoint:
                    ppId = n.OwnerId; order = plan.PlotPoints.TryGetValue(ppId, out var pp) ? pp.OrderInChapter : 0; break;
                case PlanReader.OwnerLink:
                    linkId = n.OwnerId;
                    if (plan.Links.TryGetValue(linkId, out var l))
                    {
                        ppId = l.PlotPointId; subjectId = l.SubjectId;
                        order = plan.PlotPoints.TryGetValue(ppId, out var lp) ? lp.OrderInChapter : 0;
                    }
                    break;
                case PlanReader.OwnerSubject: subjectId = n.OwnerId; break;
            }

            rows.Add(new Row(n, plan.OwnerTypeName(n.OwnerType), plan.OwnerName(n),
                ch is null ? "" : plan.StoryName(ch.StoryId),
                ch is null ? "" : $"CH#{ch.OrderIndex} {ch.Title}",
                ch?.OrderIndex ?? 0, ppId, order, linkId, subjectId,
                m, label, role, planFirst, echoed, echoCandidate, firstSnap, firstDate, sources, window, prompt));
            if (++done % 1000 == 0) log($"  matched {done}/{plan.Notes.Count}");
        }
        log($"  PlanFirst flipped model → brian: {flipped}; echo candidates: {echoes}");
        return rows;
    }

    public static string Summary(List<Row> rows, string? excludeStory)
    {
        var sb = new System.Text.StringBuilder();
        var pop = excludeStory is null ? rows : rows.Where(r => !r.Story.Contains(excludeStory, StringComparison.OrdinalIgnoreCase)).ToList();
        string Cell(IEnumerable<Row> g, string label) => g.Count(r => r.Label == label).ToString().PadLeft(5);
        void Table(string title, IEnumerable<Row> src, Func<Row, string> key)
        {
            sb.AppendLine($"## by {title}");
            sb.AppendLine($"  {"",-30} {"n",5} " + string.Join(" ", VoiceLabel.All.Select(l => l.PadLeft(12))));
            foreach (var g in src.GroupBy(key).OrderBy(g => g.Key))
                sb.AppendLine($"  {g.Key,-30} {g.Count(),5} " + string.Join(" ", VoiceLabel.All.Select(l => Cell(g, l).PadLeft(12))));
        }
        Table($"label (all{(excludeStory is null ? "" : $", {excludeStory} excluded")})", pop, r => "all");
        Table("owner type", pop, r => r.OwnerTypeName);
        Table("origin layer/role (matched only)", pop.Where(r => r.Match.Origin is not null),
            r => $"{r.OriginLayer}/{r.Role}{(r.PlanFirst ? " (PlanFirst)" : "")}");
        sb.AppendLine("## counts for the deposits");
        sb.AppendLine($"  pastes (verbatim+edited+framed) by role: model={pop.Count(r => r.IsPaste && r.Role == "model")}  brian={pop.Count(r => r.IsPaste && r.Role == "brian")}");
        sb.AppendLine($"  lifts (fragment) by role:               model={pop.Count(r => r.IsLift && r.Role == "model")}  brian={pop.Count(r => r.IsLift && r.Role == "brian")}");
        sb.AppendLine($"  phrase (never counted): {pop.Count(r => r.Label == VoiceLabel.Phrase)}   none: {pop.Count(r => r.Label == VoiceLabel.None)}   short: {pop.Count(r => r.Label == VoiceLabel.Short)}");
        sb.AppendLine($"  PlanFirst flips: {pop.Count(r => r.PlanFirst)}   echo candidates: {pop.Count(r => r.EchoCandidate)}   stitched (≥2 sources): {pop.Count(r => r.SourceCount >= 2)}   ties: {pop.Count(r => r.Match.Tie)}");
        sb.AppendLine($"  first-snapshot absent: {pop.Count(r => r.FirstSnapshot == "absent")}   ambiguous: {pop.Count(r => r.FirstSnapshot == "ambiguous")}");
        return sb.ToString();
    }
}
