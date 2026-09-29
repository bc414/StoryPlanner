using System.Text.RegularExpressions;
using StoryPlanner.CodeSessions;

namespace StoryPlanner.SessionTree;

public enum ItemKind
{
    /// <summary>Brian's own words: a typed prompt, or a slash command he invoked.</summary>
    Prompt,
    /// <summary>His action in fixed text: a question answered, a plan approved or rejected, an interruption.</summary>
    Action,
    /// <summary>The assistant's text, tool calls removed.</summary>
    Reply,
    /// <summary>A subagent's report arriving in the parent session.</summary>
    Handback,
    /// <summary>Where a session lost its context to a compaction; the summary itself was never stored.</summary>
    Compaction
}

/// <param name="Shape">For a hand-back, which envelope carried the report: "agent-message" or "task-notification".</param>
public sealed record Item(ItemKind Kind, string Text, string? AgentId = null, string? Shape = null);

/// <summary>
/// Maps one archive record to what a reader of the dialogue needs, or to nothing. Communication
/// is kept word for word; what was DONE (tool-call stubs, elided results) and what the harness
/// put in front of the model are dropped, except the two harness records that carry dialogue: a
/// subagent's report, and the marker where a compaction cut the context. Every rule is textual
/// and mechanical — this reads the extractor's own stubs, it never judges content.
/// </summary>
public static class TurnFilter
{
    private static readonly Regex ElidedResult = new(@"^\[tool result elided — [\d,]+ chars\]$", RegexOptions.Compiled);
    private static readonly Regex ExtraBlankLines = new(@"\n{3,}", RegexOptions.Compiled);

    private const string ToolUseOpening = "[tool_use: ";
    private const string PlanStub = "[tool_use: ExitPlanMode]";
    public const string PlanMarker = "[Plan proposed — ExitPlanMode]";

    private const string IdeOpenedFile = "<ide_opened_file>";
    private const string IdeOpenedFileClose = "</ide_opened_file>";

    private const string AgentMessageOpening = "Another Claude session sent a message:";
    private const string AgentMessageTag = "<agent-message from=\"";
    private const string AgentMessageClose = "</agent-message>";
    private const string HandbackPreamble = "[Subagent hand-back]";
    private const string TaskNotification = "<task-notification>";
    private const string CompactionOpening = "[compaction summary dropped";

    /// <summary>A task notification that only points at the report delivered elsewhere — not a report.</summary>
    private const string ResultPointer = "This agent's report was delivered to you as a message from";

    /// <summary>The author's actions arriving as fixed text (the extractor keeps them in the user role).</summary>
    private static readonly string[] ActionOpenings =
    [
        "[AskUserQuestion",
        "[Plan approved by user]",
        "[Rejected by user]",
        "[Request interrupted by user",
    ];

    public static Item? Map(SessionRecord record)
    {
        var body = record.Body.Replace("\r\n", "\n");

        // A hand-back is recognised by shape in either role: sessions still at extract version 2
        // carry what the harness injected in the user role.
        if (record.Role is "user" or CodeSessionExtractor.HarnessRole)
        {
            if (body.StartsWith(AgentMessageOpening, StringComparison.Ordinal)) return AgentMessage(body);
            if (body.StartsWith(TaskNotification, StringComparison.Ordinal)) return TaskResult(body);
        }

        return record.Role switch
        {
            "user" => User(body),
            "assistant" => Assistant(body),
            CodeSessionExtractor.HarnessRole when body.StartsWith(CompactionOpening, StringComparison.Ordinal)
                => new Item(ItemKind.Compaction, body.Trim()),
            _ => null
        };
    }

    /// <summary>True for a record holding Brian's own typed prompt — the unit a rewind re-sends.</summary>
    public static bool IsPrompt(SessionRecord record) => record.Role == "user" && Map(record)?.Kind == ItemKind.Prompt;

