# Forward Plan 1 — Ordering Audit

Created: 2026-08-31
Status: **living document.** Initial audit complete 2026-08-31 (both passes run,
order derived); maintained under the amendment protocol below as specs change
and WUs complete.

This audit re-derives the execution ordering of forward-plan-1's remaining work
units from documented, individually challengeable judgments, replacing the
plan's asserted sequence. Motivation: the plan's ordering claims (preconditions,
enrichment chains) were authored conclusions that repeatedly reversed under
scrutiny during the 2026-08-31 ordering discussion. Method: blind evaluator
agents apply the criteria below to ordering-metadata-stripped WU spec cards;
the main session's work is mechanical (tabulation, closure, sort).

WU1.1 (Corpus Synthesis) and WU1.2 (Keep Notes Assessment) are complete; their
outputs exist as available evidence and constrain nothing. The audit covers the
eleven remaining WUs: 1.3, 1.4, 1.5, 1.7, 1.8, 1.9, 1.10, 1.11, 1.12, 1.13,
1.14.

## Evaluation criteria

The following was given verbatim to every evaluating agent, along with the WU
roster (titles + one-line questions) and the WU spec card(s) under evaluation —
cards stripped of all ordering metadata (Preconditions, Status, Scale,
parenthetical ordering notes). Agents were instructed to read nothing else.

> ### Consumption pass (first) — constitutive consumption
>
> WU B **constitutively consumes** WU A when B's stated question is unanswerable
> without A's output artifact existing — not merely aided, contextualized, or
> enriched by it. The measure is the question and scope as written, nothing else.
>
> Cross-WU references inside an evidence-sources list are claims to be classified,
> not facts to be accepted: for each, judge **constitutive** (the question is about
> or requires that output) vs **incidental** (listed as helpful context; the
> question stands without it).
>
> ### Enrichment pass (second) — enrichment edge
>
> The governing counterfactual: **if B executed before A, name the specific
> question B would have failed to ask.** If nothing nameable, the verdict is
> **none** — and none is a recorded finding, not a gap.
>
> An enrichment edge A → B exists only if BOTH can be named:
>
> (a) a **specific finding-type** A could produce, traceable to a question A's
>     spec explicitly poses — not a hypothetical byproduct (serendipitous
>     findings are handled at execution time by the wrap-up step, never by
>     ordering), and
> (b) the **specific spec content in B** — a scope question, a hypothesis test —
>     that finding would ADD TO or SHARPEN before B runs. A finding that would
>     only change how B's completed results are *interpreted* is not an edge:
>     interpretation belongs to the downstream synthesis WUs.
>
> Not sufficient: sharing a hypothesis ID; thematic relatedness; "would provide
> useful context."
>
> Rate every passing edge: **strong** (B's spec would gain multiple specs or a
> reshaped question) or **weak** (one marginal question among many). Evaluate
> each direction independently. If both directions pass at comparable strength,
> record **comparable** — do not manufacture a winner.
>
> If an edge is actually consumption-grade (B's question is unanswerable without
> A's output), flag it as a consumption-pass inconsistency instead of recording
> it as enrichment.
>
> ### Excluded from ordering entirely
>
> - Tooling preconditions (skills to build, ingests, preprocessing) — these are
>   the first tasks *inside* the WU that needs them, not ordering inputs.
> - Readiness, convenience, throughput, estimated duration.
> - The plan's own ordering metadata (Preconditions, Status, Scale, parenthetical
>   ordering notes, the execution-sequence section) — stripped from all inputs.
>
> ### Assembly rules (main session, mechanical)
>
> 1. Consumption-pass constitutive edges → transitive closure → pairs still
>    unordered go to the enrichment pass.
> 2. Cycles among enrichment edges: drop the weakest edge in the cycle by
>    recorded rating, documented. A cycle whose candidate-drop edges are rated
>    comparable is not auto-broken — it escalates to Brian as a judgment call.
> 3. Topological sort of all surviving edges.
> 4. Remaining ties broken by v3-buildout skill criteria, in order:
>    infrastructure hypotheses first (4), unblocking value (5), foundation
>    before application (6). Each tie-break documented. Skill criterion 3
>    (precondition blockers) gates execution timing only and never participates
>    in ordering.

## Consumption pass — verdicts (2026-08-31)

Eleven blind agents, one per WU, each returning which other WUs' outputs its WU
constitutively consumes. Verdicts transcribed verbatim.

### WU1.3 consumption verdict

**Constitutively consumes:** none (among remaining WUs). WU1.1 is constitutive
but complete, so it constrains nothing.

**Per-WU classification**
- **WU1.1 (complete): constitutive** — Several scope items are unanswerable as
  written without WU1.1's output: hypothesis 038's grounding checklist ("WU1.1
  identified seven perception-gap delivery mechanisms… look for these specific
  techniques"), the H030 spectrum placement ("the 112-story corpus shows a
  spectrum… Where does Brian's writing fall on this spectrum?"), the candidate
  design targets ("Candidate design targets from WU1.1"), and the obstacle-type
  breakdown "from the corpus analyses." Complete — constrains nothing.
- **WU1.2 (complete): no relation** — not mentioned anywhere in the card.
- **WU1.4: incidental** — every mention is a forward delegation of a parallel
  check to WU1.4, not a consumption of its output: "Also check the v1 archive
  (WU1.4) for whether TLTT's multi-focalizer scene graph shows…", "Check v1
  archive (WU1.4) for whether the Demonstration track notes…", "WU1.4 (v1
  archive — do the 1,125 links… show asymmetric access design…)". WU1.3's own
  question about Brian's fiction stands without any of these.
- **WU1.5: incidental** — "Lineage discusses this intertwining — WU1.5 should
  check" and "(c) WU1.5 — trace the hopepunk thesis lineage" are hand-offs of
  related questions to WU1.5, not requirements on WU1.3's inputs.
- **WU1.7: incidental** — "v2 working plan (WU1.7 — do the Character-Reader
  Perception Gap or Reader Opinion Plan notes show designed asymmetry…)"
  delegates the working-plan side of the asymmetric-interiority question to
  WU1.7. The counterargument scope item that touches the working plan is done
  by WU1.3 itself "via MCP" against live data — a raw corpus, not another WU's
  output artifact.
