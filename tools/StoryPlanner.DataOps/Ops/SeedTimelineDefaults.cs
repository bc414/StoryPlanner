using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.DataOps;

/// <summary>
/// Creates the timeline's Theater rows and Pivot rows from a config Brian has reviewed
/// (configs/timeline-defaults.v2.json). Does NOT assign any subject or plot point to a
/// theater — that mapping is categorization (Brian's authorial work, done in the app or via a
/// future author-written config), so everything stays at the "(Unplaced)" sentinel until he
/// moves it. Idempotent: theaters match by name, pivots by year; re-runs update ordering only
/// and never duplicate. Names and descriptions are stamped ONLY on row creation — they are
/// Brian's framing of his fictional world, so a re-run must be incapable of clobbering an
/// in-app rewrite (CLAUDE.md, "Seeders seed structure, never prose").
/// </summary>
public sealed class SeedTimelineDefaults : IDataOperation
{
    public string Name => "seed-timeline-defaults";

    private Dictionary<string, long> _rowCountsBefore = new();

    public async Task Apply(AppDbContext ctx, JsonElement config)
    {
        _rowCountsBefore = PlanIntegrity.SnapshotRowCounts(ctx);

        var theaters = await ctx.Theaters.ToListAsync();
        if (config.TryGetProperty("theaters", out var theaterArray))
        {
            foreach (var e in theaterArray.EnumerateArray())
            {
                var name = e.GetProperty("name").GetString() ?? "";
                var existing = theaters.FirstOrDefault(t => t.Name == name);
                if (existing is null)
                {
                    existing = new Theater
                    {
                        Name = name,
                        Description = e.TryGetProperty("description", out var d) ? d.GetString() ?? "" : ""
                    };
                    ctx.Theaters.Add(existing);
                    theaters.Add(existing);
                }
                existing.OrderIndex = e.GetProperty("orderIndex").GetInt32(); // structure, not prose
            }
        }

        var pivots = await ctx.Pivots.ToListAsync();
        if (config.TryGetProperty("pivots", out var pivotArray))
        {
            foreach (var e in pivotArray.EnumerateArray())
            {
                var year = e.GetProperty("year").GetInt32();
                var existing = pivots.FirstOrDefault(p => p.Year == year);
                if (existing is null)
                {
                    // Name and description only at creation — see the class doc.
                    var pivot = new Pivot
                    {
                        Year = year,
                        Name = e.GetProperty("name").GetString() ?? "",
                        Description = e.TryGetProperty("description", out var d) ? d.GetString() ?? "" : ""
                    };
                    ctx.Pivots.Add(pivot);
                    pivots.Add(pivot);
                }
            }
        }
    }

    public IEnumerable<PlanIntegrity.Violation> ExtraChecks(AppDbContext ctx, JsonElement config)
    {
        var violations = new List<PlanIntegrity.Violation>();

        violations.AddRange(PlanIntegrity.CompareRowCounts(
            _rowCountsBefore,
            PlanIntegrity.SnapshotRowCounts(ctx),
            allowedToChange: new HashSet<string> { "Theaters", "Pivots" }));

        foreach (var dup in ctx.Theaters.AsEnumerable().GroupBy(t => t.Name).Where(g => g.Count() > 1))
            violations.Add(new PlanIntegrity.Violation("theater.duplicate_name", dup.Key));
        foreach (var dup in ctx.Pivots.AsEnumerable().GroupBy(p => p.Year).Where(g => g.Count() > 1))
            violations.Add(new PlanIntegrity.Violation("pivot.duplicate_year", dup.Key.ToString()));

        return violations;
    }
}
