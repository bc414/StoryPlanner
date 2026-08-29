# Framework v3 Synthesis Plan (2026-08-29)

## Origin

On 2026-08-14, Brian set up automated analysis of his Fimfiction favorites during a
vacation week: *"I am going to be on vacation for a week but don't want my Claude
subscription to go to waste."* The analysis framework was not invented for the
pipeline — it was derived from Brian's own shipped track system (the mechanism ×
inference-stage matrix from conv 47's note categorization bootstrapping). The v2
framework, designed to organize the story planner's note tracks, was repurposed as a
measurement instrument applied to 112 other people's stories.

The results surprised. 42 stories under v1-v3 Briefs produced 8 hypotheses about
where the framework was overfit or incomplete. 112 stories under the v4 Brief
produced 7 meta-analysis reports that confirmed some hypotheses and challenged
foundational assumptions: FID as the only delivery mechanism for perception gap,
theme as the only goal category, the P→WI→T chain as universal. The measurement
instrument revealed its own systematic biases when applied at scale.

This is the scientific method applied to narrative design. Brian built an instrument
(v2 framework), ran experiments (112 story analyses), the results revealed the
instrument's limitations, and now the instrument needs to evolve. The v3 framework
is that evolution — designed by the same empirical method, with the same epistemic
humility: every finding is a best-effort hypothesis, not a settled fact. Future
evidence may revise v3, just as v3 revises v2.

This method is not only how v3's framework is DESIGNED — it is also how v3's
framework OPERATES on TLTT content. v0 and v1 captured instincts (observation). v2 named
them (hypothesis formation without testing). v3 tests them (experimentation against
evidence). The planner becomes an instrument that can be linted: any assertion about
craft technique can be checked against 112 analyzed stories, Brian's own writing,
v1 archive instincts, and framework provenance. Before the v3 codebase (the MCP
server, agentic workflows, external corpora), this linting was impossible — the model
could hypothesize but not verify. Now it can.

## The three-fold goal

1. **Understand** — what stories Brian reads do at the scene level, what Brian
   instinctively does in his own writing and v1 planning, and where the current
   framework vocabulary came from. Build the evidence base.
2. **Evaluate** — test the v2 framework against the evidence it generated. Identify
   where it measures accurately, where it is overfit (FID fixation, theme-only goals),
   and where it is blind (non-inferential reader operations, plot-point-level tracks,
   the narrator-character blend). Every prior assertion is a hypothesis until the
   evidence supports it.
3. **Evolve** — redesign the framework's conceptual hierarchy, track architecture,
   and scope coverage so the planner can support scene-level work across the full
   range of what Brian wants to do, across all stories in the multi-story project,
   with clean voice separation and iterative methodology built in.

## Version history

Two things evolve across versions: the **codebase** (the software — WPF app, database
schema, tools, MCP server) and the **narrative design framework** (the conceptual
system that determines what the planner tracks and how stories are analyzed — the
mechanism × inference-stage vocabulary, cognitive modes, track definitions, scope
levels, EditorModes, and the analysis brief that applies this vocabulary to other
stories).

In v0 through v2, the codebase and the narrative design framework evolved together —
each code rewrite was also a framework rewrite. v3 is the first time they diverge:
the v3 codebase shipped in Jul 2026 (MCP server, external corpora, agentic workflows),
and the v3 framework evolves now, on top of the existing code, through evidence
rather than through a rebuild.

**v0 (Apr-Dec 2025):** Single Google Doc (22K→132K chars). Brian wrote naive TLTT
chapters 1-2 and hit a wall: fabula leaking into the syuzhet (worldbuilding delivered
through dialogue, revelation architecture unplanned). The Canalave Library (TCL) website
was a parallel project (Oct-Nov 2025). Gemini web chat began Nov 30-Dec 7 for TLTT.

