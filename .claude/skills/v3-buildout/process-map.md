# Process map — the skill's spine (rows; UNVALIDATED draft of 2026-09-04)

> Status: **authored 2026-09-04, not yet validated** (methodology revision 2,
> `docs/v3-framework/methodology-revision-2-handoff.md`). The validator
> (`tools/StoryPlanner.ProcessMap`) does not exist yet; the generated sections are empty. Until
> the revision lands, `SKILL.md`'s routing table is still the router and this file is a
> companion under construction. Nothing here is settled: the columns are the schema, the rows
> are in flux.

This file is the **topology** of the buildout: what processes exist, who runs each, what it
consumes and produces, why it exists, and which prose section governs it. The prose
companions (`evidence-pipeline.md`, `wu-execution.md`, `hypothesis-records.md`,
`forward-plans.md`, `consolidation.md`, the `agent-runner` skill, `fanout/PROTOCOL.md`) hold
the **procedure**. Precedence when they disagree: topology from these tables, procedure from
the prose, and the disagreement itself is a gap to rule on.

**Consumers are never written here.** A process's consumers are the processes whose `inputs`
include its `outputs`; the validator derives them. A file with no consumer, a process with no
root, and two processes citing one root without an independence reason are the three
mechanical gap kinds; the validator reports them.

## Format (the schema)

Every table is parsed by the runner's markdown unit rule: one row per line, cells never
contain `|`, ids are `[A-Za-z0-9.-]+`, lists are space-separated ids. Column sets are fixed:

- **Roots** — `id | kind | root | source`. `kind` ∈ `goal | rule | incident | hypothesis`.
  `source` names where the root is stated.
- **Files** — `id | path | keep | governed-by`. `keep` ∈ `committed | gitignored |
  regenerable | outside-repo`. Files are the only things processes read and write.
- **Processes** — `id | level | kind | process | actor | inputs | outputs | roots | governed-by | state`.
  `level` ∈ `P E V S I R F M`. `kind` ∈ `sop | bootstrap | reactive`. `actor` ∈ `brian |
  hitl:<model> | agent:<model> | script | tool`, exactly one per row (a row needing two is
  split). `inputs` / `outputs` are file ids. `roots` are root ids, at least one.
  `governed-by` is `file § section`. `state` ∈ `exists | specified | unbuilt | contradictory`.
- **Edges** — `from | to | kind | label`. Control flow between processes only; data flow is
  derived from inputs/outputs. `kind` ∈ `flow | choice | fork | join | optional`. A choice
  edge's `label` is the branch condition.
- **Generated sections** sit between `<!-- generated:<name> -->` and `<!-- /generated -->`
  and are never hand-edited.

Validation (`tools/StoryPlanner.ProcessMap`, built 2026-09-04): unique ids; every reference
resolves; every process has ≥ 1 root, one actor, ≥ 1 input and ≥ 1 output; every output file
has a consumer; every `governed-by` resolves to a file; every `Roots.source` locus resolves;
every root is cited by ≥ 1 process; no path from a candidates file to a hypothesis file, or to
a `.storyplan`, without a `brian` node before the write; `SKILL.md` within its line budget;
codebook examples declare the rules they exercise; bootstrap rows listed with the WU that
retires them.

Four points of the schema were ruled on 2026-09-04 while the tool was built; the reasons are in
`docs/v3-framework/methodology-revision-2-handoff.md` § Rulings so far, items 7–13.

- **`governed-by` is one repo-relative file path, and a `§` in it is a syntax error.** The cell
  is a reading assignment ("read this in full before acting") and the precedence declaration
  (which prose wins on *how*) — the same thing, both document-granular. Section precision lives
  in `Roots.source`, where it is a citation rather than a second cell to keep in step.
- **`Roots.source` uses `<path> [§ <heading>] [¶ <n>]`**, strictly: the heading must match
  exactly one heading in the file, `¶ n` addresses the nth top-level ordered-list item, and a
  bare trailing integer is an error rather than an item pointer. Several sources in one cell are
  separated by `;`.
- **Paths are repo-relative, one form only** — no search order, so no ambiguity. `fanout/` is in
  this repo; the external `StoryPlanner-fanout` is the agent launch folder and is never cited.
