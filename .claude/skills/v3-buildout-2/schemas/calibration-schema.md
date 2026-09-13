# calibration-schema

`docs/v3-framework/studies/<study>/calibration-<date>.md`, beside the directions version it
judged; the referee's, in `docs/v3-framework/referee/`, are the same class reached by
reference from the definition of a batch that names them. One per calibration of one
directions version, frozen. It is the measurement that lets a version be trusted to say
what Brian would have said, and it is what "calibrated" means: a version with no accepting
calibration at its body hash is uncalibrated whatever anyone says of it. Two dates on one
day take a letter after the date. Not yet reviewed to the four-section shape; the title's
hash and the verdict are what the definition check reads.

```markdown
# Calibration — directions-N@<hash> — <date>

## Sample
<the calibration batch, cited as <study>/<batch>; the item ids, how drawn (stratified by
class, spanning ≥ 3 targets for the referee), count; the held-out split: which items were
ruled on, which were scored after the rulings>

## Scorings
| item | Brian | agent | agree |
<Brian's verdicts written by the session as he gave them, the agent's results withheld
until his were complete; the agent's from the batch's results>

## Agreement
<per class, on the ruled items and on the held-out items separately; the model that scored>

## Rulings
<one per disagreement: the item, both verdicts, Brian's ruling under rule 10, and the criterion it
produced or changed, if any — each such edit is what makes the next version>

## Verdict
<Brian: accepted at this hash, or a new version to calibrate>
```
