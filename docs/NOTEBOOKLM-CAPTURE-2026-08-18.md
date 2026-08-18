# NotebookLM Capture & Ingestion — Handoff (2026-08-18)

The NotebookLM layer of `lineage.db` (LINEAGE corpus, shipped 2026-08-18). The first pass is
**complete as of 2026-08-18**: every notebook Brian chose to include is captured, dated, and
ingested. This file stays as the procedure for adding or re-capturing a notebook later — the
decisions are made, do not re-litigate them.

## State as of 2026-08-18

| Done | Detail |
|---|---|
| Infrastructure | `tools/StoryPlanner.Lineage` (parser + ingest CLI), `NlmNotebooks`/`NlmTurns`/`NlmNotes` tables in `Desktop/TLTT Lineage.db`, served by `list_lineage`/`search_lineage`/`get_lineage` |
| Captures | Seven notebooks, all staged in `C:\Users\Brian\Documents\NotebookLM Captures\` and configured |
| Dates | **All seven dated** — the standing undated flag is clear |

The seven, with their authored dates (Brian's assignment, read off the NotebookLM
"Recent notebooks" cards on 2026-08-18):

| Slug | Authored date |
|---|---|
| `tltt-story-plan` | 2026-01-13 |
| `perspective-analysis` | 2026-02-02 |
| `tltt-history` | 2026-02-10 |
| `ambition-and-harmony` | 2026-02-11 |
| `refinement-of-aquileian-lore` | 2026-02-13 |
| `prompt-history-2025-12-to-2026-03` | 2026-03-23 |
| `mlp-transcripts-and-tltt-story-plan` | 2026-03-30 |

**The date is a START date, and nothing records an end (decided 2026-08-18).** A capture carries
no last-activity signal at all — the card's date is the only mechanical evidence, and an end
would have to be authored from memory. A long-running notebook therefore shows one date; that it
ran for months is simply not recorded. Considered and declined: range notation in the existing
free-text field (`2026-02-13..`), and a separate `AuthoredDateEnd` column.

**The population is authored, not the full account.** Six notebooks visible on the cards
(`TLTT MD Only`, `All MLP Transcripts`, `Untitled notebook`, `The Predator's Dilemma`,
`Harmonic Ambition`, `TLTT Prompt Only`) were **deliberately omitted** — Brian saved `.htm` only
for the seven he wanted. An omitted notebook is a choice, never a gap; do not "complete" the set
without asking.

## Why it works this way (decided 2026-08-17, five question rounds — see the decision log in
`C:\Users\Brian\.claude\plans\my-story-planner-mcp-synthetic-charm.md`)

- **No export, no API, no Takeout coverage for chats** (verified 2026-08-17). Chat history
  is persistent per-notebook (since 2025-12) but reachable only through the rendered site.
- **Manual saves + parser, NO automation.** Playwright/extension routes were considered and
  declined ("manual saves + parser only"). Do not build capture automation without a new
  decision from Brian.
- **Dates are AUTHORED.** Captures carry no timestamps at all (chat turns have none in the
  DOM). A notebook's date is Brian's assignment in the ingest config — year, year-month, or
  full-day precision, and a **start** date (see above), never inferred from content, names,
  or note ages. The seven ingested on 2026-08-18 are full-day, read off the notebook cards.
  **Undated is a standing
  flag**: the ingest re-prints per-notebook date status on every apply run, and an undated
  notebook "must be revisited manually or through other means" (his explicit instruction).
- **Note bodies are not captured.** The studio panel renders preview cards (title +
  relative age like "179d"); bodies would require a capture with each note opened —
  a possible future decision, currently out of scope. Title-only is disclosed, never
  treated as a defect. (Note relative ages are stored raw; converting them against
  `CapturedUtc` was deliberately NOT done — the authored date is the date.)
- Standing corpus rules apply: provenance never ground truth; retrieval not suggestion;
  lineage is opt-in archeology.

## The capture procedure (Brian's part, per notebook)

