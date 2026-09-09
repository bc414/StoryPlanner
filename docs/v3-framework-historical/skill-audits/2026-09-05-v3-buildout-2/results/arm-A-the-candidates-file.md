## unit-080
- section: ## The candidates file
- quote: One per verification WU, in the WU's work folder under `fanout/` (the table above; the `docs/` artifact cites it), **append-only**…
- counterpart: artifacts.md › Candidate
- relation: narrowed
- note: B keeps the finding/source/proposed-by fields and the append-only, per-instance-folder location, but drops the authored `status` field entirely — status is derived from the last line present, and B's outcome step only allows promoted|declined, not the old candidate|diagnostic|non-diagnostic|promoted|declined|held enum.

## unit-081
- section: ## The candidates file
- quote: Referee append (one per target; a candidate bearing on two hypotheses is two candidates):…
- counterpart: artifacts.md › Candidate
- relation: restated
- note: —

## unit-082
- section: ## The candidates file
- quote: Promotion append:…
- counterpart: artifacts.md › Candidate
- relation: narrowed
- note: B renames `disposition` to `outcome` and keeps promoted/declined verbatim, but drops the `held — <what it waits on>` option; an undecided candidate in B simply has no outcome line yet, with no reason recorded (promoting-checked-candidates.md › promote).

## unit-083
- section: ## The candidates file
- quote: Status is derived from the last append and is the only field that changes.…
- counterpart: artifacts.md › Candidate
- relation: narrowed
- note: B keeps deriving status from the last line present (a candidate's status is never an authored/edited field, only read off the sequence of appends), but the `held` state and its "waits on something named" definition are gone — B's outcome step admits only promoted or declined.