- **WU1.9: incidental (explicitly excluded)** — the card's own boundary:
  "**What it does NOT do:** Compare to the 112-story corpus (that is WU1.9)";
  the canon-virtues item likewise assigns the comparison "(d) WU1.9 — compare
  Brian's treatment against Salvation's and P&K's" outward.
- **WU1.8, WU1.10, WU1.11, WU1.12, WU1.13, WU1.14: no relation** — not
  mentioned in the card, and no judged dependency: WU1.3's evidence base is
  entirely raw local texts (`source_material_references/`), conversations, and
  lineage, none of which are WU outputs.

### WU1.4 consumption verdict

**Constitutively consumes:** none (among remaining WUs). WU1.1 (complete) is
constitutive but constrains nothing.

**Per-WU classification**
- **WU1.1 (complete): constitutive** — the scope as written embeds WU1.1's
  output throughout: "WU1.1 identified seven named perception-gap delivery
  mechanisms beyond FID — look for traces of these specific techniques
  (especially DT-based knowledge asymmetry, strategic opacity, and narrated
  denial)"; "three candidate design targets from WU1.1: (a) designed
  information architecture… (b) reader stance trajectory… (c) structural
  correspondence"; plus a dozen "the corpus shows/documents…" checks (Pinkie
  non-focalizer, dream-sequence comparanda list, Carrot Top Season stasis,
  conversation-as-resolution 4.2b/4.2c, content-rating elision, deliberate-
  breach absence). These specific check-items are unanswerable as written
  without the corpus synthesis existing. Output exists — no ordering constraint.
- **WU1.2 (complete): no relation** — not mentioned anywhere in the card.
- **WU1.5: incidental** — "Check lineage (WU1.5) for NLM's reasoning behind the
  suggestion." This is a pointer delegating the lineage check *to* WU1.5, not a
  requirement that WU1.5's output exist; WU1.4's own check ("Check v1 archive
  for whether the TwiJack perspective switch was designed") stands on the v1
  archive alone.
- **WU1.7: incidental / no consumption** — the AU-ambient-field passage says
  "Check v1 archive and v2 working plan for whether existing tracks are already
  doing this work"; it directs a raw-data check of the v2 plan via MCP, not
  consumption of WU1.7's output artifact. WU1.4's stated question is fully
  answerable without WU1.7 having run.
- **WU1.9: incidental (reversed direction)** — "The structured format serves
  WU1.9 (cross-corpus comparison can query patterns)": WU1.4 is the producer
  here, not the consumer.
- **WU1.13: incidental (reversed direction)** — "The framework question for
  WU1.13: should the planner support marking designed perspective breaches…":
  a question handed downstream, not an input consumed.
- **WU1.3, WU1.8, WU1.10, WU1.11, WU1.12, WU1.14: no relation** — not mentioned
  in the card, and no consumption relation judged.

Notes for the record: the "downstream voice linting protocol (implementation
candidates)" and the hypothesis files (019–022, 038, 043, 044) are referenced
but are not WU outputs; the dated Google Drive v1 snapshots are explicitly
marked "an enhancement, not a precondition" by the card itself. All evidence
sources in the card's evidence-sources list are raw corpora (v1 archive via
MCP, lineage), not other WUs' outputs.

### WU1.5 consumption verdict

**Constitutively consumes:** WU1.3 (among remaining WUs). Also WU1.1 —
constitutive but complete, so it constrains nothing.

**Per-WU classification**
- **WU1.1: constitutive (complete — constrains nothing).** The WU's core
  question ("where is it overfit?") is defined as assessment against WU1.1's
  output: scope says "For each, assess against the corpus evidence from WU1.1:
  confirmed, partially validated, overfit, or untested," and chains 1, 4, 6, 7,
  8 each pose "does the corpus evidence from WU1.1 support…" questions. The
  overfit half of the stated question is unanswerable without this artifact.
  Output exists; no ordering constraint.
- **WU1.3: constitutive.** Chain 4 (M2 provenance) directs: "compare that
  reasoning against both the 112-story corpus evidence (WU1.1 …) and Brian's
  own fiction (WU1.3). Is the M2 vocabulary accurate to how prior-belief work
  actually operates in the stories Brian reads and writes…" The "and writes"
  half of that scope question requires an analysis of Brian's fiction, which
  WU1.5 does not itself produce — the card names WU1.3's output as that
  comparand, in the same grammatical construction as the clearly-constitutive
  WU1.1 reference. Without WU1.3's artifact, chain 4's write-side assessment is
  unanswerable, not merely unenriched. Note the dependency is narrow: it
  grounds only chain 4's accuracy comparison, not the rest of the card.
- **WU1.12: incidental (boundary reference, not consumption).** "What it does
  NOT do: Propose framework replacements (that is WU1.12)" — a demarcation of
  scope handed downstream; WU1.5 consumes no WU1.12 output.
- **WU1.4: no relation.** Chain 3 asks whether v1 was hypothesize-gather-
  iterate in practice, but sources it from "the lineage evidence," not from
  WU1.4's mining output; WU1.4 is never cited. The "dated v1 database
  snapshots" evidence line is preprocessing ("If preprocessed…"), which the
  criteria exclude as a tooling precondition inside the WU, and is in any case
  not WU1.4's output.
- **WU1.7, WU1.8, WU1.9, WU1.10, WU1.11, WU1.13, WU1.14: no relation.** None is
  mentioned in the card's evidence sources or scope, and no scope question
  depends on their outputs.

All other evidence sources in the card (NLM notebooks, AI Studio session,
conversations 8/17/21/36/47/64, Google Doc revision history, Keep notes,
VERSION-HISTORY-DRAFT1.md's provenance table) are pre-existing corpora or
documents, not WU outputs, so they create no cross-WU edges.

### WU1.7 consumption verdict

**Constitutively consumes:** none (among remaining WUs)

**Per-WU classification**
- WU1.1 Corpus Synthesis (complete): **constitutive** — scope question 5 is
  framed directly on its output: "The corpus shows dramatic irony (reader ahead
  of characters) as the dominant information architecture. How does the current
  v2 track setup handle this?" Without the corpus-synthesis finding, "this" has
  no referent and question 5 as written cannot be posed. Complete, so it
  constrains ordering nothing; recorded for the record.
