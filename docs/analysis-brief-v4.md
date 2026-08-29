# Analysis Brief v4 — Local Adaptation

Pulled from Google Drive (Doc ID: `1VUYIhCd70oU0Uyh0sOE88Hf4POxqpRQvaHUYV7oQtEA`)
on 2026-08-26. Adapted for local Claude Code analysis (no Drive read connector).

## Changes from the Drive version

- Removed the Drive-connector backslash-escape asterisk paragraph. Local markdown files
  use standard `*italicized text*` — no backslash escaping.
- Added `CONTEXT` field to the metadata block.
- Otherwise identical to the canonical v4 Brief on Drive.

---

ANALYSIS BRIEF

READING MANDATE

Read the COMPLETE text of the story. Every word, every chapter, beginning to end. Do not sample, skip, skim, or summarize-then-analyze. The analysis draws on a faithful and complete reading. A reader who has read this story will know if you have not.

The text has emphasis preserved as asterisk markers: *italicized text*. This is load-bearing: Free Indirect Discourse is identified partly by the ABSENCE of italic marking. Unmarked narrator-voice text that adopts a character's vocabulary is FID. Do not ignore these markers. Note: not every instance of *...* is Direct Thought -- authors also use italics for emphasis, foreign words, or titles. Use context to distinguish: Direct Thought is a character's inner voice, typically a complete clause or sentence. Brief emphasis within narration (a single stressed word or short phrase) is emphasis, not DT.

OUTPUT FORMAT

Begin the analysis with a metadata block:

BRIEF VERSION: v4
STORY FILE SIZE: [size of the source file in KB]
MODEL: [model used for this analysis]
CONTEXT: [context window and mode, e.g. "1M (full read, no compaction)"]
DATE: [current date]

Follow the metadata block with a blank line, then the analysis title and content.

Write your analysis as a Google Doc with readable plain-text formatting:

- Write section headers in UPPERCASE on their own line, followed by a blank line.

- Write subsection headers on their own line followed by a colon.

- Enclose quoted story text in double quotes.

- Use numbered or lettered lists and blank lines for visual hierarchy.

