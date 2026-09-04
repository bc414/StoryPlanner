using StoryPlanner.ProcessMap;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The citation grammar used by <c>Roots.source</c>. Nothing is normalised away: the point of
/// the ruling is that a pointer either resolves to one place or is a syntax error, so a
/// precise citation can never quietly widen into a vague one.
/// </summary>
public class LocusTests
{
    static Locus Parse(string cell)
    {
        Assert.True(Locus.TryParse(cell, out var locus, out var error), error);
        return locus!;
    }

    static string Error(string cell)
    {
        Assert.False(Locus.TryParse(cell, out _, out var error));
        return error!;
    }

    [Fact]
    public void A_bare_path_is_the_whole_document()
    {
        var l = Parse("docs/proc.md");
        Assert.Equal("docs/proc.md", l.Path);
        Assert.Null(l.Heading);
        Assert.Null(l.Item);
    }

    [Fact]
    public void A_section_sign_addresses_one_heading()
        => Assert.Equal("What it does", Parse("docs/proc.md § What it does").Heading);

    [Fact]
    public void An_item_sign_addresses_one_ordered_list_item()
    {
        var l = Parse("docs/goal.md § Rules ¶ 2");
        Assert.Equal("Rules", l.Heading);
        Assert.Equal(2, l.Item);
    }

    [Fact]
    public void A_heading_ending_in_a_bare_number_is_a_syntax_error_not_an_item_pointer()
        => Assert.Contains("never normalised away", Error("docs/goal.md § Rules 2"));

    [Fact]
    public void A_path_with_a_space_is_not_a_path()
        => Assert.Contains("contains a space", Error("agent-runner SKILL.md § Layout"));

    [Fact]
    public void A_parenthetical_is_not_part_of_a_path()
        => Assert.Contains("parenthetical", Error("docs/rev.md(record-only)"));

    [Fact]
    public void A_cell_asserting_that_nothing_governs_it_is_a_syntax_error_not_a_path()
        => Assert.Contains("contains a space", Error("docs/rev.md (record only; no governing protocol)"));

    [Fact]
    public void A_bare_file_name_cannot_resolve_because_there_is_no_search_order()
        => Assert.Contains("no directory", Error("SKILL.md § Provenance"));

    [Fact]
    public void An_absolute_path_is_rejected_because_paths_are_repo_relative()
        => Assert.Contains("repo-relative", Error("C:/docs/proc.md"));

    [Fact]
    public void A_section_sign_with_nothing_after_it_is_an_error()
        => Assert.Contains("no heading", Error("docs/proc.md §"));

    [Fact]
    public void An_item_that_is_not_a_positive_number_is_an_error()
        => Assert.Contains("item number", Error("docs/goal.md § Rules ¶ zero"));

    [Fact]
    public void Display_round_trips_the_grammar()
        => Assert.Equal("docs/goal.md § Rules ¶ 2", Parse("docs/goal.md § Rules ¶ 2").Display());

    [Fact]
    public void An_outline_finds_a_heading_once_and_reports_a_repeat_as_ambiguous()
    {
        var outline = new MarkdownOutline("# One\n\n## Twice\n\ntext\n\n## Twice\n\ntext\n");
        Assert.Equal(2, outline.Find("Twice").Count);
        Assert.Empty(outline.Find("Thrice"));
    }

    [Fact]
    public void Ordered_items_are_counted_under_their_heading_only()
    {
        var outline = new MarkdownOutline("""
            ## Rules

            1. one
            2. two

            ## Other

            1. not counted
            """);
        Assert.Equal(2, outline.CountOrderedItems(outline.Find("Rules")[0]));
    }

    [Fact]
    public void A_nested_list_item_is_not_addressable_because_its_numbering_restarts()
    {
        var outline = new MarkdownOutline("## Rules\n\n1. one\n    1. nested\n2. two\n");
        Assert.Equal(2, outline.CountOrderedItems(outline.Find("Rules")[0]));
    }

    [Fact]
    public void A_heading_inside_a_fenced_block_is_not_a_heading()
    {
        var outline = new MarkdownOutline("## Real\n\n```\n## Not real\n```\n");
        Assert.Single(outline.Headings);
    }
}
