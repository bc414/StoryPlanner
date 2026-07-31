using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Specialized;
using System.Linq;

namespace WindowedStoryPlanner;

/// <summary>
/// Cross-cut of notes whose track declares an optional field applicable but which carry no value
/// for it — the same "show me every note where X" machinery as the theme and source-material
/// views, with emptiness as the criterion instead of a tag.
///
/// Retrieval only, and this one sits closest to the line. It reports which notes have an empty
/// field and nothing more: no ordering by how worth filling a note looks, no proposed values, no
/// completion percentage, and no claim that an empty field is a defect. Whether an absence is a
/// gap or a deliberate state is the author's to know — a missing theme is frequently permanent.
///
/// Membership is live, so a note leaves the list the moment its field is filled in from the
/// editor embedded in the row. That is the whole working loop.
/// </summary>
public partial class MissingFieldNotesViewModel : TaggedNotesViewModelBase
{
    // Field initializer, not a constructor assignment: derived initializers run BEFORE the base
    // constructor, which seeds the list by calling Matches(), which reads this.
    [ObservableProperty]
    private MissingNoteField _selectedField = MissingNoteField.SourceMaterial;

    public MissingFieldNotesViewModel(IViewModelRegistry registry, MissingNoteField field)
        : base(registry)
    {
        SelectedField = field;
        ((INotifyCollectionChanged)Notes).CollectionChanged += (_, _) => RaiseStatus();
    }

    protected override bool Matches(NoteViewModel note) => SelectedField switch
    {
        MissingNoteField.Theme => note.SupportsTheme && note.SelectedTheme is null,
        MissingNoteField.SourceMaterial => note.SupportsSourceMaterial && note.SourceReferences.Count == 0,
        _ => false
    };

    // The union of every property either criterion depends on — cheap, and correct whichever
    // field is selected. A track change moves both the Supports* flag and the value.
    protected override bool AffectsMembership(string? propertyName) =>
        propertyName is nameof(NoteViewModel.SelectedTheme)
                     or nameof(NoteViewModel.SourceReferences)
                     or nameof(NoteViewModel.SupportsTheme)
                     or nameof(NoteViewModel.SupportsSourceMaterial)
                     or null or "";

    partial void OnSelectedFieldChanged(MissingNoteField value)
    {
        Reevaluate();
        RaiseStatus();
        OnPropertyChanged(nameof(IsThemeSelected));
        OnPropertyChanged(nameof(IsSourceSelected));
    }

    // Two-way radio-button bindings; a ComboBox over an enum would need a converter for no gain.
    public bool IsThemeSelected
    {
        get => SelectedField == MissingNoteField.Theme;
        set { if (value) SelectedField = MissingNoteField.Theme; }
    }

    public bool IsSourceSelected
    {
        get => SelectedField == MissingNoteField.SourceMaterial;
        set { if (value) SelectedField = MissingNoteField.SourceMaterial; }
    }

    // ── Status ──────────────────────────────────────────────────────────────────
    // Describes the retrieval, never characterises it. No "should", no "gap", no share of a
    // total — a count and the predicate that produced it.

    public string StatusLine => SelectedField switch
    {
        MissingNoteField.Theme =>
            $"{Notes.Count} note(s) on a track that supports themes, carrying no theme.",
        MissingNoteField.SourceMaterial =>
            $"{Notes.Count} note(s) on a track that supports source material, carrying no citation.",
        _ => ""
    };

    public bool HasNotes => Notes.Count > 0;

    public string EmptyText => SelectedField switch
    {
        MissingNoteField.Theme => "Every note on a theme-supporting track carries a theme.",
        MissingNoteField.SourceMaterial => "Every note on a source-supporting track carries a citation.",
        _ => ""
    };

    private void RaiseStatus()
    {
        OnPropertyChanged(nameof(StatusLine));
        OnPropertyChanged(nameof(EmptyText));
        OnPropertyChanged(nameof(HasNotes));
    }
}
