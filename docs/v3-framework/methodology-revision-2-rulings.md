# Methodology revision 2 — rulings log

Append-only. One entry per ruling, dated, in Brian's words where he gave them, with the
reason. This is the dated record of the revision's construction; the write-once
`methodology-revision-2.md` is written from it when the revision lands. Rulings 1–13 of
2026-09-04 (daytime) are in `methodology-revision-2-handoff.md` § Rulings so far; several
are superseded below and the handoff is stamped as superseded when handoff 2 is written.

## 2026-09-04 (evening) — schema

- **Readable ids.** Every id in the map is a slug that reads in a sentence; the abbreviated
  ids (`f.cand`, `V.c`, `G1` colliding with gap `G1`) were the cognitive friction that
  opened the session. Rename is mechanical because the validator holds every reference.
- **Four units became three tables: Activities, Processes, Artifacts.** Brian: "an
  activity is the whole row, id is how we identify it, the prose on the right is a
  description." Activities are things Brian does, gerund with object; the relation between
  them is *enables*, a DAG (ruled: "Yes, a DAG"), with exactly one terminus that owns no
  processes: `changing-the-planner-for-v3`, out of scope, enabled by
  `baselining-a-hypothesis`. Brian: "baselining-hypotheses is the terminal activity that
  has a process. It enables changing-the-planner-for-v3, which does not have processes
  because it's out of scope."
- **Roots retired.** The roots table was invented on 2026-09-04 (18:25) by the session
  assessing cuts, as the upstream half of a cut criterion, and never ruled. Its incidents
  were dated records standing in for rules; its rules were constraints, not reasons. The
  chain of activities replaced it; constitutional rules appear only as validator checks.
- **Mode, not actor.** Brian: "agent runs autonomously with replaced instructions. session
  runs autonomously with story planner's claude.md and skills available. brian is really
  just human in the loop. I'm not editing text files." Claude Code CLI is the environment
  and appears in no table. Ruled precisely by decision: `hitl` if a decision that is Brian's
  is made during the process; `session` if he only starts it and reads its output; `agent`
  under an inlined instrument. Every `hitl` process writes the artifact that records the
  decision ("into the artifact the process already writes").
- **A process splits only at a change of mode or when it invokes the runner.** Brian: "I
  like the assertion." One session under one activity file is one process however many
  steps the prose lists. Process ids are short slugs of their own.
- **No instructions column.** The activity file is the instruction source for its session
  and hitl processes; an agent's instruction is the instrument it reads; a process invoking
  the runner is governed by the agent-runner skill. Brian: "Do instructions go on processes
  or activities?" — on activities, so the column carried nothing.
- **`state` is development state of the process type** (`built | specified`), never the
  state of a run. Brian: "state is actually development-state? Not the state of one
  instance."
- **No Edges table, no `when`.** Order is derived from reads and writes; every "needed"
  edge turned out to be a missing artifact for an hitl decision, and every branch condition
  a predicate on an artifact the successor reads. Brian: "Why are 'edges' and 'data flow'
  two concepts? Do they have to be?" `when` dropped for now: "We will drop it for now and
  iterate later."
- **No gate column.** The per-artifact gate was the session's generalisation of the one
  endorsed check (every path to a hypothesis write passes an hitl process), which stays a
  validator rule. Brian: "I didn't propose gates."
- **Artifacts are classes; files are instances.** Brian: "an artifact is a set. Like a C#
  class while individual files are objects of the class." One `corpus` row with seven
  instances; the hypothesis file is three artifacts (statement, record, status) because
  three processes treat its sections under three disciplines.
- **`mutation`, four values, is the standing policy for every artifact.** `in-place ·
  succeeded · append · frozen`. Seven observed write disciplines reduced to these; Brian:
  "should we instead change how we plan to operate with writing artifacts instead of
  having a mishmash? We should evolve to something more organized and systematic." The
  forward plan's pattern, edited while active then retired whole by a numbered successor,
  is `succeeded`, and codebooks and reading protocols move to it because a superseded
  codebook's text survived only in git.
- **Rule 9 is the mutation rule**, replacing the "standing versus dated" draft after the
  reanalysis found that the hypothesis record is not history but the evidence
  relationship, a codebook's calibration record is a measurement, a revision note is a
  change record. Brian: "the history of hypothesis file and the history of the other
  things [do not] have the same meaning and/or purpose."
