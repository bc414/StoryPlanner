using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnnecessaryModelsAndMadeHybrids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointCharacter_Characters_CharacterId",
                table: "PlotPointCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointCharacter_PlotPoints_PlotPointId",
                table: "PlotPointCharacter");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointTheme_PlotPoints_PlotPointId",
                table: "PlotPointTheme");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointTheme_Themes_ThemeId",
                table: "PlotPointTheme");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThread_PlotPoints_PlotPointId",
                table: "PlotPointThread");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThread_Threads_StoryThreadId",
                table: "PlotPointThread");

            migrationBuilder.DropTable(
                name: "NoteDependency");

            migrationBuilder.DropTable(
                name: "PlotPointDependency");

            migrationBuilder.DropTable(
                name: "PlotPointNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointThread",
                table: "PlotPointThread");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointTheme",
                table: "PlotPointTheme");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointCharacter",
                table: "PlotPointCharacter");

            migrationBuilder.RenameTable(
                name: "PlotPointThread",
                newName: "PlotPointThreads");

            migrationBuilder.RenameTable(
                name: "PlotPointTheme",
                newName: "PlotPointThemes");

            migrationBuilder.RenameTable(
                name: "PlotPointCharacter",
                newName: "PlotPointCharacters");

            migrationBuilder.RenameColumn(
                name: "ColorHex",
                table: "Threads",
                newName: "Icon");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPointThread_StoryThreadId",
                table: "PlotPointThreads",
                newName: "IX_PlotPointThreads_StoryThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPointTheme_ThemeId",
                table: "PlotPointThemes",
                newName: "IX_PlotPointThemes_ThemeId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPointCharacter_CharacterId",
                table: "PlotPointCharacters",
                newName: "IX_PlotPointCharacters_CharacterId");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Themes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointThreads",
                table: "PlotPointThreads",
                columns: new[] { "PlotPointId", "ThreadId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointThemes",
                table: "PlotPointThemes",
                columns: new[] { "PlotPointId", "ThemeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointCharacters",
                table: "PlotPointCharacters",
                columns: new[] { "PlotPointId", "CharacterId" });

            migrationBuilder.CreateTable(
                name: "PlotPointCodexEntries",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    CodexEntryId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsageType = table.Column<int>(type: "INTEGER", nullable: false),
                    Commentary = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotPointCodexEntries", x => new { x.PlotPointId, x.CodexEntryId });
                    table.ForeignKey(
                        name: "FK_PlotPointCodexEntries_CodexEntries_CodexEntryId",
                        column: x => x.CodexEntryId,
                        principalTable: "CodexEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlotPointCodexEntries_PlotPoints_PlotPointId",
                        column: x => x.PlotPointId,
                        principalTable: "PlotPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_LocationId",
                table: "Notes",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointCodexEntries_CodexEntryId",
                table: "PlotPointCodexEntries",
                column: "CodexEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Locations_LocationId",
                table: "Notes",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointCharacters_Characters_CharacterId",
                table: "PlotPointCharacters",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointCharacters_PlotPoints_PlotPointId",
                table: "PlotPointCharacters",
                column: "PlotPointId",
                principalTable: "PlotPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThemes_PlotPoints_PlotPointId",
                table: "PlotPointThemes",
                column: "PlotPointId",
                principalTable: "PlotPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThemes_Themes_ThemeId",
                table: "PlotPointThemes",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThreads_PlotPoints_PlotPointId",
                table: "PlotPointThreads",
                column: "PlotPointId",
                principalTable: "PlotPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThreads_Threads_StoryThreadId",
                table: "PlotPointThreads",
                column: "StoryThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Locations_LocationId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointCharacters_Characters_CharacterId",
                table: "PlotPointCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointCharacters_PlotPoints_PlotPointId",
                table: "PlotPointCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThemes_PlotPoints_PlotPointId",
                table: "PlotPointThemes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThemes_Themes_ThemeId",
                table: "PlotPointThemes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThreads_PlotPoints_PlotPointId",
                table: "PlotPointThreads");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThreads_Threads_StoryThreadId",
                table: "PlotPointThreads");

            migrationBuilder.DropTable(
                name: "PlotPointCodexEntries");

            migrationBuilder.DropIndex(
                name: "IX_Notes_LocationId",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointThreads",
                table: "PlotPointThreads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointThemes",
                table: "PlotPointThemes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointCharacters",
                table: "PlotPointCharacters");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "PlotPointThreads",
                newName: "PlotPointThread");

            migrationBuilder.RenameTable(
                name: "PlotPointThemes",
                newName: "PlotPointTheme");

            migrationBuilder.RenameTable(
                name: "PlotPointCharacters",
                newName: "PlotPointCharacter");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Threads",
                newName: "ColorHex");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPointThreads_StoryThreadId",
                table: "PlotPointThread",
                newName: "IX_PlotPointThread_StoryThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPointThemes_ThemeId",
                table: "PlotPointTheme",
                newName: "IX_PlotPointTheme_ThemeId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotPointCharacters_CharacterId",
                table: "PlotPointCharacter",
                newName: "IX_PlotPointCharacter_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointThread",
                table: "PlotPointThread",
                columns: new[] { "PlotPointId", "ThreadId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointTheme",
                table: "PlotPointTheme",
                columns: new[] { "PlotPointId", "ThemeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointCharacter",
                table: "PlotPointCharacter",
                columns: new[] { "PlotPointId", "CharacterId" });

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
                name: "PlotPointDependency",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(type: "INTEGER", nullable: false),
                    DependentId = table.Column<int>(type: "INTEGER", nullable: false),
                    DependencyType = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "PlotPointNotes",
                columns: table => new
                {
                    PlotPointId = table.Column<int>(type: "INTEGER", nullable: false),
                    NoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    UsageType = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "IX_PlotPointDependency_DependentId",
                table: "PlotPointDependency",
                column: "DependentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlotPointNotes_NoteId",
                table: "PlotPointNotes",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointCharacter_Characters_CharacterId",
                table: "PlotPointCharacter",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointCharacter_PlotPoints_PlotPointId",
                table: "PlotPointCharacter",
                column: "PlotPointId",
                principalTable: "PlotPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointTheme_PlotPoints_PlotPointId",
                table: "PlotPointTheme",
                column: "PlotPointId",
                principalTable: "PlotPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointTheme_Themes_ThemeId",
                table: "PlotPointTheme",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThread_PlotPoints_PlotPointId",
                table: "PlotPointThread",
                column: "PlotPointId",
                principalTable: "PlotPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThread_Threads_StoryThreadId",
                table: "PlotPointThread",
                column: "StoryThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
