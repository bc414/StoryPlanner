using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class WorkPhases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "NarrativePropertyDefinitions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GatingWorkPhaseId",
                table: "NarrativePropertyDefinitions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkPhases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    RequiresZeroFlaggedNotes = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiresZeroUnsetNotes = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPhases", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkPhases");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "NarrativePropertyDefinitions");

            migrationBuilder.DropColumn(
                name: "GatingWorkPhaseId",
                table: "NarrativePropertyDefinitions");
        }
    }
}
