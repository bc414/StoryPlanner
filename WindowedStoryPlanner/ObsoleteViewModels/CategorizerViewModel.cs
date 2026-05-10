using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

// Assuming you have a standard RelayCommand implementation in your project.
// If not, you'll need to use whatever ICommand implementation your project uses.

namespace WindowedStoryPlanner.ViewModels
{
    public partial class CategorizerViewModel
    {
        public ObservableCollection<BucketCardViewModel> Cards { get; set; } = new ObservableCollection<BucketCardViewModel>();
        

        public CategorizerViewModel()
        {
            
        }
        
        [RelayCommand]
        private void PasteNotes()
        {
            try
            {
                string jsonText = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(jsonText)) return;

                using (JsonDocument document = JsonDocument.Parse(jsonText))
                {
                    JsonElement root = document.RootElement;

                    // Ensure the root is an array, as produced by CopyNotes
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        int count = 0;
                        foreach (JsonElement noteElement in root.EnumerateArray())
                        {
                            if (noteElement.TryGetProperty("Content", out JsonElement contentElement))
                            {
                                string contentText = contentElement.GetString();
                        
                                if (!string.IsNullOrWhiteSpace(contentText))
                                {
                                    var newCard = new BucketCardViewModel
                                    {
                                        NoteText = contentText
                                    };
                            
                                    Cards.Add(newCard);
                                    count++;
                                }
                            }
                        }
                
                        // Optional: Provide feedback on success
                        // MessageBox.Show($"Pasted {count} notes from clipboard!", "Paste Successful");
                    }
                    else
                    {
                        MessageBox.Show("The clipboard does not contain a valid list of notes.", "Paste Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("The clipboard text could not be parsed as valid JSON.", "Paste Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred while pasting:\n{ex.Message}", "Paste Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void PasteBuckets()
        {
            try
            {
                string jsonText = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(jsonText)) return;

                using (JsonDocument document = JsonDocument.Parse(jsonText))
                {
                    JsonElement root = document.RootElement;

                    if (root.TryGetProperty("CategorizedBuckets", out JsonElement categorizedBuckets))
                    {
                        foreach (JsonElement paradigm in categorizedBuckets.EnumerateArray())
                        {
                            string paradigmName = paradigm.GetProperty("ParadigmName").GetString();

                            if (paradigm.TryGetProperty("Buckets", out JsonElement buckets))
                            {
                                foreach (JsonElement bucket in buckets.EnumerateArray())
                                {
                                    string bucketString = bucket.GetString();
                    
                                    // Simplified: Just pass the paradigm and bucket. 
                                    // The BucketCardViewModel will figure out its own color!
                                    Cards.Add(new BucketCardViewModel(paradigmName, bucketString));
                                }
                            }
                        }
                    }
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("The clipboard text could not be parsed as valid JSON.", "Paste Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred while pasting:\n{ex.Message}", "Paste Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void CopyNotes()
        {
            // THE DIRTY CHECK: Only grab cards where text differs from the default
            // We project it into an anonymous object that matches your NoteContentDto structure
            var populatedNotes = Cards
                .Where(c => c.IsDirty)
                .Select(c => new { Content = c.NoteText.Trim() })
                .ToList();

            if (populatedNotes.Any())
            {
                // Serialize the list of objects into a JSON string
                var options = new JsonSerializerOptions { WriteIndented = true };
                string finalExport = JsonSerializer.Serialize(populatedNotes, options);

                Clipboard.SetText(finalExport);
                MessageBox.Show($"Copied {populatedNotes.Count} notes to clipboard!", "Export Successful");
            }
        }
        
        [RelayCommand]
        private void CopyAllNotes()
        {
            // THE DIRTY CHECK: Only grab cards where text differs from the default
            // We project it into an anonymous object that matches your NoteContentDto structure
            var populatedNotes = Cards
                .Select(c => new { Content = c.NoteText.Trim() })
                .ToList();

            if (populatedNotes.Any())
            {
                // Serialize the list of objects into a JSON string
                var options = new JsonSerializerOptions { WriteIndented = true };
                string finalExport = JsonSerializer.Serialize(populatedNotes, options);

                Clipboard.SetText(finalExport);
                MessageBox.Show($"Copied {populatedNotes.Count} notes to clipboard!", "Export Successful");
            }
        }

        [RelayCommand]
        private void AddCard()
        {
            Cards.Add(new BucketCardViewModel());
        }
    }
}