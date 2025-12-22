using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class NeedsFurtherAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsStrictRule",
                table: "Notes",
                newName: "NeedsFurtherAnalysis");
            migrationBuilder.Sql("UPDATE Notes SET NeedsFurtherAnalysis = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NeedsFurtherAnalysis",
                table: "Notes",
                newName: "IsStrictRule");
        }
    }
}
