using StoryPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowedStoryPlanner.ViewModels
{
    public interface IEditorCoordinator
    {
        void OpenEditorWindow(OwnerViewModel viewModel);
    }
}
