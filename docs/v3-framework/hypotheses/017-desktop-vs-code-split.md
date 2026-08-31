---
id: 17
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

The Desktop-analyzes/Code-builds role separation remains the correct
architectural boundary because story analysis and planner building require
different session contexts, despite both consumers now sharing MCP and
instructional infrastructure.

## Record

- created | 2026-08-31T20:00: The original reasoning was role separation:
  Desktop analyzes the story, Code builds the planner. Code's capabilities
  have since grown — it has skills, MCP, agentic workflows, and "can do way
  more" than Desktop in some dimensions. Both consumers now share MCP as
  data infrastructure and the instructional text stack has parallel layers
  for each (hypothesis 014). The question is whether the role separation
  still justifies the architectural boundary, or whether the shared
  infrastructure makes the split an accident of history. The case for
  maintaining the split: story analysis and planner building require
  different session contexts (analysis needs the full narrative corpus and
  conversational depth; building needs the codebase, tests, and agentic
  tool use), and collapsing them into one consumer would overload the
  context with both concerns simultaneously. The case against: skills and
  MCP connectors may be harness-agnostic enough that the split is
  organizational habit rather than architectural necessity.