1. Open the notebook on notebooklm.google.com.
2. **Scroll the chat panel to the absolute top until loading stops.** History is
   server-side lazy-loaded; an under-scrolled save silently loses older turns. Once
   loaded, the DOM retains all turns (proven: the fully-scrolled 08-13 save yielded all
   172 — exactly the DOM's `chat-message-pair` count ×2).
3. Ctrl+S → "Webpage, Complete" → into `C:\Users\Brian\Documents\NotebookLM Captures\`.
   Only the `.htm` matters; the `_files` folder is ignored.
4. Add an entry to `tools/StoryPlanner.Lineage/configs/lineage.json` under
   `notebooklm.notebooks`:
   ```json
   { "slug": "authored-slug", "file": "Saved Name.htm", "title": "Optional Override", "authoredDate": "2026-03" }
   ```
   `slug` and `authoredDate` are Brian's to author. An `.htm` in the dir with no config
   entry is listed by the dry run as "not configured" — visible, never silently ingested.

## The ingest (agent's part)

```
dotnet run --project tools/StoryPlanner.Lineage -- tools/StoryPlanner.Lineage/configs/lineage.json --source notebooklm            # dry run
dotnet run --project tools/StoryPlanner.Lineage -- tools/StoryPlanner.Lineage/configs/lineage.json --source notebooklm --apply
```

- `--source notebooklm` replaces ONLY the NLM tables (per-source replace; gemini and
  aistudio layers untouched). Re-capturing a notebook and re-running is always safe.
- Dry run prints per-notebook turn/note counts and date status — **have Brian eyeball it
  before `--apply`** (population correctness is authored). A suspiciously low turn count
  means an under-scrolled capture: re-capture, don't ingest.
- Refusals (exit 1): configured capture file missing; a capture parsing to zero turns AND
  zero notes (wrong file).
- After apply, verify from a reconnected MCP session: `list_lineage` shows the notebook
  with its authored date and notes; `search_lineage` reaches its turns (`nlm:{id} t#{n}`).

## Parser facts (for debugging a bad capture)

`tools/StoryPlanner.Lineage/NlmCaptureParser.cs` — dependency-free scanner over the saved
Angular DOM, fixture-tested in `tests/.../Ingest/NlmCaptureParserTests.cs`:

- Structure markers: `chat-message-pair` (one exchange) → `from-user-message-inner-content`
  (user) / `to-user-container` (model); notes are `<artifact-library-note>` cards; notebook
  title from `<title>` minus the " - Gemini Notebook"/" - NotebookLM" suffix.
- `<i>/<em>` → `*…*`, `<b>/<strong>` → `**…**` (italics are load-bearing for Brian's craft
  analysis); `<mat-icon>/<button>/<svg>/<style>/<script>/<chat-actions>` stripped wholesale.
- Reads UTF-8 explicitly — the CP1252 mojibake ("â€™") in the old
  `_perspective_extract.txt` came from the retired extraction, not the capture.
- If Google changes the DOM markers, the symptom is a zero/low turn count in the dry run —
  the parser fails visibly, never silently. Update the markers, run the fixture tests.

## Open items

1. **Note bodies** — still open, still out of scope: the studio panel renders preview cards, so
   most notes are title-only. A capture variant with each note opened would be needed (the note
   parser already stores any body text it finds after the title line). Title-only is disclosed on
   every run, never treated as a defect.
2. **Adding or re-capturing a notebook** — follow the procedure above: capture, config entry with
   authored slug + start date, dry run, eyeball the turn counts, `--apply`. An apply replaces the
   whole NLM layer, so a re-capture is always safe.

Pointers: CLAUDE.md "LINEAGE is the fifth corpus" bullet · `tools/StoryPlanner.Lineage/`
(`Program.cs`, `NlmCaptureParser.cs`, `configs/lineage.json`) · ServerInfo.cs FIFTH-corpus
paragraph · auto-memory `lineage-codesessions-build`.
