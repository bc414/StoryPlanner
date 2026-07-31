using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.DataOps;

/// <summary>
/// Seeds the two-tier Source Material coverage model (configs/source-material.v2.json) and
/// flips SupportsSourceMaterial on the tracks Brian decided should carry citations — the six
/// TrackType.Canon tracks, per the plan this shipped with. Does two things, both idempotent:
///
/// 1. UPSERT WORKS AND PARTS — SourceMaterials matched by Name, SourceMaterialParts matched by
///    (SourceMaterialId, Code). Re-running with edited config prose updates rows in place rather
///    than duplicating; a Part's ReviewState is only set on first creation, never overwritten,
///    so a re-run never silently un-reviews something Brian has already checked off.
/// 2. ENABLE TRACKS — sets SupportsSourceMaterial = true on each configured track id, tolerant
///    of a missing id (v1 archive has no track rows at all — same tolerance ConvertWorldDates
///    uses for the same reason).
///
/// Deliberately does NOT touch Notes or NoteSourceReferences — citing a seeded Part is authorial
/// work done in the app, never this op's job (CLAUDE.md: retrieval, not suggestion — an op
/// proposing which notes cite which episode would be exactly the abandoned coverage-suggestion
/// shape).
///
/// The seed list itself is intentionally NOT the full ~221-episode MLP:FiM catalog: it covers
/// only Parts already evidenced in Brian's own note text (grep'd verbatim from the real
/// TLTT v2.storyplan), plus a small number of season premieres/finales whose SxxExx numbering is
/// unambiguous, well-established public fact. Assigning confident episode numbers beyond that
/// would be exactly the kind of guess CLAUDE.md's "flag, never guess" rule warns against for a
/// reference dataset Brian will trust. Extending the list — by hand-editing the config and
/// re-running, or via the in-app "+ Add new" quick-add on the picker — is explicitly left to him.
/// </summary>
public sealed class SeedSourceMaterial : IDataOperation
{
    public string Name => "seed-source-material";

    private Dictionary<string, long> _rowCountsBefore = new();

    public async Task Apply(AppDbContext ctx, JsonElement config)
    {
        _rowCountsBefore = PlanIntegrity.SnapshotRowCounts(ctx);

        var works = await ctx.SourceMaterials.ToListAsync();
        var parts = await ctx.SourceMaterialParts.ToListAsync();

        if (config.TryGetProperty("sourceMaterials", out var workArray))
        {
            foreach (var w in workArray.EnumerateArray())
            {
                var name = w.GetProperty("name").GetString() ?? "";
                var work = works.FirstOrDefault(x => x.Name == name);
                if (work is null)
                {
                    work = new SourceMaterial { Name = name };
                    ctx.SourceMaterials.Add(work);
                    works.Add(work);
                }
                work.Description = w.TryGetProperty("description", out var d) ? d.GetString() ?? "" : "";
                work.PartNoun = w.TryGetProperty("partNoun", out var pn) ? pn.GetString() ?? "" : "";
                work.OrderIndex = w.GetProperty("orderIndex").GetInt32();

                // Parts need their Work's Id, which only exists after a save when the Work is
                // new — same "save to get the Id, then re-file" move ConvertWorldDates uses for
                // its condition tracks.
                if (work.Id == 0) await ctx.SaveChangesAsync();

                if (!w.TryGetProperty("parts", out var partArray)) continue;
                foreach (var p in partArray.EnumerateArray())
                {
                    var code = p.GetProperty("code").GetString() ?? "";
                    var part = parts.FirstOrDefault(x => x.SourceMaterialId == work.Id && x.Code == code);
                    if (part is null)
                    {
                        part = new SourceMaterialPart
                        {
                            SourceMaterialId = work.Id, Code = code,
                            ReviewState = SourcePartReviewState.NotReviewed // set once, never re-stamped on re-run
                        };
                        ctx.SourceMaterialParts.Add(part);
                        parts.Add(part);
                    }
                    part.Name = p.TryGetProperty("name", out var pName) ? pName.GetString() ?? "" : "";
                    part.Description = p.TryGetProperty("description", out var pDesc) ? pDesc.GetString() ?? "" : "";
                    part.OrderIndex = p.GetProperty("orderIndex").GetInt32();
                }
            }
        }

        // ── Enable citation on the configured tracks ────────────────────────────
        var enabled = 0;
        if (config.TryGetProperty("enableSourceMaterialOnTracks", out var trackIdArray))
        {
            var trackIds = trackIdArray.EnumerateArray().Select(e => e.GetInt32()).ToHashSet();
            var tracks = await ctx.NoteTrackDefinitions
                .Where(t => trackIds.Contains(t.Id))
                .ToListAsync();
            foreach (var t in tracks)
            {
                t.SupportsSourceMaterial = true;
                enabled++;
            }
        }

        Console.WriteLine($"  {works.Count} Work(s), {parts.Count} Part(s) upserted; " +
                          $"SupportsSourceMaterial enabled on {enabled} track(s)");
    }

    public IEnumerable<PlanIntegrity.Violation> ExtraChecks(AppDbContext ctx, JsonElement config)
    {
        var violations = new List<PlanIntegrity.Violation>();

        violations.AddRange(PlanIntegrity.CompareRowCounts(
            _rowCountsBefore,
            PlanIntegrity.SnapshotRowCounts(ctx),
            allowedToChange: new HashSet<string> { "SourceMaterials", "SourceMaterialParts" }));

        foreach (var dup in ctx.SourceMaterials.AsEnumerable().GroupBy(w => w.Name).Where(g => g.Count() > 1))
            violations.Add(new PlanIntegrity.Violation("sourcematerial.duplicate_name", dup.Key));

        foreach (var dup in ctx.SourceMaterialParts.AsEnumerable()
                     .GroupBy(p => (p.SourceMaterialId, p.Code)).Where(g => g.Count() > 1))
            violations.Add(new PlanIntegrity.Violation(
                "sourcepart.duplicate_code", $"material:{dup.Key.SourceMaterialId} code:{dup.Key.Code}"));

        return violations;
    }
}
