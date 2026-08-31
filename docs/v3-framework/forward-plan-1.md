# Forward Plan 1

Created: 2026-08-31
After: consolidation-1

## Rationale

Consolidation-1 produced 45 hypotheses from three source documents, organized in
ten conceptual tiers (A-J). All 45 are `untested`. No evidence has been formally
deposited against any hypothesis in the skill's record format. This plan is the
first experimental agenda: what evidence to gather, in what order, to move the
hypothesis set from untested toward evidenced or challenged.

### What the landscape looks like

The hypothesis set spans two domains: the **narrative design framework** (what the
planner tracks — Tiers A, E-J, 32 hypotheses) and the **operating pipeline** (how
model, data, instructions, and harness compose — Tiers B-D, 13 hypotheses). The
consolidation noted that these two domains are predicted to be separable (hypothesis
008), and the pipeline hypotheses are uniterated from a Google Keep dump that may
consolidate further as pipeline work gets scoped. This plan addresses both domains
but acknowledges their different evidence bases and maturity levels.

The framework domain has rich evidence sources ready: the 112-story corpus (7
meta-analysis reports), Brian's own fiction (6 stories + naive chapters, italics
verified), the v1 archive (450 plot points, 1,125 links, accessible via MCP), the
lineage corpus (all layers ingested), and the v2 working plan (accessible via MCP).
Framework hypotheses are testable now.

The pipeline domain has thinner evidence and more speculative hypotheses. Its
evidence sources — lineage transcripts, session records, configuration history —
exist but have not been systematically mined for pipeline-specific questions. Some
pipeline hypotheses (010-013, model-intrinsic properties) may not be testable with
current evidence at all, requiring controlled model comparisons that are expensive
and outside the framework buildout's primary mission. The pipeline WU is designed
to extract what evidence exists and identify which hypotheses need a different kind
of investigation.

### Strategic ordering

Five ordering considerations shape this plan:

**1. The corpus synthesis is the highest-value first step.** It produces the
baseline against which everything else is assessed — the retrospective needs it to
evaluate framework provenance, the cross-corpus comparison needs it as one of three
corpora, and the framework evaluation draws on it directly. No experiment can
bypass this step, and it has no preconditions.

**2. Infrastructure hypotheses deserve early attention.** Three hypotheses predict
properties of the experimental infrastructure itself:

- **045** (Keep notes provenance): does a new evidence source exist? If the Keep
  notes contain unique provenance material, later WUs should be designed to use it.
  Quick to assess.
- **008** (framework vs pipeline separable): if confirmed, the pipeline WU can
  proceed independently from the framework evidence chain. If challenged, pipeline
  findings may constrain framework design.
- **020** (voice separation prerequisite): if confirmed, framework evolution is
  constrained — track definitions built on mixed-voice data inherit the confusion.
  This determines whether the framework evaluation (WU1.12) must wait for voice
  separation or can proceed on current data.

All three can be assessed early without large-scale experiments: 045 by inspecting
the Keep notes, 008 by examining the evidence-base overlap between framework and
pipeline hypotheses, and 020 as a byproduct of V1 archive mining (WU1.4).

**3. Two WUs need skills that don't exist yet.** Own fiction analysis (WU1.3)
needs an adapted analyze-story skill that handles self-diagnostic framing and
unfinished works. V1 archive mining (WU1.4) needs a new skill for consistent
extraction methodology, voice attribution, and structured output. Both skills are
preconditions — the WUs are in this plan with the preconditions noted, not deferred
to a future plan. Building these skills is part of the work.

**4. Planning evolution data needs preprocessing.** The KU/NTL, GIYC, and Falldale
revision histories exist as raw exports in `Planning_Document_Revision_History/`
(mix of `.txt` appscript exports and manually copied text). They need preprocessing
before `GDocHistory` can ingest them into lineage.db. This is Brian's task; WU1.8
is designed to proceed without it if needed.

**5. The convergence pattern is: evidence → comparison → adjudication →
evaluation → connection.** Three primary evidence WUs (corpus synthesis, own
fiction, V1 mining) feed into a cross-corpus comparison, which feeds into a
favorites lens, which feeds into hypothesis adjudication and framework evaluation,
which feeds into the connection to TLTT. The retrospective and pipeline
investigation run alongside this chain, feeding into the adjudication and
evaluation at the convergence point. The working plan assessment and planning
evolution are independent smaller WUs that feed into specific hypotheses.

### Hypothesis coverage

Every hypothesis is targeted by at least one WU. The mapping:

| Tier | Hypotheses | Primary WUs |
|------|-----------|-------------|
| A (001-005) | Purpose, epistemology | WU1.5 (retrospective), WU1.7 (working plan), WU1.12 (framework eval), WU1.13 (connection) |
| B (006-009) | Pipeline decomposition | WU1.10 (pipeline) |
| C (010-013) | Model properties | WU1.10 (pipeline) |
| D (014-018) | Instructions, environment | WU1.5 (retrospective), WU1.8 (planning evolution), WU1.10 (pipeline) |
| E (019-022) | Voice, interaction | WU1.4 (V1 mining), WU1.5 (retrospective) |
| F (023-027) | Framework architecture | WU1.1 (corpus), WU1.9 (cross-corpus), WU1.12 (framework eval) |
| G (028-032) | Perspective, perception | WU1.1 (corpus), WU1.3 (own fiction), WU1.5 (retrospective), WU1.9 (cross-corpus) |
| H (033-037) | Goals, scope, boundaries | WU1.1 (corpus), WU1.3 (own fiction), WU1.9 (cross-corpus), WU1.12 (framework eval) |
| I (038-040) | Brian's practice | WU1.3 (own fiction), WU1.4 (V1 mining), WU1.9 (cross-corpus) |
| J (041-045) | Planner instrument | WU1.4 (V1 mining), WU1.5 (retrospective), WU1.2 (Keep assessment), WU1.7 (working plan), WU1.12 (framework eval) |


## Work Units

### WU1.1: Corpus Synthesis

**Question:** What do the 112 analyzed stories do at the scene level — what
patterns emerge from first-principles consolidation of the 7 meta-analysis reports?

**Hypotheses:** 023, 024, 025, 026, 028, 029, 030, 031, 032, 033, 034, 035, 036,
040

**Evidence sources:** The 7 meta-analysis reports (4.1a, 4.2a-e, 4.3) in
`source_material_references/Reading Archive Analyses/`.

**Scope:** First-principles consolidation across all 7 reports. Cross-category
patterns: what recurs across romance, dark-premise, AU, ensemble, and explicit/plot
stories vs what is category-specific. The M4/FID question (the 7:1 calibration
divergence and the FID→M4 correlation the reports documented). Framework gaps:
where the v4 Brief measured accurately vs where it was overfit or blind. Unnamed
techniques that recur across categories. Length effects on mechanism distribution,
perspective technique, and obstacle architecture. The narrator-character blend's
frequency and treatment across the corpus. Non-thematic goal categories observed
(humor, structural setup, emotional investment). WI-terminal patterns — how many
chains terminate at structural purpose vs thematic argument. Embedded text and dream
sequences as structural devices. First-person narration effects on perception gap
delivery.

