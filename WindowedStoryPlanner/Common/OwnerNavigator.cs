using System.Linq;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

/// <summary>
/// Opens the entity a note actually lives on. The navigation counterpart of
/// <see cref="OwnerBreadcrumbResolver"/> — that one turns (OwnerId, OwnerType) into a label,
/// this one turns it into an open editor window.
///
/// The editor mode per owner type is a deliberate default, matching what the owner's own library
/// tab opens with: a Subject in Expansion, a PlotPoint in Gardener, a link in Linking (with the
/// link preselected), a Chapter in its own window. Every cross-owner surface routes through here
/// so they cannot drift apart.
/// </summary>
public static class OwnerNavigator
{
    public static void Open(
        int ownerId,
        OwnerType ownerType,
        IViewModelRegistry registry,
        IWindowManager windowManager)
    {
        switch (ownerType)
        {
            case OwnerType.Subject:
                if (registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == ownerId) is { } subject)
                    windowManager.OpenSubjectWindow(subject);
                break;

            case OwnerType.PlotPoint:
                if (registry.AllPlotPointViewModels.FirstOrDefault(p => p.Id == ownerId) is { } plotPoint)
                    windowManager.OpenPlotPointWindow(plotPoint);
                break;

            case OwnerType.Chapter:
                if (registry.AllChapterViewModels.FirstOrDefault(c => c.Id == ownerId) is { } chapter)
                    windowManager.OpenChapterWindow(chapter);
                break;

            case OwnerType.PlotPointSubjectLink:
                // A link has no window of its own — it opens as its SUBJECT in Linking mode with
                // the link preselected. The subject, not the plot point: CommonWindow casts
                // primaryElement to SubjectViewModel for both Expansion and Linking, and to
                // PlotPointViewModel for Gardener (CommonWindow.xaml.cs:103-115). Those casts are
                // unchecked, and an unhandled InvalidCastException takes the whole app down —
                // there is no DispatcherUnhandledException handler.
                var link = registry.AllPlotPointSubjectLinkViewModels.FirstOrDefault(l => l.Id == ownerId);
                if (link is not null &&
                    registry.AllSubjectViewModels.FirstOrDefault(s => s.Id == link.SubjectId) is { } linkSubject)
                    windowManager.OpenSubjectWindow(linkSubject, EditorMode.Linking, link);
                break;
        }
    }

    public static void Open(NoteViewModel note, IViewModelRegistry registry, IWindowManager windowManager) =>
        Open(note.OwnerId, note.OwnerType, registry, windowManager);
}
