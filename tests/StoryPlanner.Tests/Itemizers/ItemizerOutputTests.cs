using System;
using System.IO;
using System.Linq;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// What every itemizer shares (pure tier, a temp folder): a batch folder with an index is refused,
/// ids the index checker would reject are caught before anything is written, and what is written
/// parses back clean as an index with its bodies beside it.
/// </summary>
public class ItemizerOutputTests : IDisposable
{
    readonly string _dir = Directory.CreateTempSubdirectory("itemizer-output-").FullName;

    public void Dispose() => Directory.Delete(_dir, recursive: true);

    [Fact]
    public void A_missing_folder_and_a_folder_with_an_index_are_refused()
    {
        Assert.NotNull(ItemizerOutput.Refusal(Path.Combine(_dir, "absent")));
        var batch = Directory.CreateDirectory(Path.Combine(_dir, "01-x")).FullName;
        Assert.Null(ItemizerOutput.Refusal(batch));
        File.WriteAllText(Path.Combine(batch, "index.md"), "");
        Assert.Contains("once per batch", ItemizerOutput.Refusal(batch));
    }

    [Fact]
    public void Ids_that_are_not_slugs_or_repeat_and_empty_locators_are_problems()
    {
        var problems = ItemizerOutput.Problems([
            new("ok-1", "b", "l", "d"),
            new("Not_A_Slug", "b", "l", "d"),
            new("ok-1", "b", "l", "d"),
            new("ok-2", "b", " ", "d"),
        ]);
        Assert.Equal(3, problems.Count);
        Assert.Contains(problems, p => p.Contains("Not_A_Slug"));
        Assert.Contains(problems, p => p.Contains("repeats"));
        Assert.Contains(problems, p => p.Contains("empty locator"));
        Assert.Single(ItemizerOutput.Problems([]));
    }

    [Fact]
    public void What_is_written_parses_back_as_an_index_with_the_bodies_beside_it()
    {
        var batch = Directory.CreateDirectory(Path.Combine(_dir, "02-cut")).FullName;
        ItemizerOutput.Write(batch, "tools/StoryPlanner.Test, 2026-09-17 abc1234", "`x`: a test locator",
            [new("item-a", "body a\n", "a | 1", "first"), new("item-b", "body b\n", "b", "second")], "the test's narrowing");

        var index = IndexFile.Read(Path.Combine(batch, "index.md"));
        Assert.Empty(index.Problems);
        Assert.Equal("02-cut — index", index.Title);
        Assert.Equal("the test's narrowing", index.Narrowing);
        Assert.Equal(["item-a", "item-b"], index.Rows.Select(r => r.Item).ToArray());
        Assert.Equal("a | 1", index.Rows[0].Locator);
        Assert.Equal("body a\n", File.ReadAllText(Path.Combine(batch, "items", "item-a.md")));
    }
}