The synthesis should discover patterns, not only confirm expected ones. If
hypothesis-relevant patterns emerge, they feed into the target hypotheses' evidence
records. Patterns that don't map to any existing hypothesis may indicate gaps in
the hypothesis set — note them for the wrap-up step.

**What it does NOT do:** Apply the favorites lens (that is WU1.11). Compare to
Brian's own fiction (that is WU1.9). Reference TLTT or the working plan. Propose
tracks or display questions.

**Output:** A consolidated corpus synthesis report
(`docs/v3-framework/WU1.1-corpus-synthesis.md`).

**Scale:** Single session reading 7 reports. The reports total several hundred
pages; the session needs the full 1M context.

**Preconditions:** None. Can start immediately.

**Status:** proposed

(No dependencies — this is the first WU to start. Highest downstream value: WU1.5,
WU1.9, WU1.11, and WU1.12 all depend on its findings.)

---

### WU1.2: Keep Notes Assessment

**Question:** Do Brian's Google Keep notes contain provenance material — early
hypotheses, intuitions, corrections — not already captured in the existing lineage
corpora?

**Hypotheses:** 045, and potentially 002

**Evidence sources:** Brian's Google Keep notes in
`C:/Users/Brian/Google Drive Analysis`. The existing lineage corpus (Gemini
conversations, AI Studio chats, NotebookLM captures) as the comparison baseline
for uniqueness.

**Scope:** This is a quick assessment, not a full ingest. Read the Keep notes
and answer three questions: (1) Does the collection contain material with
provenance value (hypotheses about the story or framework, intuitions, corrections,
fragments of ideas that influenced later decisions)? (2) Is any of that material
unique — not already present in the Gemini conversations, AI Studio chats, or
NotebookLM captures in lineage.db? (3) If unique material exists, what is its
character — timestamped or undated? Structured or fragmentary? Would a lineage
ingest path (following the sidecar pattern: tool writes to lineage.db, own tables,
manifest in IngestRuns, source-prefixed ids) be feasible, or would the material
resist structured ingest?

If the assessment finds unique provenance material, hypothesis 045 moves toward
`evidenced` and an ingest path is warranted — the forward plan's later WUs
(particularly WU1.5, WU1.8, and WU1.10) should be designed to query it. If the
assessment finds that the Keep notes duplicate material already in lineage, 045
moves toward `challenged` and no ingest path is needed.

**What it does NOT do:** Build the ingest path (that is downstream implementation
work). Mine the notes exhaustively. Judge the quality of the ideas in the notes.

**Output:** A brief assessment document
(`docs/v3-framework/WU1.2-keep-assessment.md`) with findings on uniqueness,
character, and feasibility recommendation.

**Scale:** Single session. Quick — hours, not days.

**Preconditions:** Access to the Google Keep notes directory.

**Status:** proposed

(Independent of all other WUs. Infrastructure hypothesis — test early to determine
whether a new evidence source exists before later WUs are scoped in detail.)

---

### WU1.3: Own Fiction Analysis

**Question:** What does Brian instinctively do when he writes — what mechanism
profiles, perspective techniques, interiority rendering, and structural patterns
appear in his fiction?

**Hypotheses:** 028, 029, 030, 031, 032, 037, 038, 039, 040

**Evidence sources:** Brian's 6 published stories + naive TLTT chapters, all as
local markdown files in `source_material_references/open_stories_md/` and
`source_material_references/`:

- THLB (Pokemon, 2015-2017, first-person alternating POV)
- Wish (Pokemon, 2020, one-shot)
- TEatS (Pokemon, 2020-2021)
- NTL (Pokemon, 2021-2023, unfinished)
- GIYC (MLP, 2024-2025, unfinished)
- Falldale (EaW, Nov 2025, one-shot)
- Naive TLTT Ch1-2 (June 2025)

**Scope:** Apply the v4 Analysis Brief (adapted for self-diagnostic framing) to
each text. Then produce a self-diagnostic synthesis comparing across all 7 texts.
The core question is open-ended: what does Brian instinctively do? Expected areas
based on prior analysis include mechanism profiles, interiority technique
preferences (DT vs FID vs narrator-character blend), perspective discipline, and
the DT/FID relationship — but the synthesis should discover patterns, not just
confirm expected ones.

Hypothesis 039 (FiM reading effect) is directly testable here: compare the pre-FiM
Pokemon stories (THLB 2015-2017, Wish 2020, TEatS 2020-2021, NTL 2021-2023)
against the post-FiM texts (GIYC 2024-2025, Falldale Nov 2025, naive TLTT June
2025) for changes in FID usage, variable-focalization sophistication, and
interiority rendering. THLB's alternating first-person already shows perspective
restriction instincts, so the prediction is not that perspective awareness appeared
from nothing but that the specific toolkit shifted.

Hypothesis 040 (fabula-dialogue replacement) is testable against the naive chapters
specifically: do they show the fabula-through-dialogue delivery pattern that the
hypothesis predicts was the v0 wall? What techniques (if any) do the later texts
use instead?

Handle unfinished works (NTL, GIYC) by analyzing what exists, noting the
incompleteness as context rather than a defect. The naive TLTT chapters are partial
text, not an unfinished story — they predate v1 entirely and are evidence of v0
prose instincts.

**What it does NOT do:** Compare to the 112-story corpus (that is WU1.9). Judge
quality. Propose what Brian should change about his writing.

**Output:** 7 per-text v4 analyses (in
`source_material_references/Reading Archive Analyses/`) + an own-voice synthesis
report (`docs/v3-framework/WU1.3-own-voice-synthesis.md`).

**Scale:** 7 subagent runs (skill: analyze-story, adapted), then 1 synthesis
session.

**Preconditions:** An adapted analyze-story skill that handles self-diagnostic
framing, unfinished works, and the naive chapters as partial text. The skill
adaptation is part of the buildout work — build it before running this WU. All
source texts are ready (CORPUS-STATUS: "All texts converted with italics intact").

**Status:** proposed

(Independent of WU1.1. Blocks WU1.9 and WU1.11. The skill adaptation is the
gating precondition.)

---

### WU1.4: V1 Archive Mining

**Question:** What scene-level instincts did Brian capture in v1 before having
formal vocabulary, and whose voice is doing the capturing?

**Hypotheses:** 019, 020, 021, 022, 038, 043, 044

