using System.IO;
using System.Linq;
using StoryPlanner.DocIntegrity;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Rendering. Generated text is files only, so the things worth pinning are that map.md
/// carries what the tables derive, that the same rows render byte-identical text, that
/// <see cref="Render.Write"/> touches an authored file only to remove a generated block the
/// earlier convention left there, and that stripping such a block preserves everything else.
/// </summary>
public class RenderTests
{
    [Fact]
    public void Level_one_draws_every_activity_and_its_enables_edges()
    {
        using var f = new MapFixture();
        var level1 = MermaidRenderer.Level1(f.Doc);
        Assert.Contains("changingtheplannerforv3[[\"changing-the-planner-for-v3\"]]:::terminus", level1);
        Assert.Contains("refereeingacandidate[\"refereeing-a-candidate\"]:::activity", level1);
        Assert.Contains("refereeingacandidate --> promotingcheckedcandidates", level1);
        Assert.Contains("promotingcheckedcandidates --> changingtheplannerforv3", level1);
    }

    [Fact]
    public void An_activity_section_draws_its_processes_by_mode_and_the_artifacts_they_touch()
    {
        using var f = new MapFixture();
        var section = MermaidRenderer.Activity(f.Doc, "refereeing-a-candidate");
        Assert.Contains("refereerun[\"referee-run<br/>session\"]:::session", section);
        Assert.Contains("refereejudge([\"referee-judge<br/>agent\"]):::agent", section);
        Assert.Contains("candidates[/\"candidates\"/]:::artifact", section);
        Assert.Contains("codebook --> refereerun", section);
        Assert.Contains("refereeappend --> candidates", section);
        Assert.DoesNotContain("promote", section);
    }

    [Fact]
    public void An_hitl_process_is_a_hexagon()
    {
        using var f = new MapFixture();
        Assert.Contains("promote{{\"promote<br/>hitl\"}}:::hitl", MermaidRenderer.Activity(f.Doc, "promoting-checked-candidates"));
    }

    [Fact]
    public void An_instrument_read_is_a_dashed_edge()
    {
        using var f = MapFixture.With(MapFixture.RefereeingFile,
            "| referee-judge | agent | | codebook items |", "| referee-judge | agent | items | codebook |");
        Assert.Contains("items -.-> refereejudge", MermaidRenderer.Activity(f.Doc, "refereeing-a-candidate"));
    }

    [Fact]
    public void An_activity_section_carries_what_the_tables_derive_for_it()
    {
        using var f = new MapFixture();
        var section = MermaidRenderer.Activity(f.Doc, "refereeing-a-candidate");
        Assert.Contains("- **inputs**: calibration-record codebook instances", section);
        Assert.Contains("- **outputs**: candidates items results", section);
        Assert.Contains("- **instruments**: runner", section);
        Assert.Contains("- **enabled by**: —", section);
        Assert.Contains("- **enables**: promoting-checked-candidates", section);
        Assert.Contains("- **enabled by**: refereeing-a-candidate",
            MermaidRenderer.Activity(f.Doc, "promoting-checked-candidates"));
    }

    [Fact]
    public void The_map_holds_the_activities_each_activity_the_whole_graph_the_consumers_and_the_verdict()
    {
        using var f = new MapFixture();
        var map = MermaidRenderer.Map(f.Doc, f.Report, forced: false);
        Assert.Contains("## The activities", map);
        Assert.Contains("### refereeing-a-candidate", map);
        Assert.Contains("### promoting-checked-candidates", map);
        Assert.DoesNotContain("### changing-the-planner-for-v3", map);
        Assert.Contains("- **enables**: promoting-checked-candidates", map);
        Assert.Contains("subgraph refereeingacandidate[\"refereeing-a-candidate\"]", map);
        Assert.Contains("| candidates | promote referee-append | promote referee-run referee-append | — |", map);
        Assert.Contains("Last run: **passed**", map);
        Assert.DoesNotContain(MapTables.GeneratedOpenPrefix, map);
    }

    [Fact]
    public void The_same_rows_render_byte_identical_text()
    {
        using var f = new MapFixture();
        var doc = f.Doc;
        Assert.Equal(MermaidRenderer.Level1(doc), MermaidRenderer.Level1(doc));
        Assert.Equal(MermaidRenderer.Activity(doc, "promoting-checked-candidates"),
            MermaidRenderer.Activity(doc, "promoting-checked-candidates"));
        Assert.Equal(MermaidRenderer.Map(doc, f.Report, false), MermaidRenderer.Map(doc, f.Report, false));
    }

