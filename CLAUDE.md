# StoryPlanner

A personal WPF (`net10.0-windows`) + EF Core/SQLite planning instrument for **The Lioness of
Tall Tale** (TLTT), an MLP:FiM × Equestria at War hopepunk epic. One user, no deadline, no spec.

**The `.storyplan` data files are the product; this code is the instrument.** Features arrive
from design conversations rather than a specification, and the data's actual shape outranks
every document describing it.

## Two AI roles — this file governs the first

| | **Claude Code** (here) | **Claude Desktop** |
|---|---|---|
| Purpose | Builds the planner | Analyzes the story |
| Autonomy | Agentic, autonomous, guided by these conventions | n/a — writes no code |
| Governed by | This file + skills | The MCP server's own instructions |

Claude Code **reads story content to understand how to build features** — you cannot correctly
implement a flagged-note wall without reading flagged notes. That reading is instrumental. What
Claude Code does not do is form or offer opinions about the story itself: whether a flagged note
is resolved, whether a theme is well-evidenced, what a subject needs next. Same data, different
purpose. Story analysis happens in Desktop, through the MCP server.

## What the tool must never do

**Retrieval, not suggestion.** Machine-proposed structure has been built and abandoned twice
here: note categorization, and the Conversation Reader's suggested subject×track coverage
(4,062 rows, `IsAdded = 0` on every one — "turned out to not be helpful"). A third attempt was
struck three times during the MCP server build under three different names.

> Tools answer *"what is here."* Never *"what should you do"* or *"what's interesting."*
> An obvious bottleneck in the data is not a mandate for a feature. When a feature idea encodes
> workflow, intent, ranking, or suggestion — stop and ask.

**The coverage suggestion is now cut, not merely unused (2026-07-31).** No code path writes
`ConversationSubjectCoverage`, the reader's checklist column is gone, and a meta file's
`subjectsCovered` array is inert on import — a test asserts it writes zero rows. The tables and
their existing data survive, dormant, so `DeleteConversationAsync`'s cascade still works. The
block-level `HasDecisions` flag went with it (same judgment call, one layer down): column dropped.
Do not rebuild either. Conversation import no longer requires an AI pass at all — see below.

