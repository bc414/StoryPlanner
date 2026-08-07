---
name: wpf-conventions
description: Conventions for the WindowedStoryPlanner WPF/MVVM layer — the ViewModel hierarchy, how relationships are expressed without navigation properties, the create/delete/save patterns, and the traps. Read before writing or modifying anything in WindowedStoryPlanner/, especially before adding a new entity type, a new note-owning type, or a new cross-cutting view.
---

# WindowedStoryPlanner conventions

WPF, `net10.0-windows`, MVVM via CommunityToolkit.Mvvm source generators. ~60 view models, 6
interfaces, **two shallow inheritance families**. The conventions below are already consistent
across the codebase — they just weren't written down, which is the condition under which a
session invents a second way of doing the same thing.

## File and folder organization (adopted 2026-07-30 — see `.editorconfig`)

**One flat namespace per project.** Everything in this project is `namespace WindowedStoryPlanner`,
everything in Core is `StoryPlanner.Core`, regardless of folder (sole exception:
`StoryPlanner.Core.Migrations`, which dotnet-ef generates into). Folders are feature
organization; namespaces are assembly identity; the two are decoupled so files move freely
without touching a using directive or a XAML reference. The `.editorconfig` suppresses the
analyzers that fight this — do not "fix" a namespace to match its folder. In XAML there are
exactly two prefixes for our own code: `local:` (this assembly) and `core:` (StoryPlanner.Core).
Never introduce `vm:`/`v:`-style prefixes again.

**Feature-first folders, no Views/ or ViewModels/ parents.** A feature's views, view models, and
controls live together in one folder — the folder is the working set, the grep scope, and the
diff. Current layout: `Shell/` (app composition: MainWindow, locator, registry, window manager,
settings), `Common/` (genuinely shared machinery: converters, behaviors, NoteView, pickers +
`ScopedPickerController`, ContentFactory/Deleter, OwnerNavigator, TaggedNotesViewModelBase,
CrossCutNoteListView), `Editing/` (the entity-editor machinery — NarrativeElement family, note
tracks, CommonWindow, widgets — deliberately framed as a feature: the libraries are thin
browsers over it), then one folder per feature: `Subjects/ Chapters/ Stories/ PlotPoints/
Themes/ Sources/ Definitions/ Files/ Conversations/ Export/ Timeline/ Search/ Progress/
PropertyGaps/ MissingFields/`. Root keeps only `App.xaml`, `Styles.xaml`, `AssemblyInfo.cs`;
quarantined dead code lives in `RetainedOldViews/`.

The rules that keep it healthy:

1. **One public class per file, file named for the class.** With flat namespaces, filename is
   how a type is found — this rule is load-bearing, not cosmetic.
2. **A feature with more than ~3 files gets a folder; never create a folder ahead of need**
   (the old empty `NewViewModels/` was the cautionary tale).
3. **`Common/` admits a file only on second use.** Promotion, never speculation — otherwise it
   becomes the junk drawer the reorganization existed to kill.
4. **`Models/` in Core stays flat and shared, permanently.** Row vessels are polymorphically
   owned and deliberately not feature-owned; moving `Note.cs` into a feature folder would
   misstate the architecture.

**The architecture this layer sits on is deliberate.** Model classes have no navigation
properties, the schema has no foreign keys and no indexes, and note ownership is polymorphic.
That is a decided design for a single-user, load-everything-at-startup desktop app, not an
oversight — see `CLAUDE.md` and `docs/design-conversations/019_…json` (blocks 126–135). Do not
"fix" it by adding navigation properties or FK constraints.

## The two inheritance families

**`NarrativeElementViewModel : ObservableObject, IDropTarget, IEditorModeAware`** — the
owner-composition base. Its four subclasses are exactly the four `OwnerType` values:

| Subclass | OwnerType |
|---|---|
| `SubjectViewModel` | `Subject` |
| `PlotPointViewModel` | `PlotPoint` |
| `ChapterViewModel` | `Chapter` |
| `PlotPointSubjectLinkViewModel` | `PlotPointSubjectLink` |

It owns the shared behavior: building `NoteTrackViewModel`s and `NarrativePropertyViewModel`s
from the definition rows. **A new note-owning entity type subclasses this** — do not
reimplement track composition, and do not push shared behavior down into the model classes
(they are row vessels with no responsibilities).

