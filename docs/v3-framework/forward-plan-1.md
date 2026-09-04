# Forward Plan 1

> **Retired 2026-09-04.** Successor: `forward-plan-2.md`. Reason: methodology revision 1
> (2026-09-03) re-typed every WU — exploratory, verification, synthesis, infrastructure —
> and this plan's single WU type, testing-spec accretion and derived execution sequence no
> longer describe the work. Its unexecuted WUs are retired proposals, not evidence; its
> testing specs were seeded into `spec-pools/` with provenance; WU1.4's instrument,
> `attribution.csv`, calibration records and card rulings stand. Reference only.

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

The framework domain has rich evidence sources ready. Twelve distinct upstream
sources are available, grouped by access pattern:

**Analysis corpus (already analyzed, local files):**
- The 112-story analysis corpus — 7 meta-analysis reports + individual per-story
  analyses, in `source_material_references/Reading Archive Analyses/`. These are
  the product of the v4 analysis pipeline (Aug 2026) — structured findings ready
  for synthesis

**Raw material (not yet analyzed, local files):**
- Brian's own fiction — 6 published stories + naive TLTT chapters, as markdown in
  `source_material_references/open_stories_md/` and `source_material_references/`.
  Raw text awaiting analysis under an adapted v4 Brief (WU1.3)
- Supplementary material — comments (P&K, Pax Chrysalia, others), reviews (Filly
  Fooling essay + spreadsheet), favorites tiers, TLTT paradigm annotations, in
  `source_material_references/`. Brian's unprocessed analytical voice
- Design conversations — framework-origin transcripts in
  `docs/design-conversations/` (conv 020 and 039 are NOT in the MCP database;
  the rest are accessible via both MCP and local files)

**MCP corpora (three independent corpora in the .storyplan, never joined):**
- V2 working plan — the current plan (`*_plan` tools: notes, subjects, chapters,
  plot points, links, track definitions, narrative properties)
- V1 archive — the older capture-era dataset (`*_archive` tools: notes, subjects,
  chapters, plot points, links). Different organizing principles, no id
  correspondence with v2
- Conversations — imported AI chat transcripts (`search_conversations`,
  `get_blocks`, `list_conversations`). Brian's per-block read states and his own
  navigation notes (Summary field). Arc summaries are frozen text from a retired
  import pass

**MCP sidecar corpora (separate databases, queried via MCP):**
- Lineage corpus (`lineage.db`) — four ingested layers: Google Doc revision
  history (53 diffs + 54 snapshots), Gemini web conversations, AI Studio chats,
  NotebookLM captures. `search_lineage` / `get_lineage` / `list_lineage`
- Source texts (`sources.db`) — published material citations point at: FiM
  episode transcripts, fanfic chapters, EaW flavor text. `search_source_texts` /
  `get_source_text`

**Non-MCP corpora (queried directly via sqlite3 or file access):**
- Code sessions (`codesessions.db`) — sealed Claude Code transcript archive,
  engineering-process provenance. Deliberately outside MCP; queried via sqlite3
  (recipes in the `code-sessions` skill)
- Google Keep notes (`C:/Users/Brian/Google Drive Analysis`) — available for
  assessment, no ingest path exists yet (hypothesis 045)
- Planning doc revision histories — TLTT already in lineage; KU/NTL, GIYC,
  Falldale as raw exports in `Planning_Document_Revision_History/`, preprocessing
  needed before GDocHistory ingest
- V1 database snapshots — dated `.db` files in Google Drive
  (`TheLionessOfTallTale[date].db`). Time-series of the v1 planner's state,
  showing how the data evolved during v1's lifetime. Preprocessing needed to
  make them queryable (likely opening each as read-only SQLite and diffing
  against the final v1 archive)

Framework hypotheses are testable now against most of these sources. Pipeline
hypotheses draw primarily from the lineage corpus, code sessions, and
configuration history.

The pipeline domain has thinner evidence and more speculative hypotheses. Its
evidence sources — lineage, code sessions, configuration files — exist but have
not been systematically mined for pipeline-specific questions. Some pipeline
hypotheses (010-013, model-intrinsic properties) may not be testable with current
evidence at all, requiring controlled model comparisons that are expensive and
outside the framework buildout's primary mission. The pipeline WU is designed to
extract what evidence exists and identify which hypotheses need a different kind
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

**5. Enrichment flow determines execution order.** Each WU's post-review adds
testing specs to downstream WUs — proven by WU1.1, which added ~35 testing
specs across 7 downstream WUs. Discovery WUs (open-ended mining, corpus
synthesis) produce findings that add questions to downstream survey, comparison,
and assessment WUs. This information flow is one-directional and determines
execution order: WU1.4 (V1 mining, discovery) → WU1.3 (own fiction, discovery)
→ WU1.7 (working plan survey, enriched by both discoveries) → WU1.5
(retrospective) → WU1.9 (cross-corpus comparison) → convergence WUs.
Practical considerations (which skill is ready, which WU is quicker) are not
ordering criteria. The convergence pattern remains: evidence → comparison →
adjudication → evaluation → connection — but the evidence WUs are sequential,
not parallel, so each one's findings enrich the next.

### Hypothesis coverage

Every hypothesis is targeted by at least one WU. The mapping:

| Tier | Hypotheses | Primary WUs |
|------|-----------|-------------|
| A (001-005) | Purpose, epistemology | WU1.5 (retrospective), WU1.7 (working plan), WU1.12 (framework eval), WU1.13 (connection) |
| B (006-009) | Pipeline decomposition | WU1.10 (pipeline) |
| C (010-013) | Model properties | WU1.10 (pipeline) |
| D (014-018) | Instructions, environment | WU1.5 (retrospective), WU1.8 (planning evolution), WU1.10 (pipeline) |
| E (019-022) | Voice, interaction | WU1.4 (V1 mining), WU1.5 (retrospective) |
| F (023-027) | Framework architecture | WU1.1 (corpus), WU1.3 (own fiction, 023/024), WU1.9 (cross-corpus), WU1.12 (framework eval) |
| G (028-032) | Perspective, perception | WU1.1 (corpus), WU1.3 (own fiction), WU1.4 (V1 mining, 028/029/031 from scope reconciliation), WU1.5 (retrospective), WU1.9 (cross-corpus) |
| H (033-037) | Goals, scope, boundaries | WU1.1 (corpus), WU1.3 (own fiction, +033-036 from scope reconciliation), WU1.4 (V1 mining, 035 from scope reconciliation), WU1.9 (cross-corpus), WU1.12 (framework eval) |
| I (038-040) | Brian's practice | WU1.3 (own fiction), WU1.4 (V1 mining, +040 from scope reconciliation), WU1.9 (cross-corpus) |
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

**Status:** complete

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

**Status:** complete

---

### WU1.3: Own Fiction Analysis

**Question:** What does Brian instinctively do when he writes — what mechanism
profiles, perspective techniques, interiority rendering, and structural patterns
appear in his fiction?

