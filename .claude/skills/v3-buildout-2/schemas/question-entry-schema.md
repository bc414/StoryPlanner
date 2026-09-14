# question-entry-schema

`docs/v3-framework/questions.md`, the buildout's one question list, the class `question-list`.
The shape below is what the hook holds; the example is a conforming list a writer fills in
and the block the checker's tests read as their fixture; then how the list is queried, and
the check ids the hook reports.

## Shape

The title is `# Questions`.

| section | present | holds |
|---|---|---|
| the whole file after the title | required | entries; no head prose, no sections |

An entry's heading is `### questions/<slug>`, type token of question-list, where `<slug>` is
unique in the list, authored with the entry and never changed; a reworded question is a new
entry and the old one withdrawn. The heading is the token every other file cites, so one grep
finds the definition and the uses together. No field names the data sets a question concerns.
Then keyed lines in this order and no other line, a value continuing on lines indented two
spaces:

| key | present | type | value |
|---|---|---|---|
| `date` | required | date | the day Brian asked; never earlier than the entry before it |
| `raised by` | required | block | why the question exists: the occasion and what raised it, and Brian's beliefs, recollections and motivation behind it; what raised it cited by its own token where it has one, as a trail a later session can follow when asked, `recall` where his recall raised it, `carried from the founding pool` for a pool question; under rule 10 |
| `question` | required | block | a neutral assertion of what is asked |
| `suggested test` | optional | block | a naive note on how the question might be tested, procedure that holds whatever the answer and never a belief about it, for the author of the directions to take or leave; never a criterion; under rule 10 |

Appended lines, `- withdrawn: <date> <reason>` and `- reinstated: <date> <reason>`, each a
date then a reason under rule 10, beneath the fields, alternating and starting with
withdrawn: a withdrawn question Brian wants back is reinstated under its own slug, and a
different iteration of it is a new entry. They are written by the hitl process in which
Brian withdraws or reinstates the question.

Before any entry is added, the session reads the list and puts in front of Brian every
withdrawn entry that bears on the question, with its reasons, so that he reinstates it,
writes a new entry, or lets it rest.

An entry is cited everywhere as `questions/<slug>`, one token, its heading. Open is an entry
with no withdrawn line, or whose last withdrawn line is followed by a reinstated line; frozen
and answered are derived by the tool from a directions version's and a verification's citations of the id; none of the three is written. Every writer is an hitl
process, since a question is Brian's, and the class is append: no line is ever edited.

## Example

```markdown
# Questions

### questions/heavy-dt-two-classes

- date: 2026-09-07
- raised by: review of exploration-of-own-fiction, from exploration-of-own-fiction/dt-carries-concealment
- question: <the question>
- suggested test: <a naive note on how it might be tested>

### questions/narrator-register-outside-giyc

- date: 2026-09-08
- raised by: recall, in the verify-plan for verification-of-own-fiction-narrator-register
- question: <the question>
- withdrawn: 2026-09-09 <why>

### questions/letters-mark-subplot-transitions

- date: 2026-09-10
- raised by: recall, at the review of exploration-of-own-fiction
- question: <the question>
- withdrawn: 2026-09-11 <why>
- reinstated: 2026-09-18 <why>
```

## Queries

| question | how |
|---|---|
| every question, in order | `grep -n '^### ' questions.md` |
| the open questions | the entries with no `- withdrawn:` line, or whose last withdrawn line is followed by `- reinstated:`; or state.md § Questions |
| the withdrawn questions and why | `grep -n -B6 '^- withdrawn:' questions.md` |
| one question's definition and every use | `grep -rn 'questions/<slug>' docs/v3-framework .claude/skills`; the `### ` hit is the entry, the rest are where it is in view, frozen or answered |
| what raised a question | its `raised by` line, and any token cited in it |
| whether calibrated directions cover a question | state.md, derived from the directions' frontmatter |

## Checks

| check | fails when |
|---|---|
| `question.title` | the title is not `# Questions` |
| `question.slug` | a heading is not `questions/<slug>` with a lowercase slug, or repeats a slug in the list |
| `question.entry.fields` | a key is missing, unknown or out of order; a line in an entry is neither keyed nor continuation; a line sits outside every entry |
| `question.entry.date` | a date is not `YYYY-MM-DD`, or is earlier than the entry before it |
| `question.withdrawn` | a withdrawn or reinstated line is not a date then text, sits before the fields or before a keyed line, or the two do not alternate starting with withdrawn |