**`TaggedNotesViewModelBase : ObservableObject, IDisposable`** — the cross-cutting tag view.
Subclasses: `ThemeDetailViewModel`, `SourceMaterialDetailViewModel` (every note citing a Work at
any depth), `SourceMaterialPartDetailViewModel` (every note citing one specific Part — the
coverage grid's drill-down), `DateRangeNotesViewModel` (every EVENT-track note dated into a world
date range), `MissingFieldNotesViewModel` (every note whose track declares a field applicable but
which carries no value — the criterion inverted). **A new "show me every note where X" surface
subclasses this**, supplies `Matches` + `AffectsMembership`, and renders through
`Common/CrossCutNoteListView` — do not write another copy of the breadcrumb + `NoteView` item
template.

The criterion is always read from the **track** (`SupportsTheme`, `SupportsSourceMaterial`,
`SupportsWorldDate`/`SupportsWorldDateEnd`), never from `TrackType` and never from the shape of the
stored value. A track flag is an authored statement that the field applies; that is what makes an
empty-field view retrieval rather than a machine-generated to-do list. Keep it that way: no
ordering by how worth filling a note looks, no proposed values, no completion percentage.

Three things about this family are load-bearing:

1. **Subclasses use primary constructors, and must.** The base constructor seeds the list by
   calling the virtual `Matches`. Derived *field/property initializers* run before the base
   constructor; a derived *constructor body* runs after it. Assigning the criterion in a body
   therefore dereferences null during seeding — that was a real crash, fixed 2026-07-31. Anything
   `Matches` reads has to be an initializer (`public ThemeViewModel Theme { get; } = theme;`) or a
   field with a default (`private WorldDateRange? _range = null;`).
2. **It is LIVE, and it handles `CollectionChanged` Reset.** `ViewModelRegistry.Clear()` raises
   Reset on every project load, with neither `NewItems` nor `OldItems`. Ignoring it leaves an open
   window showing the previous file's rows and then appending the new file's — two corpora blended
   in one list. The base re-seeds from a `HashSet` of what it actually subscribed to; `Dispose()`
   works off that set, never off the registry (which no longer holds those notes by then).
3. **Disposal belongs to `WindowManager.ShowSingleton`**, not to each window's code-behind, so a
   new cross-cut window cannot forget it. The call site declares
   `ContextLifetime.OwnedByWindow` — see "Who owns a singleton window's DataContext" below, and
   note that a cross-cut VM is the owned case precisely because it was built for that window.

