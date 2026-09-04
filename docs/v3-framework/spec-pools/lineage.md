# Spec pool — lineage

Questions about the lineage corpus (the Google Doc revision history, the Gemini
conversations and reports, the AI Studio chats, the NotebookLM captures — and the Keep
notes once ingested) awaiting a verification pass. Provenance point-checks belong here.
Format and rules: `README.md` in this directory. Append only; a superseded entry changes
its `status` line.

Created 2026-09-03 as an empty shell; seeded 2026-09-04 by forward-plan-2 from plan 1's
WU1.5 provenance chains and table, WU1.8, WU1.10 and the pre-revision entries on 045.
Entries marked "waits on WU2.23" need the Keep layer; "waits on WU2.22" need the non-TLTT
revision histories. "Brian's recall" marks recall; recall is a question, never evidence.

### The FID fixation chain — does each step appear where claimed?
- asked-by: plan 1 WU1.5 chain 1; re-housed 2026-09-04
- bears-on: 028, 029
- question: Does `nlm:3` t#54 distinguish DT from FID, do t#94–130 prescribe deep third for TLTT, and does the chain continue into conversation 21 and conversation 36 block 1245 (see the conversations pool)? What does each locus actually say?
- candidate-predicate: `chain-step` per locus
- status: open

### When do deep-third / FID / DT / variable-focalization terms first appear, and who introduced them?
- asked-by: plan 1 WU1.5 chain 2 (the unverified claim "NLM introduced vocabulary from Pokemon analysis, not from P&K"); re-housed 2026-09-04
- bears-on: 002, 028
- question: For each term, the earliest dated lineage locus and its speaker; whether the term predates the NLM Pokemon analysis.
- candidate-predicate: `first-appearance` per term (the search alternation recorded)
- status: open — partly waits on WU2.23 (pre-AI Keep timestamps)

### Was v1's practice hypothesize-gather-iterate before the vocabulary existed?
- asked-by: plan 1 WU1.5 chain 3 and WU1.8; re-housed 2026-09-04
- bears-on: 002
- question: Over dated stretches of Gemini turns and gdoc diffs, does Brian test and correct claims (e.g. correcting Gemini's axis definitions against his plan) or accumulate them without testing?
- candidate-predicate: `practice-shape` per stretch
- status: open — partly waits on WU2.23

### Where did the M2 prior-belief vocabulary come from?
- asked-by: plan 1 WU1.5 chain 4; re-housed 2026-09-04
- bears-on: 029
- question: The earliest loci formulating prior-belief clash and revision, and the prescriptive reasoning that shaped it.
- candidate-predicate: `first-appearance`; `chain-step`
- status: open

### Where did The Spark come from?
- asked-by: plan 1 WU1.5 chain 5; re-housed 2026-09-04
- bears-on: 029, 033
- question: The earliest locus for "why two characters have something special" as a design concept, and its reasoning — for comparison with the M2 and FID chains in WU2.17.
- candidate-predicate: `first-appearance`; `chain-step`
- status: open

### Was DT-based gap work considered when Track 99's FID prescription was written?
- asked-by: plan 1 WU1.5 chain 6; re-housed 2026-09-04
- bears-on: 028, 029, 031
- question: In the loci that formulate the perception-gap track's usage directive, is DT considered and rejected, or never considered?
- candidate-predicate: `chain-step` (adopted / narrowed / rejected / absent)
- status: open

### How did perception gap come to occupy the pinnacle, and were its peers considered?
- asked-by: plan 1 WU1.5 chain 6b; 029 iteration @ 2026-09-01T03:30 ("WU1.5 should trace how it came to be elevated"); re-housed 2026-09-04
- bears-on: 029
- question: The loci where perception gap is named as the central design target; whether prior-belief management, investment accumulation, revelation sequencing or reader stance trajectory are named as peers there.
- candidate-predicate: `chain-step` per peer
- status: open

### Where did the v2 prescribed workflow come from, and where did it break?
- asked-by: plan 1 WU1.5 chain 9; re-housed 2026-09-04
- bears-on: 041, 042
- question: The loci proposing Stage 0→3 and the five EditorModes, and any v2-era loci showing scene-level work attempted and stalled.
- candidate-predicate: `chain-step`; dated stall loci
- status: open

### Does the read-generate-paste-reread cycle show in the Gemini record?
- asked-by: plan 1 WU1.5 chain 10 (reports W05–W07 on the ~940K-char paste); re-housed 2026-09-04
- bears-on: 019
- question: For a sample of note ↔ response pairs from `attribution.csv`, does the next session's plan-paste contain the pasted AI text?
- candidate-predicate: per pair, presence of the pasted span in the subsequent paste stub or window
- status: open

### Why was the planner built — is cross-scene architecture the stated motivation?
- asked-by: plan 1 WU1.5 chain 11 — Brian's recall; 034 iteration @ 2026-09-01T03:30; re-housed 2026-09-04
- bears-on: 034, 001
- question: What do the earliest planning conversations (Nov–Dec 2025) and gdoc revisions say about why the planner was created?
- candidate-predicate: `first-appearance` of the motivation; quoted locus
- status: open — partly waits on WU2.23 (StoryPlanner conception, Dec 6 2025, is a Keep note)

### Where did the hopepunk thesis come from, and how does it relate to canon-virtue-as-trap?
- asked-by: plan 1 WU1.5 chain 12 — Brian's recall (P&K's grimdark thesis is the trap; TLTT subverts it; chapters 3–8 approach it from another angle); re-housed 2026-09-04
- bears-on: 033
- question: The dated loci where the thesis is formulated, and whether subversion or reframing is the stated relation to P&K.
- candidate-predicate: `first-appearance`; `chain-step`
- status: open

