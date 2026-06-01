using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class Themes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Explanation",
                table: "NoteTrackDefinitions",
                newName: "UsageDirective");

            migrationBuilder.AddColumn<string>(
                name: "AuditDirective",
                table: "NoteTrackDefinitions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "CanEditInAuditMode",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsTheme",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ThemeId",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Proposition = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropColumn(
                name: "AuditDirective",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "CanEditInAuditMode",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "SupportsTheme",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "ThemeId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "UsageDirective",
                table: "NoteTrackDefinitions",
                newName: "Explanation");
        }
    }
}
