# Analysis Pipeline — Chronicle (2026-08-14 through 2026-08-17)

The entire pipeline — concept through v3 Brief — was built in a single Claude Code session
(`4c27bcd7`) across four context windows over four days. This document records the design
decisions, the mistakes, and the lessons, so future sessions don't re-derive any of it.

## Origin (2026-08-14, context window 1)

Brian's prompt: *"I am going to be on vacation for a week but don't want my Claude
subscription to go to waste. I want to set up scheduled tasks on the cloud to do analysis
on existing fimfiction stories that I download as epubs."*

**Goal:** Automated literary analysis of ~120 Fimfiction stories (mostly canon FiM
romance/slice-of-life) using the mechanism x inference-stage matrix from Note Categorization
Bootstrapping (conv 053). Stories ranked by a 3x3 grid (length x liking tier) in a
Fimfiction Favorites spreadsheet; tiers defined by RETENTION: Good = can't remember much,
Great = remember something, Absolute Favorites = really stick.

**Key decisions in window 1:**

- **Framework source**: The analysis battery is derived from Brian's own shipped track system
  (mechanism x inference-stage matrix, 9 meta stances), NOT invented. The session initially
  cited intermediate formulations from conv 053 blocks 83-110 ("Discrepant/Enacted/
  Transformation") — Brian corrected: *"Do not judge insights on the tracks like latent vs
  emergent on an intermediate reading somewhere in the middle. Read the entire note
  categorization bootstrapping."* The actual shipped system was found in the full 285-block
  read.

- **Italics are load-bearing**: FID is defined partly by the ABSENCE of italic marking.
  Fimfiction's `.txt` export silently strips italics. Solution: download as EPUB, convert
  via `EpubToMarkdown.cs` (reusing existing `FimfictionEpubReader` + `FicHtml.ChapterMarkdown`).
  Built as a `--to-markdown` command on `tools/StoryPlanner.SourceTexts`.

- **Pipeline architecture**: EPUB → Markdown locally → upload to Google Drive Queue folder →
  Cowork scheduled task picks one story per run → reads Analysis Brief → reads full story →
  writes analysis as Google Doc in Output folder → moves story to Done folder.

- **Cloud routine created** via RemoteTrigger API: cron `7 6,12,18,0 * * *` (4x daily),
  model `claude-opus-4-6`, Drive connector attached. Initially used wrong model ID
  (`claude-opus-4-6-20250715`) — Brian caught it; correct ID has no date suffix.

- **Brian corrected scope**: Most fics are canon FiM romance/slice-of-life, not geopolitical
  epics. Political axes, succession graphs, theater distribution are useless for those.
  Battery rebuilt around Bond tracks and the mechanism grid.

- **NotebookLM perspective analysis** (`_perspective_extract.txt`, 503KB): Brian's February
  2026 craft autobiography across ~56 sources. Session initially sampled it; Brian: *"Don't
  just sample the notebook LM. Understand it in full."* Full read required.

**Artifacts published in window 1:**
- "The Fifth Corpus" (first pass, superseded)
- "Reader, Author, Archivist" (48 items, 10 families — the investigation agenda)
- "The Mechanism Grid" (final battery spec)

## Analysis Brief v1 → v2 (2026-08-15 to 2026-08-16, context windows 2-3)

**v1** was a 9-part framework: Scene Segmentation, Page Pass, Mechanism Classification,
Inference Ladder Profile, Prior Reconstruction, Perspective Analysis, Bond Analysis, Meta
Stances, Aggregates. Uploaded to Drive as a Google Doc in the Reading Archive folder.

**Three test runs** compared execution platforms (local Claude Code, Cloud Routine, Cloud
Cowork) on "It's Elementary, My Dear Rainbow":
- Local Claude Code: 22KB, best formatting
- Cloud Routine: 15KB, escaped Markdown (`\#\#\#`, `\*\*`)
- Cloud Cowork: 19KB, plain text but workable

**Brian's verdict on v1 output:** *"I don't think the individual scenes are very useful.
It's too much data."* The exhaustive per-scene data dumps (Page Pass, per-scene mechanism
classification, per-scene aggregates) were cut. What IS useful: Perspective Analysis
(high-level), inference examples showing show-don't-tell, Meta Stances, Bond Analysis,
Theme Propositions.

