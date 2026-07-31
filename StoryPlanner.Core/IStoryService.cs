using System.Collections.ObjectModel;
using StoryPlanner.Core.Models;

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
    ObservableCollection<Theme> Themes { get; }
    ObservableCollection<SourceMaterial> SourceMaterials { get; }
    ObservableCollection<GeminiEntry> GeminiEntries { get; }
    ObservableCollection<Idea> Ideas { get; }

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
    Task StoreGeminiEntriesAsync(string file);
    Task ImportConversationsAsync(string contentPath, string metaPath);
    Task ImportConversationsFolderAsync(string folderPath);
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
    void DeleteNote(int noteId);
    string GetFullProjectJson();
    string GetAiContextJson(bool includeVerbatim);
    Task PurgeUnassignedNotesAsync();
    NoteTrackDefinition? GetNoteTrackDefinition(int id);
}