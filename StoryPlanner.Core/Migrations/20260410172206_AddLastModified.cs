using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddLastModified : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PlotPointThreads",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PlotPointThemes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PlotPoints",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PlotPointCodexEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PlotPointCharacters",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            var tables = new[] { "PlotPointThreads", "PlotPointThemes", "PlotPoints", "PlotPointCodexEntries", "PlotPointCharacters", "Notes" };
            foreach (var table in tables)
            {
                migrationBuilder.Sql($@"
                    UPDATE {table}
                    SET LastModified = datetime('now', '-120 days', '+' || CAST((rowid * 120.0 * 24 * 60 * 60 / (SELECT MAX(rowid) FROM {table})) AS INTEGER) || ' seconds')
                    WHERE (SELECT MAX(rowid) FROM {table}) IS NOT NULL;
                ");
            }
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PlotPointThreads");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PlotPointThemes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PlotPointCodexEntries");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PlotPointCharacters");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Notes");
        }
    }
}
