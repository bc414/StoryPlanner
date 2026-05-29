using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views // Adjust namespace if needed
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the Command is null, Collapse. Otherwise, Visible.
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false; // Fallback if binding fails
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Two negatives make a positive, so logic is identical
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }

    public class CanMoveToChapterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ChapterViewModel || value is FloatingPlotPointsViewModel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }

    public class IsFloatingWindowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Returns TRUE if the bound object (the Window's DataContext) is the Floating VM
            return value is FloatingPlotPointsViewModel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }

    public class IdeaStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IdeaState ideaState)
            {
                if (ideaState == IdeaState.Written)
                {
                    return Brushes.Red;
                }
                else if (ideaState == IdeaState.PartiallyAnalyzed)
                {
                    return Brushes.Yellow;
                }
                else
                {
                    return Brushes.LawnGreen;
                }
            }

            return Brushes.CornflowerBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }

    public class UtcToLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime utcDateTime)
            {
                // Converts from UTC to the local system time of the user's machine
                return utcDateTime.ToLocalTime().ToString("g"); // "g" is a short date/time pattern
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If you need two-way binding, convert back to UTC before saving
            if (DateTime.TryParse(value?.ToString(), out DateTime localDateTime))
            {
                return localDateTime.ToUniversalTime();
            }
            return value;
        }
    }

    public class RecencyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime lastModified)
            {
                // Calculate the difference between now (UTC) and the modified time (UTC)
                TimeSpan diff = DateTime.UtcNow - lastModified;

                if (diff.TotalHours <= 24)
                {
                    return Brushes.Green; // Modified within the last 24 hours
                }
                return Brushes.Red; // Older than 24 hours
            }
            return Brushes.Black; // Default fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CognitiveModeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not TrackType mode)
                return new SolidColorBrush(Colors.LightGray);

            var color = mode switch
            {
                // Layer 1 - Reds
                TrackType.Ontology              => Color.FromRgb(0xF4, 0xAA, 0xA8), // Pastel Red        - foundational rules

                // Layer 2 - Oranges
                TrackType.History               => Color.FromRgb(0xF4, 0xC8, 0x9A), // Pastel Orange     - temporal record
                TrackType.Civilization          => Color.FromRgb(0xF8, 0xDA, 0xA0), // Pastel Amber      - built world

                // Layer 3 - Yellows
                TrackType.Characterization      => Color.FromRgb(0xF8, 0xF0, 0x9C), // Pastel Yellow     - psychological depth

                // Layer 4 - Greens
                TrackType.NarrativeArchitecture => Color.FromRgb(0xB8, 0xE4, 0xA8), // Pastel Green       - structural design
                TrackType.PageDesign            => Color.FromRgb(0x9C, 0xDC, 0xA4), // Pastel Pure Green  - staging & scene (prose-facing)
                TrackType.WorldInference        => Color.FromRgb(0x90, 0xC4, 0xE8), // Pastel Cyan-Blue   - reader cognition (must not enter prose)

                // Layer 5 - Blues
                TrackType.ThematicEvidence      => Color.FromRgb(0xA0, 0xB0, 0xF4), // Pastel Blue        - philosophical meaning

                // Author Voice - Indigo → Purple → Pink
                TrackType.Analogies             => Color.FromRgb(0xB0, 0xB0, 0xE8), // Pastel Indigo     - real-world connection
                TrackType.Canon                 => Color.FromRgb(0xCC, 0xAA, 0xE4), // Pastel Purple     - established canon
                TrackType.NotesToSelf           => Color.FromRgb(0xF2, 0xB0, 0xCC), // Pastel Pink       - intimate author voice

                // Unset
                _ => Colors.LightGray
            };

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}