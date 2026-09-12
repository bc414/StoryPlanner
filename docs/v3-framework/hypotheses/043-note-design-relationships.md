## Hypothesis

The v1 archive contains note-to-note design relationships — setup-to-payoff,
parallel, contradicts, revelation chain — that are currently invisible in the
data: held only in Brian's memory or written as prose within a note, with no
structural link connecting the notes involved.

## Origin

- date: 2026-08-31
- reasoning: Raised on 2026-08-29, mid-session, while tracing how Note already
  attaches non-hierarchical data (world date, theme, source material). Brian's
  own observation: "v1 had typed edges - plot point links were specific to the
  entity type, like plotpointtheme, plotpointcharacter and they had enums
  attached besides the free textbox. I eliminated typed edges for polymorphic
  notes and track types for code simplification." His assertion followed: "I
  resisted note <-> note relationships in v1 and v2. Maybe it has to come
  back? v3 tooling makes the codebase complexity achievable." The motivation
  is distinguishing this from the already-rejected note supersession link
  (FEATURE-AUDIT C1): supersession says "this note replaces that note" and
  was resolved by the Reader Prior Belief Update and Garden Notes tracks; a
  design relationship instead says a note is designed to prepare the reader
  for another note's payoff — a semantic never previously proposed. What
  raised it to a hypothesis rather than a settled design call: the same
  exchange noted that once a chapter is published its setups cannot be
  rewritten, so if such connections matter, they need to be explicit and
  checkable before publication rather than held only in Brian's memory.

## Record
