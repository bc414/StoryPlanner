# refereeing-a-candidate

Enables promoting-checked-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| referee-materialise | session | tool-source | hypothesis-statement candidates directions calibration | definition index items | specified | The referee's itemizer: for each candidate without a referee line, one item holding the target's current statement and the candidate's finding, into a batch under the verification whose definition names the referee's directions and calibration by path |
| referee-run | session | runner | definition results | calls tally | specified | Per the agent-runner skill: dry-run-batch, execute-batch under the host, tally-batch; no pilot, the referee's calibration was it |
| referee-judge | agent | | directions items | results | specified | Writes the falsifier blind and classifies: diagnostic supporting, diagnostic challenging, non-diagnostic; the only writer of results |
| referee-append | session | | results tally candidates | candidates | specified | Copies each well-formed result's two lines under its candidate; a malformed one is called again by a later execution |

## Preconditions

The referee's current directions version, in the referee folder, has an accepting
calibration at its hash. The candidates in scope carry no `falsifier` line. Each target
hypothesis's statement is the wording the candidate was written against; a candidate
written against an earlier wording is re-queued by iterating-a-statement before it is
refereed.

## referee-materialise

One item per candidate in scope, from the verification's candidates file, its re-queued
candidates included. The item holds exactly two things: the target's current statement,
copied from `## Hypothesis` with no frontmatter, no record and no other candidate; and the
candidate's `finding` line. It holds no locator, no other candidate, and nothing anyone
wrote as a falsifier. The batch folder is under the verification the candidates belong
to, `batches/<nn>-<slug>/`; the itemizer, a tool under `tools/`, writes the index with the
corpus `candidates` and the items; the definition, of kind full, names the referee's
directions and their accepting calibration by relative path into the referee folder, and
the referee's model and effort.

## referee-run

Per the `agent-runner` skill: dry-run-batch, then execute-batch under the host, then
tally-batch. No pilot: the referee's calibration was it, and a calibrated hash is the
precondition.

## referee-judge

Instructed by the referee's directions body as its system prompt and nothing else; no
tools, no MCP. Given the statement and the finding, it writes the falsifier — what the
finding would have been if the statement were false — and classifies the candidate by
which side of that observable the finding shows, or non-diagnostic if no such observable
can be named or the finding is consistent with both. Two fields out, which the runner
renders as the result. Tuned to over-flag: a false non-diagnostic costs one adjudication;
a false diagnostic costs a hypothesis's record.

## referee-append

For each result, the session appends the `falsifier` and `referee` lines under the
candidate they belong to, verbatim, in the file the candidate came from, the referee line
citing the call as `<study>/<batch>/<item>` with the model, the time and the directions
hash. A malformed result, flagged by the tally, is not appended; its item is called again
by a later execution of the batch.

## Never

Gives the referee a source, an excerpt, a locator, another candidate, or the target's
record; edits a candidate's finding; executes under a hash without an accepting
calibration; lets a session write a falsifier; authors or edits the referee's directions
in a verification.
