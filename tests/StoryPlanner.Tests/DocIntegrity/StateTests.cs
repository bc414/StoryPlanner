using System.IO;
using System.Linq;
using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The <c>state</c> verb's three derivations, ruled 2026-09-05: where an instance stands
/// (artifacts present, furthest process whose instance-scoped writes all exist), which open
/// questions a calibrated codebook covers and a round answered, and each hypothesis's
/// authored status beside the one its entries imply. Absent inputs are said to be absent.
/// </summary>
public class StateTests
{
    static string Build(MapFixture f) => StateBuilder.Build(f.RepoRoot, f.Doc);

    [Fact]
    public void An_instance_lists_the_instance_scoped_artifacts_present()
    {
        using var f = new MapFixture().WithStateTree();
        var state = Build(f);
        Assert.Contains($"### {MapFixture.Instance}", state);
        Assert.Contains("- type: verification · corpus: analysis-corpus · go: 2026-09-20", state);
        var line = state.Split('\n').Single(l => l.StartsWith("- artifacts present:"));
        Assert.Contains(" items", line);
        Assert.Contains(" results", line);
        Assert.Contains(" candidates [2 candidate(s), 2 referee line(s), 1 outcome line(s)]", line);
        Assert.Contains(" verification-artifact", line);
    }

    [Fact]
    public void The_furthest_process_is_the_last_in_chain_order_whose_scoped_writes_all_exist()
    {
        using var f = new MapFixture().WithStateTree();
        Assert.Contains("furthest process whose instance-scoped writes all exist: promote (promoting-checked-candidates)", Build(f));
    }

    [Fact]
    public void A_missing_artifact_moves_the_furthest_process_back()
    {
        using var f = new MapFixture().WithStateTree();
        File.Delete(f.TreePath("fanout", MapFixture.Instance, "candidates.md"));
        Assert.Contains("furthest process whose instance-scoped writes all exist: referee-judge (refereeing-a-candidate)", Build(f));
    }

    [Fact]
    public void An_empty_directory_does_not_count_as_present()
    {
        using var f = new MapFixture().WithStateTree();
        File.Delete(f.TreePath("fanout", MapFixture.Instance, "2026-09-20", "results", "item-001.md"));
        var line = Build(f).Split('\n').Single(l => l.StartsWith("- artifacts present:"));
        Assert.DoesNotContain(" results", line);
    }

    [Fact]
    public void The_referee_instances_share_one_folder()
    {
        Assert.Equal("referee", StateBuilder.InstanceFolder("referee-1"));
        Assert.Equal("referee", StateBuilder.InstanceFolder("referee-12"));
        Assert.Equal("round-of-x-1", StateBuilder.InstanceFolder("round-of-x-1"));
    }

    [Fact]
    public void An_open_question_is_covered_by_a_calibrated_codebook_naming_it_and_answered_by_a_round_listing_it()
    {
        using var f = new MapFixture().WithStateTree();
        var state = Build(f);
        Assert.Contains("### analysis-corpus", state);
        Assert.Contains("1 open, 1 withdrawn.", state);
        var row = state.Split('\n').Single(l => l.StartsWith($"| {MapFixture.OpenQuestion} |"));
        Assert.Contains("| 031 |", row);
        Assert.Contains($"| fanout/{MapFixture.Instance}/codebook-1@", row);
        Assert.Contains($"| {MapFixture.Instance} |", row);
        Assert.DoesNotContain("An old one", state);
    }

    [Fact]
    public void A_codebook_with_no_accepting_calibration_record_at_its_hash_covers_nothing()
    {
        using var f = new MapFixture().WithStateTree();
        File.Delete(f.TreePath("fanout", MapFixture.Instance, "calibration-2026-09-19.md"));
        var row = Build(f).Split('\n').Single(l => l.StartsWith($"| {MapFixture.OpenQuestion} |"));
        Assert.Contains("| nothing |", row);
    }

    [Fact]
    public void An_edited_codebook_is_no_longer_calibrated_because_its_hash_moved()
    {
        using var f = new MapFixture().WithStateTree();
        File.AppendAllText(f.TreePath("fanout", MapFixture.Instance, "codebook-1.md"), "an edit\n");
        var row = Build(f).Split('\n').Single(l => l.StartsWith($"| {MapFixture.OpenQuestion} |"));
        Assert.Contains("| nothing |", row);
    }

    [Fact]
    public void A_hypothesis_shows_its_authored_status_and_the_one_its_entries_imply()
    {
        using var f = new MapFixture().WithStateTree();
        var state = Build(f);
        Assert.Contains($"| 031 | dt-classes | evidenced | false | evidenced | analysis-corpus: {MapFixture.OpenQuestion} |", state);
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
            "- evidence | 2026-09-17T10:00 | (round-of-analysis-corpus-1 C-007; codebook-1@abc) [challenging]:\n  a counter\n  Falsifier: f\n");
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
    public void Question_entries_parse_their_status_and_hypotheses()
    {
        var qs = StateBuilder.ParseQuestions("c", "### One\n- hypotheses: 031, 050\n- status: open\n\n### Two\n- hypotheses: 007\n- status: withdrawn (why)\n");
        Assert.Equal(2, qs.Count);
        Assert.True(qs[0].IsOpen);
        Assert.Equal([31, 50], qs[0].Hypotheses);
        Assert.False(qs[1].IsOpen);
    }
}
