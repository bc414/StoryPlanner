using System.Collections.Generic;
using System.Linq;
using StoryPlanner.DocIntegrity;
using StoryPlanner.SurfacingItemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The surfacing itemizers' item computation (decisions d-2026-09-10-5, -6): claiming cuts a
/// verification's standing findings into one item per finding; refereeing cuts the claims into
/// one item per (finding, target), each the target's statement and the finding, blind. The
/// referee item's locator is the identity compose-candidates reads back.
/// </summary>
public class SurfacingItemizerTests
{
    const string Findings = """
        # verification-of-x-y — findings

        ## Findings

        ### verification-of-x-y/most-are-a
        - finding: Most notes are class a.
        - cites:
          - verification-of-x-y/01-full § class

        ### verification-of-x-y/gone
        - finding: This did not hold.
        - cites:
          - verification-of-x-y/01-full § class
        - withdrawn: 2026-09-22 the item shows otherwise

        ### verification-of-x-y/old
        - finding: Superseded finding.
        - cites:
          - verification-of-x-y/01-full § class

        ### verification-of-x-y/new
        - supersedes: verification-of-x-y/old
        - finding: The amended finding.
        - cites:
          - verification-of-x-y/01-full § class
        """;

    [Fact]
    public void Standing_findings_exclude_withdrawn_and_superseded()
    {
        var standing = Itemizers.StandingFindings(Findings);
        Assert.Equal(["most-are-a", "new"], standing.Select(f => f.Slug).ToArray());
        Assert.Equal("Most notes are class a.", standing[0].Text);
    }

    [Fact]
    public void Claim_items_are_one_per_standing_finding_with_the_finding_as_the_body()
    {
        var items = Itemizers.ClaimItems(Findings);
        Assert.Equal(["most-are-a", "new"], items.Select(i => i.Id).ToArray());
        Assert.Equal("verification-of-x-y/most-are-a", items[0].Locator);
        Assert.Contains("Most notes are class a.", items[0].Body);
    }

    [Fact]
    public void Referee_items_hold_the_statement_and_finding_blind_and_carry_the_identity_locator()
    {
        var claims = new List<(string, string)> { ("most-are-a", "031-dt-classes") };
        var findings = new Dictionary<string, string>(System.StringComparer.Ordinal) { ["most-are-a"] = "Most notes are class a." };
        var statements = new Dictionary<string, string>(System.StringComparer.Ordinal) { ["031-dt-classes"] = "DT has two classes." };
        var it = Assert.Single(Itemizers.RefereeItems(claims, findings, statements));
        Assert.Equal("most-are-a-031", it.Id);
        Assert.Equal($"most-are-a {Itemizers.Arrow} 031-dt-classes", it.Locator);
        Assert.Contains("DT has two classes.", it.Body);
        Assert.Contains("Most notes are class a.", it.Body);
        // The referee is blind: the target's file name is not in the body it reads.
        Assert.DoesNotContain("031-dt-classes", it.Body);
    }

    [Fact]
    public void The_referee_locator_uses_the_same_identity_separator_compose_parses()
        => Assert.Equal(Compose.Arrow, Itemizers.Arrow);

    [Fact]
    public void The_hypothesis_statement_is_the_section_text()
    {
        const string text = "---\nid: 31\n---\n\n## Hypothesis\n\nDT has two classes.\n\n## Record\n\n- created | 2026-09-01T10:00: why\n";
        Assert.Equal("DT has two classes.", Itemizers.HypothesisStatement(text));
    }

    // ---- reverify (iterating-a-statement) ----

    const string Hypothesis = """
        ---
        id: 31
        status: challenged
        baselined: false
        created: 2026-09-01
        ---

        ## Hypothesis

        DT has two classes.

        ## Record

        - created | 2026-09-01T10:00: why it exists
        - evidence | 2026-09-02T10:00 | (study-a/before-iter; directions-1@abc123) [supporting]:
          This is bound to the prior wording, above the boundary.
          Falsifier: f0
        - iteration | 2026-09-10T09:00: reworded. Entries above are bound to the prior wording.
        - evidence | 2026-09-11T10:00 | (study-a/supporting-one; directions-1@abc123) [supporting]:
          Supporting finding text.
          Falsifier: f1
        - evidence | 2026-09-12T10:00 | (study-a/the-challenge; directions-1@def456) [challenging]:
          The challenging finding text.
          Falsifier: f2
        """;

    [Fact]
    public void Current_wording_evidence_is_the_entries_after_the_last_iteration_boundary()
    {
        var ev = Itemizers.CurrentWordingEvidence(Hypothesis);
        Assert.Equal(["study-a/supporting-one", "study-a/the-challenge"], ev.Select(e => e.Token).ToArray());
        Assert.Equal("Supporting finding text.", ev[0].FindingText);
    }

    [Fact]
    public void Reverify_items_pair_the_proposed_wording_with_each_frozen_finding_blind()
    {
        var ev = Itemizers.CurrentWordingEvidence(Hypothesis);
        var items = Itemizers.ReverifyItems("DT has three classes.", ev);
        Assert.Equal(["supporting-one", "the-challenge"], items.Select(i => i.Id).ToArray());
        Assert.Equal("study-a/the-challenge", items[1].Locator);
        Assert.Contains("DT has three classes.", items[0].Body);
        Assert.Contains("Supporting finding text.", items[0].Body);
        // The once-challenging entry is included: it too must come out supporting of the new wording.
        Assert.Contains("The challenging finding text.", items[1].Body);
        // Blind: the frozen falsifier and the finding token are not in the body the referee reads.
        Assert.DoesNotContain("f1", items[0].Body);
        Assert.DoesNotContain("study-a", items[0].Body);
    }
}