- Do NOT use Markdown formatting characters (#, **, *, ```, etc.) as they render as literal characters in Google Docs, not as formatting.

The report's length should scale with the story. A one-chapter story yields a concise analysis; a twenty-chapter novel yields a proportionally longer one. Do not pad short stories to fill a template, and do not truncate long stories to fit one.

VOCABULARY REFERENCE

These terms are from a shipped literary analysis framework (mechanism x inference-stage matrix). Use them precisely -- do not paraphrase, rename, or substitute.

Mechanisms (what the reader does):

Enacted: Reader witnesses events on the page and rationally concludes their meaning. One inference step. Every story has this.

Latent: A truth is disclosed that clashes with what the reader previously believed -- from canon, genre trope, the story's own misdirection, or fanon. The clash does the work: prior belief x disclosed fact = revised understanding.

Development: A character's psychology changes in a way that cannot be stated. The author invents behavioral proxy on the page. Two inference steps: proxy to inferred change, then change to meaning. Character and Bond subjects only.

Perception Gap: The reader inhabits the POV character's self-understanding through FID and simultaneously holds structural knowledge that contradicts it. Requires third-person limited with FID. POV character only.

The mechanisms describe structural complexity of the reader's cognitive operation, not the quality or effectiveness of the writing. A story's primary operating mechanism is a finding about its technique, not a grade. A story operating at Mechanism 2 can be more powerful than one that briefly touches Mechanism 4.

Inference stages (the destination):

Page: What is on the page -- said, shown, done, disclosed.

World Inference: What the reader concludes about the story world. The intermediate step.

Meaning: What the reader constructs from the world inference. If this directly serves as evidence for a thematic proposition, name the proposition as a falsifiable claim. If this moment's primary contribution is structural setup (building a prior belief for a later clash, enriching characterization that will pay off elsewhere, establishing world-building the reader needs), name that function instead. An instance that terminates at World Inference with a named structural purpose is a complete finding -- not every inferential moment carries a thematic payload.

The inference ladder (Mechanisms 0-4):

Mechanism 0 (Told): Conclusion stated outright. Zero inference.
Mechanism 1 (Enacted): Reader observes and concludes. One step.
Mechanism 2 (Latent): Prior clashes with disclosure. One step, but requires the reader to bring something.
Mechanism 3 (Development): Psychological change inferred from behavioral proxy. Two steps.
Mechanism 4 (Perception Gap): Reader inhabits FID while holding contradicting structural knowledge.

FID (Free Indirect Discourse): Narrator-voice adopting a character's vocabulary and bias without italic tags or "she thought" markers. The narrator becomes the character's consciousness. Distinguished from Direct Internal Monologue, which is marked with *...* and creates a barrier between narrator and character rather than dissolving it.

Direct Thought can also produce reader-character knowledge asymmetry when the marked thought is demonstrably wrong. This creates sympathetic observation (the reader evaluates from outside) as distinct from Mechanism 4's sympathetic inhabitation (the reader inhabits from inside through FID). Both are valid techniques; DT-based knowledge asymmetry operates adjacent to Mechanism 4 but through a different phenomenological register.

--- ANALYSIS SECTIONS ---

Report each section in the order below. Use the section name as a header.

1. STORY MAP

Brief structural overview.
- Single-chapter stories: one line per scene (setting and key event).
- Multi-chapter stories: one line per chapter (chapter title if any, arc summary), noting major scenes.
Keep it proportional. This is navigation, not analysis.

2. OPENING

What does the first scene or chapter establish? What does it promise the reader -- genre, tone, stakes, whose story this is? How quickly is the focalizer locked in, and what perspective mode is established? What is withheld that the opening could have disclosed? For multi-chapter stories, where is the first payoff -- the moment the reader's investment is rewarded?

Keep this brief. One to three paragraphs.

3. PERSPECTIVE ANALYSIS

This is the primary analysis section. Be thorough.

Focalizer Roster:
Which characters hold the narrative camera? Which never do? How is access distributed?

Mode:
For each major section, chapter, or scene group, classify the perspective -- first person, third-person omniscient, third-person limited (soft), deep third / Free Indirect Discourse. Summarize the whole-story pattern, noting when and why it shifts.

Direct Thought vs FID:
Cite every notable instance of each. This is comprehensive, not sampled.

- Direct Internal Monologue (marked with *...*): Quote the text. Name the character. Note the *...* markers that identify it as Direct Thought rather than FID. Distinguish Direct Thought from emphasis: a complete clause channeling a character's inner voice is DT; a single stressed word within narration is emphasis. Report both uses but label them differently.

- Free Indirect Discourse (unmarked narrator-voice adopting character consciousness): Quote the text. Name the character. Explain how you identified it as FID -- whose vocabulary? whose bias? what self-understanding is being channeled? The key signal is the ABSENCE of *...* markers around text that clearly channels a character's interiority.

The distinction is load-bearing: FID is what makes the Perception Gap mechanism possible. Direct Thought creates a barrier between narrator and character; FID dissolves it.

Interiority Techniques:
Beyond DT and FID, what other techniques does the author use to convey inner life? Told narration ("she felt sad"), behavioral proxy (actions revealing psychology without stating it), physical sensation (bodily experience conveying emotion), dialogue that reveals psychology through what is said or avoided. Which techniques does this author prefer, and which are absent? A story that relies entirely on told narration and DT tags is doing something different from one that works through behavioral proxy and FID -- name the pattern.

Switch Triggers:
When narrative perspective changes, what triggers it? Scene breaks? Physical actions (the "baton pass" -- one character does something that draws the camera)? Section headers? Nothing (head-hopping)? Is the pattern consistent?

Discipline:
For the story's predominant mode, how consistently does the author maintain it? Note breaches -- moments where the narration accesses knowledge or interiority beyond what the established perspective allows. Are breaches deliberate (the author breaking mode for a specific effect) or accidental (head-hopping, information the focalizer could not have)? Do they cluster in certain scene types (battles, emotional peaks)? A story that is strict throughout is a finding; a story that drifts is equally a finding.

Narrative Voice:
What is the story's narrative register? Warm, clinical, sardonic, breathless, lyrical, conversational? Does the voice stay consistent or shift with content? Does the narrator sound distinct from the characters, or does narrator voice blend with the focalizer's consciousness (which is itself a form of FID)? For multi-POV stories: does the narrator's register change when the focalizer changes, or does every character's section sound the same?

Character Voice Distinction:
Do different characters sound different in dialogue and, where applicable, in their FID passages? Note distinguishing markers: vocabulary, sentence length, register, verbal tics, question patterns. For the focalizer(s): does the narrator's voice shift to match each character's consciousness, or does it stay uniform regardless of whose head we are in? If characters are indistinguishable by voice alone, note that as a finding.

Narrative Shortcuts:
Where does the author use a narrative convenience to bypass a harder technique? Examples: telepathy or mind-reading that grants direct access to another character's thoughts, convenient eavesdropping, letters or journals that reveal exactly what is needed, exposition-dump dialogue ("as you know..."). Note the convenience and what craft problem it sidesteps. This is not a fault -- a shortcut is sometimes deliberate and effective -- but name it.

4. INFERENCE ANALYSIS

This section traces show-don't-tell through the mechanism x inference-stage framework. For each mechanism present, report instances where the text does significant inferential work. The goal is to see how the framework captures (or fails to capture) what the story does.

Scale to the text. A short story may yield a handful of instances across all mechanisms; a multi-chapter story may yield many. Do not cap the count arbitrarily -- report what you find. Equally, do not pad: if a story operates mostly at Mechanism 1 with one Mechanism 2 moment, say so.

For Enacted instances (Mechanism 1):
Report instances where the inference from page content to world meaning to theme is non-trivial -- where the reader does real interpretive work, not just noting that a character walked into a room. For each: name the subject, cite the page content, state the world inference, state the meaning (as a thematic proposition if this moment directly argues one, or as a named structural purpose if it serves setup, characterization, or exposition).

For Latent instances (Mechanism 2):
Report EVERY instance. For each, give the full prior reconstruction:
a) The prior belief and its source (canon fact, genre trope, the story's own earlier misdirection, or fanon convention -- name it specifically).
b) The disclosed fact that clashes with the prior.
c) The revision -- what the reader now believes. Note asymmetries: part of the prior may survive while part is demolished.
d) The meaning the revision produces -- as a thematic proposition if this moment directly argues one, or as a named structural purpose if it serves setup for a later clash or recontextualizes prior scenes.

