# Spec pool — 112-story analysis corpus

Questions about the 112-story analysis corpus (the per-story v4 analyses and the seven
meta-analysis reports; verification reads the story text, never the analysis alone)
awaiting a verification pass. Format and rules: `README.md` in this directory. Append only;
a superseded entry changes its `status` line.

Created 2026-09-03 as an empty shell; seeded 2026-09-04 by forward-plan-2 from (a) every
pre-revision evidence entry that cites this corpus — each is unverified and is re-housed
here as the question it answered, with its original timestamp — and (b) every plan-1
testing spec that this corpus could verify. "Brian's recall" marks a question whose origin
is recall; recall is a question, never evidence.

### Do the three concerns vary independently across stories?
- asked-by: pre-revision entry 023 @ 2026-08-31T23:30 (WU1.1); re-housed by forward-plan-2 (2026-09-04)
- bears-on: 023, 024
- question: In a sample of stories read at source, do goal, mechanism and rendering technique combine without nesting — the same mechanism serving different goals, the same technique at different mechanism levels, the same goal via different techniques?
- candidate-predicate: per designed moment (analysis-located), label goal / mechanism / technique under a codebook; test for constraint between the three columns
- status: open

### Does M1 hide more variation than the levels between it and M2?
- asked-by: pre-revision entries 025 @ 2026-08-31T23:30 (WU1.1, two entries); re-housed 2026-09-04
- bears-on: 025
- question: Read at source, is the range within M1 (explicit statement) wider than the range between adjacent mechanism levels, and do the analyses' M1-dominance figures (75% of the short corpus) hold when M1 instances are re-classified under a frozen rule?
- candidate-predicate: `perspective-mode` / mechanism codebook over analysis-cited M1 loci; distribution of sub-kinds within M1
- status: open

### Does the DT/FID choice determine whether M4 is available?
- asked-by: pre-revision entry 026 @ 2026-08-31T23:30 (WU1.1); entry 028 @ 2026-08-31T23:30 (the ensemble 6/6 zero-M4-with-DT-only finding); re-housed 2026-09-04
- bears-on: 026, 028, 031
- question: In stories whose analyses classify interiority as DT-only, is M4 (classical FID-delivered irony) absent at source, and in FID-using stories present — i.e. is the rendering-mode → mechanism-ceiling coupling real in the text, not only in the analyses' section structure?
- candidate-predicate: per story, `perspective-mode` majority class × presence of any M4 passage under a frozen M4 rule
- status: open

### Does the narrator-character blend exist distinctly in the corpus?
- asked-by: pre-revision entry 030 @ 2026-08-31T23:30 (WU1.1) and iteration 030 @ 2026-09-01T00:30 (source review of Where Earth Meets Sky, Salvation, Best Night Ever, filly-fooling); re-housed 2026-09-04
- bears-on: 030, 027
- question: Across FID passages read at source, is there a class where the vocabulary is demonstrably the narrator's rather than the character's, and does it cluster with vocabulary-limited focalizers (the Salvation / Rainbow Dash observation)?
- candidate-predicate: `perspective-mode` with a narrator-vocabulary flag per passage; count per story; co-occurrence with focalizer
- status: open

### Do the richest perspective effects use both DT and FID?
- asked-by: pre-revision entries 028 @ 2026-08-31T23:30 (WU1.1, two entries: P&K / Salvation / Not Unless You Mean It use both; FID prevalence does not rise with length); re-housed 2026-09-04
- bears-on: 028
- question: At source, do P&K, Salvation and Not Unless You Mean It shift between external narration, close third, FID and DT within scenes (variable focalization), and does FID share per story fail to correlate with length across the corpus?
- candidate-predicate: per-scene mode sequence under `perspective-mode`; Spearman of FID share against word count
- status: open

### Are there at least seven non-FID perception-gap delivery mechanisms in the corpus?
- asked-by: pre-revision entries 029 @ 2026-08-31T23:30 (WU1.1, two entries); re-housed 2026-09-04
- bears-on: 029, 038
- question: Read at source, do the loci the analyses cite for DT-based knowledge asymmetry, first-person unreliability, dual-POV structural dramatic irony, strategic opacity, narrated denial, the adversarial inner voice and the Mother voice each show a reader-holds-contradicting-knowledge structure without FID?
- candidate-predicate: `gap-delivery` codebook over cited loci; per mechanism, count of loci confirmed / not supported at source
- status: open

