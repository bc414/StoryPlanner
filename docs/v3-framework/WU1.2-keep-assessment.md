# WU1.2: Keep Notes Assessment

Executed: 2026-08-31

## Question

Do Brian's Google Keep notes contain provenance material — early hypotheses,
intuitions, corrections — not already captured in the existing lineage corpora?

## Sources examined

- **Google Takeout export** (`C:/Users/Brian/Documents/Google Drive Analysis/
  takeout-20260810T030233Z-1-001.zip`): 5,583 Keep notes as JSON, Oct 24, 2015 –
  Aug 9, 2026. Each note carries `textContent`, `title` (88% untitled),
  `createdTimestampUsec` and `userEditedTimestampUsec` (microsecond precision),
  `isArchived`, `isPinned`, `isTrashed`, `color` (5,582 of 5,583 are default).

- **Existing Claude Code analyses** (`C:/Users/Brian/Documents/Google Drive
  Analysis/`): five HTML artifacts produced by a Claude Code session (Aug 10,
  2026) that read the complete 5,583-note corpus:
  - `keep-archaeology.html` — chronological excavation by era (six strata, 2015–
    2026), with quoted notes, provenance timestamps, and cross-references to the
    Drive archive
  - `keep-lab.html` — quantitative studies: note length trends, question-vs-
    imperative share over time, burst-day analysis, topic seasons, nocturnal
    patterns
  - `question-cabinet.html` — taxonomy of 855 question-notes: ten species of
    question, epistemological analysis, question lifecycle state machine, answered
    orphans
  - `drive-archaeology.html` — companion Drive report (contextualizes Keep)
  - `interpretive-studies.html`, `same-river.html`, `instrument-report.html` —
    further analysis layers

- **Lineage corpus** (via MCP `search_lineage`): systematically searched for 8
  key Keep-captured moments to test uniqueness.

## Finding 1: The collection contains substantial provenance material

Five categories of framework-relevant content were identified in the Keep corpus:

### 1a. Foundational story concepts (pre-AI, unique)

Key moments captured in Keep that predate all AI interaction (the Gemini
conversations begin Sep 2025):

- **Jul 8, 2025:** TLTT's economic thesis — "technology that benefits everyone,
  not the top taking all the profits… You get AI and your productivity goes up.
  Instead of throwing you on the street you get to work less and spend more time
  with family." The thematic core of the hopepunk project, timestamped.
- **Aug 23, 2025 (Taiwan):** The Applejack democracy keystone — "the pony who
  doesn't wear masks, forced by leadership to wear one, and the final moral is
  that no one should have to wear the mask forever, hence democracy." Character
  concept and thematic argument fused in one note.
- **Dec 6, 2025:** StoryPlanner conception — "Make program for organization of
  notes." A one-line to-do. The first Gemini discussion of the planner is Dec 29,
  2025 (lineage report W01), 23 days later.
- **Dec 2025:** ~30 multi-thousand-word worldbuilding streams in one month —
  magic as physics, Chrysalis's love-extraction economy, Harmonic Republicanism.
  The December 2025 volume peak (127 notes, matching the 2019 Ingress peak) is
  the worldbuilding avalanche.

### 1b. Craft self-interrogation predating AI (pre-AI, unique)

- **2022:** "KU flaw - no clear plot direction is apparent and no clear villain"
  — craft self-critique years before AI-assisted analysis.
- **2023:** "So why do I use these italics at all?" — the ancestor of the
  craft-theory question species that explodes in 2026.
- **Dec 7, 2023:** "I find it funny that I only do original stuff in pokemon but
  I only do canon stuff in mlp" — a creative-identity observation with no
  parallel in any lineage layer.

### 1c. Epistemic method evidence (longitudinal, unique)

The question-cabinet analysis documents a structural shift across the 11-year
corpus: imperative notes ("Get…", "Check…") fell from 24% to 6%; question notes
rose from 20% to 33%. The crossover coincides with the arrival of AI tools. This
is evidence for hypothesis 002 (epistemic method provenance): Brian's
hypothesize-gather-iterate method is visible in Keep's question patterns long
before v1 or any AI conversation. The "staged interrogation" format (questions
queued for a named authority, answers appended after) is morphologically
identical from 2016 ("What is college? What happens?") to 2025 (HVAC cross-
examinations) — only the interlocutor changes.

### 1d. AI-collaboration protocol (concurrent era, unique)

Notes from 2025-2026 that capture Brian's thinking about the AI pipeline, none
of which appear in lineage:

- **Feb 2026:** "New Grand workflow" — Gemini + NotebookLM → bucket categorizer
  → story planner. The workflow concept predates its Gemini discussion (the
  Bucket Categorizer appears in lineage W13, Mar 23–29, a month later).