For Development instances (Mechanism 3):
Report EVERY instance. For each:
a) The behavioral proxy on the page (what the reader observes).
b) The inferred psychological change (the first inference step).
c) The meaning (the second inference step -- what the change tells the reader about the character or bond).

For Perception Gap instances (Mechanism 4):
Report EVERY instance. For each:
a) The FID text -- quote the passage. Note the absence of *...* markers (which confirms FID, not Direct Thought).
b) Whose consciousness the narrator has adopted, and what vocabulary or bias identifies it.
c) What the character believes (the self-understanding the FID channels).
d) What the reader knows (the structural knowledge or accumulated evidence that contradicts it).
e) Why the gap matters -- what the reader learns from holding both simultaneously.

If a mechanism is absent from the story, say so. That is a finding about the story's technique, not a gap in your analysis. A story that never reaches Mechanism 4 is operating on other mechanisms -- describe which and why.

Revelation Architecture:
How does the story manage the reader's knowledge state over time? What information is withheld and later revealed? What is disclosed early to create dramatic irony? Is the information architecture deliberate (planted setups that pay off, reveals that recontextualize earlier scenes) or ad hoc? Note the major reveals and what made them land -- or fail to land -- as reading experiences.

5. INFERENCE PROFILE

One paragraph. Where does this story top out? What is its primary operating mechanism? How does the distribution of mechanisms shape the reading experience? Does the story sustain its highest mechanism or touch it briefly?

6. BOND ANALYSIS

Only for stories with a central romantic or significant interpersonal relationship. If the story does not center a relationship, skip this section entirely -- do not force-fit.

The Spark: Why do these two have something special? Data on both sides or only one?

Source Material / Fanon: What canon interactions or fanon conventions does the fic build on? What does it recontextualize?

Relationship Phases: What is the state of the relationship at different points -- conditions (spans, not points)? How long does each phase last?

Reader Opinion Trajectory: How does the reader's opinion of the relationship change from start to end? When is investment earned?

Demonstration: How is the bond shown physically and emotionally on the page? (Not told -- shown.)

Characterization Inference: What should the reader infer about what the bond means to each of them, separately?

Development Inference: What should the reader infer about how the bond is changing?

