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
    int LargePasteStubs,
    int HumanResults = 0,
    int PlanSnapshots = 0,
    int PlanDrift = 0)
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
/// One clause qualifies that line (2026-09-04): a tool result is KEPT when it carries a
/// human's words or a human's decision. Some results are not computation at all — they are
/// the author talking, routed through a tool envelope: an AskUserQuestion answer, a permission
/// denial's typed reason, a plan verdict. Those were the highest-citation-weight content the
/// archive was throwing away. Recognition is STRUCTURAL (the shape of the record), not a
/// tool-name switch, with one deliberate exception for ExitPlanMode noted on ToolUseStub.
///
/// Within a kept answer the distinction that matters is authorship, and the two markers carry
/// it: "Chose:" is a Claude-authored option label that the author SELECTED — a decision, but
/// not his prose — while "Typed:" is his own free text, stored verbatim and weighted exactly
/// like a freestanding prompt. Selection is EXACT equality against the offered labels; a real
/// answer may begin with a label and keep going ("Keep it prose only. The whole point is…"),
/// so a prefix match would silently reattribute the author's words to the machine.
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
        var ctx = new ExtractContext();

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

                ctx.PlanIdsInCurrentRecord.Clear(); // never carry ids across a dropped record
                var body = root.TryGetProperty("message", out var message) &&
                           message.ValueKind == JsonValueKind.Object &&
                           message.TryGetProperty("content", out var content)
                    ? MapContent(content, root, ctx)
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

                foreach (var planId in ctx.PlanIdsInCurrentRecord)
                    ctx.PlanRecordIndex[planId] = records.Count - 1;
                ctx.PlanIdsInCurrentRecord.Clear();
            }
        }

        // A plan is revised between the call and the approval often enough to matter (a wording
        // fix, or an entirely different plan later in a resumed session), and the approved text
        // is the one that was actually agreed to. Both are kept, on the record that PROPOSED
        // them — the user record holds only the verdict, so no Claude-authored prose lands on a
        // user-role row. Appended after the stream because the approval is only seen later.
        foreach (var (toolUseId, approved) in ctx.DriftedApprovals)
        {
            if (!ctx.PlanRecordIndex.TryGetValue(toolUseId, out var idx)) continue;
            var rec = records[idx];
            records[idx] = rec with
            {
                Body = $"{rec.Body}\n\n[Plan as approved — differs from the proposal above]\n\n{approved}"
            };
        }

        // Timestamp order, input order as the tiebreak (OrderBy is stable).
        var ordered = records.OrderBy(r => r.Timestamp, StringComparer.Ordinal).ToList();

        return new ExtractedSession(
            title, slug, ordered, duplicates, malformed, empty, largePastes,
            ctx.HumanResults, ctx.PlanSnapshots, ctx.PlanDrift);
    }

    /// <summary>
    /// Per-session mutable state. PlanByToolUseId lets the plan the assistant PROPOSED
    /// (input.plan, present on all calls) be compared against the plan the author APPROVED
    /// (toolUseResult.plan, present only on approvals) — a mismatch is counted and disclosed
    /// rather than silently resolved in favour of either one.
    /// </summary>
    private sealed class ExtractContext
    {
        public readonly Dictionary<string, string> PlanByToolUseId = new(StringComparer.Ordinal);
        /// <summary>Where each proposing record landed, so a later approval can be appended to it.</summary>
        public readonly Dictionary<string, int> PlanRecordIndex = new(StringComparer.Ordinal);
        /// <summary>Plan tool_use ids seen while mapping the record currently being built.</summary>
        public readonly List<string> PlanIdsInCurrentRecord = [];
        /// <summary>toolUseId → the approved text, when it differs from what was proposed.</summary>
        public readonly List<(string ToolUseId, string Approved)> DriftedApprovals = [];
        public int HumanResults;
        public int PlanSnapshots;
        public int PlanDrift;
    }

    /// <summary>message.content is either a plain string (user turns) or an array of typed parts.</summary>
    private static string MapContent(JsonElement content, JsonElement root, ExtractContext ctx)
    {
        if (content.ValueKind == JsonValueKind.String)
            return (content.GetString() ?? "").Trim();
        if (content.ValueKind != JsonValueKind.Array)
            return "";

        var parts = new List<string>();

        // The answer/verdict lives in a TOP-LEVEL "toolUseResult" sidecar, not in the
        // tool_result part itself (whose prose wrapper varies by CLI version and is not worth
        // parsing). The sidecar describes one result, so it is consumed by the first
        // tool_result part only — a record carrying several falls back to elision for the rest.
        var sidecar = root.ValueKind == JsonValueKind.Object &&
                      root.TryGetProperty("toolUseResult", out var tur)
            ? tur
            : (JsonElement?)null;
        var sidecarUsed = false;

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
                    parts.Add(ToolUseStub(part, ctx));
                    break;

                case "tool_result":
                    // The author's words or decision, if this result carries either; the
                    // ordinary elision otherwise, byte-identical to what it always was.
                    var human = HumanResult(part, sidecarUsed ? null : sidecar, ctx);
                    if (human is not null)
                    {
                        sidecarUsed = true;
                        ctx.HumanResults++;
                        parts.Add(human);
                    }
                    else
                    {
                        parts.Add($"[tool result elided — {ToolResultChars(part):N0} chars]");
                    }
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

    private const string DenialPrefix = "The user doesn't want to proceed with this tool use";
    private const string DenialReasonMarker = "The user provided the following reason for the rejection:";

    /// <summary>
    /// The three shapes in which a tool result carries a human's words or decision, or null to
    /// let the caller fall back to the ordinary elision. Every guard here is a real shape found
    /// in the transcripts (a bare-string sidecar, an unparseable question set, an answer whose
    /// question is missing) — none of them may throw, because one malformed record must never
    /// cost a whole session.
    /// </summary>
    private static string? HumanResult(JsonElement part, JsonElement? sidecar, ExtractContext ctx)
    {
        // 1. A permission denial, and the reason the author typed into it. Read from the part's
        //    own content: the denial text is the result, and it is stable across CLI versions.
        if (part.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.String)
        {
            var text = (content.GetString() ?? "").Trim();
            if (text.StartsWith(DenialPrefix, StringComparison.Ordinal))
            {
                var at = text.IndexOf(DenialReasonMarker, StringComparison.Ordinal);
                if (at < 0) return "[Rejected by user]";
                var reason = text[(at + DenialReasonMarker.Length)..].Trim();
                return reason.Length > 0 ? $"[Rejected by user]\nTyped: {reason}" : "[Rejected by user]";
            }
        }

        if (sidecar is not { ValueKind: JsonValueKind.Object } s) return null;

        // 2. An AskUserQuestion answer set.
        if (s.TryGetProperty("questions", out var questions) &&
            s.TryGetProperty("answers", out var answers) &&
            questions.ValueKind == JsonValueKind.Array &&
            answers.ValueKind == JsonValueKind.Object)
        {
            // No question resolved to an answer (an unparseable question set, or a text
            // mismatch): fall through to the ordinary elision rather than emit an empty block.
            var formatted = FormatAskUserQuestion(questions, answers);
            return formatted.Length > 0 ? formatted : null;
        }

        // 3. A plan verdict. The plan TEXT is stored on the assistant record that proposed it
        //    (see ToolUseStub) — putting Claude-authored prose on this user-role record would
        //    pollute the very query this whole feature exists to serve.
        if (s.TryGetProperty("plan", out var approvedPlan) && approvedPlan.ValueKind == JsonValueKind.String)
        {
            var id = GetString(part, "tool_use_id");
            // Compare like with like: the proposal was stored trimmed, so trailing whitespace
            // alone must not read as a revision.
            var approved = (approvedPlan.GetString() ?? "").Trim();
            if (id.Length > 0 && ctx.PlanByToolUseId.TryGetValue(id, out var proposed) &&
                !string.Equals(proposed, approved, StringComparison.Ordinal))
            {
                ctx.PlanDrift++;
                if (approved.Length > 0) ctx.DriftedApprovals.Add((id, approved));
            }
            return "[Plan approved by user]";
        }

        return null;
    }

    /// <summary>
    /// Renders one AskUserQuestion exchange. "answers" is an OBJECT keyed by the full question
    /// text (not an array, and there is no id to join on), so questions are walked in their
    /// asked order and looked up by text. An answer equal to an offered label is a selection;
    /// anything else is the author's own typing.
    /// </summary>
    private static string FormatAskUserQuestion(JsonElement questions, JsonElement answers)
    {
        var blocks = new List<string>();
        foreach (var q in questions.EnumerateArray())
        {
            if (q.ValueKind != JsonValueKind.Object) continue;
            var text = GetString(q, "question");
            if (text.Length == 0) continue;
            if (!answers.TryGetProperty(text, out var a) || a.ValueKind != JsonValueKind.String) continue;

            var answer = (a.GetString() ?? "").Trim();
            if (answer.Length == 0) continue;

            var labels = new List<string>();
            if (q.TryGetProperty("options", out var options) && options.ValueKind == JsonValueKind.Array)
                foreach (var o in options.EnumerateArray())
                {
                    if (o.ValueKind != JsonValueKind.Object) continue;
                    var label = GetString(o, "label");
                    if (label.Length > 0) labels.Add(label);
                }

            blocks.Add($"Q: {text}\n{(IsSelection(answer, labels) ? "Chose" : "Typed")}: {answer}");
        }

        if (blocks.Count == 0) return "";
        return $"[AskUserQuestion — {blocks.Count} answered]\n\n" + string.Join("\n\n", blocks);
    }

    /// <summary>
    /// True only when the answer IS one of the offered labels, or (multi-select) a comma-joined
    /// set of them. Deliberately exact: free text that happens to open with a label is the
    /// author's prose and must never be recorded as a machine-authored choice.
    /// </summary>
    private static bool IsSelection(string answer, List<string> labels)
    {
        if (labels.Count == 0) return false;
        if (labels.Contains(answer, StringComparer.Ordinal)) return true;

        var pieces = answer.Split(", ", StringSplitOptions.TrimEntries);
        return pieces.Length > 1 && pieces.All(p => labels.Contains(p, StringComparer.Ordinal));
    }

    /// <summary>
    /// "[tool_use: Edit — WorldDateModel.cs]" — the tool name plus the first recognizable main
    /// argument. Mechanical field extraction, not a summary.
    ///
    /// ExitPlanMode is the ONE tool-name special case in this extractor, and an exception rather
    /// than a precedent: the plan is stored in full. It is Claude's prose, so it belongs on this
    /// assistant record; it is taken from input.plan rather than the approval sidecar because
    /// that is the only place a REJECTED plan survives — and a rejected plan is an abandoned
    /// proposal, exactly the "what was tried and cut" this archive exists to hold.
    /// </summary>
    private static string ToolUseStub(JsonElement part, ExtractContext ctx)
    {
        var name = GetString(part, "name");

        if (name == "ExitPlanMode" &&
            part.TryGetProperty("input", out var planInput) &&
            planInput.ValueKind == JsonValueKind.Object)
        {
            var plan = GetString(planInput, "plan").Trim();
            if (plan.Length > 0)
            {
                var id = GetString(part, "id");
                if (id.Length > 0)
                {
                    ctx.PlanByToolUseId[id] = plan;
                    ctx.PlanIdsInCurrentRecord.Add(id);
                }
                ctx.PlanSnapshots++;
                return $"[tool_use: ExitPlanMode]\n\n{plan}";
            }
        }

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