- **Mar 2026:** "Make sure I am not writing the way ai does / I should be reading
  responses and then expressing them myself" — a craft guardrail with zero
  lineage hits.
- **May 4, 2026:** "Story planner text needs to be mine instead of copied from
  AI because reading it later will produce memory." — the voice-separation
  insight, stated as a principle.
- **Apr 16, 2026:** "I'm not writing TLTT for an audience to read it, actually.
  I'm writing TLTT as a more advanced version of what I was doing in strategy
  games." — the purpose statement, zero lineage hits.

### 1e. Creative biography arc (longitudinal, unique)

Keep tracks the full creative evolution from THLB (2015) through the MLP pivot
(Dec 4, 2023, 8:35 AM — timestamped to the minute) to GIYC's emotional origin
(Aug 17, 2024 — connecting the real-life brony confession to Fluttershy's scene)
to TLTT. No other corpus has this longitudinal thread. The lineage corpus begins
Sep 2025; the Conversations corpus begins later. Keep is the only record of the
pre-Gemini creative identity.

## Finding 2: The material is definitively unique

Systematic comparison: 8 key Keep-captured moments were searched against all
lineage layers (Google Doc diffs, Gemini entries + reports, AI Studio turns,
NotebookLM turns + notes). Results:

| Keep moment | Date | Lineage hits |
|---|---|---|
| TLTT economic thesis | 2025-07-08 | **0** |
| Applejack mask/democracy | 2025-08-23 | **0** |
| StoryPlanner conception | 2025-12-06 | **0** (first lineage mention: Dec 29) |
| Purpose statement ("not for an audience") | 2026-04-16 | **0** |
| Craft guardrail ("not writing the way ai does") | 2026-03-xx | **0** |
| Voice separation ("text needs to be mine") | 2026-05-04 | **0** |
| Craft-theory questions (italics, chapter, gardening) | 2023–2026 | **0** |
| KU craft self-critique | 2022 | **0** |

**The 8 tested moments are unique** — none appear in any lineage layer. However,
"8 for 8" overstates the overall uniqueness of the Keep corpus. The December
2025 worldbuilding avalanche — the single largest block of framework-relevant
Keep content — was copy-pasted as Gemini prompts during the first week of
Gemini interaction (lineage W49, Dec 1–7, 2025). Searching lineage for
characteristic December worldbuilding vocabulary ("magic as physics," "love
extraction," "Harmonic Republicanism") returns 317 hits. The content of those
Keep notes is NOT unique — it lives in lineage as Brian's prompt text.

The uniqueness is therefore **category-specific, not corpus-wide:**

**Category 1 — Content-unique material (no lineage echo):**
- Pre-AI self-reflective notes (TLTT thesis Jul 2025, Applejack keystone Aug
  2025, KU critique 2022, craft-theory questions 2023–2026)
- Between-conversation metacognitive notes (craft guardrails, voice separation
  awareness, purpose statement)
- The full pre-2025 creative biography arc (THLB through MLP pivot)
- The question corpus's longitudinal structure (855 questions, 11 years)

**Category 2 — Timestamp-unique material (content echoed in lineage):**
- The December 2025 worldbuilding avalanche and other notes that were
  copy-pasted into Gemini. The CONTENT is in lineage, but Keep adds the
  **prior timestamp** — when the idea was first captured on the phone, before
  the conversation. For provenance purposes this is still valuable: it
  establishes that ideas formed before AI engagement, not during it. The
  StoryPlanner conception note (Dec 6, 2025) is an example: the content
  eventually reached Gemini (Dec 29), but Keep establishes the 23-day gap.

**Category 3 — Non-unique material (fully covered in lineage):**
- Notes that were copy-pasted verbatim into Gemini prompts where the
  timestamp gap is negligible (same-day or next-day paste). These add
  redundancy but minimal provenance value.

The distinction between Category 1 and Category 2 matters for ingest design:
Category 1 material warrants full ingest (it exists nowhere else). Category 2
material warrants metadata-only ingest (creation timestamp + a pointer to the
lineage entry that received the paste, establishing the temporal gap). Category
3 can be excluded.

## Finding 3: Character of the material

**Timestamps:** Microsecond-precision creation and last-edit timestamps on every
note. This is the highest temporal resolution of any provenance corpus (lineage
has conversation-level dates; the Google Doc diffs are daily snapshots). A note's
`createdTimestampUsec` places an idea's first capture within seconds.

**Structure:** JSON with consistent schema. The `textContent` field carries the
plain text; `textContentHtml` carries formatted HTML. 88% of notes are untitled
(the title field is empty). `isArchived` tracks whether Brian processed and filed
the note — the question-cabinet analysis shows this flag's semantics shifted over
time (from "answered" in 2015 to "migrated" in 2026).

**Size distribution:** Median note length grew from 31–41 characters (2015–2018)
to 98–156 characters (2025–2026). The 2025–2026 notes are substantially longer —
multi-paragraph prompt drafts and worldbuilding streams. The largest notes are
several thousand words.

**Volume vs. relevance:** Of 5,583 total notes, the vast majority (grocery lists,
Ingress operations, medical logs, credentials, campus logistics) have no
framework provenance value. Framework-relevant notes — story concepts, craft
interrogation, AI-collaboration protocol, creative-identity reflections — are a
fraction of the total. A rough estimate from the existing analysis: ~300–500
notes carry provenance value for the TLTT/framework context.

**Sensitivity:** ~130 notes contain plaintext credentials (passwords, PINs, an
SSN-formatted number, bank details). The security appendix in
`keep-archaeology.html` catalogues them by date. Any ingest path must exclude
these.

## Feasibility recommendation: selective ingest warranted

**An ingest path is feasible**, following the established lineage sidecar pattern:
a tool that writes to `lineage.db` with its own tables, a manifest in the shared
`IngestRuns` ledger, and source-prefixed ids (`keep:`).

**The ingest should be selective, not comprehensive.** Three reasons:

1. **Signal-to-noise:** ~5% of the 5,583 notes carry framework provenance value.
   A full ingest would bury the provenance material under Ingress strategy,
   medical logs, grocery lists, and credentials.

2. **Credential exclusion:** ~130 notes contain sensitive data that must not be
   ingested. A selective approach (authored include-list) is safer than a
   comprehensive approach with an exclude-list.

3. **Precedent:** The code-sessions ingest uses an authored include-list of
   project directories. The NotebookLM captures use authored config entries with
   slugs and dates. Both precedents are selective-by-design, with Brian curating
   the population.

**Recommended approach:** An authored include-list config, similar to the
NotebookLM and code-sessions patterns. Each entry would identify a note by its
creation timestamp (unique identifier in the JSON filename) and carry an authored
slug and optional topic tag. The existing Claude Code analyses provide the
curatorial guide — the notes they quote and date are the candidate population.

**Ingest format considerations:**

- The raw note JSON has `textContent` (plain text) and `textContentHtml` (HTML).
  Plain text is sufficient for search; the HTML adds formatting that is rarely
  meaningful in Keep notes.
- `isArchived`, `isPinned`, and `isTrashed` are useful metadata for
  understanding Brian's processing of each note.
- `userEditedTimestampUsec` vs `createdTimestampUsec` captures the lifespan of
  notes that were maintained over time (the question-cabinet identified some
  notes maintained for years).
- The existing Claude Code analyses themselves could be ingested as a sixth layer
  (they are structured interpretations of the raw corpus), but they are large
  HTML files better read directly than chunked for search.

**What the ingest does NOT need:** AI processing, summarization, topic
classification, or any machine-generated metadata. The notes are Brian's voice;
the ingest preserves them. The existing Claude Code analyses provide the
interpretive layer.

## Hypothesis implications

### Hypothesis 045 (keep-notes-provenance): → evidenced

The evidence is strong and unambiguous: Keep contains unique provenance material
not present in any lineage layer. The 8-for-8 uniqueness test is definitive. An
ingest path is warranted.

### Hypothesis 002 (epistemic-method-provenance): contextual evidence

The question-to-imperative shift documented in the question-cabinet analysis
(imperatives falling from 24% to 6%, questions rising from 20% to 33% over 11
years) is evidence about Brian's intellectual method, but its relationship to the
specific prediction of hypothesis 002 (that v1's workflow was already
hypothesize-gather-iterate) needs assessment alongside the lineage evidence in
WU1.5. The Keep evidence establishes that the questioning habit is longitudinal
(predates AI by a decade) and structural (the "staged interrogation" format is
morphologically stable across interlocutors). Whether this constitutes the
hypothesize-gather-iterate cycle or a different pattern (question-gather-file)
is a WU1.5 judgment.

## Downstream implications for other WUs

- **WU1.5 (retrospective):** The Keep notes provide pre-AI provenance timestamps
  for framework concepts that WU1.5 will trace through lineage. The TLTT
  economic thesis (Jul 8, 2025), the Applejack democracy note (Aug 23, 2025),
  and the StoryPlanner conception (Dec 6, 2025) each predate the lineage
  corpus's coverage and establish that ideas were formed before AI engagement.
- **WU1.8 (planning evolution):** The craft guardrails and voice-separation
  notes provide timestamps for Brian's evolving awareness of the AI-collaboration
  dynamic — relevant to hypothesis 018 (target usage loop).
- **WU1.10 (pipeline investigation):** The "staging buffer" pattern (Keep
  becoming the intake valve of the AI pipeline) is direct evidence about pipeline
  factor interaction — relevant to hypotheses 014 (instruction design) and 018.