- WU1.2 Keep Notes Assessment (complete): **no relation** — not mentioned
  anywhere in the card.
- WU1.12 Hypothesis Adjudication: **incidental / no consumption relation** —
  appears only as a scope exclusion: "What it does NOT do: Evaluate the
  framework (that is WU1.12)." A downstream hand-off pointer, not an input;
  WU1.7's question stands entirely without WU1.12's output.
- All other remaining WUs (1.3, 1.4, 1.5, 1.8, 1.9, 1.10, 1.11, 1.13, 1.14):
  **no relation** — none is named in the card's evidence sources or scope. The
  evidence-sources list is exclusively live-plan MCP queries (`get_stats`,
  `count_notes_plan`, `get_track_definitions`, `list_subjects`,
  `list_stories`, `get_chapters_plan`, `get_notes_plan`), which are tooling,
  not WU outputs. Shared hypothesis IDs (003, 004, 041, 042) with other WUs do
  not constitute consumption under the criteria. The CLAUDE.md zero-Confirmed
  reference is a project document, not a WU artifact.

WU1.7 is a self-contained data survey of the live v2 plan; its only genuine
dependency is on the already-complete WU1.1, via scope question 5's premise.

### WU1.8 consumption verdict

**Constitutively consumes:** none

**Per-WU classification:**
- WU1.3 (Own Fiction Analysis): **incidental** — mentioned only as a boundary
  demarcation: "What it does NOT do: Analyze the prose (that is WU1.3)." This
  is an explicit scope *exclusion*, the opposite of consumption; WU1.8's
  question about planning-document evolution stands with no WU1.3 output in
  existence.
- WU1.1 (Corpus Synthesis, complete): **no relation** — not mentioned anywhere
  in the card's evidence sources or scope text; and being complete it would
  constrain nothing regardless.
- WU1.2 (Keep Notes Assessment, complete): **no relation** — likewise
  unmentioned in the card; constrains nothing regardless.
- All other remaining WUs (1.4, 1.5, 1.7, 1.9, 1.10, 1.11, 1.12, 1.13, 1.14):
  **no relation** — none is named or implied in the card. Every listed evidence
  source is raw or pre-existing material, not a WU output: "TLTT revision
  history (already in lineage: 53 diffs, `gdoc:` ids)" and three sets of "raw
  exports in `Planning_Document_Revision_History/`". Hypothesis IDs
  002/004/018/039 are shared hypothesis references, which the criteria
  explicitly rule insufficient to create an edge.

Rationale in one line: WU1.8's stated question — how thinking evolved across
each story's planning lifecycle — is answerable entirely from revision
histories that already exist as raw artifacts, so no other WU's output is
constitutive.

### WU1.9 consumption verdict

**Constitutively consumes:** WU1.3, WU1.4 (also WU1.1, but it is complete and
constrains nothing)

**Per-WU classification**
- **WU1.1: constitutive (complete — constrains nothing).** The question's first
  leg, "what do stories Brian reads do," is exactly WU1.1's corpus-patterns
  output; the evidence list cites "WU1.1 (corpus patterns)" and axis 1 compares
  against "the 112 analyzed stories." The leg is unanswerable without that
  artifact — but WU1.1's output already exists, so this imposes no ordering
  constraint.
- **WU1.3: constitutive.** The question's second leg, "what does Brian
  instinctively do," is WU1.3's stated output. Axes 1–3 each bind to it
  explicitly ("that Brian also uses instinctively (from WU1.3 and WU1.4)",
  "based on WU1.3 and WU1.4", "What does Brian do instinctively (from WU1.3 and
  WU1.4)"), and the hypothesis-037 comparison requires "technique profiles
  across Brian's stories," which is WU1.3's artifact. Without it, one of the
  three corpora being compared does not exist as a describable term — the
  question as written cannot be answered.
- **WU1.4: constitutive.** The question's third leg, "what did v1 capture," is
  WU1.4's stated output, and axis 4 depends on a specific WU1.4 deliverable by
  name: "From WU1.4's voice attribution, how does Brian's instinctive
  scene-level practice… relate to the corpus patterns?" No voice-attribution
  artifact, no axis 4 — and no third corpus term for the convergence/divergence
  question.
- **WU1.12: no relation (scope exclusion).** Mentioned only negatively: "Test
  hypotheses systematically (that is WU1.12)." WU1.9 consumes nothing from it.
- **WU1.11: no relation (scope exclusion).** Mentioned only negatively: "Apply
  the favorites lens (that is WU1.11)." Nothing consumed.
- **WU1.7: no relation.** The Celestia passage names "v2 working plan, lineage
  discussions" as raw data to consult, not WU1.7's output artifact (state
  distributions / track usage / mode adoption). WU1.9's question stands without
  WU1.7's report, so no consumption edge.
- **WU1.5, WU1.8, WU1.10, WU1.13, WU1.14: no relation.** Not mentioned in the
  card, and nothing in the question or scope requires their outputs.

### WU1.10 consumption verdict

**Constitutively consumes:** none

