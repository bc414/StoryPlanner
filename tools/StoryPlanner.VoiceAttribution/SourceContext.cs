using Microsoft.Data.Sqlite;
using StoryPlanner.Core;

namespace StoryPlanner.VoiceAttribution;

/// <summary>Fetches the text of one lineage source by its prefixed id (cached), for the echo check and the calibration sheet.</summary>
public sealed class SourceContext : IDisposable
{
    private readonly SqliteConnection _conn;
    private readonly Dictionary<(string, string), (string Text, string Prompt)> _cache = new();

    public SourceContext(string lineagePath)
    {
        _conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = lineagePath, Mode = SqliteOpenMode.ReadOnly }.ToString());
        _conn.Open();
    }

    public void Dispose() => _conn.Dispose();

    /// <returns>(source text, the Brian prompt that produced it if the source is a model response)</returns>
    public (string Text, string Prompt) Fetch(string id, string role)
    {
        if (_cache.TryGetValue((id, role), out var hit)) return hit;
        var result = FetchUncached(id, role);
        _cache[(id, role)] = result;
        return result;
    }

    private (string Text, string Prompt) FetchUncached(string id, string role)
    {
        try
        {
            if (id.StartsWith("gemini:"))
            {
                using var cmd = _conn.CreateCommand();
                cmd.CommandText = "select Prompt, Response from Entries where Id = $id";
                cmd.Parameters.AddWithValue("$id", int.Parse(id[7..]));
                using var r = cmd.ExecuteReader();
                if (!r.Read()) return ("", "");
                return role == "model" ? (r.GetString(1), r.GetString(0)) : (r.GetString(0), "");
            }
            if (id.StartsWith("aistudio:"))
            {
                var (chatId, turn) = SplitTurn(id[9..]);
                using var cmd = _conn.CreateCommand();
                cmd.CommandText = "select t.Body from AiStudioTurns t join AiStudioChats c on c.ChatKey = t.ChatKey where c.Id = $c and t.TurnIndex = $t";
                cmd.Parameters.AddWithValue("$c", chatId); cmd.Parameters.AddWithValue("$t", turn);
                return ((cmd.ExecuteScalar() as string) ?? "", "");
            }
            if (id.StartsWith("nlm:"))
            {
                var (nbId, turn) = SplitTurn(id[4..]);
                using var cmd = _conn.CreateCommand();
                cmd.CommandText = "select t.Body from NlmTurns t join NlmNotebooks n on n.Slug = t.Slug where n.Id = $n and t.TurnIndex = $t";
                cmd.Parameters.AddWithValue("$n", nbId); cmd.Parameters.AddWithValue("$t", turn);
                return ((cmd.ExecuteScalar() as string) ?? "", "");
            }
        }
        catch (Exception e) { return ($"(lookup failed: {e.Message})", ""); }
        return ("", "");
    }

    private static (int, int) SplitTurn(string s)
    {
        var parts = s.Split("#t");
        return (int.Parse(parts[0]), parts.Length > 1 ? int.Parse(parts[1]) : 0);
    }

    /// <summary>Token index in <paramref name="source"/> where <paramref name="passage"/> begins (first 8 tokens probed), or -1.</summary>
    public static int Locate(List<VoiceText.Token> st, string passage)
    {
        var pt = VoiceText.Tokenize(passage);
        if (pt.Count == 0 || st.Count == 0) return -1;
        int probeLen = Math.Min(pt.Count, 8);
        for (int i = 0; i + probeLen <= st.Count; i++)
        {
            bool ok = true;
            for (int j = 0; j < probeLen; j++) if (st[i + j].Text != pt[j].Text) { ok = false; break; }
            if (ok) return i;
        }
        return -1;
    }

    /// <summary>The source text immediately before the passage (up to <paramref name="chars"/> characters), or null if the passage isn't located.</summary>
    public static string? LeadInBefore(string source, string passage, int chars)
    {
        if (string.IsNullOrEmpty(source)) return null;
        var st = VoiceText.Tokenize(source);
        int at = Locate(st, passage);
        if (at < 0) return null;
        int end = st[at].Start;
        return source[Math.Max(0, end - chars)..end];
    }

    /// <summary>The source text windowed around where the note's matched passage sits, ±<paramref name="words"/> words.</summary>
    public static string Window(string source, string passage, int words = 60)
    {
        if (string.IsNullOrEmpty(source)) return "(empty)";
        var st = VoiceText.Tokenize(source);
        var pt = VoiceText.Tokenize(passage);
        if (pt.Count == 0 || st.Count == 0) return Trunc(source, 600);
        int at = Locate(st, passage);
        if (at < 0) return "(passage not located in source text — showing head)\n" + Trunc(source, 600);
        int from = Math.Max(0, at - words), to = Math.Min(st.Count - 1, at + pt.Count + words);
        var slice = source[st[from].Start..st[to].End];
        return (from > 0 ? "…" : "") + slice.Replace("\r", "") + (to < st.Count - 1 ? "…" : "");
    }

    private static string Trunc(string s, int n) => s.Length <= n ? s : s[..n] + "…";
}
