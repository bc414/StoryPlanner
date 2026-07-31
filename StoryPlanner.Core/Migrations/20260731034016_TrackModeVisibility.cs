using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class TrackModeVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HiddenInAuditMode",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HiddenInExpansionMode",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HiddenInGardenerMode",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HiddenInLinkingMode",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HiddenInSceneDesignMode",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiddenInAuditMode",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "HiddenInExpansionMode",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "HiddenInGardenerMode",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "HiddenInLinkingMode",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "HiddenInSceneDesignMode",
                table: "NoteTrackDefinitions");
        }
    }
}
