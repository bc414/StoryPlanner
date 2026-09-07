---
id: 45
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

Brian's Google Keep notes contain provenance material — early hypotheses,
intuitions, corrections — not already captured in the existing lineage corpora,
and if so they warrant an ingest path into the lineage corpus.

## Record

- created | 2026-08-31T20:00: The existing lineage corpus covers four source
  layers: the pre-AI Google Doc revision history (Apr 2025 through Jan 2026),
  Gemini web conversations (Sep 2025 through Jun 2026), AI Studio chats
  (early 2026), and NotebookLM captures. Google Keep was used for quick
  capture — short notes, reminders, fragments of ideas — and sits outside all
  four layers. The pipeline hypotheses (the Google Keep dump that produced
  pipeline-hypotheses-raw.md) demonstrate that Keep contains material with
  provenance value: hypotheses about the operating pipeline, intuitions about
  model behavior, corrections to earlier thinking. The question is conditional:
  does Keep contain unique material not already present in the other corpora?
  If the same insights were captured in Gemini conversations or AI Studio
  chats, Keep adds redundancy but not new evidence. If Keep captured thoughts
  that never made it into a conversation — quick intuitions, corrections made
  between sessions, ideas that arrived outside of an AI interaction — then
  those represent a gap in the provenance record. If unique material exists,
  an ingest path is warranted, following the existing lineage sidecar pattern:
  a tool that writes to lineage.db with its own tables, a manifest in the
  shared IngestRuns ledger, and source-prefixed ids (presumably "keep:").
  The ingest would need to handle Keep's unstructured format (no guaranteed
  timestamps on all notes, variable length, mixed content types) — similar
  challenges to the NotebookLM captures, which also required authored dates
  in the config because the source data lacked reliable timestamps.
