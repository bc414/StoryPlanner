using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Core.Migrations
{
    /// <inheritdoc />
    public partial class SubjectRelationsAndBoards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "NarrativePropertyValueDefinitions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PropertyBoardId",
                table: "NarrativePropertyDefinitions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PropertyBoards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IncludeUnsetBand = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectRelationDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubjectDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetSubjectDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    InverseName = table.Column<string>(type: "TEXT", nullable: false),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    Explanation = table.Column<string>(type: "TEXT", nullable: false),
                    IsSingle = table.Column<bool>(type: "INTEGER", nullable: false),
                    FormsHierarchy = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectRelationDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RelationDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetSubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectRelations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyBoards");

            migrationBuilder.DropTable(
                name: "SubjectRelationDefinitions");

            migrationBuilder.DropTable(
                name: "SubjectRelations");

            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "NarrativePropertyValueDefinitions");

            migrationBuilder.DropColumn(
                name: "PropertyBoardId",
                table: "NarrativePropertyDefinitions");
        }
    }
}
