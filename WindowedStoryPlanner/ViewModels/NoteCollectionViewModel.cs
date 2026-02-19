using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using StoryPlanner.Core.Models;
using System.Linq;
using System.Text.Json;

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
        // 1. Identify the dragged data
        if (dropInfo.Data is not Note sourceNote) return;

        // 2. CHECK TARGET: Is it the Trash Zone?
        // We check the Name of the visual element that the mouse is hovering over
        var targetElement = dropInfo.VisualTarget as FrameworkElement;

        // 3. CHECK TARGET: Is it the List (Reordering)?
        if (dropInfo.TargetItem is Note)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        var sourceNote = dropInfo.Data as Note;
        if (sourceNote == null) return;

        var targetElement = dropInfo.VisualTarget as FrameworkElement;

        // --- CASE B: REORDER (Dropped on List) ---
        GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);
        UpdateSortOrders();
    }

    // ... inside NoteCollectionViewModel class ...

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

    [RelayCommand]
    private void DeleteNote(Note noteToDelete)
    {
        if (noteToDelete == null) return;
        NoteCollection.Remove(noteToDelete);
        UpdateSortOrders();
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

        var json = JsonSerializer.Serialize(dtos, new JsonSerializerOptions { WriteIndented = true });
        Clipboard.SetText(json);
        
        MessageBox.Show($"Copied {dtos.Count} notes to clipboard.");
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