**Hypotheses:** 023, 024, 028, 029, 030, 031, 032, 033, 034, 035, 036, 037, 038, 039, 040

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
confirm expected ones. Hypothesis 030 (narrator-character blend, now reframed as
FID with varying narrator presence): the 112-story corpus shows a spectrum from
full character-voice FID to narrator-literary-register-visible passages, most
prominent with vocabulary-limited characters. Where does Brian's writing fall on
this spectrum? Brian is particularly interested in this question.

Hypothesis 039 (FiM reading effect) is directly testable here: compare the pre-FiM
Pokemon stories (THLB 2015-2017, Wish 2020, TEatS 2020-2021, NTL 2021-2023)
against the post-FiM texts (GIYC 2024-2025, Falldale Nov 2025, naive TLTT June
2025) for changes in FID usage, variable-focalization sophistication, and
interiority rendering. THLB's alternating first-person already shows perspective
restriction instincts, so the prediction is not that perspective awareness appeared
from nothing but that the specific toolkit shifted.

Hypotheses 033/034 (comedy): the 112-story corpus shows comedy is almost never a
primary genre but is a major structural force as a delivery register in 85%+ of
stories. Does Brian's fiction use comedic moments at structural positions (after
intensity, before reveals, at character-introduction beats)? If so, comedy-as-
moment placement is something he's already designing, and the question is whether
the planner should support it.

DT prevalence: the corpus shows DT is the primary interiority technique in the
majority of stories and does more perception-gap-adjacent work than FID (DT-based
knowledge asymmetry is the most frequently documented unnamed technique across all
categories). The own-fiction analysis should specifically track Brian's DT/FID
ratio and what perception-gap work each does — if Brian's instinctive toolkit is
DT-heavy (like most of the corpus), the planner should support DT-based gap design,
not just FID-based. This is the flip side of hypothesis 028: variable focalization
means designing when to use DT (observation stance) vs FID (inhabitation stance)
vs other techniques, and DT may be where most of the designed work lives.

Hypothesis 038 (instinctive mechanism practice): WU1.1 identified seven
perception-gap delivery mechanisms beyond FID (DT-based knowledge asymmetry,
first-person unreliability, dual-POV structural dramatic irony, strategic opacity,
narrated denial, the adversarial inner voice, the Mother voice). Brian recognizes
six of seven from his own practice (all except the Mother voice) — look for these
specific techniques in the fiction as a grounding checklist.

Hypothesis 037 (multi-story focalization): Brian's recall is that he finds
unintegrated parallel character arcs annoying (negative examples: Filly Fooling,
About Last Night) and his instinctive pattern is: split briefly, converge with
payoff, don't re-split (TLTT's Agency/Tempest/Crash split, NTL's similar
pattern). Does his fiction confirm this pattern? The multi-story architecture
may be a deliberate response — pre-TLTT material in its own stories rather
than parallel subplots within TLTT.

Bond obstacle architecture: the corpus shows characterological obstacles as the
dominant primary barrier in bonded stories (13/15 romance/SoL, all ensemble, 10/11
explicit/plot). Brian's recall: his stories will differ — they aren't pure romance,
and the bond is structurally intertwined with a parallel plot, so structural
obstacles should be more prominent. Does his fiction confirm structural obstacles
as primary or co-primary? Lineage discusses this intertwining — WU1.5 should check.
(Per-story obstacle-type breakdown from corpus analyses moved to WU1.9 —
scope reconciliation 2026-09-01.)

Behavioral proxy: the corpus shows it as a near-universal significant secondary
technique across all categories — wing movements, physical displacement behaviors,
involuntary responses. Does Brian's fiction use behavioral proxy, and if so, what
are his specific recurring proxies? This informs how the planner should represent
the precursor design work (the planned behavioral setup) such that it invokes what
Brian actually wrote — the planner's Demonstration and Character Actions tracks
should connect to Brian's instinctive proxy vocabulary.

Candidate design targets from WU1.1 (not yet hypotheses — test whether Brian
designs these): (a) Information architecture — does Brian's fiction show designed
macro-level management of what the reader knows when, beyond individual M2
instances? (b) Reader stance trajectory — does Brian design shifts in the reader's
relationship to the characters (observation → inhabitation → solidarity → irony)
as a cross-scene arc? (c) Structural correspondence — does Brian use echo
architecture, structural rhyme, or designed mirroring between scenes?

Dual-POV structural dramatic irony: the corpus identifies this as a new gap
(4.2b, 6 analyses) — reader holds knowledge from one focalizer's chapters while
reading the other's, producing sustained asymmetric awareness that is neither
Latent nor Perception Gap nor Development. Brian's recall: this may be the
*foundational* mechanism that got him to want to write in the first place. THLB
and many other stories are likely built on it. Question for the own-fiction
analysis: Is dual-POV structural dramatic irony Brian's primary technique?
(Corpus comparison (b) moved to WU1.9; framework surfacing (c) moved to
WU1.13 — scope reconciliation 2026-09-01.) Also check the v1 archive (WU1.4) for whether
TLTT's multi-focalizer scene graph shows designed cross-focalizer knowledge
asymmetry.

Demonstration over declaration: the corpus confirms bonds are demonstrated through
behavioral evidence and accumulated gesture rather than verbal declaration — "I
love you" moments are payoffs earned by chapters of behavioral demonstration.
Does Brian's fiction follow this instinct? Check own fiction (WU1.3) for whether
bonds are demonstrated behaviorally or declared verbally. Check v1 archive (WU1.4)
for whether the Demonstration track notes and link notes design behavioral bond
evidence rather than confession/declaration scenes. This connects to the
behavioral proxy finding — if Brian's instinctive bond design is behavioral, the
planner's Demonstration track is doing the right work.

Asymmetric interiority access: the corpus shows it as the norm in bonded stories
— one partner receives deeper narrative access, creating dramatic tension about
the less-accessed partner's feelings. Does Brian's fiction use this instinctively?
Check WU1.3 (own fiction — which characters in Brian's dual-focalizer stories get
more interiority?), WU1.4 (v1 archive — do the 1,125 links and plot point notes
show asymmetric access design across character pairings?), and v2 working plan
(WU1.7 — do the Character-Reader Perception Gap or Reader Opinion Plan notes
show designed asymmetry between bonded partners?).

Bond obstacle architecture — Brian's prediction: his fiction will show the opposite
of the corpus's characterological dominance. His bonds exist alongside structural/
external conflicts rather than requiring character change before the relationship
is possible. Does his written fiction confirm this?
(Unwritten story plans as supplementary input moved to WU1.8 — planning-
process question, not fiction-analysis question. Source: Google Drive
"Miscellaneous Story Stuff" folder. Scope reconciliation 2026-09-01.)

Counterargument architecture: Does Brian's written fiction contain genuine
counterarguments? The hopepunk thesis specifically predicts engagement with
genuine opposition. (v2 working plan MCP query for designed counterarguments
moved to WU1.7; corpus counterargument density comparison moved to WU1.9 —
scope reconciliation 2026-09-01.)

