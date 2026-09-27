using System.Text;
using StoryPlanner.BatchFiles;
using StoryPlanner.Core;
using StoryPlanner.VoiceAttribution;

namespace StoryPlanner.TurnItemizer;

/// <summary>
/// The lineage layers read beside the v1 archive: which archive notes took their text from which
/// lineage record, as the voice attribution computes it in the itemizer's own run (never read from
/// the sidecar attribution.csv, a derived artifact). Two cuts. <c>prompts-by-copy</c>: one item per
/// model turn, carrying only the user turn before it, so a reader describes the prompt blind; whether
/// the model turn was taken into the archive is the index row's description and never the item's.
/// <c>threads-with-notes</c>: one item per conversation, cut into parts at turn boundaries when it
/// runs long, each record followed by the archive notes whose text originated in it.
/// <see cref="Taken"/> projects an attribution row so both cuts are tested without a lineage database.
/// </summary>
public static class Copies
{
    /// <summary>One archive note whose attributed origin is a lineage record.</summary>
    public sealed record Taken(int NoteId, string Content, string Owner, string Label, string Role, bool PlanFirst, bool Echo, string OriginId);

    public const string PromptsLocatorNotation = Turns.LocatorNotation;

    public const string PromptsNarrowing =
        "the user turns that precede a model turn in the lineage layers, one item per model turn, a user turn made only of attached documents that were never captured left out; the item carries the user turn alone, and whether the model turn's text was taken into the v1 archive — a note pasted or lifted from it, as the attribution run finds it — is in the index description only";

    public const string ThreadsLocatorNotation =
        "`<first record> … <last record>`: the source ids of the first and last records the item carries, as the MCP lineage tools cite them — `gemini:<entry id> prompt|response`, `aistudio:<chat id>#t<turn>`, `nlm:<notebook id>#t<turn>`";

    public static string ThreadsNarrowing(int maxChars) =>
        $"every conversation of the lineage layers read, one item per conversation, a conversation longer than {maxChars:N0} characters cut into consecutive parts at turn boundaries; under each record, the v1 archive notes the attribution run traces to it, except flagged notes and the notes of any story the config excludes; a note sharing only a short phrase with a record is not listed";

    /// <summary>The archive notes whose attributed origin is a lineage record, by that record's id; flagged notes and phrase-only matches are left out.</summary>
    public static ILookup<string, Taken> ByOrigin(IEnumerable<Row> rows, string? excludeStory) =>
        rows.Where(r => r.Match.Origin is not null
                        && r.Note.State != 1
                        && r.Label is not (VoiceLabel.Phrase or VoiceLabel.None or VoiceLabel.Short)
                        && (excludeStory is null || !r.Story.Contains(excludeStory, StringComparison.OrdinalIgnoreCase)))
            .Select(r => new Taken(r.Note.Id, r.Note.Content, $"{r.OwnerTypeName} \"{r.OwnerName}\"", r.Label, r.Role, r.PlanFirst, r.EchoCandidate,
                RecordId(r.Match.Origin!.Id, r.Match.Origin.Role)))
            .ToLookup(t => t.OriginId, StringComparer.Ordinal);

    /// <summary>
    /// The dialogue record an attribution source is. AI Studio and NotebookLM sources are one turn
    /// each and carry the record's own id; a Gemini entry is one source per side under one id, told
    /// apart by the source's role — which PlanFirst and an echo leave as it was, flipping only the
    /// row's — so its prompt and its response are keyed apart here.
    /// </summary>
    public static string RecordId(string sourceId, string sourceRole) =>
        sourceId.StartsWith("gemini:", StringComparison.Ordinal) ? $"{sourceId} {(sourceRole == "model" ? "response" : "prompt")}" : sourceId;

    /// <summary>Whether a note took its text from the model's record: a model-role paste or lift, as against the plan having held the text first.</summary>
    public static bool IsCopy(Taken t) =>
        t.Role == "model" && t.Label is VoiceLabel.Verbatim or VoiceLabel.EditedPaste or VoiceLabel.FramedPaste or VoiceLabel.Fragment;

    /// <summary>What a taken note's relation to its record is, in words a reader can use.</summary>
    public static string Mark(Taken t) =>
        t.PlanFirst ? "the plan held this text before this reply"
        : t.Echo ? "the reply was quoting the plan"
        : t.Role == "brian" ? "the author's own words in this record"
        : t.Label switch
        {
            VoiceLabel.Verbatim => "pasted whole from this reply",
            VoiceLabel.EditedPaste => "pasted from this reply with cuts",
            VoiceLabel.FramedPaste => "pasted from this reply inside the author's own framing",
            VoiceLabel.Fragment => "one sentence lifted from this reply",
            _ => "shares text with this record",
        };

