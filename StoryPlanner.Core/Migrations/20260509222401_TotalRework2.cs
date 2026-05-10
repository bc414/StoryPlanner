using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class TotalRework2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Chapters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Chapters",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Chapters",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
