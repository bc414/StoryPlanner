---
id: 44
status: untested
baselined: false
created: 2026-08-31
---

## Hypothesis

v1's bespoke-but-vibe-coded approach produced scene-level planning density
that v2's clean architecture could not replicate because scene-level work
stalled — v3 may restore that density at v2's quality level, or may introduce
its own tech-debt trap at a higher abstraction level.

## Record

- created | 2026-08-31T20:00: v1 had a rich scene graph (1,125 links, 450
  plot points) with instinctive mechanism usage embedded in free-form
  textboxes — Brian was doing scene-level design work without formal
  vocabulary, guided by instinct and Gemini's analytical framing. v2 replaced
  the free-form textboxes with a clean Type Object architecture (tracks,
  cognitive modes, display questions) but scene-level work stalled: the
  FID-centric perception-gap vocabulary felt too narrow to be actionable, the
  sequential workflow gated scenes on subject completion, and the plan did not
  fully feel like Brian's own instrument due to AI voice contamination. The
  result: v2 has architectural clarity but thin scene-level content, while v1
  has architectural debt but dense scene-level content. The hallmark wall
  observation frames this as a recurring pattern: each version hit the same
  wall at a higher abstraction level. v0's wall was fabula leaking into the
  chapter plan (the data's complexity exceeded a Google Doc). v1's wall was
  architectural density exceeding free-form textboxes (the scene graph's
  complexity exceeded the instrument). v2's wall was scene-level work stalling
  under a narrow vocabulary and voice contamination (the framework's
  prescriptiveness exceeded what felt actionable). Whether these are really
  "the same wall" — data complexity exceeding instrument capacity at
  progressively higher abstraction — or three unrelated problems with
  superficial structural similarity is part of what this hypothesis tests. If
  the pattern is real, v3's version of the wall is predictable: whatever v3
  introduces will eventually be exceeded by the next level of complexity. The
  question is whether v3's evidence-based, iterative method (hypothesize, test,
  revise) can absorb that complexity incrementally rather than requiring another
  architectural break — or whether that belief is itself the v3 tech-debt trap.
