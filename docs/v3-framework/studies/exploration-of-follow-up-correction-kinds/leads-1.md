# exploration-of-follow-up-correction-kinds — leads

- items with results: 771 of 4093
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results at a sample count; written before the rule that
  leads are written by a fresh session

## Leads

### exploration-of-follow-up-correction-kinds/leads-1/two-in-five-user-turns-correct-the-turn-before-them
- lead: Over 771 of the batch's 4,093 exchanges, 315 user turns correct something in the model turn before
  them and 456 do not. The 315 carry 779 corrections between them, a median of 2 per correcting turn and up
  to 11. The batch's execution called items in one shuffle of the index, so these 771 are a random sample of
  the 4,093 rather than the first of them.
- seen in: user turns across the Gemini web, AI Studio and Claude conversations layers
- query:
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections view=health

### exploration-of-follow-up-correction-kinds/leads-1/what-is-corrected-is-overwhelmingly-a-fact-of-the-world
- lead: Of the 779 corrections: 399 over 193 turns correct a fact of the story's world — a name, an event, a
  date, a relationship, a piece of history, a character's psychology. 220 over 160 turns correct the model's
  reading of the plan, where it took the planning material to say something it does not. 44 over 43 turns
  correct which body of material the model drew on. 41 over 38 turns correct the model's reading of the
  request. Only 9 over 8 turns correct register or format. 66 over 54 turns carry a name of the reader's own.
  So the corrections are about the world and about what the plan says, and almost never about how the model
  wrote.
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections view=terms col=kind top=24
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"fact of the world" view=cites
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"reading of the plan" view=cites
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"which material" view=cites
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"reading of the request" view=cites
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"register or format" view=cites

### exploration-of-follow-up-correction-kinds/leads-1/the-rate-of-correcting-rises-across-the-layers
- lead: Read against the layer prefix of each item id, 154 of the Gemini web layer's 451 answered turns
  correct the turn before them (34%), 130 of the Claude conversations' 266 (48%), and 31 of AI Studio's 54
  (57%). So a user turn in the later layers is half again to two thirds again as likely to be a correction
  as one in the Gemini web layer.
- seen in: user turns of the Gemini web, AI Studio and Claude conversation layers
- query:
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections view=health

### exploration-of-follow-up-correction-kinds/leads-1/the-mix-of-kinds-shifts-toward-the-plan-and-the-material
- lead: The mix within the corrections shifts across the layers. In the Gemini web layer the 154 correcting
  turns carry 175 world-fact corrections against 89 plan-reading, 11 which-material and 20 request-reading.
  In the Claude conversations the 130 correcting turns carry 165 world-fact against 114 plan-reading, 31
  which-material and 19 request-reading. In AI Studio the 31 correcting turns are almost entirely world-fact,
  59 against 17 plan-reading and 2 which-material. So correcting which body of material the model drew on is
  roughly three times as common per correcting turn in the Claude layer as in the Gemini web layer, and
  correcting the model's reading of the plan rises too.
- seen in: user turns of all three layers
- query:
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"which material" view=cites
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"reading of the plan" view=cites

### exploration-of-follow-up-correction-kinds/leads-1/how-a-correction-is-put
- lead: The manner column's commonest words are flat and flatly (301 and 90 lines), stated (236), reason and
  given (210 and 147), question (123), passing (106), without (78), apology (64). So the usual correction is
  stated flatly, often with a reason given and often in passing while the turn gets on with something else; a
  correction put as a question is about a sixth of them and one carrying an apology about a twelfth.
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections view=terms col=manner top=18

### exploration-of-follow-up-correction-kinds/leads-1/register-and-format-are-almost-never-corrected
- lead: Only 9 corrections over 8 turns are about register or format — how the model wrote, how long, in what
  shape. Against 399 about facts of the world, that is one in ninety. Over the whole sample of 771 exchanges
  it is roughly one turn in a hundred.
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-follow-up-correction-kinds/01-user-turns answered=771 field=corrections where kind~"register or format" view=cites

## Proposed questions

- Of the corrections to a fact of the story's world, how many correct a fact the plan holds and how many a
  fact the model supplied from outside it?
- Where a user turn corrects which material the model drew on, which stratum was wrongly drawn on, and did
  the model have any way of knowing?
- The rate of correcting rises from the Gemini web layer to the Claude conversations: does it rise because
  the model says more, or because the plan grew large enough to be misread?
- Do the exchanges with no correction differ from the correcting ones in what the model turn did — an
  analysis, a list, a draft, a question?
- Where a correction is put in passing while the turn gets on with something else, does the model's next turn
  take the correction up?
