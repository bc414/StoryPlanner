# Skill supersession audit — v3-buildout (revision 1) → v3-buildout-2 (revision 2)

**Work:** `skill-audits` (a non-evidence action: the result is an adjudication, not candidates).
**Question:** does the revision-2 rewrite carry everything the revision-1 skill said? Unruled
by `methodology-revision-2-handoff-2.md`, whose step 3 compares process nodes against draft 1;
this run compares instruction units, and both feed the omissions list of the revision note.
**Cell:** auditor — slice scope (one section of the old skill per job), frozen predicate (the
three questions of `../protocol.md`, unchanged since the 2026-09-03 run), verify role. Sonnet.
**Instrument:** `../protocol.md`, per-unit form. Planned pilot: `arm-A-22-the-referee`
(seven units, the area the revision changed most).
**Document A:** `document-a.md`, eight files of the old method concatenated verbatim by
`../make-document-a.ps1` (the split verb takes one document): the old skill's six rule files —
SKILL.md units 001–045, hypothesis-records 046–069, evidence-pipeline 070–096, wu-execution
097–129, forward-plans 130–148, consolidation 149–159 — then the two rule-bearing documents
the revision retires outside the skill folder, added on Brian's ruling of 2026-09-05:
`fanout/PROTOCOL.md` 160–175 (its rules move into the agent-runner skill at handoff step 4)
and `docs/v3-framework/spec-pools/README.md` 176–179 (the pool entry format, succeeded by
the question entry format). Left out: `process-map.md` (the object of handoff step 3's node
comparison), `VERSION-HISTORY.md` (a fact file, not rules), and forward-plan-2 (a retired
plan; its § Codebooks section carried rules but was not itemized). No `#`/`##` heading repeats
across the eight files, so a unit's section names its file.
**Enumeration:** 179 units cut by `split`; `items/manifest.md`.
**Document set B:** the fifteen files of `.claude/skills/v3-buildout-2/` (SKILL.md,
artifacts.md, thirteen activity files), plus `CORPUS-STATUS.md` from the old folder (the new
SKILL.md names it as a companion; it moves unchanged at the swap and does not yet sit in the
new folder), the `agent-runner` skill and `fanout/referee/codebook.md` (the new skill delegates
to both, as the revision-1 audit's set B did). The rulings log is excluded on the 2026-09-03
ruling: intent is applied at adjudication, never given to the auditor.
**Arms:** arm A only. **Not measured:** stability (no second arm, so no disagreement count).
**Ceilings:** the run's `maxParallel` is 64 so the host's ceiling governs; the host's is raised
at launch (Brian's instruction of 2026-09-05: maximum fan-out).
**Status:** prepared 2026-09-05; dry run, pilot and batch held until the other session's edits
to `v3-buildout-2` have landed, since the ledger pins set B's hashes at launch. Because
document A carries PROTOCOL.md, the batch belongs after handoff step 4 (its rules moved into
the agent-runner skill) and before step 5 (the swap): run earlier, every PROTOCOL.md unit
comes back `absent` by construction; run later, the old folder is gone.
**Adjudication:** the omissions list of `docs/v3-framework/methodology-revision-2.md`, each
omission with its ruling; written after the run.

**Before launch (2026-09-05, later the same day; the session Brian put in charge):** the
holds above cleared — handoff step 2 landed on `v3-buildout-2` (validated, rendered, so the
generated sections now carry diagrams) and PROTOCOL.md's rules moved into the agent-runner
skill in revision-2 vocabulary (step 4's first move, taken early for this run), so units
160–175 have a place in B to be found. Set B changed once: `fanout/referee/codebook.md` was
removed and `jobs.json` regenerated, same ids, same sections. It is the retired three-input
draft, an A-side instrument — the new skill delegates to `codebook-N.md`, which referee-1
authors — and in B its excerpt rules would have let the old skill's excerpt units pass as
preserved; the new revising-the-method file now states the rule (no retired instrument in
set B). The audit itself was written into `revising-the-method.md` as the method's second
lint after the validator, as `audit-run` and `audit-judge`, so B also audits the rule that
this run exists. The ledger pins B's hashes at launch. Dry run 2026-09-05: 46 prompts
composed, 142–149K characters each (the 2026-09-03 run's were 75–90K: fifteen B files now,
with their rendered diagrams), nothing launched. Host started 2026-09-05 10:26 at its
configured ceilings (4 parallel, cap 80); raising them for maximum fan-out is a page knob at
launch. Pilot and batch not enqueued — Brian's go.

**Pilot (2026-09-05 14:28 UTC):** `arm-A-22-the-referee`, seven units, one attempt, 79 s,
exit 0, output check ok, api-equiv $0.39. Verdicts: two reversed (the three-input rule and
the excerpt handed to the referee — ruling 6 of 2026-09-04, the auditor citing the row and
the Never line that reverse it), two narrowed (both the falsifier's object moving from the
excerpt to the finding), two restated, one non-instructional. Read by Brian; his verdict,
in his words: "Proceed with the whole batch."

**Batch (14:33–14:37 UTC):** host ceiling raised to 64 (his instruction of the morning:
maximum fan-out); 45 jobs in flight at once, 46 of 46 succeeded, none failed, none
retried; api-equiv $16.78 for the run. Harness 2.1.258. **Tally:** `tally.md`, by
`../tally.ps1 -Flag absent,reversed,delegated,narrowed` — 179 blocks in 46 files: 82
narrowed, 45 restated, 20 absent, 17 reversed, 10 broadened, 5 non-instructional. **Not
measured:** stability (one arm). **Noticed about the harness:** the page's stream view
renders every `system` event as the init line, and harness 2.1.258 emits a
`system/thinking_tokens` event per step, so a running job shows empty "init" lines;
display only (`StreamEvents.cs` case "system"); a runner change and a traps line for the
agent-runner skill, after this run since that skill is in set B.

**Adjudication (2026-09-05, after the tally):** the draft omissions list is
`docs/v3-framework/methodology-revision-2-omissions-draft.md` — 14 open units with the
session's proposals, 105 assigned to rulings already in the log — and Brian's rulings on it
are pending, by his word. The tool defects this run surfaced (the stream view, the stage
strip and counts under a pilot-only enqueue, the tallier writing no file) are in
`docs/v3-framework/engineering-handoff-2026-09-05.md`.
