---
id: 22
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

Each AI voice in the corpora requires defined treatment rules rather than uniform
handling: Gemini's RLHF-tuned finality in lineage carries hypothesis-level not
fact-level authority, Claude's formulations in conversations carry different
authority from Brian's turns, and v1 text copy-pasted from Gemini needs epistemic
recognition as unfinished proposals despite their air of settled certainty.

## Record

- created | 2026-08-31T20:00: The v1 notes contain Gemini-voice text that was
  pasted into the .storyplan so it could be used as working material — insights,
  framings, analytical observations. Brian pasted these because the content was
  useful, not because he endorsed the framing. The problem is that Gemini's RLHF
  tuning produces text with hyperbolic conviction: capitalized emphasis, an air of
  finality, declarative framing that reads as settled conclusion rather than
  working hypothesis. "This is THE fundamental tension" reads as a closed judgment
  when it was an unfinished proposal from a model whose reinforcement learning was
  tuned for user satisfaction and perceived certainty. The treatment rule: Gemini
  lineage text is hypothesis-level, never fact-level, regardless of how final it
  sounds. Claude's formulations in the conversations corpus carry a different
  authority profile — Claude's analytical turns are responses to Brian's questions
  and operate under explicit system instructions that shape their framing, so
  their authority traces through the instruction chain rather than being
  self-contained. Brian's own turns in those same conversations are his analytical
  voice (register 4 from H021), not AI voice at all, and carry a different
  citation status. The practical consequence: any mining or linting pass over v1
  notes must recognize Gemini-pasted text as a distinct voice category with its
  own epistemic standing, not conflate it with Brian's fabula or analytical voice
  just because it lives in the same note. The same applies to any future corpus
  where AI-generated text is preserved — the treatment protocol is per-model and
  per-context, not a blanket "AI voice = lower authority" rule, because the
  specific failure modes differ (Gemini's false finality vs Claude's instruction-
  shaped framing vs NotebookLM's synthesis artifacts).
