using StoryPlanner.PocketReader;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The reader never migrates. A file missing a reader migration is refused as older; a file
/// with a migration beyond the reader's latest is refused as newer; a file with extra ids that
/// sort before the reader's latest is accepted, because those are orphaned history (applied
/// migrations whose source was later removed from the repo), which the real working plan has.
/// </summary>
public class MigrationGateTests
{
    private static readonly string[] Reader = ["20260101_A", "20260201_B", "20260301_C"];

    [Fact]
    public void Identical_sets_are_compatible()
    {
        var (verdict, _) = MigrationGate.Check(Reader, Reader);
        Assert.Equal(MigrationVerdict.Compatible, verdict);
    }

    [Fact]
    public void File_with_migration_beyond_latest_known_is_newer()
    {
        var (verdict, detail) = MigrationGate.Check([.. Reader, "20260401_D"], Reader);
        Assert.Equal(MigrationVerdict.FileNewer, verdict);
        Assert.Contains("20260401_D", detail);
    }

    [Fact]
    public void File_missing_a_reader_migration_is_older()
    {
        var (verdict, detail) = MigrationGate.Check(Reader[..2], Reader);
        Assert.Equal(MigrationVerdict.FileOlder, verdict);
        Assert.Contains("20260301_C", detail);
        Assert.Contains("desktop app", detail);
    }

    [Fact]
    public void Empty_history_is_older_not_compatible()
    {
        var (verdict, _) = MigrationGate.Check([], Reader);
        Assert.Equal(MigrationVerdict.FileOlder, verdict);
    }

    [Fact]
    public void Orphaned_older_ids_in_the_file_are_accepted_and_disclosed()
    {
        // The real TLTT v2.storyplan carries 20260531195318_expansionmode and
        // 20260531195405_linkAndAudit, whose source files are gone; both sort before the
        // reader's latest and must not block reading.
        var (verdict, detail) = MigrationGate.Check(["20260101_A", "20260115_removed", "20260201_B", "20260301_C"], Reader);
        Assert.Equal(MigrationVerdict.Compatible, verdict);
        Assert.Contains("1 older id", detail);
    }

    [Fact]
    public void Older_wins_when_both_directions_differ()
    {
        // Lacks C and has D: opening in the app would add C, and the rebuilt reader would then
        // know D, so "open in the app first" is the first actionable step.
        var (verdict, _) = MigrationGate.Check(["20260101_A", "20260201_B", "20260401_D"], Reader);
        Assert.Equal(MigrationVerdict.FileOlder, verdict);
    }
}
