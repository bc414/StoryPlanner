# Methodology revision 2 — omissions list (draft, from the supersession audit)

Draft written 2026-09-05 by the session adjudicating with Brian; folded into the write-once
`methodology-revision-2.md` at handoff step 5 and retired then. Source: the audit run
`fanout/skill-audits/2026-09-05-v3-buildout-2/` (46 jobs, ledger `ledger.jsonl`, tally `tally.md`;
harness 2.1.258, Sonnet, one arm). Document A: the six rule files of revision 1, `fanout/PROTOCOL.md`
and `spec-pools/README.md`, 179 units; set B: the fifteen files of `v3-buildout-2`, the old
`CORPUS-STATUS.md`, the agent-runner skill.

Per revising-the-method § revise: a unit the audit reports narrowed, reversed or absent gets one
line with Brian's ruling: superseded, and by which ruling, or restored to the new text. Units
reported restated (45), broadened (10) or non-instructional (5) get no line. The auditor's note
for every unit is in `tally.md`; this list cites it and does not repeat it.

**Status of this draft:** the "superseded" groups are the session's assignment of each unit to a
ruling already in the rulings log, for Brian's confirmation; the "open" list is what no ruling
covers, with the session's proposal, for his ruling. A ruling he makes is recorded on the line as
`Brian:` and in the rulings log.

Flagged: 119: 82 narrowed, 17 reversed, 20 absent. Assigned to a ruling: 105. Open: 14.

## Open: for Brian's ruling

### unit-065 · narrowed · ## Ceremony scaling
- audit: **Significant** (scope changed; evidence prompted a rethink): full iteration entry with …
- **Amending the created entry after a reframing.** Revision 1 allowed it; rule 9 says appended artifacts are never edited. Proposal: superseded by rule 9; a misleading created entry is answered by the iteration entry that follows it.
- Brian: *pending*

### unit-067 · narrowed · ## Challenging a hypothesis
- audit: A challenge is evidence: a specific counterexample with ids …
- **No channel for Brian's own counterexample.** Revision 1 let a challenge be evidence directly; B routes everything through a round. Proposal: keep the strong form (his counterexample enters the corpus's question list and a round produces the candidate) unless he wants an hitl-authored candidate that still passes the referee.
- alternatives: (a) question list, then a round: the strong form holds and the record never holds an unrefereed entry; (b) an hitl-authored candidate marked asked-by Brian, still refereed and promoted like any other, with new prose in promoting-checked-candidates and a proposed-by form for a person; (c) a direct challenging entry with no referee line, as revision 1, reversing rule 2 and "an entry without a falsifier is malformed".
- Brian: *pending*

### unit-043 · narrowed · ## Context documents
- audit: Two files travel with this skill and are facts, not rules: `VERSION-HISTORY.md` (dated project timeline — no interpretive claims) and `CORPUS-STATUS.md` (what material exists and its state). …
- **VERSION-HISTORY.md has no home.** It sits in the old skill folder only; the new folder's validator rejects an unknown .md, and step 5 deletes the old folder. Proposal: move it to docs/v3-framework/VERSION-HISTORY.md at the swap and name it in SKILL.md § Provenance as a fact file.
- alternatives: (a) docs/v3-framework at the swap, named in § Provenance, no schema change; (b) into the new skill folder, a validator change (a known non-activity file) plus a § Companions line; (c) retire it, git keeps the text, the line records it as dropped by ruling.
- Brian: *pending*

### unit-108 · narrowed · ## Design rules for all types
- audit: **One factor at a time.**…
- **Factorial intent for a mixed arm design.** Dropped with the design rules. Proposal: restore one clause in explore-plan: one factor varies across arms, and a design that varies two declares factorial intent or is uninterpretable.
- Brian: *pending*

### unit-096 · narrowed · ## Entries this pipeline did not produce
- audit: An evidence entry with no referee line and no codebook hash was not produced by this pipeline…
- **Pre-pipeline record entries have no stated fate.** B says an entry without a falsifier is malformed, but every existing record holds such entries, and handoff step 6 plans their re-queue as referee-1's first input. Proposal: one sentence in artifacts.md § Hypothesis file (entries written before the referee existed count toward no status and are re-queued as candidates for the first round), and the state verb ignores them when it implies a status.
- alternatives: (a) the sentence, and the state verb skips entries without a falsifier (a small tool change with a fixture; engineering handoff ProcessMap 3); (b) the sentence only, the mismatch flag showing on every old hypothesis until referee-1 has run; (c) no rule text, referee-1's own plan and run.md carry it, the line records the omission as deliberate.
- Brian: *pending*

