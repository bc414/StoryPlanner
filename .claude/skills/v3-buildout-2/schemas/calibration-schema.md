# calibration-schema

`fanout/<study>/calibration-<date>.md`, or `fanout/referee/calibration-<date>.md`. One
per calibration of one codebook version, frozen. It is the measurement that lets a codebook
be trusted to say what Brian would have said, and it is what "calibrated" means: a codebook
version with no calibration at its hash is uncalibrated whatever its status line says.

```markdown
# Calibration — <codebook id>@<hash> — <date>

## Sample
<item ids, how drawn (stratified by class, spanning ≥ 3 targets for the referee), count;
the held-out split: which items were ruled on, which were scored after the rulings>

## Scorings
| item | Brian | agent | agree |
<Brian's verdicts written by the session as he gave them, the agent's results withheld
until his were complete; the agent's from the calibration run's results>

## Agreement
<per class, on the ruled items and on the held-out items separately; the model that scored>

## Rulings
<one per disagreement: the item, both verdicts, Brian's ruling, and the codebook edit it
produced, if any — each edit is what makes the next version>

## Verdict
<Brian: accepted at this hash, or re-run after edits>
```
