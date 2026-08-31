---
id: 6
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

The AI-assisted planning experience decomposes into four independently varying
factors — model, data stream, instructions, and harness — that were historically
confounded in each era change, and each factor independently improves analytical
outcomes.

## Record

- created | 2026-08-31T19:30: Each era (v0-v3) changed multiple factors
  simultaneously, making it impossible to attribute outcomes to any single one.
  The four factors: model (none → Gemini → Claude Sonnet/Opus, with sub-model
  variation), data stream (none → full-plan paste ~940K chars → MCP sidecar
  queries), instructions (platform default → custom gem / AI Studio system prompt
  → CLAUDE.md + skills), and harness (Google Doc → Gemini web chat / AI Studio →
  Claude web chat → Claude Desktop / Claude Code). The factors are "independent,
  not orthogonal" — correlated (a more capable model benefits more from better
  instructions) but varying on separate axes. Whether they are truly independent
  or merely less coupled is itself part of the testable claim. The synthesis
  plan identified the same decomposition from the analytical-outcomes angle: four
  independent factors (intent, system prompt quality, data architecture, model
  capability) each improve results, and none substitutes for another.
