# Story Planner — Outstanding Feature Audit

**Date:** 2026-07-04
**Method:** Nine long Claude/Gemini design transcripts in `Additional Potential Conversations with Specs/` were mined for discussion *about the Story Planner tool itself* (as opposed to the fanfiction content that dominates them), then each proposed feature was checked against the current `master` codebase (`StoryPlanner.Core` + `WindowedStoryPlanner`) to judge implementation status.

> **Note added 2026-07-28.** The transcripts referenced throughout now live in `docs/design-conversations/`. Two source files cited below as grep targets — `ModelClasses.txt`, `Models.txt`, `ViewModels.txt` — were **deleted** that day: they were snapshots of the *pre-TotalRework* v1 architecture and were actively misleading about the current schema (see conv 020, which introduces `ModelClasses.txt` as *"the old model classes … not to be confused with the new design"*). Every citation of them in this document is a claim about **absence** — that a field exists only in old dumps and *not* in the live code — so the claims still hold; only the artifacts are gone. Verify absence against the code itself, or `git show` the deleted files from history.

> **Note added 2026-07-28 (later the same day).** A1 (chapter-level multi-story grouping) shipped —
> see its entry in Section A below for what changed. Per-note narrative tagging, the other half of
> A1's original ask, was deliberately deferred and remains outstanding.

> **Note added 2026-07-30.** C1 (note supersession) is **closed, not built** — it was resolved by
> track design that already shipped, and the original entry's reading of the transcripts as
> "contested" was wrong. See its entry in Section C. This is the first item to close as ⚪ by
> *existing design* rather than by in-conversation rejection; the legend was widened accordingly.

> **Note added 2026-07-31 (later the same day).** D1 (POV/focal-character on `PlotPoint`) shipped —
> see its entry in Section D for what changed, including how it also delivered 053 blocks 262-263's
> conditional gap-track display.

> **Note added 2026-07-31.** This document's "SourceMaterial tagging — done" claim (below and in
> Section G) was **shipped in code and dark in data**: the June 2026 rework built the full UI
> (Sources tab, per-note picker, cross-cut detail window) but the migration defaulted
> `SupportsSourceMaterial` to `false` on every track and nothing ever ticked it — 0 of 112 tracks,
> 0 `SourceMaterials` rows, 0 tagged notes across every real file. It has since been rebuilt as a
> two-tier Work/Part coverage tracker (not a flat tag) with a seeded, pre-enumerated Part set, a
> negative-space ("untouched") view, and `SupportsSourceMaterial` now enabled on the six
> `TrackType.Canon` tracks. See CLAUDE.md's "Source material is a coverage tracker, not a tag."

**Bottom line:** The **data-model refactor** those conversations argued for is substantially *shipped* — the unified polymorphic `Note`, data-driven `SubjectDefinition`/`NoteTrackDefinition` tracks, the `TrackType` cognitive-layer taxonomy, `Unset/Flagged/Confirmed` note states, per-editor-mode track ordering, `WorldDate`/`Theme` tagging and `SourceMaterial` coverage tracking, `PlotPointSubjectLink` as a first-class noteable, F-key track assignment, and the anchor+scope export resolver all exist. What remains outstanding is almost entirely the **layer built on top of that model**: aggregation/navigation views, multi-story scoping, the scene-architecture (perception-gap) layer, note-provenance lifecycle, and a handful of definition-system refinements.

