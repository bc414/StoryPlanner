# Handoff: forward-plan-2

Written 2026-09-03 for the session that writes forward-plan-2. Everything here is
one-time and reactive — the state of the buildout on this date and the rulings Brian made
in the 2026-09-02/03 design conversation that a fresh session could not reconstruct. None
of it belongs in the `v3-buildout` skill; the skill is the standing method and this is
the situation it is applied to. Where this file and the skill disagree, the skill wins and
the disagreement is worth reporting.

Read first, in full: `v3-buildout` SKILL.md and `forward-plans.md` (then the files its
routing table names for plan creation), `methodology-revision-1.md`, `forward-plan-1.md`
with its ordering audit (as history), every hypothesis file, every spec pool, and
`CORPUS-STATUS.md`.

## Preconditions

- The adversarial comparison of `v3-buildout` against `v3-buildout` has run and been
  adjudicated, the old skill folder is deleted, and the skill's name is settled (Brian's
  call: keep `v3-buildout` or rename to `v3-buildout`). Plan 2 is written under a settled
  skill, not a provisional one. If this has not happened, stop and say so.
- The retroactive referee pass has **not** run and does not need to. Plan 2 treats every
  pre-revision hypothesis status as unverified and gives the pass a card.

## What plan 2 is

A **priority reassessment**, not a consolidation: the hypothesis set is structurally
unchanged (046 hypotheses at consolidation-1 plus 047–050 minted 2026-09-03 and 013
iterated the same day). Retire plan 1 with a header stamp (date, successor: plan 2,
reason: methodology revision 1 re-typed every WU). Stamp `forward-plan-1-ordering-audit.md`
as historical at the same time — it retires with its plan; its blind-evaluator procedure
is the referee's ancestor and its edge verdicts describe WUs that no longer exist.

## The shape

Corpus pairs (exploratory + verification) for each corpus, a synthesis layer, and
infrastructure cards where needed. The corpora as of this date: the 112-story analysis
corpus, Brian's own fiction, the v1 archive, the v2 working plan, the lineage layers,
the conversations, the code sessions. The execution section is a status board, not a
sequence.

## Re-housing plan 1's work — rulings already made

- **WU1.1 (corpus synthesis)** is *pathfinding-complete*: an exploratory pass over the
  meta-analyses, not over the source texts. Its verification is owed — a verification pass on the
  112-story corpus against the story text, Sonnet classifiers under calibrated codebooks
  (the 046 DT-class classification in WU1.9's spec is the template). Brian: "1.1 isn't
  really complete, only the broad pathfinding part."
- **WU1.3 (own fiction)** is the own-fiction exploratory pass, complete; its deposits are
  unverified; verification of own fiction is owed.
- **WU1.9 (cross-corpus comparison)** stays a synthesis and waits on verification of the analysis corpus,
  verification of own fiction and verification of the v1 archive. Its verification-shaped specs — the 046 classification,
  the counterargument re-read against source, the per-story obstacle breakdown — move into
  the analysis-corpus spec pool.
- **WU1.4 (v1 archive)** is re-specced whole. What stands: the voice-attribution
  instrument, `attribution.csv`, the calibration records, and the rulings on plan 1's WU
  card. What is re-typed:
  - The reading becomes the v1 **exploratory pass** run as a **factorial**: two reading
    conditions (pathfinder — the whole scene corpus in one context; slice readers — one
    per arc file plus Aris) × three models (Fable 5.1, Opus 4.6, Sonnet 5). The pathfinder
    condition runs under all three; the slice-reader condition runs fully under one model
    and on a **subset of arcs** under the other two — choose one dense/contaminated arc and
    one sparse/clean arc so the model comparison sees both regimes. Brian accepted the
    extra spend because the v1 archive is his current work.
  - All six arms are runner jobs from the fanout folder with an identical explicit
    protocol — the `v1-archive-mining` skill's reading protocol and record format handed
    over as a protocol file, no CLAUDE.md in any arm, the Fable arm launched when the
    weekly window is fresh. Check the CLI model ids (Brian's interactive model is
    `claude-fable-5-1[1m]`; confirm what `--model` accepts for each arm).
  - Arms are blind to each other and labelled neutrally; the label→(condition, model)
    map lives in `read-manifest.md` and is not opened until disagreements are binned.
  - Adjudication is redesigned for six record sets: do not join all pairs. Spine: each
    model's slice-reader set against the *same-model* pathfinder set (condition effect —
    047's disjointness question); the three slice-reader sets against each other on the
    subset arcs (model effect); the interaction from both (050). Extend the bin taxonomy
    beyond plan 1's three (cross-arc / missed by the pathfinder / over-read by the fresh
    reader) to distinguish missed-by-one-model from missed-by-all. Brian adjudicates the
    drills.
  - The ~20 named checks accreted on the card become the v1 **verification pass**: for
    each, a frozen decision rule (evidence source, rule, what is written if absent),
    calibrated before the batch; 049's with/without-instruction-stack cell can ride on
    this pass (already in the v1 spec pool). Whether the subject reading (six named
    subjects + 18 triage-labelled) is part of the exploratory pass or a focused-reader step of the
    verification pass is plan 2's decision.
  - Instrument settings still to confirm from plan 1's banner: R = 8 against borderline
    `fragment` rows; the six named subjects' ids. The 2025-12-23 backup is not needed.
  - The twelve target hypotheses of plan 1's card (019, 020, 021, 022, 028, 029, 031, 035,
    038, 040, 043, 044) become spec-pool questions for the v1 corpus; three questions
    (047, 049, 050) are already in `spec-pools/v1-archive.md`.
  - Among exploratory passes, v1 runs first: Brian's stated priority.
