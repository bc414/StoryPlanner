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
    public void SnapshotNote_appears_only_when_the_counts_differ()
    {
        var same = Render<SnapshotNote>(p => p.Add(c => c.Expected, 600).Add(c => c.Actual, 600));
        Assert.Empty(same.FindAll(".note"));
        var moved = Render<SnapshotNote>(p => p.Add(c => c.Expected, 600).Add(c => c.Actual, 720));
        Assert.Contains("600", moved.Find(".note").TextContent);
        Assert.Contains("720", moved.Find(".note").TextContent);
    }
}