- **A process reads at least one file and writes at least one file.** One that writes nothing
  cannot be told apart from one that did not run; one that reads nothing is deriving from
  recall, which C8 forbids. So a file read only by a person is read by a *process this map is
  missing a row for*, and there is no terminal-record exemption.

## Validating and rendering this file

```
dotnet run --project tools/StoryPlanner.ProcessMap -- validate .claude/skills/v3-buildout
dotnet run --project tools/StoryPlanner.ProcessMap -- render   .claude/skills/v3-buildout
dotnet run --project tools/StoryPlanner.ProcessMap -- nodes    <file.md>
```

`validate` prints every finding as `rule-id | row | message` and exits 1 if any is a failure.
Three weights: **failure** sets the exit code; **info** never does (fan-in per governing file,
schema values no row uses); **vacuous** marks a check whose subject set is empty — reported as
vacuous rather than silently passing, because "no process writes a `.storyplan`" and "every
path to one passes a Brian node" are different facts.

`render` writes the five generated sections and **refuses while `validate` fails**. To look at
the diagrams before the rows are clean, copy the folder to a scratchpad and render the copy with
`--force --repo <repo root>`; every section is then stamped UNVALIDATED. Never `--force` this
file: the generated sections stay empty until validation passes. `nodes` prints the node ids and
edge pairs of every mermaid block in a markdown file, normalised, for set-comparing a rendering
against `process-map-1-draft.md`.

Tests are `tests/StoryPlanner.Tests/ProcessMap/`, pure tier, on inline fixtures. One test runs
`validate` over this real folder and is skipped until the gap review lands; from then on
`dotnet test` pins the method's topology the way `PlanIntegrity` pins the data's.

## Roots

| id | kind | root | source |
|---|---|---|---|
| G1 | goal | Every relevant finding is preserved and findable near the hypothesis it bears on | evidence-pipeline.md § Goal |
| G2 | goal | Discriminating evidence is separated from context by status, never by exclusion | evidence-pipeline.md § Goal |
| G3 | goal | The separation is not judged by the party that produced the finding | evidence-pipeline.md § Goal |
| G4 | goal | The human-facing record is never touched by unreviewed machine judgment | evidence-pipeline.md § Goal |
| C1 | rule | Only Brian baselines; only Brian decides story content, taxonomy, what is interesting, what is written to a .storyplan | SKILL.md § Constitutional rules 1 |
| C2 | rule | Strong form: only verification passes produce evidence; nothing else writes to hypotheses/ | SKILL.md § Constitutional rules 2 |
| C3 | rule | Verification debt: no consumer of a corpus before its verification round; questions flow, findings wait | SKILL.md § Constitutional rules 3 |
| C4 | rule | A codebook is an instrument: HITL-authored, calibrated before use, hash-versioned, re-run on revision | SKILL.md § Constitutional rules 4 |
| C5 | rule | Explicit context for autonomous agents: protocol + item, exact tools, no CLAUDE.md/skills/memory/MCP unless opted in, no transcript | SKILL.md § Constitutional rules 5 |
| C6 | rule | Claude never creates a hypothesis file autonomously | SKILL.md § Constitutional rules 6 |
| C7 | rule | The story-content boundary: technique, architecture and methodology only | SKILL.md § Constitutional rules 7 |
| C8 | rule | Never derive from recall; recall is a question | SKILL.md § Constitutional rules 8 |
| C9 | rule | Recall is atmosphere, evidence is grounding: query the source before acting on any statement about the data | SKILL.md § Epistemic framework |
| I1 | incident | Findings conflated with evidence, 2026-08-31 to 09-01: the searching session self-administered the clause | methodology-revision-1.md § Prompting evidence 1 |
| I2 | incident | WU1.3's first deposits 13 supporting to 0 challenging; four FID misclassifications caught only on source check, 2026-09-01 | methodology-revision-1.md § Prompting evidence 1; hypothesis 048 created entry |
| I3 | incident | The contextual tag used as a soft landing for non-discriminating findings, 2026-08-31 to 09-01 | methodology-revision-1.md § What changed (candidate statuses) |
| I4 | incident | AnalysisRunner's infinite retry left 9245 transcripts in the project history, 2026-08-27 | methodology-revision-1.md § Prompting evidence 4 |
| I5 | incident | A corpus-scale single job with nine inlined documents ran 39 minutes with no output, 2026-09-03 | methodology-revision-1.md § Addendum; agent-runner SKILL.md § A well-formed job |
| I6 | incident | Hand-kept mirrors stale on the day written: the index status column and plan 2's coverage table, 2026-09-04 | hypothesis-records.md § Files and the index; forward-plans.md § Structure |
| I7 | incident | Plan 1's hand-asserted ordering and enrichment chain reversed under blind evaluation the same day, 2026-08-31 | methodology-revision-1.md § What changed (ordering) |
| I8 | incident | The referee's third input and rules R2/R5/R6 entered by accretion across five turns of 2026-09-03, never by ruling | docs/v3-framework/WU2.15-plan.md § The ruling |
| H013 | hypothesis | Instruction-stack effects on autonomous readers (label agreement between explicit-context and HITL-context arms) | hypotheses/013; hypotheses/049 |
| H047 | hypothesis | Pathfinder and slice-reader record sets are substantially disjoint | hypotheses/047 |
| H048 | hypothesis | Self-administered clauses pass non-diagnostic findings at a materially higher rate than a blind referee | hypotheses/048 |
| H049 | hypothesis | Instruction stack changes an agent's labels at a measurable rate | hypotheses/049 |
| H050 | hypothesis | Reading condition interacts with model | hypotheses/050 |