### The nine-row provenance table — does each concept first appear where claimed?
- asked-by: plan 1 WU1.5 chain 13 (`VERSION-HISTORY-DRAFT1.md` table preserved in `consolidation-1-plan.md`: fabula/syuzhet at `aistudio:6`, Architect/Gardener at `nlm:3`, and the rest); re-housed 2026-09-04
- bears-on: 002, 007, 028
- question: Per row, the earliest lineage locus for the concept and whether it matches the claimed one.
- candidate-predicate: `first-appearance` per row
- status: open

### Do the revision histories show batch sweeps or incremental editing?
- asked-by: plan 1 WU1.8; re-housed 2026-09-04
- bears-on: 004
- question: Over the gdoc diffs (TLTT now; KU/NTL, GIYC, Falldale after ingest), is change concentrated in time or spread evenly?
- candidate-predicate: diff size per day; concentration measure
- status: open — extended scope waits on WU2.22

### Do revision patterns differ with tooling across stories?
- asked-by: plan 1 WU1.8; re-housed 2026-09-04
- bears-on: 018
- question: Do KU/NTL and GIYC (Google Doc), Falldale (a v0 document contemporary with early v1) and TLTT (Doc → v1) show revision patterns that correlate with the tooling of their era — the Falldale quasi-experiment?
- candidate-predicate: per story, the sweep measure above by era
- status: open — waits on WU2.22

### Does the planning record show a perspective-thinking shift in the FiM reading period?
- asked-by: plan 1 WU1.8; re-housed 2026-09-04
- bears-on: 039
- question: Do planning-document revisions dated in or after the FiM reading period discuss perspective technique differently from earlier ones?
- candidate-predicate: `first-appearance` of perspective terms per story history
- status: open — waits on WU2.22 for the non-TLTT histories

### Is the v0 wall visible in the earliest TLTT revisions?
- asked-by: plan 1 WU1.8 (the fabula/syuzhet mix in the early Doc; later separation emerging); re-housed 2026-09-04
- bears-on: 040, 002
- question: In the 2025 gdoc snapshots, is fabula delivered inside chapter plans as dialogue-to-be-written, and does a separation emerge at a datable revision?
- candidate-predicate: per snapshot, share of chapter-plan text that is fabula statement; first separated revision
- status: open

