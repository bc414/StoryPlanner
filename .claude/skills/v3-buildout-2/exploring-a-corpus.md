# exploring-a-corpus

Enables reviewing-leads.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| pathfind | session | | corpus question-list | leads-artifact | built | One session reads the whole corpus in one context, questions in view, and writes the leads artifact |
| slice-run | session | generator runner | studies items items-manifest reading-protocol | jobs ledger run-page | specified | On Brian's registered go: one job per slice, or per slice and arm, under the piloted protocol; the batch under the host; no pilot here, preparing did it |
| slice-read | agent | | reading-protocol items | results | specified | A slice reader reads one slice under the protocol and writes its lead set; the only writer of results |
| join-and-bin | session | | results run-page question-list | leads-artifact | specified | Joins the arms' lead sets, sorts disagreements between arms into the named bins with the arm key closed, and writes the leads artifact |

## Preconditions

The study is registered with Brian's go and its plan is approved. For slices: the
items exist, the reading protocol at its current version has been piloted, and the arm key
is written and closed. For a pathfinder: the corpus fits one context.

## pathfind

One session, Fable by default, reads the corpus whole with the questions in view named on
the study's plan. It writes `docs/v3-framework/<study>/leads.md`: the method, the
questions in view, and the leads organised by what was observed, each what was seen and
what it was seen in. Leads only: ideas for questions and for what to itemize, never a
finding, never a claim about a hypothesis. Proposed questions go in their own section, as proposals.

## slice-run

One run folder under `fanout/<study>/<run>/`. The generator writes one job per slice,
or one per slice per arm when the exploration has arms, each under the protocol at its
piloted hash with the arm's neutral label; the batch runs under the host per the
`agent-runner` skill. No pilot job: preparing-to-explore-a-corpus piloted the protocol.

## slice-read

Instructed by the reading protocol and nothing else; Opus by default; tools Read and
Write; no MCP. One slice in, one lead set out in the protocol's form: one entry per lead,
what was seen and what it was seen in. Nothing about what a lead means for any hypothesis.

## join-and-bin

The session joins the arms' lead sets; the key it joins on is the plan's. With arms, it
sorts every disagreement between arms into the named bins, expected-structural,
missed-by-one, unsupported-by-source and whatever the plan added, before any is
investigated, and the count per bin is a finding of the exploration; the arm key stays
closed, so the session does not know which condition produced which lead. With one arm
there are no bins and the artifact says no disagreement was measured. It writes
`leads.md`: method, questions in view, leads by what was seen, bins with counts, proposed
questions.

## Never

Opens the arm key; drills a bin; writes a question into a list; writes a candidate or
evidence; makes a claim about a hypothesis; investigates a disagreement before binning.
