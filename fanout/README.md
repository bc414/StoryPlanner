# fanout — everything the agent runner takes in and puts out, grouped by work

One folder per piece of work that runs autonomous agents through
`tools/StoryPlanner.AgentRunner`. A work folder holds the whole apparatus for that work and
nothing else: the protocol or codebook it runs under, the calibration record, the generator
and the tallier, and one folder per run with its items, job file, results, attempt records,
ledger and `run.md`. Vertical, by work — never a shared `protocols/` or `codebooks/` folder
across works. What a person writes afterwards (a WU's synthesis, an audit's adjudication) is
a document and lives in `docs/`, citing the run here by ledger row.

```
fanout/
  PROTOCOL.md                       the experiment lifecycle — served by the host at /protocol
  host-log.txt                      the host's log: lifecycle, enqueues, knob changes (gitignored)
  referee/                          the referee: codebook.md, calibration record, runs
  WU<n>.<m>-<slug>/                 a verification WU: codebooks, items, jobs, results, candidates
  skill-audits/                     document supersession audits
    protocol.md  make-jobs.ps1  tally.ps1
    2026-09-03-v3-buildout/
      run.md                        the run's authored front page (what PROTOCOL.md says it holds)
      jobs.json                     generated — the host writes beside it
      items/manifest.md             the enumeration's index (unit bodies regenerable, not committed)
      results/                      one agent output per job
      attempts/<job-id>/attempt-N/  prompt.md, stream.jsonl — local only
      ledger.jsonl                  one row per attempt: hashes, model, harness, cost, turns, mode
  smoke-test/                       the harness check
```

Rules, all in the `agent-runner` skill: the run folder is the job file's folder and must be
under this tree; every path in a job file is relative to it; the child process runs from
`RiderProjects\StoryPlanner-fanout` (outside the repo, so no instruction stack reaches it)
and writes back here by absolute path. Enqueue with `AgentRunner.exe <run>\jobs.json`; the
host (`http://127.0.0.1:5190`) shows every run here, live or finished.

One commit convention for every run (`.gitignore`): `attempts/` is never committed
(`prompt.md` is ~85 KB and reconstructible from the ledger's input hashes plus the committed
inputs; `stream.jsonl` is for watching a live run); `items/` holds regenerable inputs and only
its `manifest.md` is committed; everything else under a run — `run.md`, jobs, results, ledger,
protocol or codebook, generator and tallier — is committed. A non-regenerable input (a
source excerpt) is written outside `items/` so it is kept.

Created 2026-09-03.