### unit-010 · narrowed · ## Epistemic framework (applied)
- audit: **Recall is atmosphere; evidence is grounding.** A statement about the data from anyone…
- **The recall rule's catch-all.** "anything else" and "not limited to" dropped. Proposal: restore the two words; trivial.
- Brian: *pending*

### unit-072 · narrowed · ## Goal
- audit: Vocabulary the pipeline depends on: a **finding** is what a pass observed…
- **"candidate" is not in § Vocabulary.** Defined only by artifacts.md § Candidate. Proposal: add one vocabulary line.
- Brian: *pending*

### unit-092 · narrowed · ## Promotion
- audit: Report to Brian, after the commit — at least: candidates per target…
- **The promotion summary names no undecided count.** held is gone by ruling, but the count of candidates left without an outcome, and whether the distribution warrants a recheck, are not in § Promotion. Proposal: restore the undecided count; the recheck is Brian's call and needs no rule.
- Brian: *pending*

### unit-050 · narrowed · ## Record entries
- audit: Four kinds, each one citable unit; a two-finding entry is two entries…
- **A two-finding entry is two entries.** Stated for candidates in B, not for record entries. Proposal: restore the clause in artifacts.md § Hypothesis file.
- Brian: *pending*

### unit-056 · narrowed · ## Record entries
- audit: Grep: `^- created`, `^- evidence`, `^- iteration`, `^- baselined`, `\[challenging\]`.
- **No grep for the challenging tag.** Proposal: restore the challenging-tag grep to the list in § Hypothesis file.
- Brian: *pending*

### unit-176 · narrowed · # Spec pools
- audit: One file per corpus (`<corpus>.md`). The seven corpora of the buildout as of 2026-09-03, …
- **CORPUS-STATUS.md does not name the corpora as ids.** artifacts.md says a corpus name comes from CORPUS-STATUS.md, but its headings are prose and the ids in use are the spec-pool filenames (analysis-corpus, own-fiction, v1-archive, working-plan, lineage, conversations, code-sessions). Proposal: CORPUS-STATUS.md carries the id beside each corpus, at step 4 with the spec-pools move.
- alternatives: (a) the id beside each corpus section in CORPUS-STATUS.md, a fact-file edit; (b) the question-list filenames are the ids and artifacts.md points at questions/ for the name, so a corpus with no list has no id; (c) an id · name · state table at the top of CORPUS-STATUS.md that the state verb could later read.
- Brian: *pending*

### unit-122 · reversed · ## The four phases
- audit: Infrastructure | Build; test; calibrate if it is a codebook…
- **building-a-tool may not write a question.** Its Never forbids writing a question, though build is hitl and every hitl activity may write into any corpus's list. Proposal: restore; writes gains question-list and the Never loses "a question".
- Brian: *pending*

### unit-125 · narrowed · ## The four phases
- audit: *Challenge* — Brian questions a finding…
- **A corrected finding's re-entry.** Revision 1 had a corrected finding re-enter as a new candidate through the referee. B: a challenged lead gets a dated correction; a candidate whose source read fails is declined. Proposal: a corrected observation is a new candidate only if the round's results carry it (write-candidates), otherwise a question; state it in promote.
- Brian: *pending*

### unit-045 · narrowed · ## What this skill does not govern
- audit: Among other things: story content decisions; prose technique (the planner specifies goals and mechanisms, never how to write); planner features…
- **FEATURE-AUDIT check before a feature proposal.** B names FEATURE-AUDIT as governance but drops the instruction to check it first. Proposal: restore the clause in § What this skill does not govern.
- Brian: *pending*

## Raised in session, outside document A: for the note's "not adopted" and "owed"

Not units of the audit; things the session noticed on 2026-09-05 while checking what the
old setup held outside its skill folder. Each is a line for the revision note once Brian
has ruled, and none is recorded anywhere else.

