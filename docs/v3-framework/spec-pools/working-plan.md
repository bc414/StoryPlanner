# Spec pool — v2 working plan

Questions about the v2 working plan corpus awaiting a verification pass. Format and rules:
`README.md` in this directory. Append only; a superseded entry changes its `status` line.

### Can per-claim evidence work on the plan stay on the investigator side of the line?
- asked-by: methodology revision 1 design conversation (2026-09-02/03); recorded 2026-09-03
- bears-on: 001, 003
- question: Brian's design intent is that the buildout's item-scoped machinery (calibrated codebooks, the referee, promotion review) eventually runs against the working plan itself — per note or claim, "what evidence bears on this?" — as an investigator in the verify role. Does a trial of that mode on a bounded set of working-plan notes produce evidence pictures without any output that ranks, proposes, or judges what is interesting about a note (the retrieval-not-suggestion line, CLAUDE.md)?
- candidate-predicate: per output, a classifier applies "does this line propose, rank, or evaluate story content?" — any positive is a line crossing; count and cite
- status: open

<!-- Seeded 2026-09-04 by forward-plan-2 from plan 1's WU1.7 survey questions, the specs moved to it, and the working-plan items of WU1.13 and WU1.14. Counts come from the tools at run time; never cite a number from here. -->

### What is the note-state distribution?
- asked-by: plan 1 WU1.7 question 1; re-housed 2026-09-04
- bears-on: 003
- question: How many notes are unset, confirmed and flagged (`get_stats`, `count_notes_plan`), and is the zero-Confirmed figure still true?
- candidate-predicate: census
- status: open

### Is track population lumpy in the way sweeps would produce?
- asked-by: plan 1 WU1.7 question 2; re-housed 2026-09-04
- bears-on: 004
- question: Per track, note counts; are they dense in capture-sweep tracks and sparse in tracks needing incremental attention?
- candidate-predicate: census; a lumpiness measure defined before the count
- status: open

### Is scene-level content sparse relative to subject-level content?
- asked-by: plan 1 WU1.7 question 3; re-housed 2026-09-04
- bears-on: 041, 044
- question: Plot-point-level and link-level note counts versus subject-level, by track type.
- candidate-predicate: census
- status: open

### Was the cognitive-mode split practiced in the data?
- asked-by: plan 1 WU1.7 question 4; re-housed 2026-09-04
- bears-on: 042
- question: Track populations by cognitive mode (ZeroFocalization versus NarrativeDesign); any notes whose content belongs to the other mode under a frozen rule.
- candidate-predicate: census; classifier on a sample
- status: open

### Are notes doing cross-focalizer knowledge management informally?
- asked-by: plan 1 WU1.7 question 5; re-housed 2026-09-04
- bears-on: 029
- question: In Reader Prior Belief Update and Reader Opinion notes, how many track what the reader knows from focalizer A that focalizer B does not, despite no track being designed for it?
- candidate-predicate: classifier per note
- status: open

### Do Theme Plan and Scene Theme Evidence notes articulate opposing positions?
- asked-by: plan 1 WU1.7 (moved from WU1.3); re-housed 2026-09-04
- bears-on: 036
- question: Under `counterargument`, which theme propositions carry a designed opposing position?
- candidate-predicate: `counterargument` per proposition
- status: open

### Does the Kitty of Westkeep plan show shame-about-desire as thematic testing ground?
- asked-by: plan 1 WU1.7 (moved from WU1.3); re-housed 2026-09-04
- bears-on: 033
- question: In the Kitty subjects, notes and story plan, is the pattern designed, with ids?
- candidate-predicate: focused reader under a frozen rule
- status: open

### Do Character-Reader Perception Gap or Reader Opinion notes design asymmetry between bonded partners?
- asked-by: plan 1 WU1.7 (from WU1.3); re-housed 2026-09-04
- bears-on: 031
- question: Per bonded pairing, are the two partners designed with unequal interiority?
- candidate-predicate: `asymmetry-design` per pairing
- status: open

### Are the Canon and Reader Prior Belief tracks already doing the AU ambient-field work?
- asked-by: plan 1 WU1.4/WU1.7 (Brian questions whether the framework lacks a term or an architecture); re-housed 2026-09-04
- bears-on: 029, 036
- question: How many notes on those tracks design a canon-versus-AU clash, and is the gap vocabulary or architecture?
- candidate-predicate: classifier per note; count
- status: open

### Does Reader Opinion's display question invite investment (affective) or only opinion (cognitive)?
- asked-by: pre-revision entry 033 @ 2026-09-01T01:30 (WU1.1 post-discussion: the goal exists but the track configuration does not fully invite it); re-housed 2026-09-04
- bears-on: 033
- question: Do Reader Opinion notes in the plan carry affective-investment content despite the display question's cognitive framing, and at what rate?
- candidate-predicate: classifier per note (opinion / investment / both)
- status: open

### Does Track 99's usage directive prescribe FID, and do its notes follow?
- asked-by: 029's and 028's created entries; plan 1 WU1.13 area 3; re-housed 2026-09-04
- bears-on: 028, 029, 031
- question: The directive's text (`get_track_definitions`), and whether the track's notes design gaps by FID only or by other deliveries.
- candidate-predicate: `gap-delivery` per note on the track
- status: open

### How much AI voice is in the working plan?
- asked-by: plan 1 WU1.4 downstream (the lint report: the same attribution tool on the v2 plan, read-only); re-housed 2026-09-04
- bears-on: 019, 020
- question: Running `VoiceAttribution` against the working plan with the same lineage and settings (read-only, no persistence), the label and role census by track and subject type.
- candidate-predicate: census by script over the v2 CSV
- status: open

### Does the Character Development track assume a direction?
- asked-by: plan 1 WU1.13 area 3; re-housed 2026-09-04
- bears-on: 033
- question: Do any Character Development notes design stasis or decline under the current display question, and how many?
- candidate-predicate: classifier per note (growth / stasis / decline)
- status: open

### What does "Hasbro mandate versions" mean in Brian's fabula notes?
- asked-by: plan 1 WU1.14 item 7 (Brian questions whether the Faust-era distinction is as stark as assumed); re-housed 2026-09-04
- bears-on: 037
- question: Which fabula notes characterise Mane 6 versions as mandate-era, and what specifically differs from what is "brought back"?
- candidate-predicate: search-enumerated notes; per note, the stated difference extracted verbatim
- status: open

### Where does the v2 data already hold scene-level design, per story?
- asked-by: plan 1 WU1.14 item 2; re-housed 2026-09-04
- bears-on: 044, 037
- question: Per story, plot points with at least one link note by track type — a readiness census, no ranking.
- candidate-predicate: census
- status: open

### How is Celestia designed in the working plan?
- asked-by: plan 1 WU1.9 — Brian's recall (inscrutability → retroactive recontextualization); re-housed 2026-09-04
- bears-on: 029
- question: Do Celestia's subject and link notes design hidden knowledge for later disclosure, with ids?
- candidate-predicate: focused reader under a frozen rule
- status: open

### Which syuzhet-shaped v1 loci have no v2 counterpart?
- asked-by: the v1 pool's migration question (2026-09-04), v2 side; re-housed 2026-09-04
- bears-on: 044, 041
- question: For each syuzhet-shaped locus in the v1 inventory, does a search of the working plan by name and vocabulary find a counterpart? Never by id.
- candidate-predicate: per locus, hit yes/no; a count
- status: open
