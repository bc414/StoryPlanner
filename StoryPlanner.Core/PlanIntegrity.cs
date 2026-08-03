using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

/// <summary>
/// Id-based referential-integrity checks over an <see cref="AppDbContext"/>. The schema
/// deliberately has no foreign keys or indexes (see CLAUDE.md), so nothing but application code
/// prevents an orphaned row — today that's <c>ContentDeleter</c>'s guards, written ad hoc against
/// view models. This class is the same assumptions expressed once, against ids, so they can run
/// both around a <see cref="StoryPlanner.DataOps"/> operation and directly in tests — the
/// extraction the testing skill's "Known gap" note asks for.
///
/// These checks report what is — they never repair anything.
/// </summary>
public static class PlanIntegrity
{
    public readonly record struct Violation(string Rule, string Detail);

    /// <summary>SQLite's own structural check — corrupt pages, broken indexes, etc.</summary>
    public static string SqliteIntegrityCheck(AppDbContext ctx) =>
        ctx.Database.SqlQueryRaw<string>("PRAGMA integrity_check").AsEnumerable().First();

    /// <summary>Row count for every table EF knows about, keyed by table name.</summary>
    public static Dictionary<string, long> SnapshotRowCounts(AppDbContext ctx)
    {
        var counts = new Dictionary<string, long>();
        foreach (var entityType in ctx.Model.GetEntityTypes())
        {
            var table = entityType.GetTableName();
            if (table is null || counts.ContainsKey(table)) continue;
            var count = ctx.Database
                .SqlQueryRaw<long>($"SELECT COUNT(*) AS \"Value\" FROM \"{table}\"")
                .AsEnumerable().First();
            counts[table] = count;
        }
        return counts;
    }