**Per-WU classification**
- WU1.1 (complete): no relation — the card never cites the Corpus Synthesis
  output; the framework-side evidence base invoked for hypothesis 008
  ("examining whether the evidence bases for framework and pipeline hypotheses
  overlap") is a comparison of evidence *bases*, examinable from the hypothesis
  files and raw corpora the card names, and constrains nothing regardless since
  WU1.1 is complete.
- WU1.2 (complete): no relation — not mentioned anywhere in the card.
- WU1.3, WU1.4, WU1.5, WU1.7, WU1.8, WU1.9, WU1.11: no relation — none is named
  in the card's evidence sources or scope text; every evidence source is a raw
  corpus or configuration artifact ("Lineage corpus", "Code sessions
  (`codesessions.db` via sqlite3)", "Session transcripts", "Configuration
  evidence", "VERSION-HISTORY.md").
- WU1.12: no relation (downstream, not consumed) — the card's "What it does NOT
  do" places implementation "downstream of the evidence-gathering this WU
  does"; adjudication consumes WU1.10, not the reverse.
- WU1.13, WU1.14: no relation — not mentioned; both are downstream synthesis
  per the roster's framing.

Notes for the record: the two "candidate model-intrinsic properties observed
during the forward-plan-1 ordering session (2026-08-31)" reference a session
observation, not any WU's output artifact — incidental context, not
consumption. Sub-question 2's "Sonnet 4.6's documented historical preference
(009)" cites a hypothesis ID, which the criteria explicitly rule insufficient.
WU1.10's question as written is answerable entirely from the pre-existing
corpora it names.

### WU1.11 consumption verdict

**Constitutively consumes:** WU1.9

**Per-WU classification**
- **WU1.9: constitutive** — Not merely listed as an evidence source: the
  scope's defining operation is "Overlay favorites tiers onto WU1.9's
  cross-corpus findings." Question 1 (tier clustering) asks whether favorites
  cluster around "particular mechanism profiles, perspective techniques, or
  obstacle architectures" — the per-story cross-corpus substrate those tiers
  are overlaid on *is* WU1.9's output. As scoped, the tier-clustering question
  has nothing to overlay onto without that artifact existing; that is
  unanswerability, not enrichment.
- **WU1.1: constitutive (complete — constrains nothing)** — Questions 2 and 3
  require the formal analyses to compare against: comments must map to "the
  same patterns the v4 Brief formally identifies," and Special-tier context
  "must be read alongside their v4 analyses." Without WU1.1's outputs the
  comment-mapping and Special-tier questions are unanswerable. Recorded for the
  record only; WU1.1 is complete.
- **WU1.2: no relation** — not mentioned anywhere in the card.
- **WU1.3: no relation** — the card's "Naive TLTT chapters 1-2 (as evidence of
  v0 prose instincts)" is a raw-source citation of the chapters themselves, not
  WU1.3's analysis report; nothing in the question or scope requires WU1.3's
  output.
- **All other remaining WUs (1.4, 1.5, 1.7, 1.8, 1.10, 1.12, 1.13, 1.14): no
  relation** — none is mentioned in the card, and no scope question depends on
  their outputs.

Note: the hypothesis-021 "register 4" citation is a shared-hypothesis
reference, which the criteria explicitly exclude as an ordering consideration;
it creates no edge.

### WU1.12 consumption verdict

**Constitutively consumes:** WU1.3, WU1.4, WU1.5, WU1.7, WU1.8, WU1.9, WU1.10,
WU1.11

**Rationale for the blanket edge:** The stated question — "What does **ALL**
the evidence say about each hypothesis" — is definitionally about the totality
of evidence deposited by the evidence-gathering WUs. The card's evidence-
sources line names "All prior WU outputs," and the scope operationalizes it:
"For each hypothesis, synthesize the evidence from all WUs that produced
relevant findings" and "This WU reads those entries [deposited by prior WUs]
and performs a set-level synthesis — it does not re-derive the evidence." If
any evidence-producing WU has not run, its evidence does not exist and 1.12's
question as written cannot be answered — hypotheses would be misclassified as
"Insufficient evidence" or "Untouched" not as a finding but as an artifact of
ordering. (The "Untouched" category does not soften this: it is defined as a
coverage flag — "should be rare if the plan's coverage is working" — for
hypotheses no WU targeted, not a tolerance for unexecuted WUs; the card's
premise is that "by this point" prior WUs have deposited entries.)

**Per-WU classification:**
- WU1.3: constitutive — evidence-producing WU covered by "synthesize the
  evidence from all WUs that produced relevant findings"; its absence leaves
  hypothesis assessments unanswerable as posed.
- WU1.4: constitutive — same ground ("All prior WU outputs" / set-level
  synthesis over deposited entries).
- WU1.5: constitutive — same ground.
- WU1.7: constitutive — same ground.
- WU1.8: constitutive — same ground.
- WU1.9: constitutive — same ground; itself a comparison WU, but it produces
  findings 1.12's "ALL the evidence" must include.
- WU1.10: constitutive — same ground; the pipeline hypotheses are inside "ALL
  (001-045)".
- WU1.11: constitutive — same ground.
- WU1.13: no relation (from 1.12's side) — not mentioned in the card; 1.13 is
  downstream synthesis ("based on all the evidence gathered"), not an evidence
  source for 1.12.
- WU1.14: no relation — not mentioned in the card; downstream of 1.13.
- WU1.1 (complete): constitutive in character — its output is part of "All
  prior WU outputs" the synthesis reads; complete, so it constrains nothing.
- WU1.2 (complete): constitutive in character — same ground; complete,
  constrains nothing.

Non-WU references, for the record: the hypothesis files in
`docs/v3-framework/hypotheses/` and `FEATURE-AUDIT.md` are standing artifacts,
not work-unit outputs — outside the ordering question.

### WU1.13 consumption verdict

**Constitutively consumes:** WU1.3, WU1.4, WU1.5, WU1.7, WU1.9, WU1.12

**Per-WU classification**
- **WU1.12: constitutive** — The card's evaluation areas are framed as
  hypothesis verdicts ("Does the evidence support three separable concerns
  (hypotheses 023, 024)?", "hypotheses 025, 026, 028…"), and WU1.12's stated
  output — what ALL the evidence says about each hypothesis — is the direct
  substrate for those area questions. A question phrased "based on all the
  evidence gathered" over an 18-hypothesis roster cannot be answered as scoped
  without the adjudication artifact; without it WU1.13 would have to *be*
  WU1.12.
- **WU1.9: constitutive** — Listed as "(cross-corpus patterns)", and the
  scope's track-coverage test ("Which existing tracks hold up against the
  evidence?") and candidate-design-target evaluation hinge on whether corpus-
  documented patterns are confirmed in Brian's own practice — precisely the
  corpora-convergence output WU1.9 produces. The scope's self-description,
  "the convergence of all evidence into framework-level findings," consumes
  that comparison rather than reperforming it.
- **WU1.5: constitutive** — Area 3 asks "Which are overfit (Track 99's FID
  specification)?" — this is WU1.5's stated question verbatim ("where is it
  overfit?"). The evaluation area is *about* WU1.5's output; without it the
  overfit verdicts the area weighs do not exist.
- **WU1.7: constitutive** — Area 2's "What does iterative subject-to-scene work
  look like in practice? Do the EditorModes' visibility and writability rules
  support or hinder this?" and area 3's "WI-terminal patterns — should the
  'every link must have T' rule be relaxed?" are unanswerable without the
  working-plan usage/state/mode-adoption data that is WU1.7's output artifact.
- **WU1.4: constitutive** — Two scope evaluations are explicitly conditioned on
  WU1.4's findings: "if WU1.4 confirms Chrysalis's designed stasis is a real
  pattern, the Character Development track (id:6) may need its definition
  broadened," and the candidate design targets "if confirmed by WU1.3/WU1.4."
  A conditional gated on WU1.4's verdict cannot even be resolved without
  WU1.4's output existing.
- **WU1.3: constitutive** — Same clause: candidate design targets from WU1.1
  are evaluated only "if confirmed by WU1.3/WU1.4 as things Brian actually
  designs." The confirmation of Brian's practice is WU1.3's output; the
  evaluation cannot proceed without it. (Not in the evidence-sources list, but
  the scope text requires it — also reachable transitively through WU1.9.)
- **WU1.8: incidental** — The card itself marks it "(planning evolution, **if
  available**)" — by its own wording WU1.13's question stands without this
  artifact.
- **WU1.11: incidental** — Listed as "(favorites lens)" in evidence sources
  only; no evaluation area in the scope references favorites, reactions, or
  what sticks with Brian. The question as written stands without it — it is
  helpful context, not a requirement.
- **WU1.1: constitutive, but complete — constrains nothing** — The scope
  repeatedly rests on the corpus study ("the corpus shows…", "the three axes
  from the earlier corpus work", "Candidate design targets from WU1.1"). Its
  output exists; no ordering constraint.
- **WU1.2: no relation** — Not mentioned anywhere in the card.
- **WU1.10: no relation** — Not mentioned in evidence sources or scope text.
- **WU1.14: no relation** — Not mentioned in the card (the roster suggests it
  consumes WU1.13, the reverse direction — not judged here).

Assembly note (evaluator's own): WU1.12 itself claims "ALL the evidence," so
several of these edges (WU1.3/4/5/7/9 → WU1.13) will likely also arrive via
transitive closure through WU1.12; recorded as direct because the card's scope
text requires each artifact by name or by verbatim question.

### WU1.14 consumption verdict

**Constitutively consumes:** WU1.13

**Per-WU classification**
- **WU1.13: constitutive** — The stated question is *about* WU1.13's output:
  "What do the framework evaluation findings mean for TLTT" presupposes that
  the framework evaluation exists. It is the first listed evidence source
  ("WU1.13 framework evaluation"), and the scope references it directly and
  repeatedly: item 5 asks "Does the framework evaluation confirm the planner's
  purpose," and the whole scope opens with "Connect the framework evaluation to
  the actual stories Brian is planning." With no WU1.13 output there is no
  object for the question — unanswerable, not merely unenriched.
- **WU1.1 (complete): constitutive in character, constrains nothing** — Several
  scope items are questions about corpus findings: item 3 "What do the corpus
  patterns in each paradigm suggest," item 6 "The corpus identified five stable
  thematic territories," item 7 "The corpus universally uses early-season
  characterization as baseline," item 1 "mechanism profiles [of] Brian's
  favorites." These require the corpus synthesis output to exist — but WU1.1 is
  complete, so this imposes no ordering constraint.
- **WU1.7: incidental** — Scope item 2 asks "Where does the v2 data already
  contain scene-level design? Where is it thin?" — but the card's evidence
  source for this is the raw data itself ("V2 working plan via MCP (stories,
  subjects, tracks, plot points, links, notes)"), not WU1.7's assessment
  document. WU1.14 can query the v2 plan directly; WU1.7's report would aid,
  not enable. Not listed as an evidence source; the question stands without it.
- **WU1.11: incidental** — Item 1's "What mechanism profiles do Brian's
  favorites in the same paradigm exhibit?" touches WU1.11's territory
  (favorites × technique patterns), but the mechanism profiles of favorite
  stories are products of the completed corpus analyses (WU1.1), and WU1.11 is
  not cited in the evidence-sources list. The question as written is answerable
  from corpus evidence; WU1.11's correlation finding would enrich, not enable.
- **WU1.12: no relation** — Not mentioned anywhere in the card. Shared
  hypothesis IDs (001, 037, 044 appear in the hypotheses line) are explicitly
  insufficient under the criteria. Any dependence on adjudication reaches
  WU1.14 only transitively through WU1.13, which is the assembly step's
  business, not a direct edge from this card.
- **WU1.2 (complete): no relation** — Not mentioned in the card; constrains
  nothing regardless.
- **WU1.3, WU1.4, WU1.5, WU1.8, WU1.9, WU1.10: no relation** — None is named or
  implied in the card's question, evidence sources, or scope text. The phrase
  "grounded in corpus evidence and Brian's own instincts" in the output line
  gestures at territory WU1.3 studies, but it names no WU1.3 artifact and the
  seven scope questions are each answerable from WU1.13 + the completed corpus
  + live v2 data; that is thematic relatedness, below the bar.

## Consumption pass — assembly (mechanical)

### Constitutive edge set (A → B: B consumes A's output; A must precede B)

| Edge | Source verdict |
|---|---|
| WU1.3 → WU1.5 | WU1.5's evaluator (provenance chain 4's own-fiction comparand) |
| WU1.3 → WU1.9 | WU1.9's evaluator (second comparison leg) |
| WU1.4 → WU1.9 | WU1.9's evaluator (third comparison leg + voice-attribution axis) |
| WU1.9 → WU1.11 | WU1.11's evaluator (overlay substrate) |
| WU1.3, 1.4, 1.5, 1.7, 1.8, 1.9, 1.10, 1.11 → WU1.12 | WU1.12's evaluator (set-level synthesis over all deposited evidence) |
| WU1.3, 1.4, 1.5, 1.7, 1.9, 1.12 → WU1.13 | WU1.13's evaluator (direct scope requirements; several also transitive via 1.12) |
| WU1.13 → WU1.14 | WU1.14's evaluator (the question is about 1.13's findings) |

### Divergences from the plan's claimed dependencies (for Brian's review)

1. **WU1.5's dependencies inverted.** The plan hard-required WU1.4 (plus Keep
   ingest) and listed WU1.3 as "benefits from." The blind verdict: **WU1.3 is
   constitutive** (chain 4 names its output as comparand) and **WU1.4 is no
   relation** (chain 3 sources from lineage, not the mining output). The Keep
   ingest is tooling inside WU1.5 per the criteria.
