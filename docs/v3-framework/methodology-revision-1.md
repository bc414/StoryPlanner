# Methodology revision 1 — 2026-09-03

Write-once. Records why the `v3-buildout` skill was replaced by `v3-buildout2`, what each
change replaced, and what evidence prompted it. The skill says what to do; this note says
what it superseded and why. Rulings are Brian's (2026-09-02 design conversation, 2026-09-03
execution); the wording is Claude's.

## Prompting evidence

1. **Findings were conflated with evidence** (2026-08-31 → 2026-09-01). Evidence entries
   were deposited by the same session that had spent hours looking for the predicted
   patterns, with the "would differ if false" clause self-administered; findings that failed
   the clause were given the `contextual` tag as a soft landing instead of being kept out.
   WU1.3's post-review found its first deposits biased (13 supporting, 0 challenging) and
   four per-story FID misclassifications caught only on source check.
2. **The forward plan was an exploratory programme with confirmation bolted on.** Of the
   WUs in forward-plan-1, only WU1.4's voice-attribution instrument was confirmatory-grade;
   verification tasks accreted onto exploratory cards as "testing specs" (WU1.4 carried ~20
   named checks on top of an open-ended read; WU1.9 was silently becoming a source-
   verification campaign) because the plan had no verification WU type. The one-pass,
   globally ordered design strained for the same reason: confirmation cannot be scheduled
   before exploration has produced its questions.
3. **Tier numbering was incoherent** (0 → 3 → 2 → 1 as an escalation order) because it
   compressed two independent axes — context scope and judgment location — into one
   ladder. The provenance point-checks exposed the independence: item-scoped, but with
   method discretion.
4. **The harness incident of 2026-08-27.** `tools/StoryPlanner.AnalysisRunner` relaunched
   the same failing `/analyze-story` job until the utilization cap, from the repo root, with
   no model pin: 9,245 transcripts in the StoryPlanner project history (9,247 matching the
   rule; two dated 2026-08-26). The VS Code extension lagged on every new session; a
   codesessions ingest would have re-imported anything cleaned by hand, since the ingest
   has no delete path by design. Deleted 2026-09-03 after the exclusion rule existed
   (9,322 → 77 files); the two 2026-08-26 transcripts were left for Brian's ruling.
5. **The skill had outgrown one file.** Every session loaded protocol detail for
   activities it was not performing; the Canalave conventions skill's router-plus-companion
   structure (always-loaded axioms, per-topic files read on demand) was judged productive
   organisation, not naive best effort, and adopted.

## What changed, and what it replaced

