using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class modes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "NoteTrackDefinitions",
                newName: "ExpansionModeDisplayOrder");

            migrationBuilder.AddColumn<int>(
                name: "AuditModeDisplayOrder",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LinkingModeDisplayOrder",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GardenerModeDisplayOrder",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SceneDesignModeDisplayOrder",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditModeDisplayOrder",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "ExpansionModeDisplayOrder",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "GardenerModeDisplayOrder",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "LinkingModeDisplayOrder",
                table: "NoteTrackDefinitions");

            migrationBuilder.RenameColumn(
                name: "SceneDesignModeDisplayOrder",
                table: "NoteTrackDefinitions",
                newName: "DisplayOrder");
        }
    }
}
