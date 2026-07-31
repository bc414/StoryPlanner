# Timeline — proposed refactoring backlog

**Written 2026-07-30**, from the post-build audit of the timeline arc. This is the *forward*
worklist; history and rationale live in `TIMELINE-IMPLEMENTATION-DECISIONS.md`. Items are
ordered by recommended execution. Already done from the same audit (not repeated below): the
namespace flatten, feature-first folders, and the `TimelineViewModel.cs` one-class-per-file
split (24 files in `WindowedStoryPlanner/Timeline/`).

Ground rules for whoever executes: work under the organization rules in the `wpf-conventions`
skill; run `dotnet test tests/StoryPlanner.Tests` (157 green as of writing) before finishing;
republish per CLAUDE.md when `WindowedStoryPlanner/` or `StoryPlanner.Core/` change.

---

## 1. Latent bugs (small, real — do first)

**1a. Hover-card date picker self-destructs.** The hover popup closes ~260ms after the pointer
leaves `HoverPopupBorder`; the advanced picker's own `Popup` is a separate HWND, so moving into
it counts as leaving — the hover card closes under the open picker and unloads it. Pinned cards
are unaffected. Fix options: suspend `_closeTimer` while a descendant popup is open, or make the
interaction rule "hover to read, pin to edit" and show the date row only on pinned cards (the
cleaner rule; Brian's call which). File: `Timeline/TimelineView.xaml.cs`.

**1b. Double hover surface.** `ConditionBarItem` and `EventMarkerItem` templates still carry
`ToolTip="{Binding Tooltip}"` alongside the rich-card handlers — both appear at once. Remove the
ToolTips there, and stop building the now-redundant full-content `Tooltip` strings for those
items in `Rebuild()` (wasted allocation, second stale render path). Cells already dropped theirs.

**1c. Doc drift.** `storyplan-data` skill line ~299: the coverage recipe still counts
`SUM(WorldDate <> '')` — against a converted file that returns 2 (the unconvertibles), not 362.
Must count structured columns. Also `count_notes_archive`'s `[Description]` omits the three
dimensions its engine now accepts (`theater`, `dateShape`, `worldDateYear`).

**1d. `Era.Label` doesn't BLB-format.** Negative years render as "-400" in the Eras panel while
everywhere else says "400 BLB". Move `FormatYear` to Core (e.g. on `WorldDatePoint` or a
`WorldDates` helper) and use it in `Era.Label` — which also kills the WPF-local copy.

## 2. Consolidation into Core (kills duplication + a test smell)

**2a. One `EffectiveWorldDate`.** The structured-first/legacy-fallback read exists twice:
`Mcp/Query.EffectiveWorldDate` and `Timeline/TimelineViewModel.EffectiveDate`. Move to Core
(`WorldDateModel.GetEffectiveWorldDate(this Note)`), both consumers delegate. Same treatment for
the track-shaped notation render duplicated between `TimelineViewModel.DateLabel` and the
`NoteViewModel.WorldDate` getter.

**2b. `PointAtFractionalYear` to Core, kill the test mirror.** The fractional-year→point
conversion (with its 1e-6 epsilon and the 12×31 grid coupling to `WorldDatePoint`) is pure math
living in the VM, and `WorldDateTests` currently tests a *copy* of it. Move next to
`WorldDatePoint`; point the tests at the real implementation; delete the mirror.

**2c. One confirm path for date assignment.** `TriageRow.Assign` is a near-verbatim subset of
`DateAssignment.Confirm` (both re-implement interval-on-event-track refusal with drifting
wording). Triage's Assign button should construct/delegate to a `DateAssignment`. Third copy of
the validation stays where it belongs: the `NoteViewModel.WorldDate` setter, which is the one
write path.

**2d. One palette.** Subject-type hues exist in `TimelineViewModel.SubjectTypeFill` AND
hardcoded in the XAML legend. Single source (static class + `x:Static` from XAML, or generate
the legend from the VM).

## 3. `Rebuild()` extraction + the missing projection tests (the big one)

`Rebuild()` is ~250 lines doing snapshot → grouping → packing → item emission → status, and the
entire canvas ships screenshot-verified only. Extract the pure layout computation (inputs:
snapshot lists, ppy, collapse sets; output: the item collections + status facts) into a
testable class — then write the Fixture-tier projection tests over `SyntheticPlan` that were
promised in the testing discussion:

- collapsed theater → collapsed header + density ticks, expanded columns unaffected
- a 5-event cell → 5 `Entries`/`Notes`, composition widths sum to bar width, flagged edge set
- collapsing an era → its events leave the canvas, one labelled band appears, axis shortens
- year-view flip at the threshold → strips replace bars, glyphs unchanged
- drop-precision: ppy below/above month & day thresholds → year/month/day pre-fill

The VM keeps only orchestration (commands, viewport, persistence hooks). This unlocks tier-1
testing for every future timeline change — highest-value item on this list.

## 4. Features that ought to exist (each small)

**4a. UI-state persistence.** Zoom, theater-collapse set, era-collapse set, viewport center —
all reset per launch; deferred three times, now four consumers. One JSON in
`%LOCALAPPDATA%\StoryPlanner\`, keyed by `.storyplan` path. Nothing else in the app persists UI
prefs, so this creates the mechanism (keep it tiny).

**4b. AutomationIds + cells as real Buttons.** `AutomationProperties.AutomationId` on marks,
toolbar buttons, and side-panel controls so UIA tests stop keying off display text ("◧" broke
this once already). Cells/bars as `Button`s with a `ControlTemplate` additionally gives
`InvokePattern` (no synthetic mouse needed) and first-ever keyboard access to the canvas.

**4c. Esc/click-away dismissal.** Esc closes: hover card → pending drop-confirm → topmost pinned
card (in that priority). Click-away cancels the drop-confirm popup. Currently everything needs
precise travel to a ✕.

**4d. Theater-delete guard into `ContentDeleter`.** The orphan-to-sentinel logic lives on
`TimelineViewModel`, violating the skill's "every deletable entity's guard lives in
ContentDeleter" rule (logged as a deviation at the time; audit verdict: move it). Shape it on
ids, not VMs, per the testing skill's known-gap note.

## 5. Decisions Brian owns (blocked on him, not on code)

**5a. Retire the side panel's "Selected item" section?** His stated position: "not that useful;
the rich popups are meant to compensate." If retired: delete `SelectedCellEntries`, `CellEntry`
(fully redundant with `NoteCard`), `SelectCellEntryCommand`, and the vestigial
`SelectItemCommand.Execute` inside `Mark_Click`. Triage/Placement/Theaters/Eras/Pivots panels
stay.

**5b. Hover-edit vs pin-to-edit** (interacts with 1a — deciding 5b may make 1a's fix trivial).
