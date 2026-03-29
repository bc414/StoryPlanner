using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core.Models;
using System.Linq;
using System.Text.Json;
using System.Text;

namespace WindowedStoryPlanner.ViewModels;

public partial class NoteCollectionViewModel : ObservableObject, IDropTarget
{
    public ObservableCollection<Note> NoteCollection { get; set; }
    public ObservableCollection<SourceMaterial> AvailableSources
    {
        get
        {
            return MainViewModel.Instance.SourceMaterials;
        }
    }
    
    [ObservableProperty]
    private bool _isNoteReorderMode;

    [ObservableProperty]
    private bool _showCommands;

    [ObservableProperty] private int _characterCount;

    public NoteCollectionViewModel(ObservableCollection<Note> sourceCollection)
    {
        var sortedNotes = sourceCollection.OrderBy(n => n.SortOrder).ToList();

        // Check if the current list is out of order to avoid unnecessary UI updates
        if (!sourceCollection.SequenceEqual(sortedNotes))
        {
            sourceCollection.Clear();
            foreach (var note in sortedNotes)
            {
                sourceCollection.Add(note);
            }
        }

        NoteCollection = sourceCollection;

        int charCount = 0;
        foreach (var note in NoteCollection)
        {
            charCount += note.Content.Length;
        }
        CharacterCount = charCount;
    }

    [RelayCommand]
    private void AddNote()
    {
        var newNote = new Note
        {
            Content = "New Note",
            SortOrder = NoteCollection.Count
        };
        NoteCollection.Add(newNote);
    }

    // --- Drag & Drop Implementation ---

