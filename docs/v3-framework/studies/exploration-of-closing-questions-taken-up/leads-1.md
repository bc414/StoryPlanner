# exploration-of-closing-questions-taken-up — leads

- items with results: 801 of 1619
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results at a sample count; written before the rule that
  leads are written by a fresh session

## Leads

### exploration-of-closing-questions-taken-up/leads-1/most-closing-questions-are-never-taken-up
- lead: Over 801 of the batch's 1,619 model turns ending in a question, the readers named 1,293 questions
  put to the user. Of those, 868 over 529 exchanges were ignored — the next user turn goes on to something
  else and never touches them; 228 over 144 had no user turn at all, the conversation ending there; 96 over
  82 were partly answered; 77 over 59 were answered; and 23 over 21 were refused, deferred or called wrong.
  So of the questions a model put to the author, about one in eight was taken up in any degree and two in
  three were passed over. The batch's execution called items in one shuffle of the index, so these 801 are
  a random sample of the 1,619 and not the first of them.
- seen in: model turns across the Gemini web, AI Studio and Claude conversations layers
- query:
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions view=health
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions view=terms col=outcome top=10
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions where outcome~^ignored view=cites
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions where outcome~"^no user turn" view=cites
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions where outcome~^partly view=cites
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions where outcome~^answered view=cites
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions where outcome~^refused view=cites

### exploration-of-closing-questions-taken-up/leads-1/the-three-layers-differ-in-how-they-fail-to-answer
- lead: Read against the item ids, which carry the layer, the rate of ignoring is highest in the Claude
  conversations and lowest in the Gemini web layer, while the rate of conversations simply ending at the
  question is the reverse. Of the Gemini web layer's 419 answered exchanges: 307 question lines ignored
  (61%), 109 with no user turn (21%), 40 partly answered (8%), 36 answered (7%), 6 refused. Of AI Studio's
  140: 185 ignored (64%), 82 with no user turn (28%), 10 partly (3%), 4 answered (1%), 6 refused. Of the
  Claude conversations' 242: 376 ignored (74%), 37 with no user turn (7%), 46 partly (9%), 37 answered (7%),
  11 refused. AI Studio is the layer whose closing questions are answered least of all, at one line in a
  hundred.
- seen in: the Gemini web, AI Studio and Claude conversation layers, by the layer prefix of each item id
- query:
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions view=health
- cites:
  - exploration-of-closing-questions-taken-up/01-question-endings/gemini-2015-response
  - exploration-of-closing-questions-taken-up/01-question-endings/aistudio-67-t93

### exploration-of-closing-questions-taken-up/leads-1/what-the-user-turn-does-instead-is-redirect
- lead: On the separate whole-turn question of what the user turn does, the word that dominates every layer
  is redirect: 167 of the Gemini web layer's turns, 103 of the Claude conversations', 50 of AI Studio's.
  Correcting the model comes next (48, 71, 30), then giving an instruction (55, 28, 5), then ending the
  conversation (27, 21, 20). Answering is named in only 12, 24 and 9 turns respectively. So the usual reply
  to a model turn that closes with a question is to set the direction of the work rather than to settle the
  question.
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions view=health

### exploration-of-closing-questions-taken-up/leads-1/the-user-turns-settle-a-great-deal-that-the-question-did-not-ask
- lead: Beside the questions, the readers were asked to record every decision about the work itself that the
  user turn settles. Over the 801 exchanges they recorded 1,642 such decisions — 715 in the Claude
  conversations layer, 516 in the Gemini web layer, 411 in AI Studio — roughly two per exchange, against
  0.2 questions answered per exchange. So the user turns following a closing question are dense in
  decisions; they are simply not the decisions the model asked about.
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=settles view=health

### exploration-of-closing-questions-taken-up/leads-1/one-exchange-in-six-has-no-user-turn-after-the-question
- lead: 228 question lines over 144 exchanges have no user turn after them: the conversation ends on the
  model's question. This is the second commonest outcome, and it is concentrated in AI Studio (82 lines over
  140 answered exchanges, 28%) and the Gemini web layer (109 over 419, 21%) rather than in the Claude
  conversations (37 over 242, 7%).
- seen in: the ends of conversations in all three layers, chiefly AI Studio and Gemini web
- query:
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions where outcome~"^no user turn" view=cites

### exploration-of-closing-questions-taken-up/leads-1/how-many-questions-a-closing-turn-puts
- lead: A model turn that ends in a question puts a median of 2 questions to the author and up to 12; 1,293
  question lines over 801 exchanges. So the closing question is usually a small set rather than one, which
  means the count of ignored questions per turn is higher than the count of turns that ignored something.
- seen in: model turns across all three layers
- query:
  - rq1 batch=exploration-of-closing-questions-taken-up/01-question-endings answered=801 field=questions view=health

## Proposed questions

- Of the decisions the user turns settle after a closing question, how many appear afterwards in the
  working plan, and in whose voice?
- AI Studio's system instruction asked the model to end with Socratic questions, and AI Studio's closing
  questions are the least answered of the three layers: did the instruction change what the model asked, or
  only that it asked?
- Where a user turn redirects rather than answers, does it return to the ignored question later in the same
  conversation?
- Do the closing questions that were answered differ in kind from those that were ignored — for instance by
  asking about a fact of the world rather than about what to do next?
- In the Claude conversations, where the ignoring rate is highest, does the block's read state record
  whether the author read the model turn that asked?