- **Path is a pattern, never prose; artifacts get a `description` column.** Brian: "Why is
  there prose in the path column?"
- **Placeholders fixed and `<work>` retired.** `<instance>`, `<run>`, `NNN`, `N`; referee
  runs live under the instance they serve; `fanout/referee/` holds only the shared
  instrument. Brian: "Are directory names drifting or staying tight?" and "what is work?"
- **Activity files have one shape** (title, enables line, Processes table, generated
  section, Preconditions as conditions not lists, one section per process in order,
  Never) and carry procedure only. Brian: "carefully review requirements for these files
  after all the discussion, don't tunnel vision on what landed for the first one."
- **The skill is rebuilt from scratch in a sibling folder**, `v3-buildout-2`, one activity
  file per step, validated as it grows, swapped in one commit. Brian: "Why would we not
  build a new skill from scratch from the ground up incrementally?" The map is not a
  document: the router is the Activities table in SKILL.md, each activity file opens with
  its Processes table, `artifacts.md` holds the Artifacts table and every format, and
  `map.md` and `state.md` are generated only.
- **`fanout/PROTOCOL.md` retires.** Its order is the map's; its rules move to the
  agent-runner skill; the host serves the rendering.

## 2026-09-04 (evening) to 2026-09-05 — activities, one at a time

- **baselining-a-hypothesis** stands as articulated: Brian's dated judgment, one hitl
  process.
- **promoting-checked-candidates**: one hitl session, no autonomous part. Brian: "When
  promoting, I will be saying interactively what to promote, after iterating with
  analysis." Scope is his, by hypothesis or by round: "Isn't this activity invoked ad hoc
  when I feel something should be [decided], and it's one hypothesis at a time?" The
  closing line is `outcome` ("outcome is good"), replacing "disposition" after its legal
  sense was explained; `held` dropped as redundant with "no outcome line yet". The source
  read is the citation check and is required before promoting, not before declining.
- **refereeing-a-candidate**: two inputs, the current statement and the finding text,
  no source locator. Brian: "Shouldn't it be only the hypothesis's current statement and
  the candidate's finding? It has no other context at all. Its only job is making a
  ruling." The clause is a **falsifier** (Popper's term), replacing "discrimination
  clause". Codebook hash explained: SHA-256 of the file's bytes, stamped by the runner.
- **iterating-a-statement**: revision 1's immediate re-referee was never ruled by Brian;
  his 2026-08-31 ruling had been re-tag in place. New ruling: entries are dated and never
  invalidated; an iteration entry is a wording boundary; status derives from
  current-wording entries and may return to `untested`; the prior findings become
  iteration candidates consumed by the next round. Brian: "the history is part of the
  epistemic data, and revision adds an entry. This seems to imply that evidence getting
  promoted and its falsifier are not necessarily live truth." A statement iterates on
  evidence only; a lead never rewords a hypothesis. Enabled by promoting alone.
- **writing-candidates-from-verification** stands as its own activity, `session` mode.
- **conducting-a-verification-round**, renamed from running-verification-cells: "cells"
  pointed at the union with exploration, "running" at the runner alone. A round is one
  execution of a calibrated instrument over an item set; it starts on Brian's go, has no
  plan approval inside it, and anything the instrument does not cover goes back through
  preparing. Fully decoupled from exploration.
