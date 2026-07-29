---
name: testing
description: Test conventions for StoryPlanner — the two tiers, the synthetic .storyplan fixture, what to test and what deliberately isn't tested, and why the rules differ from a server project's. Read before adding or modifying tests, or when deciding whether a new piece of logic is worth a test.
---

# Testing

Adapted from `TheCanalaveLibrary`'s conventions, which are more battle-tested than anything here.
Where this file diverges from theirs, the divergence is deliberate and the reason is stated —
StoryPlanner is a single-user desktop tool over SQLite, not a public web app over Postgres, and
several of their rules **invert** rather than transfer.

## The tiers, organized by kind — not by production project

The fundamental axis is **what infrastructure the test needs**, not which project the
type-under-test lives in.

| Tier | Project | May use | Targets |
|---|---|---|---|
| **Pure** | `tests/StoryPlanner.Tests` (`net10.0`) | Anything constructed directly. No `.storyplan` file, no `DbContext` | `ConversationSyncScanner.Scan`, `HtmlToMarkdown.Convert`, the markdown exporters (they take DTOs), `Query`'s parsers and label functions |
| **Fixture** | same project | `SyntheticPlan` — a real, throwaway SQLite `.storyplan` in a temp dir | MCP tools, `ExportResolver` + `NoteExportRenderer`, anything reached through `IStoryService` or `StoryPlanSources` |
| **App** | *does not exist yet* — `tests/StoryPlanner.App.Tests` (`net10.0-windows`) when it does | WPF project reference | `ContentDeleter` guards, `ContentFactory`. See "Known gap" below |

**Pure and Fixture share one project on purpose.** They are separate tiers conceptually but not
structurally, because a temp SQLite file needs no container, no host, and no fixture collection —
it costs ~50 ms and disposes itself. Splitting them would be ceremony without benefit. The App
tier *is* a real boundary (a different TFM plus `UseWPF`), so it earns its own project when built.

**Placement is behavioral.** If you can construct the type and assert without a `.storyplan`, it
is a Pure test even if it lives in `StoryPlanner.Core`. If it needs data, it is a Fixture test
even if the type is trivial.

## Test against real SQLite — this inverts Canalave's rule, and the reason matters

Canalave says *"never InMemory/SQLite for integration tests"* because **their** production
database is Postgres, and SQLite would give false confidence about Postgres-specific behavior.
The principle is *test against the real provider* — and here the real provider **is SQLite**.

So: **`SyntheticPlan` creates an actual `.storyplan` file via `Database.Migrate()`** — the same
migrations the app runs, on the same provider, producing the same schema. Never EF Core's
InMemory provider, which executes no SQL and would hide exactly the things worth testing:
the polymorphic `(OwnerId, OwnerType)` joins, WAL behavior, and `PRAGMA data_version`
invalidation.

Use `Database.Migrate()`, never `EnsureCreated()` — `EnsureCreated` skips migrations entirely
and would let a broken migration pass.

**Don't mock `DbContext` or `IStoryService`.** A mocked `IStoryService` returning canned
collections proves the test's own assumptions, not that the code works against the real
change-tracked `DbSet.Local` projections. The fixture is cheap enough that there is no excuse.

## Per-test isolation: a throwaway file, not a reset

Each test calls `SyntheticPlan.Create()`, which builds a fresh database in its own temp
directory and deletes it on `Dispose`. There is no shared state, no Respawn equivalent, and no
serial-execution requirement — tests are independent by construction and may run in parallel.

Because every test starts from an identical known baseline, **absolute assertions are expected**:
exact counts, "must be the only hit", ordering from empty. Write the natural test.

**Seed what you need.** The fixture's baseline is deliberately small and documented in
`Fixtures/SyntheticPlan.cs`. If a test needs a row the baseline lacks, add it via
`plan.ExternalWrite(ctx => …)` rather than expanding the shared baseline — a growing baseline
makes every other test's assertions harder to reason about.

## Assertion shape: assert on structure, not on the absence of a string

The lesson that cost a test failure on 2026-07-28: **tool output legitimately echoes the
caller's own input.** `search_plan`'s header and its walled-count line both repeat the search
pattern, so `Assert.DoesNotContain(secret, output)` fails for a reason that has nothing to do
with the wall.

The correct assertion is structural — that the *envelope text surrounding* a match never
appears, and that no hit line was emitted:

```csharp
// WRONG — the pattern is echoed back in the header and the follow-up hint
Assert.DoesNotContain(FlaggedContentSecret, tools.SearchPlan(FlaggedContentSecret));

// CORRECT — proves no snippet escaped
Assert.DoesNotContain(FlaggedContentEnvelope, result);   // surrounding prose never leaks
Assert.DoesNotContain($"note:{FlaggedNoteId} ", result); // no hit line for the flagged note
Assert.Contains("notes 0", result);                      // and zero ordinary hits
```

