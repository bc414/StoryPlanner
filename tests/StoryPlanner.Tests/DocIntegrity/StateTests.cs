using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The <c>state</c> derivations, ruled 2026-09-05 and brought to the study folders on
/// 2026-09-09: where a study stands (artifacts present, its batches, furthest process whose
/// study-scoped writes all exist), which open questions calibrated directions cover and a
/// verification answered, and each hypothesis's authored status beside the one its entries
/// imply. Absent inputs are said to be absent.
/// </summary>
public class StateTests
{
    static string Build(MapFixture f) => StateBuilder.Build(f.RepoRoot, f.Doc);

    [Fact]
    public void A_study_lists_the_study_scoped_artifacts_present_and_its_batches()
    {
        using var f = new MapFixture().WithStateTree();
        var state = Build(f);
        Assert.Contains($"### {MapFixture.Study}", state);
        Assert.Contains("- type: verification · corpus: analysis-corpus · go: 2026-09-20", state);
        var line = state.Split('\n').Single(l => l.StartsWith("- artifacts present:"));
        Assert.Contains(" items", line);
        Assert.Contains(" results", line);
        Assert.Contains(" definition", line);
        Assert.Contains(" candidates [2 candidate(s), 2 referee line(s), 1 outcome line(s)]", line);
        // findings is read, never written, by the fixture's two activities, so it is not among the study-scoped writes listed here.
        Assert.DoesNotContain(" findings", line);
        Assert.Contains($"- batches: {MapFixture.Batch} [full, directions-1, itemized]", state);
    }

    [Fact]
    public void A_batch_with_calls_and_a_tally_shows_its_stage()
    {
        using var f = new MapFixture().WithStateTree();
        File.WriteAllText(Path.Combine(f.BatchDir, "calls.md"),
            "# 01-full — calls\n\n- definition: abc\n\n### item-001 — call 1\n\n- model: sonnet\n- harness: 2\n- directions hash: d\n- item hash: i\n- prompt hash: p\n- started: t\n- ended: t\n- exit: 0\n- check: ok\n- pilot: no\n");
        Assert.Contains($"{MapFixture.Batch} [full, directions-1, executed]", Build(f));
        File.WriteAllText(Path.Combine(f.BatchDir, "tally.md"), "# tally\n");
        Assert.Contains($"{MapFixture.Batch} [full, directions-1, tallied]", Build(f));
    }

    [Fact]
    public void The_furthest_process_is_the_last_in_chain_order_whose_scoped_writes_all_exist()
    {
        using var f = new MapFixture().WithStateTree();
        Assert.Contains("furthest process whose study-scoped writes all exist: promote (promoting-refereed-candidates)", Build(f));
    }

    [Fact]
    public void A_missing_artifact_moves_the_furthest_process_back()
    {
        using var f = new MapFixture().WithStateTree();
        File.Delete(Path.Combine(f.StudyDir, "candidates.md"));
        Assert.Contains("furthest process whose study-scoped writes all exist: assess-referee-items (refereeing-candidates)", Build(f));
    }

    [Fact]
    public void An_empty_directory_does_not_count_as_present()
    {
        using var f = new MapFixture().WithStateTree();
        File.Delete(Path.Combine(f.BatchDir, "results", "item-001.md"));
        var line = Build(f).Split('\n').Single(l => l.StartsWith("- artifacts present:"));
        Assert.DoesNotContain(" results", line);
    }

    [Fact]
    public void An_open_question_is_covered_by_calibrated_directions_naming_it_and_answered_by_a_verification_listing_it()
    {
        using var f = new MapFixture().WithStateTree();
        var state = Build(f);
        Assert.Contains("### analysis-corpus", state);
        Assert.Contains("1 open, 1 withdrawn.", state);
        var row = state.Split('\n').Single(l => l.StartsWith($"| {MapFixture.OpenQuestion} |"));
        Assert.Contains("| 031 |", row);
        Assert.Contains($"| docs/v3-framework/studies/{MapFixture.Study}/directions-1@", row);
        Assert.Contains($"| {MapFixture.Study} |", row);
        Assert.DoesNotContain("| an-old-one |", state);
    }

    [Fact]
    public void Directions_with_no_accepting_calibration_at_their_hash_cover_nothing()
    {
        using var f = new MapFixture().WithStateTree();
        File.Delete(Path.Combine(f.StudyDir, "calibration-2026-09-19.md"));
        var row = Build(f).Split('\n').Single(l => l.StartsWith($"| {MapFixture.OpenQuestion} |"));
        Assert.Contains("| nothing |", row);
    }

    [Fact]
    public void Edited_directions_are_no_longer_calibrated_because_their_body_hash_moved()
    {
        using var f = new MapFixture().WithStateTree();
        File.AppendAllText(Path.Combine(f.StudyDir, "directions-1.md"), "\nAn edit.\n");
        var row = Build(f).Split('\n').Single(l => l.StartsWith($"| {MapFixture.OpenQuestion} |"));
        Assert.Contains("| nothing |", row);
    }

    [Fact]
    public void A_hypothesis_shows_its_authored_status_and_the_one_its_entries_imply()
    {
        using var f = new MapFixture().WithStateTree();
        var state = Build(f);
        Assert.Contains($"| 031 | dt-classes | evidenced | false | evidenced | analysis-corpus/{MapFixture.OpenQuestion} |", state);
        Assert.Contains("| 032 | other | untested | false | evidenced — MISMATCH | — |", state);
    }

    [Fact]
    public void An_iteration_entry_is_a_wording_boundary_for_the_implied_status()
    {
        using var f = new MapFixture().WithStateTree();
        File.AppendAllText(f.TreePath("docs", "v3-framework", "hypotheses", "031-dt-classes.md"),
            "- iteration | 2026-09-16T09:15: Reworded because. Entries above this line are bound to the prior wording.\n");
        Assert.Contains("| 031 | dt-classes | evidenced | false | untested — MISMATCH |", Build(f));
    }

    [Fact]
    public void A_challenging_entry_implies_challenged()
    {
        using var f = new MapFixture().WithStateTree();
        File.AppendAllText(f.TreePath("docs", "v3-framework", "hypotheses", "031-dt-classes.md"),
            $"- evidence | 2026-09-17T10:00 | ({MapFixture.Study}/a-counter-finding; directions-1@abc) [challenging]:\n  a counter\n  Falsifier: f\n");
        Assert.Contains("| 031 | dt-classes | evidenced | false | challenged — MISMATCH |", Build(f));
    }

    [Fact]
    public void Absent_inputs_are_reported_as_absent_never_as_empty()
    {
        using var f = new MapFixture();
        var state = Build(f);
        Assert.Contains("Registry absent", state);
        Assert.Contains("Question lists absent", state);
        Assert.Contains("Hypothesis files absent", state);
    }

    [Fact]
    public void Question_entries_parse_their_withdrawal_and_hypotheses()
    {
        var qs = StateBuilder.ParseQuestions("c", "### c/one\n- date: 2026-09-01\n- hypotheses: 031 050\n- raised by: x\n- question: y\n\n### c/two\n- date: 2026-09-02\n- hypotheses: 007\n- raised by: x\n- question: z\n- withdrawn: 2026-09-03 why\n");
        Assert.Equal(2, qs.Count);
        Assert.True(qs[0].IsOpen);
        Assert.Equal("c/one", qs[0].Cite);
        Assert.Equal([31, 50], qs[0].Hypotheses);
        Assert.False(qs[1].IsOpen);
    }
}
