# Engineering handoff, 2026-09-05: tool fixes found by the first audit run under revision 2

Written 2026-09-05 by the session that prepared and ran the skill supersession audit
(`fanout/skill-audits/2026-09-05-v3-buildout-2/`, 46 jobs, harness 2.1.258). Every item
below is something a tool did wrong or a script left to the session's hands during that
run, with the evidence, the place, the fix and the test. Items that need a ruling before
code say so; the rest is building-a-tool work under the `testing` skill.

Read first, in full: this file; the `agent-runner` skill; the `testing` skill. For the
ProcessMap items, `.claude/skills/v3-buildout-2/SKILL.md` § Schema and `artifacts.md`.
Evidence for the runner items is the pilot's attempt:
`fanout/skill-audits/2026-09-05-v3-buildout-2/attempts/arm-A-22-the-referee/attempt-1/stream.jsonl`
(local, gitignored) and its ledger row (`ledger.jsonl`, job `arm-A-22-the-referee`).

## Agent runner (`tools/StoryPlanner.AgentRunner`)

**1. The stream parser treats every `system` event as the init event.**
Symptom: while a job ran, the page's stream view showed "init — model ; tools []; mcp
servers 0" over and over, and the label read "initinit" (the kind label plus the text's
own "init"). The raw stream had one `system/init` (tools `["Write"]`, model
`claude-sonnet-5`) and 55 `system/thinking_tokens` events, one per thinking step; harness
2.1.258 also emits a `rate_limit_event` after init. Where: `StreamEvents.cs` `case "system"`
(no subtype check); `Head/StreamPane.razor` (label + text). Fix: switch on `subtype`:
`init` renders as now; `thinking_tokens` becomes kind `thinking` with the estimated total
(or one running counter rather than 55 lines); any other subtype renders raw with the
subtype named; `rate_limit_event` becomes kind `usage` with the two utilizations, which
is a live figure the usage bar cannot get otherwise. Drop the redundant "init —" prefix in
the text. Test: stream-event tests with fixture lines copied from this run's stream (the
fourth shape the child's output has had; `RunnerPlan.ParseResultSummary` should be
re-checked against it too). Text follow-up: a line in the agent-runner skill § Traps.

**2. The run route and the stage strip describe the enqueued subset, not the job file.**
Symptom: after the pilot-only enqueue (`--job`), `/api/runs/<work>/<run>` reported
`pending 0` and `stages.batchComplete true` while 45 of the file's 46 jobs had never
launched; the batch enqueue then answered "46 job(s) enqueued" when 45 launched and one
was skipped as succeeded. Where: `RunCatalog.cs` line 81 (`jobSnapshots` come from the
live runner's `Jobs`, which under `--job` holds one), `RunnerHost.cs` line 217 (the message
counts `runner.Jobs.Count`). Fix: snapshots over the union of `jobs.json` and the ledger;
`pending` = jobs in the file with no terminal ledger row; batch complete = every job in
the file terminal; the enqueue message says launched / skipped-succeeded / already
terminal. Test: a catalog test with a 3-job file and a pilot-only ledger.

**3. The "tallied" stage means the tallier is present, not that a tally was produced.**
Symptom: `tallied true` at pilot time because `tally.ps1` sits in the work folder.
Where: `RunCatalog.cs` line 82. Fix, after tallier item 1: the stage is `tally.md` in the
run folder (the artifact row `tally-output`). Text follow-up: the stage list in the
agent-runner skill § The host and its page ("tally present").

**4. `/protocol` serves `fanout/PROTOCOL.md`, which is a retirement notice since 2026-09-05.**
The ruling (rulings log, 2026-09-04 evening): the file retires, the order is the map's,
the host serves the rendering. Where: `Head/Protocol.razor` (reads `FanoutRoot/PROTOCOL.md`),
`Head/Layout.razor` nav link, `Head/Runs.razor` and `Head/Run.razor` link text,
`RunSnapshot.cs` doc comment, the csproj comment. Fix: the route renders the v3-buildout
skill's generated `map.md` (repo root is the parent of `fanoutRoot`; the folder is
`v3-buildout-2` until the swap, so resolve `v3-buildout` first and fall back, or take a
`mapPath` in `host.json`). Then delete `fanout/PROTOCOL.md` and the README's line about it.
**Needs a ruling first:** the map's diagrams are mermaid blocks, which `MarkdownView`
(Markdig) does not render; the choice is to load mermaid.js client-side, or to render the
consumers table and validation report as HTML and link the file for the diagrams.

