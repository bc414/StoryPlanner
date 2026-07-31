using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace WindowedStoryPlanner;

/// <summary>
/// Backs the "Map to Existing…" picker: a manual override for when the Scan Preview's automatic
/// date + block-count heuristic either misses a real match or (rarely) proposes the wrong one.
/// Lists every un-uuid'd Claude conversation already in the DB so the user can point an export
/// row directly at the correct one, rather than relying on the guess.
/// </summary>
public partial class ConversationPickerViewModel : ObservableObject
{
    public ObservableCollection<Conversation> Candidates { get; }
    public ICollectionView FilteredCandidates { get; }

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private Conversation? _selectedConversation;

    public int? SelectedConversationId => SelectedConversation?.Id;

    public ConversationPickerViewModel(IEnumerable<Conversation> candidates)
    {
        Candidates = new ObservableCollection<Conversation>(candidates.OrderByDescending(c => c.ConversationDate));
        FilteredCandidates = new ListCollectionView(Candidates) { Filter = FilterCandidate };
    }

    partial void OnSearchTextChanged(string value) => FilteredCandidates.Refresh();

    private bool FilterCandidate(object obj) =>
        obj is Conversation c &&
        (string.IsNullOrWhiteSpace(SearchText) || c.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
}
