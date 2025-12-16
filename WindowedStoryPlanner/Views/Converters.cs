using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

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
}