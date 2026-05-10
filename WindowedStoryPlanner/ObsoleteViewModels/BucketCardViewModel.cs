using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WindowedStoryPlanner.ViewModels
{
    public class BucketCardViewModel : INotifyPropertyChanged
    {
        private string _noteText;
        
        private string _backgroundHex;
        
        // Centralized color map so both the View Model and text-updater can use it
        public static readonly Dictionary<string, string> ColorMap = new Dictionary<string, string>
        {
            { "Timeline", "#BBDEFB" },
            { "Demographic", "#C8E6C9" },       
            { "Concepts", "#FFE0B2" },
            { "Epistemological", "#E1BEE7" },
            { "Dialectics", "#FFCDD2" },
            { "System Mechanics", "#CFD8DC" },       
            { "Meta-Narrative", "#B2EBF2" }
        };
        
        public string ParadigmName { get; }
        public string BucketName { get; }
        public string DefaultText { get; }
        
        public string BackgroundHex
        {
            get => _backgroundHex;
            set
            {
                if (_backgroundHex != value)
                {
                    _backgroundHex = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NoteText
        {
            get => _noteText;
            set
            {
                if (_noteText != value)
                {
                    _noteText = value;
                    UpdateBackgroundColor();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsDirty)); // Notify that IsDirty may have changed too
                }
            }
        }

        // True if the user typed something beyond the default prepopulated text
        public bool IsDirty => NoteText.Trim() != DefaultText.Trim();

        // Constructor for pasted buckets
        public BucketCardViewModel(string paradigm, string bucket)
        {
            ParadigmName = paradigm;
            BucketName = bucket;
            
            // Figure out the default color based on the paradigm name
            var match = ColorMap.FirstOrDefault(kvp => ParadigmName.Contains(kvp.Key));
            _backgroundHex = match.Key != null ? match.Value : "#E0E0E0"; // Light gray fallback
            
            // Prepopulate the note with the category headers
            DefaultText = $"{ParadigmName} - {BucketName}\n";
            _noteText = DefaultText; 
        }

        public BucketCardViewModel()
        {
            ParadigmName = string.Empty;
            BucketName = string.Empty;
            _backgroundHex = "#FFFFFF"; // Default to white
            DefaultText = string.Empty;
            _noteText = string.Empty;
        }

        private void UpdateBackgroundColor()
        {
            if (string.IsNullOrWhiteSpace(_noteText))
            {
                BackgroundHex = "#FFFFFF";
                return;
            }

            // Check the first line of the note to see if it contains a paradigm keyword
            string firstLine = _noteText.Split('\n').FirstOrDefault() ?? string.Empty;
            
            var match = ColorMap.FirstOrDefault(kvp => firstLine.Contains(kvp.Key));
            
            if (match.Key != null)
            {
                BackgroundHex = match.Value;
            }
            else
            {
                BackgroundHex = "#FFFFFF"; // Revert to white if the user deletes the keyword
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}