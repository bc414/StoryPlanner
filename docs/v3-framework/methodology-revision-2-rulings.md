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
