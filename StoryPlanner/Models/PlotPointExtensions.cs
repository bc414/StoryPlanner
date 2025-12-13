namespace StoryPlanner.Models;

public static class PlotPointExtensions
{
    public static void UpdateAllOrderInChapter(this List<PlotPoint> plotPoints)
    {
        for (int i = 0; i < plotPoints.Count; i++)
        {
            plotPoints[i].OrderInChapter = i;
        }
    }
}