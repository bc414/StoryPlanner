using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.DataOps;

/// <summary>
/// Seeds the WorkPhase rows and the narrative property definitions + their allowed values from a
/// config Brian has reviewed (configs/narrative-properties.v2.json). Idempotent on all three
/// tiers; re-runs update order, flags, and the gating reference, never duplicate.
///
/// SEEDS STRUCTURE, NEVER PROSE. The config has no field for Question, Explanation, or a value's
/// Description — not "the op leaves them alone", but "there is nowhere to put them". Those fields
/// carry Brian's framing of what a property *asks*, and a plausible machine-written one is worse
/// than an empty one: an empty field is visibly unfinished, a wrong one reads as decided. (The
/// 2026-07-30 History-track split is the precedent — its seeded display questions had to be
/// rewritten.) They are authored in the app's Definitions tab, and because the config cannot
/// express them, no re-run can ever clobber that work. See CLAUDE.md, "What the tool must never do".
///
/// Deliberately does NOT write NarrativePropertyValues — assigning a subject a position on an axis
/// is categorization, which is Brian's alone (CLAUDE.md: retrieval, not suggestion). An op that
/// picked values would be the abandoned coverage-suggestion shape wearing a different hat. A test
/// asserts the table stays untouched.
///
/// Tolerant of a subject type the file does not have, the same way ConvertWorldDates and
/// SeedSourceMaterial tolerate missing track ids: the v1 archive's SubjectDefinitions hold triage
/// labels rather than categories, so every property is skipped there and the op is a no-op that
/// still brings the file to migration head.
/// </summary>
public sealed class SeedNarrativeProperties : IDataOperation
{
    public string Name => "seed-narrative-properties";

    private Dictionary<string, long> _rowCountsBefore = new();

    public async Task Apply(AppDbContext ctx, JsonElement config)
    {
        _rowCountsBefore = PlanIntegrity.SnapshotRowCounts(ctx);

        // ── Work phases ─────────────────────────────────────────────────────────
        var phases = await ctx.WorkPhases.ToListAsync();
        if (config.TryGetProperty("workPhases", out var phaseArray))
        {
            foreach (var e in phaseArray.EnumerateArray())
            {
                var name = e.GetProperty("name").GetString() ?? "";
                var phase = phases.FirstOrDefault(p => p.Name == name);
                if (phase is null)
                {
                    phase = new WorkPhase { Name = name };
                    ctx.WorkPhases.Add(phase);
                    phases.Add(phase);
                }
                phase.DisplayOrder = e.GetProperty("displayOrder").GetInt32();
                phase.RequiresZeroFlaggedNotes = e.TryGetProperty("requiresZeroFlaggedNotes", out var f) && f.GetBoolean();
                phase.RequiresZeroUnsetNotes   = e.TryGetProperty("requiresZeroUnsetNotes", out var u) && u.GetBoolean();
            }

            // Phases are referenced by NAME from the property config below, so they need their
            // EF-assigned Ids before the next block runs — the same "save to get the Id, then
            // re-file" move SeedSourceMaterial uses between Works and Parts.
            if (phases.Any(p => p.Id == 0)) await ctx.SaveChangesAsync();
        }

        // ── Property definitions and their allowed values ───────────────────────
        var subjectDefs = await ctx.SubjectDefinitions.ToListAsync();
        var propertyDefs = await ctx.NarrativePropertyDefinitions.ToListAsync();
        var valueDefs = await ctx.NarrativePropertyValueDefinitions.ToListAsync();

        var created = 0;
        var skipped = 0;

        if (config.TryGetProperty("properties", out var propertyArray))
        {
            foreach (var e in propertyArray.EnumerateArray())
            {
                var subjectType = e.GetProperty("subjectType").GetString() ?? "";
                var subjectDef = subjectDefs.FirstOrDefault(s => s.SubjectType == subjectType);
                if (subjectDef is null)
                {
                    skipped++;
                    continue;   // v1 archive, or a type Brian has not created yet — never invent one
                }

                var ownerTypeName = e.TryGetProperty("ownerType", out var ot) ? ot.GetString() ?? "Subject" : "Subject";
                if (!Enum.TryParse<OwnerType>(ownerTypeName, ignoreCase: true, out var ownerType))
                    throw new InvalidOperationException($"Unknown ownerType '{ownerTypeName}' in config.");

                var name = e.GetProperty("name").GetString() ?? "";
                var property = propertyDefs.FirstOrDefault(p => p.OwnerType == ownerType
                                                            && p.SubjectDefinitionId == subjectDef.Id
                                                            && p.Name == name);
                if (property is null)
                {
                    property = new NarrativePropertyDefinition
                    {
                        OwnerType = ownerType, SubjectDefinitionId = subjectDef.Id, Name = name
                        // Question / Explanation stay string.Empty — authored in the app, see above.
                    };
                    ctx.NarrativePropertyDefinitions.Add(property);
                    propertyDefs.Add(property);
                    created++;
                }
                property.DisplayOrder = e.GetProperty("displayOrder").GetInt32();

                // Gating phase resolved by NAME, never by id: the config has to survive being run
                // against a different file whose WorkPhase ids differ.
                property.GatingWorkPhaseId = e.TryGetProperty("gatingWorkPhase", out var g)
                                             && g.ValueKind == JsonValueKind.String
                    ? phases.FirstOrDefault(p => p.Name == g.GetString())?.Id
                    : null;

                if (property.Id == 0) await ctx.SaveChangesAsync();

                if (!e.TryGetProperty("values", out var valueArray)) continue;
                foreach (var v in valueArray.EnumerateArray())
                {
                    var valueName = v.GetProperty("valueName").GetString() ?? "";
                    if (valueDefs.Any(x => x.NarrativePropertyDefinitionId == property.Id && x.ValueName == valueName))
                        continue;

                    var valueDef = new NarrativePropertyValueDefinition
                    {
                        NarrativePropertyDefinitionId = property.Id, ValueName = valueName
                        // Description stays string.Empty — authored in the app.
                    };
                    ctx.NarrativePropertyValueDefinitions.Add(valueDef);
                    valueDefs.Add(valueDef);
                }
            }
        }

        Console.WriteLine($"  {phases.Count} work phase(s); {propertyDefs.Count} property definition(s) " +
                          $"({created} new); {valueDefs.Count} allowed value(s)" +
                          (skipped > 0 ? $"; {skipped} propert(ies) skipped — subject type not in this file" : ""));
        Console.WriteLine("  0 NarrativePropertyValues written (assignment is authorial, by design)");
    }

