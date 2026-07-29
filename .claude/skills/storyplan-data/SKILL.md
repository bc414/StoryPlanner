---
name: storyplan-data
description: >
  Inspect Brian's real StoryPlanner data (.storyplan files = SQLite databases) read-only to see
  how the app's features are ACTUALLY used in practice. Use when auditing features, planning a
  refactor/migration, checking whether a model/table/track is populated or orphaned, or answering
  "how is X used in my data" — instead of guessing from the C# models alone. Pairs with
  FEATURE-AUDIT.md, which mines design transcripts; this skill mines the live data.
---

# StoryPlanner data inspection

`.storyplan` files are SQLite databases written by this app's EF Core layer (`StoryPlanner.Core/AppDbContext.cs`).
The schema is the C# model — every table maps 1:1 to a class in `StoryPlanner.Core/Models/` or
`OtherModels/`. Querying the real data lets you confirm (or refute) claims about how the tool is
actually used, instead of inferring from code structure alone.

## The files

| File | ~Size | Role |
|------|-------|------|
| `C:\Users\Brian\Desktop\TLTT v2.storyplan` | 14 MB | **Current / live** working file — default target |
| `C:\Users\Brian\Desktop\TLTT v1 Archive.storyplan` | 5.4 MB | Orthogonal legacy dataset — see below, **not** an old copy of v2 |

Default to `TLTT v2.storyplan` unless the user names a different file or explicitly asks about the
v1 archive or a v1-vs-v2 comparison.

**V1 Archive is not "an older v2."** It's Story Planner V1's data, migrated into the *current*
(v2) schema so the same tables/columns exist, but the data itself was never reworked to match how
those columns are used in v2. Practically:

- **Almost no track assignment.** `NoteTrackDefinitions` has **0 rows** in the v1 file (tracks are
  a v2-era concept) — so every single `Note` (5,843 of 5,843, confirmed by query) has
  `NoteTrackDefinitionId IS NULL`. Don't read "0 tracks used" here as "nothing happened"; read it
  as "this file predates the track system entirely."
- **`NoteState.Confirmed` means something different.** In v2, `Confirmed` (2) means "stable,
  locked, safe for downstream work" (see `Models/Note.cs`). In the v1 archive, Brian used
  `Confirmed` as a review-closure marker: **"no need to look at this anymore."** That covers
  TWO dispositions the data does not distinguish: carried over into v2, OR deemed superseded
  and deliberately NOT carried over. So a v1 `Confirmed` count is NOT migration progress, and a
  v1 `Confirmed` note is neither necessarily in v2 nor necessarily current truth — it is just
  closed. Don't conflate the two meanings across files.
- **`SubjectDefinitions.SubjectType` is a workflow queue, not a category taxonomy.** In v2 it holds
  real subject-kind labels (`Character`, `Technology`, `Organization`, ...). In the v1 archive the
  same free-text column instead holds triage states like `"First Pass, subject notes only"`,
  `"Deferred for other reasons"`, `"Uncategorized"`, `"Deferred for Minette's Prequel"`, `"Complete"`
  — i.e. "where is this subject in the migration/review pipeline," not "what kind of subject is
  this." Same schema, unrelated meaning — always inspect the actual distinct values live rather
  than assuming a column's v2 meaning carries over.
- **Some legacy tables are only alive in v1.** `GeminiEntries` (278 rows) and `Ideas` (66 rows) are
  populated in the v1 archive but sit at 0 in current v2 — they're pre-Conversation-Reader features
  Brian used at the time, superseded later. Conversely `Conversations`/`ConversationBlocks` (the
  Conversation Reader) are 0 in v1 — that feature didn't exist yet.
- **Structurally larger and messier**, consistent with "not rigorously sorted": 5,843 Notes (vs.
  2,130 in v2), 450 PlotPoints with 39 still unplaced (vs. 1 of 362 in v2), 226 Subjects across 10
  `SubjectDefinitions` (the triage labels above, vs. 7 real categories in v2).

Because of this, treat v1-archive query results as describing *a different workflow era*, not a
smaller/older version of the same one. If you want a structural (not semantic) diff between the
two files, `sqldiff.exe` in the same tools folder can do that — but it will show identical schema,
so it won't surface the meaning differences above; those have to be read from the live data or
this doc.

## Safety — read this before running anything

These are Brian's irreplaceable creative writing files. Treat them as production data.

- **Never** run `INSERT`, `UPDATE`, `DELETE`, `DROP`, `ALTER`, `CREATE`, `VACUUM`, `PRAGMA` writes,
  or any statement that mutates the database. Every query in this skill is `SELECT`/`.schema`/`.tables` only.
