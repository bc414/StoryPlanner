using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;

namespace WindowedStoryPlanner
{
    /// <summary>
    /// One narrative property on one entity — a labelled picker over the property's allowed values.
    ///
    /// SINGLE-SELECT, and unset is a first-class state: no <see cref="NarrativePropertyValue"/> row
    /// means the author has not decided, which is legal and long-lived. There is deliberately no
    /// "(none)" entry in <see cref="ValidValues"/> — its id would be written into
    /// <c>ValueDefinitionId</c> and read back as a real answer. Clearing goes through
    /// <see cref="ClearCommand"/>, which deletes the row.
    ///
    /// The value is never derived. Nothing here inspects note text, links, or names to guess an
    /// answer; assignment is authorial (CLAUDE.md: retrieval, not suggestion).
    /// </summary>
    public partial class NarrativePropertyViewModel : ObservableObject
    {
        private readonly NarrativePropertyDefinition _definition;
        private readonly IStoryService _storyService;
        private readonly int _ownerId;

        /// <summary>The persisted assignment, or null when unset.</summary>
        private NarrativePropertyValue? _assignment;

        public ObservableCollection<NarrativePropertyValueViewModel> ValidValues { get; }

        public string Name => _definition.Name;
        public string Question => _definition.Question;
        public string Explanation => _definition.Explanation;

        /// <summary>
        /// True when this property gates a work phase and has no value. A report, never a block —
        /// nothing consults this to disable an action, and note promotion to Confirmed is
        /// unaffected.
        /// </summary>
        public bool IsGatedAndUnset => _definition.GatingWorkPhaseId is not null && SelectedValue is null;

        /// <param name="ownerType">
        /// Part of the signature for symmetry with the rest of the owner-composition family, and
        /// deliberately not stored: the definition already declares its OwnerType, and the callers
        /// in NarrativeElementViewModel only ever pass definitions matching their own owner type.
        /// Ownership is disambiguated below by value id, which is stricter than OwnerType would be.
        /// </param>
        public NarrativePropertyViewModel(
            int ownerId,
            OwnerType ownerType,
            NarrativePropertyDefinition narrativePropertyDefinition,
            IViewModelRegistry viewModelRegistry,
            IStoryService storyService)
        {
            _definition   = narrativePropertyDefinition;
            _storyService = storyService;
            _ownerId      = ownerId;

            // Read the model collection rather than the registry's VM collection: it is populated
            // in StoryService.LoadDataAsync, long before any entity view model is constructed, so
            // this cannot depend on ProjectLoader's ordering.
            ValidValues = new ObservableCollection<NarrativePropertyValueViewModel>(
                _storyService.NarrativePropertyValueDefinitions
                    .Where(vd => vd.NarrativePropertyDefinitionId == _definition.Id)
                    .Select(vd => new NarrativePropertyValueViewModel(vd)));

            // NarrativePropertyValue has no OwnerType column, so OwnerId alone does not identify an
            // owner — subject 7 and chapter 7 would collide. Scoping the lookup to THIS property's
            // value ids resolves it, because a value definition belongs to exactly one property and
            // a property declares its owner type. Same trace as
            // ContentDeleter.RemoveOwnedNarrativePropertyValues and PlanIntegrity.
            var validValueIds = ValidValues.Select(v => v.Id).ToHashSet();
            _assignment = _storyService.NarrativePropertyValues
                .FirstOrDefault(v => v.OwnerId == _ownerId && validValueIds.Contains(v.ValueDefinitionId));

            // Null when unset, and null when the assignment points at a value definition that no
            // longer exists — never a fabricated first entry.
            _selectedValue = ValidValues.FirstOrDefault(v => v.Id == _assignment?.ValueDefinitionId);
        }

        private NarrativePropertyValueViewModel? _selectedValue;
        public NarrativePropertyValueViewModel? SelectedValue
        {
            get => _selectedValue;
            set
            {
                if (!SetProperty(ref _selectedValue, value)) return;

                if (value is null)
                {
                    if (_assignment is not null)
                    {
                        _storyService.NarrativePropertyValues.Remove(_assignment);
                        _assignment = null;
                    }
                }
                else if (_assignment is null)
                {
                    _assignment = new NarrativePropertyValue { OwnerId = _ownerId, ValueDefinitionId = value.Id };
                    _storyService.NarrativePropertyValues.Add(_assignment);
                }
                else
                {
                    _assignment.ValueDefinitionId = value.Id;
                }

                // Not awaited — nothing here reads the new row's assigned Id, the condition
                // NoteViewModel's source-reference commands document for the same choice. But
                // .FireAndForget() rather than a bare discard: a discarded Task swallows its
                // exception, so a failed save would be silent, which is worse than a crash for an
                // app whose product is the data file.
                _storyService.SaveAsync().FireAndForget();

                OnPropertyChanged(nameof(IsGatedAndUnset));
                ClearCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanClear() => SelectedValue is not null;

        [RelayCommand(CanExecute = nameof(CanClear))]
        private void Clear() => SelectedValue = null;
    }
}
