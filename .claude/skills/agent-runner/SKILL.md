---
name: agent-runner
description: How to run autonomous agents through tools/StoryPlanner.AgentRunner — classifiers, investigators, auditors, referees, reading arms, document audits, any job that must run with explicit context and no transcript. What a well-formed job is (one item, enumerated by an instrument, a mechanically checked output, a pilot before a batch), the run-folder layout under fanout/, the job-file schema, the launch-folder invariants, the ledger as queue, prompt composition and hashes, the split verb, and the traps found in first use. Load before writing a job file, launching a batch, or citing a runner result in an artifact. Governs the instrument; the v3-buildout skill governs when a cell calls for it.
paths: "tools/StoryPlanner.AgentRunner/**, fanout/**"
---

# Agent runner

`tools/StoryPlanner.AgentRunner` launches `claude -p` children one at a time from a folder
**outside the repo**, each with its whole context passed explicitly — instructions, protocol
files and input files inlined and hashed — an exact toolset, no skills, no CLAUDE.md, no
memory, no MCP unless the job opts in, and no transcript persisted. It replaced
`AnalysisRunner` on 2026-09-03 after that tool's infinite retry of one failing job left 9,245
transcripts in the project history (`docs/v3-framework/methodology-revision-1.md`).

```
tools/StoryPlanner.AgentRunner/publish/StoryPlanner.AgentRunner.exe fanout/<work>/<run>/jobs.json [--dry-run] [--job ID]
tools/StoryPlanner.AgentRunner/publish/StoryPlanner.AgentRunner.exe split <document.md> fanout/<work>/<run>/items
```

**Run the published copy, not `dotnet run`** — the same pattern as the MCP server and the
app: `dotnet publish tools/StoryPlanner.AgentRunner -c Release -o tools/StoryPlanner.AgentRunner/publish`
(gitignored) after any runner change. A batch runs for minutes to hours with the exe
loaded, and `bin/Debug` is what `dotnet build`/`dotnet test` write to; separating them is what
lets the runner be changed and tested while a batch runs. Launch a long batch detached
(`Start-Process` with stdout redirected to `runner-log.txt` in the run folder), never under a
tool call with a timeout.

Always dry-run first: it composes every pending prompt (a missing input fails here, not
mid-batch), prints each job's state, model, MCP flag, attempts and item, and launches nothing.

Jobs run **`maxParallel` at a time** (file-level, default 1). Each child is its own process;
the runner's concurrency is async I/O over their stdout, nothing more. Five section jobs
that took eight minutes sequentially took two at four-wide on 2026-09-03; rate limits are
the subscription's either way, so parallelism buys wall-clock time, not capacity.

## A well-formed job

The runner is a fan-out instrument: many small jobs, each judging one thing. Four rules,
each learned on 2026-09-03 when the first real job broke all of them at once (a single agent
handed nine documents and told to enumerate 174 units itself; killed after 39 minutes with
nothing written):

1. **One job, one item.** The `item` field names the single thing the agent judges, in a
   line. A job whose item cannot be named in a line is not a job yet — it is a corpus, and
   corpus-scale work is split into items or read by a HITL session.
2. **Enumeration is an instrument.** Whatever produces the set of items — a splitter, a
   query, a manifest — runs once at design time, writes files, and is hashed like any input.
   A protocol never tells the agent what the items are; a rule the agent applies is judgment
   at runtime, however precisely it is worded. `split` is the splitter for Markdown documents.
3. **The output contract is mechanical.** `requireOnce` lists markers (item ids, row keys)
   the output must contain exactly once; the runner checks them and records a failed attempt
   on a miss or a duplicate. No human reads a batch to find a skipped item.
