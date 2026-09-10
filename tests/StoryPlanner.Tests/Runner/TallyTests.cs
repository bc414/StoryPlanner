using StoryPlanner.AgentRunner;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The tally: counts per enum field from the rendered results read by the shared parser, the
/// malformed and the missing, free fields listed and never counted; written once, by the host
/// when the last item has a successful call, or by the verb when absent. A grouping by an index
/// column is a printed view. Tier: pure (temp folders).
/// </summary>
public class TallyTests
{
    [Fact]
    public async Task A_tally_counts_the_classes_and_lists_malformed_and_missing_and_a_group_by_is_a_view()
    {
        using var t = new TempBatch();
        t.WriteItems(4);
        var launcher = new FakeLauncher
        {
            AnswerFor = r => r.Item switch
            {
                "item-01" => """{"class":"a","why":"1"}""",
                "item-02" => """{"class":"b","why":"1"}""",
                "item-03" => """{"class":"a","why":"1"}""",
                _ => null,
            },
        };
        var (runner, _) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, null, t.LaunchDir, launcher, new OpenGate(), _ => { }, "test");
        await runner!.RunAsync(CancellationToken.None);
        // item-04 never answered, so the host wrote no tally.
        Assert.False(File.Exists(Path.Combine(t.BatchDir, "tally.md")));
        // A result edited by hand into a shape the declaration does not allow is malformed at the tally.
        File.WriteAllText(Path.Combine(t.BatchDir, "results", "item-03.md"), "- class: a\n- why: 1\n- extra: x\n");

        var (batch, _) = Batch.Load(t.DefinitionPath, t.WorkingDir);
        var text = Tally.Build(batch!);
        Assert.StartsWith("# 01-full — tally\n", text);
        Assert.Contains("- items: 4\n", text);
        Assert.Contains("- answered: 2\n", text);
        Assert.Contains("- malformed: 1\n", text);
        Assert.Contains("- missing: 1\n", text);
        Assert.Contains("## class\n\n| value | count |\n|---|---|\n| a | 1 |\n| b | 1 |\n| cannot-place | 0 |\n", text);
        Assert.Contains("| item-03 | ", text);
        Assert.Contains("- item-04\n", text);
        Assert.Contains("- why (line)\n", text);
        Assert.DoesNotContain("## Flagged", text);
        Assert.DoesNotContain("## By ", text);

        var view = Tally.GroupBy(batch!, "description");
        Assert.StartsWith("## By description\n\n| description | class=a | class=b | class=cannot-place |", view);
        Assert.Contains("| note 2 | 0 | 1 | 0 |", view);
        Assert.Contains("is not a column of the index", Tally.GroupBy(batch!, "colour"));
        Assert.False(File.Exists(Path.Combine(t.BatchDir, "tally.md")));

        var (ok, _) = Tally.Write(batch!);
        Assert.True(ok);
        Assert.True(File.Exists(Path.Combine(t.BatchDir, "tally.md")));
        var (again, message) = Tally.Write(batch!);
        Assert.False(again);
        Assert.Contains("written once", message);
    }

    /// <summary>d-2026-09-09-18: the host writes the tally when the last item has a successful call, so no session step is left to forget.</summary>
    [Fact]
    public async Task The_host_writes_the_tally_when_the_last_item_is_answered_and_a_pilot_does_not()
    {
        using var t = new TempBatch();
        t.WriteItems(2);
        var launcher = new FakeLauncher { AnswerFor = _ => """{"class":"a","why":"1"}""" };
        var log = new List<string>();

        var (pilot, _) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, "item-01", t.LaunchDir, launcher, new OpenGate(), log.Add, "test");
        await pilot!.RunAsync(CancellationToken.None);
        Assert.False(File.Exists(Path.Combine(t.BatchDir, "tally.md")));

        var (rest, _) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, null, t.LaunchDir, launcher, new OpenGate(), log.Add, "test");
        await rest!.RunAsync(CancellationToken.None);
        var tally = Path.Combine(t.BatchDir, "tally.md");
        Assert.True(File.Exists(tally));
        Assert.Contains("- answered: 2\n", File.ReadAllText(tally));
        Assert.Contains(log, l => l.Contains("tally written"));

        // A later execution with nothing to call leaves the written tally alone.
        var written = File.ReadAllText(tally);
        var (third, _) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, null, t.LaunchDir, launcher, new OpenGate(), log.Add, "test");
        await third!.RunAsync(CancellationToken.None);
        Assert.Equal(written, File.ReadAllText(tally));
    }
}