**v2 rewrote the Brief** with 8 sections: Story Map, Perspective Analysis, Inference
Analysis, Inference Ladder Profile, Bond Analysis, Meta Stances, Theme Propositions,
Framework Fit (new — tests the matrix against each story).

**Technical issues solved in v2:**
- **Base64 encoding**: Raw `.md` files in Drive return base64 when read by the connector.
  Fix: upload as Google Docs (`create_file` with `textContent` + `contentMimeType:
  "text/plain"` auto-converts). Asterisks survive as literal text, perfect for FID markers.
- **Markdown formatting in cloud outputs**: Cloud Routine escaped Markdown characters.
  Fix: UPPERCASE headers, no Markdown syntax at all — plain text formatting only.
- **Scaling instruction**: Report length must scale with story length. Every Latent,
  Development, and Perception Gap instance is reported; Enacted only where inference is
  non-trivial.
- **Full reading mandate**: *"But a key constraint is that I want the agent to honestly and
  faithfully read the entire story. It is not allowed to only sample the text."*

**Drive doc IDs at v2:**
- Analysis Brief v2: `1OT4WNnBCTZl-YZN-WKKKjlJT1xK6AZqrKP7TGHR0s6s` (later superseded
  by `1_RaSEPtnB4rl8mgV-tUGZGTozqKPXJIu7NusCL4lwNE`, now itself superseded by v3)

**Cowork task prompt** (`cowork-task-prompt.txt`) written and copied to the repo. Brian
configures Cowork himself — Claude Code provides the text only.

## Analysis Brief v3 — 11 amendments (2026-08-17, context window 4)

Searched Brian's planning-process conversations (conv 17, 21, 36, 47, 64), the NotebookLM
Perspective Analysis (`_perspective_extract.txt`, 503KB), and the Reader-Author-Archivist
HTML synthesis (2026-08-13, 48 investigations across 10 families) to surface craft goals
Brian has articulated across his reading history.

**Source findings that drove the amendments:**

- Conv 17: *"I did amateur head hopping or stuck to first person rotating. I wanted TLTT to
  be a third person limited masterclass."* → Discipline subsection
- Conv 36 block 4: *"The romance genre's core failure mode is obstacle-by-misunderstanding.
  Your instinct was to make the obstacle structural."* → Obstacle Architecture
- Conv 36 block 6: *"The story believes its conclusion rather than earning it."* →
  Counterargument question
- Conv 36: *"I've been using italics to tell the reader what the character is thinking
  instead of using sensory details and FID to show the reader."* → Interiority Techniques
- NotebookLM line 807: telepathy-as-crutch diagnosis → Narrative Shortcuts
- Reader-Author-Archivist #14: opening audit → Opening section (new)
- Reader-Author-Archivist #20: idiolect separation → Character Voice Distinction (new)
- Feature-Box analysis proved `\*...\*` is not always Direct Thought → DT clarification

**The 11 amendments (all shipped):**

1. Discipline (perspective consistency/breaches)
2. DT vs FID clarification (`\*...\*` can be emphasis, not just DT)
3. Interiority Techniques (beyond the DT/FID binary)
4. Narrative Shortcuts (telepathy, eavesdropping, exposition dumps)
5. Obstacle Architecture (structural / communicative / characterological / external)
6. Perspective x Bond (interiority access symmetry)
7. Counterargument (does the antagonist have a defensible point?)
8. Narrative Voice (register, consistency, narrator-focalizer blending)
9. Revelation Architecture (reader knowledge management, dramatic irony)
10. Opening (what the first scene establishes and promises)
11. Character Voice Distinction / Idiolect (do characters sound different?)