- **preparing-to-verify-a-corpus**, split from the round at the autonomous handoff on
  cadence (once per instrument, not per round): itemize, author the codebook against real
  items, calibrate on a sample Brian scores blind. It owns the itemizer ("the plan has to
  make the enumeration tool, and then run the enumeration"). Renamed from
  instrumenting-a-corpus because "instrument" was ambiguous. It is "building a measuring
  instrument for a corpus, with your judgment as its reference standard."
- **The work matrix retires** as a routing device; its three functions are carried by
  mode and activity shape; cell names survive as descriptions of agent rows. The
  **investigator and focused-reader cells retire**: an investigator has no frozen
  predicate and unmeasured recall; itemizing is always mechanical, a script or an authored
  query in Brian's vocabulary, and relevance is a calibrated classifier predicate. Brian:
  "Is this even a necessary mode, or is it just going to lead to drift?"
- **Exploration is three activities**, named by Brian: preparing-to-explore-a-corpus
  (hitl), exploring-a-corpus (session or agent), reviewing-leads (hitl). No instrumenting
  for exploration. "Arms" corrected to "slices" as the unit; arms are the optional A/B
  design over the same slices. Preparing reads the corpus's question list and Brian's
  opening question: "Questions are not hypotheses, are they? And without questions, what
  is there to discover?" The read-manifest is renamed **arm-key**.
- **Vocabulary**: lead (exploration's output), question and question list (replacing
  spec and spec pool; `predicate` replacing `candidate-predicate`), result, finding (on a
  candidate only), evidence, falsifier, outcome. "Synthesis" retired entirely: an
  exploration over verified artifacts is an exploration. "Finding" was colliding with the
  pipeline's word and could not name exploration's output.
- **Questions are written only by hitl processes.** Brian: "I don't think asking
  questions about a corpus can ever be non hitl." Consequently the exploratory pass writes
  proposed questions into its artifact and the review writes the list; every hitl
  activity may write a question; "asking-questions-about-a-corpus" merged into
  reviewing-leads, which is named for what it reviews and carries no name in the gerund.
  The `answered-by` and `frozen-into` statuses are derived, never written.
- **Post-WU review is not uniform.** Brian: "'WU review' is no longer a valid uniform
  thing. Verification and Exploration are different shapes that were conflated before."
  Exploration's review is reviewing-leads; verification's is promotion; a tool's
  acceptance is inside building.
- **The runner is a process inside each invoking activity**, not an activity; one runner
  row per invoking activity, not shared ("Not shared"); each activity keeps its own agent
  child row. The pilot lives in the preparing activities: calibration is the codebook's
  pilot, one job read by Brian is the protocol's; no pilot inside a round.
- **building-a-tool** (renamed from building-an-instrument-or-ingest): code with tests
  that serves more than one activity; the itemizer is preparing-to-verify's own.
- **consolidating-the-hypothesis-set retires.** With refereeing in place its reasons are
  taken one by one: statuses are computed, re-verification is the referee's, merges and
  splits are minting plus iterating in any hitl session, a structural change is a
  priority reassessment. Brian: "Not sure this is even necessary anymore now that
  refereeing is in the setup." Retired.
- **writing-a-forward-plan retires.** The plan's bookkeeping is derivable into a generated
  `state.md`; its judgment, what to do next, was never a document's to hold. Brian: "How
  do I know what activity to do when? ... Is workplan outdated or does it still serve a
  purpose?" What survives is the **instance registry**, one row per instance
  (`id · type · corpus · go`), appended at plan approval, because "the c# tool needs
  something to work with" and inference from file names is forbidden. Cards, the card
  format, the status board and "WU N.M" numbering all retire; instances are named
  `exploration-of-<corpus>[-n]`, `round-of-<corpus>-n`, `referee-<n>`. A tool is built as
  the first task inside the instance that needs it, never a card of its own.
- **revising-the-method** stands; G10 closes when the revision note is written.
- **minting-a-hypothesis reads only the current set.** Brian: "Why does mint need
  anything besides the current hypothesis set?" Nothing enables it; it enables
  reviewing-leads; the permission to mint in any hitl session is a sentence in its file,
  not an edge.
- **The referee's preparation is an instance**, `referee-<n>`, of preparing-to-verify with
  `candidates` as its corpus, prepared once and again only when a ruling changes the
  codebook. Brian: "The referee's preparation is bootstrapping, right? Not standard
  operating procedure." Bootstrap is a fact about instances, never about activities.
- **A codebook carries no status line**, since any line is part of the hash; calibrated
  means a calibration record exists at the hash (draft 1's G22, closed).
- **The hypothesis record is the evidence relationship, not history**; that reasoning is
  carried here so it is not lost, and lands in `artifacts.md` § Hypothesis file.

## 2026-09-05 — the tool rework (handoff 2, step 1)

Brian answered each of these by selecting a Claude-authored label; the log records the
selection and the reason offered with it, never as his phrasing.

- **The tool is reworked in place.** The from-scratch ruling of 2026-09-04 applied to the
  skill because other sessions load the live one during construction. The tool has no live
  consumer: not in the solution, no publish folder, its real-folder test skipped, and the old
  `process-map.md` deleted at the swap. Git keeps the old validator. Parser, marker writer,
  mermaid scanner and the SKILL.md checks survive unchanged; model, reader, validator, graph
  and renderer are rewritten; roots, edges, bootstrap, governed-by, the locus grammar and the
  codebook-example check are deleted.
- **The validator enforces the `enables` DAG as ruled.** The rows of 2026-09-05 carry a
  cycle — promoting-checked-candidates enables iterating-a-statement, which enables
  refereeing-a-candidate, which enables promoting-checked-candidates — and it is a step 2
  finding for Brian, not something the tool is shaped around.
- **`state` ships in step 1**, tested against fixtures; its real inputs (`instances.md`,
  `questions/`) arrive in step 4. Its derivations, ruled here: per instance, the
  instance-scoped artifacts present on disk (paths resolved from the Artifacts table,
  placeholders as wildcards) and the furthest process in chain order whose instance-scoped
  writes all exist; a question is covered by a codebook when a calibrated version (a
  calibration record at its hash, accepted) lists the question's title in a new `## Questions`
  section of the codebook format — the format change lands in `artifacts.md` § Codebook with
  this step; per hypothesis, the frontmatter status as authored plus a mismatch flag when the
  entries after the last `iteration` line imply a different one.
- **The read-and-write row check covers `frozen` only.** A process row listing one artifact
  under both reads and writes is reported when that artifact's mutation is `frozen`; a
  `succeeded` artifact is not reported, because reading version N to write version N+1 is
  what succession is, and flagging it would push rows to drop a real read. The check examines
  rows, never files: it says the method as written has an edit-shaped process, not that a
  file was edited.
- Decided by the session without a ruling, recorded so they can be overturned: edges into the
  terminus are exempt from the backed-by-data-flow check, since the terminus owns no
  processes; an instrument token that is not an artifact id is reported as information, since
  a typo cannot otherwise be told from a free name; the activity file's shape (title is the
  id; the sections after the generated marker are Preconditions, one per process id in table
  order, Never) is checked; the artifact `path` cell is checked against § Schema's "one
  pattern, never prose", so a cell carrying ", or" or " and" is a finding.

## 2026-09-05 — validating the new folder (handoff 2, step 2)

The first `validate` over `v3-buildout-2` reported ten failures: five path cells carrying
alternatives, one cycle in `enables`, three unbacked `enables` edges, one frozen artifact
read and written by one process. Each was fixed as a row edit and a prose edit together.
Where Brian ruled, he did so by selecting a Claude-authored label, and the log records the
selection and the reason offered with it, never as his phrasing; the one thing he typed is
quoted as such. The session's own choices are marked so they can be overturned.

- **Path cells (session).** `codebook` and `calibration-record` are one pattern each under
  `fanout/<instance>/`, with `<instance>` defined in the `artifacts.md` preamble as the
  instance's *folder*: its registry id, except that every `referee-<n>` shares the folder
  `referee` — the mapping the state builder already made. `itemizer` is
  `fanout/<instance>/itemize.*`; an itemizer that is a tool project is `tool-source`, which
  the `itemize` row already writes. `skill` is the folder `.claude/skills/v3-buildout/`, and
  the agent-runner skill is a new artifact, `runner-skill`, written by `build` (a runner
  change updates its skill in the same commit) and by `revise`, read by `revise`.
  `tool-source` is `tools/StoryPlanner.<Name>/`, its tests named in the description.
  `<date>` and `<Name>` were added to the placeholder list, since paths already used them.
- **revising-the-method → the two preparing activities (Brian, by selection: confirmed).**
  `explore-plan` and `verify-plan` read `skill`: the plan is written against the chain's
  activity files, read whole at preparing rather than each at its own start, which is more
  than the derivable own-file read. Not taken: the validator counting `skill` as implicitly
  read by every session and hitl process; a different `enables` target for revising.
- **building-a-tool → revising-the-method (Brian, by selection): the edge is dropped and
  `ProcessMap` stays a free name.** The session had named `tool-source` as `revise`'s
  instrument, reading § Schema literally (the validator's code is in the Artifacts table),
  which backed the edge; Brian chose the free name and no edge. A revision that needs a tool
  change still builds it as its first task; the router shows no edge for it, and `runner`
  and `ProcessMap` are free names on the same footing.
- **The cycle (Brian, by selection): iterating-a-statement is a root, like minting.**
  promoting-checked-candidates no longer enables it; nothing does. The decision to reword,
  in a promotion session or any hitl session, is the whole trigger and is a sentence in the
  file's Preconditions, exactly as minting's permission is. It keeps enabling
  refereeing-a-candidate through the iteration candidates. The promote → iterate data flow
  stays visible in `map.md`; promoting's § promote points at iterating for a rethink. This
  overturns "enabled by promoting alone" (above); that ruling's substance — a statement
  iterates on evidence only and a lead never rewords — stands in the Preconditions. Not
  taken: iterating enables baselining instead (backed, hollow); retiring iterating as an
  activity (by the split rule it is part of `promote` or `mint`); overturning the DAG.
- **The frozen read-and-write (Brian, by selection, after typing "What is this tool
  business logic about in the first place?").** Answered: the check is rule 9's row-level
  shadow — the validator never sees a file edited, and the one edit-shaped thing a row can
  show is the same artifact under reads and writes; in-place is the declared discipline,
  append is how appending works, so only frozen is reported; it fired because `revise` reads
  the prior revision note and writes the next, a series the tool could not tell from an
  edit; in this method it would catch little today, every frozen artifact being a series,
  run-scoped, or written and read by different processes. Ruled: a frozen artifact whose
  path carries `N` or `<date>` is exempt, on the reasoning that exempted `succeeded` —
  reading one member to write the next is not an edit, and flagging it pushes rows to drop a
  real read. `<run>` is not a series marker. Tool change with fixtures: `ArtifactPath.IsSeries`,
  one path test, one validator test; the existing frozen-failure test stands on a run-scoped
  artifact. Not taken: retiring the check; dropping the read and exempting never-read for
  frozen; calling the revision note `succeeded`.
- **Left standing as information:** `state` is written by no process (the tool generates
  it whenever a session runs `state`); the free names `ProcessMap`, `dotnet`, `git`, `runner`.
- **Noticed, not fixed — a `state` gap for a later step.** Referee runs live under
  `fanout/<instance>/referee/<run>/`, but the run-scoped patterns carry one `<run>` segment,
  so `state` will not count a referee run's items, jobs, results, tally or `run.md` as present
  for the round it serves. Not a validator finding; recorded so the state verb's first real
  run does not surprise.
- **Landed with the step:** `validate` passes on `v3-buildout-2` with three notes; `render`
  wrote the `level-1` section, thirteen `activity` sections and `map.md`; the real-folder
  integration test is un-skipped against `v3-buildout-2` (it repoints at `v3-buildout` in
  the swap commit, where it also gains `claude plugin validate`); `dotnet test` green.

## 2026-09-05 — step 4's first move, taken early: `fanout/PROTOCOL.md` retires

Brian's instruction, in his words: "Proceed with moving protocol.md's rules." Taken ahead
of the rest of step 4 because the skill audit of the two folders launches within the hour
and its document A carries PROTOCOL.md as units 160–175 while set B has only the
agent-runner skill to answer them. Done as the `revise` row says: the `runner-skill`
artifact edited, the file retired.

- **What moved, and where.** The lifecycle table's order moved nowhere: it is derived from
  the process tables and rendered into `map.md`, and the agent-runner skill now says so in
  place of naming PROTOCOL.md as the lifecycle. The rules PROTOCOL.md alone stated went into
  the agent-runner skill: the run folder's contents as a tree under `fanout/<instance>/`,
  the stage strip and what it never judges, the dry run as a step before the pilot (rule 4),
  and `run.md` as a pointer to `artifacts.md` § run.md for every run, buildout or not. The
  harness-control and JSON-route paragraphs were already there and were not duplicated.
- **Vocabulary brought to revision 2 in the same edit**, since the audit's set B inlines the
  skill beside the new one and the protocol treats any member of B as B: instances for WUs
  (`fanout/<instance>/`, `referee/` holding only the shared instrument, referee runs under
  the round they serve), `codebook-N.md` / `protocol-N.md` / `calibration-<date>.md`, `agent`
  processes for cells, slice readers for reading arms, investigators gone, the referee given
  no excerpt. The "Two mechanisms" line of 2026-09-03 is re-worded, not moved.
- **The file is a retirement notice, not deleted**, because the host's `/protocol` route
  reads `fanout/PROTOCOL.md` at request time and every run page links to it; the notice says
  where the order and the rules went. The route serving the rendering is a runner change
  (building-a-tool) owed to a later step, not made before the audit since the host is about
  to run it. `fanout/README.md` and CLAUDE.md's one pointer follow the retirement.
- **Not moved:** the codebook edit, the spec-pools move, `instances.md`, the forward-plan
  stamp — the rest of step 4 stands.
- **The referee codebook in the audit's set B (session's assessment, Brian asked why it is
  there).** The 2026-09-03 audit's set B carried it because revision 1's `evidence-pipeline.md`
  delegated the referee's rules to it, so a unit about the referee could only be judged with
  the codebook present. For this audit it is an A-side document: the revision retires it, a
  new numbered version is authored and calibrated in `referee-1`, and the new skill delegates
  to `codebook-N.md`, which does not exist yet. Kept in B, its excerpt rules would let the
  old skill's excerpt units pass as preserved. The run folder is the preparing session's;
  removing the file from B is a `jobs.json` regeneration there.

## 2026-09-05 — Brian delegates the audit's preparation; the audit becomes the method's second lint

Brian, in his words: "You are in charge now. Consider the agent runner for skill audit the
2nd level of linting after the validator tool. Make the appropriate changes and start the
blazor server process but don't kick off anything yet." The session's decisions under that
delegation, each overturnable:

- **The audit is two process rows of revising-the-method**, not prose alone: a runner
  invocation is its own process by the split rule, so `audit-run` (session; runner,
  generator, tallier; reads the prior text and the protocol; writes items, manifest, jobs,
  ledger, tally and `run.md`) and `audit-judge` (agent; the protocol's three questions per
  unit; the only writer of results), with `revise` reading the results and the tally to
  adjudicate them into the revision note's omissions list. One new artifact,
  `audit-protocol` (`fanout/skill-audits/protocol.md`, in-place, authored outside the
  method, so never-written is its honest report). The audit's runs reuse the run-scoped
  artifact classes by admitting `skill-audits` to the `<instance>` placeholder as a work
  outside the buildout's instances. Both new rows are `specified`: the 2026-09-03 run
  predates the text and today's has not launched. The router's description of
  revising-the-method and § Provenance name the two lints; the swap now waits on both.
- **Two rules the run taught, written into audit-run:** the rulings log is never in set B
  (the 2026-09-03 ruling, now text), and set B holds no instrument the revision retires,
  since a retired file in B lets its own rules pass as preserved — the reason the referee
  codebook left set B.
- **Set B regenerated without `fanout/referee/codebook.md`:** 46 jobs, ids and sections
  unchanged, 17 B files per job. Recorded in the run's `run.md` as an appended pre-launch
  paragraph; the 10:01 text stands above it.
- **`fanout/README.md` reduced to a pointer** (what the folder is; layout and rules in the
  agent-runner skill; the order in `map.md`), not added to document A: it carried no rule
  of its own, and a second copy of the tree is the stale-mirror failure.
- **Validate passes with four notes; rendered.** The dry run and the host start are
  recorded in the run's `run.md`; pilot and batch not enqueued — Brian's go.

## 2026-09-05 to 2026-09-06 — the audit ran; adjudication parked

The pilot (`arm-A-22-the-referee`) was read by Brian; his verdict, in his words: "Proceed
with the whole batch." 46 of 46 jobs succeeded; `run.md` and `tally.md` in the run folder
carry the figures. The draft omissions list, `methodology-revision-2-omissions-draft.md`,
holds 14 open units with the session's proposals and alternatives, 105 units assigned to
rulings above, and an appendix of things raised outside document A. Brian, in his words:
"I'll decide and skill rulings later." The tool defects the run surfaced went to
`engineering-handoff-2026-09-05.md`; its code landed on 2026-09-06 (its § Status), and four
of its items wait on him with the omissions list.
