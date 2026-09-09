# verification-artifact-schema

`docs/v3-framework/studies/<study>/verification.md`, at the top of the study. Written once
by verifying-a-corpus; the promotion session appends its summary; corrections are
appended, dated. Per-item results are not copied here: they live in the batch's
`results/` and `tally.md`, cited as `<study>/<batch>/<item>`.

```markdown
# verification-of-fimfiction-stories-fid-primary

## Method
<the directions version and body hash and its calibration; the itemizer and item count;
the model and effort; the harness version; the batch and its calls; what was not measured>

## Questions answered
<the questions this verification's items and criteria cover, one per line, each cited as
<corpus>/<slug>; this list is what the tool derives a question's answered state from>

## Counts
<from tally.md: results per class, the flagged, the malformed, the missing; each table
cites the batch that produced it>

## Promotion
<appended by the promotion session: candidates per target; diagnostic and non-diagnostic;
promoted supporting and challenging; declined with reasons; disagreements with the referee
and how Brian ruled; anything noticed about the pipeline's own behaviour>

## Corrections
<appended, dated>
```

Counts cite the batch that produced them; a classification that bears on a hypothesis
reaches a record only through a candidate, never from this file.
