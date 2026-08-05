using Microsoft.Data.Sqlite;

namespace StoryPlanner.SourceTexts;

/// <summary>
/// The Work/Part skeleton read out of a .storyplan, read-only.
///
/// This tool never writes to a .storyplan — adding or renaming a Part is authorial work done
/// through the app or the seed-source-material DataOps op. The connection is opened
/// Mode=ReadOnly so that even a bug could not change one, and no AppDbContext is constructed,
/// so nothing here can trigger a migration.
/// </summary>
public sealed class PlanSpine
{
    public sealed record Work(int Id, string Name, string PartNoun, IReadOnlyList<Part> Parts);
    public sealed record Part(int Id, string Code, string Name, int OrderIndex);

    public required IReadOnlyList<Work> Works { get; init; }

    public Work? FindWork(string name) =>
        Works.FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public static PlanSpine Load(string storyplanPath)
    {
        if (!File.Exists(storyplanPath))
            throw new FileNotFoundException($"No .storyplan at {storyplanPath}");

        var connString = new SqliteConnectionStringBuilder
        {
            DataSource = storyplanPath,
            Mode = SqliteOpenMode.ReadOnly,
            Pooling = false
        }.ToString();

        using var conn = new SqliteConnection(connString);
        conn.Open();

        var parts = new List<(int WorkId, Part Part)>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText =
                "SELECT Id, SourceMaterialId, Code, Name, OrderIndex FROM SourceMaterialParts ORDER BY OrderIndex, Id";
            using var r = cmd.ExecuteReader();
            while (r.Read())
                parts.Add((r.GetInt32(1), new Part(r.GetInt32(0), r.GetString(2), r.GetString(3), r.GetInt32(4))));
        }

        var works = new List<Work>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT Id, Name, PartNoun FROM SourceMaterials ORDER BY OrderIndex, Id";
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var id = r.GetInt32(0);
                works.Add(new Work(id, r.GetString(1), r.GetString(2),
                    parts.Where(p => p.WorkId == id).Select(p => p.Part).ToList()));
            }
        }

        return new PlanSpine { Works = works };
    }
}