- **Which step carries the codebook text fixes of ruling 6.** Handoff 1 (ruling 6, still
  current) makes the referee codebook's text fixes part of this revision: inputs by
  reference to the row, R2 recast, R5 and R6 removed, E1 to E5 removed. Handoff 2 step 4
  names only the status line and the inputs section. The new format has the referee's
  codebook authored afresh as `codebook-1.md` in referee-1. Which of the three carries the
  fixes is unruled; a session could take any one and the others would assume it done.
- **The `v1-archive-mining` skill.** A reading protocol in everything but location, named
  by the old CORPUS-STATUS.md and by nothing in the new skill. Whether the exploration of
  the v1 archive authors it as `protocol-1.md` under its instance or keeps the skill is a
  preparing-to-explore decision; recorded so it is not lost in between.
- **Three carry-forward items of `WU2.15-plan.md` with no home in the new text:** the
  referee's four-goal purpose test (the new Never carries only its narrow form); the rule
  that a worked example silently carries a rule (the Anchors format answers it in
  structure only); and copying approved plan-mode files into the repo, partly overtaken
  since the code-sessions ingest keeps AskUserQuestion answers.
- **Revision-1 vocabulary outside the skill folder,** to change at the swap: CLAUDE.md's
  third-role paragraph ("when a cell calls for one") and its v3 references, and the memory
  file's router pointer. Listed in handoff 2 step 5.

## Draft 1's gaps: disposition (handoff 2 step 4, drafted 2026-09-06)

The node comparison handoff 2 once named for this step is void: draft 1
(`git show 32b6d4b:docs/v3-framework/process-map-1-draft.md`) has 109 node ids and the new
`map.md` 67, none shared, because draft 1 abbreviated its ids and the map uses slugs; the
audit above already mapped the text unit by unit. What draft 1 holds that document A did
not is its § Gaps. One line per gap, the session's assessment for Brian's confirmation:
closed (by which ruling or text), dissolved (the frame that carried it retired), partly
(what remains), or open.

| gap | draft 1 said | disposition |
|---|---|---|
| G1 | the referee is given an excerpt; R2, R5, R6 act on it | closed by ruling 6 (two inputs); the codebook's own text fix is step 5's open choice |
| G2 | who appends the referee's verdict to the candidate | closed: `referee-append` (session) copies each result's two lines under its candidate; a runner child never appends |
| G3 | the referee's generator and tallier do not exist | still unbuilt; referee-1's `author-codebook` writes them |
| G4 | the agent-runner skill's excerpt example | closed 2026-09-05 (the skill's commit-convention bullet) |
| G5 | PROTOCOL.md presents one lifecycle for every type | dissolved: the order is derived per activity; PROTOCOL.md retired |
| G6 | the 2026-09-03 plan-mode rounds are elided from the archive | open as history: the ingest keeps such answers since 2026-09-04; those two rounds stay lost |
| G7 | three owners named for authoring and calibrating a codebook | closed: preparing-to-verify's `author-codebook` and `calibrate`; building-a-tool's Never forbids it |
| G8 | a script-produced (census) candidate cites nothing | dissolved: census retired; write-candidates never writes a script's count with no item behind it |
| G9 | what an investigator must record for promotion's read | dissolved: investigator retired; the candidate's `source` locator and promote's step 1 |
| G10 | no text governs a methodology revision | closed: revising-the-method, with two lints |
| G11 | the calibration run needs a name distinct from a batch | partly: `calibrate-run` is the codebook's pilot under the draft hash; the ledger's mode for it is unstated |
| G12 | the mechanics of Brian's blind scoring | closed in `calibrate` (verdicts written as given, results withheld until his are complete); no scoring-sheet artifact is named |
| G13 | the stage strip marks "calibrated" on any record beside a codebook | open: the host still checks any `calibration-*.md`, not one at the current hash (the state verb checks the hash); an engineering item |
| G14 | calibration per hash, or per hash and model | partly: the record names the model that scored; that a different model needs its own record is unstated |
| G15 | two homes for exploratory arm outputs | closed: `results/` under the run in fanout; `leads.md` under docs |
| G16 | no process updates a pool entry's status after a round | closed: `answered-by` and `frozen-into` are derived into state.md, never written |
| G17 | whether the card's status rides in the promotion commit | dissolved: no cards; state is generated |
| G18 | "every WU runs four phases in one session" against multi-session WUs | dissolved: chains of separately typed processes |
| G19 | the HITL-context arm of 049 has no mechanism | open: none under rule 5; the instance that wants it must say how, or drop the arm |
| G20 | what a pilot must show for a protocol to proceed | closed: `pilot-run`, Brian reads the record set and rules; his verdict in run.md |
| G21 | promotion's source read is not recorded | partly: `promote` reads the source and reports before any decision; the promoted outcome line carries no field for it |
| G22 | the codebook's status line and its place in the hash | closed by ruling: no status line; calibrated means a record at the hash |
| G23 | plan-mode rulings leave no durable record | partly: rulings logs live in the repo; AskUserQuestion answers are kept by the ingest; approved plan files still sit outside |
| G24 | SOP / one-time / reactive tagging per step | dissolved by ruling: mode by decision; bootstrap is a fact about instances, never about activities |

