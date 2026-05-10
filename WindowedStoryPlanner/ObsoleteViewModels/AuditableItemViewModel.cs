using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class AuditableItemViewModel : ObservableObject
    {
        private readonly object _owner;
        private readonly Action _onAudited;
        private readonly Action _onNavigate;

        [ObservableProperty]
        private DateTime _lastModified;

        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private string _type;

        [ObservableProperty]
        private string _ownerName;

        public AuditableItemViewModel(object owner, string ownerName, string text, string type, DateTime lastModified, Action onAudited, Action onNavigate)
        {
            _owner = owner;
            OwnerName = ownerName;
            Text = text;
            Type = type;
            LastModified = lastModified;
            _onAudited = onAudited;
            _onNavigate = onNavigate;
        }

        [RelayCommand]
        public void MarkAsAudited()
        {
            _onAudited?.Invoke();
            LastModified = DateTime.UtcNow;
        }

        [RelayCommand]
        public void NavigateToOwner()
        {
            _onNavigate?.Invoke();
        }
    }
}
