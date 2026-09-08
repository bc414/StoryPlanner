# question-entry-schema

`docs/v3-framework/questions/<corpus>.md`, one list per corpus, the class `question-list`.
The shape below is what the hook holds; the example is a conforming list a writer fills in
and the block the checker's tests read as their fixture; then how a list is queried, and
the check ids the hook reports.

## Shape

The file is `# <corpus> — questions`, `<corpus>` the file's own name and an id in
CORPORA.md, then entries and nothing else: no head prose, no sections.

An entry is `### <slug>`, the slug lowercase `[a-z0-9-]+`, unique in its list, authored
with the entry and never changed; a reworded question is a new entry and the old one
withdrawn. Then keyed lines in this order and no other line, the first two values exact
and the rest free, a value continuing on lines indented two spaces:

| key | present | value |
|---|---|---|
| `date` | always | exactly `YYYY-MM-DD`, one line: the day Brian asked; never earlier than the entry before it |
| `hypotheses` | when there are any | ids `NNN` separated by single spaces, one line, each a hypothesis file; the one authored edge from a corpus's questions to the hypotheses its answers bear on |
| `raised by` | always | free: the occasion and what raised it, the citation as one token, `<study>/L-NNN` for a lead, `<study>/P-NNN` for a proposal, `<study>/C-NNN` for a candidate, `recall`, or `carried from the founding pool` |
| `question` | always | free: the question, in Brian's words |
| `suggested test` | when one suggests itself | free: a naive note on how the question might be tested, for the codebook author to take or leave; never a criterion |

Beneath the fields, appended later by the hitl process that withdraws the question, at
most once: `- withdrawn: YYYY-MM-DD <reason>`, the date exact and the reason free.

An entry is cited everywhere as `<corpus>/<slug>`, one token. Open is an entry with no
withdrawn line; frozen and answered are derived by the tool from a codebook's and a
round's citations of the id; none of the three is written. Every writer is an hitl
process, since a question is Brian's, and the class is append: no line is ever edited.

## Example

```markdown
# own-fiction — questions

### heavy-dt-two-classes

- date: 2026-09-07
- hypotheses: 031 032
- raised by: review of exploration-of-own-fiction, from exploration-of-own-fiction/L-012
- question: <the question, in Brian's words>
- suggested test: <a naive note on how it might be tested>

### narrator-register-outside-giyc

- date: 2026-09-08
- raised by: recall, in the verify-plan for round-of-own-fiction-1
- question: <the question, in Brian's words>
- withdrawn: 2026-09-09 <why>
```

## Queries

| question | how |
|---|---|
| every question of a corpus, in order | `grep -n '^### ' questions/<corpus>.md` |
| the open questions | the entries with no `- withdrawn:` line beneath them, or state.md per corpus |
| the questions bearing on hypothesis NNN | `grep -n '^- hypotheses:.*\bNNN\b' questions/*.md` |
| where a question is in view, frozen or answered | `grep -rn '<corpus>/<slug>' docs/v3-framework fanout .claude/skills` |
| what raised a question | its `raised by` line; the token in it is the lead, proposal or candidate |
| whether a calibrated codebook covers a question | state.md, derived from the codebook's `## Questions` |

## Checks

| check | fails when |
|---|---|
| `question.title` | the title is not `# <corpus> — questions` with the file's own name, or the corpus is not an id in CORPORA.md |
| `question.slug` | a heading is not a lowercase slug, or repeats one in the list |
| `question.entry.fields` | a key is missing, unknown or out of order; a line in an entry is neither keyed nor continuation; a line sits outside every entry |
| `question.entry.date` | a date is not `YYYY-MM-DD`, or is earlier than the entry before it |
| `question.hypotheses` | an id is not `NNN`, or names no hypothesis file |
| `question.withdrawn` | a withdrawn line is not a date then text, sits before the fields, or appears twice |
