using System.Text.Json;
using StoryPlanner.Core;

namespace StoryPlanner.DataOps;

/// <summary>
/// A single one-time structural operation, run inside <see cref="DataOpEnvelope"/>'s safety
/// procedure. See the scope note in the plan this shipped with: an op belongs here only when
/// the shape of the change is fully decided by Brian already (a reviewable config file), never
/// when running it requires making a categorization decision — that is story-content work, not
/// a mechanical operation, and does not belong in this tool.
/// </summary>
public interface IDataOperation
{
    string Name { get; }

    /// <summary>
    /// Mutate <paramref name="ctx"/> per <paramref name="config"/>. Runs inside a transaction
    /// the envelope owns — do not call SaveChanges expecting it to commit; the envelope decides
    /// commit vs. rollback after checking the result.
    /// </summary>
    Task Apply(AppDbContext ctx, JsonElement config);

    /// <summary>
    /// Job-specific invariants beyond <see cref="PlanIntegrity.Check"/>, evaluated against
    /// <paramref name="ctx"/> after <see cref="Apply"/> returns. Default: none.
    /// </summary>
    IEnumerable<PlanIntegrity.Violation> ExtraChecks(AppDbContext ctx, JsonElement config)
        => Enumerable.Empty<PlanIntegrity.Violation>();
}