Obstacle Architecture: What is the nature of the romantic obstacle? Structural (embedded in the world's conditions -- would not dissolve if the characters simply talked), communicative (would resolve with one honest conversation), characterological (one person must transform before the relationship is possible), or external threat (danger that pushes them together or apart)? Multiple types can coexist; name which dominates and when the type shifts.

Perspective x Bond: How does the story's perspective architecture shape the reader's experience of the bond? Does the reader get inside both characters' interiority, only one, or neither? Is the access symmetric or asymmetric? What does the author gain or lose from that choice? If the story switches POV between the partners, how are the transitions handled?

7. META STANCES

Positions the work takes toward its source material, medium, and commercial conditions -- not toward its own world. Answer each. "Not applicable" or "the story does not engage with this" is a valid answer.

1. Faust vs Mandate: Which era's characterization does this fic treat as the real one? Four postures: accept the Hasbro-mandated version as baseline, rescue it with an in-universe explanation, subvert it, or diagnose it (treat the mandate itself as a pathology inside the world). Which version of each major character is this?

2. Doylist / Watsonian ratio: For inherited canon oddities the fic touches, does it explain in-universe, ignore, or lampshade?

3. Canon-fidelity decisions: Which canon events are accepted, expanded, recontextualized, or rejected?

4. Fanon adoption: Which fandom conventions does the fic take as given?

5. Onboarding cost: How much must a reader know before the story gives anything back? Where is the first payoff?

6. Authorial fiat: Where does the plot happen because the author needed it to? (Descriptive, not a fault -- a contrived proximity device is often deliberate and effective.)

7. Triangulation: What will readers mistake this for, and what does the text do to disambiguate? How early does the first page settle it?

8. Content-rating discipline: What does the fic refuse to show, and what does it gain from the refusal?

9. Paratext posture: Does the author explain themselves in notes? Before or after the text? Does the note reframe the reading?

8. THEME PROPOSITIONS

What falsifiable claims does the whole story argue? State each as a proposition, not a topic word. ("Strength is the prerequisite for mercy," not "mercy.")

Scale to the text: a story may argue one clear proposition or several. Report what you find. Prioritize propositions the text provides the most evidence for.

Counterargument: Does the story present a genuine counterargument to its own thesis -- an opposing position with real merit that the story must honestly defeat rather than simply dismiss? Does the antagonist or opposing force have a defensible point? If the counterargument is absent or a straw position, note that as a finding.

9. FRAMEWORK FIT

How well does the mechanism x inference-stage framework capture what this story does?

Where does it fit cleanly -- which techniques map directly onto the mechanisms and inference stages?

Where does the story do something the framework's categories do not fully capture? Are there narrative techniques that fall between mechanisms, or effects that the Page / World Inference / Meaning column structure does not describe well?

This section tests the framework against the story, not the story against the framework. Be honest about gaps in both directions.

STANDING CONSTRAINTS

- Read the ENTIRE text. Every word, every chapter. Do not sample.
- Do NOT rank, score, rate, or grade. Report what the text does.
- Do NOT recommend what any other story should do based on this one.
- Do NOT propose craft transplants ("this technique would improve X").
- Counting is presentation. Grading is not.
- An empty result is a finding, not a failure.
- Reference use only. Fimfiction robots.txt: ai-train=no, use=reference.
- The framework operates at the scene level. Cumulative effects across chapters -- motif repetition, structural rhyme, progressive escalation -- are a real dimension of craft that this analysis identifies when present but does not reconstruct as an arc. That is the work of the author's own planning system.
- Comedy, atmospheric immersion, and narrative voice are prose-craft dimensions the framework intentionally does not measure. The framework captures inferential work (what the reader concludes). It does not capture affective work (what the reader feels while concluding it) or performative work (why something is funny). When noting these dimensions, name them as the prose-craft layer rather than as a framework gap.

CHANGELOG

v4 (2026-08-17): Renamed "Levels" to "Mechanisms" to clarify structural complexity vs quality ranking. Renamed per-instance "Theme" column to "Meaning" to allow World-Inference-terminal instances with named structural purposes. Added DT-based knowledge asymmetry acknowledgment. Added metadata header to output format. Added standing constraints for prose-craft boundary and scene-level granularity. These changes derive from a meta-analysis of 42 unique stories analyzed under v2/v3.

v3 (2026-08-17): 11 amendments adding Opening, Discipline, DT clarification, Interiority Techniques, Narrative Shortcuts, Obstacle Architecture, Perspective x Bond, Counterargument, Narrative Voice, Revelation Architecture, Character Voice Distinction.

v2 (2026-08-16): 8-section framework replacing v1's exhaustive per-scene data dumps.

v1 (2026-08-15): Initial 9-part framework. Superseded after test runs.