2. **WU1.12's blanket edge is stricter than the plan.** The plan marked WU1.3,
   WU1.8, WU1.10 "ideal but not hard blocking." The blind verdict reads "ALL
   the evidence" literally: every evidence-producing WU is constitutive —
   which makes WU1.8 (gated on Brian's preprocessing) a hard blocker for the
   1.12→1.13→1.14 tail unless WU1.12's spec is revised to tolerate absent WUs.
3. **WU1.13's direct edges to 1.3/1.4/1.5/1.7/1.9** are mostly redundant with
   transitivity through WU1.12 but were recorded as direct because the scope
   text requires each artifact by name.

### Transitive closure — settled order relations

- WU1.3 precedes: 1.5, 1.9, 1.11, 1.12, 1.13, 1.14
- WU1.4 precedes: 1.9, 1.11, 1.12, 1.13, 1.14
- WU1.5 precedes: 1.12, 1.13, 1.14
- WU1.7 precedes: 1.12, 1.13, 1.14
- WU1.8 precedes: 1.12, 1.13, 1.14
- WU1.9 precedes: 1.11, 1.12, 1.13, 1.14
- WU1.10 precedes: 1.12, 1.13, 1.14
- WU1.11 precedes: 1.12, 1.13, 1.14
- WU1.12 precedes: 1.13, 1.14
- WU1.13 precedes: 1.14

33 of the 55 unordered pairs are settled by these relations.

### Residual pairs — to the enrichment pass (22)

| # | Pair | | # | Pair |
|---|------|---|---|------|
| 1 | 1.3 – 1.4 | | 12 | 1.5 – 1.11 |
| 2 | 1.3 – 1.7 | | 13 | 1.7 – 1.8 |
| 3 | 1.3 – 1.8 | | 14 | 1.7 – 1.9 |
| 4 | 1.3 – 1.10 | | 15 | 1.7 – 1.10 |
| 5 | 1.4 – 1.5 | | 16 | 1.7 – 1.11 |
| 6 | 1.4 – 1.7 | | 17 | 1.8 – 1.9 |
| 7 | 1.4 – 1.8 | | 18 | 1.8 – 1.10 |
| 8 | 1.4 – 1.10 | | 19 | 1.8 – 1.11 |
| 9 | 1.5 – 1.7 | | 20 | 1.9 – 1.10 |
| 10 | 1.5 – 1.8 | | 21 | 1.10 – 1.11 |
| 11 | 1.5 – 1.9 | | 22 | 1.5 – 1.10 |

(Notable: 1.5 – 1.9 is residual — neither evaluator found consumption in either
direction, so the retrospective-vs-comparison order is an enrichment question.
Likewise 1.3 – 1.4: the two discovery WUs have no constitutive relation.)

## Enrichment pass — verdicts (2026-08-31)

Twenty-two blind agents, one per residual pair, each receiving the criteria,
the roster, and the two stripped cards. Verdict summaries below preserve each
agent's stated (a)/(b) grounds; full transcripts in the session record.

### Pairs resolving to NO edge in either direction (8)

- **1.3 – 1.10: none.** Hypothesis sets fully disjoint (028-040 vs 006-018);
  craft findings touch no pipeline sub-question and vice versa; the only links
  are excluded tooling/interpretation effects.
- **1.7 – 1.8: none.** Shared hypothesis 004 is parallel evidence-gathering on
  disjoint corpora (v2 live data vs pre-v2 revision histories); convergence
  belongs to WU1.12, not ordering.
- **1.7 – 1.9: none.** WU1.9's axes are defined entirely over WU1.1/1.3/1.4
  outputs; WU1.7's distributional findings add no comparison question, and
  WU1.9's findings add no survey question to a spec pinned to hypotheses
  003/004/041/042.
- **1.7 – 1.10: none.** Disjoint hypothesis sets and evidence bases; the
  hypothesis 008 separability test is defined over evidence-base populations,
  not over WU1.7's survey results.
- **1.7 – 1.11: none.** Plan-internal data shape vs external-corpus reader
  response; hypothesis sets don't overlap; cross-effects are interpretation
  only.
- **1.9 – 1.10: none.** The one bridging hypothesis (008) is tested inside
  WU1.10 by spec-level comparison of evidence bases, not by consuming WU1.9's
  convergence results.
- **1.10 – 1.11: none.** Pipeline findings touch no favorites-lens scope
  question; reader-response findings touch no pipeline sub-question
  (acceptance signals concern AI proposals in the planning pipeline, a
  different evidence family from reactions to published stories).
- **1.5 – 1.9: comparable (no constraint).** Weak edges BOTH directions through
  the same hopepunk/canon-virtue-as-trap content: WU1.5's chain-12 trace would
  reformulate WU1.9's "does his subversion use a different mechanism?"
  question, and WU1.9's mechanism-level finding would tell chain 12 what
  relation to seek the origin of. Each sharpens exactly one question in the
  other; no winner manufactured.

### Pair resolving comparable (no constraint) — second instance

- **1.8 – 1.9: comparable (no constraint).** Weak edges both directions:
  WU1.8's dated hypothesis-039 shift would anchor WU1.9's Celestia
  instinctive-vs-informed question; WU1.9's hypothesis-037 per-story profile
  differences would add a deliberateness probe to WU1.8's perspective tracing.
  Same marginality both ways. (This verdict dissolved the only potential cycle
  — see assembly.)

### Pairs resolving to a directed edge (13)

- **1.3 → 1.4 (STRONG).** Three nameable mining questions in WU1.3's spec are
  directed at WU1.4 and absent from WU1.4's own card: the recurring
  behavioral-proxy vocabulary search, the Demonstration-track
  bond-evidence-vs-declaration check, and the asymmetric-interiority link
  check. WU1.4 is a read-once pass over all notes — a question not asked
  during the pass is not recorded, so this is scope addition, not
  interpretation. Reverse: none (emergent categories are wrap-up-step
  serendipity; v1-specific designs touch only the naive chapters, which
  predate v1).
- **1.4 → 1.5 (STRONG; weak reverse dropped).** WU1.4's voice-attribution map
  gives chain 10 concrete note↔conversation pairs; the cross-scene
  architecture finding sharpens chain 11's founding-motivation test; the
  TwiJack design status makes the NLM-breach reasoning question askable.
  Reverse (weak, dropped as the weaker of a 2-cycle): chain 2's vocabulary
  dating as one extra attribution heuristic.
- **1.3 → 1.7 (weak).** The shape of Brian's cross-focalizer asymmetry in
  prose sharpens the recognition criterion for survey question 5's
  "doing this work informally" scan. Reverse: none.
- **1.3 → 1.8 (weak).** The hypothesis-039 toolkit-shift inventory turns
  WU1.8's generic perspective-shift trawl into a targeted trace. Reverse:
  none (interpretation-only).
- **1.4 → 1.7 (weak).** V1-attested scene-design categories sharpen question 3
  from a raw count into "which categories are absent or informally carried in
  v2." Reverse: none (survey statistics can't redirect a read-everything
  emergent-category pass).
