using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? !b : false;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? !b : false;
    }

    public class CanMoveToChapterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is ChapterViewModel || value is FloatingPlotPointsViewModel;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class IsFloatingWindowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is FloatingPlotPointsViewModel;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class IsEntityViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is EntityViewModel;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class IdeaStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IdeaState ideaState)
            {
                return ideaState switch
                {
                    IdeaState.Written           => Brushes.Red,
                    IdeaState.PartiallyAnalyzed => Brushes.Yellow,
                    _                           => Brushes.LawnGreen
                };
            }
            return Brushes.CornflowerBlue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    public class UtcToLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime utcDateTime)
                return utcDateTime.ToLocalTime().ToString("g");
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParse(value?.ToString(), out DateTime localDateTime))
                return localDateTime.ToUniversalTime();
            return value;
        }
    }

    public class RecencyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime lastModified)
            {
                TimeSpan diff = DateTime.UtcNow - lastModified;
                return diff.TotalHours <= 24 ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Black;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Maps CognitiveMode to a border/accent Brush for NoteTrackView.
    /// Edit the switch expression here to retheme all tracks at once.
    ///
    /// ZeroFocalization  — deep blue       (historian, in-universe factual)
    /// SceneArchitecture — amber/gold      (director, structural)
    /// Metatextual       — purple          (author to self, project-level)
    /// Analogical        — teal            (bridging fiction ↔ reality)
    /// LinguisticExecution — forest green  (prose writer mode)
    /// </summary>
    public class CognitiveModeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CognitiveMode mode)
            {
                return mode switch
                {
                    CognitiveMode.ZeroFocalization    => new SolidColorBrush(Color.FromRgb(0x42, 0x85, 0xF4)), // Google Blue
                    CognitiveMode.SceneArchitecture   => new SolidColorBrush(Color.FromRgb(0xF9, 0xAB, 0x00)), // Amber
                    CognitiveMode.Metatextual         => new SolidColorBrush(Color.FromRgb(0x9C, 0x27, 0xB0)), // Purple
                    CognitiveMode.Analogical          => new SolidColorBrush(Color.FromRgb(0x00, 0x89, 0x7B)), // Teal
                    CognitiveMode.LinguisticExecution => new SolidColorBrush(Color.FromRgb(0x2E, 0x7D, 0x32)), // Forest Green
                    _                                 => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}