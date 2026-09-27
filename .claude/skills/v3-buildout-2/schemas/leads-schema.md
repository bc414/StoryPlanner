# leads-schema

`docs/v3-framework/studies/<study>/leads-N.md`, at the top of an exploration study, the class
`leads`: one write-up of one batch's results at one count of its items with results, the study's files numbered
from 1 in the order they are written. Written by conducting-an-exploration's `write-leads`, in a
fresh session, which draws leads from the batch's results so that Brian reads leads rather than
every result; reviewing-leads appends what its sittings produce: a reread line beneath a lead it
checked, a shortcoming where a reading showed the study's own instrument at fault, and one next
step per sitting. Nothing else writes here. Every file of a study is appended, an earlier one
included; the study's highest-numbered file is its current write-up, derived from the names and
recorded nowhere, and an earlier file is history. A lead is an idea for a question and for what to
itemize, never a finding and never evidence; it is cited as a lead only, by any later session. A
lead points back at what it was drawn from in one or both of two ways: a query, the canonical
string a results tool printed for one question put to the batch's results, which the review
re-runs to read the table the session read; and cites, the item tokens of the slices whose results
it came from. A study need have no query tool; its leads then carry cites alone. How the
exploration ran, which questions it read with and what it did not read are not in this file: the
definition, the directions, the index head, the calls and the tally hold them, and the review
brings them up from there.

## Shape

The title is `# <study> — leads`, where `<study>` is the id of the study whose folder holds
the file, an exploration's id in the registry. The file is `leads-<N>.md`, `<N>` a number from 1
with no leading zero, the next number in its study when the file is written.

| section | present | holds |
|---|---|---|
| the head, after the title | required | fields |
| `## Leads` | required | entries; may hold none |
| `## Proposed questions` | optional | entries, one line each; what the leads raise that no question asks |
| `## Shortcomings` | optional | entries, one line each; what the review found wrong with the study's own instrument |
| `## Next steps` | optional | entries, one line each; what each review sitting ruled the study does next |

**Head**:

| key | present | type | value |
|---|---|---|---|
| `items with results` | required | line | `<n> of <items>`: how many of the batch's items had a result when the file was written, which the batch no longer shows once it moves on, out of its index's items; `<n>` at least 1 and at most `<items>` |
| `written by` | required | block | in words, the session that wrote the file and what it could see; a statement of fact |

**Leads**: an entry is `### <study>/leads-<N>/<slug>`, where `<study>` repeats the id of the
study whose folder holds the file and `<N>` is the file's own number, so that the heading is the
token every other file cites, and `<slug>` is a slug unique in the file, naming what was seen and
never what it means; a later file may reuse a slug for a lead it restates. Then keyed lines in
this order:

| key | present | type | value |
|---|---|---|---|
| `lead` | required | block | what was seen, in words, as a neutral statement; nothing about what it means for any hypothesis |
| `seen in` | required | line | coarsely and in words, whatever the lead was seen in, within the item the reader had, for instance a story, a subject or a stretch of the corpus; never a position inside a slice |
| `query` | optional: at least one of `query` and `cites` | list of line | one canonical query string per line, as the results tool prints it: `rq1 batch=<study>/<batch> answered=<n> field=<key> [where <column>~<regex> ...] [story=<slug>] [item=<id>] [sample=<n> seed=<s>] view=<verb> [...]`, naming a batch under this study and, as `answered=`, its count of items with results that the query was run over; at least one when present |
| `cites` | optional: at least one of `query` and `cites` | list of line | the item token `<study>/<batch>/<item>` of each slice whose result the lead came from, one per line; for an exploration of the whole corpus, its one item; at least one when present |

Every query and every cite in the file names one batch, the one the file was written from; the
file draws from that batch alone.

An appended line, `- reread: <date> <what the source showed>`, written beneath the fields by
reviewing-leads each time the review checks the lead, through its queries re-run and its cited
slices against the corpus, zero or more per entry, in date order, after every field and followed
by no keyed field line; what the source showed includes whatever part of the lead holds, and says
so where the lead's words are not what its re-run query's table or its cited results show. An
entry is never edited, and there is no other form for a lead that holds in part.

No order of leads is held by a check.

**Proposed questions**: `- <what the leads raise that no question asks>`, one per line; none
is a question until Brian raises it into the question list, and none is a lead.

**Shortcomings**: `- <part>: <what the review found>`, naming no token, `<part>` one of `slice`,
`itemizer`, `directions`, `execution`, `corpus`. A part is a standing part of the study's
instrument, which carries its fault into every later batch until it is changed. `slice` is the
choice made at the plan of what one slice is, not suiting the reading, such as a slice that joined
two things the reader needed apart, or a whole corpus read as one item where the questions wanted
it cut. `itemizer` is the itemizer's cut at fault, in one of two cases: the tool does not cut what
its index head says, such as an item dropped, repeated or cut short, or a narrowing or locator the
code does not follow; or the tool cuts exactly what its index head says and what it says is wrong,
such as a cut that takes items a rule applying everywhere excludes, flagged notes among them, or
keeps items the study cannot use. `directions` is the text wanting, where a reader that followed
it faithfully still went wrong, such as a reading the directions were silent on. `execution` is a
reader not doing what clear directions asked, such as writing about a hypothesis against the
directions' Never. `corpus` is the corpus not being what CORPORA.md says. The examples lead and do
not exhaust. Each is a fact about the study's own instrument, never about the corpus's content, and
is written only by reviewing-leads. A lead whose words are not what its query or its cited results
show is no shortcoming: it takes a reread line.

