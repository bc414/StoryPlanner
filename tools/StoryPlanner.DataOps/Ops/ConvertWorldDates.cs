using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;

namespace StoryPlanner.DataOps.Ops;

/// <summary>
/// The master-timeline data conversion, in three mechanical moves (config: configs/world-dates.v2.json):
///
/// 1. TRACK SPLIT — each configured History track splits into an event track (the existing row,
///    retargeted question, SupportsWorldDateEnd=false) and a NEW condition track
///    (SupportsWorldDateEnd=true). Four of the six original DisplayQuestions were already
///    compound along exactly this seam ("invention and usage", "formation and activities", …);
///    the split separates what the questions already asked for, and removes any need for a
///    per-note event/condition discriminator: the track IS the discriminator.
/// 2. DATE CONVERSION — every legacy free-text WorldDate string becomes structured columns at
///    year precision via <see cref="WorldDateLegacy"/> (mechanical, deterministic: trailing '?'
///    stripped, "N BLB" → -N, "A-B" → interval). Converted strings are blanked. Unconvertible
///    strings ("?", inverted ranges like "954-914") are LEFT IN PLACE — flag, never guess; they
///    surface on the triage page. Nothing is destroyed.
/// 3. RE-FILE BY NOTATION — notes that carried a legacy RANGE move to their track's new
///    condition twin; point notes stay put. Ranges are ~90% correctly shaped already; the
///    residual mis-filed points are Brian's re-file-by-hand as he works the timeline (a
///    NoteTrackDefinitionId change, no data loss).
///
/// Deliberately does NOT touch note Content/FlagReason/NoteState (the envelope's checksum
/// proves it), does not assign theaters (author-written config, never derived from names), and
/// does not create pivots/theaters (see SeedTimelineDefaults).
///
/// Idempotent: a second run finds blank legacy strings (nothing to convert), existing condition
/// twins (nothing to create), and identical track prose (no-op updates).
/// </summary>
public sealed class ConvertWorldDates : IDataOperation
{
    public string Name => "convert-world-dates";

    private Dictionary<string, long> _rowCountsBefore = new();

    private sealed record TrackText(string TrackName, string DisplayQuestion, string? UsageDirective, string? AuditDirective);
    private sealed record Split(int SourceTrackId, TrackText Event, TrackText Condition);

    public async Task Apply(AppDbContext ctx, JsonElement config)
    {
        _rowCountsBefore = PlanIntegrity.SnapshotRowCounts(ctx);

        var splits = ParseSplits(config);
        var tracks = await ctx.NoteTrackDefinitions.ToListAsync();
        var conditionIdBySourceId = new Dictionary<int, int>();

        // ── 1. Track split ──────────────────────────────────────────────────────
        foreach (var split in splits)
        {
            var source = tracks.FirstOrDefault(t => t.Id == split.SourceTrackId);
            if (source is null) continue; // tolerant: v1 archive has no track rows at all

            source.TrackName = split.Event.TrackName;
            source.DisplayQuestion = split.Event.DisplayQuestion;
            if (split.Event.UsageDirective is not null) source.UsageDirective = split.Event.UsageDirective;
            if (split.Event.AuditDirective is not null) source.AuditDirective = split.Event.AuditDirective;
            source.SupportsWorldDate = true;
            source.SupportsWorldDateEnd = false;

            var condition = tracks.FirstOrDefault(t =>
                t.SubjectDefinitionId == source.SubjectDefinitionId &&
                t.OwnerType == source.OwnerType &&
                t.TrackName == split.Condition.TrackName &&
                t.SupportsWorldDateEnd);

            if (condition is null)
            {
                condition = new NoteTrackDefinition
                {
                    SubjectDefinitionId = source.SubjectDefinitionId,
                    OwnerType = source.OwnerType,
                    TrackType = source.TrackType,
                    IsSingleton = source.IsSingleton,
                    SupportsWorldDate = true,
                    SupportsWorldDateEnd = true,
                    SupportsTheme = source.SupportsTheme,
                    SupportsSourceMaterial = source.SupportsSourceMaterial,
                    CanEditInAuditMode = source.CanEditInAuditMode,
                };
                // Slot the condition track directly after its event twin in every editor
                // mode: bump later tracks of the same owner group by one, then take source+1.
                foreach (var other in tracks.Where(t =>
                             t.SubjectDefinitionId == source.SubjectDefinitionId &&
                             t.OwnerType == source.OwnerType && t.Id != source.Id))
                {
                    if (other.ExpansionModeDisplayOrder > source.ExpansionModeDisplayOrder) other.ExpansionModeDisplayOrder++;
                    if (other.LinkingModeDisplayOrder > source.LinkingModeDisplayOrder) other.LinkingModeDisplayOrder++;
                    if (other.GardenerModeDisplayOrder > source.GardenerModeDisplayOrder) other.GardenerModeDisplayOrder++;
                    if (other.AuditModeDisplayOrder > source.AuditModeDisplayOrder) other.AuditModeDisplayOrder++;
                    if (other.SceneDesignModeDisplayOrder > source.SceneDesignModeDisplayOrder) other.SceneDesignModeDisplayOrder++;
                }
                condition.ExpansionModeDisplayOrder = source.ExpansionModeDisplayOrder + 1;
                condition.LinkingModeDisplayOrder = source.LinkingModeDisplayOrder + 1;
                condition.GardenerModeDisplayOrder = source.GardenerModeDisplayOrder + 1;
                condition.AuditModeDisplayOrder = source.AuditModeDisplayOrder + 1;
                condition.SceneDesignModeDisplayOrder = source.SceneDesignModeDisplayOrder + 1;

                ctx.NoteTrackDefinitions.Add(condition);
                tracks.Add(condition);
            }

            condition.TrackName = split.Condition.TrackName;
            condition.DisplayQuestion = split.Condition.DisplayQuestion;
            condition.UsageDirective = split.Condition.UsageDirective ?? condition.UsageDirective;
            condition.AuditDirective = split.Condition.AuditDirective ?? condition.AuditDirective;

            await ctx.SaveChangesAsync(); // assign the condition track's Id before re-filing
            conditionIdBySourceId[source.Id] = condition.Id;
        }

        // ── 2 + 3. Date conversion and re-file by notation ─────────────────────
        var converted = 0; var refiled = 0; var unconvertible = new List<string>();
        foreach (var note in await ctx.Notes.ToListAsync())
        {
            if (string.IsNullOrWhiteSpace(note.WorldDate)) continue;

            var outcome = WorldDateLegacy.TryConvert(note.WorldDate, out var date);
            switch (outcome)
            {
                case WorldDateLegacy.Outcome.Point:
                    note.SetWorldDate(date);
                    note.WorldDate = string.Empty;
                    converted++;
                    break;
                case WorldDateLegacy.Outcome.Range:
                    note.SetWorldDate(date);
                    note.WorldDate = string.Empty;
                    converted++;
                    if (note.NoteTrackDefinitionId is int trackId &&
                        conditionIdBySourceId.TryGetValue(trackId, out var conditionId))
                    {
                        note.NoteTrackDefinitionId = conditionId;
                        refiled++;
                    }
                    break;
                default:
                    unconvertible.Add($"note:{note.Id} \"{note.WorldDate}\"");
                    break; // left in place, structured stays null — triage surface
            }
        }

        Console.WriteLine($"  converted {converted} legacy WorldDate strings " +
                          $"({refiled} range notes re-filed to condition tracks)");
        Console.WriteLine(unconvertible.Count == 0
            ? "  0 unconvertible strings"
            : $"  {unconvertible.Count} unconvertible, left as-is for triage: {string.Join(", ", unconvertible)}");
    }

