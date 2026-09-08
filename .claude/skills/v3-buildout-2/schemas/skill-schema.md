# skill-schema

`.claude/skills/v3-buildout/` (`v3-buildout-2` until the router swap), the class `skill`:
the router with its two tables, the activity files and the schema files. The shape below is
what the validator holds and what a session changing this folder follows; the example is a
conforming folder; the queries are a writer's; the checks are the validator's check ids. A
session running an activity reads SKILL.md § Reading the tables instead.

## Shape

The folder holds `SKILL.md`, one `<activity id>.md` per activity but the terminus, one
`schemas/<name>-schema.md` per schema the Artifacts table links to, `CORPORA.md`, and the
generated `map.md` and `state.md`. Every other file in the folder root is named by
SKILL.md, so references are one level deep. `SKILL.md` has frontmatter with `name` and a
`description` of at most 1024 characters, is at most 500 lines, and holds the Activities
and Artifacts tables.

**Tables.** Header and separator are structure, every body row is one unit, cells never
contain `|`, ids are lowercase `[a-z0-9-]+` and unique across all three tables, lists are
space-separated ids. A table is identified by its column signature, never by position or
heading; a table in these files with any other signature, or a second copy of one, is a
refusal.

**Activities**, in SKILL.md: `id · enables · description`. An activity is something Brian
does, a gerund with its object. `enables` lists activity ids; the graph is acyclic; exactly
one activity enables nothing and owns no processes, the terminus. Every other `enables`
edge is backed by data flow: something a process of the enabling activity writes is read
by a process of the enabled one. The activity's file is `<id>.md` in the folder.

**Processes**, the first table of each activity file: `id · mode · instruments · reads ·
writes · state · description`. `mode` is exactly one of `hitl · session · agent`, by
decision: `hitl` if a decision that is Brian's is made during the process, `session` if he
only starts it and reads what it produced, `agent` if it runs under an inlined instrument
with no repo context. A process splits only at a change of mode or when it invokes the
runner; steps by the same session in the same activity are one process. `instruments` are
the programs the process invokes: the artifact id where the program's code is in the
Artifacts table, otherwise a free name, which is reported as information; an artifact named
as an instrument counts as read. `reads` and `writes` are artifact ids, at least one each,
and an `hitl` process writes the artifact that records the decision made in it. `state` is
`built`, executed at least once under the current text, or `specified`, only written down:
the development state of the process type, never the state of a run. `description` is
never empty.

**Artifacts**, in SKILL.md: `id · path · mutation · schema · description`. `path` is one
repo-relative pattern with placeholders in angle brackets, or `outside the repo`; never
prose, never an alternative. `mutation` is one of `in-place · succeeded · append · frozen`;
a `frozen` artifact that is not a series (`N` or `<date>` in its path) is never both read
and written by one process. `schema` is empty or a link
`[<name>-schema](schemas/<name>-schema.md)` whose text is a slug ending in `-schema` that
is not an artifact id, whose target is the file the text names, which exists and is titled
`# <name>-schema`. Every artifact is read by some process; one written and read by nothing
is a missing row. Every path from `candidates` to a hypothesis write passes an `hitl`
process, and every writer of `question-list` is `hitl`.

**An activity file** is the title `# <id>`, one line naming what it enables, the Processes
table, `## Preconditions` (the state each input must be in, never a list of inputs), one
`## <process id>` section per process in table order, and `## Never`, activity-specific
only. It carries procedure only: nothing in it restates a table, an artifact's mutation, a
rule, or a word SKILL.md or a schema file defines; files are types, and no corpus or study
appears in one. A session reads it whole at the start of the activity; an `hitl` section
says what the session prepares and presents, how it batches Brian's questions, and what it
writes as each decision lands, and cannot script the middle; an `agent` section names the
instrument and what the generator materialises, and the agent never sees the file; a
runner section names the run's reads and writes and defers to the `agent-runner` skill.

**A schema file** is its title `# <name>-schema`, one sentence saying what follows, then
Shape, Example, Queries and Checks in that order. Its Shape declares five things: the
sections that partition the class's file, in order; the fixed fields, their order, and for
each machine-read one its exact grammar; each entry array's line form and its
continuation; each reference a line may carry, its form, what it resolves to and which
checker resolves it; or, for a class with no machine-read line, that the class is prose. A
Shape declares and never restates its row's path or mutation. The Example is a conforming
file with placeholders whose first fenced block is the checker's fixture; Queries is one
grep per question a reader asks of the class; Checks lists each check id and when it
fails.

**Derived, never authored:** order and data flow, consumers of each artifact, each
activity's inputs, outputs and instruments, and the backing of every `enables` edge. There
is no edges table. `map.md` and `state.md` are written whole by the tool and never by
hand, and no generated block sits inside an authored file.

**Decision ids** `d-YYYY-MM-DD-n` appear in `revising-the-method.md` and nowhere else in
the folder; a fenced example in a schema file is the shape, not a citation.

## Example

A conforming folder, the one the validator's tests build: SKILL.md, then one activity
file. The validator's tests build this fixture in code today; whether they read it from
here is not decided.

