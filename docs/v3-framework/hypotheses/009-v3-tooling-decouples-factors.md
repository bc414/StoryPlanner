## Hypothesis

V3 tooling — MCP sidecar queries for data/instruction decoupling, skills for
instruction/model decoupling, and MCP's open-protocol design for
harness/model decoupling — makes the pipeline's four factors independently
testable for the first time. Because the MCP server can query the
conversation, lineage and code-session archives instead of relying on a
single whole-plan paste, retrospective review of prior AI-plan interactions
— previously impossible — is now possible.

## Origin

- date: 2026-08-31
- reasoning: Raised while organizing the Google Keep dump into the pipeline
  hypotheses (2026-08-30), merging two of Brian's observations there. First,
  decomposing his experience of each model/era into independent factors
  (model, data stream, instructions, harness) led him to notice that v3's own
  tooling supplies a mechanism for each pairing: MCP's sidecar queries
  replace the whole-plan paste that used to carry data and instructions
  together, skills carry grounding independent of which model executes them,
  and MCP's open-protocol design means the model itself is theoretically
  swappable behind a stable harness. His assertion: whether this independence
  is real rather than aspirational only becomes checkable now, because every
  prior era changed several of these factors at once and none isolated one
  from the rest. Second, and separately, he had already observed that before
  the MCP server existed only the most recent whole-plan paste was in
  context, with no way to compare what an earlier session had seen or
  produced; the MCP server's queryable history of conversations, lineage and
  code sessions is what makes that after-the-fact review possible for the
  first time. The two observations were merged into one entry at
  consolidation as two consequences of the same MCP/skills architecture.

## Record