Perspective mode in Brian's ensemble stories: the corpus shows 5/6 ensemble stories
use rotated limited third rather than omniscient (only The Best Night Ever Repeat
uses omniscient). NotebookLM labelled TEatS and NTL as omniscient — test this
claim. Are they actually omniscient, or rotated limited? If rotated limited, Brian's
instinct aligns with the corpus pattern. If genuinely omniscient, that's a
distinctive choice the framework should account for.

Shame-about-desire and sex-as-thematic-testing-ground: does Brian's written
fiction show these patterns? The per-story analyses will discover thematic
families if present. (Specific TLTT/Kitty evidence sources moved to their
respective WUs: v1 archive "Passion" chapter to WU1.4, v2 working plan Kitty
subjects to WU1.7, conversations/lineage to WU1.5 — scope reconciliation
2026-09-01.)

Canon virtues as psychological traps: does Brian's fiction show canon virtues
as traps? Only the MLP texts (GIYC, naive TLTT) and potentially Falldale (EaW)
can show this in the fandom-specific sense; the Pokemon stories can show the
general "virtue-as-trap" pattern. (Corpus comparison (d) moved to WU1.9;
v1 archive (b) already tagged for WU1.4; hopepunk lineage (c) already tagged
for WU1.5 — scope reconciliation 2026-09-01.)

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

**Status:** complete

**Completion (2026-09-01):** Seven analyses and the synthesis produced; brief
additions preserved at `docs/analysis-briefs/v4-self-diagnostic-additions.txt`.
Post-WU review found the initial deposits biased (13 supporting, 0
challenging) and four per-story FID misclassifications on source check; the
Deposit protocol was added to the v3-buildout skill and the deposits redone
under it. Final tag counts: 16 targets (15 + new 046), 14 entries — 11
supporting, 3 challenging (038, 039, 040 → `challenged`), 2 not deposited
(028, 035). Hypothesis 046 (dt-two-classes) created. Counterargument finding
corrected (5/7, not 3/7). Corrections and audit trail: synthesis, Correction
section. Open for Brian: statement sweep on 038/039/040; 046 borderline
adjudication; the brief-bias hypothesis offer.

---

### WU1.4: V1 Archive Mining

**Question:** What scene-level instincts did Brian capture in v1 before having
formal vocabulary, and whose voice is doing the capturing?

**Hypotheses:** 019, 020, 021, 022, 028, 029, 031, 035, 038, 040, 043, 044
(029, 031, 035 added by scope reconciliation 2026-09-02 — the testing specs
accumulated below from the WU1.1 and WU1.3 reviews target 029's named
cross-scene design targets, 031's focalization-plan prediction, and 035's
"structural scaffolding" open question, which 035's own record already
delegates to this WU. 028 and 040 added the same day at Brian's direction:
the Pinkie external-only-access and TwiJack perspective-breach checks are
focalization-variation design questions for 028; the v1 notes are where
040's four named replacement techniques — behavioral evidence, designed
incomplete understanding, revelation architecture, designed mistakes — would
appear as designed rather than as written, which WU1.3's `[challenging]`
entry could only test in prose.)

