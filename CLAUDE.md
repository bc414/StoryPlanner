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
Claude Code does not do is form or offer **opinions** about story content: whether a flagged note
is resolved, whether a theme is well-evidenced, what a subject needs next, what prose to write.

Claude Code CAN form **hypotheses** about craft technique and framework design, grounded in
evidence (analyzed corpora, Brian's own writing, v1 archive patterns, provenance research).
A hypothesis is a testable prediction, not a judgment. "The corpus shows M4 correlates with
FID dominance" is a finding. "TLTT should use more FID" is an opinion. Findings and hypotheses
are Claude Code's domain. Opinions and decisions are Brian's. This is the scientific method
applied to narrative design: observe, hypothesize, test against evidence, report findings,
Brian adjudicates. See the `v3-buildout` skill for the framework synthesis methodology,
and `docs/v3-framework/` for hypothesis files and forward plans.

## What the tool must never do

**Retrieval, not suggestion.** Machine-proposed structure has been built and abandoned twice
here: note categorization, and the Conversation Reader's suggested subject×track coverage
(4,062 rows, `IsAdded = 0` on every one — "turned out to not be helpful"). A third attempt was
struck three times during the MCP server build under three different names.

> Tools answer *"what is here."* Never *"what should you do"* or *"what's interesting."*
> An obvious bottleneck in the data is not a mandate for a feature. When a feature idea encodes
> workflow, intent, ranking, or suggestion — stop and ask.

This rule applies to **MCP tools and planner features** — the instrument must not propose story
content. It does not prohibit **framework analysis**: testing hypotheses about craft technique
against corpora, identifying patterns in analyzed stories, or evaluating whether track definitions
are overfit. The three cuts above were machine-proposed *content* (AI categorizing notes, AI
suggesting coverage, AI writing summaries). Framework analysis (testing hypotheses about how
narrative technique works, grounded in evidence) is a different activity — see the synthesis plan.

**The coverage suggestion is now cut, not merely unused (2026-07-31).** No code path writes
`ConversationSubjectCoverage`, the reader's checklist column is gone, and a meta file's
`subjectsCovered` array is inert on import — a test asserts it writes zero rows. The tables and
their existing data survive, dormant, so `DeleteConversationAsync`'s cascade still works. The
block-level `HasDecisions` flag went with it (same judgment call, one layer down): column dropped.
Do not rebuild either. Conversation import no longer requires an AI pass at all — see below.

**The AI block summaries are cut too, and this time the data went with them (2026-08-11).** Same
judgment, the third time: machine-written per-block summaries "turned out to not be helpful." The
Cowork round trip that produced them is deleted (`ConversationContentExporter`, the *Export Checked
for Cowork…* button, `ExportConversationContentAsync`), a `_meta.json`'s summaries are inert on
import, and the existing text was blanked by the `wipe-block-summaries` DataOps op. The **column
stays and changes hands**: `ConversationBlock.Summary` is now Brian's own navigation note, typed in
the reader. That is the pattern to notice — the field was worth keeping, the machine filling it was
not.

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
referential integrity. **Application code IS the integrity system**, in three layers
(2026-08-02): id-based guard predicates in `ContentIntegrity` (Core, fixture-tested),
unconditional cascades in `StoryService.DeleteNote`/`DeleteLink` (a note takes its citation
rows; a link takes its owned narrative-property values), and the guarded, registry-syncing
`TryDelete*Async` deletes in `ContentDeleter.cs` — none of it decoration, and every UI delete
path routes through it. `PlanIntegrity.Check` is the after-the-fact auditor for the same
invariants. Indexes follow from the same premise: nothing queries the database after load.

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
  (`NoteExportRenderer.cs`) and the MCP server's ordinary tools both exclude them. Counts are
  disclosed; content requires the flagged tool family. `FlagReason` is itself a corpus Brian
  drafts into.
- **v1 and v2 never join.** Different organizing principles on purpose; no id correspondence,
  ~40% name overlap, and no join is wanted.
- **The scene graph is in v1** (1,125 links); v2 holds the taxonomy. Migrating it is Brian's
  future authorial work — matching v1 links to v2 subjects/plot points is categorization, not a
  mechanical operation, and no tool should propose the mapping. **The track definitions are
  final in shape** (the 2026-07-30 event/condition split of the six History tracks was a
  definition-row change, not a schema change — the design's whole point); the data is in flux.
  "Final in shape" means the Type Object schema is stable — tracks are data rows, not code
  classes. The definitions themselves (which tracks exist, their display questions, cognitive
  modes) are under active review as part of the v3 framework buildout
  (`docs/v3-framework/`; methodology: the `v3-buildout` skill). The schema supports this
  evolution by design.
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
  Confirmed, and no audit pass has run. Surprising ≠ broken. The v3 epistemic framework
  (see below) reframes Confirmed as "baselined" — Brian has reviewed and is comfortable
  acting on the content, but it remains challengeable. The `NoteState.Confirmed` enum value
  and Audit mode's promotion mechanism are unchanged in code; the semantic shift is
  framework-level, not schema-level. The v1 archive's `Confirmed` retains its distinct
  meaning (review closed, disposition not recorded) — the v3 vocabulary applies only to
  the working plan.
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
- **Source *text* is a fourth corpus, and it lives outside the `.storyplan` (2026-08-03).** The
  published material a citation points at — FiM episode transcripts, the fanfics' chapters, EaW's
  per-country flavour text (one unit per localisation key) — is ingested by
  `tools/StoryPlanner.SourceTexts` into a standalone `sources.db` (`STORYPLAN_SOURCE_TEXTS`,
  Brian's is `Desktop/TLTT Sources.db`, ~52 MB). **Not** in the `.storyplan`: the app eager-loads
  its whole database and has no use for prose it doesn't own, and every `VACUUM INTO` backup would
  carry it. The MCP server reads it; the WPF app never opens it. Joined to the plan **only by
  `(SourceMaterial.Name, SourceMaterialPart.Code)`** — never by id, since a reseed must not
  silently re-point text — and the ingest reports both directions of mismatch rather than zipping
  a short list onto a long one. Bodies are streamed per query and never cached; only a manifest is
  resident, which is also why this one file carries an index (the `.storyplan`'s "nothing queries
  it after load" premise is exactly what is untrue here). **Acquire as EPUB/structured, never as
  plain text**: the `.txt` Fimfiction export silently drops every italic, and this author sets
  internal monologue in them. Partial coverage is normal and is reported as coverage, never as a
  defect — the fic is ongoing, the movie was never transcribed. Same standing rule as everywhere:
  the tools show what the source says, and never rank Parts by likely yield, propose what to write
  from a passage, or propose a citation. **Splitting a Part is authorial**: P&K's two "Wind that
  Fanned the Flames" chapters are an ontology of role-vignettes, so their 22 sections were promoted
  to Parts of their own (`ch121-queens-scientist`, …) via a **P&K-scoped** seed config — never
  re-run the full `source-material.v2.json`, which would recreate the 14 FiM two-parters Brian
  merged in-app.
- **LINEAGE is the fifth corpus: the founding-era material, four source layers in ONE
  `lineage.db`, ONE tool family (2026-08-18; absorbed the 2026-08-16 gemini.db as its first
  layer; 2026-08-27: added the pre-AI Google Doc revision history as the zeroth layer).** The
  layers: the pre-AI Google Doc revision history (Apr 2025 – Jan 2026, `tools/StoryPlanner.GDocHistory`
  — 53 diffs between daily snapshots searched by default, 54 full snapshots retrievable under
  scope "snapshots" only, ids `gdoc:`/`gdoc-snapshot:`); the Gemini web-app conversations
  (Sep 2025 – Jun 2026) with their curated weekly reports; the early-2026 Google AI Studio chats
  **never imported into Conversations** (populations disjoint by construction — the ingest
  excludes any raw chat whose `<name>.json` sits in `Selected_Chats`, plus an authored `exclude`
  list for near-miss filenames); and NotebookLM captures. One tool family (`list_lineage` /
  `search_lineage` / `get_lineage`, source-prefixed ids) because the caller's question is
  lineage-shaped — *"where did this come
  from / when was X decided"* — not platform-shaped; `STORYPLAN_LINEAGE` replaced
  `STORYPLAN_GEMINI_CORPUS` in all three MCP configs, and the four gemini-specific tools retired
  with it. Same sidecar pattern as `sources.db`: bodies streamed, manifest-resident, guarded on
  `IsConfigured`; a shared `IngestRuns` ledger lets the tools disclose "never ingested"
  distinctly from "zero rows", since each ingest creates only its own tables. **The chain**
  (2026-08-17): founding chats → v1 archive (absorbed them, plus more) → v1 freeze →
  Conversations (post-freeze, unmined) → v2 plan; lineage is **opt-in archeology** — the default
  for any question is the working plan. **Provenance, never ground truth.** Per-layer caveats,
  all mechanical: gemini's export is damaged (its APPENDIX-D catalogues it) and giant plan-paste
  prompts are stubbed; AI Studio thinking chunks were stripped at ingest and a Drive-document
  turn is a placeholder (never captured ≠ withheld), system instructions searched only under
  `scope=system` (boilerplate dedup); **NotebookLM captures carry no timestamps, so a notebook's
  date is Brian's authored assignment in the ingest config** (year or year-month precision;
  undated = not yet resolved, flagged on every apply run — never inferred from content), and
  studio notes are title-only until a capture that opens them exists. Capture procedure: manual
  Ctrl+S of the notebook page with the chat panel scrolled fully to the TOP (history is
  server-side lazy-loaded; once loaded the DOM retains all turns) into
  `Documents/NotebookLM Captures/`, then a config entry with authored slug + date.
- **Claude Code transcripts are a sixth corpus — sealed-but-greppable, and deliberately NOT in
  the MCP server (2026-08-18).** `tools/StoryPlanner.CodeSessions` progressively ingests an
  **authored include-list** of `~/.claude/projects/` dirs (StoryPlanner, Gemini-Full-Analysis,
  Takeout-Scan, Fimfiction-Comments-Capture — where planner process knowledge was created;
  relevance is never auto-detected) into `codesessions.db` (Brian's is
  `Desktop/TLTT CodeSessions.db`). No MCP surface because the corpus split mirrors the Two AI
  roles: this is **instrument lineage** (how the planner was built, what was tried and cut),
  single-consumer — future Claude Code sessions query it directly with sqlite3; Desktop's story
  needs are met by the products those sessions shipped (reports in lineage.db, docs, briefs).
  Query recipes and schema: the `code-sessions` skill. **The ingest's write unit is one session
  (per-session replace on a bytes+mtime stamp) and there is no delete path** — Claude Code
  removes transcripts after its retention window (raised to 3650 days on 2026-08-17, after the
  30-day default silently ate the pre-mid-July era; a full snapshot sits in
  `Documents/ClaudeCode Projects Snapshot 2026-08-17/`), so a session absent from disk RETAINS
  its rows: the db is the durable record. Extraction is communication-vs-computation, Brian's
  policy of 2026-08-17: user/assistant text verbatim, subagent transcripts as their own sessions
  (`Kind='subagent'`, `ParentSessionId`), each tool call a mechanical one-liner stub, thinking
  and tool-result payloads dropped with char-count disclosure (`[tool result elided — N chars]`
  means never stored, not withheld), >20k-word user pastes stubbed like gemini's plan-pastes.
  Records keep `Uuid`/`ParentUuid` — a rewound session's branches stay visible; the DAG is never
  linearized. In the engineering authority order this corpus sits at the transcript level: the
  record of *why*, authoritative for *nothing* — FEATURE-AUDIT first, always.
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
- **Narrative properties now serve two purposes, and the second is the larger one (2026-08-04).**
  They were built as project-management bookkeeping (a gating `WorkPhase`, the Property Gaps
  report). They are now also **the densest structured story data in the working plan** — every
  Civilizational System sits somewhere on the five political axes, while most carry no scene link
  and no note is Confirmed. Both faces stay: Property Gaps is the bookkeeping view, the Boards tab
  is the content view. `NarrativePropertyValueDefinition.ColorHex` exists so a value reads as a
  colour wherever it is shown; it is authored, empty is a legal unfinished state, and nothing
  auto-assigns one from a palette.
- **Subject relations are authored edges between subjects, and are never inferred (2026-08-04).**
  `SubjectRelationDefinition` is a Type Object row scoped by `(SubjectDefinitionId,
  TargetSubjectDefinitionId)`; `SubjectRelation` is the assignment. **Subject→Subject only and
  deliberately not polymorphic** — both endpoints are Subjects and `RelationDefinitionId` resolves
  both types, so there is no `OwnerType` to omit and none may be added. `IsSingle` is the
  single-select invariant (`PlanIntegrity`: `subjectrelation.duplicate_for_single`);
  `FormsHierarchy` means acyclic-and-walkable and **requires the same subject type at both ends**,
  because a chain that changes type is not a chain — a single same-type relation can otherwise be
  legitimately cyclic (a symmetric "Rival of"), which is why the flag is explicit rather than
  derived. Absence of a row is unset, a legal permanent state; there is no sentinel target.
  **Edges carry no notes** — no fifth `OwnerType` — because why a succession happened belongs on
  the successor's Causality of Creation / History track, where that content already lives.
  Assignment is Brian's: the one succession recorded in the file (`Griffonian Republic` ← `Grover
  III's Enlightenment`, note 1630) skips three intervening regimes and shares no name token with
  its target, so **never propose an edge** from names, dates, or shared vocabulary. Nothing is
  seeded — the relation row is authored in the Definitions tab, like its prose.
- **A `PropertyBoard` is an authored set of properties under comparison (2026-08-04)** — the scope
  for the Boards tab's three independent views: C(n,2) pairwise grids, exact-match groups, and a
  generic subject tree.
  Membership is opt-in (`NarrativePropertyDefinition.PropertyBoardId`, null = on no board), which
  is what keeps a future bookkeeping property out of the political-axes board; a board must never
  acquire a property just because the scope matches. `IncludeUnsetBand` is **per board and changes
  the population, not just the layout**: off, a subject unset on either axis of a grid is absent
  from that grid entirely, so grid totals legitimately differ from each other and from the subject
  count — that is the configuration working, never missing data. The views share the board and the
  card control and **nothing else**: there is no ancestry overlay on the grids, no cross-
  highlighting, and the card renders identically in all three because it does not know where it
  is. Same standing rule: cells are occupancy, never a ranking, a score, or a coverage figure, and
  an empty cell is a fact about the world.
- **The Matches view groups subjects identical on EVERY board property (2026-08-04)** — the
  full-tuple collisions a pairwise grid structurally cannot show, since a grid crosses two
  properties and its cells mix subjects that agree on those two and differ elsewhere. Two rules,
  both deliberate: a subject unset on **any** board property is **not grouped at all** and is
  listed separately with its unset count (you cannot say two systems agree on all five when three
  are unknown), and singletons are shown in a trailing "alone on their coordinates" section rather
  than hidden — a system unique in the world is a fact, and hiding it is how you fail to notice one
  that should have had company. Ordering is largest-group-first, tie-broken by authored value
  order; that is counting and sorting authored data, the same category as the Progress tab, and
  **not** a score. Exact predicate only: no similarity measure, no near-miss ("differs on one
  axis"), no explanation of why a group exists, and never a proposal that something ought to join
  one. Grouping lives in `NarrativePropertyMatchGroups` (Core, Pure-tested).

Schema detail and query recipes: `.claude/skills/storyplan-data/SKILL.md`.
**Live counts: `mcp__storyplanner get_stats`. Never hardcode counts in a document.**

## Epistemic framework (2026-08-31)

All claims in the planner — story content, framework design, pipeline methodology — are
**hypotheses with evidence-relationship status**, not facts to be confirmed. This applies
uniformly: a fabula assertion ("Chrysalis controls the economy through MEFO bills") is a
hypothesis evidenced by canon compliance and materialist historicist analysis; a framework
assertion ("FID is the primary perception gap mechanism") is a hypothesis evidenced by the
112-story corpus. Both are revisable. Neither has a terminal "confirmed" state.

**Three epistemic states:** `untested` (no evidence examined), `evidenced` (evidence
gathered and currently supporting — thin or thick, always revisable), `challenged`
(unresolved counterevidence exists). Transitions are reversible: evidenced → challenged
when counterevidence arrives, challenged → evidenced when the challenge is resolved by
refining the claim or addressing the counterevidence.

**Baselining is progress tracking, not truth.** When Brian reviews a hypothesis or note's
evidence picture and judges it sufficient to act on, that is a **baseline** — a progress
checkpoint recording Brian's attention and judgment. Baselining does not make the claim
stronger, more important, or less challengeable. A baselined claim has identical
challengeability to an evidenced-but-not-baselined claim with the same evidence. Evidence
drives the framework and the story, not top-down labels.

**The method:** hypothesize → gather evidence → iterate. v3 tooling makes this
sustainable at scale: MCP for queryable evidence, skills for consistent methodology,
external corpora for voice separation. Whether this cycle was already Brian's natural
workflow in v1 (and v2's prescriptive staging moved away from it), or whether v3
genuinely introduces it, is itself a hypothesis to be tested against the lineage
evidence. The `v3-buildout` skill governs the framework buildout; these principles
govern the planner at every level.

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
- **Source-text ingest** (`tools/StoryPlanner.SourceTexts`) is offline and separate from DataOps
  because it writes `sources.db`, not a `.storyplan` — it opens the plan `Mode=ReadOnly` purely to
  learn the Work/Part spine. `dotnet run --project tools/StoryPlanner.SourceTexts -- <config.json>
  [--apply] [--work NAME]`; dry run prints the full chapter↔Part mapping and both directions of
  mismatch, and refuses to write if anything is unresolved. Re-ingest replaces a Work wholesale, so
  a re-download can shed chapters that vanished upstream. `STORYPLAN_SOURCE_TEXTS` is **optional**
  in all three MCP configs — absent, the source-text tools say so and the rest of the server is
  unaffected.
- **Lineage ingest is THREE tools writing ONE `lineage.db`, each replacing only its own tables
  (2026-08-18; gdoc layer added 2026-08-27).** The Google Doc revision history layer:
  `dotnet run --project tools/StoryPlanner.GDocHistory --
  tools/StoryPlanner.GDocHistory/configs/gdoc-history.json [--apply]` (reads the 54 merged
  daily snapshots from `source_material_references/TLTT Story Plan Revision History (merged)/`,
  computes line-level diffs using DiffPlex; diffs searched by default, snapshots under scope
  "snapshots" only). The gemini layer: `dotnet run --project tools/StoryPlanner.GeminiCorpus --
  <gemini_markdown_dir> <lineage.db> [--apply]` (reads `gemini_markdown/corpus_index.json` +
  entry files + the sibling `story_development_report/`; source corpus static, re-run replaces
  its tables). The AI Studio + NotebookLM layers: `dotnet run --project tools/StoryPlanner.Lineage
  -- tools/StoryPlanner.Lineage/configs/lineage.json [--apply] [--source aistudio|notebooklm]`
  — dry run names every included/excluded/dropped chat so the population is eyeballed before a
  write, refuses on any unparseable non-ignored candidate, and every apply run re-prints the
  NotebookLM date status (undated notebooks are a standing flag, resolved only by an authored
  `authoredDate` in the config). Each ingest appends to the shared `IngestRuns` ledger.
  `STORYPLAN_LINEAGE` is **optional** in all three MCP configs — absent, the lineage tools say
  so and the rest of the server is unaffected.
- **Code-sessions ingest** (`tools/StoryPlanner.CodeSessions`) writes `codesessions.db`, which
  the MCP server never opens (see the sixth-corpus bullet above).
  `dotnet run --project tools/StoryPlanner.CodeSessions --
  tools/StoryPlanner.CodeSessions/configs/code-sessions.json [--apply] [--project NAME]` —
  progressive: per-session replace on a `(SourceBytes, SourceMtimeUtc)` stamp; dry run prints
  per-project new / changed / unchanged / **absent-but-retained** tallies. Re-run it whenever the
  archive should catch up with recent sessions; sessions aged off disk keep their rows.
- `.storyplan` is raw SQLite in **WAL mode**. Reads never block the running app. The main file's
  **mtime does not advance on write** — change detection uses `PRAGMA data_version`.
- **`StoryService` is not read-only:** `OpenProjectAsync` runs `MigrateAsync()` (upgrades the
  schema in place — since 2026-08-02 it takes a `VACUUM INTO` snapshot into `Backups/` first and
  **refuses to migrate if the backup fails**) and silently no-ops if a project is already
  loaded. `SaveAsync()` is a bare `SaveChangesAsync` (the `.md`/`_stats.csv` litter it used to
  write was removed 2026-08-02), and the app also saves on exit. The MCP server bypasses it
  with `Mode=ReadOnly`.
- **stdout is JSON-RPC in the MCP process** — stderr only. Never `Console.WriteLine` from code the
  server can reach. (`ConversationImporter` used to be the standing example; as of 2026-07-31 it
  reports through a returned `ConversationImportResult` instead of printing, but it is still a
  write path and the read-only server has no business calling it.)
- **Conversation import needs no AI pass, and no longer offers one (2026-07-31, cut 2026-08-11).**
  One live route: *Scan Claude Export… → Import Checked Directly* puts raw blocks straight in the
  reader. *Import from Folder (legacy)…* survives for `_content.json` folders already on disk —
  nothing produces one any more. A `_meta.json` is parsed and **entirely inert**: its `ArcSummary`
  and per-block `Summary` write nothing, exactly like `subjectsCovered` since 2026-07-31, and a
  test asserts it. **An import writes no authored field** — not `Summary`, not `BlockState`; a
  re-import refreshes the transcript and nothing else.
- **A block `Summary` is Brian's own navigation note (2026-08-11).** It is typed into the reader's
  middle column, two-way bound and committed on focus-leave — his words, not a machine's, which is
  a different citation status from `RawContent` when one turns up in a search. The AI-written
  summaries that used to fill this column were wiped by the `wipe-block-summaries` DataOps op
  ("not helpful"): same judgment as the coverage-suggestion cut above, one feature over. Empty is
  ordinary and permanent — most blocks will never carry a note — and **never substitute an excerpt
  for an absent one**. `Conversation.ArcSummary` is the frozen remainder: still displayed
  read-only, never written by anything again.

## Settled — do not propose alternatives

- The architecture above (no nav properties / FKs / indexes; polymorphic ownership; Type Object).
- **MCP design:** dumb tools; grep→fetch two-pass; no ranking; no fuzzy matching (the caller
  supplies vocabulary, the tool supplies alternation); hard flagged wall with count disclosure;
  corpora never joined.
- **Abandoned after being built:** note categorization, coverage-track suggestions, the
  41-report insight pipeline.
- `FEATURE-AUDIT.md` ⚪ records features rejected in-conversation or closed as already-resolved at the time of writing — its assertions are testable against current evidence, not settled.
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
`tools/StoryPlanner.Mcp` (server work) · `docs/CONVERSATION-READER-SPEC.md` (historical — see
its banner for drift).

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
