using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class SourceMaterialRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "SourceMaterials");

            migrationBuilder.RenameColumn(
                name: "ColorHex",
                table: "SourceMaterials",
                newName: "Description");

            migrationBuilder.AddColumn<bool>(
                name: "SupportsSourceMaterial",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SourceMaterialId",
                table: "Notes",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportsSourceMaterial",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "SourceMaterialId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "SourceMaterials",
                newName: "ColorHex");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "SourceMaterials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
