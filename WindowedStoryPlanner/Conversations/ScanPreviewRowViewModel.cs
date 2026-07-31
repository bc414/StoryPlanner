using CommunityToolkit.Mvvm.ComponentModel;
using StoryPlanner.Core;
using System;

namespace WindowedStoryPlanner;

/// <summary>
/// Wraps one ConversationSyncItem for the Scan Preview grid: the checkbox that drives export
/// selection, plus display text that stays in sync as Confirm/Reject/Ignore mutate the underlying
/// item's classification in place.
/// </summary>
public partial class ScanPreviewRowViewModel : ObservableObject
{
    public ConversationSyncItem Item { get; }

    // Shared "acted upon" checkbox — the toolbar's Export/Ignore-selected buttons both operate on
    // whatever is checked here. New/Reopened rows start checked as a convenience default.
    [ObservableProperty] private bool _isSelected;

    public ScanPreviewRowViewModel(ConversationSyncItem item)
    {
        Item = item;
        IsSelected = item.Classification is ConversationSyncClassification.New
                                          or ConversationSyncClassification.Reopened;
    }

    public string Title => Item.Export.Title;
    public string Date  => FormatDate(Item.Export.ConversationDate);

    public ConversationSyncClassification Classification => Item.Classification;
    public string ProposedMatchTitle => Item.ProposedMatchTitle;

    public bool IsNeedsConfirmation => Classification == ConversationSyncClassification.NeedsConfirmation;

    // Ignoring only makes sense for a conversation with no DB row yet — Reopened/Unchanged/
    // NeedsConfirmation all correspond to something already imported.
    public bool CanIgnore   => Classification == ConversationSyncClassification.New;
    public bool CanUnignore => Classification == ConversationSyncClassification.Ignored;

    // A manual override is available whenever there's no *certain* match yet — either the
    // heuristic found nothing (New) or its guess needs a human decision anyway (NeedsConfirmation).
    // Reopened/Unchanged already have a certain uuid match; Ignored must be un-ignored first.
    public bool CanMapToExisting => Classification is ConversationSyncClassification.New
                                                     or ConversationSyncClassification.NeedsConfirmation;

    public string BlockCountLabel => Classification switch
    {
        ConversationSyncClassification.Reopened          => $"{Item.DbBlockCount} → {Item.ExportBlockCount}  (+{Item.BlockCountDelta})",
        ConversationSyncClassification.NeedsConfirmation => $"{Item.DbBlockCount} → {Item.ExportBlockCount}",
        _                                                 => $"{Item.ExportBlockCount}"
    };

    /// <summary>Call after mutating Item.Classification (Confirm/Reject/Ignore) so bound
    /// properties that derive from it refresh.</summary>
    public void RefreshClassification()
    {
        OnPropertyChanged(nameof(Classification));
        OnPropertyChanged(nameof(IsNeedsConfirmation));
        OnPropertyChanged(nameof(CanIgnore));
        OnPropertyChanged(nameof(CanUnignore));
        OnPropertyChanged(nameof(CanMapToExisting));
        OnPropertyChanged(nameof(BlockCountLabel));
        OnPropertyChanged(nameof(ProposedMatchTitle));
    }

    private static string FormatDate(string iso) =>
        DateTime.TryParse(iso, out var dt) ? dt.ToString("yyyy-MM-dd") : iso;
}
