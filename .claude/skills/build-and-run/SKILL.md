---
name: build-and-run
description: How to build, test, run and publish every component — the WPF app, the MCP server, the agent runner, the pocket reader, DocIntegrity — and why each runs from a published copy rather than bin/Debug. Read before building or running anything, and ALWAYS after changing code under WindowedStoryPlanner, StoryPlanner.Core, tools/StoryPlanner.Mcp or tools/StoryPlanner.AgentRunner, because shipping a change needs an explicit republish.
---

# Build and run

.NET 10.

```
dotnet test tests/StoryPlanner.Tests
```

Covers the MCP server's invariants and `StoryPlanner.Core`'s export/scan/transform logic. **Run
it before finishing any work in `tools/` or `StoryPlanner.Core/`.** Conventions and the known
WPF-layer gap: the `testing` skill.

## The publish split, and why it exists

Four components run from a **published copy**, not from `bin/Debug`. The reason is the same in
every case and it is not cosmetic: `dotnet build`, `dotnet run` and `dotnet test` write to
`bin/`, so anything Brian or another session has running out of `bin/` is locked, and every build
collides with it. Publishing to a separate gitignored folder means **any number of parallel
sessions can build and test freely while live processes keep running.**

The cost of the split is that shipping a change is an explicit act. Nothing you build reaches a
running component until you republish it.

| component | published to | after changing |
|---|---|---|
| WPF app | `WindowedStoryPlanner/publish/` | `WindowedStoryPlanner/` or `StoryPlanner.Core/` |
| MCP server | `tools/StoryPlanner.Mcp/publish/` | `tools/StoryPlanner.Mcp/` or `StoryPlanner.Core/` |
| agent runner | `tools/StoryPlanner.AgentRunner/publish/` | `tools/StoryPlanner.AgentRunner/` |
| DocIntegrity | `process-docs/StoryPlanner.DocIntegrity/publish/` | `process-docs/StoryPlanner.DocIntegrity/` |

### The WPF app

```
dotnet publish WindowedStoryPlanner -c Debug -o WindowedStoryPlanner/publish
```

**Debug on purpose**, so a debugger can still attach and hit breakpoints. Brian launches
`WindowedStoryPlanner/publish/WindowedStoryPlanner.exe` day to day; he closes and relaunches it
manually to pick up a republish — there is no live-reconnect equivalent for a WPF window, and the
point of the split is to remove *build* contention, not to make the relaunch unnecessary.

**Claude Code does not drive the app.** Verifying a UI change means launching `bin/Debug` against
a *copy* of a `.storyplan` (with its `-wal`/`-shm`), then stopping and handing Brian a numbered
checklist — he clicks and signs off, and only then does the publish happen. Procedure and
rationale: the `testing` skill, "The third tier is Brian".

### The MCP server

```
dotnet publish tools/StoryPlanner.Mcp -c Release -o tools/StoryPlanner.Mcp/publish
```

`.mcp.json` points every session's `storyplanner` connection at
`tools/StoryPlanner.Mcp/publish/StoryPlanner.Mcp.dll`. After republishing, reconnect via `/mcp`
in each session that should pick it up; until reconnected, a session keeps running the server
code from its last connect.

If the publish itself fails on a locked file, some session still holds the *publish* folder open
mid-reconnect — wait for it to finish, or ask that session to retry `/mcp`.

### The agent runner

A persistent host serving its page on `http://127.0.0.1:5190`, which holds the exe loaded for as
long as it lives — so a republish needs a stop first:

```
tools/StoryPlanner.AgentRunner/publish/StoryPlanner.AgentRunner.exe stop
dotnet publish tools/StoryPlanner.AgentRunner -c Release -o tools/StoryPlanner.AgentRunner/publish
```

Its log is `host-log.txt` beside the exe. Every batch verb takes a batch's `definition.md`; the
rules are the `agent-runner` skill's.

### The pocket reader

`tools/StoryPlanner.PocketReader` is a Blazor WebAssembly PWA — needs the `wasm-tools` workload;
`WasmBuildNative=true` links SQLitePCLRaw's `e_sqlite3` into the runtime. It opens `.storyplan`
files Brian picks on the device with real SQLite in WASM. Deployed from this public repo to
GitHub Pages (`.github/workflows/pocket-reader.yml`), which is safe because **the page carries no
story data**.

```
dotnet run --project tools/StoryPlanner.PocketReader            # local run
dotnet publish tools/StoryPlanner.PocketReader -c Release       # publish check
```

Read-only forever, never migrates. Pure tests for the draw and the migration gate live under
`tests/StoryPlanner.Tests/PocketReader/`.

## Other runnable tools

The corpus ingests — source texts, lineage, code sessions — are offline tools that write sidecar
databases rather than a `.storyplan`. Their commands and their rules live with their corpora: the
`corpora` skill for source texts and lineage, the `code-sessions` skill for code sessions.

## Traps

- **`StoryService` is not read-only.** `OpenProjectAsync` runs `MigrateAsync()`, which upgrades
  the schema in place — since 2026-08-02 it takes a `VACUUM INTO` snapshot into `Backups/` first
  and **refuses to migrate if the backup fails** — and silently no-ops if a project is already
  loaded. `SaveAsync()` is a bare `SaveChangesAsync`, and the app also saves on exit. The MCP
  server bypasses all of this with `Mode=ReadOnly`.
- **Copy a `.storyplan` only after the WPF app has closed.** In WAL mode the newest changes sit
  in `-wal` until then.
- **`stdout` is JSON-RPC in the MCP process.** Never `Console.WriteLine` from code the server can
  reach — see the `code-conventions` skill.
