# Work units

Read in full before scoping, planning, executing or reviewing a WU. The evidence rules a
WU must obey are in `evidence-pipeline.md`; this file is what a WU *is* and how it runs.

## Four types

A WU is one of four types, and the card's `Type` field says which. The types are peers;
what distinguishes them is what they read, what they may write, and what they must never
write.

| Type | Reads | Writes | Never writes | Cells and who runs them |
|---|---|---|---|---|
| **Exploratory** | A raw corpus, discovery-first, with no hypothesis in view | A **WU artifact** organised by what was observed (never by hypothesis id); **questions** into spec pools — each a testable question about a corpus, with a candidate predicate where one suggests itself | Candidates; evidence. Its findings are leads. | Pathfinder and slice-reader cells — a HITL session, or arms run through the runner under an explicit reading protocol |
| **Verification** | One corpus's spec pool, and the **source itself** (e.g. the story text, the archive notes, the lineage turn) — never an intermediate analysis alone | A WU artifact (per-item results with codebook hashes, counts, tables); a **candidates file**; referee verdicts; the promotion commit; questions into spec pools (e.g. when a codebook is found wanting) | A codebook revision (that is a HITL task, triggered by its question); evidence by any route but promotion | Classifier, investigator and auditor cells (or any frozen-predicate / method-discretion cell) as runner jobs under calibrated codebooks; the referee likewise; promotion is a HITL session |
| **Synthesis** | The **verified** artifacts of the corpora it names — verification pass results and promoted evidence. It may read exploratory artifacts for the origin of a question, and cites them only as leads | A WU artifact (the comparison, retrospective, adjudication, evaluation, connection, or the like); questions into spec pools — a synthesis's own insight reaches a record only by becoming a question that a verification round answers | Candidates; evidence; anything into `hypotheses/` | A salience-discretion cell over verified artifacts (a pathfinder whose corpus is the buildout's own outputs) — a HITL session, Fable |
| **Infrastructure** | Whatever the thing being built needs | An instrument (tool, ingest, render), a codebook and its calibration record, tests, a `CORPUS-STATUS.md` update; questions into spec pools if the build raises any | Candidates; evidence; prose that is Brian's to author (display questions, definitions — see CLAUDE.md on seeders) | Instrument-authoring: HITL plus code, tested per the `testing` skill |

Every write that could reach a hypothesis record goes through `evidence-pipeline.md`;
the table above says which types can produce candidates (only verification) and which cannot.

## The corpus pair, and what is ordered