*Codebase:* none (Google Doc).
*Narrative design framework:* implicit. Brian knew instinctively that stories need
dramatic irony (from Silver, 2014), that worldbuilding should drive plot (from strategy
games and Pokemon writing), and that perspective restriction creates reader engagement
(from THLB's alternating first-person). None of this was named. The naive TLTT chapters
show FID instincts ("as if roses were red"), behavioral proxy ("Is the castle secured?"),
and fabula-through-dialogue as the default delivery mechanism. The Google Doc mixed
fabula, syuzhet plans, chapter drafts, and character notes inline — no separation.

**v1 (Dec 8, 2025 - Apr 25, 2026):** Gemini-built story planner. 42 commits, 17-day
build sprint, served 4.5 months. "Don't lose the thought" — raw capture into free-form
textboxes. NotebookLM (NLM) Perspective Analysis notebook (Feb 2026, 172 turns) and
Refinement of Aquileian Lore notebook ran in parallel; the Aquileian lore overflow
birthed the multi-story fabula paradigm. NLM introduced deep third/FID/DT vocabulary
from analyzing Pokemon stories Brian read (not from P&K — the vocabulary predated
P&K analysis). Late in v1's life, Claude web chat arrived (Apr 9) and immediately
produced insights (Conscience as 6th Element, conv 8) too dense for v1's architecture.

*Codebase:* WPF + EF Core + SQLite, hardcoded navigation properties, free-form
textboxes per entity, no track system.
*Narrative design framework:* partially named. NLM gave Brian vocabulary (deep third,
FID, DT, Architect/Director), and AI Studio gave him fabula/syuzhet separation
(aistudio:6, Mar 27 2026). But the planner had no structure to hold this vocabulary —
the framework lived in NLM notebooks and AI conversations, not in the tool. The
planner was a capture instrument, not a design instrument. v1's notes contain Brian's
instincts alongside Gemini's analytical voice, unseparated.

**v2 (Apr 26 - Jul 18, 2026):** Claude web chat + GitHub Copilot. 54-day build
(17 paradigm + 16 code + 21 bootstrapping). Served ~7 weeks of fabula migration at
subject level. Scene-level work stalled — the FID/perception-gap prescription from
NLM and early Claude conversations made scene design feel premature. Meanwhile Brian
learned Claude Code via The Canalave Library (Jun 13 - Jul 31).

*Codebase:* rebuilt from ground up. Type Object pattern (SubjectDefinition,
NoteTrackDefinition), polymorphic ownership, no navigation properties, data-driven
configuration.
*Narrative design framework:* formally named and systematized. The 5-layer split
(world truth / omniscient timeline / character psychology / narrative architecture /
thematic argument — where layers 1-3 are fabula, layer 4 is syuzhet design, and
layer 5 is the argument that emerges from both), cognitive
mode separation (ZeroFocalization / NarrativeDesign), 113 tracks with display
questions, 5 EditorModes (Expansion / Linking / Gardener / Audit / Scene Design),
the P→WI→T inference chain, perception gap taxonomy (ironic / tragic / closing /
aligned). Derived from 5 Claude conversations totaling 571 blocks + NLM notebooks +
42 analyzed stories. Untested against broader evidence.

**v3 codebase (Jul 19 - present):** Born from TCL's Claude Code apprenticeship.
Conversation Reader (Jul 19), CLAUDE.md + MCP server (Jul 28), explosive 4-day feature
burst (Jul 28-31), external corpora absorbed (Aug 16-26). The MCP server is the v3
paradigm: external corpora (lineage, conversations, source texts, code sessions) live
OUTSIDE the .storyplan, queryable but not mixed with Brian's notes. The analysis
pipeline (112 stories, Aug 18-28) is the first experiment testing v2's framework
assertions against evidence.

**v3 narrative design framework (this plan):** The v3 codebase enables evidence-based
framework revision for the first time. v0 was observation (unnamed instinct). v1
was partial naming (vocabulary from NLM/AI Studio without structure to hold it). v2
was hypothesis formation (formal system from conversations, untested). v3 adds
experimentation: test hypotheses against 112 analyzed stories + Brian's own fiction +
v1 archive mining + planning doc revision history + framework provenance. This is the
scientific method applied to narrative design.

## The scientific method framing

Brian's intellectual method has always been: hypothesize, test, correct. This is
visible across all paradigm eras — in v1 Gemini conversations, Brian corrects
Gemini's axis definitions against his own plan knowledge; in v3 MCP conversations,
Brian demands Claude verify against specific notes. The stance was always there.
What changed is the TOOLING: v1 treated the plan text as atmosphere (pasted in bulk,
used loosely); v3 treats it as queryable evidence (specific notes by ID, verified
against established data). Same person, same intellectual method, different compliance
infrastructure (see H25).

Whether the evidence-as-queryable-data approach consistently produces better outcomes
than the atmosphere approach is itself a hypothesis (see H26), supported by the
post-MCP conversations (conv 76 political axes, conv 80 hippogriff ontology) but
not yet validated across all work types. The synthesis work itself is a test of this
hypothesis.

A hypothesis is a testable prediction. Every assertion about how narrative technique
works — from the original 8 hypotheses to the new ones this plan introduces — is
treated as a hypothesis until evidence supports or refutes it. Prior assertions from
NLM, AI Studio, and Claude web chat are best efforts with limited data, not settled
facts. Findings from the 112-story corpus are better-evidenced best efforts, not
definitive conclusions. The framework is iterative and cumulative; no version is final.

Claude Code can form hypotheses about craft technique grounded in evidence. It cannot
form opinions about story content (what TLTT should say, what a character should do).
This distinction is in CLAUDE.md and governs all work units below.

## Voice registers (H27)

All data in the planner and its corpora was authored by a voice. The current
hypothesis identifies five distinct registers. Whether five is the correct count —
whether some collapse, further split, or new ones are discovered — is tested by
WU2b (v1 mining) and WU5d (framework evaluation). The list below is the best-effort
starting hypothesis, not a closed taxonomy.

1. **Brian's prose/craft voice** — dialogue fragments, behavioral proxy instincts, the
   "Is the castle secured?" flatness, the "as if roses were red" narrator wit. This is
   what eventually becomes prose. No AI involvement in producing it.
2. **Brian's fabula voice** — structural truth about the world: "Chrysalis controls the
   economy through MEFO bills." What IS true, independent of how the reader encounters
   it. Maps to the ZeroFocalization cognitive mode in v2.
3. **Brian's syuzhet/design voice** — how the reader encounters the fabula: "the reader
   should learn this in ch28 through Eagleclaw's testimony," "this scene parallels that
   scene." Maps to the NarrativeDesign cognitive mode in v2.
4. **Brian's analytical voice** — Brian examining patterns in stories and his own work:
   his comments on P&K, his filly-fooling review, his questions in conversations ("where
   does this trope come from?", "is this the same as globalization vs isolationism?").
   This is Brian's own voice, distinct from AI analytical voice — it records what Brian
   notices, values, and pushes back on.
5. **AI analytical voice** — Gemini's framing, Claude's formulations, NLM's terminology.
   Provenance and context, useful for understanding but not authoritative for the plan.
   In v3, this voice lives in external corpora (lineage, conversations) accessible via
   MCP. It should not be in the .storyplan notes.

v1 mixed all five. v2 separated fabula (voice 2) from syuzhet/design (voice 3) via
cognitive modes, but left AI analytical voice entangled with both (copy-paste from
Gemini conversations into notes) and did not distinguish Brian's analytical voice
(voice 4) from AI analytical voice (voice 5). v3 decouples all five — the MCP
server's lineage corpus makes AI voice greppable and separable, and Brian's analytical
voice (his comments, reviews, conversation turns) is identifiable as HIS data, not
AI contamination.

## Hypotheses about the planner itself

These are hypotheses about the instrument, not about craft technique. They frame
the synthesis work and can be updated by evidence.

**Purpose of the story planner (hypothesis):** Managing trajectories for serialized
multi-story publication. Converting revision debt (arcs that drift, gaps that
disappear, irony that evaporates accidentally) into design specifications that can be
validated before prose is written. This is non-negotiable for serialized publication
where published chapters cannot be rewritten. The exact scope of "trajectories" — what
the planner should and should not track — is what the synthesis informs.

**The AI context contradiction (hypothesis):** The fundamental design struggle across
v0-v2 was the contradiction between needing AI context for architectural analysis and
not wanting AI voice in the prose layer. v0 had no AI (no contamination, but no
analysis — Brian hit the wall alone). v1 introduced the full-plan-paste paradigm
(~940K chars into Gemini's 1M context every session, reports W05-W07) which created
a feedback loop: AI reads plan → generates insight → Brian pastes insight back →
AI reads its own prior insight next session → AI voice accumulates in the notes.
v2 tried to separate cognitive modes within the plan (ZF/ND tracks, "craft notes,"
"garden notes") but couldn't break the loop because full-plan export was still the
interaction paradigm. The v3 MCP sidecar architecture is the first design that
reconciles this: the AI has context access via targeted queries without the plan
absorbing the AI's voice. The sidecar corpora (lineage, conversations, code sessions)
hold the AI's analytical voice; the .storyplan holds Brian's voices.

**Why v2 stalled at scene level (hypothesis, two causes):** First, the prescribed
sequential workflow (accumulate → establish truth → design experience → write) gated
scene-level work on subject-level completion, which is unbounded. The experience-design
vocabulary was too narrow (FID-centric, perception-gap-focused) to feel actionable.
Second, the AI feedback loop meant the plan's notes were a mix of Brian's voice and
AI voice. When Brian tried to use the plan for scene-level design — the step closest
to prose — the plan didn't fully feel like his own instrument. Both causes reinforced
each other: the vocabulary was narrow AND the notes felt foreign. The corpus analysis
is the bottom-up alternative (broaden the vocabulary), and the voice linting protocol
is the data-quality fix (clean the notes).

**Voice separation as prerequisite (hypothesis):** Clean voice separation — Brian's
voices in the .storyplan, AI voice in sidecars — is a structural prerequisite for
the v3 framework to operate correctly. If the .storyplan still contains AI analytical
voice, any v3 track architecture built on top of mixed-voice data inherits the
confusion. The three-voice separation is not a cleanup task for after the framework
evolves — it must precede or run alongside the framework evolution. (See H24.)

**The hallmark wall pattern (observation):** Each version hit the same wall at a higher
level of abstraction — the data's complexity exceeded the instrument's structure. v0:
fabula leaked into chapter plan. v1: architectural density exceeded free-form textboxes.
v2: scene-level work stalled by narrow experience-design vocabulary + voice
contamination. Each upgrade was a structural expansion. v3's expansion is: tested
vocabulary + scene-level architecture informed by evidence + voice separation via
sidecar corpora.

## Consolidated hypothesis set

### From the original 8 (2026-08-17, derived from 42 stories)

- **H1:** WI-terminal links are legitimate — not every inference serves a theme.
  (Tested by: WU1, WU5b, WU5d)
- **H2:** World Inference is a superset of Thematic Evidence — the P→WI→T pipeline is
  a special case. (Tested by: WU1, WU5b, WU5d)
- **H3:** DT-based knowledge asymmetry is a real technique adjacent to M4.
  (Tested by: WU1, WU2a, WU5b)
- **H4:** First-person narration produces M4-adjacent effects.
  (Tested by: WU1, WU2a, WU5b)
- **H5:** The mechanism hierarchy is structural complexity, not quality.
  (Tested by: WU1 — likely already confirmed)
- **H6:** Comedy, atmosphere, and narrative voice are prose-craft, not framework.
  (Tested by: WU1, WU5d)
- **H7:** Embedded texts and dreams sit outside mechanism categories.
  (Tested by: WU1, WU5b)
- **H8:** The three-axis model (mechanism × inference stage × rendering mode) is
  structurally independent but semantically coupled.
  (Tested by: WU1, WU5d)

### From this conversation (2026-08-28/29)

- **H9:** Variable focalization is the master perspective principle, not deep
  third/FID. FID is one mode in the variable-focalization toolkit; the power
  comes from the variation, not from staying deep.
  (Tested by: WU1, WU2a, WU2b, WU4, WU5b)
- **H10:** Character-reader perception gap is the correct WorldInference target;
  FID is not the only delivery mechanism. Conv 21's gap taxonomy (ironic, tragic,
  closing, aligned) was designed technique-agnostic; the FID narrowing happened
  afterward. Track 99's usage directive should specify the inference target, not
  the prose technique.
  (Tested by: WU1, WU2b, WU4, WU5b, WU5d)
- **H11:** Theme is not the only goal category. Humor, establishing prior beliefs,
  emotional warmth, structural setup, and potentially others are peer goal
  categories alongside thematic evidence.
  (Tested by: WU1, WU5b, WU5d)
- **H12:** The P→WI→T chain is one pathway, not the universal one. Some pathways
  (P→emotional resonance, P→humor, P→prior-belief setup) may not pass through
  "inference" at all.
  (Tested by: WU1, WU5d)
- **H13:** Different stories in the multi-story project need different variable-
  focalization mixes. Chrysalis's Greek tragedy ≠ TLTT's ensemble ≠ Kitty's
  bildungsroman. One track architecture, variable usage profiles.
  (Tested by: WU2a, WU5d, WU6)
- **H14:** Brian already practices all mechanism types instinctively, unnamed. The
  v1 archive and naive TLTT chapters show M2, M3, behavioral proxy, structural
  parallels — all designed without formal vocabulary.
  (Tested by: WU2a, WU2b, WU5a)
- **H15:** FiM reading improved Brian's FID/variable-focalization instincts. The
  naive TLTT chapters and Harvest of Falldale (post-FiM) should show different
  technique profiles from the Pokemon stories (pre-FiM).
  (Tested by: WU2a)
- **H16:** Narrator-character blend (the "roses were red" mode) is a distinct
  technique — the narrator's own literary register channeling the focalizer's
  assessment — not captured by the current ZF/ND cognitive mode split.
  (Tested by: WU1, WU2a, WU2b, WU5d)
- **H17:** The cognitive mode separation principle (ZeroFocalization vs
  NarrativeDesign) is correct; the specific boundaries and modes may need revision
  to accommodate the three-level framework and non-inferential goals.
  (Tested by: WU4, WU5d)
- **H18:** The three-level framework (prose technique → mechanism → goal) describes
  the hierarchy that organizes what the planner should track. Prose techniques are
  the lowest level (never planned, chosen at writing time). Mechanisms are the
  middle level (designed reader operations). Goals are the highest level (what the
  scene is FOR). The planner tracks goals and mechanisms. Prose techniques live in
  a reference catalog (Writing Techniques).
  (Tested by: WU1, WU2, WU5d)
- **H19:** "What replaces fabula-through-dialogue" — the naive chapters delivered
  worldbuilding through characters explaining things to each other. The replacement
  is: behavioral evidence (the reader observes and infers), designed incomplete
  understanding (the POV character's limits generate reader inference), revelation
  architecture (when each piece of fabula enters awareness), and designed mistakes
  (the reader's wrong inferences are planned).
  (Tested by: WU1, WU2a, WU5a)
- **H20:** The "sufficient stability" principle (Stage 2 design can proceed per scene
  as relevant Stage 1 content stabilizes, not after all Stage 1 is complete) is
  correct but was never practiced. The v3 framework should enable iterative
  subject↔scene work rather than sequential subject-then-scene.
  (Tested by: WU4, WU5d)
- **H21:** The EditorModes (Expansion, Linking, Gardener, Audit, Scene Design)
  describe real cognitive directions that are approximately correct. Whether the
  specific mode designs (what's visible, what's writable, what's excluded) are
  optimal should be evaluated.
  (Tested by: WU4, WU5d)
- **H22:** The fundamental design struggle across v0-v2 was the contradiction
  between needing AI context for architectural analysis and not wanting AI voice
  in the prose layer. v0 had no AI (no contamination, but no analysis). v1
  introduced the full-plan-paste paradigm (~940K chars into Gemini's 1M context
  every session) which created a feedback loop: AI reads plan → generates insight
  → Brian pastes insight back → AI reads its own prior insight next session. v2
  tried to separate cognitive modes within the plan but couldn't break the loop
  because full-plan export was still the interaction paradigm. The v3 MCP sidecar
  architecture is the first design that reconciles this: the AI has context access
  via targeted queries without the plan absorbing the AI's voice.
  (Tested by: WU2b — the Gemini-voice separation will quantify how much AI voice
  is in the v1 archive. WU4 — the retrospective traces the feedback loop's effects.)
- **H23:** Once AI analytical voice is exorcised from the .storyplan and accessible
  in sidecars, the plan becomes a cleaner instrument for scene-level work — Brian
  reads only his own voice (fabula/architectural + prose/craft) when designing
  scenes and writing prose. The AI's insights remain available one MCP query away
  but do not contaminate the reading experience. This may partially resolve the v2
  scene-level stall: the stall was not only about the FID vocabulary being too
  narrow, but also about the plan not feeling like Brian's own instrument when
  he tried to use it for scene-level design.
  (Tested by: practice — after the linting protocol runs, does scene-level work
  feel less premature? This is experiential, not analytically testable.)
- **H24:** The five-voice separation (see "The five voices" section) is the
  structural prerequisite for the v3 framework to operate correctly. If the
  .storyplan still contains AI analytical voice (voice 5), the framework cannot
  distinguish between "what Brian designed" and "what an AI framed" — and any v3
  track architecture built on top of mixed-voice data inherits the confusion.
  Clean voice separation must precede or run alongside the framework evolution,
  not follow it.
  (Tested by: WU2b — quantifies the problem. Downstream linting protocol — fixes
  it. WU5d — evaluates whether the track architecture can be designed from the
  current mixed-voice data or requires clean data first.)
- **H25:** Brian's prompt pattern has always asked for grounding against the plan
  data ("check my plans," "look at the archive," "review story plans where needed").
  The v1 paste paradigm made compliance imprecise — 940K chars as atmosphere, used
  loosely, no specific notes referenced. The v3 MCP paradigm enables the precise
  grounding Brian always wanted — targeted queries returning specific notes by ID.
  The issue was tooling, not intent. This is independent from H22 (the feedback
  loop): grounding compliance is about how AI accesses INPUT; voice mixing is about
  what happens to AI OUTPUT.
  (Tested by: the post-MCP conversations (conv 76, 80) vs the pre-MCP conversations
  (conv 5, 11, 17) provide direct evidence — same prompt pattern, different
  compliance. WU4 retrospective can trace this across the full conversation history.)
- **H26:** Treating the plan as queryable evidence (v3) consistently produces
  better outcomes than treating it as pasted atmosphere (v1). "Better" means: fewer
  internal contradictions, more precise elimination of overfit structures, new lore
  constrained by existing lore in real time. The v3 tooling enables this; whether
  it consistently delivers is tested by the synthesis work itself. Note: v1 also
  produced eliminations and restructurings (the Stagnation of Harmony reframe from
  1000 years to 80 years is a major v1-era elimination). The hypothesis is about
  the PROCESS (evidence-grounded vs intuition-driven), not about whether elimination
  occurs at all.
  (Tested by: the synthesis work units themselves — do evidence-grounded findings
  produce more durable framework revisions than conversation-derived ones did?)
- **H27:** Brian's voice has five distinct registers (prose/craft, fabula,
  syuzhet/design, Brian's analytical, AI analytical) rather than three. The
  distinction between fabula and syuzhet/design maps to ZeroFocalization vs
  NarrativeDesign cognitive modes already in v2. The distinction between Brian's
  analytical voice and AI analytical voice matters for WU2b (v1 mining must not
  treat Brian's own analytical observations as AI contamination) and for the
  favorites lens (Brian's comments are his analytical voice, not AI's). Whether
  five is the correct count, or some voices collapse or further split, is tested
  by the mining and evaluation work.
  (Tested by: WU2b — the voice separation will reveal whether five is the right
  count. WU5d — framework evaluation assesses whether the cognitive mode system
  needs revision to accommodate the finer distinction.)

Each work unit reports hypothesis-relevant findings as it encounters them. WU5b
performs meta-adjudication: synthesizing all prior findings into per-hypothesis
verdicts.

## What this plan is and is not

- Not a sequential dependency chain. Lesson from v2: prescribed sequential
  workflows stall at transitions. Work units feed each other iteratively,
  not sequentially.
- Not definitive. Every finding is best effort. The plan itself is best effort.
  No language declares finality or completeness.
- Not prescriptive about prose technique. The planner specifies goals and
  mechanisms, never how to write.
- Not a machine proposing changes to the planner. The synthesis surfaces evidence
  and states hypotheses. Brian decides what to change. "Seeders seed structure,
  never prose" — even if the synthesis identifies a gap, it cannot author a display
  question.
- Not a one-shot exercise. The framework is iterative. v3 will be a best effort
  that future evidence may revise, just as this work revises v2.

## Work units

### WU1: Corpus Synthesis
*"What do the stories I read do at the scene level?"*

**Inputs:** 4.1a, 4.2a-e, 4.3 (the 7 existing meta-analysis reports, all in
`source_material_references/Reading Archive Analyses/`).

**What it does:** First-principles consolidation across all 7 reports. Cross-category
patterns. The M4/FID question (the 7:1 calibration divergence + universal FID→M4
correlation). Framework gaps (universal vs category-specific). Unnamed techniques
(which recur across categories). Length effects on mechanism distribution, perspective
technique, and obstacle architecture. If hypothesis-relevant patterns emerge, report
them, but do not make hypothesis testing the primary focus.

**What it does NOT do:** Apply lenses. Test hypotheses systematically. Propose tracks.
Reference TLTT or Brian's own writing.

**Output:** A consolidated corpus synthesis report.

**Scale:** Single session reading 7 reports. Can start immediately.

---

### WU2a: Own Fiction Analysis
*"What do I instinctively do when I write?"*

**Inputs:** Brian's 6 published stories as local files (epub/markdown):
- To Hone a Leaf Blade (Pokemon, 2015-2017, first-person alternating POV, ~complete)
- Wish (Pokemon, 2020, one-shot, finished)
- The Ember and the Spark (Pokemon, 2020-2021, finished)
- Nine Tales of Liberty (Pokemon, 2021-2023, unfinished — analyze what exists)
- Green Is Your Color (MLP, 2024-2025, unfinished — analyze what exists)
- The Harvest of Falldale (EaW, Nov 2025, one-shot, finished)

Also: Naive TLTT Chapters 1-2 (June 2025, in `source_material_references/`). These
predate v1 entirely and are the earliest evidence of Brian's TLTT prose voice.

**What it does:** Apply the v4 Analysis Brief to each story (using the analyze-story
skill, possibly adapted for self-diagnostic framing). Then produce a self-diagnostic
synthesis comparing across all 7 texts. The core question is open-ended: what does
Brian instinctively do when writing prose? Expected areas based on prior analysis
include mechanism profiles, interiority technique preferences, perspective discipline,
and the DT/FID relationship — but the synthesis should discover patterns, not just
confirm expected ones.

H15 is directly testable here: compare the pre-FiM Pokemon stories against the
post-FiM texts (GIYC, Falldale, naive TLTT chapters) for changes in FID/variable-
focalization instincts.

**What it does NOT do:** Compare to the corpus (that's WU5a). Judge quality. Propose
what Brian should change about his writing.

**Output:** 7 per-text v4 analyses + an own-voice synthesis report.

**Scale:** 7 subagent runs (skill: analyze-story), then 1 synthesis session.

**Blocks on:** Brian downloading stories from Drive with italics preserved. The 4
fanfiction.net stories (via fichub epub) need verification that italics survived
export. The 2 Fimfiction stories (GIYC, Falldale) are already epub.

---

### WU2b: V1 Archive Mining
*"What scene-level instincts did I capture before having formal vocabulary?"*

**Inputs:** All 450 plot points, 1,125 links, and relevant notes in the v1 archive,
accessible via MCP (`get_plot_points_archive`, `get_links_archive`,
`get_notes_archive`). The Gemini conversation corpus in lineage
(`search_lineage source:"gemini"`) for voice separation.

**What it does:** Read ALL plot point notes and link notes (no skipping — Brian's
explicit instruction). The core question is open-ended: what is Brian instinctively
doing at the scene level, and whose voice is doing it? Expected categories based on
the samples analyzed in this conversation include prose fragments, designed reader
experiences, instinctive mechanism usage, Gemini-voice analytical framing, and fabula
assertions — but the mining should discover categories, not just sort into
predetermined bins. Gemini-voice content is identifiable by grepping against the
lineage corpus (`search_lineage source:"gemini"`); Brian's analytical voice (his own
observations, corrections, and hypotheses in conversation turns) should be
distinguished from AI analytical voice rather than lumped together.

The output is a map of what Brian instinctively does at the scene level — with voice
attribution and an honest accounting of categories that emerged vs categories that
were expected.

**What it does NOT do:** Migrate anything to v2/v3. Propose tracks. Write display
questions. Judge whether Brian's instincts are "correct."

**Output:** Per-arc files in `docs/v3-framework/WU2b-v1-scene-instincts/`, each in
a structured format (note ID, voice attribution, discovered category, chapter,
excerpt). The structured format serves both WU5a (cross-corpus comparison can query
patterns) and the downstream linting protocol (voice attribution enables grep-based
cleanup). One file per arc batch.

**Scale:** Multiple subagents batched by TLTT chapter arc (e.g., Arc 1 chapters,
Arc 2 chapters, etc.), plus Aris and Paratext. Needs a skill for consistent
extraction methodology and structured output format.

**Blocks on:** Nothing — v1 archive accessible now via MCP.

---

### WU3: Planning Evolution
*"How did my thinking evolve per story?"*

**Inputs:** Planning doc revision history for:
- KU/Nine Tales of Liberty (Drive doc `1KKpKgwhyQ738zqyvSgcwdbIHFaqV2XJKrCNN5Zv3Jpw`,
  `1n-uCPOrovG3_8g03s5898-kgM5I2XPGiusTyNVMqYrE`)
- Green Is Your Color (Drive doc `1Scy_DFrIgU0Gx3_h83qYoOKwc3bNyMdZkjrFc40SXhI`)
- The Harvest of Falldale (Drive doc `1OtZ4Xd2Dx4SSsyyX1EZxIcM50XNysLLvGHdiX40_4aI` —
  planning doc nearly as long as the prose itself, a "proto v1 story planner")
- TLTT (already in lineage: 53 diffs, `gdoc:` ids)

**What it does:** Trace how Brian's scene-level thinking, thematic framework,
perspective decisions, and fabula/syuzhet separation evolved across each story's
planning lifecycle. Identify which instincts were stable across years vs which changed.
The v0 paradigm's fabula/syuzhet mix (or lack of separation) is visible in the
earliest revisions. Later revisions may show the separation emerging.

**What it does NOT do:** Analyze the prose (that's WU2a). Propose framework changes.

**Output:** A planning-evolution synthesis.

**Scale:** Processing (appscript export + GDocHistory tool) then 1 synthesis session.

**Blocks on:** Brian running the appscript to export revision histories from Drive,
then running `dotnet run --project tools/StoryPlanner.GDocHistory` to ingest them
into lineage.db.

---

### WU4: Retrospective
*"Where did my current vocabulary come from and where is it overfit?"*

**Inputs:**
- NLM Perspective Analysis notebook (lineage `nlm:3`, 172 turns)
- NLM Refinement of Aquileian Lore (lineage `nlm:6`)
- AI Studio fabula session (lineage `aistudio:6`, Mar 27 2026 — first use of
  fabula/syuzhet terminology)
- Early Claude conversations that birthed v2: conv 8 (Conscience), conv 17
  (multi-story fabula), conv 21 (perception gap + data architecture, 151 blocks),
  conv 36 (planning vs writing, 72 blocks), conv 47 (note categorization
  bootstrapping, 285 blocks)
- Conv 64 (P&K ASOIAF inspirations, 289 blocks — multi-topic, extract only
  framework-relevant blocks; some sections became Changeling Lands lore)
- This conversation's findings (the FID provenance chain, the variable-
  focalization diagnosis, the three-level framework proposal)
- The provenance table (which concept first appeared where):

  | Concept | First appeared | Source |
  |---|---|---|
  | Fabula/syuzhet separation | Mar 27, 2026 | AI Studio `aistudio:6` |
  | Architect/Gardener | Feb 2026 | NLM `nlm:3` |
  | Deep third / FID / DT distinction | Feb 2026 | NLM `nlm:3` t#54 (Bandits of the Forest) |
  | Variable focalization | Feb 2026 | NLM `nlm:3` t#94 |
  | 5-layer split | Apr 15, 2026 | Conv 17 |
  | Perception gap taxonomy | Apr 20, 2026 | Conv 21 |
  | P→WI→T chain | May 11, 2026 | Conv 47 |
  | Gardening the architecture | May 4, 2026 | Conv 36 (Brian's phrase) |
  | Conscience as 6th Element | Apr 10, 2026 | Conv 8 |

**What it does:** Trace the provenance of each major framework concept. For each,
assess against the corpus evidence (from WU1): confirmed, partially validated,
overfit, or untested. Position NLM and early Claude sessions as best efforts that
did important work with limited data. Specifically:
- The FID fixation chain: NLM t#54 (DT/FID distinguished) → NLM t#94-130 (deep
  third prescribed for TLTT) → Conv 21 (perception gap designed technique-agnostic,
  then narrowed to FID in Track 99) → Conv 36 block 1245 (italics epiphany cemented
  FID as the goal). Each step was reasonable; the cumulative effect was overfit.
- The "every link must have T" rule (conv 47 block 1520): does the corpus support
  relaxing it?
- The four gap types (ironic/tragic/closing/aligned) from conv 21 block 613: one
  AI's taxonomy, to be held as hypothesis, not tunneled on.
- The v2 prescribed workflow (Stage 0→1→2→3, 5 EditorModes): what stalled and why.

**What it does NOT do:** Propose replacements (that's WU5d). Judge whether prior
work was "wrong" — it was best effort with the data available.

**Output:** A retrospective document with per-concept verdicts and provenance chains.

**Scale:** Single session. All sources accessible now via MCP and lineage.

**Blocks on:** WU1 complete (needs the corpus evidence baseline to assess against).

---

### WU5a: Cross-Corpus Pattern Synthesis
*"What do stories I read do vs what do I instinctively do?"*

**Inputs:** WU1 (corpus patterns), WU2a (own fiction patterns), WU2b (v1 archive
instincts).

**What it does:** Compare the three corpora:
- What techniques appear in the 112 analyzed stories that Brian also uses
  instinctively (confirmation of instinct)?
- What techniques appear in the corpus that Brian doesn't use (potential learning)?
- What does Brian do instinctively that the corpus doesn't show (Brian's distinctive
  voice)?
- Where do Brian's favorites cluster — do the stories that "stick" share techniques
  with Brian's own instincts, or do they do something Brian doesn't yet do?

**Output:** A cross-corpus comparison report.

**Scale:** Single session reading 3 synthesis reports.

**Depends on:** WU1, WU2a, WU2b complete.

---

### WU5b: Hypothesis Meta-Adjudication
*"What does ALL the evidence say about each hypothesis?"*

**Inputs:** All prior WU outputs (each reports hypothesis-relevant findings as
encountered). The consolidated hypothesis set (H1-H21). FEATURE-AUDIT.md (to verify
no hypothesis reinvents a cut feature).

**What it does:** For each hypothesis, synthesize the evidence from all WUs that
tested it. Verdict: confirmed, refuted, refined, or insufficient evidence. Where
evidence conflicts across WUs, report the conflict rather than forcing a resolution.
Brian adjudicates conflicts.

**What it does NOT do:** Resolve conflicts by choosing one WU's evidence over
another's. That is Brian's decision.

**Output:** A hypothesis adjudication document — each hypothesis with verdict,
evidence citations, and any unresolved conflicts.

**Scale:** Single session. Reads prior WU outputs + hypothesis set.

**Depends on:** WU1, WU2, WU4, WU5a complete. (WU3 findings feed in if available
but are not blocking.)

---

### WU5c: Favorites and Supplementary Lens
*"Does what sticks with me correlate with specific technique patterns?"*

**Inputs:**
- WU5a cross-corpus comparison
- `source_material_references/corpus-favorites-tiers.txt` (Brian's subjective tiers:
  Absolute Favorite, Great, Good, Neutral, Abandoned, Special)
- Brian's comments on stories: `P&K comments.md`, `pax-chrysalia-comments.md`,
  `Comments.md` (golden-disks, the-parent-trap, others)
- Brian's analytical work: `Filly Fooling review.txt`, `Filly Fooling analysis.csv`
- Naive TLTT chapters 1-2 (as evidence of v0 prose instincts)

**Biasing caveats (must be stated in the output):**
- Brian's comments skew toward P&K and Pax Chrysalia (long, impactful, active). Most
  favorites have no comments. Absence of a comment ≠ absence of a reaction.
- The filly-fooling review is Brian theorizing improvements, not pure reader response.
- The Special tier stories (P&K, filly-fooling, ribbons-and-lace, injuring-eternity,
  the-last-train-home, clocktower-society) carry specific analytical context that
  must be read alongside their v4 analyses, not averaged into the general population.
- P&K is "the MOST impactful story on my journey, yet I disagree with its structural
  themes so much that it spawned the TLTT project itself" — impact ≠ agreement.
- Abandoned tier stories (flying-high-falling-hard, romance-reports, the-moons-
  apprentice) were not finished by Brian. Their analyses may cover content past
  Brian's reading point. The favorites lens cannot apply to unread sections.

**What it does:** Overlay favorites tiers onto WU5a's findings. Ask: do Absolute
Favorites cluster around particular mechanism profiles, perspective techniques, or
obstacle architectures? Is there a craft signature that distinguishes "sticks with
me" from "forgettable"? Do Brian's comments reveal instinctive reactions that map
to named framework patterns?

For Special tier stories: read Brian's supplementary material alongside the v4
analysis and note where Brian's instinctive analytical voice (pre-framework) noticed
the same patterns the v4 Brief's formal analysis identified.

**Output:** A favorites-lens report with biasing caveats.

**Scale:** Single session.

**Depends on:** WU5a complete. Brian's tier arbitration in `corpus-favorites-tiers.txt`
complete.

---

### WU5d: Framework and Architecture Evaluation
*"What should the planner track, at what scope, and in what conceptual hierarchy?"*

**Inputs:** WU5a (cross-corpus patterns), WU5b (hypothesis verdicts), WU5c (favorites
lens), WU4 (retrospective), WU3 (planning evolution, if available), v2 current track
definitions via MCP (`get_track_definitions`).

**What it does:** The convergence of all evidence into framework-level findings. Four
areas:

1. **The three-level hierarchy evaluation (H18):** Does the evidence support three
   separable levels (prose technique → mechanism → goal)? Where do they blur? What
   are the goal categories? Where is the plannable/prose-craft boundary — which
   non-inferential effects (humor, warmth, tension) are plannable goals vs which are
   purely prose-craft?

2. **The three scope levels:** Subject-level tracks (the reference layer), link-level
   tracks (the application layer), plot-point-level tracks (the scene's own tracks —
   currently underdeveloped). What should each scope track? How do they feed each
   other (subject → link → plot point)? Conv 47's Expansion/Linking/Gardener modes
   describe real cognitive directions — evaluate whether the specific mode designs
   are optimal (H21).

3. **Current track coverage:** Which existing tracks hold up against the evidence?
   Which are overfit (H10: Track 99's FID specification)? Which are missing (plot
   point tracks underdeveloped, non-thematic goal tracks absent)? Which cognitive
   modes (ZF/ND/Analogical/LinguisticExecution/Garden) need revision (H17)?

4. **The multi-story dimension (H13):** Different stories need different usage
   profiles. The track architecture should be universal; the density of usage per
   track varies per story. Chrysalis's Greek tragedy → heavy perception gap.
   TLTT's ensemble → heavy prior-belief management. Kitty's bildungsroman → heavy
   bond and development. How does the planner support per-story perspective strategy?

**What it does NOT do:** Propose specific tracks. Write display questions. Author
cognitive-mode definitions. These are Brian's decisions informed by this evaluation's
evidence.

**Output:** A framework evaluation document — the evidence base for v3 decisions.

**Scale:** Large session reading all prior WU5 outputs + WU4 + WU3.

**Depends on:** WU5a, WU5b, WU5c, WU4 complete. WU3 ideal but not blocking.

---

### WU6: Connection
*"What does this mean for TLTT and the broader project?"*

**Inputs:**
- WU5d framework evaluation
- `source_material_references/long-corpus-categories.txt` (TLTT paradigm annotations)
- v2 working plan via MCP (stories, subjects, tracks, plot points, links, notes)
- The multi-story architecture: TLTT, TKOW, Countess of Krystal Rock (Chrysalis),
  Lord of the Hedge (Blueblood), Aris, Grover III and Celestia, plus planned stories
  not yet in v2 (Daring Do / Cocoltic Yaoyotl, Flowing Current, Applejack's Parents,
  Mali & Luna, Henri & Fleur, and new focal-character short stories as they emerge)

**What it does:** Connect the framework evaluation to the actual stories Brian is
planning. For each story (or story cluster):
- What variable-focalization mix does the evidence suggest works for this paradigm?
- What mechanism profiles do Brian's favorites in the same paradigm exhibit?
- Where does the v2 data already contain scene-level design (e.g., Chrysalis's 3
  perception gap notes) and where is it thin?
- What does "variable focalization with a plan" mean for TLTT chapter 1 specifically,
  given the naive chapter evidence?

Brian's 5 paradigm connections from the categories file:
- Ensemble stories → TLTT's multi-focalizer structure
- Romance/SoL → TwiJack arc foundation
- Dark premise → hopepunk-over-darkness surface
- AU → EaW × FiM identity
- Explicit/plot → Kitty of Westkeep

The 75%/25% reading-order model: prequels stand alone for 75%, then converge with
TLTT for the final 25%. Prior beliefs are conditional on reading order. A reader
who read TLTT first has different priors entering a prequel than a reader who starts
with the prequel. Both experiences are designed; neither is primary. Re-read value
is a consequence, not an accident.

**What it does NOT do:** Propose plot changes. Write prose. Suggest story content.
Determine what any subject "needs next." Those are Brian's decisions.

**Output:** A connection document — per-story perspective strategy implications
grounded in corpus evidence and Brian's own instincts.

**Scale:** Single session.

**Depends on:** WU5d complete.

## Dependency graph

```
WU1 (corpus synthesis) ─────────────────────────────────────────┐
  |                                                              |
  ├──→ WU4 (retrospective, needs WU1 baseline)                  |
  |                                                              |
WU2a (own fiction) ──────────┐                                   |
WU2b (v1 archive mining) ───┤                                   |
                             ↓                                   |
                    WU5a (cross-corpus comparison) ──────────────┤
                             |                                   |
                    WU5b (hypothesis meta-adjudication) ←── WU4  |
                             |                                   |
                    WU5c (favorites + supplementary lens)        |
                             |                                   |
WU3 (planning evolution) ──→ WU5d (framework + arch evaluation) |
                             |                                   |
                    WU6 (connection to TLTT project) ←───────────┘
```

**Parallel work:** WU1, WU2a, WU2b, WU3 can all start in parallel (WU1 can start
immediately; the others block on Brian's preparation). WU4 blocks on WU1.
WU5a blocks on WU1 + WU2a + WU2b. WU5b blocks on WU5a + WU4. WU5c blocks on
WU5a + Brian's tier arbitration. WU5d blocks on WU5a-c + WU4 + WU3 (ideal, not
hard blocking). WU6 blocks on WU5d.

WU3 feeds into WU5d if available but is not a hard blocker — the framework
evaluation benefits from planning evolution data but can proceed without it.

## Output artifacts

All synthesis outputs go in `docs/v3-framework/`. The synthesis plan itself stays
in `docs/ANALYSIS-SYNTHESIS-PLAN.md` as the governing document. Per-text v4
analyses from WU2a go in `source_material_references/Reading Archive Analyses/`
alongside the corpus analyses.

```
docs/v3-framework/
  WU1-corpus-synthesis.md
  WU2a-own-voice-synthesis.md
  WU2b-v1-scene-instincts/           (directory — per-arc structured files)
    arc-1-opening.md (or .csv)
    arc-2-command.md
    ...
    aris.md
    paratext.md
  WU3-planning-evolution.md
  WU4-retrospective.md
  WU5a-cross-corpus-comparison.md
  WU5b-hypothesis-adjudication.md
  WU5c-favorites-lens.md
  WU5d-framework-evaluation.md
  WU6-connection.md
```

File format is markdown unless the structured nature of WU2b's output makes CSV
or another tabular format more useful (determined when the WU2b skill is designed).

## Corpus notes

**Stories in the analysis corpus (112):** 59 short-corpus (cloud v4 analyses), 53
long-corpus (1M local analyses). Ground truth: `.claude/skills/analyze-story/
populations.md`.

**Unanalyzed stories:** 4 Tier 3 stories remain unanalyzed:
- green (Steel Resolve, 2,656 KB) — Brian read it; was in NLM Perspective Analysis;
  Wings of Dew draws from it. Large. Defer unless Brian prioritizes.
- fallout-equestria (3,509 KB) — Brian has NOT read it. Drop from active corpus.
- your-human-and-you (3,834 KB) — Brian read snippets early on, doesn't remember.
  Drop from active corpus.
- romance-reports (1,049 KB) — Abandoned by Brian; partial analysis may cover
  unread content. Keep the existing analysis for WU1; flag in favorites as Abandoned.

**Brian's own fiction (6 texts + naive chapters):**
- THLB, Wish, TEatS — from fanfiction.net via fichub epub (verify italics)
- NTL — from fanfiction.net via fichub epub (verify italics; unfinished)
- GIYC — from Fimfiction epub (unfinished)
- Falldale — from Fimfiction epub (finished)
- Naive TLTT Ch1-2 — already in `source_material_references/` as markdown

**Supplementary material:**
- `P&K comments.md` — 25 comments, chapter-by-chapter reader reactions
- `pax-chrysalia-comments.md` — 15 comments + author replies
- `Comments.md` — index + inline comments for other stories
- `Filly Fooling review.txt` — ~2,500-word essay with proposed structural revision
- `Filly Fooling analysis.csv` — chapter-by-chapter spreadsheet of three parallel arcs
- `The Princess and the Kaiser - Sheet1.csv` — unknown contents (Brian to confirm)

**Planning doc revision histories (for WU3):**
- TLTT — already in lineage (53 diffs, `gdoc:` ids)
- KU/NTL — Drive doc IDs above, to be exported via appscript
- GIYC — Drive doc ID above, to be exported
- Falldale — Drive doc ID above; planning doc "almost as long as the prose itself"

## What needs skills

- **WU2a:** Adapted analyze-story skill for own-fiction analysis (self-diagnostic
  framing, handle unfinished works, naive chapters as partial text)
- **WU2b:** New skill for v1 archive extraction (multiple subagents, batched by
  chapter arc, Gemini-voice separation via lineage grep, consistent categorization
  methodology)

Other WUs are single-session or ad-hoc.

## What blocks on Brian

- Complete favorites tier arbitration in `corpus-favorites-tiers.txt` (blocks WU5c)
- Download own fiction from Drive as epub, verify italics in fichub exports (blocks
  WU2a)
- Run appscript to export planning doc revision histories (blocks WU3)
- Add pending story entities to v2 (Daring Do, Applejack's Parents, etc.) — does
  not block any WU but informs WU6

## Downstream (after this plan completes)

These are hypotheses about what comes next, not commitments:

- **v3 framework decisions:** Brian reviews WU5d and WU6, decides which track
  definitions to add/modify/remove, what display questions to write, whether
  new cognitive modes or TrackTypes are needed
- **V1 archive migration:** With v3 tracks designed, migrate Brian's scene-level
  instincts from v1 into v3 .storyplan notes (in Brian's voice, not Gemini's —
  WU2b's voice separation enables this)
- **Conversation content migration:** Unmigrated fabula from conversations
  (Chrysalis, Aquileia, etc.) into v3 .storyplan
- **New story entities:** Story entities for planned stories not yet in v2
- **Voice linting protocol:** The v2 .storyplan contains notes copy-pasted from
  Gemini conversations during v1 that now exist in the lineage and conversations
  corpora (the sidecar databases). This copy-paste pattern was a structural
  consequence of the v1 paradigm: the entire .storyplan was exported as markdown
  (~940K chars) and pasted into Gemini's 1M context every session (reports W05-W07,
  Jan-Feb 2026). Brian then copy-pasted Gemini's insights back into the planner.
  This created a feedback cycle: AI reads plan → AI generates insight → Brian pastes
  insight into plan → AI reads its own prior insight next session. The MCP server
  paradigm is the structural inverse — the AI queries specific data on demand via
  tools, never ingests the full plan, and its analytical voice stays in its own
  transcript (conversations, code sessions) rather than entering the .storyplan.
  A linting pass could identify legacy copy-paste artifacts by grepping .storyplan
  note text against the lineage corpus, flagging matches, and either removing or
  rewriting them in Brian's voice. WU2b's Gemini-voice separation methodology is
  the prototype for this. The hypothesis: once the sidecars hold the AI analytical
  voice accessibly via MCP, the .storyplan notes should contain only Brian's
  fabula/architectural voice and prose/craft voice — the AI voice is retrievable
  but not embedded.
- **Own fiction analysis follow-up:** Use the self-diagnostic synthesis to inform
  specific writing practice decisions (Brian's domain)
- **Writing:** Eventually, write TLTT chapter 1. With a plan.
