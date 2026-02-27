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
        private void PasteBuckets()
        {
            // This maps paradigms to subtle background colors for the cards
            var colorMap = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Timeline", "#BBDEFB" },
                { "Demographic", "#C8E6C9" },       // Shortened for fuzzy matching
                { "Concepts", "#FFE0B2" },
                { "Epistemological", "#E1BEE7" },
                { "Dialectics", "#FFCDD2" },
                { "System Mechanics", "#CFD8DC" },       // Shortened for fuzzy matching
                { "Meta-Narrative", "#B2EBF2" }
            };

            try
            {
                // 1. Grab text from the clipboard
                string jsonText = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(jsonText)) return;

                // 2. Parse the JSON dynamically
                using (JsonDocument document = JsonDocument.Parse(jsonText))
                {
                    JsonElement root = document.RootElement;

                    if (root.TryGetProperty("CategorizedBuckets", out JsonElement categorizedBuckets))
                    {
                        // Optional: Clear existing cards before pasting a new batch
                        // Cards.Clear(); 

                        // 3. Iterate through each Paradigm block
                        foreach (JsonElement paradigm in categorizedBuckets.EnumerateArray())
                        {
                            string paradigmName = paradigm.GetProperty("ParadigmName").GetString();
                    
                            // 4. Find the matching color (using a Contains check in case the 
                            // AI includes the parentheticals like "Chronological (Time)")
                            string hex = "#E0E0E0"; // Default light gray fallback for unknown axes
                            var match = colorMap.FirstOrDefault(kvp => paradigmName.Contains(kvp.Key));
                    
                            if (match.Key != null)
                            {
                                hex = match.Value;
                            }

                            // 5. Iterate through the string array of buckets
                            if (paradigm.TryGetProperty("Buckets", out JsonElement buckets))
                            {
                                foreach (JsonElement bucket in buckets.EnumerateArray())
                                {
                                    string bucketString = bucket.GetString();
                            
                                    // 6. Create the view model and add it to the UI
                                    Cards.Add(new BucketCardViewModel(paradigmName, bucketString, hex));
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
        private void AddCard()
        {
            Cards.Add(new BucketCardViewModel());
        }
    }
}