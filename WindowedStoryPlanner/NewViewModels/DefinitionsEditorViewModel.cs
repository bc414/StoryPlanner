using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public partial class DefinitionsEditorViewModel : ObservableObject
    {
        private readonly IStoryService _storyService;

        public ObservableCollection<SubjectDefinitionViewModel> SubjectDefinitions { get; } = new();
        public ObservableCollection<NoteTrackDefinitionViewModel> NoteTrackDefinitions { get; } = new();

        // Populated once, used as ComboBox ItemsSource in NoteTrackDefinition rows
        public IReadOnlyList<string> AvailableSubjectTypes { get; private set; } = [];
        public IReadOnlyList<OwnerType> OwnerTypes { get; } = Enum.GetValues<OwnerType>();
        public IReadOnlyList<CognitiveMode> CognitiveModes { get; } = Enum.GetValues<CognitiveMode>();
        public IReadOnlyList<int?> FunctionKeyOptions { get; } =
            new int?[] { null }.Concat(Enumerable.Range(1, 12).Cast<int?>()).ToList();

        public DefinitionsEditorViewModel(IStoryService storyService)
        {
            _storyService = storyService;
        }

        public void Initialize()
        {
            foreach (var m in _storyService.SubjectDefinitions)
                SubjectDefinitions.Add(new SubjectDefinitionViewModel(m, _storyService));

            AvailableSubjectTypes = SubjectDefinitions.Select(s => s.SubjectType).ToList();

            foreach (var m in _storyService.NoteTrackDefinitions)
                NoteTrackDefinitions.Add(new NoteTrackDefinitionViewModel(m, _storyService, SubjectDefinitions));
        }

        [RelayCommand]
        private async Task AddSubjectDefinition()
        {
            var model = new SubjectDefinition { SubjectType = "NewType" };
            _storyService.SubjectDefinitions.Add(model);
            await _storyService.SaveAsync();
            SubjectDefinitions.Add(new SubjectDefinitionViewModel(model, _storyService));
        }

        [RelayCommand]
        private async Task DeleteSubjectDefinition(SubjectDefinitionViewModel vm)
        {
            _storyService.SubjectDefinitions.Remove(vm.Model);
            SubjectDefinitions.Remove(vm);
            await _storyService.SaveAsync();
        }

        [RelayCommand]
        private async Task AddNoteTrackDefinition()
        {
            var model = new NoteTrackDefinition { TrackName = "New Track", DisplayOrder = 0 };
            _storyService.NoteTrackDefinitions.Add(model);
            await _storyService.SaveAsync();
            NoteTrackDefinitions.Add(new NoteTrackDefinitionViewModel(model, _storyService, SubjectDefinitions));
        }

        [RelayCommand]
        private async Task DeleteNoteTrackDefinition(NoteTrackDefinitionViewModel vm)
        {
            _storyService.NoteTrackDefinitions.Remove(vm.Model);
            NoteTrackDefinitions.Remove(vm);
            await _storyService.SaveAsync();
        }
    }
}