- **WU1.5 (retrospective)** is a synthesis. Its provenance point-checks (the nine-row
  unverified provenance table; the thirteen chains) are investigator jobs — a lineage
  verification pass; move them to a lineage spec pool. The Keep sidecar ingest remains
  its precondition (an infrastructure card).
- **WU1.7 (working-plan survey)** is mostly census work (tool-level counts over the v2
  plan) with a small exploratory residue; type accordingly.
- **WU1.8 (planning evolution)** still waits on Brian's preprocessing of the revision-
  history exports (infrastructure precondition, unchanged).
- **WU1.10 (pipeline investigation)**: its "check code sessions for other instances of
  this error class" items are investigator jobs over `codesessions.db` — a code-sessions
  verification pass. Note that the sub-question it called untestable (controlled model
  comparisons) is now cheap: the runner makes one-factor cells routine, and WU1.4's
  factorial is the first.
- **WU1.11–1.14** stay syntheses, each waiting on the debts of the corpora it consumes.
- **Retroactive referee pass**: one verification card, handoff
  `retroactive-referee-pass-handoff.md`, calibration of `fanout/referee/codebook.md` as its
  first task.

## Seeding the spec pools

Every plan-1 "testing spec" and every pre-revision evidence entry becomes a spec-pool
question in the corpus that could verify it, with provenance (which WU or review raised
it, which hypothesis it bears on, Brian's-recall items marked as such — recall is a
question, never evidence). This unwinds the card bloat the revision note describes.
`spec-pools/v1-archive.md` exists with three entries; the others are created here.

## Codebooks the plan must name

`referee` (drafted, uncalibrated); `dt-classes` (046, to write); the v1 check decision
rules (to write, per check); whatever the analysis-corpus verification pass needs. Each verification card names
its codebooks as "to calibrate" and calibration is the card's first task.

## Also open, Brian's rulings

- The two `/analyze-story` transcripts dated 2026-08-26 left in the project dir.
- The 53 rule-matching sessions still in codesessions.db (no delete path; a one-time
  purge would be a deliberate op).
- Whether hypothesis 013's refinement stays an iteration or becomes its own file.
- Skill name after the comparison.

## Standing reminders that are easy to lose here

Nothing in this file is evidence. Plan 1's syntheses and WU1.1's/WU1.3's deposits are
leads. The plan reads statuses as unverified. Findings wait; questions flow. The runner,
not the Agent tool, for every autonomous arm.
