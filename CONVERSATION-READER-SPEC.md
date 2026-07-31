# Conversation Reader — Feature Spec for Story Planner v2

> **HISTORICAL (banner added 2026-07-28). Built 2026-07 — do not update this file.**
> It records the *intent* behind the Conversation Reader and is worth reading for that.
> It is **not** an accurate description of what shipped. Known drift, per `FEATURE-AUDIT.md` §F:
> - **`BlockSubjectMention` was never built** — only conversation-level `ConversationSubjectCoverage` exists, so the cross-conversation subject view (F2) that depends on it does not exist either.
> - **`TrackTypesCsv` became a real junction table**, `ConversationSubjectCoverageTrack`, with an added `IsAdded` flag.
> - **The subject/track coverage feature was abandoned in practice** — 4,062 coverage-track rows, `IsAdded = 0` on every one. Automated subject routing "turned out to not be helpful." Do not revive it.
> - **`IgnoredConversation` and the Conversation Picker** are post-spec additions not described here.
> - Multi-select bulk state ops (F3) and the unresolved-material dashboard metric (F4) were not built. *(Banner amendment 2026-07-31: F3 has since shipped — see `FEATURE-AUDIT.md` §F. F4 was declined pending a conversation-pipeline redesign.)*
>
> What *is* live and actively used: the three-column reader, block-level triage via the F1–F4 keys (`BlockState`), WebView2 markdown rendering, and folder-pair import.
> For current state, query the data (`storyplanner` MCP `list_conversations`) rather than trusting this document.

## Background

Story Planner v2 has ~50 conversations (Gemini AI Studio + Claude) containing story-planning decisions that need to enter v2 as notes. These conversations are long (up to 37,000 lines), structurally opaque, and end-loaded — insights concentrate in later turns while earlier turns are setup, deliberation, and AI reformulation. Reading them serially (or backwards, which was the v1 workflow) is ineffective. The Conversation Reader makes the corpus navigable so the author can survey conversations holistically, identify which turns contain decisions worth reading in full, and work through the corpus systematically with progress tracking.

The reader does NOT replace reading. It provides wayfinding. The author reads his own original text for anything that matters. AI-generated per-turn summaries tell him which turns to read and which to skip. Trust in the summaries is deliberately low — a misleading summary wastes time (he reads a turn that had nothing), it doesn't lose data (the full text is always right there).

## Goals

1. Process the entire conversation corpus into v2 notes, written in the author's voice, without losing any story-planning decision.
2. Make the process navigable rather than miserable — a well-organized research library, not spelunking through chat logs.
3. Always know where you stand: how much is done, how much is left, what's flagged, what subjects have unresolved material.

---

## Existing Codebase Context

Stack: WPF (.NET 10), EF Core, SQLite, CommunityToolkit.Mvvm, GongSolutions.Wpf.DragDrop

Relevant existing entities:
- `Note` — polymorphic ownership via `OwnerId`/`OwnerType`. Has `NoteState` (Unset/Flagged/Confirmed), `SourceMaterialId`, `NoteTrackDefinitionId`.
- `NoteTrackDefinition` — per-subject-type tracks with `TrackType` enum (12 types), `DisplayQuestion`, `UsageDirective`, `AuditDirective`.
- `Subject` — typed by `SubjectDefinition` (6 types: Character, Bond, Organization, World Law, Civilizational System, Technology).
- `SourceMaterial` — simple Name+Description tag. Notes reference it via nullable FK. Unrelated to the Conversation Reader; remains as-is.
- `GeminiEntry` — existing flat prompt+response entity with `IsAnalyzed` bool. Predecessor concept but not reusable (no conversation grouping, no structured summaries, no multi-state tracking). May eventually be deprecated in favor of the Conversation Reader's model.
- `Idea` — has `IdeaState` enum (Written/PartiallyAnalyzed/FullyAnalyzed). Precedent for state tracking.

Architecture patterns:
- Models in `StoryPlanner.Core.Models` (POCOs, some with `[ObservableProperty]` from CommunityToolkit)
- ViewModels in `WindowedStoryPlanner.ViewModels` (use `IViewModelRegistry` for central observable collections)
- Views in `WindowedStoryPlanner.Views` (UserControls for library/widget views, Windows for detail/dedicated views)
- `TaggedNotesViewModelBase` for cross-cutting note filtering (used by SourceMaterialDetailViewModel)
- MainWindow uses left-side `TabControl`. Each tab is a library view with its own ViewModel.
- `ViewModelLocator` is the root DataContext, injected with all tab ViewModels.
- Detail windows (SubjectWidget, ChapterWindow, ThemeWindow, SourceMaterialWindow) open as separate Window instances.

