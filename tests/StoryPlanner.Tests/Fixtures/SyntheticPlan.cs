using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;

using StoryPlanner.Mcp;

namespace StoryPlanner.Tests;

/// <summary>
/// Builds a small synthetic .storyplan file in a temp directory and disposes it on teardown.
/// NEVER touches Brian's real files — every test owns its own database.
///
/// The fixture is deliberately shaped to exercise the invariants that matter:
/// one subject with both visible and flagged notes, one flagged note whose FlagReason
/// carries text that must never leak into ordinary results, all four OwnerType values,
/// a chapter → plot point → link chain, and a spread of WorldDate formats including an
/// unparseable one.
/// </summary>
public sealed class SyntheticPlan : IDisposable
{
    public string Path { get; }
    public StoryPlanSources Sources { get; }

    // Well-known ids the tests assert against.
    public const int CharacterDefId = 1;
    public const int SubjectId = 1;          // "Testcharacter", 2 visible + 2 flagged notes
    public const int EmptySubjectId = 2;     // no notes at all
    public const int ChapterId = 1;                 // StoryId 0 — "(Unassigned)", the default/legacy state
    public const int StoryId = 1;
    public const int SecondChapterId = 2;            // belongs to StoryId, exercises story-grouped output
    public const int PlotPointId = 1;
    public const int LinkId = 1;
    public const int BackstoryTrackId = 1;
    public const int LinkTrackId = 2;

    public const int VisibleNoteId = 1;
    public const int FlaggedNoteId = 2;      // FlagReason contains FlaggedReasonSecret
    public const int FlaggedNoteId2 = 3;
    public const int PlotPointNoteId = 4;
    public const int ChapterNoteId = 5;
    public const int LinkNoteId = 6;
    public const int UnparseableDateNoteId = 7;

    /// <summary>Appears ONLY in a flagged note's Content — must never surface in ordinary tools.</summary>
    public const string FlaggedContentSecret = "ZZFLAGGEDCONTENT";

    /// <summary>Appears ONLY in a flagged note's FlagReason — must never surface in ordinary tools.</summary>
    public const string FlaggedReasonSecret = "ZZFLAGGEDREASON";

    /// <summary>Appears only in a normal, retrievable note.</summary>
    public const string VisibleSecret = "ZZVISIBLE";

    // Envelope text surrounding the secrets. Search output legitimately echoes the caller's
    // own pattern back (so the suggested follow-up call is copy-pasteable), so "the secret
    // never appears" is the wrong assertion. These phrases are what a leaked *snippet* would
    // drag along, and they are never a search term — asserting on them proves no note body escaped.
    public const string FlaggedContentEnvelope = "An unstable claim containing";
    public const string FlaggedReasonEnvelope = "Is this still true?";

    private SyntheticPlan(string path, StoryPlanSources sources)
    {
        Path = path;
        Sources = sources;
    }

    public static SyntheticPlan Create(bool archiveSemantics = false)
    {
        var dir = Directory.CreateTempSubdirectory("storyplan-tests-");
        var file = System.IO.Path.Combine(dir.FullName, "synthetic.storyplan");

        Seed(file, archiveSemantics);

        // The server always opens both corpora; point them at the same file when a test
        // only cares about one, then read whichever Corpus it needs.
        var sources = new StoryPlanSources(file, file);
        sources.LoadAll();
        return new SyntheticPlan(file, sources);
    }

    private static void Seed(string file, bool archiveSemantics)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={file}")
            .Options;

        using var ctx = new AppDbContext(options);
        ctx.Database.Migrate();

        ctx.SubjectDefinitions.Add(new SubjectDefinition
        {
            Id = CharacterDefId, SubjectType = "Character", DisplayOrder = 0
        });

        ctx.NoteTrackDefinitions.AddRange(
            new NoteTrackDefinition
            {
                Id = BackstoryTrackId,
                SubjectDefinitionId = CharacterDefId,
                OwnerType = OwnerType.Subject,
                TrackName = "Backstory",
                DisplayQuestion = "What is this character's history?",
                UsageDirective = "Revelations should draw from here.",
                AuditDirective = "All backstory should have a reveal.",
                TrackType = TrackType.History,
                SupportsWorldDate = true,
                ExpansionModeDisplayOrder = 1
            },
            new NoteTrackDefinition
            {
                Id = LinkTrackId,
                SubjectDefinitionId = CharacterDefId,
                OwnerType = OwnerType.PlotPointSubjectLink,
                TrackName = "Revelation",
                DisplayQuestion = "What does the reader learn here?",
                TrackType = TrackType.WorldInference,
                ExpansionModeDisplayOrder = 1
            });

        ctx.Subjects.AddRange(
            new Subject { Id = SubjectId, Name = "Testcharacter", SubjectDefinitionId = CharacterDefId },
            new Subject { Id = EmptySubjectId, Name = "Lonelysubject", SubjectDefinitionId = CharacterDefId });

