# building-a-tool

Enables preparing-to-explore-a-corpus and preparing-to-verify-a-corpus.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| build | hitl | dotnet git | corpora tool-source corpus | tool-source runner-skill corpora corpus | built | Brian fixes what is built and its acceptance; the session builds it with tests under the testing skill; he signs off the verification checklist; an ingest writes what it ingests and the corpus's state is recorded; a runner change updates its skill |

## Preconditions

Something needs a tool that does not exist: a corpus to make readable, a change to the
runner or the validator, a render. The need is named by the study or the revision that
has it; an itemizer is preparing-to-verify-a-corpus's own. A check is added for a failure a
run or a study has shown, never for one a session can imagine. A check is minted in one
of two ways: declared, it holds what a schema's Shape says about one class's files and is
held from the schema's first write; earned, it prevents a failure that has been observed,
the only way a check that spans files, or the code and the tables, or a run comes to
exist. The decision that mints a check says which.

## build

The session states what is to be built, what will consume it and how acceptance will be
shown: the tests that must pass, the CORPORA.md entry that must be true afterwards, the
checklist Brian will click through. Brian approves; the decision is recorded as the code and
its tests. The session builds under the `testing` skill: the tool with its pure tests, the
publish step where one applies, the ingest run when it is an ingest. When a corpus
becomes readable, or how it is read changes, CORPORA.md is updated in the same commit; when the
runner gains or changes a verb, the `agent-runner` skill is updated in the same commit. Brian
signs off the checklist; a tool he sends back is rebuilt in the same activity. This is the
first task inside the study that needs the tool, never a study of its own. A tool
never authors prose that is Brian's: no display questions, no definitions, no codebook,
no question. A checker over governed files is not armed until its first run over the real
files has been predicted in writing, from the decisions and the schema, and the run
compared with the prediction; a discrepancy is a wrong derivation or a decision not yet
made, and goes to Brian first.

## Never

Writes a codebook or a protocol; writes a candidate, a lead or a question; changes a
`.storyplan`; ships without tests.
