using StoryPlanner.AgentRunner;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The tally verb: counts per enum field from the rendered results read by the shared parser,
/// the flagged items, the malformed and the missing, free fields listed and never counted, a
/// grouping by an index column; written once. Tier: pure (temp folders).
/// </summary>
public class TallyTests
{
    [Fact]
    public async Task A_tally_counts_the_classes_flags_a_named_value_and_lists_malformed_and_missing()
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
        // A result edited by hand into a shape the declaration does not allow is malformed at the tally.
        File.WriteAllText(Path.Combine(t.BatchDir, "results", "item-03.md"), "- class: a\n- why: 1\n- extra: x\n");

        var (batch, _) = Batch.Load(t.DefinitionPath, t.WorkingDir);
        var text = Tally.Build(batch!, [new Tally.Flag("class", "b")], "description");
        Assert.StartsWith("# 01-full — tally\n", text);
        Assert.Contains("- items: 4\n", text);
        Assert.Contains("- answered: 2\n", text);
        Assert.Contains("- malformed: 1\n", text);
        Assert.Contains("- missing: 1\n", text);
        Assert.Contains("## class\n\n| value | count |\n|---|---|\n| a | 1 |\n| b | 1 |\n| cannot-place | 0 |\n", text);
        Assert.Contains("| item-02 | class | b |", text);
        Assert.Contains("| item-03 | ", text);
        Assert.Contains("- item-04\n", text);
        Assert.Contains("- why (line)\n", text);
        Assert.Contains("## By description\n\n| description | class=a | class=b | class=cannot-place |", text);
        Assert.Contains("| note 2 | 0 | 1 | 0 |", text);

        var (ok, _) = Tally.Write(batch!, [], null);
        Assert.True(ok);
        Assert.True(File.Exists(Path.Combine(t.BatchDir, "tally.md")));
        var (again, message) = Tally.Write(batch!, [], null);
        Assert.False(again);
        Assert.Contains("written once", message);
    }
}
