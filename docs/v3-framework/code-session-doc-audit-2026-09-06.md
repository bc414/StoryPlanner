# Code-session documentation audit — 2026-09-06

An audit of every Claude Code session from 2026-08-25 through 2026-09-06 for v3 framework
and v3 story-planner **decisions, insights, and todos that are not represented in any
repo or skill documentation.** Requested by Brian; run against the `codesessions.db`
archive (the sixth corpus, `code-sessions` skill).

This is a dated audit record. It is provenance, authoritative for nothing — every gap below
is a pointer back to a transcript, and a transcript is the record of *why*, not of what is
live (CLAUDE.md read-order; the standing transcript trap). Counts are as of the audit date.

## Method

- **Scope read:** all 43 human-rooted *main* sessions in the archive with activity on or
  after 2026-08-25, plus 9 substantial *research/plan* subagents whose task bore on the
  instrument or methodology (lineage/gdoc provenance, WU1.1 execution-plan design, the
  meta-analysis-skill provenance, the ProcessMap build recon). Full transcript of each,
  not a sample.
- **Excluded, by design:** the ~90 remaining subagents of the window. They are autonomous
  execution — `analyze-story` continuation/merge runs, Google-Drive analysis extraction,
  and v1-archive reading passes. Framework decisions are made in dialogue with Brian, which
  happens only in main sessions; a story-reading pass produces story-content findings, which
  CLAUDE.md places outside what Claude Code documents. A keyword sweep over the excluded set
  for decision/todo/methodology language returned only incidental hits, confirming the split.
- **Extraction:** one agent per session pulled every in-scope item (decision / insight /
  todo) tagged by domain (framework · planner · corpora · runner · process), with a verbatim
  quote, date, and Seq range, hard-excluding story-content opinion. 714 items surfaced
  (framework 383, corpora 115, process 100, runner 74, planner 42). Every item was then
  read by hand against the live documentation set below.

## Headline finding

**The v3 work of this window is documented to an exceptional degree.** Almost every one of
the 714 items traces to a live home:

- the 50 hypothesis files and their index;
- the epistemic framework in CLAUDE.md, and `methodology-revision-1.md` / the revision-2
  rulings log, handoffs, and omissions draft;
- `consolidation-1*.md`, `forward-plan-1*.md`, `forward-plan-2*.md`, `WU2.15-plan.md`,
  `implementation-candidates.md`, the spec pools, and `CORPUS-STATUS.md`;
- the `v3-buildout`, `v3-buildout-2`, `agent-runner`, `code-sessions`, and `analyze-story`
  skills;
- `engineering-handoff-2026-09-05.md` for the tool fixes.

The pocket reader, the AgentRunner and its head, ProcessMap, the lineage/gdoc layers, the
code-sessions extraction policy (including the 2026-09-04 human-authored-tool-result clause),
the voice-attribution instrument, the strong-form evidence pipeline, and the whole
work-matrix → activities evolution are all fully captured. What follows is the residue.

## Gaps that want a home

### G1 — the `StoryPlanner.FimComments` tool is non-functional dead code (the corpus itself is documented)

**Corrected 2026-09-06 after Brian's note; the original finding overstated this.** The
Fimfiction API is not accessible to Brian (no API access), so the tool never worked and was
correctly never run — the missing output JSON is that, not a pending task. The corpus it was
meant to acquire — Brian's own comments, his critical-literary-analysis voice — was instead
**downloaded by hand** and already lives in `source_material_references/` (local-only,
gitignored):

- `Comments.md` — his in-depth analytical comments across favorites, with an opening note on
  why most old favorites went uncommented (in-depth analysis only where he did comment);
- `P&K comments.md` (25 comments) and `pax-chrysalia-comments.md` (15 comments + author
  replies) — the two outlier long/actively-updating stories;
- `Filly Fooling review.txt` — a ~2,500-word review essay.

These four are **already documented** in `CORPUS-STATUS.md` under "Supplementary material,"
and the framing (his comments are his own analytical voice, a register distinct from the AI
voices) is hypothesis 021. So there is **no undocumented corpus here.**

What remains is only that `tools/StoryPlanner.FimComments` (committed in `27722ce`) is
non-working dead code, marked as such nowhere — a cleanup candidate, Brian's call: a one-line
header note that the API is inaccessible and the corpus was hand-downloaded, or deletion.
(The raw saves — the two `… - Fimfiction.htm` pages and `Fimfiction Favorites - Sheet1.csv`
— are incidental source for the documented `.md` extractions, not a gap.)

