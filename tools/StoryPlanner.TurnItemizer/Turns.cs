using System.Text;
using System.Text.RegularExpressions;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.TurnItemizer;

/// <summary>
/// The exchange cut over dialogue from any layer, kept out of the CLI so it is tested directly.
/// A conversation is its records in order; a turn is a run of consecutive records of one role, so
/// an AI Studio upload followed by a typed prompt is one user turn, as it was one message. Ids are
/// the source ids the MCP lineage and conversation tools cite, which the locator carries.
/// </summary>
public static class Turns
{
    public const string User = "user", Model = "model";

    /// <summary>One record of a conversation. A placeholder stands for an attached document that was never captured.</summary>
    public sealed record Record(string Id, string Role, string Text, bool IsPlaceholder = false);
    public sealed record Conversation(string Layer, string Label, string Date, IReadOnlyList<Record> Records);
    public sealed record Run(string Role, IReadOnlyList<Record> Records)
    {
        public string Ids => string.Join(" + ", Records.Select(r => r.Id));
        public bool PlaceholdersOnly => Records.All(r => r.IsPlaceholder);
        public string Text => string.Join("\n\n", Records.Select(r =>
            r.IsPlaceholder ? $"{r.Text.Trim()} (an attached document, never captured)" : r.Text.Replace("\r\n", "\n").Trim()));
    }

    public const string LocatorNotation =
        "`<model turn> → <user turn>`, each turn as the source ids of its records, joined by + when it spans several: " +
        "`gemini:<entry id> prompt|response` and `aistudio:<chat id>#t<turn>` and `nlm:<notebook id>#t<turn>` in lineage.db, " +
        "`block:<block id>` in the conversations tables of the working-plan .storyplan; a Gemini turn's neighbour is the entry " +
        "before or after it in its thread as the Gemini corpus index groups entries; `(none)` when no user turn follows";

    /// <summary>Consecutive records of one role, grouped.</summary>
    public static IReadOnlyList<Run> Runs(IReadOnlyList<Record> records)
    {
        var runs = new List<Run>();
        var current = new List<Record>();
        foreach (var r in records)
        {
            if (current.Count > 0 && current[0].Role != r.Role) { runs.Add(new Run(current[0].Role, current)); current = []; }
            current.Add(r);
        }
        if (current.Count > 0) runs.Add(new Run(current[0].Role, current));
        return runs;
    }

    static readonly Regex BlankLine = new(@"\n\s*\n", RegexOptions.Compiled);

    /// <summary>Whether the turn's last paragraph — its last block of text after a blank line — holds a question mark.</summary>
    public static bool EndsWithQuestion(string text)
    {
        var paragraphs = BlankLine.Split(text.Replace("\r\n", "\n").Trim());
        var last = paragraphs.LastOrDefault(p => p.Trim().Length > 0);
        return last is not null && last.Contains('?');
    }

    /// <summary>One item per user turn that follows a model turn, the model turn beside it; a user turn of attached documents only is not a turn Brian typed and is left out.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> UserTurns(IEnumerable<Conversation> conversations)
    {
        var items = new List<ItemizerOutput.Item>();
        foreach (var c in conversations)
        {
            var runs = Runs(c.Records);
            for (var i = 1; i < runs.Count; i++)
            {
                if (runs[i].Role != User || runs[i - 1].Role != Model || runs[i].PlaceholdersOnly) continue;
                var body = Head(c, runs[i - 1], runs[i])
                    .Append("## The model turn before it\n\n").Append(runs[i - 1].Text).Append("\n\n")
                    .Append("## The user turn\n\n").Append(runs[i].Text).Append('\n');
                items.Add(new ItemizerOutput.Item(Slug(runs[i].Records[0].Id), body.ToString(), $"{runs[i - 1].Ids} → {runs[i].Ids}",
                    $"{c.Layer}, {c.Label}, {c.Date}"));
            }
        }
        return items;
    }

    /// <summary>One item per model turn whose last paragraph holds a question, the user turn after it beside it, or none when the conversation ends there.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> QuestionEndings(IEnumerable<Conversation> conversations)
    {
        var items = new List<ItemizerOutput.Item>();
        foreach (var c in conversations)
        {
            var runs = Runs(c.Records);
            for (var i = 0; i < runs.Count; i++)
            {
                if (runs[i].Role != Model || !EndsWithQuestion(runs[i].Text)) continue;
                var next = i + 1 < runs.Count && runs[i + 1].Role == User ? runs[i + 1] : null;
                var body = Head(c, runs[i], next)
                    .Append("## The model turn\n\n").Append(runs[i].Text).Append("\n\n")
                    .Append("## The next user turn\n\n").Append(next?.Text ?? "(none: the conversation ends here)").Append('\n');
                items.Add(new ItemizerOutput.Item(Slug(runs[i].Records[^1].Id), body.ToString(), $"{runs[i].Ids} → {next?.Ids ?? "(none)"}",
                    $"{c.Layer}, {c.Label}, {c.Date}"));
            }
        }
        return items;
    }

    public const string UserTurnsNarrowing =
        "the user turns that follow a model turn, a turn being a run of consecutive records of one role; a user turn made only of attached documents that were never captured is left out";
    public const string QuestionEndingsNarrowing =
        "the model turns whose last paragraph, the last block of text after a blank line, holds a question mark";

    static StringBuilder Head(Conversation c, Run model, Run? user) => new StringBuilder()
        .Append("# ").Append(c.Layer).Append(": ").Append(c.Label).Append('\n')
        .Append("date: ").Append(c.Date).Append('\n')
        .Append("model turn: ").Append(model.Ids).Append('\n')
        .Append("user turn: ").Append(user?.Ids ?? "(none)").Append("\n\n");

    static readonly Regex NonSlug = new("[^a-z0-9]+", RegexOptions.Compiled);

    /// <summary>A source id as an item id: lowercase, every other run of characters a single hyphen.</summary>
    public static string Slug(string id) => NonSlug.Replace(id.ToLowerInvariant(), "-").Trim('-');
}