### Which factors changed at each era transition?
- asked-by: plan 1 WU1.10 sub-question 1; re-housed 2026-09-04
- bears-on: 006, 007
- question: For v0→v1, v1→v2, v2→v3, and the mid-era changes (Claude web chat arriving 2026-04-09), which of model / data stream / instructions / harness changed, on what dates, from the lineage record?
- candidate-predicate: per transition, a dated factor table with loci
- status: open

### What did the instruction evolution look like before v3?
- asked-by: plan 1 WU1.10 sub-question 3; re-housed 2026-09-04
- bears-on: 014
- question: The Gemini gem's four rules and the AI Studio system prompts (`scope=system`) as dated texts: what did each instruct, and what changed between them?
- candidate-predicate: quoted texts with dates
- status: open

### Are acceptance signals mineable from the lineage record?
- asked-by: plan 1 WU1.10 sub-question 3; re-housed 2026-09-04
- bears-on: 015
- question: For AI turns whose text was pasted into the plan (`attribution.csv` pastes), what distinguishes the prompting context from turns that were not pasted?
- candidate-predicate: per turn, pasted yes/no × prompt features under a frozen list
- status: open

### Does Gemini's register carry false finality?
- asked-by: 022's created entry (2026-08-31); re-housed 2026-09-04
- bears-on: 022
- question: In a sample of Gemini turns later pasted into v1 notes, what share use declarative-final framing (capitalised emphasis, "THE fundamental…") for what the surrounding turn shows to be a proposal?
- candidate-predicate: `speaker-register` variant per turn
- status: open

### Do the Keep notes hold material absent from every other layer?
- asked-by: pre-revision entries 045 @ 2026-08-31T22:30, T23:00 (Brian's correction: the December 2025 avalanche was pasted into Gemini, 317 hits), T23:30 (verification round 2: 6 of 8 items content-unique, 2 timestamp-unique) (WU1.2); re-housed 2026-09-04 as unverified
- bears-on: 045
- question: Once ingested, for each authored include-list note, does a concept search over the other layers return zero hits (content-unique), hits later than the Keep timestamp (timestamp-unique), or same-day hits (non-unique)?
- candidate-predicate: per note, category by search
- status: open — waits on WU2.23

### Was the TwiJack perspective switch NLM's suggestion, and on what reasoning?
- asked-by: plan 1 WU1.4/WU1.5 spec; re-housed 2026-09-04
- bears-on: 028
- question: The `nlm:3` locus that proposes a planned perspective switch for TwiJack scenes, and its stated reasoning.
- candidate-predicate: `first-appearance`; quoted locus
- status: open

### How was Celestia's design discussed in the founding conversations?
- asked-by: plan 1 WU1.9 spec — Brian's recall (inscrutability → retroactive recontextualization); re-housed 2026-09-04
- bears-on: 029
- question: Do dated lineage loci design Celestia as a site of hidden knowledge for later disclosure, and do they cite the corpus AU stories?
- candidate-predicate: `first-appearance`; `chain-step`
- status: open

### Does the lineage record the bond/structural-plot intertwining as a design intent?
- asked-by: plan 1 WU1.3 spec ("Lineage discusses this intertwining — WU1.5 should check"); re-housed 2026-09-04
- bears-on: 033, 037
- question: Which dated loci state that TLTT's bonds sit alongside a structural conflict rather than requiring character change first?
- candidate-predicate: quoted loci
- status: open

### What obstacle architecture do the unwritten story plans design?
- asked-by: plan 1 WU1.8 spec (Google Drive "Miscellaneous Story Stuff", ~15 plans 2016–2024; a source not in any corpus); re-housed 2026-09-04
- bears-on: 033, 037
- question: Per plan, the designed primary obstacle type, and whether it correlates with which plans became stories.
- candidate-predicate: `obstacle-type` per plan
- status: open — source not ingested; an infrastructure question before a verification one
