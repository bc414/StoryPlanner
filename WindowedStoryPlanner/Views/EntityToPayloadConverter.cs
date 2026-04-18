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
            var plotPointVM = values[0] as PlotPointViewModel ?? values[1] as PlotPointViewModel;
            var contextItem = (values[0] is PlotPointViewModel) ? values[1] : values[0];

            if (plotPointVM == null || contextItem == null) return null;

            // 2. Perform the lookup based on the Type of the context item
            if (contextItem is CharacterViewModel charVM)
            {
                return plotPointVM.Model.CharacterAppearances
                    .FirstOrDefault(x => x.CharacterId == charVM.Character.Id);
            }

            if (contextItem is ThemeViewModel themeVM)
            {
                return plotPointVM.Model.ThemeAssignments
                    .FirstOrDefault(x => x.ThemeId == themeVM.Theme.Id);
            }

            if (contextItem is StoryThreadViewModel threadVM)
            {
                return plotPointVM.Model.ThreadAssignments
                    .FirstOrDefault(x => x.StoryThreadId == threadVM.StoryThread.Id);
            }
    
            if (contextItem is CodexEntryViewModel codexVM)
            {
                return plotPointVM.Model.CodexReferences
                    .FirstOrDefault(x => x.CodexEntryId == codexVM.CodexEntry.Id);
            }

            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}