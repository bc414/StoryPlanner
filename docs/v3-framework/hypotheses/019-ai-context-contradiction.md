---
id: 19
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

The fundamental v0-v2 design struggle was the contradiction between needing AI
context for architectural analysis and not wanting AI voice in the prose layer.
V1's full-plan-paste feedback loop (~940K chars per session) accumulated AI voice
in the .storyplan through a read-generate-paste-reread cycle; v2's cognitive modes
couldn't break this because full-plan export was still the interaction paradigm;
v3's MCP sidecar architecture is the first design that reconciles AI context
access with voice separation.

## Record

- created | 2026-08-31T20:00: V0 had no AI involvement: no voice contamination,
  but no architectural analysis either — Brian hit the complexity wall alone. V1
  introduced the full-plan-paste paradigm: approximately 940K characters of the
  entire working plan loaded into Gemini's 1M context window every session (reports
  W05-W07 document the scale). This created a feedback cycle with four steps: the
  AI reads the plan, generates insight or analysis, Brian pastes the insight back
  into the plan as notes, and the AI reads its own prior output in the next
  session. Each iteration layered more AI-voiced text into the .storyplan. The
  accumulation was not accidental contamination but a structural consequence of the
  interaction model — the plan was simultaneously the working instrument and the
  context delivery mechanism, so anything the AI produced that was worth keeping
  had to go back into the plan to be available next time. V2 attempted to break
  this by introducing cognitive modes (ZeroFocalization, NarrativeDesign) that
  separated what was visible and writable, but the underlying interaction paradigm
  was unchanged: full-plan export was still how the AI received context. The modes
  separated fabula from syuzhet within the plan but could not separate Brian's
  voice from the AI's, because both were already entangled in the notes the modes
  operated on. The v3 MCP sidecar architecture reverses the flow: the AI accesses
  plan data through targeted queries rather than absorbing the entire plan, and the
  AI's analytical voice lives in external corpora (lineage.db, conversations,
  code sessions) rather than in the .storyplan itself. This is the first
  architecture where the AI can have full context without the plan absorbing the
  AI's output as a side effect of providing that context.