### Is DT-based knowledge asymmetry present in every category?
- asked-by: pre-revision entry 031 @ 2026-08-31T23:30 (WU1.1); re-housed 2026-09-04
- bears-on: 031
- question: Do the ~6 short-corpus, 4/6 ensemble, 6/15 romance/SoL and 6/11 explicit/plot stories the reports name show, at source, marked-thought passages held against contradicting evidence with no FID present?
- candidate-predicate: `dt-classes` class A over the named stories' DT loci
- status: open

### Do first-person stories produce M4-adjacent effects by four techniques?
- asked-by: pre-revision entry 032 @ 2026-08-31T23:30 (WU1.1); re-housed 2026-09-04
- bears-on: 032
- question: In the 11 first-person stories, do unreliable self-assessment, retrospective temporal collapse, first-person enacted irony and epistolary dual positioning each appear at source, and is classical M4 absent?
- candidate-predicate: per story, presence per technique under a frozen rule
- status: open

### Is comedy present as a delivery register in 85%+ of stories, and almost never primary?
- asked-by: pre-revision entries 033 @ 2026-08-31T23:30 and 033/034 @ 2026-09-01T01:00 (WU1.1, post-discussion: Best Night Ever, Fixing Up Miss Smartypants, Magic Tutor, Carrot Top Season, On a Cross and Arrow); re-housed 2026-09-04
- bears-on: 033, 034
- question: Sampling across categories at source, do comedic beats sit at structural positions (introductions, post-intensity, pre-reveal, irony setup) and serve tonal management / characterization / attachment rather than a thematic proposition?
- candidate-predicate: `comedy-position` per beat; share of beats at a named position; share traceable to a theme proposition
- status: open

### Do non-thematic goals follow pathways that bypass inference?
- asked-by: pre-revision entries 033 @ 2026-08-31T23:30 and 033 @ 2026-09-01T01:30 (WU1.1; emotional investment through accumulation of M1 gestures); re-housed 2026-09-04
- bears-on: 033, 036
- question: For accumulation arcs the analyses cite (wing-wraps, shared meals, nuzzles), do the individual instances at source ask no world inference and reach no theme, and does the payoff land affectively?
- candidate-predicate: per cited instance, inference asked (yes/no) and theme reached (yes/no)
- status: open

### Are shame-about-desire, chosen-family and leadership-and-power category-specific goal families?
- asked-by: pre-revision entry 033 @ 2026-08-31T23:30 (WU1.1: 7/11 explicit/plot, 4/10 dark premise, 5/11 AU); re-housed 2026-09-04
- bears-on: 033
- question: Do the named counts hold when each story's theme-bearing passages are classified at source rather than from the analyses' summaries?
- candidate-predicate: per story, presence of the family under a frozen definition
- status: open

### Does the prose-craft boundary fall on cross-scene architecture?
- asked-by: pre-revision entries 034 @ 2026-08-31T23:30 and @ 2026-09-01T01:00/01:30 (WU1.1); re-housed 2026-09-04
- bears-on: 034
- question: For effects the reports classify as prose-craft (joke execution, atmosphere, voice) versus plannable (prior-belief construction, revelation sequencing, accumulation placement), does a frozen "depends on more than one scene" predicate reproduce the reports' split at source?
- candidate-predicate: per cited effect, cross-scene dependency yes/no versus the report's label
- status: open