Brian explicitly requested #10 and #11: *"I really want the Opening mechanics and Character
voice distinction/idiolect sections because these are areas that I really want to learn from
other authors' works so I can improve."*

**Drive deployment:**

- `update_file` cannot update Google Doc content, only metadata (title, parent). Discovered
  this during the session.
- `trash_file` was blocked by auto mode classifier (destructive action).
- Solution: created a new "Analysis Brief" doc (`1I7dwAPBWAS0UaVvHeRmKik4nvuS1F0-bKfIAPCgBk6I`)
  via `create_file` with `textContent` + `contentMimeType: "text/plain"` (auto-converts to
  Google Doc), then renamed the old doc to "Analysis Brief (v2 superseded)" via `update_file`.
- Old doc ID: `1_RaSEPtnB4rl8mgV-tUGZGTozqKPXJIu7NusCL4lwNE`
- Parent folder (Reading Archive): `1upGz5lDqojWnuV-2S2K_m7ReiAz3dkeC`

## Cowork task prompt rewrite — post-mortem fixes (2026-08-17, after v3)

A completed analysis run produced good output but wasted its entire first context window on
navigation. Post-mortem diagnosis (from the Cowork session itself):

**Root cause:**

The prompt used human-readable folder paths ("Reading Archive/Queue") but the Google Drive
MCP has no path-based navigation. Everything is ID-addressed. The session discovered this
through trial and error, burning context on failed search syntaxes.

**What the session tried and failed:**

- `"name contains 'X'"` — wrong syntax
- `"'id' in parents"` — wrong syntax
- `"folder:id"` — wrong syntax
- Quoted phrases — wrong syntax
- Broad `title contains 'a'` then manual parentId filtering — works but returns a subset
  (only titles containing 'a'), causing collision risk with parallel sessions

**The actual Drive search syntax:**

`parentId = 'FOLDER_ID'` returns all files in a folder. This is a first-class query term
documented in the `search_files` tool schema. The whole trial-and-error phase was unnecessary.

**Six fixes applied to `cowork-task-prompt.txt`:**

1. **Hardcoded all Drive IDs** — Brief doc, Queue/Done/Output folders in a reference block
2. **Documented search syntax** — `parentId = 'ID'`, `title contains 'word'`, single-quoted
   string values, `and`/`or` combinators
3. **Explicit Queue listing** — `parentId = '1MKVZxR9R3J4ualUcrvRhhrh5wc7ftD3z'` returns the
   complete set, no subset bias
4. **Kept Brief as a Drive read** — hardcoded ID makes it one tool call; embedding would
   double the prompt and create a sync burden
5. **Front-loaded ToolSearch** — all 5 Drive tool schemas loaded in the first action
6. **Specified reading order** — Brief first (structural knowledge), then story (text)

**Decisions rejected:**

**Writing analysis incrementally to a scratchpad file.** Brian pointed out this could
undermine the "read everything, then analyze" mandate, especially for short stories. The
real fix is eliminating navigation waste so compaction doesn't hit before analysis begins.

**Checking the Output folder for existing analyses.** Unnecessary — a story still in Queue
is by definition unanalyzed, because the final step moves it to Done.

## Key Google Drive MCP lessons

- `update_file` is **metadata only** (title, parentId) — cannot update document content
- `create_file` with `textContent` + `contentMimeType: "text/plain"` auto-converts to a
  Google Doc
- `trash_file` may be blocked by auto mode classifier (destructive)
- The read connector backslash-escapes `*`, `#`, `!` in content — `\*text\*` in a doc reads
  back as `\\\*text\\\*`
- **No path-based navigation exists** — everything is ID-addressed; human-readable paths in
  prompts force the session into trial-and-error discovery
- `parentId = 'ID'` in `search_files` is the proper way to list folder contents
- `update_file` can change `parentId` to move a file between folders in one operation
  (alternative to copy + delete)

## File locations

