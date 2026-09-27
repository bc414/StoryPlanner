# exploration-of-v1-analysis-loop-pattern — leads

- items with results: 746 of 909
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results; written before the rule that leads are written by
  a fresh session

## Leads

### exploration-of-v1-analysis-loop-pattern/leads-1/how-many-conversations-left-a-trace-in-the-archive
- lead: Over 746 of the batch's 909 conversations and parts — called in one shuffle of the index, so a random
  sample — 398 have at least one v1 archive note traced to one of their messages and 348 have none. By the
  index's description column: 340 of the 664 answered Gemini web threads, 45 of the 69 AI Studio chats and
  parts, and all 13 NotebookLM parts. The readers named a median of 8 moves per conversation, up to 72. In 708
  of the 746 the author brought something in from the plan or from elsewhere.
- seen in: Gemini web threads, AI Studio chats and NotebookLM notebooks of the lineage corpus
- query:
  - rq1 batch=exploration-of-v1-analysis-loop-pattern/01-lineage-threads answered=746 field=steps view=health

### exploration-of-v1-analysis-loop-pattern/leads-1/what-the-author-does-in-the-loop
- lead: The author's moves are named, most often, with question (773 step lines), asks (485), request (254),
  brings (251), proposes (230), poses (206), corrects and correction (325 between them), supplies (139), adds
  (121) and extends (94). The first move of a conversation is most often a request, supplied or attached
  material, a proposal or a correction.
- seen in: author turns across the lineage layers
- query:
  - rq1 batch=exploration-of-v1-analysis-loop-pattern/01-lineage-threads answered=746 field=steps view=health

### exploration-of-v1-analysis-loop-pattern/leads-1/what-the-model-does-in-the-loop
- lead: The model's moves are named, most often, with analysis and analyzes (925 between them), elaborates
  (197), delivers (186), verdict (170), validates (155), supplies (148), drafts and draft (229), extends (118),
  builds (108), mechanism (105), proposes (104) and options (103). So the model's side of the loop is mainly
  analysing, elaborating and passing a verdict on what the author brought, with drafting and option sets less
  often.
- seen in: model turns across the lineage layers
- query:
  - rq1 batch=exploration-of-v1-analysis-loop-pattern/01-lineage-threads answered=746 field=steps view=health

### exploration-of-v1-analysis-loop-pattern/leads-1/what-the-archive-kept-and-how
- lead: The readers listed 1,962 archive notes traced to messages of the 746 conversations. By the relation the
  item gives: pasted whole from the reply 654, the author's own words in the record 553, pasted from the reply
  inside the author's own framing 394, one sentence lifted from the reply 127, pasted from the reply with cuts
  117, the plan holding the text before the reply 76, and the reply quoting the plan 28. So of what the
  archive kept, a little under half is a paste of the model's reply and over a quarter is the author's own
  prompt text.
- seen in: the archive notes traced to lineage messages
- query:
  - rq1 batch=exploration-of-v1-analysis-loop-pattern/01-lineage-threads answered=746 field=kept view=health

### exploration-of-v1-analysis-loop-pattern/leads-1/the-loop-as-the-readers-describe-it
- lead: The loops the readers describe repeat one shape with variations: the author brings a compact lore
  fragment, a constraint, a canon detail or a correction together with a question; the model returns an
  expanded, sectioned analysis or an in-world mechanism; the author affirms, corrects or builds the next
  constraint on top of it; and the archive keeps selected passages verbatim or lightly trimmed. One thread runs
  six such rounds, each built on the model's last answer. In another, the plan keeps the author's own premise
  verbatim on a plot point and pastes two of the model's passages beside it. Where nothing is traced, the
  readers describe the same shape ending with the model's option set or framework and nothing captured back.
- seen in: Gemini web threads of the lineage corpus
- cites:
  - exploration-of-v1-analysis-loop-pattern/01-lineage-threads/gemini-th-c4148bc7
  - exploration-of-v1-analysis-loop-pattern/01-lineage-threads/gemini-th-b16dbdce
  - exploration-of-v1-analysis-loop-pattern/01-lineage-threads/gemini-th-97948aa6
  - exploration-of-v1-analysis-loop-pattern/01-lineage-threads/gemini-th-cde3fb46
  - exploration-of-v1-analysis-loop-pattern/01-lineage-threads/gemini-th-97d2f0c0
  - exploration-of-v1-analysis-loop-pattern/01-lineage-threads/gemini-th-9cdc9874

## Proposed questions

- Do the conversations whose replies the archive kept differ from those it did not by the author's first move?
- Where the archive kept the author's own prompt text rather than the reply, what had the model returned?
- Across a thread of several rounds, does the archive keep the last round's text or text from every round?
- Where the plan held the text before the reply quoted it back, what did the author then ask of the model?
- Does the shape of the loop differ between the Gemini web threads and the AI Studio chats?