The level-1 model change, for the note: draft 1's cycle — consolidation on demand, a
forward plan, a card typed by a matrix cell, four phases in one session, a post-WU review,
iteration and immediate re-referee — became thirteen activities in an enables graph, run as
registered instances of two chains, each process typed by who decides in it, what is here
generated into state.md, and the pick Brian's.

## Superseded by a ruling: for Brian's confirmation

### Iteration semantics (9)

Entries are dated and never invalidated; an iteration entry is a wording boundary; status derives from current-wording entries and may return to untested; prior findings become iteration candidates consumed by the next round; no superseded marker; no immediate re-referee (rulings log § iterating-a-statement; artifacts.md § Hypothesis file).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-064 | reversed | ## Ceremony scaling | **Minor** (wording tightened, no conceptual change): edit the statement; one-line … |
| unit-066 | reversed | ## Ceremony scaling | **Structural** (split, merge, supersede): new file(s); final iteration entry in the … |
| unit-006 | narrowed | ## Epistemic framework (applied) | `untested` — no verified evidence in the record. |
| unit-007 | narrowed | ## Epistemic framework (applied) | `evidenced` — verified evidence currently supports the statement; thin or thick, the record shows the weight; always revisable. |
| unit-008 | narrowed | ## Epistemic framework (applied) | `challenged` — at least one verified challenging entry is unresolved. One open challenge puts the hypothesis here regardless of how much support also exists. |
| unit-009 | reversed | ## Epistemic framework (applied) | Transitions are reversible except one: once verified evidence exists a hypothesis never returns to `untested`… |
| unit-093 | reversed | ## Iteration → re-referee | When a statement changes (an iteration entry), every evidence entry under the old wording is a candidate again… |
| unit-054 | narrowed | ## Record entries | **`iteration`** — a change to the statement… |
| unit-153 | narrowed | ## What it does | Structural changes follow `hypothesis-records.md` § Ceremony scaling… |

### Consolidation retired (6)

consolidating-the-hypothesis-set retires; merges and splits are minting plus iterating in any hitl session; a structural change was a priority reassessment, and there is no plan to reassess (rulings log).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-149 | narrowed | # Consolidation | Read in full before restructuring the hypothesis set. … |
| unit-040 | absent | ## Session routing — read the named file in full before acting | Consolidation |
| unit-150 | absent | ## Trigger and scope | On Brian's demand … |
| unit-151 | narrowed | ## What it does | Merges (one claim in two wordings)… |
| unit-154 | absent | ## What it does | Re-points spec-pool entries and forward-plan references to the surviving ids. |
| unit-157 | absent | ## What it produces | `docs/v3-framework/consolidation-N.md` — write-once, the set-level provenance: what merged, split, was archived or created, why, and what the landscape looks like now. … |

### Spec pools became question lists (11)

Vocabulary: question and question list replace spec and spec pool; questions are written only by hitl processes; answered-by and folded-into are derived into state.md, never written; a round that finds its codebook wanting stops and records it, and the question is Brian's to raise in the promotion session (rulings log; constitutional rule 4).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-025 | narrowed | ## Constitutional rules | **Strong form: only verification passes produce evidence.**… |
| unit-027 | narrowed | ## Constitutional rules | **A codebook is an instrument.**… |
| unit-109 | narrowed | ## Design rules for all types | **Codebooks are not written by the pass that applies them.**… |
| unit-177 | reversed | # Spec pools | An entry is one question: … |
| unit-178 | narrowed | # Spec pools | Rules: questions flow freely — any pass may add to any corpus's pool; findings wait — … |
| unit-161 | narrowed | ## The steps | / 1 / **Question** / HITL session / an entry in `docs/v3-framework/spec-pools/<corpus>.md` / … |
| unit-094 | narrowed | ## Verification debt and codebook versioning | A corpus's exploratory pass opens a debt paid only by its verification pass. … |
| unit-060 | narrowed | ## What never enters a record | Observations that do not change the statement (a spec-pool question, or nothing). |
| unit-073 | narrowed | ## Where each kind of write goes | A finding of an exploratory pass … |
| unit-074 | narrowed | ## Where each kind of write goes | A question raised about a corpus … |
| unit-075 | narrowed | ## Where each kind of write goes | A candidate (a finding that a verification pass claims bears on a hypothesis) … |

