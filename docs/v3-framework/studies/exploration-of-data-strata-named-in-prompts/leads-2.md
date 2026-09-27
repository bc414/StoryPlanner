# exploration-of-data-strata-named-in-prompts — leads

- items with results: 4093 of 4093
- written by: the session that wrote leads-1.md, the Claude Code session that ran the autonomous study
  campaign of 2026-09-26 to 27 and planned this study, wrote its directions, built or chose its
  itemizer and read its pilot; it drew these leads over the finished batch and appended them to the
  file it had written at 748 items with results, before the rule that leads are written by a fresh
  session

## Leads

### exploration-of-data-strata-named-in-prompts/leads-2/naming-sources-over-all-4093-turns
- lead: Over the whole batch, every one of the 4,093 user turns answered, the readers found 6,801 named sources:
  2,592 in the Gemini web layer's 2,412 turns, 1.07 per turn, with 1,053 of those turns (43%) naming none;
  3,688 in the Claude conversations' 1,444 turns, 2.55 per turn, with 283 (19%) naming none; 521 in AI Studio's
  237 turns, 2.20 per turn, with 47 (19%) naming none. The sample's contrast holds at full count: naming the
  body of material and saying how to weigh it is well over twice as dense per turn after the Gemini web layer.
  4,952 of the named sources are spoken of as already known and 1,840 introduced as new.
- seen in: user turns of the Gemini web, AI Studio and Claude conversation layers
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=sources view=health
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=sources view=terms col=new top=4

### exploration-of-data-strata-named-in-prompts/leads-2/weights-over-all-4093-turns
- lead: At full count the weights divide as in the sample: a negation — not, never, ignore, avoid, do not — in
  1,537 lines over 1,140 turns; treat as true, established, settled, canon, a given or a premise in 483 over
  382; marked as a suggestion, provisional, undecided or proposed in 471 over 418; marked outdated,
  superseded or of an earlier era in 417 over 349; checked against, verified, compared or read first in 262
  over 216. An explicit ranking of one source over another appears in 366 places: 228 in the Claude
  conversations, 96 in the Gemini web layer, 42 in AI Studio.
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=sources where weight~"not|never|ignore|avoid|do not" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=sources where weight~"treat as (true|fact|establish|settled|canon|given|premise)" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=sources where weight~"suggestion|provisional|undecided|proposed|not settled" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=sources where weight~"outdated|superseded|old|earlier era|stale" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=sources where weight~"check against|verify|compare|read first|prefer" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=4093 field=order view=health