- **Always copy first, no exceptions.** Copy the target `.storyplan` file into the scratchpad
  directory and run every query against the copy, never the original. This has zero risk to the
  source file and avoids lock/WAL contention if the WindowedStoryPlanner app happens to be open on
  the same file. The copy is instant (~14 MB) — there's no size or convenience threshold that
  justifies querying the original directly.

## Invoking sqlite3 (don't assume PATH)

Try in this order and use whichever works — don't hardcode one:

1. `sqlite3` — works if the user's PATH includes the tools folder (may need a fresh shell/session
   after a PATH change; env vars are read at process start).
2. Full path: `"C:\Users\Brian\Downloads\sqlite-tools-win-x64-3530100\sqlite3.exe"`
   (also has `sqldiff.exe` and `sqlite3_analyzer.exe` alongside it).
3. Python fallback (always available, confirmed working — Python 3.13, sqlite3 module 3.50.4):
   ```
   python -c "import sqlite3; con=sqlite3.connect('file:PATH?mode=ro', uri=True); ..."
   ```

Example copy-then-query flow (PowerShell):
```powershell
$scratch = "<scratchpad dir for this session>"
Copy-Item "C:\Users\Brian\Desktop\TLTT v2.storyplan" "$scratch\probe.db" -Force
sqlite3 "$scratch\probe.db" ".tables"
```

## Orientation — always run first

Don't assume the schema below is still current — confirm live, then proceed:

```sql
.tables
.schema Notes          -- or whichever table you're about to query
```

Then a population sweep to see what's actually used vs. empty/orphaned:
```sql
SELECT 'Chapters', COUNT(*) FROM Chapters
UNION ALL SELECT 'PlotPoints', COUNT(*) FROM PlotPoints
UNION ALL SELECT 'Subjects', COUNT(*) FROM Subjects
UNION ALL SELECT 'Notes', COUNT(*) FROM Notes
UNION ALL SELECT 'PlotPointSubjectLinks', COUNT(*) FROM PlotPointSubjectLinks
UNION ALL SELECT 'SubjectDefinitions', COUNT(*) FROM SubjectDefinitions
UNION ALL SELECT 'NoteTrackDefinitions', COUNT(*) FROM NoteTrackDefinitions
UNION ALL SELECT 'NarrativePropertyDefinitions', COUNT(*) FROM NarrativePropertyDefinitions
UNION ALL SELECT 'NarrativePropertyValueDefinitions', COUNT(*) FROM NarrativePropertyValueDefinitions
UNION ALL SELECT 'NarrativePropertyValues', COUNT(*) FROM NarrativePropertyValues
UNION ALL SELECT 'Themes', COUNT(*) FROM Themes
UNION ALL SELECT 'SourceMaterials', COUNT(*) FROM SourceMaterials
UNION ALL SELECT 'GeminiEntries', COUNT(*) FROM GeminiEntries
UNION ALL SELECT 'Ideas', COUNT(*) FROM Ideas
UNION ALL SELECT 'Conversations', COUNT(*) FROM Conversations
UNION ALL SELECT 'ConversationBlocks', COUNT(*) FROM ConversationBlocks
UNION ALL SELECT 'ConversationSubjectCoverages', COUNT(*) FROM ConversationSubjectCoverages
UNION ALL SELECT 'ConversationSubjectCoverageTracks', COUNT(*) FROM ConversationSubjectCoverageTracks
UNION ALL SELECT 'IgnoredConversations', COUNT(*) FROM IgnoredConversations;
```

**Known orphan models — expect these to NOT appear as tables at all** (confirmed absent from
`.tables` on the live v2 file): `Story` (`Models/Story.cs`) and `SubjectCluster`
(`Models/SubjectCluster.cs`) have no `DbSet<>` in `AppDbContext.cs`, so EF never created a table
for them. If a question hinges on multi-story scoping or subject clusters, the honest answer is
"not implemented in the data model yet" — see `FEATURE-AUDIT.md` items A1 and E2.

## Schema reference — tables, key columns, and what integers mean

`.schema <Table>` gives you column names and types, but SQLite stores enums as plain `INTEGER` —
it won't tell you what `NoteState = 0` means. Use this map (source: `StoryPlanner.Core/Models/*.cs`):

