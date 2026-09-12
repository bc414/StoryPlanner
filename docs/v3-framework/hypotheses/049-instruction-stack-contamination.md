## Hypothesis

An agent applying a codebook with CLAUDE.md and the buildout skill in its context
produces classifications that drift toward the framework vocabulary those files assert
(for example, FID-centred readings of DT passages), relative to an explicit-context agent
given the same items and the same codebook and nothing else.

## Origin

- date: 2026-09-03
- reasoning: Raised while designing the explicit-context rule for autonomous fanout
  agents (classifiers, investigators, referees) in the 2026-09-02/03 design conversation.
  Brian's own question pushed the exclusion past skills to CLAUDE.md itself: "In theory,
  wouldn't I actually want the subagents running without StoryPlanner's claude.md either?
  All context passed in explicitly? Their instructions don't have to be skills, they can
  be files in the repo." The motivation named for excluding CLAUDE.md specifically:
  CLAUDE.md asserts the v2 framework vocabulary (cognitive modes, track semantics, the
  mechanism model) as settled, while the buildout treats all of it as provisional and
  under test, so an agent applying a frozen predicate — or a referee whose independence
  depends on a fresh context — should not carry the framework's own opinions about the
  thing it is classifying. The explicit-context rule (protocol plus item only, agent
  launched outside the repo) was adopted on this reasoning immediately, before it was
  tested; this hypothesis is the empirical claim the adopted rule assumes to be true.
  Brian endorsed the statement on 2026-09-03.

## Record
