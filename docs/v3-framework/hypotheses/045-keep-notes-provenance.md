---
id: 45
status: evidenced
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
- evidence | 2026-08-31T22:30 | (WU1.2) [supporting]: The Google Takeout
  export contains 5,583 Keep notes (Oct 2015 – Aug 2026). Systematic
  uniqueness test: 8 framework-relevant Keep-captured moments were searched
  against all lineage layers (gdoc diffs, Gemini entries + reports, AI Studio
  turns, NotebookLM turns + notes). All 8 returned zero hits: TLTT economic
  thesis (Jul 8, 2025), Applejack mask/democracy keystone (Aug 23, 2025),
  StoryPlanner conception (Dec 6, 2025 — lineage first discusses it Dec 29),
  purpose statement "not writing TLTT for an audience" (Apr 16, 2026), craft
  guardrail "not writing the way ai does" (Mar 2026), voice separation "text
  needs to be mine" (May 4, 2026), craft-theory questions spanning 2023–2026,
  and the 2022 KU craft self-critique. Unique material spans two categories:
  pre-AI (Oct 2015 – Aug 2025, 10 years with zero lineage coverage) and
  concurrent-era between-conversation thoughts that never entered any AI
  session. An existing Claude Code analysis (Aug 10, 2026, five HTML
  artifacts) already read the complete corpus and provides a curatorial guide.
  Timestamps are microsecond-precision. Ingest feasibility is high, following
  the lineage sidecar pattern with an authored include-list (precedent:
  code-sessions, NotebookLM captures). Selective ingest recommended (~300–500
  provenance-relevant notes of 5,583 total; ~130 credential-containing notes
  must be excluded). Full assessment: WU1.2-keep-assessment.md.
- evidence | 2026-08-31T23:00 | (WU1.2, Brian's correction) [supporting]:
  The 8-for-8 uniqueness test is real but overstates overall uniqueness.
  The December 2025 worldbuilding avalanche — the single largest block of
  framework-relevant Keep content — was copy-pasted as Gemini prompts
  during the first week of Gemini interaction (lineage W49). Searching
  lineage for characteristic December vocabulary returns 317 hits. The
  uniqueness is therefore category-specific: (1) content-unique material
  (self-reflective, metacognitive, pre-AI, craft-theory — no lineage echo),
  (2) timestamp-unique material (content echoed in lineage via copy-paste,
  but Keep adds the prior capture timestamp, establishing ideas formed
  before AI engagement), (3) non-unique material (same-day paste, minimal
  provenance value). An ingest path is still warranted — Category 1 exists
  nowhere else, and Category 2 timestamps have provenance value — but the
  ingest design should distinguish the categories rather than treating all
  Keep notes as equally unique. Assessment updated accordingly.
- evidence | 2026-08-31T23:30 | (WU1.2, verification round 2) [supporting]:
  Broader concept searches (not just exact phrasing) tested whether the IDEAS
  behind the 8 test items appear in lineage in different words. Result: the
  initial exact-phrasing test was methodologically insufficient — 2 of the 8
  items (TLTT economic thesis, Applejack mask/democracy) are actually Category
  2 (timestamp-unique), not Category 1 (content-unique). The Applejack+mask
  concept has 55 hits in lineage from Dec 4, 2025 onward; the Keep note (Aug
  23) is the temporal origin but the idea entered lineage. The remaining 6
  items (StoryPlanner conception, purpose statement, craft guardrail, voice
  separation, craft-theory questions, KU critique) returned 0 hits even with
  broadened concept searches — these are genuine Category 1. The pattern:
  worldbuilding/story-content ideas entered lineage via copy-paste; self-
  reflective, metacognitive, and craft-methodology notes did not. Brian's
  thinking about his own process is unique to Keep; his story content largely
  is not. Hypothesis remains evidenced — Category 1 material exists and
  warrants ingest.