    public IEnumerable<PlanIntegrity.Violation> ExtraChecks(AppDbContext ctx, JsonElement config)
    {
        var violations = new List<PlanIntegrity.Violation>();

        violations.AddRange(PlanIntegrity.CompareRowCounts(
            _rowCountsBefore,
            PlanIntegrity.SnapshotRowCounts(ctx),
            allowedToChange: new HashSet<string> { "NoteTrackDefinitions" }));

        var trackById = ctx.NoteTrackDefinitions.ToDictionary(t => t.Id);

        foreach (var n in ctx.Notes)
        {
            // Structured dates must be well-formed (month/day ranges, day-implies-month,
            // start <= end). The struct constructor is the validator; a throw is a violation.
            WorldDate? date;
            try { date = n.GetWorldDate(); }
            catch (Exception ex)
            {
                violations.Add(new PlanIntegrity.Violation(
                    "worlddate.malformed", $"note:{n.Id} — {ex.Message}"));
                continue;
            }

            if (date is null) continue;

            // A note with both structured and legacy text would be two representations at once.
            if (!string.IsNullOrWhiteSpace(n.WorldDate))
                violations.Add(new PlanIntegrity.Violation(
                    "worlddate.double_representation", $"note:{n.Id} has structured date AND legacy \"{n.WorldDate}\""));

            // An interval on an event track contradicts the track split's whole premise.
            if (date.Value.End is not null &&
                n.NoteTrackDefinitionId is int tid && trackById.TryGetValue(tid, out var track) &&
                track.SupportsWorldDate && !track.SupportsWorldDateEnd)
                violations.Add(new PlanIntegrity.Violation(
                    "worlddate.interval_on_event_track", $"note:{n.Id} on track:{tid} ({track.TrackName})"));
        }

        // Every configured split must have produced its condition twin (when the source exists).
        foreach (var split in ParseSplits(config))
        {
            if (!trackById.TryGetValue(split.SourceTrackId, out var source)) continue;
            var hasTwin = ctx.NoteTrackDefinitions.Any(t =>
                t.SubjectDefinitionId == source.SubjectDefinitionId &&
                t.OwnerType == source.OwnerType &&
                t.TrackName == split.Condition.TrackName &&
                t.SupportsWorldDate && t.SupportsWorldDateEnd);
            if (!hasTwin)
                violations.Add(new PlanIntegrity.Violation(
                    "tracksplit.condition_missing", $"source track:{split.SourceTrackId} ({split.Event.TrackName})"));
        }

        return violations;
    }

    private static List<Split> ParseSplits(JsonElement config) =>
        config.GetProperty("trackSplits").EnumerateArray()
            .Select(e => new Split(
                e.GetProperty("sourceTrackId").GetInt32(),
                ParseText(e.GetProperty("event")),
                ParseText(e.GetProperty("condition"))))
            .ToList();

    private static TrackText ParseText(JsonElement e) => new(
        e.GetProperty("trackName").GetString() ?? "",
        e.GetProperty("displayQuestion").GetString() ?? "",
        e.TryGetProperty("usageDirective", out var u) ? u.GetString() : null,
        e.TryGetProperty("auditDirective", out var a) ? a.GetString() : null);
}
