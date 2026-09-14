# calibration-schema

`docs/v3-framework/studies/<study>/calibration-<date>.md`, beside the directions version it
judged; the pipeline directions', the referee's in `docs/v3-framework/pipeline/referee/` and
claiming's in `docs/v3-framework/pipeline/claiming/`, are the same class reached by reference
from the definition of a batch that names them. One per calibration of one directions
version, frozen. It is the measurement that lets a version be trusted to say what Brian would
have said, and it is what "calibrated" means: a version with no accepting calibration at its
body hash is uncalibrated whatever anyone says of it, and it is calibrated for the model its
calibration batch ran. Two dates on one day take a letter after the date. Not yet reviewed to
the four-section shape; the title's hash and the verdict are what the definition check reads.

```markdown
# Calibration — directions-N@<hash> — <date>

## Sample
<the calibration batch, cited as <study>/<batch>, or as referee/<batch> or claiming/<batch>
for the pipeline directions; the item ids, how drawn (stratified by class, spanning ≥ 3
targets for the referee; standing findings across several verifications for claiming),
count; the held-out split: which items were ruled on, which were scored after the rulings>

## Scorings
| item | Brian | agent | agree |
<Brian's verdicts written by the session as he gave them, the agent's results withheld
until his were complete; the agent's from the batch's results; for claiming, the hypotheses
each finding bears on>

## Agreement
<per class, on the ruled items and on the held-out items separately; for claiming, per
(finding, hypothesis) pair, recall and precision separately, recall deciding acceptance;
the model that scored>

## Rulings
<one per disagreement: the item, both verdicts, Brian's ruling under rule 10, and the criterion it
produced or changed, if any — each such edit is what makes the next version; for claiming, a
pair Brian ruled missed at promotion or baselining, with his reason>

## Verdict
<Brian: accepted at this hash, or a new version to calibrate>
```