### Activities replace WUs, cards and the four phases (25)

One activity file per step, mode by decision; post-WU review is not uniform (exploration's review is reviewing-leads, verification's is promotion, a tool's acceptance is inside building); the router table replaces session routing (rulings log, same section).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-068 | narrowed | ## Creating a hypothesis | The same file protocol applies whether one hypothesis is created in conversation or forty in a consolidation… |
| unit-098 | narrowed | ## Four types | A WU is one of four types, and the card's `Type` field says which… |
| unit-099 | narrowed | ## Four types | / **Exploratory** / A raw corpus, discovery-first, with no hypothesis in view /… |
| unit-100 | narrowed | ## Four types | / **Verification** / One corpus's spec pool, and the **source itself**… |
| unit-103 | narrowed | ## Four types | Every write that could reach a hypothesis record goes through `evidence-pipeline.md`… |
| unit-046 | narrowed | # Hypothesis records | Read in full before touching any file in `docs/v3-framework/hypotheses/`. … |
| unit-032 | narrowed | ## Session routing — read the named file in full before acting | Hypothesis iteration (e.g. rewording, re-tagging, statement sweep) … |
| unit-033 | narrowed | ## Session routing — read the named file in full before acting | Promotion session (turning referee-checked candidates into record entries) … |
| unit-034 | narrowed | ## Session routing — read the named file in full before acting | Exploratory WU (e.g. discovery reading, pathfinding, slice reading) … |
| unit-035 | narrowed | ## Session routing — read the named file in full before acting | Verification WU (classifier / investigator / auditor fanout under a codebook, or any other frozen-predicate work) … |
| unit-037 | narrowed | ## Session routing — read the named file in full before acting | Infrastructure WU (an instrument, ingest, codebook or calibration to build) … |
| unit-038 | absent | ## Session routing — read the named file in full before acting | Post-WU review with Brian |
| unit-041 | narrowed | ## Session routing — read the named file in full before acting | Ad hoc conversation about the framework |
| unit-111 | absent | ## The four phases | Every WU of every type runs the same four phases in one HITL session (Fable): scope reconciliation, plan mode, execution, post-WU review… |
| unit-112 | absent | ## The four phases | **1. Scope reconciliation** (auto mode)… |
| unit-113 | absent | ## The four phases | Exploratory / The card; the corpus's `CORPUS-STATUS.md` entry; the reading protocol; the arm design… |
| unit-114 | narrowed | ## The four phases | Verification / The card; the corpus's spec pool; the named codebooks and their calibration records… |
| unit-116 | narrowed | ## The four phases | Infrastructure / The card; the thing to build and what consumes it… |
| unit-117 | narrowed | ## The four phases | **2. Plan mode** (all types)… |
| unit-119 | narrowed | ## The four phases | Exploratory / Run the arms; bin disagreements if there is more than one… |
| unit-120 | narrowed | ## The four phases | Verification / Run the jobs; write the artifact; write the candidates file… |
| unit-123 | narrowed | ## The four phases | New-hypothesis proposals are held for the wrap-up in every type and offered only when novelty, testability and independence all hold. |
| unit-124 | narrowed | ## The four phases | **4. Post-WU review** with Brian, same session, two interleaving modes for all types, with one per-type difference in what "verify against the source" means: |
| unit-127 | narrowed | ## The four phases | Statement changes are batched at the end of the review, then handled as iterations (`evidence-pipeline.md` § Iteration)… |
| unit-128 | narrowed | ## WU artifacts | `docs/v3-framework/WU<plan>.<unit>-<slug>.md` or a directory of that name when the WU has several files (records, renders, manifests)… |

### The work matrix retired (12)

The work matrix retires as a routing device; its functions are carried by mode and activity shape; cell names survive only as descriptions of agent rows; the investigator and focused-reader cells retire (rulings log, 2026-09-04 evening to 2026-09-05).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-105 | narrowed | ## Design rules for all types | **Scale is a cell, not a token count.**… |
| unit-001 | narrowed | (frontmatter) | Methodology for the v3 narrative design framework buildout, revision 1 (2026-09-03) — the work matrix (instrument / classifier / investigator / auditor / focused reader / slice reader / census / pathfinder), … |
| unit-173 | narrowed | ## `run.md` — the front page of a run | Small, authored, committed, rendered at the top of the run's page… |
| unit-162 | absent | ## The steps | / 2 / **Cell and WU type** / HITL / the WU card: `Type`, `Corpus`, `Scale` (= the matrix cell) / … |
| unit-011 | absent | ## The work matrix | Every piece of buildout work sits in one cell of a 3 × 3 matrix… |
| unit-012 | narrowed | ## The work matrix | **item** — e.g. one note, passage, claim, candidate… |
| unit-013 | narrowed | ## The work matrix | **slice** — e.g. one story, arc, analysis section… |
| unit-014 | narrowed | ## The work matrix | **corpus** — everything, one context… |
| unit-016 | absent | ## The work matrix | The cell names are the current set… |
| unit-017 | absent | ## The work matrix | Orthogonal to the cell: **role** — *generate* (produce records, labels, findings) or… |
| unit-020 | narrowed | ## The work matrix | **Model, as revisable doctrine (2026-09-03), not a finding:** Fable for HITL design and… |
| unit-021 | narrowed | ## The work matrix | **Verification owed.** The more discretion, the more checking downstream… |

### PROTOCOL.md retired (2)

fanout/PROTOCOL.md retires; the order is the map's; its rules moved into the agent-runner skill on 2026-09-05 (rulings log § step 4's first move).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-110 | narrowed | ## Design rules for all types | **Every autonomous job is a runner job.**… |
| unit-160 | reversed | # The experiment lifecycle | One sequence, from a question to a promoted evidence entry, served by the agent runner's host at `/protocol` and kept in the repo as `fanout/PROTOCOL.md`. … |

