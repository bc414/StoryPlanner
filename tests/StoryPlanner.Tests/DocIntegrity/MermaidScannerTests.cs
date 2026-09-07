using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The <c>nodes</c> verb's scanner, whose whole job is the set comparison against
/// <c>process-map-1-draft.md</c> at 32b6d4b: draft 1 wrote <c>P1</c> and <c>fCand</c> by hand
/// for what the rows call <c>P.1</c> and <c>f.cand</c>, so ids are compared normalised.
/// </summary>
public class MermaidScannerTests
{
    const string TwoDiagrams = """
        # A document

        ```mermaid
        flowchart TD
          classDef brian fill:#fff
          P1["P.1 first"]:::hitl
          P2["P.2 second"]
          fCand[/"candidates"/]
          P1 --> P2
          P2 -- "a label" --> fCand
        ```

        Prose between the diagrams, mentioning P9 which is not a node.

        ```mermaid
        flowchart TD
          R1{"choice"}
          R2(("∥ fork"))
          R1 -- unquoted label --> R2
          R2 -. "optional" .-> R1
        ```
        """;

    [Fact]
    public void Ids_and_edge_pairs_are_extracted_from_every_diagram_in_the_file()
    {
        var scan = MermaidScanner.Scan(TwoDiagrams);
        Assert.Equal(["fcand", "p1", "p2", "r1", "r2"], scan.Nodes);
        Assert.Equal(["p1 -> p2", "p2 -> fcand", "r1 -> r2", "r2 -> r1"], scan.Edges);
    }

    [Fact]
    public void An_unquoted_mid_arrow_label_is_not_read_as_a_node()
        => Assert.DoesNotContain("unquoted", MermaidScanner.Scan(TwoDiagrams).Nodes);

    [Fact]
    public void Prose_outside_a_mermaid_block_contributes_nothing()
        => Assert.DoesNotContain("p9", MermaidScanner.Scan(TwoDiagrams).Nodes);

    [Fact]
    public void A_class_definition_is_not_a_node()
        => Assert.DoesNotContain("classdef", MermaidScanner.Scan(TwoDiagrams).Nodes);

    [Fact]
    public void An_ampersand_list_is_one_edge_per_member()
    {
        var scan = MermaidScanner.Scan("```mermaid\nflowchart TD\n  E & V & S --> P6\n```");
        Assert.Equal(["e -> p6", "s -> p6", "v -> p6"], scan.Edges);
    }

    [Fact]
    public void A_chain_on_one_line_is_read_as_its_pairs()
    {
        var scan = MermaidScanner.Scan("```mermaid\nflowchart TD\n  A --> B --> C\n```");
        Assert.Equal(["a -> b", "b -> c"], scan.Edges);
    }

    [Fact]
    public void The_raw_to_normalised_mapping_is_reported_so_a_rename_is_visible()
    {
        var scan = MermaidScanner.Scan(TwoDiagrams);
        Assert.Equal("fcand", scan.Normalisation["fCand"]);
    }

    [Fact]
    public void Normalisation_drops_the_separators_the_two_id_styles_disagree_on()
    {
        Assert.Equal(MermaidScanner.Normalise("P.1"), MermaidScanner.Normalise("P1"));
        Assert.Equal(MermaidScanner.Normalise("f.cand"), MermaidScanner.Normalise("fCand"));
        Assert.Equal(MermaidScanner.Normalise("f_cand"), MermaidScanner.Normalise("f-cand"));
    }
}
