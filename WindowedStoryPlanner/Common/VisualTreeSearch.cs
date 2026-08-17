using System.Windows;
using System.Windows.Media;

namespace WindowedStoryPlanner;

/// <summary>
/// Walking up the visual tree from an event's OriginalSource is how this app resolves "which
/// row/control did that actually come from" when a container gesture and a descendant control
/// compete for the same input — a routed event names the innermost element, not the one that
/// owns the behaviour. Promoted here on its second use (ConversationCard's double-click-to-open
/// vs. its title editor; the Conversation Reader's note editor vs. its block list).
/// </summary>
public static class VisualTreeSearch
{
    /// <summary>Nearest ancestor of type T, inclusive of <paramref name="node"/> itself.</summary>
    public static T? FindAncestor<T>(DependencyObject? node) where T : DependencyObject
    {
        while (node is not null)
        {
            if (node is T match) return match;
            node = VisualTreeHelper.GetParent(node);
        }
        return null;
    }
}