`Notes` stays a bare `ReadOnlyObservableCollection` — the base *is* the membership mechanism, so an
`ICollectionView` filter over it would be a second one that can silently disagree. A subclass that
needs an order overrides `NotesSource` with its own `ListCollectionView` (see
`DateRangeNotesViewModel`'s chronological comparer).

Everything else derives from `ObservableObject` directly and is declared `partial`
(`[ObservableProperty]` / `[RelayCommand]` generate into the other half).

## Relationships without navigation properties

Models carry `int` ids only. Relationship traversal is **filtering over in-memory collections**,
never a navigation property or a database query:

```csharp
// Notes owned by this entity — the polymorphic pair is the join.
_storyService.Notes.Where(n => n.OwnerId == Id && n.OwnerType == OwnerType.Subject)

// Tracks that apply to this entity — definitions know their owner, not the reverse.
_storyService.NoteTrackDefinitions.Where(t => t.SubjectDefinitionId == defId
                                           && t.OwnerType == OwnerType.Subject)
```

`IStoryService`'s collections are `ObservableCollection<T>` projections of
`DbSet<T>.Local`, so adding to or removing from them *is* `context.Add`/`Remove`, and mutating
a POCO property marks it Modified. Bind `ICollectionView`s over them for filtered display.

**These collections are reassigned when a project loads** (`StoryService.LoadDataAsync`), so
never capture a reference before open.

## Mutation: mutate then save

There is no unit of work, no transaction, and no undo. The pattern appears ~39 times:

```csharp
_storyService.Notes.Add(newNote);      // or mutate a POCO property
await _storyService.SaveAsync();
```

Create and delete go through `IContentFactory` / `IContentDeleter` rather than being
open-coded — they also keep `IViewModelRegistry`'s collections in sync, which is easy to
forget:

```csharp
_storyService.Notes.Add(newNote);
await _storyService.SaveAsync();          // Id is assigned here
var vm = new NoteViewModel(newNote, …);
_registry.AllNoteViewModels.Add(vm);      // registry stays in sync
```

Save immediately on create: EF assigns the `Id`, and notes cannot be associated with an owner
until it exists. Do not compute ids yourself.

**Never discard a save Task.** `_ = _storyService.SaveAsync();` throws its exception away: a
discarded Task's fault surfaces only if the GC finalizes it, and .NET does not terminate for that,
so **a failed save is completely silent** — the author keeps editing a project they believe is on
disk. That is the worst failure this app can have, because unlike a crash it has no symptom. When
you cannot await, use the extension instead:

```csharp
_storyService.SaveAsync().FireAndForget();   // observed; reports the failure when it happens
```

`Common/FireAndForgetExtensions.cs`; `[CallerMemberName]` labels the report with the calling
method, so the dialog names the operation that did not persist.

## Deletion is a three-layer system (reshaped 2026-08-02)

The database has no foreign keys, so **application code is the only thing preventing orphaned
rows.** The responsibilities are split so no single caller can get it wrong:

1. **`ContentIntegrity` (Core)** — id-based *guard predicates* (`HasNotes`, `ThemeHasNotes`,
   `SubjectDefinitionHasDependents`, `NoteTrackDefinitionHasNotes`, the source-material and
   narrative-property guards). Pure reads over `IStoryService`, fixture-tested in
   `ContentIntegrityTests`. New guard logic goes HERE, so it stays testable.
2. **`StoryService.DeleteNote` / `DeleteLink` (Core)** — the *unconditional cascades* that must
   hold no matter which UI path triggered the delete: a note takes its `NoteSourceReference`
   rows with it; a link takes its owned `NarrativePropertyValue` rows
   (`RemoveOwnedNarrativePropertyValues` — `NarrativePropertyValue` has an `OwnerId` but **no
   `OwnerType`**, so ownership resolves only by tracing `ValueDefinitionId →
   NarrativePropertyDefinitionId → .OwnerType`; never reinvent that trace). Tested in
   `StoryServiceTests`. Every note/link delete path must come through these.
3. **`ContentDeleter` (this layer)** — the *guarded entity deletes*: consult a `ContentIntegrity`
   predicate, refuse with `false` rather than cascade, keep `IViewModelRegistry` in sync, save.
   Sentinel-orphaning deletes (Story → `StoryId 0`, Theater → `TheaterId 0`) and the one
   documented deliberate cascade (a property definition's provably-unassigned allowed values)
   also live here.

```csharp
public async Task<bool> TryDeleteLinkAsync(PlotPointSubjectLinkViewModel link)
{
    if (ContentIntegrity.HasNotes(_storyService, link.Id, OwnerType.PlotPointSubjectLink))
        return false;                                 // guard, not cascade
    _storyService.DeleteLink(link.Id);                // Core cascade
    _registry.AllPlotPointSubjectLinkViewModels.Remove(link);
    await _storyService.SaveAsync();
    return true;
}
```

**Any new deletable entity needs its predicate in `ContentIntegrity` and its `TryDelete*Async`
here — never an inline `Remove` in a view model.** That inline shortcut is exactly how the
definitions/themes deletes went unguarded for a year (FEATURE-AUDIT F6). A refused delete is
silent by default: pair every refusing command with a status line
(`DefinitionsEditorViewModel.DefinitionStatus` / `NarrativePropertyStatus`,
`ThemeLibraryViewModel.ThemeStatus` are the pattern).

## Editor modes reorganize the UI, and the behavior lives in data

Two cooperating enums. `EditorMode` (Core: `Expansion`/`Linking`/`Gardener`/`Audit`/
`SceneDesign`) is the window-level stance passed to `OpenCommonWindow`, and each value has its
own `*ModeDisplayOrder` column on `NoteTrackDefinition` — entering a mode re-sorts which tracks
surface. `TrackDisplayMode` (`Active`/`Reference`/`Audit`) is per-track and selects which of the
definition's three prose fields becomes the header (`DisplayQuestion` / `UsageDirective` /
`AuditDirective`), whether the track is read-only, and whether notes can be promoted to
`Confirmed` (**Audit only**).

So a mode change alters ordering, header text, editability, and legal state transitions — all
driven by definition rows, not by code. Adding a stance is data entry. Preserve that: put new
per-mode behavior in the definition table, not in `switch` statements.

## Registry, windows, locator

- **`IViewModelRegistry`** is the central lookup for all observable collections. Resolve through
  it rather than passing view model references around.
- **`IWindowManager`** opens windows; `ViewModelLocator` is the root DataContext. Its editor
  entry points are **typed on purpose**: `OpenSubjectWindow(subject, mode, initialLink)` and
  `OpenPlotPointWindow(plotPoint)`. There is no `OpenCommonWindow(EditorMode, NarrativeElementViewModel, …)`
  any more — that shape let any mode pair with any element while `CommonWindow` resolved it with
  an *unchecked* cast (`SubjectViewModel` for Expansion **and Linking**, `PlotPointViewModel` for
  Gardener). Passing a plot point to Linking mode was a compile-clean hard process kill; it shipped
  in Global Search for months before anyone double-clicked a link-owned note. Do not reintroduce a
  loosely-typed opener — the pairing is the compiler's job.
  A note owned by a `PlotPointSubjectLink` opens as its **subject** in Linking mode with the link
  preselected; the plot point is wrong on both counts. `Common/OwnerNavigator.Open(...)` is the one
  implementation — every cross-owner surface routes through it.
- **Views:** UserControls for library and widget surfaces, Windows for detail surfaces.
  Value converters live in `Common/Converters.cs`; drag-drop behaviors in `Common/Behaviors/`.

## Lifecycle: who unsubscribes constructor-era subscriptions (adopted 2026-08-02)

The registry's collections and events, and `AppSettings`, are **app-lifetime singletons** — any
handler attached to them pins its owner (and everything its owner references) until removed. The
convention, after this leak class was found in three places at once:

- **A VM that subscribes must have a teardown, and something must own calling it.**
  `NarrativeElementViewModel` implements `IDisposable`; `ProjectLoader.Load()` disposes every
  element VM it is about to replace *before* `registry.Clear()`. A subclass adding its own
  registry subscriptions overrides `Dispose` and calls base
  (`PlotPointSubjectLinkViewModel` is the example).
- **`NoteTrackViewModel` subscribes in `Initialize()` and unsubscribes in `Uninitialize()`** —
  never in the constructor. Track sets are rebuilt wholesale on every window open; a
  constructor-subscribed track that misses teardown re-scans all notes on every mutation forever.
  `RebuildNoteTracks` tears the old set down fully before clearing; both lifecycle methods are
  idempotent.
- **`SubjectGroupViewModel`** tracks what it subscribed to in a `HashSet` (Reset-safe, same
  discipline as `TaggedNotesViewModelBase`) and is disposed by `RebuildGroups` before
  replacement.
- **`CommonWindow.SelectedLink`'s setter is the ONLY caller of the selected link's
  `OnWindowOpened`/`OnWindowClosed` pair.** An extra explicit call at any site unbalances the
  refcount and silently skips teardown — the `Math.Max(0, …)` clamp hides the underflow, so the
  bug has no symptom.

### Who owns a singleton window's DataContext (stated, not inferred — 2026-08-06)

`ShowSingleton` used to end its `Closed` handler with `(DataContext as IDisposable)?.Dispose()`.
That asks *"is this disposable"* when the question is *"does this window own it"*, and the two
answers diverge: of the nine call sites, three pass a **borrowed** context — `ChapterViewModel` and
`ConversationViewModel` (registry elements) and `FloatingPlotPointsViewModel` (a DI singleton).
Only the first is `IDisposable`, so the other two were correct by accident; giving
`ConversationViewModel` an `IDisposable` later would have silently started destroying a registry
element on every reader close. Every call site now names a `ContextLifetime`, and there is no
default to fall through:

- **`OwnedByWindow`** — built by the factory lambda for this window (every cross-cut VM). Disposed
  on close. If you construct it inside the `create` delegate, it is this.
- **`OutlivesWindow`** — a registry element or DI singleton. Never disposed here; its teardown
  belongs to whoever made it (ProjectLoader, the container). Per-open setup is the refcounted
  `OnWindowOpened`/`OnWindowClosed` pair instead — that pair, not disposal, is what builds and
  tears down note tracks.

Two consequences worth keeping straight. `NarrativeElementViewModel.Dispose()` is **project-lifetime
teardown, not window teardown**: it drops the constructor-era registry subscriptions and nothing
ever re-adds them, so calling it on window close permanently deadens a registry-owned VM. And
`ProjectLoader.Load()` calls `IWindowManager.CloseAllProjectWindows()` **first**, before it disposes
the outgoing file's VMs — otherwise every open window keeps showing the previous file bound to
view models that were just disposed. That is why switching files closes the editor windows.

**A per-entity singleton keyed on its VM must be re-keyed if the window is ever re-pointed.**
`ChapterWindow`'s Story → Chapter picker swaps DataContext in place, so `RetargetChapterWindow`
moves the dictionary key with it; correspondingly `ShowSingleton`'s `Closed` handler unregisters
**by window, not by the key it captured at creation** — a retargeted window closing would otherwise
evict whichever window had since claimed its original key, leaving that one open but unregistered.

## Pickers: one controller, thin adapters

`Common/ScopedPickerController<TScope,TItem>` owns the scope-combo + item-combo + scoped-search
picker behavior. `SubjectPickerControl` (Type → Subject), `ChapterPickerControl` (Story → Chapter)
and `PlotPointPickerControl` (Story → Chapter → PlotPoint) are thin adapters over it — they had
been line-for-line clones, and a search enhancement once had to be hand-ported across three
controls. A future picker (Theme, WorkPhase, …) instantiates the controller rather than becoming
the next copy. `SourceMaterialPickerControl` keeps its extra chip/quick-add layer but shares the
shape.

All three **commit on selection** — they raise `XSelected` and reset themselves — rather than
exposing a bindable `SelectedX`. A picker is a go-to gesture, not a bound field; the host closes
its popup in the handler.

**A third level is the controller's *outer scope*, not a second controller** (2026-08-06).
`SetOuterScope(filter, hint)` installs a coarser narrowing applied before the scope combo's, and
`Candidates()` — pool → outer → scope — is the single definition of "still in play" that the item
combo and the search box both draw from, so the search can never disagree with the combo about
what is in scope. The hint falls through scope → outer → unscoped, and `Reset()` clears both
levels. `SubjectPickerControl` never calls it; a two-level picker is unchanged by its existence.

Two rules the plot point picker illustrates for any future third level. **Every level clears
independently** (its own ✕) — Story alone, meaning "somewhere in TLTT, chapter unknown", is a
first-class state and not a half-finished path to Story + Chapter. And **a lower combo that can now
span several parents must be re-ordered and re-labelled to say which parent it is in**: chapters
show `FullNumberAndTitle` sorted by (story, chapter), and plot points sort by (story, chapter,
position) rather than `OrderInChapter` alone, which used to stack every chapter's "1." together.
Those sorts live in `Common/NarrativeOrder` because both pickers need the same answer: with no
navigation properties the "which story is this chapter in" join is a dictionary built once per
sort, where a per-item `FirstOrDefault` would re-scan on every keystroke in the search box.

### A `Popup` inside a `DataGrid` cell cannot host an editor (learned 2026-08-05)

`WorldDatePickerControl` and `SourceMaterialPickerControl` use the same shape — a `ToggleButton`
driving a `StaysOpen="False"` `Popup` — and both work. **That shape does not survive being put in
a `DataGridTemplateColumn`.** The colour picker was built that way first and every control inside
the popup was inert: swatch clicks, the hex `TextBox`, and all three of Clear/Cancel/Apply. Not one
handler, all of them.

The cause is routing, not any individual handler. A `Popup`'s content lives in its own HWND but its
**logical** parent chain still runs Popup → the declaring element → `DataGridCell` → `DataGridRow`
→ `DataGrid`, and WPF builds routed-event routes across that seam. So the grid's own mouse and key
handling sits in the path of every event the popup's content needs, and tunneling `Preview*` events
reach the grid *first*. Debugging this by chasing individual handlers is a dead end — the symptom
is "everything is dead", which reads like a broken binding and isn't.

**Rule:** an editor hosted in a `DataGrid` cell is a modal `Window`, not a `Popup`.
`ColorPickerWindow` is the worked example, and `ColorPickerControl` is the cell-side face — a
`Button` showing the value, which opens the window and performs one write on return. Keep `Popup`
for the surfaces where it already works (`NoteView`, `TimelineView`, `CommonWindow`), none of which
is a grid.

Two things the modal buys beyond just working: the `ToggleButton.IsChecked` ↔ `Popup.IsOpen`
close-then-reopen wart disappears, and so does the row-recycling hazard — a virtualized `DataGrid`
can re-point a cell's bindings at a different row mid-edit, but a modal dialog blocks the scroll
that would cause it, so no open-time target snapshot is needed.

Whichever you use, the colour column itself must be `IsReadOnly="True"` with a `CellTemplate` and
**no** `CellEditingTemplate`: a template column lacking an editing template falls back to using the
`CellTemplate` as its editor, so F2 or a second click pushes the cell into edit state around a live
editor. The control *is* the editor; the grid must never think it also has one.

## The crash safety net (added 2026-07-31)

`App`'s constructor installs three handlers before any window or project load can throw; the
reporting lives in `Shell/CrashReporter.cs`, which logs to `crash-log.txt` beside the executable.

| door | what it catches | outcome |
|---|---|---|
| `DispatcherUnhandledException` | UI thread: commands, bindings, `async void` handlers | `Handled = true` — **the app survives** |
| `TaskScheduler.UnobservedTaskException` | faulted tasks nobody awaited | reported instead of vanishing |
| `AppDomain.UnhandledException` | non-UI threads | CLR terminates anyway; log and say so |

Why it exists: every note edit is typed straight into a live POCO, so a process kill costs
unsaved authoring. An unchecked cast in one window is not a good enough reason to lose an
afternoon's work.

Two deliberate choices, do not "improve" them:

- **No emergency save on crash.** In-memory state is of unknown validity, writes to a `.storyplan`
  are Brian's decision, and a well-meant emergency save is how a recoverable session becomes a
  corrupted file. The dialog tells him what happened and stops there.
- **Repeat suppression is per-signature, not per-session.** Three identical `(origin, type,
  message)` reports stop raising dialogs so a per-keystroke failure cannot trap the user in a
  modal loop with no way to reach File > Save; a *different* failure still gets its dialog, and
  everything is logged regardless.

This is a safety net, not a licence to throw. It does not make an exception an acceptable control
flow, and it does not excuse an unchecked cast — it means one gets a dialog instead of taking the
window down mid-edit.

## Traps

- **New files go in their feature's folder** (or `Common/` on second use) — there is no
  `Views/`, `ViewModels/`, or `NewViewModels/` and none may be recreated; the 2026-07-30
  reorganization abolished them.
- **`RetainedOldViews/DesignTimeStoryService.cs`** is entirely commented out and references a
  pre-`TotalRework` model (`Character`, `CodexEntry`, `StoryThread`). It will not compile against
  the current `IStoryService` and is not a usable template.
- **`App.xaml.cs` registers a DI `AppDbContext`** against `"Data Source=StoryPlanner.db"` that is
  never used — `StoryService` builds its own context from the opened file path. Don't follow it.
- **`AppSettings.IsArchiveMode`** is set from the *filename* containing "archive"
  (`App.xaml.cs`). It makes notes read-only, includes `TrackType.Unset` in exports, and re-sorts
  subjects by unconfirmed count. Archive files are a different mode, not just different data.
- **Stubs that look like bugs:** `StoryService.GetAiContextJson()` returns `string.Empty`;
  `PurgeUnassignedNotesAsync()` is a no-op; `PlotPoint.GetCombinedText()` returns
  `string.Empty`. Check intent before "fixing" any of them.
- **The final save lives in `App.OnExit`** — note prose binds `PropertyChanged` straight into
  live POCOs, so edits accumulate untracked-by-any-command until a save runs. Do not remove the
  exit save, and any new "close the app" path must go through normal shutdown so it fires.
- **A `Popup` inside a `DataGrid` cell goes completely inert** — every handler, not one. Use a
  modal `Window`; see "A `Popup` inside a `DataGrid` cell cannot host an editor" above.

## Before you finish

Run `dotnet test tests/StoryPlanner.Tests`. If the WPF app is running, build the specific
project rather than the solution — the running app locks
`WindowedStoryPlanner/bin/Debug/net10.0-windows/`.

**Verifying in the live app is a handoff, not something you drive.** Build, launch the
`bin/Debug` binary against a *copy* of a `.storyplan` (with its `-wal`/`-shm`), then stop and give
Brian a numbered checklist; publish only after he signs off. Full procedure:
`.claude/skills/testing/SKILL.md`, "The third tier is Brian".

**The WPF layer itself has no automated coverage, and that is a known, documented gap** — see
`.claude/skills/testing/SKILL.md` "Known gap". The deletion story is no longer part of that gap:
the guards live id-based in `ContentIntegrity`, the cascades in `StoryService.DeleteNote`/
`DeleteLink`, and both are fixture-tested — what remains untested is only `ContentDeleter`'s
thin registry-sync wrappers, along with everything else that needs a running dispatcher.
