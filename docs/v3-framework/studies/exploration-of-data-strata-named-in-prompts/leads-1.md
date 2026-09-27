# exploration-of-data-strata-named-in-prompts — leads

- items with results: 748 of 4093
- written by: the Claude Code session that ran the autonomous study campaign of 2026-09-26 to 27,
  which planned this study, wrote its directions, built or chose its itemizer and read its pilot,
  and drew these leads from the batch's results at a sample count; written before the rule that
  leads are written by a fresh session

## Leads

### exploration-of-data-strata-named-in-prompts/leads-1/how-often-a-user-turn-names-a-source-at-all
- lead: Over 748 of the batch's 4,093 user turns the readers found 1,256 places where the turn names a source
  of data and says what the model is to do with it — a median of 2 per turn, up to 12. 254 of the 748 turns,
  a third, name no source at all. The batch's execution called items in one shuffle of the index, so these
  748 are a random sample of the 4,093 rather than the first of them.
- seen in: user turns across the Gemini web, AI Studio and Claude conversations layers
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources view=health
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources view=terms col=source top=26

### exploration-of-data-strata-named-in-prompts/leads-1/naming-a-source-becomes-twice-as-dense-in-the-later-layers
- lead: Read against the layer prefix of each item id, the Gemini web layer's 462 answered turns name 530
  sources, 1.15 per turn, and 199 of those turns — 43% — name none. The Claude conversations' 248 turns name
  633 sources, 2.55 per turn, with only 47 turns — 19% — naming none. AI Studio's 38 turns name 93 sources,
  2.45 per turn, with 8 naming none. So the practice of telling the model which body of material to weigh,
  and how, is about twice as dense per turn in the AI Studio and Claude layers as in the Gemini web layer.
- seen in: user turns of the Gemini web, AI Studio and Claude conversation layers
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources view=health

### exploration-of-data-strata-named-in-prompts/leads-1/what-the-weights-ask-for
- lead: Under the loose alternations, the commonest thing a weight does is exclude: 293 lines over 219 turns
  carry a negation — not, never, ignore, avoid, do not. Then 105 lines over 88 turns say to treat the source
  as true, established, settled, canon, a given or a premise; 93 lines over 83 turns mark it as a
  suggestion, provisional, undecided or proposed rather than settled; 67 lines over 58 turns mark it as
  outdated, superseded or of an earlier era; and 51 lines over 45 turns ask for it to be checked against,
  verified, compared or read first. The weight column's commonest words are treat (398 lines), settled
  (141), against (130), established (82).
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources view=terms col=weight top=24
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources where weight~"not|never|ignore|avoid|do not" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources where weight~"treat as (true|fact|establish|settled|canon|given|premise)" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources where weight~"suggestion|provisional|undecided|proposed|not settled" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources where weight~"outdated|superseded|old|earlier era|stale" view=cites
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources where weight~"check against|verify|compare|read first|prefer" view=cites

### exploration-of-data-strata-named-in-prompts/leads-1/what-counts-as-a-source-in-practice
- lead: The sources named are not mostly corpora. They are the story's own established facts ("Vanhoover
  occupation events as already established", "the same chapter in TLTT where the drug deal is motivated"),
  the model's own preceding turn taken as a premise ("the model's preceding analysis of Vérany's reforms"),
  a stated property of the setting ("the setting's 1900-1940 era, as the author states it"), the canon show
  and earlier generations of the franchise ("the canon School of Friendship", "cutie marks before G4"), an
  attached document ("Cossacks Story.docx, the brief story idea jotted in January 2023"), a track of the
  plan ("chronology track / objective backstory", "the event map"), the author's own recollection about
  writing practice, and the conversation itself ("this entire conversation", to be reported on
  comprehensively with superseded insights noted).
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources sample=12 seed=8 view=list

### exploration-of-data-strata-named-in-prompts/leads-1/most-sources-are-spoken-of-as-already-known
- lead: Asked whether each source is introduced as if the model does not yet know of it or spoken of as
  already known, the readers said referred-to for 941 of the 1,256 lines and first-named for 311. So three
  named sources in four are named as something already in play, which means most of the weighing is
  re-weighting material already on the table rather than adding a body of data.
- seen in: user turns across all three layers
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=sources view=terms col=new top=6

### exploration-of-data-strata-named-in-prompts/leads-1/one-source-put-above-another-is-rare-and-specific
- lead: The readers found only 60 places over the 748 turns where the turn sets one source above another: 36
  in the Claude conversations layer, 17 in the Gemini web, 7 in AI Studio. When it happens it is a named
  precedence, usually the author's own later material over an earlier framing: the author's own story over
  The Princess and the Kaiser on when a spell was invented; a democratic-socialist description over an
  earlier Marxist framing; the user's correction over the model's account of a spell; the author's
  clarifications on in-story Equestria over an earlier WWII-America mapping; a rebuilt materialist
  foundation over the current story plan; "my story plan evolved past the 'destroy order'"; v1 over v2 for
  the TLTT plot; Celestia's own later in-story explanation over two characters' stated beliefs; coherence
  with material conditions over the EaW tech tree.
- seen in: user turns of all three layers, chiefly the Claude conversations
- query:
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=order view=health
  - rq1 batch=exploration-of-data-strata-named-in-prompts/01-user-turns answered=748 field=order sample=12 seed=3 view=list

### exploration-of-data-strata-named-in-prompts/leads-1/one-explicit-ranking-puts-the-v1-archive-above-the-working-plan
- lead: Among the 60 rankings is one that puts the v1 archive above the working plan for one purpose: "v1
  over v2 for the TLTT plot", marked "the TLTT plot is better fully represented in v1". Another puts a
  rebuilt materialist foundation above the current story plan, and another says the story plan has evolved
  past a named earlier order.
- seen in: user turns of the Claude conversations layer
- cites:
  - exploration-of-data-strata-named-in-prompts/01-user-turns/block-2861
  - exploration-of-data-strata-named-in-prompts/01-user-turns/block-2643
  - exploration-of-data-strata-named-in-prompts/01-user-turns/block-2818

## Proposed questions

- The naming of sources is twice as dense per turn after the Gemini web layer: did that change come with the
  MCP server, with the conversations import, or with neither?
- Of the sources a user turn tells the model to treat as settled, how many are facts the working plan holds
  and how many exist only in the conversation?
- Where a user turn marks a source as outdated or superseded, which stratum is it, and does the plan still
  hold that material?
- Explicit precedence between two sources appears in only 60 of 748 turns: where the turn names several
  sources without ranking them, is an order implied by the order they are named in?
- How often does a user turn take the model's own preceding turn as a source to be treated as a premise,
  and does the plan ever end up holding what was premised that way?
