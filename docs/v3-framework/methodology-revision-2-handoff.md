# Handoff: methodology revision 2 — the process map as the skill's router

Written 2026-09-04 (evening) for the sessions that carry it out. Revision 1 (2026-09-03) gave
the `v3-buildout` skill its rules. This revision gives it a **spine**: a table of processes
with actor, inputs, outputs, root purpose and governing section, validated by a tool and
rendered to diagrams, living in the skill folder as its router. WU2.15 (the retroactive
referee pass) is **deferred** until this lands; its plan is `WU2.15-plan.md`.

## What prompted it

Drawing the pipeline as the instruction text states it (`process-map-1-draft.md`, commit
`32b6d4b`) found twenty-four gaps in a day, one of them a duty (citation support) assigned to
three parties by accretion and never by decision. The map found them because the skill has
no place where a process's existence, owner, inputs, outputs and purpose are stated together;
the prose companions carry all of that implicitly. A router of "activity → file" says where
to read, not what exists or why.

## The principle (Brian, 2026-09-04)

The same answer v2 gave the planner's taxonomy: **Type Object**. Schema stable, definitions
as rows, evolution by data entry validated by the instrument. Here the schema is the fixed
columns plus the validator; the rows are processes, roots, files and edges; iterating the
method is editing rows and re-running validation, never rewriting a document. "Final in
shape, data in flux." Nothing here is settled, exactly as the epistemic framework is not:
the rows are the current state, git is the history, a revision note is the migration record.
There is no numbered snapshot series; that idea (from the WU2.15 plan) is dropped.

## Rulings so far (all 2026-09-04)

1. The map moves **into the skill folder** and becomes the router. Topology (what exists,
   who runs it, inputs, outputs, purpose, governing section) is ground truth in the tables;
   procedure (how) stays ground truth in the prose companions; a conflict is a gap, with
   precedence declared: topology from the table, procedure from the prose.
2. **Source** = markdown tables in the skill (fixed columns, ids in every cross-reference).
   **Engine** = a small C# tool under `tools/` with pure tests, reading tables through the
   runner's markdown unit rule where reusable. **Visualizer** = generated mermaid in marked
   sections of the same files, never hand-edited; the artifact is republished from them.
   Rejected: JSON as source, PowerShell as engine, a separate visualizer.
