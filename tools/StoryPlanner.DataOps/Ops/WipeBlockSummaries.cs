using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.DataOps;

/// <summary>
/// Blanks every <see cref="ConversationBlock.Summary"/> (config: configs/wipe-block-summaries.json).
///
/// Block summaries were AI-written navigation aids produced by the retired Cowork round trip.
/// They are now Brian's own hand-written notes, edited in the Conversation Reader — so the
/// machine-written text has to go, and it has to go BEFORE the editable UI ships: once a note
/// can be typed, nothing distinguishes an authored summary from an imported one, and this op
/// cannot tell them apart either. Run it once, with the app closed (the app holds every block as
/// a live POCO and its exit save would write the old summaries straight back over the wipe).
///
/// Deliberately does NOT touch <see cref="Conversation.ArcSummary"/>: arc summaries keep their
/// imported text as frozen historical prose, displayed read-only. ExtraChecks proves it.
///
/// Unlike every other op here this one only destroys, so it takes a confirmation token in its
/// config rather than running off the op name alone.
///
/// Idempotent: a second run finds nothing to clear and exits 0.
/// </summary>
public sealed class WipeBlockSummaries : IDataOperation
{
    public string Name => "wipe-block-summaries";

    private const string ConfirmToken = "wipe-all-conversation-block-summaries";

    private Dictionary<string, long> _rowCountsBefore = new();
    private string _blockFingerprintBefore = string.Empty;
    private string _arcFingerprintBefore = string.Empty;

    public async Task Apply(AppDbContext ctx, JsonElement config)
    {
        var confirm = config.TryGetProperty("confirm", out var c) ? c.GetString() : null;
        if (confirm != ConfirmToken)
            throw new InvalidOperationException(
                $"Refusing: config must carry \"confirm\": \"{ConfirmToken}\". This op destroys text and cannot be undone.");

        _rowCountsBefore = PlanIntegrity.SnapshotRowCounts(ctx);
        _blockFingerprintBefore = BlockFingerprint(ctx);
        _arcFingerprintBefore = ArcFingerprint(ctx);

        var blocks = await ctx.ConversationBlocks.ToListAsync();
        var titleById = await ctx.Conversations.ToDictionaryAsync(c2 => c2.Id, c2 => c2.Title);

        var cleared = 0;
        var clearedByConversation = new Dictionary<int, int>();
        foreach (var block in blocks.Where(b => b.Summary.Length > 0))
        {
            block.Summary = string.Empty;
            cleared++;
            clearedByConversation[block.ConversationId] =
                clearedByConversation.GetValueOrDefault(block.ConversationId) + 1;
        }

        // Row counts don't move when only a text column is blanked, so the envelope's own report
        // shows no "<-- changed" marker for this op. This IS the evidence — print it in full.
        Console.WriteLine($"  cleared {cleared} of {blocks.Count} block summaries " +
                          $"({blocks.Count - cleared} were already empty) " +
                          $"across {clearedByConversation.Count} conversation(s)");

        const int listCap = 20;
        foreach (var (convId, count) in clearedByConversation.OrderByDescending(kv => kv.Value).Take(listCap))
            Console.WriteLine($"    conv:{convId} \"{titleById.GetValueOrDefault(convId, "(missing)")}\" — {count}");
        if (clearedByConversation.Count > listCap)
            Console.WriteLine($"    … and {clearedByConversation.Count - listCap} more conversation(s)");

        Console.WriteLine("  ArcSummary is NOT touched by this op.");
    }

    public IEnumerable<PlanIntegrity.Violation> ExtraChecks(AppDbContext ctx, JsonElement config)
    {
        var violations = new List<PlanIntegrity.Violation>();

        // Nothing may be added or removed anywhere: this op only blanks one text column.
        violations.AddRange(PlanIntegrity.CompareRowCounts(
            _rowCountsBefore,
            PlanIntegrity.SnapshotRowCounts(ctx),
            allowedToChange: new HashSet<string>()));

        // The job actually finished.
        foreach (var b in ctx.ConversationBlocks.AsEnumerable().Where(b => b.Summary.Length > 0).Take(10))
            violations.Add(new PlanIntegrity.Violation(
                "wipeblocksummaries.summary_remaining",
                $"block:{b.Id} (conv:{b.ConversationId} #{b.BlockNumber})"));

        // Only Summary moved. The transcript text and the triage state are the irreplaceable half
        // of this table — the fingerprint deliberately excludes Summary so it can prove that.
        if (BlockFingerprint(ctx) != _blockFingerprintBefore)
            violations.Add(new PlanIntegrity.Violation(
                "wipeblocksummaries.block_changed",
                "a block's content, speaker or triage state changed — only Summary may move"));

        if (ArcFingerprint(ctx) != _arcFingerprintBefore)
            violations.Add(new PlanIntegrity.Violation(
                "wipeblocksummaries.arcsummary_changed",
                "a conversation's ArcSummary changed — this op must never touch it"));

        return violations;
    }

    /// <summary>
    /// Every block field EXCEPT Summary, ordered by Id. Excluding Summary is the whole point:
    /// including it would make the check fail by construction.
    /// </summary>
    private static string BlockFingerprint(AppDbContext ctx) => Sha256Lines(
        ctx.ConversationBlocks
            .OrderBy(b => b.Id)
            .Select(b => $"{b.Id}␟{b.ConversationId}␟{b.BlockNumber}␟{b.Speaker}␟{b.RawContent}␟{b.IsCompaction}␟{(int)b.BlockState}"));

    private static string ArcFingerprint(AppDbContext ctx) => Sha256Lines(
        ctx.Conversations
            .OrderBy(c => c.Id)
            .Select(c => $"{c.Id}␟{c.ArcSummary}"));

    private static string Sha256Lines(IQueryable<string> lines) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("\n", lines.ToList()))));
}
