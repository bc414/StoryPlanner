using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using WindowedStoryPlanner.ViewModels;
using StoryPlanner.Core.Models;

namespace WindowedStoryPlanner.Views
{
    public class EntityToPayloadConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return null;

            // 1. Identify which input is the PlotPoint and which is the Context Entity
            //    We check both slots because the order depends on where the binding is written.
            

            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}