# exploration-of-google-keep — leads

## Method

Retroactive. This study is the registry's account of WU1.2, executed on 2026-08-31 under
revision 1 of the method, before studies, reading protocols, the runner or question lists
existed. Scale: pathfinder. One session read the Takeout export's notes through the five
Claude Code analysis artifacts of 2026-08-10 in the same directory as its guide
(`keep-archaeology.html`, `keep-lab.html`, `question-cabinet.html`,
`drive-archaeology.html`, `interpretive-studies.html`, `same-river.html`,
`instrument-report.html`), and searched the lineage corpus through the MCP server for
eight Keep-captured moments in two rounds: exact phrasing, then, after Brian's correction
the same day, the concepts in other words. It wrote `docs/v3-framework/WU1.2-keep-assessment.md`,
this study's source and full account; this artifact is its index of leads. No protocol, no
hash, no arms, no runner run; the model and harness were not recorded. Loci are notes by
date, the analysis artifacts, and the lineage searches. One arm: no disagreement was
measured.

## Questions in view

None from a question list; the lists did not exist. The reading was done with the
revision-1 card's question in view, in the source's words: "Do Brian's Google Keep notes
contain provenance material — early hypotheses, intuitions, corrections — not already
captured in the existing lineage corpora?"

## Leads

**The export.** 5,583 notes as JSON, Oct 24, 2015 to Aug 9, 2026, each with `textContent`,
`title` (88% untitled), `createdTimestampUsec` and `userEditedTimestampUsec` at microsecond
precision, `isArchived`, `isPinned`, `isTrashed`, `color` (all but one default). The
creation timestamp is the highest temporal resolution of any provenance corpus (lineage has
conversation-level dates; the Google Doc diffs are daily). Median note length grew from
31–41 characters (2015–2018) to 98–156 (2025–2026); the largest notes run to several
thousand words. `isArchived` shifted meaning across the years, "answered" in 2015 to
"migrated" in 2026 (question-cabinet analysis). Most notes are grocery lists, Ingress
operations, medical logs, credentials and campus logistics; the existing analysis estimates
300–500 notes with provenance value; about 130 carry plaintext credentials, catalogued by
date in `keep-archaeology.html`'s security appendix.

**Dated notes, pre-AI.** Jul 8, 2025: TLTT's economic thesis ("technology that benefits
everyone, not the top taking all the profits"). Aug 23, 2025 (Taiwan): the Applejack
keystone ("the pony who doesn't wear masks, forced by leadership to wear one, and the final
moral is that no one should have to wear the mask forever, hence democracy"); exact
phrasing absent from lineage, the concept in 55 lineage hits from Dec 4, 2025. Dec 6, 2025:
"Make program for organization of notes", a one-line to-do; no lineage hit even broadly;
the first Gemini discussion of the planner is Dec 29, 2025 (lineage report W01). December
2025: about thirty multi-thousand-word worldbuilding streams (magic as physics, Chrysalis's
love-extraction economy, Harmonic Republicanism), copy-pasted as Gemini prompts in the first
week of AI interaction (lineage W49, Dec 1–7), with 317–379 lineage hits for their
vocabulary.

**Dated notes, craft and identity.** 2022: "KU flaw - no clear plot direction is apparent
and no clear villain". 2023: "So why do I use these italics at all?". Dec 7, 2023: "I find
it funny that I only do original stuff in pokemon but I only do canon stuff in mlp". Dec 4,
2023, 8:35 AM: the MLP pivot, timestamped to the minute. Aug 17, 2024: GIYC's emotional
origin, the real-life brony confession connected to Fluttershy's scene. The creative arc
from THLB (2015) onward has no lineage coverage before Sep 2025.

**Dated notes, AI collaboration.** Feb 2026: "New Grand workflow", Gemini and NotebookLM to
a bucket categorizer to the story planner; the Bucket Categorizer appears in lineage W13
(Mar 23–29), a month later. Mar 2026: "Make sure I am not writing the way ai does / I should
be reading responses and then expressing them myself", no lineage hit. Apr 16, 2026: "I'm
not writing TLTT for an audience to read it, actually. I'm writing TLTT as a more advanced
version of what I was doing in strategy games", no lineage hit beyond a thematic echo on
Apr 19. May 4, 2026: "Story planner text needs to be mine instead of copied from AI because
reading it later will produce memory", no lineage hit.

**The question corpus.** The question-cabinet analysis counts 855 question-notes in ten
species over eleven years; imperative notes fell from 24% to 6% and question notes rose from
20% to 33%, the crossover at the arrival of AI tools; the "staged interrogation" format
(questions queued for a named authority, answers appended after) is morphologically the
same from 2016 ("What is college? What happens?") to 2025 (HVAC cross-examinations), only
the interlocutor changing.

**Uniqueness, two rounds.** Round 1, exact phrasing: all eight moments returned zero hits in
every lineage layer (gdoc diffs, Gemini entries and reports, AI Studio turns, NotebookLM
turns and notes). Round 2, concepts in other words: two of the eight (the economic thesis,
the Applejack keystone) are in lineage as ideas with Keep holding the earlier timestamp; six
(the planner conception, the purpose statement, the craft guardrail, voice separation, the
craft-theory questions, the KU critique) have no lineage echo even broadly. The pattern the
source states: worldbuilding and story-content ideas entered lineage by copy-paste;
self-reflective, metacognitive and craft-methodology notes did not. Three categories fall
out: content-unique, timestamp-unique with the content echoed in lineage, and non-unique
(same-day or next-day paste).

**Ingest facts.** The lineage sidecar pattern exists (own tables in `lineage.db`, a
manifest row in the shared `IngestRuns` ledger, source-prefixed ids); the code-sessions and
NotebookLM ingests are selective by authored include-list or config; a note is identified by
its creation timestamp in the JSON filename; `textContent` suffices for search and
`textContentHtml` adds formatting that rarely matters; the analysis artifacts are large HTML
files that read directly rather than chunking for search.

## Bins

One arm: no disagreement was measured.

## Proposed questions

- Should a Keep ingest be built, selective by an authored include-list, with content-unique
  notes in full and timestamp-unique notes as a timestamp and a pointer to the lineage entry
  that received the paste?
- Is the question-to-imperative shift the hypothesize-gather-iterate cycle, or a
  question-gather-file pattern?
- Should the five analysis artifacts be ingested as a lineage layer or stay read directly?
- Which of the 300–500 candidate notes does Brian judge provenance-relevant?

## Corrections

- 2026-08-31 (Brian's correction, inside the WU): the exact-phrasing test that found all
  eight moments absent from lineage overstated uniqueness, since an idea pasted into Gemini
  appears in other words. Broader concept searches showed the December 2025 worldbuilding and
  two of the eight test items present in lineage; "definitively unique" narrowed to the
  content-unique category.
