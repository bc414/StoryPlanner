using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

/// <summary>
/// Merges a *_content.json (raw block text) with a *_meta.json (AI-generated summaries and routing)
/// and persists the resulting entities to the database.
/// Both files are matched by blockNumber. The app never re-emits raw content during extraction.
///
/// Additive/incremental: a content file whose sourceUuid (or, absent that, NNN_{slug} prefix)
/// matches an existing Conversation is treated as a re-analysis of a reopened conversation rather
/// than a duplicate. Blocks are upserted by BlockNumber — already-reviewed blocks keep their
/// BlockState (Done/Flagged/Skipped) while newly-added turns land as Unread — and the arc summary
/// / subject coverage are refreshed from the new meta file.
/// </summary>
public class ConversationImporter
{
    private readonly AppDbContext                     _context;
    private readonly ObservableCollection<Subject>   _subjects;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ConversationImporter(AppDbContext context, ObservableCollection<Subject> subjects)
    {
        _context  = context;
        _subjects = subjects;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Import one paired content+meta file set. Creates a new Conversation, or — when the content
    /// file's sourceUuid/prefix matches one already in the DB — updates it additively.
    /// </summary>
    public async Task ImportAsync(string contentPath, string metaPath)
    {
        string prefix  = PrefixOf(Path.GetFileName(contentPath), "_content.json");
        var    content = DeserializeContent(contentPath);
        var    meta    = DeserializeMeta(metaPath);

        var existing = await FindExistingAsync(prefix, content.SourceUuid);
        if (existing is null)
        {
            Console.WriteLine($"  [new] {prefix}");
            await CreateAsync(prefix, content, meta);
        }
        else
        {
            Console.WriteLine($"  [update] {prefix} (conversation #{existing.Id})");
            await UpdateAsync(existing, content, meta);
        }
    }

    /// <summary>
    /// Import all paired content+meta files in a folder.
    /// Pairs are matched by the NNN_{slug} prefix (content suffix _content.json, meta suffix _meta.json).
    /// </summary>
    public async Task ImportFolderAsync(string folderPath)
    {
        var contentFiles = Directory.GetFiles(folderPath, "*_content.json")
            .ToDictionary(f => PrefixOf(Path.GetFileName(f), "_content.json"), f => f);

        var metaFiles = Directory.GetFiles(folderPath, "*_meta.json")
            .ToDictionary(f => PrefixOf(Path.GetFileName(f), "_meta.json"), f => f);

        foreach (var (prefix, contentPath) in contentFiles)
        {
            if (!metaFiles.TryGetValue(prefix, out var metaPath))
            {
                Console.WriteLine($"  [skip] no matching meta file for {Path.GetFileName(contentPath)}");
                continue;
            }

            await ImportAsync(contentPath, metaPath);
        }
    }

    // ── Deserialization ───────────────────────────────────────────────────────

    private static ContentFile DeserializeContent(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<ContentFile>(stream, JsonOpts)
               ?? throw new InvalidDataException($"Failed to deserialize content file: {path}");
    }

    private static MetaFile DeserializeMeta(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<MetaFile>(stream, JsonOpts)
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

    private async Task CreateAsync(string sourceFilePrefix, ContentFile content, MetaFile meta)
    {
        var conversation = new Conversation
        {
            Title            = content.Title,
            ConversationDate = ParseDate(content.ConversationDate) ?? DateTime.MinValue,
            Platform         = content.Platform,
            BlockCount       = content.Blocks.Count,
            ArcSummary       = meta.ArcSummary,
            SourceFilePrefix = sourceFilePrefix,
            SourceUuid       = content.SourceUuid,
            SourceUpdatedAt  = ParseDate(content.SourceUpdatedAt)
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(); // flush to get conversation.Id

        var metaBlockMap = meta.Blocks.ToDictionary(b => b.BlockNumber);
        foreach (var cb in content.Blocks)
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
                HasDecisions   = mb?.HasDecisions ?? false,
                BlockState     = BlockState.Unread
            });
        }
        await _context.SaveChangesAsync();

        await ReplaceSubjectCoverageAsync(conversation, meta);
    }

    // ── Update (additive re-import of a reopened conversation) ──────────────────