Every corpus in the buildout — the 112-story analysis corpus, Brian's own fiction, the v1
archive, the v2 working plan, the lineage layers, the conversations, the code sessions, and
any corpus added later — is worked as an **exploratory/verification pair**, and the one hard ordering rule is
**exploratory pass on c → verification pass on c → any consumer of c**: a synthesis that names corpus c waits on a verification round
of c covering the questions it relies on. Questions flow freely in every direction — an
exploratory pass, a verification pass, a synthesis or an infrastructure WU may write into any corpus's spec
pool. Findings wait. Nothing else is ordered: exploratory passes are unordered among
themselves (Brian's choice which runs first); verification passes are triggered and run
in rounds — a round for corpus c is due when its pool holds questions with a calibrated
codebook ready; infrastructure runs when something needs it (`forward-plans.md`
§ Ordering is structural).

## Design rules for all types

- **Scale is a cell, not a token count.** Decide which matrix cell the work is in; that
  decides context, model and verification. A corpus that does not fit one context is read
  by slice readers with a shared protocol, never by one reader with compaction.
- **Arms and blinding.** When a pass runs more than one reader over the same material —
  two conditions, two models, a factorial of both — each arm gets an identical explicit
  context, arms are blind to each other, record files are labelled neutrally (`arm-A`,
  `arm-B`) and the mapping to condition and model is kept in the WU's `read-manifest.md`
  and not opened until the adjudication has binned the disagreements. A pass may run one
  arm where compute forbids two, but then its artifact names what was not measured (no
  stability figure, no disagreement count) rather than letting a single arm pass as a design.
- **Binning.** Disagreements between arms are sorted into named bins *before* any is
  investigated (expected-structural / missed-by-one / unsupported-by-source, extended as
  the design requires); the counts per bin are findings; only the interesting bins are
  drilled, with Brian adjudicating the drills. Binning is what keeps adjudication from
  becoming "discuss every mismatch".
- **One factor at a time.** A WU whose *method* is under study (e.g. context length,
  reading condition) holds the model constant across arms; a model comparison holds everything
  else constant. Mixed designs are factorial by intent and say so, or they are
  uninterpretable.
- **Codebooks are not written by the pass that applies them.** A verification pass finds its
  codebook wanting → spec-pool entry → stop. Calibration (sample scored, Brian's verdicts
  beside the model's, rulings recorded) precedes any batch.
- **Every autonomous job is a runner job.** Job file in the WU's run folder under
  `fanout/` (`fanout/WU<n>.<m>-<slug>/…/jobs.json`), launched from the external fanout
  folder, items, results, attempts and ledger written beside it, the ledger's prompt and
  codebook hashes cited in the artifact's method section (`agent-runner` skill: one item
  per job, enumeration by an instrument, a checked output, a pilot before the batch). A pass that needs the MCP server sets `mcp: true` on that
  job alone. The Agent tool exists only inside a HITL session and is not a runner job: it
  inherits the instruction stack and its transcript enters the archive, so it serves only
  salience-discretion help to that session in ones and twos — never a batch, never an
  arm, never a cell that calls for explicit context. The Workflow tool is not used
  (`agent-runner` skill § Two mechanisms).

## The four phases

Every WU of every type runs the same four phases in one HITL session (Fable): scope
reconciliation, plan mode, execution, post-WU review. Phases 2 and 4 are the same for all
types with the per-type notes given; phases 1 and 3 differ by type, as the tables say.

**1. Scope reconciliation** (auto mode). Read the active forward plan's card and bring its
metadata (hypotheses, scale, type, corpus) in line with the work the WU now contains —
clerical, not judgment. Per type:

| Type | What is read to reconcile | What is reconciled |
|---|---|---|
| Exploratory | The card; the corpus's `CORPUS-STATUS.md` entry; the reading protocol; the arm design | The protocol and arms are confirmed; the hypothesis list on the card is informational only (an exploratory pass targets no hypothesis — it targets a corpus) |
| Verification | The card; the corpus's spec pool; the named codebooks and their calibration records | The pool's open questions *are* the scope; the card's hypothesis list is recomputed from what those questions bear on; codebooks without a calibration record are flagged as the first task |
| Synthesis | The card; the debt status of every corpus it names (exploratory pass done? which verification rounds? which questions answered?) | The synthesis proceeds only over corpora whose relevant questions are verified; anything it wanted from an unverified corpus is written as a question and dropped from scope |
| Infrastructure | The card; the thing to build and what consumes it | Preconditions and acceptance (tests, calibration) confirmed; nothing hypothesis-related to reconcile |

**2. Plan mode** (all types). Read or size the sources (`CORPUS-STATUS.md` for corpus
work). Collect every open question — ambiguities in the card, decisions that are Brian's,
assumptions to confirm, and anything else unresolved — and ask them all, batched at four
per call, before writing a line of plan. Then write the plan, covering at least: what is
read in what order, the arms and their explicit contexts (exploratory), the codebooks and their
hashes and the job files (verification), the verified inputs and their debt status (synthesis), the
acceptance criteria (infrastructure), what the artifact covers, the binning scheme where
arms exist, and what the WU does **not** do. Exit plan mode; Brian approves.

**3. Execution.** Per type:

| Type | Primary work | Wrap-up sweep (full hypothesis index, for anything touched outside the targets) |
|---|---|---|
| Exploratory | Run the arms; bin disagreements if there is more than one; write the artifact; append spec-pool questions; mark the card `complete` | More spec-pool questions — never candidates |
| Verification | Run the jobs; write the artifact; write the candidates file; run the referee; hold the promotion session; commit once; report the counts (`evidence-pipeline.md` § Promotion) | More candidates through the same referee — never a side door |
| Synthesis | Read the verified artifacts; write the synthesis artifact; append spec-pool questions for every insight that bears on a hypothesis; mark `complete` | Spec-pool questions — a synthesis never writes to `hypotheses/` |
| Infrastructure | Build; test; calibrate if it is a codebook; update `CORPUS-STATUS.md` if it is a corpus; mark `complete` | Spec-pool questions if the build raised any |

New-hypothesis proposals are held for the wrap-up in every type and offered only when
novelty, testability and independence all hold.

**4. Post-WU review** with Brian, same session, two interleaving modes for all types, with
one per-type difference in what "verify against the source" means:

- *Challenge* — Brian questions a finding. Verify against the source, never the
  intermediate analysis: for exploratory and verification that is the corpus itself (the story text, the notes,
  the turn); for a synthesis it is the verified artifact *and* the source that artifact
  cites; for infrastructure it is the test or calibration record. Correct the artifact if
  the finding does not hold. For a verification pass, a corrected finding re-enters as a new candidate
  through the referee.
- *Enrichment* — Brian connects a finding to his practice or recall. That is a question:
  write it to the relevant corpus's spec pool with its provenance ("Brian's recall,
  2026-…: does the v1 archive show X?"). Recall never enters a record.

Statement changes are batched at the end of the review, then handled as iterations
(`evidence-pipeline.md` § Iteration). Story-content drift is redirected to the framework
question or named as out of scope.

## WU artifacts

`docs/v3-framework/WU<plan>.<unit>-<slug>.md` or a directory of that name when the WU
has several files (records, renders, manifests). Runner jobs, items, results, attempts, the
ledger and the candidates file live in the WU's folder under `fanout/`, cited from here.
Write-once evidence: later
sessions cite them, never edit them; a correction is an appended dated section. An
artifact's method section names at least the protocol and codebook hashes, the arms, the
harness version and models, and the read-manifest — whatever it takes that the pass could
be re-run.

Counts in an artifact cite the instrument that produced them; classifications that bear on
a hypothesis cite the source locus and were read there.
