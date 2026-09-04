# WU2.15 — Retroactive referee pass: process map first, then the pass

> **DEFERRED 2026-09-04 (evening)** until methodology revision 2 lands
> (`methodology-revision-2-handoff.md`): the process map moves into the skill as its router,
> and this plan's step 2 text fixes fold into that revision. Step 1 here is complete
> (draft 1 at `32b6d4b`); step 3 resumes only after the revision. The copy of this plan in
> `~/.claude/plans/` is superseded by this file.
>
> **Approved plan of 2026-09-04, saved to the repo because the work spans sessions.**
> Status at close of the planning session (2026-09-04 evening): **step 1 delivered, paused
> for Brian's review** — `process-map-1-draft.md` and its artifact
> (https://claude.ai/code/artifact/0a5fee6b-7be7-4172-8544-7239a9d97983), gap table G1–G24.
> Nothing under step 2 or step 3 has run. The next session: read this file, the draft map
> and its gap table, Brian's review feedback, then `v3-buildout` SKILL.md → `wu-execution.md`
> → `evidence-pipeline.md` → the `agent-runner` skill in full, before touching anything.
> Iterate the map through the review (changelog in the draft), apply step 2 on outcome A,
> write the snapshot, then Session A of step 3. Plan-mode rulings taken in that session are
> written back into this file's § Rulings log, because AskUserQuestion answers are elided
> from the code-sessions archive (§ Carry-forward, item 1).

Plan written 2026-09-04, revised the same day after two rulings by Brian: the referee is
**discrimination-only with two inputs** (statement, candidate); and a **process-flow map of
the whole pipeline** is produced and reviewed before any WU2.15 work runs. Governing texts read
in full: `v3-buildout` SKILL.md and its companions, `fanout/referee/codebook.md`,
`fanout/PROTOCOL.md`, `methodology-revision-1.md`, the `agent-runner` skill, the WU2.15 card,
`retroactive-referee-pass-handoff.md`. Provenance of the ruling traced in `codesessions.db`,
session `24ad1d89`, 2026-09-02 22:27 to 2026-09-03 22:16.

## Glossary

- **Finding / candidate / evidence / question** — what a pass observed; a finding a
  verification pass claims bears on a hypothesis (a block in `candidates.md`); a promoted
  candidate; a spec-pool entry.
- **Diagnostic candidate** — one for which the referee could name an observable that would
  differ if the statement were false, tagged supporting or challenging by which side the
  finding falls on. Non-diagnostic: no observable, vacuous clause, or consistent with both.
- **Disposition** — the promotion session's appended line: promoted (with the entry
  timestamp), declined (with reason), held (with the named blocker). Status is derived from
  the last appended line.
- **Calibration** — first-use requirement of every codebook hash: sample scored blind by the
  agent and by Brian, adjudicated, rulings applied, re-hashed, record written. Repeats only
  when a ruling changes the file.
- **Citation support** — whether a finding is true of its source. Under the 2026-09-04
  ruling it belongs to the verification experiment's instrument in SOP and to the promotion
  session's source read always; **never to the referee**.

## Context — why this WU exists

Forty evidence entries across twenty hypothesis files (023–040, 045, 046; WU1.1 23, WU1.3 14,
WU1.2 3) were deposited 2026-08-31 to 2026-09-01 by the sessions that searched for the
patterns, clause self-administered or absent (only 9 of 20 files carry any clause). Under
the strong form all forty are unverified leads, as are the 17 `evidenced` and 3 `challenged`
statuses computed from them. Re-check them with the appropriate greps before relying on any
of these numbers:

```
grep -h -o '^- evidence | [^|]*| ([^)]*)' docs/v3-framework/hypotheses/0*.md
grep -c 'Would differ if false' docs/v3-framework/hypotheses/0*.md
grep -h '^status:' docs/v3-framework/hypotheses/0*.md | sort | uniq -c
```

The pass does three things at once: cleans the record (each entry re-promoted verbatim with a
referee line, or declined with its status recomputed; the pools already hold every question);
tests hypothesis 048 (blind clause versus original clause, a comparison only possible once);
and calibrates the referee codebook that every later verification WU promotes through.

