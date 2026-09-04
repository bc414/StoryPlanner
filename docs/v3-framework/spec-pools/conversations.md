# Spec pool — conversations

Questions about the Conversations corpus (the imported Claude chat transcripts in the
`.storyplan`, with Brian's block states and navigation notes) awaiting a verification
pass. Format and rules: `README.md` in this directory. Append only; a superseded entry
changes its `status` line.

Created 2026-09-03 as an empty shell; seeded 2026-09-04 by forward-plan-2 from plan 1's
WU1.5 chains that cite conversation blocks and WU1.10's conversation-side questions.
Conversations 020 and 039 are not in the database; a question on them is answered from
`docs/design-conversations/` files, cited by file and block.

### Was the perception-gap taxonomy technique-agnostic before Track 99 narrowed it?
- asked-by: plan 1 WU1.5 chains 1 and 8 (conversation 21 block 613: ironic / tragic / closing / aligned); re-housed 2026-09-04
- bears-on: 029, 028
- question: Does block 613 state the four gap types without naming a delivery technique, and which later block of conversation 21 narrows delivery to FID?
- candidate-predicate: `chain-step` per block
- status: open

### Did conversation 36 block 1245 cement FID as the goal?
- asked-by: plan 1 WU1.5 chain 1 (the "italics epiphany"); re-housed 2026-09-04
- bears-on: 028
- question: What does block 1245 say, and does it move FID from a technique to the target?
- candidate-predicate: `chain-step`
- status: open

### Does conversation 47 block 1520 state the every-link-must-have-T rule, and why?
- asked-by: plan 1 WU1.5 chain 7; re-housed 2026-09-04
- bears-on: 036
- question: The rule's text at block 1520 and its stated reasoning; whether WI-terminal links were considered.
- candidate-predicate: `chain-step`
- status: open

### Where do the five EditorModes and Stage 0→3 originate?
- asked-by: 042's created entry (conversations 36 and 47); plan 1 WU1.5 chain 9; re-housed 2026-09-04
- bears-on: 041, 042
- question: The blocks that name each mode and stage, with the visibility and writability reasoning given there.
- candidate-predicate: `chain-step` per mode
- status: open

### Do the block states carry acceptance signal?
- asked-by: plan 1 WU1.10 sub-question 3; 015's created entry; re-housed 2026-09-04
- bears-on: 015
- question: For blocks marked done / flagged / skipped, does the working plan show the block's proposal adopted, rejected, or neither — and at what rates per state?
- candidate-predicate: `acceptance-signal` per block, joined to plan content by search
- status: open

### Are Brian's turns and Claude's formulations distinguishable as registers?
- asked-by: 021's and 022's created entries; plan 1 WU1.11 (register 4); re-housed 2026-09-04
- bears-on: 021, 022
- question: Under a frozen register rule, do Brian's user turns classify as his analytical voice and the assistant's as instruction-shaped framing, and do the authored block summaries form a third class?
- candidate-predicate: `speaker-register` per block
- status: open

### Did v2's staging displace an existing iterative practice?
- asked-by: 002's created entry; plan 1 WU1.5 chain 3; re-housed 2026-09-04
- bears-on: 002
- question: In the v2-era conversations, does Brian describe a practice he already had that the staging replaced, or a practice he was adopting for the first time?
- candidate-predicate: quoted blocks; `practice-shape`
- status: open

### What do Desktop's interaction patterns look like?
- asked-by: plan 1 WU1.10 sub-question 4; re-housed 2026-09-04
- bears-on: 017
- question: Across conversations, what share of Brian's turns ask for analysis, for retrieval, for generation, or correct a prior turn — a census by frozen categories?
- candidate-predicate: classifier per user turn
- status: open

### Is the Conversations corpus a missing voice source for the attribution instrument?
- asked-by: `WU1.4-execution-plan.md` § Background (one `none`-tier archive note had a Claude-voiced sentence; the corpus is not indexed); re-housed 2026-09-04
- bears-on: 019, 020
- question: Indexing the conversations corpus as an additional voice source (Apr 2026 overlap with late v1), how many archive notes change label?
- candidate-predicate: instrument re-run with the extra layer; label-change count
- status: open — an instrument extension; the count is the answer

### Does conversation 64 hold framework-relevant blocks?
- asked-by: plan 1 WU1.5 evidence sources (P&K ASOIAF inspirations, 289 blocks, multi-topic); re-housed 2026-09-04
- bears-on: 033, 037
- question: Which blocks of conversation 64 discuss technique or architecture rather than story content, and what do they claim?
- candidate-predicate: classifier per block (framework / story / other); the framework blocks read
- status: open
