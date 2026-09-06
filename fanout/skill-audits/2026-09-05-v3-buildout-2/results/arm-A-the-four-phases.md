## unit-111
- section: ## The four phases
- quote: Every WU of every type runs the same four phases in one HITL session (Fable): scope reconciliation, plan mode, execution, post-WU review…
- counterpart: none
- relation: absent
- note: B has no WU or fixed four-phase-in-one-session structure; work is a chain of separate activities (e.g. preparing-to-explore-a-corpus → exploring-a-corpus → reviewing-leads), each its own file, each process independently typed hitl/session/agent, not four fixed phases of one HITL session.

## unit-112
- section: ## The four phases
- quote: **1. Scope reconciliation** (auto mode)…
- counterpart: none
- relation: absent
- note: no forward plan, card, WU, or "auto mode" phase exists in B; the nearest artifact is the instance registry (SKILL.md › Instance registry), written once at Brian's go by a preparing activity's plan process, never reconciled afterward as a separate clerical step.

## unit-113
- section: ## The four phases
- quote: Exploratory | The card; the corpus's `CORPUS-STATUS.md` entry; the reading protocol; the arm design…
- counterpart: none
- relation: absent
- note: no "card" or hypothesis list exists per instance; preparing-to-explore-a-corpus § explore-plan reads question-list, corpus-status, corpus, state and skill and writes the plan fresh (scale, questions, arms) rather than reconciling pre-existing metadata, and the reading protocol/arm key are authored later in the same activity, not read beforehand.

## unit-114
- section: ## The four phases
- quote: Verification | The card; the corpus's spec pool; the named codebooks and their calibration records…
- counterpart: preparing-to-verify-a-corpus.md › ## verify-plan
- relation: narrowed
- note: B keeps open questions (question-list) as the basis for scope and requires a calibrated codebook before a round, but there is no "card" or hypothesis list to recompute — a question names its hypotheses directly, and building/calibrating an uncalibrated codebook is its own separate process (author-codebook/calibrate), not something "flagged as the first task" of reconciliation.

## unit-115
- section: ## The four phases
- quote: Synthesis | The card; the debt status of every corpus it names…
- counterpart: none
- relation: absent
- note: B has no synthesis WU type or activity at all — no chain reads verified artifacts across corpora and writes synthesis findings gated on verification debt; the closest activities (exploring-a-corpus, reviewing-leads) operate on one corpus, not on the debt status of several.