**5. Host ceilings are not persisted** (noted, not a bug). `host.json` has `maxParallel 4`,
`utilizationCap 80`; the run raised the ceiling to 64 through `PUT /api/host/settings`,
logged to `host-log.txt`, and the next host start reverts to 4. This matches "scheduled
intent lives in the host, not a file". If a different default is wanted for audit batches,
it is a `host.json` edit, not code.

## Tallier (`fanout/skill-audits/tally.ps1`)

**1. It prints to stdout and writes no file.** The artifact row `tally-output` is
`<run>/tally.md`; this run's was written by the session by hand, with a header line naming
the script and flags. Fix: `-Out` defaulting to `<run>/tally.md`, UTF-8 without BOM,
header line (script, flags, date), stdout kept. Test: run it on this run's `results/` and
diff against the committed `tally.md` below its header.

**2. Default flags omit `narrowed`.** Defaults are `absent, reversed, delegated`; the
method's omissions list (revising-the-method § revise) takes narrowed, reversed and absent.
`delegated` is a real relation in `protocol.md` (it occurred zero times here) and stays.
Fix: default `absent, reversed, delegated, narrowed`.

**3. No per-section breakdown.** Adjudication wanted relation counts per section of
document A in document order; the session computed it ad hoc and the table survives only
in the transcript. Fix: a "By section" table in the tally (section, first unit, one column
per relation), built from the manifest's section order.

**4. Malformed blocks are not flagged**, though the method says the tallier flags them.
The script accepts any relation string and never checks a block against the manifest.
Fix: a "Malformed" section listing blocks whose relation is not one of the protocol's
seven labels, whose note is empty on a relation other than `restated`, whose unit id is
absent from `items/manifest.md`, or whose id appears twice across result files; and units
of the manifest with no block at all. Nonzero exit when any. Test: a results folder with
one of each defect.

## Generator and document builder (`make-jobs.ps1`, `make-document-a.ps1`)

**1. The generator's invocation is not recorded.** Regenerating `jobs.json` without the
codebook meant reconstructing set B, `DocA`, arms, model, timeout and ceiling by reading
the old `jobs.json`. Fix: `make-jobs.ps1` stamps its full parameter set into `_comment`
(the set B list verbatim), so a regeneration is a copy of the stamp.

**2. Document A's file list and unit ranges are recorded by hand in `run.md`.** Fix:
`make-document-a.ps1` writes `document-a.files.md` (file, characters, order) beside the
document; the unit ranges per file need the manifest, so either a small script that joins
the two after `split`, or the split verb accepting a boundary marker and recording it.
Low priority.

**3. Convention question, Brian's:** `document-a.md` (56 KB, derived by concatenation of
committed sources) sits committed under the run root. The 2026-09-03 run split a single
file and had no such document. Either gitignore it as regenerable (the manifest's source
hash pins the bytes) or keep it as the audited text. Not code until ruled.

## ProcessMap (`tools/StoryPlanner.ProcessMap`)

**1. `state` cannot see referee runs.** Referee runs live at
`fanout/<instance>/referee/<run>/` (refereeing-a-candidate, artifacts.md § items), but the
run-scoped artifact patterns carry one `<run>` segment, so a round's referee-run artifacts
never count as present and `referee-run` is never the furthest process. **Needs a ruling:**
(a) the `<run>` placeholder admits an optional `referee/` prefix (a tool change plus one
sentence in the artifacts preamble), or (b) referee runs are named `referee-<date>` under
`fanout/<instance>/<run>/` (no tool change; the prose in refereeing-a-candidate and the
`items` description change). Recorded in the rulings log § validating the new folder.

