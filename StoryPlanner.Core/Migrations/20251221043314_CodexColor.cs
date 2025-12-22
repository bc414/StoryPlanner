using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class CodexColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "CodexEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "CodexEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "CodexEntries");

            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "CodexEntries");
        }
    }
}
