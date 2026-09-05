# refereeing-a-candidate

Enables promoting-checked-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| referee-materialise | session | itemizer generator | hypothesis-statement candidates iteration-candidates codebook | items items-manifest jobs | specified | The referee instance's itemizer: for each candidate without a referee line, one item holding the target's current statement and the candidate's finding; one job per item |
| referee-run | session | runner tallier | jobs codebook calibration-record items results | ledger tally-output run-record | specified | The batch under fanout/<instance>/referee/<run>/, per the agent-runner skill; no pilot, the codebook's calibration was its pilot; the tally over the results |
| referee-judge | agent | | codebook items | results | specified | Writes the falsifier blind and classifies: diagnostic supporting, diagnostic challenging, non-diagnostic; two lines out; the only writer of results |
| referee-append | session | | results tally-output candidates iteration-candidates | candidates iteration-candidates | specified | Copies each well-formed result's two lines under its candidate; a malformed one is re-run |

<!-- generated:activity -->
<!-- /generated -->

## Preconditions

The referee codebook's current version has a calibration record at its hash. The
candidates in scope carry no `falsifier` line. Each target hypothesis's statement is the
wording the candidate was written against; if an iteration entry postdates the candidate,
the candidate belongs in an iteration candidates file, not here.

## referee-materialise

One item per candidate in scope, from the round's candidates file and any iteration
candidates for its targets. The item holds exactly two things: the target's current
statement, copied from `## Hypothesis` with no frontmatter, no record and no other
candidate; and the candidate's `finding` line. It holds no `source` line, no other
candidate, and nothing anyone wrote as a falsifier. The generator writes one job per item
with the two verdict-line markers as its output contract. The run folder is
`fanout/<instance>/referee/<run>/`.

## referee-run

Per the `agent-runner` skill: dry run, then batch under the host. No pilot job: the
codebook's calibration was its pilot, and a calibrated hash is the precondition. Reads the
jobs, the referee codebook at its calibrated hash and the items; writes the ledger,
attempts, results, `tally.md` (class counts, malformed outputs) and `run.md`.

## referee-judge

Instructed by `fanout/referee/codebook-N.md` and nothing else; Sonnet by default; tools Read
and Write; no MCP. Given the statement and the finding, it writes the falsifier — what the
finding would have been if the statement were false — and classifies the candidate by which
side of that observable the finding shows, or non-diagnostic if no such observable can be
named or the finding is consistent with both. Two lines out, in the candidate's format,
citing its job, model, time and codebook hash. Tuned to over-flag: a false non-diagnostic
costs one adjudication; a false diagnostic costs the record.

## referee-append

For each result, the session appends the `falsifier` and `referee` lines under the
candidate they belong to, verbatim, in the file the candidate came from. A malformed
result, flagged by the tally, is not appended; its job is re-run under a new id.

## Never

Gives the referee a source, an excerpt, a locator, another candidate, or the target's record;
edits a candidate's finding; runs under a hash without a calibration record; lets a session
write a falsifier.
