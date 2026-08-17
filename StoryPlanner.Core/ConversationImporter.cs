using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

/// <summary>
/// One conversation's raw block text, independent of where it came from. Both the file-based
/// import (a legacy *_content.json) and the direct import (straight from a scanned
/// conversations.json export) project onto this shape, so the merge logic below has a single
/// implementation.
/// </summary>
public sealed record ConversationImportSource(
    string Platform,
    string Title,
    string ConversationDate,
    string SourceUuid,
    string SourceUpdatedAt,
    IReadOnlyList<ConversationImportBlock> Blocks);

public sealed record ConversationImportBlock(
    int BlockNumber,
    string Speaker,
    string RawContent,
    bool IsCompaction);

/// <summary>
/// A legacy *_meta.json, still parsed and now entirely INERT (2026-08-11).
///
/// Block summaries used to be machine-written navigation aids produced by a Cowork round trip.
/// They are now Brian's own hand-written notes, authored in the Conversation Reader — so nothing
/// may write them but him, and the round trip that produced them is retired. This type survives
/// so a folder still holding meta files imports cleanly rather than throwing; the values it
/// carries are read and dropped, exactly like a meta file's subjectsCovered array since
/// 2026-07-31. Modelling the fields and visibly discarding them is what makes the inertness
/// testable, which an unmapped property would not be.
/// </summary>
public sealed class ConversationMeta
{
    public string                       ArcSummary { get; set; } = string.Empty;
    public List<ConversationMetaBlock>  Blocks     { get; set; } = new();
}

public sealed class ConversationMetaBlock
{
    public int    BlockNumber { get; set; }
    public string Summary     { get; set; } = string.Empty;
}

/// <summary>What a batch import actually did.</summary>
public sealed record ConversationImportResult(int Created, int Updated)
{
    public int Total => Created + Updated;

    public static ConversationImportResult operator +(ConversationImportResult a, ConversationImportResult b) =>
        new(a.Created + b.Created, a.Updated + b.Updated);

    public static readonly ConversationImportResult Empty = new(0, 0);
}

