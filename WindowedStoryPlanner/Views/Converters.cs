using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using WindowedStoryPlanner.ViewModels;

namespace WindowedStoryPlanner.Views // Adjust namespace if needed
{
    /// <summary>Visible when false — the complement of BooleanToVisibilityConverter, for the
    /// pairs of templates that swap on a single bool (e.g. a theater column's expanded vs
    /// collapsed header).</summary>
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is true ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the Command is null, Collapse. Otherwise, Visible.
            // ConverterParameter="Invert" flips that (null => Visible, non-null => Collapsed).
            bool isNull = value == null;
            bool invert = string.Equals(parameter as string, "Invert", StringComparison.OrdinalIgnoreCase);
            if (invert) isNull = !isNull;
            return isNull ? Visibility.Collapsed : Visibility.Visible;
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
                // Layer 1 - Red
                TrackType.Ontology              => Color.FromRgb(0xF4, 0xAA, 0xA8), // Pastel Red         - foundational rules

                // Layer 2 - Oranges
                TrackType.History               => Color.FromRgb(0xF4, 0xC8, 0x9A), // Pastel Orange      - temporal record
                TrackType.Civilization          => Color.FromRgb(0xF8, 0xDA, 0xA0), // Pastel Amber       - built world

                // Layer 3 - Yellow
                TrackType.Characterization      => Color.FromRgb(0xF8, 0xF0, 0x9C), // Pastel Yellow      - psychological depth

                // Layer 4 - Greens
                TrackType.NarrativeArchitecture => Color.FromRgb(0xB8, 0xE4, 0xA8), // Pastel Green       - structural design
                TrackType.PageDesign            => Color.FromRgb(0x9C, 0xDC, 0xA4), // Pastel Pure Green  - staging & scene (prose-facing)
                TrackType.WorldInference        => Color.FromRgb(0x90, 0xC4, 0xE8), // Pastel Cyan-Blue   - reader cognition (must not enter prose)

                // Layer 5 - Light Blue (abstract philosophical output)
                TrackType.ThematicEvidence      => Color.FromRgb(0xB8, 0xD4, 0xF8), // Pastel Light Blue  - philosophical meaning

                // Author Voice - wraps the color wheel back toward red via violet/purple
                TrackType.Allegories            => Color.FromRgb(0x88, 0xB0, 0xF0), // Pastel Deep Blue   - deliberate social commentary
                TrackType.Analogies             => Color.FromRgb(0xA4, 0x98, 0xEC), // Pastel Blue-Violet - real-world inspiration
                TrackType.Canon                 => Color.FromRgb(0xC0, 0x9C, 0xE0), // Pastel Purple      - established canon (blue+red, echoes ontology)
                TrackType.NotesToSelf           => Color.FromRgb(0xF2, 0xB0, 0xCC), // Pastel Pink        - intimate author voice

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

    /// <summary>
    /// Shows an element only when the bound Platform string ("Claude"/"Gemini") matches
    /// ConverterParameter — used to switch between brand-styled badges per platform.
    /// </summary>
    public class PlatformToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            string.Equals(value as string, parameter as string, StringComparison.OrdinalIgnoreCase)
                ? Visibility.Visible
                : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>
    /// Resolves a note's "from: ..." breadcrumb inside ThemeWindow/SourceMaterialWindow.
    /// values[0] is the NoteViewModel (the ItemTemplate's own DataContext),
    /// values[1] is the hosting window's TaggedNotesViewModelBase DataContext.
    /// </summary>
    public class ConversationDerivedStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is ConversationDerivedState state
                ? state switch
                {
                    ConversationDerivedState.Unstarted  => new SolidColorBrush(Color.FromRgb(160, 160, 160)),
                    ConversationDerivedState.InProgress => new SolidColorBrush(Color.FromRgb(230, 150,  30)),
                    ConversationDerivedState.Complete   => new SolidColorBrush(Color.FromRgb( 40, 160,  80)),
                    _                                   => Brushes.Gray
                }
                : Brushes.Gray;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class ConversationDerivedStateToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is ConversationDerivedState state
                ? state switch
                {
                    ConversationDerivedState.Unstarted  => "Unstarted",
                    ConversationDerivedState.InProgress => "In Progress",
                    ConversationDerivedState.Complete   => "Complete",
                    _                                   => string.Empty
                }
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class BlockStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is BlockState state
                ? state switch
                {
                    BlockState.Unread  => new SolidColorBrush(Color.FromRgb(160, 160, 160)), // gray
                    BlockState.Skipped => new SolidColorBrush(Color.FromRgb( 70, 130, 210)), // blue
                    BlockState.Flagged => new SolidColorBrush(Color.FromRgb(220, 110,  20)), // orange
                    BlockState.Done    => new SolidColorBrush(Color.FromRgb( 40, 160,  80)), // green
                    _                  => Brushes.Gray
                }
                : Brushes.Gray;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value is bool b && b;
            bool invert = string.Equals(parameter as string, "Invert", StringComparison.OrdinalIgnoreCase);
            return (flag ^ invert) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>
    /// Pale/pastel version of BlockStateToColorConverter, for filling an entire row's
    /// background rather than a thin accent bar. Selection/hover are layered on top
    /// of this in the reader window's ListBoxItem template, not mixed into these values.
    /// </summary>
    public class BlockStateToRowBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is BlockState state
                ? state switch
                {
                    BlockState.Unread  => new SolidColorBrush(Color.FromRgb(242, 242, 242)), // pale gray
                    BlockState.Skipped => new SolidColorBrush(Color.FromRgb(227, 237, 251)), // pale blue
                    BlockState.Flagged => new SolidColorBrush(Color.FromRgb(253, 234, 217)), // pale orange
                    BlockState.Done    => new SolidColorBrush(Color.FromRgb(227, 245, 232)), // pale green
                    _                  => Brushes.White
                }
                : Brushes.White;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    /// <summary>Badge color for a Scan Preview row's advisory classification.</summary>
    public class SyncClassificationToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is ConversationSyncClassification c
                ? c switch
                {
                    ConversationSyncClassification.New               => new SolidColorBrush(Color.FromRgb( 70, 130, 210)), // blue
                    ConversationSyncClassification.Reopened          => new SolidColorBrush(Color.FromRgb( 40, 160,  80)), // green
                    ConversationSyncClassification.Unchanged         => new SolidColorBrush(Color.FromRgb(160, 160, 160)), // gray
                    ConversationSyncClassification.NeedsConfirmation => new SolidColorBrush(Color.FromRgb(220, 110,  20)), // orange
                    ConversationSyncClassification.Ignored           => new SolidColorBrush(Color.FromRgb(190, 190, 190)), // pale gray
                    _                                                => Brushes.Gray
                }
                : Brushes.Gray;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class SyncClassificationToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is ConversationSyncClassification c
                ? c switch
                {
                    ConversationSyncClassification.New               => "New",
                    ConversationSyncClassification.Reopened          => "Reopened",
                    ConversationSyncClassification.Unchanged         => "Unchanged",
                    ConversationSyncClassification.NeedsConfirmation => "Needs confirmation",
                    ConversationSyncClassification.Ignored           => "Ignored",
                    _                                                => string.Empty
                }
                : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class NoteBreadcrumbConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is [NoteViewModel note, TaggedNotesViewModelBase owner])
                return $"from: {owner.Breadcrumb(note)}";
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}