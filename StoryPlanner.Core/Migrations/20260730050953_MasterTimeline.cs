using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class MasterTimeline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheaterId",
                table: "Subjects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FabulaDay",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FabulaMonth",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FabulaYear",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheaterId",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsWorldDateEnd",
                table: "NoteTrackDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WorldDateEndDay",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorldDateEndMonth",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorldDateEndYear",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorldDateStartDay",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorldDateStartMonth",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorldDateStartYear",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pivots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pivots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theaters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theaters", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pivots");

            migrationBuilder.DropTable(
                name: "Theaters");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "FabulaDay",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "FabulaMonth",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "FabulaYear",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "SupportsWorldDateEnd",
                table: "NoteTrackDefinitions");

            migrationBuilder.DropColumn(
                name: "WorldDateEndDay",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "WorldDateEndMonth",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "WorldDateEndYear",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "WorldDateStartDay",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "WorldDateStartMonth",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "WorldDateStartYear",
                table: "Notes");
        }
    }
}