/// <summary>
/// Persists imported conversations. Raw block text is the only thing this class writes.
///
/// Additive/incremental: a source whose sourceUuid (or, absent that, NNN_{slug} prefix) matches an
/// existing Conversation is treated as a re-import of a reopened conversation rather than a
/// duplicate. Blocks are upserted by BlockNumber — already-reviewed blocks keep their BlockState
/// (Done/Flagged/Skipped) while newly-added turns land as Unread.
///
/// **This class never writes a summary (2026-08-11).** Block summaries are Brian's own navigation
/// notes now, typed in the reader; an import that could overwrite them would destroy authored work
/// with no undo, so re-import refreshes speaker/content/compaction and touches nothing else.
/// ArcSummary is likewise never written: existing arc summaries are frozen historical text from
/// the retired Cowork pass, and no path replaces them.
///
/// Two other things this class deliberately ignores, for the same reason one layer back: a meta
/// file's summaries (see <see cref="ConversationMeta"/>) and its subjectsCovered array — the
/// AI-suggested subject×track routing cut on 2026-07-31. Both import cleanly and write nothing.
/// </summary>
public class ConversationImporter
{
    private readonly AppDbContext _context;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ConversationImporter(AppDbContext context)
    {
        _context = context;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Import one conversation. Creates a new Conversation, or — when the source's
    /// sourceUuid/prefix matches one already in the DB — updates it additively.
    /// </summary>
    public async Task<ConversationImportResult> ImportAsync(
        ConversationImportSource source, string sourceFilePrefix)
    {
        var existing = await FindExistingAsync(sourceFilePrefix, source.SourceUuid);
        if (existing is null)
        {
            await CreateAsync(sourceFilePrefix, source);
            return new ConversationImportResult(Created: 1, Updated: 0);
        }

        await UpdateAsync(existing, source);
        return new ConversationImportResult(Created: 0, Updated: 1);
    }

    /// <summary>
    /// Import one legacy content file. <paramref name="metaPath"/> is parsed when supplied purely
    /// to prove it is well-formed — nothing it contains is written (see <see cref="ConversationMeta"/>).
    /// </summary>
    public async Task<ConversationImportResult> ImportFileAsync(string contentPath, string? metaPath)
    {
        string prefix  = PrefixOf(Path.GetFileName(contentPath), "_content.json");
        var    content = DeserializeContent(contentPath);
        if (metaPath is not null) DeserializeMeta(metaPath);

        return await ImportAsync(ToSource(content), prefix);
    }

    /// <summary>
    /// Import every *_content.json in a folder — the legacy route, for folders written by the
    /// retired export before it was cut. A *_meta.json alongside one is inert.
    /// </summary>
    public async Task<ConversationImportResult> ImportFolderAsync(string folderPath)
    {
        var contentFiles = Directory.GetFiles(folderPath, "*_content.json")
            .ToDictionary(f => PrefixOf(Path.GetFileName(f), "_content.json"), f => f);

        var metaFiles = Directory.GetFiles(folderPath, "*_meta.json")
            .ToDictionary(f => PrefixOf(Path.GetFileName(f), "_meta.json"), f => f);

        var result = ConversationImportResult.Empty;
        foreach (var (prefix, contentPath) in contentFiles)
        {
            metaFiles.TryGetValue(prefix, out var metaPath);
            result += await ImportFileAsync(contentPath, metaPath);
        }
        return result;
    }

    /// <summary>
    /// Import scan rows straight from a parsed Claude export — the one live route. New
    /// conversations are assigned the same NNN_{slug} prefix the retired file export would have
    /// given them, so a re-import of a reopened conversation still matches its existing record.
    /// </summary>
    public async Task<ConversationImportResult> ImportScannedAsync(IReadOnlyList<ConversationSyncItem> items)
    {
        // Snapshot the next free index once: several New rows in one batch must not all claim it.
        int nextIndex = ConversationPrefix.NextIndex(await _context.Conversations.ToListAsync());

        var result = ConversationImportResult.Empty;
        foreach (var item in items)
        {
            string prefix = string.IsNullOrEmpty(item.ExistingSourceFilePrefix)
                ? ConversationPrefix.Build(nextIndex++, item.Export.Title)
                : item.ExistingSourceFilePrefix;

            result += await ImportAsync(ToSource(item.Export), prefix);
        }
        return result;
    }

    // ── Projection ────────────────────────────────────────────────────────────

    private static ConversationImportSource ToSource(ContentFile content) =>
        new(content.Platform,
            content.Title,
            content.ConversationDate,
            content.SourceUuid,
            content.SourceUpdatedAt,
            content.Blocks
                .Select(b => new ConversationImportBlock(b.BlockNumber, b.Speaker, b.RawContent, b.IsCompaction))
                .ToList());

    private static ConversationImportSource ToSource(ParsedClaudeConversation export) =>
        new("Claude",
            export.Title,
            export.ConversationDate,
            export.Uuid,
            export.UpdatedAt,
            export.Blocks
                .Select(b => new ConversationImportBlock(b.BlockNumber, b.Speaker, b.RawContent, b.IsCompaction))
                .ToList());

    // ── Deserialization ───────────────────────────────────────────────────────

    private static ContentFile DeserializeContent(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<ContentFile>(stream, JsonOpts)
               ?? throw new InvalidDataException($"Failed to deserialize content file: {path}");
    }

    private static ConversationMeta DeserializeMeta(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<ConversationMeta>(stream, JsonOpts)
               ?? throw new InvalidDataException($"Failed to deserialize meta file: {path}");
    }

    // ── Matching ──────────────────────────────────────────────────────────────

    private async Task<Conversation?> FindExistingAsync(string prefix, string sourceUuid)
    {
        if (!string.IsNullOrEmpty(sourceUuid))
        {
            var byUuid = await _context.Conversations.FirstOrDefaultAsync(c => c.SourceUuid == sourceUuid);
            if (byUuid is not null) return byUuid;
        }

        return await _context.Conversations.FirstOrDefaultAsync(c => c.SourceFilePrefix == prefix);
    }

    // ── Create ────────────────────────────────────────────────────────────────

    private async Task CreateAsync(string sourceFilePrefix, ConversationImportSource source)
    {
        var conversation = new Conversation
        {
            Title            = source.Title,
            ConversationDate = ParseDate(source.ConversationDate) ?? DateTime.MinValue,
            Platform         = source.Platform,
            BlockCount       = source.Blocks.Count,
            ArcSummary       = string.Empty,
            SourceFilePrefix = sourceFilePrefix,
            SourceUuid       = source.SourceUuid,
            SourceUpdatedAt  = ParseDate(source.SourceUpdatedAt)
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(); // flush to get conversation.Id

        foreach (var cb in source.Blocks)
        {
            _context.ConversationBlocks.Add(new ConversationBlock
            {
                ConversationId = conversation.Id,
                BlockNumber    = cb.BlockNumber,
                Speaker        = cb.Speaker,
                RawContent     = cb.RawContent,
                IsCompaction   = cb.IsCompaction,
                // Blank on arrival, always. The note is Brian's to write in the reader.
                Summary        = string.Empty,
                BlockState     = BlockState.Unread
            });
        }
        await _context.SaveChangesAsync();
    }

    // ── Update (additive re-import of a reopened conversation) ──────────────────

    private async Task UpdateAsync(Conversation conversation, ConversationImportSource source)
    {
        conversation.Title            = source.Title;
        conversation.ConversationDate = ParseDate(source.ConversationDate) ?? conversation.ConversationDate;
        conversation.Platform         = source.Platform;
        conversation.BlockCount       = source.Blocks.Count;

        // ArcSummary is deliberately absent from this list. Existing arc summaries are frozen
        // text from the retired Cowork pass; nothing writes one any more, and a re-import least
        // of all.

        if (!string.IsNullOrEmpty(source.SourceUuid))
            conversation.SourceUuid = source.SourceUuid;
        conversation.SourceUpdatedAt = ParseDate(source.SourceUpdatedAt) ?? conversation.SourceUpdatedAt;

        var existingBlocksByNumber = await _context.ConversationBlocks
            .Where(b => b.ConversationId == conversation.Id)
            .ToDictionaryAsync(b => b.BlockNumber);

        foreach (var cb in source.Blocks)
        {
            if (existingBlocksByNumber.TryGetValue(cb.BlockNumber, out var block))
            {
                // Refresh the transcript, and nothing else. BlockState is the reader's triage
                // and Summary is Brian's own note — both are authored, both must survive a
                // re-import, and neither has an undo if this ever writes over them.
                block.Speaker      = cb.Speaker;
                block.RawContent   = cb.RawContent;
                block.IsCompaction = cb.IsCompaction;
            }
            else
            {
                // A genuinely new turn from reopening the conversation.
                _context.ConversationBlocks.Add(new ConversationBlock
                {
                    ConversationId = conversation.Id,
                    BlockNumber    = cb.BlockNumber,
                    Speaker        = cb.Speaker,
                    RawContent     = cb.RawContent,
                    IsCompaction   = cb.IsCompaction,
                    Summary        = string.Empty,
                    BlockState     = BlockState.Unread
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string PrefixOf(string fileName, string suffix)
        => fileName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
            ? fileName[..^suffix.Length]
            : fileName;

    private static DateTime? ParseDate(string? iso) =>
        !string.IsNullOrEmpty(iso) && DateTime.TryParse(iso, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt)
            ? dt
            : null;

    // ── JSON DTOs (content file shape) ────────────────────────────────────────

    private class ContentFile
    {
        public string             Platform         { get; set; } = string.Empty;
        public string             Title            { get; set; } = string.Empty;
        public string             ConversationDate { get; set; } = string.Empty;
        public string             SourceUuid       { get; set; } = string.Empty;
        public string             SourceUpdatedAt  { get; set; } = string.Empty;
        public List<ContentBlock> Blocks           { get; set; } = new();
    }

    private class ContentBlock
    {
        public int    BlockNumber  { get; set; }
        public string Speaker      { get; set; } = string.Empty;
        public string RawContent   { get; set; } = string.Empty;
        public bool   IsCompaction { get; set; }
    }
}