### Rationale moved to provenance (7)

The rewrite keeps the rule and moves rationale, examples and history to the provenance files (the rulings log, the revision notes); no instruction was lost, only its justification (SKILL.md § Provenance).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-004 | narrowed | ## Epistemic framework (applied) | **Scope.** Two coupled but separable domains: the *narrative design framework* … |
| unit-047 | narrowed | ## Files and the index | `docs/v3-framework/hypotheses/NNN-slug.md` — `NNN` a zero-padded three-digit id, stable, … |
| unit-091 | narrowed | ## Promotion | A HITL session, Fable, with the candidates file, the referee verdicts, and the hypothesis files open… |
| unit-044 | narrowed | ## Provenance | `docs/v3-framework/` holds the buildout's record, including: `hypotheses/` (the working instrument), … |
| unit-051 | narrowed | ## Record entries | **`created`** — always first… |
| unit-003 | narrowed | # V3 framework buildout — revision 1 | Why revision 1 exists, and what it replaced, is recorded once in… |
| unit-095 | narrowed | ## Verification debt and codebook versioning | Every verification result cites the codebook hash it was produced under. … |

### The forward plan retired (18)

writing-a-forward-plan retires; the instance registry and the generated state.md replace it; there is no plan and the pick is Brian's; a tool is built as the first task inside the instance that needs it (rulings log).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-048 | narrowed | ## Files and the index | `status` ∈ `untested / evidenced / challenged`; `baselined` is `false` or an ISO date; … |
| unit-130 | reversed | # Forward plans | Read in full before creating, revising or retiring a forward plan… |
| unit-131 | reversed | ## Numbering and lifecycle | `docs/v3-framework/forward-plan-N.md`; the active plan is the highest number… |
| unit-132 | absent | ## Numbering and lifecycle | Two creation triggers… |
| unit-141 | narrowed | ## Ordering is structural, not derived | Exploratory passes consume no other WU — they read a raw corpus — and are unordered among themselves.… |
| unit-145 | reversed | ## Ordering is structural, not derived | The plan's execution section is a status board, not a sequence… |
| unit-146 | absent | ## Ordering is structural, not derived | Hypothesis status is advice for choosing among independent exploratory passes: `challenged` first… |
| unit-039 | reversed | ## Session routing — read the named file in full before acting | Forward plan creation or revision |
| unit-133 | reversed | ## Structure of the agenda | The plan is comprehensive — every hypothesis is targeted by at least one WU — and it is a **catalog found by id, not a schedule**… |
| unit-134 | narrowed | ## Structure of the agenda | Corpus pairs. For each corpus: an exploratory WU and a verification WU… |
| unit-136 | narrowed | ## Structure of the agenda | Infrastructure WUs where the plan needs one: e.g. an instrument to build… |
| unit-137 | absent | ## Structure of the agenda | A revision may add a fourth kind; the card's `Type` field is where it would show. |
| unit-138 | absent | ## Structure of the agenda | Each card:… |
| unit-139 | narrowed | ## Structure of the agenda | Hypothesis references live here and in spec pools, never in hypothesis files… |
| unit-159 | reversed | ## What it produces | A pending forward plan: consolidation always requires a new plan, because the old plan's references are stale. … |
| unit-061 | narrowed | ## What never enters a record | Methodological pointers ("WU2.4 should check this" — the forward plan or spec pool). |
| unit-147 | reversed | ## Writing the plan | Start from the landscape: read the full index and every hypothesis file … |
| unit-148 | absent | ## Writing the plan | The rationale section is written once and can be long — at least: what the last consolidation, revision or reassessment showed … |