**Evidence sources:** All 450 plot points, 1,125 links, and relevant notes in the
v1 archive, accessible via MCP (`get_plot_points_archive`, `get_links_archive`,
`get_notes_archive`). The Gemini conversation corpus in lineage
(`search_lineage source:"gemini"`) for voice separation.

**Scope:** Read ALL plot point notes and link notes — no skipping, no sampling.
The core question is open-ended: what is Brian instinctively doing at the scene
level, and whose voice is doing it?

**Voice attribution** is the infrastructure contribution of this WU. For each
note, determine whether the voice is Brian's (fabula, syuzhet/design, analytical,
prose/craft) or Gemini's (AI analytical) by grepping the note text against the
lineage corpus. Gemini-voice content is identifiable because the full-plan-paste
paradigm means Gemini's output was copied back into the plan; the same text should
appear in the Gemini conversation corpus. Brian's analytical voice (his own
observations, corrections, hypotheses in conversation turns) must be distinguished
from Gemini's rather than lumped as "AI." The five-voice register model (hypothesis
021) provides the taxonomy; this WU tests whether five is the right count.

**Scene-level density** is the content contribution. The v1 archive has a rich
scene graph that v2 could not replicate because scene-level work stalled (hypothesis
044). Mining the scene graph reveals what Brian instinctively designed: perception
gap setups, prior-belief management, revelation sequencing, structural parallels —
all without formal vocabulary. Categorize what is found by the mechanism and goal
vocabulary the v3 framework is developing (hypotheses 023-036), but the categories
should EMERGE from the data, not be imposed.

**Note-to-note relationships** (hypothesis 043) are testable here: does the v1
archive show designed connections between moments (a setup note in chapter 3 and
its payoff in chapter 15) that are currently invisible in the data? The v1 archive's
1,125 links are typed plot-point-to-subject connections, not note-to-note edges;
the question is whether the NOTE CONTENT reveals relationships the link structure
cannot represent.

**Voice contamination assessment** (hypotheses 019, 020) is the infrastructure
output: how much of the v1 archive is Gemini voice vs Brian's voice? If the
contamination is extensive, hypothesis 020 (voice separation as prerequisite for
framework evolution) gains supporting evidence. If the contamination is localized
or easily separable, 020 may be challengeable.

**What it does NOT do:** Migrate anything to v2/v3. Propose tracks. Write display
questions. Judge whether Brian's instincts are "correct." Clean the data — that is
the downstream voice linting protocol (implementation candidates).

**Output:** Per-arc files in `docs/v3-framework/WU1.4-v1-scene-instincts/`, each
in a structured format (note ID, voice attribution, discovered category, chapter,
excerpt). The structured format serves WU1.9 (cross-corpus comparison can query
patterns) and the downstream voice linting protocol (voice attribution enables
grep-based cleanup). One file per arc batch + a summary.

**Scale:** Multiple subagents batched by TLTT chapter arc (e.g., Arc 1 chapters,
Arc 2 chapters, etc.), plus Aris and Paratext.

**Preconditions:** A new skill for V1 archive extraction — consistent methodology,
voice attribution via lineage grep, structured output format. The skill must be
built before running this WU. The v1 archive is accessible now via MCP; no other
blockers.

**Status:** proposed

(Independent of WU1.1. Blocks WU1.9 and WU1.11. Tests infrastructure hypothesis
020 — results determine whether framework evolution must wait for voice separation.
The skill is the gating precondition.)

---

### WU1.5: Retrospective

**Question:** Where did the current framework vocabulary come from, and where is
it overfit?

**Hypotheses:** 002, 004, 005, 007, 014, 019, 022, 028, 029, 041, 042, 044

**Evidence sources:**
- NLM Perspective Analysis notebook (lineage `nlm:3`, 172 turns)
- NLM Refinement of Aquileian Lore (lineage `nlm:6`)
- AI Studio fabula session (lineage `aistudio:6`, Mar 27 2026)
- The five framework-origin conversations: conv 8 (Conscience), conv 17
  (multi-story fabula), conv 21 (perception gap + data architecture, 151 blocks),
  conv 36 (planning vs writing, 72 blocks), conv 47 (note categorization
  bootstrapping, 285 blocks)
- Conv 64 (P&K ASOIAF inspirations, 289 blocks — extract framework-relevant blocks)
- Google Doc revision history (lineage `gdoc:` ids, 53 diffs + 54 snapshots)
- WU1.1 findings (corpus evidence baseline to assess against)
- The provenance claims from VERSION-HISTORY-DRAFT1.md (the unverified provenance
  table preserved in consolidation-1-plan.md)

**Scope:** Trace the provenance of each major framework concept. For each, assess
against the corpus evidence from WU1.1: confirmed, partially validated, overfit,
or untested. Position NLM and early Claude sessions as best efforts that did
important work with limited data.

Specific provenance chains to trace:

1. **The FID fixation chain** (hypotheses 028, 029): NLM t#54 (DT/FID
   distinguished) → NLM t#94-130 (deep third prescribed for TLTT) → conv 21
   (perception gap designed technique-agnostic, then narrowed to FID in Track 99)
   → conv 36 block 1245 (italics epiphany cemented FID as the goal). Each step was
   reasonable; the cumulative effect was overfit. Does the corpus evidence from
   WU1.1 support the v3 reframing (variable focalization as master principle, FID
   as one mode)?

2. **The NLM vocabulary provenance** (hypothesis 002, 028): the consolidation plan
   noted that "NLM introduced vocabulary from Pokemon analysis, not from P&K —
   vocabulary predated P&K" is an unverified claim. Verify against lineage: when do
   deep-third/FID/DT/variable-focalization terms first appear? Does the vocabulary
   predate NLM analysis of Pokemon stories Brian read, or did NLM introduce it?

3. **The epistemic method hypothesis** (hypothesis 002): is v1's
   capture-everything pattern actually hypothesize-gather-iterate in practice
   (before the vocabulary existed), or is that a retrospective reading? The lineage
   evidence should show whether Brian was testing and correcting ideas in v1
   (supporting 002) or accumulating them without testing (challenging 002).

4. **The "every link must have T" rule** (hypothesis 036): conv 47 block 1520.
   Does the corpus evidence support relaxing it?

5. **The four gap types** (hypothesis 029): ironic/tragic/closing/aligned from
   conv 21 block 613. One AI's taxonomy, held as hypothesis. Does the corpus show
   these four types, different types, or a more complex picture?

6. **The v2 prescribed workflow** (hypotheses 041, 042): Stage 0→1→2→3, five
   EditorModes. What stalled and why? Was the sequential model the problem, or the
   specific mode designs, or the narrowness of the experience-design vocabulary?
   Lineage evidence from the v2 working period should show what was attempted and
   where it broke down.

