# Consolidation 1 — Plan (2026-08-31)

## Purpose

This document proposes the unified hypothesis set for the v3 framework buildout,
consolidating three source documents into 45 hypotheses with stable IDs, slugs,
conceptual-hierarchy ordering, and merge/split decisions. It is reviewed before
any hypothesis files are created. After review, Brian tests the file format on a
small batch, then the remainder are created in stages.

The initial population uses the same creation protocol the skill defines for
future emergent hypotheses — the same file format, the same record conventions,
the same frontmatter. There is no special-case machinery for the first batch.
After the files exist and the index is populated, a forward plan follows.

This document is NOT the consolidation report (`consolidation-1.md`). The report
will record what was actually done when the hypothesis files are created. This
document is the design for that work.

## Sources and labeling

Three sources feed this consolidation. Each source item keeps its original label
for traceability in the "Sources" column:

1. **H-series (H1-H28)** — `docs/ANALYSIS-SYNTHESIS-PLAN.md`. Narrative design
   framework hypotheses. Written 2026-08-28/29. Gaps from merged pairs: H1/H2,
   H11/H12, H22/H23/H24, H25/H26. The section "Hypotheses about the planner
   itself" contains four un-numbered hypotheses cited as `Synth.§Purpose`,
   `Synth.§AIContradiction`, `Synth.§V2Stall`, `Synth.§VoicePrereq`,
   `Synth.§HallmarkWall`.

2. **D-series (D1-D20) and T-series (T1-T3)** —
   `docs/v3-framework/pipeline-hypotheses-raw.md`. Pipeline/apparatus hypotheses
   organized from a Google Keep dump. Status: organized only, uniterated.

3. **E-series (E1-E14)** — emergent hypotheses from the 2026-08-31 design
   conversation that produced the v3-buildout skill. Labeled here for
   traceability:
   - E1: Epistemic vocabulary applies to notes (replacing unset/confirmed/flagged)
   - E2: "Evidenced" for Layer 1 = canon compliance + materialist historicism
   - E3: Different layers have different evidence requirements
   - E4: V3's deepest contribution may be epistemological, not structural
   - E5: V1's natural workflow may have been closer to hypothesize-gather-iterate
   - E6: Natural working cadence is sweeps, not incremental; Audit mode wrong cadence
   - E7: Memory files should be pointers and policies, not state claims
   - E8: FEATURE-AUDIT assertions are hypotheses, not settled authority
   - E9: The epistemic framework itself needs testing against v1/v2 evidence
   - E10: AI voice in corpora needs deliberate per-voice arbitration
   - E11: V1 Gemini-voice text is hypotheses framed with finality
   - E12: Ideal steady state = v1 friction + v3 rigor + v2 goal support
   - E13: Claude's default behavior (collaborative professional, not executive report)
   - E14: Skills/instructions have multiple dimensions (input, approach, retrieval, output)

## Ordering criterion

