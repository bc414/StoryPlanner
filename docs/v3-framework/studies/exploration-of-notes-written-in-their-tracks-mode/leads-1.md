# exploration-of-notes-written-in-their-tracks-mode — leads

- items with results: 547 of 2066
- written by: a fresh Claude Code subagent session opened for this write-up alone, on 2026-09-27,
  which did not plan the study, write its directions, build its itemizer or read its pilot. It
  could see the batch's definition, directions-2, the index, the results through the
  StoryPlanner.ResultsQuery tool and its README, the question list and the v3-buildout-2 skill;
  it did not read the batch's calls or attempts, the items, any other batch of the study, any
  session transcript or any corpus. Where a lead names tracks, the track of each item was taken
  from the index's description column beside the tool's item column.

## Leads

### exploration-of-notes-written-in-their-tracks-mode/leads-1/out-of-mode-things-often-read-as-answering
- lead: Over the answered notes, the readers placed more of the things the notes say inside the
  track type's declared mode than outside it. Of the things read as outside the mode, more
  were read as giving part of an answer to the track's display question than not; things read
  as inside the mode and not answering were fewer again.
- seen in: the answered notes across all their tracks
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?yes" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z]" where answers~"^(answers.? ?)?yes" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z]" where answers~"^(answers.? ?)?no" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?yes" where answers~"^(answers.? ?)?no" view=list

### exploration-of-notes-written-in-their-tracks-mode/leads-1/planning-tracks-hold-in-world-statements
- lead: In the tracks whose declared mode the readers paraphrased as planning how the reader
  experiences the story, most things were read as outside that mode and named as in-world
  statements instead: plot events and outcomes, lore and setting rules, backstory, a
  character's psychology or decision, technical specifications, business models, naming and
  translation facts. These tracks hold the largest share of the out-of-mode readings in the
  batch.
- seen in: Storytelling Plan, Character Appearance Plan and Usage Plan notes, across subjects
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(reader)" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(in-world|in-story|in-universe)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-639
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-708
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-944
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1086
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1118
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1122
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1291
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1313
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1403
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1404
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1413
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1442
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1511
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1545
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1582
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1594
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1754
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2268
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2288
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2336
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2401
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-188
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-502
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-559
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-618
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-671
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-913
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-914
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1470
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2208
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2371
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2440
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2472
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2520
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-349
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-718
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-732
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-756
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-890
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1396
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1485
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1801
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1802
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1808

### exploration-of-notes-written-in-their-tracks-mode/leads-1/planning-tracks-split-on-answering
- lead: The in-world statements read as outside the planning mode were read differently against
  the display question from one track to another: in Usage Plan and Storytelling Plan most were
  read as giving part of an answer, while in Character Appearance Plan most were read as not
  answering.
- seen in: Usage Plan, Storytelling Plan and Character Appearance Plan notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(reader)" where answers~"^(answers.? ?)?yes" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(reader)" where answers~"^(answers.? ?)?no" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-502
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-559
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-618
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1470
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2208
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2371
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2440
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2472
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2520
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-349
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-718
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-732
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-756
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-890
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1396
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1485
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1801
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1808
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1122
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1442
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2336
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2401

### exploration-of-notes-written-in-their-tracks-mode/leads-1/reader-tracks-hold-world-facts-and-authorial-notes
- lead: In the tracks about the reader's opinion and understanding, things read as outside the
  mode were named as in-world lore or world facts, systemic or game-theoretic explanations of
  the setting, a character's in-story claim or belief, plot summaries, and authorial working
  notes: consistency checks, source citations or tags, statements of creative intent.
- seen in: Reader Opinion, Reader Opinion Plan and Reader Understanding Plan notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(reader)" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(author|citation|source)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-232
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-493
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-534
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-586
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-649
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1006
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2306
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-423
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-425
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-444
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-657
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1041
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1660
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2514
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-182
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-772
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-857
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1337
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1450
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2366

### exploration-of-notes-written-in-their-tracks-mode/leads-1/theme-tracks-state-the-thesis-outright
- lead: In the theme tracks, whose mode the readers paraphrased as evidence marshalled toward a
  thematic proposition, things read as outside the mode were most often named as the thesis or
  thematic principle stated outright rather than evidence for it; most of those were still read
  as giving part of an answer to the display question. Others were named as plain worldbuilding
  or in-world mechanics, hypothetical premises, or plot events.
- seen in: Themes, Theme Plan and Scene Theme Evidence notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(thesis|rather than (as )?(cited |supporting |neutral )?evidence)" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(thesis|rather than (as )?(cited |supporting |neutral )?evidence)" where answers~"^(answers.? ?)?yes" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-161
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-386
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-786
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1563
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1761
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1834
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2550
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-5
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-9
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-481
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1829
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2342
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-314
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1058
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1374
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2416

### exploration-of-notes-written-in-their-tracks-mode/leads-1/staging-tracks-hold-summary
- lead: In the tracks whose mode the readers paraphrased as staging what the reader observes on
  the page, things read as outside the mode were named as summarized plot beats, reported or
  paraphrased speech, stated motives and interior decisions, background exposition, and
  evaluative summaries; most were still read as answering the display question. In Delivery
  Blueprint, some things that were verbatim lines of dialogue were also read as outside the
  mode, named as the dialogue itself rather than a staging description.
- seen in: Delivery Blueprint, Character Actions and Revelation notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(staged|staging|observ)" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(staged|staging|observ)" where answers~"^(answers.? ?)?yes" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(quot|dialogue)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-144
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-937
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1332
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1335
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1587
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1842
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1861
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1876
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2057
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2462
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2464
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2482
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2502
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-146
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-498
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-505
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-994
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1017
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1033
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-332
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-627
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1205
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1371

