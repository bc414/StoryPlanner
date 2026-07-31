namespace StoryPlanner.Core;

/// <summary>
/// A key/value row for UI preferences that persist with the file (e.g. the timeline
/// viewport). Settings live in the .storyplan itself — this is a single-user desktop
/// app, and prefs traveling with the file beats machine-local state (decided
/// 2026-07-30; supersedes the %LOCALAPPDATA% proposal in TIMELINE-REFACTOR-BACKLOG).
/// Value is an opaque payload the owning feature serializes; readers must tolerate
/// missing rows and unparseable payloads by falling back to defaults.
/// </summary>
public class UiSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
