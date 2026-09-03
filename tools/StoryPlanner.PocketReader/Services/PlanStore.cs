using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.PocketReader;

/// <summary>One of the two file slots: what was picked, when, and what came of opening it.</summary>
public sealed class PlanSlot
{
    public required Corpus Corpus { get; init; }
    public string? FileName { get; set; }
    public DateTime? PickedAtUtc { get; set; }
    public PlanCache? Cache { get; set; }
    /// <summary>Why the file is not usable (gate refusal, not SQLite, unreadable). Null when loaded or empty.</summary>
    public string? Error { get; set; }
    public string Detail { get; set; } = "";
    public bool IsLoaded => Cache is not null;
    public bool HasFile => FileName is not null;
}

/// <summary>
/// Holds the two optional plans for the life of the page. A picked file's bytes go to the
/// Emscripten in-memory filesystem (where SQLite opens them read-only) and to IndexedDB (so
/// the next visit restores them without re-picking). Never migrates: MigrationGate refuses
/// any file whose migration set differs from the reader's.
/// </summary>
public sealed class PlanStore
{
    private const long MaxFileBytes = 1024L * 1024 * 1024;
    private const string PlansDir = "/plans";

    public PlanSlot Working { get; } = new() { Corpus = Corpus.Working };
    public PlanSlot Archive { get; } = new() { Corpus = Corpus.Archive };

    public event Action? Changed;

    public PlanSlot Slot(Corpus c) => c == Corpus.Working ? Working : Archive;
    public PlanCache? Get(Corpus c) => Slot(c).Cache;
    public bool AnyLoaded => Working.IsLoaded || Archive.IsLoaded;

    public async Task InitializeAsync()
    {
        await Interop.InitializeAsync();
        foreach (var slot in new[] { Working, Archive })
        {
            var slug = Labels.CorpusSlug(slot.Corpus);
            var meta = await Interop.PrepareStoredPlan(slug);
            if (meta is null) continue;
            var bytes = Interop.TakeStoredPlan(slug);
            if (bytes is null) continue;

            var (name, pickedAt) = ParseMeta(meta);
            Load(slot, bytes, name, pickedAt);
        }
        Changed?.Invoke();
    }

    public async Task PickAsync(Corpus corpus, IBrowserFile file)
    {
        var slot = Slot(corpus);
        slot.Error = null;
        slot.Detail = "Reading file…";
        Changed?.Invoke();

        byte[] bytes;
        try
        {
            await using var stream = file.OpenReadStream(MaxFileBytes);
            using var ms = new MemoryStream((int)Math.Min(file.Size, int.MaxValue));
            await stream.CopyToAsync(ms);
            bytes = ms.ToArray();
        }
        catch (Exception ex)
        {
            slot.Error = $"Could not read the file: {ex.Message}";
            slot.Detail = "";
            Changed?.Invoke();
            return;
        }

        var pickedAt = DateTime.UtcNow;
        // Store first, so a gate refusal still survives a reload with its message.
        await Interop.SavePlan(Labels.CorpusSlug(corpus), file.Name, pickedAt.ToString("O"), bytes);
        Load(slot, bytes, file.Name, pickedAt);
        Changed?.Invoke();
    }

    public async Task ForgetAsync(Corpus corpus)
    {
        var slot = Slot(corpus);
        await Interop.RemovePlan(Labels.CorpusSlug(corpus));
        slot.Cache = null;
        slot.FileName = null;
        slot.PickedAtUtc = null;
        slot.Error = null;
        slot.Detail = "";
        var path = PathFor(corpus);
        if (File.Exists(path)) File.Delete(path);
        Changed?.Invoke();
    }

    private static string PathFor(Corpus c) => $"{PlansDir}/{Labels.CorpusSlug(c)}.storyplan";

    private static (string Name, DateTime PickedAt) ParseMeta(string json)
    {
        using var doc = System.Text.Json.JsonDocument.Parse(json);
        var name = doc.RootElement.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "";
        var pickedAt = doc.RootElement.TryGetProperty("pickedAt", out var p)
                       && DateTime.TryParse(p.GetString(), null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt)
            ? dt : DateTime.MinValue;
        return (name, pickedAt);
    }

    private static void Load(PlanSlot slot, byte[] bytes, string fileName, DateTime pickedAtUtc)
    {
        slot.FileName = fileName;
        slot.PickedAtUtc = pickedAtUtc;
        slot.Cache = null;
        slot.Error = null;
        slot.Detail = "";

        if (bytes.Length < 16 || System.Text.Encoding.ASCII.GetString(bytes, 0, 15) != "SQLite format 3")
        {
            slot.Error = "That is not a SQLite database, so not a .storyplan.";
            return;
        }

        var path = PathFor(slot.Corpus);
        try
        {
            Directory.CreateDirectory(PlansDir);
            File.WriteAllBytes(path, bytes);

            var connString = new SqliteConnectionStringBuilder
            {
                DataSource = path,
                Mode = SqliteOpenMode.ReadOnly,
                Pooling = false
            }.ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            using var ctx = new AppDbContext(options);

            // GetAppliedMigrations reads __EFMigrationsHistory (empty set if the table is absent);
            // GetMigrations lists the ids compiled into StoryPlanner.Core. Neither writes.
            var applied = ctx.Database.GetAppliedMigrations().ToList();
            var known = ctx.Database.GetMigrations().ToList();
            var (verdict, detail) = MigrationGate.Check(applied, known);
            if (verdict != MigrationVerdict.Compatible)
            {
                slot.Error = detail;
                return;
            }

            slot.Cache = PlanCache.Build(ctx, slot.Corpus, fileName, bytes.LongLength);
            slot.Detail = detail;
        }
        catch (Exception ex)
        {
            slot.Error = $"Could not open the file: {ex.GetType().Name}: {ex.Message}";
        }
    }
}
