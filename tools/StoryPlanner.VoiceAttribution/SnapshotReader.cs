using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using StoryPlanner.Core;

namespace StoryPlanner.VoiceAttribution;

/// <summary>
/// Dated planner backups — v1 (<c>TheLionessOfTallTale*.db</c>) and v2 (the app's
/// <c>Backups/*.bak</c> VACUUM INTO copies) — read schema-agnostically: every TEXT column of
/// every table is harvested as plan text. The v1 schema drifted weekly and its note ids never
/// joined to the archive's, so containment of the note's text is the only join.
/// Tables that hold AI output or imported transcripts rather than plan text are skipped —
/// otherwise a Gemini response or a Claude conversation stored in the planner would count as
/// "in the plan before the model said it". The conversation tables exist only in v2 files.
/// </summary>
public static class SnapshotReader
{
    private static readonly HashSet<string> SkipTables = new(StringComparer.OrdinalIgnoreCase)
    {
        "GeminiEntries", "__EFMigrationsHistory", "__EFMigrationsLock", "sqlite_sequence",
        "Conversations", "ConversationBlocks", "IgnoredConversations",
        "ConversationSubjectCoverages", "ConversationSubjectCoverageTracks", "UiSettings",
    };

    public sealed record Loaded(string File, DateOnly Date, string DateSource, int Tables, int TextCells);

    /// <summary>A source loaded by <see cref="LoadAsSources"/> — the plan itself, never a voice.</summary>
    public static bool IsPlanSource(VoiceSource s) => s.Layer.EndsWith("-plan", StringComparison.Ordinal);

    /// <summary>Note-level dating: each backup feeds the plan-snapshot index (FirstSnapshot / PlanFirst).</summary>
    public static List<Loaded> Load(string spec, PlanSnapshotIndex snapshots, Action<string> log) =>
        Harvest(spec, log, (name, date, texts) => snapshots.Add(new PlanSnapshotIndex.Snapshot(name, date), texts));

    /// <summary>
    /// Span-level dating: each backup becomes one role-<c>brian</c> source in the voice index
    /// (<c>{layer}:{file}</c>), dated by its filename. Earliest-wins then credits every run of
    /// a note that was in the plan before any captured AI source to the plan — so a v1 sentence
    /// carried into an edited v2 note keeps its date, where note-level containment would lose it
    /// at the first cut. Text the plan got from an AI source keeps that source, which is older.
    /// Load lineage and conversations first: on the same day the earlier-added source wins.
    /// </summary>
    public static List<Loaded> LoadAsSources(string spec, string layer, VoiceIndex index, Action<string> log) =>
        Harvest(spec, log, (name, date, texts) => index.Add(new VoiceSource($"{layer}:{name}", layer, "brian", date), texts));

    /// <param name="spec">A directory (every <c>*.db</c> in it) or a file pattern such as <c>…/Backups/TLTT v2.2*.bak</c>.</param>
    private static List<Loaded> Harvest(string spec, Action<string> log, Action<string, DateOnly, List<string>> add)
    {
        var loaded = new List<Loaded>();
        foreach (var file in Files(spec))
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
            add(Path.GetFileName(file), date.Value, texts);
            loaded.Add(new Loaded(Path.GetFileName(file), date.Value, dateSource, tables, texts.Count));
            log($"  {Path.GetFileName(file)} → {date:yyyy-MM-dd} ({dateSource}); {tables} tables, {texts.Count} text cells");
        }
        return loaded;
    }

    public static IEnumerable<string> Files(string spec)
    {
        if (Directory.Exists(spec)) return Directory.GetFiles(spec, "*.db").OrderBy(f => f);
        var dir = Path.GetDirectoryName(spec);
        var pattern = Path.GetFileName(spec);
        if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return Array.Empty<string>();
        return Directory.GetFiles(dir, pattern).OrderBy(f => f);
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

    /// <summary>The snapshot's date is the first <c>yyyy-MM-dd</c> in its filename (Brian standardised the v1 names 2026-09-02; the app stamps its v2 backups <c>yyyy-MM-dd_HH-mm-ss</c>); a file without one is skipped and reported.</summary>
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