    private static Item? User(string body)
    {
        var text = Tidy(string.Join('\n', body.Split('\n').Where(l => !ElidedResult.IsMatch(l.Trim()))));

        // The IDE prefixes an opened-file notice to the prompt; a selection is his own
        // highlighted text and stays.
        while (text.StartsWith(IdeOpenedFile, StringComparison.Ordinal))
        {
            var close = text.IndexOf(IdeOpenedFileClose, StringComparison.Ordinal);
            if (close < 0) break;
            text = text[(close + IdeOpenedFileClose.Length)..].TrimStart();
        }

        if (text.Length == 0) return null;
        var kind = ActionOpenings.Any(o => text.StartsWith(o, StringComparison.Ordinal)) ? ItemKind.Action : ItemKind.Prompt;
        return new Item(kind, text);
    }

    private static Item? Assistant(string body)
    {
        var kept = new List<string>();
        foreach (var line in body.Split('\n'))
        {
            var t = line.TrimEnd();
            // The extractor stores a plan in full after its stub; only the stub line becomes a label.
            if (t == PlanStub) kept.Add(PlanMarker);
            else if (t.StartsWith(ToolUseOpening, StringComparison.Ordinal) && t.EndsWith(']')) continue;
            else kept.Add(line);
        }
        var text = Tidy(string.Join('\n', kept));
        return text.Length == 0 || text == PlanMarker ? null : new Item(ItemKind.Reply, text);
    }

    /// <summary>
    /// "Another Claude session sent a message: &lt;agent-message from="ID"&gt; [Subagent hand-back] …
    /// The report follows:" then the report, every line indented two spaces by the harness, then
    /// the closing tag and the harness's own notice. The report is what is kept, unindented.
    /// </summary>
    private static Item? AgentMessage(string body)
    {
        var tag = body.IndexOf(AgentMessageTag, StringComparison.Ordinal);
        if (tag < 0) return null;
        var idStart = tag + AgentMessageTag.Length;
        var idEnd = body.IndexOf('"', idStart);
        if (idEnd < 0) return null;
        var agentId = body[idStart..idEnd];

        var contentStart = body.IndexOf('>', idEnd) + 1;
        var close = body.LastIndexOf(AgentMessageClose, StringComparison.Ordinal);
        if (close < contentStart) close = body.Length;

        var lines = body[contentStart..close].Trim('\n').Split('\n').ToList();
        if (lines.Count > 0 && lines[0].StartsWith(HandbackPreamble, StringComparison.Ordinal)) lines.RemoveAt(0);
        if (lines.Where(l => l.Trim().Length > 0).All(l => l.StartsWith("  ", StringComparison.Ordinal)))
            lines = lines.Select(l => l.Length >= 2 ? l[2..] : l.TrimStart()).ToList();

        var report = Tidy(string.Join('\n', lines));
        return report.Length == 0 ? null : new Item(ItemKind.Handback, report, agentId, "agent-message");
    }

    /// <summary>
    /// A task notification carries the report in &lt;result&gt; in older sessions; newer ones only
    /// point at the agent-message that carried it, and one without a result is a status line.
    /// </summary>
    private static Item? TaskResult(string body)
    {
        var agentId = Between(body, "<task-id>", "</task-id>");
        var open = body.IndexOf("<result>", StringComparison.Ordinal);
        var close = body.LastIndexOf("</result>", StringComparison.Ordinal);
        if (agentId is null || open < 0 || close < open) return null;

        var result = Tidy(body[(open + "<result>".Length)..close]);
        if (result.Length == 0 || result.StartsWith(ResultPointer, StringComparison.Ordinal)) return null;
        return new Item(ItemKind.Handback, result, agentId.Trim(), "task-notification");
    }

    private static string? Between(string s, string open, string close)
    {
        var a = s.IndexOf(open, StringComparison.Ordinal);
        if (a < 0) return null;
        a += open.Length;
        var b = s.IndexOf(close, a, StringComparison.Ordinal);
        return b < 0 ? null : s[a..b];
    }

    private static string Tidy(string s) => ExtraBlankLines.Replace(s, "\n\n").Trim();
}
