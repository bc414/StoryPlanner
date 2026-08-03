using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

/// <summary>
/// One conversation's raw block text, independent of where it came from. Both the file-based
/// import (a *_content.json written for a Cowork round trip) and the direct import (straight from
/// a scanned conversations.json export) project onto this shape, so the merge logic below has a
/// single implementation.
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
/// The optional enrichment half: summaries authored outside the app (a *_meta.json). Summaries are
/// a navigation aid only — nothing here proposes structure, and a null ConversationMeta is an
/// ordinary, fully supported import.
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

/// <summary>
/// What a batch import actually did. <paramref name="WithoutSummaries"/> counts conversations
/// imported with no meta file — worth reporting back, since an unpaired content file now imports
/// rather than being skipped and the user would otherwise not know which ones landed bare.
/// </summary>
public sealed record ConversationImportResult(int Created, int Updated, int WithoutSummaries)
{
    public int Total => Created + Updated;

    public static ConversationImportResult operator +(ConversationImportResult a, ConversationImportResult b) =>
        new(a.Created + b.Created, a.Updated + b.Updated, a.WithoutSummaries + b.WithoutSummaries);

    public static readonly ConversationImportResult Empty = new(0, 0, 0);
}

/// <summary>
/// Persists imported conversations. The raw block text is required; summaries are optional.
///
/// Additive/incremental: a source whose sourceUuid (or, absent that, NNN_{slug} prefix) matches an
/// existing Conversation is treated as a re-import of a reopened conversation rather than a
/// duplicate. Blocks are upserted by BlockNumber — already-reviewed blocks keep their BlockState
/// (Done/Flagged/Skipped) while newly-added turns land as Unread.
///
/// Re-importing *without* meta never erases summaries an earlier meta pass produced; meta only
/// ever adds. Nothing in this class writes ConversationSubjectCoverage — the AI-suggested
/// subject×track routing was cut on 2026-07-31 (the tables and their existing rows remain, but no
/// code path adds to them). A meta file that still carries a subjectsCovered array imports
/// cleanly; the property is simply ignored.
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
    /// A null <paramref name="meta"/> imports raw content with no summaries.
    /// </summary>
    public async Task<ConversationImportResult> ImportAsync(
        ConversationImportSource source, string sourceFilePrefix, ConversationMeta? meta)
    {
        int bare = meta is null ? 1 : 0;

        var existing = await FindExistingAsync(sourceFilePrefix, source.SourceUuid);
        if (existing is null)
        {
            await CreateAsync(sourceFilePrefix, source, meta);
            return new ConversationImportResult(Created: 1, Updated: 0, WithoutSummaries: bare);
        }

        await UpdateAsync(existing, source, meta);
        return new ConversationImportResult(Created: 0, Updated: 1, WithoutSummaries: bare);
    }

    /// <summary>
    /// Import one content file, optionally paired with a meta file. Pass null for
    /// <paramref name="metaPath"/> to import raw content with no summaries.
    /// </summary>
    public async Task<ConversationImportResult> ImportFileAsync(string contentPath, string? metaPath)
    {
        string prefix  = PrefixOf(Path.GetFileName(contentPath), "_content.json");
        var    content = DeserializeContent(contentPath);
        var    meta    = metaPath is null ? null : DeserializeMeta(metaPath);

        return await ImportAsync(ToSource(content), prefix, meta);
    }

    /// <summary>
    /// Import every *_content.json in a folder. A matching *_meta.json (same NNN_{slug} prefix)
    /// is used when present; a content file without one imports without summaries rather than
    /// being skipped.
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
    /// Import scan rows straight from a parsed Claude export — no content files, no meta, no
    /// Cowork round trip. New conversations are assigned the same NNN_{slug} prefix a file export
    /// would have given them, so exporting one later for a summary pass still pairs up.
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

            result += await ImportAsync(ToSource(item.Export), prefix, meta: null);
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

    private async Task CreateAsync(string sourceFilePrefix, ConversationImportSource source, ConversationMeta? meta)
    {
        var conversation = new Conversation
        {
            Title            = source.Title,
            ConversationDate = ParseDate(source.ConversationDate) ?? DateTime.MinValue,
            Platform         = source.Platform,
            BlockCount       = source.Blocks.Count,
            ArcSummary       = meta?.ArcSummary ?? string.Empty,
            SourceFilePrefix = sourceFilePrefix,
            SourceUuid       = source.SourceUuid,
            SourceUpdatedAt  = ParseDate(source.SourceUpdatedAt)
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(); // flush to get conversation.Id

        var metaBlockMap = BuildMetaBlockMap(meta);
        foreach (var cb in source.Blocks)
        {
            metaBlockMap.TryGetValue(cb.BlockNumber, out var mb);
            _context.ConversationBlocks.Add(new ConversationBlock
            {
                ConversationId = conversation.Id,
                BlockNumber    = cb.BlockNumber,
                Speaker        = cb.Speaker,
                RawContent     = cb.RawContent,
                IsCompaction   = cb.IsCompaction,
                Summary        = mb?.Summary ?? string.Empty,
                BlockState     = BlockState.Unread
            });
        }
        await _context.SaveChangesAsync();
    }

    // ── Update (additive re-import of a reopened conversation) ──────────────────

    private async Task UpdateAsync(Conversation conversation, ConversationImportSource source, ConversationMeta? meta)
    {
        conversation.Title            = source.Title;
        conversation.ConversationDate = ParseDate(source.ConversationDate) ?? conversation.ConversationDate;
        conversation.Platform         = source.Platform;
        conversation.BlockCount       = source.Blocks.Count;

        // Only a meta pass may touch the arc summary — a content-only re-import of a conversation
        // that was summarized earlier must not blank it. And "meta never destroys" extends inside
        // the meta itself: an EMPTY ArcSummary in a meta file means "nothing supplied", never
        // "erase what an earlier pass wrote".
        if (!string.IsNullOrWhiteSpace(meta?.ArcSummary))
            conversation.ArcSummary = meta!.ArcSummary;

        if (!string.IsNullOrEmpty(source.SourceUuid))
            conversation.SourceUuid = source.SourceUuid;
        conversation.SourceUpdatedAt = ParseDate(source.SourceUpdatedAt) ?? conversation.SourceUpdatedAt;

        var metaBlockMap = BuildMetaBlockMap(meta);
        var existingBlocksByNumber = await _context.ConversationBlocks
            .Where(b => b.ConversationId == conversation.Id)
            .ToDictionaryAsync(b => b.BlockNumber);

        foreach (var cb in source.Blocks)
        {
            metaBlockMap.TryGetValue(cb.BlockNumber, out var mb);

            if (existingBlocksByNumber.TryGetValue(cb.BlockNumber, out var block))
            {
                // Refresh content/summary but deliberately leave BlockState untouched —
                // the reader's read-state on already-reviewed blocks must survive re-import.
                // A meta block carrying an empty Summary is "nothing supplied", not an erase.
                block.Speaker      = cb.Speaker;
                block.RawContent   = cb.RawContent;
                block.IsCompaction = cb.IsCompaction;
                if (!string.IsNullOrWhiteSpace(mb?.Summary))
                    block.Summary = mb!.Summary;
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
                    Summary        = mb?.Summary ?? string.Empty,
                    BlockState     = BlockState.Unread
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static Dictionary<int, ConversationMetaBlock> BuildMetaBlockMap(ConversationMeta? meta) =>
        meta is null
            ? new Dictionary<int, ConversationMetaBlock>()
            : meta.Blocks
                .GroupBy(b => b.BlockNumber)
                .ToDictionary(g => g.Key, g => g.First());

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
