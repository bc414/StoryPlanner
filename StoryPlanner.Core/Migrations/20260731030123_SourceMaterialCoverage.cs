using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class SourceMaterialCoverage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceMaterialId",
                table: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "SourceMaterials",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PartNoun",
                table: "SourceMaterials",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NoteSourceReferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceMaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceMaterialPartId = table.Column<int>(type: "INTEGER", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteSourceReferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SourceMaterialParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceMaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewState = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceMaterialParts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteSourceReferences");

            migrationBuilder.DropTable(
                name: "SourceMaterialParts");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "SourceMaterials");

            migrationBuilder.DropColumn(
                name: "PartNoun",
                table: "SourceMaterials");

            migrationBuilder.AddColumn<int>(
                name: "SourceMaterialId",
                table: "Notes",
                type: "INTEGER",
                nullable: true);
        }
    }
}
