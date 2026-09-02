using System.Globalization;
using Microsoft.Data.Sqlite;
using StoryPlanner.Core;

namespace StoryPlanner.VoiceAttribution;

/// <summary>
/// Streams lineage.db bodies into a <see cref="VoiceIndex"/>. Two roles only: <c>brian</c>
/// (Gemini prompts, AI Studio and NotebookLM user turns) and <c>model</c> (Gemini responses,
/// AI Studio and NotebookLM model turns). Reports are skipped — they quote entries and would
/// double-count. The pre-AI Google Doc is not a voice source at all. Source ids are the prefixed ids the MCP server's
/// <c>get_lineage</c> accepts, so any row in the output can be opened for its surrounding turn.
/// </summary>
public static class LineageReader
{
    public sealed record Stats(int GeminiEntries, int AiStudioTurns, int NlmTurns, int GDocSnapshots);

    /// <summary>
    /// AI Studio chats whose model turns re-emit their inputs rather than author anything: the
    /// Feb 2026 Note Organizer pipeline (Exhaustive Cartographer, Note Organizer Parts 0/1/2).
    /// Verified 2026-09-02: the Sorter's model turns are 43-77% verbatim other lineage (mostly
    /// Brian's own Gemini prompt text) and its inputs are Drive-document placeholders; no archive
    /// note has its origin in the other three. Excluded from the voice index by Brian's ruling.
    /// </summary>
    public static readonly int[] DefaultExcludedAiStudioChats = { 22, 23, 24, 25 };

    public static Stats Load(string path, VoiceIndex index, PlanSnapshotIndex? planSnapshots, bool includeGDocSnapshots,
        IReadOnlySet<int> excludedAiStudioChats, Action<string> log)
    {
        var cs = new SqliteConnectionStringBuilder { DataSource = path, Mode = SqliteOpenMode.ReadOnly }.ToString();
        using var conn = new SqliteConnection(cs);
        conn.Open();

        int gem = 0, ais = 0, nlm = 0, gdoc = 0, planDumps = 0, skippedTurns = 0;

        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select Id, Date, Prompt, Response, IsPlanPaste from Entries";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var id = r.GetInt32(0);
                var date = ParseDate(r.GetString(1));
                var response = r.GetString(3);
                // Prompt first, response second: on equal dates the earlier-added source keeps a
                // shingle, so a prompt beats the response that repeats it.
                index.Add(new VoiceSource($"gemini:{id}", "gemini", "brian", date), r.GetString(2));
                // APPENDIX-D §D4: one response answers a question by emitting print(open(...)) and
                // dumping the raw plan. That is a copy of the plan, not anyone's voice — it goes to
                // the plan-snapshot index (dated) and never to the voice index.
                bool planDump = response.Length > 0 && response[..Math.Min(300, response.Length)].Contains("print(open(", StringComparison.Ordinal);
                if (planDump)
                {
                    planDumps++;
                    if (planSnapshots is not null && date is DateOnly d) planSnapshots.Add(new PlanSnapshotIndex.Snapshot($"gemini:{id} (plan dump)", d), new[] { response });
                    log($"  gemini:{id} ({date:yyyy-MM-dd}) is a raw plan dump → plan-snapshot index only");
                }
                else index.Add(new VoiceSource($"gemini:{id}", "gemini", "model", date), response);
                gem++;
            }
        }
        log($"  gemini entries: {gem}");

        var chatDates = new Dictionary<string, (int Id, DateOnly? Date)>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select Id, ChatKey, Date from AiStudioChats";
            using var r = cmd.ExecuteReader();
            while (r.Read()) chatDates[r.GetString(1)] = (r.GetInt32(0), ParseDate(r.GetString(2)));
        }
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select ChatKey, TurnIndex, Role, CreateTime, Body, IsPlaceholder from AiStudioTurns";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (r.GetInt32(5) != 0) continue;
                var key = r.GetString(0);
                var (chatId, chatDate) = chatDates.GetValueOrDefault(key, (0, null));
                if (excludedAiStudioChats.Contains(chatId)) { skippedTurns++; continue; }
                var date = r.IsDBNull(3) ? chatDate : (ParseDate(r.GetString(3)) ?? chatDate);
                var role = r.GetString(2).Equals("user", StringComparison.OrdinalIgnoreCase) ? "brian" : "model";
                index.Add(new VoiceSource($"aistudio:{chatId}#t{r.GetInt32(1)}", "aistudio", role, date), r.GetString(4));
                ais++;
            }
        }
        log($"  aistudio turns: {ais} (excluded chats {string.Join(",", excludedAiStudioChats.OrderBy(x => x))}: {skippedTurns} turns skipped)");

        var notebookDates = new Dictionary<string, (int Id, DateOnly? Date)>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select Id, Slug, AuthoredDate from NlmNotebooks";
            using var r = cmd.ExecuteReader();
            while (r.Read()) notebookDates[r.GetString(1)] = (r.GetInt32(0), r.IsDBNull(2) ? null : ParseDateEndOfPrecision(r.GetString(2)));
        }
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select Slug, TurnIndex, Role, Body from NlmTurns";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var (nbId, date) = notebookDates.GetValueOrDefault(r.GetString(0), (0, null));
                var role = r.GetString(2).Equals("user", StringComparison.OrdinalIgnoreCase) ? "brian" : "model";
                index.Add(new VoiceSource($"nlm:{nbId}#t{r.GetInt32(1)}", "notebooklm", role, date), r.GetString(3));
                nlm++;
            }
        }
        log($"  notebooklm turns: {nlm}");

        // The pre-AI Google Doc is NOT a voice source (Brian's decision, 2026-09-02: it is not
        // part of the voice question). Optionally it feeds the plan-snapshot index only, as a
        // dated record of the plan for the PlanFirst rule — off by default.
        if (planSnapshots is not null && includeGDocSnapshots)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "select Id, Date, Body from GDocSnapshots order by Date";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                if (ParseDate(r.GetString(1)) is DateOnly d)
                {
                    planSnapshots.Add(new PlanSnapshotIndex.Snapshot($"gdoc-snapshot:{r.GetInt32(0)}", d), new[] { r.GetString(2) });
                    gdoc++;
                }
            }
            log($"  gdoc snapshots: {gdoc} (plan-snapshot index only; never a voice source)");
        }
        else log("  gdoc: excluded");

        return new Stats(gem, ais, nlm, gdoc);
    }

    public static DateOnly? ParseDate(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        if (DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dto))
            return DateOnly.FromDateTime(dto.DateTime);
        if (DateOnly.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) return d;
        return null;
    }

    /// <summary>"2026" → 2026-12-31, "2026-02" → 2026-02-28, full date as-is. Conservative for earliest-wins: an imprecise date only wins when it is clearly earlier.</summary>
    public static DateOnly? ParseDateEndOfPrecision(string s)
    {
        s = s.Trim();
        if (s.Length == 4 && int.TryParse(s, out var y)) return new DateOnly(y, 12, 31);
        if (s.Length == 7 && int.TryParse(s[..4], out var y2) && int.TryParse(s[5..], out var m))
            return new DateOnly(y2, m, DateTime.DaysInMonth(y2, m));
        return ParseDate(s);
    }
}
