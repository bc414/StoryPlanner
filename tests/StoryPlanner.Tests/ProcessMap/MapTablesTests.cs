using System.Linq;
using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The markdown table reader. Its rule matters most where it refuses: a malformed table is
/// flagged, never guessed at, so a dropped column can never silently become a shifted one.
/// </summary>
public class MapTablesTests
{
    [Fact]
    public void Header_and_separator_are_structure_and_every_other_row_is_a_body_row()
    {
        var tables = MapTables.ReadAll("""
            ## Section

            | a | b |
            |---|---|
            | one | two |
            | three | four |
            """);

        var t = Assert.Single(tables);
        Assert.Equal("Section", t.Section);
        Assert.Equal(["a", "b"], t.Headers);
        Assert.Equal(2, t.Rows.Count);
        Assert.Equal(["three", "four"], t.Rows[1].Cells);
        Assert.Equal(6, t.Rows[1].Line);
    }

    [Fact]
    public void An_escaped_pipe_is_a_literal_pipe_inside_one_cell()
    {
        var cells = MapTables.SplitCells(@"| a \| b | c |", 1);
        Assert.Equal(["a | b", "c"], cells);
    }

    [Fact]
    public void A_row_with_the_wrong_cell_count_is_refused_rather_than_padded()
    {
        var ex = Assert.Throws<MapFormatException>(() => MapTables.ReadAll("""
            | a | b |
            |---|---|
            | only-one |
            """));
        Assert.Contains("Refusing to guess", ex.Message);
    }

    [Fact]
    public void A_header_with_no_separator_is_refused_rather_than_read_as_a_body_row()
        => Assert.Throws<MapFormatException>(() => MapTables.ReadAll("| a | b |\n| one | two |"));

    [Fact]
    public void Tables_inside_a_fenced_block_are_not_source()
    {
        var tables = MapTables.ReadAll("""
            ```mermaid
            | not | a | table |
            ```

            | a | b |
            |---|---|
            | one | two |
            """);
        Assert.Single(tables);
    }

    [Fact]
    public void A_rendered_table_inside_a_generated_section_is_never_read_back_as_source()
    {
        var tables = MapTables.ReadAll("""
            <!-- generated:consumers -->
            | file | written by |
            |---|---|
            | f.a | P.1 |
            <!-- /generated -->

            | a | b |
            |---|---|
            | one | two |
            """);
        var t = Assert.Single(tables);
        Assert.Equal(["a", "b"], t.Headers);
    }

    [Fact]
    public void A_table_matching_no_known_schema_is_refused_rather_than_ignored()
    {
        var map = MapFixture.ValidMap.Replace(
            "## Generated",
            "## Something new\n\n| surprise | column |\n|---|---|\n| one | two |\n\n## Generated");
        var ex = Assert.Throws<MapFormatException>(() => MapReader.Read(map));
        Assert.Contains("matches no known schema", ex.Message);
    }

    [Fact]
    public void The_five_tables_are_read_into_typed_rows()
    {
        var doc = MapReader.Read(MapFixture.ValidMap);
        Assert.Equal(2, doc.Roots.Count);
        Assert.Equal(4, doc.Files.Count);
        Assert.Equal(4, doc.Processes.Count);
        Assert.Equal(3, doc.Edges.Count);
        Assert.Empty(doc.Bootstrap);

        var p2 = doc.Processes.Single(p => p.Id == "P.2");
        Assert.Equal("agent:sonnet", p2.Actor);
        Assert.Equal(["f.b"], p2.Inputs);
        Assert.Equal(["f.cand"], p2.Outputs);
        Assert.Equal(["C1"], p2.Roots);
    }

    [Fact]
    public void An_empty_id_list_is_an_empty_list_and_not_a_one_element_one()
    {
        var doc = MapReader.Read(MapFixture.ValidMap.Replace("| script | f.a | f.b |", "| script |  | f.b |"));
        Assert.Empty(doc.Processes.Single(p => p.Id == "P.1").Inputs);
    }
}
