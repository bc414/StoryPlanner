# StoryPlanner.VoiceAttribution

Read-only provenance for the notes of a `.storyplan`: does each note's text exist in the
lineage corpus, which dated source holds it earliest, in whose role, and in what structural
relationship. Built 2026-09-02 for WU1.4 (v1 archive mining); the same tool pointed at the v2
working plan is the voice-lint report. It never writes to a plan file. The matching engine and
the label rules are `StoryPlanner.Core/VoiceMatch.cs` (fixture-tested in `VoiceMatchTests`).

## What it measures

- Both sides are tokenised the same way (lowercase alphanumeric runs; punctuation, apostrophes,
  markdown are separators — so "doesn't" is two tokens), then cut into k-word **shingles** (k = 6).
- **Origin** = the source explaining the most matched shingles; per shingle the index keeps the
  *earliest-dated* source, so origin is "earliest source wins". On equal dates the source added
  first wins — a Gemini prompt is added before its response, so a prompt beats the response that
  repeats it.
- **Role** is the origin's role: `brian` (Gemini prompts, AI Studio / NotebookLM user turns) or
  `model` (Gemini responses, AI Studio / NotebookLM model turns). The pre-AI Google Doc is not a
  voice source. The Feb 2026 Note Organizer chats (`aistudio:22–25`) are excluded: their model
  turns re-emit their inputs (Brian's ruling 2026-09-02, verified — see the WU1.4 synthesis).
  The one Gemini response that is a raw `print(open(...))` plan dump is a plan snapshot, not a
  voice source.
- **Label** describes the note's structure against its sources, from the token mask (which
  words any matched shingle covers), the matched runs and the uncovered runs:
  `verbatim` (token and shingle coverage ≥ 0.90, no authored gap) · `edited-paste` (a paste with
  cuts or rewording, no gap ≥ G) · `framed-paste` (a paste plus an uncovered run ≥ G — the
  author's sentence) · `fragment` (a matched run ≥ R inside a note that is otherwise not a
  paste) · `phrase` (shared phrasing only; never attribution) · `none` · `short` (< k words).
  Provisional thresholds R = 8, G = 6, paste scale = 2R.
- **FirstSnapshot** (with `--snapshots`): the earliest dated v1 backup whose text contains the
  note (every TEXT column of every table except `GeminiEntries`; schema-agnostic).
- **PlanFirst**: origin is `model` but the note was in a plan snapshot dated *before* that
  response — the model echoed the plan. Role becomes `brian`; `EchoedBy` keeps the response id.
  Resolution is the snapshot interval.
- **EchoCandidate**: the origin's text has a citation lead-in ("in your notes…", "as you
  noted…") just before the matched passage — the model quoting the author within the same
  snapshot week. A flag, not a verdict.

What it does not do: judge registers (fabula / syuzhet / prose / analytical), decide whether a
pasted proposal was adopted, recognise design patterns, see paraphrase, or resolve ties.

## Usage

```
dotnet run --project tools/StoryPlanner.VoiceAttribution -c Release -- <plan.storyplan> <lineage.db>
    [--snapshots <dir>]            dated backups named "* yyyy-MM-dd.db"; opened immutable
    [--gdoc-snapshots]             add the Google Doc snapshots to the plan-snapshot index only (off)
    [--out attribution.csv] [--exclude-story Paratext]
    [--k 6] [--R 8] [--G 6] [--verbatim-coverage 0.90] [--paste-scale 16] [--max-sources 0] [--min-words 4]
    [--exclude-aistudio 22,23,24,25]
    [--sample 30 [--sample-labels a,b,c] [--sample-flips 30] [--seed 40] [--sample-out file.md]]
    [--verdicts sheet1.md,sheet2.md]
    [--render <dir> --arcs "1-5,6-9" --subjects 1,2 --subject-types "Deferred for a plot point" --manifest read-manifest.md]
```

Progress, the label/origin tables, and the deposit counts go to stderr. `attribution.csv` has
one row per note; `OriginId` is an id `get_lineage` accepts, `SourceWindow` is ±40 words of the
origin around the match, `UncoveredText` the longest run no source explains, `Sources` every
source with a meaningful share. `--exclude-story` affects the summary, sampling and rendering,
never the CSV (which describes the whole file).

`--sample` writes a calibration sheet (N per label, plus every echo candidate and M random
PlanFirst flips with `--sample-flips M`); `--verdicts` crosses a filled-in sheet's verdicts
with the current run's labels. `--render` writes the reading view (one markdown per arc in
chapter → plot point → link order, one per requested subject, and a manifest).

## scan.html — the census view

`scan.html` is a fixed viewer, not generated output: open it in a browser and drop
`attribution.csv` on it. It renders every note in scene order, coloured by role (green brian,
red model) and shaded by label, with filters (label, role, owner, layer, PlanFirst, echo,
stitched, tie, text), per-chapter counts, a detail pane (origin, sources, the passage the origin
explains, the uncovered run, the source window), and a scratch box for notes to Claude (press
`n` on a selected note to add its id). Drop a filled-in calibration sheet (.md) on the page for
the agreement matrix. Nothing is uploaded or saved.
