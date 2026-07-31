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
settings), `Common/` (genuinely shared machinery: converters, behaviors, NoteView, pickers,
ContentFactory/Deleter, TaggedNotesViewModelBase), `Editing/` (the entity-editor machinery —
NarrativeElement family, note tracks, CommonWindow, widgets — deliberately framed as a feature:
the libraries are thin browsers over it), then one folder per feature: `Subjects/ Chapters/
Stories/ PlotPoints/ Themes/ Sources/ Definitions/ Files/ Conversations/ Export/ Timeline/`.
Root keeps only `App.xaml`, `Styles.xaml`, `AssemblyInfo.cs`, and quarantined dead code.

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
3. **Disposal belongs to `WindowManager.ShowSingleton`**, not to each window's code-behind —
   `(window.DataContext as IDisposable)?.Dispose()` on `Closed`, so a new cross-cut window cannot
   forget it.

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

## `ContentDeleter` is the referential integrity system

The database has no foreign keys, so **nothing but this class prevents orphaned rows.** Deletes
that could orphan return `bool` and refuse rather than cascading:

```csharp
public async Task<bool> TryDeleteLinkAsync(PlotPointSubjectLinkViewModel link)
{
    bool hasNotes = _storyService.Notes
        .Any(n => n.OwnerId == link.Id && n.OwnerType == OwnerType.PlotPointSubjectLink);
    if (hasNotes) return false;                       // guard, not cascade
    RemoveOwnedNarrativePropertyValues(link.Id, OwnerType.PlotPointSubjectLink);
    …
}
```

**Any new deletable entity needs its guard added here.** And note
`RemoveOwnedNarrativePropertyValues` — `NarrativePropertyValue` has an `OwnerId` but **no
`OwnerType`**, so ownership resolves only by tracing `ValueDefinitionId →
NarrativePropertyDefinitionId → .OwnerType`. Copy that method's logic; do not reinvent it.

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
- **`IWindowManager`** opens windows; `ViewModelLocator` is the root DataContext.
- **Views:** UserControls for library and widget surfaces, Windows for detail surfaces.
  Value converters live in `Views/Converters.cs`; drag-drop behaviors in `Views/Behaviors/`.

## Traps

- **`NewViewModels/`** is an empty folder declared in the `.csproj`. Dead scaffolding — do not
  populate it; put view models in `ViewModels/`.
- **`DesignTimeStoryService.cs`** is entirely commented out and references a pre-`TotalRework`
  model (`Character`, `CodexEntry`, `StoryThread`). It will not compile against the current
  `IStoryService` and is not a usable template.
- **`App.xaml.cs` registers a DI `AppDbContext`** against `"Data Source=StoryPlanner.db"` that is
  never used — `StoryService` builds its own context from the opened file path. Don't follow it.
- **`AppSettings.IsArchiveMode`** is set from the *filename* containing "archive"
  (`App.xaml.cs`). It makes notes read-only, includes `TrackType.Unset` in exports, and re-sorts
  subjects by unconfirmed count. Archive files are a different mode, not just different data.
- **Stubs that look like bugs:** `StoryService.GetMarkdown()` and `GetAiContextJson()` return
  `string.Empty`; `PurgeUnassignedNotesAsync()` is a no-op; `PlotPoint.GetCombinedText()` returns
  `string.Empty`. Check intent before "fixing" any of them.

## Before you finish

Run `dotnet test tests/StoryPlanner.Tests`. If the WPF app is running, build the specific
project rather than the solution — the running app locks
`WindowedStoryPlanner/bin/Debug/net10.0-windows/`.

**Verifying in the live app is a handoff, not something you drive.** Build, launch the
`bin/Debug` binary against a *copy* of a `.storyplan` (with its `-wal`/`-shm`), then stop and give
Brian a numbered checklist; publish only after he signs off. Full procedure:
`.claude/skills/testing/SKILL.md`, "The third tier is Brian".

**This layer has no automated coverage yet, and that is a known, documented gap** — see
`.claude/skills/testing/SKILL.md` "Known gap". `ContentDeleter`'s guards are the highest-value
untested code in the repo; the blocker is that its methods take view models rather than ids.
When you next touch deletion logic, extracting `bool HasNotes(int ownerId, OwnerType ownerType)`
would make the guards testable without standing up the view-model graph.
