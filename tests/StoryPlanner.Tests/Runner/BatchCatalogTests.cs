using StoryPlanner.AgentRunner;
using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The history view and the stage strip are read from a batch folder on disk: the id from the
/// path relative to the working directory, item states from the calls file, stages from what
/// files exist. The items of a batch are the index's, never a pilot's filter. Tier: pure (temp folders).
/// </summary>
public class BatchCatalogTests
{
    static CallEntry Call(string item, int call, int exit, string check, bool pilot = false, string ended = "2026-09-09T20:00:00+00:00") =>
        new(item, call, "sonnet", null, "2.1", "d", "i", "p", "2026-09-09T19:59:00+00:00", ended, exit, check, 0.2, 2, "s", pilot);

    [Fact]
    public void Reads_a_batch_from_disk_with_its_items_and_stages()
    {
        using var t = new TempBatch();
        t.WriteItems(2, kind: "full");
        Directory.CreateDirectory(Path.Combine(t.BatchDir, "attempts", "item-01", "call-1"));
        File.WriteAllText(Path.Combine(t.BatchDir, "calls.md"),
            CallsFile.RenderHead("01-full", "abc") + CallsFile.RenderEntry(Call("item-01", 1, 0, CallEntry.Ok, pilot: true)) + CallsFile.RenderEntry(Call("item-02", 1, 0, CallEntry.Ok)));

        var snap = BatchCatalog.Load(t.DefinitionPath, t.WorkingDir);
        Assert.Equal(t.Id, snap.Id);
        Assert.Equal("verification-of-v1-archive-test", snap.Study);
        Assert.Equal("01-full", snap.Name);
        Assert.Equal("full", snap.Kind);
        Assert.Equal("sonnet", snap.Model);
        Assert.Equal("directions-1", snap.Directions);
        Assert.False(snap.Live);
        Assert.True(snap.Completed);
        Assert.Equal(2, snap.Succeeded);
        Assert.Equal(0.4, snap.CostUsd, 3);
        Assert.Equal("Succeeded", snap.Items[0].State);
        Assert.Equal("note-1", snap.Items[0].Locator);
        Assert.Equal(60, snap.Items[0].LastSeconds);
        Assert.EndsWith(Path.Combine("attempts", "item-01", "call-1", "stream.jsonl"), snap.Items[0].StreamPath);

        var s = snap.Stages;
        Assert.True(s.Defined);
        Assert.True(s.Itemized);
        Assert.True(s.Piloted);
        Assert.True(s.Executed);
        Assert.False(s.Tallied);
        File.WriteAllText(Path.Combine(t.BatchDir, "tally.md"), "# tally\n");
        Assert.True(BatchCatalog.Load(t.DefinitionPath, t.WorkingDir).Stages.Tallied);
    }

    [Fact]
    public async Task A_pilot_leaves_the_rest_of_the_index_pending_whether_read_from_disk_or_from_the_live_execution()
    {
        using var t = new TempBatch();
        t.WriteItems(3);
        File.WriteAllText(Path.Combine(t.BatchDir, "calls.md"), CallsFile.RenderHead("01-full", "abc") + CallsFile.RenderEntry(Call("item-02", 1, 0, CallEntry.Ok, pilot: true)));

        var disk = BatchCatalog.Load(t.DefinitionPath, t.WorkingDir);
        Assert.Equal(3, disk.Items.Count);
        Assert.Equal(["Pending", "Succeeded", "Pending"], disk.Items.Select(i => i.State));
        Assert.Equal(2, disk.Pending);
        Assert.False(disk.Completed);
        Assert.True(disk.Stages.Piloted);
        Assert.False(disk.Stages.Executed);

        var (runner, error) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, "item-02", t.LaunchDir, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Null(error);
        var live = BatchCatalog.Build(t.DefinitionPath, t.WorkingDir, runner);
        Assert.Equal(3, live.Items.Count);
        Assert.Equal(2, live.Pending);

        var (batch, _) = BatchRunner.Create(t.DefinitionPath, t.WorkingDir, null, t.LaunchDir, new FakeLauncher(), new OpenGate(), _ => { }, "test");
        Assert.Equal("3 item(s) — 2 to call, 1 skipped as answered", batch!.Summary());
        await batch.RunAsync(CancellationToken.None);
        var after = BatchCatalog.Build(t.DefinitionPath, t.WorkingDir, batch);
        Assert.True(after.Completed);
        Assert.Equal(0, after.Pending);
    }

    [Fact]
    public void A_batch_with_no_calls_is_itemized_and_one_missing_a_body_is_not()
    {
        using var t = new TempBatch();
        t.WriteItems(2);
        var snap = BatchCatalog.Load(t.DefinitionPath, t.WorkingDir);
        Assert.True(snap.Stages.Defined);
        Assert.True(snap.Stages.Itemized);
        Assert.False(snap.Stages.Piloted);
        Assert.Equal(2, snap.Pending);
        Assert.Null(snap.LastActivityUtc);
        File.Delete(Path.Combine(t.BatchDir, "items", "item-02.md"));
        Assert.False(BatchCatalog.Load(t.DefinitionPath, t.WorkingDir).Stages.Itemized);
    }

    [Fact]
    public void A_definition_that_does_not_read_is_still_listed_with_its_error()
    {
        using var t = new TempBatch();
        t.WriteItems(1);
        File.WriteAllText(t.DefinitionPath, "# 01-full — definition\n\n- model: sonnet\n");
        var snap = BatchCatalog.Load(t.DefinitionPath, t.WorkingDir);
        Assert.NotNull(snap.Error);
        Assert.False(snap.Stages.Defined);
        Assert.Single(snap.Items);
    }

    [Fact]
    public void Definitions_are_found_under_batches_folders_beneath_the_working_directory()
    {
        using var t = new TempBatch();
        t.WriteItems(1);
        var other = Path.Combine(t.StudyDir, "batches", "02-referee");
        Directory.CreateDirectory(other);
        File.WriteAllText(Path.Combine(other, "definition.md"), "# 02-referee — definition\n");
        Directory.CreateDirectory(Path.Combine(t.WorkingDir, "elsewhere"));
        File.WriteAllText(Path.Combine(t.WorkingDir, "elsewhere", "definition.md"), "not under batches");
        var found = BatchCatalog.Definitions(t.WorkingDir).Select(p => Batch.IdFor(Path.GetDirectoryName(p)!, t.WorkingDir)).OrderBy(x => x).ToList();
        Assert.Equal([t.Id, t.Id.Replace("01-full", "02-referee")], found);
    }
}
