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

Example copy-then-query flow (PowerShell). **Copy the `-wal`/`-shm` sidecars too** — the files
are in WAL mode, so recent transactions (potentially including whole schema migrations) live in
`*.storyplan-wal`; a bare copy of the main file silently reads stale data (learned 2026-07-30
when a copy appeared to be two migrations behind):
```powershell
$scratch = "<scratchpad dir for this session>"
Copy-Item "C:\Users\Brian\Desktop\TLTT v2.storyplan" "$scratch\probe.db" -Force
Copy-Item "C:\Users\Brian\Desktop\TLTT v2.storyplan-wal" "$scratch\probe.db-wal" -Force -ErrorAction SilentlyContinue
Copy-Item "C:\Users\Brian\Desktop\TLTT v2.storyplan-shm" "$scratch\probe.db-shm" -Force -ErrorAction SilentlyContinue
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
UNION ALL SELECT 'Stories', COUNT(*) FROM Stories
UNION ALL SELECT 'Theaters', COUNT(*) FROM Theaters
UNION ALL SELECT 'Pivots', COUNT(*) FROM Pivots
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
UNION ALL SELECT 'SourceMaterialParts', COUNT(*) FROM SourceMaterialParts
UNION ALL SELECT 'NoteSourceReferences', COUNT(*) FROM NoteSourceReferences
UNION ALL SELECT 'GeminiEntries', COUNT(*) FROM GeminiEntries
UNION ALL SELECT 'Ideas', COUNT(*) FROM Ideas
UNION ALL SELECT 'Conversations', COUNT(*) FROM Conversations
UNION ALL SELECT 'ConversationBlocks', COUNT(*) FROM ConversationBlocks
UNION ALL SELECT 'ConversationSubjectCoverages', COUNT(*) FROM ConversationSubjectCoverages
UNION ALL SELECT 'ConversationSubjectCoverageTracks', COUNT(*) FROM ConversationSubjectCoverageTracks
UNION ALL SELECT 'IgnoredConversations', COUNT(*) FROM IgnoredConversations;
```

**`Stories` is no longer an orphan model — A1 shipped.** `Models/Story.cs` now has a `DbSet<>`
and a real `Stories` table (`Id`, `Title`, `Abbreviation`, `ColorHex`, `OrderIndex`), and
`Chapters.StoryId` groups chapters under them. `StoryId = 0` is a permanent, legal sentinel
meaning "(Unassigned)" (see `UnassignedStory` in `Story.cs`) — not a missing reference, and not
evidence a backfill is incomplete. **v1 and v2 have independent Stories tables, never joined or
id-shared** — a story of the same name in both files (e.g. "The Lioness of Tall Tale") is a
coincidence for the reader, not a correspondence, consistent with "v1 and v2 never join" above.

**Still a known orphan model** (confirmed absent from `.tables`): `SubjectCluster`
(`Models/SubjectCluster.cs`) has no `DbSet<>` in `AppDbContext.cs`, so EF never created a table
for it. If a question hinges on subject clusters, the honest answer is "not implemented in the
data model yet" — see `FEATURE-AUDIT.md` item E2.

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
- `ThemeId` → `Themes.Id` (nullable). Source material citations are **not** a column here (2026-07-31)
  — see `NoteSourceReferences` below; a note may cite several Parts for one claim
- World dates are structured (2026-07-30): `WorldDateStartYear/Month/Day` +
  `WorldDateEndYear/Month/Day`, all nullable ints — all-null = undated; nulls at month/day level
  mean "to be determined", never "approximately". Event vs condition is the note's TRACK
  (`SupportsWorldDateEnd`), not a field here. The old free-text `WorldDate` string column is
  legacy: blanked per-note by the `convert-world-dates` DataOps op; a non-blank value on a note
  with null structured columns means "unconvertible, awaiting triage" (e.g. `"?"`, `"954-914"`)

