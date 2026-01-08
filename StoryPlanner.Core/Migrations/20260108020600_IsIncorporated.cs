using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class IsIncorporated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncorporated",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncorporated",
                table: "Notes");
        }
    }
}
