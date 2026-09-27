# exploration-of-prompts-behind-copied-responses — leads

- items with results: 662 of 3934
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results; written before the rule that leads are written by
  a fresh session

## Leads

### exploration-of-prompts-behind-copied-responses/leads-1/the-copied-and-the-not-copied-in-the-sample
- lead: The execution was stopped at 662 of the batch's 3,934 prompts; it called items in one shuffle of the
  index, so the 662 are a random sample. Read against the index's description column, which the readers never
  saw, 133 of the 662 are prompts whose model reply was pasted or lifted into v1 archive notes and 529 are
  prompts whose reply was not. The share differs by layer: 94 of 553 in the Gemini web layer, 22 of 48 in AI
  Studio, 17 of 61 in NotebookLM. Every contrast in the leads below may therefore be partly a contrast between
  layers.
- seen in: user turns of the Gemini web, AI Studio and NotebookLM layers, by the index's copy description
- query:
  - rq1 batch=exploration-of-prompts-behind-copied-responses/01-lineage-prompts answered=662 field=asks view=health

### exploration-of-prompts-behind-copied-responses/leads-1/copied-prompts-are-longer-and-ask-for-more
- lead: The prompts behind a copied reply are longer, a median of 687 characters against 259, and ask the
  model for more: 2.65 requests per prompt against 1.94.
- seen in: user turns of the three lineage layers
- query:
  - rq1 batch=exploration-of-prompts-behind-copied-responses/01-lineage-prompts answered=662 field=asks view=health

### exploration-of-prompts-behind-copied-responses/leads-1/copied-prompts-supply-material-and-shape-the-reply-more-often
- lead: 105 of the 133 copied prompts (78%) hand the model a body of material to work on, against 285 of the
  529 others (53%); 56 of the 133 (42%) give an instruction about the reply's form, length, stance or content,
  against 161 of the 529 (30%). The material most often named is an idea (22% of copied prompts against 14%),
  a plot (18%), a plan (15% against 9%), worldbuilding (14% against 9%) and a premise (13% against 5%).
- seen in: user turns of the three lineage layers
- query:
  - rq1 batch=exploration-of-prompts-behind-copied-responses/01-lineage-prompts answered=662 field=supplies view=health
  - rq1 batch=exploration-of-prompts-behind-copied-responses/01-lineage-prompts answered=662 field=shaping view=health

### exploration-of-prompts-behind-copied-responses/leads-1/copied-prompts-lean-toward-an-answer-they-name
- lead: On how open each prompt is, 85 of the 133 copied prompts (63%) lean toward an answer they name, against
  238 of the 529 others (44%); 21 (15%) leave the answer open, against 165 (31%); 7 (5%) ask for a choice
  between named options, against 54 (10%). Examples of copied prompts: asking whether a proposed piece of lore
  works and how to justify it; asking whether a love-economy mechanism is plausible while brainstorming its
  consequences; naming a spectrum of idea-nouns with answers proposed for most of them.
- seen in: user turns of the three lineage layers
- cites:
  - exploration-of-prompts-behind-copied-responses/01-lineage-prompts/gemini-210-prompt
  - exploration-of-prompts-behind-copied-responses/01-lineage-prompts/gemini-246-prompt
  - exploration-of-prompts-behind-copied-responses/01-lineage-prompts/gemini-338-prompt
  - exploration-of-prompts-behind-copied-responses/01-lineage-prompts/gemini-1815-prompt
  - exploration-of-prompts-behind-copied-responses/01-lineage-prompts/gemini-1274-prompt

### exploration-of-prompts-behind-copied-responses/leads-1/what-the-copied-prompts-ask-for
- lead: The kinds of request the readers named, as the share of prompts carrying the word: check in 39% of
  copied prompts against 27% of the others, brainstorm 34% against 20%, evaluate 15% against 8%, propose 14%
  against 8%, confirm 11% against 3%, validate 9%. Explain is level at 21% in both. So the replies that reached
  the archive answer prompts that more often put a proposal of the author's up for checking and extension.
- seen in: user turns of the three lineage layers
- query:
  - rq1 batch=exploration-of-prompts-behind-copied-responses/01-lineage-prompts answered=662 field=asks view=health

## Proposed questions

- Within the Gemini web layer alone, do the prompts behind copied replies still differ from the others in length,
  openness and supplied material?
- Of the prompts behind copied replies that lean toward an answer they name, did the reply confirm the named
  answer or depart from it?
- Does the share of replies copied into the v1 archive change over the months the Gemini web layer covers?
- Are the replies copied whole preceded by different prompts from the replies of which one sentence was lifted?
- How many of the prompts behind copied replies supply a stretch of the plan itself, as against an idea stated in
  the prompt?
