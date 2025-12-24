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
            // We need the Row Item (e.g., CharacterViewModel) and the Parent (PlotPointViewModel)
            if (values.Length < 2 || values[1] is not PlotPointViewModel plotPointVM) 
                return null;

            // 1. Handle Characters
            if (values[0] is CharacterViewModel charVM)
            {
                return plotPointVM.Model.CharacterAppearances
                    .FirstOrDefault(x => x.CharacterId == charVM.Character.Id);
            }

            // 2. Handle Themes
            if (values[0] is ThemeViewModel themeVM)
            {
                return plotPointVM.Model.ThemeAssignments
                    .FirstOrDefault(x => x.ThemeId == themeVM.Theme.Id);
            }

            // 3. Handle Threads
            if (values[0] is StoryThreadViewModel threadVM)
            {
                return plotPointVM.Model.ThreadAssignments
                    .FirstOrDefault(x => x.ThreadId == threadVM.StoryThread.Id);
            }

            if (values[0] is CodexEntryViewModel codexEntryVM)
            {
                return plotPointVM.Model.CodexReferences.FirstOrDefault(x =>
                    x.CodexEntryId == codexEntryVM.CodexEntry.Id);
            }
            
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}