7. **The AI context contradiction** (hypothesis 019): trace the read-generate-
   paste-reread feedback loop through lineage evidence. Reports W05-W07 document
   the ~940K-char paste scale. The Gemini conversations should show the cycle: the
   plan enters the prompt, the AI produces output, Brian pastes output back, the
   next session's plan paste contains the AI's prior output.

8. **The unverified provenance table** from VERSION-HISTORY-DRAFT1.md: nine claims
   about when specific concepts first appeared (fabula/syuzhet at aistudio:6,
   Architect/Gardener at NLM nlm:3, etc.). Each row is checkable against lineage.
   Deposit findings as evidence into the relevant hypothesis records.

**What it does NOT do:** Propose framework replacements (that is WU1.12). Judge
whether prior work was "wrong" — it was best effort with the data available.

**Output:** A retrospective document
(`docs/v3-framework/WU1.5-retrospective.md`) with per-concept provenance chains
and assessments against corpus evidence.

**Scale:** Single session. All sources accessible via MCP and lineage.

**Preconditions:** WU1.1 complete (needs the corpus evidence baseline to assess
framework concepts against).

**Status:** proposed

(Requires WU1.1 complete. Independent of WU1.3 and WU1.4. Feeds into WU1.11 and
WU1.12.)

---

*(WU1.6 skipped — numbering artifact from draft.)*

---

### WU1.7: Working Plan Assessment

**Question:** What does the v2 working plan data actually look like — what state
distributions, track usage patterns, and mode adoption patterns exist?

**Hypotheses:** 003, 004, 041, 042

**Evidence sources:** V2 working plan via MCP: `get_stats`, `count_notes_plan`,
`get_track_definitions`, `list_subjects`, `list_stories`,
`get_chapters_plan`, `get_notes_plan`.

**Scope:** A data survey of the live working plan, not a deep analysis. Four
questions:

1. **NoteState distribution** (hypothesis 003): How many notes are in each state
   (unset, confirmed, flagged)? The epistemic vocabulary hypothesis predicts that
   the three-state model should be replaced. The current distribution — specifically
   the zero-Confirmed count documented in CLAUDE.md — is evidence about how the
   existing vocabulary has been used (or not used). Query `get_stats` and
   `count_notes_plan` to establish the exact distribution.

2. **Track usage density** (hypothesis 004): Which tracks have notes? How many?
   The cadence hypothesis predicts that batch sweeps are the natural working pattern.
   If track population is lumpy (dense in tracks used during capture sweeps, sparse
   in tracks that require incremental attention), that supports the sweep hypothesis.
   Query across all tracks.

3. **Scene-level content** (hypothesis 041): How many plot-point-level notes exist
   vs subject-level notes? The sufficient-stability hypothesis predicts that scene
   design was gated on subject completion and stalled. If the data shows dense
   subject-level content and sparse scene-level content, that is consistent with
   the predicted stall.

4. **Editor mode usage** (hypothesis 042): The EditorModes hypothesis predicts that
   the mode designs may not be optimal. What does actual usage look like? (The data
   itself may not reveal mode-specific usage patterns directly, but the track
   populations by cognitive mode — ZeroFocalization vs NarrativeDesign — show
   whether the cognitive split was practiced in the data.)

**What it does NOT do:** Evaluate the framework (that is WU1.12). Mine note
content for patterns (that is v2-scope work, not v1 mining). Propose changes to
NoteState or tracks.

**Output:** A brief assessment document
(`docs/v3-framework/WU1.7-working-plan-assessment.md`) with data survey findings.

**Scale:** Single session. Quick — queries via MCP, no large-scale reading.

**Preconditions:** None. Can start immediately.

**Status:** proposed

(Independent of all other WUs. Quick. Most valuable alongside WU1.5 — the
working plan data contextualizes the retrospective's findings about what stalled
and why.)

---

### WU1.8: Planning Evolution

**Question:** How did Brian's scene-level thinking, thematic framework, and
perspective decisions evolve across each story's planning lifecycle?

**Hypotheses:** 002, 004, 018, 039

**Evidence sources:**
- TLTT revision history (already in lineage: 53 diffs, `gdoc:` ids)
- KU/NTL revision history (raw exports in `Planning_Document_Revision_History/`)
- GIYC revision history (raw exports in `Planning_Document_Revision_History/`)
- Falldale revision history (raw exports in `Planning_Document_Revision_History/`;
  the planning doc was "almost as long as the prose itself," a proto v1 story
  planner)

**Scope:** Trace how thinking evolved across each story's planning lifecycle.
Identify which instincts were stable across years vs which changed. The v0
paradigm's fabula/syuzhet mix (or lack of separation) should be visible in the
earliest TLTT revisions; later revisions may show the separation emerging.

For hypothesis 002 (epistemic method provenance): do the planning documents show
hypothesize-gather-iterate at work before the vocabulary existed? Or are they
accumulation without testing?

For hypothesis 004 (working cadence): do the revision histories show batch sweeps
(large changes concentrated in time) or incremental editing (small changes spread
evenly)?

For hypothesis 018 (target usage loop): each story's planning workflow was shaped
by the available tooling. KU/NTL and GIYC were Google Docs (v0 paradigm). Falldale
was a v0 document but created in November 2025, contemporary with early v1. TLTT
transitioned from Google Doc to v1 planner. Do the revision patterns differ across
stories in ways that correlate with tooling changes?

For hypothesis 039 (FiM reading effect): do the planning documents show a shift
in Brian's thinking about perspective technique that correlates with his FiM
reading period?

**What it does NOT do:** Analyze the prose (that is WU1.3). Propose framework
changes.

**Output:** A planning-evolution synthesis
(`docs/v3-framework/WU1.8-planning-evolution.md`).

**Scale:** Processing (preprocessing + GDocHistory ingest for the non-TLTT
stories) then 1 synthesis session.

**Preconditions:** Brian preprocessing the raw revision history exports in
`Planning_Document_Revision_History/` and running the GDocHistory ingest. The raw
exports exist; the preprocessing pipeline exists (the TLTT revision history was
already processed through it). Brian's action is required. The TLTT revision
history is already in lineage and can be analyzed independently while the other
stories' histories are being preprocessed.

**Status:** proposed

(Independent of WU1.1, WU1.3, WU1.4. Blocks on Brian's preprocessing action.
Feeds into WU1.12 and WU1.11 if available, but neither hard-blocks on it.)

---

### WU1.9: Cross-Corpus Comparison

**Question:** What do stories Brian reads do vs what does Brian instinctively do
vs what did v1 capture — and where do these three corpora converge or diverge?

**Hypotheses:** 023, 024, 025, 026, 027, 028, 029, 030, 033, 034, 037, 038, 039,
040

**Evidence sources:** WU1.1 (corpus patterns), WU1.3 (own fiction patterns),
WU1.4 (v1 archive instincts).

**Scope:** The convergence point for the three primary evidence WUs. Four
comparison axes:

1. **Confirmation of instinct:** What techniques appear in the 112 analyzed stories
   that Brian also uses instinctively (from WU1.3 and WU1.4)? These are candidates
   for framework vocabulary that names what Brian already does.

2. **Potential learning:** What techniques appear in the corpus that Brian doesn't
   use (based on WU1.3 and WU1.4)? These are candidates for framework vocabulary
   that could expand Brian's toolkit — but "potential learning" is a description,
   not a recommendation. Brian decides what to adopt.

3. **Distinctive voice:** What does Brian do instinctively (from WU1.3 and WU1.4)
   that the corpus doesn't show? These are Brian's signature patterns — things the
   framework should support even if they are rare in the broader corpus.

4. **Voice separation overlay:** From WU1.4's voice attribution, how does Brian's
   instinctive scene-level practice (his fabula, syuzhet, analytical, and
   prose/craft voices) relate to the corpus patterns? Is Brian's own analytical
   voice identifying the same patterns that the corpus analysis identifies formally?

For hypothesis 037 (multi-story focalization): compare technique profiles across
Brian's stories that serve different paradigms (THLB's first-person alternating vs
Falldale's third-person one-shot vs GIYC's ongoing vs naive TLTT's ensemble). Do
the profiles differ in ways that correlate with the paradigm differences the
hypothesis predicts?

For hypotheses 023-024 (three-concern separation, dimensional vs hierarchical):
does the cross-corpus evidence show goal, mechanism, and technique varying
independently across all three corpora? Or are they correlated in ways that
challenge the dimensional model?

**What it does NOT do:** Test hypotheses systematically (that is WU1.11). Propose
tracks. Apply the favorites lens (that is WU1.11).

**Output:** A cross-corpus comparison report
(`docs/v3-framework/WU1.9-cross-corpus-comparison.md`).

**Scale:** Single session reading 3 synthesis reports (WU1.1, WU1.3, WU1.4
outputs).

**Preconditions:** WU1.1, WU1.3, and WU1.4 complete.

**Status:** proposed

(Requires three prior WUs. Central convergence point — WU1.11 and WU1.12 both
draw on it heavily.)

---

### WU1.10: Pipeline Investigation

**Question:** What does the available evidence say about the four-factor pipeline
decomposition, model-intrinsic properties, and instruction design — and are the
pipeline hypotheses separable from the framework hypotheses?

**Hypotheses:** 006, 007, 008, 009, 010, 011, 012, 013, 014, 015, 016, 017, 018

**Evidence sources:**
- Lineage corpus (Gemini conversations, AI Studio chats, NLM notebooks — tracing
  how each pipeline factor changed across eras)
- Code sessions (`codesessions.db` via sqlite3 — tracing harness evolution and
  instruction design decisions)
- Session transcripts from the v2 and v3 building periods (in lineage and
  conversations)
- Configuration evidence: the `.mcp.json`, CLAUDE.md revision history (git log),
  skill files, MCP server instructions (the current instructional text stack)
- VERSION-HISTORY.md (the dated factor-change timeline)

**Scope:** This WU addresses all 13 pipeline hypotheses (Tiers B-D). The
hypotheses cluster into four sub-questions, each with different evidence
availability:

**Sub-question 1: The decomposition model (006-009).** Can the four factors
(model, data, instructions, harness) be retroactively identified as independently
varying across era changes? The lineage corpus records the era transitions with
dates: v0→v1 (Gemini + WPF app + plan-paste), v1→v2 (Claude + Type Object
rebuild + cognitive modes), v2→v3 (MCP + skills + CLAUDE.md). For each
transition, identify which factors changed and which held constant. If multiple
factors always changed together, the independence claim (006) is weakened. If
some transitions changed only one factor (e.g., Claude web chat arriving in
April 2026 changed the model factor without changing data, instructions, or
harness), independence is supported. Hypothesis 008 (framework-pipeline
separability) is testable by examining whether the evidence bases for framework
and pipeline hypotheses overlap — if they are drawing from the same evidence and
the same mechanisms, separability is challenged.

**Sub-question 2: Model-intrinsic properties (010-013).** What can be determined
about model-intrinsic properties from available evidence? This sub-question has
the thinnest evidence base: controlled model comparisons (same data, same
instructions, different models) have not been run. What exists: Brian's
impressions of Gemini vs Claude vs Fable (in lineage and code sessions), the
pre-MCP vs post-MCP conversation quality (confounded with data architecture
changes), and Sonnet 4.6's documented historical preference (009). Mine what
evidence exists, note which hypotheses remain untestable with current evidence,
and identify what controlled experiments would be needed to make them testable.
Do NOT recommend running expensive controlled experiments — surface the evidence
gap and move on.

**Sub-question 3: Instruction design (014-015).** What evidence exists for
evidence-based instruction design? The lineage corpus contains the instruction
evolution: v1's custom Gemini gem (four rules), AI Studio system prompts, v2's
absence of instructions ("Claude just works"), v3's CLAUDE.md + skills. The
conversation corpus and code sessions contain acceptance signals (hypothesis 015):
copy-pasted AI text in the plan (lineage can identify these), Conversation Reader
block states (done/flagged/skipped), Brian's corrections in user turns, and
endorsed vs rejected proposals. This sub-question assesses what the instruction
evolution looked like and whether the acceptance signals are systematically
mineable, not whether they should be mined (that is downstream implementation
work).

**Sub-question 4: Architecture questions (016-018).** Are there patterns in how
Brian uses the Desktop vs Code consumers, how data sources are queried, and what
the target workflow looks like? These are the most architectural of the pipeline
hypotheses and may be more assessable from reflection on current usage patterns
than from historical evidence. The code-sessions corpus records which tools and
MCP queries are used in Claude Code sessions; the conversation corpus records
Desktop's interaction patterns. Whether the data-source-unification question (016)
is worth pursuing vs whether the bespoke schemas serve well enough is partly a
judgment about engineering effort, not purely an evidence question.

**What it does NOT do:** Run controlled model comparisons. Build an instruction
audit framework. Implement data-source unification. These are all downstream of
the evidence-gathering this WU does.

**Output:** A pipeline investigation report
(`docs/v3-framework/WU1.10-pipeline-investigation.md`).

**Scale:** Single session. The evidence is distributed across lineage, code
sessions, and configuration files — the session needs access to all of them.

**Preconditions:** None. Can start anytime.

**Status:** proposed

(Independent of the framework evidence chain. Tests infrastructure hypothesis 008
— results determine whether future pipeline work can proceed independently. Can
run in parallel with any other WU. Most valuable after WU1.5 — the retrospective's
provenance chains provide context for the instruction evolution — but does not
hard-block on it.)

---

### WU1.11: Favorites and Supplementary Lens

**Question:** Does what sticks with Brian correlate with specific technique
patterns, and do his instinctive analytical reactions (in comments and reviews)
map to named framework patterns?

