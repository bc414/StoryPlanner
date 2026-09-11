using System.Text.RegularExpressions;

namespace StoryPlanner.SurfacingItemizer;

/// <summary>
/// The item computation of the two surfacing itemizers, kept out of the CLI so it is tested
/// directly. Claiming (d-2026-09-10-6): one item per standing finding, the finding as the
/// call's message. Referee (d-2026-09-10-5): one item per (finding, target) claim, the target's
/// current statement and the finding, blind — nothing else. The referee item's locator is the
/// candidate's identity <c>&lt;finding-slug&gt; → &lt;target&gt;</c>, which compose-candidates
/// reads back; the id is a unique slug.
/// </summary>
public static class Itemizers
{
    public const char Arrow = '→';

    public sealed record Item(string Id, string Body, string Locator, string Description);
    public sealed record Finding(string Study, string Slug, string Text);

    // ---- claiming ----

    /// <summary>One item per standing finding; the finding text is the call's message.</summary>
    public static IReadOnlyList<Item> ClaimItems(string findingsText)
        => StandingFindings(findingsText)
            .Select(f => new Item(f.Slug, f.Text + "\n", $"{f.Study}/{f.Slug}", Truncate(f.Text)))
            .ToList();

    // ---- referee ----

    /// <summary>
    /// One item per (finding, target) claim: the target's current statement and the finding,
    /// blind. The locator carries the identity for compose-candidates; the item id is
    /// <c>&lt;finding-slug&gt;-&lt;NNN&gt;</c>, unique because the finding slug is unique in the
    /// study and the target's numeric id disambiguates the hypothesis.
    /// </summary>
    public static IReadOnlyList<Item> RefereeItems(
        IReadOnlyList<(string FindingSlug, string Target)> claims,
        IReadOnlyDictionary<string, string> findings,
        IReadOnlyDictionary<string, string> statements)
    {
        var items = new List<Item>();
        foreach (var (findingSlug, target) in claims)
        {
            var finding = findings.GetValueOrDefault(findingSlug, "");
            var statement = statements.GetValueOrDefault(target, "");
            var body = $"## Statement\n\n{statement}\n\n## Finding\n\n{finding}\n";
            items.Add(new Item($"{findingSlug}-{IdOf(target)}", body,
                $"{findingSlug} {Arrow} {target}", $"{findingSlug} against {target}"));
        }
        return items;
    }

    /// <summary>The numeric id of a target hypothesis file name <c>NNN-slug</c>.</summary>
    static string IdOf(string target)
    {
        var dash = target.IndexOf('-');
        return dash > 0 ? target[..dash] : target;
    }

    // ---- reverify (iterating-a-statement) ----

    public sealed record Evidence(string Token, string FindingText);

    /// <summary>
    /// One item per current-wording evidence entry (decisions d-2026-09-11-2, -3): each holds the
    /// proposed wording and that entry's frozen finding text, blind, for the referee to re-judge
    /// against the proposed wording. The locator is the entry's finding token, by which
    /// gate-and-commit maps a result back to the entry it re-verifies.
    /// </summary>
    public static IReadOnlyList<Item> ReverifyItems(string proposedWording, IReadOnlyList<Evidence> evidence)
    {
        var items = new List<Item>();
        foreach (var e in evidence)
        {
            var body = $"## Statement\n\n{proposedWording.Trim()}\n\n## Finding\n\n{e.FindingText}\n";
            items.Add(new Item(SlugOf(e.Token), body, e.Token, $"{SlugOf(e.Token)} against the proposed wording"));
        }
        return items;
    }

    static readonly Regex Entry = new(@"^- (?<kind>[a-z]+) \| ", RegexOptions.Compiled);
    static readonly Regex EvidenceCitation = new(@"^- evidence \| \S+ \| \((?<token>[a-z0-9-]+/[a-z0-9-]+);", RegexOptions.Compiled);