## The ruling of 2026-09-04 and why (recorded here; to be recorded in the repo in step 1)

The referee codebook carried two checks under one name: discrimination (R1, R3, R4, R7, R8,
R9; decidable from statement and finding wording) and citation support (R2, R5, R6; needs
the source, whose shape varies by cell). The second entered by accretion, not decision: the
00:31 check list on 2026-09-03 put "citation support" on the referee; the 00:46 design gave
citation to the depositor; the 01:56 draft example had the referee verify a count against
the CSV; the 01:57 explicit-context argument listed "the cited source" as an input while
arguing about exclusions; the 02:49 plan codified three inputs; the 14:36 pipeline file and
the 22:11 codebook wrote rules around an input already present. R6 is the repealed deposit
protocol's rule 4 moved to the referee. **Ruling:** referee = discrimination only, two
inputs; citation support = experiment instrument (SOP) + promotion source read (always).

## Step 1 — the process map, then pause (Brian reviews)

**Deliverable, two objects.** A *draft*, `docs/v3-framework/process-map-1-draft.md` and an
artifact (artifacts render mermaid natively), iterated in place through the review loop
with a dated changelog section and gap rows updated to "fixed in file § section" as step 2
lands each fix. Then the *snapshot*, `docs/v3-framework/process-map-1.md`, written once at
the end of step 2 when the instruction text it maps is settled, numbered with the
methodology revision, regenerated at each revision and otherwise never edited (the
stale-mirror trap: the index status column and the plan-2 coverage table). The draft file
is deleted when the snapshot lands.

**Sources:** instruction text only — `v3-buildout` SKILL.md and companions, `agent-runner`
SKILL.md, `fanout/PROTOCOL.md`, `fanout/referee/codebook.md`, `spec-pools/README.md`,
`fanout/README.md`, the WU2.15 card and handoff, CLAUDE.md's third-role paragraph. Never a
transcript. Every node attribute and every edge cites file § section.

**Shape:**
- Three levels, separate mermaid diagrams, one id scheme (`P.n` plan cycle, `E.n` / `V.n` /
  `S.n` / `I.n` per WU type, `R.n` runner lifecycle, `F.n` referee run, `M.n` promotion).
- Two node shapes: process and file. File nodes carry path and committed / gitignored /
  regenerable.