- **1.4 → 1.8 (weak).** The v1 instinct inventory supplies backward
  trace-targets for "which instincts were stable across years vs changed."
  Reverse: none.
- **1.4 → 1.10 (weak).** The voice-contamination assessment is a full-scale
  trial of exactly the mining operation sub-question 3's hypothesis-015
  mineability test asks about. Reverse: none.
- **1.7 → 1.5 (weak).** The survey's stall-shape findings (scene-sparse vs
  subject-dense, unpopulated designed tracks) give chain 9's "what stalled and
  why" specific targets, and designed-but-unpopulated tracks are nameable
  concepts for the overfit roster. Reverse: none (interpretation-only).
- **1.5 → 1.8 (weak).** Verified vocabulary-dating findings anchor WU1.8's
  "before the vocabulary existed" (hypothesis 002) and fabula/syuzhet-
  emergence questions. Reverse: none.
- **1.5 → 1.10 (weak).** Per-concept provenance verdicts (AI-introduced vs
  pre-AI vocabulary) are direct input to the hypothesis-008 separability
  test. Reverse: none.
- **1.8 → 1.10 (weak).** The per-story tooling-correlation finding hands
  sub-question 1 a within-era single-factor comparison (the Falldale
  quasi-experiment) its transition-based method would not otherwise pose.
  Reverse: none (the dated timeline WU1.8 needs is VERSION-HISTORY.md, not a
  WU1.10 output).
