# verifying-a-corpus

Enables reviewing-findings.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| assemble-full-batch | session | runner tool-source | studies directions calibration corpus | definition index items calls tally | specified | On Brian's go: the batch made and handed off. The definition written naming the accepted version and its calibration; the itemizer run once into the batch; dry-run-batch; execute-batch, after which the host calls every item and writes the tally when the last has a result |
| assess-items | agent | | directions items | results | specified | One call per item: the directions as its system prompt, the item as its message, the answer in the declared fields; an item the criteria cannot place goes to the class reserved for it; the only writer of results |
| write-findings | session | | definition index calls results tally question-list | findings | specified | The analysis: every result read through the tally and the index with the questions in view, and what the data shows written as findings, null results included, each citing the tally sections and items it rests on; what the data raised as proposed questions; what the results showed wrong with the instrument as shortcomings; never what a finding means for a hypothesis |

## Preconditions

The study is in the registry with Brian's go. The directions version the batch will name
has an accepting calibration at its body hash, and the itemizer that cut the calibration
sample exists as a tool. Every open question the verification is to answer is named in
that version's frontmatter; a question it does not cover is not this verification's and
goes back through preparing.

## assemble-full-batch

One batch folder under the study, `batches/<nn>-<slug>/`, its definition of kind full
naming the accepted version by path and the calibration that accepted it, the study's
model and effort, and any tool the calls opt into. The itemizer runs once into the folder,
writing the index and the item bodies. Then, per the `agent-runner` skill: dry-run-batch,
which checks the definition, the directions, the index and every item in memory and writes
nothing; then execute-batch, the hand-off. From there the host calls every item that has
no result, under its ceilings, appends each call to the calls file as it ends, and writes
the tally when the last item has a successful call. No pilot: the calibration was it. An
item still without a result after the host is done is called again by typing execute-batch
again; nothing about the batch changes. The session watches the page or the calls file and
does nothing else until every item is answered.

## assess-items

Instructed by the directions body as its system prompt and nothing else; the item's text
as its message; the study's model and effort; no tools and no MCP unless the definition
opts in, since the itemizer put into the item everything the call needs. One item in, one
answer out in the declared fields, which the runner renders as the result. An item the
criteria cannot place is put in the class the directions reserve for that, never left
blank.

## write-findings

The session writes `findings.md` under the study, in its schema's shape. It reads every
result through the tally and the index, with the questions the version's frontmatter names
in view, using `tally-batch --group-by` for any cross-tab it needs, and writes what the
data shows: one finding per thing shown, a count over the corpus, a pattern across items, a
contrast between classes, an answer that is null stated as plainly as one that is not. A
finding that answers a frozen question names it; one the data raised names none; one
bearing on two questions is two findings. Each cites what it rests on, the tally section as
`<study>/<batch> § <field>` and the items as `<study>/<batch>/<item>`, whose locators the
index gives. Per-item results and counts are cited, never copied. What the data raised
that is not yet a claim goes under Proposed questions. Where the results show the study's
own instrument wanting, the reserved class filling, a criterion the results split on, a
frozen question the fields cannot answer, an item cut wrong, a sample that never held a
case the batch did, the session writes it under Shortcomings, naming the part, as a fact
about the study and never about the corpus; the question it raises is Brian's, in
reviewing-findings, and the fix is a new version or a new tool through preparing. The
method section holds only what no batch file says: what was deliberately not measured,
and any caveat of the execution.

## Never

Revises the directions after the batch has started (a revision is a new version, a new
calibration and a new batch); names a hypothesis in a finding; writes a candidate, a
falsifier or a question; executes a batch whose definition names a version with no
accepting calibration; copies a count or a result into the findings; reads an
intermediate analysis in place of the item; withdraws or supersedes a finding, which is
the review's.
