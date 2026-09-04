using System.Text.Json;
using System.Text.RegularExpressions;

namespace StoryPlanner.CodeSessions;

/// <summary>
/// The AUTHORED exclusion rule for the ingest (2026-09-03): a session whose first human
/// user message matches a configured pattern is never ingested, and its subagents go with
/// it. The archive is provenance for human-in-the-loop sessions; programmatic batch children
/// (a runner invoking <c>claude -p</c> per job) carry none. The rule exists because of the
/// 2026-08-27 incident — 9,245 transcripts from one runner's infinite retry of
/// <c>/analyze-story</c> — and because there is deliberately no delete path: a re-run of the
/// ingest would otherwise re-import anything cleaned by hand.
///
/// Prevention, not detection: patterns are authored in the config, never inferred.
/// </summary>
public static class IngestExclusion
{
    /// <summary>
    /// The trimmed text of the first <c>type == "user"</c> record whose <c>message.content</c>
    /// is a string. Array-content user records are tool results or images, not what the
    /// human typed, and are skipped. Streams and stops at the first hit; malformed lines are
    /// skipped with the extractor's tolerance. Null when no such record exists.
    /// </summary>
    public static string? FirstUserBody(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            JsonDocument doc;
            try { doc = JsonDocument.Parse(line); }
            catch (JsonException) { continue; }

            using (doc)
            {
                var root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Object) continue;
                if (!root.TryGetProperty("type", out var type) || type.ValueKind != JsonValueKind.String) continue;
                if (type.GetString() != "user") continue;
                if (!root.TryGetProperty("message", out var message) || message.ValueKind != JsonValueKind.Object) continue;
                if (!message.TryGetProperty("content", out var content)) continue;
                if (content.ValueKind != JsonValueKind.String) continue;

                var body = content.GetString()?.Trim();
                if (string.IsNullOrEmpty(body)) continue;
                return UnwrapSlashCommand(body);
            }
        }
        return null;
    }

    private static readonly Regex CommandName = new("<command-name>(?<name>[^<]*)</command-name>", RegexOptions.CultureInvariant);
    private static readonly Regex CommandArgs = new("<command-args>(?<args>[^<]*)</command-args>", RegexOptions.CultureInvariant);

    /// <summary>
    /// A slash-command turn is stored as markup — <c>&lt;command-message&gt;…&lt;/command-message&gt;
    /// &lt;command-name&gt;/x&lt;/command-name&gt; &lt;command-args&gt;y&lt;/command-args&gt;</c> — never as the
    /// typed text. Reduce it to <c>/x y</c> so a rule can be authored the way the command was typed.
    /// Anything else is returned unchanged.
    /// </summary>
    public static string UnwrapSlashCommand(string body)
    {
        var name = CommandName.Match(body);
        if (!name.Success) return body;
        var args = CommandArgs.Match(body);
        var command = name.Groups["name"].Value.Trim();
        var arguments = args.Success ? args.Groups["args"].Value.Trim() : "";
        return arguments.Length == 0 ? command : $"{command} {arguments}";
    }

    /// <summary>The pattern text of the first rule the body matches, or null.</summary>
    public static string? MatchingRule(string? firstUserBody, IReadOnlyList<Regex> rules)
    {
        if (firstUserBody is null) return null;
        foreach (var rule in rules)
            if (rule.IsMatch(firstUserBody)) return rule.ToString();
        return null;
    }

    public static IReadOnlyList<Regex> CompileRules(IEnumerable<string> patterns) =>
        patterns.Select(p => new Regex(p, RegexOptions.CultureInvariant)).ToList();
}