### Do embedded texts create a double interpretive layer?
- asked-by: pre-revision entry 035 @ 2026-08-31T23:30 (WU1.1: ~14% of the short corpus; letters, diaries, dreams, fantasy sequences); re-housed 2026-09-04
- bears-on: 035
- question: At the cited loci (Letters From a Secret Admirer, A Certain Type of Chic, Don't Want Perfection, The Notebook, In Everything But Name; the dark-premise dream sequences), does the reader process the embedded content through one frame and its relationship to the embedding narrative through another — and does any corpus story use an embedded text as a subplot transition mechanism (the Friendship Letters comparison, plan 1 WU1.9)?
- candidate-predicate: per locus, two-frame structure present; per story, embedded text at a subplot boundary
- status: open

### Does any corpus story mix dream-as-psychological-evidence with dream-as-infrastructure?
- asked-by: plan 1 WU1.4/WU1.9 spec (dreamscape aid network comparison: Third Time's a Charm, Inner Strength, Salvation, Perfect on Paper, The Sky is Falling, Ribbons and Lace, Controlling Your Desires); re-housed 2026-09-04
- bears-on: 035
- question: In the named stories, is any dream sequence also literal in-world communication or collaboration, or are all of them interiority evidence only?
- candidate-predicate: per dream sequence, in-world functional yes/no
- status: open

### What is the corpus-wide ratio of WI-terminal to T-terminal chains?
- asked-by: pre-revision entry 036 @ 2026-08-31T23:30 (WU1.1: 76% no genuine counterargument; AU 8/11 genuine); re-housed 2026-09-04
- bears-on: 036
- question: With counterargument read against the structural plot's thesis rather than the bond's (the WU1.3 post-review correction), does the romance/SoL deficit survive, and what share of inference chains terminate at structural purpose?
- candidate-predicate: `counterargument` codebook per story; ratio per category
- status: open

### Does the counterargument deficit survive a re-read against the structural thesis?
- asked-by: plan 1 WU1.9 spec (WU1.3 post-review, 2026-09-01); re-housed 2026-09-04
- bears-on: 036, 033
- question: Re-reading a sample of romance/SoL analyses' counterargument sections against source, with the structural plot's thesis as the proposition, how many "none" verdicts become "genuine"?
- candidate-predicate: `counterargument` per sampled story; flips counted
- status: open

### Do story paradigms produce different technique profiles under one vocabulary?
- asked-by: pre-revision entry 037 @ 2026-08-31T23:45 (WU1.1: ensemble M4-absent DT-dominant; dark premise M4 only in the two literary-FID stories; AU M2-dominant); re-housed 2026-09-04
- bears-on: 037
- question: At source, do the per-category profiles hold, and does any story need a category the vocabulary lacks?
- candidate-predicate: per story, mechanism and mode distribution under frozen rules; gaps recorded
- status: open

### Do DT-dominant corpus stories show the two-class DT split?
- asked-by: plan 1 WU1.9 spec (046, 2026-09-01); re-housed 2026-09-04
- bears-on: 046
- question: Classifying DT instances in DT-dominant corpus stories as class A (gap-producing) or class B (told interiority in italics) with 046's sub-types, is the split comparable to Brian's ~3:1, or is class B distinctive to Brian?
- candidate-predicate: `dt-classes` over sampled DT loci per story; ratio per story
- status: open

### Which corpus stories use structural or combined obstacles rather than characterological?
- asked-by: plan 1 WU1.9 spec (moved from WU1.3, 2026-09-01: 13/15 romance/SoL, all ensemble, 10/11 explicit/plot characterological); re-housed 2026-09-04
- bears-on: 033, 037
- question: Per bonded story, is the primary barrier characterological, structural or combined when read at source — and do P&K, Promises and the AU stories with structural barriers cluster differently from the stated dominance?
- candidate-predicate: `obstacle-type` per story
- status: open

### Is behavioral proxy a near-universal secondary technique?
- asked-by: plan 1 WU1.3 spec (from WU1.1: wing movements, displacement behaviours, involuntary responses); re-housed 2026-09-04
- bears-on: 033, 034
- question: In a category-stratified sample read at source, what share of stories use behavioral proxy for interiority, and what are the recurring proxies?
- candidate-predicate: per story, proxy present; proxy inventory
- status: open

### Is asymmetric interiority access the norm in bonded stories?
- asked-by: plan 1 WU1.3 spec (from WU1.1); re-housed 2026-09-04
- bears-on: 031, 037
- question: In bonded stories, does one partner receive more interiority at source, and does the less-accessed partner's feeling carry dramatic tension?
- candidate-predicate: per story, interiority word share per partner; asymmetry threshold
- status: open

### Are bonds demonstrated behaviourally before they are declared?
- asked-by: plan 1 WU1.3 spec (from WU1.1); re-housed 2026-09-04
- bears-on: 033, 034
- question: In bonded stories, does the first verbal declaration follow chapters of behavioral demonstration at source?
- candidate-predicate: position of first declaration relative to first demonstrated gesture, per story
- status: open

### Is Pinkie Pie consistently denied interiority across stories?
- asked-by: plan 1 WU1.4 spec (from WU1.1); re-housed 2026-09-04
- bears-on: 028
- question: In stories where Pinkie appears, is she ever a focalizer or given marked thought at source, or is she inferred from outside throughout?
- candidate-predicate: per story, any Pinkie interiority passage yes/no
- status: open

### Does any corpus story use deliberate sustained perspective breach?
- asked-by: plan 1 WU1.4 spec (from WU1.1: none found; breaches accidental, clustering at intensity and exposition); re-housed 2026-09-04
- bears-on: 028
- question: Re-reading the analyses' cited breach loci at source, are all breaches accidental, and does the clustering hold?
- candidate-predicate: per breach locus, deliberate marker present (framing, recurrence) yes/no
- status: open

### Does the AU ambient Latent field appear as a persistent canon/AU comparison?
- asked-by: plan 1 WU1.4/WU1.7 spec (from WU1.1: every character encounter generates a clash); re-housed 2026-09-04
- bears-on: 029, 036
- question: In AU stories at source, does each character encounter generate a canon-versus-AU clash the reader performs, and is M2 the dominant mechanism as 4.2d states?
- candidate-predicate: per encounter, clash present; per story, M2 share
- status: open

### Is Carrot Top Season's stasis a structurally significant absence of development?
- asked-by: plan 1 WU1.4/WU1.13 spec (from WU1.1); re-housed 2026-09-04
- bears-on: 033
- question: At source, does the refusal to change carry designed reader experience (mounting evidence, no change) rather than merely no arc?
- candidate-predicate: evidence-mounting beats counted; change beats counted
- status: open

### Does content-rating elision do structural work in the corpus?
- asked-by: plan 1 WU1.4 spec (from WU1.1: "the kisses land harder for being the ceiling"); re-housed 2026-09-04
- bears-on: 034
- question: In the analyses that make the claim, does the source show the elided act as the story's physical ceiling and the shown act as its payoff?
- candidate-predicate: per story, ceiling act identified; payoff placement
- status: open

### Is conversation-as-resolution a shortcut only for characterological obstacles?
- asked-by: plan 1 WU1.4 spec (from 4.2b, 4.2c); re-housed 2026-09-04
- bears-on: 033, 037
- question: In stories the reports flag, is the resolving conversation preceded by accumulated behavioral evidence, and is the obstacle it resolves characterological?
- candidate-predicate: per flagged scene, prior behavioral beats counted; `obstacle-type`
- status: open

### Is Celestia a recontextualization vessel in the AU corpus?
- asked-by: plan 1 WU1.9 spec (Brian's recall that his handling follows the same mechanism — the recall is a v1/working-plan question, see those pools); re-housed 2026-09-04
- bears-on: 029
- question: In the four AU stories the reports name, is retroactive disclosure about Celestia the strongest M2 operation at source?
- candidate-predicate: per story, Celestia disclosure locus; prior-belief revision magnitude under a frozen rule
- status: open

### How do Salvation, Dash's New Mom and P&K deliver canon-virtue-as-trap?
- asked-by: plan 1 WU1.9 spec (comparison against Brian's treatment); re-housed 2026-09-04
- bears-on: 033
- question: At source, is the trap delivered by the same mechanism in the three stories (Loyalty/Generosity, Loyalty/Honesty, "power is the only thing that matters"), and what is it?
- candidate-predicate: per story, mechanism label at the trap's payoff locus
- status: open

### Do the explicit/plot stories deliver shame-about-desire by named mechanisms?
- asked-by: plan 1 WU1.9 spec (self-constructed priors demolished; fantasy sequences; split-self DT dialogue); re-housed 2026-09-04
- bears-on: 033, 046
- question: At source in the 7/11 explicit/plot stories, which of the three mechanisms appear, and is the framing romance-centred?
- candidate-predicate: per story, mechanism presence
- status: open

### Do Absolute Favorites cluster on technique profiles?
- asked-by: plan 1 WU1.11 spec; re-housed 2026-09-04
- bears-on: 028, 029, 033, 034, 038
- question: Over verified per-story classifications, do tier groups (`corpus-favorites-tiers.txt`) differ in mechanism profile, perspective mode or obstacle type? A count, never a score; Abandoned-tier analyses may cover unread content.
- candidate-predicate: cross-tabulation of tier × classification
- status: open

### Do Brian's comments name the patterns the analyses name?
- asked-by: plan 1 WU1.11 spec (P&K comments, Pax Chrysalia comments, Comments.md; Brian's analytical voice, register 4 of 021); re-housed 2026-09-04
- bears-on: 038, 021
- question: For each chapter-level comment, does it name a pattern the story's analysis names for that chapter? Absence of a comment is not absence of a reaction.
- candidate-predicate: per comment, match to an analysis-named pattern yes/no
- status: open

### Do length effects hold at source?
- asked-by: plan 1 WU1.1 scope (length effects on mechanism distribution, perspective technique, obstacle architecture); re-housed 2026-09-04
- bears-on: 025, 028
- question: Do the reports' length effects survive when the classifications are re-done at source under frozen rules?
- candidate-predicate: classification × word count
- status: open