The fixture provides paired constants for this: a `…Secret` to search for and a `…Envelope`
that only a leaked snippet would drag along.

## Tests that double as documentation

Where an invariant depends on a non-obvious platform behavior, write the test so that its
failure points at the cause. `The_main_file_mtime_does_not_track_writes_so_it_cannot_be_the_signal`
asserts the mtime does **not** advance, then asserts the change is picked up anyway. If journal
mode ever changes, that test fails and names `StoryPlanSources.EnsureFresh` — instead of leaving
a silently broken cache-invalidation path.

Prefer this over a comment. A comment goes stale; a test fails.

## What to test

**Test these — nothing else guards them:**

- **The flagged wall**, in all three faces: search excludes content and reason, fetch-by-id
  returns a stub, traversal discloses a tally. This is the server's central epistemic guarantee
  and a regression is silent — unstable content would simply start appearing as fact.
- **Per-file state labels.** v1 `Confirmed` must never render as "confirmed". A regression here
  asserts something the data does not support.
- **Anything with the word "polymorphic" near it.** The schema has no foreign keys, so owner
  resolution is hand-written and a wrong join returns plausible-looking wrong data.
- **Mechanical parsers** — `WorldDate`, and any future free-text field. Especially the
  *unparseable* case: the rule is flag, never guess.
- **Graph traversal and absence.** "This subject has no links" is information (221 of 263 real
  subjects have none); a tool that silently omits the section is wrong.
- **Cache invalidation.** See above.
- **Guard predicates that stand in for database constraints** — once they're reachable.

**Don't test these:**

- **Exact prose in tool output.** Assert that a fact is present, not that a sentence reads a
  particular way; otherwise every wording improvement is a test failure.
- **EF Core's own behavior.** Migrations applying, `Id` assignment on save, `DbSet.Local`
  tracking — that's the framework's test suite.
- **Stubs.** `GetMarkdown()`, `GetAiContextJson()`, `PurgeUnassignedNotesAsync()` all return
  empty or do nothing deliberately. A test asserting "returns empty" locks in an accident.
- **Deliberately-rejected features.** `FEATURE-AUDIT.md`'s ⚪ list is not a to-do list.

## Known gap: the WPF layer is not covered, and why

`ContentDeleter` is the referential-integrity system — with no foreign keys in the schema, its
`TryDelete*` guards are the only thing preventing orphaned rows. It is the highest-value untested
code in the repository.

It is untested because its methods take **view models**, and constructing one
(`PlotPointSubjectLinkViewModel`, `SubjectViewModel`) requires a six-dependency graph
(`IViewModelRegistry`, `IStoryService`, `IContentFactory`, `AppSettings`, `ExportService`,
sometimes `IWindowManager`) *and* the constructors do real work — `InitializeTracksAndProperties()`,
and `BuildLinkView()` when the registry reports a loaded story. Standing that up is the
"spin up the world" cost the tier split exists to avoid.

**What would unlock it** is a small production refactor, not a test trick: extract the guard
predicates to operate on ids rather than view models —

```csharp
bool HasNotes(int ownerId, OwnerType ownerType);   // pure, testable against a fixture
```

— leaving `TryDeleteLinkAsync` as a thin wrapper. That change is worth doing when the deletion
logic is next touched. Until then this is a **known, deliberate gap**; do not paper over it by
mocking `IStoryService` (see above) or by asserting on a view model built with null dependencies.

## What the tiers structurally can't see

Each tier trades runtime realism for speed and determinism. Neither existing tier runs WPF, so
data binding, `ICollectionView` filtering, drag-drop, and dispatcher behavior are invisible —
a green suite means the tiers' trade-offs held, not that the app works. The MCP server's
JSON-RPC transport is likewise untested at the protocol level; tests call the tool methods
directly, so a serialization or transport regression would pass. For those, drive the server
over stdio manually (the pattern is in this repo's git history) or check `/mcp` in a live session.

Neither tier sees Brian's real `.storyplan` files, by design — **tests never touch them.**

## Running

```
dotnet test                                              # whole solution
dotnet test tests/StoryPlanner.Tests                     # this project
dotnet test --filter FullyQualifiedName~FlaggedWall      # one area
```

If the WPF app is running, build/test the specific project rather than the solution — the running
app locks `WindowedStoryPlanner/bin/Debug/net10.0-windows/`.

## Project setup reference

`tests/StoryPlanner.Tests` — `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`.
Project reference: `tools/StoryPlanner.Mcp` (which brings `StoryPlanner.Core` transitively).
No assertion library beyond xUnit's — the suite is small and `Assert` is adequate; adding
FluentAssertions later is fine but don't mix styles within a file.

Folders mirror the tiers: `Fixtures/`, `Core/`, `Mcp/`.
