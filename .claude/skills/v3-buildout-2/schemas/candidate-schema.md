# candidate-schema

`docs/v3-framework/studies/<study>/candidates.md`, one per verification, beside
`verification.md`; the findings a rewording of a hypothesis re-queues are appended here
too, to the study each came from. Append-only: a candidate is never edited after it is
written; the referee's lines and the outcome are appended beneath it. A finding bearing on
two hypotheses is two candidates. Status is read from the last line present: a diagnostic
candidate with no outcome line is awaiting Brian's decision. The file is
`# <study> — candidates`, the study the file's own, then entries and nothing else; an
entry is headed by its citation token, `### <study>/<slug>`, the slug lowercase
`[a-z0-9-]+`, unique in the file, naming what was observed and never what it means, a
per-item finding carrying the item in its slug. The fields below are not yet reviewed.

```markdown
# verification-of-fimfiction-stories-fid-primary — candidates

### verification-of-fimfiction-stories-fid-primary/fid-primary-in-one-of-seven
- target: 031
- finding: <one citable unit: what was observed, with ids, counts, passages>
- source: <the item, cited <study>/<batch>/<item>, and its locator from the index>
- proposed-by: <call, <study>/<batch>/<item> and its number> / <model> / <ISO time> / directions-N@<hash> / harness <version>
```

Referee append, from the referee's result for this candidate (the referee saw the target's
current statement and the `finding` line only — never `source`, never a falsifier anyone
else wrote):

```markdown
- falsifier: <if the statement were false, the finding would have been ___ instead>
- referee: <call> / <model> / <ISO time> / directions-N@<hash> / diagnostic [supporting|challenging] | non-diagnostic — <one-line reason>
```

Outcome append, written by the promotion session for a diagnostic candidate only, recording
Brian's decision; a non-diagnostic candidate gets no further line and stays as context:

```markdown
- outcome: promoted <ISO time> as evidence entry <timestamp in NNN-slug.md> | declined — <Brian's reason>
```

The `source` line is the citation check: before a candidate is promoted, the promotion
session reads the item it names, at its locator, and confirms the finding is there as
stated. A decline needs no read. Nothing upstream confirms the item; the call named it and
the referee judged only the finding's shape.