| Revision 1 | Replaces | Note |
|---|---|---|
| **The work matrix** — context scope × judgment location, cell names *instrument / classifier / investigator / auditor / focused reader / slice reader / census / pathfinder*, role (generate / verify) orthogonal | Tier numbers 0–3; the "~600K tokens → subagents" scale rule | Scale is a cell, not a token count. Model allocation (Fable HITL, Opus slice/investigator, Sonnet classifier/auditor/referee) is stated as revisable doctrine. |
| **Strong-form evidence pipeline** — exploratory passes write findings + spec-pool questions only; verification passes write candidates; a fresh-context referee attempts the clause blind; a promotion session Brian reviews writes the record in one commit | The deposit protocol (two writes by the main session; self-administered clause; exclusion at deposit) | Repeals "deposits are judgment and never delegated to a subagent": enforcement moved from *who writes* to *what merges*. |
| **Candidate statuses** `candidate / diagnostic / non-diagnostic / promoted / declined / held` in an append-only candidates file | The `contextual` alignment tag | `contextual` is retired outright; non-discriminating findings now have an honest home (non-diagnostic, in the WU directory) instead of a softer tag in the record. |
| **Iteration → re-referee** — entries under an old wording become candidates again; tags are never edited in place | In-place alignment re-assessment after an iteration | Fixes a latent defect: re-tagging left the recorded clause describing the counterfactual for the *previous* wording. |
| **Verification debt** — exploratory pass on c → verification pass on c → consumers; questions flow freely, findings wait | Enrichment flow that appended testing specs to the next downstream card | Spec pools per corpus replace card bloat; verification passes are triggered, not sequenced, and may run in rounds. |
| **Ordering is structural** — exploratory passes unordered (Brian's choice, two advisory heuristics), verification passes triggered, synthesis gated by debt; the plan's execution section is a status board | The ordering audit (`forward-plan-1-ordering-audit.md`): a living pairwise DAG over WUs derived by blind consumption and enrichment passes, with an amendment protocol | The audit was itself reactive — created 2026-08-31 because plan 1's hand-asserted order "repeatedly reversed under scrutiny" — and its problem dissolves once questions have a home (the pool) independent of order. It retires with plan 1 as historical record. Gleaned from it: (1) its blind-evaluator procedure — strip the authored conclusion, hand a fresh agent the raw cards and one fixed counterfactual question, record *none* as a finding — is the referee's direct ancestor; (2) two principles survive verbatim (readiness/convenience/throughput/duration are never ordering inputs; preconditions gate timing, never position); (3) its finding that the hand-asserted enrichment chain did not survive blind evaluation the same day is evidence of the family hypothesis 048 names — the author of a dependency is a poor judge of whether it holds; (4) the model-intrinsic observation it produced is already in WU1.10's spec. Superseded: the pairwise DAG as the order-determining mechanism, the amendment protocol, the specific edge verdicts. |
| **Two WU types per corpus** (exploratory, verification) plus synthesis and infrastructure | One WU type; one pass per evidence source | WU1.1 is re-read as pathfinding-complete with its verification owed; WU1.4 is to be re-specced whole in forward-plan-2 (instrument, CSV and card rulings stand). |
| **Codebooks as instruments** (`docs/v3-framework/codebooks/`) — HITL-authored, calibrated on Brian's verdicts, hash-versioned, re-run on revision | Classification embedded in per-story analyses under a brief | The 046 DT-class classification in WU1.9's spec is the template. |
| **The referee has a codebook** (`codebooks/referee.md`: three inputs, three classes, decision rules R1–R9, worked examples, calibration requirement) and the skill only states what the codebook must implement | The referee's procedure as prose in the skill — a document the explicit-context rule says the referee never sees | Drafted 2026-09-03 at Brian's prompt ("don't we need a codebook for this procedure instead of just text in a skill?"); **uncalibrated** until a calibration record exists, which now precedes the retroactive pass. |
| **Explicit context for autonomous agents** — protocol + item, exact toolset, no CLAUDE.md / skills / memory / MCP unless opted in, no transcript | Subagents of a repo session inheriting the full instruction stack | Control (hashable inputs), framing contamination (CLAUDE.md asserts what the buildout is testing), and cost. |
| **`tools/StoryPlanner.AgentRunner`** — job files, launch from `RiderProjects\StoryPlanner-fanout`, `--restricted --tools … --no-session-persistence --strict-mcp-config`, ledger-derived queue with `maxAttempts`, prompt and per-file hashes, cost and turns recorded | `tools/StoryPlanner.AnalysisRunner` (deleted) | Smoke-tested 2026-09-03: exact toolset, no MCP servers, no transcript written, a missing-output attempt recorded failed-not-retried. |
| **Two mechanisms, one line** — the runner for every autonomous cell, arm and batch; the Agent tool only inside a HITL session, for salience-discretion help in ones and twos; the Workflow tool not used | Brian's 2026-09-02 statement that "there are three ways to do the fan out: Agent tool, Workflow tool, and Runner v2", left as three equal options | Two later rulings (explicit context; codesessions as prevention) eliminated the in-session mechanisms for anything batch-shaped or hash-cited, but the elimination was never written down until Brian asked (2026-09-03), when he also ruled the Agent tool HITL-only and the Workflow tool unnecessary. Recorded in the `agent-runner` skill and `wu-execution.md`. |
| **The `agent-runner` skill** — job-file schema, launch-folder invariants, tool/MCP semantics, ledger-as-queue, prompt hashes, placement of protocols vs codebooks, first-use traps | Runner knowledge scattered across code comments, the fanout README and a transcript | Written 2026-09-03 after three traps surfaced in one afternoon (output dir must pre-exist under `--restricted`; `allowedTools` pre-approves but does not restrict; same-named inputs collided in headings and ledger). Harness-level, so its own skill — the supersession audit already uses the runner outside buildout work. |
| **codesessions.db is prevention, not curation** — `excludeFirstUserMessage` rule (`^/analyze-story `, slash-command markup unwrapped), subagents excluded with their parent, `--list-excluded` as the one sanctioned selector for cleanup | Manual deletion of rows that the next ingest would have restored | Documented in CLAUDE.md, the `code-sessions` skill, and the ingest config together. Dry run 2026-09-03 before deletion: 9,247 excluded; 53 rule-matching sessions remained in the db from before the manual cleanup — their disposition is Brian's. |
| **Router skill** — SKILL.md carries the epistemic framework, the matrix, constitutional rules and a routing table; protocols live in `hypothesis-records.md`, `evidence-pipeline.md`, `wu-execution.md`, `forward-plans.md`, `consolidation.md` | One 600-line SKILL.md | Companion files are loaded only by instruction ("read X in full before Y"); that is why the always-needed rules stay in SKILL.md. |
| **Vocabulary**: "finding" (what a pass observed), "candidate" (a finding a verification pass claims bears on a hypothesis), "evidence" (a promoted candidate), "question" (a spec-pool entry) | Undefined use of "finding" and "evidence" | The conflation in §Prompting evidence 1 was partly a vocabulary failure. |

Deliberately dropped, not merely moved: the `contextual` tag; the token-count scale rule;
the per-hypothesis WU coverage table as a *required* plan section (a verification card's hypothesis
list is now "per spec pool"); the instruction that the main session synthesises what
subagents extract (a slice reader's records are an artifact, adjudicated, not synthesised
by a long-context session). Everything else in the old skill has a restated counterpart.

## Framing and vocabulary lineage

The revision's vocabulary was derived in the 2026-09-02 conversation by mapping the
buildout onto established disciplines, at Brian's request ("frame my rigorous study of
literary practice and AI assisted work against conventional examples"). Recorded here
because the skill deliberately carries no rationale, and because a later reader should
be able to tell a borrowed term from an invented one.

- **The "would differ if false" clause** is a severity test (Mayo's error-statistical
  account: a claim is warranted only by a test that could have failed) and, in Heuer's
  *Analysis of Competing Hypotheses*, the rule that non-diagnostic evidence gets no weight.
  `non-diagnostic` is Heuer's word. The courtroom's relevance standard (evidence must make
  a disputed fact more or less probable) is the same test; the case file vs the trial
  record is the artifact vs the record. For a software engineer the nearest native form
  is mutation testing: a test that still passes when the code is mutated to be wrong has
  no discriminating power.
- **Exploratory vs verification passes** is the exploratory/confirmatory split of
  registered reports; **spec pools** are pre-registration's question list; **verification
  debt** is "no consumer before confirmation".
- **Codebook, calibration, inter-rater agreement, adjudication, blind arms, binning** are
  from qualitative content analysis and corpus annotation, where the standing finding is
  that annotation quality is dominated by codebook quality, not annotator quality — the
  reasoning behind putting Sonnet in frozen-predicate cells.
- **Referee** is peer review / the newsroom fact-checker / the audit firm's independence
  rule: the party that produced a claim never certifies it. The ordering audit of
  2026-08-31 was the first instance of the pattern in this project.
- **Baselining** is the newsroom editor's "run it" and the audit partner's signed
  opinion — sufficient to act on, not a truth claim — not academic acceptance.
- **Source grading and compartmentalisation** (provenance stamps on every artifact;
  corpora never joined) are intelligence-analysis practice; **textual criticism**
  (attribution of authorship, stemmatics, dating by external anchor) is what the voice
  attribution instrument and the lineage rules are.
- **The paradigm shift Brian named**: software engineering is the one discipline with a
  cheap total oracle (the compiler, the test suite), so its culture assumes verification
  is cheaper than assertion. Everything in this revision is what rational practice looks
  like where no oracle exists — hand-built verification from redundancy, independence,
  frozen rules and a designated adjudicator — and the method's standing move is to push
  as much as possible back across the line into oracle territory (instruments,
  `PlanIntegrity`, the attribution CSV) before switching machinery.

## Rejected alternatives (2026-09-02/03)

- **A staging git branch or worktree for candidate deposits.** Considered so the clean
  hypothesis files stayed untouched during autonomous phases; rejected once candidates
  became *new files in the WU directory* — the pipeline never touches `hypotheses/`, so
  there is nothing to quarantine, and promotion is an ordinary reviewed commit.
- **SVN-style branch folders inside the repo per run.** Rejected with the above; nothing
  needs a second copy of anything.
- **A worktree for transcript isolation.** Rejected in favour of a plain folder outside
  the repo: the constraint is the launch cwd, not git.
- **Keeping the 2026-08-27 batch transcripts as evidence.** Initially chosen, then
  reversed: they are not human-rooted sessions and carry no provenance the incident
  record does not; deleted once the ingest rule existed.
- **A decisions brief as the skill-revision handoff.** Skipped because the revision was
  done in the same session; this note is the replacement.
- **Including the revision note in the supersession audit's inputs** (so deliberate drops
  were distinguishable). The comparison session excluded it: stripping the authored
  conclusion is what makes the audit independent; intent is applied at adjudication.