**Conceptual hierarchy** — most foundational and meta hypotheses get lowest IDs.
A reader scanning the index top to bottom encounters the premises first, then
increasingly specific claims. This is comprehension order, NOT testing priority
(which is the forward plan's job, written separately later).

Ten tiers, from meta to specific:

| Tier | Range | Domain |
|------|-------|--------|
| A | 001-005 | Purpose and epistemology — why the planner exists and how knowledge works in it |
| B | 006-009 | Pipeline decomposition — the four-factor model and its testability |
| C | 010-013 | Model properties — what is intrinsic to different AI models |
| D | 014-018 | Instructions and environment — how instructional text, data architecture, and harness shape outcomes |
| E | 019-022 | Voice and interaction — how AI and author voices interact, separate, and should be treated |
| F | 023-027 | Framework architecture — the conceptual structure of what the planner tracks |
| G | 028-032 | Perspective and perception — specific techniques for managing reader perspective and perception gap |
| H | 033-037 | Goals, scope, and boundaries — what goal categories the framework recognizes and where its boundaries fall |
| I | 038-040 | Brian's practice — what Brian instinctively does as a writer |
| J | 041-045 | Planner instrument — how the planner implements the framework |

---

## Proposed hypothesis set

### Tier A: Purpose and epistemology (001-005)

Why the planner exists and how knowledge works in it. These are the premises
everything else rests on. Testing them means checking the epistemic framework
itself against the lineage evidence and historical working patterns.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 001 | planner-purpose-trajectories | Synth.§Purpose | The planner's purpose is managing trajectories for serialized multi-story publication — converting revision debt into design specifications validatable before prose — and the exact scope of "trajectories" is what the v3 synthesis informs. | untested |
| 002 | epistemic-method-provenance | E4, E9, E5 | The v3 framework's deepest contribution is epistemological (the hypothesize-gather-iterate cycle applied uniformly to narrative design), and this cycle may already have been v1's natural workflow before v2's prescriptive staging displaced it. | untested |
| 003 | epistemic-vocabulary-for-content | E1, E2, E3 | The v3 epistemic vocabulary (untested/evidenced/challenged + baselining) applies to v2 working-plan notes, replacing unset/confirmed/flagged, with layer-specific evidence requirements: Layer 1 fabula evidences against the negotiation between canon compliance and materialist historicist analysis; Layers 2-3 against internal consistency with lower layers; syuzhet against reader plausibility. | untested |
| 004 | working-cadence-sweeps | E6, E18 | The natural working cadence is thorough periodic sweeps rather than incremental note-by-note hygiene, and Audit mode's promotion-centered note-by-note design may encode the wrong cadence. | untested |
| 005 | recall-vs-evidence | E8 | No document or recall (FEATURE-AUDIT, memory files, design transcripts) carries intrinsic authority — all are hypotheses about what was true at their writing time, testable against current evidence. | untested |

**Notes on Tier A:**

- **001** functions as a design constraint more than an empirical prediction — it
  is testable (the planner could turn out to be needed for something other than
  trajectory management), but its role is primarily scoping. Brian labeled it
  "(hypothesis)" explicitly in the synthesis plan.

- **003 caveat (critical):** `Confirmed` retains its working meaning in the v1
  archive (review closed, disposition not recorded). The epistemic vocabulary
  replacement applies ONLY to v2 working-plan notes. The v1 archive's state
  vocabulary is a different system with no correspondence.

- **003 rationale will be rich.** The Layer 1 evidence requirements section needs
  the full hippogriff/seapony example (negotiating canon's Pearl of
  Transformation against materialist historicist reading), the open question of
  whether other constraint sources exist beyond canon and materialist historicism,
  and the backward check on how Brian arrived at materialist historicism as a
  constraint.

- **002** requires lineage evidence: was v1's capture-everything pattern actually
  hypothesize-gather-iterate in practice (before the vocabulary existed), or is
  that a retrospective reading?

---

### Tier B: Pipeline decomposition (006-009)

The four-factor model of AI-assisted planning. These hypotheses frame how to
think about the pipeline (model × data × instructions × harness) independently
from the narrative design framework.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 006 | four-factor-decomposition | D1, H25, H26 | The AI-assisted planning experience decomposes into four independently varying factors — model, data stream, instructions, and harness — that were historically confounded in each era change, and each factor independently improves analytical outcomes. | untested |
| 007 | version-labels-as-bookmarks | D2, D6 | V0-v3 are point-in-time configurations of the four pipeline factors, not paradigm shifts, and each era's outcomes should be retroactively decomposed along individual factor changes using dated lineage evidence. | untested |
| 008 | framework-vs-pipeline-separable | D3 | Designing the v3 narrative design framework (what the planner tracks) and designing the v3 operating pipeline (how model/data/instructions/harness compose) are related but separable tasks with different evidence bases and WU dependency chains. | untested |
| 009 | v3-tooling-decouples-factors | D4, D15 | V3 tooling (MCP for data/instruction decoupling, skills for instruction/model decoupling, open protocol for harness/model decoupling) makes the four-factor independence testable for the first time and enables retrospective review of prior AI-plan interactions. | untested |

**Notes on Tier B:**

- **006** merges D1 (the four-factor decomposition itself) with H25/H26 (four
  independent factors improving outcomes). Same decomposition, different
  granularity — D1 names the factors, H25/H26 claims each independently helps.

- D5 (testing methodology unresolved) is excluded as a task item, not a
  prediction. The forward plan will design the testing methodology when pipeline
  WUs are scoped.

---

### Tier C: Model properties (010-013)

What is intrinsic to different AI models after subtracting instructions and data.
These are all pipeline-level hypotheses — currently thin and uniterated from the
Google Keep dump. They may consolidate further as the pipeline work gets defined.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 010 | model-intrinsic-properties | D7 | After subtracting instructional scaffolding and data connectivity, models retain intrinsic properties (voice register, reasoning quality, tool-use affinity, capability ceiling) that are not fully reducible to instruction engineering. | untested |
| 011 | model-comparison-convergence | D8, D11, T1, T2 | The gap between models (Opus 4.6's voice vs Opus 5's agentic capability, Sonnet 4.6's historical out-of-box preference) may narrow as instructional scaffolding matures — with whether AI voice enters the .storyplan determining how much real-time session voice matters for model choice. | untested |
| 012 | constitutional-ai-constraint | D9 | Constitutional AI (Claude family) aligns better with the analytical rigor this work needs than attention-based RLHF (OpenAI/Gemini), constraining the practical model space to Claude family members with MCP as the portability valve. | untested |
| 013 | fable-role | D10 | Fable's value is primarily in open-ended work when the apparatus lacks definition (immature skills/instructions), and diminishes as instructional scaffolding matures. | untested |

**Notes on Tier C:**

- **011** merges four source items that are all aspects of one question: does model
  choice converge or diverge as the apparatus matures? D8 (Opus 4.6 vs 5), D11
  (Sonnet 4.6 preference), T1 (voice matters vs moot), T2 (instructions close
  all gaps vs irreducible properties).

- E13 (Claude's default behavior as collaborative professional) is not its own
  hypothesis — it is evidence for 010 and 012. The observation that "Sonnet/Opus
  reads closer to hypothesis proposals from a collaborative professional" while
  "Terra reads like wanting to deliver an executive report" is evidence about
  model-intrinsic properties and constitutional AI's effect on register.

---

### Tier D: Instructions and environment (014-018)

How instructional text, data architecture, harness choice, and workflow shape
outcomes. Spans from the principle of evidence-based design through the
operational questions about harness architecture.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 014 | evidence-based-instruction-design | D12, D14, E14, Synth.§Downstream | Instructions and skills should be iteratively crafted from evidence of what worked across 9 months of conversation history (not from first principles), the real paradigm shift is from "find a model that works out of box" to "build instructions that make any adequate model work," and the full instructional text stack (CLAUDE.md, skills, MCP instructions, project prompts) has multiple dimensions (what to ask, analytical approach, what to retrieve, output constraints) deserving the same craft as v1's custom gem. | untested |
| 015 | acceptance-signals | D13 | Copy-pasted AI text into the plan is a positive acceptance signal about output quality (analogous to RLHF reward), and other signals (Conversation Reader states, user corrections, endorsed vs rejected proposals) may be systematically mineable for evidence-based instruction design. | untested |
| 016 | data-source-unification | D16 | The bespoke per-corpus schemas (conversations, lineage, code sessions, source texts) might benefit from a common API standard for harness/model portability, but whether a conversational API shape fits non-conversational data (source texts, lineage doc diffs) is unresolved. | untested |
| 017 | desktop-vs-code-split | D17 | The Desktop-analyzes/Code-builds role separation remains the correct architectural boundary because story analysis and planner building require different session contexts, despite both consumers now sharing MCP and instructional infrastructure. | untested |
| 018 | target-usage-loop | D18, E12 | The target workflow pattern for using model + harness + data has been implicitly whatever the current tooling afforded (never explicitly defined), and the ideal steady state combines v1's low-friction high-throughput capture with v3's epistemic rigor and v2's author-goal support. | untested |

**Notes on Tier D:**

- **014** absorbs four source items (D12, D14, E14, and the Synth.§Downstream
  instructional text hierarchy) because they are all aspects of one claim:
  instructions matter, should be evidence-designed, and the paradigm shift is
  about building good instructions rather than finding a model that doesn't
  need them.

---

### Tier E: Voice and interaction (019-022)

How AI and author voices interact, how to separate them, and how to treat each
voice in the corpora. These hypotheses are structural prerequisites for the
narrative design framework — if the .storyplan contains mixed voice, the
framework inherits the confusion.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 019 | ai-context-contradiction | H22, H23, H24, Synth.§AIContradiction, Synth.§V2Stall | The fundamental v0-v2 struggle was the contradiction between needing AI context for architectural analysis and not wanting AI voice in the prose layer — v1's full-plan-paste feedback loop (~940K chars per session) accumulated AI voice in the .storyplan, v2's cognitive modes couldn't break it, and v3's MCP sidecar is the first architecture that reconciles this. | untested |
| 020 | voice-separation-prerequisite | H22, H23, H24, Synth.§VoicePrereq | Clean voice separation (Brian's voices in the .storyplan, AI voice in sidecars) is a structural prerequisite for the v3 framework — track architecture built on mixed-voice data inherits the confusion, so separation must precede or run alongside framework evolution, not follow it. | untested |
| 021 | five-voice-registers | H27 | Brian's voice has five distinct registers (prose/craft, fabula, syuzhet/design, Brian's analytical, AI analytical) rather than three, with the fabula/syuzhet split mapping to existing ZF/ND cognitive modes and the Brian-analytical/AI-analytical distinction mattering for mining, linting, and the favorites lens. | untested |
| 022 | voice-treatment-protocol | E10, E11 | Each AI voice in the corpora requires defined treatment rules: Gemini's RLHF-tuned finality in lineage is hypothesis-level not fact-level, Claude's formulations in conversations carry different authority from Brian's turns, and v1 text copy-pasted from Gemini needs epistemic recognition as unfinished proposals despite their air of settled certainty. | untested |

**Notes on Tier E:**

- **019** and **020** are both from H22/H23/H24 but make separable claims. 019
  is the historical diagnosis (the feedback loop existed and caused voice
  contamination). 020 is the forward prediction (you can't build v3 on
  mixed-voice data). Either could be refuted independently.

- D19 (copy-paste detection DataOp) and D20 (UI surfacing of AI voice) are
  excluded as implementation proposals downstream of 019/020 — they are
  candidate features for FEATURE-AUDIT evaluation, not framework hypotheses.

---

### Tier F: Framework architecture (023-027)

The conceptual structure of what the planner tracks. These hypotheses define the
framework's shape — how goals, mechanisms, and techniques relate; what axes the
framework has; how cognitive modes partition the work.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 023 | three-concern-separation | H18 | Three distinct concerns — goal (what the scene is FOR), mechanism (what the reader does), and prose technique (how it's written) — should be recognized separately by the planner, with the reader-experience-moment as the unit of designed reader operation at scene-level intersections. | untested |
| 024 | dimensional-vs-hierarchical | H18 | The three concerns (goal/mechanism/technique) are independent dimensions rather than a containment hierarchy — a reader-experience-moment occupies positions on all three simultaneously, like a civilizational system on the political axes, rather than nesting goal inside mechanism inside technique. | untested |
| 025 | mechanism-hierarchy-is-complexity | H5 | The mechanism hierarchy (M1-M4+) represents structural complexity — the degree of reader inference required — not narrative quality; higher mechanism does not mean better story. | untested |
| 026 | three-axis-independence | H8 | The three-axis model (mechanism × inference stage × rendering mode) is structurally independent (each axis can vary without the others) but semantically coupled (specific combinations produce distinct effects), and this independence should be preserved in the framework architecture. | untested |
| 027 | cognitive-mode-principle | H17 | The cognitive mode separation principle (ZeroFocalization vs NarrativeDesign — "what is true" vs "how the reader encounters truth") is correct as a principle, but the specific boundaries and mode definitions may need revision to accommodate the three-level framework and non-inferential goals. | untested |

**Notes on Tier F:**

- **023** and **024** are split from H18, which conflated two separable claims:
  (a) three concerns exist and should be recognized, and (b) how they relate
  structurally. The first is a prediction about the framework's ontology. The
  second is an open design question with multiple candidate answers.

---

### Tier G: Perspective and perception (028-032)

Specific techniques for managing reader perspective and perception gap. These
are the most directly testable against the 112-story corpus and Brian's own
fiction.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 028 | variable-focalization-master | H9 | Variable focalization (strategic shifts in narrative distance and perspective restriction) is the master perspective principle — FID is one mode in the toolkit, and the power comes from variation across the focalization spectrum, not from staying deep. | untested |
| 029 | perception-gap-delivery | H10 | Character-reader perception gap is the correct WorldInference target for experience design, with FID as one of several delivery mechanisms (not the privileged one) and conv 21's technique-agnostic gap taxonomy (ironic/tragic/closing/aligned) as the starting hypothesis for gap types. | untested |
| 030 | narrator-character-blend | H16 | The narrator-character blend ("as if roses were red" — the narrator's literary register channeling the focalizer's assessment) is a distinct technique not captured by the current ZF/ND cognitive mode split, requiring its own recognition as a mode or cross-cutting phenomenon. | untested |
| 031 | dt-knowledge-asymmetry | H3 | DT-based knowledge asymmetry (deep third revealing what a character knows that the reader doesn't, or vice versa) is a real technique adjacent to M4/dramatic irony, distinct enough from FID-delivered irony to warrant separate recognition. | untested |
| 032 | first-person-m4-effects | H4 | First-person narration produces M4-adjacent effects (narrator unreliability, gaps between what is reported and what the reader infers) through a different mechanism than FID, broadening the perception-gap delivery toolkit beyond third-person techniques. | untested |

---

### Tier H: Goals, scope, and boundaries (033-037)

What goal categories the framework recognizes, where the plannable/prose-craft
boundary falls, and how scope varies across the multi-story project.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 033 | non-thematic-goal-categories | H11, H12 | Theme is one goal category among peers (humor, structural setup, emotional investment, prior-belief establishment, and potentially others), with P→WI→T as the thematic pathway and other goals potentially following different pathways that bypass inference entirely; which non-thematic goals are plannable vs prose-craft is open. | untested |
| 034 | prose-craft-boundary | H6 | Comedy, atmosphere, and narrative voice are prose-craft (chosen at writing time, not planned in the planner) rather than framework-level categories, and the boundary between plannable goals and prose-craft needs evidence-based definition rather than theoretical line-drawing. | untested |
| 035 | embedded-text-category | H7 | Embedded texts and dreams sit outside the standard mechanism categories (M1-M4) as a distinct structural device that frames reader interpretation differently from the embedding narrative's own mechanism. | untested |
| 036 | wi-terminal-ratio | H1, H2 | WI-terminal links (World Inference serving structural purposes without reaching Thematic Evidence) are legitimate and expected to be common — P→WI→T is a special case of the more general P→WI→[T or structural purpose] pipeline, and the natural corpus-wide ratio is an open empirical question. | untested |
| 037 | multi-story-focalization-profiles | H13 | Different stories in the multi-story project need different variable-focalization mixes (Chrysalis's Greek tragedy ≠ TLTT's ensemble ≠ Kitty's bildungsroman), supported by one universal track architecture with variable usage density per story. | untested |

---

### Tier I: Brian's practice (038-040)

What Brian instinctively does as a writer, distinct from what the framework
prescribes. These hypotheses are tested primarily by WU2a (own fiction analysis)
and WU2b (v1 archive mining).

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 038 | instinctive-mechanism-practice | H14 | Brian already practices all mechanism types (M2, M3, behavioral proxy, structural parallels) instinctively and without formal vocabulary, as evidenced in the v1 archive and naive TLTT chapters. | untested |
| 039 | fim-reading-effect | H15 | Reading FiM fiction improved Brian's FID/variable-focalization instincts, testable by comparing the pre-FiM Pokemon stories against post-FiM texts (GIYC, Falldale, naive TLTT chapters) for technique profile changes. | untested |
| 040 | fabula-dialogue-replacement | H19 | The naive chapters' fabula-through-dialogue delivery is replaced by four identified techniques: behavioral evidence (reader observes and infers), designed incomplete understanding (POV character's limits generate inference), revelation architecture (when fabula enters awareness), and designed mistakes (reader's planned wrong inferences). | untested |

---

### Tier J: Planner instrument (041-045)

How the planner implements the framework — workflow modes, note relationships,
and the tension between bespoke and clean architecture.

| ID | Slug | Sources | Summary | Status |
|----|------|---------|---------|--------|
| 041 | sufficient-stability-iterative | H20 | The "sufficient stability" principle (scene design can proceed per scene as relevant subject-level content stabilizes) is correct but was never practiced in v2, and v3 should enable iterative subject↔scene work rather than sequential subject-then-scene gating. | untested |
| 042 | editor-modes-evaluation | H21 | The EditorModes (Expansion, Linking, Gardener, Audit, Scene Design) describe real cognitive directions that are approximately correct, but the specific mode designs (what's visible, what's writable, what's excluded) may not be optimal and should be evaluated against evidence. | untested |
| 043 | note-design-relationships | H28 | Note-to-note design relationships (setup→payoff, parallel, contradicts, revelation chain) may be needed for structurally managing serialized publication trajectories — distinct from the rejected supersession links (FEATURE-AUDIT C1), currently invisible in data, and supportable by existing codebase patterns (SubjectRelation model, NarrativePropertyValue). | untested |
| 044 | bespokeness-tension | T3 | V1's bespoke-but-vibe-coded approach produced scene-level planning density (rich scene graph, instinctive mechanism usage in notes) that v2's clean architecture could not replicate because scene-level work stalled — v3 may restore that density at v2's quality level, or may introduce its own tech-debt trap. | untested |
| 045 | keep-notes-provenance | E16 | Brian's Google Keep notes contain provenance material (early hypotheses, intuitions, corrections) not already captured in existing corpora (lineage, conversations, code sessions), and if so they warrant an ingest path into the lineage corpus. | untested |

**Notes on Tier J:**

- **044** should include the hallmark wall observation (Synth.§HallmarkWall) in
  its created entry: each version hit the same wall at a higher abstraction
  level — v0: fabula leaked into chapters; v1: architectural density exceeded
  free-form textboxes; v2: scene-level work stalled by narrow vocabulary + voice
  contamination. Whether the pattern is really "the same wall" or three unrelated
  problems is itself part of what 044 tests.

---

## Merge decisions

Major merges, with rationale. Each takes multiple source items into one
hypothesis because they make the same prediction at different granularity or
from different angles.

| Unified ID | Merged sources | Rationale |
|------------|---------------|-----------|
| 006 | D1 + H25 + H26 | Same decomposition — D1 names the four factors, H25/H26 claims each independently helps. One hypothesis: the factors exist AND independently matter. |
| 007 | D2 + D6 | Same claim: version labels are factor snapshots. D6 adds the retroactive decomposition method, which is the way to test D2. |
| 009 | D4 + D15 | Both about what v3 enables: D4 claims decoupling, D15 claims retrospective review. Both are consequences of the MCP/skills architecture. |
| 011 | D8 + D11 + T1 + T2 | All four are aspects of one question: does model choice converge or diverge as the apparatus matures? D8 and D11 name specific model pairs; T1 and T2 articulate the tension as two sub-hypotheses. |
| 014 | D12 + D14 + E14 + Synth.§Downstream | All about instruction design: D12 (evidence-based), D14 (paradigm shift), E14 (multiple dimensions), Synth.§Downstream (iterative craft on the full stack). |
| 019 | H22 + H23 + H24 + Synth.§AIContradiction + Synth.§V2Stall | The AI context contradiction and its consequences. Three numbered hypotheses and two un-numbered passages about the same feedback loop. |
| 003 | E1 + E2 + E3 | All about what the epistemic vocabulary means for content: E1 (it applies), E2 (Layer 1 specifics), E3 (per-layer differences). |
| 002 | E4 + E9 + E5 | All about the epistemic method's provenance: E4 (deepest contribution is epistemological), E9 (needs testing against evidence), E5 (v1 may have already practiced it). |
| 004 | E6 + E18 | Same observation: sweeps over incremental. E6 from the design conversation evidence; E18 from the Audit mode rethinking note. |

## Split decisions

| Original | Split into | Rationale |
|----------|-----------|-----------|
| H18 | 023 + 024 | H18 conflated two separable predictions: (a) three concerns exist and should be separately recognized, and (b) how they relate structurally (hierarchy vs dimensions vs graph). Either could be confirmed while the other is refuted. |
| H22/H23/H24 | 019 + 020 | The historical diagnosis (feedback loop existed, caused contamination) is a different claim from the forward prediction (v3 framework requires clean voice to function). The diagnosis could be confirmed while the prerequisite is found to be unnecessary, or vice versa. |

## Interpretive claims extracted from VERSION-HISTORY.md (2026-08-31)

The original VERSION-HISTORY.md (now VERSION-HISTORY-DRAFT1.md in the skill
folder) mixed verifiable facts with interpretive claims. The facts-only version
was created; the following interpretive claims need homes:

| Claim | Maps to | Action |
|-------|---------|--------|
| "Fabula leaking into syuzhet" as diagnosis of v0 wall | 040 (fabula-dialogue-replacement) | Context for 040's created entry |
| "NLM introduced vocabulary from Pokemon analysis, not from P&K — vocabulary predated P&K" | 002, 028 | Too narrow for its own hypothesis. Verification target for the retrospective WU; when checked against lineage, the finding becomes evidence deposited into 002 (Brian was already doing analytical work before formal vocabulary) or 028 (where focalization concepts entered). |
| "Aquileian lore overflow birthed the multi-story fabula paradigm" | None | Unverified causal claim. Context for forward plan (WU4 retrospective) |
| "v2 scene-level work stalled because FID prescription made scene design feel premature" | 019 (ai-context-contradiction) | Already in 019's scope |
| "The planner was a capture instrument, not a design instrument" (v1 characterization) | 002 (epistemic-method-provenance) | Context for 002's created entry |
| "v0=observation, v1=naming, v2=hypothesis, v3=experimentation" | 002 | Core of 002 — should NOT be stated as fact elsewhere |
| "v3 is the first time codebase and framework diverge" | Challenged by 007 (version-labels-as-bookmarks) | Should not be stated as fact |
| "The analysis pipeline is the first experiment" | Challenged by 002 (whether v1 was already experimenting) | Should not be stated as fact |
| Provenance table: 9 rows of "concept X first appeared at source Y on date Z" | None specifically | Unverified provenance claims. Each row is testable against lineage. Candidate for a WU4-scoped verification pass. Preserved in VERSION-HISTORY-DRAFT1.md. |

## Caveats

**V1 archive `Confirmed` (critical for 003).** Hypothesis 003 proposes replacing
unset/confirmed/flagged with untested/evidenced/challenged for v2 working-plan
notes. The v1 archive's `Confirmed` retains its existing meaning (review closed,
disposition not recorded — see CLAUDE.md data semantics). The v1 archive's state
vocabulary is a different system. The hypothesis file for 003 must state this
scope limitation in the hypothesis statement itself, not just in a caveat.

**All hypotheses start `untested`.** No evidence has been formally examined
against any of these predictions yet. Prior discussion in the source documents
(the synthesis plan's analysis of the FID provenance chain, the design
conversation's observations about cadence) is rationale for formulating the
hypotheses, not evidence deposited in the record format. Evidence comes from WU
execution under the forward plan.

**Pipeline hypotheses (Tiers B-D) are uniterated.** The D-series and T-series
came from a Google Keep dump organized in one pass. They may consolidate further
as pipeline work gets scoped. The initial 45-hypothesis count is a starting
point, not a floor.

## Context documents

The skill defines two companion files in `.claude/skills/v3-buildout/`. Both
already exist with content extracted from the synthesis plan. Inventory of what
each should contain and whether additions are needed:

### VERSION-HISTORY.md (rewritten 2026-08-31)

Now contains facts only: timeline with dates/counts/tools/architecture, key
conversation table, and an explicit "What is NOT in this document" section
listing interpretive claims that were removed and where they went. The original
mixed-content version is preserved as VERSION-HISTORY-DRAFT1.md.

The hallmark wall observation (Synth.§HallmarkWall) was NOT added — it is
interpretive, not factual. The three-fold goal (understand / evaluate / evolve)
was NOT added — it is framing that belongs in the skill's preamble or as
context for the forward plan, not in a facts document.

### CORPUS-STATUS.md (already populated)

Currently contains: 112-story corpus status, Brian's own fiction (WU2a
material), Supplementary material, Planning doc revision histories (WU3
material), Skills needed.

**Should add:**
- Other AI Studio accounts as potential lineage source. Brian's task item:
  "Import lineage from other AI Studio accounts." The current lineage ingest
  reads one AI Studio account. If other accounts hold relevant chats, the
  ingest config needs expanding.
- **Update WU numbering.** The "Skills needed" section references old WU
  numbers (WU2a, WU2b) from the synthesis plan. These will be renumbered in
  the forward plan. Defer the update until the forward plan assigns new numbers.

### Are more context documents needed?

No. The skill + VERSION-HISTORY + CORPUS-STATUS cover stable context.
Hypotheses, forward plans, consolidation reports, and WU artifacts are the
working instruments, defined by the skill's provenance section. A glossary of
framework vocabulary was considered but rejected — it would go stale; MCP
(`get_track_definitions`) is the live source. A decisions log was considered but
rejected — framework decisions go through the normal CLAUDE.md/FEATURE-AUDIT
process.

## Items requiring separate action

These items from the conversation notes are tasks, not hypotheses or context
document updates:

- **Reorganize folders in the story planner directory.** Brian's note: "Need to
  reorganize my folders in the story planner directory." A file-organization
  task, not framework work.
- **Import lineage from other AI Studio accounts.** A lineage ingest task.
  Noted in CORPUS-STATUS.md additions above.
- **Verify italics in fichub exports.** Blocks WU2a (own fiction analysis).
  Already tracked in CORPUS-STATUS.md.

## Next steps

After Brian reviews this plan:

1. **Pick 2-3 hypotheses for format testing.** Candidates that exercise different
   aspects of the file format:
   - One from Tier A (rich rationale, no evidence yet — tests the blank-record
     starting state)
   - One from Tier F or G (narrative design hypothesis — tests the kind of
     hypothesis that will be most common)
   - Optionally one from Tier B or C (pipeline hypothesis — tests whether the
     thinner pipeline hypotheses need a different level of detail)

2. **Create those hypothesis files** using the skill's file format. Review the
   format in practice: does the frontmatter work, is the statement/rationale
   split natural, does the empty record read correctly?

3. **Iterate the format** if needed based on the test batch.

4. **Create the remaining hypothesis files** in batches (by tier is natural).
   Create `hypotheses/INDEX.md` as the routing table.

5. **Write forward-plan-1.md** — the first experimental agenda, with WU specs
   referencing the new hypothesis IDs. This is a separate session.
