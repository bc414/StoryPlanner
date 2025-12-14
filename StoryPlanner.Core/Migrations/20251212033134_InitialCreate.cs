using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Inspiration = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodexEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodexEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SourceMaterials",
                columns: table => new
                {
                    SourceMaterialId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Abbreviation = table.Column<string>(type: "TEXT", nullable: false),
                    ColorHex = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceMaterials", x => x.SourceMaterialId);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ColorHex = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlotPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    VerbatimText = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Intensity = table.Column<int>(type: "INTEGER", nullable: false),
                    WorldDate = table.Column<string>(type: "TEXT", nullable: true),
                    ChapterId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrderInChapter = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlotPoints_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlotPoints_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    IsStrictRule = table.Column<bool>(type: "INTEGER", nullable: false),
                    CodexEntryId = table.Column<int>(type: "INTEGER", nullable: true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    ThemeId = table.Column<int>(type: "INTEGER", nullable: true),
                    ChapterId = table.Column<int>(type: "INTEGER", nullable: true),
                    StoryThreadId = table.Column<int>(type: "INTEGER", nullable: true),
                    SourceMaterialId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_CodexEntries_CodexEntryId",
                        column: x => x.CodexEntryId,
                        principalTable: "CodexEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_SourceMaterials_SourceMaterialId",
                        column: x => x.SourceMaterialId,
                        principalTable: "SourceMaterials",
                        principalColumn: "SourceMaterialId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Threads_StoryThreadId",
                        column: x => x.StoryThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointCharacter",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    DevelopmentImpact = table.Column<int>(type: "INTEGER", nullable: false),
                    DevelopmentNote = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointCharacter", x => new { x.PlotPointId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_PlotPointCharacter_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlotPointCharacter_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointDependency",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    DependentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointDependency", x => new { x.PrerequisiteId, x.DependentId });
                    table.ForeignKey(
                        name: "FK_PlotPointDependency_PlotPoints_DependentId",
                        column: x => x.DependentId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlotPointDependency_PlotPoints_PrerequisiteId",
                        column: x => x.PrerequisiteId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointTheme",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    ThemeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Prominence = table.Column<int>(type: "INTEGER", nullable: false),
                    Commentary = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointTheme", x => new { x.PlotPointId, x.ThemeId });
                    table.ForeignKey(
                        name: "FK_PlotPointTheme_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlotPointTheme_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointThread",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    ThreadId = table.Column<int>(type: "INTEGER", nullable: false),
                    StoryThreadId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointThread", x => new { x.PlotPointId, x.ThreadId });
                    table.ForeignKey(
                        name: "FK_PlotPointThread_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlotPointThread_Threads_StoryThreadId",
                        column: x => x.StoryThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteDependency",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    DependentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteDependency", x => new { x.PrerequisiteId, x.DependentId });
                    table.ForeignKey(
                        name: "FK_NoteDependency_Notes_DependentId",
                        column: x => x.DependentId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NoteDependency_Notes_PrerequisiteId",
                        column: x => x.PrerequisiteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlotPointNotes",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    NoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsageType = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointNotes", x => new { x.PlotPointId, x.NoteId });
                    table.ForeignKey(
                        name: "FK_PlotPointNotes_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlotPointNotes_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteDependency_DependentId",
                table: "NoteDependency",
                column: "DependentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ChapterId",
                table: "Notes",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CharacterId",
                table: "Notes",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CodexEntryId",
                table: "Notes",
                column: "CodexEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_SourceMaterialId",
                table: "Notes",
                column: "SourceMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_StoryThreadId",
                table: "Notes",
                column: "StoryThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ThemeId",
                table: "Notes",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointCharacter_CharacterId",
                table: "PlotPointCharacter",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointDependency_DependentId",
                table: "PlotPointDependency",
                column: "DependentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointNotes_NoteId",
                table: "PlotPointNotes",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_ChapterId",
                table: "PlotPoints",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_LocationId",
                table: "PlotPoints",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointTheme_ThemeId",
                table: "PlotPointTheme",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointThread_StoryThreadId",
                table: "PlotPointThread",
                column: "StoryThreadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteDependency");

            migrationBuilder.DropTable(
                name: "PlotPointCharacter");

            migrationBuilder.DropTable(
                name: "PlotPointDependency");

            migrationBuilder.DropTable(
                name: "PlotPointNotes");

            migrationBuilder.DropTable(
                name: "PlotPointTheme");

            migrationBuilder.DropTable(
                name: "PlotPointThread");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PlotPoints");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "CodexEntries");

            migrationBuilder.DropTable(
                name: "SourceMaterials");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
