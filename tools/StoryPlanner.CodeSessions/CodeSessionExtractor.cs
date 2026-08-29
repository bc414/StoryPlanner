using System.Text;
using System.Text.Json;

namespace StoryPlanner.CodeSessions;

public sealed record ExtractedRecord(string Uuid, string? ParentUuid, string Timestamp, string Role, string Body);

public sealed record ExtractedSession(
    string? Title,
    string? Slug,
    IReadOnlyList<ExtractedRecord> Records,
    int DuplicateUuids,
    int MalformedLines,
    int EmptyRecords,
    int LargePasteStubs)
{
    /// <summary>Total chars of assistant content — the signal for minimum-content filtering.</summary>
    public long AssistantChars => Records.Where(r => r.Role == "assistant").Sum(r => (long)r.Body.Length);
}

/// <summary>
/// Reduces one Claude Code session transcript (JSONL, one record per line) to its dialogue
/// record. The extraction line is communication vs computation (the author's policy,
/// 2026-08-17): keep what was SAID — user and assistant text verbatim — stub what was DONE
/// (each tool call becomes a one-liner naming the tool and its main argument), and drop what
/// was COMPUTED (thinking, tool-result payloads). Every stub is a mechanical disclosure:
/// "[tool result elided — N chars]" means the bytes were never stored, not that they are
/// being withheld.
///
/// Records keep uuid/parentUuid, so a rewound session's branches stay visible in timestamp
/// order — the DAG is never linearized into one reconstructed thread. The record-type filter
/// is an ALLOW-list (user, assistant, ai-title), not a deny-list: the transcript format grows
/// new metadata record types over time and none of them are dialogue.
/// </summary>
public static class CodeSessionExtractor
{
    /// <summary>Same threshold as the gemini layer's plan-paste stub.</summary>
    private const int LargePasteWordThreshold = 20_000;

    public static ExtractedSession Extract(IEnumerable<string> lines)
    {
        string? title = null;
        string? slug = null;
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var records = new List<ExtractedRecord>();
        int duplicates = 0, malformed = 0, empty = 0, largePastes = 0;
        var lastTimestamp = "";

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(line);
            }
            catch (JsonException)
            {
                // A live session appends in place; the last line of a file copied mid-write is
                // legitimately torn. Count it, never fail on it.
                malformed++;
                continue;
            }

            using (doc)
            {
                var root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Object) { malformed++; continue; }

                var type = GetString(root, "type");

                if (type == "ai-title")
                {
                    // The platform retitles as the session evolves — last one wins. A machine
                    // label, stored as a label, never as the author's words.
                    var t = GetString(root, "aiTitle");
                    if (t.Length > 0) title = t;
                    continue;
                }

                if (type is not ("user" or "assistant")) continue;

                var recordSlug = GetString(root, "slug");
                if (recordSlug.Length > 0) slug = recordSlug;

                var uuid = GetString(root, "uuid");
                if (uuid.Length == 0) { malformed++; continue; }
                if (!seen.Add(uuid)) { duplicates++; continue; }

                var timestamp = GetString(root, "timestamp");
                if (timestamp.Length == 0) timestamp = lastTimestamp;
                else lastTimestamp = timestamp;

                var parentUuid = GetString(root, "parentUuid");

                var body = root.TryGetProperty("message", out var message) &&
                           message.ValueKind == JsonValueKind.Object &&
                           message.TryGetProperty("content", out var content)
                    ? MapContent(content)
                    : "";

                if (body.Length == 0) { empty++; continue; }

                if (type == "user" && WordCount(body) > LargePasteWordThreshold)
                {
                    body = $"[Large paste — {WordCount(body):N0} words, {body.Length:N0} chars]";
                    largePastes++;
                }

