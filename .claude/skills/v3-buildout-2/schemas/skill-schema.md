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
repo-relative pattern with placeholders in angle brackets, or `no single pattern`; never
prose, never an alternative. `mutation` is one of `in-place · succeeded · append · frozen`;
a `frozen` artifact that is not a series (`N` or `<date>` in its path) is never both read
and written by one process. A section of a file carrying its own mutation is its own row,
which is how rule 9's requirement that a multi-section file name each mutation is met; a
Shape therefore never names one. `schema` is empty or a link
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
directions the call runs under and what the item holds, and the agent never sees the
file; a section whose process invokes the runner names the batch's reads and writes and
defers to the `agent-runner` skill.

**A schema file** is its title `# <name>-schema`, one sentence saying what follows, then
Shape, Example, Queries and Checks in that order. A Shape declares and never restates its
row's path or mutation, and it is written in the one grammar below, the last level of
the artifacts: the grammar is held by the engine's tests, and no document governs it. A
title is a class's own to declare and hold; the grammar says nothing of titles.

The grammar. A sections table, columns section, present and holds, partitions the file in
order, holds being `fields`, `entries`, `table`, `prose` or `fenced`; a section is named
by its `## heading` in a code span, or as the frontmatter, the head after the title, the
table after the head, or the whole file after the title. A field table has the columns
key, present, type and value; a table class's column table the same with column for key.
The field tables follow the sections table and are claimed in order, first by the
sections that hold fields, then by those that hold entries; an entries section with no
table holds one-line entries, each `- ` or numbered line one entry. An entry array is
declared by its heading's type and its field table; an appended line by its form, its
multiplicity and the process that writes it. A key in angle brackets, `<field>`, is any
slug, one or more. Present
is `required` or `optional`; any rule about when an optional field must or must not appear
is the class's own, named in its Checks. Checks has the columns check and fails when, ids
`<class>.<name>` unique in the file; Queries has question and how; the Example's first
fenced block is the fixture. A new type, present or holds value is a decision adding a
word, never a new level.

The types, each defined by the JSON Schema fragment a Shape compiles to; a reference type
names its target class, and the engine resolves it:

| type | fragment | resolves to |
|---|---|---|
| `slug` | `{"type":"string","pattern":"^[a-z0-9-]+$"}`, unique in its file | |
| `token of <class>` | `{"type":"string","pattern":"^[a-z0-9-]+/[a-z0-9-]+$"}` | an entry of that class |
| `id of <class>` | `{"type":"string","pattern":"^[0-9]{3}$"}` | a file of that class |
| `path to <class>`, or `path` | `{"type":"string"}`, a relative path | a file of that class, or of none |
| `date` | `{"type":"string","pattern":"^[0-9]{4}-[0-9]{2}-[0-9]{2}$"}` | |
| `timestamp` | `{"type":"string","pattern":"^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}$"}` | |
| `hash` | `{"type":"string","pattern":"^[0-9a-f]{64}$"}` | |
| `enum` | `{"enum":[…]}`, the set the Shape names, or for a result field the directions' Classes | |
| `line` | `{"type":"string","pattern":"^[^\n]*$"}` | |
| `block` | `{"type":"string"}` | |
| `list of <type>` | `{"type":"array","items":<the type's fragment>}`; on the line, space-separated, for slug, token, id, date, timestamp, hash and enum; one per continuation line for line and block | |

How the engine holds a Shape. A governed file is one object: keyed lines its properties,
`###` entries an array of objects, a table an array of rows, frontmatter an object,
prose a string, by one fixed mapping, the same the runner uses in the other direction to
render a result from the model's JSON. The engine parses the file to its object,
compiles the Shape to a JSON Schema by substituting the fragments, validates the object
by a standard validator, resolves every reference itself, and applies the class's own
rules, the ones no schema language expresses, which the Checks section names. The Example is a conforming
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
| promoting-refereed-candidates | changing-the-planner-for-v3 | Brian decides the refereed candidates |
| refereeing-candidates | promoting-refereed-candidates | A blind agent classifies each candidate |

## Artifacts

| id | path | mutation | schema | description |
|---|---|---|---|---|
| hypothesis-record | docs/v3-framework/hypotheses/NNN-slug.md § Record | append | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | The evidence relationship |
| hypothesis-origin | docs/v3-framework/hypotheses/NNN-slug.md § Origin | frozen | [hypothesis-file-schema](schemas/hypothesis-file-schema.md) | Why one hypothesis exists |
| question-list | docs/v3-framework/questions/<corpus>.md | append | [question-entry-schema](schemas/question-entry-schema.md) | Brian's open questions |
| studies | docs/v3-framework/studies.md | append | [study-registry-schema](schemas/study-registry-schema.md) | One row per study |
| candidates | docs/v3-framework/studies/<study>/candidates.md | in-place | | A generated view, never hand-edited |
| directions | docs/v3-framework/studies/<study>/directions-N.md | succeeded | [directions-schema](schemas/directions-schema.md) | The system prompt of a batch's calls |
| calibration | docs/v3-framework/studies/<study>/calibration-<date>.md | frozen | | One version's agreement |
| findings | docs/v3-framework/studies/<study>/findings.md | append | | One verification's findings |
| definition | docs/v3-framework/studies/<study>/batches/<batch>/definition.md | frozen | [definition-schema](schemas/definition-schema.md) | What a batch runs under |
| items | docs/v3-framework/studies/<study>/batches/<batch>/items/ | frozen | | The item bodies |
| results | docs/v3-framework/studies/<study>/batches/<batch>/results/ | frozen | | The model's answers as rendered |

## Companions

schemas/ holds one file per schema the Artifacts table links to; map.md and state.md are generated only.
```

```markdown
# refereeing-candidates

Enables promoting-refereed-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| assemble-referee-batch | session | runner | studies calibration directions candidates | definition items | specified | The batch assembled and handed off |
| assess-referee-items | agent | | directions items | results | specified | Writes the falsifier blind |
| append-verdicts | session | | results candidates | candidates | specified | Copies each verdict under its candidate |

## Preconditions

The referee's directions are calibrated.

## assemble-referee-batch

Assembles the batch.

## assess-referee-items

Assesses one item.

## append-verdicts

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
| `schema.fields` | a schema file in the four-section shape has a Shape outside the grammar: no sections table with the columns section, present, holds; a field or column table without the columns key or column, present, type, value; a present value other than required or optional; a holds value outside the five; a type outside the vocabulary; a reference type naming a class no row declares; a field or column table no section claims; or an entries section with no field table whose Example fixture shows `###` entries |
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
| `info.artifact.never-written`, `info.instrument.free-name`, `info.unused-enum-value`, `info.generated.inline-block`, `check.no-row` | reported, never failed: an artifact no process writes, an instrument that is a free name, a declared value no row uses, a generated block still inside an authored file, a class with a checker whose row gives no in-repo file pattern |
