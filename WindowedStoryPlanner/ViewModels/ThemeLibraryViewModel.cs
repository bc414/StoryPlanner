using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using StoryPlanner.Core.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowedStoryPlanner.ViewModels;

/// <summary>
/// Tab ViewModel for the Theme Library editor.
/// Follows the same pattern as DefinitionsEditorViewModel:
/// collections live in IViewModelRegistry, this class only handles commands.
/// </summary>
public partial class ThemeLibraryViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;

    // Registry-owned collection — exposed as passthrough for XAML binding
    public ObservableCollection<ThemeViewModel> Themes => _registry.AllThemeViewModels;

    public ThemeLibraryViewModel(IStoryService storyService, IViewModelRegistry registry)
    {
        _storyService = storyService;
        _registry     = registry;
    }

    /// <summary>
    /// Called by ProjectLoader after it repopulates the registry.
    /// </summary>
    public void Reload() { /* registry already repopulated by ProjectLoader */ }

    [RelayCommand]
    private async Task AddTheme()
    {
        var model = new Theme { Name = "New Theme", Proposition = string.Empty };
        _storyService.Themes.Add(model);
        await _storyService.SaveAsync();
        Themes.Add(new ThemeViewModel(model, _storyService));
    }

    [RelayCommand]
    private async Task DeleteTheme(ThemeViewModel vm)
    {
        _storyService.Themes.Remove(vm.Model);
        Themes.Remove(vm);
        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private async Task SaveAll()
    {
        await _storyService.SaveAsync();
    }

    [RelayCommand]
    private void ExportThemesToMarkdown()
    {
        string projectPath = _storyService.CurrentFilePath;
        string projectName = Path.GetFileNameWithoutExtension(projectPath);
        string outputPath  = Path.Combine(Path.GetDirectoryName(projectPath)!, $"{projectName}-themes.md");

        var data = Themes.Select(t => new ThemeExportData(t.Name, t.Proposition));
        string markdown = ThemesMarkdownExporter.Build(data);
        File.WriteAllText(outputPath, markdown);

        MessageBox.Show($"Exported to:\n{outputPath}", "Export Complete",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private void OpenThemeWindow(ThemeViewModel vm)
    {
        // TODO: open a dedicated theme detail window for the given theme
    }
}
