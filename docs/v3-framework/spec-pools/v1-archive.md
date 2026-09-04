# Spec pool — v1 archive

Questions about the v1 archive corpus awaiting a verification pass. Format and rules:
`README.md` in this directory. Append only; a superseded entry changes its `status` line.

### Do the reading conditions fail at different loci?
- asked-by: methodology revision 1 design conversation (2026-09-02); recorded 2026-09-03
- bears-on: 047
- question: Across the WU1.4 exploratory arms (pathfinder read vs per-arc slice readers, same model), are the loci in the "missed by one" bins disjoint between conditions, or do the two conditions miss the same loci?
- candidate-predicate: per locus, present-in-arm(A) × present-in-arm(B) from the mechanically joined record sets; disjointness = share of missed loci missed by exactly one condition
- status: open

### Is the long-context penalty the same size for every model?
- asked-by: methodology revision 1 design conversation (2026-09-02); recorded 2026-09-03
- bears-on: 050, 010
- question: With WU1.4 run as a factorial (two reading conditions × three models, subset arcs for the slice-reader condition), is the pathfinder-vs-slice gap per model larger in its variation across models than the between-model gap at the slice level?
- candidate-predicate: gap(model) = |records(pathfinder, model) Δ records(slice, model)| on the subset arcs; compare spread of gap(model) across models to spread of records(slice, model) across models
- status: open — scope narrowed 2026-09-03 (Brian): Sonnet is not a pathfinder option, so gap(model) exists for Fable and Opus only; the slice-level comparison still spans all three models

