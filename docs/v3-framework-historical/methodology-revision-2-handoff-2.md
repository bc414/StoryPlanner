# Handoff 2: landing methodology revision 2

Written 2026-09-05; state and sequence brought current on 2026-09-06, after the audit ran.
Supersedes `methodology-revision-2-handoff.md` (stamped). Read in full, then
`.claude/skills/v3-buildout-2/SKILL.md` in full: it is the schema the tool is built to and
the router the swap installs. The reasons for every choice are in
`methodology-revision-2-rulings.md`; do not re-derive them. A session that adjudicates, or
that is deciding what comes next, reads `methodology-revision-2-omissions-draft.md` in full
as well.

## State on 2026-09-06

- `.claude/skills/v3-buildout-2/`: SKILL.md (framework, rules, schema, router, vocabulary),
  `artifacts.md` (the Artifacts table and every format), thirteen activity files each
  opening with its Processes table, and `CORPUS-STATUS.md` (copied in on 2026-09-06 as the
  working copy, its wording brought to revision 2, the corpus ids still owed). Validated
  and rendered; revising-the-method carries the supersession audit as `audit-run` and
  `audit-judge`, both `built`. Not loaded by any router; the live skill is `v3-buildout`,
  which nobody edits before the swap.
- `tools/StoryPlanner.ProcessMap`: reworked to SKILL.md § Schema (`validate`, `render`,
  `state`, `nodes`); the read-and-write check covers frozen artifacts that are not a
  numbered or dated series (`ArtifactPath.IsSeries`, ruled 2026-09-05); the real-folder test
  runs against `v3-buildout-2`. The `state` verb has never run on real inputs: its inputs
  arrive in step 5.
- The audit, the method's second lint: `fanout/skill-audits/2026-09-05-v3-buildout-2/`,
  46 of 46 jobs, `run.md`, `tally.md`, the ledger. Its adjudication is the draft
  `methodology-revision-2-omissions-draft.md`: 14 open units with the session's proposals
  and alternatives, 105 units assigned to rulings already in the log, an appendix of items
  raised outside document A, and the disposition of draft 1's 24 gaps. All of it awaits
  Brian's rulings, which he parked on 2026-09-05.
- `engineering-handoff-2026-09-05.md`: the tool defects the run surfaced. Its code landed
  2026-09-06 (its § Status says what and how it was verified); four items wait on a ruling.
- `fanout/PROTOCOL.md` is deleted; the agent-runner skill and `fanout/README.md` speak
  revision 2; the host's `/protocol` renders `map.md`.
- `code-session-doc-audit-2026-09-06.md`: another session's audit of every transcript
  since 2026-08-25 for decisions and todos no document carries. Provenance, authoritative
  for nothing, and an input to deciding what comes next.
- `process-map-1-draft.md`: draft 1; stamped at the swap, never edited.
- Working tree: everything through the audit is committed (`056607b` and after). The
  CORPUS-STATUS copy, this file's 2026-09-06 revision and the rulings log's last entry are
  not. Commits are Brian's.

## What waits on Brian, and what does not