    private async Task UpdateAsync(Conversation conversation, ContentFile content, MetaFile meta)
    {
        conversation.Title            = content.Title;
        conversation.ConversationDate = ParseDate(content.ConversationDate) ?? conversation.ConversationDate;
        conversation.Platform         = content.Platform;
        conversation.BlockCount       = content.Blocks.Count;
        conversation.ArcSummary       = meta.ArcSummary;
        if (!string.IsNullOrEmpty(content.SourceUuid))
            conversation.SourceUuid = content.SourceUuid;
        conversation.SourceUpdatedAt = ParseDate(content.SourceUpdatedAt) ?? conversation.SourceUpdatedAt;

        var metaBlockMap = meta.Blocks.ToDictionary(b => b.BlockNumber);
        var existingBlocksByNumber = await _context.ConversationBlocks
            .Where(b => b.ConversationId == conversation.Id)
            .ToDictionaryAsync(b => b.BlockNumber);

        foreach (var cb in content.Blocks)
        {
            metaBlockMap.TryGetValue(cb.BlockNumber, out var mb);

            if (existingBlocksByNumber.TryGetValue(cb.BlockNumber, out var block))
            {
                // Refresh content/summary but deliberately leave BlockState untouched —
                // the reader's read-state on already-reviewed blocks must survive re-analysis.
                block.Speaker      = cb.Speaker;
                block.RawContent   = cb.RawContent;
                block.IsCompaction = cb.IsCompaction;
                block.Summary      = mb?.Summary ?? block.Summary;
                block.HasDecisions = mb?.HasDecisions ?? block.HasDecisions;
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
                    HasDecisions   = mb?.HasDecisions ?? false,
                    BlockState     = BlockState.Unread
                });
            }
        }
        await _context.SaveChangesAsync();

        await ReplaceSubjectCoverageAsync(conversation, meta);
    }

    // ── Subject coverage ──────────────────────────────────────────────────────

    /// <summary>
    /// Drops and re-adds this conversation's subject coverage + track rows from a fresh meta file.
    /// Coverage carries no read-state of its own (unlike blocks), so a clean replace is safe and
    /// keeps a re-analyzed conversation's routing fully in sync with the latest Cowork output.
    /// </summary>
    private async Task ReplaceSubjectCoverageAsync(Conversation conversation, MetaFile meta)
    {
        var oldCoverages = await _context.ConversationSubjectCoverages
            .Where(c => c.ConversationId == conversation.Id)
            .ToListAsync();

        if (oldCoverages.Count > 0)
        {
            var oldCoverageIds = oldCoverages.Select(c => c.Id).ToList();
            var oldTracks = await _context.ConversationSubjectCoverageTracks
                .Where(t => oldCoverageIds.Contains(t.ConversationSubjectCoverageId))
                .ToListAsync();

            _context.ConversationSubjectCoverageTracks.RemoveRange(oldTracks);
            _context.ConversationSubjectCoverages.RemoveRange(oldCoverages);
            await _context.SaveChangesAsync();
        }

        foreach (var sc in meta.SubjectsCovered)
        {
            // Resolve SubjectId by the explicit ID from the meta file if present,
            // otherwise fall back to name matching against the in-memory subjects collection.
            int? resolvedId = ResolveSubjectId(sc);
            if (resolvedId is null) continue; // skip unresolved subjects

            var coverage = new ConversationSubjectCoverage
            {
                ConversationId = conversation.Id,
                SubjectId      = resolvedId.Value
            };

            _context.ConversationSubjectCoverages.Add(coverage);
            await _context.SaveChangesAsync(); // flush to get coverage.Id

            foreach (var trackId in sc.NoteTrackDefinitionIds)
            {
                _context.ConversationSubjectCoverageTracks.Add(new ConversationSubjectCoverageTrack
                {
                    ConversationSubjectCoverageId = coverage.Id,
                    NoteTrackDefinitionId         = trackId
                });
            }

            await _context.SaveChangesAsync();
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private int? ResolveSubjectId(MetaSubjectCovered sc)
    {
        if (sc.SubjectId.HasValue && sc.SubjectId.Value > 0)
        {
            return _subjects.Any(s => s.Id == sc.SubjectId.Value)
                ? sc.SubjectId.Value
                : null;
        }

        var match = _subjects.FirstOrDefault(s =>
            string.Equals(s.Name, sc.SubjectName, StringComparison.OrdinalIgnoreCase));
        return match?.Id;
    }

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

    // ── JSON DTOs (meta file shape) ───────────────────────────────────────────

    private class MetaFile
    {
        public string                ArcSummary      { get; set; } = string.Empty;
        public List<MetaSubjectCovered> SubjectsCovered { get; set; } = new();
        public List<MetaBlock>       Blocks          { get; set; } = new();
    }

    private class MetaSubjectCovered
    {
        public int?   SubjectId              { get; set; }
        public string SubjectName            { get; set; } = string.Empty;
        public List<int> NoteTrackDefinitionIds { get; set; } = new();
    }

    private class MetaBlock
    {
        public int    BlockNumber  { get; set; }
        public string Summary      { get; set; } = string.Empty;
        public bool   HasDecisions { get; set; }
    }
}
