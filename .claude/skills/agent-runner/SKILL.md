---
name: agent-runner
description: How to run autonomous agents through tools/StoryPlanner.AgentRunner — classifiers, auditors, referees, readers of one item, calibration samples, any call that must run with explicit context and no transcript. The persistent host and its page (http://127.0.0.1:5190), the call (the directions body as the system prompt, one item on stdin, the answer as JSON the CLI enforces), the batch folder and its files (definition, index, items, calls, results, tally), the five verbs (dry-run-batch, execute-batch, tally-batch, start, stop), the launch-folder invariants, what a batch commits, the JSON routes, harness control versus what a call is, how a result and a call are cited, and the traps found in use. Load before writing a definition, executing a batch, or citing a result. Governs the runner; the v3-buildout skill's process tables say which rows run here.
paths: "tools/StoryPlanner.AgentRunner/**, tools/StoryPlanner.BatchFiles/**, tools/StoryPlanner.MarkdownItemizer/**, docs/v3-framework/studies/**/batches/**"
---

# Agent runner

`tools/StoryPlanner.AgentRunner` makes `claude -p` calls from a folder **outside the
repo**, one call per item of a batch, each with exactly two inputs — the batch's directions
as its system prompt and one item as its message, both hashed — an exact toolset, no skills,
no CLAUDE.md, no memory, no MCP unless the batch's definition opts in, and no transcript
persisted. The answer comes back as JSON the CLI validates against a schema derived from the
directions, and the runner writes it as the result; the model writes no file and is told no
id. It is a **persistent host**: one process owns the page's port and runs any number of
batches, up to a global parallel ceiling, under one utilization cap and one idle limit; the
CLI hands it a batch and returns. It replaced `AnalysisRunner` on 2026-09-03 after that
tool's infinite retry of one failing call left 9,245 transcripts in the project history
(`docs/v3-framework/methodology-revision-1.md`), and was rebuilt around the batch files on
2026-09-09 (decisions.md, the entries of 2026-09-08 and 2026-09-09 on the runner).

```
AgentRunner.exe start                                     start the host if none answers; open http://127.0.0.1:5190
AgentRunner.exe stop [--now]                              stop the host after in-flight calls (--now: kill them)
AgentRunner.exe dry-run-batch <definition.md> [--item ID] compose every call in memory, write nothing (serverless)
AgentRunner.exe execute-batch <definition.md> [--item ID] [--at HH:mm|ISO|reset]
                                                          one call per item without a result; --item names the pilot
AgentRunner.exe tally-batch   <definition.md> [--group-by item|locator|description]
                                                          print tally.md, writing it first if the host never did
                                                          (serverless); --group-by prints a cross-tab view, writes nothing
```

`AgentRunner.exe` is `tools/StoryPlanner.AgentRunner/publish/StoryPlanner.AgentRunner.exe`.
**Run the published copy, not `dotnet run`** — the same pattern as the MCP server and the
app: `dotnet publish tools/StoryPlanner.AgentRunner -c Release -o tools/StoryPlanner.AgentRunner/publish`
(gitignored). The host holds that exe loaded for as long as it lives, so a republish needs
`AgentRunner.exe stop` first; `bin/Debug` stays free for `dotnet build`/`dotnet test`
throughout. An execute returns at once, so a Claude Code session can run it under an
ordinary tool call. **Every batch verb takes the path of a batch's `definition.md`** and
resolves the batch's other files beside it; the runner holds no root, no folder rule and no
notion of a study. Where a definition sits and what it says are the `v3-buildout` skill's
Artifacts row and `schemas/definition-schema.md`, held by DocIntegrity at the write; the
runner never re-checks them.

**This skill holds the rules of the runner and never the order of a batch.** What precedes
and follows a batch — the question, the directions and their calibration, the itemizer, the
tally, the referee, the promotion — is derived from the `v3-buildout` skill's process tables
and rendered into its `map.md`; each activity file names the process that invokes the
runner and reads this skill in full for it. The host's `/map` route renders that `map.md`
(the `v3-buildout` folder's, falling back to `v3-buildout-2`'s until the router swap, or
`mapPath` in `host.json`), diagrams folded as mermaid source.

## The call

One execution of the CLI in print mode, with exactly these inputs:

- **the directions body** as the system prompt, in place of Claude Code's own — everything
  after the frontmatter's closing `---` of the file the definition's `directions` line names,
  written to the call's folder and passed by `--system-prompt-file`; its SHA-256 (UTF-8, LF
  line endings, leading and trailing blank lines dropped) is the version every call cites;
