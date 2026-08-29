---
name: meta-analysis
description: Produce a category-level meta-analysis of v4 per-story analyses. Used for 4.2a-e (long corpus categories) and reusable for any future meta-analysis pass. Governs the subagent prompt, output structure, and standing constraints.
---

# Meta-analysis skill — category-level synthesis of v4 per-story analyses

## Purpose

Each invocation reads ALL per-story v4 analyses in an assigned batch and produces a
single meta-analysis report. The report is a peer of Meta-Analysis 4.1a (short corpus)
and will be consumed by the grand synthesis (4.4) alongside the other category reports,
4.1a, and 4.3.

## Provenance

This skill's structure derives from:
- The v4 Analysis Brief's 9 sections (the analytical vocabulary and categories)
- The 4.1a output (session `b229dc5f`, 2026-08-26): 7-section structure that worked
  and that 4.4 expects to read — adapted here for the long corpus
- The 51-story meta-analysis (sessions `4c27bcd7` / `e16846b2`, 2026-08-17/18):
  the original methodology where the hypotheses and v4 Brief were derived
- Brian's directives in the current session: full framework coverage with no tunnel
  vision, no focused/directed questions, no TLTT connection material, category grouping
  is batch management not an analytical filter
- The 4.3 methodology (5 comparison dimensions) as a reference for structured comparison
- `populations.md` as ground truth for which stories exist and their analysis status

## Invocation

A subagent receives:
1. This skill's instructions
2. A category label and list of local file paths to read
3. The v4 Analysis Brief vocabulary reference (embedded below or read from
   `docs/analysis-brief-v4.md`)

The subagent reads every analysis file in full, then writes the meta-analysis.

## Input

Per-story analyses are plain text files in:
`source_material_references/Reading Archive Analyses/`

File naming:
- `<slug>-1m.txt` — single-session 1M analysis
- `<slug>-1m-merged.txt` — canonical merged analysis (two-part stories that were
  truncated and continued; use this, not the individual parts)
- `<slug>-1m-part1.txt` / `<slug>-1m-part2.txt` — The Princess and the Kaiser only
  (deliberate split, no merged version; read BOTH as one analysis)

Read EVERY file assigned to the batch. No sampling, no skipping. The meta-analysis
draws on a complete reading of every per-story analysis. A reader who has read the
analyses will know if you have not.

## Output structure

The report follows a 6-section structure. Each section examines the batch's analyses
through a different lens. The sections are ordered to build on each other.

Begin with a metadata block:

```
META-ANALYSIS 4.2X -- [CATEGORY NAME]

DATE: [current date]
MODEL: [model used]
CORPUS: [N] long-corpus v4 analyses ([list story slugs])
BRIEF VERSION: v4 (all analyses)
PIPELINE: 1M local Claude Code (all analyses)
```

### SECTION 1: CROSS-STORY MECHANISM DISTRIBUTION

The analytical core. For the batch:

1a. Primary Operating Mechanism:
What mechanism does each story primarily operate at (from its Inference Profile)?
Report the distribution. Name each story and its primary mechanism.

1b. Mechanism Presence:
Which mechanisms appear at any level across the batch? Where is each mechanism
present, absent, approached-but-not-sustained?

1c. Instance Patterns:
What patterns emerge in how instances cluster? Do longer stories produce more
instances or denser instances? Do certain mechanism combinations recur?

1d. The M4 Boundary:
How many stories achieve sustained Mechanism 4? How many approach it? What
distinguishes those that sustain it from those that approach but don't reach it?
Report without applying a threshold judgment — describe what each analysis says.

1e. Mechanism 2 Prior Sources:
What sources of prior belief appear across the batch? Canon, genre trope, fanon,
the story's own misdirection? Are there prior sources specific to longer stories
(e.g., priors built across many chapters, then clashed late)?

### SECTION 2: PERSPECTIVE TECHNIQUE PATTERNS

2a. Perspective Mode Distribution:
First person, third-person omniscient, third-person limited, deep third / sustained
FID. What is the distribution across this batch?

2b. Focalizer Patterns:
Single focalizer, dual, multi-focalizer (3+). How is access distributed? Do longer
stories use more focalizers?

2c. FID Prevalence:
How common is FID across the batch? What is the range of FID depth (light, moderate,
heavy, sustained/dominant)? Does FID prevalence correlate with mechanism ceiling?

2d. Direct Thought Usage:
How common is DT? What is the range of usage? Note the relationship between DT
and FID in stories that use both.

2e. Interiority Technique Preferences:
Across the batch, which interiority techniques dominate? Told narration, DT,
behavioral proxy, FID, dialogue, physical sensation? Is the distribution different
from what 4.1a found in the short corpus (told narration dominant there)?

2f. Perspective Discipline:
How consistently do these stories maintain their chosen mode? Are breaches common?
Do they cluster at particular scene types? Do longer stories have more or fewer
breaches?

2g. Character Voice Distinction:
Are characters distinguishable by dialogue alone? Is voice distinction stronger or
weaker in longer stories with larger casts?

### SECTION 3: BOND ANALYSIS PATTERNS

3a. Presence and Applicability:
How many stories in the batch have bond analysis? Romantic vs non-romantic bonds?

3b. Obstacle Architecture:
What obstacle types appear? Characterological, communicative, structural, external
threat? What combinations? Do longer stories develop more complex obstacle
architectures (e.g., type shifts across the story)?

3c. Cross-Patterns:
Asymmetric interiority access, the Spark question, relationship phase granularity,
anything else that emerges across multiple stories.

