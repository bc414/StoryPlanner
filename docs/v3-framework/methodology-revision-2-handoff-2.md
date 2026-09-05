# Handoff 2: landing methodology revision 2

Written 2026-09-05. Supersedes `methodology-revision-2-handoff.md` (stamped). Read in
full, then `.claude/skills/v3-buildout-2/SKILL.md` in full: it is the schema the tool is
built to and the router the swap installs. The reasons for every choice are in
`methodology-revision-2-rulings.md`; do not re-derive them.

## State on 2026-09-05

- `.claude/skills/v3-buildout-2/`: SKILL.md (framework, rules, schema, router, vocabulary),
  `artifacts.md` (Artifacts table and twelve formats), thirteen activity files each opening
  with its Processes table. Authored, cross-checked by grep for dangling ids and closed
  sets, not yet validated by the tool. Not loaded by any router; the live skill is
  `v3-buildout`.
- `tools/StoryPlanner.ProcessMap`: built 2026-09-04 to the *previous* schema (four tables in
  one file, roots, edges, `governed-by`, locus grammar). Its markdown unit parser, marker
  writer, mermaid scanner and test layout are reusable; its validator rules and model are
  not.
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
3. **Comparison against draft 1** (`git show 32b6d4b:docs/v3-framework/process-map-1-draft.md`,
   `nodes` on both): the result is the omissions list for the revision note, each omission
   with its ruling, not a pass/fail.
4. **Text moves outside the skill**: the rules `fanout/PROTOCOL.md` alone states move into
   the agent-runner skill and the file retires; `spec-pools/` becomes `questions/` with
   `bears-on` → `hypotheses` and `candidate-predicate` → `predicate`; the referee codebook
   loses its status line and its inputs section points at the `referee-judge` row;
   `docs/v3-framework/instances.md` is created empty; forward-plan-2 is stamped retired.
5. **Swap in one commit**: rename `v3-buildout-2` to `v3-buildout` (the old folder deleted,
   its text preserved in git and cited from the note); the memory file's v3 entry points at
   the new router; `methodology-revision-2.md` written once from the rulings log (what
   prompted it, the rulings, the omissions list, what was not adopted, what is owed);
   `process-map-1-draft.md` and both handoffs stamped; the integration test un-skipped and
   extended to run `claude plugin validate .claude/skills`.
6. **Then WU2.15 resumes**, re-specified in the new vocabulary: it is `referee-1`, the
   referee's preparation, followed by the retroactive candidates as the first round's input.

## Owed, not part of the landing

The exploratory session on skill hooks (handoff 1 § Build-vs-buy check, record 2); the
section-aware check that a hypothesis record's existing lines are unchanged between
commits; the codebook anchor convention in the agent-runner skill once a calibration has
produced one.

## Must not

Edit the live `v3-buildout` before the swap; hand-edit a generated section; author a
consumers column, an edges table or a plan; keep two copies of any table; treat any row as
settled.
