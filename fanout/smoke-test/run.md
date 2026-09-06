# Harness smoke test

**Work:** `smoke-test` — the check that the runner itself behaves: one job, no MCP, `Write`
only, writes `OK`, lists the tools it can see. Not an experiment; no question, no instrument,
no tally. Re-run after any runner change under a new job id (the ledger never re-launches a
succeeded id). **Passes when:** the output is present, the stream's init event lists only the
configured tools and no MCP servers, and no transcript appears under the StoryPlanner project
directory.

**2026-09-06:** `smoke-sonnet-2026-09-06-stream`, after the stream-reader, catalog and map-page
change of the 2026-09-05 engineering handoff. Passed: output `OK`, init lists `[Write]` and no
MCP servers, no transcript; the enqueue answered "2 job(s) enqueued — 1 to launch, 1 skipped as
succeeded" and the stream showed the new `usage` kind. Harness 2.1.258, 5 s, api-equiv $0.017.