    public void DragOver(IDropInfo dropInfo)
    {
        // 1. Identify the dragged data: Allow internal Notes OR external BucketCards
        if (dropInfo.Data is not Note && dropInfo.Data is not BucketCardViewModel) return;

        // 2. CHECK TARGET: Is it the List (Reordering or Inserting)?
        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

        // Tell the UI to show a "Move" cursor (no plus sign)
        dropInfo.Effects = DragDropEffects.Move;
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is Note)
        {
            // --- CASE A: REORDER (Internal Drag) ---
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);
            UpdateSortOrders();
        }
        else if (dropInfo.Data is BucketCardViewModel incomingCard)
        {
            // --- CASE B: IMPORT AND MOVE (Dragged from Categorizer) ---
            var newNote = new Note
            {
                Content = incomingCard.NoteText.Trim()
            };

            // Insert at the specific index the user dropped it at
            int insertIndex = dropInfo.InsertIndex;
            if (insertIndex >= 0 && insertIndex <= NoteCollection.Count)
            {
                NoteCollection.Insert(insertIndex, newNote);
            }
            else
            {
                NoteCollection.Add(newNote);
            }

            UpdateSortOrders();

            // EXPLICIT REMOVAL: Grab the source collection from where the drag started 
            // and manually remove the original item.
            if (dropInfo.DragInfo?.SourceCollection is System.Collections.IList sourceList)
            {
                sourceList.Remove(incomingCard);
            }

            // Crucial: Explicitly report this as a Move so the source 
            // DragHandler knows to delete the original BucketCardViewModel
            dropInfo.Effects = DragDropEffects.Move;
        }
    }

    [RelayCommand]
    public void MoveItemUp(Note item)
    {
        if (item == null) return;

        int index = NoteCollection.IndexOf(item);
        if (index > 0)
        {
            NoteCollection.Move(index, index - 1);
            UpdateSortOrders(); // Ensures the SortOrder property is persisted
        }
    }

    [RelayCommand]
    public void MoveItemDown(Note item)
    {
        if (item == null) return;

        int index = NoteCollection.IndexOf(item);
        if (index < NoteCollection.Count - 1)
        {
            NoteCollection.Move(index, index + 1);
            UpdateSortOrders(); // Ensures the SortOrder property is persisted
        }
    }

    private void UpdateSortOrders()
    {
        for (int i = 0; i < NoteCollection.Count; i++)
        {
            NoteCollection[i].SortOrder = i;
        }
    }
    private readonly Stack<(Note DeletedNote, int OriginalIndex)> _deletedNotesHistory = new();
    [RelayCommand]
    private void DeleteNote(Note noteToDelete)
    {
        if (noteToDelete == null) return;

        // 2. Remember the note and its position before deleting it
        int originalIndex = NoteCollection.IndexOf(noteToDelete);
        _deletedNotesHistory.Push((noteToDelete, originalIndex));

        NoteCollection.Remove(noteToDelete);
        UpdateSortOrders();
        
        // 3. Notify the UI that the Undo command's execution state has changed
        UndoDeleteCommand.NotifyCanExecuteChanged();
    }
    
    [RelayCommand(CanExecute = nameof(CanUndoDelete))]
    private void UndoDelete()
    {
        if (_deletedNotesHistory.Count == 0) return;

        // Pop the most recently deleted note
        var (restoredNote, originalIndex) = _deletedNotesHistory.Pop();

        // Safety check: ensure the index is still within bounds in case the list size changed drastically
        if (originalIndex >= 0 && originalIndex <= NoteCollection.Count)
        {
            NoteCollection.Insert(originalIndex, restoredNote);
        }
        else
        {
            // Fallback: just add it to the end
            NoteCollection.Add(restoredNote);
        }

        UpdateSortOrders();
        UndoDeleteCommand.NotifyCanExecuteChanged();
    }
    
    private bool CanUndoDelete() => _deletedNotesHistory.Count > 0;

    [RelayCommand]
    private void CopyPlain()
    {
        if (!NoteCollection.Any()) return;
        StringBuilder stringBuilder = new StringBuilder();
        foreach(Note note in NoteCollection)
        {
            stringBuilder.AppendLine(note.Content);
            stringBuilder.AppendLine(); // Add an extra line for spacing
        }
        Clipboard.SetText(stringBuilder.ToString());
        MessageBox.Show($"Copied {NoteCollection.Count} notes to clipboard as plain text.");
    }
    
    [RelayCommand]
    private void CopyToJson()
    {
        if (!NoteCollection.Any()) return;

        // Extract only the content to save tokens
        var dtos = NoteCollection.Select(n => new NoteContentDto 
        { 
            Content = n.Content 
        }).ToList();

        var json = JsonSerializer.Serialize(dtos, new JsonSerializerOptions
        { 
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        Clipboard.SetText(json);
        
        MessageBox.Show($"Copied {dtos.Count} notes to clipboard as json.");
    }

    [RelayCommand]
    private void PasteFromJson()
    {
        string json = string.Empty;
        try
        {
            json = Clipboard.GetText();
        }
        catch 
        {
            MessageBox.Show("Could not access clipboard.");
            return;
        }

        if (string.IsNullOrWhiteSpace(json)) return;

        try
        {
            var newNotes = JsonSerializer.Deserialize<List<NoteContentDto>>(json);

            if (newNotes != null && newNotes.Any())
            {
                // Safety Check
                var result = MessageBox.Show(
                    $"This will replace your current {NoteCollection.Count} notes with {newNotes.Count} reorganized notes.\n\nProceed?", 
                    "Confirm Reorganization", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    BackupCurrentNotes();
                    NoteCollection.Clear();

                    foreach (var dto in newNotes)
                    {
                        var note = new Note
                        {
                            Content = dto.Content,
                            SortOrder = NoteCollection.Count
                        };
                        NoteCollection.Add(note);
                    }
                }
            }
        }
        catch (JsonException)
        {
            MessageBox.Show("Clipboard content was not valid JSON for notes.");
        }
    }
    
    private void BackupCurrentNotes()
    {
        // If there are no notes, there is nothing to backup
        if (NoteCollection == null || !NoteCollection.Any()) return;

        try
        {
            // Save to a "Backups" folder in the app's running directory
            var backupDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            Directory.CreateDirectory(backupDirectory);

            // Timestamped filename: notes_backup_20260215_143000.json
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filename = $"notes_backup_{timestamp}.json";
            var fullPath = Path.Combine(backupDirectory, filename);

            var dtos = NoteCollection.Select(n => new NoteContentDto 
            { 
                Content = n.Content 
            }).ToList();

            var json = JsonSerializer.Serialize(dtos, new JsonSerializerOptions { WriteIndented = true });
        
            File.WriteAllText(fullPath, json);
        }
        catch (Exception ex)
        {
            // Non-blocking error. We likely still want to proceed with the paste, 
            // but we should warn the user that the safety net failed.
            MessageBox.Show($"Warning: Automatic backup failed.\n{ex.Message}", "Backup Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
    public class NoteContentDto
    {
        public string Content { get; set; } = string.Empty;
    }
}