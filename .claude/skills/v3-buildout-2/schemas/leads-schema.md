# leads-schema

`docs/v3-framework/studies/<study>/leads.md`, at the top of an exploration study, the class
`leads`. Written by exploring-a-corpus's `write-leads`, which consolidates the batch's results
so that Brian reads leads rather than every result; reviewing-leads appends what its readings
of the source produce: a reread line beneath a lead it checked, and a shortcoming where a
reading showed the study's own instrument at fault. Nothing else writes here. A lead is an
idea for a question and for what to itemize, never a finding and never evidence; it is cited
as a lead only, by any later session, until a verification of its corpus has run. How the
exploration ran, which questions it read with and what it did not read are not in this file:
the definition, the directions, the index head, the calls and the tally hold them, and the
review brings them up from there.

## Shape

The title is `# <study> — leads`, where `<study>` is the id of the study whose folder holds
the file, an exploration's id in the registry.

| section | present | holds |
|---|---|---|
| `## Leads` | required | entries; may hold none |
| `## Proposed questions` | optional | entries, one line each; what the leads raise that no question asks |
| `## Shortcomings` | optional | entries, one line each; what the review found wrong with the study's own instrument |

**Leads**: an entry is `### <study>/<slug>`, where `<study>` repeats the id of the study whose
folder holds the file, so that the heading is the token every other file cites, and `<slug>`
is a slug unique in the file, naming what was seen and never what it means. Then keyed lines
in this order:

| key | present | type | value |
|---|---|---|---|
| `lead` | required | block | what was seen, in words, as a neutral statement; nothing about what it means for any hypothesis |
| `seen in` | required | line | coarsely and in words, whatever the lead was seen in, at the grain the reader had, for instance a story, a subject or a stretch of the corpus; never a position inside a slice |
| `cites` | required | list of line | the item token `<study>/<batch>/<item>` of each slice whose result the lead came from, one per line, at least one; for an exploration of the whole corpus, its one item |

An appended line, `- reread: <date> <what the source showed>`, written beneath the fields by
reviewing-leads each time the review checks the lead at the source, zero or more per entry,
in date order, after every field and followed by no keyed field line; what the source showed
includes whatever part of the lead holds. An entry is never edited, and there is no other
form for a lead that holds in part.

No order of leads is held by a check.

**Proposed questions**: `- <what the leads raise that no question asks>`, one per line; none
is a question until Brian raises it into the corpus's list, and none is a lead.

**Shortcomings**: `- <part>: <what the review found>`, `<part>` one of `slice`, `itemizer`,
`directions`, `consolidation`, `execution`, `corpus`. `slice` is the choice made at the plan
of what one slice is, not suiting the reading, such as a slice that joined two things the
reader needed apart, or a whole corpus read as one item where the questions wanted it cut;
`itemizer` is the tool not cutting what its stated grain, narrowing or locators say, such as
a dropped, repeated or truncated stretch; `directions` is the text wanting, where a reader
that followed it faithfully still went wrong, such as a reading the directions were silent
on; `consolidation` is the consolidation not saying what the cited results say; `execution`
is a reader not doing what clear directions asked, such as writing about a hypothesis against
the directions' Never; `corpus` is the corpus not being what CORPORA.md says. The examples
lead and do not exhaust. Each is a fact about the study's own instrument, never about the
corpus's content, and is written only by reviewing-leads.

## Example

```markdown
# exploration-of-v1-archive — leads

## Leads

### exploration-of-v1-archive/first-seen-thing
- lead: <what was seen, in words>
- seen in: <whatever it was seen in, in words>
- cites:
  - exploration-of-v1-archive/01-scene-slices/slice-01
  - exploration-of-v1-archive/01-scene-slices/slice-02
- reread: 2026-09-22 <what the source showed>

### exploration-of-v1-archive/second-seen-thing
- lead: <what was seen, in words, continuing
  on an indented line where it runs long>
- seen in: <whatever it was seen in, in words>
- cites:
  - exploration-of-v1-archive/01-scene-slices/slice-02

## Proposed questions

- <what the leads raise that no question asks>

## Shortcomings

- consolidation: <what the review found>
```

## Queries

| question | how |
|---|---|
| every lead of an exploration, in order | `grep -n '^### ' studies/<study>/leads.md` |
| the leads that came from one slice | `grep -n '^  - <study>/<batch>/<item>$' studies/<study>/leads.md`, then the `### ` heading above each |
| what the review found at the source for one lead | `grep -n -A20 '^### <study>/<slug>' studies/<study>/leads.md`, its `- reread:` lines |
| every reread of an exploration's leads, and when | `grep -n '^- reread:' studies/<study>/leads.md` |
| what the review found wrong with the instrument | the lines under `## Shortcomings`, the part as the first word |
| the questions the leads raise | the lines under `## Proposed questions` |
| how the exploration ran and what it read with | not here: the batch's definition, the directions' frontmatter and body, the index head, `calls.md` and `tally.md` |

## Checks

| check | fails when |
|---|---|
| `leads.title` | the title is not `# <study> — leads` with the id of the study whose folder holds the file, or that study is not an exploration in the registry |
| `leads.shape` | a section is missing, out of order or holds other than the sections table says |
| `leads.entry` | an entry heading is not `### <study>/<slug>` with the id of the study whose folder holds the file, a slug repeats, a field is missing, unknown, out of order or of the wrong type, or a line is neither keyed nor continuation |
| `leads.cites` | a token names no item in the index of a batch under the study, or the list is empty |
| `leads.reread` | a reread line is not a date then text, sits before the fields, is followed by a keyed field line, or is dated earlier than the reread line before it on the same lead |
| `leads.shortcoming` | a Shortcomings line's first word is not one of the six parts |