**`Notes`** (`Models/Note.cs`) — the core content table. `OwnerId` + `OwnerType` together are a
polymorphic FK (no DB-level foreign key; resolve manually per `OwnerType`).
- `OwnerType`: `0=Subject, 1=PlotPoint, 2=Chapter, 3=PlotPointSubjectLink` (`Models/OwnerType.cs`)
- `NoteState`: `0=Unset` (captured, not reviewed), `1=Flagged` (needs research, see `FlagReason`),
  `2=Confirmed` (stable, safe for downstream work) — **this meaning is v2-specific**; in the v1
  archive file `Confirmed` means "review closed — no need to look at this anymore" (migrated to
  v2 OR deliberately superseded; not recorded which), see "The files" above
- `NoteTrackDefinitionId` → `NoteTrackDefinitions.Id` (nullable — null means unassigned to a track)
- `ThemeId` → `Themes.Id` (nullable), `SourceMaterialId` → `SourceMaterials.Id` (nullable)
- `WorldDate` is a free-text year/year-range string, not a real date

**`NoteTrackDefinitions`** (`Models/NoteTrackDefinition.cs`) — defines the *kinds* of notes a
`Subject`/`PlotPoint`/etc. can have. Not all tracks apply to all owner types — filter by `OwnerType`.
- `TrackType`: `0=Unset, 1=Ontology, 2=Civilization, 3=History, 4=Characterization, 5=PageDesign,
  6=WorldInference, 7=ThematicEvidence, 8=NotesToSelf, 9=Analogies, 10=NarrativeArchitecture,
  11=Canon, 12=Allegories` — these are the "cognitive layers" of the planning method; see
  `TrackTypeExtensions.GetCognitiveMode()` in the same file for the full description of each.
  A `TrackType` with zero `Notes` rows pointing at its tracks is a layer that's *defined but unused*.
- `*ModeDisplayOrder` columns (Expansion/Linking/Gardener/Audit/SceneDesign) control per-`EditorMode`
  ordering; `EditorMode` enum (`Models/EditorMode.cs`): `0=Expansion,1=Linking,2=Gardener,3=Audit,4=SceneDesign`
- `IsSingleton`, `SupportsWorldDate`, `SupportsTheme`, `SupportsSourceMaterial`, `CanEditInAuditMode` are booleans (0/1)

**`Subjects`** (`Models/Subject.cs`) — the entity buckets (characters, locations, etc.).
`SubjectDefinitionId` → `SubjectDefinitions.Id` classifies *what kind* of subject it is.

