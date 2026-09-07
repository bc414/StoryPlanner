# candidate-schema

`fanout/<study>/candidates.md`, one per round; `fanout/referee/iterations/NNN-<date>/candidates.md`
for the findings re-queued by a rewording of hypothesis NNN. Append-only: a candidate is never edited after it
is written; the referee's lines and the outcome are appended beneath it. A finding bearing
on two hypotheses is two candidates. Status is read from the last line present: a diagnostic
candidate with no outcome line is awaiting Brian's decision.

```markdown
## C-014
- target: 031
- finding: <one citable unit: what was observed, with ids, counts, passages>
- source: <the locator: corpus locus, note ids, story and chapter, artifact section>
- proposed-by: <job id> / <model> / <ISO time> / <codebook id>@<hash> / harness <version>
```

Referee append, from the referee's result for this candidate (the referee saw the target's
current statement and the `finding` line only — never `source`, never a falsifier anyone
else wrote):

```markdown
- falsifier: <if the statement were false, the finding would have been ___ instead>
- referee: <job id> / <model> / <ISO time> / referee-N@<hash> / diagnostic [supporting|challenging] | non-diagnostic — <one-line reason>
```

Outcome append, written by the promotion session for a diagnostic candidate only, recording
Brian's decision; a non-diagnostic candidate gets no further line and stays as context:

```markdown
- outcome: promoted <ISO time> as evidence entry <timestamp in NNN-slug.md> | declined — <Brian's reason>
```

The `source` locator is the citation check: before a candidate is promoted, the promotion
session reads the source it names and confirms the finding is there as stated. A decline
needs no read. Nothing upstream confirms the locus; the classifier named it and the referee
judged only the finding's shape.
