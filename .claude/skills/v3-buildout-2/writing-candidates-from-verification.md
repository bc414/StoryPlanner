# writing-candidates-from-verification

Enables refereeing-a-candidate.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| write-candidates | session | | index results tally verification-artifact question-list | candidates | specified | One candidate per finding the verification claims bears on a hypothesis: target, finding as one citable unit, the item cited, the call it came from with its hashes |

## Preconditions

The verification's `verification.md` exists with its method and counts, and the batch's
`tally.md` names the flagged and malformed results. The directions version the batch ran
under has an accepting calibration at its hash.

## write-candidates

The session reads the verification's results through the tally and the index, never the
raw batch alone, and the questions the verification answered. For each result whose class
bears on a hypothesis named by one of those questions, it writes one candidate into
`candidates.md` under the study, beside `verification.md`: the target hypothesis id; the
finding as one citable unit (what was observed, with the ids, counts or passages the
result carries); the item it came from, cited as `<study>/<batch>/<item>`, whose locator
the index gives; and the call it came from, with the model, the time, the directions
version and hash and the harness version, from the batch's calls file. A result bearing on
two hypotheses is two candidates. A finding is stated as observed, not as what it means
for the hypothesis; the referee decides that.

Flagged results are not candidates. A result the tally flagged as malformed or outside
the directions' classes is left out and named in `verification.md` § Corrections.

## Never

Writes a falsifier or a verdict; writes a candidate from a leads artifact, a result under
an uncalibrated hash, or a script's count with no item behind it; edits a candidate once
written; writes to a hypothesis file.