## unit-116
- section: ## The four phases
- quote: Infrastructure | The card; the thing to build and what consumes it…
- counterpart: building-a-tool.md › ## build
- relation: narrowed
- note: B keeps stating what is built and what consumes it, plus acceptance (tests, CORPUS-STATUS entry, Brian's checklist), but folds this into one single hitl build process rather than a phase-1-of-4 "scope reconciliation" step, and drops the "card"/WU framing entirely.

## unit-117
- section: ## The four phases
- quote: **2. Plan mode** (all types)…
- counterpart: preparing-to-explore-a-corpus.md › ## explore-plan
- relation: narrowed
- note: B keeps "ask Brian's open questions batched four per call, then write the plan, then Brian approves" (explore-plan; also verify-plan, build) but each type's planning is its own activity file rather than a shared "phase 2"; synthesis as a type is dropped entirely, and "exit plan mode" as a named step doesn't exist.

## unit-118
- section: ## The four phases
- quote: **3. Execution.** Per type:
- counterpart: none
- relation: non-instructional
- note: a bare header introducing the per-type execution rows that follow; carries no independent instruction of its own.

## unit-119
- section: ## The four phases
- quote: Exploratory | Run the arms; bin disagreements if there is more than one…
- counterpart: exploring-a-corpus.md › ## join-and-bin
- relation: narrowed
- note: B keeps running arms, binning disagreements and writing the leads artifact, but exploring-a-corpus only proposes questions (leads.md § Proposed questions) — writing them into the corpus's question list is a separate, Brian-led reviewing-leads step, not part of execution; there is no "card" to mark complete, since instance state is derived from artifacts.

## unit-120
- section: ## The four phases
- quote: Verification | Run the jobs; write the artifact; write the candidates file…
- counterpart: conducting-a-verification-round.md › ## round-write
- relation: narrowed
- note: substance is kept (run jobs, write round.md, write candidates, run the referee, hold a promotion session, one commit, report counts) but split across four separately gated activities — conducting-a-verification-round, writing-candidates-from-verification, refereeing-a-candidate, promoting-checked-candidates — rather than one execution phase of a single WU session; `evidence-pipeline.md` no longer exists as a citation target.

## unit-121
- section: ## The four phases
- quote: Synthesis | Read the verified artifacts; write the synthesis artifact…
- counterpart: none
- relation: absent
- note: no synthesis activity exists in B; nothing reads verified artifacts across corpora, writes a synthesis artifact, or appends spec-pool questions for insights bearing on a hypothesis.

## unit-122
- section: ## The four phases
- quote: Infrastructure | Build; test; calibrate if it is a codebook…
- counterpart: building-a-tool.md › ## build
- relation: reversed
- note: B keeps build/test/CORPUS-STATUS-update, but building-a-tool § Never explicitly forbids writing a question ("writes a candidate, a lead or a question"), where the old text has infrastructure execution append spec-pool questions if the build raised any; codebook calibration is also not part of building-a-tool at all — it is the separate hitl process preparing-to-verify-a-corpus § calibrate.

## unit-123
- section: ## The four phases
- quote: New-hypothesis proposals are held for the wrap-up in every type and offered only when novelty, testability and independence all hold.
- counterpart: minting-a-hypothesis.md › ## mint
- relation: narrowed
- note: the three criteria (novelty, testability, independence) and holding proposals for a later review/promotion session match closely for exploratory and verification, but "every type" no longer holds — building-a-tool (infrastructure) has no hypothesis-proposal wrap-up, and synthesis as a type is gone entirely.

## unit-124
- section: ## The four phases
- quote: **4. Post-WU review** with Brian, same session, two interleaving modes for all types, with one per-type difference in what "verify against the source" means:
- counterpart: reviewing-leads.md › ## review-leads
- relation: narrowed
- note: B keeps Brian-led review that verifies findings against the source (reviewing-leads for exploratory; promoting-checked-candidates for verification), but these are separate, independently gated activities with their own preconditions, not "the same session" as execution or phase 4 of one WU; synthesis and infrastructure have no equivalent challenge/enrichment review structure.

## unit-125
- section: ## The four phases
- quote: *Challenge* — Brian questions a finding…
- counterpart: reviewing-leads.md › ## review-leads
- relation: narrowed
- note: B keeps verifying a challenged finding against the source (the corpus itself, at the lead's locus) but never corrects the artifact in place — a lead only gets an appended, dated correction ("the lead itself is not edited"); synthesis as a type is gone entirely; the "corrected finding re-enters as a new candidate through the referee" detail isn't stated for a challenge (only reworded-hypothesis findings are re-queued, via iterating-a-statement).

## unit-126
- section: ## The four phases
- quote: *Enrichment* — Brian connects a finding to his practice or recall…
- counterpart: reviewing-leads.md › ## review-leads
- relation: restated
- note: —

## unit-127
- section: ## The four phases
- quote: Statement changes are batched at the end of the review, then handled as iterations (`evidence-pipeline.md` § Iteration)…
- counterpart: SKILL.md › ## Constitutional rules
- relation: narrowed
- note: the story-content-drift redirect/out-of-scope language is kept almost verbatim (rule 7), but "batched at the end of the review" is dropped — iterating-a-statement is triggered by Brian's decision in a promotion session or any hitl session, not by batching at review's end, and reviewing-leads § Never explicitly forbids rewording a statement itself.