**Hypotheses:** 028, 029, 033, 034, 038

**Evidence sources:**
- WU1.9 cross-corpus comparison
- `source_material_references/corpus-favorites-tiers.txt` (Brian's subjective
  tiers: Absolute Favorite, Great, Good, Neutral, Abandoned, Special — completed
  2026-08-29)
- Brian's comments: `P&K comments.md` (25 comments), `pax-chrysalia-comments.md`
  (15 comments + author replies), `Comments.md` (index + inline comments)
- Brian's analytical work: `Filly Fooling review.txt` (~2,500-word essay),
  `Filly Fooling analysis.csv` (chapter-by-chapter three-arc spreadsheet)
- Naive TLTT chapters 1-2 (as evidence of v0 prose instincts)

**Scope:** Overlay favorites tiers onto WU1.9's cross-corpus findings. Three
questions:

1. **Tier clustering:** Do Absolute Favorites cluster around particular mechanism
   profiles, perspective techniques, or obstacle architectures? Is there a craft
   signature that distinguishes "sticks with me" from "forgettable"?

2. **Comment mapping:** Do Brian's chapter-by-chapter comments (particularly the
   dense P&K and Pax Chrysalia comments) reveal instinctive reactions that map to
   named framework patterns? Brian's comments are his analytical voice (register 4
   from hypothesis 021) — pre-framework, unself-conscious reactions. If his
   comments notice the same patterns the v4 Brief formally identifies, that
   supports hypothesis 038 (instinctive mechanism practice).

3. **Special tier analysis:** The Special tier stories (P&K, filly-fooling,
   ribbons-and-lace, injuring-eternity, the-last-train-home, clocktower-society)
   carry specific analytical context that must be read alongside their v4 analyses.
   For each, note where Brian's instinctive analytical voice (in comments or
   reviews) noticed patterns the formal analysis also identifies. P&K is
   distinguished: "the MOST impactful story on my journey, yet I disagree with its
   structural themes so much that it spawned the TLTT project itself" — impact ≠
   agreement.

**Biasing caveats (must be stated in the output):**
- Brian's comments skew toward P&K and Pax Chrysalia. Most favorites have no
  comments. Absence of comment ≠ absence of reaction.
- The Filly Fooling review is Brian theorizing improvements, not pure reader
  response.
- Abandoned tier stories were not finished by Brian. Analyses may cover unread
  content. The favorites lens cannot apply to unread sections.

**What it does NOT do:** Rank stories. Propose which techniques Brian should
adopt. Judge stories' quality relative to each other.

**Output:** A favorites-lens report
(`docs/v3-framework/WU1.11-favorites-lens.md`) with biasing caveats.

**Scale:** Single session reading WU1.9 output + supplementary material.

**Preconditions:** WU1.9 complete. Brian's tier arbitration complete (done
2026-08-29).

**Status:** proposed

(Requires WU1.9 complete. Feeds into WU1.12 and WU1.13.)

---

### WU1.12: Hypothesis Adjudication

**Question:** What does ALL the evidence say about each hypothesis — confirmed,
refuted, refined, or insufficient evidence?

**Hypotheses:** ALL (001-045)

**Evidence sources:** All prior WU outputs. The hypothesis files in
`docs/v3-framework/hypotheses/` (which by this point should have evidence entries
deposited by prior WUs). FEATURE-AUDIT.md (to verify no hypothesis reinvents a
cut feature).

**Scope:** For each hypothesis, synthesize the evidence from all WUs that
produced relevant findings. Assessment categories:

- **Evidenced:** Supporting evidence deposited, no open challenges.
- **Challenged:** Counterevidence exists or evidence is mixed.
- **Refined:** The hypothesis statement should change to accommodate findings
  (the evidence is there but the prediction was not quite right).
- **Insufficient evidence:** The hypothesis was targeted by WUs but the evidence
  gathered is too thin to move it from untested.
- **Untouched:** No WU produced evidence for this hypothesis (should be rare if
  the plan's coverage is working — flag any untouched hypotheses for the next
  consolidation).

Where evidence conflicts across WUs, report the conflict rather than forcing a
resolution. Brian adjudicates conflicts.

Check each hypothesis against FEATURE-AUDIT.md: does any hypothesis's implication
reinvent a feature that was previously cut? If so, note the conflict — the
hypothesis may be evidenced but its implementation blocked by a prior decision.
The prior decision may itself be re-evaluable (hypothesis 005 says no document
carries intrinsic authority), but the conflict should be surfaced, not silently
overridden.

**Relationship to evidence entries in hypothesis files:** Prior WUs should have
deposited evidence entries directly in the hypothesis files as findings emerged.
This WU reads those entries and performs a set-level synthesis — it does not
re-derive the evidence but assesses the overall picture per hypothesis and across
the hypothesis set. If prior WUs did not deposit entries (e.g., a finding was
noted in the WU artifact but not recorded in the hypothesis file), this WU
deposits them.

**What it does NOT do:** Resolve conflicts by choosing one WU's evidence over
another's. Propose framework changes. Baseline any hypothesis (only Brian can
baseline). Propose what to do about challenged hypotheses — surface the evidence
picture for Brian to adjudicate.

**Output:** A hypothesis adjudication document
(`docs/v3-framework/WU1.12-hypothesis-adjudication.md`) — each hypothesis with
assessment, evidence citations, and any unresolved conflicts.

**Scale:** Single session reading all prior WU outputs + the hypothesis files.
Large context needed.

**Preconditions:** WU1.1 (corpus synthesis), WU1.4 (V1 mining), WU1.5
(retrospective), and WU1.9 (cross-corpus comparison) complete. WU1.3 (own
fiction), WU1.8 (planning evolution), and WU1.10 (pipeline) are ideal but not
hard blocking — the adjudication notes which hypotheses lack evidence from
incomplete WUs.

**Status:** proposed

(The first integration WU. Most WUs should be complete before this runs. Feeds
into WU1.13.)

---

### WU1.13: Framework and Architecture Evaluation

**Question:** What should the planner track, at what scope, and in what conceptual
hierarchy — based on all the evidence gathered?

**Hypotheses:** 001, 023, 024, 025, 026, 027, 028, 029, 030, 033, 034, 035, 036,
037, 041, 042, 043, 044

**Evidence sources:** WU1.9 (cross-corpus patterns), WU1.11 (favorites lens),
WU1.12 (hypothesis adjudication), WU1.5 (retrospective), WU1.7 (working plan
assessment), WU1.8 (planning evolution, if available), v2 current track
definitions via MCP (`get_track_definitions`).

**Scope:** The convergence of all evidence into framework-level findings. Seven
evaluation areas:

