using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// PlanIntegrity is the id-based extraction of the referential-integrity assumptions
/// ContentDeleter's guards encode ad hoc against view models — see the testing skill's
/// "Known gap" note. These tests are its first direct coverage: seed a fixture with rows
/// that deliberately violate the invariants no foreign key enforces, and confirm Check
/// reports them rather than silently returning plausible-looking wrong data.
/// </summary>
public class PlanIntegrityTests
{
    [Fact]
    public void Check_passes_clean_on_an_unmodified_synthetic_plan()
    {
        using var plan = SyntheticPlan.Create();
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Empty(violations);
    }

    [Fact]
    public void Check_reports_a_note_whose_owner_does_not_exist()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.Notes.Add(new Note
        {
            Id = 999, OwnerId = 424242, OwnerType = OwnerType.Subject,
            NoteState = NoteState.Unset, Content = "orphaned", SortOrder = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "note.owner_missing" && v.Detail.Contains("note:999"));
    }

    [Fact]
    public void Check_reports_a_link_whose_plot_point_or_subject_is_dangling()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPointSubjectLinks.Add(new PlotPointSubjectLink
        {
            Id = 999, PlotPointId = 424242, SubjectId = 424243
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "link.plotpoint_missing" && v.Detail.Contains("link:999"));
        Assert.Contains(violations, v => v.Rule == "link.subject_missing" && v.Detail.Contains("link:999"));
    }

    [Fact]
    public void Check_reports_a_plot_point_pointing_at_a_missing_chapter()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPoints.Add(new PlotPoint
        {
            Id = 999, Title = "Dangling", ChapterId = 424242, OrderInChapter = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "plotpoint.chapter_missing" && v.Detail.Contains("plotpoint:999"));
    }

    [Fact]
    public void Check_reports_a_plot_point_pointing_at_a_missing_focal_character()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PlotPoints.Add(new PlotPoint
        {
            Id = 999, Title = "Dangling focal", FocalCharacterId = 424242, OrderInChapter = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "plotpoint.focal_character_missing" && v.Detail.Contains("plotpoint:999"));
    }

