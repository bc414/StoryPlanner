namespace StoryPlanner.PocketReader;

public enum MigrationVerdict
{
    /// <summary>The file has every migration this reader was built with, and nothing beyond its latest.</summary>
    Compatible,
    /// <summary>The file has a migration that sorts after the reader's latest: rebuild the reader.</summary>
    FileNewer,
    /// <summary>This reader knows a migration the file lacks: open it in the app once, then copy again.</summary>
    FileOlder
}

/// <summary>
/// The reader never migrates. It compares the file's applied migration ids with the ids
/// compiled into StoryPlanner.Core. Two refusals: the file lacks a migration the reader has
/// (older), or the file has one that sorts after the reader's latest (newer). A third case is
/// allowed on purpose: ids in the file that the reader does not know but that sort BEFORE its
/// latest. Those are orphaned history, migrations that were applied and whose source files were
/// later removed from the repo (the real working plan carries two from 2026-05-31), and they say
/// nothing about the schema the reader will find. Pure: takes two id sets, returns a verdict.
/// </summary>
public static class MigrationGate
{
    public static (MigrationVerdict Verdict, string Detail) Check(
        IReadOnlyCollection<string> fileMigrationIds,
        IReadOnlyCollection<string> readerMigrationIds)
    {
        var file = fileMigrationIds.ToHashSet(StringComparer.Ordinal);
        var reader = readerMigrationIds.ToHashSet(StringComparer.Ordinal);

        var onlyInReader = reader.Except(file).OrderBy(x => x, StringComparer.Ordinal).ToList();
        if (onlyInReader.Count > 0)
            return (MigrationVerdict.FileOlder,
                $"This file is behind the reader by {onlyInReader.Count} migration(s), up to {onlyInReader[^1]}. " +
                "Open it in the desktop app once (which migrates it), close the app, and copy it again.");

        var latestKnown = reader.Count > 0 ? reader.Max(StringComparer.Ordinal)! : "";
        var onlyInFile = file.Except(reader).OrderBy(x => x, StringComparer.Ordinal).ToList();
        var beyond = onlyInFile.Where(id => string.CompareOrdinal(id, latestKnown) > 0).ToList();
        if (beyond.Count > 0)
            return (MigrationVerdict.FileNewer,
                $"This file was migrated past what this reader knows ({beyond[^1]}). " +
                "Rebuild and redeploy the reader from the current code.");

        var orphaned = onlyInFile.Count == 0 ? "" : $", plus {onlyInFile.Count} older id(s) no longer in the code";
        return (MigrationVerdict.Compatible, $"{reader.Count} migrations, matching the reader{orphaned}.");
    }
}
