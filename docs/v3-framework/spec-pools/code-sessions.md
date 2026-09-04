# Spec pool — code sessions

Questions about the code-sessions corpus (`codesessions.db`, the sealed Claude Code
transcript archive — including questions about the buildout's own process) awaiting a
verification pass. A runner child cannot query the database: the query is the enumerator
(a HITL session or a script writes the matching turns into `items/`), and classifier jobs
judge the items under a codebook. Format and rules: `README.md` in this directory. Append
only; a superseded entry changes its `status` line.

Created 2026-09-03 as an empty shell; seeded 2026-09-04 by forward-plan-2. Opens with the
three questions the 2026-09-03 evening raised about the buildout's own process
(`methodology-revision-1.md` addenda), then plan 1's WU1.10 items. Precondition for any
round: the purge script's two selects return zero (met 2026-09-04; re-check at enumeration).

### Does a design authored in a session violate rules loaded in that session at a measurable rate?
- asked-by: methodology revision 1 addendum (2026-09-03: the first runner job broke four rules its author had just written); recorded 2026-09-04
- bears-on: 048
- question: Over sessions where a rule text (a skill or CLAUDE.md section) was written and then applied in the same session, what share of the applying turns violate a rule written earlier in that session?
- candidate-predicate: `rule-violation` per (rule, later turn) pair; enumerator = sessions whose tool stubs show a skill or CLAUDE.md edit followed by use
- status: open

### Does an over-flagging auditor, given the author's drop list, flag fewer intended changes without drifting?
- asked-by: methodology revision 1 addendum (2026-09-03; the supersession audit excluded the revision note by design); recorded 2026-09-04
- bears-on: 049
- question: Re-running the skill-audit units with the revision note's drop list as an extra input, does the count of flagged intended changes fall, and does the count of flagged unintended changes stay the same?
- candidate-predicate: `relation-label` two arms over the 174 units, with / without the drop list; per-unit label agreement and the two counts
- status: open

### Do per-section batching and per-unit jobs yield the same relation labels?
- asked-by: methodology revision 1 addendum (the batch-size convention's own test); recorded 2026-09-04
- bears-on: 047, 013
- question: Over the same 174 units and the same protocol hash, what is the label agreement between the per-section run (`fanout/skill-audits/2026-09-03-v3-buildout/`) and a per-unit run?
- candidate-predicate: `relation-label` per unit in both arms; agreement rate and the direction of disagreements
- status: open

### Is "convenience overriding a stated principle" an error class with a model signature?
- asked-by: plan 1 WU1.10 observation (a) (2026-08-31: Opus 4.6 needed four corrections on the ordering principle; Fable none) — a session observation, a question; re-housed 2026-09-04
- bears-on: 010, 011, 013
- question: Over decision turns where a principle was stated earlier in the session, what share of each model's decisions contradict it, by activity type?
- candidate-predicate: `principle-error` per decision turn; enumerator = turns following a stated principle, labelled by model from the session record
- status: open

### Is scope of initiative task-type-dependent?
- asked-by: plan 1 WU1.10 observation (b) (2026-08-31: narrow scope hurt in framework buildout, helps in story planning) — a question; re-housed 2026-09-04
- bears-on: 010, 013
- question: Per assistant response, is it narrow or proactive, and does the rate differ by activity type (buildout / feature / data / story) and by model?
- candidate-predicate: `initiative-scope` per response with activity from the enumerator
- status: open

### What did the v3 instruction stack change, and when?
- asked-by: plan 1 WU1.10 sub-question 3; re-housed 2026-09-04
- bears-on: 014
- question: From the git log of CLAUDE.md and the skills joined to the sessions that made each change, what did each revision add or remove, and what prompted it (the user turn)?
- candidate-predicate: per revision, the prompting turn quoted
- status: open

### Are Brian's corrections in user turns a mineable negative signal?
- asked-by: plan 1 WU1.10 sub-question 3; 015's created entry; re-housed 2026-09-04
- bears-on: 015
- question: What share of user turns are corrections of the preceding assistant turn, and what do they correct (fact / scope / register / rule)?
- candidate-predicate: classifier per user turn under a frozen category list
- status: open

### Which tools and MCP queries does each consumer use?
- asked-by: plan 1 WU1.10 sub-question 4; re-housed 2026-09-04
- bears-on: 016, 017
- question: A census of tool-call stubs by tool and MCP tool name across sessions by project and month — which data sources are queried from Code, and how that compares to the conversations corpus's Desktop patterns.
- candidate-predicate: count by tool name (script; no LLM)
- status: open

### Did the MCP server enable retrospective review that was impossible before?
- asked-by: 009's created entry; re-housed 2026-09-04
- bears-on: 009
- question: Are there sessions that query a prior AI interaction with the plan (lineage or conversations tools) and act on what they find, and what did they find?
- candidate-predicate: enumerator = sessions with `search_lineage` / `search_conversations` stubs; per session, an action following the query yes/no
- status: open

### How often does a recalled or documented claim fail against current evidence?
- asked-by: 005's created entry (no document carries intrinsic authority); re-housed 2026-09-04
- bears-on: 005
- question: Over turns where a session grounded a FEATURE-AUDIT, memory-file or CLAUDE.md claim against the data, what share found a discrepancy?
- candidate-predicate: enumerator = turns containing a grounding query following a document citation; classifier: discrepancy found yes/no
- status: open

### Are there signs of a v3 wall in the engineering record?
- asked-by: 044's created entry (the hallmark-wall pattern); re-housed 2026-09-04
- bears-on: 044
- question: Do sessions record complexity exceeding an instrument (a feature cut for that reason, a rebuild, an "outgrown one file" moment) at a datable point, and at what abstraction level?
- candidate-predicate: enumerator by keyword over user turns; classifier: instrument-capacity reason yes/no
- status: open

### Does the pre-MCP versus post-MCP conversation quality differ once data architecture is held constant?
- asked-by: plan 1 WU1.10 sub-question 2 (confounded with data architecture); re-housed 2026-09-04
- bears-on: 010, 011
- question: Is there any session pair in the archive that varies the model with the same data path and instructions, and if so what differs? If none exists, the answer is "no natural experiment in the archive" and the runner's one-factor cells are the route.
- candidate-predicate: enumerator over session metadata; likely a census with an empty result
- status: open
