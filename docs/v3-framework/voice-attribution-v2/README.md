# Voice attribution over the v2 working plan

The voice-lint census: for every v2 note, which of its text exists in a dated earlier source,
whose role that source is, and how the note is built from it. Mechanical and read-only — it
writes this folder, never the `.storyplan`. The tool, its labels and its columns:
`tools/StoryPlanner.VoiceAttribution/README.md`. The write path (persisting spans into the plan,
surfacing them in the app) stays gated on hypotheses 019 / 020.

**Not for use until calibrated.** The thresholds are v1's, still provisional, and the v2
mechanisms below are new. `calibration-sample.md` is the gate: Brian's verdicts on it, crossed
with `--verdicts`, decide whether any figure from `attribution.csv` is quoted.

## Files

| file | what |
|---|---|
| `attribution.csv` | one row per v2 note, every note including flagged ones (read by script, never whole) |
| `calibration-sample.md` | the verdict sheet — per label, plus each v2 mechanism; flagged notes left off |
| `run-log.txt` | the run's stderr: sources loaded, the label / layer tables, the counts |

Browse the CSV in `tools/StoryPlanner.VoiceAttribution/scan.html`.

## Command

```
dotnet run --project tools/StoryPlanner.VoiceAttribution -c Release -- \
  "C:/Users/Brian/Desktop/TLTT v2.storyplan" "C:/Users/Brian/Desktop/TLTT Lineage.db" \
  --conversations "C:/Users/Brian/Desktop/TLTT v2.storyplan" \
  --plan-sources "v1-plan=source_material_references/v1 sqlite" \
  --plan-sources "v2-plan=C:/Users/Brian/Desktop/Backups/TLTT v2.2*.bak" \
  --plan-sources "v2-plan=C:/Users/Brian/Documents/Backups/TLTT v2.2*.bak" \
  --snapshots "source_material_references/v1 sqlite" \
  --snapshots "C:/Users/Brian/Desktop/Backups/TLTT v2.2*.bak" \
  --snapshots "C:/Users/Brian/Documents/Backups/TLTT v2.2*.bak" \
  --last-modified-guard --out docs/v3-framework/voice-attribution-v2/attribution.csv \
  --sample 10 --sample-flips 10 --seed 40 \
  --sample-out docs/v3-framework/voice-attribution-v2/calibration-sample.md \
  2> docs/v3-framework/voice-attribution-v2/run-log.txt
```

## Rulings (2026-09-28)

Brian adopted the recommended answer on each, to be revisited after calibration:

- **Conversations are a voice source** — v2 may carry pastes from them. Block `Summary` (his
  navigation note) is not indexed.
- **The LastModified check.** A model source dated after a note's `LastModified` day is an echo
  (`after-edit`, role → brian). Conversations are dated by their start; an edit inside a
  multi-day conversation is flagged `ambiguous`, one with no recorded end `span-unknown` —
  flagged, not decided.
- **Plan backups are role-brian span sources**, `v1-plan` and `v2-plan`, loaded after lineage
  and conversations so an AI source wins a same-day tie. They date spans; they are never a
  paste (`PlanSources` / `PlanCoverage`, not the label).
- **A standalone run**, here, rather than a study; it can become WU2.8's census evidence later.
- **Flagged notes:** in the CSV, off anything an LLM reads (the v1 convention).
- **Code sessions** are not a source.

## What is not covered

- **Paraphrase.** Six-word shingles see copying, not rewording: a note reworded from a v1 note or
  a proposal reads `none`. For the question *whose words* that is correct; for *where the idea
  came from* the tool is silent. Deferred until the census shows the size of the `none` pool.
- **Sources not indexed** read as Brian's: the conversations on the ignore list, any chat not
  imported (the imported set ends 2026-08-27), and any other tool.
- **Dating gaps.** No plan backup exists between the last v1 backup (2026-04-18) and the first
  v2 one (2026-07-30); only the LastModified check covers notes written then. Blocks are dated
  by their conversation's start. `TLTT v2 - backup.storyplan` on the Desktop is not used: what it
  is has not been established.