## Files

| id | path | keep | governed-by |
|---|---|---|---|
| f.plan | docs/v3-framework/forward-plan-N.md (active = highest N; retired carry a header stamp) | committed | forward-plans.md § Numbering and lifecycle |
| f.cons | docs/v3-framework/consolidation-N.md (write-once) | committed | consolidation.md § What it produces |
| f.rev | docs/v3-framework/methodology-revision-N.md (write-once) | committed | SKILL.md § Provenance |
| f.skill | .claude/skills/v3-buildout/*.md and .claude/skills/agent-runner/SKILL.md | committed | SKILL.md § Session routing |
| f.pool | docs/v3-framework/spec-pools/<corpus>.md (append-only entries; status line only changes) | committed | spec-pools/README.md |
| f.hyp | docs/v3-framework/hypotheses/NNN-slug.md | committed | hypothesis-records.md § Files and the index |
| f.index | docs/v3-framework/hypotheses/INDEX.md (id and slug only; no status) | committed | hypothesis-records.md § Files and the index |
| f.art | docs/v3-framework/WU<n>.<m>-<slug>.md or directory (write-once; corrections appended) | committed | wu-execution.md § WU artifacts |
| f.cand | fanout/WU<n>.<m>-<slug>/candidates.md (append-only) | committed | evidence-pipeline.md § The candidates file |
| f.itcand | fanout/referee/iterations/NNN-<date>/candidates.md | committed | evidence-pipeline.md § Iteration → re-referee |
| f.impl | docs/v3-framework/implementation-candidates.md | committed | SKILL.md § Provenance |
| f.corpus | .claude/skills/v3-buildout/CORPUS-STATUS.md | committed | SKILL.md § Context documents |
| f.proto | fanout/<work>/protocol.md (reading protocol; piloted, not calibrated) | committed | forward-plan-2.md § Codebooks this plan names |
| f.cb | fanout/<work>/codebook.md (hashed by the runner) | committed | agent-runner SKILL.md § Layout |
| f.cal | fanout/<work>/calibration-<date>.md | committed | fanout/referee/codebook.md § Calibration |
| f.sheet | fanout/<work>/<run>/calibration-sheet.md (Brian's blind verdicts) | committed | docs/v3-framework/WU2.15-plan.md § Step 3 |
| f.man | read-manifest.md (arm label → condition, model; closed until binning) | committed | wu-execution.md § Design rules |
| f.items | fanout/<work>/<run>/items/*.md (regenerable bodies) | gitignored | agent-runner SKILL.md § What a run commits |
| f.manifest | fanout/<work>/<run>/items/manifest.md | committed | agent-runner SKILL.md § What a run commits |
| f.exc | fanout/<work>/<run>/excerpts/ (non-regenerable experiment inputs) | committed | agent-runner SKILL.md § What a run commits |
| f.makejobs | fanout/<work>/make-jobs.* | committed | agent-runner SKILL.md § A well-formed job |
| f.tally | fanout/<work>/tally.* | committed | agent-runner SKILL.md § A well-formed job |
| f.jobs | fanout/<work>/<run>/jobs.json (generated; _comment stamp) | committed | agent-runner SKILL.md § The job file |
| f.led | fanout/<work>/<run>/ledger.jsonl (one row per attempt) | committed | agent-runner SKILL.md § Invariants |
| f.att | fanout/<work>/<run>/attempts/<id>/attempt-N/ (prompt.md, stream.jsonl) | gitignored | agent-runner SKILL.md § What a run commits |
| f.res | fanout/<work>/<run>/results/ | committed | agent-runner SKILL.md § What a run commits |
| f.runmd | fanout/<work>/<run>/run.md | committed | fanout/PROTOCOL.md § run.md |
| f.hostlog | fanout/host-log.txt | gitignored | agent-runner SKILL.md § The host and its page |
| f.mcpcfg | tools/StoryPlanner.AgentRunner/configs/storyplanner-mcp.json | committed | agent-runner SKILL.md § The job file |
| f.src | the corpora: story markdowns, own fiction, analyses, attribution.csv, the .storyplan via MCP, lineage.db, codesessions.db, the Keep export | outside-repo | CORPUS-STATUS.md |
| f.storyplan | Brian's .storyplan files (never written by the buildout) | outside-repo | CLAUDE.md § Brian's decisions |

## Processes

| id | level | kind | process | actor | inputs | outputs | roots | governed-by | state |
|---|---|---|---|---|---|---|---|---|---|
| P.1 | P | sop | Consolidation: merge, split, cluster, supersede; statuses re-derived; pools and plan re-pointed | hitl:fable | f.index f.hyp f.plan f.pool | f.hyp f.cons f.index f.pool | C1 C2 I6 | consolidation.md § What it does | exists |
| P.2 | P | sop | Forward plan creation or revision; retire the predecessor with a header stamp | hitl:fable | f.index f.hyp f.cons f.rev f.pool | f.plan | C3 I7 | forward-plans.md § Writing the plan | exists |
| P.3 | P | sop | Read one card; take its Type, Corpus and Scale as the WU's cell | hitl:fable | f.plan | f.plan | C3 | forward-plans.md § Structure of the agenda | exists |
| P.6 | P | sop | Post-WU review with Brian: challenge against the source; enrichment as questions; statement changes batched | hitl:fable | f.art f.src f.hyp | f.art f.pool | C8 C9 G1 | wu-execution.md § 4. Post-WU review | exists |
| P.6b | P | sop | Brian challenges or enriches a finding | brian | f.art | f.pool | C1 C8 | wu-execution.md § 4. Post-WU review | exists |
| P.7 | P | reactive | Methodology revision: rewrite skill files; record what changed and why | hitl:fable | f.skill f.rev | f.skill f.rev | I1 I4 I8 | methodology-revision-1.md (record only; no governing protocol) | contradictory |
| P.8 | P | sop | Iteration → re-referee: statement change; old entries become candidates; re-promote survivors; mark superseded | hitl:fable | f.hyp | f.hyp f.itcand | G2 G3 I3 | evidence-pipeline.md § Iteration → re-referee | specified |
| P.9 | P | sop | Baselining: Brian's dated judgment that the evidence picture is sufficient to act on | brian | f.hyp | f.hyp | C1 | SKILL.md § Epistemic framework | exists |
| P.10 | P | sop | Status board and card Status updated by hand in the same commit as the card | hitl:fable | f.plan | f.plan | C3 I6 | forward-plans.md § Ordering is structural | contradictory |
| E.1 | E | sop | Scope reconciliation: confirm protocol and arms against the card and CORPUS-STATUS | hitl:fable | f.plan f.corpus f.proto | f.plan | C3 | wu-execution.md § 1. Scope reconciliation | exists |
| E.2 | E | sop | Plan mode: arms, explicit contexts, binning scheme, does-not list; Brian approves | hitl:fable | f.plan f.corpus | f.plan | C1 C5 | wu-execution.md § 2. Plan mode | exists |
| E.2b | E | sop | Brian approves the WU plan | brian | f.plan | f.plan | C1 | wu-execution.md § 2. Plan mode | exists |
| E.4a | E | sop | Pathfinder read of a whole corpus in one context; output is leads only | hitl:fable | f.src | f.art | C7 I5 H047 | SKILL.md § The work matrix | exists |
| E.4b | E | sop | Reading arm: one slice under an explicit reading protocol, blind to other arms | agent:opus | f.proto f.items | f.res | C5 C7 H047 H050 | wu-execution.md § Design rules for all types | specified |
| E.5 | E | sop | Join record sets on locus; bin disagreements before any is investigated; open the read-manifest after binning | hitl:fable | f.res f.man | f.art | G3 I7 H047 H049 H050 | wu-execution.md § Design rules for all types | specified |
| E.5b | E | sop | Brian adjudicates the drilled disagreements | brian | f.art f.src | f.art | C1 | wu-execution.md § Design rules for all types | specified |
| E.6 | E | sop | Write the exploratory artifact organised by what was observed; method section names protocol hash, arms, models, harness, manifest | hitl:fable | f.res f.art f.led | f.art | G1 C2 | wu-execution.md § WU artifacts | exists |
| E.7 | E | sop | Append questions to any corpus's spec pool | hitl:fable | f.art | f.pool | C2 C3 C8 | spec-pools/README.md | exists |
| E.8 | E | sop | Mark the card complete | hitl:fable | f.plan | f.plan | C3 | wu-execution.md § 3. Execution | exists |
| V.1 | V | sop | Scope reconciliation: the pool's open questions are the scope; hypothesis list recomputed from bears-on; uncalibrated codebooks flagged | hitl:fable | f.plan f.pool f.cb f.cal | f.plan | C3 C4 | wu-execution.md § 1. Scope reconciliation | exists |
| V.2 | V | sop | Plan mode: codebooks and hashes, job files, does-not list; Brian approves | hitl:fable | f.pool f.cb | f.plan | C1 C4 | wu-execution.md § 2. Plan mode | exists |
| V.c | V | sop | Classifier or auditor job: apply a calibrated codebook to one item | agent:sonnet | f.cb f.items f.exc | f.res | C4 C5 G3 | SKILL.md § The work matrix | exists |
| V.i | V | sop | Investigator or focused-reader job: fixed question, adaptive search, loci and dates as the answer; MCP only when the item is not pre-fetched | agent:opus | f.proto f.items f.mcpcfg f.src | f.res | C5 C7 | SKILL.md § The work matrix | specified |
| V.s | V | sop | Census: counts by script over CSV or MCP output; no LLM | script | f.src f.res | f.art f.cand | C8 C9 | forward-plan-2.md § WU2.6 | contradictory |
| V.h | V | sop | HITL-context arm of the 049 cell: same items and hash, the instruction stack deliberately present | hitl:fable | f.cb f.items | f.res | H013 H049 | forward-plan-2.md § WU2.6 | contradictory |
| V.9 | V | sop | Tally results into counts and flagged rows; adjudication reads this, never the raw batch | script | f.res | f.art | I5 | fanout/PROTOCOL.md § The steps 10 | exists |
| V.10 | V | sop | Write the verification artifact: method, counts, not-measured, pool questions answered | hitl:fable | f.res f.led f.art | f.art | G1 C3 | wu-execution.md § WU artifacts | specified |
| V.11 | V | sop | Write candidates: finding, source, proposed-by, one citable unit each; append-only | hitl:fable | f.res f.art | f.cand | G1 G2 C2 I1 | evidence-pipeline.md § The candidates file | specified |
| V.13 | V | sop | Copy each referee verdict from the referee run's results into the candidate | hitl:fable | f.res f.cand | f.cand | G2 I8 | docs/v3-framework/retroactive-referee-pass-handoff.md § Sequence 3 | contradictory |
| V.15 | V | sop | Report counts per target and class, promotions, declines, disagreements, pipeline behaviour | hitl:fable | f.cand f.hyp | f.art | G4 | evidence-pipeline.md § Promotion | specified |
| V.16 | V | sop | Wrap-up sweep over the full index; more candidates through the same referee only | hitl:fable | f.index f.art | f.cand | C2 C6 | wu-execution.md § 3. Execution | specified |
| S.1 | S | sop | Scope reconciliation: the debt status of every named corpus; unverified wants become questions and leave scope | hitl:fable | f.plan f.pool f.cand f.hyp | f.plan f.pool | C3 | wu-execution.md § 1. Scope reconciliation | exists |
| S.3 | S | sop | Read verified artifacts; exploratory artifacts as leads only | hitl:fable | f.art f.cand f.hyp | f.art | C3 G1 | wu-execution.md § Four types | specified |
| S.4 | S | sop | Write the synthesis artifact; every insight that bears on a hypothesis becomes a question | hitl:fable | f.art | f.art f.pool | C2 C7 | wu-execution.md § 3. Execution | specified |
| I.3a | I | sop | Build a tool, ingest or render with tests; update CORPUS-STATUS when it is a corpus | hitl:fable | f.src | f.corpus | C9 | wu-execution.md § Four types | exists |
| I.3b | I | sop | Author a codebook or reading protocol in a HITL session | hitl:fable | f.pool f.art | f.cb f.proto | C4 | wu-execution.md § Design rules for all types | exists |
| I.4a | I | sop | Calibration scoring by the agent: the sample scored blind under the draft hash | agent:sonnet | f.cb f.items | f.res | C4 G3 | fanout/referee/codebook.md § Calibration | contradictory |
| I.4b | I | sop | Calibration scoring by Brian, blind, on the sheet; the agent's results withheld until done | brian | f.sheet | f.sheet | C1 C4 G3 | fanout/referee/codebook.md § Calibration | specified |
| I.4c | I | sop | Adjudicate the two scorings; rulings edit the codebook (new hash); write the calibration record | hitl:fable | f.res f.sheet f.cb | f.cb f.cal | C4 | fanout/referee/codebook.md § Calibration | specified |
| R.1 | R | sop | Create the work folder under fanout, vertical by work | hitl:fable | f.plan | f.runmd | I4 | agent-runner SKILL.md § Layout | exists |
| R.3 | R | sop | Enumerate items once by a tool; regenerable into items with a manifest, otherwise into a committed folder | script | f.src | f.items f.manifest f.exc | I5 C5 | agent-runner SKILL.md § A well-formed job 2 | exists |
| R.4 | R | sop | Generate the job file from the manifest: one item per job, requireOnce, ceilings, neutral arm names | script | f.manifest f.cb f.proto | f.jobs | I5 I4 | agent-runner SKILL.md § A well-formed job 1 and 3 | exists |
| R.5 | R | sop | Dry run: compose and size every prompt, launch nothing | tool | f.jobs f.cb f.proto f.items f.exc | f.att | I5 | fanout/PROTOCOL.md § The steps 7 | exists |
| R.6 | R | sop | Pilot one job; its output read by a person; ledger row Mode pilot | tool | f.jobs | f.led f.res f.att | I5 | agent-runner SKILL.md § A well-formed job 4 | exists |
| R.6b | R | sop | A person reads the pilot output before any batch | hitl:fable | f.res | f.runmd | I5 | agent-runner SKILL.md § A well-formed job 4 | exists |
| R.7 | R | sop | Batch under the host: one child per job from the external launch folder; ledger as queue; maxAttempts; timeout; requireOnce check | tool | f.jobs f.mcpcfg | f.led f.att f.hostlog | I4 C5 | agent-runner SKILL.md § Invariants the runner enforces | exists |
| R.7c | R | sop | One child: protocol and inputs inlined and hashed; exact tools; writes the output path | agent:model-varies | f.cb f.proto f.items f.exc | f.res | C5 | agent-runner SKILL.md § What the agent sees | exists |
| R.8 | R | sop | Tally results by the work's tallier | script | f.res | f.art | I5 | fanout/PROTOCOL.md § The steps 10 | exists |
| R.9 | R | sop | Write run.md; commit per the convention | hitl:fable | f.led f.res | f.runmd | I4 | fanout/PROTOCOL.md § run.md | exists |
| F.1 | F | sop | Materialise the referee's inputs: the current statement and the candidate's finding and source lines, no clause | script | f.hyp f.cand | f.items | G3 I8 | evidence-pipeline.md § The referee | contradictory |
| F.4 | F | sop | Referee job: attempt the clause blind; verdict diagnostic supporting, diagnostic challenging, or non-diagnostic; two lines out | agent:sonnet | f.cb f.items | f.res | G2 G3 H048 I1 | fanout/referee/codebook.md § The task | specified |
| F.6 | F | sop | Tally referee results: class counts, malformed verdicts | script | f.res | f.art | I5 | docs/v3-framework/retroactive-referee-pass-handoff.md § Sequence 3 | unbuilt |
| M.1 | M | sop | Promotion session opens candidates, verdicts and hypothesis files | hitl:fable | f.cand f.res f.hyp | f.cand | G4 | evidence-pipeline.md § Promotion | specified |
| M.3 | M | sop | Read the cited source, not the finding, for each diagnostic candidate: the citation check | hitl:fable | f.cand f.src | f.cand | I2 G4 | evidence-pipeline.md § Promotion | specified |
| M.4 | M | sop | Write the evidence entry: finding and clause verbatim, WU and candidate id, codebook hash, tag | hitl:fable | f.cand | f.hyp | G1 G2 C2 | hypothesis-records.md § Record entries | specified |
| M.5 | M | sop | Brian adjudicates disagreements with the referee; may decline; may promote nothing | brian | f.cand f.src | f.cand | C1 G4 | evidence-pipeline.md § Promotion | specified |
| M.6 | M | sop | Append the disposition: promoted, declined with reason, held with the named blocker | hitl:fable | f.cand | f.cand | G1 G2 I3 | evidence-pipeline.md § The candidates file | specified |
| M.8 | M | sop | Recompute each touched hypothesis's status from its entries, in frontmatter only | hitl:fable | f.hyp | f.hyp | I6 C2 | hypothesis-records.md § Files and the index | specified |
| M.9 | M | sop | One commit naming the WU and candidate ids; the diff is the review surface | hitl:fable | f.hyp f.cand | f.hyp | G4 | evidence-pipeline.md § Promotion | specified |
| M.10 | M | sop | Brian reviews the promotion diff | brian | f.hyp | f.hyp | C1 G4 | evidence-pipeline.md § Promotion | specified |
| B.1 | V | bootstrap | Retroactive candidates: one per pre-revision evidence entry, finding and source verbatim, original clause withheld | hitl:fable | f.hyp | f.cand | I1 I2 H048 | docs/v3-framework/retroactive-referee-pass-handoff.md § Sequence 2 | specified |
| B.2 | V | bootstrap | The 048 table: blind clause against original clause, five bins | hitl:fable | f.cand f.res f.hyp | f.art f.cand | H048 | docs/v3-framework/WU2.15-plan.md § Step 3 | specified |

## Edges

| from | to | kind | label |
|---|---|---|---|
| P.1 | P.2 | flow | consolidation always requires a new plan |
| P.7 | P.2 | flow | priority reassessment |
| P.2 | P.3 | flow | one card per WU |
| P.3 | E.1 | choice | Type exploratory |
| P.3 | V.1 | choice | Type verification |
| P.3 | S.1 | choice | Type synthesis |
| P.3 | I.3a | choice | Type infrastructure |
| E.1 | E.2 | flow | |
| E.2 | E.2b | flow | Brian approves |
| E.2b | E.4a | choice | Scale pathfinder |
| E.2b | R.1 | choice | Scale slice-reader arms |
| R.1 | E.4b | fork | one arm per slice and model |
| E.4b | E.5 | join | record sets joined on locus |
| E.5 | E.5b | flow | drills |
| E.5b | E.6 | flow | |
| E.4a | E.6 | flow | leads only |
| E.6 | E.7 | flow | |
| E.7 | E.8 | flow | |
| E.8 | P.6 | flow | |
| V.1 | V.2 | flow | |
| V.2 | I.3b | choice | codebook missing |
| I.3b | I.4a | fork | agent scores blind |
| I.3b | I.4b | fork | Brian scores blind |
| I.4a | I.4c | join | |
| I.4b | I.4c | join | |
| I.4c | I.4a | optional | a ruling that edits the codebook re-runs the sample |
| I.4c | R.1 | flow | calibrated |
| V.2 | R.1 | choice | codebook calibrated |
| R.1 | R.3 | flow | |
| R.3 | R.4 | flow | |
| R.4 | R.5 | flow | |
| R.5 | R.6 | flow | |
| R.6 | R.6b | flow | |
| R.6b | R.7 | flow | |
| R.7 | R.7c | fork | one child per job |
| R.7c | R.7 | join | attempt recorded; retry until maxAttempts |
| R.7 | R.8 | flow | |
| R.8 | R.9 | flow | |
| R.7c | V.c | choice | cell classifier or auditor |
| R.7c | V.i | choice | cell investigator or focused reader |
| R.7c | E.4b | choice | exploratory arm |
| R.7c | F.4 | choice | referee run |
| R.7c | I.4a | choice | calibration run |
| V.2 | V.s | choice | census, no LLM |
| V.2 | V.h | optional | the 049 cell only |
| V.9 | V.10 | flow | |
| V.10 | V.11 | flow | |
| V.s | V.11 | flow | |
| V.11 | F.1 | flow | referee run under fanout/referee |
| F.1 | R.1 | flow | a nested runner run |
| F.4 | F.6 | flow | |
| F.6 | V.13 | flow | |
| V.13 | M.1 | flow | |
| M.1 | M.3 | choice | diagnostic |
| M.1 | M.6 | choice | non-diagnostic stays as context |
| M.3 | M.4 | choice | finding holds at source |
| M.3 | M.6 | choice | declined with reason |
| M.5 | M.6 | optional | Brian adjudicates a disagreement |
| M.4 | M.6 | flow | |
| M.6 | M.8 | flow | |
| M.8 | M.9 | flow | |
| M.9 | M.10 | flow | |
| M.10 | V.15 | flow | |
| V.15 | V.16 | flow | |
| V.16 | F.1 | optional | more candidates, same referee |
| V.16 | P.6 | flow | |
| S.1 | S.3 | flow | every relied-on question verified |
| S.3 | S.4 | flow | |
| S.4 | P.6 | flow | |
| P.6 | P.6b | flow | |
| P.6b | P.8 | optional | a statement change |
| P.8 | F.1 | flow | old entries as candidates |
| P.9 | P.1 | optional | Brian's demand |
| B.1 | F.1 | flow | |
| B.2 | F.1 | flow | the table is itself a candidate |

## Bootstrap rows and what retires them

| row | retired by |
|---|---|
| B.1 | WU2.15 promotion commit |
| B.2 | WU2.15 promotion commit |

## Generated

<!-- generated:level-1 -->
<!-- /generated -->

<!-- generated:level-2 -->
<!-- /generated -->

<!-- generated:level-3 -->
<!-- /generated -->

<!-- generated:consumers -->
<!-- /generated -->

<!-- generated:validation -->
<!-- /generated -->

## Known gaps carried in (from process-map-1-draft.md G1–G24, to be re-homed to rows by the validator pass)

G1 → F.1 (contradictory: text says three inputs; ruling says two). G2 → V.13. G3 → F.6 and
the referee's make-jobs (unbuilt). G4 → f.exc's governing example. G5 → R.* per-type
applicability. G6 → I8 (elided plan-mode answers). G7 → I.3b vs V.2 (codebook ownership).
G8 → V.s (script-produced candidates' proposed-by). G9 → V.i (what an investigator records for
M.3). G10 → P.7. G11 → I.4a (a calibration run under the draft hash vs "no batch under an
uncalibrated hash"). G12 → I.4b. G13 → f.cal (stage detector trusts any record). G14 → f.cal
(per hash or per hash and model). G15 → E.4b outputs (two homes). G16 → f.pool status has no
writer (M.6 proposed). G17 → P.10 vs M.9. G18 → wu-execution "one HITL session". G19 → V.h.
G20 → f.proto pilot criterion. G21 → M.3 not recorded on the disposition. G22 → f.cb status
line. G23 → plan-mode rulings unrecorded (f.plan). G24 → the kind column (now present).