    /// <summary>
    /// SHA-256 over every note's identity-and-content fields, ordered by Id so it's
    /// reproducible regardless of query/enumeration order. Used to prove note content is
    /// byte-for-byte untouched by an operation that has no business touching notes.
    /// </summary>
    public static string ComputeNoteChecksum(AppDbContext ctx)
    {
        var lines = ctx.Notes
            .OrderBy(n => n.Id)
            .Select(n => $"{n.Id}␟{(int)n.OwnerType}␟{n.OwnerId}␟{(int)n.NoteState}␟{n.Content}␟{n.FlagReason}")
            .ToList();
        var joined = string.Join("\n", lines);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(joined)));
    }

    /// <summary>
    /// Referential-integrity checks generic to any operation: every polymorphic owner reference,
    /// every link endpoint, and every narrative-property-value's chain to its owner must resolve
    /// to a live row.
    /// </summary>
    public static IReadOnlyList<Violation> Check(AppDbContext ctx)
    {
        var violations = new List<Violation>();

        var integrity = SqliteIntegrityCheck(ctx);
        if (integrity != "ok")
            violations.Add(new Violation("sqlite.integrity_check", integrity));

        var subjectIds = ctx.Subjects.Select(s => s.Id).ToHashSet();
        var plotPointIds = ctx.PlotPoints.Select(p => p.Id).ToHashSet();
        var chapterIds = ctx.Chapters.Select(c => c.Id).ToHashSet();
        var linkIds = ctx.PlotPointSubjectLinks.Select(l => l.Id).ToHashSet();

        bool ResolvesOwner(OwnerType ownerType, int ownerId) => ownerType switch
        {
            OwnerType.Subject => subjectIds.Contains(ownerId),
            OwnerType.PlotPoint => plotPointIds.Contains(ownerId),
            OwnerType.Chapter => chapterIds.Contains(ownerId),
            OwnerType.PlotPointSubjectLink => linkIds.Contains(ownerId),
            _ => false
        };

        var trackDefinitionIds = ctx.NoteTrackDefinitions.Select(t => t.Id).ToHashSet();
        var themeIds = ctx.Themes.Select(t => t.Id).ToHashSet();

        foreach (var n in ctx.Notes)
        {
            if (!ResolvesOwner(n.OwnerType, n.OwnerId))
                violations.Add(new Violation("note.owner_missing", $"note:{n.Id} ({n.OwnerType}:{n.OwnerId})"));

            // Null is legal ("Unassigned" track / untagged); a non-null id must resolve. A note
            // whose track row vanished silently demotes to Unassigned — and for a condition track
            // its date semantics flip, since event-vs-condition lives on the track.
            if (n.NoteTrackDefinitionId is int trackId && !trackDefinitionIds.Contains(trackId))
                violations.Add(new Violation("note.track_missing", $"note:{n.Id} -> track:{trackId}"));

            if (n.ThemeId is int themeId && !themeIds.Contains(themeId))
                violations.Add(new Violation("note.theme_missing", $"note:{n.Id} -> theme:{themeId}"));
        }

        // Type Object scoping: subjects and definitions hang off SubjectDefinition rows. Id 0 is
        // tolerated as an unscoped placeholder (the add-command's fallback), so only a non-zero id
        // that fails to resolve is an orphan.
        var subjectDefinitionIds = ctx.SubjectDefinitions.Select(d => d.Id).ToHashSet();

        foreach (var s in ctx.Subjects)
            if (s.SubjectDefinitionId != 0 && !subjectDefinitionIds.Contains(s.SubjectDefinitionId))
                violations.Add(new Violation("subject.definition_missing", $"subject:{s.Id} -> subjectDef:{s.SubjectDefinitionId}"));

        foreach (var t in ctx.NoteTrackDefinitions)
            if (t.SubjectDefinitionId != 0 && !subjectDefinitionIds.Contains(t.SubjectDefinitionId))
                violations.Add(new Violation("trackdefinition.subjectdefinition_missing",
                    $"track:{t.Id} -> subjectDef:{t.SubjectDefinitionId}"));

        foreach (var p in ctx.NarrativePropertyDefinitions)
            if (p.SubjectDefinitionId != 0 && !subjectDefinitionIds.Contains(p.SubjectDefinitionId))
                violations.Add(new Violation("propertydefinition.subjectdefinition_missing",
                    $"property:{p.Id} -> subjectDef:{p.SubjectDefinitionId}"));

        // Sentinel-0 references ("(Unplaced)" / "(Unassigned)") are legal, permanent states —
        // only a non-zero id pointing at a deleted row is an orphan.
        var theaterIds = ctx.Theaters.Select(t => t.Id).ToHashSet();
        var storyIds = ctx.Stories.Select(s => s.Id).ToHashSet();

        foreach (var s in ctx.Subjects)
            if (s.TheaterId != 0 && !theaterIds.Contains(s.TheaterId))
                violations.Add(new Violation("subject.theater_missing", $"subject:{s.Id} -> theater:{s.TheaterId}"));

        foreach (var p in ctx.PlotPoints)
            if (p.TheaterId != 0 && !theaterIds.Contains(p.TheaterId))
                violations.Add(new Violation("plotpoint.theater_missing", $"plotpoint:{p.Id} -> theater:{p.TheaterId}"));

        foreach (var c in ctx.Chapters)
            if (c.StoryId != 0 && !storyIds.Contains(c.StoryId))
                violations.Add(new Violation("chapter.story_missing", $"chapter:{c.Id} -> story:{c.StoryId}"));

        var conversationIds = ctx.Conversations.Select(c => c.Id).ToHashSet();
        foreach (var b in ctx.ConversationBlocks)
            if (!conversationIds.Contains(b.ConversationId))
                violations.Add(new Violation("conversationblock.conversation_missing",
                    $"block:{b.Id} -> conversation:{b.ConversationId}"));

        foreach (var pp in ctx.PlotPoints)
        {
            if (pp.ChapterId is int chId && !chapterIds.Contains(chId))
                violations.Add(new Violation("plotpoint.chapter_missing", $"plotpoint:{pp.Id} -> chapter:{chId}"));
            if (pp.FocalCharacterId is int focalId && !subjectIds.Contains(focalId))
                violations.Add(new Violation("plotpoint.focal_character_missing", $"plotpoint:{pp.Id} -> subject:{focalId}"));
        }

        foreach (var l in ctx.PlotPointSubjectLinks)
        {
            if (!plotPointIds.Contains(l.PlotPointId))
                violations.Add(new Violation("link.plotpoint_missing", $"link:{l.Id} -> plotpoint:{l.PlotPointId}"));
            if (!subjectIds.Contains(l.SubjectId))
                violations.Add(new Violation("link.subject_missing", $"link:{l.Id} -> subject:{l.SubjectId}"));
        }

        // Source material coverage: Works, Parts (Work/Part is a real two-tier parent-child
        // relationship even though the schema has no FK for it), and per-note citations.
        var noteIds = ctx.Notes.Select(n => n.Id).ToHashSet();
        var sourceMaterialIds = ctx.SourceMaterials.Select(s => s.Id).ToHashSet();
        var partParentBySourceMaterialPartId = ctx.SourceMaterialParts.ToDictionary(p => p.Id, p => p.SourceMaterialId);

        foreach (var p in ctx.SourceMaterialParts)
        {
            if (!sourceMaterialIds.Contains(p.SourceMaterialId))
                violations.Add(new Violation("sourcepart.material_missing", $"part:{p.Id} -> material:{p.SourceMaterialId}"));
        }

        foreach (var r in ctx.NoteSourceReferences)
        {
            if (!noteIds.Contains(r.NoteId))
                violations.Add(new Violation("sourcereference.note_missing", $"reference:{r.Id} -> note:{r.NoteId}"));
            if (!sourceMaterialIds.Contains(r.SourceMaterialId))
                violations.Add(new Violation("sourcereference.material_missing", $"reference:{r.Id} -> material:{r.SourceMaterialId}"));

            if (r.SourceMaterialPartId is int partId)
            {
                if (!partParentBySourceMaterialPartId.TryGetValue(partId, out var partParent))
                    violations.Add(new Violation("sourcereference.part_missing", $"reference:{r.Id} -> part:{partId}"));
                else if (partParent != r.SourceMaterialId)
                    violations.Add(new Violation("sourcereference.part_parent_mismatch",
                        $"reference:{r.Id} cites material:{r.SourceMaterialId} but part:{partId} belongs to material:{partParent}"));
            }
        }

        // NarrativePropertyValue has no OwnerType of its own — resolve it by tracing
        // ValueDefinitionId -> NarrativePropertyDefinitionId -> OwnerType, mirroring
        // ContentDeleter.RemoveOwnedNarrativePropertyValues.
        var propertyByValueDefId = ctx.NarrativePropertyValueDefinitions
            .Join(ctx.NarrativePropertyDefinitions,
                vd => vd.NarrativePropertyDefinitionId, pd => pd.Id,
                (vd, pd) => new { vd.Id, pd.OwnerType, PropertyDefinitionId = pd.Id })
            .ToDictionary(x => x.Id, x => (x.OwnerType, x.PropertyDefinitionId));

        // Assignments that survive the definition lookup, keyed for the single-select check below.
        var resolved = new List<(int ValueId, OwnerType OwnerType, int OwnerId, int PropertyDefinitionId)>();

        foreach (var v in ctx.NarrativePropertyValues)
        {
            if (!propertyByValueDefId.TryGetValue(v.ValueDefinitionId, out var owner))
            {
                violations.Add(new Violation("narrativevalue.definition_missing", $"value:{v.Id} -> valueDef:{v.ValueDefinitionId}"));
                continue;
            }
            if (!ResolvesOwner(owner.OwnerType, v.OwnerId))
                violations.Add(new Violation("narrativevalue.owner_missing", $"value:{v.Id} ({owner.OwnerType}:{v.OwnerId})"));

            resolved.Add((v.Id, owner.OwnerType, v.OwnerId, owner.PropertyDefinitionId));
        }

        // Narrative properties are SINGLE-SELECT: at most one value per (owner, property). The
        // schema cannot say so — no unique constraint, no FKs, no unit of work — so this check is
        // the enforcement, the same role ContentDeleter plays for referential integrity. Note the
        // key includes OwnerType: without it, subject 7 and chapter 7 collide.
        foreach (var dup in resolved
                     .GroupBy(r => (r.OwnerType, r.OwnerId, r.PropertyDefinitionId))
                     .Where(g => g.Count() > 1))
            violations.Add(new Violation("narrativevalue.duplicate_for_property",
                $"{dup.Key.OwnerType}:{dup.Key.OwnerId} property:{dup.Key.PropertyDefinitionId} has " +
                $"{dup.Count()} values ({string.Join(", ", dup.Select(r => $"value:{r.ValueId}"))})"));

        var workPhaseIds = ctx.WorkPhases.Select(w => w.Id).ToHashSet();
        foreach (var pd in ctx.NarrativePropertyDefinitions)
            if (pd.GatingWorkPhaseId is int phaseId && !workPhaseIds.Contains(phaseId))
                violations.Add(new Violation("narrativepropertydefinition.workphase_missing",
                    $"property:{pd.Id} -> workPhase:{phaseId}"));

        return violations;
    }

    /// <summary>
    /// Row counts that changed between two snapshots, outside a set of tables the calling
    /// operation declares it's allowed to add/remove rows in.
    /// </summary>
    public static IReadOnlyList<Violation> CompareRowCounts(
        IReadOnlyDictionary<string, long> before,
        IReadOnlyDictionary<string, long> after,
        IReadOnlySet<string> allowedToChange)
    {
        var violations = new List<Violation>();
        foreach (var (table, beforeCount) in before)
        {
            if (allowedToChange.Contains(table)) continue;
            var afterCount = after.GetValueOrDefault(table);
            if (afterCount != beforeCount)
                violations.Add(new Violation("rowcount.changed", $"{table}: {beforeCount} -> {afterCount}"));
        }
        return violations;
    }
}