- **the item's text** as the user message, on stdin, verbatim from `items/<item>.md`; its
  SHA-256 is the item hash;
- **the model and effort** from the definition, on every call of the batch alike;
- **the JSON Schema** the CLI enforces on the answer (`--json-schema`), derived from the
  directions' § What to produce and § Classes: one property per declared field, an `enum`
  field closed over the class labels, a `line` a string without newline, a `block` a string,
  a `list of line` an array of such strings; every field required, no other allowed. It is
  written nowhere.
- **no tools** (`--tools ""`) unless the definition's `tools` line names some, then exactly
  those and `--allowed-tools` the same; `--restricted`, `--disable-slash-commands`,
  `--strict-mcp-config` always; `--mcp-config` only when the definition's `mcp` line names a
  config; `--no-session-persistence`; `--output-format stream-json --verbose
  --include-partial-messages` so the host can tee each event as it happens, the answer's own
  deltas included (see the idle limit below).

Whatever a study needs the agent to hold beyond the item — a whole skill folder for an
audit, an excerpt for a reader — is in the directions body or in the item, put there by the
itemizer; a call has no third input. The prompt hash is the SHA-256 of the body, a line of
three hyphens, and the item text: deterministic for identical inputs, so a repeat under the
same version is comparable.

**Capture.** The result event's `structured_output` is the answer; the runner renders it as
`results/<item>.md` by the one mapping below and keeps the event stream under `attempts/`.
A call with exit 0 and an answer that parses back clean is `ok`; exit 0 with no structured
output, a non-zero exit, a kill for an idle stream (`idle`), a cancel (`cancelled`), or an
answer that does not read back as declared (`malformed: …`) is a failed call, recorded like
any other. **No automatic retry**: the runner never calls an item twice in one execution and
never on its own; a failed item is called again by the next `execute-batch`.

## The batch folder

`docs/v3-framework/studies/<study>/batches/<nn>-<slug>/`, where `<nn>` is sequential within
the study from 01 and the slug names what the batch does. The referee's batches sit under
the verification they judge, their definitions naming the referee's directions by relative
path into `docs/v3-framework/referee/`. Every batch verb resolves these beside the definition:

```
batches/03-scene-notes/
  definition.md      authored by a session before any execution, never edited after the first
                     (schemas/definition-schema.md): directions by path, kind, calibration, model,
                     effort, tools, mcp
  index.md           written by the itemizer, one row per item with its locator
                     (schemas/index-schema.md); the runner calls the items in its order
  items/<item>.md    the item bodies, uncommitted and regenerable by the itemizer; each call
                     hashes the body it received
  calls.md           written by the runner: the definition's hash at its head, then one entry
                     per call (below), appended and never edited
  results/<item>.md  the model's answer as the runner rendered it, one file per item, frozen
  tally.md           written once by the host when the last item has a successful call; by
                     tally-batch only when the host never did
  attempts/<item>/call-<n>/   system-prompt.md, item.md, stream.jsonl — local only
```

**What a batch commits**: `definition.md`, `index.md`, `calls.md`, `results/`, `tally.md`.
`items/` is regenerable from the committed index and the itemizer, and `attempts/` is the
runner's — the stream is for watching a live call and reconstructible in what it proves from
the hashes in `calls.md`; both are ignored by one pattern in `.gitignore`. An input that is
not regenerable does not exist: the itemizer puts everything the call needs into the item.

