using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using StoryPlanner.Core;

namespace StoryPlanner.VoiceAttribution;

/// <summary>
/// Dated v1 planner backups (<c>TheLionessOfTallTale*.db</c>), read schema-agnostically: every
/// TEXT column of every table is harvested as plan text. The v1 schema drifted weekly and its
/// note ids never joined to the archive's, so containment of the note's text is the only join.
/// Tables that hold pasted AI output rather than plan text (<c>GeminiEntries</c>) and EF
/// bookkeeping are skipped — otherwise a Gemini response stored in the planner would count as
/// "in the plan before the model said it".
/// </summary>
public static class SnapshotReader
{
    private static readonly HashSet<string> SkipTables = new(StringComparer.OrdinalIgnoreCase)
    {
        "GeminiEntries", "__EFMigrationsHistory", "__EFMigrationsLock", "sqlite_sequence",
    };

    public sealed record Loaded(string File, DateOnly Date, string DateSource, int Tables, int TextCells);

    public static List<Loaded> Load(string dir, PlanSnapshotIndex snapshots, Action<string> log)
    {
        var loaded = new List<Loaded>();
        foreach (var file in Directory.GetFiles(dir, "*.db").OrderBy(f => f))
        {
            var (date, dateSource) = DateOf(file);
            if (date is null) { log($"  SKIP {Path.GetFileName(file)}: no date parseable from filename"); continue; }

            // immutable=1: these backups are never written, and a plain read-only open of a
            // WAL-mode file still creates -wal/-shm sidecars next to it. Immutable opens don't.
            var uri = "file:" + new Uri(Path.GetFullPath(file)).AbsolutePath + "?immutable=1";
            var cs = new SqliteConnectionStringBuilder { DataSource = uri, Mode = SqliteOpenMode.ReadOnly }.ToString();
            using var conn = new SqliteConnection(cs);
            conn.Open();
            var texts = new List<string>();
            int tables = 0;
            foreach (var table in Tables(conn))
            {
                if (SkipTables.Contains(table)) continue;
                var textCols = TextColumns(conn, table);
                if (textCols.Count == 0) continue;
                tables++;
                using var cmd = conn.CreateCommand();
                cmd.CommandText = $"select {string.Join(", ", textCols.Select(c => $"\"{c}\""))} from \"{table}\"";
                using var r = cmd.ExecuteReader();
                while (r.Read())
                    for (int i = 0; i < textCols.Count; i++)
                        if (!r.IsDBNull(i)) { var v = r.GetString(i); if (v.Length > 0) texts.Add(v); }
            }
            snapshots.Add(new PlanSnapshotIndex.Snapshot(Path.GetFileName(file), date.Value), texts);
            loaded.Add(new Loaded(Path.GetFileName(file), date.Value, dateSource, tables, texts.Count));
            log($"  {Path.GetFileName(file)} → {date:yyyy-MM-dd} ({dateSource}); {tables} tables, {texts.Count} text cells");
        }
        return loaded;
    }

    private static List<string> Tables(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "select name from sqlite_master where type='table'";
        using var r = cmd.ExecuteReader();
        var list = new List<string>();
        while (r.Read()) list.Add(r.GetString(0));
        return list;
    }

    private static List<string> TextColumns(SqliteConnection conn, string table)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"pragma table_info(\"{table}\")";
        using var r = cmd.ExecuteReader();
        var list = new List<string>();
        while (r.Read())
            if (r.GetString(2).Equals("TEXT", StringComparison.OrdinalIgnoreCase)) list.Add(r.GetString(1));
        return list;
    }

    /// <summary>The snapshot's date is the <c>yyyy-MM-dd</c> in its filename (Brian standardised the names 2026-09-02); a file without one is skipped and reported.</summary>
    public static (DateOnly? Date, string Source) DateOf(string file)
    {
        var m = Regex.Match(Path.GetFileNameWithoutExtension(file), @"(\d{4})-(\d{2})-(\d{2})");
        if (!m.Success) return (null, "none");
        return (Make(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[1].Value)), "filename");
    }

    private static DateOnly? Make(int month, int day, int year)
    {
        try { return new DateOnly(year, month, day); } catch { return null; }
    }
}