Legend: 🔴 Outstanding · 🟡 Partial · 🟢 Implemented (listed only where a conversation's "ask" is worth confirming as done) · ⚪ Deliberately rejected in-conversation, or resolved by existing design without new code

---

## Priority summary

| # | Feature | Status | Signal (convos) | Structural? |
|---|---------|--------|-----------------|-------------|
| A1 | Multi-story / narrative scoping (`StoryId` on content, `SupportsNarrativeTag`) | 🟡 | 015, 020, 053, 077, 091, 038/039 | **Enabler** |
| B1 | Master timeline view (aggregate `WorldDate` notes) | 🟡 | 015, 019, 053 | View |
| B2 | Global entity search (tab is a stub) | 🟢 | 038/039 | View |
| C1 | ~~Note supersession / preserving retconned lore~~ | ⚪ | 091, 015, 019, 038/039 | Resolved — no code |
| C2 | Stage-0 "accumulation" inbox / `IsIncorporated` flag | 🔴 | 053, 038/039, 015 | Model |
| D1 | POV / focal-character designation on `PlotPoint` | 🟢 | 015, 019, 053 | Model |
| D2 | `GapType` enum + perception-gap scene fields | 🔴 | 019, 015, 053, 038/039 | Model |
| B3 | Per-entity completion / scene-readiness dashboard | 🔴 | 019, 038/039 | View |
| E1 | Theme dialectical model (antithesis + Support/Counter/Refutation) | 🔴 | 053 | Model |
| E2 | Subject clusters surfaced in UI (`SubjectCluster` orphaned) | 🔴 | 077, 053 | Model+View |
| E3 | Per-mode track *visibility/collapse* (not just ordering) | 🟢 | 077 | Model |
| E4 | "Synthesizes Links" source-vs-aggregate track flag | 🔴 | 077 | Model |
| E5 | Chapter primary-thread spine / chapter-subject link tracks | 🔴 | 015, 053 | Model |
| C3 | `ValidationTier` opt-in enforcement system | 🔴 | 038/039 | Model |
| C4 | `LastRevisedDate` / revision tracking on notes | 🔴 | 038/039 | Model |
| B4 | Cross-thread "synchronized ratchet" chapter view | 🔴 | 015 | View |
| F0 | Conversation pipeline: cut suggested tracks, summaries optional | 🟢 | (Brian) | Model + UX |
| F1 | Conversation Reader: per-block subject mentions | 🔴 | (spec) | Model |
| F2 | Conversation Reader: cross-conversation subject view | 🔴 | (spec) | View |
| F3 | Conversation Reader: bulk multi-select state ops | 🟢 | (spec) | UX |
| G1 | Session export presets (degree-of-separation scoping) | 🟡 | 019 | Mostly done |

---

## A. Multi-story / narrative scoping — highest-signal gap

**A1 — 🟡 Multi-story project & per-note narrative tagging.** *Discussed in six of the nine transcripts* (015 blocks 219-229, 020, 053 blocks 37-42/78-79/94-96, 077 block 7, 091 blocks 2/16/31/33, 038/039 "StoryScope"). The recurring need: the same planner holds a trilogy (TLTT → Minette → Chrysalis), a Subject/Bond "lives in" one story but is inherited by prequels/sequels, and reveal/recontextualization notes need **one note per narrative** — implying a per-track `SupportsNarrativeTag` boolean and a narrative selector on notes, parallel to `WorldDate`.

- **Current state (shipped):** `Story` (`Models/Story.cs`: `Id`, `Title`, `Abbreviation`, `ColorHex`,
  `OrderIndex`) is now a real, DbSet-backed table with a `Chapters.StoryId` FK (`0` = the permanent
  "(Unassigned)" sentinel, not a missing reference). `Chapter.OrderIndex` is per-story, not the flat
  book-wide sequence it was. A `StoryPlanner.DataOps` one-time op (`AssignStories`) backfilled both
  real files: v2 into six stories, the v1 archive into three (including a paratext story for its
  "Blog Posts" chapter — deliberately not the sentinel, since it's a settled fact, not undecided).
  WPF gained a Stories tab and a Move… dialog for cross-story chapter repositioning; the MCP server
  gained `list_stories`/`get_stories`, story-qualified chapter labels, story-grouped inventories, and
  a `count_notes_plan` "story" dimension — v1's and v2's Stories are never joined or cross-referenced.
- **Still outstanding:** per-note `SupportsNarrativeTag` / narrative tagging — deliberately deferred
  (Brian's call when this shipped); `OwnerType.Story` and story-level notes are out of scope by design
  (Story stays a container). See `docs/design-conversations/` for the original per-note-tagging ask.

---

## B. Aggregation & navigation views

**B1 — 🟡 Master timeline view.** *(First pass shipped 2026-07-30 — overnight build, pending
Brian's review; see `docs/TIMELINE-IMPLEMENTATION-DECISIONS.md`.)* Shipped: structured world
dates (`WorldDateStart*/End*` on `Note`, event-only `Fabula*` + `TheaterId` on `PlotPoint`,
`Subject.TheaterId`), the event/condition track split (`SupportsWorldDateEnd`, 6 History tracks
→ 12 via the `convert-world-dates` DataOps op), `Theater`/`Pivot` entities (eras derived, never
stored), a Timeline tab (`TimelineViewModel`/`TimelineView`: y = world time, x = theaters,
condition bars with extent-proportional height, fixed-size event markers, year-precision count
glyphs, pixel-space lane packing via `Core/Timeline/LanePacker`, pivot rules, snapshot-not-live
by design), a triage panel for undated items with confirm-style assignment, and notation
editing in `NoteView`. Era-range collapse and drag-from-triage both shipped later the same
evening (2026-07-30 — see `docs/TIMELINE-IMPLEMENTATION-DECISIONS.md`; this entry was stale
until 2026-07-31). Viewport persistence shipped 2026-07-31 via a new `UiSettings` key/value
table *inside the `.storyplan`* — Brian's explicit mechanism choice over the
`%LOCALAPPDATA%` JSON the timeline backlog had proposed ("settings … contained in the sqlite
db"); zoom, center year, and the theater/era collapse sets restore on load
(`Core/Timeline/TimelineViewState.cs`, debounced writes through `SaveAsync`). Still
outstanding: the DataOps ops actually being applied to the real files (rehearsed on a copy
only — Brian's call).

**B2 — 🟢 Global entity search.** *(Shipped 2026-07-30.)* The "Global Search" tab in
[MainWindow.xaml](WindowedStoryPlanner/Shell/MainWindow.xaml#L66-L68) now hosts a real
`GlobalSearchView`/`GlobalSearchViewModel` (`WindowedStoryPlanner/Search/`), backed by a new
Pure-tier-tested matcher, `StoryPlanner.Core.EntitySearch`
(`StoryPlanner.Core/Search/EntitySearch.cs`). It searches `Subject.Name/Description/Abbreviation`,
`PlotPoint.Title`, `Chapter.Title`, `Theme.Name/Proposition`, `SourceMaterial.Name/Description`,
and — the actual gap this closed — **`Note.Content`/`FlagReason`**, which nothing else in the app
searched before (`ExportViewModel.RebuildSearchResults()` remains anchor-picking only, over
names/titles). Results are grouped by type, show a snippet, and open the owning entity's window
via the existing `IWindowManager` on double-click (a note opens its owner, including
`PlotPointSubjectLink` notes with the link pre-selected). **Deliberately includes flagged notes'
content in full**, unlike `NoteExportRenderer` and the MCP server's `Engine.Search` — this is the
author reading their own data in-app, not an LLM-facing surface; see `EntitySearchTests` for the
tests pinning that as intentional. Story is out of scope (no `OwnerType`, no detail window).

**B3 — 🟡 Per-entity completion / scene-readiness dashboard — rollup half shipped 2026-07-31, scoring half still declined.** The **Progress** tab renders per-subject and per-track Confirmed/Unset/Flagged counts, sortable, snapshot-on-load with an explicit Refresh (`Progress/ProgressViewModel.cs`), and relabels the Confirmed column in archive mode where the state means "review closed", not "stable". What is *not* built, and is not a gap to close: the derived "Completion Profile" / readiness score from 019 blocks 61/69. A number that says a scene is ready to draft is a judgment, and the same shape as F4 below. Counting and sorting authored data is presentation; scoring it is not.

**B4 — 🔴 Cross-thread "synchronized ratchet" view.** A per-chapter view showing each active thread's goal-trajectory distribution so stagnant threads stand out (015 block 521). No such view; the `GoalTrajectory` payload it assumed is no longer a structured field (payloads became notes).

---

## C. Note lifecycle & provenance

**C1 — ⚪ ~~Note supersession / preserving retconned lore.~~ Resolved 2026-07-30 — closed as track design, no code to write.** The original entry read the transcripts as disagreeing. Re-read in order, they converge, and the resolution is already live in the data.

- **015** proposed a `Retcon` NoteTrack (block 6; block 12: *"superseded by retcons should be marked Retcon … rather than deleted"*) — then Brian overrode it **in the same conversation** at block 19: *"the retcons to layers 1-3 can become the layer 4 initial perception … Seems like I've already been doing this."* Block 20 names the method: *"The retcon does not discard the naive version; it promotes it to Layer 4 material."*
- **091 block 24** is the same instruction in Brian's words: *"newer assertions can trump old ones but old ones that got displaced should be preserved as reader prior belief."*
- **019** is the only genuine reversal, and it lands in the same place: block 55 argued for a `Superseded` fourth state, block 57 retracted it — *"the old claim is not retained in the World Mechanics track. It is moved to the Project Notes track of the same entity as a Revision Directive."*

**Shipped as definition rows, per the Type Object premise** — displaced lore lands in two existing homes, neither of which is a supersession primitive:

- `Reader Prior Belief Update` (TrackType `WorldInference`) and `Reader Prior Belief Clash` (TrackType `ThematicEvidence`, `SupportsTheme`) — one pair per subject type, all scene-link scoped (`OwnerType=3`), track ids 89-93/97 and 11/28/39/49/59/69. This is 015's "Layer 4" and 091's demotion target.
- `Garden Notes` / `Craft Notes` (TrackType `NotesToSelf`) — 019 block 57's "Project Notes track" under a different name.

**The pairing is verifiable in `TLTT v2.storyplan`:** subject `EEEE!` carries Garden Note 2137, a revision directive (*"original EaW EEEE! was just a vague activist group in a pub, and I probably elevated them to a generic union … but now EEEE! is specifically a machinist guild…"*), while its link at *"Rarity attends the EEEE! meeting"* carries the displaced version promoted to reader-facing material — note 2133 (*"Readers expect an angry mob of workers. They get a highly organized guild of engineers"*) and note 2066. Same shape on Celestia, the Griffonian Republic, the Universal Translator, Rainbow Dash. Corroborating negative: `search_plan` for `retcon|supersed|previously believed` returns **0 matches** across v2 — no shadow vocabulary waiting for a feature.

**Do not build:** (a) a `Superseded` `NoteState` — rejected at 019 block 57; (b) a `Retcon` track — superseded by 015 block 19-20; (c) a supersession *link* between notes — this appeared in **no transcript**, it was invented by the original audit entry, and it would add machine-tracked provenance the method does not use.

*Not a gap:* the Reader Prior Belief tracks are scene-link scoped, so a displaced subject-level `World Truth` note cannot be *moved* into one. It is rewritten against the scene where the belief updates, and the Garden Note holds the audit trail. That is the method working as designed.

**C2 — 🔴 Stage-0 "accumulation" inbox / `IsIncorporated` flag.** A pre-structural inbox for raw hypotheses/research directives distinct from Stage-1 truth, tracked by an `IsIncorporated` marker (038/039 P23-25, 053 blocks 21-24, 015). **Concrete artifact:** the flag was scaffolded and abandoned — `IsIncorporated`/`NeedsFurtherAnalysis` appear only as **commented-out dead code** in [StoryService.cs:283-284](StoryPlanner.Core/StoryService.cs#L283-L284). `Note` has no such field today.

**C3 — 🟢 `ValidationTier` opt-in enforcement — shipped 2026-07-31 as data, not an enum.** The 019-block-123 design (a `ValidationTier` enum: None / StructuralRecord / ExperienceDesign / SceneBlueprint) was superseded rather than built: the tiers are `WorkPhase` **rows** — configurable, reorderable, named by Brian — and `NarrativePropertyDefinition.GatingWorkPhaseId` (nullable, null = never gates) names the phase at which an unset value is reported. Same opt-in property the enum was for, reached by data entry instead of a migration, consistent with the Type Object rule the rest of the configuration layer follows. Two differences from the transcript design, both deliberate: **it never blocks** (`CanPromoteToConfirmed` is untouched — the report is the whole feature), and a phase's own completion criteria are note-state counts (`RequiresZeroFlaggedNotes` / `RequiresZeroUnsetNotes`) rather than a per-entity profile. Do not add a `ValidationTier` enum.

**C4 — 🔴 `LastRevisedDate` / revision tracking.** Record when a claim was revised as upstream dependencies change (038/039 P24). `Note` has only a plain `LastModified` timestamp — no revision history or `LastRevisedDate`.

---

## D. Scene-architecture (perception-gap) layer

This is the biggest *conceptual* body of work in the transcripts (esp. 019, the "character-reader perception gap" session). Most of it was ultimately resolved to live in **note tracks** rather than hardcoded fields — so the generic machinery can express it — but the specific tracks aren't configured and a few genuinely-discrete fields remain unbuilt.

**D1 — 🟢 POV / focal-character on `PlotPoint`.** A nullable `FocalCharacterId` making POV the pivot of scene design (015 blocks 1308/3138, 019 blocks 8-10/37/69, 053 blocks 262-263 — 2026-07-31). Shipped as `PlotPoint.FocalCharacterId` (`int?`, null = undesignated), gated to candidates via `Subject.IsPovCharacter` (105 Character subjects made an unfiltered picker unworkable), and wired to 053's conditional-display proposal: `NoteTrackDefinition.IsFocalCharacterOnly` hides a scene-link track everywhere except the POV character's own link, with existing notes on a track always kept visible regardless. Surfaced in `NoteExportRenderer` (`*POV: {name}*` under the scene heading) and `get_plot_points_plan`/`get_plot_points_archive` (`focal:` line) and `get_track_definitions` (`supports: focalCharacterOnly`). Not built: the two Category-B "observed character" fields 019 block 37 separately proposed for `PlotPointSubjectLink` (`ObservedPerception`/`ObservedGapType`) — those are a content question (which tracks capture that content), not a schema gap, and are out of scope here.

**D2 — 🔴 `GapType` enum + perception-gap fields.** `GapType` (Ironic / Tragic / Closing / Aligned) was repeatedly specified to stay a **discrete enum field**, not a note (019 blocks 18/37/65-69). Also the named field/track set: `POVBelief`, `POVReaderPerception`, `ReaderState`, `FIDAnchors`, `Stakes`, `Outcome`, `ArcStatement`, `StructuralTruth`, `EvidenceDeposited`, `ReaderHypothesis`, plus `RevealLayer`/`TLTTVisibility` link enums (015 blocks 778/2467). None exist in code (`grep GapType|POVBelief|ReaderState|RevealLayer` → transcripts/`ModelClasses.txt` only). Decide per item: promote to a discrete field vs. seed as a `NoteTrackDefinition` row.

**D-note — rich PlotPoint metadata** (`DraftStatus`, `CoreDriver`, `TensionPhase`, `ConflictType`, `Presentation`) is assumed throughout 019 but appears only in legacy `StoryFileService.cs` import parsing, not on the live `PlotPoint`. Largely superseded by the payloads-as-notes decision; flag only if completion-signals (B3) need `DraftStatus`.

---

## E. Track / definition-system refinements

**E1 — 🔴 Theme dialectical model.** `Theme` should be a singleton proposition **plus antithesis**, with a three-value enum (**Support / Counterargument / Refutation**) on theme-evidence links (053 blocks 35-36). `Models/Theme.cs` has only `Id/Name/Proposition` — no antithesis field, no such enum. *(The theme cross-cut **view** itself is done — `ThemeDetailViewModel : TaggedNotesViewModelBase` filters all notes by `SelectedTheme`.)*

**E2 — 🔴 Subject clusters in UI.** Grouping headings like "Night Economy" over multiple subjects (077 blocks 18/32, 053). `Models/SubjectCluster.cs` (`Id/Name/ColorHex`) exists but is **orphaned** — no `Subject.ClusterId`, no `DbSet`, no UI (`grep SubjectCluster|ClusterId` → model file only).

**E3 — 🟢 Per-mode track visibility/collapse.** *(Shipped 2026-07-31.)* Five `HiddenIn*Mode`
booleans on `NoteTrackDefinition` (one per `EditorMode`, default false = visible, so the
migration changed nothing until opted into), edited as checkbox columns in the definitions
grid. A track hidden in the current mode — even one with notes — is demoted to a collapsed
"Hidden in this mode" expander after the empty-track panel
([NarrativeElementFullView.xaml](WindowedStoryPlanner/Editing/NarrativeElementFullView.xaml)),
Brian's chosen variant over full hiding: nothing is ever unreachable without a mode switch.
The Unassigned track is always visible; `convert-world-dates` copies the flags to condition
twins; the flags are deliberately *not* exposed via the MCP server (display preference, like
the per-mode order fields).

**E4 — 🔴 "Synthesizes Links" flag.** Distinguish fill-first source-of-truth tracks (Ontology/History) from tracks that aggregate upward from link notes — a suggested `SynthesizesLinks` boolean (077 blocks 26-28). No such field; `CanEditInAuditMode` only loosely proxies it.

**E5 — 🔴 Chapter primary-thread spine / chapter-subject link tracks.** Mark each chapter's primary narrative thread (015 block 1717) and/or add a Chapter↔Subject link scope with its own tracks (053 block 69). `Chapter` remains `Id/Title/OrderIndex`; no chapter-subject junction.

---

## F. Conversation Reader — spec vs. build

The Conversation Reader (per [CONVERSATION-READER-SPEC.md](CONVERSATION-READER-SPEC.md)) is **substantially implemented**: three-column reader window, `BlockState` context menu **and** U/S/F/D keyboard shortcuts ([ConversationReaderWindow.xaml.cs:57-71](WindowedStoryPlanner/Conversations/ConversationReaderWindow.xaml.cs#L57-L71)), WebView2 markdown rendering, folder import, derived conversation states, and dashboard stats (`ConversationLibraryViewModel`). Outstanding spec items:

- **F0 — 🟢 Pipeline redesign: AI-suggested tracks cut, summaries made optional.** *(Shipped 2026-07-31.)*
  The redesign F4 was blocked on. Two changes, both irreversible in intent:
  - **Suggested subject×track coverage is cut.** `ReplaceSubjectCoverageAsync` is gone, the reader's
    fourth column (the checklist whose ticks created notes) is gone, the library's subject filter is
    gone with it, and a `_meta.json`'s `subjectsCovered` array is now inert on import —
    `ConversationImporterTests` asserts it writes zero rows, so reconnecting the path fails a test.
    The two tables and their 1,472 / 4,062 rows survive **dormant** (deletion cascade still needs
    them). `ConversationBlock.HasDecisions` went too — same judgment call one layer down, column
    dropped by migration `DropBlockHasDecisions`.
  - **Summaries are optional.** A `_content.json` with no meta partner imports instead of being
    skipped, and *Import Checked Directly* on the Scan Preview loads conversations straight from
    the raw Claude export — no Cowork round trip, no summaries. The Cowork path survives as pure
    enrichment: `ArcSummary` + per-block `Summary`, navigation only. Meta never destroys — a
    content-only re-import preserves earlier summaries and all block triage state.
- **F1 — 🔴 Per-block subject mentions.** The spec's `BlockSubjectMention` entity (per-block subject tagging, for filtering blocks by subject within a conversation) was never built. **Re-read F0 before reviving this:** it is per-block *coverage*, one level finer than the thing that was just cut, and the spec's motivation for it was routing. If it ever returns it must be authored, never proposed.
- **F2 — 🔴 Cross-conversation subject view.** "What did every conversation say about Applejack's Characterization?" — spec §Cross-Conversation Subject View + `SubjectCoverageView.xaml`. Not present (no such view/VM; depended on F1 and on the now-cut conversation-level coverage).
- **F3 — 🟢 Bulk multi-select state ops.** *(Shipped 2026-07-31.)* Both block columns are
  `SelectionMode="Extended"` (Shift/Ctrl-click, per the spec's ask); the context menu, U/S/F/D
  keys, and F1–F4 all apply to the whole selection when the target block is part of one, via a
  single bulk path (`ConversationViewModel.ApplyStateToSelectionAsync`) that refreshes stats
  and saves exactly once. A block outside the selection stays single-target.
- **F4 — ⚪ Dashboard "subjects with unresolved material" metric — declined 2026-07-30, and the
  ground it stood on is now gone.** Brian, when offered it for an overnight build: "I need to
  redesign that conversation pipeline and cut out the AI suggested tracks anyway." That redesign
  shipped as F0 on 2026-07-31, which resolves the blocker by **deleting the data source** — the
  metric would have been computed from `ConversationSubjectCoverage`, now frozen and never
  written. This stays ⚪. It was a ranking/attention metric ("what needs work"), which is the
  suggestion pattern CLAUDE.md forbids regardless of what table it reads.
- **F5 — 🟢 Narrative properties activated; the four political axes on Civilizational Systems.**
  *(Shipped 2026-07-31.)* The `NarrativeProperty*` tables had existed since `TotalRework`
  (2026-05-06) with **0 rows in every file**, no UI binding them, and three latent bugs in
  `NarrativePropertyViewModel` that would have thrown on opening any entity the moment a definition
  row existed — plus a `ProjectLoader` ordering bug that populated the value-definition registry
  *after* the view models that read it. All fixed. Now shipping: the picker strip in the entity
  editor, authoring grids in a new Definitions sub-tab, three refusing delete guards
  (`ContentIntegrity` + `ContentDeleter`), two `PlanIntegrity` codes, a `list_narrative_properties`
  MCP tool plus a `properties:` line on `get_subjects_plan`, and the `seed-narrative-properties`
  DataOps op. First content: Human Capital / Governance / Boundary / Social Contract, two poles
  each, on the 37 Civilizational System subjects — transcribed from conv:25 (2026-04-23).
  **The op writes zero `NarrativePropertyValue` rows and no prose**; both are pinned by tests.
  **Why this is not F4.** The **Property Gaps** tab reports which entities lack a value — but
  against a requirement Brian authored (he writes the property, he sets its gating phase), which is
  the same category as `PlanIntegrity`'s violation codes, not an inference about what deserves
  attention. F4 died because it computed attention from `ConversationSubjectCoverage`, i.e. from
  *AI-suggested* data; the audit itself records that deleting that data source resolved it. The
  line that matters is **never derive or propose a value** — that would be importing analysis.
  Counting, grouping, and sorting authored data is presentation, and the app already sorts subjects
  by unconfirmed count in archive mode.
- **F6 — 🔴 The Definitions tab's *existing* deletes have no guards.** Noticed 2026-07-31 while
  adding guards for the new rows, and deliberately left alone rather than fixed in passing.
  `DefinitionsEditorViewModel.DeleteSubjectDefinition` and `DeleteNoteTrackDefinition` remove a
  definition row directly and never consult `ContentDeleter` — deleting a `SubjectDefinition`
  silently orphans every subject and every track pointing at it, and deleting a
  `NoteTrackDefinition` orphans its notes. `PlanIntegrity` would report the wreckage afterwards;
  nothing prevents it. The fix is the shape now used by the narrative-property deletes: id-based
  predicates in `ContentIntegrity`, a `TryDelete*Async` wrapper that refuses, and a status line so
  the refusal is visible. Low urgency — these are rarely-used destructive buttons on a
  configuration screen — but it is a real hole in the referential-integrity story, not an
  accepted cost.

---

## G. Largely-done items worth confirming

- **G1 — ⚪ Session export presets — remainder dropped 2026-07-30.** 019 block 63 wanted purpose-scoped exports with degree-of-separation entity scoping and track-type exclusion. Most of this **is built**: [ExportConfiguration.cs](StoryPlanner.Core/Export/ExportConfiguration.cs) has `Anchors`, `Scope`, `ChapterFrom/To`, `IncludedTrackTypes`, and [ExportResolver.cs](StoryPlanner.Core/Export/ExportResolver.cs) implements a real 0/1/2-degree expansion (anchors → links → other-end entities). The only missing piece was *named presets* as saved configurations, and Brian declined it when offered: "I don't need any more work on the markdown exports since I have the mcp server now." The MCP server superseded the export workflow the presets were for.
- **F-key track assignment** (038/039 P46) — **done**: `DefinitionsEditorViewModel.FunctionKeyOptions`, `NoteTrackSectionView.xaml.cs` `PreviewKeyDown`, `App.xaml.cs` `OnGlobalKeyDown`.
- **Cross-cut tag views** for Theme and SourceMaterial (015) — **done** via `TaggedNotesViewModelBase` (`ThemeDetailViewModel`, `SourceMaterialDetailViewModel`, `SourceMaterialPartDetailViewModel`). SourceMaterial's was built 2026-06-21 but unreachable in practice until 2026-07-31 — see the note near the top of this document. **Unified 2026-07-31:** all four now render through one `Common/CrossCutNoteListView` (the item template had been copy-pasted three times), the breadcrumb carries owner **and** track, and a fourth member joined the family — `DateRangeNotesViewModel`, the world-date cross-cut that 053 block 88 predicted when it called the WorldDate/Theme parallel "exact". Two latent bugs fell out and were fixed: the base seeded its list from a virtual call before subclass state existed (crash on open), and it ignored `CollectionChanged` Reset, so switching `.storyplan` files blended two corpora into one list. A fifth member followed the same day — `MissingFieldNotesViewModel`, the criterion inverted: notes whose track declares Theme or SourceMaterial applicable but which carry no value, reachable from the Themes and Sources tabs. Deliberately states no disposition (Brian's call): it names the predicate and the count, never whether an absence is a gap — an untagged theme is frequently permanent, while an uncited canon note usually is not, and that judgement stays with the author rather than being encoded in the view. World date is excluded because the Timeline triage panel already covers it and allows assignment in place.
- **Crash and silent-save-failure hardening** (2026-07-31) — not a feature request; it came out of a cross-cut breadcrumb opening a link-owned note and hard-killing the app. Three fixes at the level the bug actually lived: `App` now installs `DispatcherUnhandledException` / `TaskScheduler.UnobservedTaskException` / `AppDomain.UnhandledException` handlers with `Shell/CrashReporter.cs` (the UI-thread one is recoverable, so a bad click no longer costs the session); all 21 `_ = SaveAsync()` discards became `.FireAndForget()`, because a discarded Task swallows its exception and **a failed save was previously silent**, which is worse than a crash for an app whose product is the data file; and `IWindowManager`'s `OpenCommonWindow(EditorMode, NarrativeElementViewModel, …)` was split into typed `OpenSubjectWindow`/`OpenPlotPointWindow` so the mode↔element pairing is a compile error rather than an unchecked cast. Deliberately **no** emergency save on crash — see the wpf-conventions skill.

## ⚪ Proposed but deliberately rejected (do not build)

- **7th subject type** (Historical Events / Places / Threads) — evaluated and rejected across 077 blocks 21-22 and 053 (threads dissolve into subject tracks + plot-point links).
- **Unified `StoryEntity` + payload-enum menagerie** (`ArcMovement`, `GoalTrajectory`, `Prominence`, `RevealLayer`, TPH junction classes) — 015/020 proposed, then intentionally superseded by the data-driven `Subject`/`SubjectDefinition` + payloads-as-notes design that shipped.
- **Numeric confidence scores, per-note priority/export flags, full revision history** — 019 block 60 rejected as over-engineering (note C4's `LastRevisedDate` is the narrower survivor).
- **Note supersession as a schema feature** — a `Superseded` state, a `Retcon` track, or a note-to-note supersession link. See **C1** above: the transcripts resolved this into the shipped `Reader Prior Belief` and `Garden Notes` tracks, and the live data uses them that way.

---

## Appendix — transcript relevance index

| File | App-feature signal | Notes |
|------|--------------------|-------|
| 015 multi-story-fabula-for-selective-syuzhet | **High** | "Part 1" — five-layer model → schema; multi-story, timeline, retcon, focal char. |
| 019 character-reader-perception-gap | **High** | "Part 2" — the scene-architecture / perception-gap layer (Section D). |
| 020 migrating-model-classes-to-note-collections | **High** | "Part 3" — the core data-model refactor (now shipped). |
| 038 planning-versus-writing | Low | "Part 4" — mostly craft; philosophy of the tool only. |
| 039 story-design-process-insights | **High** | Gemini master-summary of Parts 1-4 — richest single feature source. |
| 053 note-categorization-bootstrapping | **High** | Derivation of the whole track taxonomy; theme dialectic, multi-story tagging. |
| 077 organizing-changeling-lore | Medium | Track-system refinements (visibility, synthesizes-links, clusters). |
| 091 categorizing-bonds-and-betrayal | Low-Med | Supersession/reader-prior-belief; cross-story bond scoping. |
| 056 chrysalis-enhancement | **None** | Pure story lore — no tool discussion. |
