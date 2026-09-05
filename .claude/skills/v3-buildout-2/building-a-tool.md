# building-a-tool

Enables preparing-to-explore-a-corpus, preparing-to-verify-a-corpus and revising-the-method.

| id | mode | instruments | reads | writes | state | description |
|---|---|---|---|---|---|---|
| build | hitl | dotnet git | corpus-status tool-source corpus | tool-source corpus-status corpus | built | Brian fixes what is built and its acceptance; the session builds it with tests under the testing skill; he signs off the verification checklist; an ingest writes what it ingests and the corpus's state is recorded |

<!-- generated:activity -->
<!-- /generated -->

## Preconditions

Something needs a tool that does not exist: a corpus to make readable, a change to the
runner or the validator, a render. The need is named by the instance or the revision that
has it; an itemizer is preparing-to-verify-a-corpus's own.

## build

The session states what is to be built, what will consume it and how acceptance will be
shown: the tests that must pass, the CORPUS-STATUS entry that must be true afterwards, the
checklist Brian will click through. Brian approves; the decision's record is the code and
its tests. The session builds under the `testing` skill: the tool with its pure tests, the
publish step where one applies, the ingest run when it is an ingest. When a corpus
becomes readable, or changes state, CORPUS-STATUS is updated in the same commit. Brian
signs off the checklist; a tool he sends back is rebuilt in the same activity. This is the
first task inside the instance that needs the tool, never an instance of its own. A tool
never authors prose that is Brian's: no display questions, no definitions, no codebook,
no question.

## Never

Writes a codebook or a protocol; writes a candidate, a lead or a question; changes a
`.storyplan`; ships without tests.