                records.Add(new ExtractedRecord(
                    uuid,
                    parentUuid.Length > 0 ? parentUuid : null,
                    timestamp,
                    type,
                    body));
            }
        }

        // Timestamp order, input order as the tiebreak (OrderBy is stable).
        var ordered = records.OrderBy(r => r.Timestamp, StringComparer.Ordinal).ToList();

        return new ExtractedSession(title, slug, ordered, duplicates, malformed, empty, largePastes);
    }

    /// <summary>message.content is either a plain string (user turns) or an array of typed parts.</summary>
    private static string MapContent(JsonElement content)
    {
        if (content.ValueKind == JsonValueKind.String)
            return (content.GetString() ?? "").Trim();
        if (content.ValueKind != JsonValueKind.Array)
            return "";

        var parts = new List<string>();
        foreach (var part in content.EnumerateArray())
        {
            if (part.ValueKind != JsonValueKind.Object) continue;
            var kind = GetString(part, "type");
            switch (kind)
            {
                case "text":
                    var text = GetString(part, "text").Trim();
                    if (text.Length > 0) parts.Add(text);
                    break;

                case "thinking":
                case "redacted_thinking":
                    break; // computed, not communicated — dropped without a marker

                case "tool_use":
                    parts.Add(ToolUseStub(part));
                    break;

                case "tool_result":
                    parts.Add($"[tool result elided — {ToolResultChars(part):N0} chars]");
                    break;

                case "image":
                    parts.Add("[image attached]");
                    break;

                default:
                    if (kind.Length > 0) parts.Add($"[{kind} part elided]");
                    break;
            }
        }
        return string.Join("\n\n", parts).Trim();
    }

    /// <summary>
    /// "[tool_use: Edit — WorldDateModel.cs]" — the tool name plus the first recognizable main
    /// argument. Mechanical field extraction, not a summary.
    /// </summary>
    private static string ToolUseStub(JsonElement part)
    {
        var name = GetString(part, "name");
        var arg = "";
        if (part.TryGetProperty("input", out var input) && input.ValueKind == JsonValueKind.Object)
        {
            foreach (var key in (string[])["file_path", "path", "pattern", "query", "command", "url", "skill", "description", "prompt"])
            {
                var v = GetString(input, key);
                if (v.Length == 0) continue;
                var firstLine = v.IndexOf('\n') is var nl && nl >= 0 ? v[..nl] : v;
                arg = firstLine.Length > 120 ? firstLine[..120] + "…" : firstLine;
                break;
            }
        }
        return arg.Length > 0 ? $"[tool_use: {name} — {arg}]" : $"[tool_use: {name}]";
    }

    private static long ToolResultChars(JsonElement part)
    {
        if (!part.TryGetProperty("content", out var content)) return 0;
        return content.ValueKind switch
        {
            JsonValueKind.String => (content.GetString() ?? "").Length,
            JsonValueKind.Array => content.GetRawText().Length,
            _ => content.GetRawText().Length
        };
    }

    private static int WordCount(string s)
    {
        var count = 0;
        var inWord = false;
        foreach (var c in s)
        {
            if (char.IsWhiteSpace(c)) inWord = false;
            else if (!inWord) { inWord = true; count++; }
        }
        return count;
    }

    private static string GetString(JsonElement element, string property) =>
        element.TryGetProperty(property, out var prop) && prop.ValueKind == JsonValueKind.String
            ? prop.GetString() ?? ""
            : "";
}

/// <summary>
/// Pure classification of one session file against its stored ingest stamp — the progressive
/// half of the ingest. "Absent" (a stored session whose file no longer exists) is the load-bearing
/// category: Claude Code deletes transcripts after its retention window, and a session that
/// aged off disk KEEPS its rows — nothing in this ingest ever deletes it.
/// </summary>
public static class IngestPlan
{
    public enum Change { New, Changed, Unchanged }

    public static Change Classify((long SourceBytes, string SourceMtimeUtc)? stored, long bytes, string mtimeUtc)
    {
        if (stored is null) return Change.New;
        return stored.Value.SourceBytes == bytes && stored.Value.SourceMtimeUtc == mtimeUtc
            ? Change.Unchanged
            : Change.Changed;
    }

    /// <summary>Stored session ids whose files were not found this run — retained, never deleted.</summary>
    public static List<string> AbsentRetained(IEnumerable<string> storedIds, IEnumerable<string> foundIds)
    {
        var found = new HashSet<string>(foundIds, StringComparer.OrdinalIgnoreCase);
        return storedIds.Where(id => !found.Contains(id)).OrderBy(id => id, StringComparer.Ordinal).ToList();
    }
}