**A result** is `results/<item>.md`: the directions' What to produce fields in that order and
nothing else, an enum or line as `- key: value`, a block as `- key: first line` with the rest
on lines indented two spaces, a list of line as `- key:` then one `  - line` per item. The
runner renders it from the answer's JSON and the tally and DocIntegrity read it back through
the same code (`tools/StoryPlanner.BatchFiles`), the render and the parse each other's
inverse under one test; the model's JSON is never read back. Two tools hold a result: the
tally reports one that does not match the declaration as malformed, and `check .` follows
every batch's definition to its directions and validates every result file, check
`results.declared`, so a bad or edited result is caught at any check and at the hook on a
write into `results/`.

**A call's entry** in `calls.md` is `### <item> — call <n>`, `<n>` the execution's number,
then keyed lines: `model`, `effort`, `harness` (the CLI's version), `directions hash`, `item
hash`, `prompt hash`, `started`, `ended`, `exit`, `check` (`ok` or the failure), `cost`,
`turns`, `session`, `pilot` (`yes` for an execution naming one item). An item with an entry
whose exit is 0 and check is `ok` has answered; every other item is called by the next
execution. The head's `- definition: <hash>` is what `definition.frozen` holds the file to.

**Queries** a reader asks of the runner's files:

| question | how |
|---|---|
| which items have answered | `grep -n '^- check: ok' calls.md` and the `### ` line above each; or the batch's page |
| an item's result | `results/<item>.md`, cited as `<study>/<batch>/<item>` |
| which call produced a result | the item's last entry in `calls.md` with `check: ok`; its `directions hash` is the version the result was judged under |
| the directions version a batch ran under | the definition's `directions` line; the body hash in any entry of `calls.md` |
| every result of one class | `grep -l '^- <field>: <value>' results/*.md`; the count is in `tally.md` § `<field>` |
| the classes per story, section or subject | `tally-batch <definition.md> --group-by locator` (or `description`, `item`): a printed cross-tab, never written |
| the malformed and the missing | `tally.md` § Malformed and § Missing; `check .` for `results.declared` |
| what was watched inside a call | `attempts/<item>/call-<n>/stream.jsonl`, the events the page shows |
| the cost of a batch | `tally.md`'s `cost` line, or the page |

## The host and its page

The host is a console process serving Blazor Server on `http://127.0.0.1:5190`
(`configs/host.json`: `port`, `bind`, `token` — the last two exist for a LAN follow-up and
are inert on localhost — `launchDir`, `maxParallel`, `utilizationCap`, `idleMinutes`,
`mapPath`). `start` starts it detached when none answers `/api/ping` and opens the browser;
the host's working directory is wherever `start` was run, and its page lists every batch
whose `definition.md` it finds under a `batches/` folder beneath that directory, live and
finished alike, one view. Closing the host is `AgentRunner.exe stop`. The ceilings and the
idle limit are read from the file at start; a change on the page lasts the host's life and
is logged, never written back, so a different default is an edit to `host.json`. The log is
`host-log.txt` beside the exe, the tool's own; no artifact reads it.

**A batch is its index, not an execution.** The batch's page and `/api/batches/<id>` describe
the union of the index and the calls file: pending is an item with no successful call, the
batch is executed when every item has one, and a pilot (`--item`) leaves the rest pending.
An execute answers with what it will do — items to call, skipped as answered — never a bare
count. The host writes `tally.md` the moment the last item has a successful call, its
sections fixed by the directions: counts per enum field, the malformed, the missing, the
fields not counted. A written `tally.md` is frozen; `tally-batch` prints it, and writes it
only when the host never did (a host that died first, a batch executed serverless). A pilot
writes none, since items remain. `--group-by` is a view for the analysis and the review:
printed as often as wanted, written nowhere.

**The stage strip** on a batch's page is detected from the folder alone: defined (the
definition reads), itemized (an index with rows and every body present), piloted (a call
marked pilot), executed (every item answered), tallied (`tally.md` present). Nothing is
judged; a missing stage is a fact about the folder.