- Four attributes per process node: actor kind (Brian · HITL session + model · autonomous
  agent via runner + model · script · C# tool), inputs, outputs with paths, governing text.
  Nodes coloured by actor kind so the explicit-context boundary is visible as a subgraph.
- Level 2 is **four** diagrams, one per WU type (exploratory, verification, synthesis,
  infrastructure); synthesis and infrastructure are short and have no runner path except
  codebook calibration.
- Three kinds of branching, drawn distinctly: **exclusive choice** as a diamond (WU type;
  explicit-context cell → runner, else HITL; regenerable → `items/`, else committed;
  `mcp`; calibrated or not; diagnostic or not; promoted / declined / held); **parallel
  fork and join** as a fork node (arms; a runner batch running while the HITL session that
  enqueued it continues and watches — the session is the orchestrator, the runner its
  instrument; the 049 two-arm cell), joined at the tally or adjudication; **optional** as a
  dashed edge (a second arm dropped for compute, obliging a "not measured" line). A branch
  the text does not decide is recorded as a gap.
- Legend states: exists today · specified not built · contradictory.

**Companion gap table** (`§ Gaps` in the same file), keyed by node id: attribute with no
citation; citations that disagree; text that does not match Brian's stated intent. Seeded
with the gaps already known: three-input referee text in the codebook and pipeline file;
who appends the verdict (pipeline file says the referee, handoff and card say promotion
copies from `results/`); referee generator and tallier not built; the runner skill's
excerpt example names the referee; PROTOCOL.md does not say which steps apply per WU type
or that the referee run is a nested lifecycle at step 11; the elided 2026-09-03 plan-mode
answers cannot show whether Brian ruled on referee inputs.

**Pause.** Brian reviews the map and the gap table. Outcome A: gaps are fixes; continue to
step 2. Outcome B: the pipeline needs restructuring; this plan is rewritten and the WU
waits. Nothing below runs before that review.

## Step 2 — record and fix (only after the pause, outcome A)

1. Dated addendum to `methodology-revision-1.md`: the referee-input provenance above and
   the ruling.
2. `fanout/referee/codebook.md`: two inputs; R2 recast to wording ("the observable must be of
   a kind the cited source could contain, judged from the source line"); R5 and R6 removed,
   R6's substance noted as item-definition guidance for classifier codebooks; status line
   updated. `evidence-pipeline.md` § The referee: "inputs are exactly two"; the verdict is
   copied from `results/` into the candidates file by the promotion session; § Promotion:
   the source read is the citation check and is recorded on the disposition line.
3. `agent-runner` SKILL.md: the non-regenerable-input example names experiment inputs
   (flagged notes for a focused reader), not the referee. `fanout/PROTOCOL.md`: which steps
   apply per WU type; step 11 = referee (discrimination) then promotion (citation + review).
4. Handoff and WU2.15 card amended: no `excerpts/`; promotion source reads instead.
5. Any further gap from the review, as Brian ruled.

## Step 3 — WU2.15 execution (SOP vs one-time, tagged)

| Step | Tag | Rationale |
|---|---|---|
| Work folder `fanout/WU2.15-retroactive-referee/` with `candidates.md`; referee run under `fanout/referee/<date>-retroactive/` | SOP | Vertical by work; every referee run under the referee's folder. |
| Candidates from existing record entries, `proposed-by: retroactive / <WU> / <timestamp>`, original clause withheld | One-time | Normal candidates come from a verification experiment; only a pre-pipeline record needs entries turned back into candidates. Recurs narrowly under iteration → re-referee. |
| Referee codebook calibration (20 of 40, stratified over WU1.1/1.3/1.2, ≥ 3 hypotheses, plausible members of all three classes; Brian scores statement + candidate blind on a sheet; adjudicated; rulings → new hash; `calibration-<date>.md`) | SOP first-use, one-time for this hash | Rule 4. Note in the record that this sample has no instrument-backed citation, unlike SOP candidates. |
| Referee jobs: one job, one candidate, one target; Sonnet; `mcp: false`; tools Read/Write; inputs = `items/statement-NNN.md`, `items/C-NNN.md` (both regenerable from the record and the candidates file); `requireOnce` = candidate id + the two verdict-line markers; `make-jobs.ps1` / `tally.ps1` beside the codebook; dry run → pilot → batch | SOP | Pipeline § The referee; the runner's four rules. Generator and tallier written here are reused by every later referee run. |
| Referee runs against the current statement even where the hypothesis iterated after the entry | SOP | Iteration → re-referee. |
| 048 table: blind clause vs original, bins same / narrower / different / original vacuous / original absent | One-time | Only possible once; the fifth bin exists because 11 of 20 files have no clause. Deposited as candidate C-041 through the same referee. |
| Promotion: source read per diagnostic candidate, recorded on the disposition line; verbatim entries; declines with reason and pool link; `(superseded by re-referee <date>)`; statuses recomputed; one commit; report | SOP; one-time in scale and in that the source read is the *only* citation check these forty ever get | Pipeline § Promotion. |
| Report: counts per class, per WU, per hypothesis; clause agreement by bin; referee disagreements; runner behaviour; every reactive fix and whether it looks like SOP | SOP form, shakedown weight | First live run. |

**Sessions.** A: candidates file, generator, tallier, sample, dry run, pilot, calibration
run, scoring sheet (statement + candidate, no referee output visible), `run.md`; stop.
Brian scores. B: adjudicate, calibration record, re-hash if rulings changed the codebook,
regenerate all 40 with new ids (new run folder if the hash changed), pilot three, batch,
tally, copy verdicts into `candidates.md`, 048 table and C-041. C: promotion with source
reads, one commit, card and status-board update, report.

## Files

- Step 1 creates `docs/v3-framework/process-map-1.md` (+ artifact).
- Step 2 edits `methodology-revision-1.md`, `fanout/referee/codebook.md`,
  `.claude/skills/v3-buildout/evidence-pipeline.md`, `.claude/skills/agent-runner/SKILL.md`,
  `fanout/PROTOCOL.md`, `retroactive-referee-pass-handoff.md`, `forward-plan-2.md` (card).
- Step 3 creates `fanout/WU2.15-retroactive-referee/candidates.md`,
  `fanout/referee/{make-jobs.ps1, tally.ps1, calibration-<date>.md}`,
  `fanout/referee/<date>-retroactive/{run.md, jobs.json, items/manifest.md, results/,
  ledger.jsonl, calibration-sheet.md}`, `docs/v3-framework/WU2.15-retroactive-referee.md`;
  Session C edits the twenty hypothesis files plus 048 in one commit.
- Pattern reused: `fanout/skill-audits/make-jobs.ps1` and `tally.ps1`.

## Verification

- Step 1: every process node has all four attributes or a gap row; every edge cites text;
  the artifact renders all three levels.
- Step 2: `grep -n 'excerpt' fanout/referee/codebook.md` returns nothing; the pipeline file
  and codebook agree on two inputs and on who appends the verdict.
- Step 3: dry run shows each prompt = codebook + two inputs and no clause; pilot ledger row
  `Mode: pilot`; batch ledger all succeeded with all markers; `tally.ps1` zero malformed;
  host page shows "calibrated" only after `calibration-<date>.md` exists; after Session C
  every evidence entry in the twenty files ends in `(superseded by re-referee <date>)` or
  carries a referee line; no transcript appears under the StoryPlanner project directory.

## Does not

Rewrite any finding; edit any tag in place; delete an entry; promote anything Brian has not
reviewed in the diff; run a batch under the draft hash; treat "consistent with" as
diagnostic; give the referee a source excerpt; write to any `.storyplan`; run any of step 3
before the step-1 review.

## Rulings log (append; one line per ruling, dated, Brian's words where given)

- 2026-09-04 — Referee is discrimination-only, two inputs (statement, candidate). Brian:
  "Referee should be discrimination only, two inputs."
- 2026-09-04 — Calibration sample: 20 of 40, stratified. Brian's blind verdicts on a scoring
  sheet the session prepares. Next execution session stops after the referee's calibration run.
- 2026-09-04 — A process map with per-node actor / inputs / outputs / governing text and a
  gap table precedes WU2.15; the plan pauses for Brian's review after it; then continue or
  rewrite. Snapshot rule accepted with the draft-then-snapshot handling.
- 2026-09-04 — Level 2 of the map is four WU types; branching drawn as exclusive choice,
  parallel fork/join, and optional.
- 2026-09-04 — Draft 2 of the map is done **in place** (same file, changelog entry), not as a
  copy; draft 1 is committed first as the fixed reference. Draft 2 adds a **root registry**
  (goals, constitutional rules, incidents, hypotheses under test) and an authored **root
  purpose** column per process. **Consumers are never authored**: the map's ground truth
  becomes a rows file (one row per process with input/output file ids; one row per file;
  edge rows carrying label and branching kind), a generator script beside it renders the
  mermaid and tables, and a consumer is a query over the rows. The generated diagrams are
  checked against draft 1 by set comparison of node ids and edge pairs extracted from the
  committed text, plus one visual look; only layout and labels are eyeballed. Cut decisions
  wait for that pass — five "confident" cuts on 2026-09-04 reversed to two once consumers
  were written out (status board → derive at read time; V artifact → drop per-item
  duplication only; pool `status` → keep, add promotion as writer; stage strip → keep, fix
  G13; scope reconciliation → keep where it gates, fold where clerical).
- 2026-09-04 — Map machinery settled (Brian: "this combination works"): **source** = markdown
  tables in the map file itself (roots, files, processes, edges — fixed columns, ids in every
  cross-reference); **engine** = a small C# tool under `tools/` with pure tests, reading the
  tables through the runner's markdown unit rule where reusable, validating (unique ids, known
  references, every process cites a root, every output has a consumer) and emitting derived
  views; **visualizer** = generated mermaid written into marked sections of the same file,
  never hand-edited, republished as the artifact. Rejected: JSON as source (rationale prose
  reads worst there), PowerShell as engine (no tests; 5.1 quirks already cost the skill-audit
  scripts), a separate visualizer (artifact + GitHub rendering suffice; the runner host could
  serve `/process-map` later if interaction is ever needed).

## Carry-forward — insights from the 2026-09-04 planning session not yet in any governing text

1. **Plan-mode rulings are invisible to the code-sessions archive.** AskUserQuestion answers
   arrive as tool results and are elided (`[tool result elided — N chars]`); the only record of
   what a plan-mode session proposed is its `~/.claude/plans/*.md` file, outside the repo.
   Today's provenance of the three-input referee hinged on one such file
   (`~/.claude/plans/concurrent-prancing-marshmallow.md`, 2026-09-03). Proposed convention,
   Brian's call: an approved plan is copied into the repo at approval (this file is the first);
   the 2026-09-03 plan is copied in as the evidence for gap G6; and a plan carries a rulings
   log written in the session. Consider noting the elision in the `code-sessions` skill.
2. **Worked examples carry rules silently.** The referee's citation check entered through an
   example line at 01:56 on 2026-09-03 ("sub-count verified against CSV") before any rule said
   it. When reading or auditing a codebook or skill, treat every example as a rule and check it
   against the stated ones.
3. **The referee's purpose test.** The four goals ruled at 00:46 on 2026-09-03 (findings
   preserved; discriminating evidence separated from context by status; the separation not
   judged by the producing party; the record never touched by unreviewed machine judgment) are
   the yardstick: a duty that is not "separation, judged by a non-producing party" does not
   belong to the referee. Use it on any future proposal to widen the referee.
