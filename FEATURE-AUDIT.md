# Story Planner — Outstanding Feature Audit

**Date:** 2026-07-04
**Method:** Nine long Claude/Gemini design transcripts in `Additional Potential Conversations with Specs/` were mined for discussion *about the Story Planner tool itself* (as opposed to the fanfiction content that dominates them), then each proposed feature was checked against the current `master` codebase (`StoryPlanner.Core` + `WindowedStoryPlanner`) to judge implementation status.

**Bottom line:** The **data-model refactor** those conversations argued for is substantially *shipped* — the unified polymorphic `Note`, data-driven `SubjectDefinition`/`NoteTrackDefinition` tracks, the `TrackType` cognitive-layer taxonomy, `Unset/Flagged/Confirmed` note states, per-editor-mode track ordering, `WorldDate`/`Theme`/`SourceMaterial` tagging, `PlotPointSubjectLink` as a first-class noteable, F-key track assignment, and the anchor+scope export resolver all exist. What remains outstanding is almost entirely the **layer built on top of that model**: aggregation/navigation views, multi-story scoping, the scene-architecture (perception-gap) layer, note-provenance lifecycle, and a handful of definition-system refinements.

Legend: 🔴 Outstanding · 🟡 Partial · 🟢 Implemented (listed only where a conversation's "ask" is worth confirming as done) · ⚪ Deliberately rejected in-conversation

---

## Priority summary

| # | Feature | Status | Signal (convos) | Structural? |
|---|---------|--------|-----------------|-------------|
| A1 | Multi-story / narrative scoping (`StoryId` on content, `SupportsNarrativeTag`) | 🔴 | 015, 020, 053, 077, 091, 038/039 | **Enabler** |
| B1 | Master timeline view (aggregate `WorldDate` notes) | 🔴 | 015, 019, 053 | View |
| B2 | Global entity search (tab is a stub) | 🔴 | 038/039 | View |
| C1 | Note supersession / preserving retconned lore | 🔴 (contested) | 091, 015, 053, 038/039 | Model |
| C2 | Stage-0 "accumulation" inbox / `IsIncorporated` flag | 🔴 | 053, 038/039, 015 | Model |
| D1 | POV / focal-character designation on `PlotPoint` | 🔴 | 015, 019 | Model |
| D2 | `GapType` enum + perception-gap scene fields | 🔴 | 019, 015, 053, 038/039 | Model |
| B3 | Per-entity completion / scene-readiness dashboard | 🔴 | 019, 038/039 | View |
| E1 | Theme dialectical model (antithesis + Support/Counter/Refutation) | 🔴 | 053 | Model |
| E2 | Subject clusters surfaced in UI (`SubjectCluster` orphaned) | 🔴 | 077, 053 | Model+View |
| E3 | Per-mode track *visibility/collapse* (not just ordering) | 🔴 | 077 | Model |
| E4 | "Synthesizes Links" source-vs-aggregate track flag | 🔴 | 077 | Model |
| E5 | Chapter primary-thread spine / chapter-subject link tracks | 🔴 | 015, 053 | Model |
| C3 | `ValidationTier` opt-in enforcement system | 🔴 | 038/039 | Model |
| C4 | `LastRevisedDate` / revision tracking on notes | 🔴 | 038/039 | Model |
| B4 | Cross-thread "synchronized ratchet" chapter view | 🔴 | 015 | View |
| F1 | Conversation Reader: per-block subject mentions | 🔴 | (spec) | Model |
| F2 | Conversation Reader: cross-conversation subject view | 🔴 | (spec) | View |
| F3 | Conversation Reader: bulk multi-select state ops | 🔴 | (spec) | UX |
| G1 | Session export presets (degree-of-separation scoping) | 🟡 | 019 | Mostly done |

---

## A. Multi-story / narrative scoping — highest-signal gap

**A1 — 🔴 Multi-story project & per-note narrative tagging.** *Discussed in six of the nine transcripts* (015 blocks 219-229, 020, 053 blocks 37-42/78-79/94-96, 077 block 7, 091 blocks 2/16/31/33, 038/039 "StoryScope"). The recurring need: the same planner holds a trilogy (TLTT → Minette → Chrysalis), a Subject/Bond "lives in" one story but is inherited by prequels/sequels, and reveal/recontextualization notes need **one note per narrative** — implying a per-track `SupportsNarrativeTag` boolean and a narrative selector on notes, parallel to `WorldDate`.

- **Current state:** `Story` model exists (`Models/Story.cs`: `Id`, `Title`, `OrderIndex`) but is an **orphan** — no `DbSet<Story>` in `AppDbContext.cs`, no `StoryId` FK on `Note`/`Subject`/`PlotPoint`/`Chapter`, no `SupportsNarrativeTag` on `NoteTrackDefinition`. `grep StoryId` matches only transcripts.
- **Assessment:** The single most-requested structural feature, and effectively unbuilt (stubbed model only). Everything else about the trilogy architecture depends on it.

---

## B. Aggregation & navigation views

**B1 — 🔴 Master timeline view.** Aggregate all `WorldDate`-tagged notes across subjects into one chronological view (015 block 1433 explicitly: "does not yet exist in the planner but is buildable"; also 019 block 60, 053). The *data substrate is done* — `Note.WorldDate` + `NoteTrackDefinition.SupportsWorldDate` + editing in `NoteViewModel` — but there is **no timeline view** (no `*Timeline*` view/VM in the project).

**B2 — 🔴 Global entity search.** The "Global Search" tab in [MainWindow.xaml](WindowedStoryPlanner/Views/MainWindow.xaml#L58-L70) is a `TextBox` bound to `SearchText` above an **empty `ScrollViewer`** — no results binding, no search VM (`grep SearchText` in `ViewModelLocator` → nothing). *Note:* a working, export-scoped search does exist in `ExportViewModel.RebuildSearchResults()` for anchor-picking, but the global cross-entity search is unimplemented.

**B3 — 🔴 Per-entity completion / scene-readiness dashboard.** Per-entity note-state rollups ("3 Confirmed, 4 Unset, 2 Flagged") and a derived "Completion Profile" that signals when a scene is ready to draft (019 blocks 61/69, 038/039 R8). Per-note/per-track VMs exist (`NoteTrackSectionViewModel`, `NoteViewModel`) but no state-rollup or readiness-scoring surface.

**B4 — 🔴 Cross-thread "synchronized ratchet" view.** A per-chapter view showing each active thread's goal-trajectory distribution so stagnant threads stand out (015 block 521). No such view; the `GoalTrajectory` payload it assumed is no longer a structured field (payloads became notes).

---

## C. Note lifecycle & provenance

**C1 — 🔴 Note supersession / preserving retconned lore. (Design-contested — resolve before building.)** Multiple conversations want superseded world-truth preserved rather than deleted: a `Retcon` track (015 block 1383), a "reader-prior-belief" demotion (091 block 24), a `Superseded` fourth `NoteState` (038/039, 053). **However, 019 blocks 56-57 explicitly considered and *rejected* a `Superseded` state as over-engineering.** Current code has only `Unset/Flagged/Confirmed` + `FlagReason`. This is a genuine open design question, not a clean backlog item — the transcripts disagree with each other. Recommend deciding between (a) a `Superseded` state, (b) a dedicated `Retcon`/`ReaderPriorBelief` track type, or (c) a supersession link between notes, before implementing.

**C2 — 🔴 Stage-0 "accumulation" inbox / `IsIncorporated` flag.** A pre-structural inbox for raw hypotheses/research directives distinct from Stage-1 truth, tracked by an `IsIncorporated` marker (038/039 P23-25, 053 blocks 21-24, 015). **Concrete artifact:** the flag was scaffolded and abandoned — `IsIncorporated`/`NeedsFurtherAnalysis` appear only as **commented-out dead code** in [StoryService.cs:283-284](StoryPlanner.Core/StoryService.cs#L283-L284). `Note` has no such field today.

**C3 — 🔴 `ValidationTier` opt-in enforcement.** A tier system (None / StructuralRecord / ExperienceDesign / SceneBlueprint) making fields like `GapType` optional until the author opts into enforcement per tier (038/039 P51). No `ValidationTier` enum in `Core`.

**C4 — 🔴 `LastRevisedDate` / revision tracking.** Record when a claim was revised as upstream dependencies change (038/039 P24). `Note` has only a plain `LastModified` timestamp — no revision history or `LastRevisedDate`.

---

## D. Scene-architecture (perception-gap) layer

This is the biggest *conceptual* body of work in the transcripts (esp. 019, the "character-reader perception gap" session). Most of it was ultimately resolved to live in **note tracks** rather than hardcoded fields — so the generic machinery can express it — but the specific tracks aren't configured and a few genuinely-discrete fields remain unbuilt.

**D1 — 🔴 POV / focal-character on `PlotPoint`.** A nullable `FocalCharacterId` FK making POV the pivot of scene design (015 blocks 1308/3138, 019 blocks 8-10/37/69). `PlotPoint` has only `Id/Title/ChapterId/OrderInChapter` — no POV field.

**D2 — 🔴 `GapType` enum + perception-gap fields.** `GapType` (Ironic / Tragic / Closing / Aligned) was repeatedly specified to stay a **discrete enum field**, not a note (019 blocks 18/37/65-69). Also the named field/track set: `POVBelief`, `POVReaderPerception`, `ReaderState`, `FIDAnchors`, `Stakes`, `Outcome`, `ArcStatement`, `StructuralTruth`, `EvidenceDeposited`, `ReaderHypothesis`, plus `RevealLayer`/`TLTTVisibility` link enums (015 blocks 778/2467). None exist in code (`grep GapType|POVBelief|ReaderState|RevealLayer` → transcripts/`ModelClasses.txt` only). Decide per item: promote to a discrete field vs. seed as a `NoteTrackDefinition` row.

**D-note — rich PlotPoint metadata** (`DraftStatus`, `CoreDriver`, `TensionPhase`, `ConflictType`, `Presentation`) is assumed throughout 019 but appears only in legacy `StoryFileService.cs` import parsing, not on the live `PlotPoint`. Largely superseded by the payloads-as-notes decision; flag only if completion-signals (B3) need `DraftStatus`.

---

## E. Track / definition-system refinements

**E1 — 🔴 Theme dialectical model.** `Theme` should be a singleton proposition **plus antithesis**, with a three-value enum (**Support / Counterargument / Refutation**) on theme-evidence links (053 blocks 35-36). `Models/Theme.cs` has only `Id/Name/Proposition` — no antithesis field, no such enum. *(The theme cross-cut **view** itself is done — `ThemeDetailViewModel : TaggedNotesViewModelBase` filters all notes by `SelectedTheme`.)*

**E2 — 🔴 Subject clusters in UI.** Grouping headings like "Night Economy" over multiple subjects (077 blocks 18/32, 053). `Models/SubjectCluster.cs` (`Id/Name/ColorHex`) exists but is **orphaned** — no `Subject.ClusterId`, no `DbSet`, no UI (`grep SubjectCluster|ClusterId` → model file only).

**E3 — 🔴 Per-mode track visibility/collapse.** Editor modes reorder tracks but can't hide/collapse irrelevant ones (077 block 41: "visibility tells you what's even relevant"). `NoteTrackDefinition` has five per-mode *order* fields but no per-mode visibility boolean.

**E4 — 🔴 "Synthesizes Links" flag.** Distinguish fill-first source-of-truth tracks (Ontology/History) from tracks that aggregate upward from link notes — a suggested `SynthesizesLinks` boolean (077 blocks 26-28). No such field; `CanEditInAuditMode` only loosely proxies it.

**E5 — 🔴 Chapter primary-thread spine / chapter-subject link tracks.** Mark each chapter's primary narrative thread (015 block 1717) and/or add a Chapter↔Subject link scope with its own tracks (053 block 69). `Chapter` remains `Id/Title/OrderIndex`; no chapter-subject junction.

---

## F. Conversation Reader — spec vs. build

The Conversation Reader (per [CONVERSATION-READER-SPEC.md](CONVERSATION-READER-SPEC.md)) is **substantially implemented**: three-column reader window, routing header with clickable subject+track chips, `BlockState` context menu **and** U/S/F/D keyboard shortcuts ([ConversationReaderWindow.xaml.cs:57-71](WindowedStoryPlanner/Views/ConversationReaderWindow.xaml.cs#L57-L71)), WebView2 markdown rendering, folder-pair import, derived conversation states, and dashboard stats (`ConversationLibraryViewModel`). Outstanding spec items:

- **F1 — 🔴 Per-block subject mentions.** The spec's `BlockSubjectMention` entity (per-block subject tagging, for filtering blocks by subject within a conversation) was never built — only conversation-level `ConversationSubjectCoverage` exists. `grep BlockSubjectMention` → spec only.
- **F2 — 🔴 Cross-conversation subject view.** "What did every conversation say about Applejack's Characterization?" — spec §Cross-Conversation Subject View + `SubjectCoverageView.xaml`. Not present (no such view/VM; depends on F1).
- **F3 — 🔴 Bulk multi-select state ops.** Spec calls multi-select "critical for marking runs of AI-deliberation turns Skipped in one action." The block `ListBox` is single-select; state commands apply one block at a time.
- **F4 — 🟡 Dashboard "subjects with unresolved material" metric.** Dashboard has conversation/block counts but not the spec's "subjects appearing in coverage for not-yet-Complete conversations" number.

---

## G. Largely-done items worth confirming

- **G1 — 🟡 Session export presets.** 019 block 63 wanted purpose-scoped exports with degree-of-separation entity scoping and track-type exclusion. Most of this **is built**: [ExportConfiguration.cs](StoryPlanner.Core/Export/ExportConfiguration.cs) has `Anchors`, `Scope`, `ChapterFrom/To`, `IncludedTrackTypes`, and [ExportResolver.cs](StoryPlanner.Core/Export/ExportResolver.cs) implements a real 0/1/2-degree expansion (anchors → links → other-end entities). What's missing is only the *named presets* ("World Expansion / Architecture Review / Scene Design") as saved configurations.
- **F-key track assignment** (038/039 P46) — **done**: `DefinitionsEditorViewModel.FunctionKeyOptions`, `NoteTrackSectionView.xaml.cs` `PreviewKeyDown`, `App.xaml.cs` `OnGlobalKeyDown`.
- **Cross-cut tag views** for Theme and SourceMaterial (015) — **done** via `TaggedNotesViewModelBase` (`ThemeDetailViewModel`, `SourceMaterialDetailViewModel`).

## ⚪ Proposed but deliberately rejected (do not build)

- **7th subject type** (Historical Events / Places / Threads) — evaluated and rejected across 077 blocks 21-22 and 053 (threads dissolve into subject tracks + plot-point links).
- **Unified `StoryEntity` + payload-enum menagerie** (`ArcMovement`, `GoalTrajectory`, `Prominence`, `RevealLayer`, TPH junction classes) — 015/020 proposed, then intentionally superseded by the data-driven `Subject`/`SubjectDefinition` + payloads-as-notes design that shipped.
- **Numeric confidence scores, per-note priority/export flags, full revision history** — 019 block 60 rejected as over-engineering (note C4's `LastRevisedDate` is the narrower survivor).

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
