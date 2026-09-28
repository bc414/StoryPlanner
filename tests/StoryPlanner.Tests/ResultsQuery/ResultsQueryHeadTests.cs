using Bunit;
using StoryPlanner.ResultsQuery;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The query page's leaf components, parameter-driven with nothing registered: the view table
/// renders the rows as given, and the snapshot note appears only when the batch has moved on
/// from the count the query carries. Tier: RazorComponents (bUnit).
/// </summary>
public class ResultsQueryHeadTests : BunitContext
{
    [Fact]
    public void ViewTable_renders_caption_columns_and_every_cell()
    {
        var cut = Render<ViewTable>(p => p
            .Add(c => c.Caption, "2 line(s) of moments")
            .Add(c => c.Columns, ["item", "technique", "experience"])
            .Add(c => c.Rows, [["a-ch01", "comic dialogue", "light amusement"], ["b-ch01", "dramatic irony", "comic dread"]]));
        Assert.Contains("2 line(s) of moments", cut.Find(".caption").TextContent);
        Assert.Equal(3, cut.FindAll("thead th").Count);
        var rows = cut.FindAll("tbody tr");
        Assert.Equal(2, rows.Count);
        Assert.Equal("dramatic irony", rows[1].QuerySelectorAll("td")[1].TextContent);
    }

    [Fact]
    public void ViewTable_says_nothing_when_there_are_no_rows()
    {
        var cut = Render<ViewTable>(p => p.Add(c => c.Caption, "0 line(s)").Add(c => c.Columns, ["item"]).Add(c => c.Rows, []));
        Assert.Empty(cut.FindAll("table"));
        Assert.Contains("nothing", cut.Find(".muted").TextContent);
    }

    [Fact]
    public void ViewTable_makes_item_and_cites_cells_open_their_item_only_when_asked()
    {
        string? opened = null;
        var cut = Render<ViewTable>(p => p
            .Add(c => c.Caption, "1 line(s)")
            .Add(c => c.Columns, ["item", "goal"])
            .Add(c => c.Rows, [["note-11", "the reader believes"]])
            .Add(c => c.OnItem, (string s) => opened = s));
        var links = cut.FindAll("button.item-link");
        Assert.Single(links);                                   // the item cell, not the goal cell
        links[0].Click();
        Assert.Equal("note-11", opened);

        var cites = Render<ViewTable>(p => p.Add(c => c.Columns, ["cites"]).Add(c => c.Rows, [["x/01-notes/note-11"]]).Add(c => c.OnItem, (string s) => opened = s));
        cites.Find("button.item-link").Click();
        Assert.Equal("x/01-notes/note-11", opened);

        var plain = Render<ViewTable>(p => p.Add(c => c.Columns, ["item"]).Add(c => c.Rows, [["note-11"]]));
        Assert.Empty(plain.FindAll("button.item-link"));        // no callback, no links
    }

    [Fact]
    public void ItemPanel_renders_body_and_result_and_says_which_file_is_absent()
    {
        var closed = false;
        var cut = Render<ItemPanel>(p => p
            .Add(c => c.Page, new ItemPage("note-1", "note-1", "Applejack, Backstory", "# working-plan note 1\nowner: Applejack", "- claims:\n  - History | a fact | text | yes"))
            .Add(c => c.OnClose, () => closed = true));
        var cols = cut.FindAll(".item-cols .md");
        Assert.Equal(2, cols.Count);
        Assert.Contains("owner: Applejack", cols[0].TextContent);
        Assert.Contains("History | a fact | text | yes", cols[1].TextContent);
        cut.Find("button.close").Click();
        Assert.True(closed);

        var bare = Render<ItemPanel>(p => p.Add(c => c.Page, new ItemPage("note-3", "note-3", "Spike, History", null, null)));
        Assert.Empty(bare.FindAll(".md"));
        Assert.Contains("no item file", bare.Markup);
        Assert.Contains("no successful call", bare.Markup);
    }

    [Fact]
    public void SnapshotNote_appears_only_when_the_counts_differ()
    {
        var same = Render<SnapshotNote>(p => p.Add(c => c.Expected, 600).Add(c => c.Actual, 600));
        Assert.Empty(same.FindAll(".note"));
        var moved = Render<SnapshotNote>(p => p.Add(c => c.Expected, 600).Add(c => c.Actual, 720));
        Assert.Contains("600", moved.Find(".note").TextContent);
        Assert.Contains("720", moved.Find(".note").TextContent);
    }
}
