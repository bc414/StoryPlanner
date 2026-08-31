---
id: 43
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

Note-to-note design relationships (setup-to-payoff, parallel, contradicts,
revelation chain) may be needed for structurally managing serialized
publication trajectories, distinct from the rejected supersession links
(FEATURE-AUDIT C1) and currently invisible in the data.

## Record

- created | 2026-08-31T20:00: The planner exists to manage trajectories for
  serialized publication where published chapters cannot be rewritten. A
  prior-belief-setup note in chapter 3 and its payoff-clash note in chapter 15
  are currently connected only in Brian's head or as prose in the note content.
  For serialized publication, these connections may need to be explicit and
  validatable before publication — if the setup is revised, every note that
  depends on it must be findable. The distinction from supersession is
  critical. Supersession (FEATURE-AUDIT C1) was "this replaces that" — a note
  making another note obsolete. That was resolved by the existing Reader Prior
  Belief Update and Garden Notes tracks, which capture revision history within
  the plan. Design relationships are "this is designed to prepare the reader
  for that" — a different semantic entirely. A setup note does not replace its
  payoff; it creates the conditions under which the payoff will land. These
  relationships have never been proposed before in the project's history. The
  codebase already has patterns that could support them without new
  architecture: SubjectRelation provides the typed-edge model between entities,
  and NarrativePropertyValue provides single-select closed-vocabulary
  annotation. Whether the need is real (do enough cross-chapter design
  dependencies exist to justify explicit tracking?) and whether existing
  subject-level arc plans already cover the need sufficiently are both open
  questions that evidence from v1 archive mining and framework evaluation
  should inform.
- evidence | 2026-09-01T01:30 | (WU1.1, post-discussion) [contextual]: The corpus
  documents emotional investment through accumulation as a cross-scene effect
  where Demonstration note 15 depends on notes 1-14 for its meaning. This is a
  type of designed note-to-note relationship — not setup→payoff (the notes don't
  resolve each other) but accumulation (each instance's effect is shaped by all
  prior instances). The v2 codebase can support this without new architecture
  (Type Object tracks are config, not code), but whether accumulation
  relationships need to be *explicit* or are adequately implied by the plan +
  instances bookend is the open question this hypothesis tests.