4. **No batch without a pilot.** A new protocol or codebook runs on a handful of items first,
   with the outputs read by a person, before the batch. Calibration (a scored sample with
   Brian's verdicts) is the pilot for a codebook; a protocol that produces no evidence still
   gets a pilot. Nothing is exempt.

And two conventions: job files are generated from the manifest, not typed (a batch that was
hand-written has not been examined); arms are named neutrally (`arm-A`, `arm-B`), because
the job id appears in the prompt header and a name that carries the condition tells the
agent which arm it is.

## Layout: `fanout/`, vertical by work

The run folder is the folder holding `jobs.json`. Every path in the job file is relative to
it; the runner writes `attempts/<id>/attempt-N/` (`prompt.md`, `stream.jsonl`) and
`ledger.jsonl` beside it. One folder is one run, and runs group by the work that owns them:

```
fanout/referee/                       codebook.md, calibration record, runs
fanout/WU<n>.<m>-<slug>/              a verification WU's codebooks, items, jobs, results, candidates
fanout/skill-audits/protocol.md       a document supersession audit's protocol
fanout/skill-audits/<date>-<doc>/     one audit: items/, jobs.json, results/, attempts/, ledger.jsonl
fanout/smoke-test/                    the harness check
```

No shared `codebooks/` or `protocols/` folder: an instrument lives with the work that
authors and calibrates it. The referee is one work — every verification WU's candidates go
through the same codebook under the same hash — so its folder holds every referee run. A
corpus codebook belongs to the verification WU that wrote it; later rounds on that corpus are
the same WU. What a person writes afterwards (a WU's synthesis, an audit's adjudication) is a
document in `docs/` that cites the run by ledger row.

**What a run commits** — one convention, no per-experiment `.gitignore` edits:

- `attempts/` is the runner's and is never committed: `prompt.md` is ~85 KB per attempt and
  reconstructible from the ledger's input hashes plus the committed inputs; `stream.jsonl`
  is for watching a live run and grows with whatever the agent does.
- `items/` holds **regenerable** inputs — a splitter's units, a query's rows — and only its
  `manifest.md` is committed; the bodies are re-made from the committed source and the tool.
- Everything else a work places under a run is committed: `jobs.json`, `results/`,
  `ledger.jsonl`, the protocol or codebook, the generator and tallier scripts.
- An input that is **not** regenerable — a source excerpt fetched from a database for the
  referee, a passage copied from a story text — is written into the run folder outside
  `items/` so it is committed; with `prompt.md` ignored, the run folder is the only place
  it exists.

**What a work ships beside its protocol:** an enumerator (its own tool unless `split`
already fits — an enumerator is the work's, and lands in the runner only when it is generic
over a format, as `split` is over Markdown), a generator that turns the manifest into
`jobs.json`, and a **tallier** that aggregates `results/` mechanically, so adjudication reads
counts and flagged rows and never the raw batch. Batch size follows the shared-context cost:
when every job carries the same large context, items are batched per job to amortize it
(prompts were 75–90K characters whether a section held one unit or 33); items that stand
alone go one per job.

**The routine sequence:** enumerate → generate → dry run → pilot one job with `--job` and
read its output → batch. The pilot took 45 seconds on 2026-09-03; the batch it cleared took
an hour of compute.

## Invariants the runner enforces (and why)

- **`launchDir` is outside the repo.** Claude Code keys transcripts on the launch directory
  and discovers CLAUDE.md and skills by walking up from it. The runner refuses a `launchDir`
  at or under the repo root, and refuses one containing `CLAUDE.md`, `.claude/`, or
  `.mcp.json`. Brian's is `C:\Users\Brian\RiderProjects\StoryPlanner-fanout`; nothing but its
  README lives there, and its transcript directory is never on the codesessions ingest
  include-list.
- **A job runs at most `maxAttempts` times, then is FAILED and never relaunched.** The
  ledger is the queue: a job is pending until an attempt has exit code 0, the output file
  present, and every `requireOnce` marker present once. Exit 0 with no output, or with a
  failed marker check, is a failed attempt.
- **An attempt past `timeoutMinutes` is killed** (whole process tree) and recorded failed.
  Default 20 minutes; a classifier-tier job that needs more is mis-sized, not slow.
- **Every attempt is recorded**, with the prompt hash, a hash per protocol and input file,
  model, harness version, cost, turns, session id and the output check. If a result cannot
  be cited by these, it was not produced by the runner.

## The job file

```json
{
  "launchDir": "C:\\Users\\Brian\\RiderProjects\\StoryPlanner-fanout",
  "maxAttempts": 2, "utilizationCap": 80, "timeoutMinutes": 20,
  "mcpConfig": "../../../tools/StoryPlanner.AgentRunner/configs/storyplanner-mcp.json",
  "defaults": { "model": "sonnet", "tools": ["Write"], "allowedTools": ["Write"], "mcp": false },
  "jobs": [
    {
      "id": "unit-014",
      "item": "unit-014 of v3-buildout/SKILL.md against the v3-buildout set",
      "instructions": "Apply the protocol to the item; write the result to the output path.",
      "protocolFiles": ["../protocol.md"],
      "inputFiles":    ["items/unit-014.md", "../../../.claude/skills/v3-buildout/SKILL.md"],
      "outputPath":    "results/unit-014.md",
      "requireOnce":   ["unit-014"]
    }
  ]
}
```

| Field | Meaning | Built-in |
|---|---|---|
| `id` | Ledger key. A new run of "the same" job is a **new id** — a succeeded id never launches again. | required |
| `item` | The one thing this job judges, in a line. Goes into the prompt header. | required |
| `model` | Passed to `--model`. Confirm what the CLI accepts before a batch. | `sonnet` |
| `effort` | Passed to `--effort` when set. | unset |
| `instructions` / `instructionsFile` | The task, in the agent's own terms. One is required. | — |
| `protocolFiles` | Inlined under `## Protocol: <name> (sha256 …)`; each hash goes in the ledger. | `[]` |
| `inputFiles` | Inlined under `## Input: <name> (sha256 …)`. Order is meaning: the instructions or protocol say what the first input is. | `[]` |
| `outputPath` | Where the agent writes. The runner creates the directory and grants it via `--add-dir`. | required |
| `requireOnce` | Markers that must each appear exactly once in the output; checked by the runner. | `[]` |
| `timeoutMinutes` | Per-attempt wall-clock cap. Job overrides default overrides file. | `20` |
| `maxParallel` | File-level: children in flight at once. | `1` |
| `tools` | The **exact available** toolset (`--tools`). `[]` disables all tools. | `["Read","Write"]` |
| `allowedTools` | Pre-approved tools. Pre-approval is not restriction — `tools` restricts. | same as `tools` |
| `addDirs` | Extra directories the file tools may touch. | `[]` |
| `mcp` | `true` attaches the servers in the file-level `mcpConfig`; `--strict-mcp-config` is always passed. | `false` |
| `permissionMode` | `--permission-mode`. | `auto` |

Relative paths resolve against the run folder. Flags the runner always passes: `-p`,
`--no-session-persistence`, `--output-format stream-json --verbose`, `--restricted` (no
code-running tools, no WebFetch, user/project/local settings ignored, file tools confined to
the working directories and `--add-dir`), `--disable-slash-commands`, `--strict-mcp-config`.

## Seeing inside a running session

The child emits one JSON event per line as it happens — its text, each tool call with its
input, each tool result, the final result with cost — and the runner tees them to
`attempts/<id>/attempt-N/stream.jsonl` as they arrive. Open it in Rider, tail it, or point
the Monitor tool at it: that is the inside of the session, live, for every job in flight.
Observation only — print mode takes no input after the prompt, so a job that goes wrong is
killed and re-run under a new id, never steered. Extended thinking is not in the stream.
The ledger row's `ResultPath` is this file; `ParseResultSummary` reads the totals from its
last `result` event. There is no status or control file: Ctrl+C kills every child and
exits, and a head, when one is wanted, hosts the loop rather than steering it through
files.

## What the agent sees

One document on stdin: `# Job: <id>`, `Item: <item>`, the instructions, an **Output**
contract naming the path and the required markers, then each protocol and each input under
its hashed heading. Nothing else — no system prompt of ours, no repo. Write the instructions
and the protocol as if to a reader with no memory of this project, because that is the
reader. Two inputs with the same file name get distinct headings (`SKILL.md`,
`v3-buildout/SKILL.md`); prefer distinct names when you control them.

Prompt hashes are deterministic for identical inputs, so a re-run under the same protocol
version is comparable and a codebook revision is visible as a new hash.

## The split verb

`split <document.md> <items-dir>` applies the unit rule to a Markdown document and writes
`unit-NNN.md` (locus, id, text verbatim) plus `manifest.md` into an empty directory. The
rule: a paragraph is a run of non-blank lines ended by a blank line or heading; every list
item, nested ones included, is its own unit; a fenced code block belongs to the unit before
it; a table is one unit per body row; headings are not units; the frontmatter block is one
unit. The rule is pinned by `UnitSplitterTests`; changing it changes the count, never the
items silently. A run splits once — to split again, start a new run folder.

## Citing a result

An artifact that rests on runner output cites, from the ledger row: job id, model, harness
version, prompt hash, and each protocol hash (the codebook version). `stream.jsonl` in the
attempt folder holds the full event stream (init tools and MCP servers, the reply, cost);
`prompt.md` beside it is exactly what the agent received (gitignored; reconstructible).

## Traps found in first use (2026-09-03)

- **A corpus-scale single job never finishes.** Nine documents inlined and 174 rows to
  produce: 39 minutes, no output, killed by hand — there was no timeout then. Now: the four
  rules above, and `timeoutMinutes`.
- **The output directory must exist before launch** — `--restricted` confines writes to
  `--add-dir` paths and a nonexistent path is silently dropped. The runner creates it.
- **`allowedTools` does not restrict.** The smoke test's agent listed Bash, WebSearch and
  Agent among its tools while "allowed" only Write. Restriction is `tools` + `--restricted`.
- **Two inputs with the same file name** collided: identical headings for the agent, one
  hash overwriting the other in the ledger. Labels are now disambiguated by parent folder.
- **A succeeded job never relaunches.** To run it again, give it a new id.
- **The child's output has had three shapes** — a `json` array of events, a single result
  object, and now `stream-json` lines. `RunnerPlan.ParseResultSummary` reads all three;
  anything else reading an attempt must too (the smoke test's attempt is the old array form).
- **A running exe blocks the build.** The first batch ran from `bin/Debug`, so every runner
  change waited for it. Hence the publish copy.
- **The utilization figure is a cache**, read from what Claude Code last wrote to
  `~/.claude.json`, and can be stale either way. The cap is a courtesy; `maxAttempts` and
  the timeout are the guards.
- **The utilization cap gated only an idle runner.** With `maxParallel` above 1 the first
  parallel loop consulted the cap only when nothing was in flight, so an over-cap batch kept
  launching while jobs ran. Now every launch checks it; the runner sleeps only when idle.
- **The fanout launch folder gets an empty `~/.claude/projects/<fanout>/memory/` directory**
  on first launch. No transcript is written; the directory is inert.

## Two mechanisms, one line between them

Work that is not the main session runs one of two ways (settled 2026-09-03; the Workflow
tool is not used — it adds nothing over the two and costs an opt-in per run — and the Agent
SDK was dropped as API-billed and outside the toolchain):

| Mechanism | Runs where | Inherits | Transcript | Serves |
|---|---|---|---|---|
| **The runner** (this skill) | Outside the repo, `claude -p` per job | Nothing but the job's protocol and inputs | None | Every autonomous cell — frozen-predicate and method-discretion work (classifier, investigator, auditor, referee, census-by-LLM), every arm of a controlled experiment, every batch, anything whose result must be cited by hash |
| **The Agent tool** | **Only inside a HITL session**, spawned by the person-facing session itself | The full instruction stack, MCP servers, memory | Written as a subagent of the interactive session — and kept by the codesessions archive as part of it | Salience-discretion help to the session in ones and twos, where inheriting the stack is acceptable and the transcript *belongs* in the archive: an Explore search, a fresh-eyes read of something under discussion, a slice reader spawned mid-conversation |

Two of Brian's rulings drew the line: explicit context (a classifier or referee must see
"protocol + item, nothing else", which an in-session subagent cannot) and codesessions as
prevention (hundreds of subagent transcripts under the repo's project directory would enter
the archive by construction). So the Agent tool is never a batch mechanism, never an arm,
and never a substitute for the runner on a cell that calls for explicit context; a runner
child cannot spawn one; and the runner is never a substitute for a HITL session on a cell
that calls for judgment.

## Never

Launch an autonomous cell from a repo cwd, or through the Agent tool of a HITL session,
when the work calls for explicit context — the `v3-buildout` skill says which cells do.
Hand an agent the enumeration of its own items. Run a batch under a protocol nobody has
piloted. Use the Workflow tool for buildout work; if a need for it ever appears, that is a
methodology revision, not a job. Put a `.mcp.json` or `CLAUDE.md` in the launch folder.
Edit a ledger. Re-run a batch to "get a better answer" — a new attempt is a new id and a
recorded decision.

## Verifying a change to the runner

`dotnet test tests/StoryPlanner.Tests --filter "FullyQualifiedName~Runner"` covers
resolution and relative paths, the queue, the output check, prompt composition and labels,
arguments, result parsing and the unit rule. Then publish, and run `fanout/smoke-test/jobs.json`
under a new job id from the published exe: the first run writes `OK`, its `stream.jsonl`
init event lists only the configured tools and no `mcp_servers`, and no file appears under
the StoryPlanner transcript directory; the second run launches nothing.
