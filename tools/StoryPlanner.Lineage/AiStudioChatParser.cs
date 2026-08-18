using System.Globalization;
using System.Text.Json;

namespace StoryPlanner.Lineage;

public sealed record AiStudioTurn(
    int TurnIndex,
    string Role,
    string? CreateTime,
    bool IsPlaceholder,
    string Body);

public sealed record AiStudioChat(
    string ChatKey,
    string Title,
    string Date,
    string? Model,
    string SystemInstruction,
    IReadOnlyList<AiStudioTurn> Turns,
    int TurnsMissingCreateTime);

/// <summary>
/// Parses one Google AI Studio Drive-export chat file — a single JSON object with runSettings,
/// systemInstruction, and chunkedPrompt.chunks[]. Adapted from the retired
/// ConversationSplitter.GeminiParser (deleted 2026-08-02, recoverable via
/// `git show 6d34f88^:tools/ConversationSplitter/GeminiParser.cs`), with two deliberate changes:
/// roles are stored VERBATIM ("user"/"model" — schema truth, no assistant mapping), and a
/// dropped or unparseable file is reported by name rather than silently skipped.
///
/// Rules carried over unchanged: isThought chunks are stripped (model reasoning, not the
/// exchange); a driveDocument chunk becomes a placeholder turn ("[Attached document: {id}]" —
/// the content was never in the export, not withheld); title = filename with underscores as
/// spaces, TitleCased; conversation date = the first chunk's createTime; chats with two or
/// fewer surviving turns are dropped (a title and a lone prompt are not a conversation).
/// </summary>
public static class AiStudioChatParser
{
    public sealed record ParseFailure(string FileName, string Reason);

    /// <summary>
    /// Returns the parsed chat, or null with <paramref name="failure"/> set. A ≤2-turn drop is
    /// a failure with reason "dropped: N surviving turn(s)" so the dry run can name it.
    /// </summary>
    public static AiStudioChat? Parse(string json, string fileName, out ParseFailure? failure)
    {
        failure = null;

        JsonDocument doc;
        try
        {
            doc = JsonDocument.Parse(json, new JsonDocumentOptions { AllowTrailingCommas = true });
        }
        catch (JsonException ex)
        {
            failure = new ParseFailure(fileName, $"not JSON: {ex.Message}");
            return null;
        }

        using (doc)
        {
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                failure = new ParseFailure(fileName, "root is not a JSON object");
                return null;
            }

            if (!root.TryGetProperty("chunkedPrompt", out var chunkedPrompt) ||
                !chunkedPrompt.TryGetProperty("chunks", out var chunks) ||
                chunks.ValueKind != JsonValueKind.Array)
            {
                failure = new ParseFailure(fileName, "no chunkedPrompt.chunks — not an AI Studio chat export");
                return null;
            }

            var systemInstruction = string.Empty;
            if (root.TryGetProperty("systemInstruction", out var sysInstr) &&
                sysInstr.ValueKind == JsonValueKind.Object &&
                sysInstr.TryGetProperty("text", out var sysText))
                systemInstruction = sysText.GetString() ?? string.Empty;

            string? model = null;
            if (root.TryGetProperty("runSettings", out var runSettings) &&
                runSettings.ValueKind == JsonValueKind.Object &&
                runSettings.TryGetProperty("model", out var modelProp))
                model = modelProp.GetString();

            var turns = new List<AiStudioTurn>();
            var date = string.Empty;
            var missingCreateTime = 0;

            foreach (var chunk in chunks.EnumerateArray())
            {
                if (chunk.TryGetProperty("isThought", out var isThought) &&
                    isThought.ValueKind == JsonValueKind.True)
                    continue;

                var role = GetStringOrEmpty(chunk, "role");
                var createTime = GetStringOrEmpty(chunk, "createTime");
                if (date.Length == 0 && createTime.Length > 0)
                    date = createTime;

                if (chunk.TryGetProperty("driveDocument", out var driveDoc))
                {
                    var docId = driveDoc.ValueKind == JsonValueKind.Object &&
                                driveDoc.TryGetProperty("id", out var idProp)
                        ? idProp.GetString() ?? ""
                        : "";
                    if (createTime.Length == 0) missingCreateTime++;
                    turns.Add(new AiStudioTurn(
                        turns.Count + 1, role,
                        createTime.Length > 0 ? createTime : null,
                        IsPlaceholder: true,
                        $"[Attached document: {docId}]"));
                    continue;
                }

                var text = GetStringOrEmpty(chunk, "text");
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                if (createTime.Length == 0) missingCreateTime++;
                turns.Add(new AiStudioTurn(
                    turns.Count + 1, role,
                    createTime.Length > 0 ? createTime : null,
                    IsPlaceholder: false,
                    text.Trim()));
            }

            if (turns.Count <= 2)
            {
                failure = new ParseFailure(fileName, $"dropped: {turns.Count} surviving turn(s)");
                return null;
            }

            return new AiStudioChat(
                ChatKey: fileName,
                Title: TitleFromFileName(fileName),
                Date: date,
                Model: model,
                SystemInstruction: systemInstruction,
                Turns: turns,
                TurnsMissingCreateTime: missingCreateTime);
        }
    }

    /// <summary>Filename → display title, exactly as the retired splitter did it.</summary>
    public static string TitleFromFileName(string fileName)
    {
        var raw = fileName.Replace('_', ' ').TrimEnd(' ', '_', '.');
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(raw.ToLowerInvariant());
    }

    private static string GetStringOrEmpty(JsonElement element, string property) =>
        element.TryGetProperty(property, out var prop) && prop.ValueKind == JsonValueKind.String
            ? prop.GetString() ?? string.Empty
            : string.Empty;
}
