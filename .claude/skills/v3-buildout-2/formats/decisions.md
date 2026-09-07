# decisions

`docs/v3-framework/decisions.md` — the method's decisions: one file for every
revision, sections by revision in order, one titled entry per decision, appended after
Brian's approval and never edited. Every entry is Brian's: a session drafts it from his
words in the conversation, he approves, then it is written; nothing lands autonomously.
Written only during revising-the-method and read there and nowhere else. The skill's
activity text is those decisions applied, so no standard-operating activity cites one, and
a decision id appears in this folder only in `revising-the-method.md`, which the validator
holds. Borrowed shape: the dated entry whose standing is derived from supersession, as
software architecture decisions are kept; the words are kept as this method uses them.

```markdown
## Revision 2

### Generated text is files only

- date: 2026-09-06
- supersedes: d-2026-09-04-2 (in part)

Brian's words in quotation marks, the rest the session's phrasing; the reason; what was not
taken; what it resolves, as a sentence ("resolves unit-096"), never as a field.
```

**Id**: `d-<date>-<n>`, derived by the tool from the entry's `date` line and its position
among that day's entries in file order; never written by hand; stable because the file is
append-only. **`date`**: the day Brian decided or approved. **`supersedes`**: decision ids
only, whole or `(in part)`, and the prose says what still stands; an entry with no
`supersedes` line establishes something new; standing and superseded are derived, never
written; a decision already superseded when the file was founded was not entered. Words in
quotation marks are Brian's verbatim and weigh as a freestanding prompt; unquoted prose is
the session's and is never quoted as his. Every decision is a titled entry; a section holds
nothing but entries and one lead-in paragraph; context, instructions received and run
verdicts go into an entry's prose, a handoff or `run.md`, never a bare paragraph. A
revision's section is read whole by `revise`; older sections by heading. A decision that
changes an earlier revision's text reaches it through the supersession audit's unit ids, in
prose.