**Evidence sources:** All 450 plot points, 1,125 links, and relevant notes in the
v1 archive, accessible via MCP (`get_plot_points_archive`, `get_links_archive`,
`get_notes_archive`). The Gemini conversation corpus in lineage
(`search_lineage source:"gemini"`) for voice separation. If preprocessed, the
dated v1 database snapshots in Google Drive (`TheLionessOfTallTale[date].db`)
could show WHEN specific notes appeared — enabling temporal voice attribution
(notes that appeared after a dated Gemini conversation are candidates for
copy-paste) — but this is an enhancement, not a precondition.

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
should EMERGE from the data, not be imposed. For hypothesis 038: WU1.1 identified
seven named perception-gap delivery mechanisms beyond FID — look for traces of
these specific techniques (especially DT-based knowledge asymmetry, strategic
opacity, and narrated denial) in Brian-voice notes. Also look for traces of
three candidate design targets from WU1.1: (a) designed information architecture
(macro-level management of what the reader learns when — beyond individual M2
prior-plants), (b) reader stance trajectory (designed shifts between observation,
inhabitation, irony across scenes), (c) structural correspondence (echo
architecture, designed mirroring between scenes). If these appear in the v1 scene
graph, they're design targets Brian was already working with. Also check for
canon-virtue-as-psychological-trap patterns in the early TLTT chapters (3-8,
Element-named titles) — how does the v1 scene graph handle the tension between
hopepunk subversion and approaching the trap from a different angle? Also check
the "Passion" chapter notes and Aquileian subject notes for shame-about-desire
and sex-as-thematic-testing-ground patterns — does the v1 archive show Brian
designing these as thematic delivery rather than decoration? Check for
dual-POV structural dramatic irony — does the v1 scene graph show designed
cross-focalizer knowledge asymmetry (information placed in one character's
scenes that the reader carries into another character's scenes)? Brian
believes this is his foundational mechanism and one of the reasons he built v1.
More broadly: does the v1 archive show designed dramatic irony (reader ahead
of characters) as a structural pattern? The corpus shows it as the dominant
information architecture in the majority of stories. The current v2 tracks
capture specific slices (Reader Prior Belief Update captures what the reader
learns; Track 99 captures one type of character-reader gap) but not the general
cross-focalizer knowledge management: what the reader knows from focalizer A
that focalizer B doesn't know. Embedded texts and dream sequences: the corpus documents embedded texts as
creating a double interpretive layer and dream sequences as psychological
evidence the character doesn't control. Brian's recall on two fronts:

(a) Embedded texts: TLTT has Friendship Letters that AJ and Twilight send to
each other, instinctively planned as the transition points between subplots for
three chapters. This is embedded text serving as structural architecture — not
just characterization through voice (the corpus pattern) but narrative
scaffolding that connects parallel threads. Heavy analysis needed on the v1
archive's Friendship Letter notes: how are they designed? Do they function as
information architecture (what the reader learns through the letter vs what the
characters know)? Do they serve the dual-POV dramatic irony pattern (reader
holds knowledge from one character's letter while reading the other's subplot)?
The instinctive choice to use letters as subplot transition points may be a
distinctive technique not documented in the corpus.

(b) Dream sequences: TLTT uses dreams in two ways that may mix and extend what
the corpus documents. First, the "AJ the collaborator" thread (in v1 archive,
not yet migrated to v2) — dreams as psychological evidence, closest to the
corpus's treatment. Second, the dreamscape aid network — a materialist magical
system where dreams are functional communication/collaboration infrastructure,
not just psychological revelation. This is probably unique: the corpus treats
dreams as interiority evidence (the character's subconscious), but TLTT's
dreamscape is BOTH interiority evidence AND literal in-world infrastructure.
The two uses may produce different reader effects that the framework needs to
distinguish. Check v1 archive for the "AJ the collaborator" dream thread notes
and the dreamscape aid network design. Good target for comparative analysis
against the corpus's dream sequence patterns (Third Time's a Charm, Inner
Strength, Salvation, Perfect on Paper, The Sky is Falling, Ribbons and Lace,
Controlling Your Desires).

Deliberate perspective breach: the corpus shows NO story using deliberate
sustained perspective breach as a technique — all breaches are accidental,
clustering at emotional intensity and expository needs. NLM suggested a planned
perspective switch for TwiJack scenes in TLTT — a deliberate "breach" of the
story's otherwise disciplined perspective. This would be unexploited technique
space with no corpus evidence for or against. Check v1 archive for whether
the TwiJack perspective switch was designed. Check lineage (WU1.5) for NLM's
reasoning behind the suggestion. The framework question for WU1.13: should
the planner support marking designed perspective breaches as deliberate
technique choices distinct from the story's default perspective discipline?

AU ambient Latent field: the corpus shows AU stories create a persistent
background comparison between canon and AU — every character encounter generates
a clash. This is TLTT's operating mode as an EaW AU. The synthesis says "the
framework has no term for it," but Brian questions whether the existing framework
actually does represent it — possibly through the Source Material References
tracks (Canon) or the Reader Prior Belief Update tracks. Check v1 archive and
v2 working plan for whether existing tracks are already doing this work. If
so, the gap may be vocabulary (the framework does it but doesn't name it)
rather than architecture (the framework can't do it).

Anti-development / structurally significant stasis: the corpus documents this in
Carrot Top Season (Applejack refuses to change despite mounting evidence). The
framework captures behavioral proxies of near-breakdown but has no category for
"development that does not occur as a structurally significant absence." This
may be central to Chrysalis's Greek tragedy — if her arc IS the refusal to
change, the designed absence of development is the tragic structure. Check v1
archive for whether Chrysalis's scene graph shows designed stasis (the reader
watches her refuse the growth that would save her) and whether the planner can
represent "this character will NOT change, and that refusal is the designed
reader experience."

Content-rating elision as technique: the corpus documents that what a story
refuses to show does real work — "the kisses land harder for being the ceiling of
physical expression." Brian's recall: his design in the v1 archive subverts this
— TLTT and the Kitty of Westkeep deliberately DO show what these stories elide.
Check the v1 archive for how this subversion is designed. If Brian's architecture
treats explicit content as thematic delivery rather than as something to elide
for restraint-based power, that's a different relationship to content-rating
discipline than the corpus's dominant pattern — and aligns with the explicit/plot
category's finding that "the explicitness is the instrument, not the goal."

Pinkie Pie as consistent non-focalizer: the corpus shows Pinkie denied narrative
interiority across multiple stories and categories, making her operate at M3 for
the reader (always inferred, never told) — a shared authorial instinct that her
unpredictability is preserved by keeping the reader outside her consciousness.
Brian's recall: he follows the same pattern instinctively, not knowing how to
write her. Check the v1 archive for whether Pinkie's plot point notes and link
notes show designed external-only access — is she consistently designed to be
inferred from outside rather than shown from inside?

Check for designed counterarguments in the v1 archive's scene graph — do plot
point notes or link notes articulate opposing positions that the story must
defeat? This measures Brian's planning instincts for counterargument design
before he had framework vocabulary for it. The corpus shows counterargument
depth correlates with mechanism ceiling and length — does Brian's v1 planning
show the same pattern?

Specifically search v1's free-form text fields
for remnants of dramatic-irony-enabling design — especially syuzhet-adjacent
notes (reader experience design, revelation timing, information sequencing)
that have NOT been migrated to v2. These unmigrated notes may be the clearest
evidence of what Brian was designing in v1 that v2's track architecture lost.

Conversation-as-resolution pattern: the corpus flags extended honest conversations
resolving emotional problems as a potential narrative shortcut (4.2b, 4.2c — "bypasses
more protracted emotional processing"). Audit the v1 archive for this — there are
many scenes where Twilight and Applejack talk extensively. Caveat: the shortcut
critique applies specifically to characterological obstacles (character changes through
one conversation rather than through accumulated behavioral evidence). TLTT's main
conflict is structural/political, not characterological, so the TwiJack conversations
may serve a different function — processing the non-characterological conflict through
their relationship rather than resolving a characterological barrier. Understanding
the characterological dimension of the TwiJack arc versus TLTT's structural main
conflict is the key distinction to make.

Three checks delegated here from WU1.3's spec (the ordering audit's 1.3 → 1.4
STRONG edge; brought onto this card by scope reconciliation 2026-09-02 so the
read-once pass asks them). WU1.3's findings on each are the comparand:

(a) Behavioral-proxy vocabulary: WU1.3 found Brian's recurring proxies in the
prose (synthesis, "Behavioral Proxy Vocabulary"). Do v1 plot point and link
notes plan behavioral setups in the same vocabulary — i.e. does the v1
Demonstration/Character Actions material name the proxies he later wrote?

(b) Demonstration over declaration: do v1 Demonstration-track notes and link
notes design behavioral bond evidence rather than confession/declaration scenes?
WU1.3 found bonds demonstrated behaviorally in the fiction (synthesis,
"Demonstration Over Declaration").

(c) Asymmetric interiority access: do the 1,125 links and plot point notes show
designed asymmetric access across character pairings — one bonded partner
planned for more interiority than the other? WU1.3 found the pattern in the
fiction (synthesis, "Asymmetric Interiority Access"); this asks whether v1
designed it or the prose produced it.

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

**Scale:** Re-measured 2026-09-02 (scope reconciliation) against the archive
file: the scene-level material the spec mandates in full — plot point notes,
link notes, chapter notes — is ~1.06M characters (~265K tokens); the subject
notes, read selectively, are another ~1.0M characters (~250K tokens). The
mandatory read fits one context window with headroom; the whole archive
(~515K tokens) sits just under the skill's ~600K subagent threshold. The
original characterization ("multiple subagents batched by chapter arc, plus
Aris and Paratext") is therefore a choice for plan mode, not a necessity —
and the Deposit protocol requires the main session to write deposits either
way. What has grown since plan creation is the question list, not the source:
the accumulated specs above (from the WU1.1 and WU1.3 reviews) are roughly
twenty named checks on top of the open-ended read. Not quick.

**Preconditions:** A new skill for V1 archive extraction — consistent methodology,
voice attribution via lineage grep, structured output format. The skill must be
built before running this WU. The v1 archive is accessible now via MCP; no other
blockers.

**Status:** in-progress

**Instrument built and validated (2026-09-02).** Voice attribution is mechanical:
`StoryPlanner.Core/VoiceMatch.cs` (shingle index, earliest-dated source wins, token
mask, structural labels) and the read-only `tools/StoryPlanner.VoiceAttribution`
(CSV + calibration sheets + reading render + `scan.html` census view). Evidence set:
`WU1.4-v1-scene-instincts/attribution.csv`, one row per non-Paratext note. Rulings
taken during validation, all Brian's: two roles (`brian` / `model`), no style
heuristics, lineage as complete ground truth; the pre-AI Google Doc is not a voice
source; the Note Organizer chats (`aistudio:22–25`) are excluded (verified
pass-through; the 32 Sorter-only notes are Brian's); the `print(open(...))` plan
dump is a plan snapshot, not a voice; labels are structural (verbatim /
edited-paste / framed-paste / fragment / phrase / none / short, R=8, G=6,
verbatim ≥0.9 both coverages) with pastes and lifts counted separately and phrase
never counted; PlanFirst (note in a dated v1 snapshot before the matching
response) and Echo (the model's "in your notes…" lead-in) both rule a match
Brian's. Calibration: `calibration-sample-1.md` (120 verdicts, kept as record)
and Brian's census scan of the page. Paratext excluded from the CSV.

**Reading redesigned as three sessions (2026-09-02).** The scene corpus is read
twice, blind — a single long read and one subagent per arc, identical locus-level
record format — then adjudicated by a third session that bins disagreements
(cross-arc / missed by the long read / over-read by the fresh reader) and drills
only the last two. The subject checks (six named subjects + 18 triage-labelled)
run as a separate session on the adjudicated inventory and write the deposits.
This is also a methodological experiment on the two historical failure modes
(lossy consolidation; lossy long-input reading) and its disagreement counts are
evidence for the pipeline hypotheses. Per-session method: the `v1-archive-mining`
skill; session sequence and outputs: `WU1.4-execution-plan.md`.
Output field for this WU therefore reads: `attribution.csv` + `read-manifest.md`
+ two dated record sets + an adjudicated inventory + one synthesis; no per-arc
files.

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
- Google Keep notes in lineage (`keep:` ids) — pre-AI timestamps for framework
  concepts (TLTT thesis Jul 2025, Applejack keystone Aug 2025, StoryPlanner
  conception Dec 6 2025, craft guardrails Mar–May 2026, purpose statement Apr
  2026). Establishes when ideas formed before AI engagement — critical for
  provenance chains 2, 3, and 11
- WU1.1 findings (corpus evidence baseline to assess against)
- The provenance claims from VERSION-HISTORY-DRAFT1.md (the unverified provenance
  table preserved in consolidation-1-plan.md)
- If preprocessed, the dated v1 database snapshots in Google Drive — time-series
  of how the plan evolved under the full-plan-paste paradigm, showing when
  subjects, notes, and the scene graph grew and how their growth correlates with
  dated Gemini conversations

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

4. **The M2 provenance chain**: Where did the prior-belief-clash-revision
   vocabulary come from? Trace through lineage how M2 was formulated, what
   prescriptive reasoning shaped its design, and compare that reasoning against
   both the 112-story corpus evidence (WU1.1 — M2 as sharpest diagnostic) and
   Brian's own fiction (WU1.3). Is the M2 vocabulary accurate to how prior-
   belief work actually operates in the stories Brian reads and writes, or is it
   overfit to the prescriptive framing that produced it?

5. **The Spark provenance chain**: The Spark (why two characters have something
   special) maps cleanly across the corpus — every bond analysis traces it to a
   specific moment or quality. Like M2, this is a v2 concept that turned out to
   match well. Trace where The Spark came from in the framework's design history
   and why it landed accurately. Compare with the FID fixation chain and the M2
   chain — three v2 concepts, two that matched the corpus well (M2, The Spark)
   and one that overfit (FID prescription). What went differently? Understanding
   why some prescriptive reasoning produced accurate vocabulary while other
   reasoning overfit is a methodological finding about how to build framework
   concepts.

6. **Track 99's FID prescription vs DT prevalence** (hypotheses 028, 029, 031):
   Track 99's usage directive prescribes FID for perception gap delivery. The
   corpus shows DT is the dominant interiority technique and DT-based knowledge
   asymmetry is the most common perception-gap-adjacent technique. Was DT-based
   gap work considered when Track 99 was designed? Was it rejected, or not
   considered? The provenance of the FID prescription should show whether the
   narrowing was deliberate (DT was considered insufficient) or incidental (the
   FID fixation chain bypassed DT entirely).

6. **The perception gap concept itself** (hypotheses 028, 029): Where did
   "perception gap" come from as the central design target? Trace through conv 21
   and its antecedents. Three questions: (a) What was perception gap designed to
   achieve — what reader effect was it naming? (b) How did it come to occupy the
   pinnacle position in the v2 framework (top of P→WI→T, its own track, FID
   prescribed as delivery)? (c) What are its peers — what other designed reader
   effects sit at the same level of architectural importance? The corpus shows
   prior-belief management (M2), reader investment accumulation, revelation
   sequencing, and reader opinion trajectory as cross-scene design targets that
   can't be done in prose alone. Were these considered as peers to perception gap
   when the framework was designed, or was perception gap elevated above them?

7. **The "every link must have T" rule** (hypothesis 036): conv 47 block 1520.
   Does the corpus evidence support relaxing it?

8. **The four gap types** (hypothesis 029): ironic/tragic/closing/aligned from
   conv 21 block 613. One AI's taxonomy, held as hypothesis. Does the corpus show
   these four types, different types, or a more complex picture?

9. **The v2 prescribed workflow** (hypotheses 041, 042): Stage 0→1→2→3, five
   EditorModes. What stalled and why? Was the sequential model the problem, or the
   specific mode designs, or the narrowness of the experience-design vocabulary?
   Lineage evidence from the v2 working period should show what was attempted and
   where it broke down.

10. **The AI context contradiction** (hypothesis 019): trace the read-generate-
    paste-reread feedback loop through lineage evidence. Reports W05-W07 document
    the ~940K-char paste scale. The Gemini conversations should show the cycle: the
    plan enters the prompt, the AI produces output, Brian pastes output back, the
    next session's plan paste contains the AI's prior output.

11. **The v1 founding motivation** (hypothesis 034): Brian's recall is that
    cross-scene architecture is why he built v1 — the planner exists because
    cross-scene design can't be done in prose alone. Verify against lineage: what
    do the earliest planning conversations and Google Doc revisions say about why
    the planner was created? If grounded, this is the strongest evidence for 034's
    prose-craft boundary falling on cross-scene architecture.

12. **The hopepunk thesis provenance**: Trace where the hopepunk thesis came from
    and how it relates to the canon-virtue-as-psychological-trap pattern found in
    the dark premise corpus (Salvation, Dash's New Mom). Brian's recall: P&K's
    grimdark "power is the only thing that matters" IS the canon-virtue-as-trap
    thesis, and TLTT subverts it. But TLTT chapters 3-8 also approach the trap
    from a different angle. How do these relate — is it subversion, reframing,
    both? Trace through lineage.

13. **The unverified provenance table** from VERSION-HISTORY-DRAFT1.md: nine claims
   about when specific concepts first appeared (fabula/syuzhet at aistudio:6,
   Architect/Gardener at NLM nlm:3, etc.). Each row is checkable against lineage.
   Deposit findings as evidence into the relevant hypothesis records.

**What it does NOT do:** Propose framework replacements (that is WU1.12). Judge
whether prior work was "wrong" — it was best effort with the data available.

**Output:** A retrospective document
(`docs/v3-framework/WU1.5-retrospective.md`) with per-concept provenance chains
and assessments against corpus evidence.

**Scale:** Single session. All sources accessible via MCP and lineage.

**Preconditions:** Keep sidecar ingest into lineage.db complete (needs pre-AI
timestamps for provenance chains — see implementation-candidates.md).

**Status:** proposed

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

5. **Dramatic irony coverage in v2 tracks**: The corpus shows dramatic irony
   (reader ahead of characters) as the dominant information architecture. How does
   the current v2 track setup handle this? Reader Prior Belief Update captures
   what the reader learns. Track 99 captures one specific character-reader gap.
   But general cross-focalizer knowledge management (what the reader knows from
   focalizer A that focalizer B doesn't know) has no dedicated track. Survey
   whether any existing notes in the working plan are doing this work informally
   — notes on Reader Prior Belief Update or Reader Opinion that track cross-
   focalizer knowledge asymmetry despite the track not being designed for it.

Testing specs moved from WU1.3 (scope reconciliation 2026-09-01):
- Do Brian's v2 plans (theme propositions, subject notes, plot points) show
  designed counterarguments? Check via MCP whether Theme Plan and Scene Theme
  Evidence notes articulate opposing positions.
- Shame-about-desire / sex-as-thematic-testing-ground: check The Kitty of
  Westkeep's subjects, notes, and story plan via MCP for these patterns.

**What it does NOT do:** Evaluate the framework (that is WU1.12). Mine note
content for patterns (that is v2-scope work, not v1 mining). Propose changes to
NoteState or tracks.

**Output:** A brief assessment document
(`docs/v3-framework/WU1.7-working-plan-assessment.md`) with data survey findings.

**Scale:** Single session. Scope grows with upstream discoveries — plan-mode
reconciliation (v3-buildout skill, step 2) will re-assess scale at execution
time.

**Preconditions:** None.

**Status:** proposed

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

Testing specs moved from WU1.3 (scope reconciliation 2026-09-01):
- Unwritten story plans from Google Drive "Miscellaneous Story Stuff" folder
  (ID: `0BzQC1JZ2OeGMOTNCNzdXdnBBcVk`) as supplementary input — design-intent
  evidence. What obstacle architecture does Brian design when planning? What
  became a story vs what didn't, and does that correlate with obstacle type?
  Includes the Ninetales/Mightyena story (conv 36) and ~15 other unwritten
  plans spanning 2016-2024.

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

Embedded texts and dream sequences: compare Brian's use of Friendship Letters as
subplot transition architecture AND his dual dream usage (psychological interiority
+ materialist magical dreamscape infrastructure) against the corpus. For dreams
specifically: the corpus uses them as psychological evidence the character doesn't
control. TLTT's dreamscape aid network adds a second function — literal in-world
infrastructure — that may produce a reader effect the corpus doesn't document
(the reader processes the dream content simultaneously as psychological evidence
AND as plot-functional communication). Does any corpus story mix these two dream
functions?

Also compare Brian's use of Friendship Letters as subplot transition
architecture against the corpus's embedded text techniques. The corpus shows
letters creating doubled reading positions (Letters From a Secret Admirer),
access-mode shifts (A Certain Type of Chic diary, Don't Want Perfection diary),
and artifact-as-plot-device (The Notebook, In Everything But Name). Brian's use
as structural scaffolding connecting parallel threads may be distinctive — does
any corpus story use embedded text as a subplot transition mechanism?

Canon virtues as psychological traps: compare Brian's treatment (hopepunk
subversion + the different-angle approach in TLTT chapters 3-8) against the dark
premise corpus's treatment (Salvation's Loyalty/Generosity as traps, Dash's New
Mom's Loyalty/Honesty as traps) and P&K's treatment (grimdark "power is the
only thing that matters" as the fullest expression of the pattern). Three
questions: does Brian's fiction use the same mechanism as the corpus? Does his
subversion use a different mechanism? Is the chapters 3-8 "different angle" a
third approach distinct from both?

Celestia as recontextualization vessel: the corpus shows four AU stories using
retroactive disclosure about Celestia as a major revelation architecture element
— her canonical inscrutability makes her a natural site for hidden knowledge that
produces the strongest M2 operations in the corpus. Brian's recall: his handling
of Celestia follows a similar mechanism. Compare TLTT's Celestia design (v1
archive subjects, v2 working plan, lineage discussions) against the corpus
pattern. Is Brian using Celestia the same way (inscrutability → retroactive
recontextualization)? Is this an instinctive choice shared with the corpus
authors, or a deliberate design informed by reading them?

Shame-about-desire and sex-as-thematic-testing-ground: compare Brian's design for
TLTT's "Passion" chapter and The Kitty of Westkeep against the explicit/plot
corpus's techniques. Does Brian use the same delivery mechanisms (self-constructed
priors demolished, fantasy sequences, split-self DT dialogue about desire)? Does
his treatment differ from the corpus's predominantly romance-centered framing? The
Kitty story is the most directly comparable to the explicit/plot category — how
does its thematic architecture compare?

Testing specs moved from WU1.3 (scope reconciliation 2026-09-01):
- Per-story obstacle-type breakdown from corpus analyses alongside own-fiction
  findings — which corpus stories use structural or combined obstacles rather
  than pure characterological? Individual stories (P&K, Promises, AU stories
  with structural barriers) may cluster differently from the stated
  characterological dominance.
- Dual-POV structural dramatic irony: how does Brian's use compare to the
  corpus's treatment?
- Counterargument density: compare Brian's counterargument density against his
  genre counterparts in the corpus.
- Canon virtues as psychological traps: compare Brian's treatment against
  Salvation's and P&K's treatments.

Added 2026-09-01 (hypothesis 046, created during WU1.3 post-review):
- Classify the DT instances the seven per-story analyses cite as class A
  (gap-producing) or class B (told interiority in italics), using the
  sub-types in 046's first evidence entry; report the share per text and
  adjudicate the borderline sub-types (revelation-in-DT, arc-climax
  realization, comic voice). Compare against the corpus: do the DT-dominant
  corpus stories show the same split, or is class B distinctive to Brian?
  Note the sampling caveat — TEatS/NTL telepathy and THLB flashback italics
  inflate raw DT counts by roughly a third.
- Counterargument reading check (WU1.3 post-review, 2026-09-01): the v4
  brief's Theme Propositions section, applied to bonded stories, read the
  bond thesis in THLB and GIYC and scored opposition to that, missing the
  structural plot's genuine counterargument in both. The corpus deficit
  (76% none; romance/SoL lowest) came from the same section on the same
  kind of stories. Re-read a sample of romance/SoL corpus analyses'
  counterargument sections against source: does the deficit survive when
  the structural plot's thesis is read instead of the bond's?

**What it does NOT do:** Test hypotheses systematically (that is WU1.12). Propose
tracks. Apply the favorites lens (that is WU1.11).

**Output:** A cross-corpus comparison report
(`docs/v3-framework/WU1.9-cross-corpus-comparison.md`).

**Scale:** Single session reading 3 synthesis reports (WU1.1, WU1.3, WU1.4
outputs).

**Preconditions:** None.

**Status:** proposed

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

Two candidate model-intrinsic properties observed during the forward-plan-1
ordering session (2026-08-31), not yet tested:

(a) Systematic principle application: Opus 4.6 needed four corrections to apply
the enrichment flow ordering principle consistently across the 14-WU graph —
each error was convenience overriding principle (WU1.7 slotted early because
"quick"; WU1.8 left floating because "blocks on Brian"; WU1.5/WU1.9 given
"scheduling freedom"; WU1.8 before WU1.3 missing product-before-process). Fable
produced a correct ordering without these errors. The task type — systematic
principle application across a relationship graph — may be distinct from
synthesis/discovery work. Check code sessions for other instances of this error
class across models.

(b) Scope of initiative: Opus 4.6 stays narrowly scoped to the immediate
question rather than proactively surfacing related implications (e.g., not
flagging that WU1.8's "floating" contradicted the principle just enshrined).
Brian's observation: this is context-dependent — a weakness in framework
buildout (logic-driven, correct answers exist, proactive identification saves
follow-up rounds) but a strength in story planning (open-ended, Brian's
decisions, unsolicited advice violates retrieval-not-suggestion). Implies
model-intrinsic properties are task-type-dependent, not simply strengths or
weaknesses. Check code sessions for patterns in when narrow scope helped vs
hurt across different activity types.

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

**Preconditions:** Tier arbitration by Brian complete (done 2026-08-29).

**Status:** proposed

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

**Preconditions:** None.

**Status:** proposed

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
   Writing Techniques track shape: the current Writing Techniques track (id:113)
   is free-form NotesToSelf. The corpus study is producing structured technique
   insights (e.g., "split-self DT as hamartia works for Greek tragedy — see
   Salvation, Ribbons and Lace"). Should the track evolve to support structured
   technique references (corpus-sourced, paradigm-tagged, with links to source
   stories) or stay free-form? This determines whether corpus-derived technique
   insights have a systematic home or remain scattered notes.
   Anti-development representation: if WU1.4 confirms Chrysalis's designed
   stasis is a real pattern, the Character Development track (id:6) may need
   its definition broadened. Currently it asks "What is the plan for how the
   character changes?" — which assumes positive direction. The hypothesis is
   that the same track could hold designed non-change or decline without new
   architecture — the display question just stops assuming the direction. A
   character's arc trajectory (growth, stasis, decline) would be expressible
   on the existing track rather than requiring a separate anti-development
   track.
   M3 adjudication fuzziness: the corpus consistently notes the boundary between
   "reader infers change from behavioral proxy" (genuine M3) and "narrator states
   change with behavioral illustration" (told-with-illustration) is fuzzy. This
   may resolve the same way as the FID/gap finding: the planner designs the
   behavioral proxy sequence (plannable cross-scene architecture), while whether
   the narrator assists the reader's inference or lets the proxy stand alone is
   a prose-craft writing-time decision. Should M3 have a gradient (like M4's
   approached/sustained), or is the proxy-vs-told boundary a technique question
   the framework shouldn't try to adjudicate?
   Counterargument architecture support: the corpus shows stories with genuine
   counterarguments are generally stronger. The current Theme Plan track holds
   thematic propositions but doesn't explicitly invite counterargument design
   ("what is the opposing position, how does the story engage with it, how is
   it defeated rather than dismissed?"). Should a track or display question
   support designing the counterargument alongside the thesis?
   WU1.3 post-review (2026-09-01): in Brian's bonded stories the genuine
   counterargument lives in the intertwined structural plot, not the bond
   arc (THLB: the Guard's self-image vs their culpability; GIYC: Rarity's
   ambition as the pro-visibility position). Any counterargument support
   must attach to the structural thesis, and a bond-centered reading will
   miss it.
   DT-based knowledge asymmetry: the corpus shows DT doing more perception-gap-
   adjacent work than FID. Does Track 99 need to recognize DT-based gap design
   alongside or instead of its current FID prescription? Should the framework
   give DT-based knowledge asymmetry its own vocabulary rather than treating it
   as "adjacent to M4"?
   Candidate design targets from WU1.1 (if confirmed by WU1.3/WU1.4 as things
   Brian actually designs): (a) information architecture — macro-level management
   of what the reader knows when; (b) reader stance trajectory — designed shifts
   in reader-character relationship across scenes; (c) structural correspondence
   — echo architecture, mirroring, refrains. These are cross-scene effects the
   corpus documents but the current tracks don't address. If Brian's practice
   confirms them, evaluate whether they need track-level or architectural support.

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
   types (setup→payoff, parallel, contradicts, revelation chain, accumulation —
   the WU1.1 finding that Demonstration note 15 depends on notes 1-14 for its
   meaning, a relationship type distinct from setup→payoff)? Do existing
   codebase patterns (SubjectRelation model, NarrativePropertyValue) serve, or
   is new architecture needed?

7. **The bespokeness tension (hypothesis 044):** Can v3 restore v1's scene-level
   density at v2's quality level? Is the hallmark wall pattern real (each version
   exceeds its instrument capacity at a higher abstraction level), and if so, does
   v3's iterative method avoid or defer the next wall?

Testing spec moved from WU1.3 (scope reconciliation 2026-09-01):
- Dual-POV structural dramatic irony: how should it surface in v3 — is it a
  design target the planner should support (cross-chapter knowledge management
  across focalizers)?

**What it does NOT do:** Propose specific tracks. Write display questions. Author
cognitive-mode definitions. Decide which goal categories are plannable. These are
Brian's decisions informed by this evaluation's evidence. The evaluation presents
findings and tradeoffs; Brian decides.

**Output:** A framework evaluation document
(`docs/v3-framework/WU1.13-framework-evaluation.md`) — the evidence base for
v3 framework decisions.

**Scale:** Large session reading all prior integration WU outputs + working plan
data.

**Preconditions:** None.

**Status:** proposed

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

3. **Paradigm connections:** (WU1.3 post-review, 2026-09-01: Brian
   identifies TEatS and NTL — political plot with intertwined bonds and
   genuine multi-position counterargument — as the nearest technique
   precedents in his own fiction for TLTT.) Brian identified 5 paradigm connections from the
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

6. **Thematic vocabulary fit:** The corpus identified five stable thematic
   territories and category-specific additions. Do the Theme Plan tracks
   support these territories architecturally? Is the thematic vocabulary the
   framework uses accurate to how Brian's stories argue their propositions?

7. **Faust-era vs Hasbro-mandate characterization:** The corpus universally uses
   early-season characterization as baseline, but Brian questions whether this
   distinction is meaningful — AUs like P&K and Moon's Apprentice have Mane 6
   still recognizable as Faust's characters, and Lunaverse "arrogant" versions
   are close to canon S1-2. Check Brian's own fabula notes for what "Hasbro
   mandate versions" actually means in his characterization design, and whether
   the difference from what he's "bringing back" is as stark as assumed. Compare
   against what the corpus AUs actually do with characterization.

**What it does NOT do:** Propose plot changes. Write prose. Suggest story content.
Determine what any subject "needs next." Those are Brian's decisions.

**Output:** A connection document
(`docs/v3-framework/WU1.14-connection.md`) — per-story perspective strategy
implications grounded in corpus evidence and Brian's own instincts.

**Scale:** Single session.

**Preconditions:** None.

**Status:** proposed

---

## Execution sequence

**Execution order** (derived 2026-08-31 by the blind two-pass ordering audit —
derivation, edge tables, and maintenance protocol in
`forward-plan-1-ordering-audit.md`, which is the living authority on this
order; this section is updated to match whenever the audit's derived order
changes):

```
WU1.1 (corpus synthesis) ── complete
WU1.2 (Keep assessment) ── complete
  ↓
WU1.3 (own fiction) ── complete
  ↓
WU1.4 (V1 archive mining) ── voice attribution; strong edge into WU1.5
  ↓
WU1.9 (cross-corpus comparison) ── consumes WU1.3 + WU1.4
  ↓
WU1.11 (favorites + supplementary lens) ── consumes WU1.9; weak edges
  ↓                                          into WU1.5 and WU1.8
WU1.7 (working plan survey) ── weak edge into WU1.5 (stall-shape targets)
  ↓
WU1.5 (retrospective) ── consumes WU1.3; Keep ingest is internal to it
  ↓
WU1.8 (planning evolution) ── waits in slot on Brian's preprocessing
  ↓                            (or runs TLTT-only per its spec)
WU1.10 (pipeline) ── weak edges in from WU1.4, WU1.5, WU1.8
  ↓
WU1.12 (hypothesis adjudication) ── needs ALL evidence WUs (Brian ratified
  ↓                                  2026-08-31); hard-blocks on WU1.8
WU1.13 (framework + architecture eval)
  ↓
WU1.14 (connection to TLTT)
```

An earlier hand-derived sequence (1.4 → 1.3 → 1.7 → 1.5 → 1.9 → …, recorded
2026-08-31 in prior revisions of this section) was superseded the same day by
the audit: its asserted enrichment chain did not survive blind pairwise
evaluation. The audit's derived order differs in three ways — WU1.3 leads
(the strong edges run from own-fiction findings into the mining pass, not the
reverse), the comparison block (WU1.9, WU1.11) runs immediately after the
discoveries, and the retrospective (WU1.5) runs late. Preconditions gate
execution timing in place, never position: the two skills to build are the
first tasks inside WU1.3 and WU1.4, the Keep ingest is internal to WU1.5, and
Brian's preprocessing of the revision-history exports is the critical-path
precondition for the WU1.12 → WU1.13 → WU1.14 tail.

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
| 023 | three-concern-separation | WU1.1 | WU1.3, WU1.9, WU1.13 |
| 024 | dimensional-vs-hierarchical | WU1.1 | WU1.3, WU1.9, WU1.13 |
| 025 | mechanism-hierarchy-is-complexity | WU1.1 | WU1.9 |
| 026 | three-axis-independence | WU1.1 | WU1.9 |
| 027 | cognitive-mode-principle | WU1.9 | WU1.13 |
| 028 | variable-focalization-master | WU1.1 | WU1.3, WU1.4, WU1.5, WU1.9, WU1.11 |
| 029 | perception-gap-delivery | WU1.1 | WU1.3, WU1.4, WU1.5, WU1.9, WU1.11 |
| 030 | narrator-character-blend | WU1.1 | WU1.3, WU1.9 |
| 031 | dt-knowledge-asymmetry | WU1.1 | WU1.3, WU1.4 |
| 032 | first-person-m4-effects | WU1.1 | WU1.3 |
| 033 | non-thematic-goal-categories | WU1.1 | WU1.3, WU1.9, WU1.11, WU1.13 |
| 034 | prose-craft-boundary | WU1.1 | WU1.3, WU1.9, WU1.11, WU1.13 |
| 035 | embedded-text-category | WU1.1 | WU1.3, WU1.4, WU1.13 |
| 036 | wi-terminal-ratio | WU1.1 | WU1.3, WU1.13 |
| 037 | multi-story-focalization-profiles | WU1.3 | WU1.9, WU1.13, WU1.14 |
| 038 | instinctive-mechanism-practice | WU1.3 | WU1.4, WU1.9, WU1.11 |
| 039 | fim-reading-effect | WU1.3 | WU1.8, WU1.9 |
| 040 | fabula-dialogue-replacement | WU1.1 | WU1.3, WU1.4, WU1.9 |
| 041 | sufficient-stability-iterative | WU1.7 | WU1.5, WU1.13 |
| 042 | editor-modes-evaluation | WU1.7 | WU1.5, WU1.13 |
| 043 | note-design-relationships | WU1.4 | WU1.13 |
| 044 | bespokeness-tension | WU1.4 | WU1.5, WU1.13, WU1.14 |
| 045 | keep-notes-provenance | WU1.2 | — |
| 046 | dt-two-classes | WU1.9 | WU1.13 |

## Preconditions summary

| Precondition | Blocks | Owner | Status |
|---|---|---|---|
| Adapted analyze-story skill (self-diagnostic framing, unfinished works) | WU1.3 | Claude Code (buildout work) | Done (embedded in subagent prompts, 2026-09-01) |
| V1 archive mining skill (extraction methodology, voice attribution, structured output) | WU1.4 | Claude Code (buildout work) | Done (2026-09-02): `v1-archive-mining` skill + `tools/StoryPlanner.VoiceAttribution` |
| Preprocessing of raw revision history exports in `Planning_Document_Revision_History/` | WU1.8 | Brian | Not started |
| V1 database snapshots (`TheLionessOfTallTale[date].db`) downloaded from Google Drive and made queryable | WU1.4, WU1.5 (enhancement) | Brian | Done (2026-09-02): 15 snapshots in `source_material_references/v1 sqlite/`, renamed `yyyy-MM-dd`, read raw by the attribution tool (no schema migration); the 2025-12-23 backup on Drive is not downloaded |
| Keep sidecar ingest into lineage.db (selective, authored include-list) | WU1.5, WU1.8 | Claude Code (buildout work) | Not started — WU1.2 assessed feasibility, see implementation-candidates.md |
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