Waiting on his word, none of it derivable by a session: the omissions draft (14 open units,
105 assignments and 24 gap dispositions to confirm, two gaps open); the referee codebook's
fate (step 5); the four engineering items (that handoff's § Status); the corpus ids' form
(unit-176) and VERSION-HISTORY.md's home (unit-043). Steps 6 and 7 cannot finish without
the first, because the note holds the omissions list.

Mechanical, for any session, in any order: `spec-pools/` to `questions/` with the two
renames and its README retired; `instances.md` created with the registry's header and no
rows; forward-plan-2 stamped retired; CLAUDE.md's revision-1 vocabulary brought over; the
real-folder test's repoint prepared. Each is a row-and-prose-free file move or stamp.

## Sequence

1. **Rework the tool to SKILL.md § Schema** — done 2026-09-05. The checks are those § Schema
   and § Derived list, recorded in the rulings log § the tool rework, with one later
   change: the read-and-write check applies to frozen artifacts that are not a series
   (rulings log § validating the new folder).
2. **Validate the new folder; fix rows and prose together; render** — done 2026-09-05
   (rulings log § validating the new folder).
3. **The supersession audit** — ran 2026-09-05. Document A: revision 1's six rule files,
   `fanout/PROTOCOL.md`, the spec-pools README (179 units); set B: the new folder, the old
   CORPUS-STATUS.md, the agent-runner skill, the referee codebook removed as a retired
   instrument. Tally: 45 restated, 10 broadened, 5 non-instructional, 82 narrowed, 17
   reversed, 20 absent. Pending: Brian's rulings on the draft, recorded on each line and in
   the rulings log.
4. **Draft 1's gaps, not its nodes** — drafted 2026-09-06 in the omissions draft § Draft 1's
   gaps. The node comparison this step once named is void: draft 1 has 109 node ids and the
   map 67, none shared, and the audit already mapped the text. One disposition per gap
   G1–G24 (closed, dissolved, partly, open), plus the level-1 model change in one
   paragraph for the note. Pending: Brian's confirmation; two gaps open — G13 (the host's
   stage strip marks a codebook calibrated on any record beside it, not one at its hash; an
   engineering item) and G19 (the HITL-context arm of 049 has no mechanism under rule 5).
5. **Text moves outside the skill.** Done: PROTOCOL.md (2026-09-05); CORPUS-STATUS.md copied
   (2026-09-06). Remaining, mechanical: `spec-pools/` becomes `questions/` with `bears-on` →
   `hypotheses` and `candidate-predicate` → `predicate`, its README retired;
   `docs/v3-framework/instances.md` created with the registry header; forward-plan-2 stamped
   retired. Remaining, on a ruling: the corpus ids in CORPUS-STATUS.md (unit-176);
   `VERSION-HISTORY.md` (unit-043); the referee codebook — (a) the minimal edit as first
   written here, status line and inputs section; (b) ruling 6's full text fixes, R2 recast,
   R5, R6 and E1–E5 removed; (c) retire `fanout/referee/codebook.md` and let referee-1
   author `codebook-1.md` in the § Codebook format, which is a rewrite in any case; (c) is
   the recommendation, the old text staying in git and in the audit's document A.
6. **Swap in one commit**: rename `v3-buildout-2` to `v3-buildout`, the old folder deleted,
   its text preserved in git and cited from the note; `methodology-revision-2.md` written
   once from the rulings log — what prompted it, the rulings, the omissions list and the gap
   dispositions folded in from the draft (which is then retired), what was not adopted,
   what is owed; `process-map-1-draft.md`, both handoffs and the engineering handoff
   stamped; CLAUDE.md's revision-1 vocabulary (the third-role paragraph's "when a cell calls
   for one", its v3 skill references) brought to revision 2; the memory file's v3 entry
   points at the new router; the real-folder test repoints at `v3-buildout` and gains
   `claude plugin validate .claude/skills`; `state` runs for the first time on the real
   inputs and its output is read, since nothing has yet.
7. **referee-1**: an instance of preparing-to-verify-a-corpus with corpus `candidates`,
   registered at Brian's go — the itemizer, `fanout/referee/codebook-1.md` in the § Codebook
   format with empty Anchors, the calibration sample he scores blind, the record at the
   hash. Then the pre-referee record entries, re-queued per the ruling on unit-096, as the
   first round's candidates. `WU2.15-plan.md` and `retroactive-referee-pass-handoff.md`
   stamped superseded; their carry-forward items are in the draft's appendix.

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
calibration has produced one; the engineering items still awaiting a ruling
(`engineering-handoff-2026-09-05.md` § Status) and G13 from step 4; G19; and the four gaps
step 4 reports partly closed, each adopted as one clause or listed as owed in the note —
the ledger's mode for a calibration batch (G11), whether a calibration is per model (G14),
whether a promoted outcome records the source read (G21), and durable plan-mode rulings
(G23, partly overtaken by the code-sessions ingest keeping AskUserQuestion answers).

## Must not

Edit the live `v3-buildout` before the swap; hand-edit a generated section; author a
consumers column, an edges table or a plan; keep two copies of any table; treat any row as
settled; delete the old folder before VERSION-HISTORY.md has its home; treat the node diff
as the comparison; edit a file in an audit's set B while its run is live.