3. **Consumers are never authored.** A consumer is a query over rows (processes whose inputs
   include this process's outputs). Draft 1's five "confident" cuts reversed to two once
   consumers were written out; no cut decision is made before the validator runs.
4. Changes are made **in place, additively, in two moves**: first the companion
   `process-map.md` and the tool land beside the unchanged skill; then, after validation, one
   commit replaces `SKILL.md`'s routing table with the level-1 process table and adds the
   revision note. Other sessions load the skill from the same tree, so nothing breaks between
   the moves. Draft 1 at `32b6d4b` is the comparison reference: node ids and edge pairs are
   set-compared, mechanically, plus one visual look.
5. `SKILL.md` keeps the epistemic framework, the work matrix and the constitutional rules, and
   its routing table becomes the level-1 process table. Levels 2–3, the root registry and the
   generated diagrams live in `process-map.md`. Autonomous agents are unaffected (they never
   see the skill).
6. The referee is discrimination-only with two inputs (ruled earlier the same day; provenance
   in `WU2.15-plan.md` § The ruling). The text fixes it implies (codebook, `evidence-pipeline.md`,
   the runner skill's excerpt example, `PROTOCOL.md` per-type applicability, the WU2.15 handoff
   and card) are **part of this revision**, made as row-and-prose edits together.

Rulings 7–13 were made 2026-09-04 (evening) in the session that built the validator, before any
code was written; they are schema decisions the tool now enforces, and every one of them fails
against the current draft rows on purpose. The validator's own § Format summary in
`process-map.md` carries the short form.

7. **`governed-by` is one column and it is file-granular.** One repo-relative path; a `§` in the
   cell is a syntax error. The cell does one job wearing two hats that are the same hat: it is
   the reading assignment (`SKILL.md`'s routing heading already says "read the named file in
   full before acting") and it is the precedence declaration of ruling 1 — when the row and the
   prose disagree, "the prose" must be a well-defined document. Both are document-granular.
   *Rejected:* a second `locus` column. For a process row the *why* is already the `roots` cell
   and `Roots.source` already carries locus precision, so a second column would mirror what the
   file and the root together already state — the stale-mirror failure of incident I6. The one
   real loss, "which section of `wu-execution.md`", comes back as a **fan-in report**: the
   validator prints how many rows each governing file carries, so a document asked to govern too
   much is visible and can be split, rather than the split being assumed.
8. **`Roots.source` keeps a strict locus grammar:** `<path> [§ <heading>] [¶ <n>]`. The heading
   must match **exactly one** heading in the file — zero and two are both failures, since a
   citation names one place. `¶ n` addresses the nth top-level ordered-list item under that
   heading and must exist, so `SKILL.md § Constitutional rules 5` becomes
   `… § Constitutional rules ¶ 5` and is checked. A bare trailing integer is a **syntax error**,
   never normalised away: silently stripping it is how a pointer to rule 5 widens into a pointer
   to all nine without anyone deciding to widen it. Several sources in one cell separate with `;`.
9. **Paths are repo-relative, one form only.** No search order means no ambiguity by
   construction; a bare `SKILL.md` that could mean either of two skills is rejected rather than
   resolved by a precedence nobody chose. **`fanout/` is in this repo** — `fanout/PROTOCOL.md`
   and `fanout/referee/codebook.md` both exist and resolve like any other path. The external
   `RiderProjects\StoryPlanner-fanout` is the agent *launch* folder, a separate thing that no
   cell cites. (This corrects § Building the validator below, which said otherwise.)
10. **Every process row needs ≥ 1 input and ≥ 1 output, both hard failures.** A process that
    writes nothing cannot be told apart from one that did not run; a process that reads nothing
    is deriving from recall, which C8 forbids. Symmetric with the existing ≥ 1 root rule, and it
    is what makes the promotion gate meaningful — a Brian node with no output cannot be shown to
    have gated anything.
11. **No terminal-record exemption.** `process-map.md` contradicted itself here: its opening note
    lists "a file with no consumer" as one of three mechanical gap kinds with no escape, while
    § Format offered one (`keep` committed and named in a `governed-by`). The absolute reading
    governs. A file read only by a person is read by a **process the map is missing a row for**,
    so the fix is a reading row, not an exemption. The tool reports three distinct kinds rather
    than one, because they are three different defects: written-never-read, read-never-written,
    and cited by no process at all.
12. **The promotion gate walks the union graph, and a path ends at the write.** Control edges
    plus derived data-flow edges; a path from `f.cand` to `f.hyp` terminates at the first process
    writing `f.hyp` and must carry a `brian` actor before that point. A review *after* the write
    is detection; C1 and C2 are preventive. `M.4` is therefore reported — it reads `f.cand` and
    writes `f.hyp` in one hop, with `M.5` only an `optional` edge and `M.10` reviewing the diff
    after the commit. Whether that is a defect in the rows or in the pipeline is the gap
    review's ruling; the gate's job was to surface it. A check whose subject set is empty is
    reported as **VACUOUS**, never as passing: no process writes a `.storyplan`, and "the rule
    holds" and "the rule has nothing to hold over" are different facts.
13. **The tool carries its own table parser rather than referencing the runner's.**
    `StoryPlanner.AgentRunner` is a `Microsoft.NET.Sdk.Web` project with Markdig and
    `OutputType=Exe`, so a `ProjectReference` would drag the ASP.NET framework reference into a
    plain console tool; `UnitSplitter` also emits raw row lines and never splits cells, which is
    most of the work. `MapTables.cs` carries the same unit rule — header and separator are
    structure, every body row is a unit — pinned by its own tests. Recorded here as
    § Building the validator asked.

## Design inputs adopted from practice (Brian: "good things to keep in mind")

1. **Promotion gate as a checkable policy** — the validator walks every path from a
   candidates file to a hypothesis file (and to any `.storyplan` write) and fails if a path
   lacks a Brian-actor node. Constitutional rules 1–2 as a test.
2. **`kind` column on process rows** — `sop | bootstrap | reactive`; the validator lists
   bootstrap rows still present after their WU completed.
3. **Line budget on `SKILL.md`**, enforced by a test; detail flows to companions.
4. **Codebook anchors sit under the rule they anchor and cite their calibration ruling.**
   *Re-stated 2026-09-04 (late).* As first written this read "examples declare the rules
   they exercise ('exercises R3, R8'); would have caught the referee's smuggled citation
   duty". That justification was wrong on inspection: R2, R5 and R6 were in the rules list,
   so examples tagged with them would have validated cleanly and entrenched the duty rather
   than exposed it. The practice stands on its own ground — codebook craft, the anchor-
   example discipline of content analysis: an example is harvested from a calibration
   disagreement, placed under the rule it anchors (Gherkin's `Rule:`/`Example:` nesting),
   and cites the calibration record it came from; a rule with no anchor is a rule that has
   not yet failed at a boundary and is not a defect. It governs the *instrument applied to
   data*, so its ruling and the nesting-versus-tag-line choice belong with the codebook
   conventions in the `agent-runner` skill, not in this revision; the validator's codebook
   check becomes "every example sits under a rule heading and cites a calibration record
   that exists", reported vacuous while there are none. The scope-drift guard that I8
   actually needed is input 8 below.
5. **Held-out calibration split** — rulings from most of the sample, agreement reported on the
   untouched remainder; the record is per (codebook hash, model).
6. **Revision notes list changed process ids**, from the table diff.
7. **The runner's item rule applied to the map** — one actor, one line per process row; a row
   needing two actors is split.
8. **A process row's declared inputs must match what its run composes** *(added 2026-09-04,
   late)*. The smuggled citation duty was a change in the referee's *scope* — a third input
   and three rules acting on it, accreted in prose across five turns — which is topology,
   the F.4 row's `inputs` cell, and the layer the map's validator exists for. The runner
   already produces the checkable artifact: every job inlines and hashes its inputs. So for
   any process with an `agent:` actor, the set of inputs the job file composes must equal
   the row's `inputs`; a job carrying a file the row does not name fails. Where this check
   runs (the validator reading `jobs.json`, or the runner's dry run reading the map) is the
   gap review's ruling.
9. **Topology stated in a codebook's prose points at the row, never restates it** *(added
   2026-09-04, late)*. The referee codebook's § What you are given ("exactly three inputs")
   is the F.4 row's job done again in prose, and it is where the accretion landed. Under
   ruling 1 the row wins; the landing step's codebook edit replaces the enumeration with a
   reference to the row (and to the job file that materialises it), so the next accretion
   has nowhere to land.
Not adopted: a measure column per process (hand-kept staleness); an executable workflow
graph (would put salience judgments behind code).

Checked against Anthropic's published skill guidance on 2026-09-04 (Claude Code docs
`skills.md`; platform "Agent Skills best practices"): the router-plus-companions shape is
their progressive-disclosure pattern 1 ("a high-level guide with references"); the two hard
numbers are 500 lines for `SKILL.md` and 1,024 characters for `description`; references must
be one level deep and every companion linked from `SKILL.md`; supporting scripts are shown
under a `scripts/` folder by example, not by requirement — this repo keeps instruments under
`tools/` with tests, and a one-line pointer from `SKILL.md` to `tools/StoryPlanner.ProcessMap`
satisfies the "say what each file is and when to use it" guidance.

## Build-vs-buy check of the validator (2026-09-04, late) — five records

After the validator landed, Brian asked whether a tested, documented tool should replace it
and the on-the-fly table format, since the planner and the epistemic framework are bespoke
but process management is not. A web survey answered it. Brian ruled to continue on the
current path; these five records are what the survey adds to the revision.

### 1. Rejected alternative: an off-the-shelf process or traceability tool

Examined 2026-09-04: requirements-traceability tools (Sphinx-Needs, StrictDoc,
OpenFastTrace, Doorstop), model-as-code tools (LikeC4, Structurizr DSL), BPMN linting
(bpmnlint), supply-chain layouts (in-toto), and graph/policy engines (SHACL with SPARQL,
Rego via conftest). Rejected, with the reason recorded so no later session re-derives the
question: every one of them covers the registry layer (typed items, unresolved references,
orphans, coverage, generated diagrams) and none covers the graph policy (the Brian-gate on
paths to a hypothesis write, ruling 12) or the locus grammar (ruling 8); each would put a
second schema beside the prose, which is the stale-mirror failure of incident I6; and the
gap list the validator produces lives in the rows, which a bought tool would report
identically. Also rejected: an MCP server as the validator — validation is a build step
that a session must not be able to skip, and an MCP tool runs at the model's discretion.
Sphinx-Needs was the closest single fit (JSON-schema network validation across link hops,
needflow diagrams, `needs.json` export); its cost is a Sphinx build and directive syntax in
place of the tables. Two pages could not be fetched (OpenFastTrace's user guide,
Sphinx-Needs' MyST page), so their Markdown-source claims rest on project descriptions.

### 2. Owed: an exploratory session on skill hooks (runtime enforcement)

The map has a fourth layer the tool does not reach. The validator checks that the rows say
a `brian` node precedes every hypothesis write; nothing checks that a live session obeyed
the rows. Claude Code's PreToolUse hook is the tested mechanism for that: a stateful script
that can deny an Edit or Write by path unless a prerequisite Read was observed in the
session, declared in a skill's frontmatter so it registers when the skill loads
(https://code.claude.com/docs/en/hooks, "Hooks in skills and agents"). Anthropic's skills
page says it directly: when a skill stops influencing behaviour, enforce with hooks rather
than stronger prose.

**Brian ruled 2026-09-04 that this needs an exploratory session on where it can apply**
before anything is built or added as a row. That session's questions, at least: which
rules in the map are hook-shaped (a path plus a required prior read, or a path that is
never written) and which are not; whether a hook's policy can be generated from the same
rows the validator checks, so the map stays the single source; what a hook can and cannot
see (it has no session history — the docs say `transcript_path` lags — so "was X read" needs
state the hook keeps itself); the cost of false denials in a HITL session; whether a hook is
also the durable trace G23 lacks for plan-mode rulings; and how autonomous agents, which
never load the skill, are or are not covered. Its output is a finding for the gap review,
not a row. Not part of this revision's landing.

### 3. Borrowed vocabulary, named

Revision 1 § Framing and vocabulary lineage exists so a reader can tell a borrowed term from
an invented one; the same for this revision. The Processes columns are IDEF0's ICOM box —
`inputs` = input, `governed-by` = control, `outputs` = output, `actor` = mechanism — and
"consumers are never authored" is IDEF0's rule that an output arrow exists only as another
box's input (SIPOC is the same shape). The Brian-gate of ruling 12 is separation of duties;
its nearest formal statement is in-toto's layout of steps, authorised functionaries,
materials and products, which verifies executed runs against a layout — the run-time half
that record 2 is about. The Roots table's "every root cited by ≥ 1 process" is the coverage
check of requirements traceability (OpenFastTrace's "needs/covers" per artifact type).
`kind = sop | bootstrap | reactive` and the locus grammar are this project's own.

### 4. Design input 4 has a native form — and was mis-justified

"Examples declare the rules they exercise" is Gherkin's `Rule:` / `Example:` nesting
(Gherkin v6+): examples sit under the rule they illustrate, so the declaration is structure
rather than a tag line a validator parses. Working that through against the file exposed two
things, both now recorded in § Design inputs (input 4 re-stated; inputs 8 and 9 added):
the practice is codebook craft that belongs with the `agent-runner` skill's codebook
conventions, not this revision; and it would **not** have caught the smuggled citation duty,
since R2/R5/R6 were legitimately in the rules list. The current E1–E5 were authored with
the rules on 2026-09-03, before any calibration, and all five reason about an excerpt, so
they go with the R2/R5/R6 cut in the landing step; anchors that replace them are harvested
from calibration disagreements, nested under their rule, citing the ruling. The
nesting-versus-tag-line choice is Brian's and is made in the `agent-runner` skill when the
first anchor exists, not here.

### 5. A duplicated duty: skill frontmatter validation

`claude plugin validate .claude/skills` checks every `SKILL.md`'s frontmatter against
Anthropic's published spec. The validator's line-budget and description-length rules cite
the same guidance. That is a duty held by two instruments — the kind of accretion the map
exists to surface. **Ruled 2026-09-04 (Brian): the integration test runs both.** The
real-folder test in `tests/StoryPlanner.Tests/ProcessMap/` invokes `claude plugin validate
.claude/skills` alongside `validate`, so a frontmatter defect fails `dotnet test` the same
way a row defect does; the in-house line-budget and description-length rules stay, since
they are Anthropic's guidance rather than its spec and the CLI does not check them. Lands
with the router swap, when that test is un-skipped.

## Sequence and session split

*This session (context-dependent judgment):* this handoff; the **table format** (columns for
roots, files, processes, edges; `kind` values; the generated-section marker convention); the
**root registry** and the transfer of draft 1's rows into the new format with root purposes
assigned — authored, unvalidated, marked as such, in `.claude/skills/v3-buildout/process-map.md`.

*Fresh sessions (mechanical, or needing eyes outside this author's framings):*
1. `tools/StoryPlanner.ProcessMap` with pure tests, built from the written format spec.
2. Generation; set comparison against `32b6d4b`; the validator's mechanical gap findings.
3. **Gap review with Brian** on the skill rows — G1–G24 re-homed plus the new findings; his
   rulings row by row. The revision-1 guard applies: reviewed by eyes not inside the author's
   framings.
4. `methodology-revision-2.md`; the router swap in one commit; the text fixes of ruling 6;
   `process-map-1-draft.md` stamped as the audit that prompted the revision.
5. Only then WU2.15 resumes, at its Session A.

## Building the validator — instructions for the fresh session

> **Done 2026-09-04.** `tools/StoryPlanner.ProcessMap` ships with the three verbs and
> `tests/StoryPlanner.Tests/ProcessMap/`; the integration test over the real folder is skipped
> with its reason until the gap review lands. `validate` on the real skill folder reports 143
> failures, which is the expected first run: the rows are a draft and rulings 7–13 are new. The
> section below is kept as written, with rulings 7–13 marked where they replaced a line.

**Read first, in full:** this handoff; `.claude/skills/v3-buildout/process-map.md` § Format
(the schema the tool implements — build from it, not from memory); the `testing` skill;
`tools/StoryPlanner.AgentRunner/UnitSplitter.cs` (the markdown unit rule: a table is one
unit per body row); `tools/StoryPlanner.CodeSessions/*.csproj` and `Program.cs` as the tool
template (top-level `Program.cs` untestable; logic in namespaced public types).

**Project:** `tools/StoryPlanner.ProcessMap`, `net10.0` console, `RootNamespace
StoryPlanner.ProcessMap`, not in the solution file (same as the other tools). Add a
`ProjectReference` from `tests/StoryPlanner.Tests` (pattern: the existing tool references in
its csproj). Reuse the runner's `UnitSplitter` by `ProjectReference` to
`StoryPlanner.AgentRunner` if the reference is clean; otherwise a table parser of its own,
pinned by the same kind of test as `UnitSplitterTests`. Decide which in the session and say
so in the tool's README header comment.

**Verbs** (`ProcessMap.exe <verb> <skill-folder>`):

- `validate` — parse the four tables (Roots, Files, Processes, Edges) from `process-map.md`
  and the level-1 table from `SKILL.md` once it exists; report every failure with the row id
  and the rule; exit 1 on any failure, 0 otherwise. Rules, all from `process-map.md` § Format:
  unique ids across tables; every reference resolves (inputs, outputs, roots, edge endpoints);
  every process has ≥ 1 root and exactly one actor from the closed set, and ≥ 1 input and ≥ 1
  output (ruling 10); every `kind`, `level`, `keep`, `state`, edge `kind` from its closed set;
  every output file has a consumer (a process whose inputs include it) — ruling 11 removed this
  line's original terminal-record exemption (`keep` committed and named by some `governed-by`),
  since a file read only by a person is read by a process the map is missing a row for; every
  root cited by ≥ 1 process; every `governed-by` resolves to a file and every `Roots.source`
  locus resolves — rulings 7–9 replaced this line's original instruction to resolve
  `file § section` against the skill folder, `fanout/` and `docs/v3-framework/`: paths are
  repo-relative in one form only, and `fanout/` is in this repo rather than beside it; no path
  over edges plus derived data-flow edges from `f.cand` to `f.hyp`, or from anything to
  `f.storyplan`, without passing a `brian` actor before the write (ruling 12);
  `SKILL.md` line count within the budget — **500 lines, Anthropic's published guidance**
  ("Keep SKILL.md under 500 lines. Move detailed reference material to separate files",
  https://code.claude.com/docs/en/skills.md; same figure in the platform best-practices page;
  no token or word figure is published; Brian ruled 2026-09-04 to use the published figure);
  the frontmatter `description` ≤ 1,024 characters (same source); **every companion file in
  the skill folder is linked directly from `SKILL.md`** and no companion is reached only
  through another companion (the published one-level-deep rule: "Keep references one level
  deep from SKILL.md … Claude may partially read files when they're referenced from other
  referenced files") — `process-map.md` currently fails this and the router swap fixes it;
  every worked example in `fanout/referee/codebook.md` (headings `E<n>`)
  carries an "exercises R…" line naming rules that exist in that file — **superseded
  2026-09-04 (late) by design input 4 as re-stated**: the check becomes "every example sits
  under a rule heading and cites a calibration record that exists", vacuous while there are
  none, and the tool's current implementation of the old line is replaced at landing;
  bootstrap rows appear
  in the "Bootstrap rows and what retires them" table.
- `render` — refuses unless `validate` passes; writes the generated sections between the
  markers in `process-map.md`, replacing only what lies between them: `level-1` (P rows and
  their edges), `level-2` (one diagram per WU type: E, V, S, I rows, plus the R/F/M rows
  referenced as single collapsed nodes), `level-3` (R, F, M diagrams), `consumers` (a table:
  file id → producing processes → consuming processes), `validation` (the report as a table,
  so the map carries its own last verdict). Mermaid conventions from draft 1: `classDef` per
  actor kind, `[/…/]` for files, `{…}` for choice nodes, `((∥))` for forks, `-.->` for
  optional, edge labels from the edges table. Deterministic output: same rows → identical text.
- `nodes <file.md>` — print the node ids and edge pairs found in every mermaid block of any
  markdown file, one per line, sorted. Used for the set comparison against draft 1
  (`git show 32b6d4b:docs/v3-framework/process-map-1-draft.md > <scratch>` then `nodes` on
  both and `comm`). Draft 1 used display ids like `P1` for `P.1`; normalise dots away before
  comparing and report the mapping.

**Tests** (`tests/StoryPlanner.Tests/ProcessMap/`, pure tier, small inline markdown fixtures,
never the real skill): each validation rule with one passing and one failing fixture; the
Brian-path rule with a path that passes through a brian node and one that does not;
consumers derived correctly, including a file consumed by no process; render is
deterministic and replaces only between markers; `nodes` extracts ids and pairs from a
fixture with two diagrams. One integration-style test runs `validate` over the real
`.claude/skills/v3-buildout/` and **is expected to fail until the gap review lands** — mark
it skipped with the reason and a date, and un-skip it in the router-swap commit; from then on
`dotnet test` pins the method's topology the way `PlanIntegrity` pins the data's.

**Acceptance:** `dotnet test tests/StoryPlanner.Tests --filter "FullyQualifiedName~ProcessMap"`
green; `validate` on the real folder prints the current gap list (it will be non-empty);
`render` on a passing fixture produces the five sections; `nodes` on draft 1 lists the same
node count as a hand count of one of its diagrams. Do not run `render` on the real
`process-map.md` until `validate` passes there; the empty generated sections stay empty.

**Does not:** edit any row of `process-map.md` or any prose companion (that is the gap
review's work); add a consumers column; swap the router; touch `fanout/`.

## Generate and compare — instructions (same session as the build, or the next)

1. `validate` over the real skill folder; save its report to the scratchpad. Expect failures;
   they are the mechanical gap list and go into the review file below, not into the rows.
2. Until `validate` passes, `render` refuses the real file. To see the diagrams anyway, run
   `render` on a scratchpad copy with a `--force` flag that the tool exposes for this purpose
   only (marks the output "UNVALIDATED" in every generated section). A copy outside the repo has
   no repository root above it, so pass `--repo <repo root>` as well or every path resolves to
   nothing. Publish that copy to the
   existing artifact URL (https://claude.ai/code/artifact/0a5fee6b-7be7-4172-8544-7239a9d97983)
   so Brian reviews one link; the artifact is a review surface, never the record.
3. Set comparison against draft 1: `git show 32b6d4b:docs/v3-framework/process-map-1-draft.md`
   to the scratchpad; `nodes` on both; `comm -3`. Each difference gets one line in the review
   file: *omission* (in draft 1, not in the rows — add a row or say why not), *addition*
   (in the rows, not in draft 1 — name the reason), *rename* (same thing, new id).
4. One visual look at the artifact, spent on layout and labels only. No second look.
5. Write `docs/v3-framework/process-map-review-<date>.md`: the validator report, the
   set-comparison lines, G1–G24 each mapped to a row id, and every root assignment listed as
   "author's assignment, unreviewed". This file is what Brian reviews. It is write-once per
   review round; a second round is a second dated file.

## The gap review with Brian — instructions (his session; fresh eyes)

The reviewer session must not be the session that authored the rows or built the tool
(the revision-1 guard: reviewed by eyes not inside the author's framings). It loads only:
this handoff, `process-map.md`, the review file, and the prose companions as needed.

Per row, in id order, one of: **stands** · **re-rooted** (new root ids, with Brian's reason)
· **split** (two actors were hiding in one row) · **cut** (no root survives and no consumer
needs it — the only place a cut is decided) · **contradiction ruled** (which text wins; the
losing text is listed for the landing step). Per gap G1–G24 and per validator failure: the
ruling in one line. Rulings are appended to this handoff's § Rulings as they are made, in
the session, because AskUserQuestion answers do not reach the archive. Four questions per
call, batched. The review ends when every row and every gap has a line; it may take more
than one session, and each session appends.

Brian's decisions here that no one else makes: which roots are real (a root he does not
recognise as his is deleted, not argued for); the `SKILL.md` line budget; whether a
bootstrap row is retired or kept; any cut.

## Landing the revision — instructions (fresh session after the review)

1. Apply every ruling as a **row edit and a prose edit together**: the row in
   `process-map.md`, the sentence in the governing companion. Never one without the other.
   The referee ruling's four text fixes are in this list (`fanout/referee/codebook.md` two
   inputs stated by reference to the F.4 row and its job file rather than enumerated
   (design input 9), R2 recast, R5/R6 removed, E1–E5 removed as pre-calibration
   illustrations that reason about an excerpt (design input 4 as re-stated);
   `evidence-pipeline.md` § The referee and § Promotion; `agent-runner` SKILL.md's excerpt
   example; `fanout/PROTOCOL.md` per-type applicability and the nested referee run).
2. `validate` must pass on the real folder; un-skip the integration test, and extend it to
   run `claude plugin validate .claude/skills` as well (record 5 of § Build-vs-buy check).
3. `render` the real file; republish the artifact from it.
4. Swap the router: replace `SKILL.md` § Session routing with the level-1 process table
   (generated by `render` into a marked section of `SKILL.md`), within the line budget.
   Update § Provenance to name `process-map.md`.
5. Write `docs/v3-framework/methodology-revision-2.md` (write-once): what prompted it (the
   map's audit), the principle, the rulings, the changed process ids from the table diff
   (`git diff` of the Processes table, ids only), what was deliberately not adopted, what is
   owed. Stamp `process-map-1-draft.md`'s header: "the audit that prompted revision 2;
   superseded by `.claude/skills/v3-buildout/process-map.md`". Stamp the WU2.15 handoff and
   card for the referee change.
6. **One commit** for the swap, the note, the fixes and the stamps; Brian reviews the diff.
7. Three-places rule: if any data semantics changed for the MCP server (none expected), the
   `storyplan-data` skill, CLAUDE.md and `ServerInfo.Instructions` update together.
8. Memory: point the v3 memory at `process-map.md` as the router; retire the "revision 2
   pending" line.

## WU2.15 resumes — pointer

Its plan is `WU2.15-plan.md` § Step 3, Session A, under the landed revision: the
calibration sample uses the held-out split (rulings from ~14, agreement on ~6) and the
record is per (codebook hash, model). Its step 2 is void: the fixes landed with the
revision.

## Must not

Rewrite a rule while moving it; hand-edit a generated section; author a consumers column;
keep two copies of the map; treat any row as settled.