## Owed by this revision

- **Adversarial comparison** of `v3-buildout` against `v3-buildout2` by a fresh-context
  auditor (a runner job) — a separate session; then the old skill folder is deleted and
  the name settled (Brian's call: keep `v3-buildout2`, or rename to `v3-buildout`).
- **forward-plan-2** — a priority reassessment. Every one-time instruction for it
  (re-housing plan 1's WUs, the WU1.4 factorial re-spec, seeding the spec pools, the
  codebooks to name, the open rulings) is in `forward-plan-2-handoff.md`, deliberately
  outside the skill.
- **Retroactive referee pass** — a verification WU under plan 2, calibration of the
  referee codebook as its first task (handoff: `retroactive-referee-pass-handoff.md`).
  Ruled 2026-09-03 not to precede plan 2 and not to live in the skill.
- The two 2026-08-26 `/analyze-story` transcripts and the 53 rule-matching sessions still
  in codesessions.db — Brian's rulings.

## Addendum 2026-09-03 (evening) — the audit's first run, and what it changed

The owed adversarial comparison was first built as one runner job per arm: nine documents
inlined (~26K tokens) and the agent told to enumerate the old skill's units itself. Arm A
ran 39 minutes with no output and was killed by hand. Two faults, both the author's: the
job matched the auditor cell by name and not by its constraint (one item, judgment spent at
design time), and a rule the agent applies was mistaken for computation. Rulings that
followed, all Brian's:

- **Enumeration is an instrument.** `split` (a verb of the runner) cuts a document into
  numbered units once; the agent receives unit files and never the rule.
- **Every job names its `item`; every output is checked mechanically** (`requireOnce`);
  every attempt has a **timeout**; batches are generated from the manifest; no batch runs
  without a pilot. All in the `agent-runner` skill.
- **`fanout/` in the repo, vertical by work.** The apparatus's inputs and outputs — protocol
  or codebook, calibration, items, jobs, results, attempts, ledger, candidates — live in
  one folder per work (`fanout/referee/`, `fanout/WU<n>.<m>-…/`, `fanout/skill-audits/…`).
  No shared `codebooks/` or `protocols/` folder: the referee is one work and its folder
  holds every referee run; a corpus codebook belongs to its verification WU. The
  `docs/v3-framework/codebooks/` folder and its README are dissolved into
  `fanout/referee/codebook.md` and the two skills. The external launch folder holds only
  its README. Documents people write (artifacts, adjudications) stay in `docs/`.
- The comparison itself is rebuilt as one job per section per arm over the 174 units
  (`fanout/skill-audits/2026-09-03-v3-buildout/`), piloted on one section before the batch.

### The audit's outcome (2026-09-03, late evening)

Run: `fanout/skill-audits/2026-09-03-v3-buildout/` — 18 jobs (one per section of the old
skill, arm A only; arm B dropped for compute), Sonnet, protocol `protocol.md@f30011c4ba9a`,
harness 2.1.258, all 174 units answered, every output check passed, total cost $4.74.
Relations: 73 restated, 50 narrowed, 19 broadened, 16 non-instructional, 14 reversed,
2 absent, 0 delegated. Adjudication (HITL, Brian ruling): all 14 reversals and ~33 of the
narrowings are the intended changes in the table above; the remaining narrowings are dropped
detail left dropped (named examples, content lists, the entry-length guidance, the
provenance pointers). Eight one-clause restorations were applied to the companion files:
ids unique across the set; baselining is not endorsement of truth; grounding proceeds after
Brian has seen the discrepancy; ad hoc sessions read the index first; plan creation reads a
fresh consolidation report; proposals outside a WU are made when the criteria hold; forty
files in a consolidation follow the one-file protocol; "prose technique" out of scope means
goals and mechanisms, never how to write. Verdict: the rewrite holds. The old skill folder's
deletion and the name (`v3-buildout2` kept, or renamed to `v3-buildout`) remain Brian's.

**Renamed 2026-09-03, after the audit:** the old `v3-buildout` folder was deleted and
`v3-buildout2` renamed to `v3-buildout`. Every `v3-buildout2` above refers to the skill now
at `.claude/skills/v3-buildout/`; the audit run's job file keeps the old paths as a record of
what ran.