```markdown
---
name: example
description: An example skill for the process map tests.
---

# Example skill

## Router

| id | enables | description |
|---|---|---|
| changing-the-planner-for-v3 | | The terminus: out of scope, owns no processes |
| promoting-checked-candidates | changing-the-planner-for-v3 | Brian decides the referee-checked candidates |
| refereeing-a-candidate | promoting-checked-candidates | A blind agent classifies each candidate |

## Artifacts

| id | path | mutation | schema | description |
|---|---|---|---|---|
| hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | The evidence relationship |
| hypothesis-status | docs/v3-framework/hypotheses/NNN-slug.md frontmatter | in-place | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | Status and baselined |
| question-list | docs/v3-framework/questions/<corpus>.md | append | [question-entry-schema](schemas/question-entry-schema.md) | Brian's open questions |
| studies | docs/v3-framework/studies.md | append | [study-registry-schema](schemas/study-registry-schema.md) | One row per study |
| candidates | docs/v3-framework/<study>/candidates.md | append | [candidate-schema](schemas/candidate-schema.md) | One verification's findings |
| codebook | fanout/<study>/codebook-N.md | succeeded | [codebook-schema](schemas/codebook-schema.md) | The frozen instrument |
| calibration | fanout/<study>/calibration-<date>.md | frozen | | One version's agreement |
| verification-artifact | docs/v3-framework/<study>/verification.md | append | | One verification's method and counts |
| items | fanout/<study>/<run>/items/ | frozen | | The units one run judges |
| results | fanout/<study>/<run>/results/ | frozen | | The agents' outputs |

## Companions

schemas/ holds one file per schema the Artifacts table links to; map.md and state.md are generated only.
```

```markdown
# refereeing-a-candidate

Enables promoting-checked-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| referee-run | session | runner | studies calibration codebook candidates | items | specified | The batch under the host |
| referee-judge | agent | | codebook items | results | specified | Writes the falsifier blind |
| referee-append | session | | results candidates | candidates | specified | Copies each verdict under its candidate |

## Preconditions

The codebook is calibrated.

## referee-run

Runs the batch.

## referee-judge

Judges one item.

## referee-append

Appends the lines.

## Never

Gives the referee a source.
```

## Queries

| question | how |
|---|---|
| every activity and what it enables | the Router table in SKILL.md |
| every process of an activity, with its mode, reads and writes | the first table of `<activity>.md` |
| which processes read or write an artifact | `grep -n '<id>' *.md` over the activity files, or the consumers table in `map.md` |
| which processes are Brian's | `grep -n '| hitl |' *.md` |
| where a class's files live and how they may change | the `path` and `mutation` cells of its Artifacts row |
| each activity's derived inputs, outputs, instruments and diagram | `map.md`, generated |
| the folder's verdict | `process-docs/StoryPlanner.DocIntegrity/publish/StoryPlanner.DocIntegrity.exe check .claude/skills/v3-buildout-2`, or the hook at every write |

## Checks

| check | fails when |
|---|---|
| `skill.missing`, `artifacts.missing` | SKILL.md is absent, or holds no Artifacts table |
| `table.unknown-signature` | a table in SKILL.md or an activity file has a signature that is none of the three, or is a second copy of one |
| `id.charset`, `id.duplicate` | an id is not lowercase `[a-z0-9-]+`, or is used twice across the tables |
| `ref.enables`, `ref.reads`, `ref.writes` | an `enables`, `reads` or `writes` cell names an id no table declares |
| `ref.companion` | an activity but the terminus has no `<id>.md` |
| `ref.schema` | a schema cell is not a link, its text is not a slug ending in `-schema`, is an artifact id, points at a file other than the one its text names, or names no file |
| `schema.shape` | a schema file's title is not `# <its id>` |
| `row.mode-count`, `enum.mode`, `enum.state`, `enum.mutation` | a mode cell holds other than one value, or a mode, state or mutation is outside its set |
| `row.reads-empty`, `row.writes-empty` | a process reads nothing (instruments that are artifacts count) or writes nothing |
| `row.hitl-writes-nothing` | an hitl process writes nothing |
| `row.description-empty` | a process has no description |
| `artifact.path-syntax` | a path cell is not one pattern |
| `artifact.never-read` | no process reads the artifact, or it is written and read by nothing |
| `enables.cycle`, `enables.terminus-count`, `enables.terminus-owns-processes` | the graph has a cycle, other than one terminus, or a terminus with processes |
| `enables.unbacked` | an edge not into the terminus has no data flow behind it |
| `gate.ungated` | a path from `candidates` to a hypothesis write passes no hitl process |
| `question-list.writer-not-hitl` | a process that is not hitl writes `question-list` |
| `mutation.read-and-write` | one process reads and writes a frozen artifact that is not a series |
| `file.orphan-activity` | a file in the folder root is neither SKILL.md, map.md, state.md, CORPORA.md nor an activity the Router names |
| `file.orphan-schema` | a file under `schemas/` that no Artifacts row links to |
| `file.shape` | an activity file's title is not its id, or its sections are not Preconditions, one per process in table order, Never |
| `decision.id-outside-revising` | a decision id appears, unfenced, in any folder file but revising-the-method.md |
| `skill.line-budget`, `skill.description-missing`, `skill.description-length` | SKILL.md is over 500 lines, or its description is missing or over 1024 characters |
| `skill.companion-unlinked` | a file in the folder root is not named by SKILL.md |
| `enables.vacuous`, `gate.vacuous`, `question-list.vacuous` | reported, never failed: a check whose subject set is empty, such as an edge into the terminus |
| `info.artifact.never-written`, `info.instrument.free-name`, `info.unused-enum-value`, `info.generated.inline-block` | reported, never failed: an artifact no process writes, an instrument that is a free name, a declared value no row uses, a generated block still inside an authored file |
