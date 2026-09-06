# Handoff 2: landing methodology revision 2

Written 2026-09-05. Supersedes `methodology-revision-2-handoff.md` (stamped). Read in
full, then `.claude/skills/v3-buildout-2/SKILL.md` in full: it is the schema the tool is
built to and the router the swap installs. The reasons for every choice are in
`methodology-revision-2-rulings.md`; do not re-derive them.

## State on 2026-09-05

- `.claude/skills/v3-buildout-2/`: SKILL.md (framework, rules, schema, router, vocabulary),
  `artifacts.md` (Artifacts table and twelve formats), thirteen activity files each opening
  with its Processes table. Authored; validated by the tool and rendered on 2026-09-05
  (step 2 — the ten findings and their rulings are in the rulings log under that date). Not
  loaded by any router; the live skill is `v3-buildout`.
- `tools/StoryPlanner.ProcessMap`: reworked in place on 2026-09-05 (step 1) to the schema
  in SKILL.md § Schema — `validate`, `render`, `state`, `nodes` — with the real-folder test
  un-skipped against `v3-buildout-2` at step 2.
- `docs/v3-framework/process-map-1-draft.md`: draft 1, the audit that prompted the revision;
  to be stamped, not edited.

## Sequence

1. **Rework the tool to the schema in SKILL.md § Schema**, fixtures first, one verb at a
   time: `validate <skill folder>` reads the router table, every activity file's Processes
   table and `artifacts.md`, and checks what § Schema and § Derived list: references
   resolve; closed sets; one mode per process; ≥ 1 read and ≥ 1 write; an artifact named in
   `instruments` counts as read; every artifact read by something (written by nothing is
   informational); `enables` acyclic, one terminus, every edge backed by data flow; every
   path from `candidates` to a hypothesis write passes an `hitl` process; every writer of
   `question-list` is `hitl`; every `hitl` process writes ≥ 1 artifact; an `append`,
   `frozen` or `succeeded`-retired artifact is not both read and written by one process
   except by append; SKILL.md ≤ 500 lines and its description ≤ 1,024 characters; every
   companion linked from SKILL.md. `render` refuses until `validate` passes, then writes the
   `level-1` section in SKILL.md, each file's `activity` section, and `map.md`. `state`
   reads `instances.md`, `questions/`, `hypotheses/`, the instance directories and
   `fanout/`, and writes `state.md`. `nodes` stays as is for the comparison.
2. **Validate the new folder**; fix rows and prose together for anything it reports; render.
   Done 2026-09-05; rulings log § 2026-09-05 — validating the new folder.
3. **Comparison against draft 1** (`git show 32b6d4b:docs/v3-framework/process-map-1-draft.md`,
   `nodes` on both): the result is the omissions list for the revision note, each omission
   with its ruling, not a pass/fail.
4. **Text moves outside the skill**: the rules `fanout/PROTOCOL.md` alone states move into
   the agent-runner skill and the file retires; `spec-pools/` becomes `questions/` with
   `bears-on` → `hypotheses` and `candidate-predicate` → `predicate`; the referee codebook
   loses its status line and its inputs section points at the `referee-judge` row;
   `docs/v3-framework/instances.md` is created empty; forward-plan-2 is stamped retired.
   The PROTOCOL.md move was taken early on 2026-09-05 for the skill audit (rulings log
   § step 4's first move); the rest stands. The audit itself is now the method's second
   lint (revising-the-method § revise, `audit-run` and `audit-judge`), and step 5 waits on
   its adjudication as well as on step 3's node comparison; both feed the omissions list.
5. **Swap in one commit**: rename `v3-buildout-2` to `v3-buildout` (the old folder deleted,
   its text preserved in git and cited from the note); the memory file's v3 entry points at
   the new router; `methodology-revision-2.md` written once from the rulings log (what
   prompted it, the rulings, the omissions list, what was not adopted, what is owed);
   `process-map-1-draft.md` and both handoffs stamped; the integration test un-skipped and
   extended to run `claude plugin validate .claude/skills`. Found 2026-09-05, also at the
   swap: `CORPUS-STATUS.md` moves into the new folder (it exists only in the old one, which
   the swap deletes, and the new SKILL.md names it) carrying the corpus ids per the ruling
   on unit-176; `VERSION-HISTORY.md` goes where the ruling on unit-043 says; CLAUDE.md's
   revision-1 vocabulary (the third-role paragraph's "when a cell calls for one", its v3
   skill references) is brought to revision 2; the real-folder test repoints at
   `v3-buildout`; the omissions draft is folded into the note and retired.
6. **Then WU2.15 resumes**, re-specified in the new vocabulary: it is `referee-1`, the
   referee's preparation, followed by the retroactive candidates as the first round's input.

## Owed, not part of the landing

**An exploratory session on Claude Code tool-use hooks as runtime enforcement.** The
validator checks that the rows say an `hitl` process precedes every hypothesis write;
nothing checks that a live session obeyed the rows. Claude Code's PreToolUse hook is the
mechanism that could: a script that denies an Edit or Write by path unless a prerequisite
Read was observed, declared in a skill's frontmatter so it registers when the skill loads
(https://code.claude.com/docs/en/hooks, "Hooks in skills and agents"); Anthropic's own
guidance is that when a skill stops influencing behaviour, enforce with hooks rather than
stronger prose. Brian ruled on 2026-09-04 that this needs exploration of where it applies
before anything is built or added as a row. That session's questions, at least: which of
the map's rules are hook-shaped (a path plus a required prior read, or a path never
written) and which are not; whether a hook's policy can be generated from the same tables
the validator reads, so the skill stays the single source; what a hook can see (no session
history; `transcript_path` lags; "was X read" needs state the hook keeps itself); the cost
of false denials in an hitl session; whether a hook is also the durable trace that an
interactive ruling otherwise lacks; and how agent jobs, which never load the skill, are or
are not covered. Its output is a finding for a later revision, not a row.

Also owed: the section-aware check that a hypothesis record's existing lines are unchanged
between commits; the codebook anchor convention in the agent-runner skill once a
calibration has produced one; and, of the tool fixes the first audit run surfaced
(`engineering-handoff-2026-09-05.md`), only the state verb's blind spot for referee runs
and the three other items awaiting a ruling — the rest landed on 2026-09-06, and that
file's § Status says what and how it was verified.

The audit ran on 2026-09-05 (46 jobs, none failed); its adjudication is the draft
omissions list `methodology-revision-2-omissions-draft.md`, awaiting Brian's rulings.

## Must not

Edit the live `v3-buildout` before the swap; hand-edit a generated section; author a
consumers column, an edges table or a plan; keep two copies of any table; treat any row as
settled.
