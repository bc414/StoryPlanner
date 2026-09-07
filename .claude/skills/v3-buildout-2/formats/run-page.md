# run-page

`fanout/<study>/<run>/run.md`, the authored front page of one run, appended to and never
rewritten. Small: which study and which questions the run serves; the instrument and its
hash; the arms, if any, and what is deliberately not measured; the pilot read, by whom and
what it showed; where the tally, the adjudication or the promotion lives. Everything else
about a run is mechanical and lives in `jobs.json`, `ledger.jsonl`, `items/manifest.md`,
`results/` and `tally.md`; the runner's own `attempts/` folder, each attempt's composed
prompt and stream, is its working store, local and never cited. A document in `docs/`
cites a run by this folder and a ledger row.