### Does the instruction stack change what a classifier sees?
- asked-by: methodology revision 1 design conversation (2026-09-03); recorded 2026-09-03
- bears-on: 049
- question: On the same v1 note set and the same codebook, does an agent run with CLAUDE.md and the buildout skill in context assign framework-vocabulary labels (e.g. FID where the codebook's DT class applies) at a higher rate than an explicit-context runner job?
- candidate-predicate: two arms, identical items and codebook hash, with/without the instruction stack; label agreement rate and the direction of disagreements
- status: open

<!-- Seeded 2026-09-04 by forward-plan-2: plan 1's WU1.4 named checks (each becomes a frozen decision rule in WU2.6) and the twelve target hypotheses as questions. "Brian's recall" marks recall; recall is a question, never evidence. The check-named subject sets are ruled in forward-plan-2 § Rulings. -->

### How much of the archive is AI voice, by owner type and arc?
- asked-by: plan 1 WU1.4 (voice contamination assessment; instrument validated 2026-09-02 — at validation 16% of notes AI pastes, 9% mixed, 3% one borrowed sentence, 72% Brian's, links more contaminated than plot points, subjects most); re-housed 2026-09-04
- bears-on: 019, 020
- question: From `attribution.csv`, the counts by label and role per owner type and per arc, pastes and lifts separately, PlanFirst / Echo / stitched disclosed — is contamination extensive and diffuse (supports 020's prerequisite claim) or localised and separable?
- candidate-predicate: census by script; a localisation measure (share of AI-paste notes in the top-k subjects and arcs)
- status: open

### Are there more than two voices in the archive?
- asked-by: plan 1 WU1.4 (tests whether five registers is the right count); 021's created entry; re-housed 2026-09-04
- bears-on: 021
- question: The instrument yields two roles (brian / model). Do Brian's `none`-tier annotations at calibration (fabula, syuzhet, prose fragment, prior-belief setup, structural-irony setup, subtext) resolve into distinguishable registers under a frozen rule, and does Brian's analytical voice appear in notes distinct from his fabula and design voices?
- candidate-predicate: `register` classifier over a stratified sample of Brian-role notes; inter-class agreement on calibration
- status: open

### Are Gemini pastes proposals whose adoption is readable from the plain neighbours?
- asked-by: 022's created entry; the `v1-archive-mining` skill's per-note adoption reading; re-housed 2026-09-04
- bears-on: 022
- question: For a sample of paste-labelled notes, does a plain note at the same locus or an edit to the paste show adoption, rejection, or neither — and what share of pastes carry hyperbolic-final framing?
- candidate-predicate: per paste, adoption class from neighbours under a frozen rule
- status: open

### Do Brian-voice notes carry traces of the corpus's non-FID gap mechanisms?
- asked-by: plan 1 WU1.4 check (038: DT-based knowledge asymmetry, strategic opacity, narrated denial especially); re-housed 2026-09-04
- bears-on: 038, 040
- question: In Brian-role scene notes, are there designed instances of each named mechanism, and of 040's four replacements (behavioral evidence, designed incomplete understanding, revelation architecture, designed mistakes) as *designed* rather than as written?
- candidate-predicate: classifier per candidate note from the WU2.5 inventory; count per mechanism, absence stated
- status: open

### Do the three candidate design targets appear in the scene graph?
- asked-by: plan 1 WU1.4 check (from WU1.1: information architecture, reader stance trajectory, structural correspondence); re-housed 2026-09-04
- bears-on: 029, 034
- question: Are there loci where notes manage what the reader learns when across scenes, design a shift in reader stance, or plan an echo between scenes — with ids?
- candidate-predicate: per inventory pattern, mapping to a target yes/no; loci listed
- status: open

### Does the early-chapter scene graph handle canon virtues as traps?
- asked-by: plan 1 WU1.4 check (chapters 3–8, Element-named titles); re-housed 2026-09-04
- bears-on: 033
- question: In chapters 3–8's plot point and link notes, is the tension between hopepunk subversion and approaching the trap from another angle designed, and how?
- candidate-predicate: focused reader over the six chapters under a frozen rule
- status: open

### Do the Passion chapter and the Aquileian subjects design shame-about-desire as thematic delivery?
- asked-by: plan 1 WU1.4 check; re-housed 2026-09-04
- bears-on: 033
- question: In CH#13 (chapter:11) and the Aquileian set {232, 269, 230, 271, 276, 280, 281, 282, 284, 297, 298, 236, 418}, is sex-as-thematic-testing-ground designed rather than decorative, with ids?
- candidate-predicate: focused reader per subject and chapter; absence stated per item
- status: open

### Does the scene graph show designed cross-focalizer knowledge asymmetry?
- asked-by: plan 1 WU1.4 check — Brian's recall (dual-POV irony as his foundational mechanism); re-housed 2026-09-04
- bears-on: 029, 031
- question: Are there loci where information placed in one character's scenes is designed to be carried by the reader into another character's scenes, and more broadly designed dramatic irony as a structural pattern?
- candidate-predicate: `+` loci in the inventory crossing focalizers; count and ids
- status: open

### How are the Friendship Letters designed in chapters 10–12?
- asked-by: plan 1 WU1.4 check — Brian's recall (letters as subplot transition points across three chapters); ruled 2026-09-04 as chapters 10, 11, 12 whole (Extraction chapter:28, Tempest chapter:32, Crash chapter:10); search hits outside them: note 2247 (CH#13), note 2021 (CH#23), link note 3625; re-housed 2026-09-04
- bears-on: 035, 029
- question: In those three chapters' plot points and links, do the letters function as information architecture (what the reader learns through a letter versus what the characters know), as dual-POV irony, and as structural scaffolding between subplots — and does the design extend beyond the three chapters?
- candidate-predicate: focused reader over the three chapters under a frozen rule; the outside hits read last
- status: open

### How are dreams designed — psychological evidence, infrastructure, or both?
- asked-by: plan 1 WU1.4 check — Brian's recall (AJ the Collaborator thread; the dreamscape aid network); re-housed 2026-09-04
- bears-on: 035
- question: In subject 264's notes (flagged included) and links and the dreamscape aid network loci (e.g. "Establishing the Dreamscape Aid Network", CH#17), is each dream designed as interiority evidence, in-world communication, or both?
- candidate-predicate: per dream locus, function class
- status: open

### Was the TwiJack perspective switch designed?
- asked-by: plan 1 WU1.4 check; re-housed 2026-09-04
- bears-on: 028
- question: In subject 601's notes and links, is a deliberate perspective breach for TwiJack scenes designed, and is it marked as distinct from the story's default discipline?
- candidate-predicate: focused reader under a frozen rule; absence stated
- status: open

### Do existing v1 notes already do the AU ambient-field work?
- asked-by: plan 1 WU1.4 check (Brian questions whether the framework lacks the term or the architecture); re-housed 2026-09-04
- bears-on: 029, 036
- question: Do canon-reference and prior-belief-shaped v1 notes design the persistent canon-versus-AU comparison, and at what density?
- candidate-predicate: classifier over canon-referencing notes; count
- status: open

### Does Chrysalis's scene graph show designed stasis?
- asked-by: plan 1 WU1.4 check; subject set ruled 2026-09-04 as {12, 612, 266, 216}; re-housed 2026-09-04
- bears-on: 033
- question: In those subjects' notes (flagged included) and scene links, is the reader designed to watch her refuse the growth that would save her — development designed as absent?
- candidate-predicate: focused reader per subject under a frozen rule
- status: open

### Is content-rating elision subverted by design?
- asked-by: plan 1 WU1.4 check — Brian's recall (TLTT and the Kitty of Westkeep show what the corpus elides); re-housed 2026-09-04
- bears-on: 034
- question: Do v1 notes design explicit content as thematic delivery rather than as something elided for restraint, with ids?
- candidate-predicate: classifier over explicit-content-referencing notes
- status: open

### Is Pinkie Pie designed for external-only access?
- asked-by: plan 1 WU1.4 check — Brian's recall (he follows the corpus pattern instinctively); subject 23; re-housed 2026-09-04
- bears-on: 028
- question: In subject 23's notes and its 11 scene links, is Pinkie ever designed with interiority, or consistently to be inferred from outside?
- candidate-predicate: per note, interiority designed yes/no
- status: open

### Does the scene graph design counterarguments?
- asked-by: plan 1 WU1.4 check; re-housed 2026-09-04
- bears-on: 036
- question: Do plot point or link notes articulate an opposing position the story must defeat, and does counterargument density vary with chapter or arc?
- candidate-predicate: `counterargument` classifier per note; density per arc
- status: open

### Which syuzhet-adjacent v1 notes were never migrated to v2?
- asked-by: plan 1 WU1.4 check (reader-experience design, revelation timing, information sequencing not in v2); re-housed 2026-09-04
- bears-on: 044, 041
- question: Of the inventory's syuzhet-shaped loci, how many have no counterpart in the v2 plan by search (name and vocabulary, never id — v1 and v2 never join)?
- candidate-predicate: per locus, v2 search hit yes/no; a count, no proposal
- status: open — the v2 side is a working-plan question too

### Are the TwiJack conversations resolving a characterological or a structural conflict?
- asked-by: plan 1 WU1.4 check (conversation-as-resolution, 4.2b/4.2c); re-housed 2026-09-04
- bears-on: 033, 037
- question: For plot points where Twilight and Applejack talk at length, is the conflict processed characterological or structural, and is it preceded by accumulated behavioral evidence?
- candidate-predicate: per plot point, `obstacle-type`; prior behavioral beats counted
- status: open

### Do v1 notes plan behavioral setups in Brian's later proxy vocabulary?
- asked-by: plan 1 WU1.4 check (a) (ordering audit's 1.3 → 1.4 edge); re-housed 2026-09-04
- bears-on: 033, 034
- question: Do Demonstration- and Character-Actions-shaped v1 notes name the proxies WU2.4's inventory finds in the prose?
- candidate-predicate: per note, proxy vocabulary match against the verified inventory
- status: open

### Do v1 notes design bond evidence behaviourally rather than as declaration?
- asked-by: plan 1 WU1.4 check (b); re-housed 2026-09-04
- bears-on: 033, 034
- question: Of bond-evidence notes, what share design behaviour versus confession or declaration scenes?
- candidate-predicate: classifier per note
- status: open

### Do the links design asymmetric interiority access across pairings?
- asked-by: plan 1 WU1.4 check (c); re-housed 2026-09-04
- bears-on: 031
- question: For bonded pairings, is one partner's link designed with more interiority than the other's?
- candidate-predicate: per pairing, interiority-designed note count per partner
- status: open

### Does note content reveal setup → payoff relationships the link structure cannot represent?
- asked-by: plan 1 WU1.4 (043); re-housed 2026-09-04
- bears-on: 043
- question: How many `+` loci (setup joined to payoff across plot points) does the adjudicated inventory hold, and what relationship types (setup→payoff, parallel, contradicts, revelation chain, accumulation) do they instantiate?
- candidate-predicate: count of `+` loci by type
- status: open

### How dense is v1's scene-level design per plot point?
- asked-by: plan 1 WU1.4 (044); re-housed 2026-09-04
- bears-on: 044
- question: What share of plot points carry at least one design-shaped record in the inventory, by arc and by voice?
- candidate-predicate: census over the inventory
- status: open

### Is the model comparison on the subset arcs enough to choose the full-slice model?
- asked-by: forward-plan-2 (2026-09-04, Brian's staged design for WU2.5); recorded 2026-09-04
- bears-on: 010, 013
- question: On arcs 6–9 and 19–22, do the three slice-reader sets differ in what they miss, and in which bins — the input to Brian's choice of the model for the remaining arcs?
- candidate-predicate: model-effect bins on the subset; per-model missed-by-one counts
- status: open