1. **The three-concern evaluation (hypotheses 023, 024):** Does the evidence
   support three separable concerns (goal, mechanism, technique)? Where do they
   blur? Is the relationship dimensional (like the political axes) or hierarchical
   (like a tree)? Do the three concerns map cleanly to the three axes from the
   earlier corpus work (mechanism × inference stage × rendering mode), or are they
   overlapping-but-distinct?

2. **The scope-level evaluation (hypotheses 041, 042):** Subject-level tracks
   (the reference layer), link-level tracks (the application layer), plot-point-
   level tracks (the scene's own tracks — currently underdeveloped). What should
   each scope track? How do they feed each other? What does iterative subject-to-
   scene work look like in practice? Do the EditorModes' visibility and
   writability rules support or hinder this?

3. **Track coverage evaluation (hypotheses 025, 026, 028, 029, 030, 035, 036):**
   Which existing tracks hold up against the evidence? Which are overfit (Track
   99's FID specification)? Which are missing (plot-point tracks underdeveloped,
   non-thematic goal tracks absent, narrator-character blend unrecognized)? Which
   cognitive modes (ZF/ND/Analogical/LinguisticExecution/Garden) need revision?
   WI-terminal patterns — should the "every link must have T" rule be relaxed?

4. **Goal categories and boundaries (hypotheses 033, 034):** What is the full
   set of goal categories the framework should recognize? Where does the
   plannable/prose-craft boundary fall — which goals belong in the planner and
   which are left to writing time? Does the evidence distinguish the boundary, or
   is it a judgment Brian makes per goal category?

5. **The multi-story dimension (hypothesis 037):** How does the framework support
   per-story perspective strategy? Universal track architecture with variable
   usage density per story — does the evidence confirm this, or do different story
   paradigms need different track structures? What does "variable focalization
   with a plan" look like for different paradigms (Chrysalis's Greek tragedy,
   TLTT's ensemble, Kitty's bildungsroman)?

6. **Note-to-note relationships (hypothesis 043):** Does the evidence require
   structural note-to-note edges for serialized publication trajectory management?
   Or does the current subject-level arc plan suffice? If edges are needed, what
   types (setup→payoff, parallel, contradicts, revelation chain)? Do existing
   codebase patterns (SubjectRelation model, NarrativePropertyValue) serve, or
   is new architecture needed?

7. **The bespokeness tension (hypothesis 044):** Can v3 restore v1's scene-level
   density at v2's quality level? Is the hallmark wall pattern real (each version
   exceeds its instrument capacity at a higher abstraction level), and if so, does
   v3's iterative method avoid or defer the next wall?

**What it does NOT do:** Propose specific tracks. Write display questions. Author
cognitive-mode definitions. Decide which goal categories are plannable. These are
Brian's decisions informed by this evaluation's evidence. The evaluation presents
findings and tradeoffs; Brian decides.

**Output:** A framework evaluation document
(`docs/v3-framework/WU1.13-framework-evaluation.md`) — the evidence base for
v3 framework decisions.

**Scale:** Large session reading all prior integration WU outputs + working plan
data.

**Preconditions:** WU1.9 (cross-corpus comparison), WU1.11 (favorites lens),
WU1.12 (hypothesis adjudication), and WU1.5 (retrospective) complete. WU1.7
(working plan assessment) and WU1.8 (planning evolution) are ideal but not
hard blocking.

**Status:** proposed

(Requires most prior WUs. The penultimate integration — feeds into WU1.14.)

---

### WU1.14: Connection to TLTT

**Question:** What do the framework evaluation findings mean for TLTT and the
broader multi-story project?

**Hypotheses:** 001, 037, 044

**Evidence sources:**
- WU1.13 framework evaluation
- `source_material_references/long-corpus-categories.txt` (TLTT paradigm
  annotations)
- V2 working plan via MCP (stories, subjects, tracks, plot points, links, notes)
- The multi-story architecture: TLTT, TKOW, Countess of Krystal Rock (Chrysalis),
  Lord of the Hedge (Blueblood), Aris, Grover III and Celestia, plus planned
  stories not yet in v2

**Scope:** Connect the framework evaluation to the actual stories Brian is
planning. For each story (or story cluster):

1. **Variable-focalization strategy:** What mix does the evidence suggest works
   for this paradigm? What mechanism profiles do Brian's favorites in the same
   paradigm exhibit?

2. **Scene-level readiness:** Where does the v2 data already contain scene-level
   design? Where is it thin? How does the bespokeness tension (hypothesis 044)
   manifest per story?

3. **Paradigm connections:** Brian identified 5 paradigm connections from the
   categories file (ensemble→TLTT, romance/SoL→TwiJack, dark premise→hopepunk,
   AU→EaW×FiM, explicit/plot→Kitty). What do the corpus patterns in each paradigm
   suggest for the corresponding TLTT story?

4. **Reading-order implications:** The 75%/25% reading-order model — prequels
   stand alone for 75%, then converge with TLTT for the final 25%. Prior beliefs
   are conditional on reading order. How does the framework support designing for
   both reading orders simultaneously?

5. **Trajectory management:** Does the framework evaluation confirm the planner's
   purpose (hypothesis 001) as trajectory management? Are there trajectory types
   the current framework cannot represent — and should they be represented in the
   planner or left to prose?

**What it does NOT do:** Propose plot changes. Write prose. Suggest story content.
Determine what any subject "needs next." Those are Brian's decisions.

**Output:** A connection document
(`docs/v3-framework/WU1.14-connection.md`) — per-story perspective strategy
implications grounded in corpus evidence and Brian's own instincts.

**Scale:** Single session.

**Preconditions:** WU1.13 complete.

**Status:** proposed

(Requires WU1.13 complete. The final WU — connects evidence to the project.)

---

## Dependency graph

```
WU1.1 (corpus synthesis) ──────────────────────────────────────────────┐
  │                                                                     │
  ├──→ WU1.5 (retrospective, needs WU1.1 baseline)                     │
  │                                                                     │
WU1.2 (Keep assessment) ── infrastructure, quick                        │
WU1.7 (working plan assessment) ── independent, quick                   │
                                                                        │
WU1.3 (own fiction) ──────────────────┐                                 │
WU1.4 (V1 archive mining) ───────────┤                                 │
                                      ↓                                 │
                          WU1.9 (cross-corpus comparison) ──────────────┤
                                      │                                 │
                          WU1.11 (favorites + supplementary lens)       │
                                      │                                 │
WU1.10 (pipeline investigation) ──→ WU1.12 (hypothesis adjudication) ←─┤
                                      │                               ←─ WU1.5
                          WU1.8 (planning evolution, if available) ──→   │
                                      │                                 │
                          WU1.13 (framework + arch evaluation) ←────────┘
                                      │                               ←─ WU1.7
                          WU1.14 (connection to TLTT)
```