- **1.11 → 1.5 (weak).** The P&K comment-mapping converts chain 12's
  recall-based hopepunk premise into a dated, testable anchor for the lineage
  trace. Reverse: none.
- **1.11 → 1.8 (weak).** The noticed-pattern set from Brian's contemporaneous
  reading reactions converts the hypothesis-039 planning-doc test into a
  directed one. Reverse: none.

No evaluator raised a consumption-grade inconsistency flag.

## Assembly and derived order (mechanical)

### Edge set used for the sort

Hard (consumption): 1.3→1.5, 1.3→1.9, 1.4→1.9, 1.9→1.11,
{1.3,1.4,1.5,1.7,1.8,1.9,1.10,1.11}→1.12, {1.3,1.4,1.5,1.7,1.9,1.12}→1.13,
1.13→1.14.

Enrichment (directed): 1.3→1.4 (strong), 1.4→1.5 (strong), 1.3→1.7, 1.3→1.8,
1.4→1.7, 1.4→1.8, 1.4→1.10, 1.7→1.5, 1.5→1.8, 1.5→1.10, 1.8→1.10, 1.11→1.5,
1.11→1.8.

Dropped edges, documented:
- **1.5→1.4 (weak)** — the weaker direction of the 1.4–1.5 pair; kept edge is
  the strong 1.4→1.5 (2-cycle broken by recorded rating, per assembly rule 2).
- **1.5↔1.9 and 1.8↔1.9 (comparable pairs)** — impose no constraint. Both
  pairs end up ordered transitively (1.9 → 1.11 → 1.5 and 1.9 → 1.11 → 1.8),
  which satisfies the 1.9→1.5 and 1.9→1.8 directions and foregoes the
  1.5→1.9 (chain-12 sharpening) and 1.8→1.9 (Celestia date anchor)
  directions. The foregone sharpenings are recorded here as accepted losses —
  consequences of transitive order, not judgment calls; the comparable-cycle
  escalation clause was never triggered.

The graph is acyclic. The anticipated 1.8→1.9→1.11→1.8 cycle did not arise
because the 1.8–1.9 pair resolved comparable (no edge).

### Topological sort with documented tie-breaks

Forced starts: 1.3 (only node with no predecessor), then 1.4 (everything else
awaits it).

Two ties arose, both broken by skill criterion 5 (unblocking value); criteria
4 and 6 were never reached, and no tie required escalation:

1. **{1.7, 1.9} after 1.4:** no edge between them. 1.9's completion unblocks
   1.11 (hard consumption), which in turn gates 1.5 and 1.8; 1.7's completion
   alone unblocks nothing (1.5 still awaits 1.11). → 1.9 first.
2. **{1.7, 1.11} after 1.9:** no edge between them. 1.11 removes a blocker
   from both 1.5 and 1.8; 1.7 only from 1.5. → 1.11 first.

The remainder is forced by edges: 1.7 → 1.5 → 1.8 → 1.10 → 1.12 → 1.13 → 1.14.

### Derived execution order

**WU1.3 → WU1.4 → WU1.9 → WU1.11 → WU1.7 → WU1.5 → WU1.8 → WU1.10 → WU1.12 →
WU1.13 → WU1.14**