### exploration-of-notes-written-in-their-tracks-mode/leads-1/history-tracks-hold-interior-quoted-and-evaluative
- lead: In the tracks whose mode the readers paraphrased as a historian's report of fact, things
  read as outside the mode were named as a character's interior motive or belief, direct quoted
  dialogue, evaluative or editorial judgments, present-tense outline or planning shorthand,
  bare titles or labels, retold legend, and vivid or dramatic description.
- seen in: Backstory, History, Historical Events, Invention and Discoveries notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(historian|reported fact|report)" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(quot|dialogue)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-30
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-39
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-43
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-63
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-92
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-126
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-194
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-256
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-329
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-331
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-763
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1259
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1376
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1379
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1527
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1559
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1645
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1647
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1655
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1731
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1784
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2041
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2223
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2402
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2499
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-156
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-306
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-692
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-825
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-955
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1548
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1578
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1663
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-600
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1296
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1845
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1998
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1999
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2233
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-761
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-792
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1679
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1820
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1932
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-162
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-341
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-421

### exploration-of-notes-written-in-their-tracks-mode/leads-1/rule-tracks-hold-narrative-and-judgment
- lead: In the tracks whose mode the readers paraphrased as a neutral statement of the world's
  rules or mechanics, things read as outside the mode were named as historical narrative of a
  past era or conquest, specific vignettes or illustrative scenes, evaluative, polemical,
  satirical or sarcastic commentary, in-world ideological claims, slogans or propaganda, and
  naming or translation glosses.
- seen in: System Ontology and Function notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(rule|mechanic)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-228
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-270
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-463
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-479
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-529
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-591
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1049
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1157
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1172
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1218
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1252
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1301
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1360
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1440
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1446
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1630
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1951
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2535
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2600
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-410
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-553
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-753
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-902
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-980
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1104
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1393
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1500
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1627
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1826
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1918
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2562

### exploration-of-notes-written-in-their-tracks-mode/leads-1/psychological-tracks-hold-appearance-and-citation
- lead: In the tracks whose mode the readers paraphrased as a psychologist's assertion about
  what a character or group is, things read as outside the mode were named as physical
  appearance or cutie mark descriptions, citations of canon episodes, a character's own quoted
  motto, voice or dialogue-writing direction, biographical or historical narration, scene
  narration, and geographic, economic or organizational description.
- seen in: Characterization and Binding Logic notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(psychologist|psychological (assertion|insight|truth|analysis))" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-60
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-209
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-282
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-283
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-510
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-620
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-678
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-683
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-962
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-974
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1076
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1445
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1524
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1669
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1839
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2049
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-824
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-871
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1850
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2229
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2239
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2287

### exploration-of-notes-written-in-their-tracks-mode/leads-1/analogies-hold-in-universe-description
- lead: In Analogies, whose mode the readers paraphrased as documenting a real-world source or
  inspiration, things read as outside the mode were most often named as in-universe
  description of the fictional system, mechanism or naming instead; others as authorial
  design rationale applying the analogy, critique of a production or writing decision, and
  bare titles or cross-reference tags.
- seen in: Analogies notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(real-world)" view=list
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(in-world|in-story|in-universe)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-50
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-775
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-781
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-829
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-963
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1162
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1517
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1811
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1960
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2013
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2312
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2375
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2619
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2650

### exploration-of-notes-written-in-their-tracks-mode/leads-1/canon-tracks-hold-critique-and-story-summary
- lead: In the tracks about canon and source material, things read as outside the mode were
  named as critical analysis or evaluation of another work's genre or devices, authorial
  intent, a personal gaming anecdote, the author's own interpretation or guess at canon
  intent, in-universe justification or invented lore, and summaries of the story's own plot
  rather than of canon.
- seen in: Canon References and Source Material References notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(canon)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-365
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1804
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1976
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1988
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2358
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2645
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-565
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-663
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-669
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1202
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1973

### exploration-of-notes-written-in-their-tracks-mode/leads-1/garden-notes-hold-worldbuilding-and-reflection
- lead: In Garden Notes, whose mode the readers paraphrased as talk about the planning process,
  things read as outside the mode were named as worldbuilding or lore description, a general
  values statement, autobiographical reflection, and social commentary.
- seen in: Garden Notes notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?no[^a-z].*(planning process|planning project)" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-250
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-253
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2137
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1009

### exploration-of-notes-written-in-their-tracks-mode/leads-1/in-mode-things-read-as-not-answering
- lead: Some things read as inside the track type's mode were read as not answering the track's
  display question. In Analogies these were real-world history, etymology and word definitions;
  in Backstory and History, reported facts
  and events; in Theme Plan, thematic contrasts and principles.
- seen in: many tracks, most in Backstory, Analogies, Theme Plan and History notes
- query:
  - rq1 batch=exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2 answered=547 field=claims where mode~"^(mode.? ?)?yes" where answers~"^(answers.? ?)?no" view=list
- cites:
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-63
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-126
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-256
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-325
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-331
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-993
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1527
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1531
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1643
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1647
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1784
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2223
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-73
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1737
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1907
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2375
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2446
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2578
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2619
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2636
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-481
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1829
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2040
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2166
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2342
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-2649
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-760
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1308
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1578
  - exploration-of-notes-written-in-their-tracks-mode/02-tracked-notes-directions-2/note-1663

## Proposed questions

- Do a track type's declared mode and the display questions of its track definitions ask for the same kind of content?
- Where a thing in a working-plan note is in its track type's mode but does not answer its track's display question, does it answer the display question of another track definition the same subject can hold?
