---
id: 15
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

Copy-pasted AI text into the plan is a positive acceptance signal about output
quality (analogous to RLHF reward), and other signals (Conversation Reader
states, user corrections, endorsed vs rejected proposals) may be systematically
mineable for evidence-based instruction design.

## Record

- created | 2026-08-31T20:00: When Brian copy-pasted AI output into the
  .storyplan, that was an implicit quality judgment — the prompting context that
  produced copy-paste-worthy output is direct evidence for instruction design
  (hypothesis 014). The signal extends beyond copy-paste: Conversation Reader
  block states (done, flagged, skipped) encode per-block acceptance/rejection
  judgments; Brian's corrections in user turns are negative signal (the AI got
  it wrong and Brian fixed it); endorsed proposals (Brian acts on a suggestion)
  vs rejected ones (Brian ignores or rebuts) carry signal about what analytical
  approach worked. Whether these signals are systematically mineable — and
  whether industrial RLHF/fine-tuning/post-training methodology offers
  applicable technique, or whether the scale is too small and manual review is
  the right approach — is itself an open question within this hypothesis.