Preconditions gate execution timing in place, never position: WU1.3's adapted
analyze-story skill and WU1.4's mining skill are the first tasks inside those
WUs; the Keep ingest is internal to WU1.5; WU1.8 waits in its slot on Brian's
preprocessing (or runs TLTT-only per its spec).

**Brian ratified the WU1.12 blanket edge (2026-08-31): WU1.12 needs ALL the
evidence.** The 1.12→1.13→1.14 tail therefore hard-blocks on every evidence
WU including WU1.8 — Brian's preprocessing of the revision-history exports is
the critical-path precondition for the tail.

### How the derived order differs from the plan's sequence

The plan's settled order was 1.4 → 1.3 → 1.7 → 1.5 → 1.9 → (1.10) → 1.11 →
1.12 → 1.8 floating → 1.13 → 1.14. The audit inverts three things:

1. **1.3 before 1.4** (was: 1.4 first). The strong edge runs from own-fiction
   findings into the read-once mining pass, not the other way.
2. **1.9 and 1.11 moved from late-middle to immediately after the
   discoveries** (was: after 1.5). Nothing orders them later — the claimed
   1.5→1.9 enrichment resolved comparable — and 1.11's outputs feed 1.5 and
   1.8, pulling the whole comparison block forward.
3. **1.5 moved from mid-plan anchor to late** (was: fourth). It consumes 1.3,
   and is enriched by 1.7 and 1.11; nothing except 1.8/1.10/the tail waits on
   it.

WU1.10 runs last of the evidence WUs — its position is fully edge-determined
(after 1.4, 1.5, and 1.8), so criterion 4's "infrastructure hypotheses first"
never applied (it breaks ties only, and no tie involved 1.10).

## How to carry out this audit (procedure)

These instructions live here, not in the v3-buildout skill: the audit is
spec-text-grounded, so it survives consolidations (hypothesis renumbering
touches no edge — the criteria rule hypothesis-ID reasoning insufficient, so
every verdict rests on Question/Scope/Evidence-source text). Only spec-text
changes move verdicts. A future plan that keeps WU bodies and remaps their
hypothesis lists carries this audit forward with an amendment noting the
remap; a full re-run is needed only if WU specs themselves are rewritten.

**1. Prepare inputs (main session, mechanical).** Extract each remaining WU's
spec section into a card, stripping all ordering metadata: the Preconditions,
Status, and Scale fields, and the trailing parenthetical ordering notes.
Question, Hypotheses, Evidence sources, Scope, What-it-does-NOT-do, and Output
stay verbatim. Prepare a roster (title + one-line question per WU, completed
WUs marked "outputs exist, constrain nothing"). The criteria section above is
given to evaluators verbatim.

**2. Consumption pass (blind agents, one per WU).** Each agent receives ONLY
the criteria, the roster, and its one card, with an explicit instruction to
read nothing else (no repository files, no MCP, no web) and judge from that
material alone. It returns: which other WUs' outputs this WU constitutively
consumes, with every cross-WU mention classified constitutive vs incidental,
one-line justifications quoting card text.

**3. Closure (main session, mechanical).** Transitive closure of the
constitutive edges; enumerate the pairs left unordered. Brian reviews the
edge set and residual list before the next pass.

**4. Enrichment pass (blind agents, one per residual pair).** Each agent
receives the criteria, the roster, and the two cards, same blindness
instruction, and evaluates both directions under the counterfactual test,
rating passing edges strong/weak, recording comparable rather than forcing a
winner, and flagging consumption-grade findings as inconsistencies.

**5. Assembly (main session, mechanical).** Per the assembly rules in the
criteria: tabulate, drop the weaker edge of any opposed pair by recorded
rating (documented), check cycles (comparable-edge cycles escalate to Brian),
topological sort, break remaining ties by skill criteria 4/5/6 in order with
each tie-break documented. Record verdicts verbatim in this document; update
the forward plan's execution-sequence section to match.

**Incremental re-run (the normal case).** After a post-WU review amends
downstream WU specs: re-strip only the changed cards; re-evaluate only the
affected pairs among REMAINING WUs, evaluators blind to the prior verdict and
the current derived order; append dated amendments; re-sort if the edge set
changed. Completed WUs' edges retire without re-evaluation.

## Amendment protocol (living document)

Verdicts above are readings of the spec cards as they stood on 2026-08-31.
They are challengeable and revisable, but through different surfaces:

- **Consumption verdicts** move only when a WU's spec text changes (post-WU
  reviews add testing specs to downstream cards; an added scope question can
  harden into a constitutive requirement).
- **Enrichment verdicts** move when spec text changes AND under better
  arguments — a proposed counterfactual the original evaluator missed, or a
  challenge that an accepted edge fails the materiality bar.
- **Completion retires edges of both kinds:** a finished WU constrains
  nothing (the WU1.1 pattern throughout the verdicts above). The audit decays
  naturally as the plan executes.

Maintenance rules:

1. Original verdicts are immutable record — never edited in place. Changes
   arrive as dated entries in the Amendments section below.
2. After each post-WU review amends downstream WU specs, the pairs among
   REMAINING WUs whose cards changed are re-evaluated blind (same criteria,
   fresh stripped cards, evaluator blind to the current derived order and to
   the prior verdict for that pair). Unchanged pairs stand.
3. A challenge to a standing verdict (from Brian or a session) is adjudicated
   the same way and recorded as an amendment, whichever way it resolves.
4. When the edge set changes, the derived order is re-sorted and re-dated
   here; the forward plan's execution-sequence section is updated to match.

## Amendments

- 2026-08-31: Brian ratified the WU1.12 blanket consumption edge ("WU1.12
  needs all"). No edge change; records that the tail's hard dependence on
  WU1.8 (and thus on the preprocessing precondition) is intended, not an
  artifact of strict reading.
- 2026-08-31: forward-plan-1 restructured to match the audit's division of
  labor — WU specs restored to numeric id order (the plan is a catalog; this
  document is the order), trailing parenthetical ordering notes deleted, and
  Preconditions fields trimmed to tooling/Brian-action blockers only (WU
  dependencies live here). The v3-buildout skill's forward-plan section was
  updated to match. No spec-content (Question/Scope/Evidence-sources) text
  changed, so no verdict is affected.