---

## Data Model

### New Entities

```csharp
public class Conversation
{
    public int Id { get; set; }
    
    // Metadata
    public string Title { get; set; } = string.Empty;       // User-editable, defaults to filename
    public DateTime ConversationDate { get; set; }           // Date of the conversation
    public string Platform { get; set; } = string.Empty;     // "Gemini" or "Claude"
    public int TurnCount { get; set; }                       // Cached count of blocks
    
    // Routing (populated by extraction prompt)
    public string ArcSummary { get; set; } = string.Empty;   // 3-5 sentence conversation-wide summary
}
```

```csharp
public class ConversationBlock
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    
    public int BlockNumber { get; set; }                     // Sequential position in conversation
    public string Speaker { get; set; } = string.Empty;      // "user" or "assistant"
    
    // Content
    public string RawContent { get; set; } = string.Empty;   // Full original text (thinking blocks stripped)
    public string Summary { get; set; } = string.Empty;      // 1-3 sentence AI-generated summary
    public bool HasDecisions { get; set; }                   // Story-planning decisions in this turn?
    
    // State
    public BlockState BlockState { get; set; }
}
```

```csharp
public enum BlockState
{
    Unread,     // Default. Haven't engaged with this block.
    Skipped,    // Scanned summary, decided full text isn't worth reading. Deliberate judgment.
    Flagged,    // Read it, something here, but needs to wait. Come back later.
    Done        // Extracted what's needed, or confirmed nothing to extract.
}
```

```csharp
public class ConversationSubjectCoverage
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public int SubjectId { get; set; }               // FK to Subject
    public string TrackTypesCsv { get; set; } = string.Empty;  // Comma-separated TrackType values
}
```

```csharp
public class BlockSubjectMention
{
    public int Id { get; set; }
    public int ConversationBlockId { get; set; }
    public string SubjectName { get; set; } = string.Empty;  // Name as extracted; may not match exactly
}
```

### Design Notes

**ConversationSubjectCoverage** stores the conversation-wide routing: which subjects this conversation touches and which tracks are relevant. Powers the routing header and the cross-conversation subject view. `SubjectId` is a FK to Subject when a match exists. `TrackTypesCsv` stores suggested TrackType enum values as a comma-separated string (advisory metadata, not worth a junction table).

**BlockSubjectMention** stores per-block subject tagging. Lighter than ConversationSubjectCoverage — just records which subjects were mentioned in a block. Used for filtering blocks by subject within a conversation.

**Why not extend GeminiEntry?** GeminiEntry is flat (individual prompt-response pairs, no conversation grouping) and stores HTML. The Conversation Reader needs hierarchy (Conversation > Block), plain text, per-block state tracking, and routing metadata. Starting fresh is cleaner. GeminiEntry can continue for its current purpose or eventually be migrated.

**Thinking blocks are stripped at import time.** Claude conversations contain large AI thinking/reasoning blocks that are internal working memory, not conversation content. These are removed during parsing before RawContent is stored. This significantly reduces the text volume and makes the right-column reading experience clean.