    [Fact]
    public void Forcing_stamps_the_map_unvalidated()
    {
        using var f = new MapFixture();
        Assert.Contains("UNVALIDATED", MermaidRenderer.Map(f.Doc, f.Report, forced: true));
        Assert.DoesNotContain("UNVALIDATED", MermaidRenderer.Map(f.Doc, f.Report, forced: false));
    }

    [Fact]
    public void Mermaid_node_ids_drop_the_hyphens_the_row_ids_carry()
        => Assert.Equal("refereerun", MermaidRenderer.NodeId("referee-run"));

    [Fact]
    public void Ids_that_merge_once_hyphens_are_dropped_are_refused_before_drawing()
    {
        using var f = MapFixture.With(MapFixture.ArtifactsFile,
            "| results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs |",
            "| results | fanout/<instance>/<run>/results/ | frozen | | The agents' outputs |\n| refereerun | docs/x.md | frozen | | Collides with referee-run |");
        Assert.Throws<MapFormatException>(() => MermaidRenderer.CheckNodeIds(f.Doc));
    }

    // ---- writing: files only ----

    [Fact]
    public void Write_produces_map_md_and_touches_no_authored_file_that_is_clean()
    {
        using var f = new MapFixture();
        var before = f.Read(MapFixture.RefereeingFile);
        var written = Render.Write(f.SkillFolder, f.Doc, f.Report, forced: false);
        Assert.Equal([Path.Combine(f.SkillFolder, Render.MapFile)], written);
        Assert.Contains("## Each activity", File.ReadAllText(Path.Combine(f.SkillFolder, Render.MapFile)));
        Assert.Equal(before, f.Read(MapFixture.RefereeingFile));
    }

    const string LeftoverBlock = "<!-- generated:level-1 -->\n```mermaid\nold diagram\n```\n<!-- /generated -->\n\n## Companions";

    [Fact]
    public void Write_removes_a_generated_block_the_earlier_convention_left_in_an_authored_file()
    {
        using var f = MapFixture.With(MapFixture.SkillFile, "## Companions", LeftoverBlock);
        Assert.Contains("info.generated.inline-block", f.Report.Findings.Select(x => x.RuleId));
        Assert.True(f.Report.Passed);

        var written = Render.Write(f.SkillFolder, f.Doc, f.Report, forced: false);
        Assert.Contains(f.Doc.SkillPath, written);

        var skill = f.Read(MapFixture.SkillFile);
        Assert.DoesNotContain(MapTables.GeneratedOpenPrefix, skill);
        Assert.DoesNotContain("old diagram", skill);
        Assert.Contains("| changing-the-planner-for-v3 | | The terminus", skill);
        Assert.Contains("## Companions", skill);
        Assert.DoesNotContain("info.generated.inline-block", f.Report.Findings.Select(x => x.RuleId));

        var again = Render.Write(f.SkillFolder, f.Doc, f.Report, forced: false);
        Assert.Equal([Path.Combine(f.SkillFolder, Render.MapFile)], again);
    }

    // ---- stripping ----

    [Fact]
    public void Stripping_removes_the_block_and_leaves_one_blank_line_where_it_stood()
    {
        const string text = "# t\n\n| a |\n|---|\n| 1 |\n\n<!-- generated:x -->\nbody\n<!-- /generated -->\n\n## Next\n";
        Assert.Equal("# t\n\n| a |\n|---|\n| 1 |\n\n## Next\n", Render.StripGenerated(text));
    }

    [Fact]
    public void Stripping_keeps_the_file_s_own_newline_style()
    {
        const string text = "# t\r\n\r\n<!-- generated:x -->\r\nbody\r\n<!-- /generated -->\r\n\r\n## Next\r\n";
        Assert.Equal("# t\r\n\r\n## Next\r\n", Render.StripGenerated(text));
    }

    [Fact]
    public void Stripping_a_file_with_no_block_changes_nothing()
        => Assert.Equal("# t\n\nprose\n", Render.StripGenerated("# t\n\nprose\n"));

    [Fact]
    public void An_unclosed_block_is_refused_rather_than_stripped_to_the_end()
        => Assert.Throws<MapFormatException>(() => Render.StripGenerated("# t\n<!-- generated:x -->\nbody\n"));
}
