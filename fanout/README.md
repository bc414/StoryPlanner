# fanout — everything the agent runner takes in and puts out, grouped by work

One folder per piece of work that runs autonomous agents through
`tools/StoryPlanner.AgentRunner`. A work folder holds the whole apparatus for that work and
nothing else: the protocol or codebook it runs under, the calibration record, the items, the
job file, the agents' results, the attempt records and the ledger. Vertical, by work — never
a shared `protocols/` or `codebooks/` folder across works. What a person writes afterwards
(a WU's synthesis, an audit's adjudication) is a document and lives in `docs/`, citing the
run here by ledger row.

```
fanout/
  referee/                          the referee: codebook.md, calibration record, runs
  WU<n>.<m>-<slug>/                 a verification WU: codebooks, items, jobs, results, candidates
  skill-audits/                     document supersession audits: protocol.md, one run per audit
    2026-09-03-v3-buildout/
      jobs.json                     the run — the runner writes beside it
      items/                        the enumerated units + manifest.md (from `split`)
      results/                      one agent output per job
      attempts/<job-id>/attempt-N/  prompt.md (exactly what the agent received), result.json
      ledger.jsonl                  one row per attempt: hashes, model, harness, cost, turns
  smoke-test/                       the harness check
```

Rules, all in the `agent-runner` skill: the run folder is the job file's folder; every path
in a job file is relative to it; the child process runs from `RiderProjects\StoryPlanner-fanout`
(outside the repo, so no instruction stack reaches it) and writes back here by absolute path.
Committed: jobs, items, results, the ledger and each attempt's result stream — the
provenance an artifact cites. Ignored (`.gitignore`): each attempt's `prompt.md`, which is
~85 KB and reconstructible from the ledger's input hashes plus the committed inputs, and the
transient runner log, status and control files.

Created 2026-09-03.
