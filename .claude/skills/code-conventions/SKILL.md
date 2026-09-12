---
name: code-conventions
description: Repo-wide C# conventions and the architecture they follow from — one flat namespace per project, feature-first folders, no navigation properties or foreign keys or indexes, polymorphic ownership with application code as the integrity system, Type Object configuration, and the MCP stdout rule. Read before writing or moving C# anywhere in this repo; the WPF layer has its own skill on top of this one.
---

# Code conventions

These hold in every project: `StoryPlanner.Core`, `WindowedStoryPlanner`, `tools/`,
`process-docs/`, `tests/`. The WPF/MVVM layer adds its own on top — see `wpf-conventions`.

## Namespaces and folders (adopted 2026-07-30)

**One flat namespace per project.** Everything in Core is `namespace StoryPlanner.Core`,
everything in the app is `WindowedStoryPlanner`, regardless of folder. The sole exception is
`StoryPlanner.Core.Migrations`, which dotnet-ef generates into.

Folders are feature organization; namespaces are assembly identity. The two are **decoupled on
purpose** so files move freely without touching a using directive or a XAML reference.

> **`.editorconfig` suppresses the analyzers that fight this. Do not "fix" a namespace to match
> its folder.** Nothing will complain if you do, and that is exactly the problem.

**Feature-first folders.** A feature's files live together in one folder — the folder is the
working set, the grep scope, and the diff. There are no `Views/` / `ViewModels/` style
type-parent folders anywhere. Each project's actual layout is documented with that project;
the WPF layer's is in `wpf-conventions`.

## The architecture, and why it is like this

Single-user, load-everything-at-startup desktop app. Every premise below follows from that, and
each is **settled** — do not propose alternatives.

**No navigation properties, no foreign keys, no indexes.** View models filter `ICollectionView`s
over in-memory `ObservableCollection`s bound to `DbSet.Local`. Models are row vessels; view
models do relationship work. Nav properties would only serve lazy loading, `Include`, and
DB-executed LINQ — none of which apply here — while adding real risks: stale lazy loads,
change-tracking surprises, circular-reference serialization. Indexes follow from the same
premise: **nothing queries the database after load.**

**Ownership is polymorphic** (`OwnerId` + `OwnerType`), so the database cannot enforce
referential integrity. **Application code IS the integrity system**, in three layers
(2026-08-02), none of it decoration, and every UI delete path routes through it:

1. id-based guard predicates in `ContentIntegrity` (Core, fixture-tested);
2. unconditional cascades in `StoryService.DeleteNote` / `DeleteLink` — a note takes its citation
   rows, a link takes its owned narrative-property values;
3. the guarded, registry-syncing `TryDelete*Async` deletes in `ContentDeleter.cs`.

`PlanIntegrity.Check` is the after-the-fact auditor for the same invariants.

**Type Object pattern + metadata-driven design.** `SubjectDefinition` and `NoteTrackDefinition`
are *rows*, not classes. A Character subject and a World Law subject are the same C# class with
different definition rows. The planner's shape evolves by data entry rather than code change;
that flexibility was the point. Editor modes are the same principle applied to the UI.

> *Accepted cost, decided at design time:* metadata-driven queries are less self-documenting.
> Filtering by `NoteTrackDefinitionId` reads worse than `IsOntologyTrack` would. **Do not "fix"
> this.** Rationale: `docs/design-conversations/019_…json` blocks 126–135.

## Process rules

**`stdout` is JSON-RPC in the MCP process — stderr only.** Never `Console.WriteLine` from any
code the server can reach. `ConversationImporter` used to be the standing example; since
2026-07-31 it reports through a returned `ConversationImportResult` instead of printing, but it
is still a write path and the read-only server has no business calling it.

## Tests

Test conventions, the two tiers, the synthetic fixture, and what is deliberately not tested:
the `testing` skill. Run `dotnet test tests/StoryPlanner.Tests` before finishing work in
`tools/` or `StoryPlanner.Core/`.
