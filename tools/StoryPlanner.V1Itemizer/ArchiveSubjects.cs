using System.Text;
using Microsoft.Data.Sqlite;
using StoryPlanner.BatchFiles;

namespace StoryPlanner.V1Itemizer;

/// <summary>
/// The subject cut over the v1 archive .storyplan: one item per subject that owns at least one
/// note, carrying its name, its triage label (the archive's subject type, which is the 2026 review's
/// workflow queue rather than a kind of subject — the storyplan-data skill), its description and
/// its subject-owned notes in their order, each under its note id. Read raw with Mode=ReadOnly,
/// never through the app's service, which migrates in place. Link notes and plot-point notes are
/// not a subject's notes and are not carried.
/// </summary>
public static class ArchiveSubjects
{
    public sealed record Subject(int Id, string Name, string TriageLabel, string Description);
    public sealed record Note(int Id, int SubjectId, int SortOrder, string Content);

    public const int OwnerSubject = 0;

    public const string LocatorNotation = "`subject-<id>`: the Subjects row id in the v1 archive .storyplan the config names";
    public const string Narrowing = "the subjects that own at least one note";

    public static (IReadOnlyList<Subject> Subjects, IReadOnlyList<Note> Notes) Load(string path)
    {
        var cs = new SqliteConnectionStringBuilder { DataSource = path, Mode = SqliteOpenMode.ReadOnly }.ToString();
        using var conn = new SqliteConnection(cs);
        conn.Open();
        var subjects = new List<Subject>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "select s.Id, s.Name, coalesce(d.SubjectType, ''), s.Description from Subjects s left join SubjectDefinitions d on d.Id = s.SubjectDefinitionId";
            using var r = cmd.ExecuteReader();
            while (r.Read()) subjects.Add(new Subject(r.GetInt32(0), r.GetString(1), r.GetString(2), r.GetString(3)));
        }
        var notes = new List<Note>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = $"select Id, OwnerId, SortOrder, Content from Notes where OwnerType = {OwnerSubject}";
            using var r = cmd.ExecuteReader();
            while (r.Read()) notes.Add(new Note(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3)));
        }
        return (subjects, notes);
    }

    /// <summary>One item per subject with notes, in subject id order; the notes by their sort order, then id.</summary>
    public static IReadOnlyList<ItemizerOutput.Item> Cut(IReadOnlyList<Subject> subjects, IReadOnlyList<Note> notes)
    {
        var bySubject = notes.ToLookup(n => n.SubjectId);
        var items = new List<ItemizerOutput.Item>();
        foreach (var s in subjects.OrderBy(s => s.Id))
        {
            var own = bySubject[s.Id].OrderBy(n => n.SortOrder).ThenBy(n => n.Id).ToList();
            if (own.Count == 0) continue;
            var body = new StringBuilder()
                .Append("# v1 archive subject: ").Append(s.Name).Append('\n')
                .Append("triage label: ").Append(s.TriageLabel.Length == 0 ? "(none)" : s.TriageLabel).Append("\n\n");
            var description = s.Description.Replace("\r\n", "\n").Trim();
            if (description.Length > 0) body.Append("## Description\n\n").Append(description).Append("\n\n");
            body.Append("## Notes\n\n");
            foreach (var n in own)
                body.Append("### note ").Append(n.Id).Append("\n\n").Append(n.Content.Replace("\r\n", "\n").Trim()).Append("\n\n");
            items.Add(new ItemizerOutput.Item($"subject-{s.Id}", body.ToString().TrimEnd('\n') + "\n", $"subject-{s.Id}",
                $"{s.Name} ({s.TriageLabel}), {own.Count} notes"));
        }
        return items;
    }
}
