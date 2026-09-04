## unit-068
- section: ### Deposit protocol
- quote: A WU's analyses run discovery-first with no hypothesis in view…
- counterpart: evidence-pipeline.md › Where each kind of write goes
- relation: reversed
- note: B forbids a WU session from writing hypothesis records at all; evidence enters only via a separate verification pass's candidates file, an independent referee, and a later promotion session (HITL, Brian) — not a two-write sequence performed once by the main session (see also wu-execution.md › Four types).

## unit-069
- section: ### Deposit protocol
- quote: **First write — the WU artifact.**…
- counterpart: wu-execution.md › Four types
- relation: narrowed
- note: B keeps "artifact organized by what was observed, never by hypothesis id" essentially verbatim, but the same WU never goes on to touch hypothesis records itself — that requires a separate verification WU, a candidates file, a referee, and a promotion session.

## unit-070
- section: ### Deposit protocol
- quote: **Second write — the hypothesis records.**…
- counterpart: evidence-pipeline.md › Where each kind of write goes
- relation: reversed
- note: B has no session-internal "second write" to hypothesis records; a verification WU writes only a candidates file per hypothesis target, and hypothesis records are written solely by a later, separate promotion session after an independent referee verdict (evidence-pipeline.md › Promotion).

## unit-071
- section: ### Deposit protocol
- quote: 1. Locate the relevant findings in the artifact.
- counterpart: evidence-pipeline.md › Promotion
- relation: reversed
- note: B requires reading the cited source itself before writing any entry ("read the cited source (not the finding — the source) before promoting"), and wu-execution.md › Four types says verification reads "the source itself... never an intermediate analysis alone" — the opposite locus from "the artifact."

## unit-072
- section: ### Deposit protocol
- quote: 2. Write the "Would differ if false:" clause.…
- counterpart: codebook.md › The task
- relation: narrowed
- note: B keeps the clause wording, the supporting/challenging/no-entry test, and routing unresolved findings onward (as spec-pool questions), but a single depositing session may no longer determine the tag itself — an independent, blind referee attempts the clause fresh with no sight of any clause the depositor wrote, and only a separate promotion session may then write an entry.

## unit-073
- section: ### Deposit protocol
- quote: 3. If the statement names specific items, address every named item.…
- counterpart: codebook.md › Decision rules
- relation: restated
- note: —

## unit-074
- section: ### Deposit protocol
- quote: 4. If the entry rests on a classification made by an intermediate analysis (FID vs DT vs blend, mechanism level, obstacle type, perspective mode), cite the passage and read the surrounding paragraph in the original source before writing the entry…
- counterpart: codebook.md › Decision rules
- relation: restated
- note: —

## unit-075
- section: ### Deposit protocol
- quote: A recall-derived testing spec ("Brian's recall: X — does the evidence confirm X?") is a prediction to test, not a search target;…
- counterpart: wu-execution.md › Post-WU review
- relation: restated
- note: —
