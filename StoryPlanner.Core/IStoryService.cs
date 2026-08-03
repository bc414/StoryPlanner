using System.Collections.ObjectModel;
using StoryPlanner.Core;

namespace StoryPlanner.Core;

public interface IStoryService : IDisposable
{
    // --- Data Collections ---
    ObservableCollection<Subject> Subjects { get; }
    ObservableCollection<PlotPoint> PlotPoints { get; }
    ObservableCollection<PlotPointSubjectLink> PlotPointsSubjectLinks { get; }
    ObservableCollection<Chapter> Chapters { get; }
    ObservableCollection<Story> Stories { get; }
    ObservableCollection<Theater> Theaters { get; }
    ObservableCollection<Pivot> Pivots { get; }
    ObservableCollection<Note> Notes { get; }
    ObservableCollection<SubjectDefinition> SubjectDefinitions { get; }
    ObservableCollection<NoteTrackDefinition> NoteTrackDefinitions { get; }
    ObservableCollection<NarrativePropertyDefinition> NarrativePropertyDefinitions { get; }
    ObservableCollection<NarrativePropertyValueDefinition> NarrativePropertyValueDefinitions { get; }
    ObservableCollection<NarrativePropertyValue> NarrativePropertyValues { get; }
    ObservableCollection<WorkPhase> WorkPhases { get; }
    ObservableCollection<Theme> Themes { get; }
    ObservableCollection<SourceMaterial> SourceMaterials { get; }
    ObservableCollection<SourceMaterialPart> SourceMaterialParts { get; }
    ObservableCollection<NoteSourceReference> NoteSourceReferences { get; }
    ObservableCollection<UiSetting> UiSettings { get; }

    ObservableCollection<Conversation> Conversations { get; }
    ObservableCollection<ConversationBlock> ConversationBlocks { get; }
    ObservableCollection<ConversationSubjectCoverage> ConversationSubjectCoverages { get; }
    ObservableCollection<ConversationSubjectCoverageTrack> ConversationSubjectCoverageTracks { get; }

    // --- State Properties ---
    string CurrentFilePath { get; }
    bool IsProjectLoaded { get; }

    // --- Methods ---
    Task CreateProjectAsync(string filePath);
    Task OpenProjectAsync(string filePath);
    Task SaveAsync();
    /// <summary>Imports one content file. <paramref name="metaPath"/> is optional — pass null to
    /// import raw block text with no summaries.</summary>
    Task<ConversationImportResult> ImportConversationsAsync(string contentPath, string? metaPath);

    /// <summary>Imports every *_content.json in a folder, using each one's *_meta.json when
    /// present. A content file with no meta partner imports without summaries.</summary>
    Task<ConversationImportResult> ImportConversationsFolderAsync(string folderPath);

    /// <summary>Imports scan rows straight from a parsed Claude export — no content files, no
    /// summaries, no Cowork round trip.</summary>
    Task<ConversationImportResult> ImportScannedConversationsAsync(IReadOnlyList<ConversationSyncItem> items);

    Task DeleteConversationAsync(Conversation conversation);

    // --- Conversation scan/export (Claude export vs. DB ground-truth) ---

    /// <summary>Parses a Claude conversations.json export and classifies each conversation
    /// against the DB (New / Reopened / Unchanged / NeedsConfirmation / Ignored). Read-only.</summary>
    Task<List<ConversationSyncItem>> ScanClaudeExportAsync(string claudeExportPath);

    /// <summary>Writes NNN_{slug}_content.json (+ index) for the given scan rows to outputFolder —
    /// the input for Cowork analysis.</summary>
    Task<List<ConversationContentExporter.ExportedFile>> ExportConversationContentAsync(
        IReadOnlyList<ConversationSyncItem> selectedItems, string outputFolder);

    /// <summary>Confirms a NeedsConfirmation scan row's proposed match by stamping the Claude uuid
    /// onto the matched Conversation, so future scans recognize it with certainty.</summary>
    Task BackfillConversationUuidAsync(int conversationId, string uuid);

    /// <summary>Marks a Claude conversation as not story-related so it stops surfacing as New on
    /// future scans.</summary>
    Task IgnoreConversationAsync(string uuid, string title);

    /// <summary>Reverses a mistaken Ignore — removes the conversation from the ignore list so it
    /// re-enters normal uuid/heuristic matching on the next scan.</summary>
    Task UnignoreConversationAsync(string uuid);

    /// <summary>Re-classifies a single already-parsed export conversation against the current DB
    /// + ignore list — used after Confirm/Reject/Ignore/Un-ignore to refresh one Scan Preview row
    /// without re-parsing or re-scanning the whole export.</summary>
    Task<ConversationSyncItem> RescanOneAsync(ParsedClaudeConversation export);

    /// <summary>Removes a note and, with it, its NoteSourceReference rows — citations are
    /// note-owned, so this cascade is the only thing standing in for the FK the schema
    /// deliberately lacks. Every note-delete path must come through here.</summary>
    void DeleteNote(int noteId);

    /// <summary>Removes a link and its owned NarrativePropertyValue rows. The value rows have no
    /// OwnerType column, so once the link is gone a recycled link id would silently inherit them —
    /// this cascade is what prevents that. Every link-delete path must come through here.</summary>
    void DeleteLink(int linkId);

    /// <summary>Removes every NarrativePropertyValue owned by the given entity, resolving the
    /// valueless OwnerType by tracing ValueDefinitionId → NarrativePropertyDefinitionId →
    /// OwnerType. Called by ContentDeleter before removing any note-owning entity.</summary>
    void RemoveOwnedNarrativePropertyValues(int ownerId, OwnerType ownerType);

    NoteTrackDefinition? GetNoteTrackDefinition(int id);
}