**`NoteTrackDefinitions`** (`Models/NoteTrackDefinition.cs`) — defines the *kinds* of notes a
`Subject`/`PlotPoint`/etc. can have. Not all tracks apply to all owner types — filter by `OwnerType`.
- `TrackType`: `0=Unset, 1=Ontology, 2=Civilization, 3=History, 4=Characterization, 5=PageDesign,
  6=WorldInference, 7=ThematicEvidence, 8=NotesToSelf, 9=Analogies, 10=NarrativeArchitecture,
  11=Canon, 12=Allegories` — these are the "cognitive layers" of the planning method; see
  `TrackTypeExtensions.GetCognitiveMode()` in the same file for the full description of each.
  A `TrackType` with zero `Notes` rows pointing at its tracks is a layer that's *defined but unused*.
- `*ModeDisplayOrder` columns (Expansion/Linking/Gardener/Audit/SceneDesign) control per-`EditorMode`
  ordering; `EditorMode` enum (`Models/EditorMode.cs`): `0=Expansion,1=Linking,2=Gardener,3=Audit,4=SceneDesign`
- `HiddenIn*Mode` columns (same five modes, 2026-07-31): booleans; 1 demotes the track to a
  collapsed "Hidden in this mode" group in that editor mode (even when it has notes). 0 =
  visible, the default for every row. A display preference like the order columns — the MCP
  server deliberately does not expose or honor them
- `IsSingleton`, `SupportsWorldDate`, `SupportsWorldDateEnd`, `SupportsTheme`,
  `SupportsSourceMaterial`, `CanEditInAuditMode` are booleans (0/1). `SupportsWorldDateEnd`
  is the 2026-07-30 event/condition track split: `SupportsWorldDate=1, SupportsWorldDateEnd=0`
  = event track (a dated note asserts *when it happened*); both 1 = condition track (a dated
  note asserts *over what period it held*, start..end)
- `IsFocalCharacterOnly` (bool, 2026-07-31) — only meaningful on `OwnerType=3`
  (`PlotPointSubjectLink`) tracks. `1` means the track shows only on the link whose
  `SubjectId` equals the owning `PlotPoint.FocalCharacterId`; on any other link for the same
  plot point it's hidden **unless that link already has notes on it** (existing content is
  never hidden). This is an app-UI display rule, not something enforced or reported by the
  MCP server — a query joining `Notes` to a focal-only track will still find rows on
  non-focal links, and that's expected, not a data error.

**`Subjects`** (`Models/Subject.cs`) — the entity buckets (characters, locations, etc.).
`SubjectDefinitionId` → `SubjectDefinitions.Id` classifies *what kind* of subject it is.
`IsPovCharacter` (bool, 2026-07-31) is an authorial flag — "this subject may narrate a scene in
third-person-limited" — and is the only thing that populates a `PlotPoint`'s focal-character
picker in the app. It does not require `SubjectDefinitionId` to be Character (no code-level
check), though that's the intended use.

