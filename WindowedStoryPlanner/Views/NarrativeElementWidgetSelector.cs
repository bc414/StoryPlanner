using System.Windows;
using System.Windows.Controls;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    public class NarrativeElementWidgetSelector : DataTemplateSelector
    {
        public DataTemplate? SubjectTemplate { get; set; }
        public DataTemplate? PlotPointTemplate { get; set; }
        public DataTemplate? LinkTemplate { get; set; }
        public DataTemplate? ChapterTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
        {
            SubjectViewModel              => SubjectTemplate,
            PlotPointViewModel            => PlotPointTemplate,
            PlotPointSubjectLinkViewModel => LinkTemplate,
            ChapterViewModel              => ChapterTemplate,
            _                             => null
        };
    }
}