**Ceilings and the idle limit.** `maxParallel` and `utilizationCap` apply across every batch;
a batch has no ceiling of its own. The cap gates every launch, so two batches cannot jointly
exceed it; over the cap with children running, the host waits for them; over the cap while
idle, it waits for the reset and the page says so. The utilization figure is what Claude Code
last cached in `~/.claude.json` — not a live query — and the page shows how old it is.
`idleMinutes` is the one limit on a call: a child whose stream has been silent that long is
killed, whole process tree, and its call recorded `idle`. There is no absolute time limit — a
long read that keeps streaming is long, not stuck. Silence means silence because the call
asks for partial messages: without them the `thinking_tokens` lines stop when thinking ends
and the whole answer arrives as one line minutes later (a 2026-09-09 probe measured 112
seconds of nothing for a 9,800-token answer, so a long enough answer under a slow enough
model would have been killed mid-write); with them the harness writes a delta every few
tokens for as long as the model writes.

**Harness control, never experiment control.** The page and its routes offer: pause and
resume launching, stop after in-flight, cancel one running call (recorded `cancelled`, exit
-4), the host's ceiling, the cap, the idle limit. Nothing changes a batch's model, directions
or items, and nothing calls an item that has answered — those are a new batch, since a
definition is never edited. Knob changes go to the log with timestamps.

**Scheduling.** `--at` holds an execution until a time: `--at 04:00` (the next such clock
time), an ISO date-time, or `--at reset` (the cached five-hour reset plus a minute; refused
if there is no cache or the reset has passed). A scheduled execution shows on the page as
"scheduled HH:mm" with one control, **unschedule**; pause/stop/cancel apply once it starts,
and a second execute while it waits is refused. Scheduled intent lives in the host, not a
file: a host that dies overnight shows the batch as never executed, which is the truth. **The
overnight recipe:** set the cap, execute with `--at reset`, check the page in the morning.
The machine must not sleep; that is a Windows power setting, not the runner's.

**The usage bar** sits on every page, from the layout: the five-hour window as a meter with
the cap marked, the seven-day window, each reset as a clock time and a countdown, when the
cache was fetched and how old it is, "stale" after an hour, and a lock reason if the
account is locked. All of it is `~/.claude.json`'s word.

**The same state as JSON** — for a terminal or a Claude Code session; batch ids contain
slashes, so batch-addressed routes take the id as a catch-all or a query value:

```
GET  /api/ping                              is a host up; its working and launch directories
GET  /api/host                              ceilings, idle limit, in flight, utilization (+ staleness), every batch's summary
GET  /api/batches                           every batch's summary
GET  /api/batches/<id>                      one batch: items with state/calls/exit/check/cost, stages
GET  /api/stream?batch=<id>&item=<item>&tail=N   the last N events of the item's latest call
POST /api/batches         {"path": "<abs definition.md>", "item": "<id>"?, "notBefore": "<ISO>"?}   execute (or schedule)
POST /api/batch-control   {"batch": "<id>", "action": "pause|resume|stop|cancel|unschedule", "item"?}
PUT  /api/host/settings   {"maxParallel"?, "utilizationCap"?, "idleMinutes"?}
POST /api/host/shutdown?now=false
```

A batch id is its folder relative to the host's working directory, forward slashes. Routes
serve live state. **Citing a result stays file-based**: an artifact cites
`<study>/<batch>/<item>` and the call's directions hash, never a URL.

## Seeing inside a running call