**`SubjectDefinitions`** (`Models/SubjectDefinition.cs`) — `SubjectType` (free text), `DisplayOrder`.
In v2 these are real subject-kind categories (`Character`, `Technology`, ...); **in the v1 archive
the same column holds workflow/triage labels instead** ("First Pass, subject notes only", "Deferred
for...", "Complete") — see "The files" above. Don't assume the v2 meaning without checking the
actual distinct values in whichever file you're querying.

**`PlotPoints`** (`Models/PlotPoint.cs`) — scenes. `ChapterId` → `Chapters.Id` (nullable — null
means not yet placed in a chapter), `OrderInChapter`. Since 2026-07-30 also `TheaterId`
(0 = "(Unplaced)" sentinel) and a fabula date `FabulaYear/Month/Day` (nullable — EVENT ONLY,
never an interval: a plot point wanting a span is holding more than one scene). A plot point
thus carries two independent temporal coordinates: fabula date (world time) and syuzhet
position (chapter + order); their divergence is flashback/non-linear telling.
`FocalCharacterId` (nullable int, 2026-07-31) → `Subjects.Id` — the scene's POV character.
Null (the overwhelming majority of scenes) means undesignated, not "no POV" — there is no
sentinel row here, unlike `TheaterId`/`ChapterId`. Only ever set to a subject with
`IsPovCharacter=1`, though nothing at the schema level enforces that after the fact (e.g. if
the subject's flag is later unticked).

**`Theaters`** (`Models/Theater.cs`, 2026-07-30) — timeline columns: `Name`, `Description`,
`OrderIndex` (narrative-density order, a display coordinate, not a taxonomy). Deliberately no
ColorHex — hue is reserved for subject type. `Subjects.TheaterId` / `PlotPoints.TheaterId`
reference it; 0 = "(Unplaced)", same sentinel pattern as `Chapter.StoryId`.

**`Pivots`** (`Models/Pivot.cs`, 2026-07-30) — authored years where the world's causal regime
changed (`Year`, `Name`, `Description`). Eras are DERIVED as intervals between consecutive
pivots (N pivots → N+1 eras) — never stored, so there is no Eras table to look for.

**`Chapters`** (`Models/Chapter.cs`) — `Title`, `OrderIndex` (now **per-story**, contiguous 1..n —
not the flat book-wide sequence it was before A1), `StoryId` → `Stories.Id`. `StoryId = 0` is the
permanent "(Unassigned)" sentinel, never a missing/dangling reference.

**`Stories`** (`Models/Story.cs`) — `Title`, `Abbreviation`, `ColorHex`, `OrderIndex` (publication/
reading order). Container only — no `OwnerType`, no notes of its own. Independent per file: v1's
and v2's `Stories` tables are never joined or id-shared, even when a title matches.

**`PlotPointSubjectLinks`** (`Models/PlotPointSubjectLink.cs`) — join table between a `PlotPoint`
and a `Subject` that is *itself* noteable (has its own `Notes` via `OwnerType=3`). This is how a
scene's specific effect on a subject gets its own notes distinct from the subject's general notes.

**`Themes`** (`Models/Theme.cs`) — `Name`, `Proposition`.

**`UiSettings`** (`Models/UiSetting.cs`, 2026-07-31) — key/value rows for UI preferences that
persist with the file (`Key`, `Value` = opaque JSON payload). Currently one key:
`Timeline.ViewState` (zoom, viewport center, collapsed theaters/eras — payload shape in
`Core/Timeline/TimelineViewState.cs`). App state, not story data: ignore it when analyzing
content, and expect readers to tolerate a missing row or unparseable payload.

**`SourceMaterials` / `SourceMaterialParts` / `NoteSourceReferences`** (`Models/SourceMaterial*.cs`,
`Models/NoteSourceReference.cs`, 2026-07-31) — a two-tier citation/coverage model, NOT a plain
tag. `SourceMaterials` is the Work (`Name`, `Description`, `PartNoun` — "Episode"/"Country"/
"Chapter", empty = no Parts, cite the Work itself — `OrderIndex`). `SourceMaterialParts` is one
unit of a mining pass under a Work (`SourceMaterialId`, `Code` e.g. `"S3E01"`, `Name`,
`Description`, `OrderIndex`, `ReviewState` 0=NotReviewed/1=Reviewed). `NoteSourceReferences` is
the join (`NoteId`, `SourceMaterialId`, `SourceMaterialPartId` nullable — null cites the Work as a
whole — `SortOrder`) — **many rows per note are expected**: a note may cite several Parts for one
claim (e.g. "the Wonderbolts were useless in a crisis, as shown in Sonic Rainboom, Secret of my
Excess, Equestria Games and Twilight's Kingdom" is one `Note` with four `NoteSourceReference` rows).
Only tracks with `NoteTrackDefinitions.SupportsSourceMaterial=1` can carry a citation — as of
2026-07-31 that's the six `TrackType.Canon` tracks (seeded via the `seed-source-material` DataOps
op), never every track that happens to mention canon in prose.
`ReviewState` is **orthogonal to citation count**, not derived from it: a Part can be `Reviewed`
with zero citations ("watched it again, nothing there — confirmed empty") or `NotReviewed` with
citations ("cited from memory, never revisited"). "Untouched" (the negative-space / rewatch-queue
signal) means **both** `NotReviewed` AND zero citations — computing it from either column alone
gives a false answer for one of those two quadrants. The Work/Part set is meant to be
pre-enumerated (seeded from a reviewable config, not accreted on first citation) — an uncited Part
is real negative space only if the set is known to be complete; do not treat an empty
`SourceMaterialParts` table as "nothing to cite," check whether seeding has run.

**`NarrativePropertyDefinitions` / `NarrativePropertyValueDefinitions` / `NarrativePropertyValues`
/ `WorkPhases`** (`Models/NarrativeProperty*.cs`, `Models/WorkPhase.cs`) — a generic
typed-enum-on-an-entity system, dormant from 2026-05-06 until its first real use on 2026-07-31.
A `NarrativePropertyDefinition` belongs to a `SubjectDefinition`+`OwnerType` and asks a `Question`;
`NarrativePropertyValueDefinition` rows are its allowed answers; `NarrativePropertyValues` rows are
the actual `OwnerId` → chosen `ValueDefinitionId` assignments.

Four things to know before querying it:

- **No `OwnerType` on the value row.** Join
  `NarrativePropertyValues → NarrativePropertyValueDefinitions → NarrativePropertyDefinitions` to
  learn what kind of owner an `OwnerId` refers to. A bare `OwnerId` match conflates subject 7 with
  chapter 7 — the same class of error as ignoring `Note.OwnerType`.
- **Single-select.** At most one value per (owner, property). Nothing in the schema enforces it;
  `PlanIntegrity` reports a second as `narrativevalue.duplicate_for_property`. If you find one,
  it is a bug, not a legal multi-value assignment.
- **Unset = no row.** There is no sentinel value row, and unset is a normal long-lived state, so a
  `LEFT JOIN` is the right shape and a count of assignments is not a count of entities.
  "Which entities lack a value" is a legitimate question about *data*; **which value one should
  get is not** — that is authorial categorization (CLAUDE.md).
- **Scope is the compound key.** `(SubjectDefinitionId, OwnerType)`, exactly like
  `NoteTrackDefinition` — and with the same asymmetry: rows for `OwnerType.PlotPoint` and
  `.Chapter` ignore `SubjectDefinitionId` (their call sites filter on `OwnerType` alone), while
  `.Subject` and `.PlotPointSubjectLink` rows use both.

`WorkPhases` are the ordered stages of the planning work (`Name`, `DisplayOrder`, plus
`RequiresZeroFlaggedNotes` / `RequiresZeroUnsetNotes` criteria). **Not `EditorMode`** — the names
overlap, the concepts do not, and neither derives from the other. Phase completion is derived from
the criteria, never stored, the same way timeline eras are derived as the gaps between pivots.
`NarrativePropertyDefinition.GatingWorkPhaseId` (nullable, null = never gates) names the phase at
which an unset value is *reported* as a gap; it blocks nothing.

Seeded by the `seed-narrative-properties` DataOps op, which writes definitions and allowed values
and **never a `NarrativePropertyValue`** — a test pins that. Its config carries no prose fields at
all, so `Question` / `Explanation` / `Description` are authored in the app and cannot be clobbered
by a re-run.

**`Conversations` / `ConversationBlocks` / `ConversationSubjectCoverages` / `ConversationSubjectCoverageTracks` / `IgnoredConversations`**
(`Models/Conversation*.cs`, `IgnoredConversation.cs`) — the Conversation Reader feature (imported
Claude/Gemini chat transcripts, block-by-block). v2-only — 0 rows in the v1 archive, which
predates this feature.
- `ConversationBlocks.BlockState`: `0=Unread, 1=Skipped, 2=Flagged, 3=Done` — the only authored
  state in this corpus, and the only one worth analyzing
- `ConversationBlocks.Speaker`: `"user"` or `"assistant"` (text, not enum)
- `Conversations.ArcSummary` and `ConversationBlocks.Summary` are **optional navigation aids**,
  not content: authored outside the app and imported from a `*_meta.json`. Since 2026-07-31 a
  conversation can be imported straight from a raw Claude export with no meta pass at all, so
  **empty is an ordinary state, not missing data** — do not report a blank summary as a gap, and
  never count summaries as a coverage/progress metric. Import never destroys one: a content-only
  re-import leaves existing summaries and `BlockState` untouched.
- `ConversationBlocks.HasDecisions` **no longer exists** — dropped 2026-07-31 (migration
  `DropBlockHasDecisions`). It was an AI judgment about which turns mattered. Any older note or
  query referencing it is stale.
- **`ConversationSubjectCoverages` / `ConversationSubjectCoverageTracks` are FROZEN.** They hold
  the abandoned AI-suggested subject×track routing. Nothing writes them any more: the import path
  and the reader's checklist column were cut 2026-07-31, and a meta file's `subjectsCovered` array
  is now inert. The rows remain only so conversation deletion keeps cascading. Point-in-time
  counts at the cut: 1,472 coverages / 4,062 tracks / **0** with `IsAdded=1`. Treat them as an
  archaeological record of a rejected feature — never as a signal about a subject, and never as
  the basis of a new query or metric.

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

**`WorldDate` / `Theme` tagging coverage** (count the structured columns, not the legacy
`WorldDate` string — on a converted file that string is blank except for the unconvertible
triage residue, so `SUM(WorldDate <> '')` undercounts by exactly the converted rows):
```sql
SELECT COUNT(*) AS total,
       SUM(WorldDateStartYear IS NOT NULL) AS with_worlddate,
       SUM(ThemeId IS NOT NULL) AS with_theme
FROM Notes;
```

**Focal-character (POV) coverage** — how many scenes have a POV designated, and by whom:
```sql
SELECT COUNT(*) AS total, SUM(FocalCharacterId IS NOT NULL) AS with_focal_character
FROM PlotPoints;

SELECT s.Name, COUNT(*) AS scenes
FROM PlotPoints pp JOIN Subjects s ON s.Id = pp.FocalCharacterId
GROUP BY s.Id ORDER BY scenes DESC;

-- Candidates: subjects flagged as POV-capable but never actually used as a scene's focal character.
SELECT s.Name FROM Subjects s
WHERE s.IsPovCharacter = 1
  AND NOT EXISTS (SELECT 1 FROM PlotPoints pp WHERE pp.FocalCharacterId = s.Id);
```

**Source material citation coverage** (many-to-many — a note may cite several Parts, so this is
not a single boolean column on `Notes`; see `NoteSourceReferences` above):
```sql
SELECT COUNT(DISTINCT NoteId) AS notes_with_a_citation, COUNT(*) AS total_citations
FROM NoteSourceReferences;

-- Negative space: untouched Parts (never reviewed AND never cited).
SELECT sm.Name AS work, p.Code, p.Name,
       (SELECT COUNT(*) FROM NoteSourceReferences r WHERE r.SourceMaterialPartId = p.Id) AS notes
FROM SourceMaterialParts p JOIN SourceMaterials sm ON sm.Id = p.SourceMaterialId
WHERE p.ReviewState = 0
  AND NOT EXISTS (SELECT 1 FROM NoteSourceReferences r WHERE r.SourceMaterialPartId = p.Id)
ORDER BY sm.OrderIndex, p.OrderIndex;
```

**Conversation Reader progress (block review state — the only live progress signal here):**
```sql
SELECT BlockState, COUNT(*) FROM ConversationBlocks GROUP BY BlockState;
```

Do **not** pair this with a `ConversationSubjectCoverageTracks` / `IsAdded` breakdown. That query
is historical: the feature was cut on 2026-07-31 and its rows are frozen, so the tally answers
"what did an abandoned experiment propose in 2026", not anything about the current file. Summary
population (`ArcSummary <> ''`) is likewise not a progress metric — summaries are optional.

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