### SECTION 4: FRAMEWORK FIT — GAPS AND UNNAMED TECHNIQUES

4a. Recurring Framework Gaps:
What gaps do the analyses identify? Organize by frequency of citation.
Compare against the 4.1a gaps (comedy/affect/prose-craft, cumulative effects,
M4 gradient, first-person narration, dialogue as interiority, embedded texts) —
do the same gaps recur? Do new ones appear?

4b. Unnamed Techniques:
Techniques that don't map cleanly to the framework's mechanism categories.
Report each with the stories that exhibit it.

4c. Where the Framework Works Best:
Which analytical tools (mechanism categories, DT/FID distinction, obstacle
architecture, prior-source naming) prove most productive for this batch?

### SECTION 5: CROSS-CUTTING FINDINGS

5a. Theme Proposition Patterns:
What thematic territories do these stories argue? Are they the same five
territories 4.1a found (vulnerability as prerequisite, self-knowledge through
experience, love expressed through behavior, identity as self-authored, authentic
connection requiring being seen) or different?

5b. Counterargument Presence:
What proportion present genuine counterarguments? Is the counterargument deficit
similar to 4.1a's finding (76% with none)?

5c. Meta Stances Patterns:
Faust vs Mandate postures, fanon adoption patterns, content-rating discipline,
anything notable.

5d. Revelation Architecture:
How do these stories manage the reader's knowledge state? Is information architecture
more deliberate in longer stories?

### SECTION 6: METHODOLOGICAL OBSERVATIONS

6a. Corpus Characteristics:
What is distinctive about THIS batch compared to the short corpus? Genre mix,
length distribution, character focus, narrative ambition. These characteristics
shape the patterns above.

6b. Length Effects:
Do any findings correlate with story length? Do longer stories produce different
mechanism distributions, more focalizers, deeper FID, more complex obstacle
architectures?

6c. Analytical Consistency:
Are the per-story analyses internally consistent in vocabulary, section structure,
mechanism assignment? Note any inconsistencies that affect the meta-analysis.

6d. Patterns Unique to This Batch:
What emerges from this group of stories that would NOT emerge from a random sample
of the same size? This is where the category grouping may surface something — but
report what you find, not what you expect to find.

## Standing constraints

- Report what the analyses show. Do NOT rank, score, rate, or grade stories.
- Do NOT recommend what any story should do, or what TLTT should do.
- Do NOT propose craft transplants between stories.
- Do NOT test hypotheses — that is 4.4's job. If a pattern is relevant to a
  hypothesis, report the pattern; do not adjudicate the hypothesis.
- Do NOT add TLTT connection notes or focused questions. The category grouping
  is a batch assignment. Analyze what the stories do, not why they were grouped.
- Counting is presentation. Grading is not.
- An empty result is a finding, not a failure.
- A finding that contradicts 4.1a is MORE interesting than one that confirms it.
  Report divergences prominently.
- Use the v4 Brief's vocabulary precisely. Do not rename, paraphrase, or substitute.
  Mechanisms 0-4, inference stages (Page, World Inference, Meaning), DT, FID.

## Output destination

Write the report as a local text file in:
`source_material_references/Reading Archive Analyses/meta-analysis-4.2X.txt`

File naming:
- meta-analysis-4.2a.txt — Ensemble Stories
- meta-analysis-4.2b.txt — Emotional Romance and Slice of Life
- meta-analysis-4.2c.txt — Dark Premise
- meta-analysis-4.2d.txt — Alternate Universe
- meta-analysis-4.2e.txt — Explicit Content as Plot

Plain text formatting (UPPERCASE section headers, no Markdown characters).
Same format rules as the per-story analyses.

After writing the file, update the 4.2 checkbox in `populations.md`.

## Output format rules

- UPPERCASE section headers on their own line, followed by a blank line
- Subsection headers on their own line followed by a colon
- Quoted text in double quotes
- Numbered or lettered lists and blank lines for visual hierarchy
- Do NOT use Markdown formatting characters (#, **, *, ```, etc.)

## Vocabulary reference

The meta-analysis uses the same vocabulary as the per-story analyses. The full
vocabulary is defined in `docs/analysis-brief-v4.md`. Key terms:

Mechanisms (what the reader does):
- Mechanism 0 (Told): Conclusion stated outright. Zero inference.
- Mechanism 1 (Enacted): Reader observes and concludes. One step.
- Mechanism 2 (Latent): Prior clashes with disclosure. One step, requires reader to bring something.
- Mechanism 3 (Development): Psychological change inferred from behavioral proxy. Two steps.
- Mechanism 4 (Perception Gap): Reader inhabits FID while holding contradicting structural knowledge.

Inference stages (the destination):
- Page: What is on the page.
- World Inference: What the reader concludes about the story world.
- Meaning: What the reader constructs. Either a thematic proposition or a named structural purpose.

The mechanisms describe structural complexity, not quality. A story at Mechanism 2
can be more powerful than one that briefly touches Mechanism 4.

Interiority techniques: Told narration, Direct Thought (DT, marked with *...*),
Free Indirect Discourse (FID, unmarked), behavioral proxy, dialogue, physical
sensation.

DT and FID are NOT mutually exclusive. They are different rendering tools, not a
progression. DT creates a barrier (sympathetic observation); FID dissolves it
(sympathetic inhabitation).

Obstacle types: Characterological (internal transformation needed), Communicative
(would resolve with honest conversation), Structural (embedded in world conditions),
External threat.
