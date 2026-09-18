using System.Text;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.TurnItemizer;

/// <summary>
/// The decision cut over code-sessions: in each main session, one item per user turn of the chosen
/// kinds, with the assistant's turn before it — the assistant records since the previous user turn.
/// The roles are who authored a record (extract version 3, the code-sessions skill): a user record
/// is Brian's, save the elided tool results the harness returns in the user role, which are not
/// turns. The assistant's turn keeps its text and its AskUserQuestion and ExitPlanMode calls, whose
/// records carry the question's marker and the plan's text; other tool calls are mechanical names
/// and are dropped. The AskUserQuestion call records no options: which one was recommended shows
/// only where the chosen label says so or the assistant's text says so.
/// </summary>
public static class SessionDecisions
{
    public sealed record Session(string SessionId, string Label, string Date);
    public sealed record Record(long Id, string SessionId, string Uuid, string Role, string Body);

    public const string Answers = "answers", Verdicts = "verdicts", Prompts = "prompts";
    public static readonly string[] Kinds = [Answers, Verdicts, Prompts];

    public const string LocatorNotation =
        "`<session id>#<record uuid>`: the user record with that SessionId and Uuid in codesessions.db; the assistant turn beside it is the session's assistant records between it and the previous user record";

    /// <summary>What a user record is: a turn of one of the three kinds, or null for what is not a turn of Brian's (a tool result, an interrupt, a command invocation).</summary>
    public static string? Kind(string body)
    {
        if (body.StartsWith("[tool result elided", StringComparison.Ordinal)) return null;
        if (body.StartsWith("[Request interrupted", StringComparison.Ordinal)) return null;
        if (body.StartsWith("<command-", StringComparison.Ordinal) || body.StartsWith("<local-command", StringComparison.Ordinal)) return null;
        if (body.StartsWith("[AskUserQuestion", StringComparison.Ordinal)) return Answers;
        if (body.StartsWith("[Plan approved by user]", StringComparison.Ordinal) || body.StartsWith("[Rejected by user]", StringComparison.Ordinal)) return Verdicts;
        return Prompts;
    }

    static bool KeptAssistant(string body) =>
        !body.StartsWith("[tool_use: ", StringComparison.Ordinal)
        || body.StartsWith("[tool_use: AskUserQuestion", StringComparison.Ordinal)
        || body.StartsWith("[tool_use: ExitPlanMode", StringComparison.Ordinal);

    public static string Narrowing(IReadOnlyCollection<string> kinds) =>
        $"in main sessions, the user turns of the kinds {string.Join(", ", kinds)} that follow assistant text; elided tool results, interrupts and command invocations are not turns";

    /// <summary>Records in session order; sessions in the order given.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> Cut(IReadOnlyList<Session> sessions, IReadOnlyList<Record> records, IReadOnlySet<string> kinds)
    {
        var bySession = records.ToLookup(r => r.SessionId);
        var items = new List<ItemizerOutput.Item>();
        foreach (var s in sessions)
        {
            var leadIn = new List<string>();
            foreach (var r in bySession[s.SessionId])
            {
                if (r.Role == "assistant") { if (KeptAssistant(r.Body) && r.Body.Trim().Length > 0) leadIn.Add(r.Body.Trim()); continue; }
                if (r.Role != "user") continue;
                if (r.Body.StartsWith("[tool result elided", StringComparison.Ordinal)) continue;
                var kind = Kind(r.Body);
                // An interrupt stops the assistant, and what Brian types next answers what came
                // before it; a command invocation starts a new stretch.
                if (kind is null) { if (!r.Body.StartsWith("[Request interrupted", StringComparison.Ordinal)) leadIn.Clear(); continue; }
                if (kinds.Contains(kind) && leadIn.Count > 0)
                {
                    var body = new StringBuilder()
                        .Append("# code session: ").Append(s.Label).Append('\n')
                        .Append("date: ").Append(s.Date).Append('\n')
                        .Append("kind: ").Append(kind).Append("\n\n")
                        .Append("## The assistant's turn before it\n\n").Append(string.Join("\n\n", leadIn)).Append("\n\n")
                        .Append("## The user turn\n\n").Append(r.Body.Trim()).Append('\n');
                    items.Add(new ItemizerOutput.Item($"cs-{r.Id}", body.ToString(), $"{r.SessionId}#{r.Uuid}", $"{s.Date} {s.Label}: {kind}"));
                }
                leadIn.Clear();
            }
        }
        return items;
    }

    public static (IReadOnlyList<Session> Sessions, IReadOnlyList<Record> Records) Load(string path)
    {
        using var conn = Sources.Open(path);
        var sessions = Sources.Query(conn, "select SessionId, coalesce(nullif(Title, ''), nullif(Slug, ''), SessionId), FirstTimestamp from Sessions where Kind = 'main' order by FirstTimestamp, SessionId",
            r => new Session(r.GetString(0), r.GetString(1), r.GetString(2).Length >= 10 ? r.GetString(2)[..10] : r.GetString(2)));
        var records = Sources.Query(conn, "select r.Id, r.SessionId, r.Uuid, r.Role, r.Body from Records r join Sessions s on s.SessionId = r.SessionId where s.Kind = 'main' order by r.SessionId, r.Seq, r.Id",
            r => new Record(r.GetInt64(0), r.GetString(1), r.GetString(2), r.GetString(3), r.GetString(4)));
        return (sessions, records);
    }
}
