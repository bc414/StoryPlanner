# decisions-schema

`docs/v3-framework/decisions.md`. The shape below is the schema the hook holds; the example
after it is a conforming file a writer fills in, and the block the checker's tests read as
their fixture; then how the file is queried, and the check ids the hook reports.

## Shape

A singleton class: the title is `# Decisions`.

| section | present | holds |
|---|---|---|
| the head, between the title and the first section | required | prose: what the file is and where the closed founding record is; no entry |
| `## Revision N`, N increasing through the file | required, one or more | prose, at most one paragraph, then entries, and nothing else |

An entry's heading is `### <ruling>`, type line, the ruling in one line; then keyed
lines in this order and no other line:

| key | present | type | value |
|---|---|---|---|
| `id` | required | slug | exactly `d-<date>-<n>`, written with the entry and never changed: `<date>` the entry's own date, `<n>` 1 for the first entry of that date and one more than the previous entry's for each after |
| `date` | required | date | the day Brian decided or approved; never earlier than the entry before it |
| `supersedes` | optional | list of slug | present only when the entry supersedes: ids `d-YYYY-MM-DD-n`, each an entry earlier in decisions.md that no other entry supersedes |
| `raised by` | required | block | what raised the decision, and in whose words |
| `decision` | required | block | what was ruled; what it leaves undecided where a reader might infer otherwise; when superseding, every clause of the older entry that still holds |
| `not taken` | required | block | the options declined and why |

- A keyed line is `- key: value` at the left margin, one space after the colon. A line
  starting with two spaces continues the value above it; a blank line followed by such a
  line continues it too, so a free-text value may hold paragraphs or sub-bullets.
- Nothing derives an id and the tool never writes one; a written id can only disagree
  with the shape by failing, and the failure names the expected id.

What enters an entry:

- One decision. An entry that a later entry could supersede by halves is two entries.
- Brian's typing, verbatim, in quotation marks. A label he selected is written unquoted.
  The session's words are unquoted and are never quoted as his.
- Nothing that happens after approval: no execution status, no owed work. A ruling that
  defers a question says it does not decide it. What the decision resolved, an audit
  unit, a gap, a check of the tool, is a sentence in `decision`.
- Supersession whole. An entry supersedes an older one entirely or not at all; the older
  entry's not-taken list stays as history; a superseded entry is never superseded again.
  An entry that changes what the closed founding record decided names the old id in
  prose and supersedes nothing.

Appended after Brian's approval and never edited; written in `revise` and read there and
nowhere else.

## Example

```markdown
# Decisions

<what the file is, and where the closed founding record is>

## Revision 2

<at most one paragraph>

### <the ruling in one line>

- id: d-2026-09-07-1
- date: 2026-09-07
- raised by: <what raised it; Brian's typing in quotation marks: "…">
- decision: <what was ruled>

  <a second paragraph of the same value, or sub-bullets indented the same way>
- not taken: <an option and why it was declined>; <another>

### <a later ruling that replaces the one above entirely>

- id: d-2026-09-08-1
- date: 2026-09-08
- supersedes: d-2026-09-07-1
- raised by: <what raised it>
- decision: <what was ruled, restating every clause of the older entry that still holds>
- not taken: <the options declined and why>
```

## Queries

| question | how |
|---|---|
| every decision, in order | `grep -n '^### '` |
| the next id for today | `grep -n '^- id: d-2026-09-07' decisions.md \| tail -1`, then one more; none means 1 |
| was this option already declined, and why | `grep -n -A6 '^- not taken:'`, or grep the option's own word |
| what is superseded | `grep -n '^- supersedes:'`; an id in no such line stands |
| what resolved an audit unit, a gap, a check of the tool | grep its id; the answer is a sentence in a `decision` |
| what a revision decided | its section, read whole by `revise`, which writes the revision note from it |

## Checks

| check | fails when |
|---|---|
| `decisions.shape` | the title, a section heading or the head is off shape; a section holds a second paragraph or a non-entry line |
| `decisions.entry.fields` | a key is missing, unknown or out of order; a line in an entry is neither keyed nor continuation |
| `decisions.entry.id` | the id is not the entry's date with the next number of that date; the message names the expected id |
| `decisions.entry.date` | a date is not `YYYY-MM-DD`, or is earlier than the entry before it |
| `decisions.supersedes` | a value that is not ids separated by single spaces, an id that is the entry's own or a later entry's, or a target another entry already supersedes |