4. **The citation record's shape is per cell**, and promotion's source read consumes it:
   classifier / auditor → the item by id plus its label and the tally, all hashed in the
   ledger; census → the script's table and the script; investigator / focused reader → the
   cited locus fetched as the corpus's whole unit (a note, a block, a lineage turn), committed
   when not regenerable. The retroactive pass has none, which is why its promotion source reads
   carry the whole citation burden. Feeds gaps G8 and G9.
5. **WU2.15 contains no experiment.** It is verification (SOP, first live run) + bootstrapping
   (referee calibration; candidates rebuilt from record entries) + one measurement (048). The
   calibration sample therefore differs from SOP candidates in having no instrument-backed
   citation; the calibration record should say so.
6. **048 measurement details.** Fifth bin "original absent" because 11 of 20 files carry no
   clause. Hypotheses 029, 030 and 033 iterated after their entries were deposited, so their
   original clauses (where present) were written against an earlier wording; the comparison
   for those rows is against a moved target and the table marks it.
7. **Brian's blind scoring mechanics** (gap G12): a `calibration-sheet.md` in the run folder
   with statement and candidate per sample item and blank clause / verdict lines; the referee's
   `results/` stay unopened by Brian until his sheet is complete; the session adjudicates and
   records agreement per class.
8. **SOP / one-time / reactive tagging** is Brian's requirement for any first-run WU, with a
   rationale per step (gap G24); it is not in `wu-execution.md`.
9. **The map's maintenance rule**: the draft iterates with a changelog; the snapshot is written
   once when the text it maps settles; regenerated at each methodology revision, never edited.
10. **Runtime facts at close**: runner exe published; host not running; the fanout launch
    folder holds only its README; `fanout/referee/` holds only the draft codebook.
11. **The excerpt storage rule survives** for experiment inputs (e.g. WU2.6's flagged notes
    fetched for a focused reader), just not as a referee concern.
12. **PROTOCOL.md is the verification lifecycle plus runner mechanics**; exploratory runs use
    steps 3–10 and 12; the referee run is a nested instance at step 11 (gap G5).
