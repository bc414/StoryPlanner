---
id: 20
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

Clean voice separation (Brian's voices in the .storyplan, AI voice in sidecars)
is a structural prerequisite for the v3 framework — track architecture built on
mixed-voice data inherits the confusion, so separation must precede or run
alongside framework evolution, not follow it.

## Record

- created | 2026-08-31T20:00: This is the forward prediction split from 019's
  historical diagnosis. The reasoning: if the .storyplan still contains AI voice
  when v3 track definitions are designed against that data, the definitions will
  encode the confusion rather than resolving it. A track definition that asks
  "what is the fabula truth here?" gets a different answer depending on whether
  the notes it covers are Brian's fabula voice or Gemini's analytical framing of
  the fabula — and the definition cannot distinguish them because both are stored
  identically as note text. The same applies to mining: v1 archive mining must
  identify which notes are Brian's analytical observations and which are AI
  output, because treating Brian's own analytical voice as contamination discards
  real signal. The five-voice register model (hypothesis 021) provides the taxonomy for
  this separation; this hypothesis predicts that the separation is prerequisite,
  not optional cleanup. Separable from 019: the historical diagnosis of the
  feedback loop could be confirmed while this prerequisite prediction could turn
  out to be unnecessary — perhaps mixed-voice data is workable if the framework
  is robust enough, or perhaps voice separation only matters for certain track
  types and not others. The v3 MCP architecture makes the separation technically
  feasible (AI voice is greppable in lineage and conversations), but whether it
  is structurally necessary for the framework to function is the testable claim.