        ctx.Chapters.Add(new Chapter { Id = ChapterId, Title = "Testchapter", OrderIndex = 1 }); // StoryId defaults to 0 (Unassigned)

        // A second story + chapter — otherwise nothing exercises story-grouped inventories,
        // list_stories, or the "(Unassigned)" grouping actually having company.
        ctx.Stories.Add(new Story { Id = StoryId, Title = "Test Story", Abbreviation = "TS", ColorHex = "#123456", OrderIndex = 1 });
        ctx.Chapters.Add(new Chapter { Id = SecondChapterId, Title = "Story chapter", StoryId = StoryId, OrderIndex = 1 });

        ctx.PlotPoints.Add(new PlotPoint { Id = PlotPointId, Title = "Testscene", ChapterId = ChapterId, OrderInChapter = 1 });
        ctx.PlotPointSubjectLinks.Add(new PlotPointSubjectLink { Id = LinkId, PlotPointId = PlotPointId, SubjectId = SubjectId });

        // In v2 semantics Confirmed = stable. In v1 archive semantics Confirmed = review-closed.
        // The same enum value is written either way; only the label should differ.
        var confirmedState = NoteState.Confirmed;

        ctx.Notes.AddRange(
            new Note
            {
                Id = VisibleNoteId, OwnerId = SubjectId, OwnerType = OwnerType.Subject,
                NoteTrackDefinitionId = BackstoryTrackId, NoteState = NoteState.Unset,
                Content = $"A perfectly ordinary retrievable note containing {VisibleSecret}.",
                WorldDate = "993", SortOrder = 1
            },
            new Note
            {
                Id = FlaggedNoteId, OwnerId = SubjectId, OwnerType = OwnerType.Subject,
                NoteTrackDefinitionId = BackstoryTrackId, NoteState = NoteState.Flagged,
                Content = $"An unstable claim containing {FlaggedContentSecret}.",
                FlagReason = $"Is this still true? {FlaggedReasonSecret}",
                WorldDate = "1002", SortOrder = 2
            },
            new Note
            {
                Id = FlaggedNoteId2, OwnerId = SubjectId, OwnerType = OwnerType.Subject,
                NoteTrackDefinitionId = BackstoryTrackId, NoteState = NoteState.Flagged,
                Content = "", // pure-question flagged note: empty content, reason only
                FlagReason = "Pure question with no content body.", SortOrder = 3
            },
            new Note
            {
                Id = PlotPointNoteId, OwnerId = PlotPointId, OwnerType = OwnerType.PlotPoint,
                NoteState = confirmedState, Content = "A plot point note.", SortOrder = 1
            },
            new Note
            {
                Id = ChapterNoteId, OwnerId = ChapterId, OwnerType = OwnerType.Chapter,
                NoteState = NoteState.Unset, Content = "A chapter note.", SortOrder = 1
            },
            new Note
            {
                Id = LinkNoteId, OwnerId = LinkId, OwnerType = OwnerType.PlotPointSubjectLink,
                NoteTrackDefinitionId = LinkTrackId, NoteState = NoteState.Unset,
                Content = "A link note — what this scene does to this subject.", SortOrder = 1
            },
            new Note
            {
                Id = UnparseableDateNoteId, OwnerId = SubjectId, OwnerType = OwnerType.Subject,
                NoteTrackDefinitionId = BackstoryTrackId, NoteState = NoteState.Unset,
                Content = "Note with a WorldDate that cannot be parsed.",
                WorldDate = "sometime after the war", SortOrder = 4
            });

        ctx.SaveChanges();
    }

    /// <summary>
    /// Opens the fixture through the REAL <see cref="StoryService"/> — not a mock — so tests of
    /// <c>ExportResolver</c> / <c>NoteExportRenderer</c> exercise the same IStoryService the app
    /// uses, including its DbSet.Local-backed ObservableCollections.
    ///
    /// Caller disposes. SaveAsync() is safe here — it is a bare SaveChangesAsync (the .md /
    /// _stats.csv litter it used to write beside the file was removed 2026-08-02).
    /// </summary>
    public async Task<IStoryService> OpenStoryServiceAsync()
    {
        var svc = new StoryService();
        await svc.OpenProjectAsync(Path);
        return svc;
    }

    /// <summary>Commits a change from a separate connection, as the WPF app would.</summary>
    public void ExternalWrite(Action<AppDbContext> mutate)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={Path}")
            .Options;
        using var ctx = new AppDbContext(options);
        mutate(ctx);
        ctx.SaveChanges();
    }

    public void Dispose()
    {
        Sources.Dispose();
        try
        {
            var dir = System.IO.Path.GetDirectoryName(Path)!;
            Directory.Delete(dir, recursive: true);
        }
        catch (IOException)
        {
            // SQLite may still hold the file briefly on Windows; a temp dir left behind is harmless.
        }
    }
}