**Next steps**: `- <date> <step>: <why>`, one line per review sitting, the last thing the sitting
writes, in date order, `<step>` one of `continue the batch`, `new directions version`, `change the
itemizer`, `new leads file`, `new study`, `stop`; the why records Brian's ruling and is composed
under rule 10. `new leads file` is a new write-up of results already present. Written only by
reviewing-leads, in the file the sitting reviewed.

## Example

```markdown
# exploration-of-v1-archive — leads

- items with results: 2 of 2
- written by: a session opened for this write-up alone, which did not plan the study, write its
  directions, build its itemizer or read its pilot, and read the batch's results through the results tool

## Leads

### exploration-of-v1-archive/leads-1/first-seen-thing
- lead: <what was seen, in words>
- seen in: <whatever it was seen in, in words>
- query:
  - rq1 batch=exploration-of-v1-archive/01-scene-slices answered=2 field=leads where what~"<a regex>" view=list
- cites:
  - exploration-of-v1-archive/01-scene-slices/slice-01
  - exploration-of-v1-archive/01-scene-slices/slice-02
- reread: 2026-09-22 <what the source showed>

### exploration-of-v1-archive/leads-1/second-seen-thing
- lead: <what was seen, in words, continuing
  on an indented line where it runs long>
- seen in: <whatever it was seen in, in words>
- cites:
  - exploration-of-v1-archive/01-scene-slices/slice-02

## Proposed questions

- <what the leads raise that no question asks>

## Shortcomings

- directions: <what the review found>

## Next steps

- 2026-09-22 new directions version: <why>
```

## Queries

| question | how |
|---|---|
| a study's leads files, and which is current | `ls studies/<study>/leads-*.md`; the highest number is current |
| how many items had results when a file was written, and who wrote it | its head: `grep -n -A3 '^- items with results:' studies/<study>/leads-<N>.md` |
| every lead of a file, in order | `grep -n '^### ' studies/<study>/leads-<N>.md` |
| every write-up that restates one lead | `grep -n '^### <study>/leads-[0-9]*/<slug>$' studies/<study>/leads-*.md` |
| the leads that came from one slice | `grep -n '^  - <study>/<batch>/<item>$' studies/<study>/leads-*.md`, then the `### ` heading above each |
| the queries a lead rests on, to re-run | `grep -n -A20 '^### <study>/leads-<N>/<slug>' studies/<study>/leads-<N>.md`, its `  - rq1 ` lines, each given to the results tool's `run` verb or pasted into its page |
| every lead drawn from one batch by query | `grep -n '^  - rq1 batch=<study>/<batch> ' studies/<study>/leads-*.md`, then the `### ` heading above each |
| what the review found at the source for one lead | `grep -n -A20 '^### <study>/leads-<N>/<slug>' studies/<study>/leads-<N>.md`, its `- reread:` lines |
| every reread of a study's leads, and when | `grep -n '^- reread:' studies/<study>/leads-*.md` |
| what the review found wrong with the instrument | the lines under `## Shortcomings`, the part as the first word |
| what each review sitting ruled next, and why | the lines under `## Next steps`; the study's latest is in `state.md` |
| the questions the leads raise | the lines under `## Proposed questions` |
| how the exploration ran and what it read with | not here: the batch's definition, the directions' frontmatter and body, the index head, `calls.md` and `tally.md` |

## Checks

| check | fails when |
|---|---|
| `leads.title` | the title is not `# <study> — leads` with the id of the study whose folder holds the file, or that study is not an exploration in the registry |
| `leads.version` | the file is not `leads-<N>.md` with `<N>` a number from 1 and no leading zero, or a lower number is missing from the study's leads files |
| `leads.head` | a head key is missing, unknown or out of order, or `items with results` is not `<n> of <items>` with `<n>` at least 1 and at most `<items>` |
| `leads.shape` | a section is missing, out of order or holds other than the sections table says |
| `leads.entry` | an entry heading is not `### <study>/leads-<N>/<slug>` with the id of the study whose folder holds the file and the file's own number, a slug repeats in the file, a field is missing, unknown, out of order or of the wrong type, a line is neither keyed nor continuation, or an entry has neither `query` nor `cites` |
| `leads.batch` | the file's queries and cites name more than one batch |
| `leads.query` | a query line does not parse as a canonical query string, or its batch token names no batch under this study, or the list is present and empty |
| `leads.cites` | a token names no item in the index of a batch under the study, or the list is present and empty |
| `leads.reread` | a reread line is not a date then text, sits before the fields, is followed by a keyed field line, or is dated earlier than the reread line before it on the same lead |
| `leads.shortcoming` | a Shortcomings line's first word is not one of the five parts |
| `leads.next` | a Next steps line is not `- <date> <step>: <why>` with a date, one of the six steps and a why, or is dated earlier than the line before it |