- **Provenance:** session `6f0c2fb1` (2026-08-29) — the OAuth-422 discovery and the
  C#-over-scraping decision, before the API turned out to be inaccessible.
- **Resolved 2026-09-06:** deleted, not annotated. `tools/StoryPlanner.FimComments` was
  standalone — two tracked files from `27722ce`, never modified after, absent from
  `StoryPlanner.sln`, no project reference in either direction, no CI reference, and no
  output artifact on disk — so nothing in the build or the test run was touched. The
  corpus survives documented in the `v3-buildout` skill's `CORPUS-STATUS.md` under
  "Supplementary material"; the source survives in git history and the reasoning in
  session `6f0c2fb1`. Same treatment as `tools/StoryPlanner.AnalysisRunner`
  (`methodology-revision-1.md`). Unrelated and deliberately left in place: the
  `Fimfiction-Comments-Capture` entry in the code-sessions ingest include-list, which
  names a Claude Code project directory rather than this tool.

### G2 — notes carry no date-modified metadata, by decision

**Not documented anywhere** (checked CLAUDE.md, the skills, and the hypothesis files;
hypothesis 007 is about version-labels-as-bookmarks, a different claim).

- **The decision:** working-plan notes should **not** carry a last-modified timestamp,
  because a note maps to a *working hypothesis* — an atemporal, current-snapshot assertion,
  not a revision history. A timestamp would measure Brian's attention, not the content's
  currency; if a "when reviewed" signal is ever wanted it belongs to the baseline flag, and
  a future note-iteration record would timestamp its own entries. The asymmetry with lineage
  and conversations (temporal archives that *do* carry dates) is intentional.
- **Why it matters:** it is a standing constraint on any future note-metadata or NoteState
  work (it bears directly on the `implementation-candidates.md` "NoteState vocabulary change"
  and "Audit mode redesign" candidates), and a plausible future session would otherwise
  re-litigate it or add the column.
- **Provenance:** session `365ef91d` (2026-08-30), restated in `0ca45d8f` / `136d09fe` /
  `969aab24` (2026-08-31). Brian: "notes map to *working hypotheses*, not the revision
  history … Working hypothesis statements don't have timestamps."
- **Home (folded in 2026-09-06):** a design-constraint line under the NoteState
  vocabulary change candidate in `implementation-candidates.md`.

## Noted, but judged not to warrant a doc entry (Brian's call)

- **"Reorganize my folders in the StoryPlanner directory"** — a standing housekeeping todo
  raised 2026-08-31 (`969aab24`), never acted on in-session, and likely overtaken by the
  extensive `fanout/` and `docs/v3-framework/` restructuring since. Vague and probably stale;
  flagged for the record, not documented.
- **MCP server / CLAUDE.md / skills are model- and harness-agnostic** (`365ef91d`,
  2026-08-30) — the server is a plain stdio process and CLAUDE.md/skills port to Codex with
  no repo change; free-tier non-Claude routes were mapped. A portability observation
  tangential to the buildout; the untested "just works" half is already implicitly covered by
  `implementation-candidates.md`'s "Instructional text audit" (the never-audited
  `ServerInfo.Instructions`).
- **"Is Prolog / abstract CS applicable?"** (`365ef91d`) — Brian's open musing, answered
  in-session (Prolog inapplicable under retrieval-not-suggestion and the open-world premise;
  state machines / graph theory / constraint satisfaction merely name what he already does —
  hypotheses 001 and 043, which are documented). The conclusion itself is undocumented but
  low-value.
- **Revision-2 construction residue** — the self-flagged "no home yet" items from the
  2026-09-05 sessions (whether `v1-archive-mining` becomes `protocol-1` or stays a skill;
  the referee's four-goal purpose test; "a worked example silently carries a rule"; copying
  approved plan files into the repo). These are already tracked as open in
  `methodology-revision-2-omissions-draft.md` and the handoff-2 "owed" list, and belong to
  the in-progress revision-2 landing, not to this audit.

## What was deliberately *not* documented

Story-content decisions, findings, and opinions surfaced in these sessions (character,
theme, plot, worldbuilding, canon) are Brian's domain and out of scope for this audit and
for Claude Code documentation generally. Many mined "insights" that read like framework
claims are already the 50 hypotheses — captured, not gaps. Every plan quoted in a transcript
is a proposal, not a shipped feature; nothing here was promoted to "live" without checking
the code and the docs.
