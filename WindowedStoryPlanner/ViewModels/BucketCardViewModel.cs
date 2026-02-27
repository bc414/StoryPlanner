using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WindowedStoryPlanner.ViewModels
{
    public class BucketCardViewModel : INotifyPropertyChanged
    {
        private string _noteText;
        
        public string ParadigmName { get; }
        public string BucketName { get; }
        public string DefaultText { get; }
        public string BackgroundHex { get; } // For color-coding

        public string NoteText
        {
            get => _noteText;
            set
            {
                if (_noteText != value)
                {
                    _noteText = value;
                    OnPropertyChanged();
                }
            }
        }

        // True if the user typed something beyond the default prepopulated text
        public bool IsDirty => NoteText.Trim() != DefaultText.Trim();

        public BucketCardViewModel(string paradigm, string bucket, string hexColor)
        {
            ParadigmName = paradigm;
            BucketName = bucket;
            BackgroundHex = hexColor;
            
            // Prepopulate the note with the category headers
            DefaultText = $"{ParadigmName} - {BucketName}\n";
            _noteText = DefaultText; 
        }

        public BucketCardViewModel()
        {
            ParadigmName = string.Empty;
            BucketName = string.Empty;
            BackgroundHex = "#FFFFFF"; // Default to white
            DefaultText = string.Empty;
            _noteText = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}