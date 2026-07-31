using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoryPlanner.Core;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowedStoryPlanner;

/// <summary>
/// Tab ViewModel for the Theme Library editor.
/// Follows the same pattern as DefinitionsEditorViewModel:
/// collections live in IViewModelRegistry, this class only handles commands.
/// </summary>
public partial class ThemeLibraryViewModel : ObservableObject
{
    private readonly IStoryService _storyService;
    private readonly IViewModelRegistry _registry;
    private readonly IWindowManager _windowManager;

    // Registry-owned collection — exposed as passthrough for XAML binding
    public ObservableCollection<ThemeViewModel> Themes => _registry.AllThemeViewModels;

    public ThemeLibraryViewModel(IStoryService storyService, IViewModelRegistry registry, IWindowManager windowManager)
    {
        _storyService  = storyService;
        _registry      = registry;
        _windowManager = windowManager;
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
        _windowManager.OpenThemeWindow(vm);
    }

    /// <summary>The other side of the theme cross-cut: notes on a theme-supporting track that
    /// carry no theme. Retrieval — an untagged note is a legal state, not a defect.</summary>
    [RelayCommand]
    private void OpenNotesWithoutTheme() =>
        _windowManager.OpenMissingFieldWindow(MissingNoteField.Theme);
}
