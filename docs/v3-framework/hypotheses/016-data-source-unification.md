---
id: 16
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

The bespoke per-corpus schemas (conversations, lineage, code sessions, source
texts) might benefit from a common API standard for harness/model portability,
but whether a conversational API shape fits non-conversational data (source
texts, lineage doc diffs) is unresolved.

## Record

- created | 2026-08-31T20:00: Each corpus was built at a different time under
  different constraints — conversations (the first MCP surface), lineage (three
  ingest tools writing one db), code sessions (engineering-process provenance,
  deliberately outside MCP), source texts (the published material citations
  point at). Unifying them under a common API standard would make them
  harness-agnostic and model-agnostic, easing portability if the harness or
  model changes. The open question is whether the conversations API is the
  right standard shape: conversations and lineage are naturally conversational
  (turns, blocks, speakers), but source texts are published prose with no
  conversational structure, and the Google Doc revision history in lineage is
  line-level diffs between daily snapshots — forcing either into a
  conversational shape may distort the data rather than standardize it.
