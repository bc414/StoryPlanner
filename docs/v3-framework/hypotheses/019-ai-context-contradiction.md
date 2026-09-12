## Hypothesis

The fundamental v0-v2 design struggle was the contradiction between needing AI
context for architectural analysis and not wanting AI voice in the prose
layer. V1's full-plan-paste feedback loop (~940K characters per session)
accumulated AI voice in the .storyplan through a read-generate-paste-reread
cycle; v2's cognitive modes couldn't break this because full-plan export was
still the interaction paradigm; v3's MCP sidecar architecture is the first
design that reconciles AI context access with voice separation.

## Origin

- date: 2026-08-31
- reasoning: Raised while a session was extending the plan's downstream
  voice-linting hypothesis with lineage evidence for why the v1 copy-paste
  habit began. Brian supplied the underlying reason himself: he pasted
  insights back into the plan not only to avoid losing them, but because
  powering any analysis at all meant exporting the whole plan into Gemini's
  1M-context window — Claude, before he held a Max subscription, could not
  hold that much, and even a full context window today would not fit the plan
  with good results. From there he asked directly whether all of v0, v1 and
  v2's design struggles reduce to one contradiction — needing the AI to have
  enough context for architectural analysis while not wanting the AI's voice
  in the prose layer — and whether v3's sidecar MCP architecture, which gives
  the AI targeted access without the plan absorbing what it produces, is what
  finally reconciles it.

## Record
