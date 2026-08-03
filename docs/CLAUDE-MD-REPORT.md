# What a CLAUDE.md for StoryPlanner Should Be — A Multi-Faceted Report

> **EXECUTED 2026-07-28 — historical reference; do not update this file.**
> All eight steps of §9 were carried out. `CLAUDE.md`, `.claude/skills/wpf-conventions/SKILL.md`,
> and `tests/StoryPlanner.Tests` (77 tests) are the live artifacts; this report is the
> reasoning that produced them. Where the two disagree, **CLAUDE.md is authoritative** — it was
> written against the finished state, this was written before it.
> One §8 item remains open: whether the wpf-conventions skill should grow to cover Views as well
> as ViewModels. It currently covers ViewModels plus the View-layer traps.

**Date:** 2026-07-28 (revision 2)
**Sources:** (1) design deliberations and hard-won facts from building `tools/StoryPlanner.Mcp`; (2) live data and **the conversation corpus queried through that server** — the architectural deliberations recovered below came from `search_conversations` / `get_blocks`, not from memory; (3) conversation 103 (AI-usage philosophy, blocks 7–52, 179–196); (4) the full `TheCanalaveLibrary` doc corpus (CLAUDE.md, 3 skills, 27 audit files, doc-lint scripts, 180 commits).

**Revision 2 corrects two errors in revision 1**, both flagged by Brian: the two AI roles were blurred, and CLAUDE.md was wrongly treated as a place to quote the author. Both corrections are load-bearing and are worked through in §1 and §6.

---

## 1. The bifurcation — the organizing fact of this document

Revision 1 got this half-right and the half it got wrong mattered. The correct split:

| | **Claude Code** | **Claude Desktop** |
|---|---|---|
| **Purpose** | Builds the story planner | Analyzes the story |
| **Accesses** | The repo; reads story content **to understand how to build features** | The story data, through the MCP server |
| **Autonomy** | Agentic, autonomous, guided by conventions — the Canalave model. (Previously hand-authored with Copilot assistance; the story-planner build proved the autonomous model, so this is the plan going forward) | Not applicable — no code is written |
| **Governed by** | CLAUDE.md + skills | The MCP server's instructions |
| **Does the AI-usage doctrine apply?** | **No.** These are software-engineering sessions | **Yes.** Analysis-not-generation, no prose, judgment stays with Brian |
| **Never does** | Story content analysis | Build the planner |

**The error in revision 1** was putting the AI-usage doctrine at the top of CLAUDE.md as if it constrained Claude Code sessions. It does not. Claude Code writing an EF Core query has nothing to do with whether AI generates prose. Conflating them would produce a CLAUDE.md that reads like a manifesto and under-serves the actual need: *how to build this codebase well, autonomously.*

**But the doctrine still belongs in CLAUDE.md** — reframed from *session rule* to **product requirement**. It is the reason features look the way they do, and the reason two shipped features were abandoned. A build session that doesn't know it will re-propose them. So it appears as a short "what this tool is for" section, not a creed.

**The subtle case, worth stating explicitly in CLAUDE.md:** Claude Code *does* read story content — flagged notes, track definitions, real note text — because you cannot correctly build a flagged-note wall without reading flagged notes. That reading is **instrumental**: understanding the data's shape to build features that serve it. What Claude Code does not do is form or offer opinions about the story itself — whether a flagged note is resolved, whether a theme is well-evidenced, what a subject needs next. Same data, different purpose. Naming this boundary prevents both failure modes: a build session too squeamish to look at the data it must model, and a build session that drifts into story consulting.

---

## 2. Why there are zero foreign keys, navigation properties, or indexes

