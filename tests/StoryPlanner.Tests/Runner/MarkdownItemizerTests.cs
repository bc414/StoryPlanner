using StoryPlanner.BatchFiles;
using StoryPlanner.MarkdownItemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The unit rule is a tool: the same document must split into the same numbered units every
/// time, and the rule's edge cases (frontmatter, nested lists, fenced blocks, tables,
/// headings) are pinned here so a change to the splitter is visible as a changed count,
/// never as silently different items. The item body carries the section and the text, no id.
/// </summary>
public class MarkdownItemizerTests
{
    private const string Doc = """
        ---
        name: sample
        description: A sample.
        ---

        # Title

        Opening paragraph, which
        continues on a second line.

        ## Section one

        **Bold lead.** A paragraph with a code block after it:

        ```
        - not a list item
        ```

        - first item
          - nested item
        - second item

        1. numbered one
        2. numbered two

        | Head | Col |
        |---|---|
        | row one | a |
        | row two | b |

        ### Sub-section

        Last paragraph.
        """;

    [Fact]
    public void Splits_by_the_unit_rule_and_numbers_in_document_order()
    {
        var units = UnitSplitter.Split(Doc);

        var ids = units.Select(u => u.Id).ToArray();
        Assert.Equal(11, units.Count);
        Assert.Equal("unit-001", ids[0]);
        Assert.Equal("unit-011", ids[^1]);

        Assert.Equal("(frontmatter)", units[0].Section);
        Assert.StartsWith("---\nname: sample", units[0].Text);

        Assert.Equal("# Title", units[1].Section);
        Assert.Equal("Opening paragraph, which\ncontinues on a second line.", units[1].Text);

        // The fenced block attaches to the paragraph before it, and its "- " line is not a list item.
        Assert.Equal("## Section one", units[2].Section);
        Assert.Contains("```\n- not a list item\n```", units[2].Text);

        Assert.Equal(["- first item", "  - nested item", "- second item"], units.Skip(3).Take(3).Select(u => u.Text));
        Assert.Equal(["1. numbered one", "2. numbered two"], units.Skip(6).Take(2).Select(u => u.Text));

        // Header and separator rows are structure; each body row is a unit.
        Assert.Equal("| row one | a |", units[8].Text);
        Assert.Equal("| row two | b |", units[9].Text);

        Assert.Equal("### Sub-section", units[10].Section);
        Assert.Equal("Last paragraph.", units[10].Text);
    }

    [Fact]
    public void Same_document_splits_identically_and_items_carry_the_section_and_text_verbatim_with_no_id()
    {
        var a = UnitSplitter.Split(Doc);
        var b = UnitSplitter.Split(Doc);
        Assert.Equal(a.Select(u => u.Text), b.Select(u => u.Text));

        var item = UnitSplitter.RenderItem(a[3]);
        Assert.Equal("Section: ## Section one\n\n- first item\n", item);
        Assert.DoesNotContain("unit-004", item);
        Assert.Equal("first item", a[3].FirstLine);
        Assert.Equal("4 in ## Section one", UnitSplitter.Locator(a[3]));

        var index = IndexFile.Parse(IndexFile.Render("01-audit", "tools/StoryPlanner.MarkdownItemizer, 1, sample.md", "skill", "n in heading",
            Hashing.Sha256Hex(Doc), a.Select(u => (u.Id, UnitSplitter.Locator(u), UnitSplitter.Description(u)))));
        Assert.Empty(index.Problems);
        Assert.Equal(11, index.Rows.Count);
        Assert.Equal("skill", index.Corpus);
        Assert.Equal("4 in ## Section one", index.Rows[3].Locator);
        Assert.Equal(64, index.SourceHash!.Length);
    }
}