    /// <summary>
    /// The evidence entries bound to the current wording of a hypothesis file — those after the
    /// last <c>iteration</c> boundary in the record (as HypothesisFile computes) — with each
    /// entry's frozen finding text, its continuation lines up to the <c>Falsifier:</c> line.
    /// </summary>
    public static IReadOnlyList<Evidence> CurrentWordingEvidence(string fileText)
    {
        var lines = fileText.Replace("\r\n", "\n").Split('\n');
        var start = Array.FindIndex(lines, l => l.Trim() == "## Record");
        if (start < 0) return [];
        var entries = new List<(string Kind, string? Token, List<string> Cont)>();
        for (var i = start + 1; i < lines.Length; i++)
        {
            var l = lines[i];
            if (l.StartsWith("#", StringComparison.Ordinal)) break;
            if (l.StartsWith("- ", StringComparison.Ordinal))
            {
                var kind = Entry.Match(l) is { Success: true } m ? m.Groups["kind"].Value : "";
                var token = EvidenceCitation.Match(l) is { Success: true } c ? c.Groups["token"].Value : null;
                entries.Add((kind, token, []));
            }
            else if (l.StartsWith("  ", StringComparison.Ordinal) && entries.Count > 0)
                entries[^1].Cont.Add(l.Trim());
        }
        var lastIteration = -1;
        for (var i = 0; i < entries.Count; i++) if (entries[i].Kind == "iteration") lastIteration = i;
        var result = new List<Evidence>();
        for (var i = lastIteration + 1; i < entries.Count; i++)
        {
            var e = entries[i];
            if (e.Kind != "evidence" || e.Token is null) continue;
            var finding = e.Cont.TakeWhile(c => !c.StartsWith("Falsifier:", StringComparison.Ordinal)).ToList();
            result.Add(new Evidence(e.Token, string.Join('\n', finding).Trim()));
        }
        return result;
    }

    // ---- parsing ----

    static readonly Regex FindingHeading = new(@"^### (?<study>[a-z0-9-]+)/(?<slug>[a-z0-9-]+)$", RegexOptions.Compiled);

    /// <summary>The standing findings — neither withdrawn nor superseded — with their text, from a findings.md.</summary>
    public static IReadOnlyList<Finding> StandingFindings(string findingsText)
    {
        var lines = findingsText.Replace("\r\n", "\n").Split('\n');
        var all = new List<Finding>();
        var withdrawn = new HashSet<string>(StringComparer.Ordinal);
        var superseded = new HashSet<string>(StringComparer.Ordinal);
        var inFindings = false;
        string? study = null, slug = null;
        var block = new List<string>();
        var capturing = false;
        void Flush() { if (slug is not null) all.Add(new Finding(study!, slug, string.Join('\n', block).Trim())); slug = null; block = []; capturing = false; }
        foreach (var l in lines)
        {
            if (l.StartsWith("## ", StringComparison.Ordinal)) { Flush(); inFindings = l[3..].Trim() == "Findings"; continue; }
            if (!inFindings) continue;
            if (l.StartsWith("### ", StringComparison.Ordinal))
            {
                Flush();
                var m = FindingHeading.Match(l);
                if (m.Success) { study = m.Groups["study"].Value; slug = m.Groups["slug"].Value; }
                continue;
            }
            if (slug is null) continue;
            if (l.StartsWith("- finding:", StringComparison.Ordinal)) { block.Add(l["- finding:".Length..].Trim()); capturing = true; }
            else if (l.StartsWith("- withdrawn:", StringComparison.Ordinal)) { withdrawn.Add(slug); capturing = false; }
            else if (l.StartsWith("- supersedes:", StringComparison.Ordinal)) { superseded.Add(SlugOf(l["- supersedes:".Length..].Trim())); capturing = false; }
            else if (l.StartsWith("- ", StringComparison.Ordinal)) capturing = false;
            else if (capturing && l.StartsWith("  ", StringComparison.Ordinal)) block.Add(l.Trim());
        }
        Flush();
        return all.Where(f => !withdrawn.Contains(f.Slug) && !superseded.Contains(f.Slug)).ToList();
    }

    /// <summary>The <c>## Hypothesis</c> section text of a hypothesis file, up to the next heading.</summary>
    public static string HypothesisStatement(string fileText)
    {
        var lines = fileText.Replace("\r\n", "\n").Split('\n');
        var body = new List<string>();
        var inside = false;
        foreach (var l in lines)
        {
            if (l.StartsWith("## ", StringComparison.Ordinal)) { if (inside) break; inside = l[3..].Trim() == "Hypothesis"; continue; }
            if (inside) body.Add(l);
        }
        return string.Join('\n', body).Trim();
    }

    static string SlugOf(string token) { var s = token.IndexOf('/'); return s < 0 ? token : token[(s + 1)..]; }
    static string Truncate(string s) { s = s.Replace('\n', ' ').Trim(); return s.Length <= 60 ? s : s[..60] + "…"; }
}
