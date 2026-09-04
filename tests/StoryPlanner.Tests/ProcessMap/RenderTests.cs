using System.Collections.Generic;
using System.Linq;
using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// Rendering and the marker contract. The generated sections are never hand-edited, so the two
/// things worth pinning are that a re-render changes nothing and that everything outside a
/// marker pair is copied through untouched.
/// </summary>
public class RenderTests
{
    static IReadOnlyDictionary<string, string> Sections(bool forced = false)
    {
        var doc = MapReader.Read(MapFixture.ValidMap);
        return MermaidRenderer.RenderAll(doc, new ValidationReport([]), forced);
    }

    [Fact]
    public void All_five_sections_are_produced()
        => Assert.Equal(MermaidRenderer.SectionNames.OrderBy(x => x),
            Sections().Keys.OrderBy(x => x));

    [Fact]
    public void The_same_rows_render_byte_identical_text()
    {
        var first = Sections();
        var second = Sections();
        foreach (var key in first.Keys) Assert.Equal(first[key], second[key]);
    }

    [Fact]
    public void Level_one_draws_the_P_rows_and_level_three_the_M_rows()
    {
        var s = Sections();
        Assert.Contains("P0[", s["level-1"]);
        Assert.DoesNotContain("P3[", s["level-1"]);   // P.3 is an M row
        Assert.Contains("P3", s["level-3"]);
    }

    [Fact]
    public void A_neighbour_outside_the_drawn_levels_becomes_one_collapsed_node()
    {
        // P.1 (level P) → P.2 (level V): the V diagram must still show the edge, collapsed.
        Assert.Contains("LVLP", Sections()["level-2"]);
    }

    [Fact]
    public void An_actor_gets_its_class_and_a_choice_row_gets_a_diamond()
    {
        var level1 = Sections()["level-1"];
        Assert.Contains(":::hitl", level1);
        Assert.Contains(":::script", level1);
        Assert.Contains("P1{", level1);   // P.1 is the source of a choice edge
    }

    [Fact]
    public void The_consumers_section_lists_producers_and_consumers_per_file()
    {
        var consumers = Sections()["consumers"];
        Assert.Contains("| f.b | P.1 | P.2 |", consumers);
    }

    [Fact]
    public void Forcing_stamps_every_section_unvalidated()
    {
        foreach (var body in Sections(forced: true).Values)
            Assert.Contains("UNVALIDATED", body);
    }

    [Fact]
    public void Mermaid_node_ids_drop_the_dots_the_row_ids_carry()
    {
        Assert.Equal("P1", MermaidRenderer.NodeId("P.1"));
        Assert.Equal("fcand", MermaidRenderer.NodeId("f.cand"));
    }

    [Fact]
    public void Writing_replaces_only_what_lies_between_the_markers()
    {
        var sections = MermaidRenderer.SectionNames.ToDictionary(n => n, n => $"body of {n}\n");
        var updated = MarkerWriter.Write(MapFixture.ValidMap, sections);

        Assert.Contains("body of level-1", updated);
        Assert.Contains("| P.0 | P | sop | Seed the cycle", updated);
        Assert.Contains("Prose above the tables is copied through untouched.", updated);
        Assert.Equal(
            MapFixture.ValidMap.Split("## Generated")[0],
            updated.Split("## Generated")[0]);
    }

    [Fact]
    public void A_second_write_over_the_first_is_idempotent()
    {
        var sections = MermaidRenderer.SectionNames.ToDictionary(n => n, n => $"body of {n}\n");
        var once = MarkerWriter.Write(MapFixture.ValidMap, sections);
        Assert.Equal(once, MarkerWriter.Write(once, sections));
    }

    [Fact]
    public void A_section_with_no_marker_pair_is_refused_rather_than_appended()
        => Assert.Throws<MapFormatException>(() =>
            MarkerWriter.Write(MapFixture.ValidMap, new Dictionary<string, string> { ["nowhere"] = "x" }));

    [Fact]
    public void An_unclosed_marker_is_refused()
        => Assert.Throws<MapFormatException>(() => MarkerWriter.Write(
            "<!-- generated:level-1 -->\n",
            new Dictionary<string, string> { ["level-1"] = "x\n" }));
}