**2. `state` ignores skill-audit runs** because they are not registry instances. By
design (state.md is the buildout's state); noted so nobody reports it as a gap.

**3. Entries without a falsifier count toward the implied status.** `StateBuilder`'s
"entries imply" reads every `- evidence` line after the last iteration; the existing
records are full of pre-referee entries. Whether the tool skips them is open item
unit-096 in `docs/v3-framework/methodology-revision-2-omissions-draft.md`; code follows
the ruling.

**4. The real-folder test repoints at the swap.** `RealProcessMapTests` reads
`v3-buildout-2`; handoff 2 step 5 repoints it at `v3-buildout` and extends it to run
`claude plugin validate .claude/skills`.

## Text follow-ups tied to these fixes (so the fix and the text land together)

- agent-runner skill § Traps: the fourth stream shape (runner item 1).
- agent-runner skill § The host and its page: "tally present" becomes the tally file
  (runner item 3).
- agent-runner skill's lifecycle paragraph and `fanout/README.md`: `PROTOCOL.md` deleted
  and `/protocol` serving the map (runner item 4).
- `revising-the-method.md` rows `audit-run` and `audit-judge`: `specified` to `built`, since
  the 2026-09-05 run executed under that text; a row edit plus `render`, not a ruling.

## Parked, not engineering

The adjudication of the audit is Brian's and is paused: the draft omissions list
(`docs/v3-framework/methodology-revision-2-omissions-draft.md`) holds 14 open items with
the session's proposals and 105 units assigned to rulings already in the log, all awaiting
his word. Handoff 2 steps 3 to 6 follow that.

## Must not

Republish the runner while a batch is live (`AgentRunner.exe stop` first). Edit a file in
an audit's set B while its run is live. Add a page control that changes what a job is.
Hand-edit a generated section. Treat any of the above as settled before its test exists.

## Status, 2026-09-06

Landed by the session that took this file up, each with its test, and verified on the
published host (stopped, republished, restarted; the smoke test enqueued under a new job
id, `smoke-sonnet-2026-09-06-stream`, succeeded with tools `[Write]` and no transcript):

- Runner 1: `StreamEvents` reads `system` by subtype (`init`; `thinking_tokens` as kind
  `thinking`, collapsed by `ReadTail` to one running line with the step count; any other
  subtype named and shown raw), `rate_limit_event` as kind `usage` with both windows, no
  kind repeated in the text. Fixture lines from the pilot's stream; `ParseResultSummary`
  pinned against the 2.1.258 result line with its late `type` key.
- Runner 2: `RunCatalog` builds the snapshot over the union of `jobs.json` and the ledger;
  pending is a file job with no terminal row; complete is every file job terminal; the
  enqueue answers "N to launch, N skipped as succeeded, N already failed and not
  relaunched" (`RunnerHost.EnqueueTally`). The host's launch gate now takes its usage
  figure from an injectable reader — the API tests had timed out against the developer's
  real cache at 81% versus the default cap of 80.
- Runner 3: the stage is `tally.md` in the run folder; strip label `tally`.
- Runner 4: `/protocol` renders `map.md` (`HostConfig.ResolveMapPath`: `mapPath` in
  `host.json`, else `v3-buildout` then `v3-buildout-2`), read on each visit;
  `fanout/PROTOCOL.md` deleted, README and the agent-runner skill updated. **Ruling still
  open:** Markdig's diagram extension already emits `<pre class="mermaid">`, so choice (a)
  is one script tag in `App.razor`; until ruled, each diagram is folded into a `details`
  block with its source (`MarkdownView.RenderWithFoldedDiagrams`, pure-tested).
- Tallier 1–4: `-Out` defaulting to `<run>/tally.md`, UTF-8 without BOM, header line,
  stdout kept; `narrowed` in the default flags; a "By section" table from the manifest's
  order; a "Malformed" section with the five checks and a nonzero exit. Self-test
  `tally.test.ps1` (one of each defect, then a clean run). Run over this run's `results/`,
  the output matched the committed `tally.md` line for line below the header, apart from
  CRLF; the committed file was **not** replaced (frozen artifact) and so lacks the two new
  sections.
- Generator 1: `make-jobs.ps1` stamps its full invocation, set B verbatim, into
  `_comment`. Generator 2: `make-document-a.ps1` writes `document-a.files.md` (order,
  file, characters, line range) beside the document; `document-a-ranges.ps1` joins it with
  the manifest through the document's headings and adds the units column, refusing on any
  heading two files share. Rebuilt from HEAD's eight files, the document matched the
  audited one apart from line endings, and the join reproduced the ranges `run.md` records.
- Text follow-ups: all four, plus `revising-the-method.md` rows re-rendered.

Awaiting Brian, unchanged: runner 4's diagram choice; generator 3 (`document-a.md`
committed or ignored); ProcessMap 1 (where a referee run lives, (a) or (b)); ProcessMap 3
(unit-096). ProcessMap 4 waits for the swap, as written.