### Synthesis retired (5)

"Synthesis" retired entirely: an exploration over verified artifacts is an exploration, under rule 3 (rulings log § Vocabulary).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-101 | narrowed | ## Four types | / **Synthesis** / The **verified** artifacts of the corpora it names… |
| unit-036 | narrowed | ## Session routing — read the named file in full before acting | Synthesis WU (comparison, retrospective, adjudication, evaluation, connection, and the like) … |
| unit-135 | absent | ## Structure of the agenda | Synthesis WUs. Comparison, retrospective, adjudication, evaluation, connection —… |
| unit-115 | absent | ## The four phases | Synthesis / The card; the debt status of every corpus it names… |
| unit-121 | absent | ## The four phases | Synthesis / Read the verified artifacts; write the synthesis artifact… |

### Codebooks are authored in preparing-to-verify (1)

building-a-tool is code with tests; the itemizer is preparing-to-verify's own, and so are author-codebook and calibrate (rulings log § preparing-to-verify-a-corpus, § building-a-tool).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-102 | reversed | ## Four types | / **Infrastructure** / Whatever the thing being built needs /… |

### Verification decoupled from exploration (2)

conducting-a-verification-round is fully decoupled from exploration; a round starts on Brian's go, and when a later round is due is his call in verify-plan (rulings log).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-142 | narrowed | ## Ordering is structural, not derived | Verification passes are triggered, and run in rounds.… |
| unit-104 | narrowed | ## The corpus pair, and what is ordered | Every corpus in the buildout … is worked as an exploratory/verification pair, and the one hard ordering rule is exploratory pass on c → verification pass on c → any consumer of c… |

### outcome replaces disposition; held dropped (3)

held dropped as redundant with "no outcome line yet"; outcome replaces disposition (rulings log § promoting-checked-candidates).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-080 | narrowed | ## The candidates file | One per verification WU, in the WU's work folder under `fanout/` (the table above; the `docs/` artifact cites it), **append-only**… |
| unit-082 | narrowed | ## The candidates file | Promotion append:… |
| unit-083 | narrowed | ## The candidates file | Status is derived from the last append and is the only field that changes.… |

### The referee has two inputs (4)

Ruling 6 of 2026-09-04: the current statement and the finding, no source, no excerpt; the clause is a falsifier (rulings log § refereeing-a-candidate).

| unit | relation | section | audit quote |
|---|---|---|---|
| unit-084 | reversed | ## The referee | A **classifier in the verify role**, and therefore an instrument… |
| unit-086 | reversed | ## The referee | - **Inputs are exactly three**… |
| unit-087 | narrowed | ## The referee | - **The task is to attempt the clause blind**… |
| unit-089 | narrowed | ## The referee | - **A vacuous clause is non-diagnostic by definition, whoever wrote it**… |

