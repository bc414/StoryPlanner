using StoryPlanner.BatchFiles;
using StoryPlanner.ResultsQuery;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The item the query page opens beside a view: which item a view's cell names, and the read of
/// that item's body and result from a batch folder in a temp dir, under the same rule the views
/// count by (a result without a successful call is no result). Tier: Pure, on a throwaway folder.
/// </summary>
public class ItemPageTests : IDisposable
{
    const string BatchId = "exploration-of-x/01-notes";
    readonly string _study = Directory.CreateTempSubdirectory("item-page-").FullName;
    string BatchDir => Path.Combine(_study, "exploration-of-x", "batches", "01-notes");
    string Definition => Path.Combine(BatchDir, "definition.md");

    public ItemPageTests()
    {
        Directory.CreateDirectory(Path.Combine(BatchDir, "items"));
        Directory.CreateDirectory(Path.Combine(BatchDir, "results"));
        File.WriteAllText(Definition, "# 01-notes — definition\n\n- directions: ../../directions-1.md\n- model: m\n- effort: high\n");
        File.WriteAllText(Path.Combine(BatchDir, "index.md"), IndexFile.Render("01-notes", "tools/X", "`note-<id>`", null,
            [("note-1", "note-1", "Applejack, Backstory"), ("note-2", "note-2", "Rarity, Theme Plan"), ("note-3", "note-3", "Spike, History")]));
        File.WriteAllText(Path.Combine(BatchDir, "calls.md"), CallsFile.RenderHead("01-notes", "h")
            + CallsFile.RenderEntry(Call("note-1", 0, "ok"))
            + CallsFile.RenderEntry(Call("note-2", 1, "exit 1")));
        File.WriteAllText(Path.Combine(BatchDir, "items", "note-1.md"), "# working-plan note 1\nowner: Applejack\n\nThe note's text.");
        File.WriteAllText(Path.Combine(BatchDir, "results", "note-1.md"), "- claims:\n  - History | a fact | text | yes\n");
        File.WriteAllText(Path.Combine(BatchDir, "results", "note-2.md"), "- claims:\n  - left from a failed call\n");
    }

    static CallEntry Call(string item, int exit, string check) =>
        new(item, 1, "m", "high", "h", "d", "i", "p", "2026-09-27T00:00:00Z", "2026-09-27T00:00:01Z", exit, check, null, null, null);

    public void Dispose() => Directory.Delete(_study, recursive: true);

    [Fact]
    public void A_cell_names_its_item_by_id_or_by_a_cites_token_of_this_batch_only()
    {
        Assert.Equal("note-11", ItemPage.ItemOf(BatchId, "note-11"));
        Assert.Equal("note-11", ItemPage.ItemOf(BatchId, $"{BatchId}/note-11"));
        Assert.Equal("note-11", ItemPage.ItemOf(BatchId, $"  {BatchId}/note-11 "));
        Assert.Null(ItemPage.ItemOf(BatchId, "exploration-of-y/01-notes/note-11"));
    }

    [Fact]
    public void An_answered_item_carries_its_index_row_body_and_result_as_the_files_hold_them()
    {
        var page = ItemPage.Load(Definition, "note-1")!;
        Assert.Equal("Applejack, Backstory", page.Description);
        Assert.Equal("# working-plan note 1\nowner: Applejack\n\nThe note's text.", page.Body);
        Assert.Contains("History | a fact | text | yes", page.Result);
    }

    [Fact]
    public void A_result_file_without_a_successful_call_is_no_result_and_a_missing_item_file_is_no_body()
    {
        var failed = ItemPage.Load(Definition, "note-2")!;
        Assert.Null(failed.Result);
        Assert.Null(failed.Body);
        var uncalled = ItemPage.Load(Definition, "note-3")!;
        Assert.Null(uncalled.Result);
        Assert.Null(ItemPage.Load(Definition, "note-9"));
    }

    [Fact]
    public void Markdown_keeps_the_files_line_breaks_and_shows_angle_brackets_as_text()
    {
        var html = MarkdownView.Render("owner: Applejack\ntrack: Backstory\n\n<b>not markup</b>").Value;
        Assert.Contains("owner: Applejack<br", html);
        Assert.Contains("&lt;b&gt;not markup&lt;/b&gt;", html);
        Assert.Equal("", MarkdownView.Render(" ").Value);
    }
}
