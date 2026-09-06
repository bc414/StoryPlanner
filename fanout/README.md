# fanout — everything the agent runner takes in and puts out, grouped by work

One folder per work that runs autonomous agents through `tools/StoryPlanner.AgentRunner`:
an instance of the v3 buildout, named by its registry id, or a standing action outside it
(`skill-audits/`, `smoke-test/`). The layout of a work folder and of a run, what a run
commits, and every rule are in the `agent-runner` skill (`.claude/skills/agent-runner/`);
the order of a run is derived by the `v3-buildout` skill into its `map.md`, which the host
renders at `/protocol`. Nothing here duplicates either: a second copy of the tree is how a
README goes stale. (`PROTOCOL.md`, the hand-kept lifecycle page of 2026-09-03, retired on
2026-09-05 and was deleted; its text is in git and in the 2026-09-05 skill audit's document A.)

Created 2026-09-03; reduced to a pointer 2026-09-05.
