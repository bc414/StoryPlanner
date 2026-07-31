namespace WindowedStoryPlanner;

/// <summary>
/// Which optional note field the missing-field cross-cut is looking at. Each value names a field
/// whose applicability is declared by the track (NoteTrackDefinition.Supports*), so "missing"
/// means the author's own definition row says the field applies here and no value is recorded.
///
/// World date is deliberately absent: the Timeline tab's triage panel already covers it, and does
/// it better — dates can be assigned in place there, and it handles plot points too.
/// </summary>
public enum MissingNoteField
{
    Theme,
    SourceMaterial
}