This is deliberated architecture, not neglect. Recovered from the corpus (conv:21 "Character-reader perception gap in story design", blocks #126–#135, 2026-04-20). Brian proposed the design; the reasoning was worked through and adopted.

**Brian's framing of the constraint (block #126):** *"Given the nature single user load everything at startup desktop application… Should these model classes actually not even have navigation properties, and only be used for change tracking with EF Core? They will only hold Id int fields."* His proposed replacement: view models filter `ICollectionView`s over in-memory `ObservableCollection`s bound to `DbSet.Local`, matching on `OwnerId`/`OwnerType`.

**Why no navigation properties.** They exist to serve lazy loading, `Include`-style eager loading, and relationship traversal in queries *that execute against the database*. When the entire dataset is loaded at startup and lives in memory, those mechanisms are overhead with no benefit. In a desktop app they also carry active risks: accidental lazy loading after the context window has moved, change-tracking surprises when one entity is reachable through several navigation paths, and circular-reference serialization problems — the last of which Brian was already patching with `[JsonIgnore]` (see conv:69, "Preventing JSON serialization cycles with JsonIgnore"). Removing them makes the layering honest: **model classes are database row representations; view models are the working data structures.** Relationship work belongs to the layer that actually does it.

**Why no foreign keys.** `Note` uses `OwnerId` + `OwnerType` — a polymorphic association — instead of separate nullable FK columns per owner type. The v1 model had the standard relational form (`ChapterId`, `CharacterId`, `CodexEntryId`, `ThemeId`…) and it produced exactly the mess described in conv:47 #19: *"plot point had singular text fields which became dumping grounds… The notes were scattered, and it was unclear what things should go where."* Collapsing to a discriminator makes the view-model filtering uniform across all four owner types. **The acknowledged cost, stated plainly at decision time:** the database can no longer enforce referential integrity, because it doesn't know which table `OwnerId` points at. *"For a single-user desktop application that controls all data entry through a typed view model layer, this is an acceptable tradeoff. The application layer enforces the integrity that the database would otherwise enforce."* That is why `ContentDeleter.cs` exists and why its guards are not optional decoration — **they are the referential integrity system.**

**Why no indexes.** This one was *not* separately deliberated in the corpus I searched — I found no block discussing indexing. It follows from the same premise: with every table loaded into memory at startup and all filtering done by `ICollectionView` over in-memory collections, there are no database queries whose plans an index would improve. Worth recording in CLAUDE.md as *consequence of the load-everything design*, and worth flagging honestly as inferred rather than quoted.

**The paradigm has names, and they belong in CLAUDE.md** so future sessions can reason about it instead of pattern-matching it to a mistake: the configuration layer is **metadata-driven / table-driven design** (`SubjectDefinition`, `NoteTrackDefinition`, `NarrativePropertyDefinition` are data, not classes), specifically the **Type Object pattern** (Fowler) — *"A Character Subject and a World Principle Subject are the same C# class with different SubjectDefinition instances. The definition is the type."* Combined with **polymorphic associations** for content ownership. It is deliberately *not* full EAV: primary content (`Subject`, `PlotPoint`, `Note`…) keeps typed columns; only configuration is table-driven.

**Why relational at all, rather than a document store:** cross-entity queries are the primary access pattern; EF change tracking provides a transactional unit of work across entity types; the configuration tables have genuine relational structure; and the entities are shallow and highly connected rather than document-shaped — *"a Note without its NoteTrackDefinition and its owner entity is meaningless. The connections are the content."*

**Why inheritance was rejected:** TPH would produce one wide table of nullable columns; TPT would add joins. Both add complexity when the shared behavior already lives correctly in `OwnerViewModel`. And more fundamentally — the four owner types are not variations on a common concept. *"The fact that they all happen to be owned by NoteTrackDefinitions via the same polymorphic pattern is a storage convenience, not evidence of a conceptual hierarchy."* `INoteable` is the whole contract: `Id` + `OwnerType`, enough for the view model to filter.

**The stated cost, so nobody rediscovers it as a bug:** metadata-driven queries are less self-documenting. Filtering by `NoteTrackDefinitionId` reads worse than filtering by `IsOntologyTrack`. Debugging requires knowing what the configuration *data* says, not just the schema. Accepted deliberately: the payoff is that the planner's shape evolves through data entry rather than code changes — *"the architectural decision that makes the system serve the project rather than constrain it."* For a design framework still evolving monthly, that flexibility was the point.

---

## 3. Other guidance recovered from the corpus

Searched via the MCP server; these are the durable items that answer "why is it like this" and can't be read off the code.

**Why the tool exists at all** (conv:47 #21, 2026-05-11). The causal chain: Google Doc notes from Feb 2025 → Nov 2025 discovery that Gemini 2.5 Pro could reason about story logic → a week-long thread of ~120 prompts → a UI bug forcing Google Takeout recovery → *"The google doc would not be able to sustain the volume of insights made in those 120-ish prompts so I wanted to make a desktop application to organize it. I thought it would take a weekend but it ended up taking 3 weeks."* Two facts in that block belong in CLAUDE.md: **"no prose" was a system instruction from the very first Gemini thread** — the constraint predates the tool — and the v2 redesign was driven by a *context-window* problem: at 300k words, pasting the whole plan stopped working, RAG degraded results, and the planner was redesigned for **"selective context to provide for further questions."** That is the direct ancestor of the MCP server. The server is not a new idea; it is the fourth attempt at the same requirement (paste → upload → export files → live retrieval).

**Why v1 and v2 are separate files** (same block). Not an accident of migration: *"in place manipulation isn't what I want… keep all the old data in the v1 story planner. Have both v1 and v2 open. Read the data in v1 and type what belongs in v2 in tracks."* He explicitly compares it to the code migration — *"I had immense friction dealing with stripping out the old v1 concepts."* The two-file split is a deliberate clean-room strategy, which is exactly why the MCP server refuses to join them.

**Why notes-in-tracks exists** (conv:47 #19): the v1 pain point was *"singular text fields which became dumping grounds"* and notes that were *"scattered, and it was unclear what things should go where."* Every track's `DisplayQuestion` is the fix — one question per track, so there is always exactly one right place.

**The editor modes are cognitive stances AND UI states — the stance is materialized in the schema.** *(Revision 2 corrects this item: revision 1 claimed they were stances "not UI states" and that they reorder tracks "rather than changing what's editable." Both halves were wrong — verified against the code below.)*

The names — `Expansion / Linking / Gardener / Audit / SceneDesign` — come from the gardener/architect writing methodology worked through across conv:36, conv:41 and conv:43. But they are not merely labels: **the UI reorganizes itself per mode, and the reorganization is table-driven configuration, not hard-coded behavior.** Two cooperating enums:

- **`EditorMode`** (`StoryPlanner.Core/Models/EditorMode.cs`) — the window-level stance, passed into `OpenCommonWindow(EditorMode, …)`. Each value has **its own display-order column** on `NoteTrackDefinition` (`ExpansionModeDisplayOrder`, `LinkingModeDisplayOrder`, `GardenerModeDisplayOrder`, `AuditModeDisplayOrder`, `SceneDesignModeDisplayOrder`), so entering a mode re-sorts which tracks surface first — each stance gets its own ordering of the same 106 tracks.
- **`TrackDisplayMode`** (`WindowedStoryPlanner/ViewModels/TrackDisplayMode.cs`) — per-track presentation: `Active / Reference / Audit`. Its own doc-comment states the design: *"Each value maps to one of the three text fields on `NoteTrackDefinition` and determines whether the track is editable."* Concretely, `HeaderText` switches between `DisplayQuestion` (Active), `UsageDirective` (Reference), and `AuditDirective` (Audit); `IsReadOnly` blocks all edits in Reference; and `CanPromoteToConfirmed` is true **only in Audit** (or archive mode).

So a mode change alters track order, header prose, editability, and which state transitions are legal. `CanEditInAuditMode` on the definition is the per-track opt-in for the last one.

**Two consequences worth putting in CLAUDE.md.** First, this is the clearest instance of the metadata-driven principle from §2 — the writing methodology is not documented *beside* the schema, it is *encoded in* it: three prose fields and five ordering columns per track, editable through the definitions UI. Adding a stance is data entry. Second, it explains an otherwise alarming number: **v2 has 0 `Confirmed` notes** because Audit is the only mode that can promote to Confirmed, and no audit pass has been run yet. That's a lifecycle position, not a defect — precisely the "surprising is not the same as broken" case Canalave's `debugging.md` warns about, and exactly the kind of thing a session would otherwise try to "fix."

---

## 4. Hard-won facts from the MCP build

Compressed; these are the items that cost effort and one of which was actively wrong in a doc for weeks.

**Data semantics.** `NoteState.Confirmed` inverts across files — v2: stable; v1: *review closed, migrated OR deliberately superseded, disposition not recorded* (mis-documented as "migration marker" until 2026-07-28). Flagged notes are walled wherever LLMs consume data — the app's export (`NoteExportRenderer.cs:28`) and the MCP server's ordinary tools both exclude them; `FlagReason` is itself a corpus Brian drafts into. v1/v2 never join (no id correspondence, ~40% name overlap, no join wanted). The scene graph lives in v1 (1,125 links) until migration; v2 holds the taxonomy — **106 track definitions are final; the data is in flux.** `WorldDate` is free text (`993`, `-100-0`, `870-928`), sorts wrong lexicographically.

**Runtime.** `.storyplan` is raw SQLite in **WAL mode**; readers never block the running app. **The main file's mtime does not track writes** — change detection must use `PRAGMA data_version`. The running WPF app locks only its own `bin/Debug/net10.0-windows/`: building the MCP project works with the app open, building the solution does not; conversely a live Claude session locks the server's DLLs. `StoryService` is not read-only or reusable (`OpenProjectAsync` runs `MigrateAsync()` — schema-upgrades the file with no backup; `SaveAsync()` writes litter files), which is why the server bypasses it with `Mode=ReadOnly`. **stdout is sacred in the MCP process** — `ConversationImporter`'s `Console.WriteLine` would corrupt JSON-RPC.

**Settled — do not relitigate.** Machine-guessed note categorization and the Conversation Reader's suggested subject×track coverage (4,062 rows, `IsAdded=0` on every one) were both **built and abandoned**; the 41-report insight pipeline was retired. The pattern: *machine-proposed structure fails here; retrieval succeeds.* MCP design rulings: dumb tools, grep→fetch two-pass, no ranking, no fuzzy matching, hard flagged wall with count disclosure, corpora never joined. `FEATURE-AUDIT.md` ⚪ lists deliberately-rejected features; C1 (note supersession) is design-contested — ask before building.

**The recurring failure mode, named.** During the MCP build, workflow-shaped features were proposed and struck **three times** under different names. The CLAUDE.md line worth its weight:

> *An obvious bottleneck in the data is not a mandate for the tool. Tools answer "what is here," never "what should you do" or "what's interesting."*

---

## 5. TheCanalaveLibrary — the differential

**What it is:** a Blazor/PostgreSQL fanfiction-library *website* (180 commits since 2025-10-25, 7 projects, 2,258 tests, CI, launching publicly). Its `.claude/` corpus: a 118-line CLAUDE.md acting as a pure routing hub; a 66-feature × 9-layer Stage grid; a 1,251-line work-unit ledger plus 3,332-line archive; 27 per-cluster audit files (8,341 lines); three skills (~530 KB); and **doc-lint in CI**. Its last five commits are all doc-process work — it just completed a detect→diagnose→mechanize cycle fixing ~110 doc defects.

| | Canalave | StoryPlanner |
|---|---|---|
| Product | The code; ships publicly | The **data**; code is a personal instrument |
| Source of truth | A frozen spec — *"the codebase is the subject of review, not the source of truth"* | No spec. Data + conversations + Brian's judgment |
| Enemy engineered against | **Staleness** across a huge corpus | **Epistemic contamination** — unstable content masquerading as truth |
| Verification | `dotnet test` + CI + browser | Human judgment (no mechanical verifier possible for analysis) |
| Scale | 180 commits, 27 clusters | ~30 commits, 3 projects |

**Transfers:** the lean routing-hub CLAUDE.md shape (theirs contains *no* build commands and *no* code conventions — both delegated to skills); "Settled Axioms — do not propose alternatives"; "Decisions that need you" with a why-it's-yours column; the no-session-relative-language rule (absolute dates only); `debugging.md`'s epistemics, especially *"check recorded intent before fixing a 'wrong' behavior — surprising is not the same as broken"* (apt for a codebase full of deliberate stubs); the provenance instinct; fresh-eyes doc audit as an occasional practice.

**Does not transfer:** the Stage grid and its ceremony; audit-file-per-cluster; doc-lint in CI (no CI here); the work-unit ledger; and above all **the spec-first stance** — here, live data has repeatedly overruled every document, which is why the storyplan-data skill exists.

**The real lesson is their growth model, not their end state.** Canalave's CLAUDE.md was born at 69 lines and grew to 118 over six weeks in small incident-driven amendments, each rule citing the failure that caused it (*"six files had it on 2026-07-27"*, *"prose rules alone already failed twice"*). It never wrote process ahead of need. Copy that; don't copy the artifact a nine-month-older, hundredfold-larger project arrived at.

---

## 6. Answers to your questions

**How extensive do process docs need to be? — Minimal, and I'd resist growth.**
Four artifacts: `CLAUDE.md` (~100 lines), `FEATURE-AUDIT.md` (already the de-facto roadmap — status, rejected, contested), the `storyplan-data` skill (data semantics + query recipes), and a small conventions skill only once there's a second convention worth stating. No grid, no ledger, no audit files. Canalave's apparatus is proportionate to 66×9 cells and a public launch; here it would immediately become the work. Add process the way they did — when something actually breaks.

**Should CLAUDE.md quote you? — No, you're right, and my open question was malformed.**
It asked whether to quote you *to preserve the AI-usage doctrine against drift* — but that doctrine governs Desktop analysis sessions, which never read CLAUDE.md. The question conflated the roles. CLAUDE.md is written **for Claude Code to consume**, in the register Claude Code acts on best: imperative, specific, rationale-carrying, no quotation. Where the doctrine appears, it appears as product context — *"machine-proposed structure has been built and abandoned twice; retrieval succeeds"* — which is a buildable fact, not a philosophy.

**Tests — yes, and the MCP server is the right first target.**
It has genuine invariants that are cheap to test and expensive to get wrong: the flagged wall in all three faces (search excludes content, fetch returns a stub, counts still disclose), per-file state labels (v1 `Confirmed` must never render as "confirmed"), `WorldDate` parsing including unparseable values flagged rather than guessed, owner resolution across all four `OwnerType` values, and `data_version` cache invalidation. Then `StoryPlanner.Core`: `ExportResolver` scope 0/1/2 expansion and `NoteExportRenderer`'s flagged exclusion. **Use a small synthetic `.storyplan` fixture built in test setup — never the real files.** *Superseded by execution:* the suite landed as `tests/StoryPlanner.Tests`, organized by test **kind** per Canalave's placement rule rather than one project per production project, and grew to cover Core's scanner and transforms too. Conventions: `.claude/skills/testing/SKILL.md`.

**Mirror Canalave's committed settings — yes.** Theirs, verbatim in shape: `"model": "opusplan"`; allow `Read(**)`, `Glob(**)`, `Grep(**)`, `Bash(dotnet build*|test*|ef*|run*)`, `Write(.claude/**)`, `Edit(.claude/**)`; empty deny. Add `mcp__storyplanner`.

*Correction to revision 1 — you're right to question the source tree, and my framing was wrong for your setup.* I wrote that the Canalave allowlist "still prompts on every code edit." That is *default-mode* behavior. **In auto mode nothing prompts** — non-allowlisted calls are adjudicated by an LLM classifier instead, which is exactly what we diagnosed as the MCP server's per-call latency earlier today (the failure message named it: *"auto mode cannot determine the safety of PowerShell right now"*).

So for you the allowlist is **not an autonomy lever — auto mode already grants the autonomy. It's a latency and determinism lever.** Adding `Edit(**)` / `Write(**)` would let source edits skip the classifier round-trip, the same win as allowlisting `mcp__storyplanner`. The cost is losing the classifier as a backstop on writes — cheap insurance given git plus file checkpointing, but real, and edits are less uniform than read-only MCP calls. **Recommendation: mirror Canalave as-is first**, then widen only if edit latency actually bites. Unlike the MCP case, there's no measurement yet showing it does.

**Which exports are to be exorcised? — the precise inventory.**
Verified against `git ls-files` and a reference grep. **Everything in groups A–C is tracked in git**, so deletion is recoverable from history — that's what makes this safe rather than destructive. Total ≈ **9 MB across ~20 files**, all of it now regenerable or superseded.

**A. Derived from the story data — the MCP server now generates these on demand.**

| File | Size | Replaced by |
|---|---|---|
| `TheLionessOfTallTale - v1.db.md` | 2.2 MB | `search_archive` + fetch tools |
| `TLTT v2-definitions.md` | 25 KB | `get_track_definitions` |
| `TLTT v2-subjects.md` | 7.5 KB | `list_subjects` |
| `TLTT v2-themes.md` | 1.6 KB | `list_themes` |

These are the four files you used to paste by hand. They are strictly worse than a live query, and **they are the staleness vector that produced the v1-`Confirmed` error.** Highest-value deletion in the repo.

**B. Derived from the code — and now actively misleading.**
`ModelClasses.txt` (25 KB), `Models.txt` (9 KB), `ViewModels.txt` (98 KB) are snapshots of the **v1 / pre-TotalRework** architecture — conv:47 #19 introduces `ModelClasses.txt` as *"the old model classes in EF Core with navigation properties, **not to be confused with the new design**."* A future session that greps them will find navigation properties that no longer exist and FK columns the schema abandoned. `codebase_dump.txt` (441 KB, already gitignored) is `packager.py` output — keep the generator, delete the artifact.

*One dependency to note:* `FEATURE-AUDIT.md` cites these as grep sources ("grep hits only transcripts/`ModelClasses.txt`"). Those citations are claims about **absence** — that a field exists only in old dumps, not in code — so the claim survives the artifact's deletion. Worth a one-line note in FEATURE-AUDIT rather than keeping 132 KB of misleading code snapshots.

**C. Raw conversation dumps — superseded by the Conversation Reader.**
`All TLTT gemini prompts text.txt` (1.8 MB), `Note categorization bootstrapping.txt` (1.2 MB), `Big subject categorization.txt` (942 KB), `Categorizing Bonds.txt` (355 KB), `Applejack's evolved element of honesty.txt` (336 KB), `Princess and the Kaiser and ASOIAF comparison.txt` (223 KB), `Chrysalis_s Economy Beyond Her Control` (104 KB), `Alouette.txt` (87 KB), and the four `Part 1–4 … .htm` files (1.6 MB).

These are **the same conversations already imported** — 71 conversations / 2,521 blocks, searchable via `search_conversations`, with your F1–F4 triage state attached. `Princess and the Kaiser and ASOIAF comparison.txt` is conv:64; `Note categorization bootstrapping.txt` is conv:47; `Applejack's evolved element of honesty.txt` is conv:8. The loose files carry no block states and no summaries. ≈6.6 MB of duplicated corpus.

**D. Junk:** `run_stdout.log` (349 B) and `run_stderr.log` (0 B), both tracked; `identifier.sqlite` (0 B, untracked).

**E. Keep.** `FEATURE-AUDIT.md` (live — the de-facto roadmap); `packager.py` and `register-file-type.ps1` (tools, not outputs); and **`CONVERSATION-READER-SPEC.md` with a banner, not deleted** — it records *why* the reader is shaped as it is, and its drift (BlockSubjectMention never built, coverage tracks abandoned) is itself information. Canalave's practice fits: *"Built 2026-07. Drift documented in FEATURE-AUDIT §F. Historical reference; do not update."*

Add the deleted names to `.gitignore` so regenerated copies don't drift back in.

**The WPF app gets a conventions skill — decided. Here's what goes in it.**
Surveyed the layer to ground this: **51 view models, 6 interfaces, 2 shallow inheritance families**, and the conventions are already consistent — they just aren't written down, which is exactly the condition under which an autonomous session invents a second way of doing things.

- **`NarrativeElementViewModel : ObservableObject, IDropTarget, IEditorModeAware`** is the owner-composition base (the design conversations called it "OwnerViewModel"; it was renamed). Its four subclasses are exactly the four `OwnerType` values: `SubjectViewModel`, `PlotPointViewModel`, `ChapterViewModel`, `PlotPointSubjectLinkViewModel`. **New noteable entity type ⇒ subclass this**, don't reimplement track composition.
- **`TaggedNotesViewModelBase : ObservableObject, IDisposable`** is the cross-cut tag-view base — `ThemeDetailViewModel` and `SourceMaterialDetailViewModel`. **New "show me everything tagged X" view ⇒ subclass this.**
- Everything else derives from `ObservableObject` directly and is `partial` (CommunityToolkit.Mvvm source generators — `[ObservableProperty]`, `[RelayCommand]`).
- **Services are not view models** and live in the same folder anyway: `ContentFactory`/`ContentDeleter` (create/delete, behind `IContentFactory`/`IContentDeleter`), `ProjectLoader`, `ExportService`, `ViewModelRegistry`, `WindowManager`, `AppSettings`.
- **The load-bearing rules to state explicitly**, because they're invisible in any single file: models carry no navigation properties (§2) so relationships are `ICollectionView` filters over `ObservableCollection`s bound to `DbSet.Local`; the mutation pattern is *mutate the collection or POCO, then `await _storyService.SaveAsync()`* — 39 call sites, no unit-of-work, no undo; `ContentDeleter`'s guards are the referential-integrity system; `IViewModelRegistry` is the central lookup rather than passing references around; Views are UserControls for library/widget surfaces and Windows for detail surfaces, with `ViewModelLocator` as root DataContext.
- **Two live traps worth naming:** `WindowedStoryPlanner.csproj` declares an empty `NewViewModels\` folder (dead scaffolding — don't populate it), and `DesignTimeStoryService.cs` is 100% commented out against a pre-TotalRework model, so it is not a usable `IStoryService` template.

Small skill, high leverage: it's the difference between an autonomous session extending `NarrativeElementViewModel` and one hand-rolling a fifth owner type.

**Do the transcripts become stale once CLAUDE.md exists? — No, but their *authority* changes. Two corpora, two answers.**
Measured the split: **8 of 71 conversations carry tool-design material** (conv:17, 21, 36, 41, 46, 47, 58, 63); the other ~63 are story content.

*Story-content conversations (~89%) — keep using them, unchanged.* These aren't documentation that can go stale; they're **unprocessed input**, 1,761 blocks still unread, and mining the May–July window is phase 1 of your own plan. They're consumed by Desktop analysis sessions that never read CLAUDE.md anyway. Retiring them would retire the corpus the Conversation Reader exists to process.

*Tool-design conversations (8) — they stop being authoritative but remain the archive of rationale.* The hierarchy, worth stating in CLAUDE.md:

> **Live data > code > CLAUDE.md/FEATURE-AUDIT > transcripts.** Transcripts are authoritative for nothing. They are the record of *why*. Consult them when the question is "why is this like this, and does the reason still hold?" — never to learn what is true now.

**This session is the proof.** The FK/navigation-property rationale existed in **no document** — it was in conv:21 blocks 723–730, recovered through the MCP server. Had transcripts been off-limits, the honest CLAUDE.md line would have been *"no FKs — reason unrecorded,"* and the next session hitting a performance concern adds indexes without knowing they were deliberately omitted. Now that it's written down, conv:21 becomes provenance rather than a lookup. That is the normal lifecycle, not a failure.

**The risk you're sensing is real, but it isn't conflict** — conflicts are trivial, CLAUDE.md wins. It's that transcripts are **seductively complete**: they contain fully-argued designs for things later abandoned. The `NarrativeProperty*` EAV system (built, 0 rows), the coverage-track suggestions (built, 4,062 rows, abandoned), `SubjectCluster` and `Story` (modeled, never mapped) all have persuasive complete arguments in the corpus. A session reading a transcript without FEATURE-AUDIT finds a compelling case for a dead feature — which is precisely how `suggest_track` nearly got rebuilt three times during the MCP session.

**Mitigation is read order, not prohibition,** and it's already in the proposed read order: CLAUDE.md + FEATURE-AUDIT establish what's live *before* any transcript is opened. FEATURE-AUDIT already has the machinery — its ⚪ rejected list and its transcript relevance index (grading the spec files High/Medium/Low/None signal).

**So: keep the MCP conversation tools.** They serve the 63 story conversations (the main use), they're how tool rationale gets recovered, and removing them wouldn't restrict anything — the transcripts also sit as loose files and in the Reader UI. **The real action is the opposite one, and it's already group C of the exorcism list:** delete the loose `.txt`/`.htm` duplicates at repo root. Those are the *uncontrolled* copy — no block states, no summaries, no triage — and they're greppable by a session that never read CLAUDE.md. Keeping the corpus only in the Reader + MCP means every access is mediated by tools that carry the state.

*One live item worth knowing:* conv:58 and conv:63 contain structural critiques of the v2 note track definitions ("splitting Analogies from Allegories"; strengths and gaps in the taxonomy). The 106 definitions are declared final — but if that's ever revisited, those two are the first transcripts to read. Exactly the rationale-lookup case above.

**Files or MCP for design rationale in the dev role? — Split them. Files for dev, MCP for story. One fact decides it.**

**The coverage gap is decisive.** Conversation **020 "Migrating model classes to note collections" is not in the database** — searching its opening line (*"change the paradigm of my model classes"*) returns zero matches. Neither is **039 "Story Design Process Insights."** FEATURE-AUDIT's own relevance index rates 020 as documenting the **shipped** model→note-collections refactor and 039 as *"the richest single feature source."* So the two highest-value dev-role transcripts are invisible to `search_conversations`. Rationale lookups through the MCP server have a silent hole exactly where it matters most. (Likely cause: the DB's 71 conversations came from the Claude-export scan path, while these were split out separately — 039 opens with an attached Google doc, so it's Gemini-sourced. Neither corpus is a superset of the other.)

Four more reasons, any one of which would be sufficient:

1. **Circular dependency.** Rebuilding the MCP server requires disconnecting it — a running session locks its DLLs. So a dev session working *on the server* loses access to the rationale for the server. Not hypothetical; it's the documented rebuild procedure.
2. **Role purity.** You just drew the line: Claude Code builds, Desktop analyzes. The MCP server is the *analysis* instrument. Routing dev-role lookups through it creates a dependency from the builder onto the built.
3. **Native tooling.** Grep/Read is the path Claude Code is optimized for — your own observation about how it works with files. The JSON is greppable and block-numbered.
4. **Stable citation.** `docs/design-conversations/020_….json` + block number is citable in CLAUDE.md and resolves with no server running. `conv:21 block 723` does not.

**But curate and track them — don't just leave them where they are.** Three current problems: the folder is **gitignored and untracked**, so an archive about to become load-bearing for CLAUDE.md isn't versioned or backed up; it includes `056_chrysalis-enhancement` (423 KB) which FEATURE-AUDIT grades *"None — pure story lore, no tool discussion"*; and the same conversations exist in **three** places (spec-folder JSON, root `.htm` — `Part 1–4` are 015/019/020/038 — and root `.txt`).

Proposed: a tracked `docs/design-conversations/` holding the JSONs, the root `.htm`/`.txt` duplicates deleted (group C — now safe, because the tracked JSON becomes the one copy), and **a manifest in CLAUDE.md doing the curation rather than folder membership** — one row per conversation: file, what decision it records, signal level, current status. That way a session reads the manifest and knows whether to open 077 (medium: track-definition refinements inside a mostly-story conversation) without opening it. Net effect: three copies → one, untracked → versioned, uncurated → manifested, and ~6.6 MB deleted against ~5.9 MB added.

*Your call on one thing:* whether unpublished creative work belongs in git. The repo already tracks 6.6 MB of the same conversations at root, so this is consistent rather than new — but it's worth deciding deliberately rather than by inheritance.

**So should the dev role use the MCP server at all? — Yes, for data shape; no, for conversations.**

| Dev role uses… | For |
|---|---|
| **MCP** — `get_stats`, `count_notes_*`, `get_track_definitions`, `list_subjects` | "What does the data actually look like?" — verifying assumptions about the data a feature will model. This is the legitimate form of *reading story content to understand how to build features*, and it's what stopped the MCP server itself from being built on wrong assumptions. |
| **Files** — `docs/design-conversations/` | "Why is this like this?" — design rationale, via Grep/Read. |
| **Neither** | Story-analytical questions. Those are Desktop's. |

**Living audit files, or is the codebase self-documenting? — Self-documenting, with one exception.**
At three projects, git history plus FEATURE-AUDIT carries the narrative; audit files earn their keep in Canalave only because 66×9 cells exceed what a person or a git log can hold. **The exception is the data semantics** — those are *not* self-documenting, they're counterintuitive (`Confirmed` inverting), and they're the one thing that has already been documented wrongly. That argues for care in the one doc that owns them (`storyplan-data/SKILL.md`), not for more files. Concretely: when data semantics change, three places update together — that skill, CLAUDE.md, and the server's `ServerInfo.Instructions`. Making that a stated rule is worth more than an audit-file system.

---

## 7. Proposed CLAUDE.md skeleton (~100 lines)

```markdown
# StoryPlanner — CLAUDE.md

## What this is
Personal WPF (net10.0-windows) + EF Core/SQLite planning instrument for TLTT.
The .storyplan data files are the product; this code is the instrument.
One user. Features arrive from design conversations, not a spec.

## Two AI roles — this file governs the first
Claude Code builds the planner: agentic, autonomous, guided by these
conventions. Reads story content to understand how to build features —
instrumental reading, not story analysis.
Claude Desktop analyzes the story through the MCP server and never
builds. Story-analysis rules live in the server's instructions, not here.
Claude Code does not offer opinions on story content.

## What the tool must never do (product requirement)
Retrieval, not suggestion. Machine-proposed structure has been built and
abandoned twice (note categorization; Conversation Reader coverage
suggestions — 4,062 rows, zero acted on). Tools answer "what is here,"
never "what should you do" or "what's interesting." A bottleneck in the
data is not a mandate for a feature.

## Architecture — deliberate, not accidental
No navigation properties, no FK constraints, no indexes. Single-user,
load-everything-at-startup desktop app: view models filter ICollectionViews
over in-memory collections bound to DbSet.Local. Models are row vessels;
view models do relationship work. Note ownership is polymorphic
(OwnerId + OwnerType), so the DB cannot enforce integrity —
ContentDeleter.cs IS the integrity system. Type Object pattern:
SubjectDefinition/NoteTrackDefinition are data, not classes; the shape
of the planner evolves by data entry, not code change.
Cost accepted: queries are less self-documenting. Do not "fix" this.
Editor modes are the same principle applied to the UI: EditorMode
re-sorts tracks via five per-mode DisplayOrder columns; TrackDisplayMode
swaps header prose (DisplayQuestion/UsageDirective/AuditDirective),
toggles read-only, and gates promotion to Confirmed (Audit only).
Behavior lives in the definition rows, not in code.
Corollary: 0 Confirmed notes in v2 = no audit pass yet, not a bug.

## Data semantics — the traps
Confirmed inverts (v2 stable / v1 review-closed, disposition unrecorded).
Flagged notes walled wherever LLMs consume data. v1 and v2 never join.
Scene graph is in v1; taxonomy in v2. Track definitions final, data in flux.
WorldDate is free text. → .claude/skills/storyplan-data/SKILL.md
Live counts: mcp__storyplanner get_stats. Never hardcode counts here.

## Build & run
dotnet 10. App open → build the project, not the solution. Live MCP
session → can't rebuild the server (/mcp disconnect first). WAL; use
PRAGMA data_version, not mtime. StoryService is not read-only.
stdout is JSON-RPC in the MCP process — stderr only.

## Settled — do not propose alternatives
[the §4 list] · FEATURE-AUDIT.md ⚪ for rejected features
· C1 note supersession is contested: ask first.

## Brian's decisions, always
Story structure, categorization, what's interesting, flagged-note
resolution, taxonomy changes, anything that writes to a .storyplan.

## Read order and source authority
Live data > code > this file/FEATURE-AUDIT > design transcripts.
Cold start: this file → FEATURE-AUDIT.md → get_stats. Per task:
storyplan-data skill, WPF conventions skill, CONVERSATION-READER-SPEC.md
(historical, see drift note), tools/StoryPlanner.Mcp.

Two sources, two purposes — do not substitute one for the other:
- MCP (get_stats, count_notes_*, get_track_definitions, list_subjects)
  answers "what does the data look like" — use it before modeling data.
- docs/design-conversations/ (Grep/Read) answers "why is this like this."
  Do NOT use search_conversations for design rationale: 020 and 039 are
  not in the database at all, and rebuilding the server disconnects it.
Transcripts are authoritative for nothing — they are the record of WHY.
Consult after FEATURE-AUDIT establishes what's live: they contain
complete, persuasive arguments for features that were later abandoned.

## Design conversation manifest
[one row per file: what decision it records, signal level, status]

## Doc rules
Absolute dates; never "this session"/"recently". Data-semantics changes
update three places together: the skill, this file, and the MCP server's
ServerInfo.Instructions.
```

**Deliberately omitted:** schema tables and enum maps (the skill owns them — duplication is how the `Confirmed` error survived); live counts (`get_stats` exists); workflow phases (session context, not constitution); grids, ledgers, audit files. **Code conventions are omitted from CLAUDE.md too** — they go in the WPF conventions skill, matching Canalave's split where CLAUDE.md routes and skills carry detail.

---

## 8. Decisions taken, and what's left

**Settled in this revision:** tests (MCP invariants first, synthetic fixture); settings (mirror Canalave, don't widen `Edit`/`Write` yet — in auto mode it's a latency knob, not an autonomy one, and nothing has measured slow); the exorcism list (§6, groups A–D delete, E keeps with a banner on the reader spec); no audit files; **the WPF conventions skill is a yes**, scoped in §6.

**Also settled (2026-07-28):** the design conversations get **tracked in git** — repo privacy is not a constraint, so the archive should simply be versioned like any other load-bearing doc. And **delete rather than `archive/`**: an `archive/` folder is just a slower staleness vector — a future session greps `ModelClasses.txt` wherever it sits and models the v1 schema from it. Git history is the archive.

**Still genuinely open:** does the WPF conventions skill cover Views as well as ViewModels? The VM conventions are crisp and worth writing now; the View layer (UserControl vs Window, `Behaviors/`, `Converters.cs`, the widget-selector pattern) is more varied. Writing the VM half now and letting the View half accrete on first friction is the growth model this report recommends everywhere else.

---

## 9. Execution order

Sequenced by dependency, not importance. **The governing principle: CLAUDE.md is a routing hub, so it is written last — after the things it routes to exist.** Writing it first guarantees it describes intentions rather than facts, which is the failure mode this whole report is trying to avoid.

1. **Delete the duplicates** (§6 groups A–D, ~20 files, ~9 MB; all git-tracked so recoverable). Add `.gitignore` entries so regenerated copies don't drift back. First, so nothing written later references a file that's about to vanish.
2. **Add the one-line note to `FEATURE-AUDIT.md`** where it cites `ModelClasses.txt` / `Models.txt` / `ViewModels.txt` as grep sources — those citations are claims about *absence* and survive, but should say so.
3. **Create `docs/design-conversations/`**, move the JSONs in, drop `056` (graded "None"). Must precede CLAUDE.md, which will contain the manifest pointing at these paths.
4. **Banner `CONVERSATION-READER-SPEC.md`** — dated, pointing at FEATURE-AUDIT §F for the drift.
5. **Mirror Canalave's `.claude/settings.json`** — independent of everything else, ~2 minutes, immediate benefit.
6. **Add the test project** (landed as `tests/StoryPlanner.Tests`) — MCP invariants against a synthetic fixture (flagged wall's three faces, per-file state labels, WorldDate parsing, owner resolution, `data_version` invalidation), then `ExportResolver` / `NoteExportRenderer`. Before CLAUDE.md so it can state the real command and layout.
7. **Write the WPF conventions skill** (§6 scope) — referenced by CLAUDE.md, so it should exist first.
8. **Write CLAUDE.md** (§7 skeleton) — last, against the finished state, with the design-conversation manifest built from what actually ended up in step 3.

Steps 1–5 are mechanical and could run in one session. Steps 6–8 are the substantive ones, and 6 is the only one that involves writing non-trivial new code.