The child emits one JSON event per line as it happens — its text, each tool call with its
input, each tool result, the final result with cost and the structured answer — and the host
tees them to `attempts/<item>/call-<n>/stream.jsonl` as they arrive. The batch's page shows
the selected item's stream parsed (init, text, tool, result, thinking, usage, system, done)
with a raw toggle, refreshed twice a second while the call runs; the same reader serves a
finished call. Observation only — print mode takes no input after the message, so a call
that goes wrong is cancelled and called again by the next execution, never steered. Extended
thinking's text is not in the stream; since harness 2.1.258 its token count is, one
`system/thinking_tokens` event per step, which the page collapses to one running line. The
partial-message deltas (`stream_event`, one per few tokens of thinking, text or the answer's
JSON) collapse the same way, to one `writing` line naming what was written and the delta
count. A `rate_limit_event` is the harness's own reading of the two usage windows at that
moment — the one live figure the usage bar cannot get.

## Invariants the runner enforces (and why)

- **`launchDir` is outside the repo.** Claude Code keys transcripts on the launch directory
  and discovers CLAUDE.md and skills by walking up from it. The host refuses a `launchDir`
  at or under the definition's repository, and one containing `CLAUDE.md`, `.claude/`, or
  `.mcp.json`. Brian's is `C:\Users\Brian\RiderProjects\StoryPlanner-fanout`, the default
  when `host.json` names none; nothing but its README lives there, and its transcript
  directory is never on the codesessions ingest include-list.
- **One call per item per execution, never a retry on its own.** The calls file is the
  batch's state: an item is answered by an entry with exit 0 and check `ok`, and every other
  item is called by the next execution, once. Nothing the runner does relaunches an item by
  itself, which is the guard against the runaway of 2026-08-27 in its one remaining form.
- **A call is killed only for silence.** The idle limit is the host's; no call has an absolute
  time limit.
- **Every call is recorded**, with the directions hash, the item hash, the prompt hash, model,
  effort, harness version, cost, turns, session id, the check and the pilot mark. If a result
  cannot be cited by these, it was not produced by the runner.
- **Nothing is written to the definition or the index**, ever; the calls file is appended;
  the results are written by the render and by nothing else.

## Dry run before every execution

`dry-run-batch` reads the definition, resolves the directions and the index, checks every
item body is present, checks the launch folder, composes every call in memory — the system
prompt, the item, the schema, the three hashes — and prints what an execution would call and
skip, then writes nothing. A mis-sized item or a wrong directions path is seen before it
costs anything. Then, for directions nobody has piloted, `execute-batch --item <id>` calls
one item, whose result a person reads before the rest run; a calibration batch is a
verification's pilot, and a one-item batch is its own.

## Cost figures are API-equivalent estimates

`total_cost_usd` in the result event is what Claude Code computes from token counts at API
list prices, cache reads included; on a Max subscription nothing is billed per token — the
usage windows on the bar are what the subscription meters. The figure is the best
proportional measure of a call's work, and the page labels it "api-equiv $" so it is never
read as a bill.

## Traps found in use

- **A corpus-scale single call never finishes** (2026-09-03: nine documents inlined and 174
  rows to produce; 39 minutes, no output, killed by hand). Now the item is the unit, one
  call judges one, and the itemizer cuts the corpus before any call.
- **A running exe blocks the build.** The first batch ran from `bin/Debug`, so every runner
  change waited for it. Hence the publish copy; and since the host lives on, `stop` before
  a republish.
- **The utilization cap gated only an idle runner.** The first parallel loop consulted the
  cap only when nothing was in flight. Now every launch checks it, host-wide.
- **`allowedTools` does not restrict.** The 2026-09-03 smoke test's agent listed Bash,
  WebSearch and Agent among its tools while "allowed" only Write. Restriction is `--tools` +
  `--restricted`; a batch with no `tools` line passes `--tools ""` and the agent has none.
- **A system prompt on the command line is capped** by Windows at about 32,000 characters; the
  body goes by `--system-prompt-file`, and an audit's directions carrying a whole skill folder
  fit.
