using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.DataOps;

/// <summary>
/// The safety procedure shared by every one-time <see cref="IDataOperation"/>: refuse unless the
/// file is at a known migration state, back up, migrate, transform inside a transaction, verify,
/// commit or roll back, and (only on a real apply) re-verify against the committed file with a
/// fresh connection. Written once here so future one-time jobs don't re-derive or copy-paste it.
/// </summary>
public static class DataOpEnvelope
{
    public static async Task<int> RunAsync(IDataOperation op, string dbPath, JsonElement config, bool apply)
    {
        if (!File.Exists(dbPath))
        {
            Console.Error.WriteLine($"Refusing: file not found: {dbPath}");
            return 2;
        }

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        await using (var ctx = new AppDbContext(options))
        {
            var all = ctx.Database.GetMigrations().ToList();
            var pending = (await ctx.Database.GetPendingMigrationsAsync()).ToList();

            if (pending.Count > 1 || (pending.Count == 1 && pending.Single() != all.Last()))
            {
                Console.Error.WriteLine(
                    $"Refusing: file is not at migration head or head-1. " +
                    $"Pending: [{string.Join(", ", pending)}]. Newest known: {all.Last()}.");
                return 2;
            }

            Console.WriteLine($"[{op.Name}] {(apply ? "APPLY" : "DRY RUN")} — {dbPath}");

            if (!StoryService.CreateSafetyBackup(dbPath))
            {
                Console.Error.WriteLine("Refusing: safety backup failed — not touching the file without one.");
                return 2;
            }
            Console.WriteLine("  backed up (see Backups/ next to the file)");

            var hadPendingMigration = pending.Count == 1;
            await ctx.Database.MigrateAsync();
            if (hadPendingMigration)
                Console.WriteLine($"  schema migrated to head ({all.Last()}) — permanent, independent of --apply");

            var rowCountsBefore = PlanIntegrity.SnapshotRowCounts(ctx);
            var noteChecksumBefore = PlanIntegrity.ComputeNoteChecksum(ctx);

            await using var transaction = await ctx.Database.BeginTransactionAsync();

            try
            {
                await op.Apply(ctx, config);
                await ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.Error.WriteLine($"  op.Apply threw — rolled back. {ex.GetType().Name}: {ex.Message}");
                return 1;
            }

            var violations = new List<PlanIntegrity.Violation>();
            violations.AddRange(PlanIntegrity.Check(ctx));
            violations.AddRange(op.ExtraChecks(ctx, config));

            var noteChecksumAfter = PlanIntegrity.ComputeNoteChecksum(ctx);
            if (noteChecksumAfter != noteChecksumBefore)
                violations.Add(new PlanIntegrity.Violation(
                    "notes.checksum_changed",
                    "note content changed — a structural DataOps operation must never touch notes"));

            var rowCountsAfter = PlanIntegrity.SnapshotRowCounts(ctx);

            PrintRowCountReport(rowCountsBefore, rowCountsAfter);

            if (violations.Count > 0)
            {
                await transaction.RollbackAsync();
                Console.Error.WriteLine($"  {violations.Count} violation(s) — rolled back, nothing written:");
                foreach (var v in violations)
                    Console.Error.WriteLine($"    [{v.Rule}] {v.Detail}");
                return 1;
            }

            if (!apply)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("  0 violations. DRY RUN — nothing written. Re-run with --apply to commit.");
                return 0;
            }

            await transaction.CommitAsync();
            Console.WriteLine("  0 violations. Committed.");

            // Re-verify against the committed file with a fresh connection — an independent
            // confirmation, not a repeat of the in-transaction checks above.
            await using var verifyCtx = new AppDbContext(options);
            var postCommitViolations = PlanIntegrity.Check(verifyCtx).ToList();
            var postCommitChecksum = PlanIntegrity.ComputeNoteChecksum(verifyCtx);

            if (postCommitViolations.Count > 0 || postCommitChecksum != noteChecksumBefore)
            {
                Console.Error.WriteLine(
                    "  POST-COMMIT VERIFICATION FAILED — the file was already written. " +
                    "Restore from Backups/ next to the file.");
                foreach (var v in postCommitViolations)
                    Console.Error.WriteLine($"    [{v.Rule}] {v.Detail}");
                if (postCommitChecksum != noteChecksumBefore)
                    Console.Error.WriteLine("    [notes.checksum_changed] confirmed on the committed file");
                return 1;
            }

            Console.WriteLine("  post-commit verification passed.");
            return 0;
        }
    }

    private static void PrintRowCountReport(
        IReadOnlyDictionary<string, long> before, IReadOnlyDictionary<string, long> after)
    {
        Console.WriteLine("  row counts (table: before -> after):");
        foreach (var table in before.Keys.OrderBy(t => t))
        {
            var b = before[table];
            var a = after.GetValueOrDefault(table);
            var marker = b == a ? "" : "  <-- changed";
            Console.WriteLine($"    {table}: {b} -> {a}{marker}");
        }
    }
}