    /// <summary>One item per model turn with the user turn before it; the copy is described in the index row, never in the body.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> PromptsByCopy(IEnumerable<Turns.Conversation> conversations, ILookup<string, Taken> taken)
    {
        var items = new List<ItemizerOutput.Item>();
        foreach (var c in conversations)
        {
            var runs = Turns.Runs(c.Records);
            for (var i = 1; i < runs.Count; i++)
            {
                if (runs[i].Role != Turns.Model || runs[i - 1].Role != Turns.User || runs[i - 1].PlaceholdersOnly) continue;
                var prompt = runs[i - 1];
                var copies = runs[i].Records.SelectMany(r => taken[r.Id]).Where(IsCopy).ToList();
                var pastes = copies.Count(t => t.Label != VoiceLabel.Fragment);
                var lifts = copies.Count - pastes;
                var body = new StringBuilder()
                    .Append("# ").Append(c.Layer).Append(": ").Append(c.Label).Append('\n')
                    .Append("date: ").Append(c.Date).Append('\n')
                    .Append("user turn: ").Append(prompt.Ids).Append("\n\n")
                    .Append("## The user turn\n\n").Append(prompt.Text).Append('\n');
                var copied = copies.Count == 0 ? "not copied" : $"copied: {pastes} pasted, {lifts} lifted into archive notes";
                items.Add(new ItemizerOutput.Item(Turns.Slug(prompt.Records[0].Id), body.ToString(), $"{runs[i].Ids} → {prompt.Ids}",
                    $"{c.Layer}, {c.Label}, {c.Date}; {copied}"));
            }
        }
        return items;
    }

    /// <summary>One item per conversation, or per part of one past <paramref name="maxChars"/>, each record followed by the archive notes traced to it.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> ThreadsWithNotes(IEnumerable<Turns.Conversation> conversations, ILookup<string, Taken> taken, int maxChars)
    {
        var items = new List<ItemizerOutput.Item>();
        foreach (var c in conversations)
        {
            if (c.Records.Count == 0) continue;
            var rendered = c.Records.Select(r => (Record: r, Text: Render(r, taken[r.Id].ToList()))).ToList();
            var parts = Split(rendered, maxChars);
            var key = Key(c);
            for (var p = 0; p < parts.Count; p++)
            {
                var part = parts[p];
                var notes = part.Sum(x => taken[x.Record.Id].Count());
                var body = new StringBuilder()
                    .Append("# ").Append(c.Layer).Append(": ").Append(c.Label).Append('\n')
                    .Append("date: ").Append(c.Date).Append('\n');
                if (parts.Count > 1) body.Append("part: ").Append(p + 1).Append(" of ").Append(parts.Count).Append('\n');
                body.Append("archive notes traced to this stretch: ").Append(notes).Append("\n\n");
                foreach (var x in part) body.Append(x.Text);
                var id = parts.Count > 1 ? $"{key}-p{p + 1}" : key;
                items.Add(new ItemizerOutput.Item(id, body.ToString().TrimEnd('\n') + "\n",
                    $"{part[0].Record.Id} … {part[^1].Record.Id}",
                    $"{c.Layer}, {c.Label}, {c.Date}{(parts.Count > 1 ? $", part {p + 1} of {parts.Count}" : "")}; {notes} archive notes"));
            }
        }
        return items;
    }

    static string Render(Turns.Record r, IReadOnlyList<Taken> notes)
    {
        var sb = new StringBuilder()
            .Append("## ").Append(r.Id).Append(" — ").Append(r.Role == Turns.User ? "the author" : "the model").Append("\n\n")
            .Append(r.IsPlaceholder ? $"{r.Text.Trim()} (an attached document, never captured)" : r.Text.Replace("\r\n", "\n").Trim())
            .Append("\n\n");
        if (notes.Count > 0)
        {
            sb.Append("### Archive notes traced to this record\n\n");
            foreach (var t in notes.OrderBy(t => t.NoteId))
                sb.Append("#### note ").Append(t.NoteId).Append(", on ").Append(t.Owner).Append(" — ").Append(Mark(t)).Append("\n\n")
                  .Append(t.Content.Replace("\r\n", "\n").Trim()).Append("\n\n");
        }
        return sb.ToString();
    }

    /// <summary>Consecutive parts no longer than the limit, cut only before a user record so an exchange stays whole; one record longer than the limit is a part of its own.</summary>
    static List<List<(Turns.Record Record, string Text)>> Split(List<(Turns.Record Record, string Text)> rendered, int maxChars)
    {
        var parts = new List<List<(Turns.Record, string)>>();
        var current = new List<(Turns.Record, string)>();
        var size = 0;
        foreach (var x in rendered)
        {
            if (current.Count > 0 && x.Record.Role == Turns.User && size + x.Text.Length > maxChars)
            {
                parts.Add(current); current = []; size = 0;
            }
            current.Add(x); size += x.Text.Length;
        }
        if (current.Count > 0) parts.Add(current);
        return parts;
    }

    /// <summary>A conversation's item id: the layer and the thread, chat or notebook it is.</summary>
    static string Key(Turns.Conversation c)
    {
        var first = c.Records[0].Id;
        if (first.StartsWith("gemini:", StringComparison.Ordinal))
            return "gemini-" + Turns.Slug(c.Label.StartsWith("thread ", StringComparison.Ordinal) ? c.Label[7..] : c.Label);
        var hash = first.IndexOf('#');
        return Turns.Slug(hash > 0 ? first[..hash] : first);
    }
}