- **The child's output has had four shapes** — a `json` array of events, a single result
  object, `stream-json` lines, and (harness 2.1.258) `stream-json` lines whose `system`
  events carry subtypes beyond `init`, with `rate_limit_event` lines after init and the
  result object's `type` key late rather than first. `StreamEvents.ParseResult` reads all
  four and takes the answer from `structured_output`, or from the reply text when it parses
  as a JSON object; anything else reading a stream must too.
- **The utilization figure is a cache**, and can be stale either way. The cap is a courtesy;
  the idle limit and one-call-per-execution are the guards.
- **The launch folder gets an empty `~/.claude/projects/<launch>/memory/` directory** on
  first launch. No transcript is written; the directory is inert.

## Two mechanisms, one line between them

Work that is not the main session runs one of two ways (settled 2026-09-03; the Workflow
tool is not used — it adds nothing over the two and costs an opt-in per execution — and the
Agent SDK was dropped as API-billed and outside the toolchain):

| Mechanism | Runs where | Inherits | Transcript | Serves |
|---|---|---|---|---|
| **The runner** (this skill) | Outside the repo, `claude -p` per item, under the host | Nothing but the directions and the item | None | Every `agent` process of the `v3-buildout` skill — a classifier or auditor, the referee, a reader of one item under exploration directions, a calibration sample — every batch, anything whose result must be cited by hash |
| **The Agent tool** | **Only inside a HITL session**, spawned by the person-facing session itself | The full instruction stack, MCP servers, memory | Written as a subagent of the interactive session — and kept by the codesessions archive as part of it | Salience-discretion help to the session in ones and twos, where inheriting the stack is acceptable and the transcript *belongs* in the archive: an Explore search, a fresh-eyes read of something under discussion |

Two of Brian's rulings drew the line: explicit context (a classifier or referee must see the
directions and the item, nothing else, which an in-session subagent cannot) and codesessions
as prevention (hundreds of subagent transcripts under the repo's project directory would enter
the archive by construction). So the Agent tool is never a batch mechanism and never a
substitute for the runner on a process whose mode is `agent`; a runner child cannot spawn
one; and the runner is never a substitute for a session on a process whose mode is `hitl` or
`session`.

## Never

Launch an `agent` process from a repo cwd, or through the Agent tool of a HITL session —
the `v3-buildout` skill's process tables say which rows run here. Hand an agent the
enumeration of its own items, its id, or an output path. Execute a full batch under
directions nobody has calibrated or piloted. Add a control that changes what a call is. Edit
a definition, an index, a calls file or a result. Use the Workflow tool for buildout work; if
a need for it ever appears, that is a methodology revision, not a batch. Put a `.mcp.json`
or `CLAUDE.md` in the launch folder. Execute a batch again "to get a better answer" — an item
that has answered is never called again, and a different answer wants a new batch.

## Verifying a change to the runner

`dotnet test tests/StoryPlanner.Tests --filter "FullyQualifiedName~Batch|FullyQualifiedName~Runner|FullyQualifiedName~Tally|FullyQualifiedName~Itemizer|FullyQualifiedName~Stream|FullyQualifiedName~Head"`
covers the batch files, the loop under a fake launcher (one call per item, the render, the
calls file, the pilot, a later execution, the gate, pause, stop, cancel), the catalog and
stages, the tally, the itemizer's unit rule, the stream reader, the JSON routes over a
loopback listener, and the leaf components under bUnit. The round trip through the real CLI
is `SmokeTest`, one item in a temporary folder, run with `STORYPLAN_RUNNER_SMOKE=1` set; it
spends a call and proves the system prompt, the stdin item, the enforced schema and the
rendered result. Then `AgentRunner.exe stop`, publish, `AgentRunner.exe start` from the repo
root (the page opens with every batch beneath it), and `curl http://127.0.0.1:5190/api/ping`
shows the working and launch directories.
