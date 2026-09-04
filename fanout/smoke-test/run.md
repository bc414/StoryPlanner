# Harness smoke test

**Work:** `smoke-test` — the check that the runner itself behaves: one job, no MCP, `Write`
only, writes `OK`, lists the tools it can see. Not an experiment; no question, no instrument,
no tally. Re-run after any runner change under a new job id (the ledger never re-launches a
succeeded id). **Passes when:** the output is present, the stream's init event lists only the
configured tools and no MCP servers, and no transcript appears under the StoryPlanner project
directory.