**`SubjectDefinitions`** (`Models/SubjectDefinition.cs`) — `SubjectType` (free text), `DisplayOrder`.
In v2 these are real subject-kind categories (`Character`, `Technology`, ...); **in the v1 archive
the same column holds workflow/triage labels instead** ("First Pass, subject notes only", "Deferred
for...", "Complete") — see "The files" above. Don't assume the v2 meaning without checking the
actual distinct values in whichever file you're querying.

**`PlotPoints`** (`Models/PlotPoint.cs`) — scenes. `ChapterId` → `Chapters.Id` (nullable — null
means not yet placed in a chapter), `OrderInChapter`.

**`Chapters`** (`Models/Chapter.cs`) — `Title`, `OrderIndex`.

**`PlotPointSubjectLinks`** (`Models/PlotPointSubjectLink.cs`) — join table between a `PlotPoint`
and a `Subject` that is *itself* noteable (has its own `Notes` via `OwnerType=3`). This is how a
scene's specific effect on a subject gets its own notes distinct from the subject's general notes.

**`Themes`** (`Models/Theme.cs`) — `Name`, `Proposition`.

**`SourceMaterials`** (`OtherModels/SourceMaterial.cs`) — `Name`, `Description`.

**`NarrativePropertyDefinitions` / `NarrativePropertyValueDefinitions` / `NarrativePropertyValues`**
(`Models/NarrativeProperty*.cs`) — a generic typed-enum-on-an-entity system. A `NarrativePropertyDefinition`
belongs to a `SubjectDefinition`+`OwnerType` and asks a `Question`; `NarrativePropertyValueDefinition`
rows are its allowed answers; `NarrativePropertyValues` rows are the actual `OwnerId` → chosen
`ValueDefinitionId` assignments (no `OwnerType` column on the value itself — cross-reference via
the definition chain).

**`Conversations` / `ConversationBlocks` / `ConversationSubjectCoverages` / `ConversationSubjectCoverageTracks` / `IgnoredConversations`**
(`Models/Conversation*.cs`, `IgnoredConversation.cs`) — the Conversation Reader feature (imported
Claude/Gemini chat transcripts, block-by-block, with subject-coverage suggestions). v2-only —
0 rows in the v1 archive, which predates this feature.
- `ConversationBlocks.BlockState`: `0=Unread, 1=Skipped, 2=Flagged, 3=Done`
- `ConversationBlocks.Speaker`: `"user"` or `"assistant"` (text, not enum)
- `ConversationSubjectCoverageTracks.IsAdded` (bool) tracks whether a suggested note was actually created

**`GeminiEntries`** (`OtherModels/GeminiEntry.cs`) — legacy Gemini prompt/response log, predates
the Conversation Reader. `IsAnalyzed` (bool). Empty (0 rows) in current v2; this is where v1-era
Gemini-assisted analysis actually lived (278 rows in the v1 archive, all `IsAnalyzed=1`) — superseded
by the Conversation Reader, not still in active use.

**`Ideas`** (`OtherModels/Idea.cs`) — freeform idea inbox. Column is `State` (not `IdeaState` —
that's just the enum type name), values `0=Written, 1=PartiallyAnalyzed, 2=FullyAnalyzed`. Empty in
v2; populated in the v1 archive (66 rows: 12 Written, 4 PartiallyAnalyzed, 50 FullyAnalyzed).

## Analytical recipes

Always run `.schema <Table>` right before these if it's been a while — don't trust memorized
column names over the live file.

**Note completion by state, overall or per track:**
```sql
SELECT NoteState, COUNT(*) FROM Notes GROUP BY NoteState;

SELECT ntd.TrackName, n.NoteState, COUNT(*)
FROM Notes n LEFT JOIN NoteTrackDefinitions ntd ON n.NoteTrackDefinitionId = ntd.Id
GROUP BY ntd.TrackName, n.NoteState
ORDER BY ntd.TrackName;
```

**Which tracks are actually used vs. defined-but-empty:**
```sql
SELECT ntd.Id, ntd.TrackName, ntd.TrackType, COUNT(n.Id) AS NoteCount
FROM NoteTrackDefinitions ntd LEFT JOIN Notes n ON n.NoteTrackDefinitionId = ntd.Id
GROUP BY ntd.Id ORDER BY NoteCount ASC;
```

**Notes with no track assigned (fell through the cracks):**
```sql
SELECT COUNT(*) FROM Notes WHERE NoteTrackDefinitionId IS NULL;
```

**Cognitive-layer (`TrackType`) usage distribution — is one layer neglected?**
```sql
SELECT ntd.TrackType, COUNT(n.Id) AS NoteCount
FROM NoteTrackDefinitions ntd LEFT JOIN Notes n ON n.NoteTrackDefinitionId = ntd.Id
GROUP BY ntd.TrackType ORDER BY NoteCount DESC;
```

**Subject population by type (`SubjectDefinition`):**
```sql
SELECT sd.SubjectType, COUNT(s.Id) FROM SubjectDefinitions sd
LEFT JOIN Subjects s ON s.SubjectDefinitionId = sd.Id
GROUP BY sd.SubjectType ORDER BY COUNT(s.Id) DESC;
```

**PlotPoints not yet placed in a chapter (planning backlog):**
```sql
SELECT COUNT(*) FROM PlotPoints WHERE ChapterId IS NULL;
```

**`WorldDate` / `Theme` / `SourceMaterial` tagging coverage:**
```sql
SELECT COUNT(*) AS total,
       SUM(WorldDate <> '') AS with_worlddate,
       SUM(ThemeId IS NOT NULL) AS with_theme,
       SUM(SourceMaterialId IS NOT NULL) AS with_source
FROM Notes;
```

**Conversation Reader progress (block review state, subject coverage acted-on rate):**
```sql
SELECT BlockState, COUNT(*) FROM ConversationBlocks GROUP BY BlockState;

SELECT IsAdded, COUNT(*) FROM ConversationSubjectCoverageTracks GROUP BY IsAdded;
```

**Flagged notes needing research (read `FlagReason` for actual content):**
```sql
SELECT Id, OwnerType, OwnerId, FlagReason FROM Notes WHERE NoteState = 1 LIMIT 20;
```

## Relating findings back to code

When a query result confirms or contradicts an assumption, name the specific evidence:
- Cite the model file (`Models/X.cs`) that defines the table/enum.
- If it bears on a claim in `FEATURE-AUDIT.md` (e.g. "is `SubjectCluster` really orphaned?", "how
  much `WorldDate` data exists to justify a timeline view?"), reference the specific item ID (A1,
  B1, etc.) and state whether the live data supports or updates that assessment.
- Prefer real counts over impressions — "1,931 Unset notes vs. 199 Flagged vs. 0 Confirmed" is a
  concrete, checkable fact; "notes seem mostly unreviewed" is not.