    public IEnumerable<PlanIntegrity.Violation> ExtraChecks(AppDbContext ctx, JsonElement config)
    {
        var violations = new List<PlanIntegrity.Violation>();

        violations.AddRange(PlanIntegrity.CompareRowCounts(
            _rowCountsBefore,
            PlanIntegrity.SnapshotRowCounts(ctx),
            allowedToChange: new HashSet<string>
            {
                "WorkPhases", "NarrativePropertyDefinitions", "NarrativePropertyValueDefinitions"
                // NarrativePropertyValues is deliberately absent: if this op ever grows a code path
                // that assigns a value, the row-count check turns it into a hard failure.
            }));

        foreach (var dup in ctx.WorkPhases.AsEnumerable().GroupBy(p => p.Name).Where(g => g.Count() > 1))
            violations.Add(new PlanIntegrity.Violation("workphase.duplicate_name", dup.Key));

        foreach (var dup in ctx.NarrativePropertyDefinitions.AsEnumerable()
                     .GroupBy(p => (p.OwnerType, p.SubjectDefinitionId, p.Name)).Where(g => g.Count() > 1))
            violations.Add(new PlanIntegrity.Violation("narrativepropertydefinition.duplicate_name",
                $"{dup.Key.OwnerType} subjectDef:{dup.Key.SubjectDefinitionId} \"{dup.Key.Name}\""));

        foreach (var dup in ctx.NarrativePropertyValueDefinitions.AsEnumerable()
                     .GroupBy(v => (v.NarrativePropertyDefinitionId, v.ValueName)).Where(g => g.Count() > 1))
            violations.Add(new PlanIntegrity.Violation("narrativepropertyvaluedefinition.duplicate_name",
                $"property:{dup.Key.NarrativePropertyDefinitionId} \"{dup.Key.ValueName}\""));

        return violations;
    }
}