**Seeders seed structure, never prose (2026-07-31).** A DataOps seed op may create rows, ids,
orderings, and flags. It must **not** author the prose on them — display questions, explanations,
usage directives, value descriptions. That prose is story metadata: it carries Brian's framing of
what a track or property *asks*, and a plausible machine-written one is worse than an empty field,
because an empty field is visibly unfinished while a wrong one reads as decided. Precedent: the
2026-07-30 History-track split shipped seeded display questions that all had to be rewritten. Seed
the names, leave the prose empty, let it be authored in the Definitions tab — and make a re-run
incapable of clobbering it. `seed-narrative-properties` does this structurally: its config has no
prose field at all, so there is nothing to overwrite with. (`seed-source-material` predates the
rule and does re-stamp `Description` on Works and Parts; those describe real published works
rather than Brian's framing, which is the reason it is left alone.)

## Architecture — deliberate, not accidental

**No navigation properties, no foreign keys, no indexes.** Single-user, load-everything-at-
startup desktop app: view models filter `ICollectionView`s over in-memory `ObservableCollection`s
bound to `DbSet.Local`. Models are row vessels; view models do relationship work. Nav properties
would only serve lazy loading, `Include`, and DB-executed LINQ — none of which apply — while
adding real risks (stale lazy loads, change-tracking surprises, circular-reference serialization).

Note ownership is **polymorphic** (`OwnerId` + `OwnerType`), so the database cannot enforce
referential integrity. **`ContentDeleter.cs` IS the integrity system** — its guards are not
decoration. Indexes follow from the same premise: nothing queries the database after load.

The configuration layer is **Type Object pattern + metadata-driven design** —
`SubjectDefinition` and `NoteTrackDefinition` are *rows*, not classes. A Character subject and a
World Law subject are the same C# class with different definition rows. The planner's shape
evolves by data entry rather than code change; that flexibility was the point. Editor modes are
the same principle applied to the UI (see the wpf-conventions skill).

**Code organization (2026-07-30): one flat namespace per project; feature-first folders.**
Folders are organization, namespaces are assembly identity — decoupled on purpose so files move
freely (`.editorconfig` suppresses the analyzers that fight this; sole exception:
`StoryPlanner.Core.Migrations`, generated). WPF layer has no `Views/`/`ViewModels/` split —
features own their folder (`Timeline/`, `Conversations/`, …) with `Shell/`, `Common/`,
`Editing/` as the only horizontals. Rules and rationale: the `wpf-conventions` skill.

*Accepted cost, decided at design time:* metadata-driven queries are less self-documenting.
Filtering by `NoteTrackDefinitionId` reads worse than `IsOntologyTrack` would. Do not "fix" this.
Rationale: `docs/design-conversations/019_…json` blocks 126–135.

## Data semantics — the traps

- **`Confirmed` inverts across files.** v2: stable, safe for downstream work. v1 archive:
  *review closed — migrated to v2 **or** deliberately superseded, disposition not recorded.*
  Never render an archive note as "confirmed"; never read it as current truth.
- **Flagged notes are walled wherever an LLM consumes data** — the app's export
  (`NoteExportRenderer.cs:28`) and the MCP server's ordinary tools both exclude them. Counts are
  disclosed; content requires the flagged tool family. `FlagReason` is itself a corpus Brian
  drafts into.
- **v1 and v2 never join.** Different organizing principles on purpose; no id correspondence,
  ~40% name overlap, and no join is wanted.
- **The scene graph is in v1** (1,125 links); v2 holds the taxonomy. Migrating it is Brian's
  future authorial work — matching v1 links to v2 subjects/plot points is categorization, not a
  mechanical operation, and no tool should propose the mapping. **The track definitions are
  final in shape** (the 2026-07-30 event/condition split of the six History tracks was a
  definition-row change, not a schema change — the design's whole point); the data is in flux.
- **World dates are structured** (2026-07-30): `Start(Y,M?,D?)` + optional `End` columns on
  `Note`, event-only `Fabula*` on `PlotPoint`. Year is the precision floor; nulls mean "to be
  determined", never "approximately". Whether a date is an event or a condition is the TRACK
  (`SupportsWorldDateEnd`), never a field on the value — and plot points are always events (a
  plot point wanting a span is holding more than one scene). Notation: `1007`, `1007-03-15`,
  `854..914`, `1007..`; negative = BLB, `0` = the banishment. The legacy free-text `WorldDate`
  string column survives only until the `convert-world-dates` DataOps op has run per file
  (unconvertible strings stay in it, surfaced by the Timeline tab's triage panel); all read
  paths prefer structured and legacy-convert mechanically — flag, never guess. That read is
  `Note.EffectiveWorldDate()` in `WorldDateModel.cs` and **range intersection is
  `WorldDateRange`** (`StoryPlanner.Core`), shared by the MCP server and the app so the two can
  never disagree. Both of its rules are subtle: an inclusive end year means an *exclusive* edge at
  `year + 1.0`, and both overlap comparisons are strict (writing `latest >= Lo` admits a note
  dated 914 into the range `915..` — a real off-by-one in `get_notes_in_date_range`, fixed
  2026-07-31).
- **Timeline x-axis**: `Subject.TheaterId` / `PlotPoint.TheaterId` (sentinel `0` =
  "(Unplaced)", same pattern as `Chapter.StoryId`). Theater assignment is authorial — never
  derive it from names. `Pivot` rows are authored years; eras are DERIVED as the gaps between
  pivots, never stored.
- **POV is `PlotPoint.FocalCharacterId`** (`int?`, 2026-07-31). Null = undesignated, a legal
  long-lived state — most scenes have none, and unlike `TheaterId`/`StoryId` there is no
  sentinel row to fall back to. Assignment is authorial, never derived from links, note counts,
  or names. Candidates are gated by `Subject.IsPovCharacter` (an unfiltered picker over every
  Character subject is unusable). It also drives which `PlotPointSubjectLink` note tracks
  display: `NoteTrackDefinition.IsFocalCharacterOnly` hides a track everywhere except the link
  whose `SubjectId` equals the plot point's `FocalCharacterId` — the gap for an *observed*
  character lives in the POV character's own link, never the observed one's (design rationale:
  `docs/design-conversations/053_…json` blocks 262-263, `019_…json` block 36-37). A link that
  already holds notes on a focal-only track keeps showing it regardless, so existing content is
  never hidden by a later POV change.
- **0 `Confirmed` notes in v2 is not a defect** — Audit is the only mode that can promote to
  Confirmed, and no audit pass has run. Surprising ≠ broken.
- **Source material is a coverage tracker, not a tag** (2026-07-31). Two-tier: `SourceMaterial`
  (a Work — MLP:FiM, Equestria at War, another fanfic) → `SourceMaterialPart` (one unit of a
  mining pass — an episode, a playable country, a chapter; empty `PartNoun` = no Parts, cite the
  Work itself). `NoteSourceReference` is the join, and **many rows per note are normal**: a note
  may cite several Parts for one claim (e.g. one Wonderbolts note citing four episodes) —
  splitting such a note into one-per-citation was considered and rejected. Only tracks with
  `SupportsSourceMaterial=1` can carry a citation (the six `TrackType.Canon` tracks, by decision —
  not every track that happens to mention canon in prose). A Part's `ReviewState` is **orthogonal
  to citation count**: Reviewed-with-zero-citations means "checked, nothing there," not the same
  as never-looked-at. "Untouched" (the negative-space/rewatch-queue signal) requires **both**
  NotReviewed and zero citations. The Work/Part set is pre-seeded (`seed-source-material` DataOps
  op) rather than accreted from citations — an uncited Part is only meaningful negative space if
  the set is known complete, so **never** rank Parts by likely yield or suggest what to look for;
  list them flat (same rule as everywhere else: retrieval, not suggestion). The coverage grid
  colours all four quadrants (2026-07-31): untouched = plain white (the baseline), cited-but-not-
  reviewed = warm blue, reviewed-and-cited = green, reviewed-with-zero-citations = beige. Four
  labels, no ranking — no quadrant is a score or a queue position.
- **Narrative properties are closed-vocabulary fields, and they are authorial** (2026-07-31, first
  real use after a year dormant). `NarrativePropertyDefinition` is a Type Object row scoped by
  `(SubjectDefinitionId, OwnerType)` exactly like `NoteTrackDefinition` —
  `NarrativePropertyValueDefinition` rows are its allowed answers, `NarrativePropertyValue` the
  assignment. **Single-select**: at most one value per (owner, property), an invariant the schema
  cannot express (no FKs, no unique constraints, no unit of work) so `PlanIntegrity` enforces it
  as `narrativevalue.duplicate_for_property`. **Absence of a row is "unset"** — a legal, long-lived
  state, never missing data, and there is deliberately no `(none)` value row because its id would
  be stored and read back as a real answer. `NarrativePropertyValue` has **no `OwnerType` column**:
  resolve ownership by tracing `ValueDefinitionId → NarrativePropertyDefinitionId → OwnerType`, or
  subject 7 and chapter 7 collide silently. Assignment is Brian's — **never derive a value** from
  note text, names, links, or real-world analogues. First use is the four orthogonal political axes
  on Civilizational System subjects (Human Capital / Governance / Boundary / Social Contract, two
  poles each); a system that moves along an axis over time is modelled as *separate era subjects*,
  so do not add date-scoped or multi-value assignments. `WorkPhase` rows are the ordered stages of
  the planning work — **not `EditorMode`**, whose values overlap by name only — and a property may
  name the phase at which an unset value is reported as a gap. That gate **reports and never
  blocks**; `CanPromoteToConfirmed` does not consult it.

Schema detail and query recipes: `.claude/skills/storyplan-data/SKILL.md`.
**Live counts: `mcp__storyplanner get_stats`. Never hardcode counts in a document.**

## Build & run

.NET 10. `dotnet test tests/StoryPlanner.Tests` — covers the MCP server's invariants and
`StoryPlanner.Core`'s export/scan/transform logic. Run before finishing any work in
`tools/` or `StoryPlanner.Core/`. Conventions and the known WPF-layer gap:
`.claude/skills/testing/SKILL.md`.

- **Brian's own instance of the app runs from a published copy, not `bin/Debug`.** Same reasoning
  as the MCP server below: `WindowedStoryPlanner/publish/WindowedStoryPlanner.exe` (a
  `dotnet publish -c Debug -o publish` output, gitignored — Debug on purpose, so a debugger can
  still attach and hit breakpoints) is what Brian actually launches day to day, deliberately
  separate from `bin/Debug/net10.0-windows/`, which is what `dotnet build`/`dotnet run` write to.
  This means Claude Code can freely build and run WindowedStoryPlanner without waiting on or
  colliding with Brian's own running instance, and vice versa. **Claude Code does not drive the
  app, though:** verifying a UI change means launching `bin/Debug` against a *copy* of a
  `.storyplan` (with its `-wal`/`-shm`), then stopping and handing Brian a numbered checklist —
  he clicks and signs off, and only then does the publish below happen. Procedure and rationale:
  `.claude/skills/testing/SKILL.md`, "The third tier is Brian".
  **After changing code under `WindowedStoryPlanner` or `StoryPlanner.Core`**, republish to ship
  it to Brian's instance: `dotnet publish WindowedStoryPlanner -c Debug -o WindowedStoryPlanner/publish`
  — Brian then closes and relaunches the app manually to pick it up (there's no live-reconnect
  equivalent for a WPF window; the point of the split is to remove *build* contention, not to make
  the relaunch itself unnecessary).
- **The MCP server runs from a published copy, not `bin/Debug`.** `.mcp.json` points every
  session's `storyplanner` connection at `tools/StoryPlanner.Mcp/publish/StoryPlanner.Mcp.dll`
  (a `dotnet publish -c Release -o publish` output, gitignored) — deliberately separate from
  `bin/Debug/net10.0/`, which is what `dotnet build`/`dotnet test` write to. This is why any
  number of parallel sessions can stay connected while others build or run
  `dotnet test tests/StoryPlanner.Tests` freely: nothing in the ordinary build path touches the
  folder live servers have locked.
  **After changing code under `tools/StoryPlanner.Mcp` or `StoryPlanner.Core`**, republish to
  ship it: `dotnet publish tools/StoryPlanner.Mcp -c Release -o tools/StoryPlanner.Mcp/publish`,
  then reconnect via `/mcp` in each session that should pick it up. Until reconnected, a session
  keeps running the server code from its last connect. If the publish step itself fails on a
  locked file, some session still holds the *publish* folder open mid-reconnect — wait for it to
  finish or ask that session to retry `/mcp`.
- `.storyplan` is raw SQLite in **WAL mode**. Reads never block the running app. The main file's
  **mtime does not advance on write** — change detection uses `PRAGMA data_version`.
- **`StoryService` is not read-only:** `OpenProjectAsync` runs `MigrateAsync()` (upgrades the
  schema in place, no backup) and silently no-ops if a project is already loaded; `SaveAsync()`
  writes `.md` and `_stats.csv` litter. The MCP server bypasses it with `Mode=ReadOnly`.
- **stdout is JSON-RPC in the MCP process** — stderr only. Never `Console.WriteLine` from code the
  server can reach. (`ConversationImporter` used to be the standing example; as of 2026-07-31 it
  reports through a returned `ConversationImportResult` instead of printing, but it is still a
  write path and the read-only server has no business calling it.)
- **Conversation import needs no AI pass (2026-07-31).** Two routes, both first-class: *Scan Claude
  Export… → Import Checked Directly* puts raw blocks straight in the reader from the export, and
  *Export Checked for Cowork… → Import from Folder…* adds summaries afterwards. A `_meta.json` is
  optional everywhere; it supplies `ArcSummary` and per-block `Summary` (navigation only) and
  nothing else. Meta never destroys: a content-only re-import leaves earlier summaries and block
  triage state intact. An empty summary is ordinary, not missing data — never substitute an
  excerpt for one.

## Settled — do not propose alternatives

- The architecture above (no nav properties / FKs / indexes; polymorphic ownership; Type Object).
- **MCP design:** dumb tools; grep→fetch two-pass; no ranking; no fuzzy matching (the caller
  supplies vocabulary, the tool supplies alternation); hard flagged wall with count disclosure;
  corpora never joined.
- **Abandoned after being built:** note categorization, coverage-track suggestions, the
  41-report insight pipeline.
- `FEATURE-AUDIT.md` ⚪ lists features rejected in-conversation or closed as already-resolved.
- **Note supersession is settled (2026-07-30, FEATURE-AUDIT C1).** No `Superseded` state, no
  `Retcon` track, no note-to-note supersession link. Displaced lore is not archived — it is
  *promoted*: rewritten as a scene-link `Reader Prior Belief Update`/`Clash` note (what the reader
  believed before the scene corrected them), with the authorial revision recorded in the subject's
  `Garden Notes`. Both track families ship and the real files use them this way.

## Brian's decisions, always

Story structure and categorization · what is interesting · whether a flagged note is resolved ·
taxonomy changes · anything that writes to a `.storyplan`.

## Read order and source authority

**Live data > code > this file / FEATURE-AUDIT > design transcripts.**

Cold start: this file → `FEATURE-AUDIT.md` → `get_stats`.
Per task: `storyplan-data` skill (data work) · `wpf-conventions` skill (WindowedStoryPlanner) ·
`tools/StoryPlanner.Mcp` (server work) · `CONVERSATION-READER-SPEC.md` (historical — see its
banner for drift).

Two sources, two purposes — do not substitute one for the other:

- **MCP** (`get_stats`, `count_notes_*`, `get_track_definitions`, `list_subjects`) answers
  *"what does the data look like"*. Use it before modeling data.
- **`docs/design-conversations/`** (Grep/Read) answers *"why is this like this"*.
  Do **not** use `search_conversations` for design rationale: conversations 020 and 039 are not
  in the database at all, and rebuilding the server disconnects it.

Transcripts are authoritative for **nothing** — they are the record of *why*. Consult them only
after FEATURE-AUDIT has established what is live: they contain complete, persuasive arguments
for features that were later abandoned.

## Design conversation manifest

`docs/design-conversations/` — signal ratings are FEATURE-AUDIT's.

| File | Records | Signal |
|---|---|---|
| `015_organizing-a-multi-story-fabula…` | Multi-story fabula → schema; chapter/thread structure | High |
| `019_character-reader-perception-gap…` | **The no-nav-properties / polymorphic-ownership decision (blocks 126–135)**; perception-gap layer | High |
| `020_migrating-model-classes-to-note-collections` | The shipped model → note-collections refactor. **Not in the MCP database.** | High |
| `038_planning-versus-writing…` | Gardener/architect methodology; where EditorMode names come from | Low (features) / High (method) |
| `039_story-design-process-insights` | Master summary of the design arc. **Not in the MCP database.** | High |
| `053_note-categorization-bootstrapping` | Track taxonomy derivation; v1→v2 migration strategy; why v1 and v2 stay separate | High |
| `077_organizing-changeling-lore…` | Track-definition refinements inside a mostly-story conversation | Medium |
| `091_categorizing-bonds-and-betrayal…` | Bond subject type; supersession discussion | Low-Med |
| `103_princess-and-the-kaiser-s-asoiaf…` | **The AI-usage model** — analysis not generation, bespoke over standardized | High (method) |

## Doc rules

Absolute dates only — never "this session", "recently", "currently".
**No live counts in prose** — not note/subject counts, not the test total. Anything that grows
goes stale the moment it is written; name the command that answers it (`get_stats`,
`dotnet test`) instead of its output. Point-in-time counts belong only in dated decision logs,
where they are a record of what was true then, not a claim about now.
When data semantics change, **three places update together**: the `storyplan-data` skill, this
file, and the MCP server's `ServerInfo.Instructions`.
Derived exports are regenerated, never committed (see `.gitignore`).