    [Fact]
    public void Check_passes_when_focal_character_is_null_or_resolves()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
            ctx.PlotPoints.Single(p => p.Id == SyntheticPlan.PlotPointId).FocalCharacterId = SyntheticPlan.SubjectId);
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.DoesNotContain(violations, v => v.Rule == "plotpoint.focal_character_missing");
    }

    [Fact]
    public void Check_reports_a_source_material_part_pointing_at_a_missing_work()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.SourceMaterialParts.Add(new SourceMaterialPart
        {
            Id = 999, SourceMaterialId = 424242, Code = "S1E01"
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "sourcepart.material_missing" && v.Detail.Contains("part:999"));
    }

    [Fact]
    public void Check_reports_a_note_source_reference_whose_note_or_material_is_dangling()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.NoteSourceReferences.Add(new NoteSourceReference
        {
            Id = 999, NoteId = 424242, SourceMaterialId = 424243
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "sourcereference.note_missing" && v.Detail.Contains("reference:999"));
        Assert.Contains(violations, v => v.Rule == "sourcereference.material_missing" && v.Detail.Contains("reference:999"));
    }

    [Fact]
    public void Check_reports_a_reference_whose_part_belongs_to_a_different_work()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SourceMaterials.AddRange(
                new SourceMaterial { Id = 1, Name = "MLP:FiM" },
                new SourceMaterial { Id = 2, Name = "Equestria at War" });
            ctx.SourceMaterialParts.Add(new SourceMaterialPart { Id = 1, SourceMaterialId = 1, Code = "S3E01" });
            // References material 2 but cites a Part that actually belongs to material 1 — the
            // invariant PlanIntegrity is meant to catch since nothing in the schema can enforce it.
            ctx.NoteSourceReferences.Add(new NoteSourceReference
            {
                Id = 999, NoteId = SyntheticPlan.VisibleNoteId, SourceMaterialId = 2, SourceMaterialPartId = 1
            });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "sourcereference.part_parent_mismatch" && v.Detail.Contains("reference:999"));
    }

    [Fact]
    public void Check_reports_a_second_narrative_property_value_for_the_same_owner_and_property()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Test Axis"
            });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "Pole A" },
                new NarrativePropertyValueDefinition { Id = 2, NarrativePropertyDefinitionId = 1, ValueName = "Pole B" });
            // Both poles assigned to the same subject — narrative properties are single-select and
            // nothing in the schema can say so, which is exactly why PlanIntegrity has to.
            ctx.NarrativePropertyValues.AddRange(
                new NarrativePropertyValue { Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 1 },
                new NarrativePropertyValue { Id = 2, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 2 });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "narrativevalue.duplicate_for_property"
                                      && v.Detail.Contains("property:1"));
    }

    [Fact]
    public void Check_does_not_confuse_a_subject_and_a_chapter_that_share_an_id()
    {
        // NarrativePropertyValue has no OwnerType column, so an OwnerId-only predicate would read
        // these two assignments as a duplicate on one owner. The fixture's SubjectId and ChapterId
        // are both 1, which is what makes this reproducible.
        Assert.Equal(SyntheticPlan.SubjectId, SyntheticPlan.ChapterId);

        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.NarrativePropertyDefinitions.AddRange(
                new NarrativePropertyDefinition
                {
                    Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.Subject, Name = "Subject Axis"
                },
                new NarrativePropertyDefinition
                {
                    Id = 2, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                    OwnerType = OwnerType.Chapter, Name = "Chapter Axis"
                });
            ctx.NarrativePropertyValueDefinitions.AddRange(
                new NarrativePropertyValueDefinition { Id = 1, NarrativePropertyDefinitionId = 1, ValueName = "Subject value" },
                new NarrativePropertyValueDefinition { Id = 2, NarrativePropertyDefinitionId = 2, ValueName = "Chapter value" });
            ctx.NarrativePropertyValues.AddRange(
                new NarrativePropertyValue { Id = 1, OwnerId = SyntheticPlan.SubjectId, ValueDefinitionId = 1 },
                new NarrativePropertyValue { Id = 2, OwnerId = SyntheticPlan.ChapterId, ValueDefinitionId = 2 });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.DoesNotContain(violations, v => v.Rule == "narrativevalue.duplicate_for_property");
        Assert.DoesNotContain(violations, v => v.Rule == "narrativevalue.owner_missing");
    }

    [Fact]
    public void Check_reports_a_property_definition_gating_on_a_missing_work_phase()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
        {
            Id = 999, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
            OwnerType = OwnerType.Subject, Name = "Dangling gate", GatingWorkPhaseId = 424242
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "narrativepropertydefinition.workphase_missing"
                                      && v.Detail.Contains("property:999"));
    }

    [Fact]
    public void Check_passes_when_a_property_gates_on_a_work_phase_that_exists()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.WorkPhases.Add(new WorkPhase
            {
                Id = 1, Name = "Expansion", DisplayOrder = 1, RequiresZeroFlaggedNotes = true
            });
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 1, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Gated", GatingWorkPhaseId = 1
            });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.DoesNotContain(violations, v => v.Rule == "narrativepropertydefinition.workphase_missing");
    }

    [Fact]
    public void ComputeNoteChecksum_is_stable_across_reads_and_changes_when_content_changes()
    {
        using var plan = SyntheticPlan.Create();
        using var ctx1 = OpenContext(plan.Path);
        using var ctx2 = OpenContext(plan.Path);

        Assert.Equal(PlanIntegrity.ComputeNoteChecksum(ctx1), PlanIntegrity.ComputeNoteChecksum(ctx2));

        var before = PlanIntegrity.ComputeNoteChecksum(ctx1);
        plan.ExternalWrite(ctx =>
        {
            var note = ctx.Notes.First(n => n.Id == SyntheticPlan.VisibleNoteId);
            note.Content += " changed";
        });
        using var ctx3 = OpenContext(plan.Path);
        var after = PlanIntegrity.ComputeNoteChecksum(ctx3);

        Assert.NotEqual(before, after);
    }

    [Fact]
    public void CompareRowCounts_flags_only_tables_outside_the_allowed_set()
    {
        var before = new Dictionary<string, long> { ["Chapters"] = 1, ["Notes"] = 5 };
        var afterGrew = new Dictionary<string, long> { ["Chapters"] = 2, ["Notes"] = 5 };

        var violations = PlanIntegrity.CompareRowCounts(before, afterGrew, allowedToChange: new HashSet<string> { "Chapters" });
        Assert.Empty(violations);

        var violationsUnexpected = PlanIntegrity.CompareRowCounts(before, afterGrew, allowedToChange: new HashSet<string>());
        Assert.Single(violationsUnexpected);
        Assert.Equal("rowcount.changed", violationsUnexpected[0].Rule);
    }

    // ── Orphans the 2026-08-02 audit found invisible to Check ────────────────
    // These line up one-to-one with delete paths that used to bypass ContentDeleter:
    // track-definition delete, theme delete, subject-definition delete. The checks make
    // that class of orphan detectable; the guards now make it unreachable from the UI.

    [Fact]
    public void Check_reports_a_note_carrying_a_deleted_track_definition()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.Notes.Add(new Note
        {
            Id = 999, OwnerId = SyntheticPlan.SubjectId, OwnerType = OwnerType.Subject,
            NoteTrackDefinitionId = 424242, NoteState = NoteState.Unset, Content = "demoted", SortOrder = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "note.track_missing" && v.Detail.Contains("note:999"));
    }

    [Fact]
    public void Check_does_not_flag_a_null_track_id_or_theme_id()
    {
        // Null = "Unassigned" / untagged — legal, long-lived states, not missing data.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.Notes.Add(new Note
        {
            Id = 999, OwnerId = SyntheticPlan.SubjectId, OwnerType = OwnerType.Subject,
            NoteTrackDefinitionId = null, ThemeId = null,
            NoteState = NoteState.Unset, Content = "unassigned", SortOrder = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.DoesNotContain(violations, v => v.Rule == "note.track_missing");
        Assert.DoesNotContain(violations, v => v.Rule == "note.theme_missing");
    }

    [Fact]
    public void Check_reports_a_note_tagged_with_a_deleted_theme()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.Notes.Add(new Note
        {
            Id = 999, OwnerId = SyntheticPlan.SubjectId, OwnerType = OwnerType.Subject,
            ThemeId = 424242, NoteState = NoteState.Unset, Content = "orphan tag", SortOrder = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "note.theme_missing" && v.Detail.Contains("note:999"));
    }

    [Fact]
    public void Check_reports_a_subject_typed_by_a_deleted_definition()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.Subjects.Add(new Subject
        {
            Id = 999, Name = "Stranded", SubjectDefinitionId = 424242
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "subject.definition_missing" && v.Detail.Contains("subject:999"));
    }

    [Fact]
    public void Check_reports_a_track_definition_scoped_to_a_deleted_subject_definition()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.NoteTrackDefinitions.Add(new NoteTrackDefinition
        {
            Id = 999, SubjectDefinitionId = 424242, OwnerType = OwnerType.Subject, TrackName = "Stranded"
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "trackdefinition.subjectdefinition_missing" && v.Detail.Contains("track:999"));
    }

    [Fact]
    public void Check_reports_dangling_theater_and_story_ids_but_not_the_zero_sentinels()
    {
        // The synthetic plan's baseline already exercises the sentinels: Chapter 1 has StoryId 0
        // and every subject/plot point has TheaterId 0 — Check_passes_clean covers that side.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.Subjects.Add(new Subject
            {
                Id = 999, Name = "Misplaced", SubjectDefinitionId = SyntheticPlan.CharacterDefId, TheaterId = 424242
            });
            ctx.Chapters.Add(new Chapter { Id = 999, Title = "Misfiled", StoryId = 424242, OrderIndex = 9 });
        });
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "subject.theater_missing" && v.Detail.Contains("subject:999"));
        Assert.Contains(violations, v => v.Rule == "chapter.story_missing" && v.Detail.Contains("chapter:999"));
    }

    [Fact]
    public void Check_reports_a_conversation_block_whose_conversation_is_gone()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.ConversationBlocks.Add(new ConversationBlock
        {
            Id = 999, ConversationId = 424242, BlockNumber = 1
        }));
        using var ctx = OpenContext(plan.Path);

        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "conversationblock.conversation_missing" && v.Detail.Contains("block:999"));
    }

    // ── Property boards ───────────────────────────────────────────────────────

    [Fact]
    public void Check_reports_a_board_scoped_to_a_missing_subject_definition()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.PropertyBoards.Add(new PropertyBoard
        {
            Id = 999, Name = "Orphan Board", SubjectDefinitionId = 424242
        }));
        using var ctx = OpenContext(plan.Path);

        Assert.Contains(PlanIntegrity.Check(ctx),
            v => v.Rule == "propertyboard.subjectdefinition_missing" && v.Detail.Contains("board:999"));
    }

    [Fact]
    public void Check_reports_a_property_on_a_board_that_no_longer_exists()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx => ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
        {
            Id = 999, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
            OwnerType = OwnerType.Subject, Name = "Stranded", PropertyBoardId = 424242
        }));
        using var ctx = OpenContext(plan.Path);

        Assert.Contains(PlanIntegrity.Check(ctx),
            v => v.Rule == "narrativepropertydefinition.board_missing" && v.Detail.Contains("property:999"));
    }

    [Fact]
    public void Check_reports_a_property_on_a_board_scoped_to_a_different_subject_type()
    {
        // Would put a subject in a grid whose axes do not apply to it.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectDefinitions.Add(new SubjectDefinition { Id = 77, SubjectType = "Civilizational System" });
            ctx.PropertyBoards.Add(new PropertyBoard { Id = 1, Name = "Axes", SubjectDefinitionId = 77 });
            ctx.NarrativePropertyDefinitions.Add(new NarrativePropertyDefinition
            {
                Id = 999, SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                OwnerType = OwnerType.Subject, Name = "Mismatched", PropertyBoardId = 1
            });
        });
        using var ctx = OpenContext(plan.Path);

        Assert.Contains(PlanIntegrity.Check(ctx),
            v => v.Rule == "narrativepropertydefinition.board_scope_mismatch" && v.Detail.Contains("property:999"));
    }

    // ── Subject relations ─────────────────────────────────────────────────────

    [Fact]
    public void Check_reports_a_hierarchy_relation_that_crosses_subject_types()
    {
        // A chain that changes subject type partway down is not a chain.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectDefinitions.Add(new SubjectDefinition { Id = 77, SubjectType = "Organization" });
            ctx.SubjectRelationDefinitions.Add(new SubjectRelationDefinition
            {
                Id = 999, Name = "Descends from",
                SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                TargetSubjectDefinitionId = 77,
                FormsHierarchy = true
            });
        });
        using var ctx = OpenContext(plan.Path);

        Assert.Contains(PlanIntegrity.Check(ctx),
            v => v.Rule == "subjectrelationdefinition.hierarchy_cross_type" && v.Detail.Contains("relationDef:999"));
    }

    [Fact]
    public void Check_reports_a_relation_row_whose_definition_or_endpoints_are_gone()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectRelationDefinitions.Add(new SubjectRelationDefinition
            {
                Id = 1, Name = "Ancestor",
                SubjectDefinitionId = SyntheticPlan.CharacterDefId,
                TargetSubjectDefinitionId = SyntheticPlan.CharacterDefId
            });
            ctx.SubjectRelations.Add(new SubjectRelation
            {
                Id = 998, RelationDefinitionId = 424242,
                SubjectId = SyntheticPlan.SubjectId, TargetSubjectId = SyntheticPlan.EmptySubjectId
            });
            ctx.SubjectRelations.Add(new SubjectRelation
            {
                Id = 999, RelationDefinitionId = 1, SubjectId = 424242, TargetSubjectId = 424243
            });
        });
        using var ctx = OpenContext(plan.Path);
        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "subjectrelation.definition_missing" && v.Detail.Contains("relation:998"));
        Assert.Contains(violations, v => v.Rule == "subjectrelation.subject_missing" && v.Detail.Contains("relation:999"));
        Assert.Contains(violations, v => v.Rule == "subjectrelation.target_missing" && v.Detail.Contains("relation:999"));
    }

    [Fact]
    public void Check_reports_a_self_referencing_edge()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectRelationDefinitions.Add(SameTypeAncestor());
            ctx.SubjectRelations.Add(new SubjectRelation
            {
                Id = 999, RelationDefinitionId = 1,
                SubjectId = SyntheticPlan.SubjectId, TargetSubjectId = SyntheticPlan.SubjectId
            });
        });
        using var ctx = OpenContext(plan.Path);

        Assert.Contains(PlanIntegrity.Check(ctx),
            v => v.Rule == "subjectrelation.self_reference" && v.Detail.Contains("relation:999"));
    }

    [Fact]
    public void Check_reports_a_second_target_on_a_single_valued_relation()
    {
        // The schema cannot express single-select — no unique constraints, no FKs — so this check
        // is the enforcement, exactly as narrativevalue.duplicate_for_property is for properties.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectDefinitions.Add(new SubjectDefinition { Id = 3, SubjectType = "Spare" });
            ctx.Subjects.Add(new Subject { Id = 3, Name = "Third", SubjectDefinitionId = SyntheticPlan.CharacterDefId });
            ctx.SubjectRelationDefinitions.Add(SameTypeAncestor());
            ctx.SubjectRelations.AddRange(
                new SubjectRelation { Id = 1, RelationDefinitionId = 1, SubjectId = SyntheticPlan.SubjectId, TargetSubjectId = SyntheticPlan.EmptySubjectId },
                new SubjectRelation { Id = 2, RelationDefinitionId = 1, SubjectId = SyntheticPlan.SubjectId, TargetSubjectId = 3 });
        });
        using var ctx = OpenContext(plan.Path);

        Assert.Contains(PlanIntegrity.Check(ctx),
            v => v.Rule == "subjectrelation.duplicate_for_single"
              && v.Detail.Contains($"subject:{SyntheticPlan.SubjectId}"));
    }

    [Fact]
    public void Check_reports_every_subject_on_a_hierarchy_cycle()
    {
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectRelationDefinitions.Add(SameTypeAncestor(formsHierarchy: true));
            ctx.SubjectRelations.AddRange(
                new SubjectRelation { Id = 1, RelationDefinitionId = 1, SubjectId = SyntheticPlan.SubjectId, TargetSubjectId = SyntheticPlan.EmptySubjectId },
                new SubjectRelation { Id = 2, RelationDefinitionId = 1, SubjectId = SyntheticPlan.EmptySubjectId, TargetSubjectId = SyntheticPlan.SubjectId });
        });
        using var ctx = OpenContext(plan.Path);
        var violations = PlanIntegrity.Check(ctx);

        Assert.Contains(violations, v => v.Rule == "subjectrelation.cycle" && v.Detail.Contains($"subject:{SyntheticPlan.SubjectId}"));
        Assert.Contains(violations, v => v.Rule == "subjectrelation.cycle" && v.Detail.Contains($"subject:{SyntheticPlan.EmptySubjectId}"));
    }

    [Fact]
    public void Check_does_not_report_a_cycle_on_a_relation_that_never_claimed_to_be_a_hierarchy()
    {
        // A symmetric relation — "Rival of" — is legitimately cyclic. Only a relation asserting
        // FormsHierarchy is audited for loops.
        using var plan = SyntheticPlan.Create();
        plan.ExternalWrite(ctx =>
        {
            ctx.SubjectRelationDefinitions.Add(SameTypeAncestor(formsHierarchy: false));
            ctx.SubjectRelations.AddRange(
                new SubjectRelation { Id = 1, RelationDefinitionId = 1, SubjectId = SyntheticPlan.SubjectId, TargetSubjectId = SyntheticPlan.EmptySubjectId },
                new SubjectRelation { Id = 2, RelationDefinitionId = 1, SubjectId = SyntheticPlan.EmptySubjectId, TargetSubjectId = SyntheticPlan.SubjectId });
        });
        using var ctx = OpenContext(plan.Path);

        Assert.DoesNotContain(PlanIntegrity.Check(ctx), v => v.Rule == "subjectrelation.cycle");
    }

    private static SubjectRelationDefinition SameTypeAncestor(bool formsHierarchy = false) => new()
    {
        Id = 1,
        Name = "Ancestor",
        SubjectDefinitionId = SyntheticPlan.CharacterDefId,
        TargetSubjectDefinitionId = SyntheticPlan.CharacterDefId,
        IsSingle = true,
        FormsHierarchy = formsHierarchy
    };

    private static AppDbContext OpenContext(string path) =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={path}").Options);
}
