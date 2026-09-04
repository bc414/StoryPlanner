---
id: 49
status: untested
baselined: false
created: 2026-09-03
---

## Hypothesis

An agent applying a codebook with CLAUDE.md and the buildout skill in its context
produces classifications that drift toward the framework vocabulary those files assert
(for example, FID-centred readings of DT passages), relative to an explicit-context agent
given the same items and the same codebook and nothing else.

## Record

- created | 2026-09-03T16:00: Originated from Claude's reasoning in the 2026-09-02/03
  design conversation, prompted and endorsed by Brian ("wouldn't I actually want the
  subagents running without StoryPlanner's claude.md either? All context passed in
  explicitly?"). The reasoning: CLAUDE.md asserts the v2 framework vocabulary (cognitive
  modes, track semantics, the mechanism model) as authoritative while the buildout treats
  it as under test; an agent whose job is to apply a frozen predicate should not carry the
  framework's opinions about the thing it is classifying. Revision 1's explicit-context
  rule (protocol + item only, launched outside the repo) was adopted on this reasoning
  before it was tested; this file is the testable claim behind the rule. Brian endorsed the
  statement on 2026-09-03.
