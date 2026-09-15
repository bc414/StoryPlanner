using System;
using System.Collections.Generic;
using System.Linq;
using StoryPlanner.BatchFiles;
using StoryPlanner.ChapterItemizer;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The chapter itemizer's cut (pure tier): one item per `## Chapter` heading of a converter-format
/// story file, the body carrying the slug, the heading and the chapter text verbatim, the id and
/// locator by ordinal; exclusion by slug becomes the index's narrowing; the rendered index parses
/// back through <see cref="IndexFile"/> clean. A file with no heading is refused, never cut into
/// nothing.
/// </summary>
public class ChapterItemizerTests
{
    const string Story = """
        # about-last-night

        ## Chapter 1 — A Cautionary Tale

        First chapter, first paragraph.

        *Italics survive.*

        ## Chapter 2 — Feel the Love | Part Two

        Second chapter.
        """;

    [Fact]
    public void Cuts_one_item_per_chapter_heading_with_the_slug_heading_and_text_verbatim()
    {
        var items = Chapters.Cut(new Chapters.Story("about-last-night", Story));
        Assert.Equal(["about-last-night-ch01", "about-last-night-ch02"], items.Select(i => i.Id).ToArray());
        Assert.Equal(["about-last-night#1", "about-last-night#2"], items.Select(i => i.Locator).ToArray());
        Assert.Equal("about-last-night, chapter 1 of 2: A Cautionary Tale", items[0].Description);
        Assert.Equal("# about-last-night\n## Chapter 1 — A Cautionary Tale\n\nFirst chapter, first paragraph.\n\n*Italics survive.*\n", items[0].Body);
        Assert.Equal("# about-last-night\n## Chapter 2 — Feel the Love | Part Two\n\nSecond chapter.\n", items[1].Body);
    }

    [Fact]
    public void Ids_pad_the_ordinal_to_the_width_the_story_needs()
    {
        var text = "# long\n\n" + string.Concat(Enumerable.Range(1, 123).Select(n => $"## Chapter {n} — T{n}\n\nx\n\n"));
        var items = Chapters.Cut(new Chapters.Story("long", text));
        Assert.Equal(123, items.Count);
        Assert.Equal("long-ch001", items[0].Id);
        Assert.Equal("long-ch123", items[^1].Id);
        Assert.Equal("long#123", items[^1].Locator);
    }

    [Fact]
    public void A_file_with_no_chapter_heading_is_refused()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => Chapters.Cut(new Chapters.Story("plain", "# plain\n\nJust text.\n")));
        Assert.Contains("plain", ex.Message);
    }

    [Fact]
    public void Excluded_stories_are_left_out_and_the_rest_are_cut_in_slug_order()
    {
        var stories = new[]
        {
            new Chapters.Story("zeta", "# zeta\n\n## Chapter 1 — Z\n\nz\n"),
            new Chapters.Story("alpha", "# alpha\n\n## Chapter 1 — A\n\na\n"),
            new Chapters.Story("unread", "# unread\n\n## Chapter 1 — U\n\nu\n"),
        };
        var items = Chapters.CutAll(stories, new HashSet<string>(StringComparer.Ordinal) { "unread" });
        Assert.Equal(["alpha-ch01", "zeta-ch01"], items.Select(i => i.Id).ToArray());
    }

    [Fact]
    public void The_narrowing_names_the_excluded_stories_and_is_absent_when_nothing_is_excluded()
    {
        Assert.Null(Chapters.Narrowing([]));
        Assert.Equal("every story file in the source folder except fallout-equestria, your-human-and-you, the config's exclude list",
            Chapters.Narrowing(["fallout-equestria", "your-human-and-you"]));
    }

    [Fact]
    public void The_rendered_index_parses_back_clean_with_the_itemizer_head_and_escaped_titles()
    {
        var items = Chapters.Cut(new Chapters.Story("about-last-night", Story));
        var text = IndexFile.Render("01-chapters", "tools/StoryPlanner.ChapterItemizer, 2026-09-14 abc1234",
            Chapters.LocatorNotation, null, items.Select(i => (i.Id, i.Locator, i.Description)), Chapters.Narrowing(["unread"]));
        var index = IndexFile.Parse(text);
        Assert.Empty(index.Problems);
        Assert.Equal("01-chapters — index", index.Title);
        Assert.StartsWith("tools/StoryPlanner.ChapterItemizer", index.Itemizer);
        Assert.Null(index.Collator);
        Assert.Contains("unread", index.Narrowing);
        Assert.Equal(2, index.Rows.Count);
        // A title holding the table's own delimiter comes back whole.
        Assert.Equal("about-last-night, chapter 2 of 2: Feel the Love | Part Two", index.Rows[1].Description);
        Assert.Equal("about-last-night#2", index.Rows[1].Locator);
    }
}
