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
  paths prefer structured and legacy-convert mechanically — flag, never guess.
- **Timeline x-axis**: `Subject.TheaterId` / `PlotPoint.TheaterId` (sentinel `0` =
  "(Unplaced)", same pattern as `Chapter.StoryId`). Theater assignment is authorial — never
  derive it from names. `Pivot` rows are authored years; eras are DERIVED as the gaps between
  pivots, never stored.
- **0 `Confirmed` notes in v2 is not a defect** — Audit is the only mode that can promote to
  Confirmed, and no audit pass has run. Surprising ≠ broken.

Schema detail and query recipes: `.claude/skills/storyplan-data/SKILL.md`.
**Live counts: `mcp__storyplanner get_stats`. Never hardcode counts in a document.**

## Build & run

.NET 10. `dotnet test tests/StoryPlanner.Tests` — 77 tests covering the MCP server's invariants
and `StoryPlanner.Core`'s export/scan/transform logic. Run before finishing any work in
`tools/` or `StoryPlanner.Core/`. Conventions and the known WPF-layer gap:
`.claude/skills/testing/SKILL.md`.

- **Brian's own instance of the app runs from a published copy, not `bin/Debug`.** Same reasoning
  as the MCP server below: `WindowedStoryPlanner/publish/WindowedStoryPlanner.exe` (a
  `dotnet publish -c Debug -o publish` output, gitignored — Debug on purpose, so a debugger can
  still attach and hit breakpoints) is what Brian actually launches day to day, deliberately
  separate from `bin/Debug/net10.0-windows/`, which is what `dotnet build`/`dotnet run` write to.
  This means Claude Code can freely build, run, or screenshot-verify WindowedStoryPlanner (see the
  `run` skill) without waiting on or colliding with Brian's own running instance, and vice versa.
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
- **stdout is JSON-RPC in the MCP process** — stderr only. This is why `ConversationImporter`
  (which uses `Console.WriteLine`) stays out of the server.

## Settled — do not propose alternatives

- The architecture above (no nav properties / FKs / indexes; polymorphic ownership; Type Object).
- **MCP design:** dumb tools; grep→fetch two-pass; no ranking; no fuzzy matching (the caller
  supplies vocabulary, the tool supplies alternation); hard flagged wall with count disclosure;
  corpora never joined.
- **Abandoned after being built:** note categorization, coverage-track suggestions, the
  41-report insight pipeline.
- `FEATURE-AUDIT.md` ⚪ lists features rejected in-conversation. **C1 (note supersession) is
  design-contested — ask before building.**

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
When data semantics change, **three places update together**: the `storyplan-data` skill, this
file, and the MCP server's `ServerInfo.Instructions`.
Derived exports are regenerated, never committed (see `.gitignore`).