**Parallel work:** WU1.1, WU1.2, WU1.7 can start immediately and in parallel.
WU1.3 and WU1.4 can start as soon as their respective skills are built. WU1.10
can start anytime. WU1.8 blocks on Brian's preprocessing. WU1.5 blocks on WU1.1.
WU1.9 blocks on WU1.1 + WU1.3 + WU1.4. WU1.11 blocks on WU1.9. WU1.12 blocks
on the major evidence WUs. WU1.13 blocks on WU1.12 + WU1.11 + WU1.9 + WU1.5.
WU1.14 blocks on WU1.13.

## Per-hypothesis WU coverage

Every hypothesis is targeted by at least one WU. The table below shows which WUs
inform each hypothesis, with the primary WU listed first.

| ID | Slug | Primary WU | Other WUs |
|----|------|-----------|-----------|
| 001 | planner-purpose-trajectories | WU1.13 | WU1.14 |
| 002 | epistemic-method-provenance | WU1.5 | WU1.8, WU1.2 |
| 003 | epistemic-vocabulary-for-content | WU1.7 | WU1.12 |
| 004 | working-cadence-sweeps | WU1.7 | WU1.5, WU1.8 |
| 005 | recall-vs-evidence | WU1.5 | (methodological — tested by the process) |
| 006 | four-factor-decomposition | WU1.10 | — |
| 007 | version-labels-as-bookmarks | WU1.10 | WU1.5 |
| 008 | framework-vs-pipeline-separable | WU1.10 | — |
| 009 | v3-tooling-decouples-factors | WU1.10 | — |
| 010 | model-intrinsic-properties | WU1.10 | — |
| 011 | model-comparison-convergence | WU1.10 | — |
| 012 | constitutional-ai-constraint | WU1.10 | — |
| 013 | fable-role | WU1.10 | — |
| 014 | evidence-based-instruction-design | WU1.10 | WU1.5 |
| 015 | acceptance-signals | WU1.10 | — |
| 016 | data-source-unification | WU1.10 | — |
| 017 | desktop-vs-code-split | WU1.10 | — |
| 018 | target-usage-loop | WU1.10 | WU1.8 |
| 019 | ai-context-contradiction | WU1.4 | WU1.5 |
| 020 | voice-separation-prerequisite | WU1.4 | — |
| 021 | five-voice-registers | WU1.4 | — |
| 022 | voice-treatment-protocol | WU1.4 | WU1.5 |
| 023 | three-concern-separation | WU1.1 | WU1.9, WU1.13 |
| 024 | dimensional-vs-hierarchical | WU1.1 | WU1.9, WU1.13 |
| 025 | mechanism-hierarchy-is-complexity | WU1.1 | WU1.9 |
| 026 | three-axis-independence | WU1.1 | WU1.9 |
| 027 | cognitive-mode-principle | WU1.9 | WU1.13 |
| 028 | variable-focalization-master | WU1.1 | WU1.3, WU1.5, WU1.9, WU1.11 |
| 029 | perception-gap-delivery | WU1.1 | WU1.3, WU1.5, WU1.9, WU1.11 |
| 030 | narrator-character-blend | WU1.1 | WU1.3, WU1.9 |
| 031 | dt-knowledge-asymmetry | WU1.1 | WU1.3 |
| 032 | first-person-m4-effects | WU1.1 | WU1.3 |
| 033 | non-thematic-goal-categories | WU1.1 | WU1.9, WU1.11, WU1.13 |
| 034 | prose-craft-boundary | WU1.1 | WU1.9, WU1.11, WU1.13 |
| 035 | embedded-text-category | WU1.1 | WU1.13 |
| 036 | wi-terminal-ratio | WU1.1 | WU1.13 |
| 037 | multi-story-focalization-profiles | WU1.3 | WU1.9, WU1.13, WU1.14 |
| 038 | instinctive-mechanism-practice | WU1.3 | WU1.4, WU1.9, WU1.11 |
| 039 | fim-reading-effect | WU1.3 | WU1.8, WU1.9 |
| 040 | fabula-dialogue-replacement | WU1.1 | WU1.3, WU1.9 |
| 041 | sufficient-stability-iterative | WU1.7 | WU1.5, WU1.13 |
| 042 | editor-modes-evaluation | WU1.7 | WU1.5, WU1.13 |
| 043 | note-design-relationships | WU1.4 | WU1.13 |
| 044 | bespokeness-tension | WU1.4 | WU1.5, WU1.13, WU1.14 |
| 045 | keep-notes-provenance | WU1.2 | — |

## Preconditions summary

| Precondition | Blocks | Owner | Status |
|---|---|---|---|
| Adapted analyze-story skill (self-diagnostic framing, unfinished works) | WU1.3 | Claude Code (buildout work) | Not started |
| V1 archive mining skill (extraction methodology, voice attribution, structured output) | WU1.4 | Claude Code (buildout work) | Not started |
| Preprocessing of raw revision history exports in `Planning_Document_Revision_History/` | WU1.8 | Brian | Not started |
| Access to Google Keep notes directory | WU1.2 | Brian | Available |
| Own fiction source texts ready | WU1.3 | — | Done (CORPUS-STATUS: italics verified 2026-08-31) |
| Favorites tier arbitration complete | WU1.11 | Brian | Done (2026-08-29) |

## Skills to build

Two skills are required before their respective WUs can execute:

**1. Adapted analyze-story skill (for WU1.3):**
The existing `analyze-story` skill runs the v4 Analysis Brief against a Fimfiction
story. WU1.3 applies this to Brian's OWN fiction, which requires adaptations:
self-diagnostic framing (the analysis should be discovery-oriented, not evaluative),
handling unfinished works (NTL, GIYC — analyze what exists), and handling the naive
TLTT chapters as partial text (not a story, but v0 prose evidence). The adaptations
may be a new skill or a mode switch in the existing one.

**2. V1 archive mining skill (for WU1.4):**
No existing skill covers batched extraction from the v1 archive with voice
attribution. This skill governs: consistent extraction methodology (what to read,
what to record), voice attribution via lineage grep (matching note text against
the Gemini conversation corpus), structured output format (note ID, voice
attribution, discovered category, chapter, excerpt), and batching strategy (by
chapter arc, with Aris and Paratext as separate batches). The skill is new.

Building these skills is part of the forward plan's execution, not a precondition
that blocks the plan's existence. Plan-mode sessions for WU1.3 and WU1.4 will
design the skills before their respective WUs run.

## What blocks on Brian

- **Preprocessing revision history exports** (blocks WU1.8): the raw exports in
  `Planning_Document_Revision_History/` need preprocessing before GDocHistory can
  ingest them. The TLTT revision history is already in lineage and can be analyzed
  independently.
- **Keep notes access** (WU1.2): confirm the directory path and contents are as
  expected.
- Everything else is either ready or gated on buildout work (skill creation, WU
  execution).
