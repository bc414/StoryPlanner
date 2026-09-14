# directions-schema

`docs/v3-framework/studies/<study>/directions-N.md`, the class `directions`, succeeded by
number; the pipeline directions, the referee's in `docs/v3-framework/pipeline/referee/` and
claiming's in `docs/v3-framework/pipeline/claiming/`, are the same class reached by
reference, held to this shape whenever a definition names them and at any write the checker
traces to one. The shape below is what the hook holds; the example is a conforming file a
writer fills in and the block the checker's tests read as their fixture; then how directions
are queried, and the check ids the hook reports.

## Shape

The file has two parts. The frontmatter is record and never enters a call. The body,
everything after the closing `---`, is the system prompt of every call of a batch that
names this file, verbatim, and the SHA-256 of the body's bytes is the version every call
cites. A change to the frontmatter is a new file with the same body hash and the same
calibration; a change to the body is a new hash and, for a verification, the referee or
claiming, a new calibration before any execution. The file has no title line; the body
begins at its first section. The kind is verification, exploration, referee or claiming,
read from the study id's prefix, `verification-of-` or `exploration-of-`, or from the
`pipeline/referee/` or `pipeline/claiming/` folder; the referee's directions have the
verification kind's sections.

| section | present | holds |
|---|---|---|
| frontmatter, between `---` lines | required | fields |
| `## What you are given` | required | prose: what one item is and where it stops, as the itemizer cut it or the collator collated it; where the item carries data from a corpus the itemizer utilized, what that data shows and what it cannot, the call reading nothing else that could tell it |
| `## How to read` | optional: an exploration's always, never otherwise | prose: how to read the item with the questions in view |
| `## Classes` | optional: a verification's and the referee's always, never an exploration's or claiming's | entries, one line per class |
| `## Criteria` | optional: every kind's but an exploration's always, never an exploration's | entries, numbered from 1, each a rule stated generally that decides an item between classes, or for claiming whether the item's finding bears on a hypothesis; a calibration ruling enters here as a rule, and the item it came from stays in the calibration file |
| `## What to produce` | required | fields: the answer's fields, one line each; claiming's one `list of line` field |
| `## Never` | optional | prose: what the agent never does |

**Frontmatter**:

| key | present | type | value |
|---|---|---|---|
| `questions` | optional: a verification's or an exploration's always, never the referee's or claiming's | list of token of question-list | the question entries the version freezes or reads with |

**Classes**, one entry per line, `- <label>: <what an item shows>`, the label a slug unique
in the section; one class reserved for an item the criteria cannot place.

**What to produce**, one keyed line per field of the answer:

| key | present | type | value |
|---|---|---|---|
| `<field>` | required, one or more | line | the field's kind, `enum`, `line`, `block` or `list of line`, then a comma and, for `enum`, nothing, the values being the Classes; for the others, what the model is to put there; for `list of line`, the form of one line |

A field's name is one lowercase word of at most 64 characters, `[a-z][a-z0-9]*`: the
runner passes it as a property key of the JSON Schema, which the API accepts with no space,
and the result file carries it as a keyed line's key, which takes no hyphen. An `enum` field
exists only when Classes does. The runner derives the JSON Schema the
CLI enforces from these lines and the Classes; it is written nowhere. No section
restates the study, the model, the item's id or the output path; the runner supplies
none of those to the agent.

## Example

```markdown
---
questions: questions/scene-notes-carry-designed-stasis questions/links-name-the-gap
---

## What you are given

<one item: what it is and where it stops, as the itemizer cut it>

## Classes

- designed-stasis: <what an item shows>
- incidental-stasis: <what an item shows>
- no-stasis: <what an item shows>
- cannot-place: the criteria do not decide it

## Criteria

1. <a rule that decides an item between two classes, stated generally>
2. <another>

## What to produce

- class: enum
- basis: line, the number of the criterion that decided it, or "definition" if none was needed

## Never

<what the agent never does>
```

## Queries

| question | how |
|---|---|
| the versions of a study's directions, in order | `ls studies/<study>/directions-*.md` |
| the versions of the pipeline directions, in order | `ls pipeline/referee/directions-*.md pipeline/claiming/directions-*.md` |
| which questions a version freezes or reads with | `grep -n '^questions:' studies/<study>/directions-N.md` |
| a version's hash, the one every call cites | `dry-run-batch` on any definition naming it, or the hash in that batch's `calls.md` |
| whether a version is calibrated | the `calibration` line of any definition naming it, or a `calibration-<date>.md` beside it titled with its hash and accepted; state.md per study, and § Pipeline directions for the referee's and claiming's |
| which batches ran under a version | `grep -rln 'directions-N.md' */*/batches/*/definition.md` |
| the classes a version labels with | its `## Classes` lines |
| the fields a version's results carry | its `## What to produce` lines, which are the keys of every result file of its batches |

## Checks

| check | fails when |
|---|---|
| `directions.frontmatter` | the frontmatter is missing or malformed; `questions` is present for the referee or claiming, absent for a verification or an exploration, or holds a token that is not `questions/<slug>` or names no question entry |
| `directions.sections` | the body's `##` sections are not exactly those the kind has, in order, an optional one absent aside |
| `directions.classes` | a Classes line is not `- <label>: <text>`, a label repeats, or no class is reserved for an item the criteria cannot place |
| `directions.criteria` | a Criteria line, of any kind, is not numbered in sequence from 1 |
| `directions.output` | a What to produce line is not `- <field>: <type>[, <text>]`, a field's name is not one lowercase word of at most 64 characters, a field repeats, a type is not one of the four, an `enum` field exists with no Classes section, a free field has no text, a `list of line` field states no form, or claiming's declares other than exactly one field, a `list of line` |
| `directions.version` | the file's `N` is not the next number in its folder |
