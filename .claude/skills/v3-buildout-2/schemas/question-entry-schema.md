# question-entry-schema

`docs/v3-framework/questions/<corpus>.md`, one list per corpus, the class `question-list`.
The shape below is what the hook holds; the example is a conforming list a writer fills in
and the block the checker's tests read as their fixture; then how a list is queried, and
the check ids the hook reports.

## Shape

The file is `# <corpus> — questions`, `<corpus>` the file's own name and an id in
CORPORA.md, then entries and nothing else: no head prose, no sections.

An entry is `### <corpus>/<slug>`, the corpus the file's own name and the slug lowercase
`[a-z0-9-]+`, unique in its list, authored with the entry and never changed; a reworded
question is a new entry and the old one withdrawn. The heading is the token every other
file cites, so one grep finds the definition and the uses together. Then keyed lines in
this order and no other line, the first two values exact and the rest free, a value
continuing on lines indented two spaces:

| key | present | value |
|---|---|---|
| `date` | always | exactly `YYYY-MM-DD`, one line: the day Brian asked; never earlier than the entry before it |
| `hypotheses` | when there are any | ids `NNN` separated by single spaces, one line, each a hypothesis file; the one authored edge from a corpus's questions to the hypotheses its answers bear on |
| `raised by` | always | free: the occasion and what raised it, the citation as one token, `<study>/<slug>` for a lead, a proposal or a candidate, `recall`, or `carried from the founding pool` |
| `question` | always | free: the question, in Brian's words |
| `suggested test` | when one suggests itself | free: a naive note on how the question might be tested, for the codebook author to take or leave; never a criterion |

Beneath the fields, appended later by the hitl process that withdraws the question, at
most once: `- withdrawn: YYYY-MM-DD <reason>`, the date exact and the reason free.

An entry is cited everywhere as `<corpus>/<slug>`, one token, its heading. Open is an entry with no
withdrawn line; frozen and answered are derived by the tool from a codebook's and a
verification's citations of the id; none of the three is written. Every writer is an hitl
process, since a question is Brian's, and the class is append: no line is ever edited.

## Example

```markdown
# own-fiction — questions

### own-fiction/heavy-dt-two-classes

- date: 2026-09-07
- hypotheses: 031 032
- raised by: review of exploration-of-own-fiction, from exploration-of-own-fiction/dt-carries-concealment
- question: <the question, in Brian's words>
- suggested test: <a naive note on how it might be tested>

### own-fiction/narrator-register-outside-giyc

- date: 2026-09-08
- raised by: recall, in the verify-plan for verification-of-own-fiction-1
- question: <the question, in Brian's words>
- withdrawn: 2026-09-09 <why>
```

## Queries

| question | how |
|---|---|
| every question of a corpus, in order | `grep -n '^### ' questions/<corpus>.md` |
| the open questions | the entries with no `- withdrawn:` line beneath them, or state.md per corpus |
| the questions bearing on hypothesis NNN | `grep -n '^- hypotheses:.*\bNNN\b' questions/*.md` |
| one question's definition and every use, in one list | `grep -rn '<corpus>/<slug>' docs/v3-framework fanout .claude/skills`; the `### ` hit is the entry, the rest are where it is in view, frozen or answered |
| what raised a question | its `raised by` line; the token in it is the lead, proposal or candidate |
| whether a calibrated codebook covers a question | state.md, derived from the codebook's `## Questions` |

## Checks

| check | fails when |
|---|---|
| `question.title` | the title is not `# <corpus> — questions` with the file's own name, or the corpus is not an id in CORPORA.md |
| `question.slug` | a heading is not `<corpus>/<slug>` with the file's own corpus and a lowercase slug, or repeats a slug in the list |
| `question.entry.fields` | a key is missing, unknown or out of order; a line in an entry is neither keyed nor continuation; a line sits outside every entry |
| `question.entry.date` | a date is not `YYYY-MM-DD`, or is earlier than the entry before it |
| `question.hypotheses` | an id is not `NNN`, or names no hypothesis file |
| `question.withdrawn` | a withdrawn line is not a date then text, sits before the fields, or appears twice |
