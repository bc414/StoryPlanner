# Master Timeline — overnight implementation, decisions made mid-flight

**Date:** 2026-07-30 (overnight run, Brian asleep). Plan: `C:\Users\Brian\.claude\plans\help-me-plan-out-merry-pizza.md`.
This file records every decision made without you, most consequential first. Nothing here
was in the approved plan's text; everything here was forced by contact with the code or data.

## Morning checklist — updated 2026-07-30 morning after your "run on my live files" go-ahead

- ✅ **DONE (on your instruction):** `convert-world-dates` applied to the live v2 file (360
  converted, 69 re-filed, `note:217`/`note:1321` left for triage), `seed-timeline-defaults`
  applied (6 theaters, 5 pivots), and `convert-world-dates` applied to the v1 archive (its 1
  dated note; track split skipped — no track rows there). All three: 0 violations, post-commit
  verification passed, backups in `Backups/` next to each file. Timeline defaults were NOT
  seeded into the archive (a legacy corpus doesn't need theaters/pivots — flag if you disagree).
  Tests re-run: 145/145. WPF publish confirmed current.
- ⬜ **Still yours:** review/reword the 12 track questions in
  `tools/StoryPlanner.DataOps/configs/world-dates.v2.json` (re-running the op updates prose in
  place); pivot descriptions for 930/978.
- ⬜ **Still blocked — MCP republish:** two live servers hold `tools/StoryPlanner.Mcp/publish/`
  open, and publishing over them fails. Quit the other Claude session(s) (or their MCP
  servers), then `dotnet publish tools/StoryPlanner.Mcp -c Release -o tools/StoryPlanner.Mcp/publish`
  and `/mcp` in each session. **Until then, connected MCP servers run the OLD binary, which
  reads the now-blanked legacy strings — date-range tools and `get_stats` will report ~2 dated
  notes. That's staleness, not data loss.**
- ⬜ Launch the app → Timeline tab. Expect: everything in "(Unplaced)" until you assign
  theaters (Subject placement panel); 362 plot points + 61 undated notes in Triage.

## What shipped (one screen)

- **Core:** `WorldDate`/`WorldDatePoint` structs + notation parser (`WorldDate.cs`),
  `WorldDateLegacy` converter, `WorldDateModel` column↔struct bridge, `Timeline/Eras.cs`
  (derived eras), `Timeline/LanePacker.cs` (optimal greedy packing). Schema: `MasterTimeline`
  EF migration — structured date columns on `Notes`, `TheaterId` on `Subjects`/`PlotPoints`,
  `Fabula*` on `PlotPoints`, `SupportsWorldDateEnd` on `NoteTrackDefinitions`, new `Theaters`
  and `Pivots` tables. All additive; migration applies automatically on next app/tool open.
- **DataOps:** `convert-world-dates` (dates + track split + re-file by notation) and
  `seed-timeline-defaults` (6 theaters, 5 pivots), both idempotent, dry-run by default,
  rehearsed clean against a copy of the real file.
- **MCP:** all date reads structured-first with mechanical legacy fallback;
  `get_notes_in_date_range_*` does true interval intersection ("1007.." on a condition track
  extends to +inf); `get_track_definitions` labels event vs condition tracks;
  `ServerInfo.Instructions` updated. NOT yet republished (see checklist §4).
- **WPF:** Timeline tab (canvas: y = time, x = theaters; condition bars = extent, fixed-size
  event markers = position, year-precision count glyphs, month whiskers, pivot rules, year
  ticks, Ctrl+wheel zoom, click → detail panel, hover → content); right panel: Triage
  (assign-with-confirm), Subject placement, Theater and Pivot management. `NoteView` date field
  now parses/validates the notation (red border + reason on bad input, nothing written).
- **Tests:** 145 pass (was 77). **Docs:** CLAUDE.md, storyplan-data skill, FEATURE-AUDIT B1 → 🟡.

## Decisions needing your reaction

1. **Neither op has been applied to your real files.** Everything was rehearsed on a scratchpad
   COPY of `TLTT v2.storyplan` (WAL sidecars included) with `--apply`: **360 legacy strings
   converted, 69 range notes re-filed to the new condition tracks, exactly 2 unconvertible**
   (`note:217 "?"`, `note:1321 "954-914"` — the two known artifacts), 0 violations, note-content
   checksum intact, post-commit verification passed. CLAUDE.md says anything that writes a
   `.storyplan` is your decision, so the morning commands are yours to run:
   ```
   dotnet run --project tools/StoryPlanner.DataOps -c Release -- convert-world-dates "C:\Users\Brian\Desktop\TLTT v2.storyplan" tools/StoryPlanner.DataOps/configs/world-dates.v2.json
   dotnet run --project tools/StoryPlanner.DataOps -c Release -- seed-timeline-defaults "C:\Users\Brian\Desktop\TLTT v2.storyplan" tools/StoryPlanner.DataOps/configs/timeline-defaults.v2.json
   ```
   (dry runs; add `--apply` when the reports look right). The v1 archive can also take
   `convert-world-dates` (its one dated note converts; the track split skips tolerantly since v1
   has no track rows) — optional.

2. **The 12 track names and display questions are my drafts** — see
   `tools/StoryPlanner.DataOps/configs/world-dates.v2.json`. Event tracks keep the existing row
   ids (so the 288+ point-dated notes stay put); condition twins are new rows named: Life Phases
   (Character), Relationship Phases (Bond), Conditions In Force (Civ System), Activities
   (Organization), Usage (Technology), Historical Study (World Law — the condition side kept the
   old name; the event side became "Discoveries"). Technology's event track was renamed
   "Invention" per the plan's own table. Reword freely and re-run the op — it updates prose in
   place, idempotently.

3. **Pivot descriptions**: only year 870 and 1006/1011 got real description text (from the plan);
   930 and 978 are empty strings awaiting your words. Theater descriptions are all empty.

4. **Legacy `WorldDate` string column kept for now.** The plan says one representation, no
   denormalisation — but the EF migration runs automatically on open (no backup!), while the
   conversion op is yours to trigger. Dropping the column in the same migration would have
   destroyed the strings before the op could read them. So: migration adds structured columns;
   the op moves data in and BLANKS the string; the column itself gets dropped in a later
   migration once both real files are converted. Unconvertible strings stay in the column
   un-blanked (nothing destroyed) and surface on the triage page. All read paths prefer
   structured and fall back to a mechanical legacy conversion, so the app/MCP stay truthful in
   every intermediate state.

5. **Three orphaned MCP servers are running from `tools/StoryPlanner.Mcp/bin/Debug`** (PIDs
   14472, 29740, 2260 tonight) — they predate the publish convention and lock that folder, which
   blocks Debug builds of the Mcp/test projects. The permission classifier (correctly) wouldn't
   let me kill processes, so I built/tested in `-c Release` instead. You may want to kill them.

## Findings that changed the plan's mechanics (no action needed, listed for review)

- **Copying a `.storyplan` requires its `-wal` sidecar.** The first rehearsal copy silently
  lacked yesterday's AddStories migration because those transactions were still in the WAL. The
  envelope's migration-head guard caught it ("2 pending migrations — refusing"), which is the
  guard doing exactly its job. The `storyplan-data` skill's copy example has the same gap
  (updated in the docs pass).

- **Flat date columns instead of an EF owned type.** The plan's §1 named owned/complex type as
  the mechanism; the point ("columns inline into the owning tables, no join, no identity") is
  satisfied by plain nullable-int columns, and an optional owned type whose columns are all
  nullable is exactly the EF configuration with materialization warnings. Models stay row
  vessels; the column↔struct bridge lives in one place (`Core/WorldDateModel.cs`).

- **`WorldDate` struct does not store "interval-ness".** "1007" (event) and "1007.." (condition,
  end TBD) store identically — start-only. The TRACK is the discriminator (new
  `NoteTrackDefinition.SupportsWorldDateEnd` flag), exactly per the plan's "no Kind field"; the
  notation renders "1007.." when the track is a condition track. An earlier draft stored an
  end-slot flag in the value; it could not round-trip through the database and died.

- **Interval semantics of "1007.." in date-range queries:** on a condition track, a start-only
  date extends to +infinity for intersection purposes (in force, end TBD); the same stored value
  on an event track occupies just its year. Implemented in the MCP's `get_notes_in_date_range_*`.

- **New Pure tests: 145 total pass** (was 77): notation round-trip incl. `1007` vs `1007..1007`,
  legacy conversion incl. all four real outliers, era derivation (N pivots → N+1, no
  overlap/gap), lane packing (lane count = max concurrency), the ConvertWorldDates op
  (apply/idempotent/dry-run) against a dedicated fixture, and the populated-file upgrade test
  extended to the MasterTimeline migration.

## 2026-07-30 evening — the four items after the theater-assignment analysis

Built after Brian filled in theaters (11 real + "(Unplaced)") and set the directive that
**year-precision collisions are a first-class permanent state**, not a transitional one.

1. **Collapsible theater columns.** Click a column header on the canvas, or the ◧ button in the
   Theaters panel. A collapsed column becomes a ~26px strip carrying a **density ribbon** — one
   tick per populated year, opacity by item count — so absence stays visible and a hidden
   column's busy periods still read peripherally. "Expand all" resets. Session-only state.
   Order remains reorderable via the panel's ▲▼ (persisted, unlike collapse).
2. **The (theater, year) CELL is now the primary object.** It carries a count, a **stacked
   composition bar** of subject-type hues, and an orange edge when anything inside is flagged;
   clicking opens the full list in the detail panel, each row clickable for its content. The
   header reads *"N item(s), no order known within the year"* — presentation order is stable but
   asserts nothing, because within-cell sequence is genuinely unknown and faking it would invent
   information. This is the "aggregate only the indistinguishable" invariant paying off: the
   glyph is the *honest* rendering, not a degraded marker.
3. **Event lane narrowed 170px → 118px** (plus MinTheaterW 130 → 120). It had been sized for
   long subject names; what the canvas actually draws is the compact cell.
4. **Era collapse**, via a new `Core/Timeline/YearAxis` — a piecewise year→pixel mapper. Eras
   (derived from pivots) each get a Collapse/Expand button in the new **Eras** side panel; a
   collapsed range compresses to a labelled band reading "⟨ X – Y collapsed · N events not
   shown ⟩". Compression is explicit, labelled, reversible, and monotonic inside the range — the
   one sanctioned departure from space=time, exactly because it is authored rather than applied
   silently.

**Verified in the app against a copy of the live converted file:** era collapse (6 eras from 5
pivots; status line updates), theater collapse (4 collapsed; strips + ribbons visible), cell
expansion (*"870 — 1 item(s), no order known within the year"*), and multi-item cells with
readable composition bars in the dense zone (`978 5 events` blue+orange, `993 7 events`
green+purple, `971 Chrysalis and E…` pink for a Bond). 151 tests pass.

**One implementation note worth keeping:** the Theaters panel's collapse control is a plain
`Button`, not a `ToggleButton`. A ToggleButton's `IsChecked` would be a second source of truth
fighting the OneWay binding to `IsCollapsed`, and its UIA `Toggle()` changes state without
raising Click — so the command silently never fired. The content already carries the state.

## 2026-07-30 late — rich hover cards and pinning

The fix for two complaints at once: rotated condition labels, and cells that say "5 events"
instead of what is actually there. Both marks now carry a **rich card**, opened on hover.

- **Interactive popup, not a ToolTip.** It stays open while the cursor is inside it, so long
  notes can be scrolled, selected, and copied — none of which a ToolTip permits. Implemented as
  a `Popup` with `StaysOpen=True` plus a ~260ms grace timer, which exists so the pointer can
  cross the gap from the mark to the popup without it closing.
- **Content:** a condition bar shows its one note in full; a cell shows **every** note it holds,
  in full, scrollable (Brian's call — he rejected per-item previews). Each note renders subject,
  track, date, state, complete body, and — when flagged — its **flag reason in its own tinted
  box**, mirroring NoteView. Flag reasons often carry more substance than the body they qualify;
  the flagged wall governs export and the MCP server, never this surface.
- **Pinning.** The popup has a 📌 button that detaches the card onto a floating layer above every
  pane, freely draggable by its header, closable individually or via "Unpin all". Pinned cards
  deliberately **survive Rebuild**, so zooming, collapsing, or refreshing never destroys a
  comparison in progress. This is the read the side panel structurally cannot do — two notes from
  different theaters and centuries, side by side. Brian's note on the side panel: "not that
  useful; the rich popups are meant to compensate." Left in place, likely superseded.

**Verified:** hover popup screenshot-confirmed on a real cell — "880 · Equestria", teal World Law
rule, full text of the Earth Pony Magic note. The pin button is present and invokable in the
popup's window. **NOT verified:** pinned-card rendering and dragging — dragging needs real
mouse-down/move/up against a live desktop and was left for Brian rather than driving his machine.

**Dating from the cards, and drag-to-date with confirm** (2026-07-30). Four asks, one system:

- **The picker moved into the cards.** `WorldDatePickerControl` now takes an explicit `Note`
  dependency property instead of reading `DataContext`, which is what lets it live in a card
  whose DataContext is a display object. Every note in a hover or pinned card gets a date row:
  compact notation box, ▾ advanced picker, inline error. Both write through the note's own
  validated setter — one write path, three surfaces (note view, card, drop confirm).
- **Card edits persist.** The picker gained an optional `SaveCommand`, invoked after a successful
  Apply or Clear. `NoteView` leaves it unset (saving there is the track section's job); the
  timeline sets it, so an edit from a card saves and re-places the mark.
- **Drag-to-date — the plan §6 item cut overnight, now built.** Two drag sources (a ⠿ knob on a
  triage row for undated items, a ⠿ knob on any note card for re-dating), one drop target (the
  canvas). The drop's Y gives the year through the axis inverse, its X gives the theater from
  the recorded column ranges, and a confirm popup opens **pre-filled but unwritten**. Confirm
  applies; Cancel discards. **No gesture ever writes on its own** — there is no undo in this app.
  A drop on a condition track pre-fills `1007..`, not `1007`: releasing establishes a start and
  says nothing about an end. Plot-point drops carry a theater combo pre-selected from the column.
- **Live ghost on drag-over.** A green rule tracks the cursor across the full canvas width with a
  chip reading the exact result ("1007-06 · Aquileia"), updating continuously. Read-only. The
  rule **snaps to where the drop truly lands**, so the preview can never promise a position the
  write wouldn't honour.
- **Drop precision follows zoom** — and this is the *point* of the feature, per Brian: in year
  view you drag a note out of its 1007 cell to a position inside 1007 and it lands with a month,
  which is how within-year order gets decided **visually** rather than typed. Thresholds:
  under 240 px/year a month is <20px and cannot be aimed at, so a drop claims only a year;
  at 240+ it claims a month; past 1100 px/year (a day is ~3px) it claims a day. Never finer than
  you could have aimed — the same honesty rule as the aggregation ladder, applied to input.
  Since the ⠿ knobs are per-note inside a card, dragging note #3 out of a 17-item cell drains
  that cell one note at a time and spreads them down the year.
  *Gotcha found by a test:* subtracting a ~1000-magnitude year from its own fraction loses
  enough precision that an exact month/day boundary floors into the slot below — a point would
  not survive a round trip through its own fraction. A 1e-6 epsilon absorbs it (1e-6 of a year
  is ~31 seconds, far below any precision the model can express).

**Cards state nothing twice** (2026-07-30, Brian's correction — a range card showed the subject
three times and the track/date twice). The rule: **the card's title is the identity, and the
body never repeats it.**

- The `Title` left the shared body template entirely. The pinned card's drag header carries it;
  the hover popup grew its own header row (title left, 📌 right) to carry it there.
- `CardContent.ShowNoteHeaders` is false for a card holding one note the title already names —
  a condition bar or a placed marker — so the body is the prose alone. True for a cell, where
  each note needs a subject line to be told apart.
- A cell's title carries the year, so each note inside shows **track only**, never the date
  again. `NoteCard.Meta` is track-only; the full "track · date" form is used just for the
  subtitle of single-note cards.
- A single-item cell now has no subtitle at all (it was echoing the note's own track and year).

Net effect on Brian's example: the range card went from name ×3 / provenance ×2 down to name ×1
and provenance ×1, and the two-item cell stopped repeating "990" on every note.

**Clicking a mark pins it** (2026-07-30, Brian's ask): hover is ephemeral, click makes it
persistent. Clicking a condition bar, a cell, or an event marker promotes the hover card into a
pinned, draggable one at the popup's current position — so click and the 📌 button do the same
thing, and the popup closes so nothing duplicates. Clicking the same mark twice does not stack
identical cards (guarded by reference in `PinCardAt`). The marks' `MouseBinding` input bindings
were replaced with a `MouseLeftButtonUp` handler, since the handler needs the event args to fall
back to the cursor position when no popup is open. Side-panel selection still fires on the same
click — unchanged until the panel's future is decided.

**Pinning freezes the card in place** (2026-07-30, Brian's correction — it had been landing in a
fixed top-left cascade). The view passes the popup's own position through screen coordinates
into the pinned layer's coordinate space (`PointToScreen` → `PointFromScreen`, necessary because
the popup is its own HWND), then closes the popup so the pinned card takes its exact place. Card
widths were unified at 360px so pinning never resizes it either.

**Three automation lessons, all cost real time:**

- **Hover testing needs the app genuinely topmost, and verifying that is the hard part.** Roughly
  an hour went into chasing a "regression" where the popup stopped opening. It was never a
  regression: the app had fallen behind VS Code, so the cursor was never over it. The decisive
  diagnostic was hovering a *condition bar*, which still carries a plain `ToolTip` — no tooltip
  window appeared either, proving hover wasn't reaching any mark and the fault was the harness,
  not the popup. **Test that cheap independent signal FIRST** before suspecting your own code.
  `SetWindowPos(HWND_TOPMOST, SWP_NOACTIVATE)` raises z-order without stealing keyboard focus and
  is the least disruptive approach, but it must be re-applied per PowerShell invocation.
- A WPF `Popup` is its **own top-level HWND**. `PrintWindow` on the main window cannot capture
  it, and a UIA search scoped to the main window cannot find it. Enumerate top-level windows
  filtered by the app's ProcessId instead.
- **Scope UIA searches to the app's process.** A desktop-wide search for a button matching
  "pin" hit VS Code's own Pin controls and invoked one of them in Brian's editor. Same class of
  mistake bit twice more: `-match "pin"` also matches "Un**pin** all", so an automated pin was
  immediately undone by its own next iteration.

## Deliberate scope cuts (deferred, not forgotten — all in plan §6, none load-bearing for v1)

- ~~**Era-range collapse**~~ — **built 2026-07-30 evening**, see above.
- **Drag-from-triage onto the canvas.** Triage assignment is a typed-notation + Assign button
  (the confirm-not-gesture rule honored); the drag pre-fill is additive polish later.
- **Viewport/zoom persistence.** Nothing persists UI prefs in this app today; inventing a
  persistence mechanism at 3am for one slider felt wrong. Zoom resets to 12 px/year per launch.
- ~~**Aggregation ladder simplified to two states.**~~ **Restored to three on 2026-07-30
  morning** (your ask): past 240 px/year the view flips to **year view** — conditions stop
  being extent bars and become per-year-band named "∥ in force" strips (identity preserved,
  full list + extents on hover), while events keep their glyph/marker rules. Below the
  threshold it's **survey** mode (bars whose height is extent). The flip is zoom-coupled, not
  a toggle — the strip becomes correct exactly when bars outgrow the viewport — with
  **Survey / Year view toolbar buttons** as one-click shortcuts to either side, and the
  viewport's center year is preserved across every zoom/mode jump (the view reports its scroll
  center to the VM). Both modes screenshot-verified against the converted data. Still true:
  year-precision events never leave their year glyph at any zoom — events unbundle by
  *authoring* month/day precision, not by zooming. The plan's month-zoom tier ("N events in
  March") remains unbuilt until month-dated data exists.
- **No virtualization on the canvas.** ~430 visual elements at current data volume (362 dated
  notes + glyph merging); revisit if the corpus 10×es.
- ~~**Canvas theater headers scroll away**~~ — **fixed 2026-07-30 evening.** The canvas is now
  four Excel-style freeze panes: a pinned theater-header strip (scrolls horizontally with the
  body, never vertically), a pinned year gutter (scrolls vertically, never horizontally), a
  fixed corner, and the body, which is the single scroll authority driving the other two from
  its `ScrollChanged`. Body coordinates now start at the origin — the gutter and header are
  separate panes rather than reserved space inside the canvas. Year *numbers* moved to the
  gutter while their rules stay full-width in the body; pivot labels stay in the body (too wide
  for a 52px gutter) with the year in the gutter styled red. Collapsed columns get a 3-letter
  short name in the header ("Equ/Aqu/Cha/Sky/Her/Sta/Tzi/Zeb/Cry/Ole" stay distinguishable at
  26px) with the full name on hover. Screenshot-verified scrolled to year ~935–998 and several
  columns right: headers and year numbers both hold.

## Smaller unilateral choices (react if any bothers you)

- **Subject-type hues:** Character #3B6EC5 · Bond #C55BA8 · Organization #3F9D53 ·
  Civ System #7E57C2 · Technology #E08A2E · World Law #1FA0A8 · Plot point #C53B3B (italic
  label, story-color border). Legend in the toolbar. Theater columns get alternating neutral
  banding only.
- **Theaters/pivots are NOT in `IViewModelRegistry`** — the Timeline tab is their only
  consumer, so they stay local to `TimelineViewModel` (bound as POCOs). If a second surface
  ever needs them, promote them to the registry then.
- **Theater deletion lives on `TimelineViewModel`** (orphans members to sentinel 0, mirroring
  `TryDeleteStoryAsync`) rather than `ContentDeleter` — it operates on POCOs, not VMs, and
  ContentDeleter's contract is VM-shaped. Same guard semantics, different home; move it if you
  want the letter of the "all guards in ContentDeleter" rule kept.
- **"..1007" (start TBD) renders nowhere on the canvas** — no honest top edge exists; it stays
  in Triage until the start is known. The MCP date-range tool still matches it.
- **`0` as a year is treated as the real year 0** (the banishment) — 7 notes carry it; they
  render at year 0, labeled "0". If any of those were "unknown but early" placeholders, they
  need re-dating by hand.
- **BLB display:** negative years render as "N BLB" in axis labels and status, "-N" in the
  editable notation.
- **The unconvertible pair** (`note:217 "?"`, `note:1321 "954-914"`) keep their legacy strings
  visible in Triage as `was: "?"` / `was: "954-914"` — nothing was destroyed, and assigning
  them a real date from Triage clears the legacy residue.
- **Timeline tab position:** inserted AFTER Subjects (index 5) specifically so
  `App.xaml.cs`'s `SelectedTabIndex = 4` hack didn't need touching.

## Verification record

- 145/145 tests green (`dotnet test -c Release`; Debug is blocked by the orphaned bin/Debug
  servers, see §5 above).
- Both ops rehearsed with `--apply` on a WAL-complete copy of the real v2 file: 0 violations,
  note-content checksum identical pre/post, post-commit re-verification passed.
- App launched against the rehearsed copy; Timeline tab driven via UI Automation. Status line
  read back: "360 dated items · 7 theaters · 400 BLB..1013 · 12 px/year · 61 undated on dated
  tracks (see Triage)"; Triage 423 (= 61 notes + 362 plot points), Placement 263, all six
  seeded theaters + "(Unplaced)" present, condition-bar labels legible in the tree
  (Temberik, Feudal Aquileia, Grover IV's Gilded Age, Skyfall Trade Federation, …).
  Pixel screenshots came back white — the session had no active display to composite to
  (you were asleep; render thread suspended), so visual confirmation is on you this morning.
  The automation tree proves layout/build; it cannot prove paint.
- WPF publish updated (your launch copy). MCP publish pending (checklist §4).

## Findings that removed planned work

- **The DataOps envelope does not need a "Notes allowed-to-change" declaration.**
  `PlanIntegrity.ComputeNoteChecksum` hashes `Id/OwnerType/OwnerId/NoteState/Content/FlagReason`
  only — `WorldDate` and `NoteTrackDefinitionId` are not in the checksum. The migration op
  changes exactly those two fields, so the envelope's "never touch notes" guard passes as-is,
  and its meaning ("never touch note *content*") is preserved unchanged. The plan's §7
  load-bearing note is moot.
