using Microsoft.Data.Sqlite;

namespace StoryPlanner.TurnItemizer;

/// <summary>
/// Reads the dialogue layers into conversations of records, each record carrying the source id
/// the MCP tools cite. Only the text as it was exchanged is read: the Gemini corpus index's
/// labels and summaries, the lineage reports, a conversation's arc summary and its blocks' navigation
/// notes are not corpus data and are never read. Everything opens read-only.
/// </summary>
public static class Sources
{
    public const string Gemini = "gemini", AiStudio = "aistudio", NotebookLm = "notebooklm", Conversations = "conversations";
    public static readonly string[] Layers = [Gemini, AiStudio, NotebookLm, Conversations];

    /// <summary>
    /// The Gemini web layer: each entry is a prompt and its response, grouped into threads by the
    /// corpus index and ordered by thread position. Activity entries (Canvas records with no
    /// prompt) are not dialogue and are not read.
    /// </summary>
    public static IEnumerable<Turns.Conversation> ReadGemini(string lineage)
    {
        using var conn = Open(lineage);
        var rows = Query(conn, "select Id, ThreadId, Date, Prompt, Response from Entries where Type <> 'activity' order by ThreadId, ThreadPos, Id",
            r => (Id: r.GetInt32(0), Thread: r.GetString(1), Date: r.GetString(2), Prompt: r.GetString(3), Response: r.GetString(4)));
        foreach (var g in rows.GroupBy(r => r.Thread))
        {
            var records = new List<Turns.Record>();
            foreach (var e in g)
            {
                if (e.Prompt.Trim().Length > 0) records.Add(new Turns.Record($"gemini:{e.Id} prompt", Turns.User, e.Prompt));
                if (e.Response.Trim().Length > 0) records.Add(new Turns.Record($"gemini:{e.Id} response", Turns.Model, e.Response));
            }
            yield return new Turns.Conversation("Gemini web (lineage)", $"thread {g.Key}", DateSpan(g.Select(e => e.Date)), records);
        }
    }

    /// <summary>The AI Studio layer: each chat's turns in order; an attached Drive document is a placeholder record.</summary>
    public static IEnumerable<Turns.Conversation> ReadAiStudio(string lineage)
    {
        using var conn = Open(lineage);
        var chats = Query(conn, "select Id, ChatKey, Title, Date from AiStudioChats order by Id",
            r => (Id: r.GetInt32(0), Key: r.GetString(1), Title: r.GetString(2), Date: r.GetString(3)));
        var turns = Query(conn, "select ChatKey, TurnIndex, Role, Body, IsPlaceholder from AiStudioTurns order by ChatKey, TurnIndex",
            r => (Key: r.GetString(0), Index: r.GetInt32(1), Role: r.GetString(2), Body: r.GetString(3), Placeholder: r.GetInt32(4) != 0))
            .ToLookup(t => t.Key);
        foreach (var c in chats)
            yield return new Turns.Conversation("AI Studio (lineage)", $"chat {c.Id} \"{c.Title}\"", Day(c.Date),
                turns[c.Key].Select(t => new Turns.Record($"aistudio:{c.Id}#t{t.Index}", Role(t.Role), t.Body, t.Placeholder)).ToList());
    }

    /// <summary>The NotebookLM layer: each notebook's chat in order; its date is Brian's authored assignment, undated when unresolved.</summary>
    public static IEnumerable<Turns.Conversation> ReadNotebookLm(string lineage)
    {
        using var conn = Open(lineage);
        var notebooks = Query(conn, "select Id, Slug, Title, AuthoredDate from NlmNotebooks order by Id",
            r => (Id: r.GetInt32(0), Slug: r.GetString(1), Title: r.GetString(2), Date: r.IsDBNull(3) ? "undated" : r.GetString(3)));
        var turns = Query(conn, "select Slug, TurnIndex, Role, Body from NlmTurns order by Slug, TurnIndex",
            r => (Slug: r.GetString(0), Index: r.GetInt32(1), Role: r.GetString(2), Body: r.GetString(3)))
            .ToLookup(t => t.Slug);
        foreach (var n in notebooks)
            yield return new Turns.Conversation("NotebookLM (lineage)", $"notebook {n.Id} \"{n.Title}\"", n.Date,
                turns[n.Slug].Select(t => new Turns.Record($"nlm:{n.Id}#t{t.Index}", Role(t.Role), t.Body)).ToList());
    }

    /// <summary>The conversations corpus: each conversation's blocks in order, compaction blocks left out; the platform is carried in the layer's name.</summary>
    public static IEnumerable<Turns.Conversation> ReadConversations(string plan)
    {
        using var conn = Open(plan);
        var conversations = Query(conn, "select Id, Title, ConversationDate, Platform from Conversations order by Id",
            r => (Id: r.GetInt32(0), Title: r.GetString(1), Date: r.GetString(2), Platform: r.GetString(3)));
        var blocks = Query(conn, "select Id, ConversationId, Speaker, RawContent from ConversationBlocks where IsCompaction = 0 order by ConversationId, BlockNumber",
            r => (Id: r.GetInt32(0), Conversation: r.GetInt32(1), Speaker: r.GetString(2), Text: r.GetString(3)))
            .ToLookup(b => b.Conversation);
        foreach (var c in conversations)
            yield return new Turns.Conversation($"conversations ({c.Platform})", $"conversation {c.Id} \"{c.Title}\"", Day(c.Date),
                blocks[c.Id].Select(b => new Turns.Record($"block:{b.Id}", Role(b.Speaker), b.Text)).ToList());
    }

    static string Role(string role) => role.Equals("user", StringComparison.OrdinalIgnoreCase) ? Turns.User : Turns.Model;

    static string Day(string date) => date.Length >= 10 ? date[..10] : date;

    static string DateSpan(IEnumerable<string> dates)
    {
        var days = dates.Select(Day).Where(d => d.Length > 0).OrderBy(d => d, StringComparer.Ordinal).ToList();
        if (days.Count == 0) return "undated";
        return days[0] == days[^1] ? days[0] : $"{days[0]} to {days[^1]}";
    }

    internal static SqliteConnection Open(string path)
    {
        var conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = path, Mode = SqliteOpenMode.ReadOnly }.ToString());
        conn.Open();
        return conn;
    }

    internal static List<T> Query<T>(SqliteConnection conn, string sql, Func<SqliteDataReader, T> map)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        var list = new List<T>();
        while (r.Read()) list.Add(map(r));
        return list;
    }
}
