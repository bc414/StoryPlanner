# refereeing-candidates

Enables promoting-refereed-candidates.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| write-candidates | session | | findings question-list hypothesis-statement hypothesis-index | candidates | specified | The sweep: every standing finding against the whole hypothesis set; one candidate per finding and per hypothesis the session claims it bears on, wide; the finding cited by its token; no falsifier, no verdict |
| assemble-referee-batch | session | runner tool-source | candidates findings hypothesis-statement directions calibration | definition index items calls tally | specified | For each candidate with no referee line, one item holding the target's current statement and the finding's text and nothing else, into a batch under the verification whose definition names the referee's directions and calibration; dry-run-batch; execute-batch as the hand-off |
| assess-referee-items | agent | | directions items | results | specified | One blind call per item: writes the falsifier and classifies diagnostic supporting, diagnostic challenging, or non-diagnostic; the only writer of results |
| append-verdicts | session | | tally results candidates | candidates | specified | Copies each well-formed result's two lines under its candidate; a malformed one is left for a later execution |

## Preconditions

For `write-candidates`: a verification whose findings have been reviewed and which has no
candidates yet. For the rest: candidates in scope with no referee line, whether the sweep's
or re-queued by iterating-a-statement, whose targets' statements are the wording each was
written against; the referee's current directions version, in the referee folder, has an
accepting calibration at its hash. A session entering from iterating-a-statement finds
the first precondition not holding and runs the last three rows.

## write-candidates

The session reads every standing finding of the verification, neither withdrawn nor
superseded, and the whole hypothesis set, the index and each statement. For each finding
it claims every hypothesis the finding could bear on, wide: the cost of a claim the
referee calls non-diagnostic is one call, the cost of a claim never made is a hypothesis
that never sees its evidence. One candidate per finding and per hypothesis, appended to
`candidates.md` under the study in its schema's shape: the target's id and the finding's
token, `<study>/<slug>`, and nothing of the finding's text; a candidate never edited once
written. No falsifier and no verdict: those are the referee's. A finding that answers a
question naming hypotheses is claimed against those at least.

## assemble-referee-batch

One item per candidate in scope, its re-queued candidates included. The item holds exactly
two things: the target's current statement, copied from `## Hypothesis` with no
frontmatter, no record and no other candidate; and the finding's text, materialised from
`findings.md` through the candidate's token. It holds no citations, no locator, no other
candidate, and nothing anyone wrote as a falsifier. The batch folder is under the
verification the candidates belong to, `batches/<nn>-<slug>/`; the itemizer, a tool under
`tools/`, writes the index with the corpus `candidates` and the items; the definition, of
kind full, names the referee's directions and their accepting calibration by relative
path into the referee folder, and the referee's model and effort. Then, per the
`agent-runner` skill, dry-run-batch and execute-batch, the hand-off; the host calls every
item and writes the tally when the last has a result. No pilot: the referee's calibration
was it.

## assess-referee-items

Instructed by the referee's directions body as its system prompt and nothing else; no
tools, no MCP. Given the statement and the finding, it writes the falsifier, what the
finding would have been if the statement were false, and classifies the candidate by which
side of that observable the finding shows, or non-diagnostic if no such observable can be
named or the finding is consistent with both. The declared fields out, which the runner
renders as the result. Tuned to over-flag: a false non-diagnostic costs one decision of
Brian's; a false diagnostic costs a hypothesis's record.

## append-verdicts

For each result, the session appends the `falsifier` and `referee` lines under the
candidate they belong to, verbatim, in the file the candidate came from, the referee line
citing the referee's item as `<study>/<batch>/<item>`, from which the call, its model, its
time and the directions hash are read in the calls file. A malformed result, named by the
tally, is not appended; its item is called again by typing execute-batch again.

## Never

Gives the referee a citation, an excerpt, a locator, another candidate, or the target's
record; writes a candidate from a withdrawn or superseded finding, or from anything but a
finding; copies a finding's text onto a candidate; edits a candidate once written; lets a
session write a falsifier or a verdict; executes under a hash without an accepting
calibration; authors or edits the referee's directions here.
