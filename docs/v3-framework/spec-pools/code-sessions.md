# Spec pool — code sessions

Questions about the code-sessions corpus (`codesessions.db`, the sealed Claude Code
transcript archive — including questions about the buildout's own process) awaiting a
verification pass. A runner child cannot query the database: the query is the enumerator
(a HITL session or a script writes the matching turns into `items/`), and classifier jobs
judge the items under a codebook. Format and rules: `README.md` in this directory. Append
only; a superseded entry changes its `status` line.

Created 2026-09-03 as an empty shell; forward-plan-2 seeds it (`forward-plan-2-handoff.md`
§ Seeding the spec pools).
