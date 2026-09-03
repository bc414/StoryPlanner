# StoryPlanner Pocket Reader

A read-only Blazor WebAssembly PWA for reading the `.storyplan` files on a phone. Built 2026-09-02
to replace doomscrolling with sifting through the plan: one random note / subject / scene / scene
link per tap, plus jump-to-context, search, and browse trees.

**No story data is in the page.** You pick the `.storyplan` file(s) on the device; real SQLite
(compiled to WebAssembly) opens them in the browser, and the bytes are kept in IndexedDB so the
next visit needs no re-pick. Nothing is uploaded anywhere, there is no server, and the reader works
offline once installed.

## Using it

1. Close the desktop app. In WAL mode the newest changes live in the `-wal` journal next to the
   file until the app closes; a copy of the main file alone would silently miss them.
2. Copy `TLTT v2.storyplan` (and, if wanted, `TLTT v1 Archive.storyplan`) to the phone: Drive, USB,
   Syncthing, anything.
3. Open the reader, tap **Files**, pick each file into its slot. The header shows the file name and
   pick time so staleness is visible.
4. **Random** is the landing view. The unit picker (note / subject / scene / scene link) and the
   corpus toggle (working plan / v1 archive / both) are the only controls. The draw is uniform;
   the only non-uniformity is a short no-repeat ring so *next* does not show what you just read.
5. To update, repeat steps 1–3. **Forget** clears a slot from the browser.

## Rules it keeps

- **Read-only, forever.** No code path writes a `.storyplan`. The SQLite connection is opened
  `Mode=ReadOnly`.
- **Never migrates.** A file whose applied migrations differ from the ones compiled into
  `StoryPlanner.Core` is refused with a message saying which side is behind
  (`Services/MigrationGate.cs`). Open the file in the desktop app to migrate it; rebuild the reader
  to teach it a newer schema.
- **Flagged notes are shown, marked, with their flag reason.** The flagged wall exists for LLM
  consumers; this is the author reading his own data, the same reasoning as the app's global search.
- **Archive states are open / flagged / closed**, never "confirmed" (the v1 file's `Confirmed`
  means review-closed, disposition not recorded).
- **Retrieval, not suggestion.** No ranking, no weighting, no filters, no "interesting" ordering.
- **Preferences live in `localStorage`** (unit, corpus toggle). The app's rule that UI settings live
  in the `.storyplan` cannot apply here: the reader cannot write the file, and these are per-device.

## Building

Requires the `wasm-tools` workload (`dotnet workload install wasm-tools`); `WasmBuildNative=true`
links SQLitePCLRaw's `e_sqlite3` into the runtime.

```
dotnet run --project tools/StoryPlanner.PocketReader            # local dev server
dotnet run --project tools/StoryPlanner.PocketReader -- --urls http://0.0.0.0:5210   # reachable from a phone on the LAN
dotnet publish tools/StoryPlanner.PocketReader -c Release        # publish/wwwroot is the static site
```

`.github/workflows/pocket-reader.yml` publishes to GitHub Pages on push to `master`. Pages must be
enabled once in the repository settings with source "GitHub Actions". The repository is public and
the page carries no data, which is why this is fine.

## Where the pieces are

- `Services/PlanStore.cs` — file intake, IndexedDB round trip, migration gate, `PlanCache.Build`.
- `Services/RandomDraw.cs` — the pool and the uniform draw (Pure-tested in `tests/`).
- `Services/Labels.cs` — every display string resolved through `PlanCache` lookups.
- `Components/` — `NoteCard`, `TrackGroups`, `SubjectView`, `PlotPointView`, `LinkView`, `ItemView`, `PlanPicker`.
- `Pages/` — `Home` (random), `Entity` (subject / plotpoint / link / note routes), `Search`, `Browse`, `Plans`.
- `wwwroot/storage.js` — the only JavaScript: IndexedDB and localStorage.
