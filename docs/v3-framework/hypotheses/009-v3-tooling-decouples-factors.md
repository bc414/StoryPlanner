---
id: 9
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

V3 tooling — MCP for data/instruction decoupling, skills for
instruction/model decoupling, open protocol for harness/model decoupling —
makes the four-factor independence (hypothesis 006) testable for the first
time and enables retrospective review of prior AI-plan interactions that were
previously inaccessible.

## Record

- created | 2026-08-31T20:00: Merges two related claims from the pipeline
  hypothesis dump. First: MCP sidecar queries replace whole-plan pastes,
  decoupling data from instructions; skills enforce analytical rigor regardless
  of which model runs them, decoupling instructions from model; MCP as open
  protocol theoretically decouples harness from model. Whether this
  independence is real or aspirational is testable by varying one factor while
  holding others constant — but the testing methodology is itself unresolved
  (who verifies, what constitutes a positive signal). Second: the MCP server
  is the first architecture that lets the AI look backward at its own prior
  interactions with the plan data — before MCP, only the most recent paste
  existed, so retrospective review of what an earlier session saw and produced
  was impossible. This backward-looking capability is what makes evidence-based
  instruction design feasible. Both claims are thin — asserting capability
  without evidence that the decoupling holds in practice or that the
  retrospective review produces actionable findings.