- Analysis Brief v3 (Drive): `1I7dwAPBWAS0UaVvHeRmKik4nvuS1F0-bKfIAPCgBk6I`
- Cowork task prompt: `cowork-task-prompt.txt` (repo root)
- Older prompt (superseded): `Instructions for fimfiction analyzer.txt` (repo root)

## Analysis Brief v4 — corpus-driven refinements (2026-08-17, context window 5)

A meta-analysis session read all 51 output documents (42 unique stories, 9 duplicates) from the
Output folder via Google Drive MCP. The session produced a published Artifact meta-review, an
extensive deliberation about framework refinements, and the v4 Brief.

**What was analyzed:** Every analysis in the Output folder was read in full — 6 directly, 45
via 8 parallel agents. Each document was catalogued for: Brief version compliance, sections
present, perspective mode, highest mechanism reached, primary pairing, formatting issues.

**Key findings driving v4 changes:**

- "Levels" implied quality hierarchy — renamed to "Mechanisms" (structural complexity)
- Per-instance "Theme" column forced hollow propositions onto WI-terminal moments — renamed
  to "Meaning" with named structural purposes as valid terminal destinations
- DT-based knowledge asymmetry was a real technique the vocabulary didn't acknowledge — added
- Comedy, atmosphere, and narrative voice were repeatedly flagged as "blind spots" when they
  are actually prose-craft dimensions outside the framework's scope — added standing constraints
- Cumulative effects were flagged as a "blind spot" when they are handled by the planner's
  subject-wide tracks — added standing constraint noting scene-level granularity is intentional

**v4 Brief changes (complete list):**

1. Renamed "Level 0-4" to "Mechanism 0-4" throughout
2. Added clarifying note: mechanisms describe structural complexity, not quality
3. Added DT-based knowledge asymmetry acknowledgment in vocabulary
4. Renamed per-instance "Theme" to "Meaning" with refined instruction
5. Renamed section 5 from "Inference Ladder Profile" to "Inference Profile"
6. Added metadata header to output format (Brief version, file size, model, date)
7. Added standing constraint: scene-level granularity is intentional
8. Added standing constraint: comedy/atmosphere/voice are prose-craft, not framework gaps
9. Added changelog section
10. Updated "Theme" references in Framework Fit section to "Meaning"

**Cowork task prompt changes:**

1. Updated Brief doc ID to v4
2. Added duplicate detection (check Output folder before starting analysis)
3. Standardized output naming: `[Story Title] - analysis v4`
4. Cron updated from `7 6,12,18,0 * * *` to `7 * * * *` (hourly)

**Operational decisions:**

- Remaining ~50 queued stories run under v4 (hourly cron, single session)
- Reruns of v2/v3 stories under v4: manual re-queue after greenfield exhausted
- Long stories (130KB+): flagged for local Claude Code with 1M context, not Cowork
- Story planner framework changes held as hypotheses pending full corpus analysis

**Hypotheses document:** `docs/CORPUS-ANALYSIS-HYPOTHESES-2026-08-17.md` records 8 hypotheses
for story planner evolution, to be tested against the remaining corpus and reviewed post-vacation.

## Google Drive folder IDs (Reading Archive)

| Resource | ID |
|---|---|
| Reading Archive (parent) | `1upGz5lDqojWnuV-2S2K_m7ReiAz3dkeC` |
| Analysis Brief v4 | `1VUYIhCd70oU0Uyh0sOE88Hf4POxqpRQvaHUYV7oQtEA` |
| Analysis Brief v3 (superseded) | `1I7dwAPBWAS0UaVvHeRmKik4nvuS1F0-bKfIAPCgBk6I` |
| Analysis Brief v2 (superseded) | `1_RaSEPtnB4rl8mgV-tUGZGTozqKPXJIu7NusCL4lwNE` |
| Queue folder | `1MKVZxR9R3J4ualUcrvRhhrh5wc7ftD3z` |
| Done folder | `13BLadzbqFpZrcyjfjWfWWFNa__r0xPW0` |
| Output folder | `1Sqf9j3v5Fi78ipDrA5VzpLJS2Q2b9TFQ` |
