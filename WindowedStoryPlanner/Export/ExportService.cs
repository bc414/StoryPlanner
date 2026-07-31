using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using StoryPlanner.Core;

namespace WindowedStoryPlanner;

public class ExportService
{
    private readonly IStoryService _storyService;

    public ExportService(IStoryService storyService)
    {
        _storyService = storyService;
    }

    public string GetExportsDirectory()
    {
        var projectName = Path.GetFileNameWithoutExtension(_storyService.CurrentFilePath);
        return Path.Combine(Path.GetDirectoryName(_storyService.CurrentFilePath)!, $"{projectName}_Exports");
    }

    public string BuildMarkdown(ExportConfiguration config)
    {
        var result = ExportResolver.Resolve(config, _storyService);
        return NoteExportRenderer.Build(result, config, _storyService);
    }

    // Writes markdown to a file and returns the written path.
    public string WriteToFile(string markdown, string relativeSubDir, string fileName)
    {
        var dir = string.IsNullOrEmpty(relativeSubDir)
            ? GetExportsDirectory()
            : Path.Combine(GetExportsDirectory(), relativeSubDir);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, SanitizeFileName(fileName) + ".md");
        File.WriteAllText(path, markdown);
        return path;
    }

    public void CopyToClipboard(string markdown) => Clipboard.SetText(markdown);

    // Req 1: Export all entities as full into one file.
    public string ExportAll()
    {
        var result   = ExportResolver.ResolveAll(_storyService);
        var config   = AllInclusiveConfig();
        var markdown = NoteExportRenderer.Build(result, config, _storyService);
        return WriteToFile(markdown, "", "full-export");
    }

    // Req 2: Export each entity into its own file. Returns (subjectCount, plotPointCount).
    public (int subjects, int plotPoints) ExportAllIndividual()
    {
        var renderConfig = AllInclusiveConfig();
        int s = 0, pp = 0;

        foreach (var subject in _storyService.Subjects)
        {
            var resolveConfig = SingleAnchorConfig(subject.Id, OwnerType.Subject, scope: 0);
            var result        = ExportResolver.Resolve(resolveConfig, _storyService);
            var md            = NoteExportRenderer.Build(result, renderConfig, _storyService);
            WriteToFile(md, Path.Combine("Individual", "Subjects"), subject.Name);
            s++;
        }

        foreach (var plotPoint in _storyService.PlotPoints)
        {
            var resolveConfig = SingleAnchorConfig(plotPoint.Id, OwnerType.PlotPoint, scope: 1);
            var result        = ExportResolver.Resolve(resolveConfig, _storyService);
            var md            = NoteExportRenderer.Build(result, renderConfig, _storyService);
            var chapter       = _storyService.Chapters.FirstOrDefault(c => c.Id == plotPoint.ChapterId);
            var subDir        = chapter != null
                ? Path.Combine("Individual", "PlotPoints", $"Ch{chapter.OrderIndex:D2} - {SanitizeFileName(chapter.Title)}")
                : Path.Combine("Individual", "PlotPoints", "(No Chapter)");
            WriteToFile(md, subDir, plotPoint.Title);
            pp++;
        }

        return (s, pp);
    }

    // Req 3: Build markdown for a single entity at the given scope (all TrackTypes included).
    public string BuildQuickMarkdown(int id, OwnerType ownerType, int scope)
        => BuildMarkdown(SingleAnchorConfig(id, ownerType, scope));

    private ExportConfiguration AllInclusiveConfig() => new()
    {
        IncludedTrackTypes = new HashSet<TrackType>(Enum.GetValues<TrackType>())
    };

    private ExportConfiguration SingleAnchorConfig(int id, OwnerType ownerType, int scope) => new()
    {
        Anchors            = new List<(int, OwnerType)> { (id, ownerType) },
        Scope              = scope,
        IncludedTrackTypes = new HashSet<TrackType>(Enum.GetValues<TrackType>())
    };

    public static string SanitizeFileName(string name)
    {
        foreach (var ch in Path.GetInvalidFileNameChars())
            name = name.Replace(ch, '_');
        return name;
    }
}