**Context compaction sections** (where the original session's context was summarized mid-conversation) are imported as a special block type. The summary for a compaction block should note that it's a compaction boundary. These are typically auto-Skipped during processing.

**DbContext additions:**
```csharp
public DbSet<Conversation> Conversations { get; set; }
public DbSet<ConversationBlock> ConversationBlocks { get; set; }
public DbSet<ConversationSubjectCoverage> ConversationSubjectCoverages { get; set; }
public DbSet<BlockSubjectMention> BlockSubjectMentions { get; set; }
```

---

## Extraction Prompt Output Schema

The extraction prompt is run by a future Claude Code or Cowork session against each raw conversation file. It produces a JSON file that the app imports. The prompt receives the raw conversation text plus the v2 schema (subject list and track definitions) but NOT v2 note content (to avoid biasing the author's judgment during entry).

### JSON Schema

```json
{
  "conversationDate": "2026-05-03",
  "platform": "Claude",
  "title": "Applejack's parents and industrial rejection",
  
  "arcSummary": "Explored Applejack's backstory around her parents' departure...",
  
  "subjectsCovered": [
    {
      "subjectName": "Applejack",
      "trackTypes": ["Characterization", "History"]
    },
    {
      "subjectName": "Sweet Apple Acres",
      "trackTypes": ["Ontology", "Civilization"]
    }
  ],
  
  "blocks": [
    {
      "blockNumber": 1,
      "speaker": "user",
      "summary": "Asks for analysis of existing Applejack parents backstory in the planner.",
      "hasDecisions": false,
      "subjectsMentioned": ["Applejack"]
    },
    {
      "blockNumber": 2,
      "speaker": "assistant",
      "summary": "Reads and restates existing planner content. No new decisions. Identifies open design questions: timing of AJ's SAA realization, distinction between resenting absence vs. resenting industrial posture.",
      "hasDecisions": false,
      "subjectsMentioned": ["Applejack", "Sweet Apple Acres"]
    },
    {
      "blockNumber": 3,
      "speaker": "user",
      "summary": "Corrects the framing: AJ doesn't resent the parents' absence, she resents the posture of industrial work. Establishes that SAA was never just a farm.",
      "hasDecisions": true,
      "subjectsMentioned": ["Applejack", "Sweet Apple Acres"]
    }
  ]
}
```

### What the Extraction Prompt Captures vs. Strips

**Captures (in the summary):**
- What was discussed in this turn
- Story-planning decisions made or changed
- Constraints established
- Open questions identified
- Displacements (what was rejected and what replaced it)

**Strips (not in the summary, not in RawContent):**
- AI thinking/reasoning blocks (removed from RawContent entirely)
- AI analytical frameworks and reasoning chains
- AI rhetorical praise ("The Brilliant Irony," "this is a clever explanation")
- AI reformulations of content already in the planner
- AI process narration ("Now I'm evaluating...", "I also need to address...")
- Duplicate content (where the AI response repeats its thinking block)

**Preserved in RawContent but not in Summary:**
- The exact phrasing of the author's messages
- The AI's substantive responses (with thinking blocks stripped)
- Everything needed for the author to re-read the turn in full

### Extraction Prompt Inputs

The extraction prompt receives:
1. The raw conversation file (full text)
2. `v2-subjects.md` — the list of v2 subjects by type (for subject name matching in routing)
3. `v2-definitions.md` — the NoteTrackDefinition display questions and usage directives (so the prompt knows what each track means)

It does NOT receive:
- `v2 full-export.md` or any v2 note content (avoids biasing the author's judgment)
- The v1 archive content

### File Naming Convention

Output files: `{conversation-date}_{sanitized-title}_reader.json`

Example: `2026-05-03_applejack-parents-industrial-rejection_reader.json`

---

## Import Process

Import is a feature in the app's File Management tab (or a dedicated button in the Conversations tab).

### Steps

1. User selects a `_reader.json` file (or a folder of them for batch import).
2. App parses the JSON into the data model entities.
3. Subject name resolution: for each entry in `subjectsCovered`, attempt to match `subjectName` against existing `Subject.Name` in the loaded database. If matched, store the `SubjectId`. If no match, store the name as-is in a `UnmatchedSubjectName` fallback field (or log it for manual resolution).
4. Raw content population: the import process also needs the original conversation file to populate `ConversationBlock.RawContent`. Two options:
   - **Option A:** The extraction prompt includes the raw content in the JSON (larger files, but self-contained).
   - **Option B:** The JSON has block boundaries (character offsets or turn markers), and the import step reads the original file separately to extract raw content per block.
   - **Recommended: Option A.** Self-contained files are simpler to manage. Storage cost is acceptable for ~50 conversations.
5. All blocks start with `BlockState.Unread`.
6. ConversationDate, Platform, Title populated from JSON metadata.

### Gemini vs. Claude Parsing

Both platforms' raw conversations need to be parsed into the same block structure. The extraction prompt handles this — it receives the raw file regardless of format and produces the same JSON schema. The prompt's instructions include guidance for both:

- **Claude conversations:** Markdown exports with `## Human` / `## Assistant` turn markers and AI thinking blocks (XML-tagged reasoning sections) to strip.
- **Gemini conversations:** Google Takeout JSON with `GeminiJsonReader`-style structure (prompt + safeHtmlItem response), or AI Studio exports. HTML content converted to plain text.

---

## UI Layout

### New Tab: "Conversations"

Added to MainWindow's left TabControl. Contains the **Conversation Library** — a list of all imported conversations with metadata and progress indicators.

### Conversation Library View

A scrollable list of conversation cards, similar to SubjectLibrary. Each card shows:

- **Title** (editable)
- **Date** and **Platform** badge
- **Arc Summary** (the conversation-wide 3-5 sentence summary, collapsed by default)
- **Progress bar**: visual indicator of block states (Unread / Skipped / Flagged / Done)
- **Stats line**: e.g., "47 blocks: 12 Done, 28 Skipped, 7 Flagged, 0 Unread"
- **Derived state badge**: Unstarted / In Progress / Complete

Sort order: chronological (oldest first). This is the default and primary ordering.

Clicking a conversation card opens the **Conversation Reader Window**.

### Conversation Reader Window

A dedicated Window (like ChapterWindow, ThemeWindow). Three-column layout.

```
+-----------------------------------------------------------------------+
| [Routing Header]                                                       |
| Arc: "This conversation explored..."                                   |
| Subjects: [Applejack > Characterization, History] [SAA > Ontology]     |
+----------+-------------------+----------------------------------------+
| Block    | Summary           | Full Text                              |
| List     |                   |                                        |
|          |                   |                                        |
| [1] User | Asks for analysis | ## Human                               |
|          | of existing AJ... | Please give a thorough analysis of     |
| [2] Asst | Reads planner,    | the backstory around Applejack's       |
|   (no    | no new decisions. | parents...                             |
|  decisions)| Identifies open |                                        |
|          | questions...      |                                        |
| [3] User | Corrects framing: |                                        |
|  *       | AJ doesn't resent |                                        |
|  (has    | absence, resents  |                                        |
|  decisions)| industrial      |                                        |
|          | posture...        |                                        |
|          |                   |                                        |
| [4] Asst |                   |                                        |
|          |                   |                                        |
+----------+-------------------+----------------------------------------+
```

#### Left Column: Block List

A compact, scrollable list of blocks. Each entry shows:
- Block number
- Speaker indicator (User / Assistant, using color or icon)
- A visual marker for `HasDecisions` (e.g., a star or dot)
- Current `BlockState` (as a colored indicator: gray=Unread, blue=Skipped, orange=Flagged, green=Done)

This column is narrow — just enough to see the block index and its state at a glance. Its purpose is to show the shape of the conversation: how many turns, which have decisions, which are processed.

Clicking a block in the left column scrolls the middle and right columns to that block.

#### Middle Column: Summaries

A scrollable list of summary cards, one per block. Each card shows:
- The block number and speaker
- The 1-3 sentence summary text
- Subject mentions (as small tags/chips)
- The `HasDecisions` flag (visually prominent if true)

The middle column is the primary navigation surface. The author scans this to decide which blocks are worth reading in full. It functions as a table of contents for the conversation.

Clicking a summary card:
1. Scrolls the right column to show that block's full text.
2. Highlights the corresponding entry in the left column.

#### Right Column: Full Text

Displays the raw content of the currently selected block. This is the full original conversation text (with thinking blocks already stripped at import). Read-only. Scrollable independently.

Rendered as formatted text (markdown rendering would be ideal but plain text with monospace is acceptable as a starting point).

#### Routing Header

Above the three columns, a panel showing:
- The **Arc Summary** (the 3-5 sentence conversation-wide summary)
- **Subject coverage links**: each subject+track combination as a clickable element. Clicking opens the subject's detail window (SubjectWidget/NarrativeElementFullView) filtered to the relevant track. This lets the author see what's already in v2 for that subject while reading the conversation.

The routing header is always visible (doesn't scroll with the columns).

### State Transitions

The author changes a block's state through:
- **Right-click context menu** on a block in the left or middle column: Unread / Skipped / Flagged / Done
- **Keyboard shortcuts** while a block is selected (configurable, but suggested defaults: S=Skip, F=Flag, D=Done, U=Unread)
- **Bulk selection**: multi-select blocks in the left column (Shift+Click or Ctrl+Click), then apply a state to all selected. This is critical for marking runs of AI-deliberation turns as Skipped in one action.

State changes persist immediately (saved to database on change).

### Conversation-Level Derived State

Not stored — computed from block states:
- **Unstarted**: all blocks are Unread
- **In Progress**: any mix that isn't all-Unread or all-terminal
- **Complete**: all blocks are either Skipped or Done, zero Flagged, zero Unread

A conversation cannot be Complete while it has Flagged blocks. Flagged means "come back" — the conversation has unfinished business.

---

## Cross-Conversation Subject View

Accessible from the Conversations tab (a toggle or sub-tab) or from a subject's detail window.

When the author selects a subject (and optionally a track), this view shows all ConversationBlocks across all conversations that mention that subject — ordered chronologically by conversation date, then by block number within each conversation.

Each entry shows:
- Which conversation it's from (title + date)
- The block's summary
- The block's state
- A click action that opens the Conversation Reader Window to that specific block

This view answers: "What did all my conversations say about Applejack's Characterization?" without opening each conversation individually.

Implementation: query `BlockSubjectMention` joined with `ConversationBlock` and `Conversation`, filtered by subject name. If the subject name in BlockSubjectMention has been resolved to a SubjectId (via ConversationSubjectCoverage), filter by that. Otherwise, filter by string match.

---

## Dashboard

A summary panel at the top of the Conversations tab (above the conversation list), showing:

- **Total conversations** imported, and counts by derived state (Unstarted / In Progress / Complete)
- **Total blocks** across all conversations, and counts by BlockState (Unread / Skipped / Flagged / Done)
- **Flagged count**: total Flagged blocks across all conversations (this is the "open items" number)
- **Subjects with unresolved material**: subjects that appear in ConversationSubjectCoverage for conversations that are not yet Complete

This is glanceable — a few numbers and maybe a progress bar. Not a separate view.

---

## Per-Conversation Statistics

Visible in the conversation card in the library view and in the Conversation Reader Window header:

- Total blocks
- Count by state: Done, Skipped, Flagged, Unread
- Percentage complete (Done + Skipped) / Total

After finishing a conversation, this confirms: "47 blocks: 12 Done, 28 Skipped, 7 Flagged." If Flagged is zero, the conversation is Complete. If not, the Flagged count tells the author how many items need revisiting.

---

## ViewModel Structure

Following existing patterns:

```
ViewModels/
  ConversationViewModel.cs            // wraps Conversation entity
  ConversationBlockViewModel.cs       // wraps ConversationBlock, exposes BlockState as observable
  ConversationLibraryViewModel.cs     // tab VM: list of conversations, dashboard stats, import command
  ConversationReaderViewModel.cs      // window VM: selected conversation, three-column state, commands
  SubjectCoverageViewModel.cs         // wraps ConversationSubjectCoverage for routing header
```

**ConversationLibraryViewModel** is added to `ViewModelLocator` and `IViewModelRegistry`:
```csharp
// ViewModelLocator additions
public ConversationLibraryViewModel Conversations { get; }

// IViewModelRegistry additions  
ObservableCollection<ConversationViewModel> AllConversationViewModels { get; }
ObservableCollection<ConversationBlockViewModel> AllConversationBlockViewModels { get; }
```

**ConversationReaderViewModel** manages:
- The selected Conversation
- The list of ConversationBlockViewModels for that conversation
- The currently selected/focused block (drives right-column content)
- Commands for state transitions (SkipCommand, FlagCommand, DoneCommand, etc.)
- Bulk selection state
- Routing header data (ConversationSubjectCoverage resolved to SubjectViewModels)

---

## View Structure

```
Views/
  ConversationLibrary.xaml            // tab content: dashboard + scrollable conversation cards
  ConversationCard.xaml               // card for one conversation in the library
  ConversationReaderWindow.xaml       // the three-column reader window
  ConversationBlockListItem.xaml      // left column item template
  ConversationSummaryCard.xaml        // middle column item template
  ConversationRoutingHeader.xaml      // routing header UserControl
  SubjectCoverageView.xaml            // cross-conversation subject view
```

---

## Interaction Flows

### Flow 1: First-time setup

1. Run extraction prompt against each raw conversation file (outside the app, via Claude Code/Cowork session)
2. Extraction produces `_reader.json` files
3. In Story Planner, go to Conversations tab, click Import
4. Select JSON file(s) — app parses and creates Conversation + ConversationBlock + routing entities
5. Conversation appears in library as Unstarted

### Flow 2: Working through a conversation

1. Open Conversations tab, see library sorted chronologically
2. Click a conversation card to open the Reader Window
3. Scan the routing header — note which subjects and tracks this conversation covers
4. Scan the middle column summaries top-to-bottom
5. For blocks marked `hasDecisions: false` that are clearly AI setup/reformulation, multi-select and mark Skipped
6. For blocks marked `hasDecisions: true`, click to load full text in right column
7. Read the full text. If there's material for v2 notes:
   - Click the subject link in the routing header to open that subject's detail window
   - Write the note in the author's own words in the appropriate track
   - Mark the block as Done
8. If a block needs to wait (depends on processing another conversation first, or needs thought), mark as Flagged
9. When all blocks are Done or Skipped with zero Flagged, the conversation is Complete

### Flow 3: Working by subject

1. Open the cross-conversation subject view
2. Select a subject (e.g., Applejack) and optionally a track (e.g., Characterization)
3. See all blocks across all conversations that mention Applejack, chronologically
4. Click a block to open its conversation in the Reader Window at that position
5. Process the block, return to the subject view for the next one

### Flow 4: Checking progress

1. Open Conversations tab
2. Dashboard shows: "50 conversations: 3 Complete, 12 In Progress, 35 Unstarted. 847 blocks total: 45 Done, 120 Skipped, 23 Flagged, 659 Unread."
3. Flagged count of 23 means 23 blocks across the corpus need revisiting
4. Scroll through conversation cards — each shows its own progress bar and stats

---

## Implementation Sequence (Suggested)

### Phase 1: Data model + import
- Add new entities to StoryPlanner.Core.Models
- Add DbSets to AppDbContext
- Create and run EF migration
- Build JSON import logic (parse `_reader.json`, create entities, resolve subject names)
- Add import button to File Manager tab (or new Conversations tab placeholder)

### Phase 2: Conversation Library
- Create ConversationViewModel, ConversationLibraryViewModel
- Add to ViewModelLocator and IViewModelRegistry
- Build ConversationLibrary.xaml with conversation cards
- Add Conversations tab to MainWindow
- Dashboard stats (computed properties on the library VM)

### Phase 3: Conversation Reader Window
- Create ConversationBlockViewModel, ConversationReaderViewModel
- Build ConversationReaderWindow.xaml with three-column layout
- Left column: block list with state indicators
- Middle column: summary cards
- Right column: full text display
- Block selection syncs across all three columns
- Routing header with arc summary

### Phase 4: State management + bulk operations
- Right-click context menu for state transitions
- Keyboard shortcuts
- Multi-select in left column
- State persistence (save on change)
- Conversation-level derived state computation

### Phase 5: Routing + subject integration
- Routing header with clickable subject+track links
- Link clicks open SubjectWidget/NarrativeElementFullView to the relevant track
- ConversationSubjectCoverage display

### Phase 6: Cross-conversation subject view
- SubjectCoverageView showing blocks filtered by subject across all conversations
- Accessible from Conversations tab and from subject detail windows

---

## Open Questions for Implementation

1. **RawContent storage**: Option A (include raw content in JSON, self-contained) vs. Option B (JSON has block boundaries, import reads original file separately). Recommended: Option A for simplicity, but files will be larger.

2. **Markdown rendering in right column**: WPF has no built-in markdown renderer. Options: use a WebBrowser control with a markdown-to-HTML library, use a third-party WPF markdown control (e.g., MdXaml, Markdig.Wpf), or start with plain text in a TextBlock/RichTextBox and add rendering later.

3. **GeminiEntry migration**: Eventually, existing GeminiEntry data could be migrated into the Conversation model. Not blocking for initial implementation — they can coexist.

4. **Block granularity**: A "block" is one message (one Human message or one Assistant message). Not a turn pair. This keeps the left-column compact and allows marking individual messages (e.g., skip the assistant's reformulation but read the user's correction).

5. **Very large RawContent**: Some assistant messages are 3,000+ lines (after thinking block removal). The right column needs to handle large text without UI lag. Consider virtualized text rendering or lazy loading.